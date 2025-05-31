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
    public class IntroAlt : StoryboardObjectGenerator
    {
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
            var sliderAccuracy = 25; // The Segment length for sliderbodies since they are rendered in slices 30 is default
            var isColored = false; // This property is used if you want to color the notes by urself for effects. It does not swap if the snap coloring is used.

            // Drawinstance Values
            var updatesPerSecond = 100; // The amount of steps the rendring engine does to render out note and receptor positions
            var scrollSpeed = 1200f; // The speed at which the Notes scroll
            var fadeTime = 150; // The time notes will fade in


            Playfield field = new Playfield();
            field.initilizePlayField(receptors, notes, starttime, endtime, width, height, receptorWallOffset, Beatmap.OverallDifficulty);
            field.initializeNotes(Beatmap.HitObjects.ToList(), Beatmap.GetTimingPointAt(starttime).Bpm, Beatmap.GetTimingPointAt(starttime).Offset, isColored, sliderAccuracy);

            Playfield field2 = new Playfield();
            field2.initilizePlayField(receptors, notes, starttime, endtime, width, height, receptorWallOffset, Beatmap.OverallDifficulty);
            field2.initializeNotes(Beatmap.HitObjects.ToList(), Beatmap.GetTimingPointAt(starttime).Bpm, Beatmap.GetTimingPointAt(starttime).Offset, isColored, sliderAccuracy);

            field.Resize(OsbEasing.None, 1608, 1608, 0, height);
            field2.Resize(OsbEasing.None, 1608, 1608, 0, height);

            field.ScaleReceptor(OsbEasing.None, 1608, 1608, new Vector2(0.5f, 0f), ColumnType.one);
            field.ScaleReceptor(OsbEasing.None, 1608, 1608, new Vector2(0f, 0.5f), ColumnType.two);
            field.ScaleReceptor(OsbEasing.None, 1608, 1608, new Vector2(0f, 0.5f), ColumnType.three);
            field.ScaleReceptor(OsbEasing.None, 1608, 1608, new Vector2(0.5f, 0f), ColumnType.four);

            field.ScaleReceptor(OsbEasing.OutCubic, 3543, 4704, new Vector2(0.5f, 0.5f), ColumnType.one);
            field.ScaleReceptor(OsbEasing.OutCubic, 3543, 4704, new Vector2(0.5f, 0.5f), ColumnType.two);
            field.ScaleReceptor(OsbEasing.OutCubic, 3543, 4704, new Vector2(0.5f, 0.5f), ColumnType.three);
            field.ScaleReceptor(OsbEasing.OutCubic, 3543, 4704, new Vector2(0.5f, 0.5f), ColumnType.four);

            field2.ScaleReceptor(OsbEasing.None, 1608, 1608, new Vector2(0.5f, 0f), ColumnType.one);
            field2.ScaleReceptor(OsbEasing.None, 1608, 1608, new Vector2(0f, 0.5f), ColumnType.two);
            field2.ScaleReceptor(OsbEasing.None, 1608, 1608, new Vector2(0f, 0.5f), ColumnType.three);
            field2.ScaleReceptor(OsbEasing.None, 1608, 1608, new Vector2(0.5f, 0f), ColumnType.four);

            field2.ScaleReceptor(OsbEasing.OutCubic, 3543, 4704, new Vector2(0.5f, 0.5f), ColumnType.one);
            field2.ScaleReceptor(OsbEasing.OutCubic, 3543, 4704, new Vector2(0.5f, 0.5f), ColumnType.two);
            field2.ScaleReceptor(OsbEasing.OutCubic, 3543, 4704, new Vector2(0.5f, 0.5f), ColumnType.three);
            field2.ScaleReceptor(OsbEasing.OutCubic, 3543, 4704, new Vector2(0.5f, 0.5f), ColumnType.four);

            field.Resize(OsbEasing.OutCubic, 3543, 4704, width, height);
            field2.Resize(OsbEasing.OutCubic, 3543, 4704, width, height);

            field.fadeAt(1608, 0);
            field.fadeAt(3543, 4704, OsbEasing.OutCubic, 1);

            field2.fadeAt(1608, 0);
            field2.fadeAt(3543, 4704, OsbEasing.OutCubic, 1);

            field.MoveOriginRelative(OsbEasing.None, 1608, 1608, new Vector2(0, -height), ColumnType.all);
            field.MoveOriginRelative(OsbEasing.None, 3543, 4704, new Vector2(0, height), ColumnType.all);

            field2.MoveOriginRelative(OsbEasing.None, 1608, 1608, new Vector2(0, -height), ColumnType.all);
            field2.MoveOriginRelative(OsbEasing.None, 3543, 4704, new Vector2(0, height), ColumnType.all);

            // Replace the large animation loop with this single function call
            CircularFieldAnimation(field, field2, 4704, 5479 - 10, 2, 150, 40, 200);

            field.Rotate(OsbEasing.OutCubic, 5479, 6253, -Math.PI * 3 - 0.15f);
            field.Rotate(OsbEasing.OutElasticHalf, 6253, 6834, +0.15f);

            field2.Rotate(OsbEasing.OutCubic, 5479, 6253, Math.PI * 3 + 0.15f);
            field2.Rotate(OsbEasing.OutElasticHalf, 6253, 6834, -0.15f);

            FlipPlayFieldDepth(field, 6640, 6737, 7027, -width, height);
            FlipPlayFieldDepth(field2, 6640, 6737, 7027, -width, height);

            FlipPlayFieldDepth(field, 7221, 7317, 7608, width, -height, 0.25f, 1f);
            FlipPlayFieldDepth(field2, 7221, 7317, 7608, width, -height, 0.25f, 1f);

            CircularFieldAnimation(field, field2, 7801, 8575 - 10, 2, -150, 40, 200);

            field.MoveColumnRelativeX(OsbEasing.OutCubic, 8575, 8769, -field.getColumnWidth(), ColumnType.one);
            field.MoveColumnRelativeX(OsbEasing.OutCubic, 8575, 8769, field.getColumnWidth(), ColumnType.two);
            field.MoveColumnRelativeX(OsbEasing.OutCubic, 8575, 8769, -field.getColumnWidth(), ColumnType.three);
            field.MoveColumnRelativeX(OsbEasing.OutCubic, 8575, 8769, field.getColumnWidth(), ColumnType.four);

            field2.MoveColumnRelativeX(OsbEasing.OutCubic, 8575, 8769, -field.getColumnWidth(), ColumnType.one);
            field2.MoveColumnRelativeX(OsbEasing.OutCubic, 8575, 8769, field.getColumnWidth(), ColumnType.two);
            field2.MoveColumnRelativeX(OsbEasing.OutCubic, 8575, 8769, -field.getColumnWidth(), ColumnType.three);
            field2.MoveColumnRelativeX(OsbEasing.OutCubic, 8575, 8769, field.getColumnWidth(), ColumnType.four);

            field.MoveColumnRelativeX(OsbEasing.OutCubic, 8963, 9156, -field.getColumnWidth() * 2, ColumnType.one);
            field.MoveColumnRelativeX(OsbEasing.OutCubic, 8963, 9156, -field.getColumnWidth() * 2, ColumnType.two);
            field.MoveColumnRelativeX(OsbEasing.OutCubic, 8963, 9156, field.getColumnWidth() * 2, ColumnType.three);
            field.MoveColumnRelativeX(OsbEasing.OutCubic, 8963, 9156, field.getColumnWidth() * 2, ColumnType.four);

            field2.MoveColumnRelativeX(OsbEasing.OutCubic, 8963, 9156, -field.getColumnWidth() * 2, ColumnType.one);
            field2.MoveColumnRelativeX(OsbEasing.OutCubic, 8963, 9156, -field.getColumnWidth() * 2, ColumnType.two);
            field2.MoveColumnRelativeX(OsbEasing.OutCubic, 8963, 9156, field.getColumnWidth() * 2, ColumnType.three);
            field2.MoveColumnRelativeX(OsbEasing.OutCubic, 8963, 9156, field.getColumnWidth() * 2, ColumnType.four);


            DrawInstance draw = new DrawInstance(CancellationToken, field, starttime, scrollSpeed, updatesPerSecond, OsbEasing.None, false, fadeTime, fadeTime);
            draw.setReceptorMovementPrecision(0.5f);
            draw.setNoteRotationPrecision(0.25f);
            draw.setHoldMovementPrecision(0.5f);
            draw.setHoldRotationPrecision(0.1f);
            draw.setHoldScalePrecision(0.1f);
            draw.drawViaEquation(duration, NoteFunction, true);

            DrawInstance draw2 = new DrawInstance(CancellationToken, field2, starttime, scrollSpeed, updatesPerSecond, OsbEasing.None, false, fadeTime, fadeTime);
            draw2.setReceptorMovementPrecision(0.5f);
            draw2.setNoteRotationPrecision(0.25f);
            draw2.setHoldMovementPrecision(0.5f);
            draw2.setHoldRotationPrecision(0.1f);
            draw2.setHoldScalePrecision(0.1f); ;
            draw2.drawViaEquation(duration, NoteFunction, true);
        }

        public void SpinFieldOnce(Playfield field, double starttime, double endtime)
        {

            field.RotateReceptor(OsbEasing.OutSine, 4704, 4898, -0.3f, CenterType.receptor);
            field.RotateReceptor(OsbEasing.InSine, 4898, 5470, 0.3f, CenterType.receptor);
            field.ScaleColumn(OsbEasing.OutSine, 4704, 4898, new Vector2(0.325f), ColumnType.one);
            field.ScaleColumn(OsbEasing.OutSine, 4704, 4898, new Vector2(0.425f), ColumnType.two);
            field.ScaleColumn(OsbEasing.OutSine, 4704, 4898, new Vector2(0.525f), ColumnType.three);
            field.ScaleColumn(OsbEasing.OutSine, 4704, 4898, new Vector2(0.575f), ColumnType.four);
            field.ScaleColumn(OsbEasing.InSine, 4898, 5470, new Vector2(0.5f, 0.5f), ColumnType.one);
            field.ScaleColumn(OsbEasing.InSine, 4898, 5470, new Vector2(0.5f, 0.5f), ColumnType.two);
            field.ScaleColumn(OsbEasing.InSine, 4898, 5470, new Vector2(0.5f, 0.5f), ColumnType.three);
            field.ScaleColumn(OsbEasing.InSine, 4898, 5470, new Vector2(0.5f, 0.5f), ColumnType.four);

        }

        // Calculate position on an ellipse with different x/y radii
        public Vector2 GetPositionOnEllipse(double centerX, double centerY, double radiusX, double radiusY, double angleRadians)
        {
            double x = centerX + radiusX * Math.Cos(angleRadians);
            double y = centerY + radiusY * Math.Sin(angleRadians);

            return new Vector2((float)x, (float)y);
        }

        double Blend(double a, double b, double t)
        {
            return a + (b - a) * t;
        }

        public void CircularFieldAnimation(Playfield field1, Playfield field2, double startTime, double endTime,
                                  int numRotations = 2, float maxRadius = 150, float maxHeight = 40, int minSteps = 200)
        {
            double totalDuration = endTime - startTime;

            var startPos = field1.columns[ColumnType.one].receptor.PositionAt(startTime);
            var field1Pos = field1.columns[ColumnType.one].receptor.PositionAt(startTime);
            var field2Pos = field2.columns[ColumnType.one].receptor.PositionAt(startTime);

            // Set up total angle to rotate
            double totalAngle = Math.PI * 2 * numRotations;

            for (int step = 0; step < minSteps; step++)
            {
                CancellationToken.ThrowIfCancellationRequested();

                // Calculate progress (0 to 1)
                double progress = step / (double)(minSteps - 1);

                // Apply easing to the rotation
                double easedRotationProgress = OsbEasing.InSine.Ease(progress);
                double currentAngle = totalAngle * easedRotationProgress;

                // Initialize scales
                float currentScale = 0.5f;
                float currentScale2 = 0.5f;

                // Get normalized angle position in the circle (0 to 2π)
                double normalizedAngle = currentAngle % (Math.PI * 2);
                double normalizedAngle2 = (currentAngle + Math.PI) % (Math.PI * 2);
                if (normalizedAngle < 0) normalizedAngle += Math.PI * 2;
                if (normalizedAngle2 < 0) normalizedAngle2 += Math.PI * 2;

                // Apply different easing based on which half of the circle we're in
                if (normalizedAngle < Math.PI)
                {
                    // First half (0 to π): InSine easing
                    double sinProgress = OsbEasing.InSine.Ease(normalizedAngle / Math.PI);
                    currentScale = (float)Blend(0.4, 0.5, sinProgress);

                    // For the second field (opposite side of circle)
                    double sinProgress2 = OsbEasing.InSine.Ease(normalizedAngle2 / Math.PI);
                    currentScale2 = (float)Blend(0.5, 0.4, sinProgress2);
                }
                else
                {
                    // Second half (π to 2π): OutSine easing
                    double sinProgress = OsbEasing.OutSine.Ease((normalizedAngle - Math.PI) / Math.PI);
                    currentScale = (float)Blend(0.5, 0.4, sinProgress);

                    // For the second field (opposite side of circle)
                    double sinProgress2 = OsbEasing.OutSine.Ease((normalizedAngle2 - Math.PI) / Math.PI);
                    currentScale2 = (float)Blend(0.4, 0.5, sinProgress2);
                }

                // If we're near the end, gradually return to normal scale
                if (progress > 0.9)
                {
                    double finalScaleProgress = (progress - 0.9) / 0.1; // Scale from 0 to 1
                    double finalEasedProgress = OsbEasing.OutQuad.Ease(finalScaleProgress);
                    currentScale = (float)Blend(currentScale, 0.5f, finalEasedProgress);
                    currentScale2 = (float)Blend(currentScale2, 0.5f, finalEasedProgress);
                }

                // Calculate current radius, shrinking towards the end
                double currentRadiusX = Blend(maxRadius, 0, easedRotationProgress);
                double currentRadiusY = Blend(maxHeight, 0, easedRotationProgress);

                // Calculate positions on ellipse
                var movement1 = GetPositionOnEllipse(startPos.X, startPos.Y, currentRadiusX, currentRadiusY, currentAngle);
                var movement2 = GetPositionOnEllipse(startPos.X, startPos.Y, currentRadiusX, currentRadiusY, currentAngle + Math.PI);

                // Calculate relative movements
                var field1Movement = movement1 - field1Pos;
                var field2Movement = movement2 - field2Pos;

                // Calculate current time based on easing for variable speed
                double currentProgress = OsbEasing.InSine.Ease(progress);
                double currentTime = startTime + (totalDuration * currentProgress);
                double nextTime = (step == minSteps - 1) ?
                                  endTime :
                                  startTime + (totalDuration * OsbEasing.InSine.Ease((step + 1) / (double)(minSteps - 1)));

                // Move receptors
                field1.MoveReceptorRelative(OsbEasing.None, currentTime, nextTime, field1Movement, ColumnType.all);
                field2.MoveReceptorRelative(OsbEasing.None, currentTime, nextTime, field2Movement, ColumnType.all);

                // Scale receptors
                field1.ScaleReceptor(OsbEasing.None, currentTime, nextTime, new Vector2(currentScale), ColumnType.all);
                field2.ScaleReceptor(OsbEasing.None, currentTime, nextTime, new Vector2(currentScale2), ColumnType.all);

                // Update positions for next step
                field1Pos = movement1;
                field2Pos = movement2;
            }
        }

        void FlipPlayFieldDepth(Playfield field, double startTime, double midTime, double endTime, float width, float height, float receptorScale = 1f, float originScale = 0.25f)
        {
            // Calculate durations based on provided timing points
            double firstSegmentDuration = midTime - startTime;
            double secondSegmentDuration = endTime - midTime;

            // Scale receptors
            field.ScaleReceptor(OsbEasing.InSine, startTime, midTime, new Vector2(receptorScale), ColumnType.all);
            field.ScaleReceptor(OsbEasing.OutCubic, midTime, endTime, new Vector2(0.5f), ColumnType.all);

            // Scale origins
            field.ScaleOrigin(OsbEasing.InSine, startTime, midTime, new Vector2(originScale), ColumnType.all);
            field.ScaleOrigin(OsbEasing.OutCubic, midTime, endTime, new Vector2(0.5f), ColumnType.all);

            var columnWidth = field.getColumnWidth();

            // Flip the playfield by inverting the height

            if (height > 0)
            {
                field.moveFieldY(OsbEasing.OutSine, startTime, endTime, -480f + 50f * 2);
                field.MoveOriginRelative(OsbEasing.OutSine, startTime, endTime, new Vector2(0, height * 2), ColumnType.all);
            }
            else
            {
                field.moveFieldY(OsbEasing.OutSine, startTime, endTime, 480f - 50f * 2);
                field.MoveOriginRelative(OsbEasing.OutSine, startTime, endTime, new Vector2(0, height * 2), ColumnType.all);
            }

            if (width > 0)
            {
                columnWidth = -columnWidth; // Invert column width for right side
            }

            // First phase: Move receptors outward
            field.MoveReceptorRelative(OsbEasing.InSine, startTime, midTime, new Vector2(columnWidth, 0), ColumnType.one);
            field.MoveReceptorRelative(OsbEasing.InSine, startTime, midTime, new Vector2(columnWidth / 2, 0), ColumnType.two);
            field.MoveReceptorRelative(OsbEasing.InSine, startTime, midTime, new Vector2(-columnWidth / 2, 0), ColumnType.three);
            field.MoveReceptorRelative(OsbEasing.InSine, startTime, midTime, new Vector2(-columnWidth, 0), ColumnType.four);

            // Second phase: Move receptors inward
            field.MoveReceptorRelative(OsbEasing.OutSine, midTime, endTime, new Vector2(-columnWidth, 0), ColumnType.one);
            field.MoveReceptorRelative(OsbEasing.OutSine, midTime, endTime, new Vector2(-columnWidth / 2, 0), ColumnType.two);
            field.MoveReceptorRelative(OsbEasing.OutSine, midTime, endTime, new Vector2(columnWidth / 2, 0), ColumnType.three);
            field.MoveReceptorRelative(OsbEasing.OutSine, midTime, endTime, new Vector2(columnWidth, 0), ColumnType.four);

            // First phase: Move origins inward
            field.MoveOriginRelative(OsbEasing.InSine, startTime, midTime, new Vector2(-columnWidth, 0), ColumnType.one);
            field.MoveOriginRelative(OsbEasing.InSine, startTime, midTime, new Vector2(-columnWidth / 2, 0), ColumnType.two);
            field.MoveOriginRelative(OsbEasing.InSine, startTime, midTime, new Vector2(columnWidth / 2, 0), ColumnType.three);
            field.MoveOriginRelative(OsbEasing.InSine, startTime, midTime, new Vector2(columnWidth, 0), ColumnType.four);

            // Second phase: Move origins outward
            field.MoveOriginRelative(OsbEasing.OutSine, midTime, endTime, new Vector2(columnWidth, 0), ColumnType.one);
            field.MoveOriginRelative(OsbEasing.OutSine, midTime, endTime, new Vector2(columnWidth / 2, 0), ColumnType.two);
            field.MoveOriginRelative(OsbEasing.OutSine, midTime, endTime, new Vector2(-columnWidth / 2, 0), ColumnType.three);
            field.MoveOriginRelative(OsbEasing.OutSine, midTime, endTime, new Vector2(-columnWidth, 0), ColumnType.four);
        }



        // NoteFunction is used to manipulate the pathway and a bunch of other things the note should do on their way to the receptor
        // Please be warry that this is beeing run async so you need to keep thread safety in mind when working on complex Functions.
        // You can use the progress to determin how far the note is in its cycle 0 = just start | 1 = ontop of receptor / finished
        // Special flags for hold bodies exist
        public Vector2 NoteFunction(EquationParameters p)
        {
            return p.position;
        }
    }
}