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
using System.Drawing.Interop;
using System.Linq;

namespace StorybrewScripts
{
    public class FirstDropAlt : StoryboardObjectGenerator
    {

        private readonly object _lock = new object();
        public override void Generate()
        {
            var receptors = GetLayer("r");
            var notes = GetLayer("n");

            // General values
            var starttime = 54156; // the starttime where the playfield is initialized
            var endtime = 68963; // the endtime where the playfield is nolonger beeing rendered
            var duration = endtime - starttime; // the length the playfield is kept alive

            // Playfield Scale
            var width = 250f; // widht of the playfield / invert to flip
            var height = 400f; // height of the playfield / invert to flip -600 = downscroll | 600 = upscropll
            var receptorWallOffset = 50f; // how big the boundary box for the receptor is 50 means it will be pushed away 50 units from the wall

            // Note initilization Values
            var sliderAccuracy = 30; // The Segment length for sliderbodies since they are rendered in slices 30 is default
            var isColored = false; // This property is used if you want to color the notes by urself for effects. It does not swap if the snap coloring is used.

            // Drawinstance Values
            var updatesPerSecond = 100; // The amount of steps the rendring engine does to render out note and receptor positions
            var scrollSpeed = 2000f / 500 * ((480 - 100) / 2); // The speed at which the Notes scroll
            var fadeTime = 0; // The time notes will fade in

            Playfield field = new Playfield();
            field.delta = 2;
            field.initilizePlayField(receptors, notes, starttime, endtime, width, height, receptorWallOffset, Beatmap.OverallDifficulty);
            field.noteEnd = 67414;
            field.initializeNotes(Beatmap.HitObjects.ToList(), Beatmap.GetTimingPointAt(starttime).Bpm, Beatmap.GetTimingPointAt(starttime).Offset, isColored, sliderAccuracy);
            //field.AddMines(Beatmap.HitObjects.ToList(), 55221, 67414, 55221);

            Playfield field2 = new Playfield();
            field2.delta = 2;
            field2.initilizePlayField(receptors, notes, 55705, endtime, width, -height, receptorWallOffset, Beatmap.OverallDifficulty);
            field2.noteEnd = 67414;
            field2.noteStart = 55834;
            field2.initializeNotes(Beatmap.HitObjects.ToList(), Beatmap.GetTimingPointAt(starttime).Bpm, Beatmap.GetTimingPointAt(starttime).Offset, isColored, sliderAccuracy);
            //field2.AddMines(Beatmap.HitObjects.ToList(), 55221, 67414, 55221);

            field.moveFieldY(OsbEasing.None, starttime, starttime, 240 - 50);
            field2.moveFieldY(OsbEasing.None, starttime, starttime, -240 + 50);

            field.ScaleOrigin(OsbEasing.None, starttime, starttime, new Vector2(0f), ColumnType.all);
            field2.ScaleOrigin(OsbEasing.None, 55705, 55705, new Vector2(0f), ColumnType.all);

            field.MoveOriginRelative(OsbEasing.OutSine, 54253, 55801, new Vector2(0, -height - 200), ColumnType.three);
            field.MoveOriginRelative(OsbEasing.OutCirc, 55801, 55801 + 350, new Vector2(0, (480 + 300) / 2), ColumnType.three);
            field.MoveOriginRelative(OsbEasing.OutSine, 54253, 55801, new Vector2(0, -height - 200), ColumnType.four);
            field.MoveOriginRelative(OsbEasing.OutCirc, 55801, 55801 + 350, new Vector2(0, (480 + 300) / 2), ColumnType.four);

            field.MoveOriginRelative(OsbEasing.OutSine, 54253, 55801, new Vector2(0, -height - 200), ColumnType.one);
            field.MoveOriginRelative(OsbEasing.OutCirc, 55801, 55801 + 350, new Vector2(0, (480 + 300) / 2), ColumnType.one);
            field.MoveOriginRelative(OsbEasing.OutSine, 54253, 55801, new Vector2(0, -height - 200), ColumnType.two);
            field.MoveOriginRelative(OsbEasing.OutCirc, 55801, 55801 + 350, new Vector2(0, (480 + 300) / 2), ColumnType.two);

            field.moveFieldY(OsbEasing.OutCirc, 55801, 55801 + 350, -240 + 50);

            field2.MoveOriginRelative(OsbEasing.OutSine, 54253, 55801, new Vector2(0, height), ColumnType.all);
            field2.MoveOriginRelative(OsbEasing.OutCirc, 55801, 55801 + 350, new Vector2(0, (-480 + 100) / 2), ColumnType.all);
            field2.moveFieldY(OsbEasing.OutCirc, 55801, 55801 + 350, 240 - 50);

            double start = 56575;
            double end = 58317;
            var maxAngle = 0.8f;
            var beatLength = Beatmap.GetTimingPointAt((int)start).BeatDuration;

            CreateCombinedEffect(field, field2, start, 58898, maxAngle, OsbEasing.InOutSine, start, end);

            field.MoveColumnRelativeX(OsbEasing.OutSine, 58317, 58898, -field.getColumnWidth() * 3, ColumnType.one);
            field.MoveColumnRelativeX(OsbEasing.OutSine, 58317, 58898, -field.getColumnWidth(), ColumnType.two);
            field.MoveColumnRelativeX(OsbEasing.OutSine, 58317, 58898, field.getColumnWidth(), ColumnType.three);
            field.MoveColumnRelativeX(OsbEasing.OutSine, 58317, 58898, field.getColumnWidth() * 3, ColumnType.four);

            field2.MoveColumnRelativeX(OsbEasing.OutSine, 58317, 58898, -field.getColumnWidth() * 3, ColumnType.one);
            field2.MoveColumnRelativeX(OsbEasing.OutSine, 58317, 58898, -field.getColumnWidth(), ColumnType.two);
            field2.MoveColumnRelativeX(OsbEasing.OutSine, 58317, 58898, field.getColumnWidth(), ColumnType.three);
            field2.MoveColumnRelativeX(OsbEasing.OutSine, 58317, 58898, field.getColumnWidth() * 3, ColumnType.four);

            CreateCombinedEffect(field, field2, 58898, 61995, -1f, OsbEasing.InOutSine, 58898, 61995 - beatLength / 2 * 3);

            field.MoveColumnRelativeX(OsbEasing.InCirc, 61221, 61221, -field.getColumnWidth(), ColumnType.one);
            field.MoveColumnRelativeX(OsbEasing.InCirc, 61221, 61221, field.getColumnWidth(), ColumnType.two);
            field.MoveColumnRelativeX(OsbEasing.InCirc, 61221, 61221, -field.getColumnWidth(), ColumnType.three);
            field.MoveColumnRelativeX(OsbEasing.InCirc, 61221, 61221, field.getColumnWidth(), ColumnType.four);

            field.MoveColumnRelativeX(OsbEasing.InCirc, 61253, 61253, field.getColumnWidth(), ColumnType.one);
            field.MoveColumnRelativeX(OsbEasing.InCirc, 61253, 61253, -field.getColumnWidth(), ColumnType.two);
            field.MoveColumnRelativeX(OsbEasing.InCirc, 61253, 61253, field.getColumnWidth(), ColumnType.three);
            field.MoveColumnRelativeX(OsbEasing.InCirc, 61253, 61253, -field.getColumnWidth(), ColumnType.four);

            field.MoveColumnRelativeX(OsbEasing.InCirc, 61285, 61285, -field.getColumnWidth(), ColumnType.one);
            field.MoveColumnRelativeX(OsbEasing.InCirc, 61285, 61285, field.getColumnWidth(), ColumnType.two);
            field.MoveColumnRelativeX(OsbEasing.InCirc, 61285, 61285, -field.getColumnWidth(), ColumnType.three);
            field.MoveColumnRelativeX(OsbEasing.InCirc, 61285, 61285, field.getColumnWidth(), ColumnType.four);

            field.MoveColumnRelativeX(OsbEasing.InCirc, 61317, 61317, field.getColumnWidth(), ColumnType.one);
            field.MoveColumnRelativeX(OsbEasing.InCirc, 61317, 61317, -field.getColumnWidth(), ColumnType.two);
            field.MoveColumnRelativeX(OsbEasing.InCirc, 61317, 61317, field.getColumnWidth(), ColumnType.three);
            field.MoveColumnRelativeX(OsbEasing.InCirc, 61317, 61317, -field.getColumnWidth(), ColumnType.four);

            field.MoveColumnRelativeX(OsbEasing.InCirc, 61350, 61350, -field.getColumnWidth(), ColumnType.one);
            field.MoveColumnRelativeX(OsbEasing.InCirc, 61350, 61350, field.getColumnWidth(), ColumnType.two);
            field.MoveColumnRelativeX(OsbEasing.InCirc, 61350, 61350, -field.getColumnWidth(), ColumnType.three);
            field.MoveColumnRelativeX(OsbEasing.InCirc, 61350, 61350, field.getColumnWidth(), ColumnType.four);

            field.MoveColumnRelativeX(OsbEasing.InCirc, 61414, 61414, field.getColumnWidth(), ColumnType.one);
            field.MoveColumnRelativeX(OsbEasing.InCirc, 61414, 61414, -field.getColumnWidth(), ColumnType.two);
            field.MoveColumnRelativeX(OsbEasing.InCirc, 61414, 61414, field.getColumnWidth(), ColumnType.three);
            field.MoveColumnRelativeX(OsbEasing.InCirc, 61414, 61414, -field.getColumnWidth(), ColumnType.four);

            field2.MoveColumnRelativeX(OsbEasing.InCirc, 61221, 61221, -field.getColumnWidth(), ColumnType.one);
            field2.MoveColumnRelativeX(OsbEasing.InCirc, 61221, 61221, field.getColumnWidth(), ColumnType.two);
            field2.MoveColumnRelativeX(OsbEasing.InCirc, 61221, 61221, -field.getColumnWidth(), ColumnType.three);
            field2.MoveColumnRelativeX(OsbEasing.InCirc, 61221, 61221, field.getColumnWidth(), ColumnType.four);

            field2.MoveColumnRelativeX(OsbEasing.InCirc, 61253, 61253, field.getColumnWidth(), ColumnType.one);
            field2.MoveColumnRelativeX(OsbEasing.InCirc, 61253, 61253, -field.getColumnWidth(), ColumnType.two);
            field2.MoveColumnRelativeX(OsbEasing.InCirc, 61253, 61253, field.getColumnWidth(), ColumnType.three);
            field2.MoveColumnRelativeX(OsbEasing.InCirc, 61253, 61253, -field.getColumnWidth(), ColumnType.four);

            field2.MoveColumnRelativeX(OsbEasing.InCirc, 61285, 61285, -field.getColumnWidth(), ColumnType.one);
            field2.MoveColumnRelativeX(OsbEasing.InCirc, 61285, 61285, field.getColumnWidth(), ColumnType.two);
            field2.MoveColumnRelativeX(OsbEasing.InCirc, 61285, 61285, -field.getColumnWidth(), ColumnType.three);
            field2.MoveColumnRelativeX(OsbEasing.InCirc, 61285, 61285, field.getColumnWidth(), ColumnType.four);

            field2.MoveColumnRelativeX(OsbEasing.InCirc, 61317, 61317, field.getColumnWidth(), ColumnType.one);
            field2.MoveColumnRelativeX(OsbEasing.InCirc, 61317, 61317, -field.getColumnWidth(), ColumnType.two);
            field2.MoveColumnRelativeX(OsbEasing.InCirc, 61317, 61317, field.getColumnWidth(), ColumnType.three);
            field2.MoveColumnRelativeX(OsbEasing.InCirc, 61317, 61317, -field.getColumnWidth(), ColumnType.four);

            field2.MoveColumnRelativeX(OsbEasing.InCirc, 61350, 61350, -field.getColumnWidth(), ColumnType.one);
            field2.MoveColumnRelativeX(OsbEasing.InCirc, 61350, 61350, field.getColumnWidth(), ColumnType.two);
            field2.MoveColumnRelativeX(OsbEasing.InCirc, 61350, 61350, -field.getColumnWidth(), ColumnType.three);
            field2.MoveColumnRelativeX(OsbEasing.InCirc, 61350, 61350, field.getColumnWidth(), ColumnType.four);

            field2.MoveColumnRelativeX(OsbEasing.InCirc, 61414, 61414, field.getColumnWidth(), ColumnType.one);
            field2.MoveColumnRelativeX(OsbEasing.InCirc, 61414, 61414, -field.getColumnWidth(), ColumnType.two);
            field2.MoveColumnRelativeX(OsbEasing.InCirc, 61414, 61414, field.getColumnWidth(), ColumnType.three);
            field2.MoveColumnRelativeX(OsbEasing.InCirc, 61414, 61414, -field.getColumnWidth(), ColumnType.four);

            field.MoveColumnRelativeX(OsbEasing.OutElasticHalf, 61511, 61995, -field.getColumnWidth() * 3, ColumnType.one);
            field.MoveColumnRelativeX(OsbEasing.OutElasticHalf, 61511, 61995, -field.getColumnWidth(), ColumnType.two);
            field.MoveColumnRelativeX(OsbEasing.OutElasticHalf, 61511, 61995, field.getColumnWidth(), ColumnType.three);
            field.MoveColumnRelativeX(OsbEasing.OutElasticHalf, 61511, 61995, field.getColumnWidth() * 3, ColumnType.four);

            field2.MoveColumnRelativeX(OsbEasing.OutElasticHalf, 61511, 61995, -field.getColumnWidth() * 3, ColumnType.one);
            field2.MoveColumnRelativeX(OsbEasing.OutElasticHalf, 61511, 61995, -field.getColumnWidth(), ColumnType.two);
            field2.MoveColumnRelativeX(OsbEasing.OutElasticHalf, 61511, 61995, field.getColumnWidth(), ColumnType.three);
            field2.MoveColumnRelativeX(OsbEasing.OutElasticHalf, 61511, 61995, field.getColumnWidth() * 3, ColumnType.four);

            CreateCombinedEffect(field, field2, 61995, 66640, 1.2f, OsbEasing.InOutSine, 61995, 66640 - beatLength / 2);

            field2.Rotate(OsbEasing.None, 66640, 66640, Math.PI / 6);
            field2.Rotate(OsbEasing.None, 66930, 66930, Math.PI / 6);
            field2.Rotate(OsbEasing.None, 67221, 67221, Math.PI / 3);
            field2.Rotate(OsbEasing.None, 67317, 67317, Math.PI / 3);

            field2.RotatePlayFieldStatic(OsbEasing.None, 66640, 66640, Math.PI / 6);
            field2.RotatePlayFieldStatic(OsbEasing.None, 66930, 66930, Math.PI / 6);
            field2.RotatePlayFieldStatic(OsbEasing.None, 67221, 67221, Math.PI / 3);
            field2.RotatePlayFieldStatic(OsbEasing.None, 67317, 67317, Math.PI / 3);

            field.ScaleOrigin(OsbEasing.OutCubic, 66446, 66640, new Vector2(0.5f), ColumnType.all);

            var scrollHeight = (scrollSpeed / height) * 500;

            field.MoveOriginRelative(OsbEasing.OutSine, 66446, 66640, new Vector2(0, 125), ColumnType.all);

            field2.fadeAt(66446, 66640, OsbEasing.OutSine, 0);

            DrawInstance draw = new DrawInstance(CancellationToken, field, starttime + 50, scrollSpeed, updatesPerSecond, OsbEasing.None, false, fadeTime, fadeTime);
            draw.setReceptorMovementPrecision(0.25f);
            draw.setNoteMovementPrecision(0.25f);
            draw.setNoteRotationPrecision(0f);
            // draw.customScale = true;
            draw.noteScaleEasing = OsbEasing.OutExpo;
            draw.drawViaEquation(67414 - starttime + 50, NoteFunction, true);

            DrawInstance draw2 = new DrawInstance(CancellationToken, field2, 55705 + 10, scrollSpeed, updatesPerSecond, OsbEasing.None, false, fadeTime, fadeTime);
            draw2.setReceptorMovementPrecision(0.25f);
            draw2.setNoteMovementPrecision(0.25f);
            draw2.setNoteRotationPrecision(0f);
            //draw2.customScale = true;
            draw2.noteScaleEasing = OsbEasing.OutExpo;
            draw2.drawViaEquation(67414 - 55705, NoteFunction, true);

        }

