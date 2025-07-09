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
    public class ProxywallBga : StoryboardObjectGenerator
    {
        public override void Generate()
        {

            var startTime = 66640;
            var endTime = 82124;

            Scene3d scene = new();
            var camera = new PerspectiveCamera();

            Color4 w = new Color4(255, 255, 255, 255);
            Color4 o = new Color4(236, 99, 74, 255);
            Color4 y = new Color4(237, 199, 112, 255);
            Color4 c = new Color4(94, 196, 192, 255);

            Color4[] colors = new Color4[]
            {
                w, o, y, c,
            };

            // Camera setup
            camera.VerticalFov.Add(startTime, 50);
            camera.HorizontalFov.Add(startTime, 50);
            camera.FarFade.Add(startTime, 750);
            camera.FarClip.Add(startTime, 1000);
            camera.NearClip.Add(startTime, 25);
            camera.NearFade.Add(startTime, 50);
            camera.TargetPosition.Add(startTime, new Vector3(0, 0, 0));
            camera.PositionX.Add(startTime, -90);
            camera.PositionX.Add(73511, -90);
            camera.PositionX.Add(73801, -45, EasingFunctions.CubicOut);
            camera.PositionX.Add(74092, 0, EasingFunctions.CubicOut);
            camera.PositionX.Add(74382, 45, EasingFunctions.CubicOut);
            camera.PositionX.Add(74769, 95, EasingFunctions.CubicOut);
            camera.PositionZ.Add(startTime, -50);
            camera.PositionY.Add(startTime, 0);

            var objects = 32;

            var objectNode = new Node3d();

            for (int i = 0; i < objects; i++)
            {
                var sprite = new Animation3d
                {
                    SpritePath = "sb/ani/sphere/frame.jpg",
                    FrameCount = 23,
                    FrameDelay = 600 / 24,
                    LoopType = OsbLoopType.LoopForever,
                    RotationMode = RotationMode.Fixed
                };


                sprite.ConfigureGenerators((s) =>
                {
                    s.ScaleDecimals = 3;
                    s.ScaleTolerance = 0.001f;
                });

                var ranY = Random(-100, 100);
                var ranZ = Random(-25, 300);
                var offsetY = Math.Sign(ranY) * 500;
                var offsetZ = Random(-1, 1) * 300;

                sprite.SpriteRotation.Add(startTime, Random(-Math.PI, Math.PI));
                sprite.SpriteScale.Add(startTime, new Vector2(0.025f));
                sprite.Additive = true;
                sprite.Coloring.Add(startTime, colors[i % colors.Length]);
                sprite.PositionX.Add(startTime, Random(-400, 1250));
                sprite.PositionY.Add(startTime, ranY + offsetY);
                sprite.PositionY.Add(68188, ranY, EasingFunctions.CircOut);
                sprite.PositionZ.Add(startTime, ranZ + offsetZ);
                sprite.PositionZ.Add(68188, ranZ, EasingFunctions.SineOut);

                objectNode.Add(sprite);
            }

            objectNode.Rotation.Add(startTime, new Quaternion(0, 0, 0));
            objectNode.PositionX.Add(startTime, 0);
            objectNode.PositionX.Add(68188, 0);
            objectNode.PositionX.Add(70124, -500, EasingFunctions.SineInOut);
            objectNode.PositionX.Add(71672, 0, EasingFunctions.SineInOut);
            objectNode.PositionX.Add(73221, -500, EasingFunctions.SineInOut);
            objectNode.PositionX.Add(74769, -1000, EasingFunctions.SineInOut);
            objectNode.PositionX.Add(76317, -500, EasingFunctions.SineInOut);
            objectNode.PositionX.Add(77866, -1000, EasingFunctions.SineInOut);
            objectNode.PositionX.Add(79414, -500, EasingFunctions.SineInOut);
            objectNode.PositionX.Add(81350, -3000, EasingFunctions.SineIn);
            scene.Add(objectNode);


            scene.Generate(camera, GetLayer("BGA"), startTime, endTime, Beatmap, 10);
        }
    }
}
