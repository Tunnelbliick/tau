using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StorybrewCommon.Mapset;
using StorybrewCommon.Scripting;
using StorybrewCommon.Storyboarding;
using StorybrewCommon.Storyboarding.Util;
using StorybrewCommon.Subtitles;
using StorybrewCommon.Util;
using OpenTK;
using StorybrewCommon.Storyboarding.CommandValues;
using storyboard.scriptslibrary.maniaModCharts.utility;
using StorybrewCommon.Animations;
using System.IO;
using OpenTK.Graphics;

namespace StorybrewScripts
{
    public class Receptor
    {

        public string receptorSpritePath = "";
        public Vector2 position = new Vector2(0, 0);
        public StoryboardLayer layer;
        public OsbSprite renderedSprite;
        public OsbSprite debug;
        public string appliedTransformation = "";

        public SortedDictionary<double, float> positionX = new SortedDictionary<double, float>();
        public SortedDictionary<double, float> positionY = new SortedDictionary<double, float>();

        public KeyframedValue<Vector2> Scale = new KeyframedValue<Vector2>((from, to, progress) => Vector2.Lerp(from, to, (float)progress), new Vector2(0.5f));

        public OsbSprite light;
        public OsbSprite hit;

        private readonly object lockX = new object();
        private readonly object lockY = new object();

        public float defaultScale = 0.5f;

        // Rotation in radiants
        public double rotation = 0f;
        public double startRotation = 0f;
        public ColumnType columnType;
        public SortedDictionary<int, double> bpmOffset = new SortedDictionary<int, double>();
        public SortedDictionary<int, double> bpm = new SortedDictionary<int, double>();

        private double deltaIncrement = 1;

        public Receptor(String receptorSpritePath, double rotation, StoryboardLayer layer, CommandScale scale, double starttime, ColumnType type, double delta)
        {

            this.deltaIncrement = delta;

            OsbSprite receptorSprite = layer.CreateSprite(receptorSpritePath, OsbOrigin.Centre);
            OsbSprite light = layer.CreateSprite("sb/sprites/light.png", OsbOrigin.Centre);
            OsbSprite hit = layer.CreateSprite("sb/sprites/hit.png", OsbOrigin.Centre);

            positionX.Add(0, 0);
            positionY.Add(0, 0);

            switch (type)
            {
                case ColumnType.one:
                    light.Rotate(starttime - 1, 1 * Math.PI / 2);
                    hit.Rotate(starttime - 1, 1 * Math.PI / 2);
                    receptorSprite.Rotate(starttime - 1, 1 * Math.PI / 2);
                    break;
                case ColumnType.two:
                    light.Rotate(starttime - 1, 0 * Math.PI / 2);
                    hit.Rotate(starttime - 1, 0 * Math.PI / 2);
                    receptorSprite.Rotate(starttime - 1, 0 * Math.PI / 2);
                    break;
                case ColumnType.three:
                    light.Rotate(starttime - 1, 2 * Math.PI / 2);
                    hit.Rotate(starttime - 1, 2 * Math.PI / 2);
                    receptorSprite.Rotate(starttime - 1, 2 * Math.PI / 2);
                    break;
                case ColumnType.four:
                    light.Rotate(starttime - 1, 3 * Math.PI / 2);
                    hit.Rotate(starttime - 1, 3 * Math.PI / 2);
                    receptorSprite.Rotate(starttime - 1, 3 * Math.PI / 2);
                    break;
            }

            Scale.Add(starttime, new Vector2(scale.X, scale.Y));

            this.light = light;
            this.hit = hit;
            this.columnType = type;
            this.receptorSpritePath = receptorSpritePath;
            this.renderedSprite = receptorSprite;
            this.rotation = rotation;
            this.startRotation = rotation;
            this.layer = layer;

        }

