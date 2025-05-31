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

    public class Transition : StoryboardObjectGenerator
    {
        //        public override bool Multithreaded => false;
        private readonly object _lock = new object();

        Playfield field = new Playfield();
        public override void Generate()
        {

            var receptors = GetLayer("r");
            var notes = GetLayer("n");

            // General values
            var starttime = 157027; // the starttime where the playfield is initialized
            var endtime = 182673; // the endtime where the playfield is nolonger beeing rendered
            var duration = endtime - starttime; // the length the playfield is kept alive

            // Playfield Scale
            var width = 250f; // widht of the playfield / invert to flip
            var height = 500f; // height of the playfield / invert to flip -600 = downscroll | 600 = upscropll
            var receptorWallOffset = 50f; // how big the boundary box for the receptor is 50 means it will be pushed away 50 units from the wall

            // Note initilization Values
            var sliderAccuracy = 30; // The Segment length for sliderbodies since they are rendered in slices 30 is default
            var isColored = false; // This property is used if you want to color the notes by urself for effects. It does not swap if the snap coloring is used.

            // Drawinstance Values
            var updatesPerSecond = 1000; // The amount of steps the rendring engine does to render out note and receptor positions
            var scrollSpeed = 1200f; // The speed at which the Notes scroll
            var fadeTime = 150; // The time notes will fade in

            field.delta = 1f;
            field.initilizePlayField(receptors, notes, starttime, endtime, width, height, receptorWallOffset, Beatmap.OverallDifficulty);
            field.noteEnd = 181404;
            field.noteStart = 157317;
            field.initializeNotes(Beatmap.HitObjects.ToList(), Beatmap.GetTimingPointAt(starttime).Bpm, Beatmap.GetTimingPointAt(starttime).Offset, isColored, sliderAccuracy);

            field.fadeAt(168059, 0);
            field.fadeAt(169995 - 50, 1);
            field.MoveOriginRelative(OsbEasing.None, 168059, 168059, new Vector2(0, -height), ColumnType.all);
            field.MoveOriginRelative(OsbEasing.OutCirc, 169995, 170382, new Vector2(0, height), ColumnType.all);

            field.RotatePlayFieldStatic(OsbEasing.None, 169995, 169995, Math.PI);
            field.RotatePlayFieldStatic(OsbEasing.OutCubic, 169995, 170382, Math.PI);

            FakeNote(ColumnType.three, 168446);
            FakeNote(ColumnType.four, 168446);

            FakeNote(ColumnType.one, 168834);
            FakeNote(ColumnType.three, 168834);

            FakeNote(ColumnType.two, 169221);
            FakeNote(ColumnType.four, 169221);

            FakeNoteFadeIn(ColumnType.four, 176705, 176446, 3);
            FakeNoteFadeIn(ColumnType.three, 176834, 176446, 3);
            FakeNoteFadeIn(ColumnType.two, 176963, 176446);
            FakeNoteFadeIn(ColumnType.one, 176963, 176446);

            FakeNoteFadeIn(ColumnType.three, 177092, 176317, 3);
            FakeNoteFadeIn(ColumnType.two, 177221, 176317, 3);
            FakeNoteFadeIn(ColumnType.three, 177350, 176317);
            FakeNoteFadeIn(ColumnType.four, 177350, 176317);

            FakeNoteFadeIn(ColumnType.two, 177479, 176188, 3);
            FakeNoteFadeIn(ColumnType.one, 177608, 176188, 3);
            FakeNoteFadeIn(ColumnType.three, 177737, 176317);
            FakeNoteFadeIn(ColumnType.four, 177737, 176317);
            FakeNoteFadeIn(ColumnType.two, 177866, 176188, 3);
            FakeNoteFadeIn(ColumnType.three, 177995, 176188, 3);

            FakeNoteFadeOut(ColumnType.two, 176188, 176446);
            FakeNoteFadeOut(ColumnType.two, 176285, 176317, 4);
            FakeNoteFadeOut(ColumnType.two, 176382, 176188, 2);
            FakeNoteFadeOut(ColumnType.two, 176479, 176188, 4);

            FakeNoteFadeOut(ColumnType.three, 176575, 176188);
            FakeNoteFadeOut(ColumnType.four, 176575, 176188);

            FakeNoteFadeOut(ColumnType.one, 176963, 176188);
            FakeNoteFadeOut(ColumnType.two, 176963, 176188);

            field.ScaleReceptor(OsbEasing.InOutSine, 179672, 181221, new Vector2(0.25f), ColumnType.all);
            field.ScaleOrigin(OsbEasing.InOutSine, 179672, 181221, new Vector2(1f), ColumnType.all);
            var colWidth = field.getColumnWidth() * 3;
            var halfwidth = field.getColumnWidth();

            field.MoveReceptorRelative(OsbEasing.None, 179672, 181221, new Vector2(colWidth / 4, 0), ColumnType.one);
            field.MoveReceptorRelative(OsbEasing.OutCubic, 181221, 181995, new Vector2(-colWidth / 4, 0), ColumnType.one);
            field.MoveReceptorRelative(OsbEasing.None, 179672, 181221, new Vector2(halfwidth / 4, 0), ColumnType.two);
            field.MoveReceptorRelative(OsbEasing.OutCubic, 181221, 181995, new Vector2(-halfwidth / 4, 0), ColumnType.two);
            field.MoveReceptorRelative(OsbEasing.None, 179672, 181221, new Vector2(-halfwidth / 4, 0), ColumnType.three);
            field.MoveReceptorRelative(OsbEasing.OutCubic, 181221, 181995, new Vector2(halfwidth / 4, 0), ColumnType.three);
            field.MoveReceptorRelative(OsbEasing.None, 179672, 181221, new Vector2(-colWidth / 4, 0), ColumnType.four);
            field.MoveReceptorRelative(OsbEasing.OutCubic, 181221, 181995, new Vector2(colWidth / 4, 0), ColumnType.four);

            field.MoveOriginRelative(OsbEasing.None, 179672, 181221, new Vector2(-colWidth, 0), ColumnType.one);
            field.MoveOriginRelative(OsbEasing.OutCubic, 181221, 181995, new Vector2(colWidth, 0), ColumnType.one);
            field.MoveOriginRelative(OsbEasing.None, 179672, 181221, new Vector2(-halfwidth, 0), ColumnType.two);
            field.MoveOriginRelative(OsbEasing.OutCubic, 181221, 181995, new Vector2(halfwidth, 0), ColumnType.two);
            field.MoveOriginRelative(OsbEasing.None, 179672, 181221, new Vector2(halfwidth, 0), ColumnType.three);
            field.MoveOriginRelative(OsbEasing.OutCubic, 181221, 181995, new Vector2(-halfwidth, 0), ColumnType.three);
            field.MoveOriginRelative(OsbEasing.None, 179672, 181221, new Vector2(colWidth, 0), ColumnType.four);
            field.MoveOriginRelative(OsbEasing.OutCubic, 181221, 181995, new Vector2(-colWidth, 0), ColumnType.four);

            field.ScaleReceptor(OsbEasing.OutCubic, 181221, 181995, new Vector2(0.45f), ColumnType.all);
            field.RotatePlayFieldStatic(OsbEasing.OutCubic, 181221, 181995, Math.PI);
            field.Resize(OsbEasing.OutCubic, 181221, 181995, 0, 0);

            field.moveFieldY(OsbEasing.OutCirc, 181221, 181995, 240 - receptorWallOffset);

            field.columns[ColumnType.one].receptor.renderedSprite.Fade(OsbEasing.OutCubic, 181221, 181995, 1, .8f);
            field.columns[ColumnType.two].receptor.renderedSprite.Fade(OsbEasing.OutCubic, 181221, 181995, 1, .8f);
            field.columns[ColumnType.three].receptor.renderedSprite.Fade(OsbEasing.OutCubic, 181221, 181995, 1, .8f);
            field.columns[ColumnType.four].receptor.renderedSprite.Fade(OsbEasing.OutCubic, 181221, 181995, 1, .8f);

            DrawInstance draw = new DrawInstance(CancellationToken, field, starttime, scrollSpeed, updatesPerSecond, OsbEasing.None, false, fadeTime, fadeTime);
            draw.setReceptorMovementPrecision(0.25f);
            draw.setNoteRotationPrecision(0.01f);
            draw.setNoteMovementPrecision(0.1f);
            draw.setHoldRotationPrecision(0.01f);
            draw.drawViaEquation(duration, NoteFunction, true);
        }

        public void FakeNote(ColumnType type, double hittime, double noteType = 1)
        {
            var height = 500f;
            var glitchtime = 168446;
            var glitchInterval = (int)Beatmap.GetTimingPointAt(glitchtime).BeatDuration / 2;
            var glitchAmount = 40;
            var scrollSpeed = 1200f;
            var endtime = 169995;

            var start = hittime - scrollSpeed;
            var pos = field.columns[type].OriginPositionAt(start);
            var travelAmount = (glitchtime - start) / 1200f * height;
            OsbSprite fakeNote = GetLayer("n").CreateSprite($"sb/sprites/{noteType}.png", OsbOrigin.Centre, pos);
            OsbSprite fakeNoteWhite = GetLayer("n").CreateSprite($"sb/sprites/arrow.png", OsbOrigin.Centre, pos);
            fakeNote.Fade(start, 1);
            fakeNote.Fade(endtime, 0);
            fakeNote.Scale(hittime - scrollSpeed, 0.5f);
            fakeNote.MoveY(OsbEasing.None, start, glitchtime, pos.Y, pos.Y - travelAmount);


            switch (type)
            {
                case ColumnType.one:
                    fakeNote.Rotate(start, 1 * Math.PI / 2);
                    fakeNoteWhite.Rotate(start, 1 * Math.PI / 2);
                    break;
                case ColumnType.two:
                    fakeNote.Rotate(start, 0 * Math.PI / 2);
                    fakeNoteWhite.Rotate(start, 0 * Math.PI / 2);
                    break;
                case ColumnType.three:
                    fakeNote.Rotate(start, 2 * Math.PI / 2);
                    fakeNoteWhite.Rotate(start, 2 * Math.PI / 2);
                    break;
                case ColumnType.four:
                    fakeNote.Rotate(start, 3 * Math.PI / 2);
                    fakeNoteWhite.Rotate(start, 3 * Math.PI / 2);
                    break;
            }

            fakeNoteWhite.StartLoopGroup(glitchtime, (endtime - glitchtime) / glitchInterval);
            fakeNoteWhite.MoveY(OsbEasing.None, 0, glitchInterval, pos.Y - travelAmount + glitchAmount, pos.Y - travelAmount);
            fakeNoteWhite.EndGroup();

            fakeNoteWhite.Scale(start, 0.5f);
            fakeNoteWhite.Additive(start);
            fakeNoteWhite.Fade(start, 0);
            fakeNoteWhite.Fade(168446, 0.5f);
            fakeNoteWhite.Fade(endtime, 0);

            fakeNote.StartLoopGroup(glitchtime, (endtime - glitchtime) / glitchInterval);
            fakeNote.MoveY(OsbEasing.None, 0, glitchInterval, pos.Y - travelAmount + glitchAmount, pos.Y - travelAmount);
            fakeNote.EndGroup();
        }

        public void FakeNoteFadeOut(ColumnType type, double hittime, double fadeOutTime, double noteType = 1)
        {
            var height = 500f;
            var scrollSpeed = 1200f;
            var endtime = 176575;

            var start = hittime - scrollSpeed;
            var currentTime = 175898 - start;
            var pos = field.columns[type].OriginPositionAt(start);
            var travelAmount = currentTime / 1200f * height;
            var stepLength = 175898 - 175801;
            var step = height / 1200f * stepLength;
            OsbSprite fakeNote = GetLayer("n").CreateSprite($"sb/sprites/{noteType}.png", OsbOrigin.Centre, pos);
            OsbSprite fakeNoteWhite = GetLayer("n").CreateSprite($"sb/sprites/arrow.png", OsbOrigin.Centre, pos);
            fakeNote.Fade(start, 1);
            fakeNote.Fade(fadeOutTime, 0);
            fakeNote.Scale(start, 0.5f);

            if (hittime < 176575)
            {
                var localTime = 175898;
                fakeNote.MoveY(OsbEasing.None, start, localTime, pos.Y, pos.Y - travelAmount);
                fakeNoteWhite.MoveY(OsbEasing.None, start, localTime, pos.Y, pos.Y - travelAmount);
                localTime += stepLength;
                pos.Y -= (float)travelAmount;

                fakeNote.MoveY(OsbEasing.None, localTime, localTime, pos.Y, pos.Y - step);
                fakeNoteWhite.MoveY(OsbEasing.None, localTime, localTime, pos.Y, pos.Y - step);
                pos.Y -= step;
                localTime += stepLength;

                fakeNote.MoveY(OsbEasing.None, localTime, localTime, pos.Y, pos.Y - step);
                fakeNoteWhite.MoveY(OsbEasing.None, localTime, localTime, pos.Y, pos.Y - step);
                pos.Y -= step;
                localTime += stepLength;

            }
            else
            {
                currentTime = fadeOutTime - 50 - start;
                travelAmount = currentTime / 1200f * height;

                fakeNote.MoveY(OsbEasing.None, start, fadeOutTime - 50, pos.Y, pos.Y - travelAmount);
                fakeNoteWhite.MoveY(OsbEasing.None, start, fadeOutTime - 50, pos.Y, pos.Y - travelAmount);
            }


            switch (type)
            {
                case ColumnType.one:
                    fakeNote.Rotate(start, 1 * Math.PI / 2);
                    fakeNoteWhite.Rotate(fadeOutTime, 1 * Math.PI / 2);
                    break;
                case ColumnType.two:
                    fakeNote.Rotate(start, 0 * Math.PI / 2);
                    fakeNoteWhite.Rotate(fadeOutTime, 0 * Math.PI / 2);
                    break;
                case ColumnType.three:
                    fakeNote.Rotate(start, 2 * Math.PI / 2);
                    fakeNoteWhite.Rotate(fadeOutTime, 2 * Math.PI / 2);
                    break;
                case ColumnType.four:
                    fakeNote.Rotate(start, 3 * Math.PI / 2);
                    fakeNoteWhite.Rotate(fadeOutTime, 3 * Math.PI / 2);
                    break;
            }

            fakeNoteWhite.Scale(fadeOutTime, 0.5f);
            fakeNoteWhite.Additive(fadeOutTime);
            fakeNoteWhite.Fade(fadeOutTime, 0);
            fakeNoteWhite.Fade(fadeOutTime, Math.Min(fadeOutTime + 150, endtime), 1, 0);
            fakeNoteWhite.Fade(endtime, 0);
        }


        public void FakeNoteFadeIn(ColumnType type, double hittime, double fadeInTime, double noteType = 1)
        {
            var height = 500f;
            var scrollSpeed = 1200f;
            var endtime = 176575;

            var start = hittime - scrollSpeed;
            var currentTime = 176575 - start;
            var pos = field.columns[type].OriginPositionAt(start);
            var travelAmount = currentTime / 1200f * height;
            OsbSprite fakeNote = GetLayer("n").CreateSprite($"sb/sprites/{noteType}.png", OsbOrigin.Centre, pos - new Vector2(0, (float)travelAmount));
            OsbSprite fakeNoteWhite = GetLayer("n").CreateSprite($"sb/sprites/arrow.png", OsbOrigin.Centre, pos - new Vector2(0, (float)travelAmount));
            fakeNote.Fade(fadeInTime, Math.Min(fadeInTime + 250, endtime), 0, 1);
            fakeNote.Fade(endtime, 0);
            fakeNote.Scale(fadeInTime, 0.5f);


            switch (type)
            {
                case ColumnType.one:
                    fakeNote.Rotate(fadeInTime, 1 * Math.PI / 2);
                    fakeNoteWhite.Rotate(fadeInTime, 1 * Math.PI / 2);
                    break;
                case ColumnType.two:
                    fakeNote.Rotate(fadeInTime, 0 * Math.PI / 2);
                    fakeNoteWhite.Rotate(fadeInTime, 0 * Math.PI / 2);
                    break;
                case ColumnType.three:
                    fakeNote.Rotate(fadeInTime, 2 * Math.PI / 2);
                    fakeNoteWhite.Rotate(fadeInTime, 2 * Math.PI / 2);
                    break;
                case ColumnType.four:
                    fakeNote.Rotate(fadeInTime, 3 * Math.PI / 2);
                    fakeNoteWhite.Rotate(fadeInTime, 3 * Math.PI / 2);
                    break;
            }

            fakeNoteWhite.Scale(fadeInTime, 0.5f);
            fakeNoteWhite.Additive(fadeInTime);
            fakeNoteWhite.Fade(fadeInTime, Math.Min(fadeInTime + 250, endtime), 1, 0);
            fakeNoteWhite.Fade(endtime, 0);
        }

        // NoteFunction is used to manipulate the pathway and a bunch of other things the note should do on their way to the receptor
        // Please be warry that this is beeing run async so you need to keep thread safety in mind when working on complex Functions.
        // You can use the progress to determin how far the note is in its cycle 0 = just start | 1 = ontop of receptor / finished
        // Special flags for hold bodies exist
        public Vector2 NoteFunction(EquationParameters p)
        {

            if (p.time > 172801 && p.note.starttime > 172705 && p.note.starttime < 173475)
            {
                float x = p.position.X;
                float y = p.lastPosition.Y;  // Default to lastPosition.Y
                double currentTime = p.time;
                float leeway = 1.1f;  // Define a leeway of 0.1 seconds

                // Check if any hit object's startTime is within the leeway of currentTime
                if (Beatmap.HitObjects.Any(ho => Math.Abs(ho.StartTime - currentTime) <= leeway))
                {
                    y = p.position.Y;  // Update y to position.Y if within leeway
                }

                return new Vector2(x, y);
            }

            if (p.time > 175898 && p.note.starttime > 175801 && p.note.starttime < 176479)
            {
                float x = p.position.X;
                float y = p.lastPosition.Y;  // Default to lastPosition.Y
                double currentTime = p.time;
                float leeway = 1.1f;  // Define a leeway of 0.1 seconds

                // Check if any hit object's startTime is within the leeway of currentTime
                if (Beatmap.HitObjects.Any(ho => Math.Abs(ho.StartTime - currentTime) <= leeway))
                {
                    y = p.position.Y;  // Update y to position.Y if within leeway
                }

                return new Vector2(x, y);
            }

            if (p.note.starttime > 176286 && p.time < 176575)
            {
                if (p.progress == 0)
                {
                    lock (_lock)
                    {
                        p.note.noteSprite.Fade(p.note.renderStart, 0);
                        p.note.noteSprite.Fade(176575, 1);
                    }
                }


                if (p.progress < 0.2f && p.time < 176575)
                    return p.lastPosition;

            }

            return p.position;
        }
    }
}
