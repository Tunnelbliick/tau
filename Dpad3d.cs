using OpenTK;
using OpenTK.Graphics;
using StorybrewCommon.Animations;
using StorybrewCommon.Mapset;
using StorybrewCommon.Scripting;
using StorybrewCommon.Storyboarding;
using StorybrewCommon.Storyboarding.Util;
using StorybrewCommon.Storyboarding3d;
using System;
using System.Collections.Generic;

namespace StorybrewScripts
{
    public class Dpad3d : StoryboardObjectGenerator
    {
        public override void Generate()
        {
            var startTime = 79801;
            var endTime = 91027;

            Scene3d scene = new();
            var camera = new PerspectiveCamera();

            // Camera setup
            camera.VerticalFov.Add(startTime, 50);
            camera.HorizontalFov.Add(startTime, 50);
            camera.FarFade.Add(startTime, 2500);
            camera.FarClip.Add(startTime, 2500);
            camera.NearClip.Add(startTime, 25);
            camera.NearFade.Add(startTime, 50);
            camera.TargetPosition.Add(startTime, new Vector3(0, 0, 0));
            camera.PositionX.Add(startTime, 0);
            camera.PositionZ.Add(startTime, -25);
            camera.PositionY.Add(startTime, 0);

            // Randomized pixel generation
            Random random = new Random();
            int pixelCount = 350; // Number of pixels to generate

            var scrollSpeed = 1700;

            // Create a list to store pixel data
            var pixels = new List<(float x, float y, float z, float rotation, Color4 color)>();

            for (int i = 0; i < pixelCount; i++)
            {
                // Randomize position, rotation, and depth
                float xg = Random(-25, 25); // Horizontal position
                float yg = Random(-25, 25); // Vertical position
                float zg = Random(0, scrollSpeed); // Depth for parallax effect
                float rotation = Random(0, 360); // Rotation in degrees

                // Randomize color blend
                Color4 color = new Color4(
                    (byte)Random(50, 255), // Red
                    (byte)Random(50, 255), // Green
                    (byte)Random(50, 255), // Blue
                    255 // Alpha
                );

                // Store the pixel data
                pixels.Add((xg, yg, zg, rotation, color));
            }

            // Sort pixels by Y value in descending order to start from the topmost pixel
            pixels.Sort((a, b) => b.z.CompareTo(a.z));

            Node3d pixelNode = new();

            int connectionsCreated = 0;
            int maxConnections = 400; // Limit the number of total connections 
            float maxConnectionDistance = 15; // Maximum distance for a connection
            int maxConnectionsPerPixel = 4; // Maximum number of connections per pixel

            // Create a dictionary to keep track of connections per pixel
            Dictionary<int, int> connectionsPerPixel = new Dictionary<int, int>();
            for (int i = 0; i < pixels.Count; i++)
            {
                connectionsPerPixel[i] = 0;
            }

            // For each pixel, find nearby pixels to connect
            for (int i = 0; i < pixels.Count && connectionsCreated < maxConnections; i++)
            {
                var (x1, y1, z1, _, color1) = pixels[i];

                // Skip this pixel if it already has maximum connections
                if (connectionsPerPixel[i] >= maxConnectionsPerPixel)
                    continue;

                // Sort neighbors by distance to prefer closer connections
                List<(int index, float distance)> neighbors = new List<(int, float)>();
                for (int j = i + 1; j < pixels.Count; j++)
                {
                    var (x2, y2, z2, _, _) = pixels[j];

                    // Skip if the target pixel already has maximum connections
                    if (connectionsPerPixel.ContainsKey(j) && connectionsPerPixel[j] >= maxConnectionsPerPixel)
                        continue;

                    // Calculate 3D distance between pixels
                    float distance = (float)Math.Sqrt(
                        Math.Pow(x2 - x1, 2) +
                        Math.Pow(y2 - y1, 2) +
                        Math.Pow(z2 - z1, 2));

                    // Only consider pixels within the maximum connection distance
                    if (distance <= maxConnectionDistance)
                    {
                        neighbors.Add((j, distance));
                    }
                }

                // Sort neighbors by distance (closest first)
                neighbors.Sort((a, b) => a.distance.CompareTo(b.distance));

                // Connect to the closest neighbors first, up to the per-pixel limit
                foreach (var (j, distance) in neighbors)
                {
                    // Stop if we've reached the maximum connections for this pixel
                    if (connectionsPerPixel[i] >= maxConnectionsPerPixel)
                        break;

                    // Stop if we've reached the maximum connections for the target pixel
                    if (connectionsPerPixel[j] >= maxConnectionsPerPixel)
                        continue;

                    // Stop if we've reached the maximum total connections
                    if (connectionsCreated >= maxConnections)
                        break;

                    var (x2, y2, z2, _, color2) = pixels[j];

                    // Create a line between the two pixels
                    Line3d connection = new Line3d
                    {
                        SpritePath = "sb/white.png",
                        UseDistanceFade = true
                    };

                    // Set line positions
                    connection.StartPosition.Add(startTime, new Vector3(x1, y1, z1));

                    connection.EndPosition.Add(startTime, new Vector3(x2, y2, z2));

                    // Animate the connection - it appears slightly after the scene starts
                    int connectionDelay = (int)(1.75f * (scrollSpeed - ((z1 + z2) / 2)));
                    int fadeInDuration = Random(300, 600);

                    connection.Opacity.Add(startTime, 0);
                    connection.Opacity.Add(startTime + connectionDelay, 0);
                    connection.Opacity.Add(startTime + connectionDelay + fadeInDuration, 1f, EasingFunctions.QuadOut);
                    connection.Opacity.Add(endTime - 1, 1f);
                    connection.Opacity.Add(endTime, 0);

                    // Make connections thinner than pixels
                    connection.Thickness.Add(startTime, 0.5f);

                    // Add to connections node
                    pixelNode.Add(connection);

                    // Update connection counts
                    connectionsPerPixel[i]++;
                    connectionsPerPixel[j]++;
                    connectionsCreated++;
                }
            }

            // Add pixels to the scene based on their Z depth
            foreach (var (x, y, z, rotation, color) in pixels)
            {
                // Adjust x dynamically based on z to keep objects in the camera's view
                float adjustedX = x; //* (1 + z / 100); // Scale x based on z depth
                float adjustedY = y; // * (1 + z / 100); // Scale x based on z depth

                // Create a pixel node
                Sprite3d pixel = new Sprite3d
                {
                    SpritePath = "sb/white.png",
                    UseDistanceFade = false
                };

                pixel.Opacity.Add(startTime, 1);
                pixel.Opacity.Add(endTime - 1, 1);
                pixel.Opacity.Add(endTime, 0);

                pixel.Coloring.Add(startTime, color);

                // Apply adjusted x coordinate
                pixel.PositionX.Add(startTime, adjustedX);
                pixel.PositionY.Add(startTime, adjustedY);

                //  pixel.PositionZ.Add(startTime, -40);
                pixel.PositionZ.Add(startTime, z, EasingFunctions.CubicOut);
                pixel.SpriteScale.Add(startTime, new Vector2(1, 1)); // Size of the pixel
                pixel.SpriteRotation.Add(startTime, 0);
                pixel.SpriteRotation.Add(endTime, (float)Random(-Math.PI * 12, Math.PI * 12));

                // Add the pixel to the scene
                pixelNode.Add(pixel);
            }

            pixelNode.PositionZ.Add(startTime, -scrollSpeed - 100);
            pixelNode.PositionZ.Add(83672 - 1, 25, EasingFunctions.CubicOut);
            pixelNode.PositionZ.Add(endTime, -scrollSpeed, EasingFunctions.SineIn);
            // pixelNode.Rotation.Add(startTime + 10, new Quaternion(-1, 0, 0));
            pixelNode.Rotation.Add(83672, new Quaternion(0, 0, 0), EasingFunctions.CubicOut);
            pixelNode.Rotation.Add(endTime, new Quaternion((float)(Math.PI), 0, 0), EasingFunctions.SineIn);


            scene.Add(pixelNode);

            // Generate the scene
            scene.Generate(camera, GetLayer("3d"), startTime, endTime, Beatmap, 4);


            var layer = GetLayer("transition");

            Vector2 size = new Vector2(900, 35);

            Color4 wC = new Color4(255, 255, 255, 255);
            Color4 oC = new Color4(236, 99, 74, 255);
            Color4 yC = new Color4(237, 199, 112, 255);
            Color4 cC = new Color4(94, 196, 192, 255);

            var endY = size.Y * 2;

            for (int i = 0; i < 12; i++)
            {

                var start = new Vector2(OsuHitObject.WidescreenStoryboardBounds.Left - 10, -size.Y);

                start.Y += endY * i;

                OsbOrigin origin = OsbOrigin.CentreLeft;

                if (i % 2 == 0)
                {
                    origin = OsbOrigin.CentreRight;
                    start.X = OsuHitObject.WidescreenStoryboardBounds.Right + 10;
                    start.Y -= size.Y;
                }

                var stripe = layer.CreateSprite("sb/white.png", origin, start);
                if (i % 2 == 0)
                {
                    stripe.ScaleVec(OsbEasing.OutCubic, 91027, 91027 + 250, new Vector2(0, size.Y), size);
                    stripe.Fade(91027, .6f);
                    stripe.Color(91027, cC);
                }
                else
                {
                    stripe.ScaleVec(OsbEasing.OutCubic, 91221, 91221 + 250, new Vector2(0, size.Y), size);
                    stripe.Fade(91027, .6f);
                    stripe.Color(91027, yC);
                }

                stripe.Rotate(91027, -0.2f);

                stripe.ScaleVec(OsbEasing.OutExpo, 92866, 93156, size, new Vector2(0, size.Y));


                //stripe.MoveY(OsbEasing.OutCubic, 176188, 176575, start.Y, start.Y - endY * i);

                double localStart = 91414;
                double duration = Beatmap.GetTimingPointAt(91414).BeatDuration / 2;
                while (localStart < 93156)
                {
                    stripe.MoveY(OsbEasing.None, localStart, localStart + duration, start.Y, start.Y - endY / 2);
                    localStart += duration;
                    stripe.MoveY(OsbEasing.None, localStart, localStart + duration, start.Y - endY / 2, start.Y - endY);
                    localStart += duration;

                    duration *= 0.8;
                }

                stripe.Fade(93156, 0f);
            }
        }
    }
}