        public Receptor(string receptorSpritePath, double rotation, StoryboardLayer layer, Vector2 position, ColumnType type, double delta)
        {
            OsbSprite receptor = layer.CreateSprite("sb/transparent.png", OsbOrigin.Centre);
            OsbSprite receptorSprite = layer.CreateSprite(receptorSpritePath, OsbOrigin.Centre);
            OsbSprite light = layer.CreateSprite("sb/sprites/light.png", OsbOrigin.Centre);
            OsbSprite hit = layer.CreateSprite("sb/sprites/hit.png", OsbOrigin.Centre);

            this.deltaIncrement = delta;

            positionX.Add(0, 0);
            positionY.Add(0, 0);

            switch (type)
            {
                case ColumnType.one:
                    light.Rotate(0 - 1, 1 * Math.PI / 2);
                    hit.Rotate(0 - 1, 1 * Math.PI / 2);
                    receptor.Rotate(0 - 1, 1 * Math.PI / 2);
                    break;
                case ColumnType.two:
                    light.Rotate(0 - 1, 0 * Math.PI / 2);
                    hit.Rotate(0 - 1, 0 * Math.PI / 2);
                    receptor.Rotate(0 - 1, 0 * Math.PI / 2);
                    break;
                case ColumnType.three:
                    light.Rotate(0 - 1, 2 * Math.PI / 2);
                    hit.Rotate(0 - 1, 2 * Math.PI / 2);
                    receptor.Rotate(0 - 1, 2 * Math.PI / 2);
                    break;
                case ColumnType.four:
                    light.Rotate(0 - 1, 3 * Math.PI / 2);
                    hit.Rotate(0 - 1, 3 * Math.PI / 2);
                    receptor.Rotate(0 - 1, 3 * Math.PI / 2);
                    break;
            }

            this.light = light;
            this.hit = hit;
            this.columnType = type;
            this.receptorSpritePath = receptorSpritePath;
            this.renderedSprite = receptorSprite;
            this.rotation = rotation;
            this.layer = layer;
            this.position = position;

        }

        // Absolute Movements overwrite any and all relative movements that might have existed before them hence why they are absolute!
        public void MoveReceptorAbsolute(double starttime, Vector2 endPos)
        {

            AddXValue(starttime, endPos.X, true);
            AddYValue(starttime, endPos.Y, true);


        }

        // Absolute Movements overwrite any and all relative movements that might have existed before them hence why they are absolute!
        public void MoveReceptorAbsolute(OsbEasing ease, double starttime, double endtime, Vector2 startPos, Vector2 endPos)
        {

            if (starttime == endtime)
            {
                AddXValue(starttime, endPos.X, true);
                AddYValue(starttime, endPos.Y, true);
                return;
            }

            easeProgressAbsolute(ease, starttime, endtime, startPos, endPos);

        }

        public void MoveReceptorRelative(OsbEasing ease, double starttime, double endtime, Vector2 offset)
        {

            if (starttime == endtime)
            {
                AddXValue(starttime, offset.X);
                AddYValue(starttime, offset.Y);
                return;
            }

            easeProgressRelative(ease, starttime, endtime, offset);

        }

        public void MoveReceptorRelativeNorm(OsbEasing ease, double starttime, double endtime, Vector2 offset, NoteOrigin origin)
        {
            easeProgressRelativeNormalized(ease, starttime, endtime, offset, origin);
        }

        public void MoveReceptorRelative(OsbEasing ease, double starttime, double endtime, Vector2 offset, Vector2 absolute)
        {

            if (starttime == endtime)
            {
                AddXValue(starttime, offset.X);
                AddYValue(starttime, offset.Y);
                return;
            }

            easeProgressRelative(ease, starttime, endtime, offset);

        }

        public void MoveReceptorRelativeX(OsbEasing ease, double starttime, double endtime, float value)
        {

            if (starttime == endtime)
            {
                AddXValue(starttime, value);
                return;
            }

            easeProgressRelative(ease, starttime, endtime, new Vector2(value, 0));

        }

        public void MoveReceptorRelativeY(OsbEasing ease, double starttime, double endtime, float value)
        {
            if (starttime == endtime)
            {
                AddYValue(starttime, value);
                return;
            }

            easeProgressRelative(ease, starttime, endtime, new Vector2(0, value));

        }

        public void ScaleReceptor(OsbEasing ease, double starttime, double endtime, Vector2 newScale)
        {

            if (starttime == endtime)
            {
                Scale.Add(starttime, ScaleAt(starttime));
                Scale.Add(starttime, newScale);
            }
            else
            {
                Scale.Add(starttime, ScaleAt(starttime));
                Scale.Add(endtime, newScale, ease.ToEasingFunction());
            }
        }

