using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using OpenTK;
using OpenTK.Graphics;
using StorybrewCommon.Animations;
using StorybrewCommon.Mapset;
using StorybrewCommon.Scripting;
using StorybrewCommon.Storyboarding;
using StorybrewCommon.Storyboarding3d;

namespace StorybrewScripts
{
    public class Groovecoaster : StoryboardObjectGenerator
    {
//        public override bool Multithreaded => false;
        static double startTime = 181221;
        static double endTime = 205995;
        public override void Generate()
        {

            Scene3d scene = new();
            var camera = new PerspectiveCamera();

            // Set up camera base parameters
            camera.VerticalFov.Add(startTime, 65);
            camera.HorizontalFov.Add(startTime, 65);
            camera.FarFade.Add(startTime, 2500);
            camera.FarClip.Add(startTime, 2500);

            camera.TargetPosition.Add(startTime, new Vector3(0, 0, 0));
            camera.PositionX.Add(startTime, 0);
            camera.PositionY.Add(startTime, 20);
            camera.PositionY.Add(182769, -50, EasingFunctions.SineOut);
            camera.PositionZ.Add(startTime, -125);

            var path = new Path3D()
                .AddPoint(new Vector3(0, -150, 0))
                .AddPoint(new Vector3(0, 0, 200))
                .AddPoint(new Vector3(-100, -25, 400))
                .AddPoint(new Vector3(-300, 0, 600)) // + 2500
                .AddPoint(new Vector3(-350, 0, 600)) // 50
                .AddPoint(new Vector3(-350, 0, 650))// 50
                .AddPoint(new Vector3(-300, 0, 650))// 50
                .AddPoint(new Vector3(-300, -50, 650))// 50
                .AddPoint(new Vector3(-350, -50, 650))// 50
                .AddPoint(new Vector3(-350, -50, 700)) // 50
                .AddPoint(new Vector3(-350, 0, 700)) // 50
                .AddPoint(new Vector3(-400, 0, 700)) // 50
                .AddPoint(new Vector3(-400, 0, 650)) // 50
                .AddPoint(new Vector3(-450, 0, 650)) // 50
                .AddPoint(new Vector3(-500, 0, 650)) // 50
                .AddPoint(new Vector3(-500 - 50, -25, 700))    // First quarter turn going up
                .AddPoint(new Vector3(-450 - 50, -50, 750))   // Second quarter turn  
                .AddPoint(new Vector3(-400 - 50, -75, 700))   // Third quarter turn
                .AddPoint(new Vector3(-450 - 50, -100, 650))  // Complete first rotation
                .AddPoint(new Vector3(-500 - 50, -125, 700))  // Continue second rotation
                .AddPoint(new Vector3(-450 - 50, -150, 750))
                .AddPoint(new Vector3(-400 - 50, -175, 700))
                .AddPoint(new Vector3(-450 - 50, -200, 650))  // Complete second rotation
                .AddPoint(new Vector3(-500 - 50, -225, 700))
                .AddPoint(new Vector3(-450 - 50, -250, 750))  // End of spiral

                .AddPoint(new Vector3(-500 + 1280 - 900, 0, 650))  // End of spiral

                .AddControlPoint(0, new Vector3(0, -0, 0))
                .AddControlPoint(1, new Vector3(0, -25, 450))
                .AddControlPoint(2, new Vector3(-300, 25, 350))
                .AddControlPoint(14, new Vector3(-550, -10, 650))
                .AddControlPoint(15, new Vector3(-550, -35, 750))
                .AddControlPoint(16, new Vector3(-450, -60, 750))
                .AddControlPoint(17, new Vector3(-450, -85, 650))
                .AddControlPoint(18, new Vector3(-550, -110, 650))
                .AddControlPoint(19, new Vector3(-550, -135, 750))
                .AddControlPoint(20, new Vector3(-450, -160, 750))
                .AddControlPoint(21, new Vector3(-450, -185, 650))
                .AddControlPoint(22, new Vector3(-550, -210, 650))
                .AddControlPoint(23, new Vector3(-550, -235, 750))
                .AddControlPoint(24, new Vector3(-500 + 1280 - 750, -250, 750));

            // Generate path with 100 interpolated points
            var interpolatedPath = path.GenerateInterpolatedPath(500);

            // Use the path for your object animation
            Node3d pathNode = CreatePathFromInterpolatedPoints(interpolatedPath);

            Node3d wireframe = CreateWireframePlane(scene, startTime, planeSize: 500, gridDetail: 20, position: new Vector3(0, 0, 0));

            // Add notes along the path
            Node3d notes = PlaceNotesAlongPath(interpolatedPath);

            pathNode.Rotation.Add(startTime, Quaternion.FromEulerAngles(new Vector3(0, 0, 0)));
            pathNode.Rotation.Add(182769, Quaternion.FromEulerAngles(0f, 1f, 0), EasingFunctions.SineInOut);
            pathNode.Rotation.Add(188421, Quaternion.FromEulerAngles(0f, -.5f, 0), EasingFunctions.SineInOut);
            pathNode.Rotation.Add(188963, Quaternion.FromEulerAngles(0f, -.5f, 0), EasingFunctions.SineInOut);
            pathNode.Rotation.Add(191092, Quaternion.FromEulerAngles(0f, -1f, 0), EasingFunctions.SineInOut);
            pathNode.Rotation.Add(193608, Quaternion.FromEulerAngles(0f, 0f, 0), EasingFunctions.SineInOut);
            pathNode.Rotation.Add(200945, Quaternion.FromEulerAngles(0f, -40f, 0), EasingFunctions.SineInOut);
            pathNode.Rotation.Add(endTime, Quaternion.FromEulerAngles(20f, -20f, -20f), EasingFunctions.SineIn);

            for (int i = 0; i < interpolatedPath.TimePoints.Count; i++)
            {
                int time = interpolatedPath.TimePoints[i];
                Vector3 position = interpolatedPath.Positions[i];

                Quaternion rotation = pathNode.Rotation.ValueAt(time);

                Vector3 adjustedPosition = Vector3.Transform(position, Matrix3.CreateFromQuaternion(rotation));

                pathNode.PositionX.Add(time, -adjustedPosition.X);
                pathNode.PositionY.Add(time, -adjustedPosition.Y);
                pathNode.PositionZ.Add(time, -adjustedPosition.Z);

                notes.PositionX.Add(time, -adjustedPosition.X);
                notes.PositionY.Add(time, -adjustedPosition.Y);
                notes.PositionZ.Add(time, -adjustedPosition.Z);

                Quaternion scaledRotation = Quaternion.Slerp(Quaternion.Identity, new Quaternion(rotation.X, rotation.Y, rotation.Z), 0.15f);
                wireframe.Rotation.Add(time, scaledRotation);

                notes.Rotation.Add(time, rotation);

                wireframe.PositionX.Add(time, -adjustedPosition.X / 10);
                if (time != startTime)
                    wireframe.PositionY.Add(time, 15 - adjustedPosition.Y / 10);
                wireframe.PositionZ.Add(time, 100 - adjustedPosition.Z / 10);
            }

            wireframe.Opacity.Add(startTime, 0f);
            wireframe.Opacity.Add(startTime + 1000, 1f, EasingFunctions.SineIn);
            wireframe.PositionY.Add(startTime, 80);
            wireframe.PositionY.Add(interpolatedPath.TimePoints[1], 15, EasingFunctions.SineOut);

            wireframe.PositionZ.Add(startTime, 100);

            Node3d receptors = new Node3d();

            Sprite3d col1 = new Sprite3d
            {
                SpritePath = "sb/sprites/receiver.png",
            };

            Sprite3d col2 = new Sprite3d
            {
                SpritePath = "sb/sprites/receiver.png",
            };

            Sprite3d col3 = new Sprite3d
            {
                SpritePath = "sb/sprites/receiver.png",
            };

            Sprite3d col4 = new Sprite3d
            {
                SpritePath = "sb/sprites/receiver.png",
            };

            col1.SpriteRotation.Add(startTime, 0);
            col2.SpriteRotation.Add(startTime, Math.PI / 2);
            col3.SpriteRotation.Add(startTime, Math.PI / 2 * 2);
            col4.SpriteRotation.Add(startTime, -Math.PI / 2);

            col1.Opacity.Add(startTime, 0);
            col2.Opacity.Add(startTime, 0f);
            col3.Opacity.Add(startTime, 0f);
            col4.Opacity.Add(startTime, 0f);

            col1.Opacity.Add(182673 - 1, 0);
            col2.Opacity.Add(182673 - 1, 0f);
            col3.Opacity.Add(182673 - 1, 0f);
            col4.Opacity.Add(182673 - 1, 0f);

            col1.Opacity.Add(182673, .8f);
            col2.Opacity.Add(182673, .8f);
            col3.Opacity.Add(182673, .8f);
            col4.Opacity.Add(182673, .8f);

            col1.SpriteScale.Add(startTime, 0.09f);
            col2.SpriteScale.Add(startTime, 0.09f);
            col3.SpriteScale.Add(startTime, 0.09f);
            col4.SpriteScale.Add(startTime, 0.09f);

            double currentTime = startTime; // Initialize currentTime
            double beatDuration = Beatmap.GetControlPointAt((int)startTime).BeatDuration; // Calculate beat duration
            double halfDuration = beatDuration / 2;
            double offset = Beatmap.GetControlPointAt((int)startTime).Offset; // Calculate beat duration

            // Calculate the new adjusted currentTime with the offset
            double adjustedTime = Math.Ceiling((currentTime - offset) / beatDuration) * beatDuration + offset;

            col1.Coloring.Add(adjustedTime, new Color4(255, 255, 255, 255));
            col2.Coloring.Add(adjustedTime, new Color4(255, 255, 255, 255));
            col3.Coloring.Add(adjustedTime, new Color4(255, 255, 255, 255));
            col4.Coloring.Add(adjustedTime, new Color4(255, 255, 255, 255));

            while (adjustedTime < endTime)
            {
                col1.Coloring.Add(adjustedTime + halfDuration, new Color4(97, 97, 97, 0), EasingFunctions.CircOut);
                col1.Coloring.Add(adjustedTime + beatDuration, new Color4(255, 255, 255, 255), EasingFunctions.CircIn);

                col2.Coloring.Add(adjustedTime + halfDuration, new Color4(97, 97, 97, 0), EasingFunctions.CircOut);
                col2.Coloring.Add(adjustedTime + beatDuration, new Color4(255, 255, 255, 255), EasingFunctions.CircIn);

                col3.Coloring.Add(adjustedTime + halfDuration, new Color4(97, 97, 97, 0), EasingFunctions.CircOut);
                col3.Coloring.Add(adjustedTime + beatDuration, new Color4(255, 255, 255, 255), EasingFunctions.CircIn);

                col4.Coloring.Add(adjustedTime + halfDuration, new Color4(97, 97, 97, 0), EasingFunctions.CircOut);
                col4.Coloring.Add(adjustedTime + beatDuration, new Color4(255, 255, 255, 255), EasingFunctions.CircIn);

                adjustedTime += beatDuration;
            }

            receptors.Add(col3);
            receptors.Add(col4);
            receptors.Add(col1);
            receptors.Add(col2);

            scene.Add(wireframe);
            scene.Add(pathNode);
            scene.Add(receptors);
            scene.Add(notes);

            scene.Generate(camera, GetLayer("3d"), startTime, endTime, Beatmap, 6);
        }

