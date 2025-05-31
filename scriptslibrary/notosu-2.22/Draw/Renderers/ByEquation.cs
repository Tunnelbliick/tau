using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using storyboard.scriptslibrary.maniaModCharts.effects;
using StorybrewCommon.Animations;
using StorybrewCommon.Storyboarding;

namespace StorybrewScripts
{

    public class EquationParameters
    {
        public Vector2 position;
        public Vector2 lastPosition;
        public double time;
        public float progress;
        public OsbSprite sprite;

        public SliderParts part;
        public Note note;
        public Column column;
        public bool isHoldBody = false;
    }

    public delegate Vector2 EquationFunction(EquationParameters parameters);

    public class ByEquation
    {

        public static string drawViaEquation(DrawInstance instance, double duration, EquationFunction equation, bool renderReceptor = true, bool enableShadow = false, Vector2 shadowOffset = new Vector2())
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
            string debug = "";

            var parallelOptions = new ParallelOptions
            {
                CancellationToken = instance.token
            };


            //foreach (var column in playfieldInstance.columns.Values)
            //{
            Parallel.ForEach(playfieldInstance.columns.Values, parallelOptions, column =>
            {

                if (renderReceptor)
                    RenderReceptor.Render(instance, column, duration, enableShadow, shadowOffset);

                RenderOrigin.Render(instance, column);

                Dictionary<double, Note> notes = playfieldInstance.columnNotes[column.type];
                var keysInRange = notes.Keys.Where(hittime => hittime >= starttime && hittime - easetime <= endtime).ToList();
                var hitlight = notes.Keys.Where(hittime => hittime >= starttime && hittime <= endtime).ToList();

                HitSound.AddHitSound(playfieldInstance, column, hitlight, notes);

                //foreach (var key in keysInRange)
                //{
                Parallel.ForEach(keysInRange, parallelOptions, key =>
                     {
                         KeyframedValue<Vector2> movement = new KeyframedValue<Vector2>(null);
                         KeyframedValue<Vector2> scale = new KeyframedValue<Vector2>(null);
                         KeyframedValue<float> rotation = new KeyframedValue<float>(null);

                         Note note = notes[key];

                         if (note.isSlider == false && hideNormalNotes)
                         {
                             return;
                         }

                         if (note.isSlider == true && hideHolds)
                         {
                             return;
                         }

                         double totalDuration = easetime;

                         double localIterationRate = instance.findCurrentUpdateRate(note.starttime - easetime);

                         double currentTime = note.starttime - easetime - localIterationRate;
                         double renderStartTime = Math.Max(currentTime, starttime);
                         double renderEndTime = Math.Min(note.endtime, endtime);
                         Vector2 currentPosition = column.origin.originSprite.PositionAt(currentTime);
                         float progress = 0f;
                         double iteratedTime = 0;
                         float initialFade = 1;
                         double lastFadeTime = -1;
                         note.invisible(currentTime - 1);
                         if (playfieldInstance.isColored)
                             note.Color(currentTime - 1, instance.color);

                         FadeEffect noteFade = instance.findFadeAtTime(currentTime);
                         if (noteFade != null)
                         {
                             initialFade = noteFade.value;
                         }

                         var currentEffect = instance.findEffectByReferenceTime(currentTime);

                         // currently defunc
                         /*if (currentEffect.Value != null && currentEffect.Value.effektType == EffectType.TransformPlayfield3D)
                         {
                             note.RenderTransformed(renderStartTime, renderEndTime, currentEffect.Value.reference, fadeInTime, fadeOutTime);
                         }*/
                         if (currentEffect.Value == null || (currentEffect.Value != null && currentEffect.Value.effektType != EffectType.RenderPlayFieldFrom))
                         {
                             note.Render(renderStartTime, renderEndTime, easing, initialFade, fadeInTime, fadeOutTime);
                         }

                         if (instance.customScale == false)
                             note.Scale(currentTime, currentTime, OsbEasing.None, column.origin.ScaleAt(currentTime), column.origin.ScaleAt(currentTime));

                         double startRotation = column.receptor.RotationAt(currentTime);
                         rotation.Add(currentTime, (float)startRotation, EasingFunctions.ToEasingFunction(easing));

                         do
                         {

                             instance.token.ThrowIfCancellationRequested();

                             if (currentTime > endtime)
                             {
                                 break;
                             }

                             noteFade = instance.findFadeAtTime(currentTime + localIterationRate);
                             if (noteFade != null && lastFadeTime != noteFade.starttime && noteFade.starttime > renderStartTime && noteFade.starttime < renderEndTime)
                             {
                                 lastFadeTime = noteFade.starttime;

                                 // If the note ends before the fade is supposed to finish
                                 if (note.renderEnd - 1 < noteFade.endtime)
                                 {
                                     // Calculate the progress of the fade at renderEnd time
                                     double fadeDuration = noteFade.endtime - noteFade.starttime;
                                     double actualDuration = note.renderEnd - 1 - noteFade.starttime;
                                     double p = actualDuration / fadeDuration;

                                     // Get the current opacity value
                                     float startOpacity = note.noteSprite.OpacityAt(noteFade.starttime);

                                     // Calculate the eased value at the shorter duration
                                     float easedValue;
                                     if (noteFade.easing == OsbEasing.None)
                                     {
                                         // Linear interpolation for None easing
                                         easedValue = startOpacity + (float)(p * (noteFade.value - startOpacity));
                                     }
                                     else
                                     {
                                         easedValue = startOpacity + (float)(noteFade.easing.Ease(p) * (noteFade.value - startOpacity));
                                     }

                                     note.Fade(noteFade.starttime, note.renderEnd - 1, OsbEasing.None, easedValue);
                                 }
                                 else
                                 {
                                     // Normal fade as before
                                     note.Fade(noteFade.starttime, noteFade.endtime, noteFade.easing, noteFade.value);
                                 }
                             }

                             double timeleft = note.starttime - currentTime;
                             double elapsedTime = currentTime - note.starttime;

                             currentEffect = instance.findEffectByReferenceTime(currentTime + localIterationRate);

                             // currently defunc
                             /*if (currentEffect.Value != null && currentEffect.Value.effektType == EffectType.TransformPlayfield3D)
                             {
                                 note.UpdateTransformed(currentTime, currentTime + localIterationRate, currentEffect.Value.reference, 10);
                             }*/

                             progress = Math.Min((float)(iteratedTime / easetime), 1);

                             // Apply easing to the progress
                             float easedProgress = (float)easing.Ease(progress);

                             if (currentEffect.Value != null && currentEffect.Value.effektType == EffectType.RenderPlayFieldFrom)
                             {
                                 if (easedProgress < currentEffect.Value.value)
                                 {

                                     Vector2 originPositionLocal = column.origin.originSprite.PositionAt(currentTime + localIterationRate);
                                     Vector2 receptorPositionLocal = column.receptor.renderedSprite.PositionAt(currentTime + localIterationRate);
                                     Vector2 newPositionLocal = Vector2.Lerp(originPositionLocal, receptorPositionLocal, easedProgress);

                                     note.Fade(currentTime, currentTime, OsbEasing.None, 0);
                                     iteratedTime += localIterationRate;
                                     currentTime += localIterationRate;
                                     currentPosition = newPositionLocal;
                                     continue;
                                 }
                                 else
                                 {
                                     float fade = 1;
                                     if (noteFade != null)
                                     {
                                         fade = noteFade.value;
                                     }
                                     note.Fade(currentTime + localIterationRate, currentTime + localIterationRate + fadeInTime, OsbEasing.None, fade);
                                 }
                             }

                             if (currentEffect.Value != null && currentEffect.Value.effektType == EffectType.RenderPlayFieldUntil)
                             {
                                 if (currentEffect.Value.value == 0)
                                 {
                                     note.Fade(currentTime, currentTime, OsbEasing.None, 0);
                                 }
                                 if (easedProgress > currentEffect.Value.value)
                                 {
                                     note.Fade(currentTime, currentTime + fadeOutTime, OsbEasing.None, 0);
                                 }
                             }

                             Vector2 originPosition = column.origin.originSprite.PositionAt(currentTime + localIterationRate);
                             Vector2 receptorPosition = column.receptor.renderedSprite.PositionAt(currentTime + localIterationRate);
                             Vector2 newPosition = Vector2.Lerp(originPosition, receptorPosition, easedProgress);

                             var par = new EquationParameters
                             {
                                 position = newPosition,
                                 lastPosition = currentPosition,
                                 time = currentTime,
                                 progress = easedProgress,
                                 sprite = note.noteSprite,
                                 note = note,
                                 column = column,
                                 isHoldBody = false,
                             };

                             newPosition = equation(par);
                             Vector2 originScale = column.origin.ScaleAt(currentTime + localIterationRate);
                             Vector2 receptorScale = column.receptor.ScaleAt(currentTime + localIterationRate);
                             Vector2 scaleProgress = Vector2.Lerp(originScale, receptorScale, (float)instance.noteScaleEasing.Ease(progress));
                             debug += receptorScale;
                             startRotation = column.receptor.RotationAt(currentTime + localIterationRate);

                             double theta = 0;
                             if (rotateToFaceReceptor)
                             {
                                 Vector2 delta = receptorPosition - currentPosition;
                                 if (currentPosition.Y > receptorPosition.Y)
                                 {
                                     delta = -delta;
                                 }
                                 theta = Math.Atan2(delta.X, delta.Y);
                             }

                             movement.Add(currentTime + localIterationRate, newPosition, EasingFunctions.ToEasingFunction(easing));
                             if (instance.customScale == false)
                                 scale.Add(currentTime + localIterationRate, scaleProgress, EasingFunctions.ToEasingFunction(easing));
                             rotation.Add(currentTime + localIterationRate, (float)(startRotation - theta), EasingFunctions.ToEasingFunction(easing));

                             iteratedTime += localIterationRate;
                             currentTime += localIterationRate;
                             currentPosition = newPosition;

                         } while (progress < 1);

                         //foreach (var part in note.sliderPositions)
                         //{
                         Parallel.ForEach(note.sliderPositions, parallelOptions, part =>
                              {

                                  KeyframedValue<Vector2> SliderMovement = new KeyframedValue<Vector2>(null);
                                  KeyframedValue<Vector2> SliderScale = new KeyframedValue<Vector2>(null);
                                  KeyframedValue<double> SliderRotation = new KeyframedValue<double>(null);

                                  double sliderIterationLenght = instance.findCurrentUpdateRate(part.Timestamp - easetime);

                                  double sliderStartime = part.Timestamp;
                                  OsbSprite sprite = part.Sprite;
                                  double sliderCurrentTime = sliderStartime - easetime - sliderIterationLenght;
                                  Vector2 currentSliderPositon = column.origin.originSprite.PositionAt(sliderCurrentTime);
                                  double sliderRenderStartTime = Math.Max(sliderStartime - easetime, starttime);
                                  double sliderRenderEndTime = Math.Min(sliderStartime, endtime);
                                  float sliderProgress = 0;
                                  double sliderIteratedTime = 0;
                                  sprite.Fade(sliderCurrentTime, 0);

                                  FadeEffect sliderFade = instance.findFadeAtTime(sliderRenderStartTime);
                                  if (sliderFade != null)
                                  {
                                      sprite.Fade(sliderRenderStartTime, sliderRenderStartTime + fadeInTime, 0, sliderFade.value);
                                  }
                                  else
                                  {
                                      sprite.Fade(sliderRenderStartTime, sliderRenderStartTime + fadeInTime, 0, 1);
                                  }

                                  sprite.Fade(sliderRenderEndTime, 0);
                                  double sliderRotation = sprite.RotationAt(sliderCurrentTime);

                                  float defaultScaleX = 0.7f / 0.5f;
                                  float defaultScaleY = 0.15f / 0.5f * ((float)part.Duration / 20f); // This scaled was based on 20ms long sliderParts

                                  Vector2 newScale = new Vector2(defaultScaleX * column.origin.ScaleAt(sliderCurrentTime).X, defaultScaleY * column.origin.ScaleAt(sliderCurrentTime).Y);

                                  SliderMovement.Add(sliderCurrentTime, currentSliderPositon, EasingFunctions.ToEasingFunction(easing));
                                  if (instance.customScale == false)
                                      SliderScale.Add(sliderCurrentTime, newScale, EasingFunctions.ToEasingFunction(easing));

                                  bool hasStartedRendering = false;
                                  float prevTheta = 0;  // You need to store the previous theta between calls

                                  do
                                  {
                                      instance.token.ThrowIfCancellationRequested();

                                      if (sliderCurrentTime > endtime)
                                      {
                                          break;
                                      }

                                      sliderFade = instance.findFadeAtTime(sliderCurrentTime + sliderIterationLenght);
                                      if (sliderFade != null && lastFadeTime != sliderFade.starttime && sliderFade.starttime > renderStartTime && sliderFade.starttime < renderEndTime)
                                      {
                                          lastFadeTime = sliderFade.starttime;

                                          // If the note ends before the fade is supposed to finish
                                          if (sliderRenderEndTime - 1 < sliderFade.endtime)
                                          {
                                              // Calculate the progress of the fade at renderEnd time
                                              double fadeDuration = sliderFade.endtime - sliderFade.starttime;
                                              double actualDuration = sliderRenderEndTime - 1 - sliderFade.starttime;
                                              double p = actualDuration / fadeDuration;

                                              // Get the current opacity value
                                              float startOpacity = sprite.OpacityAt(sliderFade.starttime);

                                              // Calculate the eased value at the shorter duration
                                              float easedValue;
                                              if (sliderFade.easing == OsbEasing.None)
                                              {
                                                  // Linear interpolation for None easing
                                                  easedValue = startOpacity + (float)(p * (sliderFade.value - startOpacity));
                                              }
                                              else
                                              {
                                                  easedValue = startOpacity + (float)(sliderFade.easing.Ease(p) * (sliderFade.value - startOpacity));
                                              }

                                              sprite.Fade(sliderFade.starttime, sliderRenderEndTime - 1, startOpacity, easedValue);
                                          }
                                          else
                                          {
                                              // Normal fade as before
                                              sprite.Fade(sliderFade.easing, sliderFade.starttime, sliderFade.endtime, sprite.OpacityAt(sliderFade.starttime), sliderFade.value);
                                          }
                                      }

                                      double timeleft = sliderStartime - sliderCurrentTime;
                                      sliderProgress = Math.Min((float)(sliderIteratedTime / easetime), 1);
                                      float sliderProgressNext = Math.Min((float)((sliderIteratedTime + sliderIterationLenght) / easetime), 1);

                                      debug += sliderProgressNext;

                                      // Apply easing to the progress
                                      float easedProgress = (float)easing.Ease(sliderProgress);
                                      float easedNextProgress = (float)easing.Ease(sliderProgressNext);

                                      if (currentEffect.Value != null && currentEffect.Value.effektType == EffectType.RenderPlayFieldFrom)
                                      {
                                          if (easedProgress < currentEffect.Value.value)
                                          {

                                              Vector2 originPositionLocal = column.origin.originSprite.PositionAt(sliderCurrentTime + sliderIterationLenght);
                                              Vector2 receptorPositionLocal = column.receptor.renderedSprite.PositionAt(sliderCurrentTime + sliderIterationLenght);
                                              Vector2 newPositionLocal = Vector2.Lerp(originPositionLocal, receptorPositionLocal, easedProgress);

                                              sprite.Fade(sliderCurrentTime, 0);
                                              sliderIteratedTime += sliderIterationLenght;
                                              sliderCurrentTime += sliderIterationLenght;
                                              currentSliderPositon = newPositionLocal;
                                          }
                                          else if (hasStartedRendering == false)
                                          {
                                              hasStartedRendering = true;
                                              float fade = 1;
                                              if (noteFade != null)
                                              {
                                                  fade = noteFade.value;
                                              }
                                              sprite.Fade(sliderCurrentTime + sliderIterationLenght, sliderCurrentTime + sliderIterationLenght + fadeInTime, 0, fade);
                                          }
                                      }

                                      if (currentEffect.Value != null && currentEffect.Value.effektType == EffectType.RenderPlayFieldUntil)
                                      {
                                          if (easedProgress > currentEffect.Value.value)
                                          {
                                              sprite.Fade(sliderCurrentTime, sliderCurrentTime + fadeOutTime, sprite.OpacityAt(currentTime), 0);
                                              break;
                                          }
                                      }

                                      Vector2 originPosition = column.origin.originSprite.PositionAt(sliderCurrentTime + sliderIterationLenght);
                                      Vector2 receptorPosition = column.receptor.renderedSprite.PositionAt(sliderCurrentTime + sliderIterationLenght);
                                      Vector2 newPosition = Vector2.Lerp(originPosition, receptorPosition, easedProgress);
                                      Vector2 nextPosition = Vector2.Lerp(originPosition, receptorPosition, easedNextProgress);

                                      var par = new EquationParameters
                                      {
                                          position = newPosition,
                                          lastPosition = currentSliderPositon,
                                          time = sliderCurrentTime,
                                          progress = easedProgress,
                                          sprite = sprite,
                                          note = note,
                                          part = part,
                                          column = column,
                                          isHoldBody = true
                                      };

                                      var parNext = new EquationParameters
                                      {
                                          position = nextPosition,
                                          lastPosition = currentSliderPositon,
                                          time = sliderCurrentTime,
                                          progress = easedNextProgress,
                                          sprite = null,
                                          note = note,
                                          column = column,
                                          part = part,
                                          isHoldBody = true
                                      };

                                      newPosition = equation(par);
                                      nextPosition = equation(parNext);

                                      Vector2 receptorScale = column.receptor.ScaleAt(sliderCurrentTime + sliderIterationLenght);
                                      Vector2 originScale = column.origin.ScaleAt(sliderCurrentTime + sliderIterationLenght);

                                      Vector2 scaleProgress = Vector2.Lerp(originScale, receptorScale, (float)instance.holdScaleEasing.Ease(sliderProgress));
                                      Vector2 renderedReceptorPosition = column.receptor.renderedSprite.PositionAt(sliderCurrentTime);


                                      float theta = 0;

                                      // Calculate the current theta
                                      Vector2 delta = new Vector2(nextPosition.X - newPosition.X, nextPosition.Y - newPosition.Y);
                                      float newTheta = (float)Math.Round(Math.Atan2(delta.X, delta.Y), 5);

                                      // Check if the difference between the new theta and the previous theta exceeds 180°
                                      float difference = newTheta - prevTheta;
                                      while (difference > Math.PI) difference -= 2f * (float)Math.PI;
                                      while (difference < -Math.PI) difference += 2f * (float)Math.PI;

                                      theta = newTheta;
                                      prevTheta = newTheta;

                                      float xScale = column.type == ColumnType.one || column.type == ColumnType.four ? scaleProgress.Y : scaleProgress.X;
                                      float yScale = column.type == ColumnType.one || column.type == ColumnType.four ? scaleProgress.X : scaleProgress.Y;
                                      newScale = new Vector2(defaultScaleX * xScale, defaultScaleY * yScale);

                                      SliderMovement.Add(sliderCurrentTime + sliderIterationLenght, newPosition);
                                      if (instance.customScale == false)
                                          SliderScale.Add(sliderCurrentTime + sliderIterationLenght, newScale);

                                      if (nextPosition != newPosition && sliderCurrentTime > 261417 || Math.Abs(theta) < instance.HoldRoationDeadzone)
                                      {
                                          // Apply rotation
                                          SliderRotation.Add(sliderCurrentTime + sliderIterationLenght, -theta);
                                      }

                                      sliderIteratedTime += sliderIterationLenght;
                                      sliderCurrentTime += sliderIterationLenght;
                                      currentSliderPositon = newPosition;

                                  } while (sliderProgress < 1);

                                  // Render out Hold keyframes
                                  SliderMovement.Simplify(instance.HoldMovementPrecision);
                                  SliderScale.Simplify(instance.HoldScalePrecision);
                                  SliderRotation.Simplify(instance.HoldRotationPrecision);
                                  if (instance.customScale == false)
                                      SliderScale.ForEachPair((start, end) => sprite.ScaleVec(start.Time, end.Time, start.Value.X, start.Value.Y, end.Value.X, end.Value.Y));
                                  SliderRotation.ForEachPair((start, end) => sprite.Rotate(OsbEasing.None, start.Time, end.Time, start.Value, end.Value));
                                  SliderMovement.ForEachPair((start, end) => sprite.Move(OsbEasing.None, start.Time, end.Time, start.Value, end.Value));
                              });


                         // Render out Note keyframes
                         movement.Simplify(instance.NoteMovementPrecision);
                         scale.Simplify(instance.NoteScalePrecision);
                         rotation.Simplify(instance.NoteRotationPrecision);


                         movement.ForEachPair((start, end) => note.Move(start.Time, end.Time - start.Time, OsbEasing.None, start.Value, end.Value));
                         if (instance.customScale == false)
                             scale.ForEachPair((start, end) => note.Scale(start.Time, end.Time, OsbEasing.None, start.Value, end.Value));
                         rotation.ForEachPair((start, end) => note.Rotate(start.Time, end.Time - start.Time, OsbEasing.None, start.Value, end.Value));

                         if (enableShadow == true && playfieldInstance.shadowLayer != null)  // Removed renderEndTime < 50584 check
                         {
                             var layer = playfieldInstance.shadowLayer;

                             var shadow = layer.CreateSprite("sb/sprites/shadow.png", OsbOrigin.Centre, (Vector2)note.noteSprite.PositionAt(renderStartTime + 100) + shadowOffset);
                             shadow.Fade(renderStartTime, 0);
                             shadow.Fade(renderStartTime + 100, renderStartTime + 100 + fadeInTime, 0, 0.75f);
                             shadow.Fade(Math.Min(note.isSlider ? note.starttime : renderEndTime, 50584), 0);

                             movement.ForEachPair((start, end) => shadow.Move(OsbEasing.None, start.Time, end.Time, start.Value + shadowOffset, end.Value + shadowOffset));

                             scale.ForEachPair((start, end) =>
                             {
                                 if (start.Time < 48596)
                                     shadow.ScaleVec(OsbEasing.None, start.Time, end.Time, start.Value * 0.8f, end.Value * 0.8f);
                             });

                             rotation.ForEachPair((start, end) => shadow.Rotate(OsbEasing.None, start.Time, end.Time, start.Value, end.Value));

                             // Changed condition to check if we need to apply the final scale
                             if (note.starttime > 48596 || renderEndTime > 48596)
                             {
                                 var lastScale = shadow.ScaleAt(48596);
                                 shadow.ScaleVec(OsbEasing.OutCubic, 48596, 50584, lastScale, new Vector2(0));
                             }
                         }

                         if (progress == 1 && renderReceptor)
                         {
                             note.ApplyHitLightingToNote(note.starttime, note.endtime, fadeOutTime, column, localIterationRate);
                         }
                     });
            });

            var cols = playfieldInstance.columns.Values;

            foreach (var col in cols)
            {
                playfieldInstance.receptorLayer.Discard(col.origin.originSprite);
            }

            return debug;
        }
    }
}