using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Input;
using StorybrewCommon.Animations;
using StorybrewCommon.Scripting;
using StorybrewCommon.Storyboarding;
using StorybrewCommon.Storyboarding.CommandValues;

namespace StorybrewScripts
{

    public class NoteOrigin
    {

        public string receptorSpritePath = "";
        public StoryboardLayer layer;
        public OsbSprite originSprite;

        public SortedDictionary<double, float> positionX = new SortedDictionary<double, float>();
        public SortedDictionary<double, float> positionY = new SortedDictionary<double, float>();

        public KeyframedValue<Vector2> Scale = new KeyframedValue<Vector2>((from, to, progress) => Vector2.Lerp(from, to, (float)progress), new Vector2(0.5f));

        public OsbSprite debug;

        private readonly object lockX = new object();
        private readonly object lockY = new object();

        // Rotation in radiants
        public double rotation = 0f;

        public float defaultScale = 0.5f;

        private double deltaIncrement = 1;

        public NoteOrigin(String receptorSpritePath, double rotation, StoryboardLayer layer, CommandScale scale, double starttime, double delta)
        {

            this.deltaIncrement = delta;

            OsbSprite origin = layer.CreateSprite("sb/sprites/arrow.png", OsbOrigin.Centre);
            origin.Rotate(starttime - 1, rotation);
            Scale.Add(starttime, new Vector2(scale.X, scale.Y));

            positionX.Add(0, 0);
            positionY.Add(0, 0);

            this.receptorSpritePath = receptorSpritePath;
            this.rotation = rotation;
            this.layer = layer;
            this.originSprite = origin;

        }

        public void Render(double starttime, double endTime)
        {
            OsbSprite receptor = this.originSprite;

            receptor.Fade(starttime, 0.5f);
            receptor.Fade(endTime, 0);

        }

        public void MoveOriginAbsolute(double starttime, Vector2 endPos)
        {

            AddXValue(starttime, endPos.X, true);
            AddYValue(starttime, endPos.Y, true);


        }

        public void MoveOriginAbsolute(OsbEasing ease, double starttime, double endtime, Vector2 startPos, Vector2 endPos)
        {

            if (starttime == endtime)
            {
                AddXValue(starttime, endPos.X, true);
                AddYValue(starttime, endPos.Y, true);
                return;
            }

            easeProgressAbsolute(ease, starttime, endtime, startPos, endPos);

        }

        public void MoveOriginRelative(OsbEasing ease, double starttime, double endtime, Vector2 offset)
        {

            if (starttime == endtime)
            {
                AddXValue(starttime, offset.X);
                AddYValue(starttime, offset.Y);
                return;
            }

            easeProgressRelative(ease, starttime, endtime, offset);

        }

        public void MoveOriginRelative(OsbEasing ease, double starttime, double endtime, Vector2 offset, Vector2 absolute)
        {

            if (starttime == endtime)
            {
                AddXValue(starttime, offset.X);
                AddYValue(starttime, offset.Y);
                return;
            }

            easeProgressRelative(ease, starttime, endtime, offset);

        }

        public void MoveOriginRelativeNorm(OsbEasing ease, double starttime, double endtime, Vector2 offset, Receptor receptor)
        {
            easeProgressRelativeNormalized(ease, starttime, endtime, offset, receptor);
        }

        public void MoveOriginRelativeX(OsbEasing ease, double starttime, double endtime, float value)
        {

            if (starttime == endtime)
            {
                AddXValue(starttime, value);
                return;
            }

            easeProgressRelative(ease, starttime, endtime, new Vector2(value, 0));

        }

        public void MoveOriginRelativeY(OsbEasing ease, double starttime, double endtime, float value)
        {
            if (starttime == endtime)
            {
                AddYValue(starttime, value);
                return;
            }

            easeProgressRelative(ease, starttime, endtime, new Vector2(0, value));

        }

        public void ScaleOrigin(OsbEasing ease, double starttime, double endtime, Vector2 newScale)
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

