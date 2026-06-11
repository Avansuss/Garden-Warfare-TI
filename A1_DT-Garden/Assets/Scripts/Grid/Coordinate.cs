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
        /// <param name="x">The X position of the tile</param>
        /// <param name="y">The Y position of the tile</param>
        /// <param name="section">The section of the tile</param>
        public Coordinate(int x, int y, Section section)
        {
            this.Position = new(x, y);
            this.Section = section; 
        }
    }
}