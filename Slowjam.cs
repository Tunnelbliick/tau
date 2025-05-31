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
    public class Slowjam : StoryboardObjectGenerator
    {
//        public override bool Multithreaded => false;
        Playfield field = new Playfield();

        Playfield field2 = new Playfield();
        public override void Generate()
        {

            var receptors = GetLayer("r");
            var notes = GetLayer("n");

            // General values
            var starttime = 205801; // the starttime where the playfield is initialized
            var endtime = 219834; // the endtime where the playfield is nolonger beeing rendered
            var duration = endtime - starttime; // the length the playfield is kept alive

            // Playfield Scale
            var width = 250f; // widht of the playfield / invert to flip
            var height = 550f; // height of the playfield / invert to flip -600 = downscroll | 600 = upscropll
            var receptorWallOffset = 50f; // how big the boundary box for the receptor is 50 means it will be pushed away 50 units from the wall

            // Note initilization Values
            var sliderAccuracy = 30; // The Segment length for sliderbodies since they are rendered in slices 30 is default
            var isColored = false; // This property is used if you want to color the notes by urself for effects. It does not swap if the snap coloring is used.

            // Drawinstance Values
            var updatesPerSecond = 250; // The amount of steps the rendring engine does to render out note and receptor positions
            var scrollSpeed = 1200f / 500f * 550f; // The speed at which the Notes scroll
            var fadeTime = 150; // The time notes will fade in

            field.initilizePlayField(receptors, notes, starttime, endtime, width, height, receptorWallOffset, Beatmap.OverallDifficulty);
            field.noteStart = 206188;
            field.noteEnd = 221865;
            field.initializeNotes(Beatmap.HitObjects.ToList(), Beatmap, isColored, sliderAccuracy);

            field2.initilizePlayField(receptors, notes, starttime, endtime, width, height, receptorWallOffset, Beatmap.OverallDifficulty);
            field2.noteStart = 206188;
            field2.noteEnd = 221865;
            field2.initializeNotes(Beatmap.HitObjects.ToList(), Beatmap, isColored, sliderAccuracy);

            field.moveFieldX(OsbEasing.None, starttime + 10, starttime + 10, -175);
            field2.moveFieldX(OsbEasing.None, starttime + 10, starttime + 10, 175);

            foreach (var column in field.columns.Values)
            {

                field.columns[column.type].receptor.renderedSprite.Fade(205995, 0);
                field2.columns[column.type].receptor.renderedSprite.Fade(205995, 0);

                field.columns[column.type].receptor.renderedSprite.Fade(206188, 1);
                field2.columns[column.type].receptor.renderedSprite.Fade(206188, 1);

                field.columns[column.type].receptor.renderedSprite.Fade(206285, 0);
                field2.columns[column.type].receptor.renderedSprite.Fade(206285, 0);

                field.columns[column.type].receptor.renderedSprite.Fade(OsbEasing.OutSine, 206382, 206382 + 750, 1, 0);
                field2.columns[column.type].receptor.renderedSprite.Fade(OsbEasing.OutSine, 206382, 206382 + 750, 1, 0);

                field2.columns[column.type].receptor.renderedSprite.Fade(OsbEasing.InCubic, 207543 - 400, 207543, 0, 1);

            }

            field.fadeAt(205995, 0);
            field2.fadeAt(205995, 0);

            field2.fadeAt(207543 - 400, 207543, OsbEasing.InCubic, 1);

            var fadeStart = 219543;
            var fadeEnd = 219834;
            var interval = 219608 - fadeStart;
            var currentFade = 0;

            while (fadeStart < fadeEnd)
            {
                foreach (var column in field.columns.Values)
                {
                    field.columns[column.type].receptor.renderedSprite.Fade(fadeStart, currentFade);
                    field2.columns[column.type].receptor.renderedSprite.Fade(fadeStart, currentFade);
                }

                field.fadeAt(fadeStart, currentFade);
                field2.fadeAt(fadeStart, currentFade);

                currentFade = Math.Abs(currentFade - 1);
                fadeStart += interval;
            }

            float localtime = 207156;
            float localend = 219543;
            var dur = (localend - localtime) / 4;
            float radiusX = 200;
            float radiusY = 80;
            var quarterDuration = dur / 4f;
            var halfDuration = dur / 2f;

            var dif = 210640 - localtime;

            field.moveFieldY(OsbEasing.None, localtime - 60, localtime - 60, 100);
            field2.moveFieldY(OsbEasing.None, localtime - 60, localtime - 60, 100);

            field.moveFieldX(OsbEasing.None, localtime - 30, localtime - 30, 25);
            field2.moveFieldX(OsbEasing.None, localtime - 30, localtime - 30, 25);

            field.moveFieldX(OsbEasing.InOutSine, 210640, 212188, -25);
            field2.moveFieldX(OsbEasing.InOutSine, 210640, 212188, -25);

            field.moveFieldX(OsbEasing.InOutSine, 210640 + dif, 212188 + dif, 25);
            field2.moveFieldX(OsbEasing.InOutSine, 210640 + dif, 212188 + dif, 25);

            field.moveFieldX(OsbEasing.InOutSine, 210640 + dif + dif, 212188 + dif + dif, -25);
            field2.moveFieldX(OsbEasing.InOutSine, 210640 + dif + dif, 212188 + dif + dif, -25);

            field.Rotate(OsbEasing.None, localtime - 50, localtime - 40, MathHelper.DegreesToRadians(30), CenterType.receptor);
            field2.Rotate(OsbEasing.None, localtime - 50, localtime - 40, MathHelper.DegreesToRadians(30), CenterType.receptor);

            field.Rotate(OsbEasing.InOutSine, 210640, 212188, -MathHelper.DegreesToRadians(44), CenterType.receptor);
            field2.Rotate(OsbEasing.InOutSine, 210640, 212188, -MathHelper.DegreesToRadians(76), CenterType.receptor);

            field.Rotate(OsbEasing.InOutSine, 210640 + dif, 212188 + dif, MathHelper.DegreesToRadians(40), CenterType.receptor);
            field2.Rotate(OsbEasing.InOutSine, 210640 + dif, 212188 + dif, MathHelper.DegreesToRadians(88), CenterType.receptor);

            field.Rotate(OsbEasing.InOutSine, 210640 + dif + dif, 212188 + dif + dif, -MathHelper.DegreesToRadians(44), CenterType.receptor);
            field2.Rotate(OsbEasing.InOutSine, 210640 + dif + dif, 212188 + dif + dif, -MathHelper.DegreesToRadians(60), CenterType.receptor);

            field.RotatePlayFieldStatic(OsbEasing.None, localtime - 50, localtime - 40, MathHelper.DegreesToRadians(30));
            field2.RotatePlayFieldStatic(OsbEasing.None, localtime - 50, localtime - 40, MathHelper.DegreesToRadians(30));

            field.RotatePlayFieldStatic(OsbEasing.InOutSine, 210640, 212188, -MathHelper.DegreesToRadians(60));
            field2.RotatePlayFieldStatic(OsbEasing.InOutSine, 210640, 212188, -MathHelper.DegreesToRadians(60));

            field.RotatePlayFieldStatic(OsbEasing.InOutSine, 210640 + dif, 212188 + dif, MathHelper.DegreesToRadians(60));
            field2.RotatePlayFieldStatic(OsbEasing.InOutSine, 210640 + dif, 212188 + dif, MathHelper.DegreesToRadians(60));

            field.RotatePlayFieldStatic(OsbEasing.InOutSine, 210640 + dif + dif, 212188 + dif + dif, -MathHelper.DegreesToRadians(60));
            field2.RotatePlayFieldStatic(OsbEasing.InOutSine, 210640 + dif + dif, 212188 + dif + dif, -MathHelper.DegreesToRadians(60));


            field.Rotate(OsbEasing.None, 219543, 219592, MathHelper.DegreesToRadians(14), CenterType.receptor);
            field.RotatePlayFieldStatic(OsbEasing.None, 219543, 219592, MathHelper.DegreesToRadians(10));
            field.moveFieldX(OsbEasing.None, 219543, 219543, 118);
            field.moveFieldY(OsbEasing.None, 219543, 219543, -15);

            field.Rotate(OsbEasing.None, 219688, 219737, MathHelper.DegreesToRadians(14), CenterType.receptor);
            field.RotatePlayFieldStatic(OsbEasing.None, 219688, 219737, MathHelper.DegreesToRadians(10));
            field.moveFieldX(OsbEasing.None, 219688, 219688, 118);
            field.moveFieldY(OsbEasing.None, 219688, 219688, -15);

            field.Rotate(OsbEasing.None, 219809, 219834, MathHelper.DegreesToRadians(16), CenterType.receptor);
            field.RotatePlayFieldStatic(OsbEasing.None, 219809, 219834, MathHelper.DegreesToRadians(10));
            field.moveFieldX(OsbEasing.None, 219809, 219809, 115.75f);
            field.moveFieldY(OsbEasing.None, 219809, 219809, -15.25f);

            field2.Rotate(OsbEasing.None, 219543, 219592, MathHelper.DegreesToRadians(4), CenterType.receptor);
            field2.RotatePlayFieldStatic(OsbEasing.None, 219543, 219592, MathHelper.DegreesToRadians(10));
            field2.moveFieldX(OsbEasing.None, 219543, 219543, -78);
            field2.moveFieldY(OsbEasing.None, 219543, 219543, -3);

            field2.Rotate(OsbEasing.None, 219688, 219737, MathHelper.DegreesToRadians(4), CenterType.receptor);
            field2.RotatePlayFieldStatic(OsbEasing.None, 219688, 219737, MathHelper.DegreesToRadians(10));
            field2.moveFieldX(OsbEasing.None, 219688, 219688, -78);
            field2.moveFieldY(OsbEasing.None, 219688, 219688, -2);

            field2.Rotate(OsbEasing.None, 219809, 219834, MathHelper.DegreesToRadians(5.8f), CenterType.receptor);
            field2.RotatePlayFieldStatic(OsbEasing.None, 219809, 219834, MathHelper.DegreesToRadians(10));
            field2.moveFieldX(OsbEasing.None, 219809, 219809, -78.25f);
            field2.moveFieldY(OsbEasing.None, 219809, 219809, -2.25f);


            field.Resize(OsbEasing.None, 219858, 219858, width - 9, height);
            field2.Resize(OsbEasing.None, 219858, 219858, width - 19, height);

            /*addSpin(207930);
            addSpin(208705);
            addSpin(209479);
            addSpin(210059);
            addSpin(210446);
            addSpin(211027);
            addSpin(211801);
            addSpin(212575);
            addSpin(213156);
            addSpin(213543);
            addSpin(214124);
            addSpin(214898);
            addSpin(215672);
            addSpin(216253);
            addSpin(216640);
            addSpin(217221);
            addSpin(217995);
            addSpin(218769);*/

            while (localtime < localend)
            {

                field.moveFieldX(OsbEasing.InSine, localtime, localtime + quarterDuration, radiusX);
                field.moveFieldY(OsbEasing.OutSine, localtime, localtime + quarterDuration, -radiusY);

                field2.moveFieldX(OsbEasing.InSine, localtime, localtime + quarterDuration, -radiusX);
                field2.moveFieldY(OsbEasing.OutSine, localtime, localtime + quarterDuration, radiusY);

                localtime += quarterDuration;

                foreach (var column in field.columns.Values)
                {
                    field.columns[column.type].receptor.renderedSprite.Fade(OsbEasing.InOutSine, localtime, localtime + halfDuration, 0, 1);
                    field2.columns[column.type].receptor.renderedSprite.Fade(OsbEasing.InOutSine, localtime, localtime + halfDuration, 1, 0);
                }

                field.fadeAt(localtime, localtime + halfDuration, OsbEasing.InOutSine, 1f);
                field.Scale(OsbEasing.InOutSine, localtime, localtime + halfDuration, new Vector2(0.75f));
                field2.fadeAt(localtime, localtime + halfDuration, OsbEasing.InOutSine, .0f);
                field2.Scale(OsbEasing.InOutSine, localtime, localtime + halfDuration, new Vector2(0.25f));

                field.moveFieldX(OsbEasing.OutSine, localtime, localtime + quarterDuration, radiusX);
                field.moveFieldY(OsbEasing.InSine, localtime, localtime + quarterDuration, radiusY);

                field2.moveFieldX(OsbEasing.OutSine, localtime, localtime + quarterDuration, -radiusX);
                field2.moveFieldY(OsbEasing.InSine, localtime, localtime + quarterDuration, -radiusY);

                localtime += quarterDuration;


                field.moveFieldX(OsbEasing.InSine, localtime, localtime + quarterDuration, -radiusX);
                field.moveFieldY(OsbEasing.OutSine, localtime, localtime + quarterDuration, radiusY);

                field2.moveFieldX(OsbEasing.InSine, localtime, localtime + quarterDuration, radiusX);
                field2.moveFieldY(OsbEasing.OutSine, localtime, localtime + quarterDuration, -radiusY);

                localtime += quarterDuration;

                if (localtime + halfDuration < localend)
                {

                    foreach (var column in field.columns.Values)
                    {
                        field.columns[column.type].receptor.renderedSprite.Fade(OsbEasing.InOutSine, localtime, localtime + halfDuration, 1, 0);
                        field2.columns[column.type].receptor.renderedSprite.Fade(OsbEasing.InOutSine, localtime, localtime + halfDuration, 0, 1);
                    }

                    field.fadeAt(localtime, localtime + halfDuration, OsbEasing.InOutSine, .01f);
                    field.Scale(OsbEasing.InOutSine, localtime, localtime + halfDuration, new Vector2(0.35f));
                    field2.fadeAt(localtime, localtime + halfDuration, OsbEasing.InOutSine, 1f);
                    field2.Scale(OsbEasing.InOutSine, localtime, localtime + halfDuration, new Vector2(0.75f));
                }
                else
                {
                    field.Scale(OsbEasing.InOutSine, localtime, 219858, new Vector2(.5f));
                    field2.Scale(OsbEasing.InOutSine, localtime, 219858, new Vector2(.5f));
                }

                field.moveFieldX(OsbEasing.OutSine, localtime, localtime + quarterDuration, -radiusX);
                field.moveFieldY(OsbEasing.InSine, localtime, localtime + quarterDuration, -radiusY);

                field2.moveFieldX(OsbEasing.OutSine, localtime, localtime + quarterDuration, radiusX);
                field2.moveFieldY(OsbEasing.InSine, localtime, localtime + quarterDuration, radiusY);

                localtime += quarterDuration;

            }

            FakeNoteFadeIn(ColumnType.three, 207543);
            FakeNoteFadeIn(ColumnType.four, 207543);
            FakeNoteFadeIn(ColumnType.two, 207737, 2);
            FakeNoteFadeIn(ColumnType.one, 207737, 2);
            FakeNoteFadeIn(ColumnType.four, 207930);
            FakeNoteFadeIn(ColumnType.three, 208027, 4);
            FakeNoteFadeIn(ColumnType.two, 208124, 2);
            FakeNoteFadeIn(ColumnType.one, 208221, 4);
            FakeNoteFadeIn(ColumnType.four, 208317);
            FakeNoteFadeIn(ColumnType.three, 208414, 4);
            FakeNoteFadeIn(ColumnType.two, 208511, 2);
            FakeNoteFadeIn(ColumnType.one, 208608, 4);

            DrawInstance draw = new DrawInstance(CancellationToken, field, 205995, scrollSpeed, updatesPerSecond, OsbEasing.None, false, fadeTime, fadeTime);
            draw.setReceptorMovementPrecision(0.25f);
            draw.setNoteRotationPrecision(0.1f);
            draw.drawViaEquation(219834 - 205995, NoteFunction, true);

            DrawInstance draw2 = new DrawInstance(CancellationToken, field2, 205995, scrollSpeed, updatesPerSecond, OsbEasing.None, false, fadeTime, fadeTime);
            draw2.setReceptorMovementPrecision(0.25f);
            draw2.setNoteRotationPrecision(0.1f);
            draw2.drawViaEquation(219834 - 205995, NoteFunction, true);
        }

        public void FakeNoteFadeIn(ColumnType type, double hittime, double noteType = 1)
        {
            var height = 550f;
            var scrollSpeed = 1200f / 500f * 550f;

            var fadeInTime = 205995;
            var endtime = 207156;

            var start = hittime - scrollSpeed;
            var currentTime = 207446 - start;
            var pos = field.columns[type].OriginPositionAt(206542);
            var pos2 = field2.columns[type].OriginPositionAt(206542);

            var travelAmount = currentTime / scrollSpeed * height;
            OsbSprite fakeNote = GetLayer("n").CreateSprite($"sb/sprites/{noteType}.png", OsbOrigin.Centre, pos - new Vector2(0, (float)travelAmount));
            OsbSprite fakeNote2 = GetLayer("n").CreateSprite($"sb/sprites/{noteType}.png", OsbOrigin.Centre, pos2 - new Vector2(0, (float)travelAmount));

            fakeNote.Scale(fadeInTime, 0.5f);
            fakeNote2.Scale(fadeInTime, 0.5f);

            fakeNote.Fade(fadeInTime, 0);
            fakeNote2.Fade(fadeInTime, 0);

            fakeNote.Fade(206188, 1);
            fakeNote2.Fade(206188, 1);

            fakeNote.Fade(206285, 0);
            fakeNote2.Fade(206285, 0);

            fakeNote.Fade(OsbEasing.OutSine, 206382, 206382 + 750, 1, 0);
            fakeNote2.Fade(OsbEasing.OutSine, 206382, 206382 + 750, 1, 0);


            switch (type)
            {
                case ColumnType.one:
                    fakeNote.Rotate(fadeInTime, 1 * Math.PI / 2);
                    fakeNote2.Rotate(fadeInTime, 1 * Math.PI / 2);
                    break;
                case ColumnType.two:
                    fakeNote.Rotate(fadeInTime, 0 * Math.PI / 2);
                    fakeNote2.Rotate(fadeInTime, 0 * Math.PI / 2);
                    break;
                case ColumnType.three:
                    fakeNote.Rotate(fadeInTime, 2 * Math.PI / 2);
                    fakeNote2.Rotate(fadeInTime, 2 * Math.PI / 2);
                    break;
                case ColumnType.four:
                    fakeNote.Rotate(fadeInTime, 3 * Math.PI / 2);
                    fakeNote2.Rotate(fadeInTime, 3 * Math.PI / 2);
                    break;
            }

        }

        public void addSpin(double starttime)
        {
            field2.RotatePlayFieldStatic(OsbEasing.None, starttime, starttime, Math.PI);
            field2.RotatePlayFieldStatic(OsbEasing.OutCirc, starttime, starttime + 300, Math.PI);

            field.RotatePlayFieldStatic(OsbEasing.None, starttime, starttime, Math.PI);
            field.RotatePlayFieldStatic(OsbEasing.OutCirc, starttime, starttime + 300, Math.PI);
        }

        public Vector2 NoteFunction(EquationParameters p)
        {
            if (p.time > 207156 && p.progress == 0)
            {
                p.sprite.Additive(207156);
            }

            return p.position;
        }
    }
}
