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
    public class Dpad : StoryboardObjectGenerator
    {

        //        public override bool Multithreaded => false;
        private readonly object _lock = new object();
        public override void Generate()
        {

            var receptors = GetLayer("r");
            var notes = GetLayer("n");

            // General values
            var starttime = 80188 - 250; // the starttime where the playfield is initialized
            var endtime = 92963; // the endtime where the playfield is nolonger beeing rendered
            var duration = endtime - starttime; // the length the playfield is kept alive

            // Playfield Scale
            var width = 250f; // widht of the playfield / invert to flip
            var height = 400; // height of the playfield / invert to flip -600 = downscroll | 600 = upscropll
            var receptorWallOffset = 60f; // how big the boundary box for the receptor is 50 means it will be pushed away 50 units from the wall

            // Note initilization Values
            var sliderAccuracy = 28; // The Segment length for sliderbodies since they are rendered in slices 30 is default
            var isColored = false; // This property is used if you want to color the notes by urself for effects. It does not swap if the snap coloring is used.

            // Drawinstance Values
            var updatesPerSecond = 200; // The amount of steps the rendring engine does to render out note and receptor positions
            var scrollSpeed = 5000; // The speed at which the Notes scroll
            var fadeTime = 150; // The time notes will fade in

            Playfield field = new Playfield();
            field.delta = 1f;
            field.initilizePlayField(receptors, notes, starttime, endtime, width, height, receptorWallOffset, Beatmap.OverallDifficulty);
            field.noteStart = 80575;
            field.noteEnd = 99156;
            field.initializeNotes(Beatmap.HitObjects.ToList(), Beatmap.GetTimingPointAt(starttime).Bpm, Beatmap.GetTimingPointAt(starttime).Offset, isColored, sliderAccuracy);

            var circleWidth = 400;
            var innerCircle = 100;
            var center = new Vector2(320, 240);

            //field.moveFieldY(OsbEasing.None, starttime + 5, starttime + 5, 100);
            field.Scale(OsbEasing.InCubic, starttime + 5, starttime + 5, new Vector2(0.0001f), true, CenterType.middle);

            field.MoveReceptorAbsolute(OsbEasing.None, starttime + 20, starttime + 100, new Vector2(center.X, center.Y - circleWidth / 2), ColumnType.two);
            field.MoveReceptorAbsolute(OsbEasing.None, starttime + 20, starttime + 100, new Vector2(center.X + circleWidth / 2, center.Y), ColumnType.four);
            field.MoveReceptorAbsolute(OsbEasing.None, starttime + 20, starttime + 100, new Vector2(center.X, center.Y + circleWidth / 2), ColumnType.three);
            field.MoveReceptorAbsolute(OsbEasing.None, starttime + 20, starttime + 100, new Vector2(center.X - circleWidth / 2, center.Y), ColumnType.one);

            float localStart = 80382f;
            float dur = (91027 - 80382) / 0.75f;
            var quarterDur = (float)dur / 4;
            while (localStart < 91000)
            {
                // Column two (up)
                field.MoveReceptorRelative(OsbEasing.InSine, localStart, localStart + quarterDur, new Vector2(circleWidth / 2, 0), ColumnType.one);
                field.MoveReceptorRelative(OsbEasing.OutSine, localStart, localStart + quarterDur, new Vector2(0, -circleWidth / 2), ColumnType.one);
                // Column four (right)
                field.MoveReceptorRelative(OsbEasing.OutSine, localStart, localStart + quarterDur, new Vector2(-circleWidth / 2, 0), ColumnType.three);
                field.MoveReceptorRelative(OsbEasing.InSine, localStart, localStart + quarterDur, new Vector2(0, -circleWidth / 2), ColumnType.three);
                // Column three (down)
                field.MoveReceptorRelative(OsbEasing.InSine, localStart, localStart + quarterDur, new Vector2(-circleWidth / 2, 0), ColumnType.four);
                field.MoveReceptorRelative(OsbEasing.OutSine, localStart, localStart + quarterDur, new Vector2(0, circleWidth / 2), ColumnType.four);
                // Column one (left)
                field.MoveReceptorRelative(OsbEasing.OutSine, localStart, localStart + quarterDur, new Vector2(circleWidth / 2, 0), ColumnType.two);
                field.MoveReceptorRelative(OsbEasing.InSine, localStart, localStart + quarterDur, new Vector2(0, circleWidth / 2), ColumnType.two);

                localStart += quarterDur;
                if (localStart > 91000) break;

                // Column two (up)
                field.MoveReceptorRelative(OsbEasing.OutSine, localStart, localStart + quarterDur, new Vector2(circleWidth / 2, 0), ColumnType.one);
                field.MoveReceptorRelative(OsbEasing.InSine, localStart, localStart + quarterDur, new Vector2(0, circleWidth / 2), ColumnType.one);
                // Column four (right)
                field.MoveReceptorRelative(OsbEasing.InSine, localStart, localStart + quarterDur, new Vector2(circleWidth / 2, 0), ColumnType.three);
                field.MoveReceptorRelative(OsbEasing.OutSine, localStart, localStart + quarterDur, new Vector2(0, -circleWidth / 2), ColumnType.three);
                // Column three (down)
                field.MoveReceptorRelative(OsbEasing.OutSine, localStart, localStart + quarterDur, new Vector2(-circleWidth / 2, 0), ColumnType.four);
                field.MoveReceptorRelative(OsbEasing.InSine, localStart, localStart + quarterDur, new Vector2(0, -circleWidth / 2), ColumnType.four);
                // Column one (left)
                field.MoveReceptorRelative(OsbEasing.InSine, localStart, localStart + quarterDur, new Vector2(-circleWidth / 2, 0), ColumnType.two);
                field.MoveReceptorRelative(OsbEasing.OutSine, localStart, localStart + quarterDur, new Vector2(0, circleWidth / 2), ColumnType.two);

                localStart += quarterDur;
                if (localStart > 91000) break;

                // Column two (up)
                field.MoveReceptorRelative(OsbEasing.InSine, localStart, localStart + quarterDur, new Vector2(-circleWidth / 2, 0), ColumnType.one);
                field.MoveReceptorRelative(OsbEasing.OutSine, localStart, localStart + quarterDur, new Vector2(0, circleWidth / 2), ColumnType.one);
                // Column four (right)
                field.MoveReceptorRelative(OsbEasing.OutSine, localStart, localStart + quarterDur, new Vector2(circleWidth / 2, 0), ColumnType.three);
                field.MoveReceptorRelative(OsbEasing.InSine, localStart, localStart + quarterDur, new Vector2(0, circleWidth / 2), ColumnType.three);
                // Column three (down)
                field.MoveReceptorRelative(OsbEasing.InSine, localStart, localStart + quarterDur, new Vector2(circleWidth / 2, 0), ColumnType.four);
                field.MoveReceptorRelative(OsbEasing.OutSine, localStart, localStart + quarterDur, new Vector2(0, -circleWidth / 2), ColumnType.four);
                // Column one (left)
                field.MoveReceptorRelative(OsbEasing.OutSine, localStart, localStart + quarterDur, new Vector2(-circleWidth / 2, 0), ColumnType.two);
                field.MoveReceptorRelative(OsbEasing.InSine, localStart, localStart + quarterDur, new Vector2(0, -circleWidth / 2), ColumnType.two);

                localStart += quarterDur;

                if (localStart > 91000) break;

                // Column two (up)
                field.MoveReceptorRelative(OsbEasing.OutSine, localStart, localStart + quarterDur, new Vector2(-circleWidth / 2, 0), ColumnType.one);
                field.MoveReceptorRelative(OsbEasing.InSine, localStart, localStart + quarterDur, new Vector2(0, -circleWidth / 2), ColumnType.one);
                // Column four (right)
                field.MoveReceptorRelative(OsbEasing.InSine, localStart, localStart + quarterDur, new Vector2(-circleWidth / 2, 0), ColumnType.three);
                field.MoveReceptorRelative(OsbEasing.OutSine, localStart, localStart + quarterDur, new Vector2(0, circleWidth / 2), ColumnType.three);
                // Column three (down)
                field.MoveReceptorRelative(OsbEasing.OutSine, localStart, localStart + quarterDur, new Vector2(circleWidth / 2, 0), ColumnType.four);
                field.MoveReceptorRelative(OsbEasing.InSine, localStart, localStart + quarterDur, new Vector2(0, circleWidth / 2), ColumnType.four);
                // Column one (left)
                field.MoveReceptorRelative(OsbEasing.InSine, localStart, localStart + quarterDur, new Vector2(circleWidth / 2, 0), ColumnType.two);
                field.MoveReceptorRelative(OsbEasing.OutSine, localStart, localStart + quarterDur, new Vector2(0, -circleWidth / 2), ColumnType.two);

                localStart += quarterDur;

            }

            field.MoveReceptorAbsolute(OsbEasing.OutElasticQuarter, 91221, 91414, new Vector2(center.X, 240f), ColumnType.three);
            field.MoveReceptorAbsolute(OsbEasing.OutElasticQuarter, 91221, 91414, new Vector2(center.X, 240f), ColumnType.four);
            field.MoveReceptorAbsolute(OsbEasing.OutElasticQuarter, 91221, 91414, new Vector2(center.X, 240f), ColumnType.two);
            field.MoveReceptorAbsolute(OsbEasing.OutElasticQuarter, 91221, 91414, new Vector2(center.X, 240f), ColumnType.one);

            field.Resize(OsbEasing.OutElasticHalf, 91027, 91414, width, 150);

            field.Resize(OsbEasing.InOutSine, 91414, 93156, width, -1250);

            field.moveFieldY(OsbEasing.InOutSine, 91414, 92575, 200);

            // Replace the individual MoveColumnRelativeX calls with this loop
            float columnMoveStart = 91414;
            float columnMoveEnd = 92575; // Add one more beat interval to the end time
            float moveInterval = 91511 - 91414; // Time between each movement pattern (91511-91414 = 97ms)

            int patternIndex = 0;
            while (columnMoveStart <= columnMoveEnd)
            {
                // Define the pattern based on which iteration we're on (0-3)
                switch (patternIndex)
                {
                    case 0:
                        // First pattern (original 91414)
                        field.MoveColumnRelativeX(OsbEasing.None, columnMoveStart, columnMoveStart, field.getColumnWidth(), ColumnType.one);
                        field.MoveColumnRelativeX(OsbEasing.None, columnMoveStart, columnMoveStart, field.getColumnWidth(), ColumnType.two);
                        field.MoveColumnRelativeX(OsbEasing.None, columnMoveStart, columnMoveStart, field.getColumnWidth(), ColumnType.three);
                        field.MoveColumnRelativeX(OsbEasing.None, columnMoveStart, columnMoveStart, field.getColumnWidth() * -3, ColumnType.four);
                        break;
                    case 1:
                        // Second pattern (original 91511)
                        field.MoveColumnRelativeX(OsbEasing.None, columnMoveStart, columnMoveStart, field.getColumnWidth(), ColumnType.one);
                        field.MoveColumnRelativeX(OsbEasing.None, columnMoveStart, columnMoveStart, field.getColumnWidth(), ColumnType.two);
                        field.MoveColumnRelativeX(OsbEasing.None, columnMoveStart, columnMoveStart, field.getColumnWidth() * -3, ColumnType.three);
                        field.MoveColumnRelativeX(OsbEasing.None, columnMoveStart, columnMoveStart, field.getColumnWidth(), ColumnType.four);
                        break;
                    case 2:
                        // Third pattern (original 91608)
                        field.MoveColumnRelativeX(OsbEasing.None, columnMoveStart, columnMoveStart, field.getColumnWidth(), ColumnType.one);
                        field.MoveColumnRelativeX(OsbEasing.None, columnMoveStart, columnMoveStart, field.getColumnWidth() * -3, ColumnType.two);
                        field.MoveColumnRelativeX(OsbEasing.None, columnMoveStart, columnMoveStart, field.getColumnWidth(), ColumnType.three);
                        field.MoveColumnRelativeX(OsbEasing.None, columnMoveStart, columnMoveStart, field.getColumnWidth(), ColumnType.four);
                        break;
                    case 3:
                        // Fourth pattern (original 91705)
                        field.MoveColumnRelativeX(OsbEasing.None, columnMoveStart, columnMoveStart, field.getColumnWidth() * -3, ColumnType.one);
                        field.MoveColumnRelativeX(OsbEasing.None, columnMoveStart, columnMoveStart, field.getColumnWidth(), ColumnType.two);
                        field.MoveColumnRelativeX(OsbEasing.None, columnMoveStart, columnMoveStart, field.getColumnWidth(), ColumnType.three);
                        field.MoveColumnRelativeX(OsbEasing.None, columnMoveStart, columnMoveStart, field.getColumnWidth(), ColumnType.four);
                        break;
                }

                // Increment time and pattern index
                columnMoveStart += moveInterval;
                patternIndex = (patternIndex + 1) % 4; // Cycle through patterns 0-3
            }

            field.ScaleReceptor(OsbEasing.OutCubic, 80382, 81156, new Vector2(0.5f), ColumnType.all);
            field.ScaleOrigin(OsbEasing.OutCubic, 80382, 81156, new Vector2(0f), ColumnType.all);
            field.ScaleOrigin(OsbEasing.OutElasticHalf, 91027, 91221, new Vector2(0.3f), ColumnType.all);
            field.ScaleOrigin(OsbEasing.InCirc, 91414, 92963, new Vector2(0.5f), ColumnType.all);



            DrawInstance draw = new DrawInstance(CancellationToken, field, 80188, scrollSpeed, updatesPerSecond, OsbEasing.None, false, fadeTime, fadeTime);
            draw.setReceptorMovementPrecision(0.01f);
            draw.setHoldRotationPrecision(0.05f);
            draw.setNoteMovementPrecision(0.005f);
            draw.setNoteRotationPrecision(0.01f);
            draw.setHoldMovementPrecision(0.25f);
            draw.setHoldScalePrecision(0.01f);
            draw.setHoldRotationDeadZone((float)Math.PI * 4);
            draw.drawViaEquation(endtime - 80188, NoteFunction, true);
        }

        // NoteFunction is used to manipulate the pathway and a bunch of other things the note should do on their way to the receptor
        // Please be warry that this is beeing run async so you need to keep thread safety in mind when working on complex Functions.
        // You can use the progress to determin how far the note is in its cycle 0 = just start | 1 = ontop of receptor / finished
        // Special flags for hold bodies exist
        public Vector2 NoteFunction(EquationParameters p)
        {
            if (p.time > 91027) return p.position;

            // Center point for the spiral
            Vector2 center = new Vector2(320, 240);

            Vector2 receptorPosition = p.column.receptor.renderedSprite.PositionAt(p.isHoldBody == false ? p.note.starttime : p.part.Timestamp);

            // Calculate final angle based on receptor position
            float finalAngle = (float)Math.Atan2(receptorPosition.Y - center.Y, receptorPosition.X - center.X);

            // Adjust progress for cleaner spiral ending
            float progressFactor = (float)OsbEasing.InCubic.Ease(p.progress);

            // Unwrapped angle: starts at 0 and spirals until aligning with receptor angle
            float startAngle = 0;
            float endAngle = finalAngle + 6 * (float)Math.PI; // Add extra rotation for spiral effect
            float angle = startAngle + (endAngle - startAngle) * p.progress;

            // Radius increases with progress
            float radius = progressFactor * 200f;

            // Calculate position on spiral
            Vector2 spiralPosition = new Vector2(
                center.X + radius * (float)Math.Cos(angle),
                center.Y + radius * (float)Math.Sin(angle)
            );

            return Vector2.Lerp(p.position, spiralPosition, 1);
        }
    }
}
