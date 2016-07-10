namespace Mages.Plugins.Random
{
    using System;
    using Troschuetz.Random;
    using Troschuetz.Random.Distributions.Continuous;
    using Troschuetz.Random.Distributions.Discrete;
    using Troschuetz.Random.Generators;

    static class Generators
    {
        public static IGenerator Mt19937()
        {
            return new MT19937Generator();
        }

        public static IGenerator Mt19937(Int32 seed)
        {
            return new MT19937Generator(seed);
        }

        public static IGenerator Standard()
        {
            return new StandardGenerator();
        }

        public static IGenerator Standard(Int32 seed)
        {
            return new StandardGenerator(seed);
        }

        public static IGenerator XorShift128()
        {
            return new XorShift128Generator();
        }

        public static IGenerator XorShift128(Int32 seed)
        {
            return new NR3Generator(seed);
        }

        public static IGenerator Nr3()
        {
            return new NR3Generator();
        }

        public static IGenerator Nr3(Int32 seed)
        {
            return new NR3Generator(seed);
        }

        public static Object Weibull(IGenerator rng, Double alpha, Double beta)
        {
            return AsFunction(new WeibullDistribution(rng, alpha, beta));
        }

        public static Object Gamma(IGenerator rng, Double alpha, Double beta)
        {
            return AsFunction(new GammaDistribution(rng, alpha, beta));
        }

        public static Object Exp(IGenerator rng, Double lambda)
        {
            return AsFunction(new ExponentialDistribution(rng, lambda));
        }

        public static Object Chisq(IGenerator rng, Int32 alpha)
        {
            return AsFunction(new ChiSquareDistribution(rng, alpha));
        }

        public static Object Beta(IGenerator rng, Double alpha, Double beta)
        {
            return AsFunction(new BetaDistribution(rng, alpha, beta));
        }

        public static Object Gauss(IGenerator rng, Double mu, Double sigma)
        {
            return AsFunction(new NormalDistribution(rng, mu, sigma));
        }

        public static Object Laplace(IGenerator rng, Double alpha, Double mu)
        {
            return AsFunction(new LaplaceDistribution(rng, alpha, mu));
        }

        public static Object Pow(IGenerator rng, Double alpha, Double beta)
        {
            return AsFunction(new PowerDistribution(rng, alpha, beta));
        }

        public static Object StudT(IGenerator rng, Int32 nu)
        {
            return AsFunction(new StudentsTDistribution(rng, nu));
        }

        public static Object Pareto(IGenerator rng, Double alpha, Double beta)
        {
            return AsFunction(new ParetoDistribution(rng, alpha, beta));
        }

        public static Object Cauchy(IGenerator rng, Double alpha, Double gamma)
        {
            return AsFunction(new CauchyDistribution(rng, alpha, gamma));
        }

        public static Object Uni(IGenerator rng, Double alpha, Double beta)
        {
            return AsFunction(new ContinuousUniformDistribution(rng, alpha, beta));
        }

        public static Object Disc(IGenerator rng, Int32 alpha, Int32 beta)
        {
            return AsFunction(new DiscreteUniformDistribution(rng, alpha, beta));
        }

        public static Object Bernoulli(IGenerator rng, Double alpha)
        {
            return AsFunction(new BernoulliDistribution(rng, alpha));
        }

        public static Object Poisson(IGenerator rng, Double lambda)
        {
            return AsFunction(new PoissonDistribution(rng, lambda));
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