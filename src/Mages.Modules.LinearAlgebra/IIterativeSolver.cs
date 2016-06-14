namespace Mages.Modules.LinearAlgebra
{
    using System;

    /// <summary>
    /// Interface for any (iterative) solver.
    /// </summary>
    public interface IIterativeSolver : IDirectSolver
    {
        /// <summary>
        /// Gets the matrix A in A * x = b.
        /// </summary>
        Double[,] Matrix { get; }

        /// <summary>
        /// Gets or sets the maximum number of iterations.
        /// </summary>
        Int32 MaxIterations { get; set; }

        /// <summary>
        /// Gets or sets the starting vector x0.
        /// </summary>
        Double[,] Guess { get; set; }

        /// <summary>
        /// Gets or sets the tolerance level (when to stop the iteration?).
        /// </summary>
        Double Tolerance { get; set; }
    }
}
