using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OpenTK;
using storyboard.scriptslibrary.maniaModCharts.utility;
using StorybrewCommon.Storyboarding;
using StorybrewCommon.Storyboarding.CommandValues;

namespace StorybrewScripts
{
    public enum ColumnType
    {
        all = 0,
        one = 1,
        two = 2,
        three = 3,
        four = 4
    };


    public class Column
    {

        public ColumnType type;

        // note x cordinates for this column
        public double offset = 0f;

        public CommandScale scale;

        public Receptor receptor;
        public NoteOrigin origin;

        public SortedDictionary<int, double> bpmOffset = new SortedDictionary<int, double>();
        public SortedDictionary<int, double> bpm = new SortedDictionary<int, double>();


        public Column(double offset, ColumnType type, String receptorSpritePath, StoryboardLayer columnLayer, CommandScale scale, double starttime, double delta)
        {
            this.offset = offset;
            this.type = type;
            this.scale = scale;
            double rotation = 0f;

            switch (type)
            {
                case ColumnType.one:
                    rotation = Math.PI / 2;
                    break;
                case ColumnType.two:
                    rotation = 0;
                    break;
                case ColumnType.three:
                    rotation = Math.PI;
                    break;
                case ColumnType.four:
                    rotation = (Math.PI * 2) - Math.PI / 2;
                    break;
            }

            receptor = new Receptor(receptorSpritePath, rotation, columnLayer, scale, starttime, this.type, delta);
            origin = new NoteOrigin(receptorSpritePath, rotation, columnLayer, scale, starttime, delta);

        }

        // This methods sets the bpm for the receptor glint on full beats
        public void setBPM(double bpm, double bpmOffset)
        {
            this.bpm.Add((int)bpmOffset, bpm);
            this.bpmOffset.Add((int)bpmOffset, bpmOffset);

            receptor.bpm.Add((int)bpmOffset, bpm);
            receptor.bpmOffset.Add((int)bpmOffset, bpmOffset);

        }

        public void setBPM(int time, double bpm, double bpmOffset)
        {
            this.bpm.Add(time, bpm);
            this.bpmOffset.Add(time, bpmOffset);

            receptor.bpm.Add(time, bpm);
            receptor.bpmOffset.Add(time, bpmOffset);

        }

        public Vector2 ScaleColumn(OsbEasing easing, double starttime, double endtime, Vector2 newScale, CenterTypeColumn centerType = CenterTypeColumn.middle, bool invertMovement = false, float movementScale = 1)
        {
            Receptor receptor = this.receptor;
            NoteOrigin origin = this.origin;

            Vector2 center = new Vector2(320, 240); // Default center

            // Determine the center based on the CenterTypeColumn
            if (centerType == CenterTypeColumn.receptor)
            {
                center = receptor.PositionAt(endtime);
            }
            else if (centerType == CenterTypeColumn.column)
            {
                center = (receptor.PositionAt(endtime) + origin.PositionAt(endtime)) / 2;
            }
            else if (centerType == CenterTypeColumn.columnX)
            {
                center = new Vector2(receptor.PositionAt(endtime).X, 240); // Keep Y fixed at 240
            }

            Vector2 receptorPosition = receptor.PositionAt(endtime);
            Vector2 originPosition = origin.PositionAt(endtime);

            Vector2 receptorScale = receptor.ScaleAt(endtime);
            Vector2 originScale = origin.ScaleAt(endtime);

            receptor.ScaleReceptor(easing, starttime, endtime, newScale);
            origin.ScaleOrigin(easing, starttime, endtime, newScale);

            // Adjust positions to maintain scaling relative to the center
            float scaleRatioX = newScale.X / receptorScale.X;
            float scaleRatioY = newScale.Y / receptorScale.Y;

            Vector2 receptorNewPosition = new Vector2(
                center.X + (receptorPosition.X - center.X) * scaleRatioX,
                center.Y + (receptorPosition.Y - center.Y) * scaleRatioY);

            Vector2 originNewPosition = new Vector2(
                center.X + (originPosition.X - center.X) * scaleRatioX,
                center.Y + (originPosition.Y - center.Y) * scaleRatioY);

            Vector2 receptorMovement = receptorNewPosition - receptorPosition;
            Vector2 originMovement = originNewPosition - originPosition;

            if(invertMovement)
            {
                receptorMovement = -receptorMovement;
                originMovement = -originMovement;
            }

            receptor.MoveReceptorRelative(easing, starttime, endtime, receptorMovement * movementScale);
            origin.MoveOriginRelative(easing, starttime, endtime, originMovement * movementScale);

            return receptorMovement * movementScale;
        }