        // Class to represent a 3D path with timing and events
        public class Path3D
        {
            private List<Vector3> points = new List<Vector3>();
            private Dictionary<double, Vector3[]> controlPoints = new Dictionary<double, Vector3[]>();
            private Dictionary<int, string> events = new Dictionary<int, string>();

            // Add a point with time
            public Path3D AddPoint(Vector3 position)
            {
                points.Add(position);
                return this;
            }

            // Add control points for bezier curve (1 or 2 control points)
            public Path3D AddControlPoint(int segmentIndex, Vector3 controlPoint1, Vector3? controlPoint2 = null)
            {
                if (segmentIndex >= 0 && segmentIndex < points.Count - 1)
                {
                    if (controlPoint2.HasValue)
                        controlPoints[segmentIndex] = new[] { controlPoint1, controlPoint2.Value };
                    else
                        controlPoints[segmentIndex] = new[] { controlPoint1 };
                }
                return this;
            }

            // Add an event at a specific time
            public Path3D AddEvent(int time, string eventName)
            {
                events[time] = eventName;
                return this;
            }

            // Generate an interpolated path with a specified number of points
            public InterpolatedPath3D GenerateInterpolatedPath(int pointCount)
            {
                var result = new InterpolatedPath3D();

                // First, estimate the true length of the path including curves
                List<double> segmentLengths = new List<double>();
                double totalLength = 0;

                // Number of samples to use when estimating curve length
                const int samplesPerSegment = 20;

                for (int i = 0; i < points.Count - 1; i++)
                {
                    var startPos = points[i];
                    var endPos = points[i + 1];
                    double segmentLength = 0;

                    // If this segment has control points, calculate curved length
                    if (controlPoints.ContainsKey(i))
                    {
                        var controls = controlPoints[i];
                        Vector3 prevPoint = startPos;

                        // Sample points along the curve to approximate its length
                        for (int j = 1; j <= samplesPerSegment; j++)
                        {
                            float t = j / (float)samplesPerSegment;
                            Vector3 curPoint;

                            if (controls.Length == 1)
                                curPoint = QuadraticBezier(startPos, controls[0], endPos, t);
                            else
                                curPoint = CubicBezier(startPos, controls[0], controls[1], endPos, t);

                            segmentLength += (curPoint - prevPoint).Length;
                            prevPoint = curPoint;
                        }
                    }
                    else
                    {
                        // For straight segments, use direct distance
                        segmentLength = (endPos - startPos).Length;
                    }

                    segmentLengths.Add(segmentLength);
                    totalLength += segmentLength;
                }

                // Now distribute points evenly along the true path length
                double segmentDistance = totalLength / (pointCount - 1);
                double accumulatedDistance = 0;
                int currentSegment = 0;

                for (int i = 0; i < pointCount; i++)
                {
                    double targetDistance = i * segmentDistance;

                    // Find the segment containing this point
                    while (currentSegment < segmentLengths.Count &&
                           accumulatedDistance + segmentLengths[currentSegment] < targetDistance)
                    {
                        accumulatedDistance += segmentLengths[currentSegment];
                        currentSegment++;
                    }

                    if (currentSegment >= segmentLengths.Count)
                        break;

                    // Calculate segment-relative position (parametric t)
                    var startPos = points[currentSegment];
                    var endPos = points[currentSegment + 1];
                    double localTargetDistance = targetDistance - accumulatedDistance;

                    // Find t value that gives the correct distance along the curve
                    float t;
                    if (controlPoints.ContainsKey(currentSegment))
                    {
                        // Binary search to find t value corresponding to desired distance
                        t = FindParameterForDistance(startPos, endPos, controlPoints[currentSegment],
                                                     localTargetDistance, segmentLengths[currentSegment]);
                    }
                    else
                    {
                        // For straight lines, t is proportional to distance
                        t = (float)(localTargetDistance / segmentLengths[currentSegment]);
                    }

                    // Calculate position using the found t value
                    Vector3 position;
                    if (controlPoints.ContainsKey(currentSegment))
                    {
                        var controls = controlPoints[currentSegment];
                        if (controls.Length == 1)
                            position = QuadraticBezier(startPos, controls[0], endPos, t);
                        else
                            position = CubicBezier(startPos, controls[0], controls[1], endPos, t);
                    }
                    else
                    {
                        position = Vector3.Lerp(startPos, endPos, t);
                    }

                    // Calculate interpolated time
                    double currentTime = startTime + (endTime - startTime) * (i / (double)(pointCount - 1));

                    result.TimePoints.Add((int)currentTime);
                    result.Positions.Add(position);
                }

                // Add events to the result
                foreach (var evt in events)
                {
                    result.Events[evt.Key] = evt.Value;
                }

                return result;
            }

