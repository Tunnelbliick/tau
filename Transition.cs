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

    public class Transition : StoryboardObjectGenerator
    {
        //        public override bool Multithreaded => false;
        private readonly object _lock = new object();

        float direction = -1;

        Playfield field = new Playfield();

        Playfield field2 = new Playfield();
        public override void Generate()
        {

            var receptors = GetLayer("r");
            var notes = GetLayer("n");

            // General values
            var starttime = 156446 - 25; // the starttime where the playfield is initialized
            var endtime = 168834; // the endtime where the playfield is nolonger beeing rendered
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

            //field.delta = 1f;
            field.initilizePlayField(receptors, notes, starttime, 169221, width, height, receptorWallOffset, Beatmap.OverallDifficulty);
            field.initializeNotes(Beatmap.HitObjects.ToList(), Beatmap.GetTimingPointAt(starttime).Bpm, Beatmap.GetTimingPointAt(starttime).Offset, isColored, sliderAccuracy);

            field2.delta = 5;
            field2.initilizePlayField(receptors, notes, endtime, 182769, width, height, receptorWallOffset, Beatmap.OverallDifficulty);
            field2.noteEnd = 181424;
            field2.initializeNotes(Beatmap.HitObjects.ToList(), Beatmap.GetTimingPointAt(starttime).Bpm, Beatmap.GetTimingPointAt(starttime).Offset, isColored, sliderAccuracy);

            field.RotateReceptor(OsbEasing.OutSine, 156446, 156640, .3, CenterType.receptor);
            field.RotateReceptor(OsbEasing.InSine, 156640, 157202, -.3, CenterType.receptor);
            field.Resize(OsbEasing.OutQuad, 156446, 157124, -width, height);

            field.MoveOriginRelative(OsbEasing.None, 152188, 152188, new Vector2(0, -height), ColumnType.all);
            field.MoveOriginRelative(OsbEasing.InSine, 155092, 156446, new Vector2(0, height), ColumnType.all);

            double start = 157231;
            double end = 158382;
            double dur = 100;
            double increase = 1.75;
            var localwidht = width;

            SpinOutAnimation(field, 157221, 158382, width, height, 32, 1.7);

            field.Resize(OsbEasing.OutCubic, 159350, 159543, width, height);

            field.RotateReceptor(OsbEasing.OutSine, 162640, 162834, .3, CenterType.receptor);
            field.RotateReceptor(OsbEasing.InSine, 162834, 163317, -.3, CenterType.receptor);
            field.Resize(OsbEasing.OutQuad, 162640, 163317, -width, height);

            SpinOutAnimation(field, 163414, 164576, width, height, 32, 1.7);

            field.ScaleColumn(OsbEasing.None, 156446, 156446, new Vector2(2f), ColumnType.all);
            field.ScaleColumn(OsbEasing.OutCubic, 156446, 157221, new Vector2(0.5f), ColumnType.all);

            ReceptorBump(field, 158382, 2, true);

            field.MoveReceptorRelative(OsbEasing.None, 158382, 158382, new Vector2(-150, 0), ColumnType.all);
            field.MoveReceptorRelative(OsbEasing.OutCubic, 158382, 158882, new Vector2(150, 0), ColumnType.all);

            field.MoveOriginRelative(OsbEasing.None, 158382, 158382, new Vector2(150, 0), ColumnType.all);
            field.MoveOriginRelative(OsbEasing.OutCubic, 158382, 158882, new Vector2(-150, 0), ColumnType.all);

            field.MoveReceptorRelative(OsbEasing.None, 158963, 158963, new Vector2(150, 0), ColumnType.all);
            field.MoveReceptorRelative(OsbEasing.OutCubic, 158963, 159350, new Vector2(-150, 0), ColumnType.all);

            field.MoveOriginRelative(OsbEasing.None, 158963, 158963, new Vector2(-150, 0), ColumnType.all);
            field.MoveOriginRelative(OsbEasing.OutCubic, 158963, 159350, new Vector2(150, 0), ColumnType.all);

            ReceptorBump(field, 158963, 2, true);

            FlipPlayField(field, 159543, 160705, width, height, 150, 2.1f);

            field.MoveReceptorRelative(OsbEasing.None, 160898, 160898, new Vector2(-150, 0), ColumnType.all);
            field.MoveReceptorRelative(OsbEasing.OutCubic, 160898, 161398, new Vector2(150, 0), ColumnType.all);

            field.MoveOriginRelative(OsbEasing.None, 160898, 160898, new Vector2(150, 0), ColumnType.all);
            field.MoveOriginRelative(OsbEasing.OutCubic, 160898, 161398, new Vector2(-150, 0), ColumnType.all);

            field.MoveReceptorRelative(OsbEasing.None, 161479, 161479, new Vector2(150, 0), ColumnType.all);
            field.MoveReceptorRelative(OsbEasing.OutCubic, 161479, 161979, new Vector2(-150, 0), ColumnType.all);

            field.MoveOriginRelative(OsbEasing.None, 161479, 161479, new Vector2(-150, 0), ColumnType.all);
            field.MoveOriginRelative(OsbEasing.OutCubic, 161479, 161979, new Vector2(150, 0), ColumnType.all);

            ReceptorBump(field, 160898, 2, true);
            ReceptorBump(field, 161479, 2, true);
            ReceptorBump(field, 162156, 2, true);

            ReceptorBump(field, 164576, 2, true);
            ReceptorBump(field, 165156, 2, true);

            field.Resize(OsbEasing.OutCubic, 165543, 165737, width, height);

            field.MoveReceptorRelative(OsbEasing.None, 164576, 164576, new Vector2(150, 0), ColumnType.all);
            field.MoveReceptorRelative(OsbEasing.OutSine, 164576, 165076, new Vector2(-150, 0), ColumnType.all);

            field.MoveOriginRelative(OsbEasing.None, 164576, 164576, new Vector2(-150, 0), ColumnType.all);
            field.MoveOriginRelative(OsbEasing.OutSine, 164576, 165076, new Vector2(150, 0), ColumnType.all);

            field.MoveReceptorRelative(OsbEasing.None, 165156, 165156, new Vector2(-150, 0), ColumnType.all);
            field.MoveReceptorRelative(OsbEasing.OutSine, 165156, 165656, new Vector2(150, 0), ColumnType.all);

            field.MoveOriginRelative(OsbEasing.None, 165156, 165156, new Vector2(150, 0), ColumnType.all);
            field.MoveOriginRelative(OsbEasing.OutSine, 165156, 165656, new Vector2(-150, 0), ColumnType.all);

            field.ScaleColumn(OsbEasing.None, 161866, 161866, new Vector2(1f), ColumnType.all);
            field.ScaleColumn(OsbEasing.OutCubic, 161866, 162640 - 50, new Vector2(0.5f), ColumnType.all);

            field.ScaleColumn(OsbEasing.None, 162640, 162640, new Vector2(2f), ColumnType.all);
            field.ScaleColumn(OsbEasing.OutCubic, 162640, 163392, new Vector2(0.5f), ColumnType.all);

            flipColumn(field, 165737, 1000, OsbEasing.OutElasticQuarter, ColumnType.all);

            field.ScaleReceptor(OsbEasing.InSine, 165737, 165834, new Vector2(1f), ColumnType.all);
            field.ScaleReceptor(OsbEasing.OutSine, 165834, 166406, new Vector2(0.5f), ColumnType.all);

            field.ScaleOrigin(OsbEasing.InSine, 165737, 165834, new Vector2(.25f), ColumnType.all);
            field.ScaleOrigin(OsbEasing.OutSine, 165834, 166406, new Vector2(0.5f), ColumnType.all);

            field.MoveReceptorRelative(OsbEasing.InSine, 165737, 165834, new Vector2(-field.getColumnWidth(), 0), ColumnType.one);
            field.MoveReceptorRelative(OsbEasing.InSine, 165737, 165834, new Vector2(-field.getColumnWidth() / 2, 0), ColumnType.two);
            field.MoveReceptorRelative(OsbEasing.InSine, 165737, 165834, new Vector2(field.getColumnWidth() / 2, 0), ColumnType.three);
            field.MoveReceptorRelative(OsbEasing.InSine, 165737, 165834, new Vector2(field.getColumnWidth(), 0), ColumnType.four);

            field.MoveReceptorRelative(OsbEasing.OutSine, 165737, 166406, new Vector2(field.getColumnWidth(), 0), ColumnType.one);
            field.MoveReceptorRelative(OsbEasing.OutSine, 165737, 166406, new Vector2(field.getColumnWidth() / 2, 0), ColumnType.two);
            field.MoveReceptorRelative(OsbEasing.OutSine, 165737, 166406, new Vector2(-field.getColumnWidth() / 2, 0), ColumnType.three);
            field.MoveReceptorRelative(OsbEasing.OutSine, 165737, 166406, new Vector2(-field.getColumnWidth(), 0), ColumnType.four);

            field.MoveOriginRelative(OsbEasing.InSine, 165737, 165834, new Vector2(field.getColumnWidth(), 0), ColumnType.one);
            field.MoveOriginRelative(OsbEasing.InSine, 165737, 165834, new Vector2(field.getColumnWidth() / 2, 0), ColumnType.two);
            field.MoveOriginRelative(OsbEasing.InSine, 165737, 165834, new Vector2(-field.getColumnWidth() / 2, 0), ColumnType.three);
            field.MoveOriginRelative(OsbEasing.InSine, 165737, 165834, new Vector2(-field.getColumnWidth(), 0), ColumnType.four);

            field.MoveOriginRelative(OsbEasing.OutSine, 165737, 166406, new Vector2(-field.getColumnWidth(), 0), ColumnType.one);
            field.MoveOriginRelative(OsbEasing.OutSine, 165737, 166406, new Vector2(-field.getColumnWidth() / 2, 0), ColumnType.two);
            field.MoveOriginRelative(OsbEasing.OutSine, 165737, 166406, new Vector2(field.getColumnWidth() / 2, 0), ColumnType.three);
            field.MoveOriginRelative(OsbEasing.OutSine, 165737, 166406, new Vector2(field.getColumnWidth(), 0), ColumnType.four);

            ScaleField(field, 166511);

            field.MoveReceptorRelative(OsbEasing.None, 167092, 167092, new Vector2(-200, 0), ColumnType.all);
            field.MoveReceptorRelative(OsbEasing.OutSine, 167092, 167382, new Vector2(200, 0), ColumnType.all);

            field.MoveOriginRelative(OsbEasing.None, 167092, 167092, new Vector2(200, 0), ColumnType.all);
            field.MoveOriginRelative(OsbEasing.OutSine, 167092, 167382, new Vector2(-200, 0), ColumnType.all);

            field.MoveReceptorRelative(OsbEasing.None, 167382, 167382, new Vector2(200, 0), ColumnType.all);
            field.MoveReceptorRelative(OsbEasing.OutSine, 167382, 167672, new Vector2(-200, 0), ColumnType.all);

            field.MoveOriginRelative(OsbEasing.None, 167382, 167382, new Vector2(-200, 0), ColumnType.all);
            field.MoveOriginRelative(OsbEasing.OutSine, 167382, 167672, new Vector2(200, 0), ColumnType.all);

            field.MoveReceptorRelative(OsbEasing.None, 167672, 167672, new Vector2(-200, 0), ColumnType.all);
            field.MoveReceptorRelative(OsbEasing.OutSine, 167672, 167962, new Vector2(200, 0), ColumnType.all);

            field.MoveOriginRelative(OsbEasing.None, 167672, 167672, new Vector2(200, 0), ColumnType.all);
            field.MoveOriginRelative(OsbEasing.OutSine, 167672, 167962, new Vector2(-200, 0), ColumnType.all);

            ReceptorBump(field, 167092, 2, false, 167382 - 167092);
            ReceptorBump(field, 167382, 2, false, 167382 - 167092);
            ReceptorBump(field, 167672, 2, false, 167382 - 167092);
            ReceptorBump(field, 167962, 2, false, 168446 - 167962);

            field.moveFieldY(OsbEasing.OutSine, 167962, 168446, -4.5f);

            //field.RotatePlayFieldStatic(OsbEasing.None, 167962, 168446, Math.PI);

            //field.moveFieldY(OsbEasing.OutSine, 167962, 168733, 240 - 65f);
            //field.Resize(OsbEasing.OutCubic, 167962, 169607, 0, height);
            //field.ScaleColumn(OsbEasing.InCubic, 167962, 169607, new Vector2(0f), ColumnType.all);

            field2.fadeAt(168059, 0);
            field2.fadeAt(169995 - 50, 1);
            field2.MoveOriginRelative(OsbEasing.None, 168059, 168059, new Vector2(0, -height), ColumnType.all);
            field2.MoveOriginRelative(OsbEasing.OutCirc, 169995, 170382, new Vector2(0, height), ColumnType.all);

            field2.RotatePlayFieldStatic(OsbEasing.None, 169995, 169995, Math.PI);
            field2.RotatePlayFieldStatic(OsbEasing.OutCubic, 169995, 170382, Math.PI);

            FakeNote(ColumnType.three, 168446);
            FakeNote(ColumnType.four, 168446);

            FakeNote(ColumnType.one, 168834);
            FakeNote(ColumnType.three, 168834);

            FakeNote(ColumnType.two, 169027, 2);

            FakeNote(ColumnType.three, 169221);
            FakeNote(ColumnType.four, 169221);

            FakeNoteFadeOut(ColumnType.three, 176188, 176188, 1);
            FakeNoteFadeOut(ColumnType.three, 176285, 176317, 4);
            FakeNoteFadeOut(ColumnType.three, 176382, 176446, 2);

            FakeNoteFadeOut(ColumnType.three, 176188, 176188, 1);
            FakeNoteFadeOut(ColumnType.three, 176285, 176317, 4);
            FakeNoteFadeOut(ColumnType.three, 176382, 176446, 2);

            FakeNoteFadeOut(ColumnType.one, 176575, 176317, 1, 176188);
            FakeNoteFadeOut(ColumnType.two, 176575, 176317, 1, 176188);

            FakeNoteFadeOut(ColumnType.four, 176769, 176317, 2, 176188);

            FakeNoteFadeOut(ColumnType.one, 176963, 176446, 1, 176188);
            FakeNoteFadeOut(ColumnType.three, 176963, 176446, 1, 176188);

            FakeNoteFadeOut(ColumnType.two, 177156, 176317, 2, 176188);

            FakeNoteFadeIn(ColumnType.one, 176575, 176188, 1);
            FakeNoteFadeIn(ColumnType.three, 176575, 176188, 1);
            FakeNoteFadeIn(ColumnType.two, 176705, 176188, 3);
            FakeNoteFadeIn(ColumnType.four, 176834, 176188, 3);

            FakeNoteFadeIn(ColumnType.one, 176963, 176317, 1);
            FakeNoteFadeIn(ColumnType.two, 176963, 176317, 1);
            FakeNoteFadeIn(ColumnType.three, 177092, 176317, 3);
            FakeNoteFadeIn(ColumnType.one, 177221, 176317, 3);

            FakeNoteFadeIn(ColumnType.two, 177350, 176446, 1);
            FakeNoteFadeIn(ColumnType.four, 177350, 176446, 1);
            FakeNoteFadeIn(ColumnType.three, 177479, 176446, 3);
            FakeNoteFadeIn(ColumnType.one, 177608, 176446, 3);

            var local = 170382;
            var beatLength = Beatmap.GetTimingPointAt(local).BeatDuration;
            var shortDur = 170479 - 170382;
            var longDur = 170769 - 170479;
            while (local < 175414)
            {


                field2.MoveReceptorRelative(OsbEasing.OutElasticQuarter, local, local + shortDur, new Vector2(field2.getColumnWidth(), 0), ColumnType.all);
                field2.MoveOriginRelative(OsbEasing.OutElasticQuarter, local, local + shortDur, new Vector2(-field2.getColumnWidth(), 0), ColumnType.all);

                field2.RotatePlayFieldStatic(OsbEasing.OutElasticQuarter, local, local + shortDur, .2, ColumnType.all);
                field2.ScaleColumn(OsbEasing.OutElasticQuarter, local, local + shortDur, new Vector2(0.6f), ColumnType.all);

                field2.MoveReceptorRelative(OsbEasing.OutElasticQuarter, local + shortDur, local + longDur, new Vector2(-field2.getColumnWidth(), 0), ColumnType.all);
                field2.MoveOriginRelative(OsbEasing.OutElasticQuarter, local + shortDur, local + longDur, new Vector2(field2.getColumnWidth(), 0), ColumnType.all);

                field2.RotatePlayFieldStatic(OsbEasing.OutElasticQuarter, local + shortDur, local + longDur, -.2, ColumnType.all);
                field2.ScaleColumn(OsbEasing.OutElasticQuarter, local + shortDur, local + longDur, new Vector2(0.5f), ColumnType.all);

                local += (int)beatLength;

                field2.MoveReceptorRelative(OsbEasing.OutElasticQuarter, local, local + shortDur, new Vector2(-field2.getColumnWidth(), 0), ColumnType.all);
                field2.MoveOriginRelative(OsbEasing.OutElasticQuarter, local, local + shortDur, new Vector2(field2.getColumnWidth(), 0), ColumnType.all);

                field2.RotatePlayFieldStatic(OsbEasing.OutElasticQuarter, local, local + shortDur, -.2, ColumnType.all);
                field2.ScaleColumn(OsbEasing.OutElasticQuarter, local, local + shortDur, new Vector2(0.6f), ColumnType.all);

                field2.MoveReceptorRelative(OsbEasing.OutElasticQuarter, local + shortDur, local + longDur, new Vector2(field2.getColumnWidth(), 0), ColumnType.all);
                field2.MoveOriginRelative(OsbEasing.OutElasticQuarter, local + shortDur, local + longDur, new Vector2(-field2.getColumnWidth(), 0), ColumnType.all);

                field2.RotatePlayFieldStatic(OsbEasing.OutElasticQuarter, local + shortDur, local + longDur, .2, ColumnType.all);
                field2.ScaleColumn(OsbEasing.OutElasticQuarter, local + shortDur, local + longDur, new Vector2(0.5f), ColumnType.all);

                local += (int)beatLength;
            }


            field2.MoveColumnRelativeX(OsbEasing.OutElasticQuarter, 170382, 170769, field2.getColumnWidth(), ColumnType.one);
            field2.MoveColumnRelativeX(OsbEasing.OutElasticQuarter, 170382, 170769, field2.getColumnWidth() * 2, ColumnType.two);
            field2.MoveColumnRelativeX(OsbEasing.OutElasticQuarter, 170382, 170769, -field2.getColumnWidth() * 2, ColumnType.three);
            field2.MoveColumnRelativeX(OsbEasing.OutElasticQuarter, 170382, 170769, -field2.getColumnWidth(), ColumnType.four);

            field2.Resize(OsbEasing.OutElasticQuarter, 170769, 171156, -width, height);

            field2.Resize(OsbEasing.OutElasticQuarter, 171156, 171543, width, height);

            field2.Resize(OsbEasing.OutElasticQuarter, 172317, 172705, -width, height);

            flipColumn(field2, 171543, 150, OsbEasing.OutElasticQuarter, ColumnType.four);
            field2.RotatePlayFieldStatic(OsbEasing.OutElasticQuarter, 171543, 171543 + 150, Math.PI * 2, ColumnType.four);
            flipColumn(field2, 171640, 150, OsbEasing.OutElasticQuarter, ColumnType.three);
            field2.RotatePlayFieldStatic(OsbEasing.OutElasticQuarter, 171640, 171640 + 150, Math.PI * 2, ColumnType.three);
            flipColumn(field2, 171737, 150, OsbEasing.OutElasticQuarter, ColumnType.two);
            field2.RotatePlayFieldStatic(OsbEasing.OutElasticQuarter, 171737, 171737 + 150, Math.PI * 2, ColumnType.two);
            flipColumn(field2, 171834, 150, OsbEasing.OutElasticQuarter, ColumnType.one);
            field2.RotatePlayFieldStatic(OsbEasing.OutElasticQuarter, 171834, 171834 + 150, Math.PI * 2, ColumnType.one);


            field2.MoveColumnRelativeX(OsbEasing.OutSine, 172705, 173285, -field2.getColumnWidth(), ColumnType.one);
            field2.MoveColumnRelativeX(OsbEasing.OutSine, 172705, 173285, -field2.getColumnWidth() * 0.4f, ColumnType.two);
            field2.MoveColumnRelativeX(OsbEasing.OutSine, 172705, 173285, field2.getColumnWidth() * 0.4f, ColumnType.three);
            field2.MoveColumnRelativeX(OsbEasing.OutSine, 172705, 173285, field2.getColumnWidth(), ColumnType.four);

            field2.ScaleColumn(OsbEasing.OutSine, 172705, 173285, new Vector2(1f), ColumnType.two);
            field2.ScaleColumn(OsbEasing.OutElasticQuarter, 173285, 173479, new Vector2(0.5f), ColumnType.two);

            field2.MoveColumnRelativeX(OsbEasing.OutElasticQuarter, 173285, 173479, field2.getColumnWidth() * 3, ColumnType.one);
            field2.MoveColumnRelativeX(OsbEasing.OutElasticQuarter, 173285, 173479, field2.getColumnWidth() * 0.4f, ColumnType.two);
            field2.MoveColumnRelativeX(OsbEasing.OutElasticQuarter, 173285, 173479, -field2.getColumnWidth() * 0.4f, ColumnType.three);
            field2.MoveColumnRelativeX(OsbEasing.OutElasticQuarter, 173285, 173479, -field2.getColumnWidth(), ColumnType.four);

            field2.Rotate(OsbEasing.OutSine, 172705, 173285, 0.35, CenterType.playfield);
            field2.Rotate(OsbEasing.OutElasticQuarter, 173285, 173479, -0.35, CenterType.playfield);

            field2.Resize(OsbEasing.OutElasticQuarter, 173479, 173866, width, height);

            field2.MoveColumnRelativeX(OsbEasing.OutElasticQuarter, 173866, 174253, field2.getColumnWidth(), ColumnType.one);
            field2.MoveColumnRelativeX(OsbEasing.OutElasticQuarter, 173866, 174253, field2.getColumnWidth() * 2, ColumnType.two);
            field2.MoveColumnRelativeX(OsbEasing.OutElasticQuarter, 173866, 174253, -field2.getColumnWidth() * 2, ColumnType.three);
            field2.MoveColumnRelativeX(OsbEasing.OutElasticQuarter, 173866, 174253, -field2.getColumnWidth(), ColumnType.four);

            field2.MoveColumnRelativeX(OsbEasing.OutElasticQuarter, 174253, 174640, -field2.getColumnWidth(), ColumnType.one);
            field2.MoveColumnRelativeX(OsbEasing.OutElasticQuarter, 174253, 174640, -field2.getColumnWidth() * 2, ColumnType.two);
            field2.MoveColumnRelativeX(OsbEasing.OutElasticQuarter, 174253, 174640, field2.getColumnWidth() * 2, ColumnType.three);
            field2.MoveColumnRelativeX(OsbEasing.OutElasticQuarter, 174253, 174640, field2.getColumnWidth(), ColumnType.four);

            flipColumn(field2, 174640, 150, OsbEasing.OutElasticQuarter, ColumnType.one);
            field2.RotatePlayFieldStatic(OsbEasing.OutElasticQuarter, 171543, 171543 + 150, Math.PI * 2, ColumnType.one);
            flipColumn(field2, 174737, 150, OsbEasing.OutElasticQuarter, ColumnType.two);
            field2.RotatePlayFieldStatic(OsbEasing.OutElasticQuarter, 174737, 174737 + 150, Math.PI * 2, ColumnType.two);
            flipColumn(field2, 174834, 150, OsbEasing.OutElasticQuarter, ColumnType.three);
            field2.RotatePlayFieldStatic(OsbEasing.OutElasticQuarter, 174834, 174834 + 150, Math.PI * 2, ColumnType.three);
            flipColumn(field2, 174930, 150, OsbEasing.OutElasticQuarter, ColumnType.four);
            field2.RotatePlayFieldStatic(OsbEasing.OutElasticQuarter, 174930, 174930 + 150, Math.PI * 2, ColumnType.four);

            field2.moveField(OsbEasing.OutSine, 171543, 176188, 3, -17);

            field2.Resize(OsbEasing.OutElasticQuarter, 175027, 175414, -width, height);

            field2.Resize(OsbEasing.OutElasticQuarter, 175414, 175801, width, height);

            local = 176575;
            beatLength = Beatmap.GetTimingPointAt(local).BeatDuration;
            shortDur = 170479 - 170382;
            longDur = 170769 - 170479;
            while (local < 179672 - beatLength)
            {


                if (local < 179672)
                {
                    field2.MoveReceptorRelative(OsbEasing.OutElasticQuarter, local, local + shortDur, new Vector2(field2.getColumnWidth(), 0), ColumnType.all);
                    field2.MoveOriginRelative(OsbEasing.OutElasticQuarter, local, local + shortDur, new Vector2(-field2.getColumnWidth(), 0), ColumnType.all);

                    field2.RotatePlayFieldStatic(OsbEasing.OutElasticQuarter, local, local + shortDur, .2, ColumnType.all);
                    field2.ScaleColumn(OsbEasing.OutElasticQuarter, local, local + shortDur, new Vector2(0.55f), ColumnType.all);

                    field2.MoveReceptorRelative(OsbEasing.OutElasticQuarter, local + shortDur, local + longDur, new Vector2(-field2.getColumnWidth(), 0), ColumnType.all);
                    field2.MoveOriginRelative(OsbEasing.OutElasticQuarter, local + shortDur, local + longDur, new Vector2(field2.getColumnWidth(), 0), ColumnType.all);

                    field2.RotatePlayFieldStatic(OsbEasing.OutElasticQuarter, local + shortDur, local + longDur, -.2, ColumnType.all);
                    field2.ScaleColumn(OsbEasing.OutElasticQuarter, local + shortDur, local + longDur, new Vector2(0.5f), ColumnType.all);
                }


                local += (int)beatLength;

                if (local < 179672)
                {
                    field2.MoveReceptorRelative(OsbEasing.OutElasticQuarter, local, local + shortDur, new Vector2(-field2.getColumnWidth(), 0), ColumnType.all);
                    field2.MoveOriginRelative(OsbEasing.OutElasticQuarter, local, local + shortDur, new Vector2(field2.getColumnWidth(), 0), ColumnType.all);

                    field2.RotatePlayFieldStatic(OsbEasing.OutElasticQuarter, local, local + shortDur, -.2, ColumnType.all);
                    field2.ScaleColumn(OsbEasing.OutElasticQuarter, local, local + shortDur, new Vector2(0.55f), ColumnType.all);

                    field2.MoveReceptorRelative(OsbEasing.OutElasticQuarter, local + shortDur, local + longDur, new Vector2(field2.getColumnWidth(), 0), ColumnType.all);
                    field2.MoveOriginRelative(OsbEasing.OutElasticQuarter, local + shortDur, local + longDur, new Vector2(-field2.getColumnWidth(), 0), ColumnType.all);


                    field2.RotatePlayFieldStatic(OsbEasing.OutElasticQuarter, local + shortDur, local + longDur, .2, ColumnType.all);
                    field2.ScaleColumn(OsbEasing.OutElasticQuarter, local + shortDur, local + longDur, new Vector2(0.5f), ColumnType.all);
                }

                local += (int)beatLength;
            }

            field2.RotatePlayFieldStatic(OsbEasing.InQuad, 179672, 181221, Math.PI * 8);

            field2.ScaleReceptor(OsbEasing.InOutSine, 179672, 181221, new Vector2(0.25f), ColumnType.all);
            field2.ScaleOrigin(OsbEasing.InOutSine, 179672, 181221, new Vector2(1f), ColumnType.all);
            var colWidth = field2.getColumnWidth() * 3;
            var halfwidth = field2.getColumnWidth();

            field2.MoveReceptorRelative(OsbEasing.None, 179672, 181221, new Vector2(colWidth / 4, 0), ColumnType.one);
            field2.MoveReceptorRelative(OsbEasing.OutCubic, 181221, 181995, new Vector2(-colWidth / 4, 0), ColumnType.one);
            field2.MoveReceptorRelative(OsbEasing.None, 179672, 181221, new Vector2(halfwidth / 4, 0), ColumnType.two);
            field2.MoveReceptorRelative(OsbEasing.OutCubic, 181221, 181995, new Vector2(-halfwidth / 4, 0), ColumnType.two);
            field2.MoveReceptorRelative(OsbEasing.None, 179672, 181221, new Vector2(-halfwidth / 4, 0), ColumnType.three);
            field2.MoveReceptorRelative(OsbEasing.OutCubic, 181221, 181995, new Vector2(halfwidth / 4, 0), ColumnType.three);
            field2.MoveReceptorRelative(OsbEasing.None, 179672, 181221, new Vector2(-colWidth / 4, 0), ColumnType.four);
            field2.MoveReceptorRelative(OsbEasing.OutCubic, 181221, 181995, new Vector2(colWidth / 4, 0), ColumnType.four);

            field2.MoveOriginRelative(OsbEasing.None, 179672, 181221, new Vector2(-colWidth, 0), ColumnType.one);
            field2.MoveOriginRelative(OsbEasing.OutCubic, 181221, 181995, new Vector2(colWidth, 0), ColumnType.one);
            field2.MoveOriginRelative(OsbEasing.None, 179672, 181221, new Vector2(-halfwidth, 0), ColumnType.two);
            field2.MoveOriginRelative(OsbEasing.OutCubic, 181221, 181995, new Vector2(halfwidth, 0), ColumnType.two);
            field2.MoveOriginRelative(OsbEasing.None, 179672, 181221, new Vector2(halfwidth, 0), ColumnType.three);
            field2.MoveOriginRelative(OsbEasing.OutCubic, 181221, 181995, new Vector2(-halfwidth, 0), ColumnType.three);
            field2.MoveOriginRelative(OsbEasing.None, 179672, 181221, new Vector2(colWidth, 0), ColumnType.four);
            field2.MoveOriginRelative(OsbEasing.OutCubic, 181221, 181995, new Vector2(-colWidth, 0), ColumnType.four);

            field2.ScaleReceptor(OsbEasing.OutCubic, 181221, 181995, new Vector2(0.45f), ColumnType.all);
            field2.RotatePlayFieldStatic(OsbEasing.OutCubic, 181221, 181995, Math.PI);
            field2.Resize(OsbEasing.OutCubic, 181221, 181995, 0, 0);

            field2.moveFieldY(OsbEasing.OutCirc, 181221, 181995, 240 - receptorWallOffset);

            field2.columns[ColumnType.one].receptor.renderedSprite.Fade(OsbEasing.OutCubic, 181221, 181995, 1, .8f);
            field2.columns[ColumnType.two].receptor.renderedSprite.Fade(OsbEasing.OutCubic, 181221, 181995, 1, .8f);
            field2.columns[ColumnType.three].receptor.renderedSprite.Fade(OsbEasing.OutCubic, 181221, 181995, 1, .8f);
            field2.columns[ColumnType.four].receptor.renderedSprite.Fade(OsbEasing.OutCubic, 181221, 181995, 1, .8f);

            DrawInstance draw = new DrawInstance(CancellationToken, field, 156446, scrollSpeed, updatesPerSecond, OsbEasing.None, false, 5, fadeTime);
            draw.setReceptorMovementPrecision(0.25f);
            draw.setNoteRotationPrecision(0.1f);
            draw.setNoteMovementPrecision(0.1f);
            draw.setHoldRotationPrecision(0.01f);
            draw.drawViaEquation(169221 - 156446, NoteFunction, true);

            DrawInstance draw2 = new DrawInstance(CancellationToken, field2, endtime + 25, scrollSpeed, 1000, OsbEasing.None, false, fadeTime, fadeTime);
            draw2.setReceptorMovementPrecision(0.25f);
            draw2.setNoteRotationPrecision(0.01f);
            draw2.setNoteMovementPrecision(0.25f);
            draw2.drawViaEquation(182769 - endtime + 25, NoteFunction, true);

            foreach (var column in field2.columns.Values)
            {
                KeyframedValue<Vector2> pos = new KeyframedValue<Vector2>(InterpolatingFunctions.Vector2);
                KeyframedValue<double> rotation = new KeyframedValue<double>(InterpolatingFunctions.Double);

                var detail = 5;
                var startTime = 170382;

                var sprite = GetLayer("s").CreateSprite("sb/white.png", OsbOrigin.Centre, new Vector2(0, field2.calculatePlayFieldCenter(171543).Y + 100));

                sprite.Fade(169995, .75f);

                sprite.ScaleVec(169995, 170382, 0, 900f, 60, 900f);

                switch (column.type)
                {
                    case ColumnType.one:
                        sprite.Color(169995, new Color4(94, 196, 192, 255));
                        sprite.Fade(176188, 0);
                        break;
                    case ColumnType.two:
                        sprite.Color(169995, new Color4(245, 244, 238, 255));
                        sprite.Fade(176285, 0);
                        break;
                    case ColumnType.three:
                        sprite.Color(169995, new Color4(237, 199, 112, 255));
                        sprite.Fade(176382, 0);
                        break;
                    case ColumnType.four:
                        sprite.Color(169995, new Color4(236, 99, 74, 255));
                        sprite.Fade(176479, 0);
                        break;
                }



                bool hasLogged = false;

                rotation.Add(startTime, 0);

                while (startTime < 176575)
                {
                    var receptorPos = column.ReceptorPositionAt(startTime, -1);
                    var originPos = column.OriginPositionAt(startTime, -1);

                    pos.Add(startTime, receptorPos);
                    if (startTime > 170382)
                    {
                        double theta = 0;

                        theta = Math.Abs(Math.Atan2(receptorPos.Y - originPos.Y, receptorPos.X - originPos.X)) - Math.PI / 2;

                        if (theta < 1f && theta > -1f)
                        {

                            // if(receptorPos.Y < originPos.Y)
                            // {
                            //    theta = -theta / 2;
                            //}

                            // Add the adjusted angle to the rotation keyframes
                            rotation.Add(startTime, theta);
                        }
                    }

                    startTime += detail;
                }

                pos.Simplify(0.5f);
                rotation.Simplify(0.01f);

                pos.ForEachPair((start, end) =>
                {
                    sprite.MoveX(OsbEasing.None, start.Time, end.Time, start.Value.X, end.Value.X);
                });

                rotation.ForEachPair((start, end) =>
                {
                    sprite.Rotate(OsbEasing.None, start.Time, end.Time, start.Value, end.Value);
                });


            }
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
            var pos = field.columns[type].OriginPositionAt(168834);
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

        public void FakeNoteFadeOut(ColumnType type, double hittime, double fadeOutTime, double noteType = 1, double travelStopTime = 0)
        {
            var height = 500f;
            var scrollSpeed = 1200f;
            var endtime = 176575;

            var start = hittime - scrollSpeed;
            var currentTime = 175898 - start;

            var pos = field2.columns[type].OriginPositionAt(start);
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
            else if (travelStopTime == 0)
            {
                currentTime = fadeOutTime - 50 - start;
                travelAmount = currentTime / 1200f * height;

                fakeNote.MoveY(OsbEasing.None, start, fadeOutTime - 50, pos.Y, pos.Y - travelAmount);
                fakeNoteWhite.MoveY(OsbEasing.None, start, fadeOutTime - 50, pos.Y, pos.Y - travelAmount);
            }
            else
            {
                currentTime = travelStopTime - start;
                travelAmount = currentTime / 1200f * height;

                fakeNote.MoveY(OsbEasing.None, start, travelStopTime, pos.Y, pos.Y - travelAmount);
                fakeNoteWhite.MoveY(OsbEasing.None, start, travelStopTime, pos.Y, pos.Y - travelAmount);
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
            var pos = field2.columns[type].OriginPositionAt(start);
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

        public void ScaleField(Playfield field, double startTime, float scale = 2f, double dur = 300)
        {
            field.RotatePlayFieldStatic(OsbEasing.None, startTime, startTime, Math.PI * direction);
            field.RotatePlayFieldStatic(OsbEasing.OutCubic, startTime, startTime + dur, Math.PI * direction);
            field.ScaleReceptor(OsbEasing.None, startTime, startTime, new Vector2(scale), ColumnType.all);
            if (dur == 2500)
            {
                field.ScaleReceptor(OsbEasing.OutCubic, startTime, startTime + dur, new Vector2(0f), ColumnType.all);
                return;
            }
            field.ScaleReceptor(OsbEasing.OutCubic, startTime, startTime + dur, new Vector2(0.5f), ColumnType.all);

            direction *= -1;
        }

        private double SpinOutAnimation(Playfield field, double startTime, double endTime, float widthValue, float heightValue, double initialDuration = 100, double growthFactor = 1.5)
        {
            double currentTime = startTime;
            double remainingTime = endTime - startTime;
            double currentDuration = initialDuration;
            float currentWidth = widthValue;
            bool positiveWidthAtEnd = true;

            // Plan the durations ahead of time to make sure we hit exactly endTime
            List<double> durations = new List<double>();
            double tempDur = currentDuration;
            double tempTime = 0;

            // Calculate how many cycles we can fit
            while (tempTime + tempDur <= remainingTime)
            {
                durations.Add(tempDur);
                tempTime += tempDur;
                tempDur *= growthFactor;
            }

            // Apply the animation for each cycle
            for (int i = 0; i < durations.Count; i++)
            {
                double duration = durations[i];

                var scaleReduction = 0.15f;
                var fullReduction = scaleReduction / 2 + scaleReduction;

                if (i % 2 == 0)
                {
                    field.ScaleColumn(OsbEasing.OutSine, currentTime, currentTime + duration / 2, new Vector2(0.5f - fullReduction), ColumnType.one);
                    field.ScaleColumn(OsbEasing.OutSine, currentTime, currentTime + duration / 2, new Vector2(0.5f - scaleReduction), ColumnType.two);
                    field.ScaleColumn(OsbEasing.OutSine, currentTime, currentTime + duration / 2, new Vector2(0.5f + scaleReduction), ColumnType.three);
                    field.ScaleColumn(OsbEasing.OutSine, currentTime, currentTime + duration / 2, new Vector2(0.5f + fullReduction), ColumnType.four);

                    field.ScaleColumn(OsbEasing.InSine, currentTime + duration / 2, currentTime + duration, new Vector2(0.5f), ColumnType.one);
                    field.ScaleColumn(OsbEasing.InSine, currentTime + duration / 2, currentTime + duration, new Vector2(0.5f), ColumnType.two);
                    field.ScaleColumn(OsbEasing.InSine, currentTime + duration / 2, currentTime + duration, new Vector2(0.5f), ColumnType.three);
                    field.ScaleColumn(OsbEasing.InSine, currentTime + duration / 2, currentTime + duration, new Vector2(0.5f), ColumnType.four);
                }
                else
                {
                    field.ScaleColumn(OsbEasing.OutSine, currentTime, currentTime + duration / 2, new Vector2(0.5f - fullReduction), ColumnType.four);
                    field.ScaleColumn(OsbEasing.OutSine, currentTime, currentTime + duration / 2, new Vector2(0.5f - scaleReduction), ColumnType.three);
                    field.ScaleColumn(OsbEasing.OutSine, currentTime, currentTime + duration / 2, new Vector2(0.5f + scaleReduction), ColumnType.two);
                    field.ScaleColumn(OsbEasing.OutSine, currentTime, currentTime + duration / 2, new Vector2(0.5f + fullReduction), ColumnType.one);

                    field.ScaleColumn(OsbEasing.InSine, currentTime + duration / 2, currentTime + duration, new Vector2(0.5f), ColumnType.four);
                    field.ScaleColumn(OsbEasing.InSine, currentTime + duration / 2, currentTime + duration, new Vector2(0.5f), ColumnType.three);
                    field.ScaleColumn(OsbEasing.InSine, currentTime + duration / 2, currentTime + duration, new Vector2(0.5f), ColumnType.two);
                    field.ScaleColumn(OsbEasing.InSine, currentTime + duration / 2, currentTime + duration, new Vector2(0.5f), ColumnType.one);
                }

                field.RotateReceptor(OsbEasing.OutSine, currentTime, currentTime + duration / 2, -.3, CenterType.receptor);
                field.RotateReceptor(OsbEasing.InSine, currentTime + duration / 2, currentTime + duration, .3, CenterType.receptor);
                field.Resize(OsbEasing.InOutSine, currentTime, currentTime + duration, currentWidth, heightValue);

                currentWidth *= -1;
                currentTime += duration;
            }

            // If we ended with a negative width, do a quick flip back to positive
            if (currentWidth < 0)
            {
                // We'd normally need another flip, but we're at the end time already
                // So just return the absolute value
                currentWidth = Math.Abs(currentWidth);
            }

            return currentWidth;
        }

        public void ReceptorBump(Playfield playfiled, double localtime, float diff = 2, bool isInverted = false, double gap = 43801 - 43414)
        {

            if (localtime < 176575)
            {
                playfiled.RotatePlayFieldStatic(OsbEasing.None, localtime - 10, localtime - 10, Math.PI);
                playfiled.RotatePlayFieldStatic(OsbEasing.OutCubic, localtime, localtime + gap - 10, Math.PI);
            }

            float defaultScale = 0.5f;
            float smallScale = defaultScale / diff;
            float bigScale = defaultScale * diff;

            var colWidth = playfiled.getColumnWidth() * 2 / 2 * diff;
            var halfwidth = playfiled.getColumnWidth() / 2 / 2 * diff;

            if (isInverted)
            {
                colWidth *= -1;
                halfwidth *= -1;
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
            }

        }

        public void flipColumn(Playfield field, double starttime, double duration, OsbEasing easing, ColumnType type)
        {
            foreach (Column currentColumn in field.columns.Values)
            {

                if (currentColumn.type == type || type == ColumnType.all)
                {

                    Vector2 receptorPos = currentColumn.ReceptorPositionAt(starttime);
                    Vector2 originPos = currentColumn.OriginPositionAt(starttime);
                    Vector2 center = new Vector2(427, 240);

                    // Calculate the change needed to flip the positions
                    Vector2 changeReceptorPos = (center - receptorPos) * 2;
                    Vector2 changeOriginPos = (center - originPos) * 2;

                    currentColumn.receptor.MoveReceptorRelativeY(easing, starttime, starttime + duration, changeReceptorPos.Y);
                    currentColumn.origin.MoveOriginRelativeY(easing, starttime, starttime + duration, changeOriginPos.Y);
                }

            }
        }

        private void FlipPlayField(Playfield field, double startTime, double endTime, float widthValue, float heightValue, double initialDuration = 100, double growthFactor = 1.5)
        {
            double currentTime = startTime;
            double remainingTime = endTime - startTime;
            double currentDuration = initialDuration;
            float currentWidth = widthValue;
            bool positiveWidthAtEnd = true;

            // Plan the durations ahead of time to make sure we hit exactly endTime
            List<double> durations = new List<double>();
            double tempDur = currentDuration;
            double tempTime = 0;

            // Calculate how many cycles we can fit
            while (tempTime + tempDur <= remainingTime)
            {
                durations.Add(tempDur);
                tempTime += tempDur;
                tempDur *= growthFactor;
            }

            // Apply the animation for each cycle
            for (int i = 0; i < durations.Count; i++)
            {
                double duration = durations[i];

                foreach (var col in field.columns.Values)
                {
                    Vector2 receptorPos = col.ReceptorPositionAt(currentTime);
                    Vector2 originPos = col.OriginPositionAt(currentTime);
                    Vector2 center = new Vector2(427, 240);

                    // Calculate the change needed to flip the positions
                    Vector2 changeReceptorPos = (center - receptorPos) * 2;
                    Vector2 changeOriginPos = (center - originPos) * 2;

                    var colWidth = field.getColumnWidth();

                    if (i % 2 == 0)
                    {
                        field.ScaleReceptor(OsbEasing.OutSine, currentTime, currentTime + duration / 2, new Vector2(1f), col.type);
                        field.ScaleOrigin(OsbEasing.OutSine, currentTime, currentTime + duration / 2, new Vector2(0.25f), col.type);

                        field.ScaleReceptor(OsbEasing.InSine, currentTime + duration / 2, currentTime + duration, new Vector2(0.5f), col.type);
                        field.ScaleOrigin(OsbEasing.InSine, currentTime + duration / 2, currentTime + duration, new Vector2(0.5f), col.type);
                    }
                    else
                    {
                        colWidth = -colWidth;
                        field.ScaleReceptor(OsbEasing.OutSine, currentTime, currentTime + duration / 2, new Vector2(0.25f), col.type);
                        field.ScaleOrigin(OsbEasing.OutSine, currentTime, currentTime + duration / 2, new Vector2(1f), col.type);

                        field.ScaleReceptor(OsbEasing.InSine, currentTime + duration / 2, currentTime + duration, new Vector2(0.5f), col.type);
                        field.ScaleOrigin(OsbEasing.InSine, currentTime + duration / 2, currentTime + duration, new Vector2(0.5f), col.type);
                    }

                    switch (col.type)
                    {
                        case ColumnType.one:
                            break;
                        case ColumnType.two:
                            colWidth = colWidth / 2;
                            break;
                        case ColumnType.three:
                            colWidth = -colWidth / 2;
                            break;
                        case ColumnType.four:
                            colWidth = -colWidth;
                            break;
                    }

                    col.receptor.MoveReceptorRelativeX(OsbEasing.OutSine, currentTime, currentTime + duration / 2, colWidth);
                    col.receptor.MoveReceptorRelativeX(OsbEasing.InSine, currentTime + duration / 2, currentTime + duration, -colWidth);

                    col.origin.MoveOriginRelativeX(OsbEasing.OutSine, currentTime, currentTime + duration / 2, -colWidth);
                    col.origin.MoveOriginRelativeX(OsbEasing.InSine, currentTime + duration / 2, currentTime + duration, colWidth);

                    col.receptor.MoveReceptorRelativeY(OsbEasing.OutSine, currentTime, currentTime + duration, changeReceptorPos.Y);
                    col.origin.MoveOriginRelativeY(OsbEasing.OutSine, currentTime, currentTime + duration, changeOriginPos.Y);
                }

                currentTime += duration;
            }
        }

        public void OriginBump(Playfield field, double localtime, float diff = 2, double gap = 43801 - 43414)
        {
            if (localtime < 176575)
            {
                field.RotatePlayFieldStatic(OsbEasing.None, localtime - 10, localtime - 10, Math.PI);
                field.RotatePlayFieldStatic(OsbEasing.OutCubic, localtime, localtime + gap, Math.PI);
            }

            float defaultScale = 0.5f;
            float smallScale = defaultScale / diff;
            float bigScale = defaultScale * diff;

            var colWidth = -field.getColumnWidth() * 2 / 2 * diff;
            var halfwidth = -field.getColumnWidth() / 2 / 2 * diff;

            field.ScaleReceptor(OsbEasing.None, localtime, localtime, new Vector2(smallScale), ColumnType.all);
            field.ScaleOrigin(OsbEasing.None, localtime, localtime, new Vector2(bigScale), ColumnType.all);

            field.ScaleReceptor(OsbEasing.OutCubic, localtime, localtime + gap, new Vector2(defaultScale), ColumnType.all);
            field.ScaleOrigin(OsbEasing.OutCubic, localtime, localtime + gap, new Vector2(defaultScale), ColumnType.all);

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
        }

        // NoteFunction is used to manipulate the pathway and a bunch of other things the note should do on their way to the receptor
        // Please be warry that this is beeing run async so you need to keep thread safety in mind when working on complex Functions.
        // You can use the progress to determin how far the note is in its cycle 0 = just start | 1 = ontop of receptor / finished
        // Special flags for hold bodies exist
        public Vector2 NoteFunction(EquationParameters p)
        {

            if (((p.time >= 157221 && p.time <= 158382) || (p.time >= 163414 && p.time <= 164575)) && p.isHoldBody == false)
            {
                lock (_lock)
                {
                    StoryboardLayer layer = p.column.receptor.layer;
                    String notePath = p.note.noteSprite.TexturePath;

                    var shadow = layer.CreateSprite(notePath, OsbOrigin.Centre, p.lastPosition);
                    shadow.ScaleVec(p.time, p.column.receptor.ScaleAt(p.time));
                    shadow.Fade(p.time, 1);
                    if (p.time <= 158382)
                    {
                        shadow.Fade(158382, 0);
                        shadow.Rotate(OsbEasing.None, p.time, p.time + 1000, p.column.receptor.RotationAt(p.time), p.column.receptor.RotationAt(p.time) + Math.PI);
                    }
                    else
                    {
                        shadow.Fade(164575, 0);
                        shadow.Rotate(OsbEasing.None, p.time, p.time + 1000, p.column.receptor.RotationAt(p.time), p.column.receptor.RotationAt(p.time) + Math.PI);
                    }

                }
            }

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


            if (p.time > 176575 && p.note.starttime > 176575 && p.note.starttime < 181350)
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
