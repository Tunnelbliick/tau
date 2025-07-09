using OpenTK;
using OpenTK.Graphics;
using StorybrewCommon.Animations;
using StorybrewCommon.Scripting;
using StorybrewCommon.Storyboarding;
using StorybrewCommon.Storyboarding3d;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StorybrewScripts
{
    public class BuildUpBGA : StoryboardObjectGenerator
    {
        public override void Generate()
        {
            var layer = GetLayer("BGA");

            var startTime = 117737;
            var endTime = 131672;

            Scene3d scene = new();
            var camera = new PerspectiveCamera();

            Color4 w = new Color4(255, 255, 255, 255);
            Color4 o = new Color4(236, 99, 74, 255);
            Color4 y = new Color4(237, 199, 112, 255);
            Color4 c = new Color4(94, 196, 192, 255);

            Color4[] colors = new Color4[] { w, o, y, c };

            // Camera setup
            camera.VerticalFov.Add(startTime, 65);
            camera.HorizontalFov.Add(startTime, 65);
            camera.FarFade.Add(startTime, 800);
            camera.FarClip.Add(startTime, 900);
            camera.NearFade.Add(startTime, 50f);
            camera.NearClip.Add(startTime, 25f);
            camera.TargetPosition.Add(startTime, new Vector3(0, 0, 0));
            camera.PositionX.Add(startTime, 0);
            camera.PositionZ.Add(startTime, -20);
            camera.PositionY.Add(startTime, 0);

            Node3d rings = new();
            Node3d objects = new();

            var numbPartices = 500;

            var particels = new List<(float depth, Sprite3d sprite)>();

            for (int i = 0; i < numbPartices; i++)
            {
                var xPos = Random(-100, 100);
                var yPos = Random(-100, 100);
                var zPos = Random(-1000, 500);

                var xOffset = Random(-1000, 1000);
                var yOffset = Random(-1000, 1000);
                //var zOffset = Random(-500, 500);

                Sprite3d sprite = new Sprite3d
                {
                    SpritePath = "sb/particle.png"
                };

                sprite.PositionX.Add(startTime, 0);
                sprite.PositionY.Add(startTime, 0);

                sprite.Coloring.Add(startTime, colors[Random(0, colors.Length)]);
                //sprite.PositionZ.Add(startTime, z + zOffset);

                sprite.SpriteScale.Add(startTime, 0.02f);


                sprite.PositionX.Add(119285, xPos, EasingFunctions.CubicOut);
                sprite.PositionY.Add(119285, yPos, EasingFunctions.CubicOut);
                sprite.PositionZ.Add(119285, zPos, EasingFunctions.CubicOut);

                particels.Add((zPos, sprite));
            }

            var ringTime = new List<int> { 125479, 126253, 127027, 128575, 128963, 129350, 129737, 130124 };
            int counter = 1;
            foreach (int time in ringTime)
            {
                Sprite3d ring = new Sprite3d
                {
                    SpritePath = "sb/circle.png",
                };

                ring.Opacity.Add(startTime, 0);
                ring.Opacity.Add(123930, 0);
                ring.Opacity.Add(125479, 0.5f, EasingFunctions.CubicOut);

                ring.SpriteScale.Add(startTime, 0);
                ring.SpriteScale.Add(123930, 0);
                ring.SpriteScale.Add(125479, 0.025f, EasingFunctions.CubicOut);

                ring.Coloring.Add(startTime, colors[counter % colors.Length]);

                ring.PositionZ.Add(123930, -50);
                ring.PositionZ.Add(125479, -50 + 100 * counter, EasingFunctions.SineOut);

                ring.PositionZ.Add(endTime, -50 + 100 * counter - 1250, EasingFunctions.SineIn);

                ring.ConfigureGenerators((s) =>
                {
                    s.ScaleDecimals = 12;
                    s.ScaleTolerance = 0.00000001f;
                });

                counter++;

                rings.Add(ring);
            }

            particels = particels.OrderBy(p => p.depth).ToList();

            foreach (var (depth, sprite) in particels)
            {
                objects.Add(sprite);
            }

            objects.Opacity.Add(startTime, 1f);



            objects.PositionZ.Add(startTime, -500);
            objects.PositionZ.Add(120834, 0, EasingFunctions.CubicOut);
            objects.PositionZ.Add(120834, 250, EasingFunctions.CubicOut);
            objects.PositionZ.Add(120834, 500, EasingFunctions.CubicOut);
            objects.PositionZ.Add(122382, 750, EasingFunctions.CubicOut);
            objects.PositionZ.Add(122382, 1000, EasingFunctions.CubicOut);
            objects.PositionZ.Add(125479, 1250, EasingFunctions.CubicOut);
            objects.PositionZ.Add(endTime, -1000, EasingFunctions.SineIn);

            objects.Rotation.Add(startTime, new Quaternion(0, 0, 0));
            objects.Rotation.Add(125479, new Quaternion(0, 0, 0f), EasingFunctions.CubicOut);
            objects.Rotation.Add(endTime, new Quaternion((float)Math.PI, 0, 0), EasingFunctions.SineIn);

            scene.Add(rings);
            scene.Add(objects);

            scene.Generate(camera, layer, startTime, endTime, Beatmap, 4);

            var follow = layer.CreateAnimation("sb/ani/curiosity/frame.jpg", 50, 2000 / 50, OsbLoopType.LoopOnce);
            follow.Rotate(OsbEasing.OutCubic, 117640, 117640 + 500, 0.2, 0);
            follow.Scale(OsbEasing.OutCubic, 117640, 117640 + 250, 0, 0.45f);
            follow.Scale(117640 + 250, 119285, 0.45, 0.5);
            follow.Scale(OsbEasing.OutCubic, 119285, 119285 + 250, 0.5f, 0.2f);
            follow.Rotate(OsbEasing.OutCubic, 119285, 119285 + 250, 0, .2f);
            follow.Additive(117640);
            follow.Fade(117640, 117640 + 250, 0, 1);
            follow.Fade(OsbEasing.OutCubic, 119285, 119285 + 250, 1, 0);

            var bg = layer.CreateSprite("sb/ani/back/bg.png", OsbOrigin.Centre, new Vector2(300, 240));
            bg.Rotate(130124, 131479, -0.3, .3f);
            bg.Scale(OsbEasing.OutCubic, 130124, 131285, 0, 0.25f);
            bg.Fade(130124, 1);
            bg.Scale(OsbEasing.OutCubic, 131285, 131479, 0.25f, 0.15f);
            bg.Fade(OsbEasing.OutCubic, 131285, 131479, 1, 0);

            var weAre = layer.CreateSprite("sb/ani/back/weare.png", OsbOrigin.Centre, new Vector2(300, 240));
            weAre.Rotate(OsbEasing.OutCubic, 130124, 130705, -0.5, 0);
            weAre.Scale(OsbEasing.OutCubic, 130124, 130705, 0, 0.25f);
            weAre.Fade(130124, 1);
            weAre.Scale(OsbEasing.OutCubic, 131285, 131479, 0.25f, 0.15f);
            weAre.Rotate(OsbEasing.OutCubic, 131285, 131479, 0, 0.2f);
            weAre.Fade(OsbEasing.OutCubic, 131285, 131479, 1, 0);

            var back = layer.CreateSprite("sb/ani/back/back.png", OsbOrigin.Centre, new Vector2(300, 240));
            back.Rotate(OsbEasing.OutCubic, 130511, 131092, 0.5, 0);
            back.Scale(OsbEasing.OutCubic, 130511, 131092, 0, 0.25f);
            back.Fade(130511, 1);
            back.Scale(OsbEasing.OutCubic, 131285, 131479, 0.25f, 0.15f);
            back.Rotate(OsbEasing.OutCubic, 131285, 131479, 0, 0.2f);
            back.Fade(OsbEasing.OutCubic, 131285, 131479, 1, 0);

        }
    }
}
