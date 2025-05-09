using ReagentSolver.Domain;
using History = System.Collections.Generic.Dictionary<int[], int>;

namespace ReagentSolver.Utility
{
    public static class HistoryHelper
    {
        public static History ToHistory(this List<Tube> tubes)
        {
            var state = new History(new ArrayComparer());
            foreach (var tube in tubes)
            {
                if (state.ContainsKey([.. tube.Colours]))
                {
                    state[[.. tube.Colours]]++;
                }
                else
                {
                    state.Add([.. tube.Colours], 1);
                }
            }

            return state;
        }
    }
}
