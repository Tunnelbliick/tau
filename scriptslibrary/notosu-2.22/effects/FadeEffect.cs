using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StorybrewCommon.Storyboarding;
using StorybrewScripts;

namespace storyboard.scriptslibrary.maniaModCharts.effects
{
    public class FadeEffect
    {
        public double starttime;
        public double endtime;
        public OsbEasing easing;
        public float value;
        public ColumnType columnType;

        public FadeEffect(double starttime, double endtime, OsbEasing easing, float value)
        {
            this.starttime = starttime;
            this.endtime = endtime;
            this.easing = easing;
            this.value = value;
            this.columnType = ColumnType.all;
        }

        public FadeEffect(double starttime, double endtime, OsbEasing easing, float value, ColumnType columnType)
        {
            this.starttime = starttime;
            this.endtime = endtime;
            this.easing = easing;
            this.value = value;
            this.columnType = columnType;
        }

    }
}