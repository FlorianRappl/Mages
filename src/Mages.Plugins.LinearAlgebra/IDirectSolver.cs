namespace Mages.Plugins.LinearAlgebra
{
    using System;

    /// <summary>
    /// Interface for any (direct) solver.
    /// </summary>
    public interface IDirectSolver
    {
        /// <summary>
        /// Solves the given system of linear equations for a target b.
        /// </summary>
        /// <param name="b">The vector b in A * x = b.</param>
        /// <returns>The solution vector x.</returns>
        Double[,] Solve(Double[,] b);
    }
}
