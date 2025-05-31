using Microsoft.VisualBasic;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Input;
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
    public class AltAltFirstBuildUp : StoryboardObjectGenerator
    {

        private readonly object _lock = new object();
        public override void Generate()
        {

            var receptors = GetLayer("r");
            var notes = GetLayer("n");

            // General values
            var starttime = 17866; // the starttime where the playfield is initialized
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
            var fadeTime = 150; // The time notes will fade in

            var playfields = 3;

            var movementDur = 2000;

            var offset = movementDur / 3 / 4;

            var quarterDur = movementDur / 4;
            var circleHeight = 200;
            var circleWidth = 200;

            var smallWidth = 100;
            var smallHeight = 40;
            var closeScale = 0.5f;
            var farScale = 0.3f;

            var totalColIndex = 0;

            for (int i = 0; i < playfields; i++)
            {
                Playfield field = new Playfield();
                field.initilizePlayField(receptors, notes, starttime - 50, endtime, 0, -height, receptorWallOffset, Beatmap.OverallDifficulty);
                field.noteEnd = 44963;
                field.initializeNotes(Beatmap.HitObjects.ToList(), Beatmap, isColored, sliderAccuracy);

                field.ScaleReceptor(OsbEasing.None, starttime - 50, starttime - 50, new Vector2(farScale), ColumnType.all);

                field.moveField(OsbEasing.None, starttime - 50, starttime - 50, 0, -circleWidth + 20);

                field.ScaleOrigin(OsbEasing.None, starttime - 50, starttime - 50, new Vector2(0f), ColumnType.all);
                field.ScaleReceptor(OsbEasing.None, starttime - 50, starttime - 50, new Vector2(0f), ColumnType.all);
                field.ScaleReceptor(OsbEasing.InCubic, 17866, 17866 + 100, new Vector2(1f), ColumnType.all);
                field.ScaleReceptor(OsbEasing.OutCubic, 17866 + 100, 18640, new Vector2(0.4f), ColumnType.all);
                field.ScaleOrigin(OsbEasing.InOutSine, 29479, 31027, new Vector2(0.4f), ColumnType.all);
                field.Resize(OsbEasing.None, starttime - 50, starttime - 50, 0, 0);

                field.MoveOriginAbsolute(OsbEasing.None, starttime - 50, starttime - 50, new Vector2(320, 240), ColumnType.all);



                var currentColumn = 0;
                var inverseColumns = field.columns.Values.Reverse();

                // Replace the circular movement code with this implementation
                foreach (var col in inverseColumns)
                {

                    KeyframedValue<float> opacity = new KeyframedValue<float>(InterpolatingFunctions.Float, 1f);

                    // Animation parameters
                    double stepTime = 50; // milliseconds per step (smaller = smoother animation)
                    double totalDuration = 41866 - starttime; // Duration until next section
                    double currentTime = starttime;

                    // Initial circle dimensions - can be adjusted
                    float baseCircleWidth = circleWidth;
                    float baseCircleHeight = circleHeight;

                    // Track the position to ensure continuous movement
                    float lastAngle = 0;

                    var totalColumns = playfields * 4;
                    var anglePerCol = Math.PI * 2 / totalColumns; // Angle increment per column

                    // Starting point
                    var centerPoint = new Vector2(320, 240); // Center of the screen
                    var lastPos = new Vector2(320, 240);
                    var lastColPos = new Vector2(320, 240);

                    var angleStep = Math.PI / 2 / movementDur * stepTime;

                    double angle = anglePerCol * totalColIndex;

                    var transitionTimeStart = 29479;
                    var transitionTimeEnd = 31027;

                    var transitionStart = 17866;
                    var transitionEnd = 18640;

                    while (currentTime < 41866)
                    {
                        double easedProgress = 0;
                        double outSineProgress = 0;
                        if (currentTime >= transitionTimeStart && currentTime <= transitionTimeEnd)
                        {
                            // Calculate eased progress for the transition
                            var transitionProgress = (currentTime - transitionTimeStart) / (transitionTimeEnd - transitionTimeStart);
                            easedProgress = OsbEasing.InOutSine.Ease(transitionProgress);
                            outSineProgress = OsbEasing.InExpo.Ease(transitionProgress);
                        }
                        else if (currentTime >= transitionTimeEnd)
                        {
                            easedProgress = 1;
                            outSineProgress = 1;
                        }

                        // Calculate current dimensions
                        double currentWidth = Blend(baseCircleWidth, smallWidth, easedProgress);
                        double currentHeight = Blend(baseCircleHeight, smallHeight, easedProgress);

                        if (currentTime >= transitionStart && currentTime <= transitionEnd)
                        {
                            // Calculate eased progress for the transition
                            var transitionProgress = (currentTime - transitionStart) / (transitionEnd - transitionStart);
                            easedProgress = 1f - OsbEasing.OutExpo.Ease(transitionProgress);

                            currentWidth = Blend(currentWidth, 0, easedProgress);
                            currentHeight = Blend(currentHeight, 0, easedProgress);
                        }

                        double x = currentWidth * Math.Cos(angle);
                        double y = currentHeight * Math.Sin(angle);

                        float currentScale;
                        float currentOpacity;

                        // Get normalized angle position in the circle (0 to 2π)
                        // Rotate by 90 degrees (π/2) clockwise
                        double normalizedAngle = (angle + Math.PI / 2) % (Math.PI * 2);
                        if (normalizedAngle < 0) normalizedAngle += Math.PI * 2;

                        // Apply different easing based on which half of the circle we're in
                        if (normalizedAngle < Math.PI)
                        {
                            // First half (0 to π): InSine easing
                            double sinProgress = OsbEasing.InSine.Ease(normalizedAngle / Math.PI);
                            currentScale = (float)Blend(farScale, closeScale, sinProgress);
                            currentOpacity = (float)Blend(0.1f, 1f, sinProgress);
                        }
                        else
                        {
                            // Second half (π to 2π): OutSine easing
                            double sinProgress = OsbEasing.OutSine.Ease((normalizedAngle - Math.PI) / Math.PI);
                            currentScale = (float)Blend(closeScale, farScale, sinProgress);
                            currentOpacity = (float)Blend(1f, 0.1f, sinProgress);
                        }

                        // Move column to the calculated position
                        Vector2 newPosition = centerPoint + new Vector2((float)x, (float)y);
                        Vector2 change = newPosition - lastPos;
                        col.MoveReceptorRelative(OsbEasing.None, currentTime, currentTime + stepTime, change);

                        // Apply the scale based on position in circle

                        // Apply fade based on position in circle (only when we're in transition or after)

                        if (currentTime >= transitionTimeStart && currentTime <= transitionTimeEnd)
                        {
                            opacity.Add(currentTime, (float)Blend(1f, currentOpacity, easedProgress));
                            field.ScaleReceptor(OsbEasing.None, currentTime, currentTime + stepTime, new Vector2((float)Blend(0.4f, currentScale, easedProgress)), col.type);
                        }
                        else if (currentTime > transitionTimeEnd)
                        {
                            opacity.Add(currentTime, currentOpacity);
                            field.ScaleReceptor(OsbEasing.None, currentTime, currentTime + stepTime, new Vector2(currentScale), col.type);
                        }

                        if (easedProgress > 0)
                        {
                            // Get the current position of column origin
                            var colPos = col.OriginPositionAt(currentTime);

                            // Calculate the target position (lastColPos is newPosition for receptors)
                            Vector2 targetPosition = newPosition;

                            // Blend between current position and target position based on easedProgress
                            Vector2 blendedPosition = new Vector2(
                                colPos.X + (targetPosition.X - colPos.X) * (float)outSineProgress,
                                colPos.Y
                            );

                            // Calculate the necessary movement to reach the blended position
                            Vector2 colChange = blendedPosition - colPos;

                            // Apply the movement
                            col.MoveOriginRelative(OsbEasing.None, currentTime, currentTime + stepTime, colChange);

                            // Store the last position for next iteration
                            lastColPos = targetPosition;
                        }

                        // Update time tracking
                        currentTime += stepTime;
                        angle += angleStep;
                        lastPos = newPosition;
                    }

                    opacity.Simplify(.05f);
                    opacity.Add(starttime, 1f);
                    opacity.Add(transitionTimeStart, 1f);

                    opacity.ForEachPair((start, end) =>
                    {
                        col.receptor.renderedSprite.Fade(start.Time, end.Time, start.Value, end.Value);
                    });

                    totalColIndex++;
                }

                field.MoveReceptorRelative(OsbEasing.InOutSine, 29479, 31027, new Vector2(0, 150), ColumnType.all);
                field.MoveOriginRelative(OsbEasing.InOutSine, 29479, 31027, new Vector2(0, -350), ColumnType.all);

                var local = 31027;
                var dur = 2500;

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

                field.MoveReceptorRelative(OsbEasing.OutQuad, 41866, 41866 + 1500, new Vector2(226.25f, 50f) - field.columns[ColumnType.one].receptor.PositionAt(41866 + 1500), ColumnType.one);
                field.MoveReceptorRelative(OsbEasing.OutQuad, 41866, 41866 + 1500, new Vector2(288.75f, 50f) - field.columns[ColumnType.two].receptor.PositionAt(41866 + 1500), ColumnType.two);
                field.MoveReceptorRelative(OsbEasing.OutQuad, 41866, 41866 + 1500, new Vector2(351.25f, 50f) - field.columns[ColumnType.three].receptor.PositionAt(41866 + 1500), ColumnType.three);
                field.MoveReceptorRelative(OsbEasing.OutQuad, 41866, 41866 + 1500, new Vector2(413.75f, 50f) - field.columns[ColumnType.four].receptor.PositionAt(41866 + 1500), ColumnType.four);

                field.MoveOriginRelative(OsbEasing.OutExpo, 41866, 41866 + 1500, new Vector2(226.25f, 550f) - field.columns[ColumnType.one].origin.PositionAt(41866 + 1500), ColumnType.one);
                field.MoveOriginRelative(OsbEasing.OutExpo, 41866, 41866 + 1500, new Vector2(288.75f, 550f) - field.columns[ColumnType.two].origin.PositionAt(41866 + 1500), ColumnType.two);
                field.MoveOriginRelative(OsbEasing.OutExpo, 41866, 41866 + 1500, new Vector2(351.25f, 550f) - field.columns[ColumnType.three].origin.PositionAt(41866 + 1500), ColumnType.three);
                field.MoveOriginRelative(OsbEasing.OutExpo, 41866, 41866 + 1500, new Vector2(413.75f, 550f) - field.columns[ColumnType.four].origin.PositionAt(41866 + 1500), ColumnType.four);


                DrawInstance draw = new DrawInstance(CancellationToken, field, 17866, scrollSpeed, updatesPerSecond, OsbEasing.None, false, fadeTime, fadeTime);
                draw.setReceptorMovementPrecision(.25f);
                draw.setNoteMovementPrecision(.25f);
                draw.noteScaleEasing = OsbEasing.OutExpo;
                //draw.customScale = true;
                draw.drawViaEquation(endtime - 17866, NoteFunction, true);
            }
        }

        double Blend(double a, double b, double t)
        {
            return a + (b - a) * t;
        }

        public Vector2 NoteFunction(EquationParameters p)
        {
            var pos = p.position;

            if (p.time > 28704)
            {
                lock (_lock)
                {
                    p.note.noteSprite.Fade(p.time, p.column.receptor.renderedSprite.OpacityAt(p.time));
                }
            }

            return pos;
        }
    }
}
