using System;
using Unity.Mathematics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

namespace Grid
{
    public class Coordinate
    {
        public Vector3 Position;
        public Section Section;

        /// <summary>
        /// The coordinate system for a tile. houses the position and what section of a tile it occupies
        /// </summary>
        public Coordinate() : this(new(), Section.Full){}
        
        /// <summary>
        /// The coordinate system for a tile. houses the position and what section of a tile it occupies
        /// <param name="x">The X position of the tile</param>
        /// <param name="y">The Y position of the tile</param>
        /// </summary>
        public Coordinate(float x, float y) : this(new(x, y), Section.North) {}

        /// <summary>
        /// The coordinate system for a tile. houses the position and what section of a tile it occupies
        /// </summary>
        /// <param name="position">The XY Position of the tile</param>
        public Coordinate(Vector2 position) : this(position, Section.North) {}

        /// <summary>
        /// The coordinate system for a tile. houses the position and what section of a tile it occupies
        /// </summary>
        /// <param name="x">The X position of the tile</param>
        /// <param name="y">The Y position of the tile</param>
        /// <param name="section">The section of the tile</param>
        public Coordinate(float x, float y, Section section) : this(new Vector2(x, y), section) {}

        /// <summary>
        /// The coordinate system for a tile. houses the position and what section of a tile it occupies
        /// </summary>
        /// <param name="position">The XY Position of the tile</param>
        /// <param name="section">The section of the tile</param>
        public Coordinate(Vector2 position, Section section)
        {
            position.x = math.floor(position.x) + 0.5f;
            position.y = math.floor(position.y) + 0.5f;
            
            this.Position = new(position.x, 1, position.y);
            this.Section = section; 
        }

        /// <summary>
        /// Get the section angle in degrees
        /// </summary>
        /// <returns></returns>
        public int GetAngle()
        {
            var angle = 0;
            switch (this.Section)
            {
                case Section.Full:
                case Section.North:
                    angle = 0;
                    break;
                case Section.East:
                    angle = 90;
                    break;
                case Section.South:
                    angle = 180;
                    break;
                case Section.West:
                    angle = 270;
                    break;
            }

            return angle + 90;
        }

        /// <summary>
        /// Used to compare coordinates since floating point imprecision cannot do a more direct approach
        /// </summary>
        /// <param name="coord">the coordinate to compare against</param>
        /// <returns></returns>
        public bool IsEqualTo(Coordinate coord)
        {
            var position = coord.Position;
            return Mathf.Approximately(position.x, this.Position.x) &&
                Mathf.Approximately(position.y, this.Position.y) &&
                Mathf.Approximately(position.z, this.Position.z) &&
                coord.Section == this.Section;
        }
    }
}