            // Helper method to find t parameter corresponding to a specific distance along a curve
            private float FindParameterForDistance(Vector3 start, Vector3 end, Vector3[] controls,
                                                  double targetDistance, double totalLength)
            {
                // Binary search to find t
                float tMin = 0f;
                float tMax = 1f;
                float tMid = 0.5f;
                float epsilon = 0.001f; // Precision threshold

                // Max iterations to prevent infinite loop
                int maxIterations = 20;
                int iterations = 0;

                while (iterations < maxIterations)
                {
                    // Calculate distance at tMid
                    double distAtTMid = CalculateDistanceAlongCurve(start, end, controls, tMid);

                    // Check if we're close enough
                    if (Math.Abs(distAtTMid - targetDistance) < epsilon * totalLength)
                        break;

                    // Adjust search range
                    if (distAtTMid < targetDistance)
                        tMin = tMid;
                    else
                        tMax = tMid;

                    tMid = (tMin + tMax) / 2;
                    iterations++;
                }

                return tMid;
            }

            // Helper method to calculate distance along a curve from start to parameter t
            private double CalculateDistanceAlongCurve(Vector3 start, Vector3 end, Vector3[] controls, float t)
            {
                // Use a small number of samples to approximate distance
                const int samples = 10;
                float stepSize = t / samples;

                Vector3 prevPoint = start;
                double distance = 0;

                for (int i = 1; i <= samples; i++)
                {
                    float currentT = i * stepSize;
                    Vector3 currentPoint;

                    if (controls.Length == 1)
                        currentPoint = QuadraticBezier(start, controls[0], end, currentT);
                    else
                        currentPoint = CubicBezier(start, controls[0], controls[1], end, currentT);

                    distance += (currentPoint - prevPoint).Length;
                    prevPoint = currentPoint;
                }

                return distance;
            }

