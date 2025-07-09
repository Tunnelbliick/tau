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
using System.Drawing;
using System.Linq;

namespace StorybrewScripts
{

    public class Finaldrop : StoryboardObjectGenerator
    {
        //        public override bool Multithreaded => false;
        Playfield field = new Playfield();
        Playfield field2 = new Playfield();
        public override void Generate()
        {

            var receptors = GetLayer("r");
            var notes = GetLayer("n");

            // General values
            var starttime = 233761; // the starttime where the playfield is initialized
            var endtime = 264230; // the endtime where the playfield is nolonger beeing rendered
            var duration = endtime - starttime; // the length the playfield is kept alive

            // Playfield Scale
            var width = 250f; // widht of the playfield / invert to flip
            var height = -500; // height of the playfield / invert to flip -600 = downscroll | 600 = upscropll
            var receptorWallOffset = 50f; // how big the boundary box for the receptor is 50 means it will be pushed away 50 units from the wall

            // Note initilization Values
            var sliderAccuracy = 45; // The Segment length for sliderbodies since they are rendered in slices 30 is default
            var isColored = false; // This property is used if you want to color the notes by urself for effects. It does not swap if the snap coloring is used.

            // Drawinstance Values
            var updatesPerSecond = 100; // The amount of steps the rendring engine does to render out note and receptor positions
            var scrollSpeed = 1200f; // The speed at which the Notes scroll
            var fadeTime = 150; // The time notes will fade in

            field.initilizePlayField(receptors, notes, starttime, endtime, width, height, receptorWallOffset, Beatmap.OverallDifficulty);
            field.noteStart = 234464;
            field.noteEnd = 265167;
            field.initializeNotes(Beatmap.HitObjects.ToList(), Beatmap, isColored, sliderAccuracy, 249230);

            field2.initilizePlayField(receptors, notes, starttime, endtime, width, height, receptorWallOffset, Beatmap.OverallDifficulty);
            field2.noteStart = 248526;
            field2.noteEnd = 265167;
            field2.initializeNotes(Beatmap.HitObjects.ToList(), Beatmap, isColored, sliderAccuracy);

            var localstart = 234698;

            while (localstart < 248761)
            {
                field.ScaleReceptor(OsbEasing.None, localstart, localstart, new Vector2(0.6f), ColumnType.all);
                field.ScaleReceptor(OsbEasing.InQuart, localstart, localstart + 200, new Vector2(0.5f), ColumnType.all);
                localstart += (int)Beatmap.GetTimingPointAt(localstart).BeatDuration;
            }

            var gap = 234933 - 234229;
            var time = 234229;

            field.RotateColumn(OsbEasing.OutExpo, time, time + gap, Math.PI, ColumnType.one, CenterTypeColumn.columnX);
            field.RotateColumn(OsbEasing.OutExpo, time, time + gap, Math.PI, ColumnType.two, CenterTypeColumn.columnX);
            ReceptorBump(field, time, 8);

            time = 236104;
            field.RotateColumn(OsbEasing.OutExpo, time, time + gap, -Math.PI, ColumnType.one, CenterTypeColumn.columnX);
            field.RotateColumn(OsbEasing.OutExpo, time, time + gap, -Math.PI, ColumnType.two, CenterTypeColumn.columnX);
            field.RotateColumn(OsbEasing.OutExpo, time, time + gap, -Math.PI, ColumnType.three, CenterTypeColumn.columnX);
            field.RotateColumn(OsbEasing.OutExpo, time, time + gap, -Math.PI, ColumnType.four, CenterTypeColumn.columnX);
            ReceptorBump(field, time, 8);

            time = 237979;
            field.RotateColumn(OsbEasing.OutExpo, time, time + gap, Math.PI, ColumnType.one, CenterTypeColumn.columnX);
            field.RotateColumn(OsbEasing.OutExpo, time, time + gap, Math.PI, ColumnType.two, CenterTypeColumn.columnX);
            field.RotateColumn(OsbEasing.OutExpo, time, time + gap, Math.PI, ColumnType.three, CenterTypeColumn.columnX);
            field.RotateColumn(OsbEasing.OutExpo, time, time + gap, Math.PI, ColumnType.four, CenterTypeColumn.columnX);
            ReceptorBump(field, time, 8);

            time = 239854;
            field.RotateColumn(OsbEasing.OutExpo, time, time + gap, Math.PI, ColumnType.one, CenterTypeColumn.columnX);
            field.RotateColumn(OsbEasing.OutExpo, time, time + gap, Math.PI, ColumnType.two, CenterTypeColumn.columnX);
            field.RotateColumn(OsbEasing.OutExpo, time, time + gap, Math.PI, ColumnType.three, CenterTypeColumn.columnX);
            field.RotateColumn(OsbEasing.OutExpo, time, time + gap, Math.PI, ColumnType.four, CenterTypeColumn.columnX);
            ReceptorBump(field, time, 8);

            time = 241729;
            field.RotateColumn(OsbEasing.OutExpo, time, time + gap, Math.PI, ColumnType.one, CenterTypeColumn.columnX);
            field.RotateColumn(OsbEasing.OutExpo, time, time + gap, Math.PI, ColumnType.two, CenterTypeColumn.columnX);
            field.RotateColumn(OsbEasing.OutExpo, time, time + gap, Math.PI, ColumnType.three, CenterTypeColumn.columnX);
            field.RotateColumn(OsbEasing.OutExpo, time, time + gap, Math.PI, ColumnType.four, CenterTypeColumn.columnX);
            ReceptorBump(field, time, 8);

            time = 243604;
            field.RotateColumn(OsbEasing.OutExpo, time, time + gap, Math.PI, ColumnType.one, CenterTypeColumn.columnX);
            field.RotateColumn(OsbEasing.OutExpo, time, time + gap, Math.PI, ColumnType.two, CenterTypeColumn.columnX);
            field.RotateColumn(OsbEasing.OutExpo, time, time + gap, Math.PI, ColumnType.three, CenterTypeColumn.columnX);
            field.RotateColumn(OsbEasing.OutExpo, time, time + gap, Math.PI, ColumnType.four, CenterTypeColumn.columnX);
            ReceptorBump(field, time, 8);

            time = 245479;
            field.RotateColumn(OsbEasing.OutExpo, time, time + gap, Math.PI, ColumnType.one, CenterTypeColumn.columnX);
            field.RotateColumn(OsbEasing.OutExpo, time, time + gap, Math.PI, ColumnType.two, CenterTypeColumn.columnX);
            field.RotateColumn(OsbEasing.OutExpo, time, time + gap, Math.PI, ColumnType.three, CenterTypeColumn.columnX);
            field.RotateColumn(OsbEasing.OutExpo, time, time + gap, Math.PI, ColumnType.four, CenterTypeColumn.columnX);
            ReceptorBump(field, time, 8);

            time = 247354;
            field.RotateColumn(OsbEasing.OutExpo, time, time + gap, Math.PI, ColumnType.one, CenterTypeColumn.columnX);
            field.RotateColumn(OsbEasing.OutExpo, time, time + gap, Math.PI, ColumnType.two, CenterTypeColumn.columnX);
            field.RotateColumn(OsbEasing.OutExpo, time, time + gap, Math.PI, ColumnType.three, CenterTypeColumn.columnX);
            field.RotateColumn(OsbEasing.OutExpo, time, time + gap, Math.PI, ColumnType.four, CenterTypeColumn.columnX);
            field2.RotateColumn(OsbEasing.OutExpo, time, time + gap, Math.PI, ColumnType.three, CenterTypeColumn.columnX);
            field2.RotateColumn(OsbEasing.OutExpo, time, time + gap, Math.PI, ColumnType.four, CenterTypeColumn.columnX);
            ReceptorBump(field, time, 8);

            time = 248761;
            field.RotateColumn(OsbEasing.OutExpo, time, time + gap, Math.PI, ColumnType.one, CenterTypeColumn.columnX);
            field.RotateColumn(OsbEasing.OutExpo, time, time + gap, Math.PI, ColumnType.two, CenterTypeColumn.columnX);
            field2.RotateColumn(OsbEasing.OutExpo, time, time + gap, Math.PI, ColumnType.three, CenterTypeColumn.columnX);
            field2.RotateColumn(OsbEasing.OutExpo, time, time + gap, Math.PI, ColumnType.four, CenterTypeColumn.columnX);
            ReceptorBump(field, time, 4);
            ReceptorBump(field2, time, 4);
            field.moveFieldX(OsbEasing.OutExpo, time, time + gap, -field.getColumnWidth(250) / 2);
            field2.moveFieldX(OsbEasing.OutExpo, time, time + gap, field.getColumnWidth(250) / 2);

            time = 250636;
            field.RotateColumn(OsbEasing.OutExpo, time, time + gap, Math.PI, ColumnType.one, CenterTypeColumn.columnX);
            field.RotateColumn(OsbEasing.OutExpo, time, time + gap, Math.PI, ColumnType.two, CenterTypeColumn.columnX);
            field.RotateColumn(OsbEasing.OutExpo, time, time + gap, Math.PI, ColumnType.three, CenterTypeColumn.columnX);
            field.RotateColumn(OsbEasing.OutExpo, time, time + gap, Math.PI, ColumnType.four, CenterTypeColumn.columnX);
            field2.RotateColumn(OsbEasing.OutExpo, time, time + gap, Math.PI, ColumnType.one, CenterTypeColumn.columnX);
            field2.RotateColumn(OsbEasing.OutExpo, time, time + gap, Math.PI, ColumnType.two, CenterTypeColumn.columnX);
            field2.RotateColumn(OsbEasing.OutExpo, time, time + gap, Math.PI, ColumnType.three, CenterTypeColumn.columnX);
            field2.RotateColumn(OsbEasing.OutExpo, time, time + gap, Math.PI, ColumnType.four, CenterTypeColumn.columnX);


            time = 252511;
            field.RotateColumn(OsbEasing.OutExpo, time, time + gap, Math.PI, ColumnType.one, CenterTypeColumn.columnX);
            field.RotateColumn(OsbEasing.OutExpo, time, time + gap, Math.PI, ColumnType.two, CenterTypeColumn.columnX);
            field.RotateColumn(OsbEasing.OutExpo, time, time + gap, Math.PI, ColumnType.three, CenterTypeColumn.columnX);
            field.RotateColumn(OsbEasing.OutExpo, time, time + gap, Math.PI, ColumnType.four, CenterTypeColumn.columnX);
            field2.RotateColumn(OsbEasing.OutExpo, time, time + gap, Math.PI, ColumnType.one, CenterTypeColumn.columnX);
            field2.RotateColumn(OsbEasing.OutExpo, time, time + gap, Math.PI, ColumnType.two, CenterTypeColumn.columnX);
            field2.RotateColumn(OsbEasing.OutExpo, time, time + gap, Math.PI, ColumnType.three, CenterTypeColumn.columnX);
            field2.RotateColumn(OsbEasing.OutExpo, time, time + gap, Math.PI, ColumnType.four, CenterTypeColumn.columnX);

            time = 254386;
            field.RotateColumn(OsbEasing.OutExpo, time, time + gap, Math.PI, ColumnType.one, CenterTypeColumn.columnX);
            field.RotateColumn(OsbEasing.OutExpo, time, time + gap, Math.PI, ColumnType.two, CenterTypeColumn.columnX);
            field.RotateColumn(OsbEasing.OutExpo, time, time + gap, Math.PI, ColumnType.three, CenterTypeColumn.columnX);
            field.RotateColumn(OsbEasing.OutExpo, time, time + gap, Math.PI, ColumnType.four, CenterTypeColumn.columnX);
            field2.RotateColumn(OsbEasing.OutExpo, time, time + gap, Math.PI, ColumnType.one, CenterTypeColumn.columnX);
            field2.RotateColumn(OsbEasing.OutExpo, time, time + gap, Math.PI, ColumnType.two, CenterTypeColumn.columnX);
            field2.RotateColumn(OsbEasing.OutExpo, time, time + gap, Math.PI, ColumnType.three, CenterTypeColumn.columnX);
            field2.RotateColumn(OsbEasing.OutExpo, time, time + gap, Math.PI, ColumnType.four, CenterTypeColumn.columnX);

            time = 256261;
            field.RotateColumn(OsbEasing.OutExpo, time, time + gap, Math.PI, ColumnType.one, CenterTypeColumn.columnX);
            field.RotateColumn(OsbEasing.OutExpo, time, time + gap, Math.PI, ColumnType.two, CenterTypeColumn.columnX);
            field.RotateColumn(OsbEasing.OutExpo, time, time + gap, Math.PI, ColumnType.three, CenterTypeColumn.columnX);
            field.RotateColumn(OsbEasing.OutExpo, time, time + gap, Math.PI, ColumnType.four, CenterTypeColumn.columnX);
            field2.RotateColumn(OsbEasing.OutExpo, time, time + gap, Math.PI, ColumnType.one, CenterTypeColumn.columnX);
            field2.RotateColumn(OsbEasing.OutExpo, time, time + gap, Math.PI, ColumnType.two, CenterTypeColumn.columnX);
            field2.RotateColumn(OsbEasing.OutExpo, time, time + gap, Math.PI, ColumnType.three, CenterTypeColumn.columnX);
            field2.RotateColumn(OsbEasing.OutExpo, time, time + gap, Math.PI, ColumnType.four, CenterTypeColumn.columnX);

            time = 258136;
            field.RotateColumn(OsbEasing.OutExpo, time, time + gap, Math.PI, ColumnType.one, CenterTypeColumn.columnX);
            field.RotateColumn(OsbEasing.OutExpo, time, time + gap, Math.PI, ColumnType.two, CenterTypeColumn.columnX);
            field.RotateColumn(OsbEasing.OutExpo, time, time + gap, Math.PI, ColumnType.three, CenterTypeColumn.columnX);
            field.RotateColumn(OsbEasing.OutExpo, time, time + gap, Math.PI, ColumnType.four, CenterTypeColumn.columnX);
            field2.RotateColumn(OsbEasing.OutExpo, time, time + gap, Math.PI, ColumnType.one, CenterTypeColumn.columnX);
            field2.RotateColumn(OsbEasing.OutExpo, time, time + gap, Math.PI, ColumnType.two, CenterTypeColumn.columnX);
            field2.RotateColumn(OsbEasing.OutExpo, time, time + gap, Math.PI, ColumnType.three, CenterTypeColumn.columnX);
            field2.RotateColumn(OsbEasing.OutExpo, time, time + gap, Math.PI, ColumnType.four, CenterTypeColumn.columnX);

            time = 260011;
            field.RotateColumn(OsbEasing.OutExpo, time, time + gap, Math.PI, ColumnType.one, CenterTypeColumn.columnX);
            field.RotateColumn(OsbEasing.OutExpo, time, time + gap, Math.PI, ColumnType.two, CenterTypeColumn.columnX);
            field.RotateColumn(OsbEasing.OutExpo, time, time + gap, Math.PI, ColumnType.three, CenterTypeColumn.columnX);
            field.RotateColumn(OsbEasing.OutExpo, time, time + gap, Math.PI, ColumnType.four, CenterTypeColumn.columnX);
            field2.RotateColumn(OsbEasing.OutExpo, time, time + gap, Math.PI, ColumnType.one, CenterTypeColumn.columnX);
            field2.RotateColumn(OsbEasing.OutExpo, time, time + gap, Math.PI, ColumnType.two, CenterTypeColumn.columnX);
            field2.RotateColumn(OsbEasing.OutExpo, time, time + gap, Math.PI, ColumnType.three, CenterTypeColumn.columnX);
            field2.RotateColumn(OsbEasing.OutExpo, time, time + gap, Math.PI, ColumnType.four, CenterTypeColumn.columnX);

            time = 261886;
            field.RotateColumn(OsbEasing.OutExpo, time, time + gap, Math.PI, ColumnType.one, CenterTypeColumn.columnX);
            field.RotateColumn(OsbEasing.OutExpo, time, time + gap, Math.PI, ColumnType.two, CenterTypeColumn.columnX);
            field.RotateColumn(OsbEasing.OutExpo, time, time + gap, Math.PI, ColumnType.three, CenterTypeColumn.columnX);
            field.RotateColumn(OsbEasing.OutExpo, time, time + gap, Math.PI, ColumnType.four, CenterTypeColumn.columnX);
            field2.RotateColumn(OsbEasing.OutExpo, time, time + gap, Math.PI, ColumnType.one, CenterTypeColumn.columnX);
            field2.RotateColumn(OsbEasing.OutExpo, time, time + gap, Math.PI, ColumnType.two, CenterTypeColumn.columnX);
            field2.RotateColumn(OsbEasing.OutExpo, time, time + gap, Math.PI, ColumnType.three, CenterTypeColumn.columnX);
            field2.RotateColumn(OsbEasing.OutExpo, time, time + gap, Math.PI, ColumnType.four, CenterTypeColumn.columnX);

            time = 262823;
            field.RotateColumn(OsbEasing.OutExpo, time, time + gap, Math.PI, ColumnType.one, CenterTypeColumn.columnX);
            field.RotateColumn(OsbEasing.OutExpo, time, time + gap, Math.PI, ColumnType.two, CenterTypeColumn.columnX);
            field.RotateColumn(OsbEasing.OutExpo, time, time + gap, Math.PI, ColumnType.three, CenterTypeColumn.columnX);
            field.RotateColumn(OsbEasing.OutExpo, time, time + gap, Math.PI, ColumnType.four, CenterTypeColumn.columnX);
            field2.RotateColumn(OsbEasing.OutExpo, time, time + gap, Math.PI, ColumnType.one, CenterTypeColumn.columnX);
            field2.RotateColumn(OsbEasing.OutExpo, time, time + gap, Math.PI, ColumnType.two, CenterTypeColumn.columnX);
            field2.RotateColumn(OsbEasing.OutExpo, time, time + gap, Math.PI, ColumnType.three, CenterTypeColumn.columnX);
            field2.RotateColumn(OsbEasing.OutExpo, time, time + gap, Math.PI, ColumnType.four, CenterTypeColumn.columnX);

            time = 263292;
            field.RotateColumn(OsbEasing.OutExpo, time, time + gap, Math.PI, ColumnType.one, CenterTypeColumn.columnX);
            field.RotateColumn(OsbEasing.OutExpo, time, time + gap, Math.PI, ColumnType.two, CenterTypeColumn.columnX);
            field.RotateColumn(OsbEasing.OutExpo, time, time + gap, Math.PI, ColumnType.three, CenterTypeColumn.columnX);
            field.RotateColumn(OsbEasing.OutExpo, time, time + gap, Math.PI, ColumnType.four, CenterTypeColumn.columnX);
            field2.RotateColumn(OsbEasing.OutExpo, time, time + gap, Math.PI, ColumnType.one, CenterTypeColumn.columnX);
            field2.RotateColumn(OsbEasing.OutExpo, time, time + gap, Math.PI, ColumnType.two, CenterTypeColumn.columnX);
            field2.RotateColumn(OsbEasing.OutExpo, time, time + gap, Math.PI, ColumnType.three, CenterTypeColumn.columnX);
            field2.RotateColumn(OsbEasing.OutExpo, time, time + gap, Math.PI, ColumnType.four, CenterTypeColumn.columnX);
            ReceptorBump(field, time, 4, gap);
            ReceptorBump(field2, time, 4, gap);

            time = 263761;
            gap = 264230 - 263761;
            field.RotateColumn(OsbEasing.OutExpo, time, time + gap, Math.PI, ColumnType.one, CenterTypeColumn.columnX);
            field.RotateColumn(OsbEasing.OutExpo, time, time + gap, Math.PI, ColumnType.two, CenterTypeColumn.columnX);
            field.RotateColumn(OsbEasing.OutExpo, time, time + gap, Math.PI, ColumnType.three, CenterTypeColumn.columnX);
            field.RotateColumn(OsbEasing.OutExpo, time, time + gap, Math.PI, ColumnType.four, CenterTypeColumn.columnX);
            ReceptorBump(field, time, 4, gap);
            ReceptorBump(field2, time, 4, gap);

            field.Resize(OsbEasing.OutExpo, time, 264112, width, height);
            field2.Resize(OsbEasing.OutExpo, time, 264112, width, height);

            field.moveFieldX(OsbEasing.OutExpo, time, 264112, field.getColumnWidth(250) / 2);
            field2.moveFieldX(OsbEasing.OutExpo, time, 264112, -field.getColumnWidth(250) / 2);

            var adjustments = new Vector2[4];
            var adjustments1 = new Vector2[4];

            time = 248761;
            gap = (250636 - time);

            // First part - applying gradual 0.05 scale increments from left to right
            adjustments[0] = field.ScaleColumn(OsbEasing.InQuad, time, time + gap, new Vector2(0.7f), ColumnType.one, CenterTypeColumn.columnX, false, 0.65f);    // +0.15 from default
            adjustments1[0] = field2.ScaleColumn(OsbEasing.InQuad, time, time + gap, new Vector2(0.7f), ColumnType.four, CenterTypeColumn.columnX, false, 0.65f);   // -0.15 from default

            adjustments[1] = field.ScaleColumn(OsbEasing.InQuad, time, time + gap, new Vector2(0.6f), ColumnType.two, CenterTypeColumn.columnX, false, 0.5f);    // +0.10 from default
            adjustments1[1] = field2.ScaleColumn(OsbEasing.InQuad, time, time + gap, new Vector2(0.6f), ColumnType.three, CenterTypeColumn.columnX, false, 0.5f);   // -0.10 from default

            adjustments[2] = field.ScaleColumn(OsbEasing.InQuad, time, time + gap, new Vector2(0.4f), ColumnType.three, CenterTypeColumn.columnX, false, 0.5f);  // +0.05 from default
            adjustments1[2] = field2.ScaleColumn(OsbEasing.InQuad, time, time + gap, new Vector2(0.4f), ColumnType.two, CenterTypeColumn.columnX, false, 0.5f); // -0.05 from default

            adjustments[3] = field.ScaleColumn(OsbEasing.InQuad, time, time + gap, new Vector2(0.3f), ColumnType.four, CenterTypeColumn.columnX, false, 0.65f);   // default
            adjustments1[3] = field2.ScaleColumn(OsbEasing.InQuad, time, time + gap, new Vector2(0.3f), ColumnType.one, CenterTypeColumn.columnX, false, 0.65f);  // default

            field.ScaleColumn(OsbEasing.None, time + gap, time + gap, new Vector2(0.5f), ColumnType.all);
            field2.ScaleColumn(OsbEasing.None, time + gap, time + gap, new Vector2(0.5f), ColumnType.all);

            field.MoveReceptorRelative(OsbEasing.None, time + gap, time + gap, -adjustments[0], ColumnType.one);
            field2.MoveReceptorRelative(OsbEasing.None, time + gap, time + gap, -adjustments1[0], ColumnType.four);

            field.MoveReceptorRelative(OsbEasing.None, time + gap, time + gap, -adjustments[1], ColumnType.two);
            field2.MoveReceptorRelative(OsbEasing.None, time + gap, time + gap, -adjustments1[1], ColumnType.three);

            field.MoveReceptorRelative(OsbEasing.None, time + gap, time + gap, -adjustments[2], ColumnType.three);
            field2.MoveReceptorRelative(OsbEasing.None, time + gap, time + gap, -adjustments1[2], ColumnType.two);

            field.MoveReceptorRelative(OsbEasing.None, time + gap, time + gap, -adjustments[3], ColumnType.four);
            field2.MoveReceptorRelative(OsbEasing.None, time + gap, time + gap, -adjustments1[3], ColumnType.one);

            field.MoveOriginRelative(OsbEasing.None, time + gap, time + gap, adjustments[0], ColumnType.one);
            field2.MoveOriginRelative(OsbEasing.None, time + gap, time + gap, adjustments1[0], ColumnType.four);

            field.MoveOriginRelative(OsbEasing.None, time + gap, time + gap, adjustments[1], ColumnType.two);
            field2.MoveOriginRelative(OsbEasing.None, time + gap, time + gap, adjustments1[1], ColumnType.three);

            field.MoveOriginRelative(OsbEasing.None, time + gap, time + gap, adjustments[2], ColumnType.three);
            field2.MoveOriginRelative(OsbEasing.None, time + gap, time + gap, adjustments1[2], ColumnType.two);

            field.MoveOriginRelative(OsbEasing.None, time + gap, time + gap, adjustments[3], ColumnType.four);
            field2.MoveOriginRelative(OsbEasing.None, time + gap, time + gap, adjustments1[3], ColumnType.one);


            // Second part - invert the pattern
            time += gap;
            adjustments[0] = field2.ScaleColumn(OsbEasing.InQuad, time, time + gap, new Vector2(0.7f), ColumnType.four, CenterTypeColumn.columnX, false, 0.65f);
            adjustments1[0] = field.ScaleColumn(OsbEasing.InQuad, time, time + gap, new Vector2(0.7f), ColumnType.one, CenterTypeColumn.columnX, false, 0.65f);

            adjustments[1] = field2.ScaleColumn(OsbEasing.InQuad, time, time + gap, new Vector2(0.6f), ColumnType.three, CenterTypeColumn.columnX, false, 0.5f);
            adjustments1[1] = field.ScaleColumn(OsbEasing.InQuad, time, time + gap, new Vector2(0.6f), ColumnType.two, CenterTypeColumn.columnX, false, 0.5f);

            adjustments[2] = field2.ScaleColumn(OsbEasing.InQuad, time, time + gap, new Vector2(0.4f), ColumnType.two, CenterTypeColumn.columnX, false, 0.5f);
            adjustments1[2] = field.ScaleColumn(OsbEasing.InQuad, time, time + gap, new Vector2(0.4f), ColumnType.three, CenterTypeColumn.columnX, false, 0.5f);

            adjustments[3] = field2.ScaleColumn(OsbEasing.InQuad, time, time + gap, new Vector2(0.3f), ColumnType.one, CenterTypeColumn.columnX, false, 0.65f);
            adjustments1[3] = field.ScaleColumn(OsbEasing.InQuad, time, time + gap, new Vector2(0.3f), ColumnType.four, CenterTypeColumn.columnX, false, 0.65f);

            field.ScaleColumn(OsbEasing.None, time + gap, time + gap, new Vector2(0.5f), ColumnType.all);
            field2.ScaleColumn(OsbEasing.None, time + gap, time + gap, new Vector2(0.5f), ColumnType.all);

            field.MoveReceptorRelative(OsbEasing.None, time + gap, time + gap, -adjustments[0], ColumnType.four);
            field2.MoveReceptorRelative(OsbEasing.None, time + gap, time + gap, -adjustments1[0], ColumnType.one);

            field.MoveReceptorRelative(OsbEasing.None, time + gap, time + gap, -adjustments[1], ColumnType.three);
            field2.MoveReceptorRelative(OsbEasing.None, time + gap, time + gap, -adjustments1[1], ColumnType.two);

            field.MoveReceptorRelative(OsbEasing.None, time + gap, time + gap, -adjustments[2], ColumnType.two);
            field2.MoveReceptorRelative(OsbEasing.None, time + gap, time + gap, -adjustments1[2], ColumnType.three);

            field.MoveReceptorRelative(OsbEasing.None, time + gap, time + gap, -adjustments[3], ColumnType.one);
            field2.MoveReceptorRelative(OsbEasing.None, time + gap, time + gap, -adjustments1[3], ColumnType.four);

            field.MoveOriginRelative(OsbEasing.None, time + gap, time + gap, adjustments[0], ColumnType.four);
            field2.MoveOriginRelative(OsbEasing.None, time + gap, time + gap, adjustments1[0], ColumnType.one);

            field.MoveOriginRelative(OsbEasing.None, time + gap, time + gap, adjustments[1], ColumnType.three);
            field2.MoveOriginRelative(OsbEasing.None, time + gap, time + gap, adjustments1[1], ColumnType.two);

            field.MoveOriginRelative(OsbEasing.None, time + gap, time + gap, adjustments[2], ColumnType.two);
            field2.MoveOriginRelative(OsbEasing.None, time + gap, time + gap, adjustments1[2], ColumnType.three);

            field.MoveOriginRelative(OsbEasing.None, time + gap, time + gap, adjustments[3], ColumnType.one);
            field2.MoveOriginRelative(OsbEasing.None, time + gap, time + gap, adjustments1[3], ColumnType.four);

            time += gap;
            // First part - applying gradual 0.05 scale increments from left to right
            adjustments[0] = field.ScaleColumn(OsbEasing.InQuad, time, time + gap, new Vector2(0.7f), ColumnType.one, CenterTypeColumn.columnX, false, 0.65f);    // +0.15 from default
            adjustments1[0] = field2.ScaleColumn(OsbEasing.InQuad, time, time + gap, new Vector2(0.7f), ColumnType.four, CenterTypeColumn.columnX, false, 0.65f);   // -0.15 from default

            adjustments[1] = field.ScaleColumn(OsbEasing.InQuad, time, time + gap, new Vector2(0.6f), ColumnType.two, CenterTypeColumn.columnX, false, 0.5f);    // +0.10 from default
            adjustments1[1] = field2.ScaleColumn(OsbEasing.InQuad, time, time + gap, new Vector2(0.6f), ColumnType.three, CenterTypeColumn.columnX, false, 0.5f);   // -0.10 from default

            adjustments[2] = field.ScaleColumn(OsbEasing.InQuad, time, time + gap, new Vector2(0.4f), ColumnType.three, CenterTypeColumn.columnX, false, 0.5f);  // +0.05 from default
            adjustments1[2] = field2.ScaleColumn(OsbEasing.InQuad, time, time + gap, new Vector2(0.4f), ColumnType.two, CenterTypeColumn.columnX, false, 0.5f); // -0.05 from default

            adjustments[3] = field.ScaleColumn(OsbEasing.InQuad, time, time + gap, new Vector2(0.3f), ColumnType.four, CenterTypeColumn.columnX, false, 0.65f);   // default
            adjustments1[3] = field2.ScaleColumn(OsbEasing.InQuad, time, time + gap, new Vector2(0.3f), ColumnType.one, CenterTypeColumn.columnX, false, 0.65f);  // default

            field.ScaleColumn(OsbEasing.None, time + gap, time + gap, new Vector2(0.5f), ColumnType.all);
            field2.ScaleColumn(OsbEasing.None, time + gap, time + gap, new Vector2(0.5f), ColumnType.all);

            field.MoveReceptorRelative(OsbEasing.None, time + gap, time + gap, -adjustments[0], ColumnType.one);
            field2.MoveReceptorRelative(OsbEasing.None, time + gap, time + gap, -adjustments1[0], ColumnType.four);

            field.MoveReceptorRelative(OsbEasing.None, time + gap, time + gap, -adjustments[1], ColumnType.two);
            field2.MoveReceptorRelative(OsbEasing.None, time + gap, time + gap, -adjustments1[1], ColumnType.three);

            field.MoveReceptorRelative(OsbEasing.None, time + gap, time + gap, -adjustments[2], ColumnType.three);
            field2.MoveReceptorRelative(OsbEasing.None, time + gap, time + gap, -adjustments1[2], ColumnType.two);

            field.MoveReceptorRelative(OsbEasing.None, time + gap, time + gap, -adjustments[3], ColumnType.four);
            field2.MoveReceptorRelative(OsbEasing.None, time + gap, time + gap, -adjustments1[3], ColumnType.one);

            field.MoveOriginRelative(OsbEasing.None, time + gap, time + gap, adjustments[0], ColumnType.one);
            field2.MoveOriginRelative(OsbEasing.None, time + gap, time + gap, adjustments1[0], ColumnType.four);

            field.MoveOriginRelative(OsbEasing.None, time + gap, time + gap, adjustments[1], ColumnType.two);
            field2.MoveOriginRelative(OsbEasing.None, time + gap, time + gap, adjustments1[1], ColumnType.three);

            field.MoveOriginRelative(OsbEasing.None, time + gap, time + gap, adjustments[2], ColumnType.three);
            field2.MoveOriginRelative(OsbEasing.None, time + gap, time + gap, adjustments1[2], ColumnType.two);

            field.MoveOriginRelative(OsbEasing.None, time + gap, time + gap, adjustments[3], ColumnType.four);
            field2.MoveOriginRelative(OsbEasing.None, time + gap, time + gap, adjustments1[3], ColumnType.one);


            // Second part - invert the pattern
            time += gap;
            adjustments[0] = field2.ScaleColumn(OsbEasing.InQuad, time, time + gap, new Vector2(0.7f), ColumnType.four, CenterTypeColumn.columnX, false, 0.65f);
            adjustments1[0] = field.ScaleColumn(OsbEasing.InQuad, time, time + gap, new Vector2(0.7f), ColumnType.one, CenterTypeColumn.columnX, false, 0.65f);

            adjustments[1] = field2.ScaleColumn(OsbEasing.InQuad, time, time + gap, new Vector2(0.6f), ColumnType.three, CenterTypeColumn.columnX, false, 0.5f);
            adjustments1[1] = field.ScaleColumn(OsbEasing.InQuad, time, time + gap, new Vector2(0.6f), ColumnType.two, CenterTypeColumn.columnX, false, 0.5f);

            adjustments[2] = field2.ScaleColumn(OsbEasing.InQuad, time, time + gap, new Vector2(0.4f), ColumnType.two, CenterTypeColumn.columnX, false, 0.5f);
            adjustments1[2] = field.ScaleColumn(OsbEasing.InQuad, time, time + gap, new Vector2(0.4f), ColumnType.three, CenterTypeColumn.columnX, false, 0.5f);

            adjustments[3] = field2.ScaleColumn(OsbEasing.InQuad, time, time + gap, new Vector2(0.3f), ColumnType.one, CenterTypeColumn.columnX, false, 0.65f);
            adjustments1[3] = field.ScaleColumn(OsbEasing.InQuad, time, time + gap, new Vector2(0.3f), ColumnType.four, CenterTypeColumn.columnX, false, 0.65f);

            field.ScaleColumn(OsbEasing.None, time + gap, time + gap, new Vector2(0.5f), ColumnType.all);
            field2.ScaleColumn(OsbEasing.None, time + gap, time + gap, new Vector2(0.5f), ColumnType.all);

            field.MoveReceptorRelative(OsbEasing.None, time + gap, time + gap, -adjustments[0], ColumnType.four);
            field2.MoveReceptorRelative(OsbEasing.None, time + gap, time + gap, -adjustments1[0], ColumnType.one);

            field.MoveReceptorRelative(OsbEasing.None, time + gap, time + gap, -adjustments[1], ColumnType.three);
            field2.MoveReceptorRelative(OsbEasing.None, time + gap, time + gap, -adjustments1[1], ColumnType.two);

            field.MoveReceptorRelative(OsbEasing.None, time + gap, time + gap, -adjustments[2], ColumnType.two);
            field2.MoveReceptorRelative(OsbEasing.None, time + gap, time + gap, -adjustments1[2], ColumnType.three);

            field.MoveReceptorRelative(OsbEasing.None, time + gap, time + gap, -adjustments[3], ColumnType.one);
            field2.MoveReceptorRelative(OsbEasing.None, time + gap, time + gap, -adjustments1[3], ColumnType.four);

            field.MoveOriginRelative(OsbEasing.None, time + gap, time + gap, adjustments[0], ColumnType.four);
            field2.MoveOriginRelative(OsbEasing.None, time + gap, time + gap, adjustments1[0], ColumnType.one);

            field.MoveOriginRelative(OsbEasing.None, time + gap, time + gap, adjustments[1], ColumnType.three);
            field2.MoveOriginRelative(OsbEasing.None, time + gap, time + gap, adjustments1[1], ColumnType.two);

            field.MoveOriginRelative(OsbEasing.None, time + gap, time + gap, adjustments[2], ColumnType.two);
            field2.MoveOriginRelative(OsbEasing.None, time + gap, time + gap, adjustments1[2], ColumnType.three);

            field.MoveOriginRelative(OsbEasing.None, time + gap, time + gap, adjustments[3], ColumnType.one);
            field2.MoveOriginRelative(OsbEasing.None, time + gap, time + gap, adjustments1[3], ColumnType.four);

            time += gap;
            // First part - applying gradual 0.05 scale increments from left to right
            adjustments[0] = field.ScaleColumn(OsbEasing.InQuad, time, time + gap, new Vector2(0.7f), ColumnType.one, CenterTypeColumn.columnX, false, 0.65f);    // +0.15 from default
            adjustments1[0] = field2.ScaleColumn(OsbEasing.InQuad, time, time + gap, new Vector2(0.7f), ColumnType.four, CenterTypeColumn.columnX, false, 0.65f);   // -0.15 from default

            adjustments[1] = field.ScaleColumn(OsbEasing.InQuad, time, time + gap, new Vector2(0.6f), ColumnType.two, CenterTypeColumn.columnX, false, 0.5f);    // +0.10 from default
            adjustments1[1] = field2.ScaleColumn(OsbEasing.InQuad, time, time + gap, new Vector2(0.6f), ColumnType.three, CenterTypeColumn.columnX, false, 0.5f);   // -0.10 from default

            adjustments[2] = field.ScaleColumn(OsbEasing.InQuad, time, time + gap, new Vector2(0.4f), ColumnType.three, CenterTypeColumn.columnX, false, 0.5f);  // +0.05 from default
            adjustments1[2] = field2.ScaleColumn(OsbEasing.InQuad, time, time + gap, new Vector2(0.4f), ColumnType.two, CenterTypeColumn.columnX, false, 0.5f); // -0.05 from default

            adjustments[3] = field.ScaleColumn(OsbEasing.InQuad, time, time + gap, new Vector2(0.3f), ColumnType.four, CenterTypeColumn.columnX, false, 0.65f);   // default
            adjustments1[3] = field2.ScaleColumn(OsbEasing.InQuad, time, time + gap, new Vector2(0.3f), ColumnType.one, CenterTypeColumn.columnX, false, 0.65f);  // default

            field.ScaleColumn(OsbEasing.None, time + gap, time + gap, new Vector2(0.5f), ColumnType.all);
            field2.ScaleColumn(OsbEasing.None, time + gap, time + gap, new Vector2(0.5f), ColumnType.all);

            field.MoveReceptorRelative(OsbEasing.None, time + gap, time + gap, -adjustments[0], ColumnType.one);
            field2.MoveReceptorRelative(OsbEasing.None, time + gap, time + gap, -adjustments1[0], ColumnType.four);

            field.MoveReceptorRelative(OsbEasing.None, time + gap, time + gap, -adjustments[1], ColumnType.two);
            field2.MoveReceptorRelative(OsbEasing.None, time + gap, time + gap, -adjustments1[1], ColumnType.three);

            field.MoveReceptorRelative(OsbEasing.None, time + gap, time + gap, -adjustments[2], ColumnType.three);
            field2.MoveReceptorRelative(OsbEasing.None, time + gap, time + gap, -adjustments1[2], ColumnType.two);

            field.MoveReceptorRelative(OsbEasing.None, time + gap, time + gap, -adjustments[3], ColumnType.four);
            field2.MoveReceptorRelative(OsbEasing.None, time + gap, time + gap, -adjustments1[3], ColumnType.one);

            field.MoveOriginRelative(OsbEasing.None, time + gap, time + gap, adjustments[0], ColumnType.one);
            field2.MoveOriginRelative(OsbEasing.None, time + gap, time + gap, adjustments1[0], ColumnType.four);

            field.MoveOriginRelative(OsbEasing.None, time + gap, time + gap, adjustments[1], ColumnType.two);
            field2.MoveOriginRelative(OsbEasing.None, time + gap, time + gap, adjustments1[1], ColumnType.three);

            field.MoveOriginRelative(OsbEasing.None, time + gap, time + gap, adjustments[2], ColumnType.three);
            field2.MoveOriginRelative(OsbEasing.None, time + gap, time + gap, adjustments1[2], ColumnType.two);

            field.MoveOriginRelative(OsbEasing.None, time + gap, time + gap, adjustments[3], ColumnType.four);
            field2.MoveOriginRelative(OsbEasing.None, time + gap, time + gap, adjustments1[3], ColumnType.one);


            // Second part - invert the pattern
            time += gap;
            adjustments[0] = field2.ScaleColumn(OsbEasing.InQuad, time, time + gap, new Vector2(0.7f), ColumnType.four, CenterTypeColumn.columnX, false, 0.65f);
            adjustments1[0] = field.ScaleColumn(OsbEasing.InQuad, time, time + gap, new Vector2(0.7f), ColumnType.one, CenterTypeColumn.columnX, false, 0.65f);

            adjustments[1] = field2.ScaleColumn(OsbEasing.InQuad, time, time + gap, new Vector2(0.6f), ColumnType.three, CenterTypeColumn.columnX, false, 0.5f);
            adjustments1[1] = field.ScaleColumn(OsbEasing.InQuad, time, time + gap, new Vector2(0.6f), ColumnType.two, CenterTypeColumn.columnX, false, 0.5f);

            adjustments[2] = field2.ScaleColumn(OsbEasing.InQuad, time, time + gap, new Vector2(0.4f), ColumnType.two, CenterTypeColumn.columnX, false, 0.5f);
            adjustments1[2] = field.ScaleColumn(OsbEasing.InQuad, time, time + gap, new Vector2(0.4f), ColumnType.three, CenterTypeColumn.columnX, false, 0.5f);

            adjustments[3] = field2.ScaleColumn(OsbEasing.InQuad, time, time + gap, new Vector2(0.3f), ColumnType.one, CenterTypeColumn.columnX, false, 0.65f);
            adjustments1[3] = field.ScaleColumn(OsbEasing.InQuad, time, time + gap, new Vector2(0.3f), ColumnType.four, CenterTypeColumn.columnX, false, 0.65f);

            field.ScaleColumn(OsbEasing.None, time + gap, time + gap, new Vector2(0.5f), ColumnType.all);
            field2.ScaleColumn(OsbEasing.None, time + gap, time + gap, new Vector2(0.5f), ColumnType.all);

            field.MoveReceptorRelative(OsbEasing.None, time + gap, time + gap, -adjustments[0], ColumnType.four);
            field2.MoveReceptorRelative(OsbEasing.None, time + gap, time + gap, -adjustments1[0], ColumnType.one);

            field.MoveReceptorRelative(OsbEasing.None, time + gap, time + gap, -adjustments[1], ColumnType.three);
            field2.MoveReceptorRelative(OsbEasing.None, time + gap, time + gap, -adjustments1[1], ColumnType.two);

            field.MoveReceptorRelative(OsbEasing.None, time + gap, time + gap, -adjustments[2], ColumnType.two);
            field2.MoveReceptorRelative(OsbEasing.None, time + gap, time + gap, -adjustments1[2], ColumnType.three);

            field.MoveReceptorRelative(OsbEasing.None, time + gap, time + gap, -adjustments[3], ColumnType.one);
            field2.MoveReceptorRelative(OsbEasing.None, time + gap, time + gap, -adjustments1[3], ColumnType.four);

            field.MoveOriginRelative(OsbEasing.None, time + gap, time + gap, adjustments[0], ColumnType.four);
            field2.MoveOriginRelative(OsbEasing.None, time + gap, time + gap, adjustments1[0], ColumnType.one);

            field.MoveOriginRelative(OsbEasing.None, time + gap, time + gap, adjustments[1], ColumnType.three);
            field2.MoveOriginRelative(OsbEasing.None, time + gap, time + gap, adjustments1[1], ColumnType.two);

            field.MoveOriginRelative(OsbEasing.None, time + gap, time + gap, adjustments[2], ColumnType.two);
            field2.MoveOriginRelative(OsbEasing.None, time + gap, time + gap, adjustments1[2], ColumnType.three);

            field.MoveOriginRelative(OsbEasing.None, time + gap, time + gap, adjustments[3], ColumnType.one);
            field2.MoveOriginRelative(OsbEasing.None, time + gap, time + gap, adjustments1[3], ColumnType.four);

            time += gap;
            // First part - applying gradual 0.05 scale increments from left to right
            adjustments[0] = field.ScaleColumn(OsbEasing.InQuad, time, time + gap, new Vector2(0.7f), ColumnType.one, CenterTypeColumn.columnX, false, 0.65f);    // +0.15 from default
            adjustments1[0] = field2.ScaleColumn(OsbEasing.InQuad, time, time + gap, new Vector2(0.7f), ColumnType.four, CenterTypeColumn.columnX, false, 0.65f);   // -0.15 from default

            adjustments[1] = field.ScaleColumn(OsbEasing.InQuad, time, time + gap, new Vector2(0.6f), ColumnType.two, CenterTypeColumn.columnX, false, 0.5f);    // +0.10 from default
            adjustments1[1] = field2.ScaleColumn(OsbEasing.InQuad, time, time + gap, new Vector2(0.6f), ColumnType.three, CenterTypeColumn.columnX, false, 0.5f);   // -0.10 from default

            adjustments[2] = field.ScaleColumn(OsbEasing.InQuad, time, time + gap, new Vector2(0.4f), ColumnType.three, CenterTypeColumn.columnX, false, 0.5f);  // +0.05 from default
            adjustments1[2] = field2.ScaleColumn(OsbEasing.InQuad, time, time + gap, new Vector2(0.4f), ColumnType.two, CenterTypeColumn.columnX, false, 0.5f); // -0.05 from default

            adjustments[3] = field.ScaleColumn(OsbEasing.InQuad, time, time + gap, new Vector2(0.3f), ColumnType.four, CenterTypeColumn.columnX, false, 0.65f);   // default
            adjustments1[3] = field2.ScaleColumn(OsbEasing.InQuad, time, time + gap, new Vector2(0.3f), ColumnType.one, CenterTypeColumn.columnX, false, 0.65f);  // default

            field.ScaleColumn(OsbEasing.None, time + gap, time + gap, new Vector2(0.5f), ColumnType.all);
            field2.ScaleColumn(OsbEasing.None, time + gap, time + gap, new Vector2(0.5f), ColumnType.all);

            field.MoveReceptorRelative(OsbEasing.None, time + gap, time + gap, -adjustments[0], ColumnType.one);
            field2.MoveReceptorRelative(OsbEasing.None, time + gap, time + gap, -adjustments1[0], ColumnType.four);

            field.MoveReceptorRelative(OsbEasing.None, time + gap, time + gap, -adjustments[1], ColumnType.two);
            field2.MoveReceptorRelative(OsbEasing.None, time + gap, time + gap, -adjustments1[1], ColumnType.three);

            field.MoveReceptorRelative(OsbEasing.None, time + gap, time + gap, -adjustments[2], ColumnType.three);
            field2.MoveReceptorRelative(OsbEasing.None, time + gap, time + gap, -adjustments1[2], ColumnType.two);

            field.MoveReceptorRelative(OsbEasing.None, time + gap, time + gap, -adjustments[3], ColumnType.four);
            field2.MoveReceptorRelative(OsbEasing.None, time + gap, time + gap, -adjustments1[3], ColumnType.one);

            field.MoveOriginRelative(OsbEasing.None, time + gap, time + gap, adjustments[0], ColumnType.one);
            field2.MoveOriginRelative(OsbEasing.None, time + gap, time + gap, adjustments1[0], ColumnType.four);

            field.MoveOriginRelative(OsbEasing.None, time + gap, time + gap, adjustments[1], ColumnType.two);
            field2.MoveOriginRelative(OsbEasing.None, time + gap, time + gap, adjustments1[1], ColumnType.three);

            field.MoveOriginRelative(OsbEasing.None, time + gap, time + gap, adjustments[2], ColumnType.three);
            field2.MoveOriginRelative(OsbEasing.None, time + gap, time + gap, adjustments1[2], ColumnType.two);

            field.MoveOriginRelative(OsbEasing.None, time + gap, time + gap, adjustments[3], ColumnType.four);
            field2.MoveOriginRelative(OsbEasing.None, time + gap, time + gap, adjustments1[3], ColumnType.one);



            field2.fadeAt(248526, 0);
            field2.fadeAt(248761, 1);

            var localtime = 249229;
            var dur = 249347 - localtime;
            var currentFade = 1;
            while (localtime < 262823)
            {
                foreach (var column in field.columns.Values)
                {
                    field.columns[column.type].receptor.renderedSprite.Fade(OsbEasing.OutExpo, localtime, localtime + dur, currentFade, Math.Abs(currentFade - 1));
                    field2.columns[column.type].receptor.renderedSprite.Fade(OsbEasing.OutExpo, localtime, localtime + dur, Math.Abs(currentFade - 1), currentFade);
                }

                field.fadeAt(localtime, localtime + dur, OsbEasing.OutExpo, Math.Abs(currentFade - 1));
                field2.fadeAt(localtime, localtime + dur, OsbEasing.OutExpo, currentFade);

                currentFade = Math.Abs(currentFade - 1);
                localtime += dur;
            }

            foreach (var column in field.columns.Values)
            {
                field.columns[column.type].receptor.renderedSprite.Fade(OsbEasing.OutExpo, localtime, localtime + dur, currentFade, 1);
                field2.columns[column.type].receptor.renderedSprite.Fade(OsbEasing.OutExpo, localtime, localtime + dur, Math.Abs(currentFade - 1), 1);
            }

            field.fadeAt(localtime, localtime + dur, OsbEasing.OutExpo, 1);
            field2.fadeAt(localtime, localtime + dur, OsbEasing.OutExpo, 1);

            var pathLayer = GetLayer("path");

            // Draw paths for first playfield
            foreach (Column column in field.columns.Values)
            {
                DrawSquareWavePath(pathLayer, 234229, 248761, column);
                DrawSquareWavePath(pathLayer, 236104, 248761, column);
            }

            DrawInstance draw = new DrawInstance(CancellationToken, field, 234229, scrollSpeed, updatesPerSecond, OsbEasing.None, false, fadeTime, fadeTime);
            draw.setReceptorMovementPrecision(0.25f);
            draw.setNoteRotationPrecision(0.1f);
            draw.setNoteMovementPrecision(0.5f);
            draw.setHoldRotationPrecision(0.01f);
            draw.setHoldMovementPrecision(0.5f);
            draw.setHoldRotationBlock(true);
            draw.drawViaEquation(endtime - 234229, NoteFunction, true);

            DrawInstance draw2 = new DrawInstance(CancellationToken, field2, 248526, scrollSpeed, updatesPerSecond, OsbEasing.None, false, fadeTime, fadeTime);
            draw2.setReceptorMovementPrecision(0.25f);
            draw2.setNoteRotationPrecision(0.1f);
            draw2.setNoteMovementPrecision(0.5f);
            draw2.setHoldRotationPrecision(0.01f);
            draw2.setHoldMovementPrecision(0.5f);
            draw2.setHoldRotationBlock(true);
            draw2.drawViaEquation(endtime - 248526, NoteFunction, true);

        }

