namespace Grid
{
    public class GridTile
    {
        public Coordinate Placement;

        private GridTileType tileType;

        /// <summary>
        /// A grid tile that houses its position and type of tile, set to Inactive by default
        /// </summary>
        /// <param name="placement">The position and section to place the tile on a grid</param>
        /// <param name="type">The type of tile to set the tile to, optional</param>
        public GridTile(Coordinate placement, GridTileType type = GridTileType.Inactive)
        {
            this.tileType = type;
            this.Placement = placement;
        }

        /// <summary>
        /// Sets the tile to a specific type. AIs are not allowed to set it to inactive or empty
        /// </summary>
        /// <param name="type">The tile type to set the tile to</param>
        /// <param name="isAI">Whether the caller of this function is an AI</param>
        /// <returns></returns>
        public bool SetTile(GridTileType type, bool isAI)
        {
            // The AI is not allowed to set tiles to inactive or empty
            if (isAI && (type == GridTileType.Inactive || type == GridTileType.Empty)) return false;

            tileType = type;
            
            return true;
        }
    }
}