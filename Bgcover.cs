using OpenTK;
using OpenTK.Graphics;
using StorybrewCommon.Animations;
using StorybrewCommon.Mapset;
using StorybrewCommon.Scripting;
using StorybrewCommon.Storyboarding;
using StorybrewCommon.Storyboarding.Util;
using StorybrewCommon.Subtitles;
using StorybrewCommon.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading;

namespace StorybrewScripts
{
    public class Bgcover : StoryboardObjectGenerator
    {
        StoryboardLayer background;
        StoryboardLayer bgOverLay;

        //        public override bool Multithreaded => false;

        public override void Generate()
        {
            background = GetLayer("bg");
            var cover = GetLayer("cover");
            bgOverLay = GetLayer("bgOverlay");


            OsbSprite backgroundCover = cover.CreateSprite("sb/white.png", OsbOrigin.Centre, new Vector2(320, 240));
            backgroundCover.ScaleVec(0, 854f, 480f);
            backgroundCover.Color(0, new Color4(0, 0, 0, 1));
            backgroundCover.Color(181221, 182769, new Color4(0, 0, 0, 1), new Color4(0, 0, 0, 1));
            backgroundCover.Fade(0, 1);
            backgroundCover.Fade(285381, 0);

            OsbSprite blackCover = bgOverLay.CreateSprite("sb/white.png", OsbOrigin.Centre, new Vector2(320, 240));
            blackCover.ScaleVec(264230, 854f, 480f);
            blackCover.Color(264230, new Color4(0, 0, 0, 1));
            blackCover.Fade(OsbEasing.InSine, 264230, 264230 + 2000, 1, 0);

            // Intro();

            OsbSprite overlay = bgOverLay.CreateSprite("sb/moody.png", OsbOrigin.Centre, new Vector2(320, 260));
            overlay.Scale(264230, 0.5f);
            overlay.Fade(264230, 1);
            overlay.Fade(285381, 0);

            OsbSprite crash = bgOverLay.CreateSprite("sb/white.png", OsbOrigin.Centre, new Vector2(320, 240));
            crash.ScaleVec(168446, 854f, 480f);
            crash.Fade(168446, 0.25f);
            crash.Fade(169995, 0);

            OsbSprite crashOverlay = bgOverLay.CreateSprite("sb/crash.png", OsbOrigin.Centre, new Vector2(320, 240));
            crashOverlay.Scale(OsbEasing.OutExpo, 168631, 168834, 0f, 0.35f);
            crashOverlay.Fade(168631, 1f);
            crashOverlay.Fade(169995, 0);


            var loopDuration = 1250;  //(int)Beatmap.GetControlPointAt(234229).BeatDuration * 3;
            var analogBit = GetMapsetBitmap("sb/ani/analog/frame-0.jpg");
            var analogDMG = bgOverLay.CreateAnimation("sb/ani/analog/frame-.jpg", 44, loopDuration / 44, OsbLoopType.LoopForever);
            analogDMG.Fade(234229, 1f);
            analogDMG.Additive(234229);
            analogDMG.ScaleVec(234229, 855f / analogBit.Width, 500f / analogBit.Height);
            analogDMG.Fade(264230, 0);


            analogDMG.StartLoopGroup(234229, (264230 - 234229) / loopDuration / 4);
            analogDMG.FlipV(loopDuration, loopDuration * 2);
            analogDMG.FlipH(loopDuration * 2, loopDuration * 3);
            analogDMG.FlipV(loopDuration * 2, loopDuration * 3);
            analogDMG.FlipH(loopDuration * 3, loopDuration * 4);
            analogDMG.EndGroup();


            analogDMG.StartLoopGroup(234229, (264230 - 234229) / 1000 / 12);
            analogDMG.FlipV(1000 / 24, 1000 / 12);
            analogDMG.EndGroup();

            var noiseR = bgOverLay.CreateAnimation("sb/atlas/noise/noise.png", 4, 60, OsbLoopType.LoopForever, OsbOrigin.Centre, new Vector2(315, 239));
            noiseR.Fade(207156, 207156 + 500, 0, 0.1f);
            noiseR.Additive(207156);
            noiseR.Fade(219834, 0);
            noiseR.Color(207156, new Color4(255, 0, 0, 1));

            var noiseG = bgOverLay.CreateAnimation("sb/atlas/noise/noise.png", 4, 60, OsbLoopType.LoopForever, OsbOrigin.Centre, new Vector2(320, 240));
            noiseG.Fade(207156, 207156 + 500, 0, 0.1f);
            noiseG.Additive(207156);
            noiseG.Fade(219834, 0);
            noiseG.Color(207156, new Color4(0, 255, 0, 1));

            var noiseB = bgOverLay.CreateAnimation("sb/atlas/noise/noise.png", 4, 60, OsbLoopType.LoopForever, OsbOrigin.Centre, new Vector2(325, 241));
            noiseB.Fade(207156, 207156 + 500, 0, 0.1f);
            noiseB.Additive(207156);
            noiseB.Fade(219834, 0);
            noiseB.Color(207156, new Color4(0, 0, 255, 1));

            OsbSprite overlayEarly = bgOverLay.CreateSprite("sb/moody.png", OsbOrigin.Centre, new Vector2(320, 260));
            overlayEarly.ScaleVec(207156, new Vector2(0.6f, 0.8f));
            overlayEarly.Fade(207156, 208317, 1, 1f);
            overlayEarly.Fade(219930, 0);

            var overlayBit = GetMapsetBitmap("sb/overlay.png");
            var overlaySprite = bgOverLay.CreateSprite("sb/overlay.png");
            overlaySprite.Fade(0, 0 + 500, 0f, 0.1f);
            overlaySprite.Additive(0);
            overlaySprite.Scale(0, 854f / overlayBit.Width);
            overlaySprite.Fade(285381, 0);

            var vigenetteBit = GetMapsetBitmap("sb/vigenette.png");
            var vignette = bgOverLay.CreateSprite("sb/vigenette.png");
            vignette.ScaleVec(0, 854f / vigenetteBit.Width, 480f / vigenetteBit.Height);
            vignette.Fade(0, 0 + 500, 0, 1);
            vignette.Fade(285381, 0);

            LostConnectionDebug();
            WindoowsXP();
        }