        public void ReceptorBump(Playfield playfiled, double localtime, float diff = 2, double gap = 43801 - 43414)
        {

            if (localtime < 230479)
            {
                playfiled.RotatePlayFieldStatic(OsbEasing.None, localtime - 10, localtime - 10, Math.PI);
                playfiled.RotatePlayFieldStatic(OsbEasing.OutCubic, localtime, localtime + gap, Math.PI);
            }

            float widht = 250f;

            float defaultScale = 0.5f;
            float smallScale = defaultScale / diff;
            float bigScale = defaultScale * diff;

            var colWidth = playfiled.getColumnWidth() * 2 / 2 * diff;
            var halfwidth = playfiled.getColumnWidth() / 2 / 2 * diff;

            if (localtime >= 248761 && localtime < 263761)
            {
                colWidth *= -1;
                halfwidth *= -1;
                widht *= 2;
            }

            playfiled.ScaleReceptor(OsbEasing.None, localtime, localtime, new Vector2(bigScale), ColumnType.all);
            playfiled.ScaleOrigin(OsbEasing.None, localtime, localtime, new Vector2(smallScale), ColumnType.all);

            playfiled.ScaleReceptor(OsbEasing.OutCubic, localtime, localtime + gap, new Vector2(defaultScale), ColumnType.all);
            playfiled.ScaleOrigin(OsbEasing.OutCubic, localtime, localtime + gap, new Vector2(defaultScale), ColumnType.all);

            if (localtime < 249229)
            {
                playfiled.MoveReceptorRelative(OsbEasing.None, localtime - 10, localtime - 10, new Vector2(-colWidth, 0), ColumnType.one);
                playfiled.MoveReceptorRelative(OsbEasing.OutCubic, localtime, localtime + gap, new Vector2(colWidth, 0), ColumnType.one);
                playfiled.MoveReceptorRelative(OsbEasing.None, localtime - 10, localtime - 10, new Vector2(-halfwidth, 0), ColumnType.two);
                playfiled.MoveReceptorRelative(OsbEasing.OutCubic, localtime, localtime + gap, new Vector2(halfwidth, 0), ColumnType.two);
                playfiled.MoveReceptorRelative(OsbEasing.None, localtime - 10, localtime - 10, new Vector2(halfwidth, 0), ColumnType.three);
                playfiled.MoveReceptorRelative(OsbEasing.OutCubic, localtime, localtime + gap, new Vector2(-halfwidth, 0), ColumnType.three);
                playfiled.MoveReceptorRelative(OsbEasing.None, localtime - 10, localtime - 10, new Vector2(colWidth, 0), ColumnType.four);
                playfiled.MoveReceptorRelative(OsbEasing.OutCubic, localtime, localtime + gap, new Vector2(-colWidth, 0), ColumnType.four);

                playfiled.MoveOriginRelative(OsbEasing.None, localtime - 10, localtime + gap - 10, new Vector2(colWidth / 4, 0), ColumnType.one);
                playfiled.MoveOriginRelative(OsbEasing.OutCubic, localtime, localtime + gap, new Vector2(-colWidth / 4, 0), ColumnType.one);
                playfiled.MoveOriginRelative(OsbEasing.None, localtime - 10, localtime + gap - 10, new Vector2(halfwidth / 4, 0), ColumnType.two);
                playfiled.MoveOriginRelative(OsbEasing.OutCubic, localtime, localtime + gap, new Vector2(-halfwidth / 4, 0), ColumnType.two);
                playfiled.MoveOriginRelative(OsbEasing.None, localtime - 10, localtime + gap - 10, new Vector2(-halfwidth / 4, 0), ColumnType.three);
                playfiled.MoveOriginRelative(OsbEasing.OutCubic, localtime, localtime + gap, new Vector2(halfwidth / 4, 0), ColumnType.three);
                playfiled.MoveOriginRelative(OsbEasing.None, localtime - 10, localtime + gap - 10, new Vector2(-colWidth / 4, 0), ColumnType.four);
                playfiled.MoveOriginRelative(OsbEasing.OutCubic, localtime, localtime + gap, new Vector2(colWidth / 4, 0), ColumnType.four);

                playfiled.Resize(OsbEasing.OutCubic, localtime, localtime + gap + 180f, widht, -550f);
            }

        }

