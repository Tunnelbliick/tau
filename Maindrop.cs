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

namespace StorybrewScripts
{
    public class Maindrop : StoryboardObjectGenerator
    {
        public override bool Multithreaded => false;
        private readonly object _lock = new object();
        public override void Generate()
        {

            var receptors = GetLayer("r");
            var notes = GetLayer("n");

            // General values
            var starttime = 130898; // the starttime where the playfield is initialized
            var endtime = 156446; // the endtime where the playfield is nolonger beeing rendered
            var duration = endtime - starttime; // the length the playfield is kept alive

            // Playfield Scale
            var width = 250f; // widht of the playfield / invert to flip
            var height = 500f; // height of the playfield / invert to flip -600 = downscroll | 600 = upscropll
            var receptorWallOffset = 50f; // how big the boundary box for the receptor is 50 means it will be pushed away 50 units from the wall

            // Note initilization Values
            var sliderAccuracy = 25; // The Segment length for sliderbodies since they are rendered in slices 30 is default
            var isColored = false; // This property is used if you want to color the notes by urself for effects. It does not swap if the snap coloring is used.

            // Drawinstance Values
            var updatesPerSecond = 250; // The amount of steps the rendring engine does to render out note and receptor positions
            var scrollSpeed = 1000f; // The speed at which the Notes scroll
            var fadeTime = 150; // The time notes will fade in

            var cols = 5;
            var scrollWidth = 252f * cols;

            var leftBound = 252f * -1 - 125f;
            var rightBound = 252f * 3 + 125f;

            for (int i = 0; i < cols; i++)
            {
                CancellationToken.ThrowIfCancellationRequested();

                bool leftAdjusted = false;
                bool rightAdjusted = false;

                Playfield field = new Playfield();
                field.delta = 10;
                field.initilizePlayField(receptors, notes, starttime, endtime, width, height, receptorWallOffset, Beatmap.OverallDifficulty);
                if (i != 0)
                {
                    field.noteEnd = 156253;
                }
                field.initializeNotes(Beatmap.HitObjects.ToList(), Beatmap, isColored, sliderAccuracy);

                var initialPos = i - 2;
                var initialX = 252f * initialPos;
                var halfTime = (131672 - 130898) / 2;
                var ranY = 15 * initialPos;

                var movementPerDuration = 252f / (132446 - 132059);

                var colWidth = field.getColumnWidth() * 1.5f;

                Log(field.getColumnWidth());

                field.moveFieldX(OsbEasing.InCubic, 130705, 131672, initialX);
                field.moveFieldY(OsbEasing.OutSine, 130898, 130898 + halfTime, ranY);
                field.moveFieldY(OsbEasing.InOutSine, 130898 + halfTime, 131672, -ranY);
                ReceptorBump(field, 130705, 5, 131672 - 130705);

                field.RotateReceptor(OsbEasing.None, 131672, 131672 + 85, 0.4f, CenterType.receptor);
                field.RotateReceptor(OsbEasing.None, 131672 + 100, 132059 - 10, -0.4f, CenterType.receptor);
                ReceptorBump(field, 131672, 3);

                field.RotatePlayFieldStatic(OsbEasing.None, 132059, 132059, Math.PI);
                field.RotatePlayFieldStatic(OsbEasing.None, 132059, 132446, Math.PI);
                field.moveFieldX(OsbEasing.None, 132059, 132446, movementPerDuration * (132446 - 132059));

                field.MoveReceptorRelative(OsbEasing.OutCubic, 132446, 132640, new Vector2(colWidth, 0), ColumnType.all);
                field.MoveOriginRelative(OsbEasing.OutCubic, 132446, 132640, new Vector2(-colWidth, 0), ColumnType.all);

                field.MoveReceptorRelative(OsbEasing.OutCubic, 132640, 132834, new Vector2(-colWidth, 0), ColumnType.all);
                field.MoveOriginRelative(OsbEasing.OutCubic, 132640, 132834, new Vector2(colWidth, 0), ColumnType.all);

                field.MoveReceptorRelative(OsbEasing.OutCubic, 132834, 133027, new Vector2(colWidth, 0), ColumnType.all);
                field.MoveOriginRelative(OsbEasing.OutCubic, 132834, 133027, new Vector2(-colWidth, 0), ColumnType.all);

                field.MoveReceptorRelative(OsbEasing.OutCubic, 133027, 133221, new Vector2(-colWidth, 0), ColumnType.all);
                field.MoveOriginRelative(OsbEasing.OutCubic, 133027, 133221, new Vector2(colWidth, 0), ColumnType.all);

                field.MoveReceptorRelative(OsbEasing.OutCubic, 133221, 133414, new Vector2(colWidth, 0), ColumnType.all);
                field.MoveOriginRelative(OsbEasing.OutCubic, 133221, 133414, new Vector2(-colWidth, 0), ColumnType.all);

                field.MoveReceptorRelative(OsbEasing.OutCubic, 133414, 133608, new Vector2(-colWidth, 0), ColumnType.all);
                field.MoveOriginRelative(OsbEasing.OutCubic, 133414, 133608, new Vector2(colWidth, 0), ColumnType.all);

                field.moveFieldX(OsbEasing.OutSine, 133608, 134575, -movementPerDuration * (134575 - 133608));
                field.RotatePlayFieldStatic(OsbEasing.OutSine, 133608, 134575, -Math.PI * 2);

                field.Resize(OsbEasing.OutElasticQuarter, 133705 + (133801 - 133705) * i, 133801 + (133801 - 133705) * i * 3, width, -height);
                field.moveFieldY(OsbEasing.OutElasticQuarter, 133705 + (133801 - 133705) * i, 133801 + (133801 - 133705) * i * 3, 480 - receptorWallOffset * 2);

                field.RotateReceptor(OsbEasing.None, 134769, 134769 + 85, 0.5f, CenterType.receptor);
                field.RotateReceptor(OsbEasing.OutSine, 134769 + 100, 134769 + 100 + 387 - 10, -0.5f, CenterType.receptor);
                ReceptorBump(field, 134769, 3);

                field.RotatePlayFieldStatic(OsbEasing.OutCubic, 134575, 135350, Math.PI * 2 + 0.2f);
                field.moveFieldX(OsbEasing.OutCubic, 134575, 135156, movementPerDuration * (135156 - 134575));

                field.moveFieldX(OsbEasing.None, 134575, 137479, movementPerDuration * (137479 - 134575));

                field.moveFieldX(OsbEasing.OutCubic, 137479, 137866, -movementPerDuration * (137866 - 137479));
                field.RotatePlayFieldStatic(OsbEasing.OutCubic, 137479, 137866, -Math.PI * 2 - 0.2f);

                int offset = 3; // The starting offset
                int o = (i + offset) % cols;

                field.Resize(OsbEasing.OutElasticQuarter, 139995 + (133801 - 133705) * o, 140092 + (133801 - 133705) * o * 3, width, height);
                field.moveFieldY(OsbEasing.OutElasticQuarter, 139995 + (133801 - 133705) * o, 140092 + (133801 - 133705) * o * 3, -480 + receptorWallOffset * 2);

                field.RotateReceptor(OsbEasing.None, 137866, 137866 + 85, -0.5f, CenterType.receptor);
                field.RotateReceptor(OsbEasing.OutSine, 137866 + 100, 137866 + 100 + 387 - 10, 0.5f, CenterType.receptor);
                ReceptorBump(field, 137866, 3);

                field.MoveReceptorRelative(OsbEasing.OutCubic, 138640, 138834, new Vector2(colWidth, 0), ColumnType.all);
                field.MoveOriginRelative(OsbEasing.OutCubic, 138640, 138834, new Vector2(-colWidth, 0), ColumnType.all);

                field.MoveReceptorRelative(OsbEasing.OutCubic, 138834, 139027, new Vector2(-colWidth, 0), ColumnType.all);
                field.MoveOriginRelative(OsbEasing.OutCubic, 138834, 139027, new Vector2(colWidth, 0), ColumnType.all);

                field.MoveReceptorRelative(OsbEasing.OutCubic, 139027, 139221, new Vector2(colWidth, 0), ColumnType.all);
                field.MoveOriginRelative(OsbEasing.OutCubic, 139027, 139221, new Vector2(-colWidth, 0), ColumnType.all);

                field.MoveReceptorRelative(OsbEasing.OutCubic, 139221, 139414, new Vector2(-colWidth, 0), ColumnType.all);
                field.MoveOriginRelative(OsbEasing.OutCubic, 139221, 139414, new Vector2(colWidth, 0), ColumnType.all);

                field.MoveReceptorRelative(OsbEasing.OutCubic, 139414, 139608, new Vector2(colWidth, 0), ColumnType.all);
                field.MoveOriginRelative(OsbEasing.OutCubic, 139414, 139608, new Vector2(-colWidth, 0), ColumnType.all);

                field.MoveReceptorRelative(OsbEasing.OutCubic, 139608, 139801, new Vector2(-colWidth, 0), ColumnType.all);
                field.MoveOriginRelative(OsbEasing.OutCubic, 139608, 139801, new Vector2(colWidth, 0), ColumnType.all);

                field.moveFieldX(OsbEasing.OutSine, 139801, 140769, movementPerDuration * (140769 - 139801));
                //field.RotatePlayFieldStatic(OsbEasing.OutSine, 139801, 140769, Math.PI * 2);


                field.RotateReceptor(OsbEasing.None, 140963, 140963 + 85, -0.4f, CenterType.receptor);
                field.RotateReceptor(OsbEasing.OutSine, 140963 + 100, 140963 + 100 + 387 - 10, 0.4f, CenterType.receptor);
                ReceptorBump(field, 140963, 3);

                field.RotatePlayFieldStatic(OsbEasing.OutCubic, 140769, 141543, -Math.PI * 2 - 0.2f);
                field.moveFieldX(OsbEasing.OutCubic, 140769, 141156, -movementPerDuration * (141156 - 140769));

                field.moveFieldX(OsbEasing.None, 140769, 142317, -movementPerDuration * (142317 - 140769));
                //field.RotatePlayFieldStatic(OsbEasing.None, 140963, 142317, -Math.PI * 2);

                var start = 142317;
                var actuallDur = 142608 - start;
                var dur = 142608 - 142317 - 20;
                var halfDur = dur / 2;

                field.RotatePlayFieldStatic(OsbEasing.OutCubic, start, start + halfDur, 0.2f);

                field.RotateReceptor(OsbEasing.OutSine, start, start + halfDur - 10, -.2, CenterType.receptor);
                field.RotateReceptor(OsbEasing.InSine, start + halfDur, start + dur - 10, .2, CenterType.receptor);
                ReceptorBump(field, start, 2);
                field.Resize(OsbEasing.OutSine, start, start + dur, -width, height);

                start += actuallDur;
                field.RotateReceptor(OsbEasing.OutSine, start, start + halfDur - 10, -.2, CenterType.receptor);
                field.RotateReceptor(OsbEasing.InSine, start + halfDur, start + dur - 10, .2, CenterType.receptor);
                ReceptorBump(field, start, 2);
                field.Resize(OsbEasing.OutSine, start, start + dur, width, height);

                start += actuallDur;
                field.RotateReceptor(OsbEasing.OutSine, start, start + halfDur - 10, -.2, CenterType.receptor);
                field.RotateReceptor(OsbEasing.InSine, start + halfDur, start + dur - 10, .2, CenterType.receptor);
                ReceptorBump(field, start, 2);
                field.Resize(OsbEasing.OutSine, start, start + dur, -width, height);

                start += actuallDur;
                field.RotateReceptor(OsbEasing.OutSine, start, start + halfDur - 10, -.2, CenterType.receptor);
                field.RotateReceptor(OsbEasing.InSine, start + halfDur, start + dur - 10, .2, CenterType.receptor);
                ReceptorBump(field, start, 2);
                field.Resize(OsbEasing.OutSine, start, start + dur, width, height);

                field.moveFieldX(OsbEasing.OutSine, 142317, 143188, -252f * initialPos + 243);

                field.moveFieldX(OsbEasing.OutSine, 143672, 144059, 252f * initialPos - 243);
                //ReceptorBump(field, 143672, 3, (144059 - 143672) * 0.95f);

                start = 144253;
                dur = (144640 - start) * 2;
                halfDur = dur / 2;

                field.RotatePlayFieldStatic(OsbEasing.OutSine, start, start, -.15f);

                // Split movement into two parts: constant speed and OutSine finish
                double linearEndTime = 154479; // 1000ms before end time
                double totalDistance = movementPerDuration * (154898 - 143672);   // Use your desired total distance

                // Calculate distances for each segment
                double linearDuration = linearEndTime - 143672;
                double totalDuration = 154898 - 143672;
                double linearRatio = linearDuration / totalDuration;

                // Linear movement from start to linearEndTime
                double linearDistance = totalDistance * linearRatio;
                field.moveFieldX(OsbEasing.None, 143672, linearEndTime, (float)linearDistance + 150);

                // OutSine movement for the final 1000ms
                double sineDistance = totalDistance - linearDistance;
                field.moveFieldX(OsbEasing.OutSine, linearEndTime, 154898, (float)sineDistance - 140);

                double wrapStart = 132059;
                var gap = field.delta;
                

                while (wrapStart < 156446)
                {
                    var leftAdjust = 0;
                    var rightAdjust = 0;

                    CancellationToken.ThrowIfCancellationRequested();

                    var xPos = field.calculatePlayFieldCenter(wrapStart).X;

                    if (xPos < leftBound)
                    {
                        // Move each column to the left by transitionwidth
                        foreach (var col in field.columns.Values)
                        {
                            col.receptor.renderedSprite.Fade(wrapStart - 10, 0);
                            col.receptor.renderedSprite.Fade(wrapStart + 10, 1);
                        }

                        field.fadeAt(wrapStart - 10, 0);
                        field.fadeAt(wrapStart + 10, 1);

                        field.moveFieldX(OsbEasing.None, wrapStart, wrapStart, scrollWidth);
                    }

                    if (xPos > rightBound)
                    {
                        // Move each column to the left by transitionwidth
                        foreach (var col in field.columns.Values)
                        {
                            col.receptor.renderedSprite.Fade(wrapStart - 10, 0);
                            col.receptor.renderedSprite.Fade(wrapStart + 10, 1);
                        }

                        field.fadeAt(wrapStart - 10, 0);
                        field.fadeAt(wrapStart + 10, 1);

                        field.moveFieldX(OsbEasing.None, wrapStart, wrapStart, -scrollWidth);
                    }

                    /*
                    foreach (var col in field.columns.Values)
                    {
                        CancellationToken.ThrowIfCancellationRequested();
                        var colPos = col.ReceptorPositionAt(wrapStart).X;


                        if (colPos < leftBound)
                        {
                            // Move each column to the left by transitionwidth
                            col.receptor.renderedSprite.Fade(wrapStart - 10, 0);
                            col.receptor.renderedSprite.Fade(wrapStart + 10, 1);

                            var x = Math.Abs(colPos);
                            field.MoveColumnRelativeX(OsbEasing.None, wrapStart, wrapStart, scrollWidth, col.type);
                        }
                        else if (colPos > rightBound)
                        {
                            // Move each column to the left by transitionwidth
                            col.receptor.renderedSprite.Fade(wrapStart - 10, 0);
                            col.receptor.renderedSprite.Fade(wrapStart + 10, 1);

                            field.MoveColumnRelativeX(OsbEasing.None, wrapStart, wrapStart, -scrollWidth, col.type);
                        }
                    }*/
                    wrapStart += gap;
                }

                while (start < 153059)
                {
                    field.MoveColumnRelativeX(OsbEasing.OutSine, start, start + 25, field.getColumnWidth() * 2, ColumnType.one);
                    field.MoveColumnRelativeX(OsbEasing.OutSine, start, start + 25, field.getColumnWidth() * 2, ColumnType.two);
                    field.MoveColumnRelativeX(OsbEasing.OutSine, start, start + 25, -field.getColumnWidth() * 2, ColumnType.three);
                    field.MoveColumnRelativeX(OsbEasing.OutSine, start, start + 25, -field.getColumnWidth() * 2, ColumnType.four);
                    //field.Resize(OsbEasing.OutSine, start, start + 25, -width, height);
                    field.RotatePlayFieldStatic(OsbEasing.OutSine, start, start + 25, .3f);
                    start += halfDur;
                    if (start < 153059)
                    {
                        field.MoveColumnRelativeX(OsbEasing.OutSine, start, start + 25, -field.getColumnWidth() * 2, ColumnType.one);
                        field.MoveColumnRelativeX(OsbEasing.OutSine, start, start + 25, -field.getColumnWidth() * 2, ColumnType.two);
                        field.MoveColumnRelativeX(OsbEasing.OutSine, start, start + 25, field.getColumnWidth() * 2, ColumnType.three);
                        field.MoveColumnRelativeX(OsbEasing.OutSine, start, start + 25, field.getColumnWidth() * 2, ColumnType.four);
                        // field.Resize(OsbEasing.OutSine, start, start + 25, width, height);
                        field.RotatePlayFieldStatic(OsbEasing.OutSine, start, start + 25, -.3f);
                        start += halfDur;
                    }
                }

                offset = 3; // The starting offset
                int invertedIndex = (cols + i - offset) % cols;

                flipColumn(field, 146188 + (133801 - 133705) * invertedIndex, (133801 - 133705) * invertedIndex * 3, OsbEasing.OutElasticQuarter, ColumnType.all);

                offset = 2; // The starting offset
                invertedIndex = (cols + i - offset) % cols;

                flipColumn(field, 152382 + (133801 - 133705) * invertedIndex, (133801 - 133705) * invertedIndex * 3, OsbEasing.OutElasticQuarter, ColumnType.all);

                field.MoveColumnRelativeX(OsbEasing.OutSine, 153350, 153350 + 25, -field.getColumnWidth() * 2, ColumnType.one);
                field.MoveColumnRelativeX(OsbEasing.OutSine, 153350, 153350 + 25, -field.getColumnWidth() * 2, ColumnType.two);
                field.MoveColumnRelativeX(OsbEasing.OutSine, 153350, 153350 + 25, field.getColumnWidth() * 2, ColumnType.three);
                field.MoveColumnRelativeX(OsbEasing.OutSine, 153350, 153350 + 25, field.getColumnWidth() * 2, ColumnType.four);

                field.MoveColumnRelativeX(OsbEasing.OutSine, 153640, 153640 + 25, field.getColumnWidth() * 2, ColumnType.one);
                field.MoveColumnRelativeX(OsbEasing.OutSine, 153640, 153640 + 25, field.getColumnWidth() * 2, ColumnType.two);
                field.MoveColumnRelativeX(OsbEasing.OutSine, 153640, 153640 + 25, -field.getColumnWidth() * 2, ColumnType.three);
                field.MoveColumnRelativeX(OsbEasing.OutSine, 153640, 153640 + 25, -field.getColumnWidth() * 2, ColumnType.four);

                field.MoveColumnRelativeX(OsbEasing.OutSine, 154124, 154124 + 25, -field.getColumnWidth() * 2, ColumnType.one);
                field.MoveColumnRelativeX(OsbEasing.OutSine, 154124, 154124 + 25, -field.getColumnWidth() * 2, ColumnType.two);
                field.MoveColumnRelativeX(OsbEasing.OutSine, 154124, 154124 + 25, field.getColumnWidth() * 2, ColumnType.three);
                field.MoveColumnRelativeX(OsbEasing.OutSine, 154124, 154124 + 25, field.getColumnWidth() * 2, ColumnType.four);

                field.MoveColumnRelativeX(OsbEasing.OutSine, 154414, 154414 + 25, field.getColumnWidth() * 2, ColumnType.one);
                field.MoveColumnRelativeX(OsbEasing.OutSine, 154414, 154414 + 25, field.getColumnWidth() * 2, ColumnType.two);
                field.MoveColumnRelativeX(OsbEasing.OutSine, 154414, 154414 + 25, -field.getColumnWidth() * 2, ColumnType.three);
                field.MoveColumnRelativeX(OsbEasing.OutSine, 154414, 154414 + 25, -field.getColumnWidth() * 2, ColumnType.four);

                field.RotatePlayFieldStatic(OsbEasing.OutSine, 154898, 156253, -0.15f);
                field.Resize(OsbEasing.OutSine, 154898, 154898, width, height);

                var center = field.calculatePlayFieldCenter(154898);
                var colY = field.columns[ColumnType.one].ReceptorPositionAt(154898).Y;

                field.moveFieldX(OsbEasing.OutSine, 154898, 156446, 320 - center.X);
                field.moveFieldY(OsbEasing.OutSine, 154898, 156446, -(colY - 50));


                DrawInstance draw = new DrawInstance(CancellationToken, field, starttime + 10, scrollSpeed, updatesPerSecond, OsbEasing.None, false, fadeTime, fadeTime);
                draw.setReceptorMovementPrecision(1f);
                draw.setNoteMovementPrecision(1f);
                draw.setNoteRotationPrecision(0.15f);
                draw.setHoldMovementPrecision(0.15f);
                draw.setHoldRotationPrecision(0.1f);
                //draw.CommandSplitThreshold = 50;
                draw.drawViaEquation(endtime - starttime, NoteFunction, true);

            }
        }

