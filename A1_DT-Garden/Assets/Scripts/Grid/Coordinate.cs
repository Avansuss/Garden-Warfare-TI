using Vector2 = UnityEngine.Vector2;

namespace DefaultNamespace
{
    public class Coordinate
    {
        public Vector2 Position;
        public Section Section;

        public Coordinate(int x, int y, Section section)
        {
            this.Position = new(x, y);
            this.Section = section; 
        }
    }
}