        public void DrawSquareWavePath(StoryboardLayer layer, double startTime, double endTime, Column column)
        {

            List<Vector2> points = new List<Vector2>();
            double accuracy = 1000;

            var posTime = 235167;

            if (startTime == 236104)
            {
                posTime = 237042;
            }
            ;

            var beatLenght = Beatmap.GetTimingPointAt((int)startTime).BeatDuration;

            var startPos = column.OriginPositionAt(posTime);
            var endPos = column.ReceptorPositionAt(posTime);
            startPos.X = endPos.X;

            double frequency = 2.0;            // Controls how many oscillations within the progress range (0-1)
            double amplitude = 30.0;           // Controls the maximum movement distance
            double threshold = 0.15;            // Threshold for cut-off (0-1), higher = more square-like
            double phaseOffset = 0;            // Adjust the starting position in the wave

            for (int i = 0; i < accuracy; i++)
            {
                double progress = (double)i / (accuracy - 1);

                // Normalize progress to 0-1 range within our valid range (0.2-1.0)
                // This ensures the wave pattern still works properly with the new range
                double normalizedProgress = (progress - 0.15) / 0.85;

                // Calculate sine value based on normalized progress
                double sineValue = Math.Sin((normalizedProgress + phaseOffset) * frequency * 2 * Math.PI);

                // Apply threshold to create cut-off/square-like effect
                double cutoffValue;
                if (Math.Abs(sineValue) > threshold)
                {
                    // When above threshold, snap to max amplitude with original sign
                    cutoffValue = Math.Sign(sineValue) * amplitude;
                }
                else
                {
                    // For smoother transitions between thresholds
                    // Scale the sine value to fill the range between -threshold and threshold
                    double scaleFactor = amplitude / threshold;
                    cutoffValue = sineValue * scaleFactor;
                }

                // Apply the effect based on column position for additional variety
                double columnFactor = 1.0;

                Vector2 position = Vector2.Lerp(startPos, endPos, (float)progress);
                position.X += (float)(cutoffValue * columnFactor);

                points.Add(position);
            }

            List<Vector2> condencedPositions = new List<Vector2>();

            float lastX = float.MaxValue;
            float lastY = float.MaxValue;
            bool isStraightLine = false;
            bool isFirstPoint = true;

            foreach (var point in points)
            {
                // Handle the first point
                if (isFirstPoint)
                {
                    condencedPositions.Add(point);
                    lastX = point.X;
                    lastY = point.Y;
                    isFirstPoint = false;
                    continue;
                }

                // Detect if we're on a straight line or diagonal
                bool currentIsStraightLine = (point.X == lastX);

                // If the line type changed (straight to diagonal or vice versa), add the last point
                if (currentIsStraightLine != isStraightLine)
                {
                    condencedPositions.Add(new Vector2(lastX, lastY));
                    isStraightLine = currentIsStraightLine;
                }

                // Always update the last point
                lastX = point.X;
                lastY = point.Y;
            }

            // Add the final point to complete the path
            condencedPositions.Add(points[points.Count - 1]);

            var drawPoints = condencedPositions;

            for (int i = 0; i < drawPoints.Count - 1; i++)
            {
                var currentPoint = drawPoints[i];
                var path = layer.CreateSprite("sb/white.png", OsbOrigin.CentreLeft, currentPoint);
                var nextPoint = drawPoints[i + 1];

                var angle = Math.Atan2(nextPoint.Y - currentPoint.Y, nextPoint.X - currentPoint.X);
                var distance = Math.Sqrt(Math.Pow(nextPoint.X - currentPoint.X, 2) + Math.Pow(nextPoint.Y - currentPoint.Y, 2));

                path.ScaleVec(startTime, new Vector2((float)distance, 2f));
                path.Rotate(startTime, angle);

                var gap = 236104 - 234229;
                var time = startTime;
                var currentFade = .8f;
                if (startTime == 236104)
                {
                    path.Fade(OsbEasing.InCubic, time, time + 150, 0, currentFade);
                    time += gap;

                    path.Fade(OsbEasing.OutCubic, time, time + 150, currentFade, 0);
                    time += gap;

                    path.Fade(OsbEasing.InCubic, time, time + 150, 0, currentFade);
                    time += gap;

                    path.Fade(OsbEasing.OutCubic, time, time + 150, currentFade, 0);
                    time += gap;

                    path.Fade(OsbEasing.InCubic, time, time + 150, 0, currentFade);
                    time += gap;

                    path.Fade(OsbEasing.OutCubic, time, time + 150, currentFade, 0);
                    time += gap;

                    path.Fade(OsbEasing.InCubic, time, time + 150, 0, currentFade);
                    time += gap;

                }
                else
                {
                    path.Fade(OsbEasing.InCubic, time, time + 250, 0, currentFade);
                    time += gap;

                    path.Fade(OsbEasing.OutCubic, time, time + 150, currentFade, 0);
                    time += gap;

                    path.Fade(OsbEasing.InCubic, time, time + 150, 0, currentFade);
                    time += gap;

                    path.Fade(OsbEasing.OutCubic, time, time + 150, currentFade, 0);
                    time += gap;

                    path.Fade(OsbEasing.InCubic, time, time + 150, 0, currentFade);
                    time += gap;

                    path.Fade(OsbEasing.OutCubic, time, time + 150, currentFade, 0);
                    time += gap;

                    path.Fade(OsbEasing.InCubic, time, time + 150, 0, currentFade);
                    time += gap;

                    path.Fade(OsbEasing.OutCubic, time, time + 150, currentFade, 0);
                    time += gap;
                }

                path.StartLoopGroup(startTime, (int)((endTime - startTime) / beatLenght) / 2);
                path.Color(OsbEasing.InCubic, 0, beatLenght, new Color4(255, 0, 0, 1), new Color4(0, 0, 0, 1));
                path.Color(OsbEasing.OutCubic, beatLenght, beatLenght * 2, new Color4(0, 0, 0, 1), new Color4(255, 0, 0, 1));
                path.EndGroup();

                path.Fade(endTime, 0);

            }

        }

