using System;

namespace ParticleSwarmOptimization.Classes
{
    public class Randoms
    {
        static Random random = new Random();

        /// <summary>
        /// Random of double type.
        /// </summary>
        /// <returns></returns>
        public static double Rand() => random.NextDouble();

        /// <summary>
        /// Uniform distribution between a and b. 
        /// </summary>
        /// <param name="a">The 1st number.</param>
        /// <param name="b">The 2nd number.</param>
        /// <returns></returns>
        public static double Rand(double a, double b) => a + (b - a) * Rand();

        /// <summary>
        /// Vector between a and b. 
        /// </summary>
        /// <param name="a">The 1st number.</param>
        /// <param name="b">The 2nd number.</param>
        /// <param name="n">Number of values.</param>
        /// <returns></returns>
        public static double[] Rand(double a, double b, int n)
        {
            double[] x = new double[n];
            for (int i = 0; i < n; i++)
            {
                x[i] = Rand(a, b);
            }
            return x;
        }
    }
}
