
namespace ReagentSolver.Utility
{
    public class ArrayComparer : IEqualityComparer<int[]>
    {
        public bool Equals(int[] x, int[] y)
        {
            if (x == null || y == null)
                return x == y;

            if (x.Length != y.Length)
                return false;

            for (int i = 0; i < x.Length; i++)
            {
                if (x[i] != y[i])
                    return false;
            }
            return true;
        }

        public int GetHashCode(int[] obj)
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));

            int hash = 17;
            foreach (var item in obj)
            {
                hash = hash * 31 + item;
            }
            return hash;
        }
    }

    public class DictionaryComparer : IEqualityComparer<Dictionary<int[], int>>
    {
        private readonly IEqualityComparer<int[]> _arrayComparer;

        public DictionaryComparer(IEqualityComparer<int[]> arrayComparer)
        {
            _arrayComparer = arrayComparer;
        }

        public bool Equals(Dictionary<int[], int> x, Dictionary<int[], int> y)
        {
            if (x == null || y == null)
                return x == y;

            // Check if both dictionaries have the same count of items
            if (x.Count != y.Count)
                return false;

            // Check if the dictionaries have the same keys and values
            foreach (var kvp in x)
            {
                if (!y.ContainsKey(kvp.Key) || y[kvp.Key] != kvp.Value)
                {
                    return false;
                }
            }

            return true;
        }

        public int GetHashCode(Dictionary<int[], int> obj)
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));

            int hash = 17;
            foreach (var kvp in obj)
            {
                // Combine the hash code for the array key and its count value
                hash = hash * 31 + _arrayComparer.GetHashCode(kvp.Key);
                hash = hash * 31 + kvp.Value.GetHashCode();
            }
            return hash;
        }
    }
}