        public Vector2 NoteFunction(EquationParameters p)
        {
            if (p.time > 234229 && p.time < 248761)
            {
                // Only apply the effect if progress is between 0.2 and 1.0
                if (p.progress >= 0.2 && p.progress <= 1.0)
                {
                    // Initialize parameters for the square-like wave
                    double frequency = 2.0;            // Controls how many oscillations within the progress range (0-1)
                    double amplitude = 30.0;           // Controls the maximum movement distance
                    double threshold = 0.15;           // Threshold for cut-off (0-1), higher = more square-like
                    double phaseOffset = 0;            // Adjust the starting position in the wave

                    // Normalize progress to 0-1 range within our valid range (0.2-1.0)
                    // This ensures the wave pattern still works properly with the new range
                    double normalizedProgress = (p.progress - 0.2) / 0.8;

                    // Calculate sine value based on normalized progress
                    double sineValue = Math.Sin((normalizedProgress + phaseOffset) * frequency * 2 * Math.PI);

                    // Apply threshold to create cut-off/square-like effect
                    double cutoffValue;
                    if (Math.Abs(sineValue) > threshold)
                    {
                        // When above threshold, snap to max amplitude with original sign
                        cutoffValue = Math.Sign(sineValue) * amplitude;
                    }
                    else
                    {
                        // For smoother transitions between thresholds
                        // Scale the sine value to fill the range between -threshold and threshold
                        double scaleFactor = amplitude / threshold;
                        cutoffValue = sineValue * scaleFactor;
                    }


                    // Apply the effect based on column position for additional variety
                    double columnFactor = 1.0;

                    return new Vector2(
                        p.position.X + (float)(cutoffValue * columnFactor),
                        p.position.Y
                    );
                }
            }

            return p.position;
        }
    }
}
