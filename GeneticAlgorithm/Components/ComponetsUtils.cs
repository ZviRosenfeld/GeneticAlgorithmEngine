using System;

namespace GeneticAlgorithm.Components
{
    static class ComponetsUtils
    {
        public static (int, int) GetTwoRandomNumbers(this Random random, int max)
        {
            var num1 = random.Next(max);
            var num2 = random.Next(max);

            return (Math.Min(num1, num2), Math.Max(num1, num2));
        }
    }
}