        public void RotateOrigin(OsbEasing ease, double starttime, double endtime, double rotation)
        {
            OsbSprite receptor = this.originSprite;

            var newRotation = this.rotation + rotation;

            if (starttime == endtime)
            {
                receptor.Rotate(starttime, newRotation);
            }
            else
            {
                receptor.Rotate(ease, starttime, endtime, RotationAt(starttime), newRotation);
            }

            this.rotation = newRotation;

        }

        public void PivotOrigin(OsbEasing ease, double starttime, double endtime, double rotation, Vector2 center)
        {

            Vector2 initialPosition = PositionAt(starttime);

            if (starttime >= endtime)
            {
                // Handle instant movement
                Vector2 rotatedPoint = Utility.PivotPoint(initialPosition, center, rotation);

                Vector2 relativeMovement = rotatedPoint - initialPosition;
                // Apply the movement
                MoveOriginRelative(ease, starttime, starttime, relativeMovement);
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
                MoveOriginRelative(ease, currentTime, currentTime, relativeMovement);

                currentTime += 1;
            }
        }

        public void PivotOriginNorm(OsbEasing ease, double starttime, double endtime, double rotation, Vector2 center, Receptor receptor)
        {
            if (starttime >= endtime)
            {
                // Handle instant movement
                Vector2 rotatedPoint = Utility.PivotPoint(PositionAt(starttime), center, rotation);
                Vector2 relativeMovement = rotatedPoint - PositionAt(starttime);
                MoveOriginRelativeNorm(ease, starttime, starttime, relativeMovement, receptor);
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
                MoveOriginRelativeNorm(ease, currentTime, currentTime, relativeMovement, receptor);

                currentTime += deltaIncrement;
            }
        }

        private void AddXValue(double time, float value, bool absolute = false)
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
        }


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
                Vector2 movement = newPos - lastPos;               // Delta movement

                // Apply the delta movement
                AddXValue(start + deltaTime, movement.X, true);
                AddYValue(start + deltaTime, movement.Y, true);


                lastPos = newPos;   // Update lastPos for the next iteration
            } while (progress < 1);

        }

        private void
        easeProgressRelative(OsbEasing ease, double start, double end, Vector2 offset)
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
                Vector2 movement = newPos - lastPos;     // Delta movement

                // Apply the delta movement
                if (offset.X != 0)
                    AddXValue(start + deltaTime, movement.X);
                if (offset.Y != 0)
                    AddYValue(start + deltaTime, movement.Y);


                lastPos = newPos;   // Update lastPos for the next iteration
            } while (progress < 1);

        }

        private void easeProgressRelativeNormalized(OsbEasing ease, double start, double end, Vector2 offset, Receptor receptor)
        {
            double duration = Math.Max(end - start + 1, 0);
            double deltaTime = 0;

            double progress = 0;
            do
            {
                deltaTime += deltaIncrement;
                progress = deltaTime / duration;
                progress = Math.Min(progress, 1);

                float t = (float)ease.Ease(progress);

                // Get current positions of both receptor and origin
                Vector2 currentReceptorPos = receptor.PositionAt(start + deltaTime);
                Vector2 currentOriginPos = PositionAt(start + deltaTime);

                // Calculate direction vector from origin to receptor
                Vector2 direction = currentReceptorPos - currentOriginPos;

                // Normalize the direction vector
                if (direction.Length > 0)
                {
                    direction.Normalize();
                }

                // Calculate movement based on normalized direction
                Vector2 movement = direction * offset;

                // Apply the normalized movement
                if (offset.X != 0)
                    AddXValue(start + deltaTime, movement.X);
                if (offset.Y != 0)
                    AddYValue(start + deltaTime, movement.Y);

            } while (progress < 1);
        }


        public Vector2 PositionAt(double time)
        {
            return new Vector2(getLastX(time), getLastY(time));
        }

        public Vector2 ScaleAt(double currentTime)
        {
            if (Scale == null)
                return new Vector2(defaultScale, defaultScale);

            try
            {
                return Scale.ValueAt(currentTime);
            }
            catch (NullReferenceException)
            {
                // Fall back to default scale if an exception occurs
                return new Vector2(defaultScale, defaultScale);
            }
        }

        public float RotationAt(double currentTIme)
        {
            return originSprite.RotationAt(currentTIme);
        }
    }
}