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
    public class Proxywall : StoryboardObjectGenerator
    {
//        public override bool Multithreaded => false;
        float farScale = 0.075f;
        float closeScale = 0.75f;
        float height = 500f; // height of the playfield / invert to flip -600 = downscroll | 600 = upscropll

        // Maintain thread-safe collections of notes being transitioned
        private readonly object _noteLock = new object();
        private HashSet<Note> _transitioningNotes = new HashSet<Note>();
        private Dictionary<Note, double> _lastTransitionTime = new Dictionary<Note, double>();

        public override void Generate()
        {

            var receptors = GetLayer("r");
            var notes = GetLayer("n");

            // General values
            var starttime = 66640; // the starttime where the playfield is initialized
            var endtime = 82124; // the endtime where the playfield is nolonger beeing rendered
            var duration = endtime - starttime; // the length the playfield is kept alive

            // Playfield Scale
            var width = 250f; // widht of the playfield / invert to flip
            var receptorWallOffset = 50f; // how big the boundary box for the receptor is 50 means it will be pushed away 50 units from the wall

            // Note initilization Values
            var sliderAccuracy = 30; // The Segment length for sliderbodies since they are rendered in slices 30 is default
            var isColored = false; // This property is used if you want to color the notes by urself for effects. It does not swap if the snap coloring is used.

            // Drawinstance Values
            var updatesPerSecond = 150; // The amount of steps the rendring engine does to render out note and receptor positions
            var scrollSpeed = 1200f; // The speed at which the Notes scroll
            var fadeTime = 150; // The time notes will fade in

            Box2 bound = DeepCopyBox2(OsuHitObject.WidescreenStoryboardBounds);

            bound.Right += 100;
            bound.Left += 0;
            var currentx = bound.Right;


            List<Column> columns = new List<Column>();
            List<Playfield> playfields = new List<Playfield>();

            for (int i = 0; i <= 7; i++)
            {
                Playfield field = new Playfield();
                field.initilizePlayField(receptors, notes, starttime, endtime, width, height, receptorWallOffset, Beatmap.OverallDifficulty);
                field.initializeNotes(Beatmap.HitObjects.ToList(), Beatmap.GetTimingPointAt(starttime).Bpm, Beatmap.GetTimingPointAt(starttime).Offset, isColored, sliderAccuracy);

                columns.Add(field.columns[ColumnType.four]);
                columns.Add(field.columns[ColumnType.three]);
                columns.Add(field.columns[ColumnType.two]);
                columns.Add(field.columns[ColumnType.one]);

                playfields.Add(field);
            }

            foreach (var column in columns)
            {
                // Calculate progress from left to right of screen
                float progress = (currentx - bound.Right) / (bound.Left - bound.Right);

                // Interpolate scale between far and close
                var currentScale = closeScale + (farScale - closeScale) * progress; //farScale + (closeScale - farScale) * progress;
                float scaleMult = currentScale / 0.5f;
                float scaleDiv = 0.5f / currentScale;

                // First step: 67414 to 67705

                var sacleDif = 0.5f - currentScale;
                var halfDif = 0.5f - currentScale / 2;

                column.ScaleColumn(OsbEasing.OutCirc, 67414, 68188, new Vector2(currentScale), CenterTypeColumn.columnX);

                currentx -= width * scaleMult * 0.25f;

                var half = (currentx - column.receptor.PositionAt(67414).X) / 2;

                column.MoveColumnRelativeX(OsbEasing.OutCirc, 67414, 67705, half);
                column.MoveColumnRelativeX(OsbEasing.OutCirc, 67705, 68188, half);

                var currentHeight = height / 0.5f * currentScale;
            }

            bound.Left = currentx;

            ScrollColumnsWithScaling(columns, 68382, 70124, 800, bound, OsbEasing.InOutSine);
            ScrollColumnsWithScaling(columns, 70124 + 10, 71672, -800, bound, OsbEasing.InOutSine);
            ScrollColumnsWithScaling(columns, 71672 + 10, 73414, 800, bound, OsbEasing.InOutSine);

            foreach (var column in columns)
            {
                var positon = column.receptor.PositionAt(73511).X;
                var targetPosition = 320f;


                switch (column.type)
                {
                    case ColumnType.one:
                        targetPosition = 226.25f;
                        break;
                    case ColumnType.two:
                        targetPosition = 288.75f;
                        break;
                    case ColumnType.three:
                        targetPosition = 351.25f;
                        break;
                    case ColumnType.four:
                        targetPosition = 413.75f;
                        break;
                }

                var difference = positon - targetPosition;
                var half = difference / 2;

                column.MoveColumnRelativeX(OsbEasing.OutCirc, 73511, 73801, -half);
                column.MoveColumnRelativeX(OsbEasing.OutCirc, 73801, 74092, -half);

                column.ScaleColumn(OsbEasing.InCirc, 73511, 74092, new Vector2(0.5f), CenterTypeColumn.columnX);

            }

            bound.Right -= 100;
            bound.Left = OsuHitObject.WidescreenStoryboardBounds.Left - 100;

            currentx = bound.Left;
            columns.Reverse();

            foreach (var column in columns)
            {
                // Calculate progress from left to right of screen
                float progress = (currentx - bound.Left) / (bound.Right - bound.Left);

                // Interpolate scale between far and close
                var currentScale = closeScale + (farScale - closeScale) * progress;
                float scaleMult = currentScale / 0.5f;
                float scaleDiv = 0.5f / currentScale;

                // First step: 67414 to 67705

                var sacleDif = 0.5f - currentScale;
                var halfDif = currentScale / 2;

                column.ScaleColumn(OsbEasing.OutCirc, 74092, 74769, new Vector2(currentScale), CenterTypeColumn.columnX);

                currentx += width * scaleMult * 0.25f;

                var half = (currentx - column.receptor.PositionAt(74092).X) / 2;

                column.MoveColumnRelativeX(OsbEasing.OutCirc, 74092, 74382, half);
                column.MoveColumnRelativeX(OsbEasing.OutCirc, 74382, 74769, half);

                var currentHeight = height / 0.5f * currentScale;

            }

            bound.Right = currentx;

            ScrollColumnsWithScaling(columns, 74769, 76317, -800, bound, OsbEasing.InOutSine);
            ScrollColumnsWithScaling(columns, 76317 + 10, 77866, 800, bound, OsbEasing.InOutSine);
            ScrollColumnsWithScaling(columns, 77866 + 10, 79414, -800, bound, OsbEasing.InOutSine);
            ScrollColumnsWithScaling(columns, 79414 + 10, endtime - 10, 1200, bound, OsbEasing.InOutSine);

            foreach (var field in playfields)
            {
                DrawInstance draw = new DrawInstance(CancellationToken, field, starttime + 10, scrollSpeed, updatesPerSecond, OsbEasing.None, false, fadeTime, fadeTime);
                draw.setReceptorMovementPrecision(1f);
                draw.setReceptorScalePrecision(.05f);
                draw.setNoteMovementPrecision(1f);
                //draw.setNoteScalePrecision(0.5f);
                //draw.CommandSplitThreshold = 50;
                draw.drawViaEquation(duration - 10, NoteFunction, true);
            }
        }

        // NoteFunction is used to manipulate the pathway and a bunch of other things the note should do on their way to the receptor
        // Please be warry that this is beeing run async so you need to keep thread safety in mind when working on complex Functions.
        // You can use the progress to determin how far the note is in its cycle 0 = just start | 1 = ontop of receptor / finished
        // Special flags for hold bodies exist
        public Vector2 NoteFunction(EquationParameters p)
        {
            // Check if note is at screen edge boundaries
            if (p.time >= 68575 && p.time <= 79414) // Time period when scrolling animations are active
            {
                float screenLeft = OsuHitObject.WidescreenStoryboardBounds.Left - 100;
                float screenRight = OsuHitObject.WidescreenStoryboardBounds.Right + 100;

                // Detect if note is at or very near screen edge
                bool atLeftEdge = p.position.X <= screenLeft + 40;
                bool atRightEdge = p.position.X >= screenRight - 40;

                if (atLeftEdge || atRightEdge)
                {
                    lock (_noteLock)
                    {
                        // Don't process the same note too frequently
                        if (!_lastTransitionTime.ContainsKey(p.note) ||
                            p.time - _lastTransitionTime[p.note] > 20)
                        {
                            // Add to transitioning collection
                            _transitioningNotes.Add(p.note);
                            _lastTransitionTime[p.note] = p.time;

                            // Fade out the note
                            p.note.noteSprite.Fade(p.time - 25, 0);
                        }
                    }
                }
                else if (p.position.X > screenLeft + 50 && p.position.X < screenRight - 50)
                {
                    // Note is safely away from edges, check if it needs to be faded back in
                    lock (_noteLock)
                    {
                        if (_transitioningNotes.Contains(p.note))
                        {
                            // Fade the note back in
                            p.note.noteSprite.Fade(p.time + 25, 1);
                            _transitioningNotes.Remove(p.note);
                        }
                    }
                }
            }

            return p.position;
        }

        public float calculateScale(double currentTime, float currentx, Box2 localbound)
        {

            var currentScale = 0f;
            float progress = getProgress(currentTime, currentx, localbound);

            if (currentTime < 73511)
            {
                currentScale = closeScale + (farScale - closeScale) * progress; //farScale + (closeScale - farScale) * progress;
            }
            else if (currentTime < 80575 && currentTime > 74092)
            {
                currentScale = closeScale + (farScale - closeScale) * progress;
            }

            // Interpolate scale between far and close
            return currentScale;
        }

        public float getProgress(double currentTime, float currentx, Box2 localBound)
        {
            float progress = 0f;
            if (currentTime < 73511)
            {
                progress = (currentx - localBound.Right) / (localBound.Left - localBound.Right);
            }
            else if (currentTime < 80575 && currentTime > 74092)
            {
                progress = (currentx - localBound.Left) / (localBound.Right - localBound.Left);
            }

            return progress;
        }

        public void ScrollColumnsWithScaling(List<Column> columns, double startTime, double endTime, float moveDistance, Box2 bounds, OsbEasing easing = OsbEasing.None)
        {
            moveDistance = moveDistance / 1000f; // Convert to seconds
            double duration = endTime - startTime;
            moveDistance = moveDistance * (float)duration;
            double step = 6; // Update every 50ms
            float totalSeps = (float)(duration / step);

            foreach (var column in columns)
            {
                double currentTime = startTime;
                float currentX = column.receptor.PositionAt(startTime).X;
                float lastProgress = 0f;

                while (currentTime < endTime)
                {
                    float easedProgress = (float)easing.Ease((currentTime - startTime) / duration);

                    float progressThisStep = easedProgress - lastProgress;
                    lastProgress = easedProgress;

                    var adjustment = startTime < 74769 ? -10 : 10;

                    // Calculate progress for scaling
                    float progress = (currentX - (OsuHitObject.WidescreenStoryboardBounds.Right + adjustment)) / (OsuHitObject.WidescreenStoryboardBounds.Left - (OsuHitObject.WidescreenStoryboardBounds.Right));

                    // Interpolate scale between far and close
                    var currentScale = startTime < 74769 ? closeScale + (farScale - closeScale) * progress : farScale + (closeScale - farScale) * progress;

                    float easedMoveDistance = moveDistance * totalSeps * progressThisStep;

                    // Calculate scaled X movement based on the current scale
                    float scaledX = easedMoveDistance * (currentScale / 0.5f);

                    // Calculate the next X position
                    float nextX = currentX + scaledX / totalSeps;

                    bool isOutOfBounds = false;

                    // Wrap around logic for bounds
                    if (nextX > bounds.Right && currentTime < 80188)
                    {
                        isOutOfBounds = true;
                        nextX = bounds.Left;
                    }
                    else if (nextX < bounds.Left && currentTime < 80188)
                    {
                        nextX = bounds.Right;
                        isOutOfBounds = true;
                    }

                    if (isOutOfBounds)
                    {
                        column.receptor.renderedSprite.Fade(currentTime - 100, 0);
                        column.receptor.renderedSprite.Fade(currentTime + 100, 1);
                    }

                    // Apply movement and scaling
                    column.ScaleColumn(OsbEasing.None, currentTime, currentTime + step - 1, new Vector2(currentScale), CenterTypeColumn.columnX);
                    column.MoveColumnRelativeX(OsbEasing.None, currentTime, currentTime + step, nextX - currentX);

                    // Update currentX and time
                    currentX = nextX;
                    currentTime += step;
                }
            }
        }

        // Deep copy of Box2 object
        public Box2 DeepCopyBox2(Box2 original)
        {
            return new Box2(
                original.Left,
                original.Top,
                original.Right,
                original.Bottom
            );
        }
    }
}
