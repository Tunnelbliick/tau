using System;
using System.Collections.Generic;
using System.Linq;
using OpenTK;
using storyboard.scriptslibrary.maniaModCharts.effects;
using StorybrewCommon.Mapset;
using StorybrewCommon.Storyboarding;
using StorybrewCommon.Storyboarding.CommandValues;

namespace StorybrewScripts
{

    public class Playfield : IDisposable
    {

        // Default SB height / width

        public double delta = 10;

        private float absoluteWidth = 640f;
        public float width = 250f;
        public float height = 480f;

        public float receptorWallOffset = 0f;

        private double rotation = 0f;

        public bool isColored = false;

        private CommandScale receptorScale = new CommandScale(0.5);
        private string receptorSpritePath = "sb/sprites/receiver.png";

        public double starttime;
        public double endtime;

        public double noteStart;
        public double noteEnd;

        public StoryboardLayer noteLayer;
        public StoryboardLayer receptorLayer;

        public StoryboardLayer shadowLayer;

        // Reference for active Columns;
        public Dictionary<ColumnType, Column> columns = new Dictionary<ColumnType, Column>();
        public Dictionary<double, EffectInfo> effectReferenceByStartTime = new Dictionary<double, EffectInfo>();

        // Notes Per Column
        public Dictionary<ColumnType, Dictionary<double, Note>> columnNotes = new Dictionary<ColumnType, Dictionary<double, Note>>();

        public Dictionary<double, FadeEffect> fadeAtTime = new Dictionary<double, FadeEffect>();

        public double od;

        private bool disposed = false;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    // Set managed objects to null
                    columns = null;
                    effectReferenceByStartTime = null;
                    columnNotes = null;
                }

                // Nullify any unmanaged resources here, if any

