using OpenTK;
using OpenTK.Graphics;
using StorybrewCommon.Animations;
using StorybrewCommon.Mapset;
using StorybrewCommon.Scripting;
using StorybrewCommon.Storyboarding;
using StorybrewCommon.Storyboarding.Util;
using StorybrewCommon.Storyboarding3d;
using StorybrewCommon.Subtitles;
using StorybrewCommon.Util;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StorybrewScripts
{
    public class MatrixBG : StoryboardObjectGenerator
    {
        public override void Generate()
        {
            var layer = GetLayer("3d");
            var startTime = 17092;
            var endTime = 50382;

            Scene3d scene = new();
            var camera = new PerspectiveCamera();

            // Camera setup
            camera.VerticalFov.Add(startTime, 65);
            camera.HorizontalFov.Add(startTime, 65);
            camera.FarFade.Add(18640, 500, EasingFunctions.CubicOut);
            camera.FarClip.Add(18640, 550, EasingFunctions.CubicOut);
            camera.NearClip.Add(startTime, 10f);
            camera.NearFade.Add(startTime, 30);
            camera.TargetPosition.Add(startTime, new Vector3(0, 0, 0));
            camera.PositionX.Add(startTime, 0);
            camera.PositionZ.Add(startTime, -100);
            camera.PositionY.Add(startTime, 0);

            var numberNode = new Node3d();
            var numbes = 350;


            Color4 w = new Color4(255, 255, 255, 255);
            Color4 o = new Color4(236, 99, 74, 255);
            Color4 y = new Color4(237, 199, 112, 255);
            Color4 c = new Color4(94, 196, 192, 255);

            Color4[] colors = new Color4[] { w, o, y, c };

            for (int i = 0; i < numbes; i++)
            {
                var localStart = startTime;
                var xPos = Random(-60, 60);
                var yPos = Random(-30, 30);

                // Skip positions within the deadzone (-5 to 5 for both X and Y)
                if (xPos >= -5 && xPos <= 5 && yPos >= -5 && yPos <= 5)
                    continue;

                var numb = new Animation3d
                {
                    SpritePath = "sb/ani/numbers/frame.png",
                    FrameCount = 10,
                    FrameDelay = 150,

                };

                var deepY = Random(10, 300);
                var deepX = Random(-100, 100);

                localStart += Random(-1000, 1000);

                numb.SpriteScale.Add(localStart, new Vector2(0.035f));
                numb.SpriteScale.Add(29479, new Vector2(0.035f));
                numb.SpriteScale.Add(31027, new Vector2(0.1f), EasingFunctions.SineOut);
                numb.RotationMode = RotationMode.Fixed;

                numb.PositionX.Add(localStart, xPos);
                numb.PositionY.Add(localStart, yPos);
                numb.PositionZ.Add(localStart, Random(10, 1400));
                numb.Coloring.Add(localStart, colors[Random(0, colors.Length)]);

                numb.PositionY.Add(29479, yPos);
                numb.PositionY.Add(31027, deepY, EasingFunctions.SineOut);
                numb.PositionX.Add(29479, xPos);
                numb.PositionX.Add(31027, deepX, EasingFunctions.SineOut);

                numb.Opacity.Add(localStart, 0.5f);
                numb.Opacity.Add(endTime - 1, 0.5f);
                numb.Opacity.Add(endTime, 0);

                numberNode.Add(numb);
            }

            numberNode.PositionZ.Add(startTime, 500);
            numberNode.PositionZ.Add(18640, -100, EasingFunctions.CubicOut);
            numberNode.PositionZ.Add(29479, -900, EasingFunctions.SineIn);
            numberNode.PositionZ.Add(31027, 0, EasingFunctions.CircOut);

            numberNode.PositionX.Add(startTime, 0);
            numberNode.PositionX.Add(24834 - 1, 0);
            numberNode.PositionX.Add(24834, -150);
            numberNode.PositionX.Add(29479, 0, EasingFunctions.SineOut);

            numberNode.PositionX.Add(37221 - 1, 0);
            numberNode.PositionX.Add(37221, 150);
            numberNode.PositionX.Add(37221 + 2500, 0, EasingFunctions.SineOut);

            numberNode.Rotation.Add(startTime, new Quaternion(0, 0, 0));
            numberNode.Rotation.Add(24834 - 1, new Quaternion(0, 0, 0));
            numberNode.Rotation.Add(24834, new Quaternion(0.3f, 0.3f, 0));
            numberNode.Rotation.Add(29479, new Quaternion(0, 0, 0), EasingFunctions.SineOut);
            numberNode.Rotation.Add(30446, new Quaternion(0, 0, (float)Math.PI / 4), EasingFunctions.CubicIn);
            numberNode.Rotation.Add(31027, new Quaternion(0, 0, (float)Math.PI / 2), EasingFunctions.CircOut);

            numberNode.Rotation.Add(37221 - 1, new Quaternion(0, 0, (float)Math.PI / 2));
            numberNode.Rotation.Add(37221, new Quaternion(-0.4f, -0.2f, (float)Math.PI / 2));
            numberNode.Rotation.Add(37221 + 2500, new Quaternion(0, 0, (float)Math.PI / 2), EasingFunctions.SineOut);

            numberNode.PositionY.Add(startTime, 0);
            numberNode.PositionY.Add(29479, 0);
            numberNode.PositionY.Add(31027, 50, EasingFunctions.SineIn);
            numberNode.PositionY.Add(endTime, 800);

            var start = 43414;
            var duration = 43801 - start;
            numberNode.Rotation.Add(start, new Quaternion(0, 0, (float)Math.PI / 2));
            numberNode.Rotation.Add(start + duration, new Quaternion(0f, (float)Math.PI / 4, (float)Math.PI / 2), EasingFunctions.SineOut);
            numberNode.PositionX.Add(start, 0);
            numberNode.PositionX.Add(start + duration, -100, EasingFunctions.SineOut);
            numberNode.PositionZ.Add(start, 0);
            numberNode.PositionZ.Add(start + duration, 100, EasingFunctions.SineOut);

            start = 44188;
            numberNode.Rotation.Add(start, new Quaternion(0, (float)Math.PI / 4, (float)Math.PI / 2));
            numberNode.Rotation.Add(start + duration, new Quaternion(0f, (float)Math.PI / 2, (float)Math.PI / 2), EasingFunctions.SineOut);
            numberNode.PositionX.Add(start, -100);
            numberNode.PositionX.Add(start + duration, -150, EasingFunctions.SineOut);
            numberNode.PositionZ.Add(start, 100);
            numberNode.PositionZ.Add(start + duration, 175, EasingFunctions.SineOut);

            start = 44963;
            numberNode.Rotation.Add(start, new Quaternion(0, (float)Math.PI / 2, (float)Math.PI / 2));
            numberNode.Rotation.Add(start + duration, new Quaternion(0f, (float)Math.PI / 2 + (float)Math.PI / 4, (float)Math.PI / 2), EasingFunctions.SineOut);
            numberNode.PositionX.Add(start, -150);
            numberNode.PositionX.Add(start + duration, -100, EasingFunctions.SineOut);
            numberNode.PositionZ.Add(start, 175);
            numberNode.PositionZ.Add(start + duration, 250, EasingFunctions.SineOut);

            start = 45737;
            numberNode.Rotation.Add(start, new Quaternion(0, (float)Math.PI / 2 + (float)Math.PI / 4, (float)Math.PI / 2));
            numberNode.Rotation.Add(start + duration, new Quaternion(0f, (float)Math.PI, (float)Math.PI / 2), EasingFunctions.SineOut);
            numberNode.PositionX.Add(start, -100);
            numberNode.PositionX.Add(start + duration, 0, EasingFunctions.SineOut);
            numberNode.PositionZ.Add(start, 250);
            numberNode.PositionZ.Add(start + duration, 375, EasingFunctions.SineOut);

            start = 46511;
            numberNode.Rotation.Add(start, new Quaternion(0, (float)Math.PI, (float)Math.PI / 2));
            numberNode.Rotation.Add(start + duration, new Quaternion(0f, (float)Math.PI + (float)Math.PI / 4, (float)Math.PI / 2), EasingFunctions.SineOut);
            numberNode.PositionX.Add(start, 0);
            numberNode.PositionX.Add(start + duration, 100, EasingFunctions.SineOut);
            numberNode.PositionZ.Add(start, 375);
            numberNode.PositionZ.Add(start + duration, 250, EasingFunctions.SineOut);

            start = 47285;
            numberNode.Rotation.Add(start, new Quaternion(0, (float)Math.PI + (float)Math.PI / 4, (float)Math.PI / 2));
            numberNode.Rotation.Add(start + duration, new Quaternion(0f, (float)Math.PI + (float)Math.PI / 2, (float)Math.PI / 2), EasingFunctions.SineOut);
            numberNode.PositionX.Add(start, 100);
            numberNode.PositionX.Add(start + duration, 150, EasingFunctions.SineOut);
            numberNode.PositionZ.Add(start, 250);
            numberNode.PositionZ.Add(start + duration, 175, EasingFunctions.SineOut);

            start = 48059;
            numberNode.Rotation.Add(start, new Quaternion(0, (float)Math.PI + (float)Math.PI / 2, (float)Math.PI / 2));
            numberNode.Rotation.Add(start + duration, new Quaternion(0f, (float)Math.PI + (float)Math.PI / 2 + (float)Math.PI / 4, (float)Math.PI / 2), EasingFunctions.SineOut);
            numberNode.PositionX.Add(start, 150);
            numberNode.PositionX.Add(start + duration, 100, EasingFunctions.SineOut);
            numberNode.PositionZ.Add(start, 175);
            numberNode.PositionZ.Add(start + duration, 100, EasingFunctions.SineOut);

            start = 48834;
            numberNode.Rotation.Add(start, new Quaternion(0, (float)Math.PI + (float)Math.PI / 2 + (float)Math.PI / 4, (float)Math.PI / 2));
            numberNode.Rotation.Add(49608, new Quaternion(0f, 0, (float)Math.PI / 2), EasingFunctions.SineIn);
            numberNode.PositionX.Add(start, 100);
            numberNode.PositionX.Add(49608, 0, EasingFunctions.SineIn);
            numberNode.PositionZ.Add(start, 100);
            numberNode.PositionZ.Add(49608, -350, EasingFunctions.SineIn);


            scene.Add(numberNode);
            scene.Generate(camera, layer, startTime, 49608, Beatmap, 8);

            var playfieldBackDrop = layer.CreateSprite("sb/white.png", OsbOrigin.TopCentre, new Vector2(320, -100));
            playfieldBackDrop.ScaleVec(OsbEasing.OutCirc, 29866, 31027, new Vector2(0, 1000), new Vector2(260, 1000));
            playfieldBackDrop.Fade(29866, 0.75);
            playfieldBackDrop.Color(29866, new Color4(0, 0, 0, 0));
            playfieldBackDrop.Rotate(37221, 37221, 0, 0.75);
            playfieldBackDrop.MoveX(37221, 37221, 320, 570);
            playfieldBackDrop.Rotate(OsbEasing.OutSine, 37221, 37221 + 2500, 0.75, 0);
            playfieldBackDrop.MoveX(OsbEasing.OutSine, 37221, 37221 + 2500, 570, 320);
            //playfieldBackDrop.MoveY(OsbEasing.OutSine, 41866, endTime, -100, -580);
            playfieldBackDrop.Fade(OsbEasing.InSine, 48834, endTime, 0.75, 0);

            Vector2 size = new Vector2(900, 35);
            var endY = size.Y * 2;


            for (int i = 0; i < 12; i++)
            {

                var startPos = new Vector2(OsuHitObject.WidescreenStoryboardBounds.Left - 10, -endY);

                startPos.Y += endY * i;

                var stripe = layer.CreateSprite("sb/white.png", OsbOrigin.CentreLeft, startPos);
                if (i % 2 == 0)
                {
                    stripe.ScaleVec(OsbEasing.OutSine, 49608, 51156, new Vector2(size.X, 0), size);
                    stripe.Fade(49608, .6f);
                    stripe.Color(49608, o);
                }
                else
                {
                    stripe.ScaleVec(OsbEasing.OutSine, 49608, 51156, new Vector2(size.X, 0), size);
                    stripe.Fade(49608, .6f);
                    stripe.Color(49608, w);
                }

                stripe.Rotate(49608, 0.2f);

                stripe.ScaleVec(OsbEasing.InSine, 54253, 55414, size, new Vector2(size.X, 0));

                double localStart = 49608;
                double dur = 750;
                while (localStart < 55414)
                {
                    stripe.MoveY(OsbEasing.None, localStart, localStart + dur, startPos.Y, startPos.Y - endY * 2);

                    localStart += dur;
                    dur *= 0.9;
                }

                stripe.Fade(55801, 0f);
            }
        }
    }
}