        public void ReceptorBump(Playfield field, double localtime, float diff = 2, double gap = 43801 - 43414)
        {

            float defaultScale = 0.5f;
            float smallScale = defaultScale / diff;
            float bigScale = defaultScale * diff;

            var colWidth = field.getColumnWidth() * 2 / 2 * diff;
            var halfwidth = field.getColumnWidth() / 2 / 2 * diff;

            field.ScaleReceptor(OsbEasing.None, localtime, localtime, new Vector2(bigScale), ColumnType.all);
            field.ScaleOrigin(OsbEasing.None, localtime, localtime, new Vector2(smallScale), ColumnType.all);

            field.ScaleReceptor(OsbEasing.OutCubic, localtime, localtime + gap, new Vector2(defaultScale), ColumnType.all);
            field.ScaleOrigin(OsbEasing.OutCubic, localtime, localtime + gap, new Vector2(defaultScale), ColumnType.all);

            field.MoveReceptorRelative(OsbEasing.None, localtime - 10, localtime - 10, new Vector2(-colWidth, 0), ColumnType.one);
            field.MoveReceptorRelative(OsbEasing.OutCubic, localtime, localtime + gap, new Vector2(colWidth, 0), ColumnType.one);
            field.MoveReceptorRelative(OsbEasing.None, localtime - 10, localtime - 10, new Vector2(-halfwidth, 0), ColumnType.two);
            field.MoveReceptorRelative(OsbEasing.OutCubic, localtime, localtime + gap, new Vector2(halfwidth, 0), ColumnType.two);
            field.MoveReceptorRelative(OsbEasing.None, localtime - 10, localtime - 10, new Vector2(halfwidth, 0), ColumnType.three);
            field.MoveReceptorRelative(OsbEasing.OutCubic, localtime, localtime + gap, new Vector2(-halfwidth, 0), ColumnType.three);
            field.MoveReceptorRelative(OsbEasing.None, localtime - 10, localtime - 10, new Vector2(colWidth, 0), ColumnType.four);
            field.MoveReceptorRelative(OsbEasing.OutCubic, localtime, localtime + gap, new Vector2(-colWidth, 0), ColumnType.four);

            field.MoveOriginRelative(OsbEasing.None, localtime - 10, localtime + gap - 10, new Vector2(colWidth / 4, 0), ColumnType.one);
            field.MoveOriginRelative(OsbEasing.OutCubic, localtime, localtime + gap, new Vector2(-colWidth / 4, 0), ColumnType.one);
            field.MoveOriginRelative(OsbEasing.None, localtime - 10, localtime + gap - 10, new Vector2(halfwidth / 4, 0), ColumnType.two);
            field.MoveOriginRelative(OsbEasing.OutCubic, localtime, localtime + gap, new Vector2(-halfwidth / 4, 0), ColumnType.two);
            field.MoveOriginRelative(OsbEasing.None, localtime - 10, localtime + gap - 10, new Vector2(-halfwidth / 4, 0), ColumnType.three);
            field.MoveOriginRelative(OsbEasing.OutCubic, localtime, localtime + gap, new Vector2(halfwidth / 4, 0), ColumnType.three);
            field.MoveOriginRelative(OsbEasing.None, localtime - 10, localtime + gap - 10, new Vector2(-colWidth / 4, 0), ColumnType.four);
            field.MoveOriginRelative(OsbEasing.OutCubic, localtime, localtime + gap, new Vector2(colWidth / 4, 0), ColumnType.four);

            field.moveFieldY(OsbEasing.None, localtime, localtime + gap, 1.7f);
        }

