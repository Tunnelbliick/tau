using Microsoft.VisualBasic;
using OpenTK;
using OpenTK.Graphics;
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
    public class Intro : StoryboardObjectGenerator
    {
        //        public override bool Multithreaded => false;

        private readonly object _lock = new object();
        float direction = -1;
        // Generate function in a storybrew script
        public override void Generate()
        {

            var receptors = GetLayer("r");
            var notes = GetLayer("n");

            // General values
            var starttime = 0; // the starttime where the playfield is initialized
            var endtime = 18253; // the endtime where the playfield is nolonger beeing rendered
            var duration = endtime - starttime; // the length the playfield is kept alive

            // Playfield Scale
            var width = 250f; // widht of the playfield / invert to flip
            var height = 500f; // height of the playfield / invert to flip -600 = downscroll | 600 = upscropll
            var receptorWallOffset = 50f; // how big the boundary box for the receptor is 50 means it will be pushed away 50 units from the wall

            // Note initilization Values
            var sliderAccuracy = 30; // The Segment length for sliderbodies since they are rendered in slices 30 is default
            var isColored = false; // This property is used if you want to color the notes by urself for effects. It does not swap if the snap coloring is used.

            // Drawinstance Values
            var updatesPerSecond = 100; // The amount of steps the rendring engine does to render out note and receptor positions
            var scrollSpeed = 1200f; // The speed at which the Notes scroll
            var fadeTime = 150; // The time notes will fade in


            Playfield field = new Playfield();
            field.initilizePlayField(receptors, notes, starttime, endtime, width, height, receptorWallOffset, Beatmap.OverallDifficulty);
            field.initializeNotes(Beatmap.HitObjects.ToList(), Beatmap.GetTimingPointAt(starttime).Bpm, Beatmap.GetTimingPointAt(starttime).Offset, isColored, sliderAccuracy);

            field.ScaleReceptor(OsbEasing.None, starttime + 5, starttime + 5, new Vector2(0f, 0.5f), ColumnType.one);
            field.ScaleReceptor(OsbEasing.None, starttime + 5, starttime + 5, new Vector2(0.5f, 0f), ColumnType.two);
            field.ScaleReceptor(OsbEasing.None, starttime + 5, starttime + 5, new Vector2(0.5f, 0f), ColumnType.three);
            field.ScaleReceptor(OsbEasing.None, starttime + 5, starttime + 5, new Vector2(0f, 0.5f), ColumnType.four);
            field.Resize(OsbEasing.None, starttime + 5, starttime + 5, 0, height);

            field.Resize(OsbEasing.OutCubic, 1414, 1801, width, height);
            field.ScaleReceptor(OsbEasing.OutCubic, 1414, 1705, new Vector2(0.5f), ColumnType.one);
            field.ScaleReceptor(OsbEasing.OutCubic, 1414, 1705, new Vector2(0.5f), ColumnType.two);
            field.ScaleReceptor(OsbEasing.OutCubic, 1414, 1705, new Vector2(0.5f), ColumnType.three);
            field.ScaleReceptor(OsbEasing.OutCubic, 1414, 1705, new Vector2(0.5f), ColumnType.four);

            field.Scale(OsbEasing.InSine, 2382, 4704, new Vector2(0.7f), true, CenterType.receptor);

            field.Scale(OsbEasing.InCubic, 4704, 4704, new Vector2(0.5f), true, CenterType.receptor);

            field.Rotate(OsbEasing.InSine, 2382, 4704 - 20, .3f, CenterType.playfield);
            field.Rotate(OsbEasing.InCubic, 4704 - 10, 4704 - 10, -.3f, CenterType.playfield);

            field.RotateReceptor(OsbEasing.OutSine, 4704, 4898, .3, CenterType.receptor);
            field.RotateReceptor(OsbEasing.InSine, 4898, 5460, -.3, CenterType.receptor);
            field.Resize(OsbEasing.OutQuad, 4704, 5382, -width, height);

            field.MoveOriginRelative(OsbEasing.None, 446, 446, new Vector2(0, -height), ColumnType.all);
            field.MoveOriginRelative(OsbEasing.InSine, 3350, 4704, new Vector2(0, height), ColumnType.all);

            double start = 5489;
            double end = 6640;
            double dur = 100;
            double increase = 1.75;
            var localwidht = width;

            SpinOutAnimation(field, 5479, 6640, width, height, 32, 1.7);

            field.Resize(OsbEasing.OutCubic, 7608, 7801, width, height);

            field.RotateReceptor(OsbEasing.OutSine, 10898, 11092, .3, CenterType.receptor);
            field.RotateReceptor(OsbEasing.InSine, 11092, 11575, -.33, CenterType.receptor);
            field.Resize(OsbEasing.OutQuad, 10898, 11575, -width, height);

            SpinOutAnimation(field, 11672, 12834, width, height, 32, 1.7);

            field.ScaleColumn(OsbEasing.None, 4704, 4704, new Vector2(2f), ColumnType.all);
            field.ScaleColumn(OsbEasing.OutCubic, 4704, 5479, new Vector2(0.5f), ColumnType.all);

            ReceptorBump(field, 6640, 2, true);

            field.MoveReceptorRelative(OsbEasing.None, 6640, 6640, new Vector2(-150, 0), ColumnType.all);
            field.MoveReceptorRelative(OsbEasing.OutCubic, 6640, 6640 + 500, new Vector2(150, 0), ColumnType.all);

            field.MoveOriginRelative(OsbEasing.None, 6640, 6640, new Vector2(150, 0), ColumnType.all);
            field.MoveOriginRelative(OsbEasing.OutCubic, 6640, 6640 + 500, new Vector2(-150, 0), ColumnType.all);

            field.MoveReceptorRelative(OsbEasing.None, 7221, 7221, new Vector2(150, 0), ColumnType.all);
            field.MoveReceptorRelative(OsbEasing.OutCubic, 7221, 7221 + 500, new Vector2(-150, 0), ColumnType.all);

            field.MoveOriginRelative(OsbEasing.None, 7221, 7221, new Vector2(-150, 0), ColumnType.all);
            field.MoveOriginRelative(OsbEasing.OutCubic, 7221, 7221 + 500, new Vector2(150, 0), ColumnType.all);

            ReceptorBump(field, 7221, 2, true);

            FlipPlayField(field, 7801, 8963, width, height, 150, 2.1f);

            field.MoveReceptorRelative(OsbEasing.None, 9156, 9156, new Vector2(-150, 0), ColumnType.all);
            field.MoveReceptorRelative(OsbEasing.OutCubic, 9156, 9156 + 500, new Vector2(150, 0), ColumnType.all);

            field.MoveOriginRelative(OsbEasing.None, 9156, 9156, new Vector2(150, 0), ColumnType.all);
            field.MoveOriginRelative(OsbEasing.OutCubic, 9156, 9156 + 500, new Vector2(-150, 0), ColumnType.all);

            field.MoveReceptorRelative(OsbEasing.None, 9737, 9737, new Vector2(150, 0), ColumnType.all);
            field.MoveReceptorRelative(OsbEasing.OutCubic, 9737, 9737 + 500, new Vector2(-150, 0), ColumnType.all);

            field.MoveOriginRelative(OsbEasing.None, 9737, 9737, new Vector2(-150, 0), ColumnType.all);
            field.MoveOriginRelative(OsbEasing.OutCubic, 9737, 9737 + 500, new Vector2(150, 0), ColumnType.all);

            ReceptorBump(field, 9156, 2, true);
            ReceptorBump(field, 9737, 2, true);
            ReceptorBump(field, 10414, 2, true);

            ReceptorBump(field, 12834, 2, true);
            ReceptorBump(field, 13414, 2, true);

            field.Resize(OsbEasing.OutCubic, 13801, 13995, width, height);

            field.MoveReceptorRelative(OsbEasing.None, 12834, 12834, new Vector2(150, 0), ColumnType.all);
            field.MoveReceptorRelative(OsbEasing.OutSine, 12834, 12834 + 500, new Vector2(-150, 0), ColumnType.all);

            field.MoveOriginRelative(OsbEasing.None, 12834, 12834, new Vector2(-150, 0), ColumnType.all);
            field.MoveOriginRelative(OsbEasing.OutSine, 12834, 12834 + 500, new Vector2(150, 0), ColumnType.all);

            field.MoveReceptorRelative(OsbEasing.None, 13414, 13414, new Vector2(-150, 0), ColumnType.all);
            field.MoveReceptorRelative(OsbEasing.OutSine, 13414, 13414 + 500, new Vector2(150, 0), ColumnType.all);

            field.MoveOriginRelative(OsbEasing.None, 13414, 13414, new Vector2(150, 0), ColumnType.all);
            field.MoveOriginRelative(OsbEasing.OutSine, 13414, 13414 + 500, new Vector2(-150, 0), ColumnType.all);

            field.ScaleColumn(OsbEasing.None, 10124, 10124, new Vector2(1f), ColumnType.all);
            field.ScaleColumn(OsbEasing.OutCubic, 10124, 10898 - 50, new Vector2(0.5f), ColumnType.all);

            field.ScaleColumn(OsbEasing.None, 10898, 10898, new Vector2(2f), ColumnType.all);
            field.ScaleColumn(OsbEasing.OutCubic, 10898, 11650, new Vector2(0.5f), ColumnType.all);

            flipColumn(field, 13995, 1000, OsbEasing.OutElasticQuarter, ColumnType.all);

            field.ScaleReceptor(OsbEasing.InSine, 13995, 14092, new Vector2(1f), ColumnType.all);
            field.ScaleReceptor(OsbEasing.OutSine, 14092, 14664, new Vector2(0.5f), ColumnType.all);

            field.ScaleOrigin(OsbEasing.InSine, 13995, 14092, new Vector2(.25f), ColumnType.all);
            field.ScaleOrigin(OsbEasing.OutSine, 14092, 14664, new Vector2(0.5f), ColumnType.all);

            field.MoveReceptorRelative(OsbEasing.InSine, 13995, 14092, new Vector2(-field.getColumnWidth(), 0), ColumnType.one);
            field.MoveReceptorRelative(OsbEasing.InSine, 13995, 14092, new Vector2(-field.getColumnWidth() / 2, 0), ColumnType.two);
            field.MoveReceptorRelative(OsbEasing.InSine, 13995, 14092, new Vector2(field.getColumnWidth() / 2, 0), ColumnType.three);
            field.MoveReceptorRelative(OsbEasing.InSine, 13995, 14092, new Vector2(field.getColumnWidth(), 0), ColumnType.four);

            field.MoveReceptorRelative(OsbEasing.OutSine, 13995, 14664, new Vector2(field.getColumnWidth(), 0), ColumnType.one);
            field.MoveReceptorRelative(OsbEasing.OutSine, 13995, 14664, new Vector2(field.getColumnWidth() / 2, 0), ColumnType.two);
            field.MoveReceptorRelative(OsbEasing.OutSine, 13995, 14664, new Vector2(-field.getColumnWidth() / 2, 0), ColumnType.three);
            field.MoveReceptorRelative(OsbEasing.OutSine, 13995, 14664, new Vector2(-field.getColumnWidth(), 0), ColumnType.four);

            field.MoveOriginRelative(OsbEasing.InSine, 13995, 14092, new Vector2(field.getColumnWidth(), 0), ColumnType.one);
            field.MoveOriginRelative(OsbEasing.InSine, 13995, 14092, new Vector2(field.getColumnWidth() / 2, 0), ColumnType.two);
            field.MoveOriginRelative(OsbEasing.InSine, 13995, 14092, new Vector2(-field.getColumnWidth() / 2, 0), ColumnType.three);
            field.MoveOriginRelative(OsbEasing.InSine, 13995, 14092, new Vector2(-field.getColumnWidth(), 0), ColumnType.four);

            field.MoveOriginRelative(OsbEasing.OutSine, 13995, 14664, new Vector2(-field.getColumnWidth(), 0), ColumnType.one);
            field.MoveOriginRelative(OsbEasing.OutSine, 13995, 14664, new Vector2(-field.getColumnWidth() / 2, 0), ColumnType.two);
            field.MoveOriginRelative(OsbEasing.OutSine, 13995, 14664, new Vector2(field.getColumnWidth() / 2, 0), ColumnType.three);
            field.MoveOriginRelative(OsbEasing.OutSine, 13995, 14664, new Vector2(field.getColumnWidth(), 0), ColumnType.four);

            ScaleField(field, 14769);


            field.MoveReceptorRelative(OsbEasing.None, 15350, 15350, new Vector2(-200, 0), ColumnType.all);
            field.MoveReceptorRelative(OsbEasing.OutSine, 15350, 15640, new Vector2(200, 0), ColumnType.all);

            field.MoveOriginRelative(OsbEasing.None, 15350, 15350, new Vector2(200, 0), ColumnType.all);
            field.MoveOriginRelative(OsbEasing.OutSine, 15350, 15640, new Vector2(-200, 0), ColumnType.all);

            field.MoveReceptorRelative(OsbEasing.None, 15640, 15640, new Vector2(200, 0), ColumnType.all);
            field.MoveReceptorRelative(OsbEasing.OutSine, 15640, 15930, new Vector2(-200, 0), ColumnType.all);

            field.MoveOriginRelative(OsbEasing.None, 15640, 15640, new Vector2(-200, 0), ColumnType.all);
            field.MoveOriginRelative(OsbEasing.OutSine, 15640, 15930, new Vector2(200, 0), ColumnType.all);

            field.MoveReceptorRelative(OsbEasing.None, 15930, 15930, new Vector2(-200, 0), ColumnType.all);
            field.MoveReceptorRelative(OsbEasing.OutSine, 15930, 16221, new Vector2(200, 0), ColumnType.all);

            field.MoveOriginRelative(OsbEasing.None, 15930, 15930, new Vector2(200, 0), ColumnType.all);
            field.MoveOriginRelative(OsbEasing.OutSine, 15930, 16221, new Vector2(-200, 0), ColumnType.all);

            /*field.MoveReceptorRelative(OsbEasing.None, 16221, 16221, new Vector2(200, 0), ColumnType.all);
            field.MoveReceptorRelative(OsbEasing.OutSine, 16221, 16898, new Vector2(-200, 0), ColumnType.all);

            field.MoveOriginRelative(OsbEasing.None, 16221, 16221, new Vector2(-200, 0), ColumnType.all);
            field.MoveOriginRelative(OsbEasing.OutSine, 16221, 16898, new Vector2(200, 0), ColumnType.all);*/

            ReceptorBump(field, 15350, 2, false, 15640 - 15350);
            ReceptorBump(field, 15640, 2, false, 15640 - 15350);
            ReceptorBump(field, 15930, 2, false, 15640 - 15350);
            ReceptorBump(field, 16221, 2, false, 1000);

            field.moveFieldY(OsbEasing.OutSine, 16221, 17092, 240 - 65f);
            field.Resize(OsbEasing.OutCubic, 16221, 17866, 0, height);
            field.ScaleColumn(OsbEasing.InCubic, 16221, 17866, new Vector2(0f), ColumnType.all);

            DrawInstance draw = new DrawInstance(CancellationToken, field, starttime + 10, scrollSpeed, updatesPerSecond, OsbEasing.None, false, fadeTime, fadeTime);
            draw.setReceptorMovementPrecision(0.25f);
            draw.setNoteRotationPrecision(0.01f);
            draw.setHoldRotationPrecision(0.01f);
            draw.drawViaEquation(duration, NoteFunction, true);
        }

        public void ScaleField(Playfield field, double startTime, float scale = 2f, double dur = 300)
        {
            field.RotatePlayFieldStatic(OsbEasing.None, startTime, startTime, Math.PI * direction);
            field.RotatePlayFieldStatic(OsbEasing.OutCubic, startTime, startTime + dur, Math.PI * direction);
            field.ScaleReceptor(OsbEasing.None, startTime, startTime, new Vector2(scale), ColumnType.all);
            if (dur == 2500)
            {
                field.ScaleReceptor(OsbEasing.OutCubic, startTime, startTime + dur, new Vector2(0f), ColumnType.all);
                return;
            }
            field.ScaleReceptor(OsbEasing.OutCubic, startTime, startTime + dur, new Vector2(0.5f), ColumnType.all);

            direction *= -1;
        }

        private double SpinOutAnimation(Playfield field, double startTime, double endTime, float widthValue, float heightValue, double initialDuration = 100, double growthFactor = 1.5)
        {
            double currentTime = startTime;
            double remainingTime = endTime - startTime;
            double currentDuration = initialDuration;
            float currentWidth = widthValue;
            bool positiveWidthAtEnd = true;

            // Plan the durations ahead of time to make sure we hit exactly endTime
            List<double> durations = new List<double>();
            double tempDur = currentDuration;
            double tempTime = 0;

            // Calculate how many cycles we can fit
            while (tempTime + tempDur <= remainingTime)
            {
                durations.Add(tempDur);
                tempTime += tempDur;
                tempDur *= growthFactor;
            }

            // Apply the animation for each cycle
            for (int i = 0; i < durations.Count; i++)
            {
                double duration = durations[i];

                var scaleReduction = 0.15f;
                var fullReduction = scaleReduction / 2 + scaleReduction;

                if (i % 2 == 0)
                {
                    field.ScaleColumn(OsbEasing.OutSine, currentTime, currentTime + duration / 2, new Vector2(0.5f - fullReduction), ColumnType.one);
                    field.ScaleColumn(OsbEasing.OutSine, currentTime, currentTime + duration / 2, new Vector2(0.5f - scaleReduction), ColumnType.two);
                    field.ScaleColumn(OsbEasing.OutSine, currentTime, currentTime + duration / 2, new Vector2(0.5f + scaleReduction), ColumnType.three);
                    field.ScaleColumn(OsbEasing.OutSine, currentTime, currentTime + duration / 2, new Vector2(0.5f + fullReduction), ColumnType.four);

                    field.ScaleColumn(OsbEasing.InSine, currentTime + duration / 2, currentTime + duration, new Vector2(0.5f), ColumnType.one);
                    field.ScaleColumn(OsbEasing.InSine, currentTime + duration / 2, currentTime + duration, new Vector2(0.5f), ColumnType.two);
                    field.ScaleColumn(OsbEasing.InSine, currentTime + duration / 2, currentTime + duration, new Vector2(0.5f), ColumnType.three);
                    field.ScaleColumn(OsbEasing.InSine, currentTime + duration / 2, currentTime + duration, new Vector2(0.5f), ColumnType.four);
                }
                else
                {
                    field.ScaleColumn(OsbEasing.OutSine, currentTime, currentTime + duration / 2, new Vector2(0.5f - fullReduction), ColumnType.four);
                    field.ScaleColumn(OsbEasing.OutSine, currentTime, currentTime + duration / 2, new Vector2(0.5f - scaleReduction), ColumnType.three);
                    field.ScaleColumn(OsbEasing.OutSine, currentTime, currentTime + duration / 2, new Vector2(0.5f + scaleReduction), ColumnType.two);
                    field.ScaleColumn(OsbEasing.OutSine, currentTime, currentTime + duration / 2, new Vector2(0.5f + fullReduction), ColumnType.one);

                    field.ScaleColumn(OsbEasing.InSine, currentTime + duration / 2, currentTime + duration, new Vector2(0.5f), ColumnType.four);
                    field.ScaleColumn(OsbEasing.InSine, currentTime + duration / 2, currentTime + duration, new Vector2(0.5f), ColumnType.three);
                    field.ScaleColumn(OsbEasing.InSine, currentTime + duration / 2, currentTime + duration, new Vector2(0.5f), ColumnType.two);
                    field.ScaleColumn(OsbEasing.InSine, currentTime + duration / 2, currentTime + duration, new Vector2(0.5f), ColumnType.one);
                }

                field.RotateReceptor(OsbEasing.OutSine, currentTime, currentTime + duration / 2, -.3, CenterType.receptor);
                field.RotateReceptor(OsbEasing.InSine, currentTime + duration / 2, currentTime + duration, .3, CenterType.receptor);
                field.Resize(OsbEasing.InOutSine, currentTime, currentTime + duration, currentWidth, heightValue);

                currentWidth *= -1;
                currentTime += duration;
            }

            // If we ended with a negative width, do a quick flip back to positive
            if (currentWidth < 0)
            {
                // We'd normally need another flip, but we're at the end time already
                // So just return the absolute value
                currentWidth = Math.Abs(currentWidth);
            }

            return currentWidth;
        }

        public void ReceptorBump(Playfield playfiled, double localtime, float diff = 2, bool isInverted = false, double gap = 43801 - 43414)
        {

            if (localtime < 230479)
            {
                playfiled.RotatePlayFieldStatic(OsbEasing.None, localtime - 10, localtime - 10, Math.PI);
                playfiled.RotatePlayFieldStatic(OsbEasing.OutCubic, localtime, localtime + gap, Math.PI);
            }

            float defaultScale = 0.5f;
            float smallScale = defaultScale / diff;
            float bigScale = defaultScale * diff;

            var colWidth = playfiled.getColumnWidth() * 2 / 2 * diff;
            var halfwidth = playfiled.getColumnWidth() / 2 / 2 * diff;

            if (isInverted)
            {
                colWidth *= -1;
                halfwidth *= -1;
            }

            playfiled.ScaleReceptor(OsbEasing.None, localtime, localtime, new Vector2(bigScale), ColumnType.all);
            playfiled.ScaleOrigin(OsbEasing.None, localtime, localtime, new Vector2(smallScale), ColumnType.all);

            playfiled.ScaleReceptor(OsbEasing.OutCubic, localtime, localtime + gap, new Vector2(defaultScale), ColumnType.all);
            playfiled.ScaleOrigin(OsbEasing.OutCubic, localtime, localtime + gap, new Vector2(defaultScale), ColumnType.all);

            if (localtime < 249229)
            {
                playfiled.MoveReceptorRelative(OsbEasing.None, localtime - 10, localtime - 10, new Vector2(-colWidth, 0), ColumnType.one);
                playfiled.MoveReceptorRelative(OsbEasing.OutCubic, localtime, localtime + gap, new Vector2(colWidth, 0), ColumnType.one);
                playfiled.MoveReceptorRelative(OsbEasing.None, localtime - 10, localtime - 10, new Vector2(-halfwidth, 0), ColumnType.two);
                playfiled.MoveReceptorRelative(OsbEasing.OutCubic, localtime, localtime + gap, new Vector2(halfwidth, 0), ColumnType.two);
                playfiled.MoveReceptorRelative(OsbEasing.None, localtime - 10, localtime - 10, new Vector2(halfwidth, 0), ColumnType.three);
                playfiled.MoveReceptorRelative(OsbEasing.OutCubic, localtime, localtime + gap, new Vector2(-halfwidth, 0), ColumnType.three);
                playfiled.MoveReceptorRelative(OsbEasing.None, localtime - 10, localtime - 10, new Vector2(colWidth, 0), ColumnType.four);
                playfiled.MoveReceptorRelative(OsbEasing.OutCubic, localtime, localtime + gap, new Vector2(-colWidth, 0), ColumnType.four);

                playfiled.MoveOriginRelative(OsbEasing.None, localtime - 10, localtime + gap - 10, new Vector2(colWidth / 4, 0), ColumnType.one);
                playfiled.MoveOriginRelative(OsbEasing.OutCubic, localtime, localtime + gap, new Vector2(-colWidth / 4, 0), ColumnType.one);
                playfiled.MoveOriginRelative(OsbEasing.None, localtime - 10, localtime + gap - 10, new Vector2(halfwidth / 4, 0), ColumnType.two);
                playfiled.MoveOriginRelative(OsbEasing.OutCubic, localtime, localtime + gap, new Vector2(-halfwidth / 4, 0), ColumnType.two);
                playfiled.MoveOriginRelative(OsbEasing.None, localtime - 10, localtime + gap - 10, new Vector2(-halfwidth / 4, 0), ColumnType.three);
                playfiled.MoveOriginRelative(OsbEasing.OutCubic, localtime, localtime + gap, new Vector2(halfwidth / 4, 0), ColumnType.three);
                playfiled.MoveOriginRelative(OsbEasing.None, localtime - 10, localtime + gap - 10, new Vector2(-colWidth / 4, 0), ColumnType.four);
                playfiled.MoveOriginRelative(OsbEasing.OutCubic, localtime, localtime + gap, new Vector2(colWidth / 4, 0), ColumnType.four);
            }

        }

        public void flipColumn(Playfield field, double starttime, double duration, OsbEasing easing, ColumnType type)
        {
            foreach (Column currentColumn in field.columns.Values)
            {

                if (currentColumn.type == type || type == ColumnType.all)
                {

                    Vector2 receptorPos = currentColumn.ReceptorPositionAt(starttime);
                    Vector2 originPos = currentColumn.OriginPositionAt(starttime);
                    Vector2 center = new Vector2(427, 240);

                    // Calculate the change needed to flip the positions
                    Vector2 changeReceptorPos = (center - receptorPos) * 2;
                    Vector2 changeOriginPos = (center - originPos) * 2;

                    currentColumn.receptor.MoveReceptorRelativeY(easing, starttime, starttime + duration, changeReceptorPos.Y);
                    currentColumn.origin.MoveOriginRelativeY(easing, starttime, starttime + duration, changeOriginPos.Y);
                }

            }
        }

        private void FlipPlayField(Playfield field, double startTime, double endTime, float widthValue, float heightValue, double initialDuration = 100, double growthFactor = 1.5)
        {
            double currentTime = startTime;
            double remainingTime = endTime - startTime;
            double currentDuration = initialDuration;
            float currentWidth = widthValue;
            bool positiveWidthAtEnd = true;

            // Plan the durations ahead of time to make sure we hit exactly endTime
            List<double> durations = new List<double>();
            double tempDur = currentDuration;
            double tempTime = 0;

            // Calculate how many cycles we can fit
            while (tempTime + tempDur <= remainingTime)
            {
                durations.Add(tempDur);
                tempTime += tempDur;
                tempDur *= growthFactor;
            }

            // Apply the animation for each cycle
            for (int i = 0; i < durations.Count; i++)
            {
                double duration = durations[i];

                foreach (var col in field.columns.Values)
                {
                    Vector2 receptorPos = col.ReceptorPositionAt(currentTime);
                    Vector2 originPos = col.OriginPositionAt(currentTime);
                    Vector2 center = new Vector2(427, 240);

                    // Calculate the change needed to flip the positions
                    Vector2 changeReceptorPos = (center - receptorPos) * 2;
                    Vector2 changeOriginPos = (center - originPos) * 2;

                    var colWidth = field.getColumnWidth();

                    if (i % 2 == 0)
                    {
                        field.ScaleReceptor(OsbEasing.OutSine, currentTime, currentTime + duration / 2, new Vector2(1f), col.type);
                        field.ScaleOrigin(OsbEasing.OutSine, currentTime, currentTime + duration / 2, new Vector2(0.25f), col.type);

                        field.ScaleReceptor(OsbEasing.InSine, currentTime + duration / 2, currentTime + duration, new Vector2(0.5f), col.type);
                        field.ScaleOrigin(OsbEasing.InSine, currentTime + duration / 2, currentTime + duration, new Vector2(0.5f), col.type);
                    }
                    else
                    {
                        colWidth = -colWidth;
                        field.ScaleReceptor(OsbEasing.OutSine, currentTime, currentTime + duration / 2, new Vector2(0.25f), col.type);
                        field.ScaleOrigin(OsbEasing.OutSine, currentTime, currentTime + duration / 2, new Vector2(1f), col.type);

                        field.ScaleReceptor(OsbEasing.InSine, currentTime + duration / 2, currentTime + duration, new Vector2(0.5f), col.type);
                        field.ScaleOrigin(OsbEasing.InSine, currentTime + duration / 2, currentTime + duration, new Vector2(0.5f), col.type);
                    }

                    switch (col.type)
                    {
                        case ColumnType.one:
                            break;
                        case ColumnType.two:
                            colWidth = colWidth / 2;
                            break;
                        case ColumnType.three:
                            colWidth = -colWidth / 2;
                            break;
                        case ColumnType.four:
                            colWidth = -colWidth;
                            break;
                    }

                    col.receptor.MoveReceptorRelativeX(OsbEasing.OutSine, currentTime, currentTime + duration / 2, colWidth);
                    col.receptor.MoveReceptorRelativeX(OsbEasing.InSine, currentTime + duration / 2, currentTime + duration, -colWidth);

                    col.origin.MoveOriginRelativeX(OsbEasing.OutSine, currentTime, currentTime + duration / 2, -colWidth);
                    col.origin.MoveOriginRelativeX(OsbEasing.InSine, currentTime + duration / 2, currentTime + duration, colWidth);

                    col.receptor.MoveReceptorRelativeY(OsbEasing.OutSine, currentTime, currentTime + duration, changeReceptorPos.Y);
                    col.origin.MoveOriginRelativeY(OsbEasing.OutSine, currentTime, currentTime + duration, changeOriginPos.Y);
                }

                currentTime += duration;
            }
        }

        // NoteFunction is used to manipulate the pathway and a bunch of other things the note should do on their way to the receptor
        // Please be warry that this is beeing run async so you need to keep thread safety in mind when working on complex Functions.
        // You can use the progress to determin how far the note is in its cycle 0 = just start | 1 = ontop of receptor / finished
        // Special flags for hold bodies exist
        public Vector2 NoteFunction(EquationParameters p)
        {

            if (((p.time >= 5479 && p.time <= 6640) || (p.time >= 11672 && p.time <= 12834)) && p.isHoldBody == false)
            {
                lock (_lock)
                {
                    if (p.progress < 0.99)
                    {
                        StoryboardLayer layer = p.column.receptor.layer;
                        String notePath = p.note.noteSprite.TexturePath;
                        if (p.time < 6592)
                        {
                            var shadow = layer.CreateSprite(notePath, OsbOrigin.Centre, p.lastPosition);
                            shadow.ScaleVec(p.time, p.column.receptor.ScaleAt(p.time));
                            shadow.Fade(p.time, 1);
                            shadow.Fade(6640, 0);
                            shadow.Rotate(OsbEasing.None, p.time, p.time + 1000, p.column.receptor.RotationAt(p.time), p.column.receptor.RotationAt(p.time) + Math.PI);
                        }
                        else if (p.time > 7221 && p.time < 12785)
                        {
                            var shadow = layer.CreateSprite(notePath, OsbOrigin.Centre, p.lastPosition);
                            shadow.ScaleVec(p.time, p.column.receptor.ScaleAt(p.time));
                            shadow.Fade(p.time, 1);
                            shadow.Fade(12834, 0);
                            shadow.Rotate(OsbEasing.None, p.time, p.time + 1000, p.column.receptor.RotationAt(p.time), p.column.receptor.RotationAt(p.time) + Math.PI);
                        }
                    }

                }
            }

            return p.position;
        }
    }
}
