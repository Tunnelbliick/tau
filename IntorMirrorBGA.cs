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
    public class IntorMirrorBGA : StoryboardObjectGenerator
    {
        public override void Generate()
        {

            double start = 131285;
            double end = 169995;
            double interval = Beatmap.GetTimingPointAt((int)start).BeatDuration;
            double animDuration = interval * 8; // 8 beats per cycle
            var t = 0;
            var layer = GetLayer("BG");

            var animationStart = 132059;
            var animationEnd = 142124;

            Color4 wC = new Color4(255, 255, 255, 255);
            Color4 oC = new Color4(236, 99, 74, 255);
            Color4 yC = new Color4(237, 199, 112, 255);
            Color4 cC = new Color4(94, 196, 192, 255);

            Color4[] colors = new Color4[] { wC, oC, yC, cC };

            var code = layer.CreateSprite("sb/atlas/code.png", OsbOrigin.Centre, new Vector2(150, 915));
            code.Scale(156446, 0.5f);
            code.Fade(156446, 156834, 0, 0.5f);
            code.Fade(169995, 0);
            code.Additive(156446);

            var blackBlock = layer.CreateSprite("sb/white.png", OsbOrigin.Centre, new Vector2(320, 470));
            blackBlock.ScaleVec(156446, 854, 100);
            blackBlock.Color(156446, new Color4(0, 0, 0, 0));
            blackBlock.Fade(156446, 1);
            blackBlock.Fade(169995, 0);

            var moveStart = 156446;
            var movementDistace = 8.9f;
            var moveEnd = 169995;
            var currentY = 900f;

            while (moveStart < 169995)
            {
                code.MoveY(moveStart, currentY);
                moveStart += 125;
                currentY -= movementDistace;
            }

            var time = 168059;
            while (time < 169995)
            {
                code.MoveY(moveStart, currentY - movementDistace);
                code.MoveY(moveStart + 125, currentY);
                time += 250;
            }

            var atlasRBig = layer.CreateAnimation("sb/ani/sphere/frame.jpg", 24, 800 / 24, OsbLoopType.LoopForever, OsbOrigin.Centre, new Vector2(320 - 2, 180));
            atlasRBig.Scale(OsbEasing.OutCubic, start - 10 / 24, start - 10 / 24 + 500, 0, 0.2f);
            atlasRBig.Color(start - 10 / 24, new Color4(255, 0, 0, 255));
            atlasRBig.Additive(start - 10 / 24);
            atlasRBig.Rotate(start, 0.1f);
            atlasRBig.Fade(start - 10, start - 10, 0, 0.2f);
            atlasRBig.Fade(end, 0f);

            atlasRBig.StartLoopGroup(animationStart, (int)((animationEnd - animationStart) / interval) / 2);
            atlasRBig.Scale(OsbEasing.OutCubic, 0, interval, 0.22f, 0.2f);
            atlasRBig.Scale(OsbEasing.OutCubic, interval, interval * 2, 0.22f, 0.2f);
            atlasRBig.MoveX(OsbEasing.OutCubic, 0, interval, 320 + 15, 320 - 2);
            atlasRBig.MoveX(OsbEasing.OutCubic, interval, interval * 2, 320 - 15, 320 - 2);
            atlasRBig.EndGroup();

            atlasRBig.StartLoopGroup(144446, (int)((animationEnd - animationStart) / interval) / 2);
            atlasRBig.Scale(OsbEasing.OutCubic, 0, interval, 0.22f, 0.2f);
            atlasRBig.Scale(OsbEasing.OutCubic, interval, interval * 2, 0.22f, 0.2f);
            atlasRBig.MoveX(OsbEasing.OutCubic, 0, interval, 320 + 15, 320 - 2);
            atlasRBig.MoveX(OsbEasing.OutCubic, interval, interval * 2, 320 - 15, 320 - 2);
            atlasRBig.EndGroup();


            var atlasGBig = layer.CreateAnimation("sb/ani/sphere/frame.jpg", 24, 800 / 24, OsbLoopType.LoopForever, OsbOrigin.Centre, new Vector2(320, 180));
            atlasGBig.Scale(OsbEasing.OutCubic, start, start + 500, 0f, 0.2f);
            atlasGBig.Rotate(start, 0.1f);
            atlasGBig.Color(start, new Color4(0, 255, 0, 255));
            atlasGBig.Additive(start);
            atlasGBig.Fade(start, start, 0, 0.2f);
            atlasGBig.Fade(end, 0f);

            var atlasBigB = layer.CreateAnimation("sb/ani/sphere/frame.jpg", 24, 800 / 24, OsbLoopType.LoopForever, OsbOrigin.Centre, new Vector2(320 + 2, 180));
            atlasBigB.Scale(OsbEasing.OutCubic, start + 10 / 24, start + 10 / 24 + 500, 0f, 0.2f);
            atlasBigB.Rotate(start + 10 / 24, 0.1f);
            atlasBigB.Color(start + 10 / 24, new Color4(0, 0, 255, 255));
            atlasBigB.Additive(start + 10 / 24);
            atlasBigB.Fade(start + 10, start + 10, 0, 0.2f);
            atlasBigB.Fade(end, 0f);

            atlasBigB.StartLoopGroup(animationStart, (int)((animationEnd - animationStart) / interval) / 2);
            atlasBigB.Scale(OsbEasing.OutCubic, 0, interval, 0.24f, 0.2f);
            atlasBigB.Scale(OsbEasing.OutCubic, interval, interval * 2, 0.24f, 0.2f);
            atlasBigB.MoveX(OsbEasing.OutCubic, 0, interval, 320 + 15, 320 + 2);
            atlasBigB.MoveX(OsbEasing.OutCubic, interval, interval * 2, 320 - 15, 320 + 2);
            atlasBigB.EndGroup();

            atlasBigB.StartLoopGroup(144446, (int)((animationEnd - animationStart) / interval) / 2);
            atlasBigB.Scale(OsbEasing.OutCubic, 0, interval, 0.24f, 0.2f);
            atlasBigB.Scale(OsbEasing.OutCubic, interval, interval * 2, 0.24f, 0.2f);
            atlasBigB.MoveX(OsbEasing.OutCubic, 0, interval, 320 + 15, 320 + 2);
            atlasBigB.MoveX(OsbEasing.OutCubic, interval, interval * 2, 320 - 15, 320 + 2);
            atlasBigB.EndGroup();

            var atlasTextR = layer.CreateSprite("sb/atlas/atlas.png", OsbOrigin.Centre, new Vector2(320 - 1, 300));
            atlasTextR.Scale(OsbEasing.OutCubic, start, start + 500, 0f, 0.35f);
            atlasTextR.Rotate(start, 0.01f);
            atlasTextR.Color(start, new Color4(255, 0, 0, 255));
            atlasTextR.Additive(start);
            atlasTextR.Fade(start - 200, start, 0, 0.2f);
            atlasTextR.Fade(end, 0f);

            atlasTextR.StartLoopGroup(animationStart, (int)((animationEnd - animationStart) / interval) / 2);
            atlasTextR.Scale(OsbEasing.OutCubic, 0, interval, 0.38f, 0.35f);
            atlasTextR.Scale(OsbEasing.OutCubic, interval, interval * 2, 0.38f, 0.35f);
            atlasTextR.MoveX(OsbEasing.OutCubic, 0, interval, 320 + 5, 320 - 1);
            atlasTextR.MoveX(OsbEasing.OutCubic, interval, interval * 2, 320 - 5, 320 - 1);
            atlasTextR.EndGroup();

            atlasTextR.StartLoopGroup(144446, (int)((animationEnd - animationStart) / interval) / 2);
            atlasTextR.Scale(OsbEasing.OutCubic, 0, interval, 0.38f, 0.35f);
            atlasTextR.Scale(OsbEasing.OutCubic, interval, interval * 2, 0.38f, 0.35f);
            atlasTextR.MoveX(OsbEasing.OutCubic, 0, interval, 320 + 5, 320 - 1);
            atlasTextR.MoveX(OsbEasing.OutCubic, interval, interval * 2, 320 - 5, 320 - 1);
            atlasTextR.EndGroup();

            var atlasTextG = layer.CreateSprite("sb/atlas/atlas.png", OsbOrigin.Centre, new Vector2(320, 300));
            atlasTextG.Scale(OsbEasing.OutCubic, start, start + 500, 0f, 0.35f);
            atlasTextG.Color(start, new Color4(0, 255, 0, 255));
            atlasTextG.Additive(start);
            atlasTextG.Fade(start - 200, start, 0, 0.2f);
            atlasTextG.Fade(end, 0f);

            var atlasTextB = layer.CreateSprite("sb/atlas/atlas.png", OsbOrigin.Centre, new Vector2(320 + 0.5f, 300));
            atlasTextB.Scale(OsbEasing.OutCubic, start, start + 500, 0f, 0.35f);
            atlasTextB.Rotate(start, -0.01f);
            atlasTextB.Color(start, new Color4(0, 0, 255, 255));
            atlasTextB.Additive(start);
            atlasTextB.Fade(start - 200, start, 0, 0.2f);
            atlasTextB.Fade(end, 0f);

            atlasTextB.StartLoopGroup(animationStart, (int)((animationEnd - animationStart) / interval) / 2);
            atlasTextB.Scale(OsbEasing.OutCubic, 0, interval, 0.4f, 0.35f);
            atlasTextB.Scale(OsbEasing.OutCubic, interval, interval * 2, 0.4f, 0.35f);
            atlasTextB.MoveX(OsbEasing.OutCubic, 0, interval, 320 + 5, 320 + 0.5f);
            atlasTextB.MoveX(OsbEasing.OutCubic, interval, interval * 2, 320 - 5, 320 + 0.5f);
            atlasTextB.EndGroup();

            atlasTextB.StartLoopGroup(144446, (int)((animationEnd - animationStart) / interval) / 2);
            atlasTextB.Scale(OsbEasing.OutCubic, 0, interval, 0.4f, 0.35f);
            atlasTextB.Scale(OsbEasing.OutCubic, interval, interval * 2, 0.4f, 0.35f);
            atlasTextB.MoveX(OsbEasing.OutCubic, 0, interval, 320 + 5, 320 + 0.5f);
            atlasTextB.MoveX(OsbEasing.OutCubic, interval, interval * 2, 320 - 5, 320 + 0.5f);
            atlasTextB.EndGroup();


            atlasBigB.Scale(OsbEasing.OutCubic, 156446, 156446 + 500, 0.3f, 0.2f);
            atlasGBig.Scale(OsbEasing.OutCubic, 156446, 156446 + 500, 0.3f, 0.2f);
            atlasRBig.Scale(OsbEasing.OutCubic, 156446, 156446 + 500, 0.3f, 0.2f);

            atlasTextB.Scale(OsbEasing.OutCubic, 156446, 156446 + 500, 0.5f, 0.35f);
            atlasTextG.Scale(OsbEasing.OutCubic, 156446, 156446 + 500, 0.5f, 0.35f);
            atlasTextR.Scale(OsbEasing.OutCubic, 156446, 156446 + 500, 0.5f, 0.35f);


            var startTime = 156446; // The start time where the playfield is initialized
            var endTime = 169995; // The end time where the playfield is no

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

            //objects.PositionZ.Add(startTime, 100);

            var rotations = 2f;
            var rotationStart = (float)startTime;
            var rotationSpeed = ((168446 - startTime) / rotations) / 2;

            for (int i = 0; i < rotations; i++)
            {
                objects.Rotation.Add(rotationStart, new Quaternion(0, 0, 0));
                objects.Rotation.Add(rotationStart + rotationSpeed, new Quaternion(0, (float)Math.PI, 0));
                objects.Rotation.Add(rotationStart + rotationSpeed + rotationSpeed, new Quaternion(0, (float)Math.PI * 2, 0));
                rotationStart += (float)rotationSpeed + (float)rotationSpeed;
            }

            objectsWrapper.Add(objects);

            objectsWrapper.PositionZ.Add(startTime, 100);

            objectsWrapper.Rotation.Add(157221, new Quaternion(0f, 0f, 0f));
            objectsWrapper.Rotation.Add(158382, new Quaternion(0f, -(float)Math.PI, 0f), EasingFunctions.SineOut);

            objectsWrapper.Rotation.Add(163414, new Quaternion(0f, -(float)Math.PI, 0f));
            objectsWrapper.Rotation.Add(164575, new Quaternion(0f, -(float)Math.PI * 2, 0f), EasingFunctions.SineOut);

            objectsWrapper.Rotation.Add(168446, new Quaternion(0f, 0f, 0f));
            objectsWrapper.Rotation.Add(168640, new Quaternion(0f, 0.2f, 0f));
            objectsWrapper.Rotation.Add(168640, new Quaternion(0f, 0f, 0f));
            objectsWrapper.Rotation.Add(168834, new Quaternion(0f, 0.2f, 0f));
            objectsWrapper.Rotation.Add(168834, new Quaternion(0f, 0f, 0f));
            objectsWrapper.Rotation.Add(169027, new Quaternion(0f, 0.2f, 0f));
            objectsWrapper.Rotation.Add(169027, new Quaternion(0f, 0f, 0f));
            objectsWrapper.Rotation.Add(169221, new Quaternion(0f, 0.2f, 0f));
            objectsWrapper.Rotation.Add(169221, new Quaternion(0f, 0f, 0f));
            objectsWrapper.Rotation.Add(169414, new Quaternion(0f, 0.2f, 0f));
            objectsWrapper.Rotation.Add(169414, new Quaternion(0f, 0f, 0f));
            objectsWrapper.Rotation.Add(169608, new Quaternion(0f, 0.2f, 0f));
            objectsWrapper.Rotation.Add(169608, new Quaternion(0f, 0f, 0f));
            objectsWrapper.Rotation.Add(169801, new Quaternion(0f, 0.2f, 0f));
            objectsWrapper.Rotation.Add(169801, new Quaternion(0f, 0f, 0f));
            objectsWrapper.Rotation.Add(169995, new Quaternion(0f, 0.2f, 0f));
            objectsWrapper.Rotation.Add(169995, new Quaternion(0f, 0f, 0f));

            scene.Add(objectsWrapper);

            scene.Generate(camera, layer, startTime, endTime, Beatmap, 4);

            var glitchR = layer.CreateAnimation("sb/atlas/noise/noise.png", 4, 250 / 4, OsbLoopType.LoopForever, OsbOrigin.Centre, new Vector2(315, 240));
            glitchR.Fade(157221, 157221 + 100, 0, 0.3f);
            glitchR.Additive(157221);
            glitchR.Fade(158382 - 100, 158382, 0.3f, 0);
            glitchR.Fade(163414, 163414 + 100, 0, 0.3f);
            glitchR.Fade(164575 - 100, 164575, 0.3f, 0);
            glitchR.Color(157221, new Color4(255, 0, 0, 255));

            var glitchG = layer.CreateAnimation("sb/atlas/noise/noise.png", 4, 250 / 4, OsbLoopType.LoopForever, OsbOrigin.Centre, new Vector2(320, 240));
            glitchG.Fade(157221, 157221 + 100, 0, 0.3f);
            glitchG.Additive(157221);
            glitchG.Fade(158382 - 100, 158382, 0.3f, 0);
            glitchG.Fade(163414, 163414 + 100, 0, 0.3f);
            glitchG.Fade(164575 - 100, 164575, 0.3f, 0);
            glitchG.Color(157221, new Color4(0, 255, 0, 255));

            var glitchB = layer.CreateAnimation("sb/atlas/noise/noise.png", 4, 250 / 4, OsbLoopType.LoopForever, OsbOrigin.Centre, new Vector2(325, 240));
            glitchB.Fade(157221, 157221 + 100, 0, 0.3f);
            glitchB.Additive(157221);
            glitchB.Fade(158382 - 100, 158382, 0.3f, 0);
            glitchB.Fade(163414, 163414 + 100, 0, 0.3f);
            glitchB.Fade(164575 - 100, 164575, 0.3f, 0);
            glitchB.Color(157221, new Color4(0, 0, 255, 255));

            var sprites = new List<OsbSprite>
            {
                atlasRBig, atlasGBig, atlasBigB,
                atlasTextR, atlasTextG, atlasTextB
            };

            var localStart = 168640;
            var inter = 168834 - localStart;
            for (int i = 0; i < 7; i++)
            {
                foreach (var sprite in sprites)
                {
                    var pos = sprite.PositionAt(localStart);
                    sprite.MoveY(OsbEasing.None, localStart, localStart + inter - 1, pos.Y, pos.Y - Random(-5, 5));
                    sprite.MoveY(localStart + inter, pos.Y);

                    sprite.Rotate(OsbEasing.None, localStart, localStart + inter - 1, 0, Random(-0.01f, 0.01f));
                    sprite.Rotate(localStart + inter, 0);

                    sprite.MoveX(OsbEasing.None, localStart, localStart + inter - 1, pos.X, pos.X - Random(-5, 5));
                    sprite.MoveX(localStart + inter, pos.X);
                }

                localStart += inter;

            }


        }
    }
}
