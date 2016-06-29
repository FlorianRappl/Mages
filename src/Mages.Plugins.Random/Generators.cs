namespace Mages.Plugins.Random
{
    using System;
    using Troschuetz.Random;
    using Troschuetz.Random.Distributions.Continuous;
    using Troschuetz.Random.Distributions.Discrete;
    using Troschuetz.Random.Generators;

    static class Generators
    {
        public static Object Weibull(Double alpha, Double beta)
        {
            var rng = GetGenerator();
            return AsFunction(new WeibullDistribution(rng, alpha, beta));
        }

        public static Object Gamma(Double alpha, Double beta)
        {
            var rng = GetGenerator();
            return AsFunction(new GammaDistribution(rng, alpha, beta));
        }

        public static Object Exp(Double lambda)
        {
            var rng = GetGenerator();
            return AsFunction(new ExponentialDistribution(rng, lambda));
        }

        public static Object Chisq(Int32 alpha)
        {
            var rng = GetGenerator();
            return AsFunction(new ChiSquareDistribution(rng, alpha));
        }

        public static Object Beta(Double alpha, Double beta)
        {
            var rng = GetGenerator();
            return AsFunction(new BetaDistribution(rng, alpha, beta));
        }

        public static Object Gauss(Double mu, Double sigma)
        {
            var rng = GetGenerator();
            return AsFunction(new NormalDistribution(rng, mu, sigma));
        }

        public static Object Laplace(Double alpha, Double mu)
        {
            var rng = GetGenerator();
            return AsFunction(new LaplaceDistribution(rng, alpha, mu));
        }

        public static Object Pow(Double alpha, Double beta)
        {
            var rng = GetGenerator();
            return AsFunction(new PowerDistribution(rng, alpha, beta));
        }

        public static Object StudT(Int32 nu)
        {
            var rng = GetGenerator();
            return AsFunction(new StudentsTDistribution(rng, nu));
        }

        public static Object Pareto(Double alpha, Double beta)
        {
            var rng = GetGenerator();
            return AsFunction(new ParetoDistribution(rng, alpha, beta));
        }

        public static Object Cauchy(Double alpha, Double gamma)
        {
            var rng = GetGenerator();
            return AsFunction(new CauchyDistribution(rng, alpha, gamma));
        }

        public static Object Uni(Double alpha, Double beta)
        {
            var rng = GetGenerator();
            return AsFunction(new ContinuousUniformDistribution(rng, alpha, beta));
        }

        public static Object Disc(Int32 alpha, Int32 beta)
        {
            var rng = GetGenerator();
            return AsFunction(new DiscreteUniformDistribution(rng, alpha, beta));
        }

        public static Object Bernoulli(Double alpha)
        {
            var rng = GetGenerator();
            return AsFunction(new BernoulliDistribution(rng, alpha));
        }

        public static Object Poisson(Double lambda)
        {
            var rng = GetGenerator();
            return AsFunction(new PoissonDistribution(rng, lambda));
        }

        private static IGenerator GetGenerator()
        {
            return new MT19937Generator();
        }

        private static Func<Object[], Object> AsFunction(IDistribution dist)
        {
            return new Func<Object[], Object>(args =>
            {
                if (args.Length > 1 && args[0] is Double && args[1] is Double)
                {
                    return Matrix(dist, (Double)args[0], (Double)args[1]);
                }
                else if (args.Length > 0 && args[0] is Double)
                {
                    return Vector(dist, (Double)args[0]);
                }

                return dist.NextDouble();
            });
        }

        private static Double[,] Matrix(IDistribution dist, Double rows, Double columns)
        {
            var m = new Double[(Int32)rows, (Int32)columns];

            for (var i = 0; i < rows; i++)
            {
                for (var j = 0; j < columns; j++)
                {
                    m[i, j] = dist.NextDouble();
                }
            }

            return m;
        }

        private static Double[,] Vector(IDistribution dist, Double length)
        {
            return Matrix(dist, 1, length);
        }
    }
}