        public void flipColumn(Playfield field, double starttime, double duration, OsbEasing easing, ColumnType type)
        {

            Vector2 receptorPos = field.columns[ColumnType.one].ReceptorPositionAt(starttime);
            Vector2 originPos = field.columns[ColumnType.one].OriginPositionAt(starttime);

            foreach (Column currentColumn in field.columns.Values.ToList())
            {

                if (currentColumn.type == type || type == ColumnType.all)
                {

                    Vector2 center = new Vector2(427, 240);

                    // Calculate the change needed to flip the positions
                    Vector2 changeReceptorPos = (center - receptorPos) * 2;
                    Vector2 changeOriginPos = (center - originPos) * 2;

                    currentColumn.receptor.MoveReceptorRelativeY(easing, starttime, starttime + duration, changeReceptorPos.Y);
                    currentColumn.origin.MoveOriginRelativeY(easing, starttime, starttime + duration, changeOriginPos.Y);
                }

            }
        }

        public Vector2 NoteFunction(EquationParameters p)
        {
            var pos = p.position;

            if ((p.time > 132446 && p.time < 133608) || (p.time > 138640 && p.time < 139801))
            {
                // Adjust the Y position based on progress
                if (p.progress < 0.45f)
                {
                    pos.X = p.column.OriginPositionAt(p.time).X;
                }
                else if (p.progress >= 0.45f && p.progress <= 0.55f)
                {
                    // Smoothly lerp between originPos and normal position
                    float lerpFactor = (p.progress - 0.45f) / (0.55f - 0.45f); // Normalize progress to [0, 1]
                    float eased = (float)OsbEasing.OutSine.Ease(lerpFactor); // Apply easing
                    Vector2 originPos = p.column.OriginPositionAt(p.time);
                    Vector2 receptorPos = p.column.ReceptorPositionAt(p.time);
                    originPos.Y = p.position.Y; // Ensure Y position matches receptor
                    receptorPos.Y = p.position.Y; // Ensure Y position matches receptor
                    pos = Vector2.Lerp(originPos, receptorPos, eased);
                }
                else
                {
                    pos.X = p.column.ReceptorPositionAt(p.time).X;
                }
            }

            return pos;
        }
    }
}
