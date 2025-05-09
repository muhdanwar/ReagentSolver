
namespace ReagentSolver.Domain
{
    public record Move(int Source, int Destination, int Colour, int Vol);

    public static class MoveHelper
    {
        public static List<Move> Clone(this List<Move> original)
        {
            return original.Select(move => move with { }).ToList();
        }
    }
}