        public void MoveColumn(OsbEasing easing, double starttime, double endtime, Vector2 from, Vector2 to)
        {
            receptor.MoveReceptorAbsolute(easing, starttime, endtime, from, to);
            origin.MoveOriginAbsolute(easing, starttime, endtime, from, to);
        }

        public void MoveColumnRelative(OsbEasing easing, double starttime, double endtime, Vector2 offset)
        {
            receptor.MoveReceptorRelative(easing, starttime, endtime, offset);
            origin.MoveOriginRelative(easing, starttime, endtime, offset);
        }

        public void MoveColumnRelativeX(OsbEasing easing, double starttime, double endtime, float value)
        {
            receptor.MoveReceptorRelativeX(easing, starttime, endtime, value);
            origin.MoveOriginRelativeX(easing, starttime, endtime, value);
        }

        public void MoveColumnRelativeY(OsbEasing easing, double starttime, double endtime, float value)
        {
            receptor.MoveReceptorRelativeY(easing, starttime, endtime, value);
            origin.MoveOriginRelativeY(easing, starttime, endtime, value);
        }

        public void MoveReceptorAbsolute(double starttime, Vector2 newReceptorPosition)
        {
            receptor.MoveReceptorAbsolute(starttime, newReceptorPosition);
        }

        public void MoveReceptorAbsolute(OsbEasing ease, double starttime, double endtime, Vector2 startPos, Vector2 endPos)
        {
            receptor.MoveReceptorAbsolute(ease, starttime, endtime, startPos, endPos);
        }

        public void MoveReceptorRelative(OsbEasing easing, double starttime, double endtime, Vector2 offset)
        {
            receptor.MoveReceptorRelative(easing, starttime, endtime, offset);
        }

        public void MoveReceptorRelativeNorm(OsbEasing easing, double starttime, double endtime, Vector2 offset)
        {
            receptor.MoveReceptorRelativeNorm(easing, starttime, endtime, offset, origin);
        }

        public void RotateReceptorRelative(OsbEasing easing, double starttime, double endtime, double rotation)
        {
            receptor.RotateReceptor(easing, starttime, endtime, rotation);
        }

        public void RotateReceptor(OsbEasing easing, double starttime, double endtime, double rotation)
        {
            receptor.RotateReceptorAbsolute(easing, starttime, endtime, rotation);
        }

        public void MoveOriginAbsoluite(double starttime, Vector2 newOriginPosition)
        {

            origin.MoveOriginAbsolute(starttime, newOriginPosition);
        }

        public void MoveOriginAbsoluite(OsbEasing ease, double starttime, double endtime, Vector2 startPos, Vector2 endPos)
        {

            origin.MoveOriginAbsolute(ease, starttime, endtime, startPos, endPos);
        }

        public void MoveOriginRelative(OsbEasing ease, double starttime, double endtime, Vector2 offset)
        {

            origin.MoveOriginRelative(ease, starttime, endtime, offset);
        }

        public void MoveOriginRelativeNorm(OsbEasing easing, double starttime, double endtime, Vector2 offset)
        {
            origin.MoveOriginRelativeNorm(easing, starttime, endtime, offset, receptor);
        }

        public Vector2 OriginPositionAt(double starttime)
        {
            return origin.PositionAt(starttime);
        }

        public float OriginRotationAt(double starttime)
        {
            return origin.RotationAt(starttime);
        }

        public Vector2 OriginScaleAt(double starttime)
        {
            return origin.ScaleAt(starttime);
        }

        public Vector2 ReceptorPositionAt(double starttime)
        {
            return receptor.PositionAt(starttime);
        }

        public double ReceptorRotationAt(double starttime)
        {
            return receptor.RotationAt(starttime);
        }

        public Vector2 ReceptorScaleAt(double starttime)
        {
            return receptor.ScaleAt(starttime);
        }


    }
}