        public Vector2 calculateReceptorCenter(Playfield field, double time)
        {
            if (field.columns.Count == 0)
                return new Vector2(320, 240); // Default center if no columns

            // Initialize min and max values with the first receptor position
            Vector2 firstPos = field.columns.First().Value.receptor.PositionAt(time);
            float minX = firstPos.X;
            float maxX = firstPos.X;
            float minY = firstPos.Y;
            float maxY = firstPos.Y;

            // Find the min and max coordinates to determine the bounding box
            foreach (var column in field.columns.Values)
            {
                Vector2 receptorPos = column.receptor.PositionAt(time);

                minX = Math.Min(minX, receptorPos.X);
                maxX = Math.Max(maxX, receptorPos.X);
                minY = Math.Min(minY, receptorPos.Y);
                maxY = Math.Max(maxY, receptorPos.Y);
            }

            // Calculate the center of the bounding box
            return new Vector2((minX + maxX) / 2, (minY + maxY) / 2);
        }

        public ColumnType inverseColumnType(ColumnType type)
        {
            switch (type)
            {
                case ColumnType.one:
                    return ColumnType.four;
                case ColumnType.two:
                    return ColumnType.three;
                case ColumnType.three:
                    return ColumnType.two;
                case ColumnType.four:
                    return ColumnType.one;
                default:
                    return type; // If it's not a valid column type, return it unchanged
            }
        }

