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
using System.Linq;

namespace StorybrewScripts
{
    public class Maindrop : StoryboardObjectGenerator
    {
        //        public override bool Multithreaded => false;
        public override void Generate()
        {

            var receptors = GetLayer("r");
            var notes = GetLayer("n");

            // General values
            var starttime = 130898; // the starttime where the playfield is initialized
            var endtime = 157414; // the endtime where the playfield is nolonger beeing rendered
            var duration = endtime - starttime; // the length the playfield is kept alive

            // Playfield Scale
            var width = 250f; // widht of the playfield / invert to flip
            var height = 500f; // height of the playfield / invert to flip -600 = downscroll | 600 = upscropll
            var receptorWallOffset = 50f; // how big the boundary box for the receptor is 50 means it will be pushed away 50 units from the wall

            // Note initilization Values
            var sliderAccuracy = 25; // The Segment length for sliderbodies since they are rendered in slices 30 is default
            var isColored = false; // This property is used if you want to color the notes by urself for effects. It does not swap if the snap coloring is used.

            // Drawinstance Values
            var updatesPerSecond = 500; // The amount of steps the rendring engine does to render out note and receptor positions
            var scrollSpeed = 1000f; // The speed at which the Notes scroll
            var fadeTime = 150; // The time notes will fade in

            var cols = 5;
            var scrollWidth = 252f * cols;

            for (int i = 0; i < cols; i++)
            {
                Playfield field = new Playfield();
                field.delta = 10;
                field.initilizePlayField(receptors, notes, starttime, endtime, width, height, receptorWallOffset, Beatmap.OverallDifficulty);
                if (i != 0)
                {
                    field.noteEnd = 156253;
                }
                field.initializeNotes(Beatmap.HitObjects.ToList(), Beatmap, isColored, sliderAccuracy);

                var initialPos = i - 2;
                var halfTime = (131672 - 130898) / 2;
                var ranY = 15 * initialPos;

                var colWidth = field.getColumnWidth() * 2;

                field.moveFieldX(OsbEasing.InCubic, 130705, 131672, 252f * initialPos);
                field.moveFieldY(OsbEasing.OutSine, 130898, 130898 + halfTime, ranY);
                field.moveFieldY(OsbEasing.InOutSine, 130898 + halfTime, 131672, -ranY);
                ReceptorBump(field, 130705, 5, 131672 - 130705);

                field.Rotate(OsbEasing.None, 131672, 131672 + 85, 0.4f);
                field.Rotate(OsbEasing.None, 131672 + 100, 132059, -0.4f);
                ReceptorBump(field, 131672, 3);
                //field.Resize(OsbEasing.OutSine, 132059, 132059 , width, height);

                field.RotatePlayFieldStatic(OsbEasing.None, 132059, 132059, Math.PI);
                field.RotatePlayFieldStatic(OsbEasing.None, 132059, 132446, Math.PI);
                field.moveFieldX(OsbEasing.None, 132059, 132446, 250);

                /*var xValue = field.columns[ColumnType.one].ReceptorPositionAt(132253).X;

                if (xValue > OsuHitObject.WidescreenStoryboardBounds.Right)
                {
                    field.fadeAt(132253 - 10, 0);
                    field.fadeAt(132253 + 10, 1);
                    xValue = field.columns[ColumnType.four].ReceptorPositionAt(132253).X;
                    field.moveFieldY(OsbEasing.None, 132253, 132253, 5);
                    field.moveFieldX(OsbEasing.None, 132253, 132253, (413f - xValue) * 2 - field.getColumnWidth());

                    field.fadeAt(133608 - 10, 0);
                    field.fadeAt(133608 + 10, 1);
                    field.moveFieldY(OsbEasing.None, 133608, 133608, -5);
                    field.moveFieldX(OsbEasing.None, 133608, 133608, -(413f - xValue) * 2 + field.getColumnWidth());
                }

                if (i == 0)
                {

                    field.fadeAt(134188 - 10, 0);
                    field.fadeAt(134188 + 10, 1);
                    field.moveFieldY(OsbEasing.None, 134188, 134188, -5);
                    field.moveFieldX(OsbEasing.None, 134188, 134188, 1229f);

                    field.fadeAt(134672 - 10, 0);
                    field.fadeAt(134672 + 10, 1);
                    field.moveFieldY(OsbEasing.None, 134672, 134672, 5);
                    field.moveFieldX(OsbEasing.None, 134672, 134672, -1229f);
                }*/


                /*field.Resize(OsbEasing.OutCubic, 132446, 132446 + 100, -width, height);
                field.Resize(OsbEasing.OutCubic, 132640, 132640 + 100, width, height);
                field.Resize(OsbEasing.OutCubic, 132834, 132834 + 100, -width, height);
                field.Resize(OsbEasing.OutCubic, 133027, 133027 + 100, width, height);
                field.Resize(OsbEasing.OutCubic, 133221, 133221 + 100, -width, height);
                field.Resize(OsbEasing.OutCubic, 133414, 133414 + 100, width, height);*/

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

                // 2nd iteration
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

                //field.Rotate(OsbEasing.None, 133608, 133608, -.2f);
                field.moveFieldX(OsbEasing.OutSine, 133608, 134575, -500);
                field.RotatePlayFieldStatic(OsbEasing.OutSine, 133608, 134575, -Math.PI * 2);

                field.Rotate(OsbEasing.None, 134769, 134769 + 85, 0.5f, CenterType.receptor);
                field.Rotate(OsbEasing.OutSine, 134769 + 100, 134769 + 100 + 387, -0.5f, CenterType.receptor);
                ReceptorBump(field, 134769, 3);
                //field.Resize(OsbEasing.OutSine, 134769 + 100 + 387, 134769 + 100 + 387, width, height);

                field.RotatePlayFieldStatic(OsbEasing.OutCubic, 134575, 135156, Math.PI * 2);
                field.moveFieldX(OsbEasing.OutCubic, 134575, 135156, 500);

                field.moveFieldX(OsbEasing.None, 134769, 137479, 1500);
                field.RotatePlayFieldStatic(OsbEasing.None, 135156, 137479, Math.PI * 4);

                field.moveFieldX(OsbEasing.OutCubic, 137479, 137866, -400);
                field.RotatePlayFieldStatic(OsbEasing.OutCubic, 137479, 137866, -Math.PI * 2);

                field.Rotate(OsbEasing.None, 137866, 137866 + 85, -0.5f, CenterType.receptor);
                field.Rotate(OsbEasing.OutSine, 137866 + 100, 137866 + 100 + 387, 0.5f, CenterType.receptor);
                ReceptorBump(field, 137866, 3);
                //field.Resize(OsbEasing.OutSine, 137866 + 100 + 387, 137866 + 100 + 387, width, height);

                field.moveFieldX(OsbEasing.OutSine, 139801, 140769, 500);
                field.RotatePlayFieldStatic(OsbEasing.OutSine, 139801, 140769, Math.PI * 2);

                field.RotatePlayFieldStatic(OsbEasing.OutCubic, 140769, 140963, -Math.PI * 2);
                field.moveFieldX(OsbEasing.OutCubic, 140769, 141156, -500);

                field.Rotate(OsbEasing.None, 140963, 140963 + 85, -0.4f, CenterType.receptor);
                field.Rotate(OsbEasing.OutSine, 140963 + 100, 140963 + 100 + 387, .4f, CenterType.receptor);
                ReceptorBump(field, 140963, 3);
                //field.Resize(OsbEasing.OutSine, 140963 + 100 + 387, 140963 + 100 + 387, width, height);

                field.moveFieldX(OsbEasing.None, 140963, 142317, -750);
                field.RotatePlayFieldStatic(OsbEasing.None, 140963, 142317, -Math.PI * 2);

                var start = 142317;
                var actuallDur = 142608 - start;
                var dur = 142608 - 142317 - 20;
                var halfDur = dur / 2;

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

                field.moveFieldX(OsbEasing.InCubic, 142317, 143188, -252f * initialPos + 252f * 2.25f);

                field.moveFieldX(OsbEasing.InCubic, 143672, 144059, 252f * initialPos - 252f * 2.25f);
                ReceptorBump(field, 143672, 3, (144059 - 143672) * 0.95f);

                start = 144253;
                dur = (144640 - start) * 2;
                halfDur = dur / 2;

                field.RotatePlayFieldStatic(OsbEasing.OutSine, start, start, -.15f);

                // Split movement into two parts: constant speed and OutSine finish
                double linearEndTime = 154479; // 1000ms before end time
                double totalDistance = 6000;   // Use your desired total distance

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

                while (start < 153059)
                {
                    field.MoveColumnRelativeX(OsbEasing.OutSine, start, start + 50, field.getColumnWidth() * 2, ColumnType.one);
                    field.MoveColumnRelativeX(OsbEasing.OutSine, start, start + 50, field.getColumnWidth() * 2, ColumnType.two);
                    field.MoveColumnRelativeX(OsbEasing.OutSine, start, start + 50, -field.getColumnWidth() * 2, ColumnType.three);
                    field.MoveColumnRelativeX(OsbEasing.OutSine, start, start + 50, -field.getColumnWidth() * 2, ColumnType.four);
                    //field.Resize(OsbEasing.OutSine, start, start + 50, -width, height);
                    field.RotatePlayFieldStatic(OsbEasing.OutSine, start, start + 50, .3f);
                    start += halfDur;
                    if (start < 153059)
                    {
                        field.MoveColumnRelativeX(OsbEasing.OutSine, start, start + 50, -field.getColumnWidth() * 2, ColumnType.one);
                        field.MoveColumnRelativeX(OsbEasing.OutSine, start, start + 50, -field.getColumnWidth() * 2, ColumnType.two);
                        field.MoveColumnRelativeX(OsbEasing.OutSine, start, start + 50, field.getColumnWidth() * 2, ColumnType.three);
                        field.MoveColumnRelativeX(OsbEasing.OutSine, start, start + 50, field.getColumnWidth() * 2, ColumnType.four);
                        // field.Resize(OsbEasing.OutSine, start, start + 50, width, height);
                        field.RotatePlayFieldStatic(OsbEasing.OutSine, start, start + 50, -.3f);
                        start += halfDur;
                    }
                }

                field.MoveColumnRelativeX(OsbEasing.OutSine, 153350, 153350 + 50, -field.getColumnWidth() * 2, ColumnType.one);
                field.MoveColumnRelativeX(OsbEasing.OutSine, 153350, 153350 + 50, -field.getColumnWidth() * 2, ColumnType.two);
                field.MoveColumnRelativeX(OsbEasing.OutSine, 153350, 153350 + 50, field.getColumnWidth() * 2, ColumnType.three);
                field.MoveColumnRelativeX(OsbEasing.OutSine, 153350, 153350 + 50, field.getColumnWidth() * 2, ColumnType.four);

                field.MoveColumnRelativeX(OsbEasing.OutSine, 153640, 153640 + 50, field.getColumnWidth() * 2, ColumnType.one);
                field.MoveColumnRelativeX(OsbEasing.OutSine, 153640, 153640 + 50, field.getColumnWidth() * 2, ColumnType.two);
                field.MoveColumnRelativeX(OsbEasing.OutSine, 153640, 153640 + 50, -field.getColumnWidth() * 2, ColumnType.three);
                field.MoveColumnRelativeX(OsbEasing.OutSine, 153640, 153640 + 50, -field.getColumnWidth() * 2, ColumnType.four);

                field.RotatePlayFieldStatic(OsbEasing.OutSine, 154898, 156253, -0.15f);
                field.Resize(OsbEasing.OutSine, 154898, 154898, width, height);

                var center = field.calculatePlayFieldCenter(154898);

                field.moveFieldX(OsbEasing.OutCirc, 154898, 156543, 320 - center.X);
                field.moveFieldY(OsbEasing.OutCirc, 154898, 156543, 2);

                var wrapStart = 132059;
                var gap = 40;
                var bounds = new Box2(OsuHitObject.WidescreenStoryboardBounds.Left - field.getColumnWidth() / 2, OsuHitObject.WidescreenStoryboardBounds.Top,
                                      OsuHitObject.WidescreenStoryboardBounds.Right + field.getColumnWidth() / 2, OsuHitObject.WidescreenStoryboardBounds.Bottom);
                var transitionWindow = 50; // Look ahead time in ms
                var transitionwidth = scrollWidth - field.getColumnWidth() / 2;

                while (wrapStart < 156446)
                {
                    /* if (wrapStart >= 143672)
                     {
                         transitionwidth = 302f * cols - field.getColumnWidth() / 2;
                     }*/

                    // Get positions for all columns to cover flipped scenarios
                    var xPos1 = field.columns[ColumnType.one].ReceptorPositionAt(wrapStart).X;
                    var xPos2 = field.columns[ColumnType.two].ReceptorPositionAt(wrapStart).X;
                    var xPos3 = field.columns[ColumnType.three].ReceptorPositionAt(wrapStart).X;
                    var xPos4 = field.columns[ColumnType.four].ReceptorPositionAt(wrapStart).X;

                    // Find the leftmost and rightmost positions, regardless of column number
                    var leftmostPos = Math.Min(Math.Min(xPos1, xPos2), Math.Min(xPos3, xPos4));
                    var rightmostPos = Math.Max(Math.Max(xPos1, xPos2), Math.Max(xPos3, xPos4));

                    // Check if currently offscreen to the left
                    if (rightmostPos < bounds.Left)
                    {
                        // Get future position to see if it's coming on screen soon
                        var futureXPos1 = field.columns[ColumnType.one].ReceptorPositionAt(wrapStart + transitionWindow).X;
                        var futureXPos2 = field.columns[ColumnType.two].ReceptorPositionAt(wrapStart + transitionWindow).X;
                        var futureXPos3 = field.columns[ColumnType.three].ReceptorPositionAt(wrapStart + transitionWindow).X;
                        var futureXPos4 = field.columns[ColumnType.four].ReceptorPositionAt(wrapStart + transitionWindow).X;

                        var futureLeftMost = Math.Min(Math.Min(futureXPos1, futureXPos2), Math.Min(futureXPos3, futureXPos4));

                        futureLeftMost += transitionwidth;

                        // Only wrap if it's not coming back on screen naturally within the transition window
                        if (futureLeftMost < bounds.Right)
                        {
                            field.fadeAt(wrapStart - 10, 0);
                            field.fadeAt(wrapStart + 10, 1);

                            foreach (var col in field.columns.Values)
                            {
                                // Move each column to the left by transitionwidth
                                col.receptor.renderedSprite.Fade(wrapStart - 10, 0);
                                col.receptor.renderedSprite.Fade(wrapStart + 10, 1);
                            }

                            field.moveFieldX(OsbEasing.None, wrapStart, wrapStart, transitionwidth);
                        }
                    }
                    // Check if currently offscreen to the right
                    else if (leftmostPos > bounds.Right)
                    {
                        // Get future position to see if it's coming on screen soon
                        var futureXPos1 = field.columns[ColumnType.one].ReceptorPositionAt(wrapStart + transitionWindow).X;
                        var futureXPos2 = field.columns[ColumnType.two].ReceptorPositionAt(wrapStart + transitionWindow).X;
                        var futureXPos3 = field.columns[ColumnType.three].ReceptorPositionAt(wrapStart + transitionWindow).X;
                        var futureXPos4 = field.columns[ColumnType.four].ReceptorPositionAt(wrapStart + transitionWindow).X;

                        var futureRightMost = Math.Max(Math.Max(futureXPos1, futureXPos2), Math.Max(futureXPos3, futureXPos4));

                        futureRightMost -= transitionwidth;

                        // Only wrap if it's not coming back on screen naturally within the transition window
                        if (futureRightMost > bounds.Left)
                        {
                            field.fadeAt(wrapStart - 10, 0);
                            field.fadeAt(wrapStart + 10, 1);

                            foreach (var col in field.columns.Values)
                            {
                                // Move each column to the left by transitionwidth
                                col.receptor.renderedSprite.Fade(wrapStart - 10, 0);
                                col.receptor.renderedSprite.Fade(wrapStart + 10, 1);
                            }

                            field.moveFieldX(OsbEasing.None, wrapStart, wrapStart, -transitionwidth);
                        }
                    }

                    wrapStart += gap;
                }


                DrawInstance draw = new DrawInstance(CancellationToken, field, starttime + 10, scrollSpeed, updatesPerSecond, OsbEasing.None, false, fadeTime, fadeTime);
                draw.setReceptorMovementPrecision(1f);
                draw.setNoteMovementPrecision(.8f);
                draw.setNoteRotationPrecision(0.15f);
                draw.setHoldMovementPrecision(0.15f);
                draw.setHoldRotationPrecision(0.1f);
                //draw.CommandSplitThreshold = 50;
                draw.drawViaEquation(endtime - starttime + 10, NoteFunction, true);

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

        public Vector2 NoteFunction(EquationParameters p)
        {
            var pos = p.position;

            if ((p.time > 132446 && p.time < 133608) || (p.time > 138640 && p.time < 139801))
            {
                // Adjust the Y position based on progress
                if (p.progress < 0.48f)
                {
                    pos.X = p.column.OriginPositionAt(p.time).X;
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
