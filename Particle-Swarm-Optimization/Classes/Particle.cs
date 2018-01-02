using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParticleSwarmOptimization.Classes
{
    public struct Particle
    {
        public double[] Position;
        public double[] ActualPosition;
        public double[] Velocity;
        public double ObjectiveValue;
        public double[] BestPosition;
        public double[] BestActualPosition;
        public double BestObjectiveValue;

        public void EvaluateIn(OptimizationProblem problem)
        {
            ActualPosition = problem.Parse(Position);
            ObjectiveValue = problem.Evaluate(ActualPosition);
        }
    }
}