        public void RotateReceptorAbsolute(OsbEasing ease, double starttime, double endtime, double rotation)
        {
            OsbSprite receptor = this.renderedSprite;


            if (starttime == endtime)
            {
                receptor.Rotate(starttime, rotation);
                light.Rotate(starttime, rotation);
                hit.Rotate(starttime, rotation);
            }
            else
            {
                receptor.Rotate(ease, starttime, endtime, RotationAt(starttime), rotation);
                light.Rotate(ease, starttime, endtime, RotationAt(starttime), rotation);
                hit.Rotate(ease, starttime, endtime, RotationAt(starttime), rotation);
            }

            this.rotation = rotation;

        }

        public void RotateReceptor(OsbEasing ease, double starttime, double endtime, double rotation)
        {
            OsbSprite receptor = this.renderedSprite;
            var currentRot = RotationAt(starttime);
            var newRotation = currentRot + rotation;

            if (starttime == endtime)
            {
                receptor.Rotate(starttime, newRotation);
                light.Rotate(starttime, newRotation);
                hit.Rotate(starttime, newRotation);
            }
            else
            {
                receptor.Rotate(ease, starttime, endtime, currentRot, newRotation);
                light.Rotate(ease, starttime, endtime, currentRot, newRotation);
                hit.Rotate(ease, starttime, endtime, currentRot, newRotation);
            }

            this.rotation = newRotation;

        }

        public void PivotReceptor(OsbEasing ease, double starttime, double endtime, double rotation, Vector2 center)
        {

            Vector2 initialPosition = PositionAt(starttime);

            if (starttime >= endtime)
            {
                // Handle instant movement
                Vector2 rotatedPoint = Utility.PivotPoint(initialPosition, center, rotation);

                Vector2 relativeMovement = rotatedPoint - initialPosition;
                // Apply the movement
                MoveReceptorRelative(ease, starttime, starttime, relativeMovement);
                return;
            }
            double duration = Math.Max(endtime - starttime, 1);
            double currentTime = starttime;

            while (currentTime <= endtime)
            {
                double progress = (currentTime - starttime) / duration;
                double easedProgress = ease.Ease(progress);
                double currentRotation = rotation * easedProgress;

                // Calculate the new rotated position
                Vector2 rotatedPoint = Utility.PivotPoint(initialPosition, center, currentRotation);

                // Calculate the relative movement
                Vector2 relativeMovement = rotatedPoint - PositionAt(currentTime);

                // Apply the movement
                MoveReceptorRelative(ease, currentTime, currentTime, relativeMovement);

                currentTime += 1;
            }
        }

        public void PivotReceptorNorm(OsbEasing ease, double starttime, double endtime, double rotation, Vector2 center, NoteOrigin origin)
        {
            if (starttime >= endtime)
            {
                // Handle instant movement
                Vector2 rotatedPoint = Utility.PivotPoint(PositionAt(starttime), center, rotation);
                Vector2 relativeMovement = rotatedPoint - PositionAt(starttime);
                MoveReceptorRelativeNorm(ease, starttime, starttime, relativeMovement, origin);
                return;
            }

            Vector2 initialPosition = PositionAt(starttime);
            double duration = Math.Max(endtime - starttime, 1);
            double currentTime = starttime;

            while (currentTime <= endtime)
            {
                double progress = (currentTime - starttime) / duration;
                double easedProgress = ease.Ease(progress);
                double currentRotation = rotation * easedProgress;

                // Calculate the new rotated position
                Vector2 rotatedPoint = Utility.PivotPoint(initialPosition, center, currentRotation);

                // Calculate the relative movement
                Vector2 relativeMovement = rotatedPoint - PositionAt(currentTime);

                // Apply the movement with normalization
                MoveReceptorRelativeNorm(ease, currentTime, currentTime, relativeMovement, origin);

                currentTime += deltaIncrement;
            }
        }

        public Vector2 ScaleAt(double currentTime)
        {
            return Scale.ValueAt(currentTime);
        }

        public float RotationAt(double currentTIme)
        {
            return this.renderedSprite.RotationAt(currentTIme);
        }

