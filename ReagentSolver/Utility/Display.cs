using ReagentSolver.Domain;
using System.Text;

namespace ReagentSolver.Utility
{
    public static class Display
    {
        public static void DisplayProgressBar(int steps, int totalSteps)
        {
            const int progressBarWidth = 50; 
            double progressPercentage = (double)steps / totalSteps;
            int progress = (int)(progressPercentage * progressBarWidth);

            string currentProgressBar = "[" + new string('#', progress) + new string('-', progressBarWidth - progress) + "]";

            Console.SetCursorPosition(0, Console.CursorTop);

            Console.Write(currentProgressBar);

            if (steps == totalSteps)
            {
                Console.Write($" DONE!  ");
            }
            else
            {
                Console.Write($" {progressPercentage * 100:0}%  ");
            }
        }

        public static void Print(this Move move, bool verbose, Dictionary<int, string> COLOURS)
        {
            if (verbose)
            {
                Console.WriteLine($"{COLOURS[move.Colour]}: {move.Vol} [{move.Source + 1} > {move.Destination + 1}]");
            }
        }

        public static void Print(this List<Tube> tubes, bool verbose)
        {
            if (verbose)
            {
                StringBuilder sb = new();
                foreach (var tube in tubes)
                {
                    sb.AppendLine(tube.Index + ": " + string.Join(", ", tube.Colours));
                }
                sb.AppendLine();

                Console.WriteLine(sb.ToString());
            }
            else
            {
                DisplayProgressBar(tubes.Count(t => !t.IsEmpty() && t.IsSolved()), tubes.Count(t => !t.IsEmpty()));
            }
        }
    }
}
