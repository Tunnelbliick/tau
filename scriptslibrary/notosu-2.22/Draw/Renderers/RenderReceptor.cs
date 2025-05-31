using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OpenTK;
using storyboard.scriptslibrary.maniaModCharts.effects;
using StorybrewCommon.Animations;
using StorybrewCommon.Storyboarding;

namespace StorybrewScripts
{
    public static class RenderReceptor
    {

        public static void Render(DrawInstance instance, Column column, double duration, bool enableShadow = false, Vector2 shadowOffset = new Vector2())
        {

            Playfield playfieldInstance = instance.playfieldInstance;
            bool hideNormalNotes = instance.hideNormalNotes;
            bool hideHolds = instance.hideHolds;
            bool rotateToFaceReceptor = instance.rotateToFaceReceptor;
            double starttime = instance.starttime;
            double endtime = starttime + duration;
            double easetime = instance.easetime;
            OsbEasing easing = instance.easing;
            double fadeInTime = instance.fadeInTime;
            double fadeOutTime = instance.fadeOutTime;

            KeyframedValue<Vector2> movement = new KeyframedValue<Vector2>(null);
            KeyframedValue<Vector2> movementHitLight = new KeyframedValue<Vector2>(null);
            KeyframedValue<Vector2> scale = column.receptor.Scale;
            KeyframedValue<double> rotation = new KeyframedValue<double>(null);

            double currentTime = starttime;
            double endTime = starttime + duration;
            double iterationLenght = 1000 / instance.updatesPerSecond;

            Receptor receptor = column.receptor;

            receptor.renderedSprite.Fade(starttime - 2500, 0);
            receptor.renderedSprite.Fade(starttime, 1);
            receptor.renderedSprite.Fade(endTime, 0);

            receptor.hit.Fade(starttime - 2500, 0);
            receptor.light.Fade(starttime - 2500, 0);

            if (instance.CommandSplitThreshold > 0)
            {
                receptor.renderedSprite.CommandSplitThreshold = instance.CommandSplitThreshold;
                receptor.light.CommandSplitThreshold = instance.CommandSplitThreshold;
                receptor.hit.CommandSplitThreshold = instance.CommandSplitThreshold;
            }


            double relativeTime = playfieldInstance.starttime;

            var pos = receptor.PositionAt(relativeTime);

            float x = pos.X;
            float y = pos.Y;

            while (relativeTime <= playfieldInstance.endtime)
            {
                Vector2 position = receptor.PositionAt(relativeTime);

                movement.Add(relativeTime, position);
                movementHitLight.Add(relativeTime, position);

                relativeTime += playfieldInstance.delta;
            }

            Dictionary<double, Note> notes = instance.playfieldInstance.columnNotes[column.type];

            movement.Simplify(instance.ReceptorMovementPrecision);
            movementHitLight.Simplify(instance.ReceptorMovementPrecision * 4);
            scale.Simplify(instance.ReceptorScalePrecision);

            scale.ForEachPair((start, end) =>
            {
                receptor.renderedSprite.ScaleVec(easing, start.Time, end.Time, start.Value, end.Value);
                receptor.light.ScaleVec(easing, start.Time, end.Time, start.Value * 2, end.Value * 2);
                receptor.hit.ScaleVec(easing, start.Time, end.Time, start.Value * 2, end.Value * 2);
            });

            movement.ForEachPair((start, end) =>
            {
                receptor.renderedSprite.Move(OsbEasing.None, start.Time, end.Time, start.Value, end.Value);
                //}
            });

            movementHitLight.ForEachPair((start, end) =>
            {
                receptor.light.Move(OsbEasing.None, start.Time, end.Time, start.Value, end.Value);
                receptor.hit.Move(OsbEasing.None, start.Time, end.Time, start.Value, end.Value);
            });

            if (enableShadow == true && playfieldInstance.shadowLayer != null)
            {
                var layer = playfieldInstance.shadowLayer;

                var shadow = layer.CreateSprite("sb/sprites/shadow.png", OsbOrigin.Centre, (Vector2)receptor.renderedSprite.PositionAt(starttime) + shadowOffset);
                shadow.Fade(starttime + 20, 0.75f);
                shadow.Fade(Math.Min(endTime, 50584), 0);

                KeyframedValue<Vector2> ShadowScale = new KeyframedValue<Vector2>(null);
                KeyframedValue<double> ShadowRotation = new KeyframedValue<double>(null);

                movement.ForEachPair((start, end) =>
                {
                    if (start.Time < 48596)
                    {
                        shadow.Move(OsbEasing.None, start.Time, end.Time, start.Value + shadowOffset, end.Value + shadowOffset);
                    }
                    else if (start.Time < 50584)
                    {
                        shadow.Move(OsbEasing.None, start.Time, end.Time, start.Value + shadowOffset, end.Value + shadowOffset);
                    }
                });
                var lastSscale = receptor.renderedSprite.ScaleAt(endTime) * 0.8f;
                var localTime = starttime;
                while (localTime < endTime && localTime < 50584)
                {
                    if (localTime < 48596)
                    {
                        ShadowScale.Add(localTime, (Vector2)receptor.renderedSprite.ScaleAt(localTime) * 0.8f);
                    }
                    ShadowRotation.Add(localTime, receptor.renderedSprite.RotationAt(localTime));
                    localTime += iterationLenght;
                }

                ShadowScale.Simplify(0.1);
                ShadowRotation.Simplify(0.15);

                ShadowScale.ForEachPair((start, end) =>
                {
                    shadow.ScaleVec(OsbEasing.None, start.Time, end.Time, start.Value, end.Value);
                });

                ShadowRotation.ForEachPair((start, end) =>
                {
                    shadow.Rotate(OsbEasing.None, start.Time, end.Time, start.Value, end.Value);
                });

                shadow.ScaleVec(OsbEasing.OutCubic, 48596, 50584, shadow.ScaleAt(48596), new Vector2(0));

            }



            var foundEntry = instance.findEffectByReferenceTime(currentTime);


            receptor.Render(currentTime, endTime);


            /*while (currentTime < endTime)
            {

                foundEntry = instance.findEffectByReferenceTime(currentTime);

                if (foundEntry.Value != null && foundEntry.Value.effektType == EffectType.TransformPlayfield3D)
                {
                    receptor.RenderTransformed(currentTime, endTime, foundEntry.Value.reference);
                }

                OsbSprite renderedReceptor = receptor.renderedSprite;

                FadeEffect receptorFade = instance.findFadeAtTime(currentTime);
                if (receptorFade != null)
                {
                    if (renderedReceptor.OpacityAt(currentTime) != receptorFade.value)
                        renderedReceptor.Fade(currentTime, receptorFade.value);
                }

                currentTime += iterationLenght;
            }*/


        }

    }
}