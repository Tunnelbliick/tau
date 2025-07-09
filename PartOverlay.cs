using OpenTK;
using OpenTK.Graphics;
using storyboard.scriptslibrary;
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
    public class PartOverlay : StoryboardObjectGenerator
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

            Writer(4704, 7801, "Part 1\nThe Intro", new Vector2(-85, 400), 0.5f);
            Writer(17866, 22511, "Part 2\nSea of Numbers", new Vector2(-85, 400), 0.5f);
            Writer(29479, 34124, "Part 3\nEvil Cylinder", new Vector2(-85, 400), 0.5f);
            Writer(43414, 47285, "Part 4\nIm Confused", new Vector2(-85, 400), 0.5f);
            Writer(53866, 57350, "Part 5\nAre you Ready?", new Vector2(-85, 400), 0.5f);
            Writer(67027, 71285, "Part 6\nPROXIES :)", new Vector2(-85, 400), 0.5f);
            Writer(80575, 85221, "Part 7\nRETURN of the DPAD", new Vector2(-85, 400), 0.5f);
            Writer(92575, 97221, "Part 8\nWhat you really want", new Vector2(-85, 400), 0.5f);
            Writer(117737, 121221, "Part 9\nYour Curiosity", new Vector2(-85, 400), 0.5f);
            Writer(130124, 133995, "Part 10\nWere so back", new Vector2(-85, 400), 0.5f);
            Writer(156446, 160317, "Part 11\nBack 2 Roots", new Vector2(-85, 400), 0.5f);
            Writer(169995, 173866, "Part 12\nStuck in Nodes", new Vector2(-85, 400), 0.5f);
            Writer(181221, 185866, "Part 13\nGROVECOASTER!!!", new Vector2(-85, 400), 0.5f);
            Writer(207156, 210640, "Part 14\nReboot post Crash", new Vector2(-85, 400), 0.5f);
            Writer(219930, 224908, "Part 15\nWindows?", new Vector2(-85, 400), 0.5f);
            Writer(234229, 238917, "Part 16\nWhat the F***", new Vector2(-85, 400), 0.5f);
            Writer(249229, 252979, "Part 17\nMy Eyes hurt", new Vector2(-85, 400), 0.5f);
            Writer(265167, 269855, "Part 18\nThe Final", new Vector2(-85, 400), 0.5f);

            GenerateConnection(15543, 17479, 17866, 500, 500);
            GenerateConnection(49608, 54253, 55027, 1500, 500);
            GenerateConnection(80188, 91027, 92963, 500, 1500);
            GenerateConnection(119285, 125479, 130124, 1000, 1500);
            GenerateConnection(169995, 176575, 181221, 1000, 500);

            CreateWarningAndMine(17866, 31027);
            CreateWarningAndMine(55414, 67414);


        }

        public void GenerateConnection(double startTime, double connectedTime, double fadeOutTime, double fadeDuration, double fadeInTime)
        {
            var layer = GetLayer("part_bga");

            var backgroundLogo = layer.CreateSprite("sb/white.png", OsbOrigin.Centre, new Vector2(0, 28) + new Vector2(0, 10));
            backgroundLogo.Fade(startTime, 1);
            backgroundLogo.Fade(fadeOutTime + fadeDuration, 0);
            backgroundLogo.Color(startTime, new Color4(0, 0, 0, 255));
            backgroundLogo.ScaleVec(startTime, 205, 40);
            backgroundLogo.MoveX(OsbEasing.OutCubic, startTime, startTime + fadeInTime, -210, 0);
            backgroundLogo.MoveX(OsbEasing.OutCubic, fadeOutTime, fadeOutTime + fadeDuration, 0, -210);

            var logoR = layer.CreateAnimation("sb/ani/sphere/frame.jpg", 24, 1000 / 48, OsbLoopType.LoopForever, OsbOrigin.Centre, new Vector2(-84, 28) + new Vector2(0, 9.5f));
            logoR.Fade(startTime, 1);
            logoR.Fade(fadeOutTime + fadeDuration, 0);
            logoR.Rotate(startTime, 0.2f);
            logoR.Additive(startTime);
            logoR.Scale(startTime, .037f);
            logoR.Color(startTime, new Color4(255, 0, 0, 255));
            logoR.MoveX(OsbEasing.OutCubic, startTime, startTime + fadeInTime, -83 - 210, -83);
            logoR.MoveX(OsbEasing.OutCubic, fadeOutTime, fadeOutTime + fadeDuration, -83, -83 - 210);

            var logoG = layer.CreateAnimation("sb/ani/sphere/frame.jpg", 24, 1000 / 48, OsbLoopType.LoopForever, OsbOrigin.Centre, new Vector2(-84, 28) + new Vector2(0, 10));
            logoG.Fade(startTime, 1);
            logoG.Fade(fadeOutTime + fadeDuration, 0);
            logoG.Rotate(startTime, 0.2f);
            logoG.Additive(startTime);
            logoG.Color(startTime, new Color4(0, 255, 0, 255));
            logoG.Scale(startTime, .037f);
            logoG.MoveX(OsbEasing.OutCubic, startTime, startTime + fadeInTime, -84 - 210, -84);
            logoG.MoveX(OsbEasing.OutCubic, fadeOutTime, fadeOutTime + fadeDuration, -84, -84 - 210);

            var logoB = layer.CreateAnimation("sb/ani/sphere/frame.jpg", 24, 1000 / 48, OsbLoopType.LoopForever, OsbOrigin.Centre, new Vector2(-84, 28) + new Vector2(0, 10.5f));
            logoB.Fade(startTime, 1);
            logoB.Fade(fadeOutTime + fadeDuration, 0);
            logoB.Rotate(startTime, 0.2f);
            logoB.Additive(startTime);
            logoB.Color(startTime, new Color4(0, 0, 255, 255));
            logoB.Scale(startTime, .037f);
            logoB.MoveX(OsbEasing.OutCubic, startTime, startTime + fadeInTime, -85 - 210, -85);
            logoB.MoveX(OsbEasing.OutCubic, fadeOutTime, fadeOutTime + fadeDuration, -85, -85 - 210);

            var text = layer.CreateSprite("sb/atlas/network.png", OsbOrigin.Centre, new Vector2(15, 30) + new Vector2(0, 10));
            text.Fade(startTime, 1);
            text.Fade(fadeOutTime + fadeDuration, 0);
            text.Scale(startTime, 0.35);
            text.MoveX(OsbEasing.OutCubic, startTime, startTime + fadeInTime, 15 - 210, 15);
            text.MoveX(OsbEasing.OutCubic, fadeOutTime, fadeOutTime + fadeDuration, 15, 15 - 210);

            var backgroundConnecting = layer.CreateSprite("sb/white.png", OsbOrigin.Centre, new Vector2(660, 450));
            backgroundConnecting.Fade(startTime, 1);
            backgroundConnecting.Fade(fadeOutTime + fadeDuration, 0);
            backgroundConnecting.Color(startTime, new Color4(0, 0, 0, 255));
            backgroundConnecting.ScaleVec(startTime, 150, 20);
            backgroundConnecting.MoveX(OsbEasing.OutCubic, startTime, startTime + fadeInTime, 854, 660);
            backgroundConnecting.MoveX(OsbEasing.OutCubic, fadeOutTime, fadeOutTime + fadeDuration, 660, 854);

            var establishingLinkR = layer.CreateSprite("sb/atlas/text/link.png", OsbOrigin.Centre, new Vector2(659.5f, 449.5f));
            establishingLinkR.StartLoopGroup(startTime, (int)((connectedTime - startTime) / 1000));
            establishingLinkR.Fade(OsbEasing.OutSine, 0, 500, 1, 0);
            establishingLinkR.Fade(OsbEasing.OutSine, 500, 1000, 0, 1);
            establishingLinkR.EndGroup();
            establishingLinkR.Fade(connectedTime, 0);
            establishingLinkR.Scale(startTime, 0.35);
            establishingLinkR.Color(startTime, new Color4(255, 0, 0, 255));
            establishingLinkR.Additive(startTime);
            establishingLinkR.MoveX(OsbEasing.OutCubic, startTime, startTime + fadeInTime, 856, 659.5f);

            var establishingLinkG = layer.CreateSprite("sb/atlas/text/link.png", OsbOrigin.Centre, new Vector2(660, 450));
            establishingLinkG.StartLoopGroup(startTime, (int)((connectedTime - startTime) / 1000));
            establishingLinkG.Fade(OsbEasing.OutSine, 0, 500, 1, 0);
            establishingLinkG.Fade(OsbEasing.OutSine, 500, 1000, 0, 1);
            establishingLinkG.EndGroup();
            establishingLinkG.Fade(connectedTime, 0);
            establishingLinkG.Scale(startTime, 0.35);
            establishingLinkG.Color(startTime, new Color4(0, 255, 0, 255));
            establishingLinkG.Additive(startTime);
            establishingLinkG.MoveX(OsbEasing.OutCubic, startTime, startTime + fadeInTime, 856, 660);

            var establishingLinkB = layer.CreateSprite("sb/atlas/text/link.png", OsbOrigin.Centre, new Vector2(660.5f, 450.5f));
            establishingLinkB.StartLoopGroup(startTime, (int)((connectedTime - startTime) / 1000));
            establishingLinkB.Fade(OsbEasing.OutSine, 0, 500, 1, 0);
            establishingLinkB.Fade(OsbEasing.OutSine, 500, 1000, 0, 1);
            establishingLinkB.EndGroup();
            establishingLinkB.Fade(connectedTime, 0);
            establishingLinkB.Scale(startTime, 0.35);
            establishingLinkB.Color(startTime, new Color4(0, 0, 255, 255));
            establishingLinkB.Additive(startTime);
            establishingLinkB.MoveX(OsbEasing.OutCubic, startTime, startTime + fadeInTime, 852, 660.5f);

            // RGB shifted versions of connecting animation
            var conntectingR = layer.CreateAnimation("sb/atlas/text/connecting/frame.png", 3, 333, OsbLoopType.LoopForever, OsbOrigin.Centre, new Vector2(659.5f, 449.5f));
            conntectingR.Fade(fadeOutTime + fadeDuration, 0);
            conntectingR.Fade(connectedTime, 1);
            conntectingR.Scale(connectedTime, 0.5);
            conntectingR.Color(connectedTime, new Color4(255, 0, 0, 255));
            conntectingR.Additive(connectedTime);
            conntectingR.MoveX(OsbEasing.OutCubic, fadeOutTime, fadeOutTime + fadeDuration, 659.5f, 856);

            var conntectingG = layer.CreateAnimation("sb/atlas/text/connecting/frame.png", 3, 333, OsbLoopType.LoopForever, OsbOrigin.Centre, new Vector2(660, 450));
            conntectingG.Fade(fadeOutTime + fadeDuration, 0);
            conntectingG.Fade(connectedTime, 1);
            conntectingG.Scale(connectedTime, 0.5);
            conntectingG.Color(connectedTime, new Color4(0, 255, 0, 255));
            conntectingG.Additive(connectedTime);
            conntectingG.MoveX(OsbEasing.OutCubic, fadeOutTime, fadeOutTime + fadeDuration, 660, 856);

            var conntectingB = layer.CreateAnimation("sb/atlas/text/connecting/frame.png", 3, 333, OsbLoopType.LoopForever, OsbOrigin.Centre, new Vector2(660.5f, 450.5f));
            conntectingB.Fade(fadeOutTime + fadeDuration, 0);
            conntectingB.Fade(connectedTime, 1);
            conntectingB.Scale(connectedTime, 0.5);
            conntectingB.Color(connectedTime, new Color4(0, 0, 255, 255));
            conntectingB.Additive(connectedTime);
            conntectingB.MoveX(OsbEasing.OutCubic, fadeOutTime, fadeOutTime + fadeDuration, 660.5f, 852);
        }

        public void Writer(double startTime, double fadeOutTime, string text, Vector2 pos, float scale = 0.5f)
        {

            var backGround = GetLayer("part_bga").CreateSprite("sb/white.png", OsbOrigin.TopLeft, pos - new Vector2(25, 9));
            backGround.Fade(startTime, 0.75f);
            backGround.Fade(fadeOutTime + 500, 0);
            backGround.Color(startTime, new Color4(0, 0, 0, 255));
            backGround.MoveX(OsbEasing.InCubic, fadeOutTime, fadeOutTime + 500, pos.X - 25, pos.X - 150 - 25);

            var spacing = 0;
            scale = scale * 0.4f;
            var scaleVert = font.GetTexture("W").BaseHeight * scale;
            var startX = pos.X;
            var localStart = startTime;

            //pos.X -= text.Length / 2 * (font.GetTexture("W").BaseWidth * scale);
            pos.X += spacing;

            List<float> widths = new List<float>();
            int currentWidthIndex = 0;

            startTime += 100; // Initial delay for the first character

            foreach (char c in text.ToUpper())
            {
                if (currentWidthIndex >= widths.Count)
                {
                    widths.Add(0);
                }
                float currentWidth = widths[currentWidthIndex];
                if (c == ' ')
                {
                    pos.X += font.GetTexture("W").BaseWidth * scale + spacing; // Add spacing for space character
                    if (currentWidthIndex >= widths.Count)
                    {
                        widths.Add(0);
                    }
                    widths[currentWidthIndex] = currentWidth + font.GetTexture("W").BaseWidth * scale + spacing;
                    continue;
                }
                if (c == '\n')
                {
                    currentWidthIndex++;
                    pos.Y += 12;
                    pos.X = startX;
                    scaleVert += 12f;
                    continue;
                }
                if (currentWidthIndex >= widths.Count)
                {
                    widths.Add(0);
                }
                widths[currentWidthIndex] = currentWidth + font.GetTexture("W").BaseWidth * scale + spacing;
                var f = GetLayer("part_bga").CreateSprite(font.GetTexture(c.ToString()).Path, OsbOrigin.Centre, pos);
                f.Fade(startTime, 0.5f);
                f.Scale(startTime, scale);
                f.Color(startTime, Color4.White);
                startTime += 100; // Adjust the time for the next character
                pos.X += font.GetTexture(c.ToString()).BaseWidth * scale; // Move the x position for the next character
                f.Fade(fadeOutTime + 500, 0);
                f.MoveX(OsbEasing.InCubic, fadeOutTime, fadeOutTime + 500, pos.X, pos.X - 150);
            }

            var maxWidth = widths.Max() * 1.1f;
            backGround.ScaleVec(OsbEasing.OutCubic, localStart, localStart + 500, new Vector2(0, scaleVert), new Vector2(maxWidth + 25, scaleVert));
        }


        public void CreateWarningAndMine(double startTime, double endTime)
        {
            var layer = GetLayer("part_bga");

            OsbSprite warning = layer.CreateSprite("sb/warning.png", OsbOrigin.Centre, new Vector2(320, 420));
            warning.Fade(startTime, .5f);
            warning.Scale(startTime, 0.08f);
            warning.MoveX(OsbEasing.OutCubic, startTime, startTime + 774, 900, 680);
            warning.MoveX(OsbEasing.InCubic, endTime - 1548, endTime, 680, 900);
            warning.Fade(endTime, 0);

            OsbSprite mine = layer.CreateSprite("sb/sprites/mine.png", OsbOrigin.Centre, new Vector2(320, 427));
            mine.Fade(startTime, .8f);
            mine.Rotate(startTime, endTime, 0f, Math.PI * 16);
            mine.Scale(startTime, 0.25f);
            mine.MoveX(OsbEasing.OutCubic, startTime, startTime + 774, 900, 680);
            mine.MoveX(OsbEasing.InCubic, endTime - 1548, endTime, 680, 900);
            mine.Fade(endTime, 0);
        }
    }
}