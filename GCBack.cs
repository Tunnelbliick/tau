using OpenTK;
using OpenTK.Graphics;
using StorybrewCommon.Mapset;
using StorybrewCommon.Scripting;
using StorybrewCommon.Storyboarding;
using StorybrewCommon.Storyboarding.Util;
using StorybrewCommon.Subtitles;
using StorybrewCommon.Util;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace StorybrewScripts
{
    public class GCBack : StoryboardObjectGenerator
    {
        public override void Generate()
        {

            var layer = GetLayer("GCBack");

            var crash = layer.CreateSprite("sb/blue.png", OsbOrigin.Centre, new Vector2(320, 240));
            crash.ScaleVec(205995, 854f / 640f, 480f / 400f);
            crash.Fade(205995, 1);
            crash.Fade(OsbEasing.OutSine, 206382, 207156, 1, 0);

            Vector2 size = new Vector2(900, 35);

            Color4 w = new Color4(255, 255, 255, 255);
            Color4 o = new Color4(236, 99, 74, 255);
            Color4 y = new Color4(237, 199, 112, 255);
            Color4 c = new Color4(94, 196, 192, 255);

            var endY = size.Y * 2;


            for (int i = 0; i < 12; i++)
            {

                var start = new Vector2(OsuHitObject.WidescreenStoryboardBounds.Left - 10, -size.Y);

                start.Y += endY * i;

                var stripe = layer.CreateSprite("sb/white.png", OsbOrigin.CentreLeft, start);
                if (i % 2 == 0)
                {
                    stripe.ScaleVec(OsbEasing.OutCubic, 176285, 176285 + 250, new Vector2(0, size.Y), size);
                    stripe.Fade(176285, .6f);
                    stripe.Color(176285, o);
                    stripe.Rotate(176285, -0.1f);
                }
                else
                {
                    stripe.ScaleVec(OsbEasing.OutCubic, 176382, 176382 + 250, new Vector2(0, size.Y), size);
                    stripe.Fade(176382, .6f);
                    stripe.Color(176382, c);
                    stripe.Rotate(176285, -0.1f);
                }

                stripe.Rotate(176285, 181221, -0.2f, 0.2f);

                stripe.ScaleVec(OsbEasing.OutExpo, 181221, 181608, size, new Vector2(size.X, 0));


                //stripe.MoveY(OsbEasing.OutCubic, 176188, 176575, start.Y, start.Y - endY * i);

                double localStart = 176575;
                double duration = 600;
                while (localStart < 181608)
                {
                    stripe.MoveY(OsbEasing.None, localStart, localStart + duration, start.Y, start.Y - endY * 2);

                    localStart += duration;
                    duration *= 0.9;
                }

                stripe.Fade(181608, 0f);
            }


            SpawnBlur(new Vector2(0, 500), 181608, o);
            SpawnBlur(new Vector2(640, 400), 181995, y);
            SpawnBlur(new Vector2(0, 340), 182382, c);
            SpawnBlur(new Vector2(640, 320), 182769, w);

            SpawnBlur(new Vector2(0, 300), 185479, o);
            SpawnBlur(new Vector2(640, 300), 185866, y);

            SpawnBlur(new Vector2(0, 300), 187801, y);
            SpawnBlur(new Vector2(640, 300), 188188, c);
            SpawnBlur(new Vector2(0, 300), 188575, o);
            SpawnBlur(new Vector2(640, 300), 188963, y);

            SpawnBlur(new Vector2(0, 300), 191672, c);
            SpawnBlur(new Vector2(640, 300), 192059, o);

            SpawnBlur(new Vector2(0, 400), 193995, y);
            SpawnBlur(new Vector2(640, 400), 194382, c);
            SpawnBlur(new Vector2(0, 400), 194769, o);
            SpawnBlur(new Vector2(640, 400), 195156, y);

            SpawnBlur(new Vector2(-200, 300), 197866, o);
            SpawnBlur(new Vector2(400, 360), 198253, y);

        }

        public void SpawnBlur(Vector2 position, double starttime, Color4 color, float initialFade = 0.3f, double duration = 300)
        {
            var s = GetLayer("GCBack").CreateSprite("sb/blur.png", OsbOrigin.Centre, position);
            s.ScaleVec(starttime, new Vector2(3f, 1f));
            s.Fade(starttime, starttime + duration, initialFade, 0);
            s.Color(starttime, color);
        }
    }
}
