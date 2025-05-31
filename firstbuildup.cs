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
    public class firstbuildup : StoryboardObjectGenerator
    {
        //        public override bool Multithreaded => false;
        private readonly object _lock = new object();
        // Generate function in a storybrew script
        public override void Generate()
        {

            var receptors = GetLayer("r");
            var notes = GetLayer("n");

            // General values
            var starttime = 17092; // the starttime where the playfield is initialized
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
            var scrollSpeed = 4000; // The speed at which the Notes scroll
            var fadeTime = 0; // The time notes will fade in

            Playfield field = new Playfield();
            field.initilizePlayField(receptors, notes, starttime, endtime, width, -height, receptorWallOffset, Beatmap.OverallDifficulty);
            field.noteEnd = 44963;
            field.initializeNotes(Beatmap.HitObjects.ToList(), Beatmap.GetTimingPointAt(starttime).Bpm, Beatmap.GetTimingPointAt(starttime).Offset, isColored, sliderAccuracy);

            var center = new Vector2(320, 240);
            field.Scale(OsbEasing.None, 17866, 17866, new Vector2(0.00001f), true);

            field.ScaleReceptor(OsbEasing.InOutCubic, 17866, 18446, new Vector2(0.5f), ColumnType.all);

            var localStart = 18253;
            var quarterDur = 2000;
            var circleWidth = -100;
            var circleHeight = 50;

            field.MoveOriginRelative(OsbEasing.InOutSine, 17866, 18446, new Vector2(-circleWidth / 2, 200), ColumnType.all);
            field.MoveReceptorRelative(OsbEasing.InOutSine, 17866, 18446, new Vector2(-circleWidth, -circleHeight / 2), ColumnType.all);

            while (localStart < 41866)
            {
                field.MoveOriginRelative(OsbEasing.InOutSine, localStart, localStart + quarterDur * 2, new Vector2(field.getColumnWidth() * 4f, 0), ColumnType.one);
                field.MoveOriginRelative(OsbEasing.InOutSine, localStart, localStart + quarterDur * 2, new Vector2(field.getColumnWidth() * 2f, 0), ColumnType.two);
                field.MoveOriginRelative(OsbEasing.InOutSine, localStart, localStart + quarterDur * 2, new Vector2(-field.getColumnWidth() * 2f, 0), ColumnType.three);
                field.MoveOriginRelative(OsbEasing.InOutSine, localStart, localStart + quarterDur * 2, new Vector2(-field.getColumnWidth() * 4f, 0), ColumnType.four);

                field.MoveOriginRelative(OsbEasing.InOutSine, localStart + quarterDur * 2, localStart + quarterDur * 4, new Vector2(-field.getColumnWidth() * 4f, 0), ColumnType.one);
                field.MoveOriginRelative(OsbEasing.InOutSine, localStart + quarterDur * 2, localStart + quarterDur * 4, new Vector2(-field.getColumnWidth() * 2f, 0), ColumnType.two);
                field.MoveOriginRelative(OsbEasing.InOutSine, localStart + quarterDur * 2, localStart + quarterDur * 4, new Vector2(field.getColumnWidth() * 2f, 0), ColumnType.three);
                field.MoveOriginRelative(OsbEasing.InOutSine, localStart + quarterDur * 2, localStart + quarterDur * 4, new Vector2(field.getColumnWidth() * 4f, 0), ColumnType.four);

                // Column two (up)
                field.moveFieldX(OsbEasing.InSine, localStart, localStart + quarterDur, circleWidth);
                field.moveFieldY(OsbEasing.OutSine, localStart, localStart + quarterDur, -circleHeight);
                localStart += quarterDur;

                // Column two (up)
                field.moveFieldX(OsbEasing.OutSine, localStart, localStart + quarterDur, circleWidth);
                field.moveFieldY(OsbEasing.InSine, localStart, localStart + quarterDur, circleHeight);
                localStart += quarterDur;

                // Column two (up)
                field.moveFieldX(OsbEasing.InSine, localStart, localStart + quarterDur, -circleWidth);
                field.moveFieldY(OsbEasing.OutSine, localStart, localStart + quarterDur, circleHeight);
                localStart += quarterDur;

                // Column two (up)
                field.moveFieldX(OsbEasing.OutSine, localStart, localStart + quarterDur, -circleWidth);
                field.moveFieldY(OsbEasing.InSine, localStart, localStart + quarterDur, -circleHeight);
                localStart += quarterDur;
            }

            field.Resize(OsbEasing.InOutCubic, 41866, 43414, width, 500f / 1200f * 2000f);
            field.moveFieldY(OsbEasing.InOutCubic, 41866, 43414, -350);
            field.moveFieldX(OsbEasing.InOutCubic, 41866, 43414, -80);
            field.ScaleOrigin(OsbEasing.InCubic, 41866, 43414, new Vector2(0.5f), ColumnType.all);

            DrawInstance draw = new DrawInstance(CancellationToken, field, 17866, scrollSpeed, updatesPerSecond, OsbEasing.None, false, fadeTime, fadeTime);
            draw.setReceptorMovementPrecision(0.25f);
            draw.setNoteMovementPrecision(0.25f);
            draw.customScale = true;
            draw.drawViaEquation(endtime - 17866, NoteFunction, true);
        }

        // NoteFunction is used to manipulate the pathway and a bunch of other things the note should do on their way to the receptor
        // Please be warry that this is beeing run async so you need to keep thread safety in mind when working on complex Functions.
        // You can use the progress to determin how far the note is in its cycle 0 = just start | 1 = ontop of receptor / finished
        // Special flags for hold bodies exist
        public Vector2 NoteFunction(EquationParameters p)
        {

            var screenCenter = p.column.OriginPositionAt(p.time);
            var targetPos = p.column.ReceptorPositionAt(p.time);

            // Base interpolation
            Vector2 basePos = Vector2.Lerp(screenCenter, targetPos, (float)OsbEasing.InQuad.Ease(p.progress));

            // Fake 3D depth factor
            float depthFactor = 1 - (float)Math.Pow(1 - p.progress, 2);

            // Time since start
            float elapsedTime = (float)p.time - 17092;

            // Period and amplitude
            float period = 8000f; // ms
            float maxAmplitude = 800f;

            // Sine wave oscillating between -600 and 600
            float fixedAmp = maxAmplitude * (float)Math.Sin(elapsedTime / period * 2f * Math.PI);


            float amplitude = Utility.SmoothAmplitudeByTime(p.time, 41866, 43414, fixedAmp, 0, fixedAmp);
            if (p.time > 43414)
            {
                amplitude = 0;
            }

            float x = (float)Utility.SineWaveValue(amplitude, 1, p.progress);

            // Smoothly interpolate x movement
            float xMultiplier = (float)Math.Sin((p.progress - 0.5f) * Math.PI);
            x *= xMultiplier;

            var multi = 1f - (float)OsbEasing.InQuad.Ease(p.progress);
            basePos.X += x * depthFactor * multi;

            // Y wave oscillation
            float yWaveFrequency = 3f; // 3 full oscillations
            float yWaveAmplitude = 33f; // 10 pixels amplitude
            float yWave = (float)Math.Sin(p.progress * yWaveFrequency * 2f * Math.PI) * yWaveAmplitude;

            // Apply y wave
            basePos.Y += yWave * depthFactor * multi;

            // 3D scale effect
            if (p.progress == 0)
            {
                lock (_lock)
                {
                    float transitionFactor = 0f; // as per your original
                    float scaleBase = .2f * transitionFactor;
                    float scaleFactor = scaleBase + (1.0f - scaleBase) * depthFactor;

                    p.note.noteSprite.ScaleVec(OsbEasing.InQuad,
                        p.note.renderStart, p.note.renderEnd,
                        new Vector2(scaleFactor), p.column.ReceptorScaleAt(p.note.renderEnd));
                }
            }

            return basePos;
        }

    }
}
