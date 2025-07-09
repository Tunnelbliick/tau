using OpenTK;
using OpenTK.Graphics;
using storyboard.scriptslibrary;
using StorybrewCommon.Animations;
using StorybrewCommon.Mapset;
using StorybrewCommon.Scripting;
using StorybrewCommon.Storyboarding;
using StorybrewCommon.Storyboarding.Commands;
using StorybrewCommon.Storyboarding.Util;
using StorybrewCommon.Storyboarding3d;
using StorybrewCommon.Subtitles;
using StorybrewCommon.Util;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace StorybrewScripts
{

    public class IntroBGA : StoryboardObjectGenerator
    {
        FontGenerator font;
        public override void Generate()
        {
            font = LoadFont("sb/f", new FontDescription()
            {
                FontPath = "assetlibrary/mono.ttf",
                FontSize = 50,
                Color = Color4.White,
                Padding = new Vector2(0, 0),
                FontStyle = FontStyle.Regular,
                TrimTransparency = false,
                EffectsOnly = false,
            },
            new RGBShift()
            {
                RED_SHIFT = new Vector2(-3, -1),
                BLUE_SHIFT = new Vector2(3, 1),
            }
            );

            var layer = GetLayer("BG");

            Color4 wC = new Color4(255, 255, 255, 255);
            Color4 oC = new Color4(236, 99, 74, 255);
            Color4 yC = new Color4(237, 199, 112, 255);
            Color4 cC = new Color4(94, 196, 192, 255);

            Color4[] colors = new Color4[] { wC, oC, yC, cC };

            var insertBackgroundR = layer.CreateSprite("sb/white.png", OsbOrigin.Centre);
            insertBackgroundR.ScaleVec(0, 200, 30);
            insertBackgroundR.Fade(0, 1);
            insertBackgroundR.ScaleVec(1511 - 100, 1511, 200, 30, 200, 0);
            insertBackgroundR.Rotate(0, 1511, 0, 0.02f); // Slight rotation for red
            insertBackgroundR.Fade(1511, 0);

            var insertBackgroundG = layer.CreateSprite("sb/white.png", OsbOrigin.Centre);
            insertBackgroundG.ScaleVec(0, 200, 30);
            insertBackgroundG.Fade(0, 1);
            insertBackgroundG.ScaleVec(1511 - 100, 1511, 200, 30, 200, 0);
            insertBackgroundG.Fade(1511, 0);

            var insertBackgroundB = layer.CreateSprite("sb/white.png", OsbOrigin.Centre);
            insertBackgroundB.ScaleVec(0, 200, 30);
            insertBackgroundB.Fade(0, 1);
            insertBackgroundB.ScaleVec(1511 - 100, 1511, 200, 30, 200, 0);
            insertBackgroundR.Rotate(0, 1511, 0, -0.02f); // Slight rotation for red
            insertBackgroundB.Fade(1511, 0);

            insertBackgroundR.Color(0, new Color4(255, 0, 0, 0));
            insertBackgroundG.Color(0, new Color4(0, 255, 0, 0));
            insertBackgroundB.Color(0, new Color4(0, 0, 255, 0));

            insertBackgroundR.Additive(0);
            insertBackgroundG.Additive(0);
            insertBackgroundB.Additive(0);

            var insert = layer.CreateSprite("sb/insert.png", OsbOrigin.Centre, new Vector2(310, 241));
            insert.Fade(0, 1);
            insert.ScaleVec(0, 0.2f, 0.2f);
            insert.ScaleVec(1511 - 100, 1511, 0.2f, 0.2f, 0.2f, 0);
            insert.Fade(1511, 0);

            var dot = layer.CreateSprite("sb/white.png", OsbOrigin.Centre, new Vector2(394, 246));
            dot.Fade(0, 1);
            dot.Scale(0, 2);
            dot.Color(0, new Color4(0, 0, 0, 0));
            dot.Fade(1511, 0);

            var dot1 = layer.CreateSprite("sb/white.png", OsbOrigin.Centre, new Vector2(394 + 8, 246));
            dot1.Fade(446, 1);
            dot1.Scale(446, 2);
            dot1.Color(446, new Color4(0, 0, 0, 0));
            dot1.Fade(1221, 0);

            var dot2 = layer.CreateSprite("sb/white.png", OsbOrigin.Centre, new Vector2(394 + 16, 246));
            dot2.Fade(834, 1);
            dot2.Scale(834, 2);
            dot2.Color(834, new Color4(0, 0, 0, 0));
            dot2.Fade(1221, 0);

            var atlasR = layer.CreateSprite("sb/atlas/atlas.png", OsbOrigin.TopCentre, new Vector2(355, 200));
            atlasR.Fade(1511 - 100, 1);
            atlasR.ScaleVec(1511 - 100, 1511, 0.3f, 0, 0.3f, 0.3f);
            atlasR.Fade(4704, 0);
            atlasR.Rotate(1511 - 100, 0.01f); // Slight rotation for blue
            atlasR.Color(1511 - 100, new Color4(255, 0, 0, 0));
            atlasR.Additive(1511 - 100);

            var atlasG = layer.CreateSprite("sb/atlas/atlas.png", OsbOrigin.TopCentre, new Vector2(355, 200));
            atlasG.Fade(1511 - 100, 1);
            atlasG.ScaleVec(1511 - 100, 1511, 0.3f, 0, 0.3f, 0.3f);
            atlasG.Fade(4704, 0);
            atlasG.Color(1511 - 100, new Color4(0, 255, 0, 0));
            atlasG.Additive(1511 - 100);

            var atlasB = layer.CreateSprite("sb/atlas/atlas.png", OsbOrigin.TopCentre, new Vector2(355, 200));
            atlasB.Fade(1511 - 100, 1);
            atlasB.ScaleVec(1511 - 100, 1511, 0.3f, 0, 0.3f, 0.3f);
            atlasB.Fade(4704, 0);
            atlasB.Color(1511 - 100, new Color4(0, 0, 255, 0));
            atlasB.Rotate(1511 - 100, -0.01f); // Slight rotation for blue
            atlasB.Additive(1511 - 100);

            var logoR = layer.CreateAnimation("sb/ani/sphere/frame.jpg", 24, 500f / 24, OsbLoopType.LoopForever, OsbOrigin.TopCentre, new Vector2(145, 184));
            logoR.Fade(1511 - 100, 1);
            logoR.ScaleVec(1511 - 100, 1511, 0.075f, 0, 0.075f, 0.075f);
            logoR.Fade(4704, 0);
            logoR.Rotate(1511 - 100, 0.10f);
            logoR.Color(1511 - 100, new Color4(255, 0, 0, 0));
            logoR.Additive(1511 - 100);

            var logoG = layer.CreateAnimation("sb/ani/sphere/frame.jpg", 24, 500f / 24, OsbLoopType.LoopForever, OsbOrigin.TopCentre, new Vector2(145, 184));
            logoG.Fade(1511 - 100, 1);
            logoG.ScaleVec(1511 - 100, 1511, 0.075f, 0, 0.075f, 0.075f);
            logoG.Fade(4704, 0);
            logoG.Rotate(1511 - 100, 0.15f);
            logoG.Color(1511 - 100, new Color4(0, 255, 0, 0));
            logoG.Additive(1511 - 100);

            var logoB = layer.CreateAnimation("sb/ani/sphere/frame.jpg", 24, 500f / 24, OsbLoopType.LoopForever, OsbOrigin.TopCentre, new Vector2(145, 184));
            logoB.Fade(1511 - 100, 1);
            logoB.ScaleVec(1511 - 100, 1511, 0.075f, 0, 0.075f, 0.075f);
            logoB.Fade(4704, 0);
            logoB.Rotate(1511 - 100, 0.20f); // Slight rotation for blue
            logoB.Color(1511 - 100, new Color4(0, 0, 255, 0));
            logoB.Additive(1511 - 100);

            var loadingBackgroundR = layer.CreateSprite("sb/white.png", OsbOrigin.TopCentre, new Vector2(320, 400));
            loadingBackgroundR.Fade(1511 - 100, 1);
            loadingBackgroundR.ScaleVec(1511 - 100, 1511, 550f, 0, 550f, 20);
            loadingBackgroundR.Fade(4704, 0);
            loadingBackgroundR.Color(1511 - 100, new Color4(255, 0, 0, 0));
            loadingBackgroundR.Additive(1511 - 100);
            loadingBackgroundR.Rotate(1511 - 100, 0.0025f);

            var loadingBackgroundG = layer.CreateSprite("sb/white.png", OsbOrigin.TopCentre, new Vector2(320, 400));
            loadingBackgroundG.Fade(1511 - 100, 1);
            loadingBackgroundG.ScaleVec(1511 - 100, 1511, 550f, 0, 550f, 20);
            loadingBackgroundG.Fade(4704, 0);
            loadingBackgroundG.Color(1511 - 100, new Color4(0, 255, 0, 0));
            loadingBackgroundG.Additive(1511 - 100);

            var loadingBackgroundB = layer.CreateSprite("sb/white.png", OsbOrigin.TopCentre, new Vector2(320, 400));
            loadingBackgroundB.Fade(1511 - 100, 1);
            loadingBackgroundB.ScaleVec(1511 - 100, 1511, 550f, 0, 550f, 20);
            loadingBackgroundB.Fade(4704, 0);
            loadingBackgroundB.Color(1511 - 100, new Color4(0, 0, 255, 0));
            loadingBackgroundB.Rotate(1511 - 100, -0.0025f); // Slight rotation for blue
            loadingBackgroundB.Additive(1511 - 100);

            var loadingInnerR = layer.CreateSprite("sb/white.png", OsbOrigin.TopCentre, new Vector2(320, 402));
            loadingInnerR.Fade(1511 - 100, 1);
            loadingInnerR.ScaleVec(1511 - 100, 1511, 550f, 0, 546f, 16);
            loadingInnerR.Color(1511 - 100, new Color4(0, 0, 0, 0));
            loadingInnerR.Fade(4704, 0);

            double start = 4704;
            double end = 17092;
            double interval = Beatmap.GetTimingPointAt((int)start).BeatDuration;
            double animDuration = interval * 8; // 8 beats per cycle
            var t = 0;

            var code = layer.CreateSprite("sb/atlas/code.png", OsbOrigin.Centre, new Vector2(150, 915));
            code.Scale(4704, 0.5f);
            code.Fade(4704, 5092, 0, 0.5f);
            code.Fade(15350, 16317, 0.5f, 0);
            code.Additive(4704);

            var blackBlock = layer.CreateSprite("sb/white.png", OsbOrigin.Centre, new Vector2(320, 470));
            blackBlock.ScaleVec(0, 854, 100);
            blackBlock.Color(0, new Color4(0, 0, 0, 0));
            blackBlock.Fade(0, 1);
            blackBlock.Fade(16317, 0);

            var moveStart = 4704;
            var movementDistace = 8.9f;
            var moveEnd = end;
            var currentY = 900f;

            while (moveStart < moveEnd)
            {
                code.MoveY(moveStart, currentY);
                moveStart += 125;
                currentY -= movementDistace;
            }


            var loadingR = layer.CreateSprite("sb/white.png", OsbOrigin.TopLeft, new Vector2(320 - 542 / 2, 404));
            loadingR.Fade(1511 - 100, 1);

            var atlasRBig = layer.CreateAnimation("sb/ani/sphere/frame.jpg", 24, 800 / 24, OsbLoopType.LoopForever, OsbOrigin.Centre, new Vector2(320 - 2, 180));
            atlasRBig.Scale(start - 10 / 24, 0.2f);
            atlasRBig.Color(start - 10 / 24, new Color4(255, 0, 0, 255));
            atlasRBig.Additive(start - 10 / 24);
            atlasRBig.Rotate(start, 0.1f);
            atlasRBig.Fade(start - 10, start - 10, 0, 0.2f);
            atlasRBig.Fade(OsbEasing.OutCubic, 16317, 17092, 0.2f, 0f);
            atlasRBig.StartLoopGroup(start, (int)(end - start) / (int)animDuration);
            atlasRBig.MoveX(OsbEasing.InOutSine, t, t + animDuration / 2, 320 - 2, 320 - 4);
            atlasRBig.MoveX(OsbEasing.InOutSine, t + animDuration / 2, t + animDuration, 320 - 4, 320 - 2);
            atlasRBig.MoveY(OsbEasing.InOutSine, t, t + animDuration / 2, 180, 179);
            atlasRBig.MoveY(OsbEasing.InOutSine, t + animDuration / 2, t + animDuration, 179, 180);
            atlasRBig.EndGroup();


            var atlasGBig = layer.CreateAnimation("sb/ani/sphere/frame.jpg", 24, 800 / 24, OsbLoopType.LoopForever, OsbOrigin.Centre, new Vector2(320, 180));
            atlasGBig.Scale(start, 0.2f);
            atlasGBig.Rotate(start, 0.1f);
            atlasGBig.Color(start, new Color4(0, 255, 0, 255));
            atlasGBig.Additive(start);
            atlasGBig.Fade(start, start, 0, 0.2f);
            atlasGBig.Fade(OsbEasing.OutCubic, 16317, 17092, 0.2f, 0f);

            var atlasBigB = layer.CreateAnimation("sb/ani/sphere/frame.jpg", 24, 800 / 24, OsbLoopType.LoopForever, OsbOrigin.Centre, new Vector2(320 + 2, 180));
            atlasBigB.Scale(start + 10 / 24, 0.2f);
            atlasBigB.Rotate(start + 10 / 24, 0.1f);
            atlasBigB.Color(start + 10 / 24, new Color4(0, 0, 255, 255));
            atlasBigB.Additive(start + 10 / 24);
            atlasBigB.Fade(start + 10, start + 10, 0, 0.2f);
            atlasBigB.Fade(OsbEasing.OutCubic, 16317, 17092, 0.2f, 0f);
            atlasBigB.StartLoopGroup(start, (int)(end - start) / (int)animDuration);
            atlasBigB.MoveX(OsbEasing.InOutSine, t, t + animDuration / 2, 320 + 2, 320 + 4);
            atlasBigB.MoveX(OsbEasing.InOutSine, t + animDuration / 2, t + animDuration, 320 + 4, 320 + 2);
            atlasBigB.MoveY(OsbEasing.InOutSine, t, t + animDuration / 2, 180, 181);
            atlasBigB.MoveY(OsbEasing.InOutSine, t + animDuration / 2, t + animDuration, 181, 180);
            atlasBigB.EndGroup();


            var atlasTextR = layer.CreateSprite("sb/atlas/atlas.png", OsbOrigin.Centre, new Vector2(320 - 1, 300));
            atlasTextR.Scale(start, 0.35f);
            atlasTextR.Rotate(start, 0.01f);
            atlasTextR.Color(start, new Color4(255, 0, 0, 255));
            atlasTextR.Additive(start);
            atlasTextR.Fade(4704, 5092, 0, 0.2f);
            atlasTextR.Fade(OsbEasing.OutCubic, 16317, 17092, 0.2f, 0f);

            atlasTextR.StartLoopGroup(start, (int)(end - start) / (int)animDuration);
            atlasTextR.MoveX(OsbEasing.InOutSine, t, t + animDuration / 2, 320 - 1, 320 - 2.5f);
            atlasTextR.MoveX(OsbEasing.InOutSine, t + animDuration / 2, t + animDuration, 320 - 2.5f, 320 - 1);
            atlasTextR.EndGroup();

            var atlasTextG = layer.CreateSprite("sb/atlas/atlas.png", OsbOrigin.Centre, new Vector2(320, 300));
            atlasTextG.Scale(start, 0.35f);
            atlasTextG.Color(start, new Color4(0, 255, 0, 255));
            atlasTextG.Additive(start);
            atlasTextG.Fade(4704, 5092, 0, 0.2f);
            atlasTextG.Fade(OsbEasing.OutCubic, 16317, 17092, 0.2f, 0f);

            var atlasTextB = layer.CreateSprite("sb/atlas/atlas.png", OsbOrigin.Centre, new Vector2(320 + 0.5f, 300));
            atlasTextB.Scale(start, 0.35f);
            atlasTextB.Rotate(start, -0.01f);
            atlasTextB.Color(start, new Color4(0, 0, 255, 255));
            atlasTextB.Additive(start);
            atlasTextB.Fade(4704, 5092, 0, 0.2f);
            atlasTextB.Fade(OsbEasing.OutCubic, 16317, 17092, 0.2f, 0f);

            atlasTextB.StartLoopGroup(start, (int)(end - start) / (int)animDuration);
            atlasTextB.MoveX(OsbEasing.InOutSine, t, t + animDuration / 2, 320 + 1, 320 + 2.5f);
            atlasTextB.MoveX(OsbEasing.InOutSine, t + animDuration / 2, t + animDuration, 320 + 2.5f, 320 + 1);
            atlasTextB.EndGroup();


            var startTime = 4704; // The start time where the playfield is initialized
            var endTime = 17092; // The end time where the playfield is no

            Scene3d scene = new();
            var camera = new PerspectiveCamera();

            Node3d objects = new Node3d();
            Node3d objectsWrapper = new Node3d();

            // Camera setup
            camera.VerticalFov.Add(startTime, 65);
            camera.HorizontalFov.Add(startTime, 65);
            camera.FarFade.Add(startTime, 0);
            camera.FarClip.Add(startTime, 300);
            camera.NearClip.Add(startTime, 10f);
            camera.NearFade.Add(startTime, 30);
            camera.TargetPosition.Add(startTime, new Vector3(0, 0, 0));
            camera.PositionX.Add(startTime, 0);
            camera.PositionZ.Add(startTime, -100);
            camera.PositionY.Add(startTime, 0);

            var numberObjects = 150;
            var radius = 100f;

            for (int i = 0; i < numberObjects; i++)
            {

                Sprite3d particle = new Sprite3d
                {
                    SpritePath = "sb/white.png"
                };


                var angle = (i / (float)numberObjects) * MathHelper.TwoPi;
                var xPos = (float)Random(-Math.Cos(angle) * radius, Math.Cos(angle) * radius);
                var yPos = Random(-50, 50);
                var zPos = (float)(Math.Sin(angle) * radius);

                particle.Coloring.Add(startTime, colors[i % colors.Length]);

                particle.PositionX.Add(startTime, xPos);
                particle.PositionY.Add(startTime, yPos);
                particle.PositionZ.Add(startTime, zPos);


                objects.Add(particle);
            }

            objects.Opacity.Add(startTime, 0);
            objects.Opacity.Add(startTime + 1000, 0.75f, EasingFunctions.CubicOut);
            objects.Opacity.Add(endTime - 1000, 0.75f);
            objects.Opacity.Add(endTime, 0, EasingFunctions.CubicOut);

            //objects.PositionZ.Add(startTime, 100);

            var rotations = 5;
            var rotationStart = startTime;
            var rotationSpeed = (endTime - startTime) / rotations;

            for (int i = 0; i < rotations; i++)
            {
                objects.Rotation.Add(rotationStart, new Quaternion(0, 0, 0));
                objects.Rotation.Add(rotationStart + rotationSpeed, new Quaternion(0, (float)Math.PI, 0));
                objects.Rotation.Add(rotationStart + rotationSpeed + rotationSpeed, new Quaternion(0, (float)Math.PI * 2, 0));
                rotationStart += rotationSpeed + rotationSpeed;
            }

            objectsWrapper.Add(objects);

            objectsWrapper.PositionZ.Add(startTime, 100);

            objectsWrapper.Rotation.Add(5479, new Quaternion(0f, 0f, 0f));
            objectsWrapper.Rotation.Add(7027, new Quaternion(0f, -(float)Math.PI, 0f), EasingFunctions.SineOut);

            objectsWrapper.Rotation.Add(11672, new Quaternion(0f, -(float)Math.PI, 0f));
            objectsWrapper.Rotation.Add(13221, new Quaternion(0f, -(float)Math.PI * 2, 0f), EasingFunctions.SineOut);


            scene.Add(objectsWrapper);

            scene.Generate(camera, layer, startTime, endTime, Beatmap, 4);



            var glitchR = layer.CreateAnimation("sb/atlas/noise/noise.png", 4, 250 / 4, OsbLoopType.LoopForever, OsbOrigin.Centre, new Vector2(315, 240));
            glitchR.Fade(5479, 5479 + 100, 0, 0.3f);
            glitchR.Additive(5479);
            glitchR.Fade(6640 - 100, 6640, 0.3f, 0);
            glitchR.Fade(11672, 11672 + 100, 0, 0.3f);
            glitchR.Fade(12834 - 100, 12834, 0.3f, 0);
            glitchR.Color(5479, new Color4(255, 0, 0, 255));

            var glitchG = layer.CreateAnimation("sb/atlas/noise/noise.png", 4, 250 / 4, OsbLoopType.LoopForever, OsbOrigin.Centre, new Vector2(320, 240));
            glitchG.Fade(5479, 5479 + 100, 0, 0.3f);
            glitchG.Additive(5479);
            glitchG.Fade(6640 - 100, 6640, 0.3f, 0);
            glitchG.Fade(11672, 11672 + 100, 0, 0.3f);
            glitchG.Fade(12834 - 100, 12834, 0.3f, 0);
            glitchG.Color(5479, new Color4(0, 255, 0, 255));

            var glitchB = layer.CreateAnimation("sb/atlas/noise/noise.png", 4, 250 / 4, OsbLoopType.LoopForever, OsbOrigin.Centre, new Vector2(325, 240));
            glitchB.Fade(5479, 5479 + 100, 0, 0.3f);
            glitchB.Additive(5479);
            glitchB.Fade(6640 - 100, 6640, 0.3f, 0);
            glitchB.Fade(11672, 11672 + 100, 0, 0.3f);
            glitchB.Fade(12834 - 100, 12834, 0.3f, 0);
            glitchB.Color(5479, new Color4(0, 0, 255, 255));

            var local = 1511d;
            var currentScale = 0f;
            while (local < 4704)
            {
                interval = Random(10, 400);
                var scaleIncrease = Random(0, 100);

                var nextScale = Math.Min(currentScale + scaleIncrease, 542f);

                loadingR.ScaleVec(local, local + interval, currentScale, 12, nextScale, 12);

                currentScale = nextScale;
                local += interval;
            }

            loadingR.Fade(4704, 0);

            Writer(1463, 2188, "Compiling Bootloader.java...", new Vector2(48, 395), 0.2f);
            Writer(2188, 2963, "Linking subspace headers...", new Vector2(48, 395), 0.2f);
            Writer(2963, 3930, "Injecting synchronization duct tape...", new Vector2(48, 395), 0.2f);
            Writer(3930, 4704, "Build succeeded with 327 warnings", new Vector2(48, 395), 0.2f);


            Vector2 size = new Vector2(900, 35);

            Color4 w = new Color4(255, 255, 255, 255);
            Color4 o = new Color4(236, 99, 74, 255);
            Color4 y = new Color4(237, 199, 112, 255);
            Color4 c = new Color4(94, 196, 192, 255);

            var endY = size.Y * 2;


            for (int i = 0; i < 12; i++)
            {

                var startPos = new Vector2(OsuHitObject.WidescreenStoryboardBounds.Left - 10, -size.Y);

                startPos.Y += endY * i;

                var stripe = layer.CreateSprite("sb/white.png", OsbOrigin.CentreLeft, startPos);
                if (i % 3 == 0)
                {
                    stripe.ScaleVec(OsbEasing.OutCubic, 15350, 15640, new Vector2(0, size.Y), size);
                    stripe.Fade(15350, .6f);
                    stripe.Color(15350, o);
                }
                else if (i % 2 == 0)
                {
                    stripe.ScaleVec(OsbEasing.OutCubic, 15640, 15930, new Vector2(0, size.Y), size);
                    stripe.Fade(15640, .6f);
                    stripe.Color(15640, c);
                }
                else
                {
                    stripe.ScaleVec(OsbEasing.OutCubic, 15930, 16221, new Vector2(0, size.Y), size);
                    stripe.Fade(15930, .6f);
                    stripe.Color(15930, w);
                }

                stripe.Rotate(16221, 18640, -0.2f, 0.2f);

                stripe.ScaleVec(OsbEasing.OutExpo, 17866, 18640, size, new Vector2(size.X, 0));

                double localStart = 15350;
                double duration = 500;
                while (localStart < 18640)
                {
                    stripe.MoveY(OsbEasing.None, localStart, localStart + duration, startPos.Y, startPos.Y - size.Y * 6);

                    localStart += duration;
                    duration *= 0.85;
                }

                stripe.Fade(18640, 0f);
            }

        }

        public void Writer(double startTime, double fadeOutTime, string text, Vector2 pos, float scale = 0.5f)
        {

            var spacing = 0;
            scale = scale * 0.4f;

            //pos.X -= text.Length / 2 * (font.GetTexture("W").BaseWidth * scale);
            pos.X += spacing;

            foreach (char c in text.ToUpper())
            {
                if (c == ' ')
                {
                    pos.X += font.GetTexture("W").BaseWidth * scale + spacing; // Add spacing for space character
                    continue;
                }
                if (c == '\n')
                {
                    pos.Y += 12;
                    continue;
                }
                var f = GetLayer("BG").CreateSprite(font.GetTexture(c.ToString()).Path, OsbOrigin.Centre, pos);
                f.Fade(startTime, 1);
                f.Scale(startTime, scale);
                f.Color(startTime, Color4.White);
                startTime += 20; // Adjust the time for the next character
                pos.X += font.GetTexture(c.ToString()).BaseWidth * scale; // Move the x position for the next character
                f.Fade(fadeOutTime, 0);
            }
        }

    }
}
