namespace Grid
{
    public class GridTile
    {
        public Coordinate Placement;
        public GridTileType TileType;

        /// <summary>
        /// A grid tile that houses its position and type of tile, set to Inactive by default
        /// </summary>
        /// <param name="placement">The position and section to place the tile on a grid</param>
        /// <param name="type">The type of tile to set the tile to, optional</param>
        public GridTile(Coordinate placement, GridTileType type = GridTileType.Inactive)
        {
            this.TileType = type;
            this.Placement = placement;
        }
    }
}