using OpenTK;
using OpenTK.Graphics;
using StorybrewCommon.Mapset;
using StorybrewCommon.Scripting;
using StorybrewCommon.Storyboarding;
using System;
using System.Drawing;
using System.Linq;

namespace StorybrewScripts
{
    public class Particles : StoryboardObjectGenerator
    {
        [Group("Timing")]
        [Configurable] public int StartTime;
        [Configurable] public int EndTime;

        [Group("Sprite")]
        [Configurable] public OsbOrigin Origin = OsbOrigin.Centre;
        [Configurable] public Vector2 Scale = new Vector2(1, 1);
        [Description("Rotation of the sprite; does not influences particle motion direction.")]
        [Configurable] public float Rotation = 0;
        [Configurable] public Color4 Color = Color4.White;
        [Description("Varies the saturation and brightness of the selected Color for each particle.")]
        [Configurable] public float ColorVariance = 0.6f;
        [Configurable] public bool Additive = false;

        [Group("Spawn")]
        [Configurable] public int ParticleCount = 32;
        [Configurable] public float Lifetime = 1000;
        [Description("The point around which particles will be created.")]
        [Configurable] public Vector2 SpawnOrigin = new Vector2(420, 0);
        [Description("The distance around the Spawn Origin point where particles will be created.")]
        [Configurable] public float SpawnSpread = 360;

        [Group("Motion")]
        [Description("The angle in degrees at which particles will be moving.\n0 is to the right, positive values rotate counterclockwise.")]
        [Configurable] public float Angle = 110;
        [Description("The spread in degrees around Angle.")]
        [Configurable] public float AngleSpread = 60;
        [Description("The speed at which particles move, in osupixels.")]
        [Configurable] public float Speed = 480;
        [Description("Eases the motion of particles.")]
        [Configurable] public OsbEasing Easing = OsbEasing.None;

        public override void Generate()
        {
            var beatLenght = Beatmap.GetControlPointAt(234229).BeatDuration * 2;

            if (StartTime == EndTime && Beatmap.HitObjects.FirstOrDefault() != null)
            {
                StartTime = (int)Beatmap.HitObjects.First().StartTime;
                EndTime = (int)Beatmap.HitObjects.Last().EndTime;
            }
            EndTime = Math.Min(EndTime, (int)AudioDuration);
            StartTime = Math.Min(StartTime, EndTime);

            var bitmap = GetMapsetBitmap("sb/ani/numbers/frame0.png");

            var duration = (double)(EndTime - StartTime);
            var loopCount = Math.Max(1, (int)Math.Floor(duration / Lifetime));

            var layer = GetLayer("");
            for (var i = 0; i < ParticleCount; i++)
            {
                var spawnAngle = Random(Math.PI * 2);
                var spawnDistance = (float)(SpawnSpread * Math.Sqrt(Random(1f)));

                var moveAngle = MathHelper.DegreesToRadians(Angle + Random(-AngleSpread, AngleSpread) * 0.5f);
                var moveDistance = Speed * Lifetime * 0.001f;

                var spriteRotation = moveAngle + MathHelper.DegreesToRadians(Rotation);

                var startPosition = SpawnOrigin + new Vector2((float)Math.Cos(spawnAngle), (float)Math.Sin(spawnAngle)) * spawnDistance;
                var endPosition = startPosition + new Vector2((float)Math.Cos(moveAngle), (float)Math.Sin(moveAngle)) * moveDistance;

                var loopDuration = duration / loopCount;
                var startTime = StartTime + (i * loopDuration) / ParticleCount;
                var endTime = startTime + loopDuration * loopCount;

                if (!isVisible(bitmap, startPosition, endPosition, (float)spriteRotation, (float)loopDuration))
                    continue;

                var color = Color;
                if (ColorVariance > 0)
                {
                    ColorVariance = MathHelper.Clamp(ColorVariance, 0, 1);

                    var hsba = Color4.ToHsl(color);
                    var sMin = Math.Max(0, hsba.Y - ColorVariance * 0.5f);
                    var sMax = Math.Min(sMin + ColorVariance, 1);
                    var vMin = Math.Max(0, hsba.Z - ColorVariance * 0.5f);
                    var vMax = Math.Min(vMin + ColorVariance, 1);

                    color = Color4.FromHsl(new Vector4(
                        hsba.X,
                        (float)Random(sMin, sMax),
                        (float)Random(vMin, vMax),
                        hsba.W));
                }

                var particle = layer.CreateAnimation("sb/ani/numbers/frame.png", 10, 2000 / 11, OsbLoopType.LoopForever, Origin);

                particle.StartLoopGroup(startTime, loopCount);
                particle.Move(Easing, 0, loopDuration, startPosition, endPosition);
                particle.ScaleVec(OsbEasing.InExpo, 0, loopDuration, new Vector2(0f), Scale);
                particle.EndGroup();

                particle.StartLoopGroup(234229, (int)((249229 - 234229) / beatLenght));
                particle.Color(OsbEasing.InOutSine, 0, beatLenght, new Color4(255, 0, 0, 1), new Color4(0, 255, 0, 1));
                particle.Color(OsbEasing.InOutSine, beatLenght, beatLenght * 2, new Color4(0, 255, 0, 1), new Color4(255, 0, 0, 1));
                particle.EndGroup();

                var localtime = 249229;
                var localend = 262823;
                var dur = 249347 - localtime;



                //field.fadeAt(localtime, localtime + dur, OsbEasing.OutExpo, Math.Abs(currentFade - 1));
                // field2.fadeAt(localtime, localtime + dur, OsbEasing.OutExpo, currentFade);

                if (i % 2 == 0)
                {
                    particle.StartLoopGroup(localtime, 64);
                    particle.Fade(OsbEasing.OutSine, 0, dur, 0, 1);
                    particle.Fade(OsbEasing.OutSine, dur, dur * 2, 1, 0);
                    particle.EndGroup();
                }
                else
                {
                    particle.StartLoopGroup(localtime, 64);
                    particle.Fade(OsbEasing.OutSine, 0, dur, 1, 0);
                    particle.Fade(OsbEasing.OutSine, dur, dur * 2, 0, 1);
                    particle.EndGroup();
                }

                particle.Fade(264230, 0);

            }
        }

        private bool isVisible(Bitmap bitmap, Vector2 startPosition, Vector2 endPosition, float rotation, float duration)
        {
            var spriteSize = new Vector2(bitmap.Width * Scale.X, bitmap.Height * Scale.Y);
            var originVector = OsbSprite.GetOriginVector(Origin, spriteSize.X, spriteSize.Y);

            for (var t = 0; t < duration; t += 200)
            {
                var position = Vector2.Lerp(startPosition, endPosition, t / duration);
                if (OsbSprite.InScreenBounds(position, spriteSize, rotation, originVector))
                    return true;
            }
            return false;
        }
    }
}
