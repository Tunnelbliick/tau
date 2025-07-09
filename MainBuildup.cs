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
using System.Runtime.InteropServices;

namespace StorybrewScripts
{
    public class MainBuildup : StoryboardObjectGenerator
    {
        //        public override bool Multithreaded => false;
        Playfield field = new Playfield();
        public override void Generate()
        {
            var receptors = GetLayer("r");
            var notes = GetLayer("n");

            // General values
            var starttime = 119285; // the starttime where the playfield is initialized
            var endtime = 130898; // the endtime where the playfield is nolonger beeing rendered
            var duration = endtime - starttime; // the length the playfield is kept alive

            // Playfield Scale
            var width = 250f; // widht of the playfield / invert to flip
            var height = 500f; // height of the playfield / invert to flip -600 = downscroll | 600 = upscropll
            var receptorWallOffset = 50f; // how big the boundary box for the receptor is 50 means it will be pushed away 50 units from the wall

            // Note initilization Values
            var sliderAccuracy = 30; // The Segment length for sliderbodies since they are rendered in slices 30 is default
            var isColored = false; // This property is used if you want to color the notes by urself for effects. It does not swap if the snap coloring is used.

            // Drawinstance Values
            var updatesPerSecond = 100; // The amount of steps the rendring engine does to render out note and receptor positions
            var scrollSpeed = 1200f; // The speed at which the Notes scroll
            var fadeTime = 150; // The time notes will fade in

            var cleaned = Beatmap.HitObjects
                .Where(h => h.StartTime != 119285 && h.StartTime != 120834 && h.StartTime != 122382 && h.StartTime != 125479)
                .ToList();

            field.initilizePlayField(receptors, notes, starttime - 25, endtime, width, height, receptorWallOffset, Beatmap.OverallDifficulty);
            field.initializeNotes(cleaned, Beatmap, isColored, sliderAccuracy);

            field.fadeAt(119963, 120543, OsbEasing.OutSine, 0);

            foreach (var column in field.columns.Values)
            {
                column.receptor.renderedSprite.Fade(OsbEasing.OutSine, 120059, 120640, 1, 0);
            }

            field.fadeAt(120834, 1);

            foreach (var column in field.columns.Values)
            {
                column.receptor.renderedSprite.Fade(120834, 1);
            }


            field.fadeAt(121511, 121995, OsbEasing.OutSine, 0);

            foreach (var column in field.columns.Values)
            {
                column.receptor.renderedSprite.Fade(OsbEasing.OutSine, 121608, 122188, 1, 0);
            }

            field.fadeAt(122382, 1);

            foreach (var column in field.columns.Values)
            {
                column.receptor.renderedSprite.Fade(122382, 1);
            }

            /*field.Resize(OsbEasing.OutElasticQuarter, 119672, 119963, -width, height);
            field.Resize(OsbEasing.OutElasticQuarter, 119963, 120253, width, height);
            field.Resize(OsbEasing.OutElasticQuarter, 120253, 120543, -width, height);
            field.Resize(OsbEasing.OutElasticQuarter, 120446, 120446 + 250, width, height);*/

            //field.RotatePlayFieldStatic(OsbEasing.OutCubic, 119285, 120834, Math.PI * 6);

            //field.moveFieldY(OsbEasing.None, starttime, starttime, -125);

            field.moveFieldY(OsbEasing.None, 120640, 120640, 480f - receptorWallOffset * 2);
            field.Resize(OsbEasing.OutExpo, 120640, 120640, width, -height);

            /*field.Resize(OsbEasing.OutElasticQuarter, 121221, 121511, -width, -height);
            field.Resize(OsbEasing.OutElasticQuarter, 121511, 121801, width, -height);
            field.Resize(OsbEasing.OutElasticQuarter, 121801, 122092, -width, -height);
            field.Resize(OsbEasing.OutElasticQuarter, 121995, 121995 + 250, width, -height);*/

            field.moveFieldY(OsbEasing.None, 122188, 122188, -480f + receptorWallOffset * 2);
            field.Resize(OsbEasing.OutExpo, 122188, 122188, width, height);

            /*field.Resize(OsbEasing.OutElasticQuarter, 122769, 123059, -width, height);
            field.Resize(OsbEasing.OutElasticQuarter, 123059, 123350, width, height);
            field.Resize(OsbEasing.OutElasticQuarter, 123350, 123640, -width, height);
            field.Resize(OsbEasing.OutElasticQuarter, 123640, 123930, width, height);
            field.Resize(OsbEasing.OutElasticQuarter, 123930, 124221, -width, height);
            field.Resize(OsbEasing.OutElasticQuarter, 124221, 124511, width, height);
            field.Resize(OsbEasing.OutElasticQuarter, 124511, 124801, -width, height);
            field.Resize(OsbEasing.OutElasticQuarter, 124801, 125092, width, height);*/

            field.RotatePlayFieldStatic(OsbEasing.OutCubic, 125092, 125092, -Math.PI);
            field.RotatePlayFieldStatic(OsbEasing.OutCubic, 125092, 125479, -Math.PI);

            // field.moveFieldX(OsbEasing.OutSine, starttime - 10, starttime - 10, 225);
            // field.moveFieldX(OsbEasing.None, 120640, 120640, -400 - 75);

            // field.moveFieldX(OsbEasing.None, 122188, 122188, 400 + 75);

            var palyfieldCenterX = field.calculatePlayFieldCenter(123930).X;

            // field.moveFieldX(OsbEasing.InOutSine, 123930, 125479, 320 - palyfieldCenterX);

            field.moveFieldY(OsbEasing.OutCubic, 125479, 126253, 480f - receptorWallOffset * 2);

            field.ScaleReceptor(OsbEasing.InSine, 125479, 125672, new Vector2(0.25f), ColumnType.all);
            field.ScaleReceptor(OsbEasing.OutCubic, 125672, 126253, new Vector2(0.5f), ColumnType.all);
            field.ScaleOrigin(OsbEasing.InSine, 125479, 125672, new Vector2(1f), ColumnType.all);
            field.ScaleOrigin(OsbEasing.OutCubic, 125672, 126253, new Vector2(0.5f), ColumnType.all);

            field.MoveReceptorRelative(OsbEasing.InSine, 125479, 125672, new Vector2(field.getColumnWidth(), 0), ColumnType.one);
            field.MoveReceptorRelative(OsbEasing.InSine, 125479, 125672, new Vector2(field.getColumnWidth() / 2, 0), ColumnType.two);
            field.MoveReceptorRelative(OsbEasing.InSine, 125479, 125672, new Vector2(-field.getColumnWidth() / 2, 0), ColumnType.three);
            field.MoveReceptorRelative(OsbEasing.InSine, 125479, 125672, new Vector2(-field.getColumnWidth(), 0), ColumnType.four);

            field.MoveReceptorRelative(OsbEasing.OutSine, 125672, 126253, new Vector2(-field.getColumnWidth(), 0), ColumnType.one);
            field.MoveReceptorRelative(OsbEasing.OutSine, 125672, 126253, new Vector2(-field.getColumnWidth() / 2, 0), ColumnType.two);
            field.MoveReceptorRelative(OsbEasing.OutSine, 125672, 126253, new Vector2(field.getColumnWidth() / 2, 0), ColumnType.three);
            field.MoveReceptorRelative(OsbEasing.OutSine, 125672, 126253, new Vector2(field.getColumnWidth(), 0), ColumnType.four);

            field.MoveOriginRelative(OsbEasing.InSine, 125479, 125672, new Vector2(-field.getColumnWidth(), 0), ColumnType.one);
            field.MoveOriginRelative(OsbEasing.InSine, 125479, 125672, new Vector2(-field.getColumnWidth() / 2, 0), ColumnType.two);
            field.MoveOriginRelative(OsbEasing.InSine, 125479, 125672, new Vector2(field.getColumnWidth() / 2, 0), ColumnType.three);
            field.MoveOriginRelative(OsbEasing.InSine, 125479, 125672, new Vector2(field.getColumnWidth(), 0), ColumnType.four);

            field.MoveOriginRelative(OsbEasing.OutSine, 125672, 126253, new Vector2(field.getColumnWidth(), 0), ColumnType.one);
            field.MoveOriginRelative(OsbEasing.OutSine, 125672, 126253, new Vector2(field.getColumnWidth() / 2, 0), ColumnType.two);
            field.MoveOriginRelative(OsbEasing.OutSine, 125672, 126253, new Vector2(-field.getColumnWidth() / 2, 0), ColumnType.three);
            field.MoveOriginRelative(OsbEasing.OutSine, 125672, 126253, new Vector2(-field.getColumnWidth(), 0), ColumnType.four);

            field.Resize(OsbEasing.OutCubic, 125479, 126253, width, -height);

            field.MoveOriginRelative(OsbEasing.OutSine, 126253, 128575, new Vector2(field.getColumnWidth() * 2 / 2, 0), ColumnType.one);
            field.MoveOriginRelative(OsbEasing.OutSine, 126253, 128575, new Vector2(field.getColumnWidth() / 2, 0), ColumnType.two);
            field.MoveOriginRelative(OsbEasing.OutSine, 126253, 128575, new Vector2(-field.getColumnWidth() / 2, 0), ColumnType.three);
            field.MoveOriginRelative(OsbEasing.OutSine, 126253, 128575, new Vector2(-field.getColumnWidth() * 2 / 2, 0), ColumnType.four);


            field.MoveReceptorRelative(OsbEasing.InSine, 126253, 130124, new Vector2(0, -200), ColumnType.all);

            field.ScaleOrigin(OsbEasing.OutSine, 126253, 128575, new Vector2(0f), ColumnType.all);

            field.Rotate(OsbEasing.OutElasticQuarter, 123930, 123930 + 100, 0.3, CenterType.playfield);
            field.Rotate(OsbEasing.OutElasticQuarter, 124221, 124221 + 100, -0.6, CenterType.playfield);
            field.Rotate(OsbEasing.OutElasticQuarter, 124511, 124511 + 100, 0.3, CenterType.playfield);

            field.Resize(OsbEasing.OutElasticQuarter, 124801, 125092, -width, -height);
            field.Resize(OsbEasing.OutElasticQuarter, 125092, 125092 + 150, width, -height);

            field.RotatePlayFieldStatic(OsbEasing.None, 130124, 130124, Math.PI / 4);
            field.Resize(OsbEasing.OutElasticQuarter, 130124, 130221, -width, height);
            field.RotatePlayFieldStatic(OsbEasing.None, 130221, 130221, Math.PI / 4);
            field.Resize(OsbEasing.OutElasticQuarter, 130221, 130317, width, height);
            field.RotatePlayFieldStatic(OsbEasing.None, 130317, 130317, Math.PI / 4);
            field.Resize(OsbEasing.OutElasticQuarter, 130317, 130414, -width, height);
            field.RotatePlayFieldStatic(OsbEasing.None, 130414, 130414, Math.PI / 4);
            field.Resize(OsbEasing.OutElasticQuarter, 130414, 130511, width, height);
            field.RotatePlayFieldStatic(OsbEasing.None, 130511, 130511, Math.PI / 4);

            field.moveFieldY(OsbEasing.OutSine, 130124, 130898, -180f);
            field.Resize(OsbEasing.InCubic, 130511, 130898, width * 2, height);

            field.RotatePlayFieldStatic(OsbEasing.None, 130511, 130511, -Math.PI / 4 * 5);


            FakeNoteFadeIn(ColumnType.one, starttime + 10, 118124, starttime + 10, 0);
            FakeNoteFadeIn(ColumnType.two, starttime + 10, 118124, starttime + 10, 0);
            FakeNoteFadeIn(ColumnType.three, starttime + 10, 118124, starttime + 10, 0);
            FakeNoteFadeIn(ColumnType.four, starttime + 10, 118124, starttime + 10, 0);

            FakeNoteFadeIn(ColumnType.two, 119672, 118124, starttime + 10);
            FakeNoteFadeIn(ColumnType.four, 119672, 118124, starttime + 10);
            FakeNoteFadeIn(ColumnType.three, 119866, 118124, starttime + 10, 2);
            FakeNoteFadeIn(ColumnType.two, 119963, 118124, starttime + 10, 4);
            FakeNoteFadeIn(ColumnType.one, 119963, 118124, starttime + 10, 4);
            FakeNoteFadeIn(ColumnType.three, 120059, 118124, starttime + 10, 1);
            FakeNoteFadeIn(ColumnType.one, 120253, 118124, starttime + 10, 2);
            FakeNoteFadeIn(ColumnType.four, 120253, 118124, starttime + 10, 2);
            FakeNoteFadeIn(ColumnType.two, 120446, 118124, starttime + 10, 1);
            FakeNoteFadeIn(ColumnType.three, 120543, 118124, starttime + 10, 4);
            FakeNoteFadeIn(ColumnType.four, 120543, 118124, starttime + 10, 4);

            FakeNoteFadeIn(ColumnType.one, 120834, 119672, 120834, 0, true);
            FakeNoteFadeIn(ColumnType.two, 120834, 119672, 120834, 0, true);
            FakeNoteFadeIn(ColumnType.three, 120834, 119672, 120834, 0, true);
            FakeNoteFadeIn(ColumnType.four, 120834, 119672, 120834, 0, true);


            FakeNoteFadeIn(ColumnType.one, 121221, 119672, 120834, 1, true);
            FakeNoteFadeIn(ColumnType.three, 121221, 119672, 120834, 1, true);
            FakeNoteFadeIn(ColumnType.two, 121414, 119672, 120834, 2, true);
            FakeNoteFadeIn(ColumnType.three, 121511, 119672, 120834, 4, true);
            FakeNoteFadeIn(ColumnType.four, 121511, 119672, 120834, 4, true);
            FakeNoteFadeIn(ColumnType.two, 121608, 119672, 120834, 1, true);
            FakeNoteFadeIn(ColumnType.one, 121801, 119672, 120834, 2, true);
            FakeNoteFadeIn(ColumnType.four, 121801, 119672, 120834, 2, true);

            FakeNoteFadeIn(ColumnType.one, 122382, 121221, 122382, 0);
            FakeNoteFadeIn(ColumnType.two, 122382, 121221, 122382, 0);
            FakeNoteFadeIn(ColumnType.three, 122382, 121221, 122382, 0);
            FakeNoteFadeIn(ColumnType.four, 122382, 121221, 122382, 0);


            FakeNoteFadeIn(ColumnType.two, 122769, 121221, 122382, 1);
            FakeNoteFadeIn(ColumnType.three, 122769, 121221, 122382, 1);
            FakeNoteFadeIn(ColumnType.three, 122963, 121221, 122382, 2);
            FakeNoteFadeIn(ColumnType.one, 123059, 121221, 122382, 4);
            FakeNoteFadeIn(ColumnType.two, 123059, 121221, 122382, 4);
            FakeNoteFadeIn(ColumnType.three, 123156, 121221, 122382, 1);
            FakeNoteFadeIn(ColumnType.one, 123350, 121221, 122382, 2);
            FakeNoteFadeIn(ColumnType.four, 123350, 121221, 122382, 2);

            DrawInstance draw = new DrawInstance(CancellationToken, field, starttime, scrollSpeed, updatesPerSecond, OsbEasing.None, false, 50, fadeTime);
            draw.setReceptorMovementPrecision(0.1f);
            draw.setNoteMovementPrecision(0.25f);
            draw.setNoteRotationPrecision(0.01f);
            draw.setHoldRotationPrecision(0.01f);
            draw.drawViaEquation(endtime - starttime, NoteFunction, true);

            BigHits(receptors, 119285, 0, -Math.PI / 2, scrollSpeed, 125, -125);
            BigHits(receptors, 120834, Math.PI / 2, Math.PI, scrollSpeed, 125, -125);
            BigHits(receptors, 122382, 0, Math.PI, scrollSpeed, 125, -125);
            BigHits(receptors, 125479, Math.PI / 2, 0, scrollSpeed, 125, -125);
        }