        // Function to calculate a beat-based progress that goes from 1 to 0 over each beat
        // Function to calculate a beat-based progress that cycles 1→0→1 over each beat
        private float getBeatProgress(double currentTime, double beatStart, double beatLength, OsbEasing easing)
        {
            // Calculate position within the current beat (0 to 1)
            double beatPosition = (currentTime - beatStart) % beatLength / beatLength;

            // Create a triangular wave pattern (0→1→0) with eased progression
            if (beatPosition < 0.5)
            {
                // First half: 0 to 1 (rising) with easing
                double normalizedPosition = beatPosition * 2; // Scale from 0-0.5 to 0-1
                return (float)easing.Ease(normalizedPosition);
            }
            else
            {
                // Second half: 1 to 0 (falling) with easing
                double normalizedPosition = (beatPosition - 0.5) * 2; // Scale from 0.5-1 to 0-1 
                return (float)(1 - easing.Ease(normalizedPosition));
            }
        }

        double Blend(double a, double b, double t)
        {
            return a + (b - a) * t;
        }

        public static Vector2 GetTangentPoint(double radius, double angle, double xOffset, Vector2 center)
        {
            // Calculate the point on the circle at the given angle
            double circleX = center.X + radius * Math.Cos(angle);
            double circleY = center.Y + radius * Math.Sin(angle);

            // Calculate the tangent vector (perpendicular to the radius vector)
            // For a circle, the tangent is perpendicular to the radius
            // The correct perpendicular vector is (-sin(angle), cos(angle))
            double tangentX = -Math.Sin(angle);
            double tangentY = Math.Cos(angle);

            // Move along the tangent by xOffset
            double finalX = circleX + (xOffset * tangentX);
            double finalY = circleY + (xOffset * tangentY);

            return new Vector2((float)finalX, (float)finalY);
        }

