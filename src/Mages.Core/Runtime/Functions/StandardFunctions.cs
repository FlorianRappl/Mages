namespace Mages.Core.Runtime.Functions
{
    using Mages.Core.Runtime.Converters;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// The collection of all standard functions.
    /// </summary>
    public static class StandardFunctions
    {
        /// <summary>
        /// Wraps the Math.Sqrt function.
        /// </summary>
        public static readonly Function Sqrt = new Function(args => 
        {
            return Curry.MinOne(Sqrt, args) ??
                If.Is<Double>(args, x => Math.Sqrt(x)) ??
                If.Is<Double[,]>(args, x => x.ForEach(Math.Sqrt)) ??
                Math.Sqrt(args[0].ToNumber());
        });

        /// <summary>
        /// Wraps the Math.Sign function.
        /// </summary>
        public static readonly Function Sign = new Function(args => 
        {
            return Curry.MinOne(Sign, args) ??
                If.Is<Double>(args, x => Helpers.Sign(x)) ??
                If.Is<Double[,]>(args, x => x.ForEach(Helpers.Sign)) ??
                Helpers.Sign(args[0].ToNumber());
        });

        /// <summary>
        /// Wraps the Math.Ceiling function.
        /// </summary>
        public static readonly Function Ceil = new Function(args => 
        {
            return Curry.MinOne(Ceil, args) ??
                If.Is<Double>(args, x => Math.Ceiling(x)) ??
                If.Is<Double[,]>(args, x => x.ForEach(Math.Ceiling)) ??
                Math.Ceiling(args[0].ToNumber());
        });

        /// <summary>
        /// Wraps the Math.Floor function.
        /// </summary>
        public static readonly Function Floor = new Function(args => 
        {
            return Curry.MinOne(Floor, args) ??
                If.Is<Double>(args, x => Math.Floor(x)) ??
                If.Is<Double[,]>(args, x => x.ForEach(Math.Floor)) ??
                Math.Floor(args[0].ToNumber());
        });

        /// <summary>
        /// Wraps the Math.Round function.
        /// </summary>
        public static readonly Function Round = new Function(args => 
        {
            return Curry.MinOne(Round, args) ??
                If.Is<Double>(args, x => Math.Round(x)) ??
                If.Is<Double[,]>(args, x => x.ForEach(Math.Round)) ??
                Math.Round(args[0].ToNumber());
        });

        /// <summary>
        /// Wraps the Math.Exp function.
        /// </summary>
        public static readonly Function Exp = new Function(args => 
        {
            return Curry.MinOne(Exp, args) ??
                If.Is<Double>(args, x => Math.Exp(x)) ??
                If.Is<Double[,]>(args, x => x.ForEach(Math.Exp)) ??
                Math.Exp(args[0].ToNumber());
        });

        /// <summary>
        /// Wraps the Math.Log function.
        /// </summary>
        public static readonly Function Log = new Function(args => 
        {
            return Curry.MinOne(Log, args) ??
                If.Is<Double>(args, x => Math.Log(x)) ??
                If.Is<Double[,]>(args, x => x.ForEach(Math.Log)) ??
                Math.Log(args[0].ToNumber());
        });

        /// <summary>
        /// Wraps the Math.Sin function.
        /// </summary>
        public static readonly Function Sin = new Function(args => 
        {
            return Curry.MinOne(Sin, args) ??
                If.Is<Double>(args, x => Math.Sin(x)) ??
                If.Is<Double[,]>(args, x => x.ForEach(Math.Sin)) ??
                Math.Sin(args[0].ToNumber());
        });

        /// <summary>
        /// Wraps the Math.Cos function.
        /// </summary>
        public static readonly Function Cos = new Function(args => 
        {
            return Curry.MinOne(Cos, args) ??
                If.Is<Double>(args, x => Math.Cos(x)) ??
                If.Is<Double[,]>(args, x => x.ForEach(Math.Cos)) ??
                Math.Cos(args[0].ToNumber());
        });

        /// <summary>
        /// Wraps the Math.Tan function.
        /// </summary>
        public static readonly Function Tan = new Function(args =>
        {
            return Curry.MinOne(Tan, args) ??
                If.Is<Double>(args, x => Math.Tan(x)) ??
                If.Is<Double[,]>(args, x => x.ForEach(Math.Tan)) ??
                Math.Tan(args[0].ToNumber());
        });

        /// <summary>
        /// Wraps the Math.Sinh function.
        /// </summary>
        public static readonly Function Sinh = new Function(args =>
        {
            return Curry.MinOne(Sinh, args) ??
                If.Is<Double>(args, x => Math.Sinh(x)) ??
                If.Is<Double[,]>(args, x => x.ForEach(Math.Sinh)) ??
                Math.Sinh(args[0].ToNumber());
        });

        /// <summary>
        /// Wraps the Math.Cosh function.
        /// </summary>
        public static readonly Function Cosh = new Function(args =>
        {
            return Curry.MinOne(Cosh, args) ??
                If.Is<Double>(args, x => Math.Cosh(x)) ??
                If.Is<Double[,]>(args, x => x.ForEach(Math.Cosh)) ??
                Math.Cosh(args[0].ToNumber());
        });

        /// <summary>
        /// Wraps the Math.Tanh function.
        /// </summary>
        public static readonly Function Tanh = new Function(args =>
        {
            return Curry.MinOne(Tanh, args) ??
                If.Is<Double>(args, x => Math.Tanh(x)) ??
                If.Is<Double[,]>(args, x => x.ForEach(Math.Tanh)) ??
                Math.Tanh(args[0].ToNumber());
        });

        /// <summary>
        /// Wraps the Math.Asin function.
        /// </summary>
        public static readonly Function ArcSin = new Function(args =>
        {
            return Curry.MinOne(ArcSin, args) ??
                If.Is<Double>(args, x => Math.Asin(x)) ??
                If.Is<Double[,]>(args, x => x.ForEach(Math.Asin)) ??
                Math.Asin(args[0].ToNumber());
        });

        /// <summary>
        /// Wraps the Math.Acos function.
        /// </summary>
        public static readonly Function ArcCos = new Function(args =>
        {
            return Curry.MinOne(ArcCos, args) ??
                If.Is<Double>(args, x => Math.Acos(x)) ??
                If.Is<Double[,]>(args, x => x.ForEach(Math.Acos)) ??
                Math.Acos(args[0].ToNumber());
        });

        /// <summary>
        /// Wraps the Math.Atan function.
        /// </summary>
        public static readonly Function ArcTan = new Function(args =>
        {
            return Curry.MinOne(ArcTan, args) ??
                If.Is<Double>(args, x => Math.Atan(x)) ??
                If.Is<Double[,]>(args, x => x.ForEach(Math.Atan)) ??
                Math.Atan(args[0].ToNumber());
        });

        /// <summary>
        /// Contains the random function.
        /// </summary>
        public static readonly Function Rand = new Function(args => 
        {
            return If.Is<Double, Double>(args, SimpleRandom.CreateMatrix) ??
                If.Is<Double>(args, SimpleRandom.CreateVector) ??
                SimpleRandom.GetNumber();
        });

        /// <summary>
        /// Contains the throw function.
        /// </summary>
        public static readonly Function Throw = new Function(args =>
        {
            return Curry.MinOne(Throw, args) ??
                new Exception(Stringify.This(args[0]));
        });

        /// <summary>
        /// Contains the catch function.
        /// </summary>
        public static readonly Function Catch = new Function(args =>
        {
            return Curry.MinOne(Catch, args) ??
                If.Is<Function>(args, Helpers.SafeExecute);
        });

        /// <summary>
        /// Contains the length function.
        /// </summary>
        public static readonly Function Length = new Function(args =>
        {
            return Curry.MinOne(Length, args) ??
                If.Is<Double[,]>(args, x => (Double)x.GetCount()) ??
                If.Is<IDictionary<String, Object>>(args, x => (Double)x.Count) ??
                If.Is<String>(args, x => (Double)x.Length) ??
                1.0;
        });

        /// <summary>
        /// Contains the sum function.
        /// </summary>
        public static readonly Function Sum = new Function(args =>
        {
            return Curry.MinOne(Sum, args) ??
                If.Is<Double[,]>(args, x => x.Reduce((a, b) => a + b)) ??
                args[0].ToNumber();
        });

        /// <summary>
        /// Wraps the Math.Min function.
        /// </summary>
        public static readonly Function Min = new Function(args =>
        {
            return Curry.MinOne(Min, args) ??
                If.Is<Double[,]>(args, x => x.Reduce(Math.Min)) ??
                args[0].ToNumber();
        });

        /// <summary>
        /// Wraps the Math.Max function.
        /// </summary>
        public static readonly Function Max = new Function(args =>
        {
            return Curry.MinOne(Max, args) ??
                If.Is<Double[,]>(args, x => x.Reduce(Math.Max)) ??
                args[0].ToNumber();
        });

        /// <summary>
        /// Wraps the Enumerable.OrderBy function.
        /// </summary>
        public static readonly Function Sort = new Function(args =>
        {
            return Curry.MinOne(Sort, args) ??
                If.Is<Double[,]>(args, x => x.ToVector().OrderBy(y => y).ToMatrix()) ??
                args[0];
        });

        /// <summary>
        /// Wraps the Enumerable.Reverse function.
        /// </summary>
        public static readonly Function Reverse = new Function(args =>
        {
            return Curry.MinOne(Reverse, args) ??
                If.Is<Double[,]>(args, x => x.ToVector().Reverse().ToMatrix()) ??
                args[0];
        });

        /// <summary>
        /// Wraps the Double.IsNaN function.
        /// </summary>
        public static readonly Function IsNaN = new Function(args =>
        {
            return Curry.MinOne(IsNaN, args) ??
                If.Is<Double>(args, x => Double.IsNaN(x)) ??
                If.Is<Double[,]>(args, x => x.ForEach(y => Double.IsNaN(y).ToNumber())) ??
                false;
        });

        /// <summary>
        /// Contains the is integer function.
        /// </summary>
        public static readonly Function IsInt = new Function(args =>
        {
            return Curry.MinOne(IsInt, args) ??
                If.Is<Double>(args, x => Logic.IsInteger(x)) ??
                If.Is<Double[,]>(args, x => x.ForEach(y => Logic.IsInteger(y).ToNumber())) ??
                false;
        });

        /// <summary>
        /// Contains the is prime function.
        /// </summary>
        public static readonly Function IsPrime = new Function(args =>
        {
            return Curry.MinOne(IsPrime, args) ??
                If.Is<Double>(args, x => Logic.IsPrime(x)) ??
                If.Is<Double[,]>(args, x => x.ForEach(y => Logic.IsPrime(y).ToNumber())) ??
                false;
        });

        /// <summary>
        /// Wraps the Double.IsInfinity function.
        /// </summary>
        public static readonly Function IsInfty = new Function(args =>
        {
            return Curry.MinOne(IsPrime, args) ??
                If.Is<Double>(args, x => Double.IsInfinity(x)) ??
                If.Is<Double[,]>(args, x => x.ForEach(y => Double.IsInfinity(y).ToNumber())) ??
                false;
        });

        /// <summary>
        /// Contains the any function.
        /// </summary>
        public static readonly Function Any = new Function(args =>
        {
            return Curry.MinOne(Any, args) ??
                If.Is<Double[,]>(args, x => x.AnyTrue()) ??
                args[0].ToBoolean();
        });

        /// <summary>
        /// Contains the all function.
        /// </summary>
        public static readonly Function All = new Function(args =>
        {
            return Curry.MinOne(All, args) ??
                If.Is<Double[,]>(args, x => x.AllTrue()) ??
                args[0].ToBoolean();
        });

        /// <summary>
        /// Contains the list function.
        /// </summary>
        public static readonly Function List = new Function(Helpers.ToArrayObject);
    }
}
