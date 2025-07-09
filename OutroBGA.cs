using OpenTK;
using OpenTK.Graphics;
using storyboard.scriptslibrary;
using StorybrewCommon.Mapset;
using StorybrewCommon.Scripting;
using StorybrewCommon.Storyboarding;
using StorybrewCommon.Storyboarding.Util;
using StorybrewCommon.Storyboarding3d;
using StorybrewCommon.Subtitles;
using StorybrewCommon.Util;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace StorybrewScripts
{

    public class OutroBGA : StoryboardObjectGenerator
    {

        FontGenerator font;

        public override void Generate()
        {

            font = LoadFont("sb/f", new FontDescription()
            {
                FontPath = "assetlibrary/mono.ttf",
                FontSize = 50,
                Color = Color4.White,
                Padding = new Vector2(0, 0),
                FontStyle = FontStyle.Regular,
                TrimTransparency = false,
                EffectsOnly = false,
            },
new RGBShift()
{
    RED_SHIFT = new Vector2(-3, -1),
    BLUE_SHIFT = new Vector2(3, 1),
}
);

            var layer = GetLayer("BG");

            var startTime = 249229; // The start time where the playfield is initialized
            var endTime = 281085; // The end time where the playfield is no

            Scene3d scene = new();
            var camera = new PerspectiveCamera();

            var sprites = new List<OsbSprite>();

            // Camera setup
            camera.VerticalFov.Add(startTime, 65);
            camera.HorizontalFov.Add(startTime, 65);
            camera.FarFade.Add(18640, 500);
            camera.FarClip.Add(18640, 550);
            camera.NearClip.Add(startTime, 10f);
            camera.NearFade.Add(startTime, 30);
            camera.TargetPosition.Add(startTime, new Vector3(0, 0, 0));
            camera.PositionX.Add(startTime, 0);
            camera.PositionZ.Add(startTime, -100);
            camera.PositionY.Add(startTime, 0);

            var numberNode = new Node3d();

            var numbers = 350;

            for (int i = 0; i < numbers; i++)
            {

                var numb = new Animation3d
                {
                    SpritePath = "sb/ani/numbers/frame.png",
                    FrameCount = 10,
                    FrameDelay = 150,

                };

                numb.PositionX.Add(startTime, Random(-100, 100));
                numb.PositionY.Add(startTime, Random(-2000, -100));
                numb.PositionZ.Add(startTime, Random(0, 150));

                numb.Coloring.Add(startTime, new Color4(0, 255, 0, 255));

                numb.SpriteScale.Add(startTime, new Vector2(0.05f));

                sprites.Add(numb.Sprites.First());

                numberNode.Add(numb);

            }

            numberNode.Opacity.Add(264230, 0.3f);
            numberNode.Opacity.Add(264230, 1f);
            numberNode.PositionY.Add(startTime, 100);
            numberNode.PositionY.Add(endTime, 2000);

            scene.Add(numberNode);

            scene.Generate(camera, GetLayer("BG"), startTime, endTime, Beatmap, 4);

            var shadowLag = 70;
            var shadowDuration = 1500;

            foreach (var child in numberNode.children)
            {
                var sprite = (OsbAnimation)((Animation3d)child).Sprites.First();


                var localTime = startTime;
                while (localTime < endTime)
                {
                    var currentFrame = sprite.GetFrameAt(localTime);
                    var pos = sprite.PositionAt(localTime);

                    if (sprite.OpacityAt(localTime) == 0 || pos.X == 320 || pos.Y == 240)
                    {
                        localTime += shadowLag;
                        continue;
                    }
                    else
                    {
                        var shadowSprite = GetLayer("BG_back").CreateSprite($"sb/ani/numbers/frame{currentFrame}.png", OsbOrigin.Centre, pos);
                        shadowSprite.Fade(OsbEasing.None, localTime, localTime + shadowDuration, sprite.OpacityAt(localTime), 0);
                        shadowSprite.Scale(localTime, sprite.ScaleAt(localTime).X);
                        shadowSprite.Color(localTime, new Color4(0, 255, 0, 255));
                        localTime += shadowLag;
                    }
                }
            }

            OsbAnimation tunnel = layer.CreateAnimation("sb/ani/tunnel/frame.jpg", 12, 200 / 12, OsbLoopType.LoopForever);
            OsbSprite mask = layer.CreateSprite("sb/ani/tunnel/mask.png");

            tunnel.Fade(234229, 1);
            //tunnel.Additive(249229);
            tunnel.ScaleVec(OsbEasing.InExpo, 234229 - 500, 234229, new Vector2(0f, 0f), new Vector2(0.5f));
            tunnel.Rotate(234229, 264230, 0, Math.PI * 4);
            tunnel.Fade(264230, 0);

            var beatLenght = Beatmap.GetControlPointAt(234229).BeatDuration;

            tunnel.StartLoopGroup(234229, (int)((249229 - 234229) / beatLenght) / 2);
            tunnel.Color(OsbEasing.InOutSine, 0, beatLenght, new Color4(255, 0, 0, 1), new Color4(0, 255, 0, 1));
            tunnel.Color(OsbEasing.InOutSine, beatLenght, beatLenght * 2, new Color4(0, 255, 0, 1), new Color4(255, 0, 0, 1));
            tunnel.EndGroup();

            tunnel.StartLoopGroup(249229, (int)((264230 - 249229) / beatLenght) / 2);
            tunnel.Fade(OsbEasing.InOutSine, 0, beatLenght, 1, 0);
            tunnel.Fade(OsbEasing.InOutSine, beatLenght, beatLenght * 2, 0, 1);
            tunnel.EndGroup();

            mask.Fade(234229, 1);
            mask.ScaleVec(OsbEasing.InExpo, 234229 - 500, 234229, new Vector2(0f, 0f), new Vector2(0.5f));
            mask.Rotate(234229, 249229, 0, Math.PI * 2);
            mask.Fade(248761, 249698, 1, 0);

            var wooden = layer.CreateSprite("sb/credits/wooden.png", OsbOrigin.Centre, new Vector2(100, 600));
            wooden.Fade(OsbEasing.None, 265167, 266105, 0, 1);
            wooden.MoveY(OsbEasing.OutCubic, 265167, 266105, 600, 240);
            wooden.Scale(265167, 0.4f);
            wooden.MoveY(OsbEasing.InCubic, 268917, 269855, 240, 240 - 600);

            var guise = layer.CreateSprite("sb/credits/guise.png", OsbOrigin.Centre, new Vector2(320 + 220, 600));
            guise.Fade(OsbEasing.None, 268917, 269855, 0, 1);
            guise.MoveY(OsbEasing.OutCubic, 268917, 269855, 600, 240);
            guise.Scale(268917, 0.4f);
            guise.MoveY(OsbEasing.InCubic, 272690, 272690 + 269855 - 268917, 240, 240 - 600);

            var tunnelblick = layer.CreateSprite("sb/credits/tunnelblick.png", OsbOrigin.Centre, new Vector2(100, 600));
            tunnelblick.Fade(OsbEasing.None, 272690, 273673, 0, 1);
            tunnelblick.MoveY(OsbEasing.OutCubic, 272690, 273673, 600, 240);
            tunnelblick.Scale(272690, 0.4f);
            tunnelblick.MoveY(OsbEasing.InCubic, 276886, 276886 + 269855 - 268917, 240, 240 - 600);

            var notosu = layer.CreateSprite("sb/credits/notosu.png", OsbOrigin.Centre, new Vector2(320 + 220, 600));
            notosu.Fade(OsbEasing.None, 277786, 279286, 0, 1);
            notosu.MoveY(OsbEasing.OutCubic, 277786, 279286, 600, 240);
            notosu.Scale(277786, 0.4f);

            notosu.Fade(281085, 282885, 1, 0);



            var cover = layer.CreateSprite("sb/white.png", OsbOrigin.Centre, new Vector2(320, 240));
            cover.Fade(264230, 0.5f);
            cover.Color(264230, new Color4(0, 0, 0, 255));
            cover.Scale(264230, 854f);
            cover.Fade(283485, 0);

            CreditsWriter(265636, 268917, "MUSIC", new Vector2(100, 175), 0.75f);
            CreditsWriter(266105, 268917, "ARTIST WOODEN       ", new Vector2(100, 302), 0.5f);
            CreditsWriter(266105, 268917, "  SONG TAU(EXTENDED)", new Vector2(100, 317), 0.5f);

            CreditsWriter(265636, 268917, "PART 2 MINES", new Vector2(320 + 220, 175), 0.75f);

            CreditsWriter(269386, 272690, "BEATMAP", new Vector2(320 + 220, 175), 0.75f);
            CreditsWriter(269855, 272690, "CHART Disguise", new Vector2(320 + 220, 302), 0.5f);

            CreditsWriter(269386, 272690, "PART 5 MINES", new Vector2(100, 175), 0.75f);

            CreditsWriter(273181, 276885, "STORYBOARD", new Vector2(100, 175), 0.75f);
            CreditsWriter(273673, 276885, " CODING TUNNELBLICK", new Vector2(100, 302), 0.5f);

            CreditsWriter(273181, 276885, "GROOVECOASTER MINES", new Vector2(320 + 220, 175), 0.75f);

            CreditsWriter(278085, 276885, "SPECIAL THANKS", new Vector2(100, 200), 0.75f, true);
            CreditsWriter(278085, 276885, "ENGINE", new Vector2(320 + 225, 160), 0.75f, true);
            CreditsWriter(278686, 276885, "v2.23", new Vector2(320 + 220, 320), 0.5f, true);

            CreditsWriter(278686, 276885, "COPPERTINE", new Vector2(100, 200 + 15), 0.5f, true);
            CreditsWriter(278686, 276885, "DAMNAE", new Vector2(100, 200 + 15 + 12), 0.5f, true);
            CreditsWriter(278686, 276885, "1Nuttelinha1", new Vector2(100, 200 + 15 + 12 + 12), 0.5f, true);
            CreditsWriter(278686, 276885, "phagocytosis", new Vector2(100, 200 + 15 + 12 + 12 + 12), 0.5f, true);
            CreditsWriter(278686, 276885, "YOU :3", new Vector2(100, 200 + 15 + 12 + 12 + 12 + 12), 0.5f, true);

            // First range: all hitobjects between 18253 and 30930
            CreateHitTriggerDisplay(18253, 30930, layer, 320 + 220, 265636, 268917);

            // Second range: doublehits from 55608 to 66834
            CreateHitTriggerDisplay(55608, 66834, layer, 100, 269386, 272690);

            // Third range: doublehits from 182769 to 202881
            CreateHitTriggerDisplay(182769, 202881, layer, 320 + 220, 273181, 276885, true);
        }

        public void CreditsWriter(double startTime, double fadeOutTime, string text, Vector2 pos, float scale = 0.5f, bool fadeOut = false)
        {

            scale = scale * 0.4f;

            var spacing = 0;

            pos.X -= text.Length / 2 * (font.GetTexture("W").BaseWidth * scale);
            pos.X += spacing;

            foreach (char c in text.ToUpper())
            {
                if (c == ' ')
                {
                    pos.X += font.GetTexture("W").BaseWidth * scale + spacing; // Add spacing for space character
                    continue;
                }
                if (c == '\n')
                {
                    pos.Y += 12;
                    continue;
                }
                var f = GetLayer("BG").CreateSprite(font.GetTexture(c.ToString()).Path, OsbOrigin.Centre, pos);
                f.Fade(startTime, 1);
                f.Scale(startTime, scale);
                f.Color(startTime, Color4.White);
                startTime += 100; // Adjust the time for the next character
                pos.X += font.GetTexture(c.ToString()).BaseWidth * scale; // Move the x position for the next character
                if (fadeOut)
                {
                    f.Fade(281085, 282885, 1, 0);
                }
                else
                    f.MoveY(OsbEasing.InCubic, fadeOutTime, fadeOutTime + 269855 - 268917, pos.Y, pos.Y - 600);
            }
        }

        public void CreateHitTriggerDisplay(double startRange, double endRange, StoryboardLayer layer, float xOffset, double displayStart, double displayEnd, bool doublehitsOnly = false)
        {

            var od = Beatmap.OverallDifficulty;
            var bad = 151 - (3 * od);

            var hitTimes = new Dictionary<ColumnType, HashSet<double>>();
            hitTimes[ColumnType.one] = new HashSet<double>();
            hitTimes[ColumnType.two] = new HashSet<double>();
            hitTimes[ColumnType.three] = new HashSet<double>();
            hitTimes[ColumnType.four] = new HashSet<double>();

            // Collect all note times per column in the specified range
            foreach (var hitobject in Beatmap.HitObjects)
            {
                if (hitobject.StartTime >= startRange && hitobject.StartTime <= endRange)
                {
                    switch ((int)hitobject.Position.X)
                    {
                        case 128:
                            hitTimes[ColumnType.one].Add(hitobject.StartTime);
                            break;
                        case 256:
                            hitTimes[ColumnType.two].Add(hitobject.StartTime);
                            break;
                        case 384:
                            hitTimes[ColumnType.three].Add(hitobject.StartTime);
                            break;
                        case 512:
                            hitTimes[ColumnType.four].Add(hitobject.StartTime);
                            break;
                    }
                }
            }

            // Create sorted list of all unique hit times
            var allHitTimes = hitTimes.Values
                .SelectMany(times => times)
                .Distinct()
                .OrderBy(t => t)
                .ToList();

            if (doublehitsOnly)
            {
                var doubleHitTimes = new List<double>();
                foreach (var time in allHitTimes)
                {
                    var columnsAtTime = hitTimes.Values.Count(columnTimes => columnTimes.Contains(time));
                    if (columnsAtTime >= 2) // Double hit or more
                    {
                        doubleHitTimes.Add(time);
                    }
                }
                allHitTimes = doubleHitTimes;
            }

            Log($"Found {allHitTimes.Count} hit times in range {startRange} to {endRange}");
            var widths = 200;
            var minesPerCol = 6;
            var spacing = widths / minesPerCol;
            var verticalSpacing = 25;
            // Create hit triggers for each time
            var counter = 0;
            xOffset -= widths / 2 - spacing / 2; // Center the first column
            var position = new Vector2(xOffset, 215);
            foreach (var time in allHitTimes)
            {

                var remainingMines = allHitTimes.Count - allHitTimes.IndexOf(time);

                CreateHitTriggerWithSlide(time, layer, hitTimes, bad, position, displayStart, displayEnd);
                position.X += spacing; // Move to the right for the next trigger
                counter++;
                if (counter >= minesPerCol)
                {
                    counter = 0;
                    position.X = xOffset;
                    position.Y += verticalSpacing;
                }
            }
        }

        public void CreateHitTriggerWithSlide(double targetTime, StoryboardLayer layer, Dictionary<ColumnType, HashSet<double>> hitTimes, double bad, Vector2 position, double displayStartTime, double displayEndTime)
        {
            var hit = layer.CreateSprite("sb/sprites/mine.png", OsbOrigin.Centre, position);
            hit.Scale(displayStartTime, 0.35f); // Smaller scale for tighter packing
            hit.Fade(displayStartTime, 0);

            // Slide in from left
            hit.MoveY(OsbEasing.OutCubic, displayStartTime, displayStartTime + 500, 500, position.Y);
            hit.Rotate(displayStartTime, displayEndTime + 500, 0, Math.PI * 6); // Rotate in

            // Slide out to right
            hit.MoveY(OsbEasing.InCubic, displayEndTime, displayEndTime + 500, position.Y, -25);

            foreach (var columnType in new[] { ColumnType.one, ColumnType.two, ColumnType.three, ColumnType.four })
            {
                if (!hitTimes[columnType].Contains(targetTime))
                {
                    var triggerStart = targetTime - bad;
                    var triggerEnd = targetTime + bad;

                    // Check for notes in this column that might conflict with our trigger window
                    var conflictingNotes = hitTimes[columnType].Where(noteTime => 
                        Math.Abs(noteTime - targetTime) <= bad * 2); // Check within 2x bad window for safety

                    if (conflictingNotes.Any())
                    {
                        // Find the closest note to adjust our trigger window
                        var closestNote = conflictingNotes.OrderBy(noteTime => Math.Abs(noteTime - targetTime)).First();
                        var noteHitWindow = bad; // The note's hit window

                        if (closestNote < targetTime)
                        {
                            // Note is before our target time, adjust trigger start
                            var noteEnd = closestNote + noteHitWindow;
                            if (triggerStart < noteEnd)
                            {
                                triggerStart = noteEnd; // Start trigger after the note's hit window ends
                            }
                        }
                        else
                        {
                            // Note is after our target time, adjust trigger end
                            var noteStart = closestNote - noteHitWindow;
                            if (triggerEnd > noteStart)
                            {
                                triggerEnd = noteStart; // End trigger before the note's hit window starts
                            }
                        }

                        // Skip if the trigger window became invalid
                        if (triggerStart >= triggerEnd)
                        {
                            continue;
                        }
                    }

                    var triggerName = columnType switch
                    {
                        ColumnType.one => "HitSoundNormalNormal",
                        ColumnType.two => "HitSoundNormalSoft",
                        ColumnType.three => "HitSoundSoftNormal",
                        ColumnType.four => "HitSoundSoftSoft",
                        _ => "HitSound0"
                    };

                    var absoluteFadeTime = displayStartTime;
                    var relativeFadeTime = absoluteFadeTime - triggerStart;

                    hit.StartTriggerGroup(triggerName, triggerStart, triggerEnd);
                    hit.Fade(relativeFadeTime, 1f);
                    hit.EndGroup();
                }
            }
        }
    }
}