        public void Render(double starttime, double endtime)
        {
            if (this.appliedTransformation != "")
            {
                return;
            }

            OsbSprite sprite = this.renderedSprite;

            switch (this.columnType)
            {
                case ColumnType.one:
                    sprite.Rotate(starttime - 1, 1 * Math.PI / 2);
                    break;
                case ColumnType.two:
                    sprite.Rotate(starttime - 1, 0 * Math.PI / 2);
                    break;
                case ColumnType.three:
                    sprite.Rotate(starttime - 1, 2 * Math.PI / 2);
                    break;
                case ColumnType.four:
                    sprite.Rotate(starttime - 1, 3 * Math.PI / 2);
                    break;
            }

            sprite.Color(starttime, new Color4(97, 97, 97, 0));

            // Create a list to hold our BPM segments
            var bpmSegments = new List<(double StartTime, double BeatDuration, int LoopCount)>();

            // Calculate BPM segments and loop counts
            double currentTime = starttime;
            int currentKey = GetCurrentBpmKey(currentTime);
            double segmentStartTime = currentTime;

            // Find the correct starting point - align with BPM
            double beatDuration = 60000 / bpm[currentKey];
            double offset = bpmOffset[currentKey];
            double adjustedStartTime = Math.Ceiling((currentTime - offset) / beatDuration) * beatDuration + offset;

            while (currentTime < endtime)
            {
                // Check if we need to switch to a new BPM
                int newKey = GetCurrentBpmKey(currentTime);
                if (newKey != currentKey)
                {
                    // Calculate how many beats fit in the previous segment
                    int loopCount = (int)Math.Floor((currentTime - adjustedStartTime) / beatDuration);
                    if (loopCount > 0)
                    {
                        bpmSegments.Add((adjustedStartTime, beatDuration, loopCount));
                    }

                    // Start a new segment
                    currentKey = newKey;
                    beatDuration = 60000 / bpm[currentKey];
                    offset = bpmOffset[currentKey];
                    adjustedStartTime = Math.Ceiling((currentTime - offset) / beatDuration) * beatDuration + offset;
                }

                currentTime += beatDuration;
            }

            // Add the final segment
            int finalLoopCount = Math.Max(1, (int)Math.Floor((endtime - adjustedStartTime) / beatDuration));
            if (finalLoopCount > 0)
            {
                bpmSegments.Add((adjustedStartTime, beatDuration, finalLoopCount));
            }

            // Apply the animations using loop groups for each segment
            foreach (var segment in bpmSegments)
            {
                // Create a loop group for this BPM segment
                var halfDuration = segment.BeatDuration / 2;

                sprite.StartLoopGroup(segment.StartTime, segment.LoopCount);
                sprite.Color(OsbEasing.OutCirc, 0, halfDuration,
                    new Color4(255, 255, 255, 255), new Color4(97, 97, 97, 0));
                sprite.Color(OsbEasing.InCirc, halfDuration, segment.BeatDuration,
                    new Color4(97, 97, 97, 0), new Color4(255, 255, 255, 255));
                sprite.EndGroup();
            }
        }

        private int GetCurrentBpmKey(double time)
        {

            int latestTime = bpm.First().Key;

            foreach (var entry in bpm)
            {
                if (entry.Key <= time && entry.Key > latestTime)
                {
                    latestTime = entry.Key;
                }
            }

            return latestTime;
        }

        /*public void RenderTransformed(double starttime, double endtime, string reference)
        {

            if (this.appliedTransformation == reference)
            {
                return;
            }

            OsbSprite oldSprite = this.renderedSprite;
            this.appliedTransformation = reference;
            oldSprite.Fade(starttime, 0);
            OsbSprite sprite = layer.CreateSprite(Path.Combine("sb", "transformation", reference, this.columnType.ToString(), "receptor", "receptor" + ".png"), OsbOrigin.Centre, PositionAt(starttime));

            sprite.Rotate(starttime, 0);
            sprite.ScaleVec(starttime, renderedSprite.ScaleAt(starttime));
            sprite.Fade(starttime, 1);
            sprite.Fade(endtime, 0);

            this.renderedSprite = sprite;

            // oldSprite = null;
        }*/

        public Color4 LerpColor(Color4 colorA, Color4 colorB, double t)
        {
            byte r = (byte)(colorA.R + t * (colorB.R - colorA.R));
            byte g = (byte)(colorA.G + t * (colorB.G - colorA.G));
            byte b = (byte)(colorA.B + t * (colorB.B - colorA.B));
            byte a = (byte)(colorA.A + t * (colorB.A - colorA.A));

            return new Color4(r, g, b, a);
        }

