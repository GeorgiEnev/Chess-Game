namespace ChessLogic
{
    public class Result
    {
        public Player Winner { get; }
        public EndReason Reason { get; }

        public Result(Player winner, EndReason endReason)
        {
            Winner = winner;
            Reason = endReason;
        }

        public static Result Win(Player winner)
        {
            return new Result(winner, EndReason.Checkmate);
        }
        public static Result Draw(EndReason reason)
        {
            return new Result(Player.None, reason);
        }
    }
}
