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
    public class FirstDropBga : StoryboardObjectGenerator
    {
        public override void Generate()
        {

            var startTime = 55027;
            var sectionEnd = 67414;

            var layer = GetLayer("BGA");

            Color4 w = new Color4(255, 255, 255, 255);
            Color4 o = new Color4(236, 99, 74, 255);
            Color4 y = new Color4(237, 199, 112, 255);
            Color4 c = new Color4(94, 196, 192, 255);

            var beatDuration = Beatmap.GetControlPointAt(startTime).BeatDuration;
            var loopinterval = beatDuration * 2;

            var scale = new Vector2(0.6f);

            var movemnetPerScale = 40 / 0.8 * scale.X;
            var rotationScale = 0.6f * scale.X;
            var xPos = 320;
            var yPos = 240;

            var front = layer.CreateSprite("sb/grid.png", OsbOrigin.Centre);
            front.Fade(OsbEasing.OutCubic, startTime, 56575, 0, 0.5f);
            front.MoveX(OsbEasing.OutCubic, startTime, 56575, xPos - movemnetPerScale * 2, xPos);
            front.MoveY(OsbEasing.OutCubic, startTime, 56575, yPos - movemnetPerScale * 2, yPos);
            front.ScaleVec(OsbEasing.OutCubic, startTime, 56575, new Vector2(1.4f), scale);
            front.ScaleVec(OsbEasing.OutCubic, 66640, sectionEnd, new Vector2(0.6f), new Vector2(1.4f));
            front.Fade(OsbEasing.OutCubic, 66640, sectionEnd, 0.5f, 0);

            // Parallax movement for the front grid
            front.StartLoopGroup(56575, (int)((58511 - 56575) / loopinterval));
            front.MoveX(OsbEasing.OutExpo, 0, beatDuration, xPos, xPos + movemnetPerScale); // Fastest movement
            front.MoveX(OsbEasing.OutExpo, beatDuration, loopinterval, xPos + movemnetPerScale, xPos + movemnetPerScale + movemnetPerScale);
            front.MoveY(OsbEasing.OutExpo, 0, beatDuration, yPos, yPos + movemnetPerScale);
            front.MoveY(OsbEasing.OutExpo, beatDuration, loopinterval, yPos + movemnetPerScale, yPos + movemnetPerScale + movemnetPerScale);
            front.EndGroup();

            front.MoveX(OsbEasing.OutExpo, 58124, 58317, xPos, xPos + movemnetPerScale);
            front.MoveY(OsbEasing.OutExpo, 58124, 58317, yPos, yPos + movemnetPerScale);
            front.MoveX(OsbEasing.OutSine, 58317, 58898, xPos + movemnetPerScale, xPos - movemnetPerScale); // Fastest movement
            front.MoveY(OsbEasing.OutSine, 58317, 58898, yPos + movemnetPerScale, yPos - movemnetPerScale);

            front.StartLoopGroup(58898, (int)((61608 - 58898) / loopinterval));
            front.MoveX(OsbEasing.OutExpo, 0, beatDuration, xPos, xPos - movemnetPerScale); // Fastest movement
            front.MoveX(OsbEasing.OutExpo, beatDuration, loopinterval, xPos - movemnetPerScale, xPos - movemnetPerScale - movemnetPerScale);
            front.MoveY(OsbEasing.OutExpo, 0, beatDuration, yPos, yPos - movemnetPerScale);
            front.MoveY(OsbEasing.OutExpo, beatDuration, loopinterval, yPos - movemnetPerScale, yPos - movemnetPerScale - movemnetPerScale);
            front.EndGroup();

            front.MoveX(OsbEasing.OutCubic, 61221, 61317, xPos, xPos - movemnetPerScale);
            front.MoveY(OsbEasing.OutCubic, 61221, 61317, yPos, yPos - movemnetPerScale);
            front.MoveX(OsbEasing.OutCubic, 61317, 61414, xPos - movemnetPerScale, xPos);
            front.MoveY(OsbEasing.OutCubic, 61317, 61414, yPos - movemnetPerScale, yPos);
            front.MoveX(OsbEasing.OutCubic, 61414, 61608, xPos, xPos - movemnetPerScale);
            front.MoveY(OsbEasing.OutCubic, 61414, 61608, yPos, yPos - movemnetPerScale);

            front.MoveX(OsbEasing.OutCubic, 61608, 61995, xPos, xPos - movemnetPerScale - movemnetPerScale); // Fastest movement
            front.MoveY(OsbEasing.OutCubic, 61608, 61995, yPos, yPos - movemnetPerScale - movemnetPerScale);

            front.StartLoopGroup(61995, (int)((sectionEnd - 61995) / loopinterval));
            front.MoveX(OsbEasing.OutExpo, 0, beatDuration, xPos, xPos + movemnetPerScale * 1.5f); // Fastest movement
            front.MoveX(OsbEasing.OutExpo, beatDuration, loopinterval, xPos + movemnetPerScale * 1.5f, xPos + movemnetPerScale * 1.5f + movemnetPerScale * 1.5f);
            front.MoveY(OsbEasing.OutExpo, 0, beatDuration, yPos, yPos + movemnetPerScale * 1.5f);
            front.MoveY(OsbEasing.OutExpo, beatDuration, loopinterval, yPos + movemnetPerScale * 1.5f, yPos + movemnetPerScale * 1.5f + movemnetPerScale * 1.5f);
            front.EndGroup();

            front.Rotate(OsbEasing.OutSine, 56575, 58317, 0, -rotationScale);
            front.Rotate(OsbEasing.InSine, 58317, 58898, -rotationScale, 0);
            front.Rotate(OsbEasing.OutSine, 58898, 61608, 0, rotationScale);
            front.Rotate(OsbEasing.InSine, 61608, 61995, rotationScale, 0);
            front.Rotate(OsbEasing.OutSine, 61995, 66640, 0, -rotationScale);
            front.Rotate(OsbEasing.OutSine, 66640, 67414, -rotationScale, 0);
            front.Color(startTime, w);
            front.Fade(sectionEnd, 0);
            front.Additive(startTime, 0);

            scale = new Vector2(0.55f);
            movemnetPerScale = 50 * scale.X;
            rotationScale = 0.6f * scale.X;

            var frontMiddle = layer.CreateSprite("sb/grid.png", OsbOrigin.Centre);
            frontMiddle.Fade(startTime, 0.4f);
            frontMiddle.MoveX(OsbEasing.OutCubic, startTime, 56575, xPos - movemnetPerScale * 2, xPos);
            frontMiddle.MoveY(OsbEasing.OutCubic, startTime, 56575, yPos - movemnetPerScale * 2, yPos);
            frontMiddle.ScaleVec(OsbEasing.OutCubic, startTime, 56575, new Vector2(1.2f), new Vector2(0.55f));
            frontMiddle.ScaleVec(OsbEasing.OutCubic, 66640, sectionEnd, new Vector2(0.55f), new Vector2(1.2f));
            frontMiddle.Fade(OsbEasing.OutCubic, 66640, sectionEnd, 0.5f, 0);

            // Parallax movement for the front-middle grid
            frontMiddle.StartLoopGroup(56575, (int)((58511 - 56575) / loopinterval));
            frontMiddle.MoveX(OsbEasing.OutExpo, 0, beatDuration, xPos, xPos + movemnetPerScale); // Fastest movement
            frontMiddle.MoveX(OsbEasing.OutExpo, beatDuration, loopinterval, xPos + movemnetPerScale, xPos + movemnetPerScale + movemnetPerScale);
            frontMiddle.MoveY(OsbEasing.OutExpo, 0, beatDuration, yPos, yPos + movemnetPerScale);
            frontMiddle.MoveY(OsbEasing.OutExpo, beatDuration, loopinterval, yPos + movemnetPerScale, yPos + movemnetPerScale + movemnetPerScale);
            frontMiddle.EndGroup();

            frontMiddle.MoveX(OsbEasing.OutExpo, 58124, 58317, xPos, xPos + movemnetPerScale);
            frontMiddle.MoveY(OsbEasing.OutExpo, 58124, 58317, yPos, yPos + movemnetPerScale);
            frontMiddle.MoveX(OsbEasing.OutSine, 58317, 58898, xPos + movemnetPerScale, xPos - movemnetPerScale); // Fastest movement
            frontMiddle.MoveY(OsbEasing.OutSine, 58317, 58898, yPos + movemnetPerScale, yPos - movemnetPerScale);

            frontMiddle.StartLoopGroup(58898, (int)((61608 - 58898) / loopinterval));
            frontMiddle.MoveX(OsbEasing.OutExpo, 0, beatDuration, xPos, xPos - movemnetPerScale); // Fastest movement
            frontMiddle.MoveX(OsbEasing.OutExpo, beatDuration, loopinterval, xPos - movemnetPerScale, xPos - movemnetPerScale - movemnetPerScale);
            frontMiddle.MoveY(OsbEasing.OutExpo, 0, beatDuration, yPos, yPos - movemnetPerScale);
            frontMiddle.MoveY(OsbEasing.OutExpo, beatDuration, loopinterval, yPos - movemnetPerScale, yPos - movemnetPerScale - movemnetPerScale);
            frontMiddle.EndGroup();

            frontMiddle.MoveX(OsbEasing.OutCubic, 61221, 61317, xPos, xPos - movemnetPerScale);
            frontMiddle.MoveY(OsbEasing.OutCubic, 61221, 61317, yPos, yPos - movemnetPerScale);
            frontMiddle.MoveX(OsbEasing.OutCubic, 61317, 61414, xPos - movemnetPerScale, xPos);
            frontMiddle.MoveY(OsbEasing.OutCubic, 61317, 61414, yPos - movemnetPerScale, yPos);
            frontMiddle.MoveX(OsbEasing.OutCubic, 61414, 61608, xPos, xPos - movemnetPerScale);
            frontMiddle.MoveY(OsbEasing.OutCubic, 61414, 61608, yPos, yPos - movemnetPerScale);

            frontMiddle.MoveX(OsbEasing.OutCubic, 61608, 61995, xPos, xPos - movemnetPerScale - movemnetPerScale); // Fastest movement
            frontMiddle.MoveY(OsbEasing.OutCubic, 61608, 61995, yPos, yPos - movemnetPerScale - movemnetPerScale);

            frontMiddle.StartLoopGroup(61995, (int)((sectionEnd - 61995) / loopinterval));
            frontMiddle.MoveX(OsbEasing.OutExpo, 0, beatDuration, xPos, xPos + movemnetPerScale * 1.5f); // Fastest movement
            frontMiddle.MoveX(OsbEasing.OutExpo, beatDuration, loopinterval, xPos + movemnetPerScale * 1.5f, xPos + movemnetPerScale * 1.5f + movemnetPerScale * 1.5f);
            frontMiddle.MoveY(OsbEasing.OutExpo, 0, beatDuration, yPos, yPos + movemnetPerScale * 1.5f);
            frontMiddle.MoveY(OsbEasing.OutExpo, beatDuration, loopinterval, yPos + movemnetPerScale * 1.5f, yPos + movemnetPerScale * 1.5f + movemnetPerScale * 1.5f);
            frontMiddle.EndGroup();

            frontMiddle.Rotate(OsbEasing.OutSine, 56575, 58317, 0, -rotationScale);
            frontMiddle.Rotate(OsbEasing.InSine, 58317, 58898, -rotationScale, 0);
            frontMiddle.Rotate(OsbEasing.OutSine, 58898, 61608, 0, rotationScale);
            frontMiddle.Rotate(OsbEasing.InSine, 61608, 61995, rotationScale, 0);
            frontMiddle.Rotate(OsbEasing.OutSine, 61995, 66640, 0, -rotationScale);
            frontMiddle.Rotate(OsbEasing.OutSine, 66640, 67414, -rotationScale, 0);
            frontMiddle.Color(startTime, o);
            frontMiddle.Fade(sectionEnd, 0);
            frontMiddle.Additive(startTime, 0);

            scale = new Vector2(0.5f);
            movemnetPerScale = 50 * scale.X;
            rotationScale = 0.6f * scale.X;

            var BackMiddle = layer.CreateSprite("sb/grid.png", OsbOrigin.Centre);
            BackMiddle.Fade(startTime, 0.3f);
            BackMiddle.MoveX(OsbEasing.OutCubic, startTime, 56575, xPos - movemnetPerScale * 2, xPos);
            BackMiddle.MoveY(OsbEasing.OutCubic, startTime, 56575, yPos - movemnetPerScale * 2, yPos);
            BackMiddle.ScaleVec(OsbEasing.OutCubic, startTime, 56575, new Vector2(0.9f), new Vector2(0.45f));
            BackMiddle.ScaleVec(OsbEasing.OutCubic, 66640, sectionEnd, new Vector2(0.45f), new Vector2(0.9f));
            BackMiddle.Fade(OsbEasing.OutCubic, 66640, sectionEnd, 0.5f, 0);

            // Parallax movement for the back-middle grid
            BackMiddle.StartLoopGroup(56575, (int)((58511 - 56575) / loopinterval));
            BackMiddle.MoveX(OsbEasing.OutExpo, 0, beatDuration, xPos, xPos + movemnetPerScale); // Fastest movement
            BackMiddle.MoveX(OsbEasing.OutExpo, beatDuration, loopinterval, xPos + movemnetPerScale, xPos + movemnetPerScale + movemnetPerScale);
            BackMiddle.MoveY(OsbEasing.OutExpo, 0, beatDuration, yPos, yPos + movemnetPerScale);
            BackMiddle.MoveY(OsbEasing.OutExpo, beatDuration, loopinterval, yPos + movemnetPerScale, yPos + movemnetPerScale + movemnetPerScale);
            BackMiddle.EndGroup();

            BackMiddle.MoveX(OsbEasing.OutExpo, 58124, 58317, xPos, xPos + movemnetPerScale);
            BackMiddle.MoveY(OsbEasing.OutExpo, 58124, 58317, yPos, yPos + movemnetPerScale);
            BackMiddle.MoveX(OsbEasing.OutSine, 58317, 58898, xPos + movemnetPerScale, xPos - movemnetPerScale); // Fastest movement
            BackMiddle.MoveY(OsbEasing.OutSine, 58317, 58898, yPos + movemnetPerScale, yPos - movemnetPerScale);

            BackMiddle.StartLoopGroup(58898, (int)((61608 - 58898) / loopinterval));
            BackMiddle.MoveX(OsbEasing.OutExpo, 0, beatDuration, xPos, xPos - movemnetPerScale); // Fastest movement
            BackMiddle.MoveX(OsbEasing.OutExpo, beatDuration, loopinterval, xPos - movemnetPerScale, xPos - movemnetPerScale - movemnetPerScale);
            BackMiddle.MoveY(OsbEasing.OutExpo, 0, beatDuration, yPos, yPos - movemnetPerScale);
            BackMiddle.MoveY(OsbEasing.OutExpo, beatDuration, loopinterval, yPos - movemnetPerScale, yPos - movemnetPerScale - movemnetPerScale);
            BackMiddle.EndGroup();

            BackMiddle.MoveX(OsbEasing.OutCubic, 61221, 61317, xPos, xPos - movemnetPerScale);
            BackMiddle.MoveY(OsbEasing.OutCubic, 61221, 61317, yPos, yPos - movemnetPerScale);
            BackMiddle.MoveX(OsbEasing.OutCubic, 61317, 61414, xPos - movemnetPerScale, xPos);
            BackMiddle.MoveY(OsbEasing.OutCubic, 61317, 61414, yPos - movemnetPerScale, yPos);
            BackMiddle.MoveX(OsbEasing.OutCubic, 61414, 61608, xPos, xPos - movemnetPerScale);
            BackMiddle.MoveY(OsbEasing.OutCubic, 61414, 61608, yPos, yPos - movemnetPerScale);

            BackMiddle.MoveX(OsbEasing.OutCubic, 61608, 61995, xPos, xPos - movemnetPerScale - movemnetPerScale); // Fastest movement
            BackMiddle.MoveY(OsbEasing.OutCubic, 61608, 61995, yPos, yPos - movemnetPerScale - movemnetPerScale);

            BackMiddle.StartLoopGroup(61995, (int)((sectionEnd - 61995) / loopinterval));
            BackMiddle.MoveX(OsbEasing.OutExpo, 0, beatDuration, xPos, xPos + movemnetPerScale * 1.5f); // Fastest movement
            BackMiddle.MoveX(OsbEasing.OutExpo, beatDuration, loopinterval, xPos + movemnetPerScale * 1.5f, xPos + movemnetPerScale * 1.5f + movemnetPerScale * 1.5f);
            BackMiddle.MoveY(OsbEasing.OutExpo, 0, beatDuration, yPos, yPos + movemnetPerScale * 1.5f);
            BackMiddle.MoveY(OsbEasing.OutExpo, beatDuration, loopinterval, yPos + movemnetPerScale * 1.5f, yPos + movemnetPerScale * 1.5f + movemnetPerScale * 1.5f);
            BackMiddle.EndGroup();

            BackMiddle.Rotate(OsbEasing.OutSine, 56575, 58317, 0, -rotationScale);
            BackMiddle.Rotate(OsbEasing.InSine, 58317, 58898, -rotationScale, 0);
            BackMiddle.Rotate(OsbEasing.OutSine, 58898, 61608, 0, rotationScale);
            BackMiddle.Rotate(OsbEasing.InSine, 61608, 61995, rotationScale, 0);
            BackMiddle.Rotate(OsbEasing.OutSine, 61995, 66640, 0, -rotationScale);
            BackMiddle.Rotate(OsbEasing.OutSine, 66640, 67414, -rotationScale, 0);
            BackMiddle.Color(startTime, y);
            BackMiddle.Fade(sectionEnd, 0);
            BackMiddle.Additive(startTime, 0);

            scale = new Vector2(0.45f);
            movemnetPerScale = 50 * scale.X;
            rotationScale = 0.6f * scale.X;

            var Back = layer.CreateSprite("sb/grid.png", OsbOrigin.Centre);
            Back.Fade(startTime, 0.2f);
            Back.MoveX(OsbEasing.OutCubic, startTime, 56575, xPos - movemnetPerScale * 2, xPos);
            Back.MoveY(OsbEasing.OutCubic, startTime, 56575, yPos - movemnetPerScale * 2, yPos);
            Back.ScaleVec(OsbEasing.OutCubic, startTime, 56575, new Vector2(0.65f), new Vector2(0.4f));
            Back.ScaleVec(OsbEasing.OutCubic, 66640, sectionEnd, new Vector2(0.4f), new Vector2(0.65f));
            Back.Fade(OsbEasing.OutCubic, 66640, sectionEnd, 0.5f, 0);

            // Parallax movement for the back grid
            Back.StartLoopGroup(56575, (int)((58511 - 56575) / loopinterval));
            Back.MoveX(OsbEasing.OutExpo, 0, beatDuration, xPos, xPos + movemnetPerScale); // Fastest movement
            Back.MoveX(OsbEasing.OutExpo, beatDuration, loopinterval, xPos + movemnetPerScale, xPos + movemnetPerScale + movemnetPerScale);
            Back.MoveY(OsbEasing.OutExpo, 0, beatDuration, yPos, yPos + movemnetPerScale);
            Back.MoveY(OsbEasing.OutExpo, beatDuration, loopinterval, yPos + movemnetPerScale, yPos + movemnetPerScale + movemnetPerScale);
            Back.EndGroup();

            Back.MoveX(OsbEasing.OutExpo, 58124, 58317, xPos, xPos + movemnetPerScale);
            Back.MoveY(OsbEasing.OutExpo, 58124, 58317, yPos, yPos + movemnetPerScale);
            Back.MoveX(OsbEasing.OutSine, 58317, 58898, xPos + movemnetPerScale, xPos - movemnetPerScale); // Fastest movement
            Back.MoveY(OsbEasing.OutSine, 58317, 58898, yPos + movemnetPerScale, yPos - movemnetPerScale);

            Back.StartLoopGroup(58898, (int)((61608 - 58898) / loopinterval));
            Back.MoveX(OsbEasing.OutExpo, 0, beatDuration, xPos, xPos - movemnetPerScale); // Fastest movement
            Back.MoveX(OsbEasing.OutExpo, beatDuration, loopinterval, xPos - movemnetPerScale, xPos - movemnetPerScale - movemnetPerScale);
            Back.MoveY(OsbEasing.OutExpo, 0, beatDuration, yPos, yPos - movemnetPerScale);
            Back.MoveY(OsbEasing.OutExpo, beatDuration, loopinterval, yPos - movemnetPerScale, yPos - movemnetPerScale - movemnetPerScale);
            Back.EndGroup();

            Back.MoveX(OsbEasing.OutCubic, 61221, 61317, xPos, xPos - movemnetPerScale);
            Back.MoveY(OsbEasing.OutCubic, 61221, 61317, yPos, yPos - movemnetPerScale);
            Back.MoveX(OsbEasing.OutCubic, 61317, 61414, xPos - movemnetPerScale, xPos);
            Back.MoveY(OsbEasing.OutCubic, 61317, 61414, yPos - movemnetPerScale, yPos);
            Back.MoveX(OsbEasing.OutCubic, 61414, 61608, xPos, xPos - movemnetPerScale);
            Back.MoveY(OsbEasing.OutCubic, 61414, 61608, yPos, yPos - movemnetPerScale);

            Back.MoveX(OsbEasing.OutCubic, 61608, 61995, xPos - movemnetPerScale, xPos - movemnetPerScale - movemnetPerScale); // Fastest movement
            Back.MoveY(OsbEasing.OutCubic, 61608, 61995, yPos - movemnetPerScale, yPos - movemnetPerScale - movemnetPerScale);

            Back.StartLoopGroup(61995, (int)((sectionEnd - 61995) / loopinterval));
            Back.MoveX(OsbEasing.OutExpo, 0, beatDuration, xPos, xPos + movemnetPerScale * 1.5f); // Fastest movement
            Back.MoveX(OsbEasing.OutExpo, beatDuration, loopinterval, xPos + movemnetPerScale * 1.5f, xPos + movemnetPerScale * 1.5f + movemnetPerScale * 1.5f);
            Back.MoveY(OsbEasing.OutExpo, 0, beatDuration, yPos, yPos + movemnetPerScale * 1.5f);
            Back.MoveY(OsbEasing.OutExpo, beatDuration, loopinterval, yPos + movemnetPerScale * 1.5f, yPos + movemnetPerScale * 1.5f + movemnetPerScale * 1.5f);
            Back.EndGroup();

            Back.Rotate(OsbEasing.OutSine, 56575, 58317, 0, -rotationScale);
            Back.Rotate(OsbEasing.InSine, 58317, 58898, -rotationScale, 0);
            Back.Rotate(OsbEasing.OutSine, 58898, 61608, 0, rotationScale);
            Back.Rotate(OsbEasing.InSine, 61608, 61995, rotationScale, 0);
            Back.Rotate(OsbEasing.OutSine, 61995, 66640, 0, -rotationScale);
            Back.Rotate(OsbEasing.OutSine, 66640, 67414, -rotationScale, 0);
            Back.Color(startTime, c);
            Back.Fade(sectionEnd, 0);
            Back.Additive(startTime, 0);

            var bgCover = layer.CreateSprite("sb/white.png", OsbOrigin.Centre);
            bgCover.ScaleVec(startTime, new Vector2(600f, 500f));
            bgCover.Color(startTime, new Color4(0, 0, 0, 1));
            bgCover.Fade(OsbEasing.OutCubic, startTime, 56575, 0, 0.3f);
            bgCover.Fade(OsbEasing.OutCubic, 66640, sectionEnd, 0.3f, 0);

            var are = layer.CreateSprite("sb/ani/ready/are.png", OsbOrigin.Centre, new Vector2(320, 220));
            var you = layer.CreateSprite("sb/ani/ready/you.png", OsbOrigin.Centre, new Vector2(320, 220));
            var ready = layer.CreateSprite("sb/ani/ready/ready.png", OsbOrigin.Centre, new Vector2(320, 280));

            are.Scale(OsbEasing.OutCubic, 54253, 54640, 0f, 0.5f);
            you.Scale(OsbEasing.OutCubic, 54640, 55027, 0f, 0.5f);
            ready.Scale(OsbEasing.OutCubic, 55027, 55221, 0f, 0.5f);

            are.Rotate(OsbEasing.OutCubic, 54253, 54640, 0.4f, 0f);
            you.Rotate(OsbEasing.OutCubic, 54640, 55027, 0.4f, 0f);
            ready.Rotate(OsbEasing.OutCubic, 55027, 55027, 0.4f, 0f);

            are.Fade(54059, 1);
            you.Fade(54446, 1);
            ready.Fade(54930, 1);

            are.MoveX(OsbEasing.InCubic, 55027, 55898, 320, -500);
            you.MoveX(OsbEasing.InCubic, 55027, 55898, 320, -500);
            ready.MoveX(OsbEasing.InCubic, 55027, 55898, 320, 1140);

            are.Scale(OsbEasing.InSine, 55221, 55898, 0.5f, 1.4f);
            you.Scale(OsbEasing.InSine, 55221, 55898, 0.5f, 1.4f);
            ready.Scale(OsbEasing.InSine, 55221, 55898, 0.5f, 1.4f);

            are.Rotate(OsbEasing.InCubic, 55027, 55898, 0f, -0.3f);
            you.Rotate(OsbEasing.InCubic, 55027, 55898, 0f, -0.3f);
            ready.Rotate(OsbEasing.InCubic, 55027, 55898, 0f, -0.3f);

            are.Fade(56575, 0);
            you.Fade(56575, 0);
            ready.Fade(56575, 0);

            var TopCover = layer.CreateSprite("sb/white.png", OsbOrigin.TopCentre, new Vector2(320, 0));
            TopCover.ScaleVec(OsbEasing.OutCubic, startTime, 56575, new Vector2(1000, 0), new Vector2(1000, 5));
            TopCover.ScaleVec(OsbEasing.OutCubic, 66640, sectionEnd, new Vector2(1000, 5), new Vector2(1000, 0));
            TopCover.Fade(55027, 1f);
            TopCover.Fade(sectionEnd, 0);

            var BottomCover = layer.CreateSprite("sb/white.png", OsbOrigin.BottomCentre, new Vector2(320, 480));
            BottomCover.ScaleVec(OsbEasing.OutCubic, startTime, 56575, new Vector2(1000, 0), new Vector2(1000, 5));
            BottomCover.ScaleVec(OsbEasing.OutCubic, 66640, sectionEnd, new Vector2(1000, 5), new Vector2(1000, 0));
            BottomCover.Fade(55027, 1f);
            BottomCover.Fade(sectionEnd, 0);

            rotationScale *= 0.35f;

            // Chromatic Aberration for Left Cover
            var LeftCoverGreen = layer.CreateSprite("sb/white.png", OsbOrigin.CentreLeft, new Vector2(OsuHitObject.WidescreenStoryboardBounds.Left + 5 - 20, 240));
            LeftCoverGreen.ScaleVec(OsbEasing.OutCubic, startTime, 56575, new Vector2(0, 600f), new Vector2(170, 600f));
            LeftCoverGreen.ScaleVec(OsbEasing.OutCubic, 66640, sectionEnd, new Vector2(170, 600f), new Vector2(0, 600f));
            LeftCoverGreen.Fade(55027, 1f);
            LeftCoverGreen.Fade(sectionEnd, 0);
            LeftCoverGreen.Rotate(OsbEasing.OutSine, 56575, 58317, 0, rotationScale);
            LeftCoverGreen.Rotate(OsbEasing.InSine, 58317, 58898, rotationScale, 0);
            LeftCoverGreen.Rotate(OsbEasing.OutSine, 58898, 61608, 0, -rotationScale);
            LeftCoverGreen.Rotate(OsbEasing.InSine, 61608, 61995, -rotationScale, 0);
            LeftCoverGreen.Rotate(OsbEasing.OutSine, 61995, 66640, 0, rotationScale);
            LeftCoverGreen.Rotate(OsbEasing.OutSine, 66640, 67414, rotationScale, 0);

            var LeftCoverRed = layer.CreateSprite("sb/white.png", OsbOrigin.CentreLeft, new Vector2(OsuHitObject.WidescreenStoryboardBounds.Left - 20, 240));
            LeftCoverRed.Color(startTime, new Color4(0, 0, 0, 1)); // Red tint
            LeftCoverRed.ScaleVec(OsbEasing.OutCubic, startTime, 56575, new Vector2(0, 600f), new Vector2(170, 600f));
            LeftCoverRed.ScaleVec(OsbEasing.OutCubic, 66640, sectionEnd, new Vector2(170, 600f), new Vector2(0, 600f));
            LeftCoverRed.Fade(55027, 1f);
            LeftCoverRed.Fade(sectionEnd, 0);
            LeftCoverRed.Rotate(OsbEasing.OutSine, 56575, 58317, 0, rotationScale);
            LeftCoverRed.Rotate(OsbEasing.InSine, 58317, 58898, rotationScale, 0);
            LeftCoverRed.Rotate(OsbEasing.OutSine, 58898, 61608, 0, -rotationScale);
            LeftCoverRed.Rotate(OsbEasing.InSine, 61608, 61995, -rotationScale, 0);
            LeftCoverRed.Rotate(OsbEasing.OutSine, 61995, 66640, 0, rotationScale);
            LeftCoverRed.Rotate(OsbEasing.OutSine, 66640, 67414, rotationScale, 0);

            // Chromatic Aberration for Right Cover
            var RightCoverGreen = layer.CreateSprite("sb/white.png", OsbOrigin.CentreRight, new Vector2(OsuHitObject.WidescreenStoryboardBounds.Right - 5 + 20, 240));
            RightCoverGreen.ScaleVec(OsbEasing.OutCubic, startTime, 56575, new Vector2(0, 600f), new Vector2(170, 600f));
            RightCoverGreen.ScaleVec(OsbEasing.OutCubic, 66640, sectionEnd, new Vector2(170, 600f), new Vector2(0, 600f));
            RightCoverGreen.Fade(55027, 1f);
            RightCoverGreen.Fade(sectionEnd, 0);
            RightCoverGreen.Rotate(OsbEasing.OutSine, 56575, 58317, 0, rotationScale);
            RightCoverGreen.Rotate(OsbEasing.InSine, 58317, 58898, rotationScale, 0);
            RightCoverGreen.Rotate(OsbEasing.OutSine, 58898, 61608, 0, -rotationScale);
            RightCoverGreen.Rotate(OsbEasing.InSine, 61608, 61995, -rotationScale, 0);
            RightCoverGreen.Rotate(OsbEasing.OutSine, 61995, 66640, 0, rotationScale);
            RightCoverGreen.Rotate(OsbEasing.OutSine, 66640, 67414, rotationScale, 0);

            var RightCoverRed = layer.CreateSprite("sb/white.png", OsbOrigin.CentreRight, new Vector2(OsuHitObject.WidescreenStoryboardBounds.Right + 20, 240));
            RightCoverRed.Color(startTime, new Color4(0, 0, 0, 1)); // Red tint
            RightCoverRed.ScaleVec(OsbEasing.OutCubic, startTime, 56575, new Vector2(0, 600f), new Vector2(170, 600f));
            RightCoverRed.ScaleVec(OsbEasing.OutCubic, 66640, sectionEnd, new Vector2(170, 600f), new Vector2(0, 600f));
            RightCoverRed.Fade(55027, 1f);
            RightCoverRed.Fade(sectionEnd, 0);
            RightCoverRed.Rotate(OsbEasing.OutSine, 56575, 58317, 0, rotationScale);
            RightCoverRed.Rotate(OsbEasing.InSine, 58317, 58898, rotationScale, 0);
            RightCoverRed.Rotate(OsbEasing.OutSine, 58898, 61608, 0, -rotationScale);
            RightCoverRed.Rotate(OsbEasing.InSine, 61608, 61995, -rotationScale, 0);
            RightCoverRed.Rotate(OsbEasing.OutSine, 61995, 66640, 0, rotationScale);
            RightCoverRed.Rotate(OsbEasing.OutSine, 66640, 67414, rotationScale, 0);


            List<OsbSprite> sprites = new List<OsbSprite>
            {
                LeftCoverRed,
                LeftCoverGreen,
                RightCoverRed,
                RightCoverGreen,
            };

            foreach (var sprite in sprites)
            {
                sprite.ScaleVec(OsbEasing.OutCubic, startTime, 56575, new Vector2(0, 600f), new Vector2(170, 600f));
                sprite.ScaleVec(OsbEasing.OutCubic, 66640, sectionEnd, new Vector2(170, 600f), new Vector2(0, 600f));
                sprite.Fade(55027, 1);
                sprite.Fade(sectionEnd, 0);

                sprite.StartLoopGroup(56575, (int)((58124 - 56575) / beatDuration));
                sprite.ScaleVec(OsbEasing.OutCubic, 0, beatDuration, new Vector2(200, 600f), new Vector2(170, 600f));
                sprite.EndGroup();

                sprite.ScaleVec(OsbEasing.OutSine, 58124, 58898, new Vector2(200, 600f), new Vector2(170, 600f));

                sprite.StartLoopGroup(58898, (int)((61221 - 58898) / beatDuration));
                sprite.ScaleVec(OsbEasing.OutCubic, 0, beatDuration, new Vector2(200, 600f), new Vector2(170, 600f));
                sprite.EndGroup();

                sprite.ScaleVec(OsbEasing.OutSine, 61221, 61995, new Vector2(200, 600f), new Vector2(170, 600f));

                sprite.StartLoopGroup(61995, 12);
                sprite.ScaleVec(OsbEasing.OutCubic, 0, beatDuration, new Vector2(200, 600f), new Vector2(170, 600f));
                sprite.EndGroup();
            }


        }
    }
}