        private void AddXValue(double time, float value, bool absolute = false)
        {

            lock (lockX)
            {
                if (positionX == null)
                {
                    positionX = new SortedDictionary<double, float>();
                }

                if (positionX.ContainsKey(time))
                {
                    if (absolute)
                        positionX[time] = value;
                    else
                        positionX[time] += value;
                }
                else
                {
                    float lastValue = getLastX(time);
                    positionX.Add(time, lastValue + value);
                }

                Parallel.ForEach(positionX.Keys.Where(k => k > time).ToList(), key =>
                {
                    {
                        positionX[key] += value;
                    }
                });
            }
        }

        /* private void AddXNorm(double time, float value, float last)
         {

             lock (lockX)
             {
                 if (positionX == null)
                 {
                     positionX = new SortedDictionary<double, float>();
                 }

                 // Update or add the value at the specified time
                 if (positionX.ContainsKey(time))
                 {
                     if (absolute)
                         positionX[time] = value;
                     else
                         positionX[time] += value;
                 }
                 else
                 {
                     float lastValue = getLastX(time);
                     positionX.Add(time, lastValue + value);
                 }

                 // Adjust all subsequent values
                 Parallel.ForEach(positionX.Keys.Where(k => k > time).ToList(), key =>
                 {
                     {
                         positionX[key] += value;
                     }
                 });
             }
         }*/


        private void AddYValue(double time, float value, bool absolute = false)
        {
            lock (lockY)
            {
                if (positionY == null)
                {
                    positionY = new SortedDictionary<double, float>();
                }

                // Update or add the value at the specified time
                if (positionY.ContainsKey(time))
                {
                    if (absolute)
                        positionY[time] = value;
                    else
                        positionY[time] += value;
                }
                else
                {
                    float lastValue = getLastY(time);
                    positionY.Add(time, lastValue + value);
                }

                // Adjust all subsequent values
                Parallel.ForEach(positionY.Keys.Where(k => k > time).ToList(), key =>
                {
                    positionY[key] += value;
                });
            }
        }

        private float getLastX(double currentTime)
        {
            if (positionX == null || positionX.Count == 0)
            {
                return 0; // Or your default value
            }

            var keys = positionX.Keys.ToList();
            int left = 0;
            int right = keys.Count - 1;
            double lastKey = -1;

            while (left <= right)
            {
                int mid = left + (right - left) / 2;
                if (keys[mid] < currentTime)
                {
                    lastKey = keys[mid];
                    left = mid + 1;
                }
                else
                {
                    right = mid - 1;
                }
            }

            return lastKey != -1 ? positionX[lastKey] : 0;
        }


        private float getLastY(double currentTime)
        {
            if (positionY == null || positionY.Count == 0)
            {
                return 0; // Or your default value
            }

            var keys = positionY.Keys.ToList();
            int left = 0;
            int right = keys.Count - 1;
            double lastKey = -1;

            while (left <= right)
            {
                int mid = left + (right - left) / 2;
                if (keys[mid] < currentTime)
                {
                    lastKey = keys[mid];
                    left = mid + 1;
                }
                else
                {
                    right = mid - 1;
                }
            }

            return lastKey != -1 ? positionY[lastKey] : 0;
        }

        private void easeProgressAbsolute(OsbEasing ease, double start, double end, Vector2 startPos, Vector2 endPos)
        {

            double duration = Math.Max(end - start, 0); // Ensure non-negative duration
            double deltaTime = 0;
            Vector2 lastPos = startPos; // Keep track of the last position to calculate the delta

            double progress = 0;
            do
            {
                deltaTime += deltaIncrement; // Increment time by deltaIncrement
                progress = deltaTime / duration; // Normalized time [0, 1]
                progress = Math.Min(progress, 1);       // Clamp progress to 1 to avoid overshooting

                float t = (float)ease.Ease(progress);   // Apply easing function

                Vector2 newPos = Vector2.Lerp(startPos, endPos, t); // Interpolated position
                if ((newPos - PositionAt(start)).Length > endPos.Length)
                {
                    newPos = PositionAt(start) + endPos; // Clamp to the maximum offset
                }
                Vector2 movement = newPos - lastPos;               // Delta movement

                // Apply the delta movement
                AddXValue(start + deltaTime, movement.X, true);
                AddYValue(start + deltaTime, movement.Y, true);


                lastPos = newPos;   // Update lastPos for the next iteration
            } while (progress < 1);

        }