            // Quadratic bezier curve formula
            private Vector3 QuadraticBezier(Vector3 p0, Vector3 p1, Vector3 p2, float t)
            {
                float u = 1 - t;
                float tt = t * t;
                float uu = u * u;

                Vector3 p = uu * p0; // (1-t)^2 * P0
                p += 2 * u * t * p1; // 2(1-t)t * P1
                p += tt * p2; // t^2 * P2

                return p;
            }

            // Cubic bezier curve formula
            private Vector3 CubicBezier(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
            {
                float u = 1 - t;
                float tt = t * t;
                float uu = u * u;
                float uuu = uu * u;
                float ttt = tt * t;

                Vector3 p = uuu * p0; // (1-t)^3 * P0
                p += 3 * uu * t * p1; // 3(1-t)^2 * t * P1
                p += 3 * u * tt * p2; // 3(1-t) * t^2 * P2
                p += ttt * p3; // t^3 * P3

                return p;
            }
        }

        // Class to store the interpolated path data
        public class InterpolatedPath3D
        {
            public List<int> TimePoints { get; } = new List<int>();
            public List<Vector3> Positions { get; } = new List<Vector3>();
            public Dictionary<int, string> Events { get; } = new Dictionary<int, string>();
        }

        // Create a visual representation of the path using Line3d segments
        private Node3d CreatePathFromInterpolatedPoints(InterpolatedPath3D path)
        {
            Node3d pathNode = new Node3d();

            // Draw path using Line3d segments for visualization
            for (int i = 0; i < path.TimePoints.Count - 1; i++)
            {
                Line3d line = new Line3d
                {
                    SpritePath = "sb/white.png",
                };

                line.CommandSplitThreshold = 250;

                int time = path.TimePoints[i];
                line.StartPosition.Add(time, path.Positions[i]);
                line.EndPosition.Add(time, path.Positions[i + 1]);
                line.Thickness.Add(time, 5f);

                line.Opacity.Add(startTime, 1);
                line.Opacity.Add(time + 250, 1);
                line.Opacity.Add(time + 1000, 0, EasingFunctions.CubicOut);

                // Optionally highlight event points with different color
                if (path.Events.ContainsKey(time))
                {
                    line.Coloring.Add(time, new Color4(255, 255, 0, 255)); // Yellow for event points
                }

                pathNode.Add(line);
            }

            return pathNode;
        }

