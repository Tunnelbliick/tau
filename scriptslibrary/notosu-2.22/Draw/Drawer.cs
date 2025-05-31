using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StorybrewCommon.Storyboarding;

namespace storyboard.scriptslibrary.maniaModCharts.Draw
{
    public class Drawer
    {

        public float NoteMovementPrecision = 2f;
        public float NoteScalePrecision = .01f;
        public float NoteRotationPrecision = 1f;
        public float NoteFadePrcision = 0f;

        public float ReceptorMovementPrecision = 2f;
        public float ReceptorScalePrecision = .01f;
        public float ReceptorRotationPrecision = 1f;

        public float HoldMovementPrecision = 2f;
        public float HoldScalePrecision = 0f;
        public float HoldRotationPrecision = 1f;

        public int CommandSplitThreshold = 0;

        public OsbEasing noteScaleEasing = OsbEasing.None;
        public OsbEasing holdScaleEasing = OsbEasing.None;

        public bool customScale = false;

        public float HoldRoationDeadzone = (float) Math.PI;

        public void setReceptorPrecision(float movement, float scale, float rotation)
        {
            this.ReceptorMovementPrecision = movement;
            this.ReceptorScalePrecision = scale;
            this.ReceptorRotationPrecision = rotation;
        }

        public void setReceptorMovementPrecision(float value)
        {
            this.ReceptorMovementPrecision = value;
        }

        public void setReceptorScalePrecision(float value)
        {
            this.ReceptorScalePrecision = value;
        }

        public void setReceptorRotationPrecision(float value)
        {
            this.ReceptorRotationPrecision = value;
        }

        public void setNotePrecision(float movement, float scale, float rotation, float fade)
        {
            this.NoteMovementPrecision = movement;
            this.NoteScalePrecision = scale;
            this.NoteRotationPrecision = rotation;
            this.NoteFadePrcision = fade;
        }

        public void setNoteMovementPrecision(float value)
        {
            this.NoteMovementPrecision = value;
        }

        public void setNoteScalePrecision(float value)
        {
            this.NoteScalePrecision = value;
        }

        public void setNoteRotationPrecision(float value)
        {
            this.NoteRotationPrecision = value;
        }

        public void setNoteFadePrecision(float value)
        {
            this.NoteFadePrcision = value;
        }

        public void setHoldPrecision(float movement, float scale, float rotation)
        {
            this.HoldMovementPrecision = movement;
            this.HoldScalePrecision = scale;
            this.HoldRotationPrecision = rotation;
        }

        public void setHoldMovementPrecision(float value)
        {
            this.HoldMovementPrecision = value;
        }

        public void setHoldScalePrecision(float value)
        {
            this.HoldScalePrecision = value;
        }

        public void setHoldRotationPrecision(float value)
        {
            this.HoldRotationPrecision = value;
        }

        public void setHoldRotationDeadZone(float value)
        {
            this.HoldRoationDeadzone = value;
        }
    }
}