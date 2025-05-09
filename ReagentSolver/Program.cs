using ReagentSolver.Domain;
using ReagentSolver.Utility;

namespace ReagentSolver
{
    public class Program
    {
        const int TUBE_SIZE = 4;
        private static readonly bool VERBOSE = false;

        private static readonly List<State> STATE = [];
        private static readonly HashSet<Dictionary<int[], int>> HISTORY = new(new DictionaryComparer(new ArrayComparer()));

        private static readonly Dictionary<string, int> IDX_BY_COLOUR = [];
        private static Dictionary<int, string> COLOUR_BY_IDX = [];

        static void Main()
        {
            List<Tube> tubes = ReadInputsAndInitSolver();
            
            if (IsInitialStateInvalid(tubes, out var messages))
            {
                Console.WriteLine("Check your data, yo!");
                foreach (string s in messages)
                {
                    Console.WriteLine($"{s}");
                }

                Console.ReadLine();
                return;
            }
            

            var validMoves = tubes.GetValidMoves(TUBE_SIZE);
            STATE.RecordState(VERBOSE, null, tubes, validMoves, COLOUR_BY_IDX);
            var solved = Solve();

            if (solved)
            {
                Console.WriteLine("Press any key to show solution.");
                Console.ReadLine();
                foreach (var move in STATE.Select(state => state.LastMove).ToList())
                {
                    move?.Print(true, COLOUR_BY_IDX);
                }
            }
            else
            {
                Console.WriteLine("NO DICE");
            }
        }

        private static List<Tube> ReadInputsAndInitSolver()
        {
            List<Tube> tubes = [];
            IDX_BY_COLOUR.Add("EMPTY", 0);

            Console.WriteLine("Enter the puzzle initial state. Include only filled tubes:");
            for (int i = 0; i < TUBE_SIZE; i++)
            {
                var line = Console.ReadLine();

                if (string.IsNullOrEmpty(line))
                {
                    break;
                }

                _ = line.Split("\t").Select((colour, t) =>
                {
                    if (tubes.Count == t)
                    {
                        tubes.Add(new Tube(t, []));
                    }

                    if (!IDX_BY_COLOUR.TryGetValue(colour, out int c))
                    {
                        c = IDX_BY_COLOUR.Count;
                        IDX_BY_COLOUR.Add(colour, c);
                    }
                    tubes[t].Colours.Add(c);

                    return 0;
                }).ToList();
            }
            COLOUR_BY_IDX = IDX_BY_COLOUR.Select(c => (c.Value, c.Key)).ToDictionary();

            Console.WriteLine("How many empty tubes?");
            int empty = int.Parse(Console.ReadLine()!);
            for (int i = 0; i < empty; i++)
            {
                var tube = Enumerable.Range(0, TUBE_SIZE).Select(_ => 0).ToList();
                tubes.Add(new Tube(tubes.Count, tube));
            }

            return tubes;
        }

        private static bool IsInitialStateInvalid(List<Tube> tubes, out List<string> messages)
        {
            messages = tubes.SelectMany(t => t.Colours)
                                .GroupBy(c => c)
                                .ToDictionary(c => c.Key, c => c.Count())
                                .Where(c => c.Value % 4 != 0)
                                .Select(c => $"{COLOUR_BY_IDX[c.Key]}: {c.Value}")
                                .ToList();

            return messages?.Count > 0;
        }

        private static bool Solve()
        {
            while (true)
            {
                if (STATE.Count is 0)
                {
                    return false;
                }

                State currentState = STATE[^1];

                if (currentState.Tubes.IsSolved())
                {
                    return true;
                }


                if (currentState.ValidMoves.Count == 0)
                {
                    STATE.PopState(VERBOSE);
                    continue;
                }

                Move move = currentState.ValidMoves.First();
                currentState.ValidMoves.Remove(move);
                var newState = currentState.Tubes.Pour(move, COLOUR_BY_IDX);
                var newStateHistory = newState.ToHistory();
                if (HISTORY.Contains(newStateHistory))
                {
                    continue;
                }
                HISTORY.Add(newStateHistory);
                STATE.RecordState(VERBOSE, move, newState, newState.GetValidMoves(TUBE_SIZE), COLOUR_BY_IDX);
            }
        }
    }
}
