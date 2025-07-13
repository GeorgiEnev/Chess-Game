namespace ChessLogic
{
    public class EnPassant : Move
    {
        public override MoveType Type => MoveType.EnPassant;

        public override Position FromPos { get; }

        public override Position ToPos { get; }

        private readonly Position capturedPos;

        public EnPassant(Position from, Position to)
        {
            FromPos = from;
            ToPos = to;
            capturedPos = new Position(from.Row, to.Column);
        }
       
        public override bool Execute(Board board)
        {
           new NormalMove(FromPos, ToPos).Execute(board);
           board[capturedPos] = null;

            return true;
        }
    }
}
