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
using System.IO;
using System.Linq;

namespace StorybrewScripts
{
    public class Altfirstbuildup : StoryboardObjectGenerator
    {
        public override void Generate()
        {

            var receptors = GetLayer("r");
            var notes = GetLayer("n");

            // General values
            var starttime = 15543; // the starttime where the playfield is initialized
            var endtime = 43414; // the endtime where the playfield is nolonger beeing rendered
            var duration = endtime - starttime; // the length the playfield is kept alive

            // Playfield Scale
            var width = 250f; // widht of the playfield / invert to flip
            var height = 500f; // height of the playfield / invert to flip -600 = downscroll | 600 = upscropll
            var receptorWallOffset = 60f; // how big the boundary box for the receptor is 50 means it will be pushed away 50 units from the wall

            // Note initilization Values
            var sliderAccuracy = 30; // The Segment length for sliderbodies since they are rendered in slices 30 is default
            var isColored = false; // This property is used if you want to color the notes by urself for effects. It does not swap if the snap coloring is used.

            // Drawinstance Values
            var updatesPerSecond = 50; // The amount of steps the rendring engine does to render out note and receptor positions
            var scrollSpeed = 1200; // The speed at which the Notes scroll
            var fadeTime = 0; // The time notes will fade in

            var playfields = 3;

            var movementDur = 3000;

            var offset = movementDur / 3 / 4;

            var quarterDur = movementDur / 4;
            var circleHeight = 40;
            var circleWidth = 100;
            var closeScale = 0.5f;
            var farScale = 0.4f;

            for (int i = 0; i < playfields; i++)
            {
                Playfield field = new Playfield();
                field.initilizePlayField(receptors, notes, starttime, endtime, 0, -height, receptorWallOffset, Beatmap.OverallDifficulty);
                field.noteEnd = 44963;
                field.initializeNotes(Beatmap.HitObjects.ToList(), Beatmap, isColored, sliderAccuracy);

                field.ScaleReceptor(OsbEasing.None, 15543, 15543, new Vector2(farScale), ColumnType.all);

                field.moveField(OsbEasing.None, 15543, 15543, -circleWidth, -circleHeight);

                var currentColumn = 0;
                foreach (var col in field.columns.Values)
                {
                    var colOffset = offset * currentColumn;
                    var localStart = starttime + offset * i + (offset * 3 * i) + colOffset;

                    while (localStart < 41866)
                    {
                        col.receptor.renderedSprite.Fade(OsbEasing.OutSine, localStart - quarterDur, localStart + quarterDur, 1f, 0.1f);
                        col.MoveColumnRelativeX(OsbEasing.InSine, localStart, localStart + quarterDur, circleWidth);
                        col.MoveColumnRelativeY(OsbEasing.OutSine, localStart, localStart + quarterDur, -circleHeight);
                        localStart += quarterDur;

                        if (localStart > 41866)
                            break;
                        // Column two (up)
                        col.receptor.ScaleReceptor(OsbEasing.InCubic, localStart, localStart + quarterDur + quarterDur, new Vector2(closeScale));
                        if (localStart + quarterDur < 41866)
                        {
                            col.origin.ScaleOrigin(OsbEasing.InCubic, localStart, localStart + quarterDur + quarterDur, new Vector2(closeScale));
                        }
                        if (currentColumn == 0)
                        {
                            field.fadeAt(localStart - quarterDur, localStart + quarterDur, OsbEasing.OutSine, 0.1f);
                        }
                        col.MoveColumnRelativeX(OsbEasing.OutSine, localStart, localStart + quarterDur, circleWidth);
                        col.MoveColumnRelativeY(OsbEasing.InSine, localStart, localStart + quarterDur, circleHeight);
                        localStart += quarterDur;

                        if (localStart > 41866)
                            break;
                        // Column two (up)
                        col.receptor.renderedSprite.Fade(OsbEasing.InSine, localStart - quarterDur, localStart + quarterDur, 0.1f, 1f);
                        col.MoveColumnRelativeX(OsbEasing.InSine, localStart, localStart + quarterDur, -circleWidth);
                        col.MoveColumnRelativeY(OsbEasing.OutSine, localStart, localStart + quarterDur, circleHeight);
                        localStart += quarterDur;

                        // Column two (up)
                        col.receptor.ScaleReceptor(OsbEasing.OutCubic, localStart, localStart + quarterDur + quarterDur, new Vector2(farScale));
                        if (localStart + quarterDur < 41866)
                        {
                            col.origin.ScaleOrigin(OsbEasing.OutCubic, localStart, localStart + quarterDur + quarterDur, new Vector2(farScale));
                        }

                        if (currentColumn == 0)
                        {
                            field.fadeAt(localStart - quarterDur, localStart + quarterDur, OsbEasing.InSine, 1f);
                        }
                        col.MoveColumnRelativeX(OsbEasing.OutSine, localStart, localStart + quarterDur, -circleWidth);
                        col.MoveColumnRelativeY(OsbEasing.InSine, localStart, localStart + quarterDur, -circleHeight);
                        localStart += quarterDur;
                    }

                    currentColumn++;

                }

                var local = 31027;
                var dur = 2500;

                while (local < 41866)
                {
                    field.ScaleOrigin(OsbEasing.InCubic, local, local + quarterDur + quarterDur, new Vector2(farScale - 0.1f), ColumnType.all);
                    field.MoveOriginRelative(OsbEasing.OutSine, local, local + quarterDur, new Vector2(circleWidth * 2, 0), ColumnType.all);
                    field.MoveOriginRelative(OsbEasing.InSine, local, local + quarterDur, new Vector2(0, circleHeight), ColumnType.all);

                    field.MoveReceptorRelative(OsbEasing.OutSine, local, local + quarterDur, new Vector2(-circleWidth, 0), ColumnType.all);
                    field.MoveReceptorRelative(OsbEasing.InSine, local, local + quarterDur, new Vector2(0, -circleHeight / 2), ColumnType.all);
                    local += quarterDur;

                    if (local > 41866)
                        break;
                    // Column two (up)
                    field.MoveOriginRelative(OsbEasing.InSine, local, local + quarterDur, new Vector2(-circleWidth * 2, 0), ColumnType.all);
                    field.MoveOriginRelative(OsbEasing.OutSine, local, local + quarterDur, new Vector2(0, circleHeight), ColumnType.all);

                    field.MoveReceptorRelative(OsbEasing.InSine, local, local + quarterDur, new Vector2(circleWidth, 0), ColumnType.all);
                    field.MoveReceptorRelative(OsbEasing.OutSine, local, local + quarterDur, new Vector2(0, -circleHeight / 2), ColumnType.all);
                    local += quarterDur;

                    if (local > 41866)
                        break;
                    field.ScaleOrigin(OsbEasing.OutCubic, local, local + quarterDur + quarterDur, new Vector2(closeScale + 0.1f), ColumnType.all);
                    field.MoveOriginRelative(OsbEasing.OutSine, local, local + quarterDur, new Vector2(-circleWidth * 2, 0), ColumnType.all);
                    field.MoveOriginRelative(OsbEasing.InSine, local, local + quarterDur, new Vector2(0, -circleHeight), ColumnType.all);

                    field.MoveReceptorRelative(OsbEasing.OutSine, local, local + quarterDur, new Vector2(circleWidth, 0), ColumnType.all);
                    field.MoveReceptorRelative(OsbEasing.InSine, local, local + quarterDur, new Vector2(0, circleHeight / 2), ColumnType.all);
                    local += quarterDur;

                    if (local > 41866)
                        break;
                    field.MoveOriginRelative(OsbEasing.InSine, local, local + quarterDur, new Vector2(circleWidth * 2, 0), ColumnType.all);
                    field.MoveOriginRelative(OsbEasing.OutSine, local, local + quarterDur, new Vector2(0, -circleHeight), ColumnType.all);

                    field.MoveReceptorRelative(OsbEasing.InSine, local, local + quarterDur, new Vector2(-circleWidth, 0), ColumnType.all);
                    field.MoveReceptorRelative(OsbEasing.OutSine, local, local + quarterDur, new Vector2(0, circleHeight / 2), ColumnType.all);
                    local += quarterDur;
                }

                field.MoveOriginRelative(OsbEasing.None, 24834, 24834, new Vector2(300, 0), ColumnType.all);
                field.MoveReceptorRelative(OsbEasing.None, 24834, 24834, new Vector2(-200, 0), ColumnType.all);

                field.MoveOriginRelative(OsbEasing.OutSine, 24834, 24834 + 2000, new Vector2(-300, 0), ColumnType.all);
                field.MoveReceptorRelative(OsbEasing.OutSine, 24834, 24834 + 2000, new Vector2(200, 0), ColumnType.all);

                field.MoveOriginRelative(OsbEasing.None, 37221, 37221, new Vector2(300, 0), ColumnType.all);
                field.MoveReceptorRelative(OsbEasing.None, 37221, 37221, new Vector2(-200, 0), ColumnType.all);

                field.MoveOriginRelative(OsbEasing.OutSine, 37221, 37221 + 2000, new Vector2(-300, 0), ColumnType.all);
                field.MoveReceptorRelative(OsbEasing.OutSine, 37221, 37221 + 2000, new Vector2(200, 0), ColumnType.all);

                field.ScaleReceptor(OsbEasing.OutQuad, 41866, 41866 + 1500, new Vector2(0.5f), ColumnType.all);
                field.ScaleOrigin(OsbEasing.OutQuad, 41866, 41866 + 1500, new Vector2(0.5f), ColumnType.all);

                field.fadeAt(41866, 41866 + 1500, OsbEasing.InExpo, 1f);

                foreach(var col in field.columns.Values)
                {
                    col.receptor.renderedSprite.Fade(OsbEasing.InExpo, 41866, 41866 + 1500, 0.1f, 1f);
                }

                field.MoveReceptorRelative(OsbEasing.OutExpo, 41866, 41866 + 1500, new Vector2(226.25f, 50f) - field.columns[ColumnType.one].receptor.PositionAt(41866 + 1500), ColumnType.one);
                field.MoveReceptorRelative(OsbEasing.OutExpo, 41866, 41866 + 1500, new Vector2(288.75f, 50f) - field.columns[ColumnType.two].receptor.PositionAt(41866 + 1500), ColumnType.two);
                field.MoveReceptorRelative(OsbEasing.OutExpo, 41866, 41866 + 1500, new Vector2(351.25f, 50f) - field.columns[ColumnType.three].receptor.PositionAt(41866 + 1500), ColumnType.three);
                field.MoveReceptorRelative(OsbEasing.OutExpo, 41866, 41866 + 1500, new Vector2(413.75f, 50f) - field.columns[ColumnType.four].receptor.PositionAt(41866 + 1500), ColumnType.four);

                field.MoveOriginRelative(OsbEasing.OutExpo, 41866, 41866 + 1500, new Vector2(226.25f, 550f) - field.columns[ColumnType.one].origin.PositionAt(41866 + 1500), ColumnType.one);
                field.MoveOriginRelative(OsbEasing.OutExpo, 41866, 41866 + 1500, new Vector2(288.75f, 550f) - field.columns[ColumnType.two].origin.PositionAt(41866 + 1500), ColumnType.two);
                field.MoveOriginRelative(OsbEasing.OutExpo, 41866, 41866 + 1500, new Vector2(351.25f, 550f) - field.columns[ColumnType.three].origin.PositionAt(41866 + 1500), ColumnType.three);
                field.MoveOriginRelative(OsbEasing.OutExpo, 41866, 41866 + 1500, new Vector2(413.75f, 550f) - field.columns[ColumnType.four].origin.PositionAt(41866 + 1500), ColumnType.four);


                DrawInstance draw = new DrawInstance(CancellationToken, field, 17866, scrollSpeed, updatesPerSecond, OsbEasing.None, false, fadeTime, fadeTime);
                draw.setReceptorMovementPrecision(0.25f);
                draw.setNoteMovementPrecision(0.25f);
                //draw.customScale = true;
                draw.drawViaEquation(endtime - 17866, NoteFunction, true);
            }
        }

        public Vector2 NoteFunction(EquationParameters p)
        {
            var pos = p.position;

            return pos;
        }
    }
}
