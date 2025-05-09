
namespace ReagentSolver.Domain
{
    public record Tube(int Index, List<int> Colours);

    public static class TubeHelpers
    {
        public static List<Tube> Clone(this List<Tube> original)
        {
            return original.Select(tube => tube with
            {
                Colours = tube.Colours.Select(c => c).ToList()
            }).ToList();
        }

        public static bool IsEmpty(this Tube tube)
        {
            return tube.Colours.All(colour => colour == 0);
        }

        public static int Top(this Tube tube)
        {
            var liquid = tube.Colours.Select((colour, i) => new { colour, i }).Where(x => x.colour != 0).ToList();

            if (liquid.Count != 0)
            {
                return liquid.Min(x => x.i);
            }

            return tube.Colours.Count;
        }

        public static bool CanReceive(this Tube tube, int SIZE, out int? colour, out int vol)
        {
            if (tube.IsEmpty())
            {
                colour = null;
                vol = SIZE;
                return true;
            }

            var top = tube.Top();
            if (top == 0)
            {
                colour = null;
                vol = 0;
                return false;
            }

            colour = tube.Colours[top];
            vol = top;
            return true;
        }

        public static List<Tube> Pour(this List<Tube> original, Move move, Dictionary<int, string> COLOURS)
        {
            var tubes = original.Clone();

            int top;
            int i;
            try
            {
                for (i = 0; i < move.Vol; i++)
                {

                    top = tubes[move.Source].Top();
                    tubes[move.Source].Colours[top] = 0;

                    top = tubes[move.Destination].Top();
                    tubes[move.Destination].Colours[top - 1] = move.Colour;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString() + $" {move.Source} > {move.Destination} ({COLOURS[move.Colour]}: {move.Vol})");
                throw;
            }

            return tubes;
        }

        public static bool IsSolved(this List<Tube> tubes)
        {
            return tubes.All(tube => tube.IsSolved());
        }

        public static bool IsSolved(this Tube tube)
        {
            return tube.Colours.All(colour => colour == tube.Colours[0]);
        }

        public static bool GetTopColour(this Tube tube, int SIZE, out int colour, out int vol)
        {
            if (tube.IsEmpty())
            {
                colour = 0;
                vol = SIZE;
                return false;
            }

            int top = tube.Top();
            colour = tube.Colours[top];
            vol = 1;
            for (int i = top + 1; i < SIZE; i++)
            {
                if (tube.Colours[i] != tube.Colours[i - 1])
                {
                    break;
                }
                vol++;
            }

            return true;
        }
    }
}

