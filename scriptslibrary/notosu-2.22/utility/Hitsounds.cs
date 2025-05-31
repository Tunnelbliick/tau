using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using StorybrewCommon.Storyboarding;

namespace StorybrewScripts
{
    public static class HitSound
    {
        public static double worstTiming(Playfield field)
        {
            return 151 - (3 * field.od);
        }

        public static void AddHitSound(Playfield field, Column column, List<double> keysInRange, Dictionary<double, Note> notes)
        {
            var marv = 16;
            var great = 64 - (3 * field.od);
            var good = 97 - (3 * field.od);
            var ok = 127 - (3 * field.od);
            var bad = 151 - (3 * field.od);

            OsbSprite light = column.receptor.light;
            OsbSprite hit = column.receptor.hit;

            string trigger = "HitSound0";

            switch (column.type)
            {
                case ColumnType.one:
                    trigger = "HitSoundNormalNormal";
                    break;
                case ColumnType.two:
                    trigger = "HitSoundNormalSoft";
                    break;
                case ColumnType.three:
                    trigger = "HitSoundSoftNormal";
                    break;
                case ColumnType.four:
                    trigger = "HitSoundSoftSoft";
                    break;
            }

            var fadeOut = 151;

            // Sort the keys to ensure we process them in order
            var sortedKeys = keysInRange.OrderBy(k => k).ToList();

            for (int i = 0; i < sortedKeys.Count; i++)
            {
                var key = sortedKeys[i];
                Note note = notes[key];

                // Calculate the max allowable end time for this note's triggers
                double maxEndTime = double.MaxValue;
                if (i < sortedKeys.Count - 1)
                {
                    // If there's a next note, limit the end time to avoid overlap
                    Note nextNote = notes[sortedKeys[i + 1]];
                    // Prioritize the next note by limiting triggers from current note
                    // Cut off half of the trailing triggers, or at least before marv timing of next note
                    maxEndTime = Math.Min(note.endtime + bad / 2, nextNote.endtime - marv);
                }

                // Function to adjust the end time of a trigger to avoid overlap
                double adjustEndTime(double originalEnd)
                {
                    return Math.Min(originalEnd, maxEndTime);
                }

                // Trigger for 300+ (±16ms) - only if not overlapping with next note
                if (note.endtime + marv <= maxEndTime)
                {
                    light.StartTriggerGroup(trigger, note.endtime - marv, adjustEndTime(note.endtime + marv));
                    light.Fade(OsbEasing.InExpo, 0, fadeOut, column.receptor.renderedSprite.OpacityAt(note.starttime), 0);
                    light.Color(0, new Color4(104, 186, 207, 0));
                    light.EndGroup();

                    hit.StartTriggerGroup(trigger, note.endtime - marv, adjustEndTime(note.endtime + marv));
                    hit.Fade(OsbEasing.InExpo, 0, fadeOut, column.receptor.renderedSprite.OpacityAt(note.starttime), 0);
                    hit.EndGroup();
                }

                // Trigger for 300 (±46ms, excludes 300+ range)
                // Before the note
                light.StartTriggerGroup(trigger, note.endtime - great, note.endtime - marv + 1);
                light.Fade(OsbEasing.InExpo, 0, fadeOut, column.receptor.renderedSprite.OpacityAt(note.starttime), 0);
                light.Color(0, new Color4(223, 181, 96, 0));
                light.EndGroup();

                hit.StartTriggerGroup(trigger, note.endtime - great, note.endtime - marv + 1);
                hit.Fade(OsbEasing.InExpo, 0, fadeOut, column.receptor.renderedSprite.OpacityAt(note.starttime), 0);
                hit.EndGroup();

                // After the note - only if not overlapping with next note
                if (note.endtime + marv + 1 < maxEndTime)
                {
                    double endTime = adjustEndTime(note.endtime + great);
                    if (endTime > note.endtime + marv + 1)
                    {
                        light.StartTriggerGroup(trigger, note.endtime + marv + 1, endTime);
                        light.Fade(OsbEasing.InExpo, 0, fadeOut, column.receptor.renderedSprite.OpacityAt(note.starttime), 0);
                        light.Color(0, new Color4(223, 181, 96, 0));
                        light.EndGroup();

                        hit.StartTriggerGroup(trigger, note.endtime + marv + 1, endTime);
                        hit.Fade(OsbEasing.InExpo, 0, fadeOut, column.receptor.renderedSprite.OpacityAt(note.starttime), 0);
                        hit.EndGroup();
                    }
                }

                // Trigger for 200 (±79ms, excludes 300 range)
                // Before the note
                light.StartTriggerGroup(trigger, note.endtime - good, note.endtime - great + 1);
                light.Fade(OsbEasing.InExpo, 0, fadeOut, column.receptor.renderedSprite.OpacityAt(note.starttime), 0);
                light.Color(0, new Color4(95, 204, 95, 0));
                light.EndGroup();

                hit.StartTriggerGroup(trigger, note.endtime - good, note.endtime - great + 1);
                hit.Fade(OsbEasing.InExpo, 0, fadeOut, column.receptor.renderedSprite.OpacityAt(note.starttime), 0);
                hit.EndGroup();

                // After the note - only if not overlapping with next note
                if (note.endtime + great + 1 < maxEndTime)
                {
                    double endTime = adjustEndTime(note.endtime + good);
                    if (endTime > note.endtime + great + 1)
                    {
                        light.StartTriggerGroup(trigger, note.endtime + great + 1, endTime);
                        light.Fade(OsbEasing.InExpo, 0, fadeOut, column.receptor.renderedSprite.OpacityAt(note.starttime), 0);
                        light.Color(0, new Color4(95, 204, 95, 0));
                        light.EndGroup();

                        hit.StartTriggerGroup(trigger, note.endtime + great + 1, endTime);
                        hit.Fade(OsbEasing.InExpo, 0, fadeOut, column.receptor.renderedSprite.OpacityAt(note.starttime), 0);
                        hit.EndGroup();
                    }
                }

                // Trigger for 100 (±109ms, excludes 200 range)
                // Before the note
                light.StartTriggerGroup(trigger, note.endtime - ok, note.endtime - good + 1);
                light.Fade(OsbEasing.InExpo, 0, fadeOut, column.receptor.renderedSprite.OpacityAt(note.starttime), 0);
                light.Color(0, new Color4(206, 128, 224, 0));
                light.EndGroup();

                hit.StartTriggerGroup(trigger, note.endtime - ok, note.endtime - good + 1);
                hit.Fade(OsbEasing.InExpo, 0, fadeOut, column.receptor.renderedSprite.OpacityAt(note.starttime), 0);
                hit.EndGroup();

                // After the note - only if not overlapping with next note
                if (note.endtime + good + 1 < maxEndTime)
                {
                    double endTime = adjustEndTime(note.endtime + ok);
                    if (endTime > note.endtime + good + 1)
                    {
                        light.StartTriggerGroup(trigger, note.endtime + good + 1, endTime);
                        light.Fade(OsbEasing.InExpo, 0, fadeOut, column.receptor.renderedSprite.OpacityAt(note.starttime), 0);
                        light.Color(0, new Color4(206, 128, 224, 0));
                        light.EndGroup();

                        hit.StartTriggerGroup(trigger, note.endtime + good + 1, endTime);
                        hit.Fade(OsbEasing.InExpo, 0, fadeOut, column.receptor.renderedSprite.OpacityAt(note.starttime), 0);
                        hit.EndGroup();
                    }
                }

                // Trigger for 50 (±133ms, excludes 100 range)
                // Before the note
                light.StartTriggerGroup(trigger, note.endtime - bad, note.endtime - ok + 1);
                light.Fade(OsbEasing.InExpo, 0, fadeOut, column.receptor.renderedSprite.OpacityAt(note.starttime), 0);
                light.Color(0, new Color4(198, 86, 37, 0));
                light.EndGroup();

                hit.StartTriggerGroup(trigger, note.endtime - bad, note.endtime - ok + 1);
                hit.Fade(OsbEasing.InExpo, 0, fadeOut, column.receptor.renderedSprite.OpacityAt(note.starttime), 0);
                hit.EndGroup();

                // After the note - only if not overlapping with next note
                if (note.endtime + ok + 1 < maxEndTime)
                {
                    double endTime = adjustEndTime(note.endtime + bad);
                    if (endTime > note.endtime + ok + 1)
                    {
                        light.StartTriggerGroup(trigger, note.endtime + ok + 1, endTime);
                        light.Fade(OsbEasing.InExpo, 0, fadeOut, column.receptor.renderedSprite.OpacityAt(note.starttime), 0);
                        light.Color(0, new Color4(198, 86, 37, 0));
                        light.EndGroup();

                        hit.StartTriggerGroup(trigger, note.endtime + ok + 1, endTime);
                        hit.Fade(OsbEasing.InExpo, 0, fadeOut, column.receptor.renderedSprite.OpacityAt(note.starttime), 0);
                        hit.EndGroup();
                    }
                }
            }
        }
    }
}