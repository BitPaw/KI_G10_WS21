namespace GA.Recombiners
{
    public class RecombinerWeaveBitPaw : IRecombiner
    {
        public string Combine(string parentA, string parentB)
        {
            return CombineSequence(parentA, parentB);
        }

        public static string CombineSequence(string parentA, string parentB)
        {
            int length = parentA.Length;
            char[] equence = new char[length];

            for (int i = 0; i < length; i++)
            {
                equence[i] = ((i + 1) % 2) == 0 ? parentA[i] : parentB[i];
            }

            return new string(equence);
        }
    }
}
