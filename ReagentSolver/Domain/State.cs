using ReagentSolver.Utility;

namespace ReagentSolver.Domain
{
    public record State(Move? LastMove, List<Tube> Tubes, List<Move> ValidMoves);

    public static class StateHelper
    {
        public static void RecordState(this List<State> state, bool verbose, Move? lastMove, List<Tube> tubes, List<Move> validMoves, Dictionary<int, string> COLOURS)
        {
            lastMove?.Print(verbose, COLOURS);
            state.Add(new State(lastMove, tubes.Clone(), validMoves.Clone()));
            if (verbose)
                Console.WriteLine($"Depth: {state.Count}");
            tubes.Print(verbose);
        }
        public static void PopState(this List<State> state, bool verbose)
        {
            state.RemoveAt(state.Count - 1);
            if (verbose)
            {
                Console.WriteLine("Reverted to:");
                Console.WriteLine($"Depth: {state.Count}");
            }
            state.LastOrDefault()?.Tubes.Print(verbose);
        }

        public static List<Move> GetValidMoves(this List<Tube> tubes, int SIZE)
        {
            List<Move> moves = [];
            foreach (var target in tubes)
            {
                if (!target.CanReceive(SIZE, out int? targetColour, out int maxVol))
                {
                    continue;
                }


                foreach (var source in tubes.Where(t => t.Index != target.Index).ToList())
                {
                    if (!source.GetTopColour(SIZE, out int sourceColour, out int vol) || sourceColour == 0 || source.IsSolved())
                    {
                        continue;
                    }

                    if (targetColour != null && sourceColour != targetColour)
                    {
                        continue;
                    }

                    if (maxVol == SIZE && source.Colours.All(c => c == 0 || c == sourceColour))
                    {
                        // useless move from a tube of 1 colour into an empty tube
                        continue;
                    }

                    if (maxVol >= vol)
                    {
                        moves.Add(new Move(source.Index, target.Index, sourceColour, vol));
                    }
                }
            }

            return moves;
        }
    }
}