                disposed = true;
            }
        }

        ~Playfield()
        {
            Dispose(false);
        }

        public void initilizePlayField(StoryboardLayer receptors, StoryboardLayer notes, double starttime, double endtime, float initialWidht, float initialHeight, float receptorWallOffset, double OverallDifficulty)
        {
            initilizePlayField(receptors, notes, starttime, endtime, initialWidht, initialHeight, receptorWallOffset, OverallDifficulty, null);
        }

        public void initilizePlayField(StoryboardLayer receptors, StoryboardLayer notes, double starttime, double endtime, float initialWidht, float initialHeight, float receptorWallOffset, double OverallDifficulty, StoryboardLayer shadow)
        {
            this.starttime = starttime;
            this.endtime = endtime;

            shadowLayer = shadow;

            this.noteStart = starttime;
            this.noteEnd = endtime;

            this.receptorLayer = receptors;
            this.noteLayer = notes;

            height = initialHeight;
            width = initialWidht;

            this.od = OverallDifficulty;

            Column one = new Column(128, ColumnType.one, receptorSpritePath, receptors, receptorScale, starttime, delta);
            Column two = new Column(256, ColumnType.two, receptorSpritePath, receptors, receptorScale, starttime, delta);
            Column three = new Column(384, ColumnType.three, receptorSpritePath, receptors, receptorScale, starttime, delta);
            Column four = new Column(512, ColumnType.four, receptorSpritePath, receptors, receptorScale, starttime, delta);

            columns.Add(one.type, one);
            columns.Add(two.type, two);
            columns.Add(three.type, three);
            columns.Add(four.type, four);

            this.receptorWallOffset = receptorWallOffset;

        }

        public void SetWidth(float width)
        {
            this.width = width;
        }

        public void SetHeight(float height)
        {
            this.height = height;
        }

        public void finishInitilization()
        {

            float position = 0f;

            foreach (Column column in columns.Values)
            {

                Receptor receptor = column.receptor;
                NoteOrigin origin = column.origin;

                var x = calculateOffset() + position + getColumnWidth() / 2;

                float y = 0;
                float opposit = height;

                if (height > 0)
                {
                    y += receptorWallOffset;
                    opposit += receptorWallOffset;
                }
                else
                {
                    y = 480 - receptorWallOffset;
                    opposit = height + y;
                }

                receptor.Render(starttime, endtime);
                origin.Render(starttime, endtime);
                receptor.MoveReceptorAbsolute(starttime, new Vector2(x, y));
                origin.MoveOriginAbsolute(starttime, new Vector2(x, opposit));

                position += getColumnWidth();
            }

        }

        public void initializeNotes(List<OsuHitObject> objects, double bpm, double offset, bool isColored = false, double msPerPart = 30)
        {

            this.isColored = isColored;

            foreach (Column column in columns.Values)
            {

                column.setBPM(bpm, offset);

                Dictionary<double, Note> notes = new Dictionary<double, Note>();
                double xOffset = column.offset;
                columnNotes.Add(column.type, notes);
            }

            objects.Reverse();

            foreach (OsuHitObject hitobject in objects)
            {
                switch ((int)hitobject.Position.X)
                {
                    case 128:
                        AddNote(columnNotes[ColumnType.one], hitobject, columns[ColumnType.one], bpm, offset, isColored, msPerPart);
                        if (hitobject.StartTime < 37085)
                        {
                            // AddMines(ColumnType.one, hitobject, columns[ColumnType.one], bpm, offset);
                        }
                        break;
                    case 256:
                        AddNote(columnNotes[ColumnType.two], hitobject, columns[ColumnType.two], bpm, offset, isColored, msPerPart);
                        if (hitobject.StartTime < 37085)
                        {
                            //  AddMines(ColumnType.two, hitobject, columns[ColumnType.one], bpm, offset);
                        }
                        break;
                    case 384:
                        AddNote(columnNotes[ColumnType.three], hitobject, columns[ColumnType.three], bpm, offset, isColored, msPerPart);
                        if (hitobject.StartTime < 37085)
                        {
                            // AddMines(ColumnType.three, hitobject, columns[ColumnType.one], bpm, offset);
                        }
                        break;
                    case 512:
                        AddNote(columnNotes[ColumnType.four], hitobject, columns[ColumnType.four], bpm, offset, isColored, msPerPart);
                        if (hitobject.StartTime < 37085)
                        {
                            //  AddMines(ColumnType.four, hitobject, columns[ColumnType.one], bpm, offset);
                        }
                        break;
                    default:
                        continue;
                }
            }

            finishInitilization();
        }

        public void initializeNotes(List<OsuHitObject> objects, Beatmap beatmap, bool isColored = false, double msPerPart = 30, double overridetime = -1)
        {

            this.isColored = isColored;

            foreach (Column column in columns.Values)
            {

                Dictionary<double, Note> notes = new Dictionary<double, Note>();
                double xOffset = column.offset;
                columnNotes.Add(column.type, notes);
            }

            objects.Reverse();

            foreach (OsuHitObject hitobject in objects)
            {

                var controlpoint = beatmap.GetControlPointAt((int)hitobject.StartTime);
                var bpm = controlpoint.Bpm;
                var offset = controlpoint.Offset;

                foreach (Column column in columns.Values)
                {
                    if (column.bpm.ContainsKey((int)offset) == false && offset >= starttime)
                    {
                        column.setBPM((int)offset, bpm, offset);
                    }
                }

                if (overridetime != -1 && hitobject.StartTime < overridetime)
                {
                    msPerPart = 20;
                }

                switch ((int)hitobject.Position.X)
                {
                    case 128:
                        AddNote(columnNotes[ColumnType.one], hitobject, columns[ColumnType.one], bpm, offset, isColored, msPerPart);
                        break;
                    case 256:
                        AddNote(columnNotes[ColumnType.two], hitobject, columns[ColumnType.two], bpm, offset, isColored, msPerPart);
                        break;
                    case 384:
                        AddNote(columnNotes[ColumnType.three], hitobject, columns[ColumnType.three], bpm, offset, isColored, msPerPart);
                        break;
                    case 512:
                        AddNote(columnNotes[ColumnType.four], hitobject, columns[ColumnType.four], bpm, offset, isColored, msPerPart);
                        break;
                    default:
                        continue;
                }
            }

            finishInitilization();
        }

        public void AddNote(Dictionary<double, Note> notes, OsuHitObject hitobject, Column column, double bpm, double offset, bool isColored, double msPerPart)
        {

            // Check for overlapping times
            if (hitobject.StartTime <= noteEnd && hitobject.EndTime >= noteStart)
            {
                Note currentNote = new Note(this.noteLayer, hitobject, column, bpm, offset, isColored, msPerPart);
                notes.Add(hitobject.StartTime, currentNote);
            }
        }

        public void AddMines(ColumnType type, OsuHitObject hitobject, Column column, double bpm, double offset)
        {

            // Check for overlapping times
            if (hitobject.StartTime <= noteEnd && hitobject.EndTime >= noteStart)
            {
                foreach (var columnType in columns.Keys)
                {
                    if (columnType != type && columnType != ColumnType.all)
                    {
                        Note currentNote = new Note(this.noteLayer, hitobject, column, bpm, offset, true);
                        columnNotes[columnType].Add(hitobject.StartTime, currentNote);
                    }
                }
            }
        }

        // TODO fix this
        /*
                public double rotateNotes(OsbEasing easing, double starttime, double duration, double rotation)
                {
                    double endtime = starttime + duration;
                    double lookupTime = starttime - duration;


                    foreach (Column column in columns.Values)
                    {
                        Dictionary<double, Note> notes = columnNotes[column.type];

                        var keysInRange = notes.Keys.Where(hittime => hittime >= lookupTime && hittime <= endtime).ToList();

                        foreach (var key in keysInRange)
                        {
                            Note note = notes[key];

                            note.Rotate(starttime, duration, easing, rotation);
                        }
                    }

                    return endtime;
                }
                */

        public void Resize(OsbEasing easing, double starttime, double endtime, float width, float height)
        {

            float position = 0;

            Vector2 currentCenter = calculatePlayFieldCenter(endtime);

            // Positive width

            Vector2 edge = currentCenter - new Vector2(width / 2, 0);

            foreach (Column column in columns.Values)
            {

                Vector2 receptorPos = column.receptor.PositionAt(endtime);
                Vector2 originPos = column.origin.PositionAt(endtime);

                Vector2 receptorOffsetToEdge = receptorPos - edge;
                Vector2 originOffsetToEdge = originPos - edge;

                float x = position + getColumnWidth(width) / 2;

                float difference = height - this.height;

                column.receptor.MoveReceptorRelative(easing, starttime, endtime, new Vector2(-receptorOffsetToEdge.X + x, 0));
                column.origin.MoveOriginRelative(easing, starttime, endtime, new Vector2(-originOffsetToEdge.X + x, difference));

                position += getColumnWidth(width);
            }

            this.width = width;
            this.height = height;
        }

        public void ScaleOrigin(OsbEasing easing, double starttime, double endtime, Vector2 scale, ColumnType type)
        {

            if (type == ColumnType.all)
            {
                foreach (Column column in columns.Values)
                {
                    column.origin.ScaleOrigin(easing, starttime, endtime, scale);
                }
            }
            else
            {
                columns[type].origin.ScaleOrigin(easing, starttime, endtime, scale);
            }
        }

        public void ScaleReceptor(OsbEasing easing, double starttime, double endtime, Vector2 scale, ColumnType type)
        {

            if (type == ColumnType.all)
            {
                foreach (Column column in columns.Values)
                {
                    column.receptor.ScaleReceptor(easing, starttime, endtime, scale);
                }
            }
            else
            {
                columns[type].receptor.ScaleReceptor(easing, starttime, endtime, scale);
            }
        }

        public void ScaleColumn(OsbEasing easing, double starttime, double endtime, Vector2 scale, ColumnType type)
        {

            if (type == ColumnType.all)
            {
                foreach (Column column in columns.Values)
                {
                    column.receptor.ScaleReceptor(easing, starttime, endtime, scale);
                    column.origin.ScaleOrigin(easing, starttime, endtime, scale);
                }
            }
            else
            {
                columns[type].receptor.ScaleReceptor(easing, starttime, endtime, scale);
                columns[type].origin.ScaleOrigin(easing, starttime, endtime, scale);
            }
        }


        public void MoveColumnRelative(OsbEasing easing, double starttime, double endtime, Vector2 offset, ColumnType type)
        {

            if (type == ColumnType.all)
            {
                foreach (Column column in columns.Values)
                {
                    column.MoveColumnRelative(easing, starttime, endtime, offset);
                }
            }
            else
            {
                columns[type].MoveColumnRelative(easing, starttime, endtime, offset);
            }
        }

        public void MoveColumnRelativeX(OsbEasing easing, double starttime, double endtime, float value, ColumnType type)
        {

            if (type == ColumnType.all)
            {
                foreach (Column column in columns.Values)
                {
                    column.MoveColumnRelativeX(easing, starttime, endtime, value);
                }
            }
            else
            {
                columns[type].MoveColumnRelativeX(easing, starttime, endtime, value);
            }
        }

        public void MoveColumnRelativeY(OsbEasing easing, double starttime, double endtime, float value, ColumnType type)
        {

            if (type == ColumnType.all)
            {
                foreach (Column column in columns.Values)
                {
                    column.MoveColumnRelativeY(easing, starttime, endtime, value);
                }
            }
            else
            {
                columns[type].MoveColumnRelativeY(easing, starttime, endtime, value);
            }
        }

        public void Scale(OsbEasing easing, double starttime, double endtime, Vector2 newScale, bool keepPosition = false, CenterType centerType = CenterType.playfield)
        {
            Vector2 center = calculatePlayFieldCenter(endtime);

            if (centerType == CenterType.playfield)
            {
                center = calculatePlayFieldCenter(endtime);
            }
            else if (centerType == CenterType.middle)
            {
                center = new Vector2(320, 240);
            }
            else if (centerType == CenterType.receptor)
            {
                // Use receptor center code as before
                Vector2 mostLeft = new Vector2(float.MaxValue, 0);
                Vector2 mostRight = new Vector2(float.MinValue, 0);

                foreach (Column column in columns.Values)
                {
                    Vector2 receptorPosition = column.receptor.PositionAt(endtime);

                    // Check for most left position based on x-coordinate
                    if (receptorPosition.X < mostLeft.X)
                    {
                        mostLeft = receptorPosition;
                    }

                    // Check for most right position based on x-coordinate
                    if (receptorPosition.X > mostRight.X)
                    {
                        mostRight = receptorPosition;
                    }
                }

                // Calculate center between receptors
                center = new Vector2(
                    (mostLeft.X + mostRight.X) / 2,
                    (mostLeft.Y + mostRight.Y) / 2
                );
            }

            // Track scaling for debugging
            Dictionary<ColumnType, Vector2> originalPositions = new Dictionary<ColumnType, Vector2>();
            Dictionary<ColumnType, Vector2> newPositions = new Dictionary<ColumnType, Vector2>();

            foreach (Column column in columns.Values)
            {
                Receptor receptor = column.receptor;
                NoteOrigin origin = column.origin;

                Vector2 receptorPosition = receptor.PositionAt(endtime);
                Vector2 originPosition = origin.PositionAt(endtime);

                Vector2 receptorScale = receptor.ScaleAt(endtime);
                Vector2 originScale = origin.ScaleAt(endtime);

                // Apply scaling to the sprites
                receptor.ScaleReceptor(easing, starttime, endtime, newScale);
                origin.ScaleOrigin(easing, starttime, endtime, newScale);

                if (!keepPosition)
                {
                    // Store original positions
                    originalPositions[column.type] = receptorPosition;

                    // Calculate scale factors
                    float scaleFactorX = newScale.X / receptorScale.X;
                    float scaleFactorY = newScale.Y / receptorScale.Y;

                    // Calculate vectors from center to each element
                    Vector2 receptorVector = receptorPosition - center;
                    Vector2 originVector = originPosition - center;

                    // Apply scale to distances while preserving angles
                    Vector2 scaledReceptorVector = new Vector2(
                        receptorVector.X * scaleFactorX,
                        receptorVector.Y * scaleFactorY
                    );

                    Vector2 scaledOriginVector = new Vector2(
                        originVector.X * scaleFactorX,
                        originVector.Y * scaleFactorY
                    );

                    // Calculate new positions
                    Vector2 newReceptorPosition = center + scaledReceptorVector;
                    Vector2 newOriginPosition = center + scaledOriginVector;

                    // Store new positions for debugging
                    newPositions[column.type] = newReceptorPosition;

                    // Calculate movement vectors
                    Vector2 receptorMovement = newReceptorPosition - receptorPosition;
                    Vector2 originMovement = newOriginPosition - originPosition;

                    // Apply movements
                    receptor.MoveReceptorRelative(easing, starttime, endtime, receptorMovement);
                    origin.MoveOriginRelative(easing, starttime, endtime, originMovement);
                }
            }

            // Ensure consistent spacing if needed
            if (!keepPosition)
            {
                // If needed, this is where we'd add logic to ensure consistent spacing between columns
                // However, preserving the scaled vectors should maintain proper spacing automatically
            }
        }

        public Vector2 ScaleColumn(OsbEasing easing, double starttime, double endtime, Vector2 newScale, ColumnType columnType, CenterTypeColumn centerType = CenterTypeColumn.middle, bool invertMovement = false, float movementSacle = 1)
        {

            if (columnType == ColumnType.all)
            {
                foreach (Column column in columns.Values)
                {
                    return column.ScaleColumn(easing, starttime, endtime, newScale, centerType, invertMovement, movementSacle);
                }
            }
            else
                return columns[columnType].ScaleColumn(easing, starttime, endtime, newScale, centerType, invertMovement, movementSacle);

            return Vector2.Zero;
        }

        public void MoveReceptorAbsolute(OsbEasing easing, double starttime, double endtime, Vector2 from, Vector2 to, ColumnType column)
        {
            if (column == ColumnType.all)
            {
                foreach (Column currentColumn in columns.Values)
                {
                    currentColumn.MoveReceptorAbsolute(easing, starttime, endtime, from, to);
                }
            }
            else
            {
                Column currentColumn = columns[column];

                currentColumn.MoveReceptorAbsolute(easing, starttime, endtime, from, to);
            }

        }

        public void MoveReceptorAbsolute(OsbEasing easing, double starttime, double endtime, Vector2 to, ColumnType column)
        {
            if (column == ColumnType.all)
            {
                foreach (Column currentColumn in columns.Values)
                {
                    Vector2 from = currentColumn.ReceptorPositionAt(starttime);

                    currentColumn.MoveReceptorAbsolute(easing, starttime, endtime, from, to);
                }
            }
            else
            {
                Column currentColumn = columns[column];
                Vector2 from = currentColumn.ReceptorPositionAt(starttime);
                currentColumn.MoveReceptorAbsolute(easing, starttime, endtime, from, to);
            }

        }

        public void MoveReceptorRelative(OsbEasing easing, double starttime, double endtime, Vector2 position, ColumnType column)
        {
            if (column == ColumnType.all)
            {
                foreach (Column currentColumn in columns.Values)
                {

                    currentColumn.MoveReceptorRelative(easing, starttime, endtime, position);
                }
            }
            else
            {
                Column currentColumn = columns[column];

                currentColumn.MoveReceptorRelative(easing, starttime, endtime, position);

            }

        }

        public void MoveReceptorRelativeNorm(OsbEasing easing, double starttime, double endtime, Vector2 position, ColumnType column)
        {
            if (column == ColumnType.all)
            {
                foreach (Column currentColumn in columns.Values)
                {

                    currentColumn.MoveReceptorRelativeNorm(easing, starttime, endtime, position);
                }
            }
            else
            {
                Column currentColumn = columns[column];

                currentColumn.MoveReceptorRelativeNorm(easing, starttime, endtime, position);
            }
        }

        public void MoveOriginAbsolute(double starttime, Vector2 to, ColumnType column)
        {
            if (column == ColumnType.all)
            {
                foreach (Column currentColumn in columns.Values)
                {
                    currentColumn.MoveOriginAbsoluite(starttime, to);
                }
            }
            else
            {
                Column currentColumn = columns[column];

                currentColumn.MoveOriginAbsoluite(starttime, to);
            }

        }

        public void MoveOriginAbsolute(OsbEasing easing, double starttime, double endtime, Vector2 to, ColumnType column)
        {
            if (column == ColumnType.all)
            {
                foreach (Column currentColumn in columns.Values)
                {
                    Vector2 from = currentColumn.OriginPositionAt(starttime);
                    currentColumn.MoveOriginAbsoluite(easing, starttime, endtime, from, to);
                }
            }
            else
            {
                Column currentColumn = columns[column];
                Vector2 from = currentColumn.OriginPositionAt(starttime);
                currentColumn.MoveOriginAbsoluite(easing, starttime, endtime, from, to);
            }

        }

        public void MoveOriginAbsolute(OsbEasing easing, double starttime, double endtime, Vector2 from, Vector2 to, ColumnType column)
        {
            if (column == ColumnType.all)
            {
                foreach (Column currentColumn in columns.Values)
                {
                    currentColumn.MoveOriginAbsoluite(easing, starttime, endtime, from, to);
                }
            }
            else
            {
                Column currentColumn = columns[column];

                currentColumn.MoveOriginAbsoluite(easing, starttime, endtime, from, to);
            }

        }

        public void MoveOriginRelative(OsbEasing easing, double starttime, double endtime, Vector2 position, ColumnType column)
        {
            if (column == ColumnType.all)
            {
                foreach (Column currentColumn in columns.Values)
                {

                    currentColumn.MoveOriginRelative(easing, starttime, endtime, position);
                }
            }
            else
            {
                Column currentColumn = columns[column];

                currentColumn.MoveOriginRelative(easing, starttime, endtime, position);

            }

        }

        public void MoveOriginNormalized(OsbEasing easing, double starttime, double endtime, Vector2 offset, ColumnType column)
        {
            if (column == ColumnType.all)
            {
                foreach (Column currentColumn in columns.Values)
                {

                    currentColumn.MoveOriginRelativeNorm(easing, starttime, endtime, offset);
                }
            }
            else
            {
                Column currentColumn = columns[column];

                currentColumn.MoveOriginRelativeNorm(easing, starttime, endtime, offset);
            }
        }


        public void RotateReceptorRelative(OsbEasing easing, double starttime, double endtime, double rotation, ColumnType column = ColumnType.all)
        {
            if (column == ColumnType.all)
            {
                foreach (Column currentColumn in columns.Values)
                {

                    currentColumn.RotateReceptorRelative(easing, starttime, endtime, Math.Round(rotation, 5));

                }
            }
            else
            {

                Column currentColumn = columns[column];

                currentColumn.RotateReceptorRelative(easing, starttime, endtime, Math.Round(rotation, 5));
            }

        }



        public void RotateReceptorAbsolute(OsbEasing easing, double starttime, double endtime, double rotation, ColumnType column = ColumnType.all)
        {
            if (column == ColumnType.all)
            {
                foreach (Column currentColumn in columns.Values)
                {

                    currentColumn.RotateReceptor(easing, starttime, endtime, rotation);

                }
            }
            else
            {

                Column currentColumn = columns[column];

                currentColumn.RotateReceptor(easing, starttime, endtime, rotation);
            }

        }

        // TODO fix this
        /*
        public void MoveOriginRelative(double starttime, double duration, OsbEasing easing, Vector2 position, ColumnType column)
        {
            if (column == ColumnType.all)
            {
                foreach (Column currentColumn in columns.Values)
                {
                    Vector2 currentPosition = currentColumn.getOriginPosition(starttime);

                    //currentColumn.MoveOrigin(starttime, duration, Vector2.Add(currentPosition, position), easing);
                }
            }
            else
            {
                Column currentColumn = columns[column];

                Vector2 currentPosition = currentColumn.getOriginPosition(starttime);

                //currentColumn.MoveOrigin(starttime, duration, Vector2.Add(currentPosition, position), easing);

            }

        }
        */

        // TODO fix this
        /*
        public double MoveOriginAbsolute(double starttime, double duration, OsbEasing easing, Vector2 position, ColumnType column)
        {

            if (column == ColumnType.all)
            {
                foreach (Column currentColumn in columns.Values)
                {
                    //currentColumn.MoveOrigin(starttime, duration, position, easing);
                }
            }
            else
            {

                Column currentColumn = columns[column];

                //currentColumn.MoveOrigin(starttime, duration, position, easing);
            }
            return starttime + duration;

        }
        */

        // This will rotate the Playfield but keep the Receptors in a static position
        public void RotatePlayFieldStatic(OsbEasing easing, double starttime, double endtime, double radians)
        {

            this.rotation = radians;

            foreach (Column column in columns.Values)
            {
                Receptor receptor = column.receptor;

                receptor.RotateReceptor(easing, starttime, endtime, radians);
            }


        }

        // This will rotate the Playfield but move the Receptors dynamically to adjust for the position
        public void Rotate(OsbEasing easing, double starttime, double endtime, double radians, CenterType centerType = CenterType.middle)
        {

            var center = new Vector2(320, 240);

            if (centerType == CenterType.playfield)
            {
                center = calculatePlayFieldCenter(endtime);
            }

            if (centerType == CenterType.receptor)
            {
                Vector2 sumPositions = new Vector2(0, 0);
                int count = 0;

                foreach (Column column in columns.Values)
                {
                    Receptor receptor = column.receptor;
                    Vector2 pos = receptor.PositionAt(endtime);

                    sumPositions += pos;
                    count++;
                }

                center = sumPositions / count;
            }

            foreach (Column column in columns.Values)
            {
                Receptor receptor = column.receptor;
                NoteOrigin origin = column.origin;

                receptor.PivotReceptor(easing, starttime, endtime, radians, center);
                origin.PivotOrigin(easing, starttime, endtime, radians, center);
            }

        }

        public void RotateReceptor(OsbEasing easing, double starttime, double endtime, double radians, CenterType centerType = CenterType.middle)
        {

            var center = new Vector2(320, 240);

            if (centerType == CenterType.playfield)
            {
                center = calculatePlayFieldCenter(endtime);
            }

            if (centerType == CenterType.receptor)
            {
                Vector2 sumPositions = new Vector2(0, 0);
                int count = 0;

                foreach (Column column in columns.Values)
                {
                    Receptor receptor = column.receptor;
                    Vector2 pos = receptor.PositionAt(endtime);

                    sumPositions += pos;
                    count++;
                }

                center = sumPositions / count;
            }

            foreach (Column column in columns.Values)
            {
                Receptor receptor = column.receptor;

                receptor.PivotReceptor(easing, starttime, endtime, radians, center);
            }

        }

        public void RotateColumn(OsbEasing easing, double starttime, double endtime, double radians, ColumnType columnType, CenterTypeColumn centerType = CenterTypeColumn.middle)
        {

            var center = new Vector2(320, 240);

            if (centerType == CenterTypeColumn.playfield)
            {
                center = calculatePlayFieldCenter(starttime);
            }

            if (columnType == ColumnType.all)
            {
                foreach (Column column in columns.Values)
                {
                    Receptor receptor = column.receptor;
                    NoteOrigin origin = column.origin;

                    if (centerType == CenterTypeColumn.receptor)
                    {
                        center = receptor.PositionAt(starttime);
                        //receptor.RotateReceptor(starttime, duration, easing, radians);
                    }

                    if (centerType == CenterTypeColumn.column)
                    {
                        center = (receptor.PositionAt(starttime) + origin.PositionAt(starttime)) / 2;
                    }

                    if (centerType == CenterTypeColumn.columnX)
                    {
                        center = new Vector2(320f, 240f);
                        center.X = receptor.PositionAt(starttime).X;
                    }

                    receptor.PivotReceptor(easing, starttime, endtime, radians, center);
                    origin.PivotOrigin(easing, starttime, endtime, radians, center);
                }
            }
            else
            {
                Column column = columns[columnType];
                Receptor receptor = column.receptor;
                NoteOrigin origin = column.origin;

                if (centerType == CenterTypeColumn.receptor)
                {
                    center = receptor.PositionAt(starttime);
                    //receptor.RotateReceptor(starttime, duration, easing, radians);
                }

                if (centerType == CenterTypeColumn.column)
                {
                    center = (receptor.PositionAt(starttime) + origin.PositionAt(starttime)) / 2;
                }

                if (centerType == CenterTypeColumn.columnX)
                {
                    center = new Vector2(320f, 240f);
                    center.X = receptor.PositionAt(starttime).X;
                }

                receptor.PivotReceptor(easing, starttime, endtime, radians, center);
                origin.PivotOrigin(easing, starttime, endtime, radians, center);
            }

        }

        public void moveFieldX(OsbEasing easing, double starttime, double endtime, float amount)
        {
            foreach (Column column in columns.Values)
            {
                Receptor receptor = column.receptor;
                NoteOrigin origin = column.origin;

                receptor.MoveReceptorRelativeX(easing, starttime, endtime, amount);
                origin.MoveOriginRelativeX(easing, starttime, endtime, amount);
            }
        }

        public void moveFieldY(OsbEasing easing, double starttime, double endtime, float amount)
        {
            foreach (Column column in columns.Values)
            {
                Receptor receptor = column.receptor;
                NoteOrigin origin = column.origin;

                receptor.MoveReceptorRelativeY(easing, starttime, endtime, amount);
                origin.MoveOriginRelativeY(easing, starttime, endtime, amount);
            }
        }

        public void moveField(OsbEasing easing, double starttime, double endtime, float amountX, float amountY)
        {
            foreach (Column column in columns.Values)
            {
                Receptor receptor = column.receptor;
                NoteOrigin origin = column.origin;

                receptor.MoveReceptorRelative(easing, starttime, endtime, new Vector2(amountX, amountY));
                origin.MoveOriginRelative(easing, starttime, endtime, new Vector2(amountX, amountY));
            }
        }

        /*
                public double moveFieldAbsolute(double starttime, double endtime, OsbEasing easing, Vector2 newCenter)
                {

                    Vector2 center = calculatePlayFieldCenter(starttime);
                    Vector2 difference = new Vector2(newCenter.X - center.X, newCenter.Y - center.Y);

                    foreach (Column column in columns.Values)
                    {
                        Receptor receptor = column.receptor;
                        NoteOrigin origin = column.origin;

                        receptor.MoveReceptorAbsolute(starttime, currentReceptorPosition, easing, duration);
                        origin.MoveOriginAbsolute(starttime, currentOriginPosition, easing, duration);
                    }

                    return starttime + duration;
                }*/

        public void addEffect(double starttime, double endtime, EffectType type, string reference)
        {

            EffectInfo info = new EffectInfo(starttime, endtime, type, reference);

            this.effectReferenceByStartTime.Add(starttime, info);

        }

        public void addEffectWithValue(double starttime, double endtime, EffectType type, string reference, float value)
        {

            EffectInfo info = new EffectInfo(starttime, endtime, type, reference, value);

            this.effectReferenceByStartTime.Add(starttime, info);

        }

        public void fadeAt(double time, float fade)
        {
            FadeEffect effect = new FadeEffect(time, time, OsbEasing.None, fade);
            this.fadeAtTime.Add(time, effect);
        }

        public void fadeAt(double time, double endtime, float fade)
        {
            FadeEffect effect = new FadeEffect(time, endtime, OsbEasing.None, fade);
            this.fadeAtTime.Add(time, effect);
        }

        public void fadeAt(double time, double endtime, OsbEasing easing, float fade)
        {
            FadeEffect effect = new FadeEffect(time, endtime, easing, fade);
            this.fadeAtTime.Add(time, effect);
        }

        public void fadeAt(double time, float fade, ColumnType type)
        {
            FadeEffect effect = new FadeEffect(time, time, OsbEasing.None, fade, type);
            this.fadeAtTime.Add(time, effect);
        }

        public void fadeAt(double time, double endtime, float fade, ColumnType type)
        {
            FadeEffect effect = new FadeEffect(time, endtime, OsbEasing.None, fade, type);
            this.fadeAtTime.Add(time, effect);
        }

        public void fadeAt(double time, double endtime, OsbEasing easing, float fade, ColumnType type)
        {
            FadeEffect effect = new FadeEffect(time, endtime, easing, fade, type);
            this.fadeAtTime.Add(time, effect);
        }

        public float findFadeAtTime(double time, ColumnType col = ColumnType.all)
        {
            KeyValuePair<double, FadeEffect> currentFadeEffect = this.fadeAtTime
                   .Where(kvp => kvp.Key <= time && (kvp.Value.columnType == col || kvp.Value.columnType == ColumnType.all))
                   .OrderByDescending(kvp => kvp.Key)
                   .FirstOrDefault();

            return currentFadeEffect.Value.value;
        }

        public float getColumnWidth()
        {
            return this.width / 4;
        }

        public float calculateOffset()
        {
            return (absoluteWidth - width) / 2;
        }

        public float getColumnWidth(float width)
        {
            return width / 4;
        }

        public float calculateOffset(float width)
        {
            return (absoluteWidth - width) / 2;
        }

        public Vector2 calculatePlayFieldCenter(double currentTime)
        {
            Vector2 center;

            // Initialize to extreme values to correctly capture the extents
            Vector2 topLeft = new Vector2(float.MaxValue, float.MaxValue);
            Vector2 bottomRight = new Vector2(float.MinValue, float.MinValue);

            foreach (Column column in columns.Values)
            {
                Vector2 receptor = column.ReceptorPositionAt(currentTime);
                Vector2 origin = column.OriginPositionAt(currentTime);

                // Adjust topLeft to the minimum extents
                if (receptor.X < topLeft.X)
                {
                    topLeft.X = receptor.X;
                }
                if (origin.X < topLeft.X)
                {
                    topLeft.X = origin.X;
                }

                if (receptor.Y < topLeft.Y)
                {
                    topLeft.Y = receptor.Y;  // Corrected from receptor.X to receptor.Y
                }
                if (origin.Y < topLeft.Y)
                {
                    topLeft.Y = origin.Y;
                }

                // Adjust bottomRight to the maximum extents
                if (receptor.X > bottomRight.X)
                {
                    bottomRight.X = receptor.X;
                }
                if (origin.X > bottomRight.X)
                {
                    bottomRight.X = origin.X;
                }

                if (receptor.Y > bottomRight.Y)
                {
                    bottomRight.Y = receptor.Y;  // Corrected from receptor.X to receptor.Y
                }
                if (origin.Y > bottomRight.Y)
                {
                    bottomRight.Y = origin.Y;
                }
            }

            // Calculate the center from the adjusted topLeft and bottomRight
            center.X = (topLeft.X + bottomRight.X) / 2;
            center.Y = (topLeft.Y + bottomRight.Y) / 2;

            return center;
        }

    }
}