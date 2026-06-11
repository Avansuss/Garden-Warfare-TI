using Vector2 = UnityEngine.Vector2;

namespace DefaultNamespace
{
    public class Coordinate
    {
        public Vector2 Position;
        public Section Section;

        /// <summary>
        /// The coordinate system for a tile. houses the position and what section of a tile it occupies
        /// </summary>
        public Coordinate()
        {
            Position = new();
            Section = Section.Full;
        }

        /// <summary>
        /// The coordinate system for a tile. houses the position and what section of a tile it occupies
        /// </summary>
        public Coordinate(int x, int y) : this(new(), Section.North) {}

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
        public Coordinate(int x, int y, Section section) : this(new Vector2(x, y), section) {}

        /// <summary>
        /// The coordinate system for a tile. houses the position and what section of a tile it occupies
        /// </summary>
        /// <param name="position">The XY Position of the tile</param>
        /// <param name="section">The section of the tile</param>
        public Coordinate(Vector2 position, Section section)
        {
            this.Position = position;
            this.Section = section; 
        }

    }
}