        // Handle specific events on the path
        private void HandlePathEvent(string eventName, int time, Vector3 position)
        {
            switch (eventName)
            {
                case "CameraShake":
                    // Add camera shake effect
                    break;
                case "SpawnObject":
                    // Spawn an object at this position
                    break;
                case "ColorChange":
                    // Change color of path or scene
                    break;
                    // Add more event types as needed
            }
        }

        private Node3d CreateWireframePlane(Scene3d scene, double time, float planeSize = 200f, int gridDetail = 20, Vector3 position = default, Vector4? color = null)
        {
            float cellSize = planeSize / gridDetail;
            float halfSize = planeSize / 2;

            Node3d wireFrame = new();

            for (int i = 0; i <= gridDetail; i++)
            {
                float pos = -halfSize + i * cellSize;

                Line3d xLine = new() { SpritePath = "sb/white.png" };
                xLine.StartPosition.Add(time, position + new Vector3(-halfSize, 0, pos));
                xLine.EndPosition.Add(time, position + new Vector3(halfSize, 0, pos));
                xLine.Thickness.Add(time, 1f);

                xLine.ConfigureGenerators((s) =>
                {
                    s.RotationDecimals = 4;
                    s.RotationTolerance = 0.0001f;
                    s.ScaleDecimals = 4;
                    s.ScaleTolerance = 0.0001f;
                });

                xLine.CommandSplitThreshold = 250;

                wireFrame.Add(xLine);
                Line3d zLine = new() { SpritePath = "sb/white.png" };
                zLine.StartPosition.Add(time, position + new Vector3(pos, 0, -halfSize));
                zLine.EndPosition.Add(time, position + new Vector3(pos, 0, halfSize));
                zLine.Thickness.Add(time, 1f);

                zLine.ConfigureGenerators((s) =>
                {
                    s.RotationDecimals = 4;
                    s.RotationTolerance = 0.0001f;
                    s.ScaleDecimals = 4;
                    s.ScaleTolerance = 0.0001f;
                });

                zLine.CommandSplitThreshold = 250;
                wireFrame.Add(zLine);
            }

            return wireFrame;
        }