        private void easeProgressRelativeNormalized(OsbEasing ease, double start, double end, Vector2 offset, NoteOrigin origin)
        {
            double duration = Math.Max(end - start + 1, 0); // Ensure non-negative duration
            double deltaTime = 0;
            Vector2 startPos = PositionAt(start); // Get the initial global position
            Vector2 endPos = startPos + offset;   // The final desired position in global coordinates

            // Calculate the initial local axes based on the origin
            Vector2 receptorPos = PositionAt(start);
            Vector2 originPos = origin.PositionAt(start);
            Vector2 directionToOrigin = originPos - receptorPos;
            float distanceToOrigin = directionToOrigin.Length;

            // Avoid division by zero
            if (distanceToOrigin == 0)
            {
                Utility.Log("Warning: Distance to origin is zero. Aborting movement.");
                return;
            }

            Vector2 localYAxis = directionToOrigin.Normalized();
            Vector2 localXAxis = new Vector2(-localYAxis.Y, localYAxis.X);

            // Normalize local axes to ensure stability
            localXAxis.Normalize();
            localYAxis.Normalize();

            Vector2 lastPos = startPos; // Start at the origin of the local coordinate system

            double progress = 0;
            do
            {
                deltaTime += deltaIncrement; // Increment time by deltaIncrement
                progress = deltaTime / duration; // Normalized time [0, 1]
                progress = Math.Min(progress, 1); // Clamp progress to 1 to avoid overshooting

                // Calculate the eased movement in the local coordinate system
                float t = (float)ease.Ease(progress);   // Apply easing function

                Vector2 newPos = Vector2.Lerp(startPos, endPos, t); // Interpolated position
                Vector2 movement = newPos - lastPos;               // Delta movement

                // Transform the local movement to the global coordinate system
                Vector2 globalMovement = (localXAxis * movement.X) + (localYAxis * movement.Y);

                // Apply the delta movement in the global coordinate system
                AddXValue(start + deltaTime, globalMovement.X);
                AddYValue(start + deltaTime, globalMovement.Y);

                // Update the last local position
                lastPos = newPos;

                // Safeguard: Break if movement becomes unstable
                if (globalMovement.Length > 1000) // Arbitrary threshold for debugging
                {
                    Utility.Log("Warning: Movement delta too large. Breaking loop.");
                    break;
                }

                // Debugging: Log the current state
                Utility.Log($"Progress: {progress}, LocalTargetPos: {lastPos}, GlobalMovement: {globalMovement}, LocalXAxis: {localXAxis}, LocalYAxis: {localYAxis}");
            } while (progress < 1);
        }

        private void easeProgressRelative(OsbEasing ease, double start, double end, Vector2 offset)
        {
            Vector2 startPos = new Vector2(0, 0); // Assuming starting at origin; replace with actual start if different
            Vector2 endPos = startPos + offset;   // The final desired position

            double duration = Math.Max(end - start + 1, 0); // Ensure non-negative duration
            double deltaTime = 0;
            Vector2 lastPos = startPos; // Keep track of the last position to calculate the delta

            double progress = 0;
            do
            {
                deltaTime += deltaIncrement; // Increment time by deltaIncrement
                progress = deltaTime / duration; // Normalized time [0, 1]
                progress = Math.Min(progress, 1);       // Clamp progress to 1 to avoid overshooting

                float t = (float)ease.Ease(progress);   // Apply easing function

                Vector2 newPos = Vector2.Lerp(startPos, endPos, t); // Interpolated position
                Vector2 movement = newPos - lastPos;               // Delta movement

                // Apply the delta movement
                if (offset.X != 0)
                    AddXValue(start + deltaTime, movement.X);
                if (offset.Y != 0)
                    AddYValue(start + deltaTime, movement.Y);


                lastPos = newPos;   // Update lastPos for the next iteration
            } while (progress < 1);

        }


        public Vector2 PositionAt(double time)
        {
            return new Vector2(getLastX(time), getLastY(time));
        }


    }
}