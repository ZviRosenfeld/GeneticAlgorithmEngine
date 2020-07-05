using System;

namespace GeneticAlgorithm.Components
{
    static class ComponetsUtils
    {
        public static (int, int) GetTwoRandomNumbers(int max)
        {
            var num1 = ProbabilityUtils.GetRandomInt(0, max);
            var num2 = ProbabilityUtils.GetRandomInt(0, max);

            return (Math.Min(num1, num2), Math.Max(num1, num2));
        }
    }
}
