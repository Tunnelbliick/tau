using System;
using System.Collections.Generic;
using System.Linq;
using OpenTK;
using OpenTK.Graphics;
using storyboard.scriptslibrary;
using StorybrewCommon.Animations;
using StorybrewCommon.Mapset;
using StorybrewCommon.Scripting;
using StorybrewCommon.Storyboarding;
using StorybrewCommon.Storyboarding.Util;
using StorybrewCommon.Storyboarding3d;
using StorybrewCommon.Subtitles;
using StorybrewCommon.Util;

namespace StorybrewScripts
{
    public class MainDropBG : StoryboardObjectGenerator
    {
        Color4 wC = new Color4(255, 255, 255, 255);
        Color4 oC = new Color4(236, 99, 74, 255);
        Color4 yC = new Color4(237, 199, 112, 255);
        Color4 cC = new Color4(94, 196, 192, 255);

        public override void Generate()
        {
            var cols = 6;
            var scrollWidth = 250f * cols;

            // General values
            var starttime = 130705; // the starttime where the playfield is initialized
            var endtime = 156446; // the endtime where the playfield is nolonger beeing rendered
            var duration = endtime - starttime; // the length the playfield is kept alive

            var leftBound = 250f * -2;
            var rightBound = 250f * 4;


            var isOffset = false;
            var secondOffset = false;

            var scale = 0.25f;

            for (int i = 0; i < cols; i++)
            {

                var initialPos = i - 2;
                var initialX = 250f * initialPos;

                var halfTime = (131672 - 130898) / 2;
                var ranY = 15 * initialPos;

                var movementPerDuration = 250f / (132446 - 132059);

                var doubleWidth = 62.5f * 2;
                var colWidth = 62.5f * 1.5f;

                var swapDur = 133801 - 133705 + 15;

                var bgT = new RelativeSprite(GetLayer("bg"), "sb/bgt.png", OsbOrigin.Centre, new Vector2(320, 240 + 25f - 240));
                var bgB = new RelativeSprite(GetLayer("bg"), "sb/bgb.png", OsbOrigin.Centre, new Vector2(320, 240 + 25f + 240));

                bgT.Fade(starttime, 0.6f);
                bgT.Fade(endtime, 0);
                bgT.ScaleVec(130705, new Vector2(0, scale));
                bgT.ScaleVec(OsbEasing.OutCubic, 130705, 131672, new Vector2(scale, 0));

                bgB.Fade(starttime, 0.6f);
                bgB.Fade(endtime, 0);
                bgB.ScaleVec(130705, new Vector2(0, scale));
                bgB.ScaleVec(OsbEasing.OutCubic, 130705, 131672, new Vector2(scale, 0));

                bgT.MoveX(130705, initialX);
                bgT.MoveY(OsbEasing.OutSine, 130705, 131672, 240);
                bgT.MoveX(OsbEasing.None, 132059, 132446, movementPerDuration * (132446 - 132059));
                bgT.MoveX(OsbEasing.OutSine, 133608, 134575, -movementPerDuration * (134575 - 133608));

                bgB.MoveX(130705, initialX);
                bgB.MoveY(OsbEasing.OutSine, 130705, 131672, -240);
                bgB.MoveX(OsbEasing.None, 132059, 132446, movementPerDuration * (132446 - 132059));
                bgB.MoveX(OsbEasing.OutSine, 133608, 134575, -movementPerDuration * (134575 - 133608));

                bgT.MoveX(OsbEasing.OutCubic, 132446, 132640, colWidth);
                bgB.MoveX(OsbEasing.OutCubic, 132446, 132640, -colWidth);

                bgT.MoveX(OsbEasing.OutCubic, 132640, 132834, -colWidth);
                bgB.MoveX(OsbEasing.OutCubic, 132640, 132834, colWidth);

                bgT.MoveX(OsbEasing.OutCubic, 132834, 133027, colWidth);
                bgB.MoveX(OsbEasing.OutCubic, 132834, 133027, -colWidth);

                bgT.MoveX(OsbEasing.OutCubic, 133027, 133221, -colWidth);
                bgB.MoveX(OsbEasing.OutCubic, 133027, 133221, colWidth);

                bgT.MoveX(OsbEasing.OutCubic, 133221, 133414, colWidth);
                bgB.MoveX(OsbEasing.OutCubic, 133221, 133414, -colWidth);

                bgT.MoveX(OsbEasing.OutCubic, 133414, 133608, -colWidth);
                bgB.MoveX(OsbEasing.OutCubic, 133414, 133608, colWidth);

                bgB.ScaleVec(OsbEasing.OutElasticQuarter, 133705 + swapDur * i, 133801 + swapDur * i * 3, new Vector2(0, -scale * 2));
                bgT.ScaleVec(OsbEasing.OutElasticQuarter, 133705 + swapDur * i, 133801 + swapDur * i * 3, new Vector2(0, -scale * 2));

                bgB.MoveY(OsbEasing.OutElasticQuarter, 133705 + swapDur * i, 133801 + swapDur * i * 3, -25f);
                bgT.MoveY(OsbEasing.OutElasticQuarter, 133705 + swapDur * i, 133801 + swapDur * i * 3, -25f);

                bgB.MoveX(OsbEasing.OutCubic, 134575, 135156, movementPerDuration * (135156 - 134575));
                bgB.MoveX(OsbEasing.None, 134575, 137479, movementPerDuration * (137479 - 134575));
                bgB.MoveX(OsbEasing.OutCubic, 137479, 137866, -movementPerDuration * (137866 - 137479));

                bgT.MoveX(OsbEasing.OutCubic, 134575, 135156, movementPerDuration * (135156 - 134575));
                bgT.MoveX(OsbEasing.None, 134575, 137479, movementPerDuration * (137479 - 134575));
                bgT.MoveX(OsbEasing.OutCubic, 137479, 137866, -movementPerDuration * (137866 - 137479));

                bgT.MoveX(OsbEasing.OutCubic, 138640, 138834, colWidth);
                bgB.MoveX(OsbEasing.OutCubic, 138640, 138834, -colWidth);

                bgT.MoveX(OsbEasing.OutCubic, 138834, 139027, -colWidth);
                bgB.MoveX(OsbEasing.OutCubic, 138834, 139027, colWidth);

                bgT.MoveX(OsbEasing.OutCubic, 139027, 139221, colWidth);
                bgB.MoveX(OsbEasing.OutCubic, 139027, 139221, -colWidth);

                bgT.MoveX(OsbEasing.OutCubic, 139221, 139414, -colWidth);
                bgB.MoveX(OsbEasing.OutCubic, 139221, 139414, colWidth);

                bgT.MoveX(OsbEasing.OutCubic, 139414, 139608, colWidth);
                bgB.MoveX(OsbEasing.OutCubic, 139414, 139608, -colWidth);

                bgT.MoveX(OsbEasing.OutCubic, 139608, 139801, -colWidth);
                bgB.MoveX(OsbEasing.OutCubic, 139608, 139801, colWidth);

                int offset = 2; // The starting offset
                int o = (i + offset) % cols;

                bgB.ScaleVec(OsbEasing.OutElasticQuarter, 139995 + swapDur * o, 140092 + swapDur * o * 3, new Vector2(0, scale * 2));
                bgT.ScaleVec(OsbEasing.OutElasticQuarter, 139995 + swapDur * o, 140092 + swapDur * o * 3, new Vector2(0, scale * 2));

                bgB.MoveY(OsbEasing.OutElasticQuarter, 139995 + swapDur * o, 140092 + swapDur * o * 3, 25f);
                bgT.MoveY(OsbEasing.OutElasticQuarter, 139995 + swapDur * o, 140092 + swapDur * o * 3, 25f);

                bgB.MoveX(OsbEasing.OutSine, 139801, 140769, movementPerDuration * (140769 - 139801));
                bgB.MoveX(OsbEasing.OutCubic, 140769, 141156, -movementPerDuration * (141156 - 140769));
                bgB.MoveX(OsbEasing.None, 140769, 142317, -movementPerDuration * (142317 - 140769));

                bgT.MoveX(OsbEasing.OutSine, 139801, 140769, movementPerDuration * (140769 - 139801));
                bgT.MoveX(OsbEasing.OutCubic, 140769, 141156, -movementPerDuration * (141156 - 140769));
                bgT.MoveX(OsbEasing.None, 140769, 142317, -movementPerDuration * (142317 - 140769));


                bgT.MoveY(OsbEasing.OutCubic, 142317, 142608, -75);
                bgT.MoveY(OsbEasing.InCubic, 142317, 142608, 25);

                bgT.MoveY(OsbEasing.OutCubic, 142608, 142898, -75);
                bgT.MoveY(OsbEasing.InCubic, 142608, 142898, 25);

                bgT.MoveY(OsbEasing.OutCubic, 142898, 143188, -75);
                bgT.MoveY(OsbEasing.InCubic, 142898, 143188, 25);

                bgT.MoveY(OsbEasing.OutCubic, 143188, 143672, -75);
                bgT.MoveY(OsbEasing.InCubic, 143188, 143672, 25);

                bgT.MoveY(OsbEasing.OutCubic, 143672, 144059, 200);

                bgB.MoveY(OsbEasing.OutCubic, 142317, 142608, 75);
                bgB.MoveY(OsbEasing.InCubic, 142317, 142608, -25);

                bgB.MoveY(OsbEasing.OutCubic, 142608, 142898, 75);
                bgB.MoveY(OsbEasing.InCubic, 142608, 142898, -25);

                bgB.MoveY(OsbEasing.OutCubic, 142898, 143188, 75);
                bgB.MoveY(OsbEasing.InCubic, 142898, 143188, -25);

                bgB.MoveY(OsbEasing.OutCubic, 143188, 143672, 75);
                bgB.MoveY(OsbEasing.InCubic, 143188, 143672, -25);

                bgB.MoveY(OsbEasing.OutCubic, 143672, 144059, -200);

                //bgB.MoveX(OsbEasing.OutSine, 142317, 143672, -initialX + 500);
                //bgB.MoveX(OsbEasing.OutSine, 143672, 144059, initialX - 500);

                // bgT.MoveX(OsbEasing.OutSine, 142317, 143672, -initialX + 500);
                // bgT.MoveX(OsbEasing.OutSine, 143672, 144059, initialX - 500);

                var loopDuration = Beatmap.GetTimingPointAt(starttime).BeatDuration / 2;
                var totalLoops = (int)((endtime - starttime) / loopDuration);

                // Split movement into two parts: constant speed and OutSine finish
                double linearEndTime = 154479; // 1000ms before end time
                double totalDistance = movementPerDuration * (154898 - 143672);   // Use your desired total distance

                // Calculate distances for each segment
                double linearDuration = linearEndTime - 143672;
                double totalDuration = 154898 - 143672;
                double linearRatio = linearDuration / totalDuration;

                // Linear movement from start to linearEndTime
                double linearDistance = totalDistance * linearRatio;
                bgB.MoveX(OsbEasing.None, 143672, linearEndTime, (float)linearDistance + 150);
                bgT.MoveX(OsbEasing.None, 143672, linearEndTime, (float)linearDistance + 150);

                // OutSine movement for the final 1000ms
                double sineDistance = totalDistance - linearDistance;
                bgB.MoveX(OsbEasing.OutSine, linearEndTime, 154898, (float)sineDistance - 140);
                bgT.MoveX(OsbEasing.OutSine, linearEndTime, 154898, (float)sineDistance - 140);

                offset = 3; // The starting offset
                o = (i + offset) % cols;

                bgB.ScaleVec(OsbEasing.OutElasticQuarter, 146188 + swapDur * o, 146285 + swapDur * o * 3, new Vector2(0, -scale * 2));
                bgT.ScaleVec(OsbEasing.OutElasticQuarter, 146188 + swapDur * o, 146285 + swapDur * o * 3, new Vector2(0, -scale * 2));

                bgB.MoveY(OsbEasing.OutElasticQuarter, 146188 + swapDur * o, 146285 + swapDur * o * 3, -25f);
                bgT.MoveY(OsbEasing.OutElasticQuarter, 146188 + swapDur * o, 146285 + swapDur * o * 3, -25f);

                offset = 5; // The starting offset
                o = (i + offset) % cols;

                bgB.ScaleVec(OsbEasing.OutElasticQuarter, 152382 + swapDur * o, 152479 + swapDur * o * 3, new Vector2(0, scale * 2));
                bgT.ScaleVec(OsbEasing.OutElasticQuarter, 152382 + swapDur * o, 152479 + swapDur * o * 3, new Vector2(0, scale * 2));

                bgB.MoveY(OsbEasing.OutElasticQuarter, 152382 + swapDur * o, 152479 + swapDur * o * 3, 25f);
                bgT.MoveY(OsbEasing.OutElasticQuarter, 152382 + swapDur * o, 152479 + swapDur * o * 3, 25f);

                var start = 144253;
                var dur = (144640 - start) * 2;
                var halfDur = dur / 2;

                while (start < 153059)
                {
                    bgB.MoveX(OsbEasing.OutSine, start, start + 25, -doubleWidth);
                    bgT.MoveX(OsbEasing.OutSine, start, start + 25, -doubleWidth);

                    start += halfDur;
                }


                bgB.MoveX(OsbEasing.OutSine, 153350, 153350 + 25, -doubleWidth);
                bgT.MoveX(OsbEasing.OutSine, 153350, 153350 + 25, -doubleWidth);

                bgB.MoveX(OsbEasing.OutSine, 153640, 153640 + 25, doubleWidth);
                bgT.MoveX(OsbEasing.OutSine, 153640, 153640 + 25, doubleWidth);

                bgB.MoveX(OsbEasing.OutSine, 154124, 154124 + 25, -doubleWidth);
                bgT.MoveX(OsbEasing.OutSine, 154124, 154124 + 25, -doubleWidth);

                bgB.MoveX(OsbEasing.OutSine, 154414, 154414 + 25, doubleWidth);
                bgT.MoveX(OsbEasing.OutSine, 154414, 154414 + 25, doubleWidth);

                var local = 132059;


                while (local < endtime)
                {
                    CancellationToken.ThrowIfCancellationRequested();
                    var pos = bgB.PositionAt(local);

                    var leftAdjust = 0f;
                    var rightAdjust = 0f;

                    if (pos.X < leftBound)
                    {
                        var x = Math.Abs(pos.X);
                        bgB.MoveX(OsbEasing.None, local, local, scrollWidth);
                        bgT.MoveX(OsbEasing.None, local, local, scrollWidth);
                    }
                    else if (pos.X > rightBound)
                    {
                        bgB.MoveX(OsbEasing.None, local, local, -scrollWidth);
                        bgT.MoveX(OsbEasing.None, local, local, -scrollWidth);
                    }
                    local += 1;
                }

                bgT.MoveY(OsbEasing.OutCubic, 154898, 155188, -75);
                bgT.MoveY(OsbEasing.InCubic, 154898, 155188, 25);

                bgT.MoveY(OsbEasing.OutCubic, 155188, 155479, -75);
                bgT.MoveY(OsbEasing.InCubic, 155188, 155479, 25);

                bgT.MoveY(OsbEasing.OutCubic, 155479, 156446, -200);

                bgB.MoveY(OsbEasing.OutCubic, 154898, 155188, 75);
                bgB.MoveY(OsbEasing.InCubic, 154898, 155188, -25);

                bgB.MoveY(OsbEasing.OutCubic, 155188, 155479, 75);
                bgB.MoveY(OsbEasing.InCubic, 155188, 155479, -25);

                bgB.MoveY(OsbEasing.OutCubic, 155479, 156446, 140);

                if (i % 2 == 0)
                {
                    bgT.Color(starttime, cC);
                    bgT.sprite.FlipH(starttime);
                    bgB.Color(starttime, cC);
                    bgB.sprite.FlipH(starttime);
                }
                else
                {
                    bgT.Color(starttime, oC);
                    bgB.Color(starttime, oC);
                }

                Simplification s = new();
                s.SetMovement(.5f);

                bgT.WriteToSprite(starttime, endtime, 1, s);
                bgB.WriteToSprite(starttime, endtime, 1, s);

            }
        }
    }
}
