namespace ChessLogic
{
    public class Direction
    {
        public readonly static Direction North = new Direction(-1,0);
        public readonly static Direction South = new Direction(1,0);
        public readonly static Direction East = new Direction(0,1);
        public readonly static Direction West = new Direction(0,-1);
        public readonly static Direction NorthEast = North + East;
        public readonly static Direction NorthWest = North + West;
        public readonly static Direction SouthEast = South + East;
        public readonly static Direction SouthWest = South + West;

        public int RowChange { get; }
        public int ColmnChange { get; }

        public Direction(int rowChange, int columnChange)
        {
            RowChange = rowChange;
            ColmnChange = columnChange;
        }

        public static Direction operator +(Direction dir1, Direction dir2)
        {
            return new Direction(dir1.RowChange + dir2.RowChange, dir1.ColmnChange+dir2.ColmnChange);
        }
        public static Direction operator *(int scalar, Direction dir)
        {
            return new Direction(scalar * dir.RowChange, scalar * dir.ColmnChange);
        }


    }
}
