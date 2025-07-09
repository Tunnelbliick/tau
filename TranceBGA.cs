using OpenTK;
using OpenTK.Graphics;
using OpenTK.Input;
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
    public class TranceBGA : StoryboardObjectGenerator
    {
        Color4 w = new Color4(255, 255, 255, 255);
        Color4 o = new Color4(236, 99, 74, 255);
        Color4 y = new Color4(237, 199, 112, 255);
        Color4 c = new Color4(94, 196, 192, 255);

        public override void Generate()
        {
            Color4[] colors = new Color4[]
            {
                w, o, y, c,
            };

            double start = 91414;
            double end = 117737;
            double interval = Beatmap.GetTimingPointAt((int)start).BeatDuration;
            double LogScale = 1;
            double SpriteScale = 2000;

            float width = 10;
            float gap = 5;

            var originPos = new Vector2(-50, 425);

            double animDuration = interval * 8; // 8 beats per cycle
            var t = 0;

            var maxBars = 40;
            var maxRunTime = interval * (maxBars - 1);
            var localStart = start - maxRunTime;
            var priorHeight = 0;

            Scene3d scene = new();
            var camera = new PerspectiveCamera();

            // Camera setup
            camera.VerticalFov.Add(92575, 65);
            camera.HorizontalFov.Add(92575, 65);
            camera.FarFade.Add(92575, 2500);
            camera.FarClip.Add(92575, 2500);
            camera.TargetPosition.Add(92575, new Vector3(0, 0, 0));
            camera.PositionX.Add(92575, 0);
            camera.PositionZ.Add(92575, -670);
            camera.PositionY.Add(92575, 0);

            Node3d objects = new();


            var atlasR = GetLayer("bga").CreateAnimation("sb/ani/sphere/frame.jpg", 24, 800 / 24, OsbLoopType.LoopForever, OsbOrigin.Centre, new Vector2(320 - 2, 180));
            atlasR.Scale(start - 800 / 24, 0.2f);
            atlasR.Color(start - 800 / 24, new Color4(255, 0, 0, 255));
            atlasR.Additive(start - 800 / 24);
            atlasR.Rotate(start, 0.1f);
            atlasR.Fade(92769 - 800, 93543 - 800, 0, 0.2f);
            atlasR.Fade(OsbEasing.OutCubic, 117737, 118124, 0.2f, 0f);

            atlasR.StartLoopGroup(start, (int)(end - start) / (int)animDuration);
            atlasR.MoveX(OsbEasing.InOutSine, t, t + animDuration / 2, 320 - 2, 320 - 4);
            atlasR.MoveX(OsbEasing.InOutSine, t + animDuration / 2, t + animDuration, 320 - 4, 320 - 2);
            atlasR.MoveY(OsbEasing.InOutSine, t, t + animDuration / 2, 180, 179);
            atlasR.MoveY(OsbEasing.InOutSine, t + animDuration / 2, t + animDuration, 179, 180);
            atlasR.EndGroup();


            var atlasG = GetLayer("bga").CreateAnimation("sb/ani/sphere/frame.jpg", 24, 800 / 24, OsbLoopType.LoopForever, OsbOrigin.Centre, new Vector2(320, 180));
            atlasG.Scale(start, 0.2f);
            atlasG.Rotate(start, 0.1f);
            atlasG.Color(start, new Color4(0, 255, 0, 255));
            atlasG.Additive(start);
            atlasG.Fade(92769, 93543, 0, 0.2f);
            atlasG.Fade(OsbEasing.OutCubic, 117737, 118124, 0.2f, 0f);

            var atlasB = GetLayer("bga").CreateAnimation("sb/ani/sphere/frame.jpg", 24, 800 / 24, OsbLoopType.LoopForever, OsbOrigin.Centre, new Vector2(320 + 2, 180));
            atlasB.Scale(start + 800 / 24, 0.2f);
            atlasB.Rotate(start + 800 / 24, 0.1f);
            atlasB.Color(start + 800 / 24, new Color4(0, 0, 255, 255));
            atlasB.Additive(start + 800 / 24);
            atlasB.Fade(92769 + 800, 93543 + 800, 0, 0.2f);
            atlasB.Fade(OsbEasing.OutCubic, 117737, 118124, 0.2f, 0f);

            atlasB.StartLoopGroup(start, (int)(end - start) / (int)animDuration);
            atlasB.MoveX(OsbEasing.InOutSine, t, t + animDuration / 2, 320 + 2, 320 + 4);
            atlasB.MoveX(OsbEasing.InOutSine, t + animDuration / 2, t + animDuration, 320 + 4, 320 + 2);
            atlasB.MoveY(OsbEasing.InOutSine, t, t + animDuration / 2, 180, 181);
            atlasB.MoveY(OsbEasing.InOutSine, t + animDuration / 2, t + animDuration, 181, 180);
            atlasB.EndGroup();


            var atlasTextR = GetLayer("bga").CreateSprite("sb/atlas/atlas.png", OsbOrigin.Centre, new Vector2(320 - 1, 300));
            atlasTextR.Scale(start, 0.35f);
            atlasTextR.Rotate(start, 0.01f);
            atlasTextR.Color(start, new Color4(255, 0, 0, 255));
            atlasTextR.Additive(start);
            atlasTextR.Fade(92769, 93543, 0, 0.2f);
            atlasTextR.Fade(OsbEasing.OutCubic, 117737, 118124, 0.2f, 0f);

            atlasTextR.StartLoopGroup(start, (int)(end - start) / (int)animDuration);
            atlasTextR.MoveX(OsbEasing.InOutSine, t, t + animDuration / 2, 320 - 1, 320 - 2.5f);
            atlasTextR.MoveX(OsbEasing.InOutSine, t + animDuration / 2, t + animDuration, 320 - 2.5f, 320 - 1);
            atlasTextR.EndGroup();

            var atlasTextG = GetLayer("bga").CreateSprite("sb/atlas/atlas.png", OsbOrigin.Centre, new Vector2(320, 300));
            atlasTextG.Scale(start, 0.35f);
            atlasTextG.Color(start, new Color4(0, 255, 0, 255));
            atlasTextG.Additive(start);
            atlasTextG.Fade(92769, 93543, 0, 0.2f);
            atlasTextG.Fade(OsbEasing.OutCubic, 117737, 118124, 0.2f, 0f);

            var atlasTextB = GetLayer("bga").CreateSprite("sb/atlas/atlas.png", OsbOrigin.Centre, new Vector2(320 + 0.5f, 300));
            atlasTextB.Scale(start, 0.35f);
            atlasTextB.Rotate(start, -0.01f);
            atlasTextB.Color(start, new Color4(0, 0, 255, 255));
            atlasTextB.Additive(start);
            atlasTextB.Fade(92769, 93543, 0, 0.2f);
            atlasTextB.Fade(OsbEasing.OutCubic, 117737, 118124, 0.2f, 0f);

            atlasTextB.StartLoopGroup(start, (int)(end - start) / (int)animDuration);
            atlasTextB.MoveX(OsbEasing.InOutSine, t, t + animDuration / 2, 320 + 1, 320 + 2.5f);
            atlasTextB.MoveX(OsbEasing.InOutSine, t + animDuration / 2, t + animDuration, 320 + 2.5f, 320 + 1);
            atlasTextB.EndGroup();

            Sprite3d backdrop = new Sprite3d
            {
                SpritePath = "sb/backdrop.png",
                Additive = true,
            };

            backdrop.ConfigureGenerators((s) =>
            {
                s.ScaleDecimals = 3;
                s.ScaleTolerance = 0.001f;
            });

            backdrop.SpriteScale.Add(92769, new Vector2(0.35f));
            backdrop.Opacity.Add(92769, 0);
            backdrop.Opacity.Add(93543, 0.25f);
            backdrop.Opacity.Add(117737, 0.25f);
            backdrop.Opacity.Add(118124, 0, EasingFunctions.CubicOut);

            objects.Add(backdrop);

            Sprite3d backDropText = new Sprite3d
            {
                SpritePath = "sb/text.png",
                Additive = true,
            };

            backDropText.ConfigureGenerators((s) =>
{
    s.ScaleDecimals = 3;
    s.ScaleTolerance = 0.001f;
});

            backDropText.SpriteScale.Add(92769, new Vector2(0.35f));
            backDropText.Opacity.Add(92769, 0);
            backDropText.Opacity.Add(93543, 1f);
            backDropText.Opacity.Add(117737, 1f);
            backDropText.Opacity.Add(118124, 0, EasingFunctions.CubicOut);

            objects.Add(backDropText);

            Sprite3d atlastGB = new Sprite3d
            {
                SpritePath = "sb/atlas/atlas.png",
                Additive = true,
            };

            atlastGB.ConfigureGenerators((s) =>
{
    s.ScaleDecimals = 3;
    s.ScaleTolerance = 0.001f;
});

            atlastGB.SpriteScale.Add(92769, new Vector2(0.22f));
            atlastGB.Opacity.Add(92769, 0);
            atlastGB.Opacity.Add(93543, 1f);
            atlastGB.Opacity.Add(117737, 1f);
            atlastGB.Opacity.Add(118124, 0, EasingFunctions.CubicOut);
            atlastGB.Coloring.Add(92769, new Color4(0, 255, 255, 255));
            atlastGB.PositionX.Add(92769, 205);
            atlastGB.PositionY.Add(92769, -240 + 86);

            objects.Add(atlastGB);

            Sprite3d atlastR = new Sprite3d
            {
                SpritePath = "sb/atlas/atlas.png",
                Additive = true,
            };

            atlastR.SpriteScale.Add(92769, new Vector2(0.22f));
            atlastR.Opacity.Add(92769, 0);
            atlastR.Opacity.Add(93543, 1f);
            atlastR.Opacity.Add(117737, 1f);
            atlastR.Opacity.Add(118124, 0, EasingFunctions.CubicOut);
            atlastR.Coloring.Add(92769, new Color4(255, 0, 0, 255));
            atlastR.PositionX.Add(92769, 206);
            atlastR.PositionY.Add(92769, -240 + 86);

            objects.Add(atlastGB);
            objects.Add(atlastR);

            atlastR.ConfigureGenerators((s) =>
{
    s.ScaleDecimals = 3;
    s.ScaleTolerance = 0.001f;
});

            Animation3d logoGB = new Animation3d
            {
                SpritePath = "sb/ani/sphere/frame.jpg",
                Additive = true,
                LoopType = OsbLoopType.LoopForever,
                FrameCount = 24,
                FrameDelay = 800 / 24,
                RotationMode = RotationMode.Fixed
            };

            logoGB.SpriteScale.Add(92769, 0.055f);
            logoGB.SpriteRotation.Add(92769, 0.15f);
            logoGB.Opacity.Add(92769, 0);
            logoGB.Opacity.Add(93543, 1f);
            logoGB.Opacity.Add(117737, 1f);
            logoGB.Opacity.Add(118124, 0, EasingFunctions.CubicOut);
            logoGB.Coloring.Add(92769, new Color4(0, 255, 255, 255));
            logoGB.PositionX.Add(92769, 320 + 40);
            logoGB.PositionY.Add(92769, -240 + 86);

            Animation3d logoR = new Animation3d
            {
                SpritePath = "sb/ani/sphere/frame.jpg",
                Additive = true,
                LoopType = OsbLoopType.LoopForever,
                FrameCount = 24,
                FrameDelay = 800 / 24,
                RotationMode = RotationMode.Fixed
            };

            logoR.SpriteScale.Add(92769, 0.055f);
            logoR.SpriteRotation.Add(92769, 0.15f);
            logoR.Opacity.Add(92769, 0);
            logoR.Opacity.Add(93543, 1f);
            logoR.Opacity.Add(117737, 1f);
            logoR.Opacity.Add(118124, 0, EasingFunctions.CubicOut);
            logoR.Coloring.Add(92769, new Color4(255, 0, 0, 255));
            logoR.PositionX.Add(92769, 320 + 41);
            logoR.PositionY.Add(92769, -240 + 86);

            objects.Add(logoGB);
            objects.Add(logoR);

            logoGB.ConfigureGenerators((s) =>
{
    s.ScaleDecimals = 3;
    s.ScaleTolerance = 0.001f;
});

            logoR.ConfigureGenerators((s) =>
{
    s.ScaleDecimals = 3;
    s.ScaleTolerance = 0.001f;
});
            Sprite3d xLine = new Sprite3d
            {
                SpritePath = "sb/white.png",
                SpriteOrigin = OsbOrigin.CentreLeft,
            };

            xLine.SpriteScale.Add(92963, new Vector2(0, 4));

            var xStartPos = originPos - new Vector2(10, 10) + new Vector2(10, 15);

            xLine.PositionX.Add(92963, xStartPos.X + 428);
            xLine.PositionY.Add(92963, 190);
            xLine.Opacity.Add(92963, 1);
            xLine.SpriteScale.Add(94124, new Vector2(610, 4), EasingFunctions.CubicOut);
            xLine.SpriteScale.Add(117737, new Vector2(610, 4));
            xLine.SpriteScale.Add(118124, new Vector2(0, 4), EasingFunctions.CubicOut);

            Sprite3d yLine = new Sprite3d
            {
                SpritePath = "sb/white.png",
                SpriteOrigin = OsbOrigin.BottomCentre,
            };

            yLine.SpriteScale.Add(92963, new Vector2(4, 0));
            var yStartPos = originPos - new Vector2(8f, 8f) + new Vector2(10, 15);
            yLine.PositionX.Add(92963, yStartPos.X + 424);
            yLine.PositionY.Add(92963, 190);
            yLine.Opacity.Add(92963, 1);
            yLine.SpriteScale.Add(94124, new Vector2(4, 200), EasingFunctions.CubicOut);
            yLine.SpriteScale.Add(117737, new Vector2(4, 200));
            yLine.SpriteScale.Add(118124, new Vector2(4, 0), EasingFunctions.CubicOut);

            objects.Add(yLine);
            objects.Add(xLine);

            while (localStart < end)
            {
                float[] vol = GetFft(localStart, 1, null, OsbEasing.None, 64000);
                float height = (float)(Math.Log10(1 + vol[0] * LogScale) * SpriteScale);

                Sprite3d sprite = new Sprite3d
                {
                    SpritePath = "sb/white.png",
                    SpriteOrigin = OsbOrigin.BottomCentre,
                };

                if (localStart < start)
                {
                    sprite.Opacity.Add(start - 1, 0);
                    sprite.Opacity.Add(start, 1);
                }
                else
                {
                    sprite.Opacity.Add(localStart - 1, 0);
                    sprite.Opacity.Add(localStart, 1);
                    sprite.SpriteScale.Add(localStart, new Vector2(width, 0));
                    sprite.SpriteScale.Add(localStart + 200, new Vector2(width, height), EasingFunctions.CubicOut);
                }

                if (localStart > 93737)
                {
                    sprite.SpriteScale.Add(localStart, new Vector2(width, 0));
                    sprite.SpriteScale.Add(localStart + 200, new Vector2(width, height), EasingFunctions.CubicOut);
                }
                else
                {
                    sprite.SpriteScale.Add(92866, new Vector2(width, 0));
                    sprite.SpriteScale.Add(93737, new Vector2(width, height), EasingFunctions.CubicOut);
                }

                if (localStart > 102930)
                {
                    sprite.SpriteScale.Add(117737, new Vector2(width, height));
                    sprite.SpriteScale.Add(118124, new Vector2(width, 0), EasingFunctions.CubicOut);
                }

                sprite.Coloring.Add(localStart, colors[Random(0, colors.Length)]);

                sprite.Opacity.Add(Math.Min(localStart + maxRunTime, 118124) - 1, 1);
                sprite.Opacity.Add(Math.Min(localStart + maxRunTime, 118124), 0);

                sprite.SpriteScale.Add(Math.Min(localStart + maxRunTime, 118124) - 250, new Vector2(width, height));
                sprite.SpriteScale.Add(Math.Min(localStart + maxRunTime, 118124), new Vector2(width, 0), EasingFunctions.CubicOut);

                sprite.PositionY.Add(localStart, 185);

                var local = localStart;
                var currentPosX = 365f;
                for (int i = 1; i < maxBars; i++)
                {
                    if (local > end)
                        break;
                    sprite.PositionX.Add(local, currentPosX);
                    sprite.PositionX.Add(local + interval - 0.1f, currentPosX);
                    currentPosX -= width + gap;
                    local += interval;
                }

                localStart += interval;

                objects.Add(sprite);
            }

            Dictionary<int, KeyframedValue<float>> keyframes = new Dictionary<int, KeyframedValue<float>>();
            localStart = 92963;
            LogScale = 100;
            SpriteScale = 150;
            while (localStart < end)
            {
                float[] volumes = GetFft(localStart, 4, null, OsbEasing.None, 0);

                for (int i = 0; i < 4; i++)
                {
                    KeyframedValue<float> height = keyframes.GetValueOrDefault(i, new KeyframedValue<float>(null));
                    height.Add(localStart, (float)(Math.Log10(1 + volumes[i] * LogScale) * SpriteScale));
                    keyframes[i] = height;
                }

                localStart += interval / 6;
            }

            var xStart = -275f;
            var intervalX = 30;
            int col = 0;

            // Define how many milliseconds each sprite should live for
            double spriteLifetime = Beatmap.GetControlPointAt(92963).BeatDuration * 8; // 10 seconds per sprite

            foreach (var keyframe in keyframes.Values)
            {
                keyframe.Simplify(2f);

                // Split the entire time range into chunks
                double timeStart = 92963;
                double timeEnd = 118124;

                // Create multiple sprites to cover the entire duration
                while (timeStart < timeEnd)
                {
                    // Calculate the end time for this particular sprite
                    double spriteEndTime = Math.Min(timeStart + spriteLifetime, timeEnd);

                    Sprite3d vol = new Sprite3d
                    {
                        SpritePath = "sb/white.png",
                        SpriteOrigin = OsbOrigin.BottomCentre,
                    };

                    vol.Coloring.Add(timeStart, colors[Math.Min(col, colors.Length - 1)]);
                    vol.PositionX.Add(timeStart, xStart);
                    vol.PositionY.Add(timeStart, 185);

                    // Fade in the sprite (except for the first one)
                    if (timeStart > 92963)
                    {
                        vol.Opacity.Add(timeStart - 50, 0);
                        vol.Opacity.Add(timeStart, 1f);
                    }
                    else
                    {
                        vol.Opacity.Add(92963, 1f);
                    }

                    // If this is the last sprite segment, handle the final fade out
                    if (spriteEndTime >= 117737)
                    {
                        vol.Opacity.Add(117737, 1f);
                        vol.Opacity.Add(118124, 0, EasingFunctions.CubicOut);
                    }
                    else
                    {
                        // Otherwise fade out at the end of this sprite's lifetime
                        vol.Opacity.Add(spriteEndTime, 1f);
                        vol.Opacity.Add(spriteEndTime + 1, 0);
                    }

                    // Only add keyframes that fall within this sprite's lifetime
                    keyframe.ForEachPair(
                        (start, end) =>
                        {
                            // Only process keyframe pairs that overlap with this sprite's lifetime
                            if (end.Time >= timeStart && start.Time <= spriteEndTime)
                            {
                                // Calculate the actual start and end times for this segment
                                double segmentStart = Math.Max(start.Time, timeStart);
                                double segmentEnd = Math.Min(end.Time, spriteEndTime);

                                // If the keyframe starts before this sprite's lifetime,
                                // interpolate the start value
                                float startValue = start.Value;
                                if (start.Time < timeStart)
                                {
                                    float ratio = (float)((timeStart - start.Time) / (end.Time - start.Time));
                                    startValue = start.Value + (ratio * (end.Value - start.Value));
                                }

                                // If the keyframe ends after this sprite's lifetime,
                                // interpolate the end value
                                float endValue = end.Value;
                                if (end.Time > spriteEndTime)
                                {
                                    float ratio = (float)((spriteEndTime - start.Time) / (end.Time - start.Time));
                                    endValue = start.Value + (ratio * (end.Value - start.Value));
                                }

                                vol.SpriteScale.Add(segmentStart, new Vector2(25, startValue));
                                vol.SpriteScale.Add(segmentEnd, new Vector2(25, endValue));
                            }
                        }
                    );

                    // For the last sprite segment, handle the final animation
                    if (spriteEndTime >= 117737)
                    {
                        var lastHeight = keyframe.Last().Value;
                        vol.SpriteScale.Add(117737, new Vector2(25, lastHeight));
                        vol.SpriteScale.Add(118124, new Vector2(25, 0), EasingFunctions.CubicOut);
                    }

                    objects.Add(vol);
                    timeStart = spriteEndTime;
                }

                xStart -= intervalX;
                col++;
            }



            localStart = start;
            while (localStart < end - 1000)
            {
                SpawnPacket(objects, new Vector2(-20, -105), 75, localStart);
                localStart += Random(200, 500);
            }

            localStart = start;
            while (localStart < end - 1000)
            {
                SpawnPacket(objects, new Vector2(-100, -91.8f), -75, localStart);
                localStart += Random(200, 500);
            }

            localStart = start;
            while (localStart < end - 1000)
            {
                SpawnPacket(objects, new Vector2(-190, -106), 75, localStart);
                localStart += Random(400, 600);
            }

            localStart = start;
            while (localStart < end - 1000)
            {
                SpawnPacket(objects, new Vector2(-265, -93.2f), -75, localStart);
                localStart += Random(400, 600);
            }


            /*objects.children.ForEach(c =>
            {
                c.ConfigureGenerators((s) =>
            {
                s.ScaleDecimals = 2;
                s.ScaleTolerance = 0.1f;
            });
            });*/


            objects.PositionZ.Add(92963 - 1, 0);
            /*objects.Opacity.Add(92769, 0);
            objects.Opacity.Add(93543, 1f);
            objects.Opacity.Add(117737, 1f);
            objects.Opacity.Add(118124, 0, EasingFunctions.CubicOut);*/

            var offsetTime = Random(100, 1000);

            SpanwPing(objects, new Vector2(225, 62), "low", Random(100, 1000));
            SpanwPing(objects, new Vector2(225, 117), "low", Random(100, 1000));
            SpanwPing(objects, new Vector2(65, 62), "low", Random(100, 1000));
            SpanwPing(objects, new Vector2(65, 117), "low", Random(100, 1000));
            SpanwPing(objects, new Vector2(-333, 45), "medium", offsetTime, 0.2f);
            SpanwPing(objects, new Vector2(-227, 45), "avg", offsetTime, 0.2f);
            SpanwPing(objects, new Vector2(-122, 45), "delta", offsetTime, 0.2f);

            Animation3d beatcycle = new Animation3d
            {
                SpritePath = "sb/numbers/cycle/frame.png",
                LoopType = OsbLoopType.LoopForever,
                FrameCount = 4,
                FrameDelay = Beatmap.GetTimingPointAt((int)start).BeatDuration,
                RotationMode = RotationMode.Fixed,
                Additive = true,
            };

            beatcycle.SpriteScale.Add(92963, 0.15f);
            beatcycle.PositionX.Add(92963, 195);
            beatcycle.PositionY.Add(92963, -92);
            beatcycle.Opacity.Add(92963, 0);
            beatcycle.Opacity.Add(93543, 1f);
            beatcycle.Opacity.Add(117737, 1f);
            beatcycle.Opacity.Add(118124, 0, EasingFunctions.CubicOut);
            beatcycle.ConfigureGenerators((s) =>
{
    s.ScaleDecimals = 3;
    s.ScaleTolerance = 0.01f;
    s.PositionDecimals = 3;
    s.PositionTolerance = 0.001f;
});

            objects.Add(beatcycle);

            var zDepth = keyframes[0];

            zDepth.ForEachPair(
                (start, end) =>
                {
                    // Add the keyframes to the objects
                    objects.PositionZ.Add(start.Time, 50 - start.Value / 2);
                    objects.PositionZ.Add(end.Time, 50 - end.Value / 2);
                }
            );

            localStart = start;
            animDuration = 2500;
            var radius = 20;

            var curX = 0;
            var curY = -30;

            while (localStart < end)
            {
                curX -= radius;
                curY += radius;
                objects.PositionX.Add(localStart, curX, EasingFunctions.SineOut);
                objects.PositionY.Add(localStart, curY, EasingFunctions.SineIn);
                curX += radius;
                curY += radius;
                localStart += animDuration;
                objects.PositionX.Add(localStart, curX, EasingFunctions.SineIn);
                objects.PositionY.Add(localStart, curY, EasingFunctions.SineOut);
                curX += radius;
                curY -= radius;
                localStart += animDuration;
                objects.PositionX.Add(localStart, curX, EasingFunctions.SineOut);
                objects.PositionY.Add(localStart, curY, EasingFunctions.SineIn);
                curX -= radius;
                curY -= radius;
                localStart += animDuration;
                objects.PositionX.Add(localStart, curX, EasingFunctions.SineIn);
                objects.PositionY.Add(localStart, curY, EasingFunctions.SineOut);
                localStart += animDuration;
            }

            scene.Add(objects);

            scene.Generate(camera, GetLayer("bga"), start, 118124, Beatmap, 4);

            var overlay = GetLayer("bga").CreateSprite("sb/white.png", OsbOrigin.Centre, new Vector2(320, 240));
            overlay.ScaleVec(start, new Vector2(1000, 480));
            overlay.Fade(start, 1);
            overlay.Fade(OsbEasing.OutCubic, 92963, 93350, 1f, 0.75f);
            overlay.Fade(118124, 0);
            overlay.Color(start, new Color4(0, 0, 0, 255));

            var whatIReallyWant = GetLayer("overlay").CreateAnimation("sb/ani/want/frame.png", 48, 1500 / 48, OsbLoopType.LoopOnce, OsbOrigin.Centre, new Vector2(320, 240));
            whatIReallyWant.Fade(91414, 1);
            whatIReallyWant.Color(91414, new Color4(255, 0, 255, 255));
            whatIReallyWant.Scale(91414, 0.75f);
            whatIReallyWant.Fade(OsbEasing.OutCubic, 92963, 93156, 1, 0f);
            whatIReallyWant.Scale(OsbEasing.OutCubic, 92963, 93156, 0.5f, 1f);

            var whatIReallyWantG = GetLayer("overlay").CreateAnimation("sb/ani/want/frame.png", 48, 1500 / 48, OsbLoopType.LoopOnce, OsbOrigin.Centre, new Vector2(320, 240));
            whatIReallyWantG.Fade(91414, 1);
            whatIReallyWantG.Color(91414, new Color4(255, 255, 0, 255));
            //whatIReallyWantG.Additive(91414);
            whatIReallyWantG.Scale(91414, 0.75f);
            whatIReallyWantG.Fade(OsbEasing.OutCubic, 92963, 93156, 1, 0f);
            whatIReallyWantG.Move(OsbEasing.InSine, 91414, 92963, new Vector2(320, 240), new Vector2(320 + 6, 240 - 1));
            whatIReallyWantG.Scale(OsbEasing.OutCubic, 92963, 93156, 0.5f, 1f);

        }

        private void SpawnPacket(Node3d node, Vector2 start, float movement, double startTime)
        {
            Color4[] colors = new Color4[]
{
                w, o, y, c,
};

            var dur = 1000;

            Sprite3d packet = new Sprite3d
            {
                SpritePath = "sb/white.png",
                SpriteOrigin = OsbOrigin.Centre,
            };

            packet.SpriteScale.Add(startTime, new Vector2(7, 7));
            packet.Opacity.Add(startTime, 0f);
            packet.Opacity.Add(startTime + 250, 1f, EasingFunctions.CubicOut);
            packet.Opacity.Add(startTime + dur - 250, 1f);
            packet.Opacity.Add(startTime + dur, 0f, EasingFunctions.CubicOut);

            packet.PositionX.Add(startTime, start.X);
            packet.PositionX.Add(startTime + dur, start.X - movement);
            packet.PositionY.Add(startTime, start.Y);
            packet.Coloring.Add(startTime, colors[Random(0, colors.Length)]);

            node.Add(packet);
        }

        private void SpanwPing(Node3d node, Vector2 offset, string type, double time = 0, float scale = 0.18f)
        {

            Animation3d ping = new Animation3d
            {
                SpritePath = $"sb/numbers/{type}/frame.png",
                LoopType = OsbLoopType.LoopForever,
                FrameCount = 10,
                FrameDelay = 250,
                RotationMode = RotationMode.Fixed
            };

            ping.SpriteScale.Add(92963, scale);
            ping.PositionX.Add(92963, -offset.X);
            ping.PositionY.Add(92963, -offset.Y);
            ping.PositionZ.Add(92963, 0);

            ping.Opacity.Add(92769 - time - 1, 0);
            ping.Opacity.Add(92769 - time, 1);
            // ping.Opacity.Add(92769, 0);
            ping.Opacity.Add(93543, 1f);
            ping.Opacity.Add(117737, 1f);
            ping.Opacity.Add(118124, 0, EasingFunctions.CubicOut);

            ping.ConfigureGenerators((s) =>
 {
     s.ScaleDecimals = 3;
     s.ScaleTolerance = 0.01f;
     s.PositionDecimals = 3;
     s.PositionTolerance = 0.001f;
 });

            node.Add(ping);

        }

    }
}