        // Method to place notes along the path
        private Node3d PlaceNotesAlongPath(InterpolatedPath3D path)
        {
            // Define note placement timings based on BPM
            double beatLength = Beatmap.GetControlPointAt((int)startTime).BeatDuration;

            Node3d noteNode = new Node3d();

            var objects = Beatmap.HitObjects.Where(h => h.StartTime >= startTime && h.StartTime <= endTime).ToList();
            objects.Reverse();

            // Group objects by time to detect duplicates
            var objectsByTime = objects.GroupBy(h => h.StartTime).ToDictionary(g => g.Key, g => g.ToList());

            // Place notes at each time, finding the exact position on the path
            foreach (var timeGroup in objectsByTime)
            {
                double noteTime = timeGroup.Key;
                var hitObjects = timeGroup.Value;

                // If there's only one object at this time, process normally
                if (hitObjects.Count == 1)
                {
                    OsuHitObject hitObject = hitObjects[0];
                    double rotation = GetRotationFromPosition(hitObject.Position.X);
                    if (rotation != double.NegativeInfinity) // Skip notes with invalid positions
                    {
                        CreateNoteAtExactTimeWithOffset(noteNode, path, noteTime, rotation, Vector3.Zero, 0);
                    }
                }
                // If there are multiple objects at the same time
                else
                {
                    // Create a list to track rotations for this timegroup
                    List<double> rotations = new List<double>();

                    foreach (OsuHitObject hitObject in hitObjects)
                    {
                        double rotation = GetRotationFromPosition(hitObject.Position.X);
                        if (rotation != double.NegativeInfinity) // Skip notes with invalid positions
                        {
                            rotations.Add(rotation);

                            // Create notes with initial offset based on rotation
                            Vector3 offset = GetOffsetFromRotation(rotation);

                            CreateNoteAtExactTimeWithOffset(noteNode, path, noteTime, rotation, offset, 500);
                        }
                    }
                }
            }

            return noteNode;
        }

        // Helper method to get rotation based on note position
        private double GetRotationFromPosition(float positionX)
        {
            switch ((int)positionX)
            {
                case 128: return Math.PI / 2;      // Left
                case 256: return 0;                  // Up
                case 384: return Math.PI;   // Down
                case 512: return -Math.PI / 2;        // Right
                default: return double.NegativeInfinity; // Invalid position
            }
        }

        // Helper method to get offset vector from rotation
        private Vector3 GetOffsetFromRotation(double rotation)
        {
            // Calculate offset (20 units in the direction perpendicular to the note's forward direction)
            float offsetDistance = 10f;

            // Adjust for rotation to create offsets in appropriate directions

            switch (rotation)
            {
                case 0: return new Vector3(0, -offsetDistance, 0);
                case Math.PI / 2: return new Vector3(offsetDistance, 0, 0);
                case Math.PI: return new Vector3(0, offsetDistance, 0);
                case -Math.PI / 2: return new Vector3(-offsetDistance, 0, 0);
            }

            return new Vector3(0, offsetDistance, 0);

        }

