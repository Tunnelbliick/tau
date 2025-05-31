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

            BigHits(receptors, 119285, Math.PI / 2, 0, scrollSpeed);
            BigHits(receptors, 120834, Math.PI, -Math.PI / 2, scrollSpeed);
            BigHits(receptors, 122382, Math.PI / 2, Math.PI, scrollSpeed);
            BigHits(receptors, 125479, 0, -Math.PI / 2, scrollSpeed);

            var cleaned = Beatmap.HitObjects
                .Where(h => h.StartTime != 119285 && h.StartTime != 120834 && h.StartTime != 122382 && h.StartTime != 125479)
                .ToList();

            field.initilizePlayField(receptors, notes, starttime, endtime, width, height, receptorWallOffset, Beatmap.OverallDifficulty);
            field.initializeNotes(cleaned, Beatmap, isColored, sliderAccuracy);


            field.Resize(OsbEasing.OutCubic, 119285, 119285, -width, -height);
            field.Resize(OsbEasing.OutExpo, 119285, 119672, width, height);

            field.ScaleColumn(OsbEasing.None, 119285, 119285, new Vector2(.5f, 0f), ColumnType.one);
            field.ScaleColumn(OsbEasing.None, 119285, 119285, new Vector2(0f, .5f), ColumnType.two);
            field.ScaleColumn(OsbEasing.None, 119285, 119285, new Vector2(0f, .5f), ColumnType.three);
            field.ScaleColumn(OsbEasing.None, 119285, 119285, new Vector2(.5f, 0f), ColumnType.four);

            field.ScaleColumn(OsbEasing.OutExpo, 119285, 119479, new Vector2(0.5f), ColumnType.one);
            field.ScaleColumn(OsbEasing.OutExpo, 119285, 119479, new Vector2(0.5f), ColumnType.two);
            field.ScaleColumn(OsbEasing.OutExpo, 119285, 119479, new Vector2(0.5f), ColumnType.three);
            field.ScaleColumn(OsbEasing.OutExpo, 119285, 119479, new Vector2(0.5f), ColumnType.four);

            field.ScaleColumn(OsbEasing.InExpo, 120543, 120809, new Vector2(.5f, 0f), ColumnType.one);
            field.ScaleColumn(OsbEasing.InExpo, 120543, 120809, new Vector2(0f, .5f), ColumnType.two);
            field.ScaleColumn(OsbEasing.InExpo, 120543, 120809, new Vector2(0f, .5f), ColumnType.three);
            field.ScaleColumn(OsbEasing.InExpo, 120543, 120809, new Vector2(.5f, 0f), ColumnType.four);

            field.ScaleColumn(OsbEasing.OutExpo, 120834, 121027, new Vector2(0.5f), ColumnType.one);
            field.ScaleColumn(OsbEasing.OutExpo, 120834, 121027, new Vector2(0.5f), ColumnType.two);
            field.ScaleColumn(OsbEasing.OutExpo, 120834, 121027, new Vector2(0.5f), ColumnType.three);
            field.ScaleColumn(OsbEasing.OutExpo, 120834, 121027, new Vector2(0.5f), ColumnType.four);

            field.Resize(OsbEasing.InExpo, 120543, 120834, 0, -height);
            field.moveFieldY(OsbEasing.None, 120834, 120834, 480f - receptorWallOffset * 2);
            field.Resize(OsbEasing.OutExpo, 120834, 121027, width, -height);

            field.ScaleColumn(OsbEasing.InExpo, 122092, 122309, new Vector2(.5f, 0f), ColumnType.one);
            field.ScaleColumn(OsbEasing.InExpo, 122092, 122309, new Vector2(0f, .5f), ColumnType.two);
            field.ScaleColumn(OsbEasing.InExpo, 122092, 122309, new Vector2(0f, .5f), ColumnType.three);
            field.ScaleColumn(OsbEasing.InExpo, 122092, 122309, new Vector2(.5f, 0f), ColumnType.four);

            field.ScaleColumn(OsbEasing.OutExpo, 122406, 122575, new Vector2(0.5f), ColumnType.one);
            field.ScaleColumn(OsbEasing.OutExpo, 122406, 122575, new Vector2(0.5f), ColumnType.two);
            field.ScaleColumn(OsbEasing.OutExpo, 122406, 122575, new Vector2(0.5f), ColumnType.three);
            field.ScaleColumn(OsbEasing.OutExpo, 122406, 122575, new Vector2(0.5f), ColumnType.four);

            field.Resize(OsbEasing.InExpo, 122092, 122358, 0, height);
            field.moveFieldY(OsbEasing.None, 122358, 122358, -480f + receptorWallOffset * 2);
            field.Resize(OsbEasing.OutExpo, 122358, 122575, width, height);

            field.moveFieldX(OsbEasing.OutSine, starttime - 1000, 120834, 250);
            field.moveFieldX(OsbEasing.None, 120834, 120834, -400);
            field.moveFieldX(OsbEasing.OutQuint, 120834, 123930, -150);

            field.moveFieldX(OsbEasing.None, 122382, 122382, 450);
            field.moveFieldX(OsbEasing.OutSine, 122382, 123930, 100);

            field.moveFieldX(OsbEasing.InOutSine, 123930, 125479, -500);
            field.moveFieldX(OsbEasing.OutCubic, 125479, 126253, 250);

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


            DrawInstance draw = new DrawInstance(CancellationToken, field, starttime + 10, scrollSpeed, updatesPerSecond, OsbEasing.None, false, fadeTime, fadeTime);
            draw.setReceptorMovementPrecision(0.1f);
            draw.setNoteMovementPrecision(0.25f);
            draw.setNoteRotationPrecision(0.01f);
            draw.setHoldRotationPrecision(0.01f);
            draw.setHoldRotationDeadZone(0.01f);
            draw.drawViaEquation(endtime - starttime + 10, NoteFunction, true);
        }

        public void BigHits(StoryboardLayer layer, double hitTime, double rot, double rot2, double scrollSpeed)
        {
            Vector2 pos1 = new Vector2(320 - 100, 240);
            Vector2 pos2 = new Vector2(320 + 100, 240);

            var starttime = hitTime - scrollSpeed;


            var note = layer.CreateSprite("sb/sprites/1.png", OsbOrigin.Centre, pos1);
            var note2 = layer.CreateSprite("sb/sprites/1.png", OsbOrigin.Centre, pos2);

            var hit1 = layer.CreateSprite("sb/outline.png", OsbOrigin.Centre, pos1);
            var hit2 = layer.CreateSprite("sb/outline.png", OsbOrigin.Centre, pos2);

            hit1.ScaleVec(OsbEasing.OutCubic, starttime, hitTime, new Vector2(.5f), new Vector2(1f));
            hit2.ScaleVec(OsbEasing.OutCubic, starttime, hitTime, new Vector2(.5f), new Vector2(1f));

            note.ScaleVec(OsbEasing.None, starttime, hitTime, -1f, 0f, 1f, 1f);
            note2.ScaleVec(OsbEasing.None, starttime, hitTime, 0f, -1f, 1f, 1f);

            note.Rotate(OsbEasing.OutCubic, starttime, hitTime, rot - Math.PI * 2, rot);
            note2.Rotate(OsbEasing.OutCubic, starttime, hitTime, rot2 - Math.PI * 2, rot2);

            hit1.Rotate(OsbEasing.OutCubic, starttime, hitTime, rot - Math.PI, rot);
            hit2.Rotate(OsbEasing.OutCubic, starttime, hitTime, rot2 - Math.PI, rot2);

            note.Fade(starttime, starttime + 500, 0, 1);
            note2.Fade(starttime, starttime + 500, 0, 1);

            hit1.Fade(starttime, starttime + 500, 0, 1);
            hit2.Fade(starttime, starttime + 500, 0, 1);

            note.ScaleVec(OsbEasing.OutCirc, hitTime, hitTime + 250, new Vector2(1f), new Vector2(1.5f));
            note2.ScaleVec(OsbEasing.OutCirc, hitTime, hitTime + 250, new Vector2(1f), new Vector2(1.5f));

            hit1.ScaleVec(OsbEasing.OutCirc, hitTime, hitTime + 250, new Vector2(1f), new Vector2(1.5f));
            hit2.ScaleVec(OsbEasing.OutCirc, hitTime, hitTime + 250, new Vector2(1f), new Vector2(1.5f));

            hit1.Fade(OsbEasing.OutCirc, hitTime, hitTime + 250, 0.8f, 0f);
            hit2.Fade(OsbEasing.OutCirc, hitTime, hitTime + 250, 0.8f, 0);

            note.Fade(OsbEasing.OutCirc, hitTime, hitTime + 250, 0.8f, 0f);
            note2.Fade(OsbEasing.OutCirc, hitTime, hitTime + 250, 0.8f, 0);

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
