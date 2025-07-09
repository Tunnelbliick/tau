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
    public class Trance : StoryboardObjectGenerator
    {
        //        public override bool Multithreaded => false;
        private readonly object _lock = new object();
        // Add a thread-safe dictionary to track processed sprites
        private readonly HashSet<int> processedSprites = new HashSet<int>();

        public override void Generate()
        {

            var receptors = GetLayer("r");
            var notes = GetLayer("n");

            // General values
            var starttime = 92769 - 200; // the starttime where the playfield is initialized
            var endtime = 118124; // the endtime where the playfield is nolonger beeing rendered
            var duration = endtime - starttime; // the length the playfield is kept alive

            // Playfield Scale
            var width = 250f; // widht of the playfield / invert to flip
            var height = 500; // height of the playfield / invert to flip -600 = downscroll | 600 = upscropll
            var receptorWallOffset = 40f; // how big the boundary box for the receptor is 50 means it will be pushed away 50 units from the wall

            // Note initilization Values
            var sliderAccuracy = 28; // The Segment length for sliderbodies since they are rendered in slices 30 is default
            var isColored = false; // This property is used if you want to color the notes by urself for effects. It does not swap if the snap coloring is used.

            // Drawinstance Values
            var updatesPerSecond = 250; // The amount of steps the rendring engine does to render out note and receptor positions
            var scrollSpeed = 1650; // The speed at which the Notes scroll
            var fadeTime = 0; // The time notes will fade in

            Playfield field = new Playfield();
            //field.delta = 2;
            field.initilizePlayField(receptors, notes, starttime, endtime, width, -height, receptorWallOffset, Beatmap.OverallDifficulty);
            field.initializeNotes(Beatmap.HitObjects.ToList(), Beatmap.GetTimingPointAt(starttime).Bpm, Beatmap.GetTimingPointAt(starttime).Offset, isColored, sliderAccuracy);

            Playfield field2 = new Playfield();
            //field2.delta = 2;
            field2.initilizePlayField(receptors, notes, starttime, endtime, width, height, receptorWallOffset, Beatmap.OverallDifficulty);
            field2.initializeNotes(Beatmap.HitObjects.ToList(), Beatmap.GetTimingPointAt(starttime).Bpm, Beatmap.GetTimingPointAt(starttime).Offset, isColored, sliderAccuracy);

            field.MoveOriginRelative(OsbEasing.InOutSine, 92963, 92963 + 2000, new Vector2(-200, 0), ColumnType.all);
            field2.MoveOriginRelative(OsbEasing.InOutSine, 92963, 92963 + 2000, new Vector2(200, 0), ColumnType.all);

            field.MoveReceptorRelative(OsbEasing.InOutSine, 92963, 92963 + 2000, new Vector2(200, 0), ColumnType.all);
            field2.MoveReceptorRelative(OsbEasing.InOutSine, 92963, 92963 + 2000, new Vector2(-200, 0), ColumnType.all);

            var curr = 92963 + 2000;
            var dur = 3000;


            while (curr < 114640)
            {
                field.MoveOriginRelative(OsbEasing.InOutSine, curr, curr + dur, new Vector2(400, 0), ColumnType.all);
                field2.MoveOriginRelative(OsbEasing.InOutSine, curr, curr + dur, new Vector2(-400, 0), ColumnType.all);

                field.MoveReceptorRelative(OsbEasing.InOutSine, curr, curr + dur, new Vector2(-400, 0), ColumnType.all);
                field2.MoveReceptorRelative(OsbEasing.InOutSine, curr, curr + dur, new Vector2(400, 0), ColumnType.all);

                curr += dur;

                field.MoveOriginRelative(OsbEasing.InOutSine, curr, curr + dur, new Vector2(-400, 0), ColumnType.all);
                field2.MoveOriginRelative(OsbEasing.InOutSine, curr, curr + dur, new Vector2(400, 0), ColumnType.all);

                field.MoveReceptorRelative(OsbEasing.InOutSine, curr, curr + dur, new Vector2(400, 0), ColumnType.all);
                field2.MoveReceptorRelative(OsbEasing.InOutSine, curr, curr + dur, new Vector2(-400, 0), ColumnType.all);

                curr += dur;
            }

            foreach (var col in field2.columns)
            {
                col.Value.receptor.renderedSprite.Fade(92964, 0);
            }
            field2.fadeAt(92964, 0);
            field2.fadeAt(starttime, 0);

            var fade = 0;
            var gap = 95479 - 93930;
            var currentTime = 93930;
            dur = 94317 - 93930;

            while (currentTime < 116188)
            {
                field.fadeAt(currentTime, currentTime + dur, OsbEasing.OutCubic, fade);
                if (fade == 0)
                {
                    field.Scale(OsbEasing.OutCubic, currentTime, currentTime + dur, new Vector2(0.7f));
                    field.RotatePlayFieldStatic(OsbEasing.OutCubic, currentTime, currentTime + dur * 2, Math.PI * 2);
                }
                else
                {
                    field.Scale(OsbEasing.OutCubic, currentTime, currentTime + dur, new Vector2(0.5f));
                    field.RotatePlayFieldStatic(OsbEasing.OutCubic, currentTime - dur / 2, currentTime + dur, Math.PI * 2);
                }
                foreach (var col in field.columns)
                {
                    col.Value.receptor.renderedSprite.Fade(OsbEasing.OutCubic, currentTime, currentTime + dur, 1 - fade, fade);
                }

                fade = 1 - fade;

                field2.fadeAt(currentTime, currentTime + dur, OsbEasing.OutCubic, fade);
                if (fade == 0)
                {
                    field2.Scale(OsbEasing.OutCubic, currentTime, currentTime + dur, new Vector2(0.7f));
                    field2.RotatePlayFieldStatic(OsbEasing.OutCubic, currentTime, currentTime + dur * 2, Math.PI * 2);
                }
                else
                {
                    field2.Scale(OsbEasing.OutCubic, currentTime, currentTime + dur, new Vector2(0.5f));
                    field2.RotatePlayFieldStatic(OsbEasing.OutCubic, currentTime - dur / 2, currentTime + dur, Math.PI * 2);
                }

                foreach (var col in field2.columns)
                {
                    col.Value.receptor.renderedSprite.Fade(OsbEasing.OutSine, currentTime, currentTime + dur, 1 - fade, fade);
                }

                currentTime += gap;
            }

            field2.columns[ColumnType.one].receptor.renderedSprite.ScaleVec(OsbEasing.OutCubic, 117737, 118124, new Vector2(0.5f), new Vector2(0.5f, 0f));
            field2.columns[ColumnType.two].receptor.renderedSprite.ScaleVec(OsbEasing.OutCubic, 117737, 118124, new Vector2(0.5f), new Vector2(0f, 0.5f));
            field2.columns[ColumnType.three].receptor.renderedSprite.ScaleVec(OsbEasing.OutCubic, 117737, 118124, new Vector2(0.5f), new Vector2(0f, 0.5f));
            field2.columns[ColumnType.four].receptor.renderedSprite.ScaleVec(OsbEasing.OutCubic, 117737, 118124, new Vector2(0.5f), new Vector2(0.5f, 0f));

            field2.MoveColumnRelativeX(OsbEasing.OutCubic, 117737, 118124, -field.getColumnWidth(), ColumnType.two);
            field2.MoveColumnRelativeX(OsbEasing.OutCubic, 117737, 118124, -field.getColumnWidth() * 2, ColumnType.three);
            field2.MoveColumnRelativeX(OsbEasing.OutCubic, 117737, 118124, -field.getColumnWidth() * 3, ColumnType.four);

            DrawInstance draw = new DrawInstance(CancellationToken, field, 92898, scrollSpeed, updatesPerSecond, OsbEasing.None, false, fadeTime, fadeTime);
            draw.setReceptorMovementPrecision(0.5f);
            draw.customScale = true;
            draw.drawViaEquation(endtime - 92898, NoteFunction, true);

            DrawInstance draw2 = new DrawInstance(CancellationToken, field2, 92963, scrollSpeed, updatesPerSecond, OsbEasing.None, false, fadeTime, fadeTime);
            draw2.setReceptorMovementPrecision(0.5f);
            draw2.customScale = true;
            draw2.drawViaEquation(endtime - 92963, NoteFunction, true);
        }

        // NoteFunction is used to manipulate the pathway and a bunch of other things the note should do on their way to the receptor
        // Please be warry that this is beeing run async so you need to keep thread safety in mind when working on complex Functions.
        // You can use the progress to determin how far the note is in its cycle 0 = just start | 1 = ontop of receptor / finished
        // Special flags for hold bodies exist
        public Vector2 NoteFunction(EquationParameters p)
        {
            var pos = p.position; // The current position of the note

            // Replace the existing progress check with a threshold-based approach
            float squashThreshold = 0.5f; // Exact progress threshold
            float squashTolerance = 0.001f; // Small tolerance value to avoid floating-point precision issues

            if (Math.Abs(p.progress - squashThreshold) <= squashTolerance)
            {
                var scale = new Vector2(0.5f);
                var adjustedScale = new Vector2(scale.X, scale.Y);
                switch (p.column.type)
                {
                    case ColumnType.one:
                        adjustedScale.X = 0;
                        break;
                    case ColumnType.two:
                        adjustedScale.Y = 0;
                        break;
                    case ColumnType.three:
                        adjustedScale.Y = 0;
                        break;
                    case ColumnType.four:
                        adjustedScale.X = 0;
                        break;
                }

                p.note.noteSprite.ScaleVec(OsbEasing.InSine, p.time, p.time + 100, scale, adjustedScale);
                p.note.noteSprite.ScaleVec(OsbEasing.OutSine, p.time + 100, p.time + 200, adjustedScale, scale);
            }

            if (p.progress < 0.5f)
            {
                pos.X = p.column.OriginPositionAt(p.time).X;
            }
            else if (p.progress >= 0.5f && p.progress <= 0.6f)
            {
                // Smoothly lerp between originPos and normal position
                float lerpFactor = (p.progress - 0.5f) / (0.6f - 0.5f); // Normalize progress to [0, 1]
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

            /*
            if (p.progress >= 0.6f && p.time > 93350)
            {
                lock (_lock)
                {
                    int spriteId = p.sprite.GetHashCode();
                    if (!processedSprites.Contains(spriteId))
                    {
                        // Mark the sprite as processed
                        processedSprites.Add(spriteId);

                        // Check if the receptor's opacity is not zero
                        /*if (p.column.receptor.renderedSprite.OpacityAt(p.time - 250) > 0 && p.column.receptor.renderedSprite.OpacityAt(p.time + 250) > 0)
                        {
                            // Fade out the sprite and fade it back in
                            p.sprite.Fade(OsbEasing.InSine, p.time - 250, p.time, p.sprite.OpacityAt(p.time), 0);
                        }
                        p.sprite.Fade(OsbEasing.InSine, p.time, p.time + 250, 0, p.column.receptor.renderedSprite.OpacityAt(p.time + 250));
                        // Fade out the sprite and fade it back in
                    }
                }
            }*/

            // Calculate the offset using a sine wave
            var offset = (float)Utility.SineWaveValue(Utility.SmoothAmplitudeByTime(p.time, 92866, 93737, 0, 65, 65), 1, p.progress);

            // Adjust the X position based on progress
            pos.X += offset;

            return pos;
        }
    }
}
