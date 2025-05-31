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
    public class predrop : StoryboardObjectGenerator
    {
        //        public override bool Multithreaded => false;
        public override void Generate()
        {

            var receptors = GetLayer("r");
            var notes = GetLayer("n");

            // General values
            var starttime = 43027 - 25; // the starttime where the playfield is initialized
            var endtime = 54253; // the endtime where the playfield is nolonger beeing rendered
            var duration = endtime - starttime; // the length the playfield is kept alive

            // Playfield Scale
            var width = 250f; // widht of the playfield / invert to flip
            var height = 500f; // height of the playfield / invert to flip -600 = downscroll | 600 = upscropll
            var receptorWallOffset = 50f; // how big the boundary box for the receptor is 50 means it will be pushed away 50 units from the wall

            // Note initilization Values
            var sliderAccuracy = 30; // The Segment length for sliderbodies since they are rendered in slices 30 is default
            var isColored = false; // This property is used if you want to color the notes by urself for effects. It does not swap if the snap coloring is used.

            // Drawinstance Values
            var updatesPerSecond = 250; // The amount of steps the rendring engine does to render out note and receptor positions
            var scrollSpeed = 1200f; // The speed at which the Notes scroll
            var fadeTime = 150; // The time notes will fade in

            Playfield field = new Playfield();
            field.initilizePlayField(receptors, notes, starttime, endtime, width, height, receptorWallOffset, Beatmap.OverallDifficulty);
            field.initializeNotes(Beatmap.HitObjects.ToList(), Beatmap.GetTimingPointAt(starttime).Bpm, Beatmap.GetTimingPointAt(starttime).Offset, isColored, sliderAccuracy);

            Playfield field2 = new Playfield();
            field2.initilizePlayField(receptors, notes, 49511, endtime, width, height, receptorWallOffset, Beatmap.OverallDifficulty);
            field2.initializeNotes(Beatmap.HitObjects.ToList(), Beatmap.GetTimingPointAt(starttime).Bpm, Beatmap.GetTimingPointAt(starttime).Offset, isColored, sliderAccuracy);

            var colWidth = field.getColumnWidth() * 3;
            var halfwidth = field.getColumnWidth();

            var localtime = 43414;
            var gap = 43801 - 43414;
            field.ScaleReceptor(OsbEasing.None, localtime, localtime, new Vector2(2f), ColumnType.all);
            field.ScaleOrigin(OsbEasing.None, localtime, localtime, new Vector2(0.25f), ColumnType.all);

            field.ScaleReceptor(OsbEasing.OutCubic, localtime, localtime + gap, new Vector2(0.5f), ColumnType.all);
            field.ScaleOrigin(OsbEasing.OutCubic, localtime, localtime + gap, new Vector2(0.5f), ColumnType.all);

            field.MoveReceptorRelative(OsbEasing.None, localtime - 10, localtime - 10, new Vector2(-colWidth, 0), ColumnType.one);
            field.MoveReceptorRelative(OsbEasing.OutCubic, localtime, localtime + gap, new Vector2(colWidth, 0), ColumnType.one);
            field.MoveReceptorRelative(OsbEasing.None, localtime - 10, localtime - 10, new Vector2(-halfwidth, 0), ColumnType.two);
            field.MoveReceptorRelative(OsbEasing.OutCubic, localtime, localtime + gap, new Vector2(halfwidth, 0), ColumnType.two);
            field.MoveReceptorRelative(OsbEasing.None, localtime - 10, localtime - 10, new Vector2(halfwidth, 0), ColumnType.three);
            field.MoveReceptorRelative(OsbEasing.OutCubic, localtime, localtime + gap, new Vector2(-halfwidth, 0), ColumnType.three);
            field.MoveReceptorRelative(OsbEasing.None, localtime - 10, localtime - 10, new Vector2(colWidth, 0), ColumnType.four);
            field.MoveReceptorRelative(OsbEasing.OutCubic, localtime, localtime + gap, new Vector2(-colWidth, 0), ColumnType.four);

            field.MoveOriginRelative(OsbEasing.None, localtime - 10, localtime + gap - 10, new Vector2(colWidth / 4, 0), ColumnType.one);
            field.MoveOriginRelative(OsbEasing.OutCubic, localtime, localtime + gap, new Vector2(-colWidth / 4, 0), ColumnType.one);
            field.MoveOriginRelative(OsbEasing.None, localtime - 10, localtime + gap - 10, new Vector2(halfwidth / 4, 0), ColumnType.two);
            field.MoveOriginRelative(OsbEasing.OutCubic, localtime, localtime + gap, new Vector2(-halfwidth / 4, 0), ColumnType.two);
            field.MoveOriginRelative(OsbEasing.None, localtime - 10, localtime + gap - 10, new Vector2(-halfwidth / 4, 0), ColumnType.three);
            field.MoveOriginRelative(OsbEasing.OutCubic, localtime, localtime + gap, new Vector2(halfwidth / 4, 0), ColumnType.three);
            field.MoveOriginRelative(OsbEasing.None, localtime - 10, localtime + gap - 10, new Vector2(-colWidth / 4, 0), ColumnType.four);
            field.MoveOriginRelative(OsbEasing.OutCubic, localtime, localtime + gap, new Vector2(colWidth / 4, 0), ColumnType.four);

            localtime = 44188;
            field.ScaleOrigin(OsbEasing.None, localtime, localtime, new Vector2(3f), ColumnType.all);
            field.ScaleReceptor(OsbEasing.None, localtime, localtime, new Vector2(0.25f), ColumnType.all);

            field.ScaleOrigin(OsbEasing.OutCubic, localtime, localtime + gap, new Vector2(0.5f), ColumnType.all);
            field.ScaleReceptor(OsbEasing.OutCubic, localtime, localtime + gap, new Vector2(0.5f), ColumnType.all);

            field.MoveReceptorRelative(OsbEasing.None, localtime - 10, localtime - 10, new Vector2(colWidth / 4, 0), ColumnType.one);
            field.MoveReceptorRelative(OsbEasing.OutCubic, localtime, localtime + gap, new Vector2(-colWidth / 4, 0), ColumnType.one);
            field.MoveReceptorRelative(OsbEasing.None, localtime - 10, localtime - 10, new Vector2(halfwidth / 4, 0), ColumnType.two);
            field.MoveReceptorRelative(OsbEasing.OutCubic, localtime, localtime + gap, new Vector2(-halfwidth / 4, 0), ColumnType.two);
            field.MoveReceptorRelative(OsbEasing.None, localtime - 10, localtime - 10, new Vector2(-halfwidth / 4, 0), ColumnType.three);
            field.MoveReceptorRelative(OsbEasing.OutCubic, localtime, localtime + gap, new Vector2(halfwidth / 4, 0), ColumnType.three);
            field.MoveReceptorRelative(OsbEasing.None, localtime - 10, localtime - 10, new Vector2(-colWidth / 4, 0), ColumnType.four);
            field.MoveReceptorRelative(OsbEasing.OutCubic, localtime, localtime + gap, new Vector2(colWidth / 4, 0), ColumnType.four);

            field.MoveOriginRelative(OsbEasing.None, localtime - 10, localtime - 10, new Vector2(-colWidth, 0), ColumnType.one);
            field.MoveOriginRelative(OsbEasing.OutCubic, localtime, localtime + gap, new Vector2(colWidth, 0), ColumnType.one);
            field.MoveOriginRelative(OsbEasing.None, localtime - 10, localtime - 10, new Vector2(-halfwidth, 0), ColumnType.two);
            field.MoveOriginRelative(OsbEasing.OutCubic, localtime, localtime + gap, new Vector2(halfwidth, 0), ColumnType.two);
            field.MoveOriginRelative(OsbEasing.None, localtime - 10, localtime - 10, new Vector2(halfwidth, 0), ColumnType.three);
            field.MoveOriginRelative(OsbEasing.OutCubic, localtime, localtime + gap, new Vector2(-halfwidth, 0), ColumnType.three);
            field.MoveOriginRelative(OsbEasing.None, localtime - 10, localtime - 10, new Vector2(colWidth, 0), ColumnType.four);
            field.MoveOriginRelative(OsbEasing.OutCubic, localtime, localtime + gap, new Vector2(-colWidth, 0), ColumnType.four);

            localtime = 44963;
            field.ScaleReceptor(OsbEasing.None, localtime, localtime, new Vector2(2f), ColumnType.all);
            field.ScaleOrigin(OsbEasing.None, localtime, localtime, new Vector2(0.25f), ColumnType.all);

            field.ScaleReceptor(OsbEasing.OutCubic, localtime, localtime + gap, new Vector2(0.5f), ColumnType.all);
            field.ScaleOrigin(OsbEasing.OutCubic, localtime, localtime + gap, new Vector2(0.5f), ColumnType.all);

            field.MoveReceptorRelative(OsbEasing.None, localtime - 10, localtime - 10, new Vector2(-colWidth, 0), ColumnType.one);
            field.MoveReceptorRelative(OsbEasing.OutCubic, localtime, localtime + gap, new Vector2(colWidth, 0), ColumnType.one);
            field.MoveReceptorRelative(OsbEasing.None, localtime - 10, localtime - 10, new Vector2(-halfwidth, 0), ColumnType.two);
            field.MoveReceptorRelative(OsbEasing.OutCubic, localtime, localtime + gap, new Vector2(halfwidth, 0), ColumnType.two);
            field.MoveReceptorRelative(OsbEasing.None, localtime - 10, localtime - 10, new Vector2(halfwidth, 0), ColumnType.three);
            field.MoveReceptorRelative(OsbEasing.OutCubic, localtime, localtime + gap, new Vector2(-halfwidth, 0), ColumnType.three);
            field.MoveReceptorRelative(OsbEasing.None, localtime - 10, localtime - 10, new Vector2(colWidth, 0), ColumnType.four);
            field.MoveReceptorRelative(OsbEasing.OutCubic, localtime, localtime + gap, new Vector2(-colWidth, 0), ColumnType.four);

            field.MoveOriginRelative(OsbEasing.None, localtime - 10, localtime + gap - 10, new Vector2(colWidth / 4, 0), ColumnType.one);
            field.MoveOriginRelative(OsbEasing.OutCubic, localtime, localtime + gap, new Vector2(-colWidth / 4, 0), ColumnType.one);
            field.MoveOriginRelative(OsbEasing.None, localtime - 10, localtime + gap - 10, new Vector2(halfwidth / 4, 0), ColumnType.two);
            field.MoveOriginRelative(OsbEasing.OutCubic, localtime, localtime + gap, new Vector2(-halfwidth / 4, 0), ColumnType.two);
            field.MoveOriginRelative(OsbEasing.None, localtime - 10, localtime + gap - 10, new Vector2(-halfwidth / 4, 0), ColumnType.three);
            field.MoveOriginRelative(OsbEasing.OutCubic, localtime, localtime + gap, new Vector2(halfwidth / 4, 0), ColumnType.three);
            field.MoveOriginRelative(OsbEasing.None, localtime - 10, localtime + gap - 10, new Vector2(-colWidth / 4, 0), ColumnType.four);
            field.MoveOriginRelative(OsbEasing.OutCubic, localtime, localtime + gap, new Vector2(colWidth / 4, 0), ColumnType.four);

            localtime = 45737;
            field.ScaleOrigin(OsbEasing.None, localtime, localtime, new Vector2(3f), ColumnType.all);
            field.ScaleReceptor(OsbEasing.None, localtime, localtime, new Vector2(0.25f), ColumnType.all);

            field.ScaleOrigin(OsbEasing.OutCubic, localtime, localtime + gap, new Vector2(0.5f), ColumnType.all);
            field.ScaleReceptor(OsbEasing.OutCubic, localtime, localtime + gap, new Vector2(0.5f), ColumnType.all);

            field.MoveReceptorRelative(OsbEasing.None, localtime - 10, localtime - 10, new Vector2(colWidth / 4, 0), ColumnType.one);
            field.MoveReceptorRelative(OsbEasing.OutCubic, localtime, localtime + gap, new Vector2(-colWidth / 4, 0), ColumnType.one);
            field.MoveReceptorRelative(OsbEasing.None, localtime - 10, localtime - 10, new Vector2(halfwidth / 4, 0), ColumnType.two);
            field.MoveReceptorRelative(OsbEasing.OutCubic, localtime, localtime + gap, new Vector2(-halfwidth / 4, 0), ColumnType.two);
            field.MoveReceptorRelative(OsbEasing.None, localtime - 10, localtime - 10, new Vector2(-halfwidth / 4, 0), ColumnType.three);
            field.MoveReceptorRelative(OsbEasing.OutCubic, localtime, localtime + gap, new Vector2(halfwidth / 4, 0), ColumnType.three);
            field.MoveReceptorRelative(OsbEasing.None, localtime - 10, localtime - 10, new Vector2(-colWidth / 4, 0), ColumnType.four);
            field.MoveReceptorRelative(OsbEasing.OutCubic, localtime, localtime + gap, new Vector2(colWidth / 4, 0), ColumnType.four);

            field.MoveOriginRelative(OsbEasing.None, localtime - 10, localtime - 10, new Vector2(-colWidth, 0), ColumnType.one);
            field.MoveOriginRelative(OsbEasing.OutCubic, localtime, localtime + gap, new Vector2(colWidth, 0), ColumnType.one);
            field.MoveOriginRelative(OsbEasing.None, localtime - 10, localtime - 10, new Vector2(-halfwidth, 0), ColumnType.two);
            field.MoveOriginRelative(OsbEasing.OutCubic, localtime, localtime + gap, new Vector2(halfwidth, 0), ColumnType.two);
            field.MoveOriginRelative(OsbEasing.None, localtime - 10, localtime - 10, new Vector2(halfwidth, 0), ColumnType.three);
            field.MoveOriginRelative(OsbEasing.OutCubic, localtime, localtime + gap, new Vector2(-halfwidth, 0), ColumnType.three);
            field.MoveOriginRelative(OsbEasing.None, localtime - 10, localtime - 10, new Vector2(colWidth, 0), ColumnType.four);
            field.MoveOriginRelative(OsbEasing.OutCubic, localtime, localtime + gap, new Vector2(-colWidth, 0), ColumnType.four);

            localtime = 46511;
            field.ScaleReceptor(OsbEasing.None, localtime, localtime, new Vector2(2f), ColumnType.all);
            field.ScaleOrigin(OsbEasing.None, localtime, localtime, new Vector2(0.25f), ColumnType.all);

            field.ScaleReceptor(OsbEasing.OutCubic, localtime, localtime + gap, new Vector2(0.5f), ColumnType.all);
            field.ScaleOrigin(OsbEasing.OutCubic, localtime, localtime + gap, new Vector2(0.5f), ColumnType.all);

            field.MoveReceptorRelative(OsbEasing.None, localtime - 10, localtime - 10, new Vector2(-colWidth, 0), ColumnType.one);
            field.MoveReceptorRelative(OsbEasing.OutCubic, localtime, localtime + gap, new Vector2(colWidth, 0), ColumnType.one);
            field.MoveReceptorRelative(OsbEasing.None, localtime - 10, localtime - 10, new Vector2(-halfwidth, 0), ColumnType.two);
            field.MoveReceptorRelative(OsbEasing.OutCubic, localtime, localtime + gap, new Vector2(halfwidth, 0), ColumnType.two);
            field.MoveReceptorRelative(OsbEasing.None, localtime - 10, localtime - 10, new Vector2(halfwidth, 0), ColumnType.three);
            field.MoveReceptorRelative(OsbEasing.OutCubic, localtime, localtime + gap, new Vector2(-halfwidth, 0), ColumnType.three);
            field.MoveReceptorRelative(OsbEasing.None, localtime - 10, localtime - 10, new Vector2(colWidth, 0), ColumnType.four);
            field.MoveReceptorRelative(OsbEasing.OutCubic, localtime, localtime + gap, new Vector2(-colWidth, 0), ColumnType.four);

            field.MoveOriginRelative(OsbEasing.None, localtime - 10, localtime + gap - 10, new Vector2(colWidth / 4, 0), ColumnType.one);
            field.MoveOriginRelative(OsbEasing.OutCubic, localtime, localtime + gap, new Vector2(-colWidth / 4, 0), ColumnType.one);
            field.MoveOriginRelative(OsbEasing.None, localtime - 10, localtime + gap - 10, new Vector2(halfwidth / 4, 0), ColumnType.two);
            field.MoveOriginRelative(OsbEasing.OutCubic, localtime, localtime + gap, new Vector2(-halfwidth / 4, 0), ColumnType.two);
            field.MoveOriginRelative(OsbEasing.None, localtime - 10, localtime + gap - 10, new Vector2(-halfwidth / 4, 0), ColumnType.three);
            field.MoveOriginRelative(OsbEasing.OutCubic, localtime, localtime + gap, new Vector2(halfwidth / 4, 0), ColumnType.three);
            field.MoveOriginRelative(OsbEasing.None, localtime - 10, localtime + gap - 10, new Vector2(-colWidth / 4, 0), ColumnType.four);
            field.MoveOriginRelative(OsbEasing.OutCubic, localtime, localtime + gap, new Vector2(colWidth / 4, 0), ColumnType.four);

            localtime = 47285;
            field.ScaleOrigin(OsbEasing.None, localtime, localtime, new Vector2(3f), ColumnType.all);
            field.ScaleReceptor(OsbEasing.None, localtime, localtime, new Vector2(0.25f), ColumnType.all);

            field.ScaleOrigin(OsbEasing.OutCubic, localtime, localtime + gap, new Vector2(0.5f), ColumnType.all);
            field.ScaleReceptor(OsbEasing.OutCubic, localtime, localtime + gap, new Vector2(0.5f), ColumnType.all);

            field.MoveReceptorRelative(OsbEasing.None, localtime - 10, localtime - 10, new Vector2(colWidth / 4, 0), ColumnType.one);
            field.MoveReceptorRelative(OsbEasing.OutCubic, localtime, localtime + gap, new Vector2(-colWidth / 4, 0), ColumnType.one);
            field.MoveReceptorRelative(OsbEasing.None, localtime - 10, localtime - 10, new Vector2(halfwidth / 4, 0), ColumnType.two);
            field.MoveReceptorRelative(OsbEasing.OutCubic, localtime, localtime + gap, new Vector2(-halfwidth / 4, 0), ColumnType.two);
            field.MoveReceptorRelative(OsbEasing.None, localtime - 10, localtime - 10, new Vector2(-halfwidth / 4, 0), ColumnType.three);
            field.MoveReceptorRelative(OsbEasing.OutCubic, localtime, localtime + gap, new Vector2(halfwidth / 4, 0), ColumnType.three);
            field.MoveReceptorRelative(OsbEasing.None, localtime - 10, localtime - 10, new Vector2(-colWidth / 4, 0), ColumnType.four);
            field.MoveReceptorRelative(OsbEasing.OutCubic, localtime, localtime + gap, new Vector2(colWidth / 4, 0), ColumnType.four);

            field.MoveOriginRelative(OsbEasing.None, localtime - 10, localtime - 10, new Vector2(-colWidth, 0), ColumnType.one);
            field.MoveOriginRelative(OsbEasing.OutCubic, localtime, localtime + gap, new Vector2(colWidth, 0), ColumnType.one);
            field.MoveOriginRelative(OsbEasing.None, localtime - 10, localtime - 10, new Vector2(-halfwidth, 0), ColumnType.two);
            field.MoveOriginRelative(OsbEasing.OutCubic, localtime, localtime + gap, new Vector2(halfwidth, 0), ColumnType.two);
            field.MoveOriginRelative(OsbEasing.None, localtime - 10, localtime - 10, new Vector2(halfwidth, 0), ColumnType.three);
            field.MoveOriginRelative(OsbEasing.OutCubic, localtime, localtime + gap, new Vector2(-halfwidth, 0), ColumnType.three);
            field.MoveOriginRelative(OsbEasing.None, localtime - 10, localtime - 10, new Vector2(colWidth, 0), ColumnType.four);
            field.MoveOriginRelative(OsbEasing.OutCubic, localtime, localtime + gap, new Vector2(-colWidth, 0), ColumnType.four);

            localtime = 48059;
            field.ScaleReceptor(OsbEasing.None, localtime, localtime, new Vector2(2f), ColumnType.all);
            field.ScaleOrigin(OsbEasing.None, localtime, localtime, new Vector2(0.25f), ColumnType.all);

            field.ScaleReceptor(OsbEasing.OutCubic, localtime, localtime + gap, new Vector2(0.5f), ColumnType.all);
            field.ScaleOrigin(OsbEasing.OutCubic, localtime, localtime + gap, new Vector2(0.5f), ColumnType.all);

            field.MoveReceptorRelative(OsbEasing.None, localtime - 10, localtime - 10, new Vector2(-colWidth, 0), ColumnType.one);
            field.MoveReceptorRelative(OsbEasing.OutCubic, localtime, localtime + gap, new Vector2(colWidth, 0), ColumnType.one);
            field.MoveReceptorRelative(OsbEasing.None, localtime - 10, localtime - 10, new Vector2(-halfwidth, 0), ColumnType.two);
            field.MoveReceptorRelative(OsbEasing.OutCubic, localtime, localtime + gap, new Vector2(halfwidth, 0), ColumnType.two);
            field.MoveReceptorRelative(OsbEasing.None, localtime - 10, localtime - 10, new Vector2(halfwidth, 0), ColumnType.three);
            field.MoveReceptorRelative(OsbEasing.OutCubic, localtime, localtime + gap, new Vector2(-halfwidth, 0), ColumnType.three);
            field.MoveReceptorRelative(OsbEasing.None, localtime - 10, localtime - 10, new Vector2(colWidth, 0), ColumnType.four);
            field.MoveReceptorRelative(OsbEasing.OutCubic, localtime, localtime + gap, new Vector2(-colWidth, 0), ColumnType.four);

            field.MoveOriginRelative(OsbEasing.None, localtime - 10, localtime + gap - 10, new Vector2(colWidth / 4, 0), ColumnType.one);
            field.MoveOriginRelative(OsbEasing.OutCubic, localtime, localtime + gap, new Vector2(-colWidth / 4, 0), ColumnType.one);
            field.MoveOriginRelative(OsbEasing.None, localtime - 10, localtime + gap - 10, new Vector2(halfwidth / 4, 0), ColumnType.two);
            field.MoveOriginRelative(OsbEasing.OutCubic, localtime, localtime + gap, new Vector2(-halfwidth / 4, 0), ColumnType.two);
            field.MoveOriginRelative(OsbEasing.None, localtime - 10, localtime + gap - 10, new Vector2(-halfwidth / 4, 0), ColumnType.three);
            field.MoveOriginRelative(OsbEasing.OutCubic, localtime, localtime + gap, new Vector2(halfwidth / 4, 0), ColumnType.three);
            field.MoveOriginRelative(OsbEasing.None, localtime - 10, localtime + gap - 10, new Vector2(-colWidth / 4, 0), ColumnType.four);
            field.MoveOriginRelative(OsbEasing.OutCubic, localtime, localtime + gap, new Vector2(colWidth / 4, 0), ColumnType.four);

            field.RotatePlayFieldStatic(OsbEasing.InOutCubic, 45995, 45995, Math.PI * 2 / 6);
            field.RotatePlayFieldStatic(OsbEasing.InOutCubic, 46059, 46059, Math.PI * 2 / 6);
            field.RotatePlayFieldStatic(OsbEasing.InOutCubic, 46188, 46188, Math.PI * 2 / 6);
            field.RotatePlayFieldStatic(OsbEasing.InOutCubic, 46317, 46317, Math.PI * 2 / 6);
            field.RotatePlayFieldStatic(OsbEasing.InOutCubic, 46382, 46382, Math.PI * 2 / 6);
            field.RotatePlayFieldStatic(OsbEasing.InOutCubic, 46511, 46511, Math.PI * 2 / 6);

            field.RotatePlayFieldStatic(OsbEasing.InOutCubic, 48059, 49608, Math.PI * 2);
            field.moveFieldY(OsbEasing.InOutCubic, 48059, 49608, 240 - 50f);

            field2.moveFieldY(OsbEasing.InOutCubic, 48059, 49608, 240 - 50f);

            double localstart = 49608;
            double end = 54253;
            double dura = (end - localstart) / 6;

            while (localstart < end)
            {
                field2.ScaleOrigin(OsbEasing.InCubic, localstart, localstart + dura / 2, new Vector2(3f), ColumnType.all);
                field2.ScaleOrigin(OsbEasing.OutCubic, localstart + dura / 2, localstart + dura, new Vector2(.5f), ColumnType.all);
                field2.MoveOriginRelative(OsbEasing.InOutSine, localstart, localstart + dura, new Vector2(0, -height * 2), ColumnType.all);

                field2.MoveOriginRelative(OsbEasing.OutSine, localstart, localstart + dura / 2, new Vector2(-colWidth * 1.5f, 0), ColumnType.one);
                field2.MoveOriginRelative(OsbEasing.OutSine, localstart, localstart + dura / 2, new Vector2(-halfwidth * 1.5f, 0), ColumnType.two);
                field2.MoveOriginRelative(OsbEasing.OutSine, localstart, localstart + dura / 2, new Vector2(halfwidth * 1.5f, 0), ColumnType.three);
                field2.MoveOriginRelative(OsbEasing.OutSine, localstart, localstart + dura / 2, new Vector2(colWidth * 1.5f, 0), ColumnType.four);

                field2.MoveOriginRelative(OsbEasing.InSine, localstart + dura / 2, localstart + dura, new Vector2(colWidth * 1.5f, 0), ColumnType.one);
                field2.MoveOriginRelative(OsbEasing.InSine, localstart + dura / 2, localstart + dura, new Vector2(halfwidth * 1.5f, 0), ColumnType.two);
                field2.MoveOriginRelative(OsbEasing.InSine, localstart + dura / 2, localstart + dura, new Vector2(-halfwidth * 1.5f, 0), ColumnType.three);
                field2.MoveOriginRelative(OsbEasing.InSine, localstart + dura / 2, localstart + dura, new Vector2(-colWidth * 1.5f, 0), ColumnType.four);
                localstart += dura;

                field2.ScaleOrigin(OsbEasing.InCubic, localstart, localstart + dura / 2, new Vector2(0.1f), ColumnType.all);
                field2.ScaleOrigin(OsbEasing.OutCubic, localstart + dura / 2, localstart + dura, new Vector2(.5f), ColumnType.all);
                field2.MoveOriginRelative(OsbEasing.InOutSine, localstart, localstart + dura, new Vector2(0, height * 2), ColumnType.all);

                field2.MoveOriginRelative(OsbEasing.OutSine, localstart, localstart + dura / 2, new Vector2(colWidth / 2, 0), ColumnType.one);
                field2.MoveOriginRelative(OsbEasing.OutSine, localstart, localstart + dura / 2, new Vector2(halfwidth / 2, 0), ColumnType.two);
                field2.MoveOriginRelative(OsbEasing.OutSine, localstart, localstart + dura / 2, new Vector2(-halfwidth / 2, 0), ColumnType.three);
                field2.MoveOriginRelative(OsbEasing.OutSine, localstart, localstart + dura / 2, new Vector2(-colWidth / 2, 0), ColumnType.four);

                field2.MoveOriginRelative(OsbEasing.InSine, localstart + dura / 2, localstart + dura, new Vector2(-colWidth / 2, 0), ColumnType.one);
                field2.MoveOriginRelative(OsbEasing.InSine, localstart + dura / 2, localstart + dura, new Vector2(-halfwidth / 2, 0), ColumnType.two);
                field2.MoveOriginRelative(OsbEasing.InSine, localstart + dura / 2, localstart + dura, new Vector2(halfwidth / 2, 0), ColumnType.three);
                field2.MoveOriginRelative(OsbEasing.InSine, localstart + dura / 2, localstart + dura, new Vector2(colWidth / 2, 0), ColumnType.four);
                localstart += dura;
                dura *= .75;
            }



            field2.MoveOriginRelative(OsbEasing.OutSine, 54253, 55801, new Vector2(0, -height), ColumnType.all);
            field2.Resize(OsbEasing.OutExpo, 55801, 55801 + 350, width, height * 2);
            field2.moveFieldY(OsbEasing.OutExpo, 55801, 55801 + 350, -240);

            DrawInstance draw = new DrawInstance(CancellationToken, field, 43414, scrollSpeed, updatesPerSecond, OsbEasing.None, true, 0, fadeTime);
            draw.setReceptorMovementPrecision(0.25f);
            draw.setNoteRotationPrecision(0.001f);
            draw.drawViaEquation(49608 - 43414, NoteFunction, true);

            DrawInstance draw2 = new DrawInstance(CancellationToken, field2, 49608, scrollSpeed, updatesPerSecond, OsbEasing.None, false, 0, fadeTime);
            draw2.setReceptorMovementPrecision(0.25f);
            draw2.setNoteRotationPrecision(0.001f);
            draw2.drawViaEquation(endtime - 49608, NoteFunction, true);
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