        public Vector2 NoteFunction(EquationParameters p)
        {
            if (p.note.isMine && p.progress == 0 && p.time > 56188)
            {
                var mineFadeStart = 56188;
                var mineFadeEnd = 58124;
                var fadeProgress = 1f - ((p.time - mineFadeStart) / (mineFadeEnd - mineFadeStart));
                var fadeAtEnd = 1f - ((p.note.renderEnd - mineFadeStart) / (mineFadeEnd - mineFadeStart));
                p.note.noteSprite.Fade(p.time, p.note.renderEnd, fadeProgress, fadeAtEnd);

            }

            return p.position;
        }

        public void CreateCombinedEffect(
            Playfield field,
            Playfield field2,
            double startTime,
            double endTime,
            float maxAngle,
            OsbEasing angleEasing,
            double collapseStart,
            double collapseEnd)
        {
            // Calculate necessary values
            double duration = endTime - startTime;
            double currentTime = startTime;
            int steps = 1000;
            double stepLength = duration / steps;
            double halfTime = duration / 2;
            int halfSteps = steps / 2;
            Vector2 center = new Vector2(320, 240);
            double radius = (480 - 100) / 2;
            double interval = Beatmap.GetTimingPointAt((int)startTime).BeatDuration;
            float colWidth = field.getColumnWidth();
            double collOfffset = 10;

            // Get initial positions of columns
            Dictionary<ColumnType, Vector2> receptorPositions = new Dictionary<ColumnType, Vector2>
            {
                { ColumnType.one, field.columns[ColumnType.one].receptor.PositionAt(currentTime) },
                { ColumnType.two, field.columns[ColumnType.two].receptor.PositionAt(currentTime) },
                { ColumnType.three, field.columns[ColumnType.three].receptor.PositionAt(currentTime) },
                { ColumnType.four, field.columns[ColumnType.four].receptor.PositionAt(currentTime) }
            };

            Dictionary<ColumnType, Vector2> originPositions = new Dictionary<ColumnType, Vector2>
            {
                { ColumnType.one, field.columns[ColumnType.one].origin.PositionAt(currentTime) },
                { ColumnType.two, field.columns[ColumnType.two].origin.PositionAt(currentTime) },
                { ColumnType.three, field.columns[ColumnType.three].origin.PositionAt(currentTime) },
                { ColumnType.four, field.columns[ColumnType.four].origin.PositionAt(currentTime) }
            };

            // Apply static rotation to the playfield
            field.RotatePlayFieldStatic(OsbEasing.InSine, startTime, startTime + halfTime, maxAngle);
            field2.RotatePlayFieldStatic(OsbEasing.InSine, startTime, startTime + halfTime, maxAngle);
            field.RotatePlayFieldStatic(OsbEasing.OutSine, startTime + halfTime, endTime, -maxAngle);
            field2.RotatePlayFieldStatic(OsbEasing.OutSine, startTime + halfTime, endTime, -maxAngle);

            Dictionary<ColumnType, float> lastColumnProgress = new Dictionary<ColumnType, float>
            {
                { ColumnType.one, 1f },
                { ColumnType.two, 1f },
                { ColumnType.three, 1f },
                { ColumnType.four, 1f }
            };

            // PART 1: ROTATION EFFECT
            for (int i = 0; i < steps; i++)
            {
                CancellationToken.ThrowIfCancellationRequested();

                // Calculate normalized progress with easing
                float normalizedProgress = i / (float)(steps - 1);
                float easedProgress = (float)angleEasing.Ease(normalizedProgress);

                // Calculate angle based on progress
                float angle;
                if (normalizedProgress <= 0.5f)
                {
                    // First half: -PI/2 to -PI/2+maxAngle
                    angle = (float)(-Math.PI / 2 + maxAngle * (easedProgress * 2));
                }
                else
                {
                    // Second half: -PI/2+maxAngle back to -PI/2
                    angle = (float)(-Math.PI / 2 + maxAngle * (1 - (easedProgress - 0.5f) * 2));
                }

                // Base position on the circle
                var x = center.X + radius * Math.Cos(angle);
                var y = center.Y + radius * Math.Sin(angle);
                var pos = new Vector2((float)x, (float)y);

                // For each column, calculate and apply movement
                foreach (var col in field.columns.Values)
                {
                    var currentPos = receptorPositions[col.type];
                    var currentPosOrigin = originPositions[col.type];
                    float offset = 0;

                    // Get column-specific offset
                    switch (col.type)
                    {
                        case ColumnType.one: offset = -colWidth * 1.5f; break;
                        case ColumnType.two: offset = -colWidth * 0.5f; break;
                        case ColumnType.three: offset = colWidth * 0.5f; break;
                        case ColumnType.four: offset = colWidth * 1.5f; break;
                    }

                    // Beat-based progress for column swapping
                    // Beat-based progress for column swapping
                    float progress = lastColumnProgress[col.type];
                    // Only apply beat progress between collapse start and end times
                    if (currentTime >= collapseStart && currentTime <= collapseEnd)
                    {
                        progress = getBeatProgress(currentTime, collapseStart - interval / 2, interval * 2, OsbEasing.InOutSine);
                    }

                    var blend = Blend(offset, -offset, progress);
                    double timeOffset = Blend(collOfffset * ((int)col.type - 1), -collOfffset * ((int)col.type - 1), progress);

                    // Calculate tangent point and movement
                    var tangentPoint = GetTangentPoint(radius, angle, blend, center);
                    var tangentPointOrigin = GetTangentPoint(0, angle, blend, center);
                    var movement = tangentPoint - currentPos;
                    var movementOrigin = tangentPointOrigin - currentPosOrigin;

                    // Apply movement with time offset
                    currentTime += timeOffset;
                    field.MoveReceptorRelative(OsbEasing.None, currentTime, currentTime + stepLength, movement, col.type);
                    field2.MoveReceptorRelative(OsbEasing.None, currentTime, currentTime + stepLength, -movement, inverseColumnType(col.type));

                    field.MoveOriginRelative(OsbEasing.None, currentTime, currentTime + stepLength, movementOrigin, col.type);
                    field2.MoveOriginRelative(OsbEasing.None, currentTime, currentTime + stepLength, -movementOrigin, inverseColumnType(col.type));

                    currentTime -= timeOffset;

                    lastColumnProgress[col.type] = progress;
                    receptorPositions[col.type] = tangentPoint;
                    originPositions[col.type] = tangentPointOrigin;
                }

                currentTime += stepLength;
            }

            // PART 2: COLLAPSE EFFECT
            double localStart = collapseStart;
            double localEnd = collapseEnd;
            double collapseDuration = interval / 2;
            bool inverse = true;
            var counter = 0;

            while (localStart < localEnd)
            {
                CancellationToken.ThrowIfCancellationRequested();

                foreach (var column in field.columns.Values)
                {
                    var pos = column.receptor.PositionAt(localStart);
                    var originPos = column.origin.PositionAt(localStart);
                    var offset = Vector2.Zero;

                    switch (column.type)
                    {
                        case ColumnType.one: offset.X = -field.getColumnWidth() * 3; break;
                        case ColumnType.two: offset.X = -field.getColumnWidth(); break;
                        case ColumnType.three: offset.X = field.getColumnWidth(); break;
                        case ColumnType.four: offset.X = field.getColumnWidth() * 3f; break;
                    }

                    if (inverse) offset = -offset;

                    var movement = center - pos;

                    localStart += collOfffset * ((int)column.type - 1);

                    field.MoveReceptorRelative(OsbEasing.InCubic, localStart - collapseDuration, localStart, movement, column.type);
                    field.MoveReceptorRelative(OsbEasing.OutSine, localStart, localStart + collapseDuration, -movement, column.type);

                    field2.MoveReceptorRelative(OsbEasing.InCubic, localStart - collapseDuration, localStart, -movement, inverseColumnType(column.type));
                    field2.MoveReceptorRelative(OsbEasing.OutSine, localStart, localStart + collapseDuration, movement, inverseColumnType(column.type));

                    if (counter % 2 == 0 && localStart + interval < localEnd)
                    {
                        field.ScaleReceptor(OsbEasing.InOutCubic, localStart - collapseDuration, localStart + collapseDuration, new Vector2(0.6f), column.type);
                        field2.ScaleReceptor(OsbEasing.InOutCubic, localStart - collapseDuration, localStart + collapseDuration, new Vector2(0.4f), column.type);
                    }
                    else if (localStart + interval < localEnd)
                    {
                        field.ScaleReceptor(OsbEasing.InOutCubic, localStart - collapseDuration, localStart + collapseDuration, new Vector2(0.4f), column.type);
                        field2.ScaleReceptor(OsbEasing.InOutCubic, localStart - collapseDuration, localStart + collapseDuration, new Vector2(0.6f), column.type);
                    }
                    else
                    {
                        field.ScaleReceptor(OsbEasing.InOutCubic, localStart - collapseDuration, localStart + collapseDuration, new Vector2(0.5f), column.type);
                        field2.ScaleReceptor(OsbEasing.InOutCubic, localStart - collapseDuration, localStart + collapseDuration, new Vector2(0.5f), column.type);
                    }

                    localStart -= collOfffset * ((int)column.type - 1);
                }
                counter++;
                inverse = !inverse;
                localStart += interval;
            }
        }
    }
}