        public void Intro()
        {
            var starttime = 2382;
            var bound = OsuHitObject.WidescreenStoryboardBounds;
            var topLeft = new Vector2(bound.Left + 20, bound.Top + 20);
            var bottomRight = new Vector2(bound.Right, bound.Bottom);

            var cols = 30;
            var rows = 18;

            var colSpacing = (bottomRight.X - topLeft.X) / cols;
            var rowSpacing = (bottomRight.Y - topLeft.Y) / rows;

            // Create a list of all grid points
            // ...existing code...
            // Create a list of all grid points
            List<Vector2> gridPoints = new List<Vector2>();

            for (int i = 0; i < cols; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    var x = topLeft.X + colSpacing * i;
                    var y = topLeft.Y + rowSpacing * j;
                    gridPoints.Add(new Vector2(x, y));
                }
            }

            // Draw lines between neighboring points to form a mesh
            for (int i = 0; i < cols; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    int idx = i * rows + j;
                    var current = gridPoints[idx];

                    //current += new Vector2(-1,-1);

                    // Connect to right neighbor
                    if (i < cols - 1)
                    {
                        var right = gridPoints[(i + 1) * rows + j];
                        var line = background.CreateSprite("sb/white.png", OsbOrigin.CentreLeft, current + new Vector2(-1, 0));
                        line.ScaleVec(starttime, new Vector2((right - current).Length + 1, 2));
                        line.Rotate(starttime, Math.Atan2(right.Y - current.Y, right.X - current.X));
                        line.Fade(17092, 0);

                        var localStart = starttime;
                        var interval = Random(500, 1000);
                        while (localStart < 17092)
                        {
                            AdjustColor(line, localStart);
                            localStart += interval;
                        }
                    }
                    // Connect to bottom neighbor
                    if (j < rows - 1)
                    {
                        var down = gridPoints[i * rows + (j + 1)];
                        var line = background.CreateSprite("sb/white.png", OsbOrigin.CentreLeft, current);
                        line.ScaleVec(starttime, new Vector2((down - current).Length + 1, 2));
                        line.Rotate(starttime, Math.Atan2(down.Y - current.Y, down.X - current.X));
                        line.Fade(17092, 0);

                        var localStart = starttime;
                        var interval = Random(1000, 2000);
                        while (localStart < 17092)
                        {
                            AdjustColor(line, localStart);
                            localStart += interval;
                        }
                    }
                }
            }

        }

        public void AdjustColor(OsbSprite sprite, double time, double transitionTime = 250, OsbEasing easing = OsbEasing.InCubic)
        {
            var colorInt = Random(-1, 2);

            if (colorInt < 0 && sprite.OpacityAt(time) != 0)
            {
                // Fade to black
                sprite.Fade(easing, time, time + transitionTime, sprite.OpacityAt(time), 0);
            }
            else if (colorInt > 0 && sprite.OpacityAt(time) != 1)
            {
                // Fade to white
                sprite.Fade(easing, time, time + transitionTime, sprite.OpacityAt(time), 1);
            }
        }

        public void LostConnectionDebug()
        {

            var lineheight = 7.255f;
            var log = background.CreateSprite("sb/atlas/log.png", OsbOrigin.CentreLeft, new Vector2(-20, 578));
            log.Fade(207156, 207156 + 500, 0, 0.5f);
            log.Scale(207156, .3f);

            var linePerTime = (218382 - 207156) / 50;
            double start = 207156;
            var end = 218382;
            while (start < end)
            {
                log.MoveY(start, log.PositionAt(start).Y - lineheight);
                start += linePerTime;
            }

            var topCover = background.CreateSprite("sb/white.png", OsbOrigin.CentreLeft, new Vector2(OsuHitObject.WidescreenStoryboardBounds.Left, 450.5f));
            var bottomCover = background.CreateSprite("sb/white.png", OsbOrigin.CentreLeft, new Vector2(OsuHitObject.WidescreenStoryboardBounds.Left, 90));

            topCover.Fade(207156, 1);
            topCover.Fade(219834, 0);
            topCover.Color(207156, new Color4(0, 0, 0, 1));
            topCover.ScaleVec(207156, 854f, 100);

            bottomCover.Fade(207156, 1);
            bottomCover.Fade(219834, 0);
            bottomCover.Color(207156, new Color4(0, 0, 0, 1));
            bottomCover.ScaleVec(207156, 854f, 200);

            var sphereR = background.CreateAnimation("sb/ani/sphere/frame.jpg", 24, 1000 / 24, OsbLoopType.LoopForever, OsbOrigin.Centre, new Vector2(-2, 59));
            sphereR.Fade(207156, 207156 + 500, 0, 0.5f);
            sphereR.Scale(207156, 0.08f);
            sphereR.Rotate(207156, 0.4f);
            sphereR.Additive(207156);
            sphereR.Color(207156, new Color4(255, 0, 0, 1));

            var sphereG = background.CreateAnimation("sb/ani/sphere/frame.jpg", 24, 1000 / 24, OsbLoopType.LoopForever, OsbOrigin.Centre, new Vector2(0, 60));
            sphereG.Fade(207156, 207156 + 500, 0, 0.5f);
            sphereG.Scale(207156, 0.08f);
            sphereG.Rotate(207156, 0.4f);
            sphereG.Additive(207156);
            sphereG.Color(207156, new Color4(0, 255, 0, 1));

            var sphereB = background.CreateAnimation("sb/ani/sphere/frame.jpg", 24, 1000 / 24, OsbLoopType.LoopForever, OsbOrigin.Centre, new Vector2(2, 61));
            sphereB.Fade(207156, 207156 + 500, 0, 0.5f);
            sphereB.Scale(207156, 0.08f);
            sphereB.Rotate(207156, 0.4f);
            sphereB.Additive(207156);
            sphereB.Color(207156, new Color4(0, 0, 255, 1));

            var atlastR = background.CreateSprite("sb/atlas/atlas.png", OsbOrigin.CentreLeft, new Vector2(44, 59));
            atlastR.Fade(207156, 207156 + 500, 0, 0.5f);
            atlastR.Scale(207156, 0.3f);
            atlastR.Additive(207156);
            atlastR.Color(207156, new Color4(255, 0, 0, 1));

            var atlastG = background.CreateSprite("sb/atlas/atlas.png", OsbOrigin.CentreLeft, new Vector2(45, 60));
            atlastG.Fade(207156, 207156 + 500, 0, 0.5f);
            atlastG.Scale(207156, 0.3f);
            atlastG.Additive(207156);
            atlastG.Color(207156, new Color4(0, 255, 0, 1));

            var atlastB = background.CreateSprite("sb/atlas/atlas.png", OsbOrigin.CentreLeft, new Vector2(47, 61));
            atlastB.Fade(207156, 207156 + 500, 0, 0.5f);
            atlastB.Scale(207156, 0.3f);
            atlastB.Additive(207156);
            atlastB.Color(207156, new Color4(0, 0, 255, 1));

            var node1 = background.CreateSprite("sb/atlas/node.png", OsbOrigin.Centre, new Vector2(25, 135));
            var node1Text = background.CreateSprite("sb/atlas/text/node1.png", OsbOrigin.Centre, new Vector2(25, 110));
            var node1Status = background.CreateSprite("sb/atlas/text/connected.png", OsbOrigin.Centre, new Vector2(25, 162));

            var node2 = background.CreateSprite("sb/atlas/node.png", OsbOrigin.Centre, new Vector2(125, 135));
            var node2Text = background.CreateSprite("sb/atlas/text/node2.png", OsbOrigin.Centre, new Vector2(125, 110));
            var node2Status = background.CreateSprite("sb/atlas/text/lost.png", OsbOrigin.Centre, new Vector2(125, 162));
            var reconnecting = background.CreateSprite("sb/atlas/text/link.png", OsbOrigin.Centre, new Vector2(125, 162));
            var connecting = background.CreateAnimation("sb/atlas/text/connecting/frame.png", 3, 300, OsbLoopType.LoopForever, OsbOrigin.Centre, new Vector2(125, 162));
            var connected = background.CreateSprite("sb/atlas/text/connected.png", OsbOrigin.Centre, new Vector2(125, 162));

            var computer1 = background.CreateSprite("sb/atlas/computer.png", OsbOrigin.Centre, new Vector2(250, 135));
            var computer2 = background.CreateSprite("sb/atlas/computer.png", OsbOrigin.Centre, new Vector2(375, 135));

            var computer1Text = background.CreateSprite("sb/atlas/text/client1.png", OsbOrigin.Centre, new Vector2(250, 110));
            var computer2Text = background.CreateSprite("sb/atlas/text/client2.png", OsbOrigin.Centre, new Vector2(375, 110));

            var client1Status = background.CreateSprite("sb/atlas/text/lost.png", OsbOrigin.Centre, new Vector2(250, 162));
            var client2Status = background.CreateSprite("sb/atlas/text/lost.png", OsbOrigin.Centre, new Vector2(375, 162));

            var client1Reconnecting = background.CreateAnimation("sb/atlas/text/connecting/frame.png", 3, 300, OsbLoopType.LoopForever, OsbOrigin.Centre, new Vector2(250, 162));
            var client2Reconnecting = background.CreateAnimation("sb/atlas/text/connecting/frame.png", 3, 300, OsbLoopType.LoopForever, OsbOrigin.Centre, new Vector2(375, 162));

            var client1Connected = background.CreateSprite("sb/atlas/text/connected.png", OsbOrigin.Centre, new Vector2(250, 162));
            var client2Connected = background.CreateSprite("sb/atlas/text/connected.png", OsbOrigin.Centre, new Vector2(375, 162));

            var link = background.CreateSprite("sb/atlas/text/link.png", OsbOrigin.CentreLeft, new Vector2(500, 450));
            var loading = background.CreateAnimation("sb/ani/loading/frame-.jpeg", 8, 100, OsbLoopType.LoopForever, OsbOrigin.Centre, new Vector2(480, 450));
            var connectedSucces = background.CreateSprite("sb/atlas/text/connected.png", OsbOrigin.CentreLeft, new Vector2(442, 450));

            var atlastNetwork = background.CreateSprite("sb/atlas/network.png", OsbOrigin.CentreLeft, new Vector2(515, 140));
            atlastNetwork.Fade(207156, 207156 + 500, 0, 0.5f);
            atlastNetwork.Scale(207156, 0.4f);

            var sphere2R = background.CreateAnimation("sb/ani/sphere/frame.jpg", 24, 1000 / 24, OsbLoopType.LoopForever, OsbOrigin.Centre, new Vector2(489, 134.5f));
            sphere2R.Fade(207156, 207156 + 500, 0, 0.5f);
            sphere2R.Scale(207156, 0.05f);
            sphere2R.Rotate(207156, Math.PI + -0.5f);
            sphere2R.Additive(207156);
            sphere2R.Color(207156, new Color4(255, 0, 0, 1));

            var sphere2G = background.CreateAnimation("sb/ani/sphere/frame.jpg", 24, 1000 / 24, OsbLoopType.LoopForever, OsbOrigin.Centre, new Vector2(490, 135));
            sphere2G.Fade(207156, 207156 + 500, 0, 0.5f);
            sphere2G.Scale(207156, 0.05f);
            sphere2G.Rotate(207156, Math.PI + -0.5f);
            sphere2G.Additive(207156);
            sphere2G.Color(207156, new Color4(0, 255, 0, 1));

            var sphere2B = background.CreateAnimation("sb/ani/sphere/frame.jpg", 24, 1000 / 24, OsbLoopType.LoopForever, OsbOrigin.Centre, new Vector2(491, 135.5f));
            sphere2B.Fade(207156, 207156 + 500, 0, 0.5f);
            sphere2B.Scale(207156, 0.05f);
            sphere2B.Rotate(207156, Math.PI + -0.5f);
            sphere2B.Additive(207156);
            sphere2B.Color(207156, new Color4(0, 0, 255, 1));

            SpawnNode(new Vector2(540, 200), 12f, true);
            SpawnNode(new Vector2(540, 295), 12f);
            SpawnNode(new Vector2(540, 390), 12f);
            SpawnNode(new Vector2(650, 200), 12f);
            SpawnNode(new Vector2(650, 238), 12f);
            SpawnNode(new Vector2(650, 276), 12f);
            SpawnNode(new Vector2(650, 314), 12f);
            SpawnNode(new Vector2(650, 352), 12f);
            SpawnNode(new Vector2(650, 390), 12f);

            double baseDuration = 180;
            double startTime = 207156;
            int totalSteps = 11;

            // Generate durations using OutSine easing
            List<double> durations = new List<double>();
            for (int i = 0; i < totalSteps; i++)
            {
                baseDuration *= 1.2f;
                durations.Add(baseDuration); // scaled
            }

            // Now draw each line using the progressively increasing durations
            startTime = DrawLine(new Vector2(540, 200), new Vector2(650, 238), startTime, durations[0]);
            startTime = DrawLine(new Vector2(650, 238), new Vector2(540, 390), startTime, durations[1]);
            startTime = DrawLine(new Vector2(540, 390), new Vector2(650, 200), startTime, durations[2]);
            startTime = DrawLine(new Vector2(650, 200), new Vector2(540, 295), startTime, durations[3]);
            startTime = DrawLine(new Vector2(540, 295), new Vector2(650, 352), startTime, durations[4]);
            startTime = DrawLine(new Vector2(650, 352), new Vector2(540, 200), startTime, durations[5]);
            startTime = DrawLine(new Vector2(540, 200), new Vector2(650, 276), startTime, durations[6]);
            startTime = DrawLine(new Vector2(650, 276), new Vector2(540, 295), startTime, durations[7]);
            startTime = DrawLine(new Vector2(540, 295), new Vector2(650, 390), startTime, durations[8]);
            startTime = DrawLine(new Vector2(650, 390), new Vector2(540, 390), startTime, durations[9]);
            startTime = DrawLine(new Vector2(540, 390), new Vector2(650, 314), startTime, durations[10]);


            link.Fade(207156, 207156 + 500, 0, 0.5f);
            link.StartLoopGroup(207156, 12);
            link.Color(0, 500, new Color4(255, 255, 0, 1), new Color4(0, 0, 0, 1));
            link.Color(500, 1000, new Color4(0, 0, 0, 1), new Color4(255, 255, 0, 1));
            link.EndGroup();
            link.Scale(207156, 0.6f);
            link.Fade(217608, 0);

            loading.Fade(207156, 207156 + 500, 0, 0.5f);
            loading.StartLoopGroup(207156, 12);
            loading.Color(0, 500, new Color4(255, 255, 0, 1), new Color4(0, 0, 0, 1));
            loading.Color(500, 1000, new Color4(0, 0, 0, 1), new Color4(255, 255, 0, 1));
            loading.EndGroup();
            loading.Scale(207156, 0.1f);
            loading.Fade(217608, 0);

            connectedSucces.Fade(217608, 0.5f);
            connectedSucces.Scale(217608, 0.8f);
            connectedSucces.Color(217608, new Color4(0, 255, 0, 1));

            node1.Fade(207156, 207156 + 500, 0, 0.5f);
            node1.Scale(207156, 0.065f);

            node2.Fade(207156, 207156 + 500, 0, 0.5f);
            node2.Scale(207156, 0.065f);

            computer1.Fade(207156, 207156 + 500, 0, 0.5f);
            computer1.Scale(207156, 0.15);

            computer2.Fade(207156, 207156 + 500, 0, 0.5f);
            computer2.Scale(207156, 0.15);

            computer1Text.Fade(207156, 207156 + 500, 0, 0.5f);
            computer1Text.Scale(207156, 0.35f);

            computer1Text.Fade(207156, 207156 + 500, 0, 0.5f);
            computer1Text.Scale(207156, 0.35f);

            computer2Text.Fade(207156, 207156 + 500, 0, 0.5f);
            computer2Text.Scale(207156, 0.35f);

            node1Text.Fade(207156, 207156 + 500, 0, 0.5f);
            node1Text.Scale(207156, 0.35f);

            node2Text.Fade(207156, 207156 + 500, 0, 0.5f);
            node2Text.Scale(207156, 0.35f);

            node1Status.Fade(207156, 207156 + 500, 0, 0.5f);
            node1Status.Scale(207156, 0.3f);
            node1Status.Color(207156, new Color4(0, 255, 0, 1));

            node2Status.Fade(207156, 207156 + 500, 0, 0.5f);
            node2Status.Scale(207156, 0.3f);
            node2Status.Color(207156, new Color4(255, 0, 0, 1));
            node2Status.Fade(209092, 0);

            client1Status.Fade(207156, 207156 + 500, 0, 0.5f);
            client1Status.Scale(207156, 0.3f);
            client1Status.Color(207156, new Color4(255, 0, 0, 1));
            client1Status.Fade(217608, 0);

            client2Status.Fade(207156, 207156 + 500, 0, 0.5f);
            client2Status.Scale(207156, 0.3f);
            client2Status.Color(207156, new Color4(255, 0, 0, 1));
            client2Status.Fade(217608, 0);

            client1Reconnecting.Scale(217608, 0.3f);
            client1Reconnecting.Color(217608, new Color4(255, 255, 0, 1));
            client1Reconnecting.Fade(217608, 0.5f);
            client1Reconnecting.Fade(219446, 0);

            client2Reconnecting.Scale(217608, 0.3f);
            client2Reconnecting.Color(217608, new Color4(255, 255, 0, 1));
            client2Reconnecting.Fade(217608, 0.5f);
            client2Reconnecting.Fade(219446, 0);

            reconnecting.Scale(209092, 0.25f);
            reconnecting.Color(209092, new Color4(255, 255, 0, 1));
            reconnecting.Fade(209092, 0.5f);
            reconnecting.Fade(211414, 0);

            connecting.Scale(211414, 0.3f);
            connecting.Color(211414, new Color4(255, 255, 0, 1));
            connecting.Fade(211414, 0.5f);
            connecting.Fade(217608, 0);

            connected.Scale(217608, 0.3f);
            connected.Color(217608, new Color4(0, 255, 0, 1));
            connected.Fade(217608, 0.5f);

            client1Connected.Scale(219446, 0.3f);
            client1Connected.Color(219446, new Color4(0, 255, 0, 1));
            client1Connected.Fade(219446, 0.5f);

            client2Connected.Scale(219446, 0.3f);
            client2Connected.Color(219446, new Color4(0, 255, 0, 1));
            client2Connected.Fade(219446, 0.5f);

            var fadeStart = 219543;
            var fadeEnd = 219834;
            var interval = 219608 - fadeStart;
            var currentFade = 1f;

            var cover = background.CreateSprite("sb/white.png", OsbOrigin.Centre, new Vector2(320, 240));
            cover.ScaleVec(fadeStart, 854f, 480f);
            cover.Color(fadeStart, new Color4(0, 0, 0, 1));

            while (fadeStart < fadeEnd)
            {

                cover.Fade(fadeStart, currentFade);

                currentFade = Math.Abs(currentFade - 1f);
                fadeStart += interval;
            }

            sphereR.Fade(fadeEnd, currentFade);
            sphereG.Fade(fadeEnd, currentFade);
            sphereB.Fade(fadeEnd, currentFade);
            atlastR.Fade(fadeEnd, currentFade);
            atlastG.Fade(fadeEnd, currentFade);
            atlastB.Fade(fadeEnd, currentFade);
            log.Fade(fadeEnd, currentFade);
            node1.Fade(fadeEnd, currentFade);
            node2.Fade(fadeEnd, currentFade);
            node1Text.Fade(fadeEnd, currentFade);
            node2Text.Fade(fadeEnd, currentFade);
            node1Status.Fade(fadeEnd, currentFade);
            connected.Fade(fadeEnd, currentFade);
            computer1.Fade(fadeEnd, currentFade);
            computer2.Fade(fadeEnd, currentFade);
            computer1Text.Fade(fadeEnd, currentFade);
            computer2Text.Fade(fadeEnd, currentFade);
            client1Connected.Fade(fadeEnd, currentFade);
            client2Connected.Fade(fadeEnd, currentFade);
            connectedSucces.Fade(fadeEnd, currentFade);
            atlastNetwork.Fade(fadeEnd, currentFade);
            sphere2R.Fade(fadeEnd, currentFade);
            sphere2G.Fade(fadeEnd, currentFade);
            sphere2B.Fade(fadeEnd, currentFade);

        }

        public void SpawnNode(Vector2 position, float size, bool filled = false)
        {
            var borderR = background.CreateSprite("sb/white.png", OsbOrigin.Centre, position + new Vector2(-1f, -0.5f));
            borderR.Fade(207156, 207156 + 1000, 0, 0.5f);
            borderR.Scale(OsbEasing.OutCubic, 207156, 207156 + 250, 0, size + 5f);
            borderR.Fade(219834, 0);
            borderR.Additive(207156);
            borderR.Color(207156, new Color4(255, 0, 0, 1));

            var borderG = background.CreateSprite("sb/white.png", OsbOrigin.Centre, position);
            borderG.Fade(207156, 207156 + 1000, 0, 0.5f);
            borderG.Scale(OsbEasing.OutCubic, 207156, 207156 + 250, 0, size + 5f);
            borderG.Fade(219834, 0);
            borderG.Additive(207156);
            borderG.Color(207156, new Color4(0, 255, 0, 1));

            var borderB = background.CreateSprite("sb/white.png", OsbOrigin.Centre, position - new Vector2(-1f, -0.5f));
            borderB.Fade(207156, 207156 + 1000, 0, 0.5f);
            borderB.Scale(OsbEasing.OutCubic, 207156, 207156 + 250, 0, size + 5f);
            borderB.Fade(219834, 0);
            borderB.Additive(207156);
            borderB.Color(207156, new Color4(0, 0, 255, 1));


            if (filled == false)
            {
                var bg = background.CreateSprite("sb/white.png", OsbOrigin.Centre, position);
                bg.Fade(207156, 207156 + 1000, 0, 1f);
                bg.Color(207156, new Color4(0, 0, 0, 1));
                bg.Scale(OsbEasing.OutCubic, 207156, 207156 + 250, 0, size);
                bg.Fade(219834, 0);
            }
        }

        public double DrawLine(Vector2 start, Vector2 end, double animateStart, double duration, OsbEasing easing = OsbEasing.None)
        {
            var width = 2f;
            var lineR = background.CreateSprite("sb/white.png", OsbOrigin.CentreLeft, start);
            var lineG = background.CreateSprite("sb/white.png", OsbOrigin.CentreLeft, start);
            var lineB = background.CreateSprite("sb/white.png", OsbOrigin.CentreLeft, start);

            var length = (end - start).Length;
            var durationPerPixel = length / 100 * duration;

            var animateEnd = animateStart + durationPerPixel;

            if (animateStart == 207156)
            {
                lineR.Fade(207156, 207156 + 500, 0, 0.5f);
                lineG.Fade(207156, 207156 + 500, 0, 0.5f);
                lineB.Fade(207156, 207156 + 500, 0, 0.5f);
            }
            else
            {
                lineR.Fade(animateStart, 0.4f);
                lineG.Fade(animateStart, 0.4f);
                lineB.Fade(animateStart, 0.4f);
            }

            start += new Vector2(-0.5f, -0.5f);
            end += new Vector2(-0.5f, -0.5f);

            lineR.Fade(219834, 0);
            lineR.Additive(207156);
            lineR.Color(207156, new Color4(255, 0, 0, 1));
            lineR.ScaleVec(easing, animateStart, animateEnd, new Vector2(0, width), new Vector2((end - start).Length, width));
            lineR.Rotate(animateStart, Math.Atan2(end.Y - start.Y, end.X - start.X));
            lineR.Move(animateStart, start);

            start -= new Vector2(-0.5f, -0.5f);
            end -= new Vector2(-0.5f, -0.5f);

            lineG.Fade(219834, 0);
            lineG.Additive(207156);
            lineG.Color(207156, new Color4(0, 255, 0, 1));
            lineG.ScaleVec(easing, animateStart, animateEnd, new Vector2(0, width), new Vector2((end - start).Length, width));
            lineG.Rotate(animateStart, Math.Atan2(end.Y - start.Y, end.X - start.X));
            lineG.Move(animateStart, start);

            start -= new Vector2(-0.5f, -0.5f);
            end -= new Vector2(-0.5f, -0.5f);

            lineB.Fade(219834, 0);
            lineB.Additive(207156);
            lineB.Color(207156, new Color4(0, 0, 255, 1));
            lineB.ScaleVec(easing, animateStart, animateEnd, new Vector2(0, width), new Vector2((end - start).Length, width));
            lineB.Rotate(animateStart, Math.Atan2(end.Y - start.Y, end.X - start.X));
            lineB.Move(animateStart, start);

            return animateEnd;
        }

        public void WindoowsXP()
        {

            var loading = background.CreateSprite("sb/windows/loading.png", OsbOrigin.Centre, new Vector2(320, 358));

            var windowsBit = GetMapsetBitmap("sb/windows/windowsxp.png");
            var windows = background.CreateSprite("sb/windows/windowsxp.png");
            windows.Fade(219929, 1);
            windows.ScaleVec(219929, 854f / windowsBit.Width, 480f / windowsBit.Height);
            windows.Fade(226729, 0);

            loading.Fade(219929, 1);
            loading.Fade(226729, 0);

            var mult = 0.9f;

            loading.StartLoopGroup(219929, 3);
            loading.MoveX(0, 230);
            loading.MoveX(100 / mult, 240);
            loading.MoveX(200 / mult, 250);
            loading.MoveX(300 / mult, 260);
            loading.MoveX(400 / mult, 270);
            loading.MoveX(500 / mult, 280);
            loading.MoveX(600 / mult, 290);
            loading.MoveX(700 / mult, 300);
            loading.MoveX(800 / mult, 310);
            loading.MoveX(900 / mult, 320);
            loading.MoveX(1000 / mult, 330);
            loading.MoveX(1100 / mult, 340);
            loading.MoveX(1200 / mult, 350);
            loading.MoveX(1300 / mult, 360);
            loading.MoveX(1400 / mult, 370);
            loading.MoveX(1500 / mult, 380);
            loading.MoveX(1600 / mult, 390);
            loading.MoveX(1700 / mult, 400);
            loading.MoveX(1800 / mult, 410);
            loading.MoveX(1900 / mult, 420);
            loading.EndGroup();

            var desktopBit = GetMapsetBitmap("sb/windows/desktop.png");
            var desktop = background.CreateSprite("sb/windows/desktop.png", OsbOrigin.Centre, new Vector2(320, 240));
            desktop.ScaleVec(226732, 854f / desktopBit.Width, 480f / desktopBit.Height);
            desktop.Fade(226732, 1);
            desktop.Fade(234229, 0);

            var cmd = background.CreateSprite("sb/windows/cmd.png", OsbOrigin.Centre, new Vector2(62, 220));
            cmd.Fade(226732, 1);
            cmd.Scale(226732, .3f);
            cmd.Fade(234229, 0);

            var bottomCover = background.CreateSprite("sb/white.png", OsbOrigin.BottomCentre, new Vector2(95, 335));
            var rightCover = background.CreateSprite("sb/white.png", OsbOrigin.CentreRight, new Vector2(280, 112));

            var lineheight = 8f;
            bottomCover.Fade(226732, 1);
            bottomCover.Color(226732, new Color4(0, 0, 0, 1));
            bottomCover.ScaleVec(226732, 360, 215);
            bottomCover.Fade(234229, 0);

            rightCover.Fade(226732, 1);
            rightCover.Color(226732, new Color4(0, 0, 0, 1));
            rightCover.ScaleVec(226732, 272, 8);
            rightCover.Fade(234229, 0);

            /*var writer = background.CreateSprite("sb/white.png", OsbOrigin.CentreLeft, new Vector2(280 - 271, 112));
            writer.Fade(226732, 1f);
            writer.ScaleVec(226732, 4, 8);
            writer.StartLoopGroup(226732, 50);
            writer.Color(OsbEasing.OutCubic, 0, 500, new Color4(255, 255, 255, 1), new Color4(0, 0, 0, 1));
            writer.Color(OsbEasing.InCubic, 500, 1000, new Color4(0, 0, 0, 1), new Color4(255, 255, 255, 1));
            writer.EndGroup();
            writer.Fade(234229, 0);

            var writerStart = 226729;
            while ()*/

            rightCover.ScaleVec(227198, 0, 8);

            bottomCover.ScaleVec(227433, (Vector2)bottomCover.ScaleAt(227433) - new Vector2(0, 14));
            bottomCover.ScaleVec(227784, (Vector2)bottomCover.ScaleAt(227784) - new Vector2(0, 18));
            bottomCover.ScaleVec(228136, (Vector2)bottomCover.ScaleAt(228136) - new Vector2(0, 16));
            bottomCover.ScaleVec(228370, (Vector2)bottomCover.ScaleAt(228370) - new Vector2(0, 8));
            bottomCover.ScaleVec(228604, (Vector2)bottomCover.ScaleAt(228604) - new Vector2(0, 9));
            bottomCover.ScaleVec(228839, (Vector2)bottomCover.ScaleAt(228839) - new Vector2(0, 9));
            bottomCover.ScaleVec(229073, (Vector2)bottomCover.ScaleAt(229073) - new Vector2(0, 8));
            bottomCover.ScaleVec(229542, (Vector2)bottomCover.ScaleAt(229542) - new Vector2(0, 17));

            rightCover.ScaleVec(229542, 305, 8);
            rightCover.MoveY(229542, rightCover.PositionAt(229542).Y + 105);
            rightCover.ScaleVec(230011, 0, 8);

            bottomCover.ScaleVec(230479, (Vector2)bottomCover.ScaleAt(230479) - new Vector2(0, 18));
            bottomCover.ScaleVec(230714, (Vector2)bottomCover.ScaleAt(230714) - new Vector2(0, 9));
            bottomCover.ScaleVec(230948, (Vector2)bottomCover.ScaleAt(230948) - new Vector2(0, 8));
            bottomCover.ScaleVec(231183, (Vector2)bottomCover.ScaleAt(231183) - new Vector2(0, 9));

            bottomCover.ScaleVec(231417, (Vector2)bottomCover.ScaleAt(231417) - new Vector2(0, 18));

            rightCover.ScaleVec(231417, 305, 8);
            rightCover.MoveY(231417, rightCover.PositionAt(231417).Y + 60);
            rightCover.ScaleVec(232120, 0, 8);

            bottomCover.ScaleVec(232354, (Vector2)bottomCover.ScaleAt(232354) - new Vector2(0, 17));
            bottomCover.ScaleVec(232823, (Vector2)bottomCover.ScaleAt(232823) - new Vector2(0, 8));
            bottomCover.ScaleVec(233058, (Vector2)bottomCover.ScaleAt(233058) - new Vector2(0, 8));
            bottomCover.ScaleVec(233761, (Vector2)bottomCover.ScaleAt(233761) - new Vector2(0, 20));

            var tunnelBit = GetMapsetBitmap("sb/windows/windowSquare.png");
            var tunnel = background.CreateSprite("sb/windows/windowSquare.png", OsbOrigin.Centre, new Vector2(540, 280));
            tunnel.Fade(231417, 1);
            tunnel.ScaleVec(231417, .3f, .3f);
            tunnel.Fade(234229, 0);

            tunnel.ScaleVec(OsbEasing.InExpo, 233526, 234229, new Vector2(.3f), new Vector2(854f / tunnelBit.Width, 480f / tunnelBit.Height));
            tunnel.Move(OsbEasing.InCubic, 233526, 234229, new Vector2(540, 280), new Vector2(320, 240));

            var atlasLogo = background.CreateAnimation("sb/ani/sphere/frame.jpg", 24, 1000 / 24, OsbLoopType.LoopForever, OsbOrigin.Centre, new Vector2(540, 250));
            atlasLogo.Fade(231417, .8f);
            atlasLogo.Scale(231417, .1f);
            atlasLogo.Fade(232120, 0);

            var atlastCorp = background.CreateSprite("sb/atlas/atlas.png", OsbOrigin.Centre, new Vector2(540, 290));
            atlastCorp.Fade(231417, .8f);
            atlastCorp.Scale(231417, .15f);
            atlastCorp.Fade(232120, 0);

            var gifBit = GetMapsetBitmap("sb/ani/tunnel/frame0.jpg");
            var gif = background.CreateAnimation("sb/ani/tunnel/frame.jpg", 12, 200 / 12, OsbLoopType.LoopForever, OsbOrigin.Centre, new Vector2(540, 295));
            gif.Fade(232120, 1);
            gif.ScaleVec(OsbEasing.OutCubic, 232120, 232120 + 500, new Vector2(0f), new Vector2(.159f, .155f));
            gif.Fade(234229, 0);

            gif.ScaleVec(OsbEasing.InExpo, 233526, 234229, new Vector2(.159f, .155f), new Vector2((854f - 10f) / gifBit.Width, (480f - 22f) / gifBit.Height));
            gif.Move(OsbEasing.InCubic, 233526, 234229, new Vector2(540, 285), new Vector2(320, 240));

            var windowWideBit = GetMapsetBitmap("sb/windows/windowWide.png");
            var windowWide = bgOverLay.CreateSprite("sb/windows/windowWide.png", OsbOrigin.Centre, new Vector2(320, 240));
            windowWide.Fade(234229, 1);
            windowWide.Fade(264230, 0);
            windowWide.ScaleVec(234229, 854f / windowWideBit.Width, 480f / windowWideBit.Height);

            var darken = background.CreateSprite("sb/white.png");
            darken.Scale(226732, 1000);
            darken.Color(226732, new Color4(0, 0, 0, 1));
            darken.Fade(OsbEasing.OutCubic, 226732, 226732 + 1000, 1, 0.25f);
            darken.Fade(234229, 0);

        }
    }
}
