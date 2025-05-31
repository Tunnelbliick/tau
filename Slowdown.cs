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
    public class Slowdown : StoryboardObjectGenerator
    {
//        public override bool Multithreaded => false;
        Playfield field = new Playfield();
        public override void Generate()
        {

            var receptors = GetLayer("r");
            var notes = GetLayer("n");

            // General values
            var starttime = 219831; // the starttime where the playfield is initialized
            var endtime = 234229; // the endtime where the playfield is nolonger beeing rendered
            var duration = endtime - starttime; // the length the playfield is kept alive

            // Playfield Scale
            var width = 250f; // widht of the playfield / invert to flip
            var height = 700f; // height of the playfield / invert to flip -600 = downscroll | 600 = upscropll
            var receptorWallOffset = 50f; // how big the boundary box for the receptor is 50 means it will be pushed away 50 units from the wall

            // Note initilization Values
            var sliderAccuracy = 30; // The Segment length for sliderbodies since they are rendered in slices 30 is default
            var isColored = false; // This property is used if you want to color the notes by urself for effects. It does not swap if the snap coloring is used.

            // Drawinstance Values
            var updatesPerSecond = 250; // The amount of steps the rendring engine does to render out note and receptor positions
            var scrollSpeed = 1200f / 500f * 700f; // The speed at which the Notes scroll
            var fadeTime = 150; // The time notes will fade in

            field.initilizePlayField(receptors, notes, starttime, endtime, width, height, receptorWallOffset, Beatmap.OverallDifficulty);
            field.noteStart = 206188;
            field.noteEnd = 235636;
            field.initializeNotes(Beatmap.HitObjects.ToList(), Beatmap, isColored, sliderAccuracy);

            field.MoveOriginRelative(OsbEasing.InOutSine, 219930, 226732, new Vector2(0, -200), ColumnType.all);

            field.MoveOriginRelative(OsbEasing.InOutSine, 228586, 232354, new Vector2(0, 200), ColumnType.all);


            field.Rotate(OsbEasing.InOutExpo, 219929, 223143, Math.PI);
            field.Rotate(OsbEasing.InOutQuint, 224012 - 500, 227198, Math.PI);

            field.Resize(OsbEasing.OutSine, 221600, 222262, width, height - 200);

            field.Resize(OsbEasing.InOutQuint, 222262, 224124, -width, height - 200);

            ReceptorBump(226732);
            ReceptorBump(227198);

            ReceptorBump(227667, 2.25f);
            ReceptorBump(228136, 2.25f);

            ReceptorBump(228604, 2.5f);
            ReceptorBump(229073, 2.5f);

            ReceptorBump(229542, 2.75f);
            ReceptorBump(230011, 2.75f);

            ReceptorBump(230479, 3f, 230714 - 230479 - 10);
            OriginBump(230714, 3f, 230714 - 230479 - 10);

            ReceptorBump(230948, 3.5f, 230714 - 230479 - 10);
            OriginBump(231183, 3.5f, 230714 - 230479 - 10);

            ReceptorBump(231417, 4f, 230714 - 230479 - 10);
            OriginBump(231651, 4, 230714 - 230479 - 10);

            ReceptorBump(231886, 4f, 230714 - 230479 - 10);
            OriginBump(232120, 4, 230714 - 230479 - 10);

            ReceptorBump(232354, 5f, 600f);

            double gap = 43801 - 43414;
            field.MoveColumnRelativeX(OsbEasing.OutCubic, 226732, 226732 + gap, -field.getColumnWidth(), ColumnType.one);
            field.MoveColumnRelativeX(OsbEasing.OutCubic, 226732, 226732 + gap, field.getColumnWidth(), ColumnType.two);
            field.MoveColumnRelativeX(OsbEasing.OutCubic, 226732, 226732 + gap, -field.getColumnWidth(), ColumnType.three);
            field.MoveColumnRelativeX(OsbEasing.OutCubic, 226732, 226732 + gap, field.getColumnWidth(), ColumnType.four);

            field.MoveColumnRelativeX(OsbEasing.OutCubic, 227198, 227198 + gap, field.getColumnWidth(), ColumnType.one);
            field.MoveColumnRelativeX(OsbEasing.OutCubic, 227198, 227198 + gap, -field.getColumnWidth(), ColumnType.two);
            field.MoveColumnRelativeX(OsbEasing.OutCubic, 227198, 227198 + gap, field.getColumnWidth(), ColumnType.three);
            field.MoveColumnRelativeX(OsbEasing.OutCubic, 227198, 227198 + gap, -field.getColumnWidth(), ColumnType.four);

            field.MoveColumnRelativeX(OsbEasing.OutCubic, 227667, 227667 + gap, -field.getColumnWidth(), ColumnType.one);
            field.MoveColumnRelativeX(OsbEasing.OutCubic, 227667, 227667 + gap, field.getColumnWidth(), ColumnType.two);
            field.MoveColumnRelativeX(OsbEasing.OutCubic, 227667, 227667 + gap, -field.getColumnWidth(), ColumnType.three);
            field.MoveColumnRelativeX(OsbEasing.OutCubic, 227667, 227667 + gap, field.getColumnWidth(), ColumnType.four);

            field.MoveColumnRelativeX(OsbEasing.OutCubic, 228136, 228136 + gap, field.getColumnWidth(), ColumnType.one);
            field.MoveColumnRelativeX(OsbEasing.OutCubic, 228136, 228136 + gap, -field.getColumnWidth(), ColumnType.two);
            field.MoveColumnRelativeX(OsbEasing.OutCubic, 228136, 228136 + gap, field.getColumnWidth(), ColumnType.three);
            field.MoveColumnRelativeX(OsbEasing.OutCubic, 228136, 228136 + gap, -field.getColumnWidth(), ColumnType.four);

            field.MoveColumnRelativeX(OsbEasing.OutCubic, 228604, 228604 + gap, -field.getColumnWidth(), ColumnType.one);
            field.MoveColumnRelativeX(OsbEasing.OutCubic, 228604, 228604 + gap, field.getColumnWidth(), ColumnType.two);
            field.MoveColumnRelativeX(OsbEasing.OutCubic, 228604, 228604 + gap, -field.getColumnWidth(), ColumnType.three);
            field.MoveColumnRelativeX(OsbEasing.OutCubic, 228604, 228604 + gap, field.getColumnWidth(), ColumnType.four);

            field.MoveColumnRelativeX(OsbEasing.OutCubic, 229073, 229073 + gap, field.getColumnWidth(), ColumnType.one);
            field.MoveColumnRelativeX(OsbEasing.OutCubic, 229073, 229073 + gap, -field.getColumnWidth(), ColumnType.two);
            field.MoveColumnRelativeX(OsbEasing.OutCubic, 229073, 228136 + gap, field.getColumnWidth(), ColumnType.three);
            field.MoveColumnRelativeX(OsbEasing.OutCubic, 229073, 229073 + gap, -field.getColumnWidth(), ColumnType.four);

            field.MoveColumnRelativeX(OsbEasing.OutCubic, 229542, 229542 + gap, -field.getColumnWidth(), ColumnType.one);
            field.MoveColumnRelativeX(OsbEasing.OutCubic, 229542, 229542 + gap, field.getColumnWidth(), ColumnType.two);
            field.MoveColumnRelativeX(OsbEasing.OutCubic, 229542, 229542 + gap, -field.getColumnWidth(), ColumnType.three);
            field.MoveColumnRelativeX(OsbEasing.OutCubic, 229542, 229542 + gap, field.getColumnWidth(), ColumnType.four);

            field.MoveColumnRelativeX(OsbEasing.OutCubic, 230011, 230011 + gap, field.getColumnWidth(), ColumnType.one);
            field.MoveColumnRelativeX(OsbEasing.OutCubic, 230011, 230011 + gap, -field.getColumnWidth(), ColumnType.two);
            field.MoveColumnRelativeX(OsbEasing.OutCubic, 230011, 230011 + gap, field.getColumnWidth(), ColumnType.three);
            field.MoveColumnRelativeX(OsbEasing.OutCubic, 230011, 230011 + gap, -field.getColumnWidth(), ColumnType.four);

            field.RotatePlayFieldStatic(OsbEasing.None, 233526, 233526, Math.PI / 3);
            field.RotatePlayFieldStatic(OsbEasing.None, 233644, 233644, Math.PI / 3);
            field.RotatePlayFieldStatic(OsbEasing.None, 233761, 233761, Math.PI / 3);

            field.Rotate(OsbEasing.InExpo, 233292, 234229, Math.PI, CenterType.middle);

            DrawInstance draw = new DrawInstance(CancellationToken, field, 219866, scrollSpeed, updatesPerSecond, OsbEasing.None, false, fadeTime, fadeTime);
            draw.setReceptorMovementPrecision(0.25f);
            draw.setNoteRotationPrecision(0.1f);
            draw.setNoteMovementPrecision(0.25f);
            draw.setHoldRotationPrecision(0.01f);
            draw.setHoldMovementPrecision(0.25f);
            draw.drawViaEquation(endtime - 219866, NoteFunction, true);

        }

        public void ReceptorBump(double localtime, float diff = 2, double gap = 43801 - 43414)
        {

            if (localtime < 230479)
            {
                field.RotatePlayFieldStatic(OsbEasing.None, localtime - 10, localtime - 10, Math.PI);
                field.RotatePlayFieldStatic(OsbEasing.OutCubic, localtime, localtime + gap, Math.PI);
            }

            float defaultScale = 0.5f;
            float smallScale = defaultScale / diff;
            float bigScale = defaultScale * diff;

            var colWidth = -field.getColumnWidth() * 2 / 2 * diff;
            var halfwidth = -field.getColumnWidth() / 2 / 2 * diff;

            field.ScaleReceptor(OsbEasing.None, localtime, localtime, new Vector2(bigScale), ColumnType.all);
            field.ScaleOrigin(OsbEasing.None, localtime, localtime, new Vector2(smallScale), ColumnType.all);

            field.ScaleReceptor(OsbEasing.OutCubic, localtime, localtime + gap, new Vector2(defaultScale), ColumnType.all);
            field.ScaleOrigin(OsbEasing.OutCubic, localtime, localtime + gap, new Vector2(defaultScale), ColumnType.all);

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
        }

        public void OriginBump(double localtime, float diff = 2, double gap = 43801 - 43414)
        {
            if (localtime < 230479)
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

        public Vector2 NoteFunction(EquationParameters p)
        {
            return p.position;
        }
    }
}