        // Method to create a note with initial offset that merges to the path
        private void CreateNoteAtExactTimeWithOffset(Node3d node, InterpolatedPath3D path, double noteTime, double rotation, Vector3 offset, double mergeDuration)
        {
            // Find position on path for both initial placement and hit time
            Vector3 notePosition = GetPositionAtTime(path, noteTime);

            var noteType = GetNoteType(noteTime);

            // Create the note sprite
            Sprite3d note = new Sprite3d
            {
                SpritePath = $"sb/sprites/{noteType}.png",
                UseDistanceFade = false
            };

            // Set initial properties with offset
            note.SpriteScale.Add(startTime, 0.09f);
            note.RotationMode = RotationMode.Fixed;
            note.SpriteRotation.Add(startTime, rotation);

            // Start position with offset
            note.PositionX.Add(startTime, notePosition.X + offset.X);
            note.PositionY.Add(startTime, notePosition.Y + offset.Y);
            note.PositionZ.Add(startTime, notePosition.Z + offset.Z);

            note.PositionX.Add(noteTime - mergeDuration, notePosition.X + offset.X);
            note.PositionY.Add(noteTime - mergeDuration, notePosition.Y + offset.Y);
            note.PositionZ.Add(noteTime - mergeDuration, notePosition.Z + offset.Z);

            // Merge position - remove offset and join the main path
            note.PositionX.Add(noteTime, notePosition.X, EasingFunctions.CubicOut);
            note.PositionY.Add(noteTime, notePosition.Y, EasingFunctions.CubicOut);
            note.PositionZ.Add(noteTime, notePosition.Z, EasingFunctions.CubicOut);

            // Fade out at hit time
            note.Opacity.Add(startTime, 1);
            note.Opacity.Add(noteTime - 1, 1);
            note.Opacity.Add(noteTime, 0);

            note.CommandSplitThreshold = 250;

            node.Add(note);
        }

        // Helper method to get position on path at a specific time
        private Vector3 GetPositionAtTime(InterpolatedPath3D path, double time)
        {
            // Find the exact index or interpolate between points
            if (path.TimePoints.Contains((int)time))
            {
                // If the exact time exists in our path points
                int pathIndex = path.TimePoints.IndexOf((int)time);
                return path.Positions[pathIndex];
            }
            else
            {
                // Find the surrounding time points and interpolate
                int pathIndex = 0;

                // Find the time points that surround our target time
                for (int i = 0; i < path.TimePoints.Count - 1; i++)
                {
                    if (path.TimePoints[i] <= time && path.TimePoints[i + 1] > time)
                    {
                        pathIndex = i;
                        break;
                    }
                }

                // If we're beyond the last point, use the last point
                if (time >= path.TimePoints[path.TimePoints.Count - 1])
                {
                    pathIndex = path.TimePoints.Count - 1;
                    return path.Positions[pathIndex];
                }
                else
                {
                    // Interpolate between the two surrounding positions
                    int time1 = path.TimePoints[pathIndex];
                    int time2 = path.TimePoints[pathIndex + 1];
                    Vector3 pos1 = path.Positions[pathIndex];
                    Vector3 pos2 = path.Positions[pathIndex + 1];

                    // Calculate the interpolation factor (0 to 1)
                    float factor = (float)(time - time1) / (time2 - time1);

                    // Linear interpolation between positions
                    return Vector3.Lerp(pos1, pos2, factor);
                }
            }
        }

        // Helper method to determine note type
        private int GetNoteType(double noteTime)
        {
            var beatDuration = Beatmap.GetControlPointAt((int)noteTime).BeatDuration;
            var offset = Beatmap.GetControlPointAt((int)noteTime).Offset;

            int cycle = (int)Math.Floor((noteTime - offset) / beatDuration);
            int adjustedTime = (int)Math.Round(noteTime - offset - (cycle * beatDuration));

            var notetype = 1;

            if (IsCloseTo(adjustedTime, beatDuration, 32)) notetype = 16;
            if (IsCloseTo(adjustedTime, beatDuration, 24)) notetype = 12;
            if (IsCloseTo(adjustedTime, beatDuration, 16)) notetype = 16;
            if (IsCloseTo(adjustedTime, beatDuration, 12)) notetype = 12;
            if (IsCloseTo(adjustedTime, beatDuration, 4)) notetype = 4;
            if (IsCloseTo(adjustedTime, beatDuration, 3)) notetype = 3;
            if (IsCloseTo(adjustedTime, beatDuration, 2)) notetype = 2;
            if (IsCloseTo(adjustedTime, beatDuration, 1)) notetype = 1;

            return notetype; // Default
        }

        bool IsCloseTo(int adjusted, double beatDuration, int divisions, int margin = 2)
        {
            double baseTick = beatDuration / divisions;
            for (int multiplier = 1; multiplier <= divisions; multiplier++)
            {
                if (Math.Abs(adjusted - (baseTick * multiplier)) <= margin)
                    return true;
            }
            return false;
        }

    }
}