        public void BigHits(StoryboardLayer layer, double hitTime, double rot, double rot2, double scrollSpeed, double offset = 0, double offset2 = 0)
        {
            if (offset2 == 0)
            {
                offset2 = offset;
            }

            // The hitTime is the time when the hit happens
            // rot and rot2 are the rotation values for the two notes
            // scrollSpeed is the speed at which the notes scroll
            // offset is the offset from the center of the screen where the notes will be placed
            {
                Vector2 pos1 = new Vector2(320 - 100, 240);
                Vector2 pos2 = new Vector2(320 + 100, 240);

                pos1.X -= (float)offset;
                pos2.X -= (float)offset2;

                var starttime = hitTime - scrollSpeed;


                var note = layer.CreateAnimation("sb/ani/arrow/frame.png", 48, 1000 / 38, OsbLoopType.LoopOnce, OsbOrigin.Centre, pos1);
                var note2 = layer.CreateAnimation("sb/ani/arrow/frame.png", 48, 1000 / 38, OsbLoopType.LoopOnce, OsbOrigin.Centre, pos2);

                var hit1 = layer.CreateAnimation("sb/ani/outline/frame.jpg", 48, 1000 / 38, OsbLoopType.LoopOnce, OsbOrigin.Centre, pos1);
                var hit2 = layer.CreateAnimation("sb/ani/outline/frame.jpg", 48, 1000 / 38, OsbLoopType.LoopOnce, OsbOrigin.Centre, pos2);

                hit1.Additive(starttime);
                hit2.Additive(starttime);

                hit1.ScaleVec(OsbEasing.OutCubic, starttime, hitTime, new Vector2(.5f), new Vector2(.33f));
                hit2.ScaleVec(OsbEasing.OutCubic, starttime, hitTime, new Vector2(.5f), new Vector2(.33f));

                note.Rotate(starttime, rot);
                note2.Rotate(starttime, rot2);

                note.ScaleVec(OsbEasing.InSine, starttime, hitTime, new Vector2(.05f), new Vector2(0.3f * 1.25f, 0.3f * 1.25f));
                note2.ScaleVec(OsbEasing.InSine, starttime, hitTime, new Vector2(.05f), new Vector2(0.3f * 1.25f, 0.3f * 1.25f));

                hit1.Rotate(OsbEasing.OutCubic, starttime, hitTime, rot - Math.PI, rot);
                hit2.Rotate(OsbEasing.OutCubic, starttime, hitTime, rot2 - Math.PI, rot2);

                note.Fade(starttime, starttime + 500, 0, 1);
                note2.Fade(starttime, starttime + 500, 0, 1);

                hit1.Fade(starttime, starttime + 500, 0, 1);
                hit2.Fade(starttime, starttime + 500, 0, 1);

                note.ScaleVec(OsbEasing.OutCirc, hitTime, hitTime + 250, new Vector2(.3f * 1.25f), new Vector2(.5f * 1.25f));
                note2.ScaleVec(OsbEasing.OutCirc, hitTime, hitTime + 250, new Vector2(.3f * 1.25f), new Vector2(.5f * 1.25f));

                hit1.ScaleVec(OsbEasing.OutCirc, hitTime, hitTime + 250, new Vector2(.33f), new Vector2(.53f));
                hit2.ScaleVec(OsbEasing.OutCirc, hitTime, hitTime + 250, new Vector2(.33f), new Vector2(.53f));

                hit1.Fade(OsbEasing.OutCirc, hitTime, hitTime + 250, 1, 0f);
                hit2.Fade(OsbEasing.OutCirc, hitTime, hitTime + 250, 1, 0);

                note.Fade(OsbEasing.OutCirc, hitTime, hitTime + 250, 1, 0f);
                note2.Fade(OsbEasing.OutCirc, hitTime, hitTime + 250, 1, 0);

            }
        }

