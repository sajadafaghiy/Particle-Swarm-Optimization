using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParticleSwarmOptimization.Classes
{
    /// <summary>
    /// Particle Swarm Optimization algorithm.
    /// </summary>
    public class PSO
    {
        #region Constructor
        public PSO(int population, int iterations)
        {
            Population = population;
            Iterations = iterations;
        }
        #endregion

        #region Properties

        public int Population { get; set; }
        public int Iterations { get; set; }
        public double Weight { get; set; }
        public double Damp { get; set; }
        /// <summary>
        /// Learning coefficients
        /// </summary>
        /// <example> 
        /// This PSO property take two coefficients as c1 and c2.
        /// <code>
        /// Coefficients[0] -> c1
        /// Coefficients[1] -> c2
        /// </code>
        /// </example>
        public double[] Coefficients { get; set; }
        public Particle BestSolution { get { return globalBest; } }
        private Particle globalBest;

        #endregion

        #region Events

        public event AlgorithmStartHandler OnStart;
        public event AlgorithmIterationHandler OnIteration;
        public event AlgorithmEndHandler OnEnd;

        #endregion

        #region Methods

        /// <summary>
        /// Set weight, damp and coefficients value of particle swarm optimization algorithm.
        /// </summary>
        /// <param name="data">Is a tupple that contain weight, damp value value as double and coefficients as double array.</param>
        public void SetData((double weight, double damp, double[] coefficients) data)
        {
            Weight = data.weight;
            Damp = data.damp;
            Coefficients = data.coefficients;
        }

        public void Run(OptimizationProblem problem)
        {
            OnStart?.Invoke(this);

            double tempWeight = Weight; // Reset weight on each run.

            // Initialization of global best
            globalBest.Position = new double[problem.VariablesCount()];
            globalBest.ObjectiveValue = problem.WorstObjeciveValue;

            // Initialization of population
            Particle[] swarm = new Particle[Population];

            for (int i = 0; i < Population; i++)
            {
                swarm[i].Position = Randoms.Rand(0, 1, problem.VariablesCount());
                swarm[i].Velocity = new double[problem.VariablesCount()];
                swarm[i].EvaluateIn(problem);
                swarm[i].BestPosition = (double[])swarm[i].Position.Clone();
                swarm[i].BestActualPosition = (double[])swarm[i].ActualPosition.Clone();
                swarm[i].BestObjectiveValue = swarm[i].ObjectiveValue;

                if (problem.IsBetter(swarm[i].BestObjectiveValue, globalBest.ObjectiveValue))
                {
                    globalBest.Position = (double[])swarm[i].BestPosition.Clone();
                    globalBest.ActualPosition = (double[])swarm[i].BestActualPosition.Clone();
                    globalBest.ObjectiveValue = swarm[i].BestObjectiveValue;
                }
            }

            //PSO main loop
            for (int iter = 0; iter < Iterations; iter++)
            {
                for (int i = 0; i < Population; i++)
                {
                    for (int j = 0; j < problem.VariablesCount(); j++)
                    {
                        // Update Velocity
                        swarm[i].Velocity[j] = tempWeight * swarm[i].Velocity[j]
                            + Coefficients[0] * Randoms.Rand() * (swarm[i].BestPosition[j] - swarm[i].Position[j])
                            + Coefficients[1] * Randoms.Rand() * (globalBest.Position[j] - swarm[i].Position[j]);

                        // Update Position
                        swarm[i].Position[j] = swarm[i].Position[j] + swarm[i].Velocity[j];

                        // Apply lower and upper bounds limit
                        swarm[i].Position[j] = Math.Max(swarm[i].Position[j], 0);
                        swarm[i].Position[j] = Math.Min(swarm[i].Position[j], 1);
                    }

                    swarm[i].EvaluateIn(problem);

                    if (problem.IsBetter(swarm[i].ObjectiveValue, swarm[i].BestObjectiveValue))
                    {
                        swarm[i].BestPosition = (double[])swarm[i].Position.Clone();
                        swarm[i].BestActualPosition = (double[])swarm[i].ActualPosition.Clone();
                        swarm[i].BestObjectiveValue = swarm[i].ObjectiveValue;

                        if (problem.IsBetter(swarm[i].BestObjectiveValue, globalBest.ObjectiveValue))
                        {
                            globalBest.Position = (double[])swarm[i].BestPosition.Clone();
                            globalBest.ActualPosition = (double[])swarm[i].BestActualPosition.Clone();
                            globalBest.ObjectiveValue = swarm[i].BestObjectiveValue;
                        }
                    }
                }

                OnIteration?.Invoke(this, new IterationInfo { Iteration = iter + 1, BestObjectiveValue = globalBest.ObjectiveValue });

                // Damping inertia rate
                tempWeight *= Damp;
            }

            OnEnd?.Invoke(this);
        }

        #endregion
    }
}
