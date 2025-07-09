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
    public class Outro : StoryboardObjectGenerator
    {
        //        public override bool Multithreaded => false;
        // Generate function in a storybrew script
        public override void Generate()
        {

            var receptors = GetLayer("r");
            var notes = GetLayer("n");

            // General values
            var starttime = 264230; // the starttime where the playfield is initialized
            var endtime = 281686; // the endtime where the playfield is nolonger beeing rendered
            var duration = endtime - starttime; // the length the playfield is kept alive

            // Playfield Scale
            var width = 250f; // widht of the playfield / invert to flip
            var height = 600f; // height of the playfield / invert to flip -600 = downscroll | 600 = upscropll
            var receptorWallOffset = 50f; // how big the boundary box for the receptor is 50 means it will be pushed away 50 units from the wall

            // Note initilization Values
            var sliderAccuracy = 10; // The Segment length for sliderbodies since they are rendered in slices 30 is default
            var isColored = false; // This property is used if you want to color the notes by urself for effects. It does not swap if the snap coloring is used.

            // Drawinstance Values
            var updatesPerSecond = 250; // The amount of steps the rendring engine does to render out note and receptor positions
            var scrollSpeed = 1200f / 500f * 600f; // The speed at which the Notes scroll
            var fadeTime = 150; // The time notes will fade in

            Playfield field = new Playfield();
            field.initilizePlayField(receptors, notes, starttime, endtime, width, height, receptorWallOffset, Beatmap.OverallDifficulty);
            field.initializeNotes(Beatmap.HitObjects.ToList(), Beatmap, isColored, sliderAccuracy);

            field.MoveOriginRelative(OsbEasing.None, 271730, 276885, new Vector2(0, -100), ColumnType.all);

            float localStart = 264230;
            float dur = (279286 - 264230) / 4;
            var quarterDur = (float)dur / 4;
            float circleWidth = 5;
            while (localStart < 279286)
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
                field.moveFieldY(OsbEasing.OutSine, localStart, localStart + quarterDur, -circleWidth);
                localStart += quarterDur;

                // Column two (up)
                field.moveFieldX(OsbEasing.OutSine, localStart, localStart + quarterDur, circleWidth);
                field.moveFieldY(OsbEasing.InSine, localStart, localStart + quarterDur, circleWidth);
                localStart += quarterDur;

                // Column two (up)
                field.moveFieldX(OsbEasing.InSine, localStart, localStart + quarterDur, -circleWidth);
                field.moveFieldY(OsbEasing.OutSine, localStart, localStart + quarterDur, circleWidth);
                localStart += quarterDur;

                // Column two (up)
                field.moveFieldX(OsbEasing.OutSine, localStart, localStart + quarterDur, -circleWidth);
                field.moveFieldY(OsbEasing.InSine, localStart, localStart + quarterDur, -circleWidth);
                localStart += quarterDur;
            }

            DrawInstance draw = new DrawInstance(CancellationToken, field, 264240, scrollSpeed, updatesPerSecond, OsbEasing.None, false, fadeTime, fadeTime);
            draw.setReceptorMovementPrecision(0.1f);
            draw.setNoteMovementPrecision(0.25f);
            draw.setNoteRotationPrecision(0.01f);
            draw.setHoldRotationPrecision(0.01f);
            draw.drawViaEquation(endtime - 264240, NoteFunction, true);
        }

        // NoteFunction is used to manipulate the pathway and a bunch of other things the note should do on their way to the receptor
        // Please be warry that this is beeing run async so you need to keep thread safety in mind when working on complex Functions.
        // You can use the progress to determin how far the note is in its cycle 0 = just start | 1 = ontop of receptor / finished
        // Special flags for hold bodies exist
        public Vector2 NoteFunction(EquationParameters p)
        {
            var pos = p.position;

            var from = p.column.OriginPositionAt(p.time).X;
            var to = p.column.ReceptorPositionAt(p.time).X;

            // Remap progress from 0.3 to 1.0 range
            var remappedProgress = Math.Max(0, Math.Min(1, (p.progress - 0.2f) / 0.8f));
            var progress = OsbEasing.OutSine.Ease(remappedProgress);

            var blend = from + (to - from) * progress;

            pos.X = (float)blend;

            if (p.time >= 279286 && p.isHoldBody)
            {
                var time = p.time;
                var endtime = 281085;

                // Calculate how far we are into the effect (0 to 1)
                var effectProgress = Math.Min(1, (time - 279286) / (float)(endtime - 279286));
                var prog = p.progress;

                // Increase amplitude and frequency as we progress
                var amplitude = 0f + (effectProgress * 100f); // Amplitude grows from 50 to 150
                var frequency = 0f + (effectProgress * 8);      // Frequency grows from 2 to 10

                // Add chaotic elements as the effect progresses
                var chaosX = Math.Sin(prog * Math.PI * frequency) * amplitude;

                // Smoothly transition Y chaos using easing function
                var yFactor = Math.Max(0, Math.Min(1, (effectProgress - 0.3f) / 0.4f)); // Smooth ramp from 0.3 to 0.7
                var chaosY = Math.Cos(prog * Math.PI * frequency * 1.7) * amplitude * 0.5 * yFactor;

                // Smoothly transition noise using easing function
                var noiseFactor = Math.Max(0, Math.Min(1, (effectProgress - 0.2f) / 0.3f)); // Smooth ramp from 0.2 to 0.5
                var noiseX = Math.Sin(prog * Math.PI * 13) * amplitude * 0.3 * noiseFactor;

                // Add rotation effect that increases with progress
                var rotFactor = Math.Max(0, Math.Min(1, (effectProgress - 0.6f) / 0.4f)); // Smooth ramp from 0.6 to 1.0
                var rotation = Math.Sin(prog * Math.PI * 5) * rotFactor * 0.1 * Math.PI;

                // Apply a perlin-like noise for a more natural, chaotic movement
                var perlinX = Math.Sin(prog * 7.3) * Math.Cos(effectProgress * 11.2) * amplitude * 0.2 * Math.Max(0, effectProgress - 0.4f) / 0.6f;
                var perlinY = Math.Cos(prog * 5.7) * Math.Sin(effectProgress * 9.3) * amplitude * 0.15 * Math.Max(0, effectProgress - 0.5f) / 0.5f;

                pos.X = (float)(blend + chaosX + noiseX + perlinX);
                pos.Y += (float)(chaosY + perlinY);
            }




            return pos;
        }
    }
}
