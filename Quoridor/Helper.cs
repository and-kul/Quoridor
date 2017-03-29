namespace Quoridor
{
    public static class Helper
    {
        public static void Swap<T>(ref T left, ref T right)
        {
            var tmp = left;
            left = right;
            right = tmp;
        }
    }
}