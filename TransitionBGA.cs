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
    public class TransitionBGA : StoryboardObjectGenerator
    {
        public override void Generate()
        {
            var startTime = 169995;
            var endTime = 178124;

            Scene3d scene = new();
            var camera = new PerspectiveCamera();

            // Camera setup
            camera.VerticalFov.Add(startTime, 65);
            camera.HorizontalFov.Add(startTime, 65);
            camera.FarFade.Add(startTime, 2500);
            camera.FarClip.Add(startTime, 2500);
            camera.TargetPosition.Add(startTime, new Vector3(0, 0, 0));
            camera.PositionX.Add(startTime, 0);
            camera.PositionZ.Add(startTime, -10);
            camera.PositionY.Add(startTime, 0);

            // Randomized pixel generation
            Random random = new Random();
            int pixelCount = 250; // Number of pixels to generate

            var scrollSpeed = 1200;

            // Create a list to store pixel data
            var pixels = new List<(float x, float y, float z, float rotation, Color4 color)>();

            for (int i = 0; i < pixelCount; i++)
            {
                // Randomize position, rotation, and depth
                float x = Random(-15, 15); // Horizontal position
                float y = Random(-100, scrollSpeed); // Vertical position
                float z = Random(20, 1000); // Depth for parallax effect
                float rotation = Random(0, 360); // Rotation in degrees

                // Randomize color blend
                Color4 color = new Color4(
                    (byte)Random(50, 255), // Red
                    (byte)Random(50, 255), // Green
                    (byte)Random(50, 255), // Blue
                    255 // Alpha
                );

                // Store the pixel data
                pixels.Add((x, y, z, rotation, color));
            }

            // Sort pixels by Y value in descending order to start from the topmost pixel
            pixels.Sort((a, b) => b.z.CompareTo(a.z));

            Node3d pixelNode = new();
            int currentIndex = 0; // Start with the topmost pixel

            int connectionDelay = 1000; // Initial delay for the first connection
            int connectionAcceleration = 50; // Acceleration factor for faster connections over time

            while (currentIndex != -1)
            {
                var (x1, y1, z1, _, color1) = pixels[currentIndex];

                // Find the closest pixel below the current one in Y
                int closestIndex = -1;
                float closestDistance = float.MaxValue;

                for (int i = 0; i < pixels.Count; i++)
                {
                    if (i == currentIndex) continue; // Skip self

                    var (x2, y2, z2, _, color2) = pixels[i];

                    // Ensure the connection is downward (y2 < y1)
                    if (y2 >= y1) continue;

                    // Skip pairs where the X distance is too large
                    float xDistance = Math.Abs(x1 - x2);
                    if (xDistance > 30) continue; // Skip if X distance exceeds 30 units

                    // Calculate the distance between the two pixels
                    float distance = Math.Abs(y2 - y1) + Math.Abs(z2 - z1);

                    // Find the closest pixel below
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestIndex = i;
                    }
                }

                // If a valid connection is found
                if (closestIndex != -1)
                {
                    var (x2, y2, z2, _, color2) = pixels[closestIndex];

                    // Adjust x dynamically based on z depth with reduced horizontal scaling
                    float adjustedX1 = x1 * (1 + z1 / 25); // Clamp x1 to [-50, 50]
                    float adjustedX2 = x2 * (1 + z2 / 25); // Clamp x2 to [-50, 50]

                    // Create a 3D line between the two pixels
                    var line = new Line3d
                    {
                        SpritePath = "sb/white.png",
                        UseDistanceFade = false
                    };

                    // Animate the line's connection over time
                    int connectionStartTime = startTime + connectionDelay;
                    int connectionEndTime = connectionStartTime + 1000; // Duration for the connection animation

                    line.StartPosition.Add(connectionStartTime, new Vector3(adjustedX1, y1, z1));
                    line.EndPosition.Add(connectionStartTime, new Vector3(adjustedX1, y1, z1)); // Start at the same position
                    line.EndPosition.Add(connectionEndTime, new Vector3(adjustedX2, y2, z2), EasingFunctions.CubicOut); // Gradually move to the end position

                    line.Opacity.Add(connectionStartTime, 1);
                    line.Opacity.Add(176188 - 1, 1);
                    line.Opacity.Add(176575, 0);

                    line.Thickness.Add(connectionStartTime, 5f); // Initial thickness

                    // Add the line to the scene
                    pixelNode.Add(line);

                    // Move to the next pixel in the chain
                    currentIndex = closestIndex;

                    // Increase the delay for the next connection and accelerate the speed
                    connectionDelay += connectionAcceleration;
                    connectionAcceleration = Math.Max(10, connectionAcceleration - 5); // Gradually reduce acceleration to speed up
                }
                else
                {
                    // No more pixels below, end the chain
                    currentIndex = -1;
                }
            }

            // Add pixels to the scene based on their Z depth
            foreach (var (x, y, z, rotation, color) in pixels)
            {
                // Adjust x dynamically based on z to keep objects in the camera's view
                float adjustedX = x * (1 + z / 25); // Scale x based on z depth

                // Create a pixel node
                Sprite3d pixel = new Sprite3d
                {
                    SpritePath = "sb/white.png",
                    UseDistanceFade = false
                };

                pixel.Opacity.Add(startTime, 1);
                pixel.Opacity.Add(176188 - 1, 1);
                pixel.Opacity.Add(176575, 0);

                pixel.Coloring.Add(startTime, color);

                // Apply adjusted x coordinate
                pixel.PositionX.Add(startTime, adjustedX);
                pixel.PositionY.Add(startTime, -100);
                pixel.PositionY.Add(171156, y, EasingFunctions.CubicOut);

                pixel.PositionZ.Add(startTime, z);
                pixel.SpriteScale.Add(startTime, new Vector2(10, 10)); // Size of the pixel
                pixel.SpriteRotation.Add(startTime, 0);
                pixel.SpriteRotation.Add(178124, (float)Random(-Math.PI * 4, Math.PI * 4));

                // Add the pixel to the scene
                pixelNode.Add(pixel);
            }

            pixelNode.PositionY.Add(startTime, 0);
            pixelNode.PositionY.Add(171156, 0);
            pixelNode.PositionY.Add(176575, -scrollSpeed, EasingFunctions.SineIn);

            // Set up rotation around Y axis to create cylindrical effect
            pixelNode.Rotation.Add(startTime, Quaternion.FromAxisAngle(Vector3.UnitY, 0));
            pixelNode.Rotation.Add(171156, Quaternion.FromAxisAngle(Vector3.UnitY, 0));
            pixelNode.Rotation.Add(176575, Quaternion.FromAxisAngle(Vector3.UnitY, MathHelper.DegreesToRadians(360)), EasingFunctions.SineIn);


            scene.Add(pixelNode);

            // Generate the scene
            scene.Generate(camera, GetLayer("3d"), startTime, endTime, Beatmap, 12);


        }
    }
}