        public void FakeNoteFadeIn(ColumnType type, double hittime, double fadeStart, double fadeEnd, double noteType = 1, bool inverse = false)
        {
            var height = 500f;
            var scrollSpeed = 1200f;

            var fadeInTime = fadeStart;
            var endtime = fadeEnd;

            var start = hittime - scrollSpeed;
            var currentTime = fadeEnd - start;
            var pos = field.columns[type].OriginPositionAt(fadeEnd);

            var travelAmount = currentTime / scrollSpeed * height;

            if (inverse)
            {
                travelAmount = -travelAmount;
            }

            var travelPos = pos - new Vector2(0, (float)travelAmount);

            OsbSprite fill = GetLayer("r").CreateSprite("sb/fill.png", OsbOrigin.Centre, travelPos);
            OsbSprite fakeNote = GetLayer("r").CreateAnimation("sb/ani/fadeOutline/frame-.png", 48, 1100 / 48, OsbLoopType.LoopOnce, OsbOrigin.Centre, travelPos);

            if (inverse == false)
                fill.MoveX(OsbEasing.InCubic, fadeInTime + 500, fadeEnd, travelPos.X + 8, travelPos.X);
            else
                fill.MoveX(OsbEasing.InCubic, fadeInTime + 500, fadeEnd, travelPos.X - 8, travelPos.X);

            fill.Scale(OsbEasing.InCubic, fadeInTime + 500, fadeEnd, 0.2f, 0.45f);
            fakeNote.Scale(fadeInTime, 0.45f);

            fill.Fade(OsbEasing.OutCubic, fadeInTime + 500, fadeEnd, 0, 1);
            fakeNote.Fade(fadeInTime, 1);

            if (noteType != 0)
                fill.Scale(OsbEasing.OutSine, endtime, endtime + 1500, 0.45f, 1f);
            //fakeNote.Scale(OsbEasing.OutSine, endtime, endtime + 1500, 0.45f, 1f);

            if (noteType != 0)
                fill.Fade(OsbEasing.OutSine, endtime, endtime + 1500, 1f, 0);
            else
                fill.Fade(endtime, 0);
            fakeNote.Fade(endtime, 0);

            switch (noteType)
            {
                case 1:
                    fill.Color(fadeInTime, new Color4(171, 93, 3, 255));
                    fakeNote.Color(fadeInTime, new Color4(171, 93, 3, 255));
                    break;
                case 2:
                    fill.Color(fadeInTime, new Color4(0, 133, 255, 0));
                    fakeNote.Color(fadeInTime, new Color4(0, 133, 255, 0));
                    break;
                case 4:
                    fill.Color(fadeInTime, new Color4(105, 201, 55, 255));
                    fakeNote.Color(fadeInTime, new Color4(105, 201, 55, 255));
                    break;
                case 0:
                    fill.Color(fadeInTime, new Color4(0, 0, 0, 255));
                    fakeNote.Color(fadeInTime, new Color4(150, 150, 150, 255));
                    break;
            }

            switch (type)
            {
                case ColumnType.one:
                    fill.Rotate(fadeInTime, 1 * Math.PI / 2);
                    fakeNote.Rotate(fadeInTime, 1 * Math.PI / 2);
                    break;
                case ColumnType.two:
                    fill.Rotate(fadeInTime, 0 * Math.PI / 2);
                    fakeNote.Rotate(fadeInTime, 0 * Math.PI / 2);
                    break;
                case ColumnType.three:
                    fill.Rotate(fadeInTime, 2 * Math.PI / 2);
                    fakeNote.Rotate(fadeInTime, 2 * Math.PI / 2);
                    break;
                case ColumnType.four:
                    fill.Rotate(fadeInTime, 3 * Math.PI / 2);
                    fakeNote.Rotate(fadeInTime, 3 * Math.PI / 2);
                    break;
            }

        }

        public Vector2 NoteFunction(EquationParameters p)
        {

            var pos = p.position;
            if (p.time > 126253)
            {
                var amplitude = 300;
                var x = -(float)Utility.SineWaveValue(Utility.SmoothAmplitudeByTime(p.time, 126253, 128575, 0, 50, 50), .5f, p.progress);
                var y = (float)Utility.SineWaveValue(Utility.SmoothAmplitudeByTime(p.time, 126253, 128575, 0, amplitude, amplitude), .5f, p.progress);
                pos.X += x;
                pos.Y += y;

            }
            return pos;
        }
    }
}
