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
    public class Firstdrop : StoryboardObjectGenerator
    {
//        public override bool Multithreaded => false;
        public override void Generate()
        {

            var receptors = GetLayer("r");
            var notes = GetLayer("n");

            // General values
            var starttime = 54156; // the starttime where the playfield is initialized
            var endtime = 68963; // the endtime where the playfield is nolonger beeing rendered
            var duration = endtime - starttime; // the length the playfield is kept alive

            // Playfield Scale
            var width = 250f; // widht of the playfield / invert to flip
            var height = 400f; // height of the playfield / invert to flip -600 = downscroll | 600 = upscropll
            var receptorWallOffset = 50f; // how big the boundary box for the receptor is 50 means it will be pushed away 50 units from the wall

            // Note initilization Values
            var sliderAccuracy = 30; // The Segment length for sliderbodies since they are rendered in slices 30 is default
            var isColored = false; // This property is used if you want to color the notes by urself for effects. It does not swap if the snap coloring is used.

            // Drawinstance Values
            var updatesPerSecond = 50; // The amount of steps the rendring engine does to render out note and receptor positions
            var scrollSpeed = 800f; // The speed at which the Notes scroll
            var fadeTime = 150; // The time notes will fade in

            Playfield field = new Playfield();
            field.initilizePlayField(receptors, notes, starttime, endtime, width, height, receptorWallOffset, Beatmap.OverallDifficulty);
            field.noteEnd = 67414;
            field.initializeNotes(Beatmap.HitObjects.ToList(), Beatmap.GetTimingPointAt(starttime).Bpm, Beatmap.GetTimingPointAt(starttime).Offset, isColored, sliderAccuracy);

            Playfield field2 = new Playfield();
            field2.initilizePlayField(receptors, notes, 55705, endtime, width, -height, receptorWallOffset, Beatmap.OverallDifficulty);
            field2.noteEnd = 67414;
            field2.initializeNotes(Beatmap.HitObjects.ToList(), Beatmap.GetTimingPointAt(starttime).Bpm, Beatmap.GetTimingPointAt(starttime).Offset, isColored, sliderAccuracy);

            field.moveFieldY(OsbEasing.None, starttime, starttime, 240 - 50);
            field2.moveFieldY(OsbEasing.None, starttime, starttime, -240 + 50);

            field.MoveOriginRelative(OsbEasing.OutSine, 54253, 55801, new Vector2(0, -height), ColumnType.all);
            field.Resize(OsbEasing.OutCirc, 55801, 55801 + 350, width, height * 1.5f);
            field.moveFieldY(OsbEasing.OutCirc, 55801, 55801 + 350, -240 + 50);

            field2.MoveOriginRelative(OsbEasing.OutSine, 54253, 55801, new Vector2(0, height), ColumnType.all);
            field2.Resize(OsbEasing.OutCirc, 55801, 55801 + 350, width, -height * 1.5f);
            field2.moveFieldY(OsbEasing.OutCirc, 55801, 55801 + 350, 240 - 50);

            double localstart = 56575 - 200;
            double gap = Beatmap.GetTimingPointAt((int)localstart).BeatDuration;
            var dur = 150;
            var currHeight = height * 1.5f / 2 - 65f;
            var delay = 15;

            field.Rotate(OsbEasing.InOutSine, 56575, 57543, 0.3, CenterType.playfield);
            field.Rotate(OsbEasing.InOutSine, 57543, 58317, -0.3, CenterType.playfield);

            field2.Rotate(OsbEasing.InOutSine, 56575, 57543, 0.3, CenterType.playfield);
            field2.Rotate(OsbEasing.InOutSine, 57543, 58317, -0.3, CenterType.playfield);

            field.moveFieldX(OsbEasing.InOutSine, 56575, 57543, -50);
            field2.moveFieldX(OsbEasing.InOutSine, 56575, 57543, 50);

            field.moveFieldX(OsbEasing.InOutSine, 57543, 58317, 50);
            field2.moveFieldX(OsbEasing.InOutSine, 57543, 58317, -50);

            while (localstart < 58317 - 200 - dur)
            {
                field.MoveReceptorRelative(OsbEasing.InCirc, localstart, localstart + dur, new Vector2(0, currHeight), ColumnType.one);
                field.MoveReceptorRelative(OsbEasing.InCirc, localstart + delay, localstart + delay + dur, new Vector2(0, currHeight), ColumnType.two);
                field.MoveReceptorRelative(OsbEasing.InCirc, localstart + delay * 2, localstart + delay * 2 + dur, new Vector2(0, currHeight), ColumnType.three);
                field.MoveReceptorRelative(OsbEasing.InCirc, localstart + delay * 3, localstart + delay * 3 + dur, new Vector2(0, currHeight), ColumnType.four);

                field.MoveColumnRelativeX(OsbEasing.InCirc, localstart, localstart + dur, field.getColumnWidth(), ColumnType.one);
                field.MoveColumnRelativeX(OsbEasing.InCirc, localstart, localstart + dur, field.getColumnWidth() / 2, ColumnType.two);
                field.MoveColumnRelativeX(OsbEasing.InCirc, localstart, localstart + dur, -field.getColumnWidth() / 2, ColumnType.three);
                field.MoveColumnRelativeX(OsbEasing.InCirc, localstart, localstart + dur, -field.getColumnWidth(), ColumnType.four);

                field2.MoveColumnRelativeX(OsbEasing.InCirc, localstart, localstart + dur, field.getColumnWidth(), ColumnType.one);
                field2.MoveColumnRelativeX(OsbEasing.InCirc, localstart, localstart + dur, field.getColumnWidth() / 2, ColumnType.two);
                field2.MoveColumnRelativeX(OsbEasing.InCirc, localstart, localstart + dur, -field.getColumnWidth() / 2, ColumnType.three);
                field2.MoveColumnRelativeX(OsbEasing.InCirc, localstart, localstart + dur, -field.getColumnWidth(), ColumnType.four);

                field2.MoveReceptorRelative(OsbEasing.InCirc, localstart, localstart + dur, new Vector2(0, -currHeight), ColumnType.four);
                field2.MoveReceptorRelative(OsbEasing.InCirc, localstart + delay, localstart + delay + dur, new Vector2(0, -currHeight), ColumnType.three);
                field2.MoveReceptorRelative(OsbEasing.InCirc, localstart + delay * 2, localstart + delay * 2 + dur, new Vector2(0, -currHeight), ColumnType.two);
                field2.MoveReceptorRelative(OsbEasing.InCirc, localstart + delay * 3, localstart + delay * 3 + dur, new Vector2(0, -currHeight), ColumnType.one);

                localstart += dur;

                field.MoveReceptorRelative(OsbEasing.OutCirc, localstart, localstart + dur, new Vector2(0, -currHeight), ColumnType.one);
                field.MoveReceptorRelative(OsbEasing.OutCirc, localstart + delay, localstart + delay + dur, new Vector2(0, -currHeight), ColumnType.two);
                field.MoveReceptorRelative(OsbEasing.OutCirc, localstart + delay * 2, localstart + delay * 2 + dur, new Vector2(0, -currHeight), ColumnType.three);
                field.MoveReceptorRelative(OsbEasing.OutCirc, localstart + delay * 3, localstart + delay * 3 + dur, new Vector2(0, -currHeight), ColumnType.four);

                field.MoveColumnRelativeX(OsbEasing.OutCirc, localstart, localstart + dur, field.getColumnWidth() * 2, ColumnType.one);
                field.MoveColumnRelativeX(OsbEasing.OutCirc, localstart, localstart + dur, field.getColumnWidth() / 2, ColumnType.two);
                field.MoveColumnRelativeX(OsbEasing.OutCirc, localstart, localstart + dur, -field.getColumnWidth() / 2, ColumnType.three);
                field.MoveColumnRelativeX(OsbEasing.OutCirc, localstart, localstart + dur, -field.getColumnWidth() * 2, ColumnType.four);

                field2.MoveColumnRelativeX(OsbEasing.OutCirc, localstart, localstart + dur, field.getColumnWidth() * 2, ColumnType.one);
                field2.MoveColumnRelativeX(OsbEasing.OutCirc, localstart, localstart + dur, field.getColumnWidth() / 2, ColumnType.two);
                field2.MoveColumnRelativeX(OsbEasing.OutCirc, localstart, localstart + dur, -field.getColumnWidth() / 2, ColumnType.three);
                field2.MoveColumnRelativeX(OsbEasing.OutCirc, localstart, localstart + dur, -field.getColumnWidth() * 2, ColumnType.four);

                field2.MoveReceptorRelative(OsbEasing.OutCirc, localstart, localstart + dur, new Vector2(0, currHeight), ColumnType.four);
                field2.MoveReceptorRelative(OsbEasing.OutCirc, localstart + delay, localstart + delay + dur, new Vector2(0, currHeight), ColumnType.three);
                field2.MoveReceptorRelative(OsbEasing.OutCirc, localstart + delay * 2, localstart + delay * 2 + dur, new Vector2(0, currHeight), ColumnType.two);
                field2.MoveReceptorRelative(OsbEasing.OutCirc, localstart + delay * 3, localstart + delay * 3 + dur, new Vector2(0, currHeight), ColumnType.one);

                localstart -= dur;
                localstart += gap;

                if (localstart > 58132)
                {
                    break;
                }

                field.MoveReceptorRelative(OsbEasing.InCirc, localstart, localstart + dur, new Vector2(0, currHeight), ColumnType.one);
                field.MoveReceptorRelative(OsbEasing.InCirc, localstart + delay, localstart + delay + dur, new Vector2(0, currHeight), ColumnType.two);
                field.MoveReceptorRelative(OsbEasing.InCirc, localstart + delay * 2, localstart + delay * 2 + dur, new Vector2(0, currHeight), ColumnType.three);
                field.MoveReceptorRelative(OsbEasing.InCirc, localstart + delay * 3, localstart + delay * 3 + dur, new Vector2(0, currHeight), ColumnType.four);

                field.MoveColumnRelativeX(OsbEasing.InCirc, localstart, localstart + dur, -field.getColumnWidth(), ColumnType.one);
                field.MoveColumnRelativeX(OsbEasing.InCirc, localstart, localstart + dur, -field.getColumnWidth() / 2, ColumnType.two);
                field.MoveColumnRelativeX(OsbEasing.InCirc, localstart, localstart + dur, field.getColumnWidth() / 2, ColumnType.three);
                field.MoveColumnRelativeX(OsbEasing.InCirc, localstart, localstart + dur, field.getColumnWidth(), ColumnType.four);

                field2.MoveColumnRelativeX(OsbEasing.InCirc, localstart, localstart + dur, -field.getColumnWidth(), ColumnType.one);
                field2.MoveColumnRelativeX(OsbEasing.InCirc, localstart, localstart + dur, -field.getColumnWidth() / 2, ColumnType.two);
                field2.MoveColumnRelativeX(OsbEasing.InCirc, localstart, localstart + dur, field.getColumnWidth() / 2, ColumnType.three);
                field2.MoveColumnRelativeX(OsbEasing.InCirc, localstart, localstart + dur, field.getColumnWidth(), ColumnType.four);


                field2.MoveReceptorRelative(OsbEasing.InCirc, localstart, localstart + dur, new Vector2(0, -currHeight), ColumnType.four);
                field2.MoveReceptorRelative(OsbEasing.InCirc, localstart + delay, localstart + delay + dur, new Vector2(0, -currHeight), ColumnType.three);
                field2.MoveReceptorRelative(OsbEasing.InCirc, localstart + delay * 2, localstart + delay * 2 + dur, new Vector2(0, -currHeight), ColumnType.two);
                field2.MoveReceptorRelative(OsbEasing.InCirc, localstart + delay * 3, localstart + delay * 3 + dur, new Vector2(0, -currHeight), ColumnType.one);

                localstart += dur;

                field.MoveReceptorRelative(OsbEasing.OutCirc, localstart, localstart + dur, new Vector2(0, -currHeight), ColumnType.one);
                field.MoveReceptorRelative(OsbEasing.OutCirc, localstart + delay, localstart + delay + dur, new Vector2(0, -currHeight), ColumnType.two);
                field.MoveReceptorRelative(OsbEasing.OutCirc, localstart + delay * 2, localstart + delay * 2 + dur, new Vector2(0, -currHeight), ColumnType.three);
                field.MoveReceptorRelative(OsbEasing.OutCirc, localstart + delay * 3, localstart + delay * 3 + dur, new Vector2(0, -currHeight), ColumnType.four);

                field.MoveColumnRelativeX(OsbEasing.OutCirc, localstart, localstart + dur, -field.getColumnWidth() * 2, ColumnType.one);
                field.MoveColumnRelativeX(OsbEasing.OutCirc, localstart, localstart + dur, -field.getColumnWidth() / 2, ColumnType.two);
                field.MoveColumnRelativeX(OsbEasing.OutCirc, localstart, localstart + dur, field.getColumnWidth() / 2, ColumnType.three);
                field.MoveColumnRelativeX(OsbEasing.OutCirc, localstart, localstart + dur, field.getColumnWidth() * 2, ColumnType.four);

                field2.MoveColumnRelativeX(OsbEasing.OutCirc, localstart, localstart + dur, -field.getColumnWidth() * 2, ColumnType.one);
                field2.MoveColumnRelativeX(OsbEasing.OutCirc, localstart, localstart + dur, -field.getColumnWidth() / 2, ColumnType.two);
                field2.MoveColumnRelativeX(OsbEasing.OutCirc, localstart, localstart + dur, field.getColumnWidth() / 2, ColumnType.three);
                field2.MoveColumnRelativeX(OsbEasing.OutCirc, localstart, localstart + dur, field.getColumnWidth() * 2, ColumnType.four);

                field2.MoveReceptorRelative(OsbEasing.OutCirc, localstart, localstart + dur, new Vector2(0, currHeight), ColumnType.four);
                field2.MoveReceptorRelative(OsbEasing.OutCirc, localstart + delay, localstart + delay + dur, new Vector2(0, currHeight), ColumnType.three);
                field2.MoveReceptorRelative(OsbEasing.OutCirc, localstart + delay * 2, localstart + delay * 2 + dur, new Vector2(0, currHeight), ColumnType.two);
                field2.MoveReceptorRelative(OsbEasing.OutCirc, localstart + delay * 3, localstart + delay * 3 + dur, new Vector2(0, currHeight), ColumnType.one);

                localstart -= dur;
                localstart += gap;
            }



            field.MoveColumnRelativeX(OsbEasing.OutSine, 58317, localstart + gap, -field.getColumnWidth() * 3, ColumnType.one);
            field.MoveColumnRelativeX(OsbEasing.OutSine, 58317, localstart + gap, -field.getColumnWidth(), ColumnType.two);
            field.MoveColumnRelativeX(OsbEasing.OutSine, 58317, localstart + gap, field.getColumnWidth(), ColumnType.three);
            field.MoveColumnRelativeX(OsbEasing.OutSine, 58317, localstart + gap, field.getColumnWidth() * 3, ColumnType.four);

            field2.MoveColumnRelativeX(OsbEasing.OutSine, 58317, localstart + gap, -field.getColumnWidth() * 3, ColumnType.one);
            field2.MoveColumnRelativeX(OsbEasing.OutSine, 58317, localstart + gap, -field.getColumnWidth(), ColumnType.two);
            field2.MoveColumnRelativeX(OsbEasing.OutSine, 58317, localstart + gap, field.getColumnWidth(), ColumnType.three);
            field2.MoveColumnRelativeX(OsbEasing.OutSine, 58317, localstart + gap, field.getColumnWidth() * 3, ColumnType.four);

            field.Rotate(OsbEasing.InOutSine, 58898, 60059, -.2, CenterType.playfield);
            field.Rotate(OsbEasing.InOutSine, 60059, 61221, .2, CenterType.playfield);

            field2.Rotate(OsbEasing.InOutSine, 58898, 60059, -.2, CenterType.playfield);
            field2.Rotate(OsbEasing.InOutSine, 60059, 61221, .2, CenterType.playfield);

            field.moveFieldX(OsbEasing.InOutSine, 58898, 60059, -75);
            field2.moveFieldX(OsbEasing.InOutSine, 58898, 60059, 75);

            field.moveFieldX(OsbEasing.InOutSine, 60059, 61221, 75);
            field2.moveFieldX(OsbEasing.InOutSine, 60059, 61221, -75);

            localstart += gap;
            while (localstart < 61221 - 200)
            {
                field.MoveReceptorRelative(OsbEasing.InCirc, localstart, localstart + dur, new Vector2(0, currHeight), ColumnType.one);
                field.MoveReceptorRelative(OsbEasing.InCirc, localstart + delay, localstart + delay + dur, new Vector2(0, currHeight), ColumnType.two);
                field.MoveReceptorRelative(OsbEasing.InCirc, localstart + delay * 2, localstart + delay * 2 + dur, new Vector2(0, currHeight), ColumnType.three);
                field.MoveReceptorRelative(OsbEasing.InCirc, localstart + delay * 3, localstart + delay * 3 + dur, new Vector2(0, currHeight), ColumnType.four);

                field.MoveColumnRelativeX(OsbEasing.InCirc, localstart, localstart + dur, field.getColumnWidth(), ColumnType.one);
                field.MoveColumnRelativeX(OsbEasing.InCirc, localstart, localstart + dur, field.getColumnWidth() / 2, ColumnType.two);
                field.MoveColumnRelativeX(OsbEasing.InCirc, localstart, localstart + dur, -field.getColumnWidth() / 2, ColumnType.three);
                field.MoveColumnRelativeX(OsbEasing.InCirc, localstart, localstart + dur, -field.getColumnWidth(), ColumnType.four);

                field2.MoveColumnRelativeX(OsbEasing.InCirc, localstart, localstart + dur, field.getColumnWidth(), ColumnType.one);
                field2.MoveColumnRelativeX(OsbEasing.InCirc, localstart, localstart + dur, field.getColumnWidth() / 2, ColumnType.two);
                field2.MoveColumnRelativeX(OsbEasing.InCirc, localstart, localstart + dur, -field.getColumnWidth() / 2, ColumnType.three);
                field2.MoveColumnRelativeX(OsbEasing.InCirc, localstart, localstart + dur, -field.getColumnWidth(), ColumnType.four);

                field2.MoveReceptorRelative(OsbEasing.InCirc, localstart, localstart + dur, new Vector2(0, -currHeight), ColumnType.four);
                field2.MoveReceptorRelative(OsbEasing.InCirc, localstart + delay, localstart + delay + dur, new Vector2(0, -currHeight), ColumnType.three);
                field2.MoveReceptorRelative(OsbEasing.InCirc, localstart + delay * 2, localstart + delay * 2 + dur, new Vector2(0, -currHeight), ColumnType.two);
                field2.MoveReceptorRelative(OsbEasing.InCirc, localstart + delay * 3, localstart + delay * 3 + dur, new Vector2(0, -currHeight), ColumnType.one);

                localstart += dur;

                field.MoveReceptorRelative(OsbEasing.OutCirc, localstart, localstart + dur, new Vector2(0, -currHeight), ColumnType.one);
                field.MoveReceptorRelative(OsbEasing.OutCirc, localstart + delay, localstart + delay + dur, new Vector2(0, -currHeight), ColumnType.two);
                field.MoveReceptorRelative(OsbEasing.OutCirc, localstart + delay * 2, localstart + delay * 2 + dur, new Vector2(0, -currHeight), ColumnType.three);
                field.MoveReceptorRelative(OsbEasing.OutCirc, localstart + delay * 3, localstart + delay * 3 + dur, new Vector2(0, -currHeight), ColumnType.four);

                field.MoveColumnRelativeX(OsbEasing.OutCirc, localstart, localstart + dur, field.getColumnWidth() * 2, ColumnType.one);
                field.MoveColumnRelativeX(OsbEasing.OutCirc, localstart, localstart + dur, field.getColumnWidth() / 2, ColumnType.two);
                field.MoveColumnRelativeX(OsbEasing.OutCirc, localstart, localstart + dur, -field.getColumnWidth() / 2, ColumnType.three);
                field.MoveColumnRelativeX(OsbEasing.OutCirc, localstart, localstart + dur, -field.getColumnWidth() * 2, ColumnType.four);

                field2.MoveColumnRelativeX(OsbEasing.OutCirc, localstart, localstart + dur, field.getColumnWidth() * 2, ColumnType.one);
                field2.MoveColumnRelativeX(OsbEasing.OutCirc, localstart, localstart + dur, field.getColumnWidth() / 2, ColumnType.two);
                field2.MoveColumnRelativeX(OsbEasing.OutCirc, localstart, localstart + dur, -field.getColumnWidth() / 2, ColumnType.three);
                field2.MoveColumnRelativeX(OsbEasing.OutCirc, localstart, localstart + dur, -field.getColumnWidth() * 2, ColumnType.four);

                field2.MoveReceptorRelative(OsbEasing.OutCirc, localstart, localstart + dur, new Vector2(0, currHeight), ColumnType.four);
                field2.MoveReceptorRelative(OsbEasing.OutCirc, localstart + delay, localstart + delay + dur, new Vector2(0, currHeight), ColumnType.three);
                field2.MoveReceptorRelative(OsbEasing.OutCirc, localstart + delay * 2, localstart + delay * 2 + dur, new Vector2(0, currHeight), ColumnType.two);
                field2.MoveReceptorRelative(OsbEasing.OutCirc, localstart + delay * 3, localstart + delay * 3 + dur, new Vector2(0, currHeight), ColumnType.one);

                localstart -= dur;
                localstart += gap;

                if (localstart > 61221)
                {
                    break;
                }

                field.MoveReceptorRelative(OsbEasing.InCirc, localstart, localstart + dur, new Vector2(0, currHeight), ColumnType.one);
                field.MoveReceptorRelative(OsbEasing.InCirc, localstart + delay, localstart + delay + dur, new Vector2(0, currHeight), ColumnType.two);
                field.MoveReceptorRelative(OsbEasing.InCirc, localstart + delay * 2, localstart + delay * 2 + dur, new Vector2(0, currHeight), ColumnType.three);
                field.MoveReceptorRelative(OsbEasing.InCirc, localstart + delay * 3, localstart + delay * 3 + dur, new Vector2(0, currHeight), ColumnType.four);

                field.MoveColumnRelativeX(OsbEasing.InCirc, localstart, localstart + dur, -field.getColumnWidth(), ColumnType.one);
                field.MoveColumnRelativeX(OsbEasing.InCirc, localstart, localstart + dur, -field.getColumnWidth() / 2, ColumnType.two);
                field.MoveColumnRelativeX(OsbEasing.InCirc, localstart, localstart + dur, field.getColumnWidth() / 2, ColumnType.three);
                field.MoveColumnRelativeX(OsbEasing.InCirc, localstart, localstart + dur, field.getColumnWidth(), ColumnType.four);

                field2.MoveColumnRelativeX(OsbEasing.InCirc, localstart, localstart + dur, -field.getColumnWidth(), ColumnType.one);
                field2.MoveColumnRelativeX(OsbEasing.InCirc, localstart, localstart + dur, -field.getColumnWidth() / 2, ColumnType.two);
                field2.MoveColumnRelativeX(OsbEasing.InCirc, localstart, localstart + dur, field.getColumnWidth() / 2, ColumnType.three);
                field2.MoveColumnRelativeX(OsbEasing.InCirc, localstart, localstart + dur, field.getColumnWidth(), ColumnType.four);


                field2.MoveReceptorRelative(OsbEasing.InCirc, localstart, localstart + dur, new Vector2(0, -currHeight), ColumnType.four);
                field2.MoveReceptorRelative(OsbEasing.InCirc, localstart + delay, localstart + delay + dur, new Vector2(0, -currHeight), ColumnType.three);
                field2.MoveReceptorRelative(OsbEasing.InCirc, localstart + delay * 2, localstart + delay * 2 + dur, new Vector2(0, -currHeight), ColumnType.two);
                field2.MoveReceptorRelative(OsbEasing.InCirc, localstart + delay * 3, localstart + delay * 3 + dur, new Vector2(0, -currHeight), ColumnType.one);

                localstart += dur;

                field.MoveReceptorRelative(OsbEasing.OutCirc, localstart, localstart + dur, new Vector2(0, -currHeight), ColumnType.one);
                field.MoveReceptorRelative(OsbEasing.OutCirc, localstart + delay, localstart + delay + dur, new Vector2(0, -currHeight), ColumnType.two);
                field.MoveReceptorRelative(OsbEasing.OutCirc, localstart + delay * 2, localstart + delay * 2 + dur, new Vector2(0, -currHeight), ColumnType.three);
                field.MoveReceptorRelative(OsbEasing.OutCirc, localstart + delay * 3, localstart + delay * 3 + dur, new Vector2(0, -currHeight), ColumnType.four);

                field.MoveColumnRelativeX(OsbEasing.OutCirc, localstart, localstart + dur, -field.getColumnWidth() * 2, ColumnType.one);
                field.MoveColumnRelativeX(OsbEasing.OutCirc, localstart, localstart + dur, -field.getColumnWidth() / 2, ColumnType.two);
                field.MoveColumnRelativeX(OsbEasing.OutCirc, localstart, localstart + dur, field.getColumnWidth() / 2, ColumnType.three);
                field.MoveColumnRelativeX(OsbEasing.OutCirc, localstart, localstart + dur, field.getColumnWidth() * 2, ColumnType.four);

                field2.MoveColumnRelativeX(OsbEasing.OutCirc, localstart, localstart + dur, -field.getColumnWidth() * 2, ColumnType.one);
                field2.MoveColumnRelativeX(OsbEasing.OutCirc, localstart, localstart + dur, -field.getColumnWidth() / 2, ColumnType.two);
                field2.MoveColumnRelativeX(OsbEasing.OutCirc, localstart, localstart + dur, field.getColumnWidth() / 2, ColumnType.three);
                field2.MoveColumnRelativeX(OsbEasing.OutCirc, localstart, localstart + dur, field.getColumnWidth() * 2, ColumnType.four);

                field2.MoveReceptorRelative(OsbEasing.OutCirc, localstart, localstart + dur, new Vector2(0, currHeight), ColumnType.four);
                field2.MoveReceptorRelative(OsbEasing.OutCirc, localstart + delay, localstart + delay + dur, new Vector2(0, currHeight), ColumnType.three);
                field2.MoveReceptorRelative(OsbEasing.OutCirc, localstart + delay * 2, localstart + delay * 2 + dur, new Vector2(0, currHeight), ColumnType.two);
                field2.MoveReceptorRelative(OsbEasing.OutCirc, localstart + delay * 3, localstart + delay * 3 + dur, new Vector2(0, currHeight), ColumnType.one);

                localstart -= dur;
                localstart += gap;
            }

            field.MoveColumnRelativeX(OsbEasing.InCirc, 61221, 61221, -field.getColumnWidth(), ColumnType.one);
            field.MoveColumnRelativeX(OsbEasing.InCirc, 61221, 61221, field.getColumnWidth(), ColumnType.two);
            field.MoveColumnRelativeX(OsbEasing.InCirc, 61221, 61221, -field.getColumnWidth(), ColumnType.three);
            field.MoveColumnRelativeX(OsbEasing.InCirc, 61221, 61221, field.getColumnWidth(), ColumnType.four);

            field.MoveColumnRelativeX(OsbEasing.InCirc, 61253, 61253, field.getColumnWidth(), ColumnType.one);
            field.MoveColumnRelativeX(OsbEasing.InCirc, 61253, 61253, -field.getColumnWidth(), ColumnType.two);
            field.MoveColumnRelativeX(OsbEasing.InCirc, 61253, 61253, field.getColumnWidth(), ColumnType.three);
            field.MoveColumnRelativeX(OsbEasing.InCirc, 61253, 61253, -field.getColumnWidth(), ColumnType.four);

            field.MoveColumnRelativeX(OsbEasing.InCirc, 61285, 61285, -field.getColumnWidth(), ColumnType.one);
            field.MoveColumnRelativeX(OsbEasing.InCirc, 61285, 61285, field.getColumnWidth(), ColumnType.two);
            field.MoveColumnRelativeX(OsbEasing.InCirc, 61285, 61285, -field.getColumnWidth(), ColumnType.three);
            field.MoveColumnRelativeX(OsbEasing.InCirc, 61285, 61285, field.getColumnWidth(), ColumnType.four);

            field.MoveColumnRelativeX(OsbEasing.InCirc, 61317, 61317, field.getColumnWidth(), ColumnType.one);
            field.MoveColumnRelativeX(OsbEasing.InCirc, 61317, 61317, -field.getColumnWidth(), ColumnType.two);
            field.MoveColumnRelativeX(OsbEasing.InCirc, 61317, 61317, field.getColumnWidth(), ColumnType.three);
            field.MoveColumnRelativeX(OsbEasing.InCirc, 61317, 61317, -field.getColumnWidth(), ColumnType.four);

            field.MoveColumnRelativeX(OsbEasing.InCirc, 61350, 61350, -field.getColumnWidth(), ColumnType.one);
            field.MoveColumnRelativeX(OsbEasing.InCirc, 61350, 61350, field.getColumnWidth(), ColumnType.two);
            field.MoveColumnRelativeX(OsbEasing.InCirc, 61350, 61350, -field.getColumnWidth(), ColumnType.three);
            field.MoveColumnRelativeX(OsbEasing.InCirc, 61350, 61350, field.getColumnWidth(), ColumnType.four);

            field.MoveColumnRelativeX(OsbEasing.InCirc, 61414, 61414, field.getColumnWidth(), ColumnType.one);
            field.MoveColumnRelativeX(OsbEasing.InCirc, 61414, 61414, -field.getColumnWidth(), ColumnType.two);
            field.MoveColumnRelativeX(OsbEasing.InCirc, 61414, 61414, field.getColumnWidth(), ColumnType.three);
            field.MoveColumnRelativeX(OsbEasing.InCirc, 61414, 61414, -field.getColumnWidth(), ColumnType.four);

            field2.MoveColumnRelativeX(OsbEasing.InCirc, 61221, 61221, -field.getColumnWidth(), ColumnType.one);
            field2.MoveColumnRelativeX(OsbEasing.InCirc, 61221, 61221, field.getColumnWidth(), ColumnType.two);
            field2.MoveColumnRelativeX(OsbEasing.InCirc, 61221, 61221, -field.getColumnWidth(), ColumnType.three);
            field2.MoveColumnRelativeX(OsbEasing.InCirc, 61221, 61221, field.getColumnWidth(), ColumnType.four);

            field2.MoveColumnRelativeX(OsbEasing.InCirc, 61253, 61253, field.getColumnWidth(), ColumnType.one);
            field2.MoveColumnRelativeX(OsbEasing.InCirc, 61253, 61253, -field.getColumnWidth(), ColumnType.two);
            field2.MoveColumnRelativeX(OsbEasing.InCirc, 61253, 61253, field.getColumnWidth(), ColumnType.three);
            field2.MoveColumnRelativeX(OsbEasing.InCirc, 61253, 61253, -field.getColumnWidth(), ColumnType.four);

            field2.MoveColumnRelativeX(OsbEasing.InCirc, 61285, 61285, -field.getColumnWidth(), ColumnType.one);
            field2.MoveColumnRelativeX(OsbEasing.InCirc, 61285, 61285, field.getColumnWidth(), ColumnType.two);
            field2.MoveColumnRelativeX(OsbEasing.InCirc, 61285, 61285, -field.getColumnWidth(), ColumnType.three);
            field2.MoveColumnRelativeX(OsbEasing.InCirc, 61285, 61285, field.getColumnWidth(), ColumnType.four);

            field2.MoveColumnRelativeX(OsbEasing.InCirc, 61317, 61317, field.getColumnWidth(), ColumnType.one);
            field2.MoveColumnRelativeX(OsbEasing.InCirc, 61317, 61317, -field.getColumnWidth(), ColumnType.two);
            field2.MoveColumnRelativeX(OsbEasing.InCirc, 61317, 61317, field.getColumnWidth(), ColumnType.three);
            field2.MoveColumnRelativeX(OsbEasing.InCirc, 61317, 61317, -field.getColumnWidth(), ColumnType.four);

            field2.MoveColumnRelativeX(OsbEasing.InCirc, 61350, 61350, -field.getColumnWidth(), ColumnType.one);
            field2.MoveColumnRelativeX(OsbEasing.InCirc, 61350, 61350, field.getColumnWidth(), ColumnType.two);
            field2.MoveColumnRelativeX(OsbEasing.InCirc, 61350, 61350, -field.getColumnWidth(), ColumnType.three);
            field2.MoveColumnRelativeX(OsbEasing.InCirc, 61350, 61350, field.getColumnWidth(), ColumnType.four);

            field2.MoveColumnRelativeX(OsbEasing.InCirc, 61414, 61414, field.getColumnWidth(), ColumnType.one);
            field2.MoveColumnRelativeX(OsbEasing.InCirc, 61414, 61414, -field.getColumnWidth(), ColumnType.two);
            field2.MoveColumnRelativeX(OsbEasing.InCirc, 61414, 61414, field.getColumnWidth(), ColumnType.three);
            field2.MoveColumnRelativeX(OsbEasing.InCirc, 61414, 61414, -field.getColumnWidth(), ColumnType.four);

            field.MoveColumnRelativeX(OsbEasing.OutElasticHalf, 61511, 61995, -field.getColumnWidth() * 3, ColumnType.one);
            field.MoveColumnRelativeX(OsbEasing.OutElasticHalf, 61511, 61995, -field.getColumnWidth(), ColumnType.two);
            field.MoveColumnRelativeX(OsbEasing.OutElasticHalf, 61511, 61995, field.getColumnWidth(), ColumnType.three);
            field.MoveColumnRelativeX(OsbEasing.OutElasticHalf, 61511, 61995, field.getColumnWidth() * 3, ColumnType.four);

            field2.MoveColumnRelativeX(OsbEasing.OutElasticHalf, 61511, 61995, -field.getColumnWidth() * 3, ColumnType.one);
            field2.MoveColumnRelativeX(OsbEasing.OutElasticHalf, 61511, 61995, -field.getColumnWidth(), ColumnType.two);
            field2.MoveColumnRelativeX(OsbEasing.OutElasticHalf, 61511, 61995, field.getColumnWidth(), ColumnType.three);
            field2.MoveColumnRelativeX(OsbEasing.OutElasticHalf, 61511, 61995, field.getColumnWidth() * 3, ColumnType.four);


            field.Rotate(OsbEasing.InOutSine, 61995, 63156, -.4, CenterType.playfield);
            field.Rotate(OsbEasing.InOutSine, 63156, 65866, .4, CenterType.playfield);

            field2.Rotate(OsbEasing.InOutSine, 61995, 63156, -.4, CenterType.playfield);
            field2.Rotate(OsbEasing.InOutSine, 63156, 65866, .4, CenterType.playfield);

            field.moveFieldX(OsbEasing.InOutSine, 61995, 63156, 100);
            field2.moveFieldX(OsbEasing.InOutSine, 61995, 63156, -100);

            field.moveFieldX(OsbEasing.InOutSine, 63156, 65866, -100);
            field2.moveFieldX(OsbEasing.InOutSine, 63156, 65866, 100);

            localstart += gap;
            while (localstart < 66253 - 200)
            {
                field.MoveReceptorRelative(OsbEasing.InCirc, localstart, localstart + dur, new Vector2(0, currHeight), ColumnType.one);
                field.MoveReceptorRelative(OsbEasing.InCirc, localstart + delay, localstart + delay + dur, new Vector2(0, currHeight), ColumnType.two);
                field.MoveReceptorRelative(OsbEasing.InCirc, localstart + delay * 2, localstart + delay * 2 + dur, new Vector2(0, currHeight), ColumnType.three);
                field.MoveReceptorRelative(OsbEasing.InCirc, localstart + delay * 3, localstart + delay * 3 + dur, new Vector2(0, currHeight), ColumnType.four);

                field.MoveColumnRelativeX(OsbEasing.InCirc, localstart, localstart + dur, field.getColumnWidth(), ColumnType.one);
                field.MoveColumnRelativeX(OsbEasing.InCirc, localstart, localstart + dur, field.getColumnWidth() / 2, ColumnType.two);
                field.MoveColumnRelativeX(OsbEasing.InCirc, localstart, localstart + dur, -field.getColumnWidth() / 2, ColumnType.three);
                field.MoveColumnRelativeX(OsbEasing.InCirc, localstart, localstart + dur, -field.getColumnWidth(), ColumnType.four);

                field2.MoveColumnRelativeX(OsbEasing.InCirc, localstart, localstart + dur, field.getColumnWidth(), ColumnType.one);
                field2.MoveColumnRelativeX(OsbEasing.InCirc, localstart, localstart + dur, field.getColumnWidth() / 2, ColumnType.two);
                field2.MoveColumnRelativeX(OsbEasing.InCirc, localstart, localstart + dur, -field.getColumnWidth() / 2, ColumnType.three);
                field2.MoveColumnRelativeX(OsbEasing.InCirc, localstart, localstart + dur, -field.getColumnWidth(), ColumnType.four);

                field2.MoveReceptorRelative(OsbEasing.InCirc, localstart, localstart + dur, new Vector2(0, -currHeight), ColumnType.four);
                field2.MoveReceptorRelative(OsbEasing.InCirc, localstart + delay, localstart + delay + dur, new Vector2(0, -currHeight), ColumnType.three);
                field2.MoveReceptorRelative(OsbEasing.InCirc, localstart + delay * 2, localstart + delay * 2 + dur, new Vector2(0, -currHeight), ColumnType.two);
                field2.MoveReceptorRelative(OsbEasing.InCirc, localstart + delay * 3, localstart + delay * 3 + dur, new Vector2(0, -currHeight), ColumnType.one);

                localstart += dur;

                field.MoveReceptorRelative(OsbEasing.OutCirc, localstart, localstart + dur, new Vector2(0, -currHeight), ColumnType.one);
                field.MoveReceptorRelative(OsbEasing.OutCirc, localstart + delay, localstart + delay + dur, new Vector2(0, -currHeight), ColumnType.two);
                field.MoveReceptorRelative(OsbEasing.OutCirc, localstart + delay * 2, localstart + delay * 2 + dur, new Vector2(0, -currHeight), ColumnType.three);
                field.MoveReceptorRelative(OsbEasing.OutCirc, localstart + delay * 3, localstart + delay * 3 + dur, new Vector2(0, -currHeight), ColumnType.four);

                field.MoveColumnRelativeX(OsbEasing.OutCirc, localstart, localstart + dur, field.getColumnWidth() * 2, ColumnType.one);
                field.MoveColumnRelativeX(OsbEasing.OutCirc, localstart, localstart + dur, field.getColumnWidth() / 2, ColumnType.two);
                field.MoveColumnRelativeX(OsbEasing.OutCirc, localstart, localstart + dur, -field.getColumnWidth() / 2, ColumnType.three);
                field.MoveColumnRelativeX(OsbEasing.OutCirc, localstart, localstart + dur, -field.getColumnWidth() * 2, ColumnType.four);

                field2.MoveColumnRelativeX(OsbEasing.OutCirc, localstart, localstart + dur, field.getColumnWidth() * 2, ColumnType.one);
                field2.MoveColumnRelativeX(OsbEasing.OutCirc, localstart, localstart + dur, field.getColumnWidth() / 2, ColumnType.two);
                field2.MoveColumnRelativeX(OsbEasing.OutCirc, localstart, localstart + dur, -field.getColumnWidth() / 2, ColumnType.three);
                field2.MoveColumnRelativeX(OsbEasing.OutCirc, localstart, localstart + dur, -field.getColumnWidth() * 2, ColumnType.four);

                field2.MoveReceptorRelative(OsbEasing.OutCirc, localstart, localstart + dur, new Vector2(0, currHeight), ColumnType.four);
                field2.MoveReceptorRelative(OsbEasing.OutCirc, localstart + delay, localstart + delay + dur, new Vector2(0, currHeight), ColumnType.three);
                field2.MoveReceptorRelative(OsbEasing.OutCirc, localstart + delay * 2, localstart + delay * 2 + dur, new Vector2(0, currHeight), ColumnType.two);
                field2.MoveReceptorRelative(OsbEasing.OutCirc, localstart + delay * 3, localstart + delay * 3 + dur, new Vector2(0, currHeight), ColumnType.one);

                localstart -= dur;
                localstart += gap;

                field.MoveReceptorRelative(OsbEasing.InCirc, localstart, localstart + dur, new Vector2(0, currHeight), ColumnType.one);
                field.MoveReceptorRelative(OsbEasing.InCirc, localstart + delay, localstart + delay + dur, new Vector2(0, currHeight), ColumnType.two);
                field.MoveReceptorRelative(OsbEasing.InCirc, localstart + delay * 2, localstart + delay * 2 + dur, new Vector2(0, currHeight), ColumnType.three);
                field.MoveReceptorRelative(OsbEasing.InCirc, localstart + delay * 3, localstart + delay * 3 + dur, new Vector2(0, currHeight), ColumnType.four);

                field.MoveColumnRelativeX(OsbEasing.InCirc, localstart, localstart + dur, -field.getColumnWidth(), ColumnType.one);
                field.MoveColumnRelativeX(OsbEasing.InCirc, localstart, localstart + dur, -field.getColumnWidth() / 2, ColumnType.two);
                field.MoveColumnRelativeX(OsbEasing.InCirc, localstart, localstart + dur, field.getColumnWidth() / 2, ColumnType.three);
                field.MoveColumnRelativeX(OsbEasing.InCirc, localstart, localstart + dur, field.getColumnWidth(), ColumnType.four);

                field2.MoveColumnRelativeX(OsbEasing.InCirc, localstart, localstart + dur, -field.getColumnWidth(), ColumnType.one);
                field2.MoveColumnRelativeX(OsbEasing.InCirc, localstart, localstart + dur, -field.getColumnWidth() / 2, ColumnType.two);
                field2.MoveColumnRelativeX(OsbEasing.InCirc, localstart, localstart + dur, field.getColumnWidth() / 2, ColumnType.three);
                field2.MoveColumnRelativeX(OsbEasing.InCirc, localstart, localstart + dur, field.getColumnWidth(), ColumnType.four);


                field2.MoveReceptorRelative(OsbEasing.InCirc, localstart, localstart + dur, new Vector2(0, -currHeight), ColumnType.four);
                field2.MoveReceptorRelative(OsbEasing.InCirc, localstart + delay, localstart + delay + dur, new Vector2(0, -currHeight), ColumnType.three);
                field2.MoveReceptorRelative(OsbEasing.InCirc, localstart + delay * 2, localstart + delay * 2 + dur, new Vector2(0, -currHeight), ColumnType.two);
                field2.MoveReceptorRelative(OsbEasing.InCirc, localstart + delay * 3, localstart + delay * 3 + dur, new Vector2(0, -currHeight), ColumnType.one);

                localstart += dur;

                field.MoveReceptorRelative(OsbEasing.OutCirc, localstart, localstart + dur, new Vector2(0, -currHeight), ColumnType.one);
                field.MoveReceptorRelative(OsbEasing.OutCirc, localstart + delay, localstart + delay + dur, new Vector2(0, -currHeight), ColumnType.two);
                field.MoveReceptorRelative(OsbEasing.OutCirc, localstart + delay * 2, localstart + delay * 2 + dur, new Vector2(0, -currHeight), ColumnType.three);
                field.MoveReceptorRelative(OsbEasing.OutCirc, localstart + delay * 3, localstart + delay * 3 + dur, new Vector2(0, -currHeight), ColumnType.four);

                field.MoveColumnRelativeX(OsbEasing.OutCirc, localstart, localstart + dur, -field.getColumnWidth() * 2, ColumnType.one);
                field.MoveColumnRelativeX(OsbEasing.OutCirc, localstart, localstart + dur, -field.getColumnWidth() / 2, ColumnType.two);
                field.MoveColumnRelativeX(OsbEasing.OutCirc, localstart, localstart + dur, field.getColumnWidth() / 2, ColumnType.three);
                field.MoveColumnRelativeX(OsbEasing.OutCirc, localstart, localstart + dur, field.getColumnWidth() * 2, ColumnType.four);

                field2.MoveColumnRelativeX(OsbEasing.OutCirc, localstart, localstart + dur, -field.getColumnWidth() * 2, ColumnType.one);
                field2.MoveColumnRelativeX(OsbEasing.OutCirc, localstart, localstart + dur, -field.getColumnWidth() / 2, ColumnType.two);
                field2.MoveColumnRelativeX(OsbEasing.OutCirc, localstart, localstart + dur, field.getColumnWidth() / 2, ColumnType.three);
                field2.MoveColumnRelativeX(OsbEasing.OutCirc, localstart, localstart + dur, field.getColumnWidth() * 2, ColumnType.four);

                field2.MoveReceptorRelative(OsbEasing.OutCirc, localstart, localstart + dur, new Vector2(0, currHeight), ColumnType.four);
                field2.MoveReceptorRelative(OsbEasing.OutCirc, localstart + delay, localstart + delay + dur, new Vector2(0, currHeight), ColumnType.three);
                field2.MoveReceptorRelative(OsbEasing.OutCirc, localstart + delay * 2, localstart + delay * 2 + dur, new Vector2(0, currHeight), ColumnType.two);
                field2.MoveReceptorRelative(OsbEasing.OutCirc, localstart + delay * 3, localstart + delay * 3 + dur, new Vector2(0, currHeight), ColumnType.one);

                localstart -= dur;
                localstart += gap;
            }

            field2.MoveOriginRelative(OsbEasing.OutSine, localstart, localstart, new Vector2(0, -currHeight + 35), ColumnType.all);

            field2.Rotate(OsbEasing.None, 66640, 66640, Math.PI / 6);
            field2.Rotate(OsbEasing.None, 66930, 66930, Math.PI / 6);
            field2.Rotate(OsbEasing.None, 67221, 67221, Math.PI / 3);
            field2.Rotate(OsbEasing.None, 67317, 67317, Math.PI / 3);

            field2.RotatePlayFieldStatic(OsbEasing.None, 66640, 66640, Math.PI / 6);
            field2.RotatePlayFieldStatic(OsbEasing.None, 66930, 66930, Math.PI / 6);
            field2.RotatePlayFieldStatic(OsbEasing.None, 67221, 67221, Math.PI / 3);
            field2.RotatePlayFieldStatic(OsbEasing.None, 67317, 67317, Math.PI / 3);

            //field.Resize(OsbEasing.OutSine, 66640, 66640, -width, height * 2);

            DrawInstance draw = new DrawInstance(CancellationToken, field, starttime + 50, scrollSpeed, updatesPerSecond, OsbEasing.None, false, fadeTime, fadeTime);
            draw.setReceptorMovementPrecision(0.25f);
            draw.setNoteMovementPrecision(0.25f);
            draw.drawViaEquation(67414 - starttime + 50, NoteFunction, true);

            DrawInstance draw2 = new DrawInstance(CancellationToken, field2, 55705 + 10, scrollSpeed, updatesPerSecond, OsbEasing.None, false, fadeTime, fadeTime);
            draw2.setReceptorMovementPrecision(0.25f);
            draw2.setNoteMovementPrecision(0.25f);
            draw2.setNoteRotationPrecision(0f);
            draw2.drawViaEquation(67414 - 55705, NoteFunction, true);
        }

        private void SpinAndCollapseReceptors(Playfield field, double starttime, double endtime)
        {
            var center = new Vector2(320, 240);
            double currentTime = starttime;
            double beatDuration = Beatmap.GetTimingPointAt((int)starttime).BeatDuration;

            // Animation parameters
            double spinDuration = beatDuration * 4; // Full rotation takes 4 beats
            double spinRadians = Math.PI * 2; // Full rotation
            float radius = 100f; // Distance from center when expanded
            float collapseTime = (float)beatDuration / 2; // Collapse happens on half beat

            while (currentTime < endtime)
            {
                // Calculate center-directed vectors for each column
                for (int i = 0; i < 4; i++)
                {
                    var column = (ColumnType)(i + 1);
                    var receptor = field.columns[column].receptor;

                    // Get vector from receptor to center
                    Vector2 toCenter = center - receptor.PositionAt(currentTime);
                    Vector2 fromCenter = receptor.PositionAt(currentTime) - center;

                    // Collapse inward
                    field.MoveReceptorRelative(OsbEasing.InCirc, currentTime, currentTime + collapseTime,
                        toCenter * 0.5f, column);

                    // Expand outward
                    field.MoveReceptorRelative(OsbEasing.OutCirc, currentTime + collapseTime,
                        currentTime + beatDuration, fromCenter * 0.5f, column);

                    // Apply rotation
                    field.columns[column].receptor.PivotReceptor(OsbEasing.None,
                        currentTime, currentTime + spinDuration,
                        spinRadians, center);
                }

                currentTime += beatDuration;
            }
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
