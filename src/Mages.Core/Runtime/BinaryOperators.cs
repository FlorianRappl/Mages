namespace Mages.Core.Runtime
{
    using Mages.Core.Runtime.Converters;
    using Mages.Core.Runtime.Functions;
    using System;

    static class BinaryOperators
    {
        public static Object Add(Object[] args)
        {
            return Curry.MinTwo(StandardOperators.Add, args) ??
                If.Is<Double, Double>(args, (y, x) => x + y) ??
                If.Is<Double[,], Double[,]>(args, (y, x) => x.Add(y)) ??
                If.Is<String, String>(args, (y, x) => String.Concat(x, y)) ??
                If.Is<Object, String>(args, (y, x) => String.Concat(x, Stringify.This(y))) ??
                If.Is<String, Object>(args, (y, x) => String.Concat(Stringify.This(x), y));
        }

        public static Object Sub(Object[] args)
        {
            return Curry.MinTwo(StandardOperators.Sub, args) ??
                If.Is<Double, Double>(args, (y, x) => x - y) ??
                If.Is<Double[,], Double[,]>(args, (y, x) => x.Subtract(y));
        }
        
        public static Object Mul(Object[] args)
        {
            return Curry.MinTwo(StandardOperators.Mul, args) ??
                If.Is<Double, Double>(args, (y, x) => x * y) ??
                If.Is<Double[,], Double[,]>(args, (y, x) => x.Multiply(y)) ??
                If.Is<Double, Double[,]>(args, (y, x) => x.Multiply(y)) ??
                If.Is<Double[,], Double>(args, Matrix.Multiply);
        }

        public static Object RDiv(Object[] args)
        {
            return Curry.MinTwo(StandardOperators.RDiv, args) ??
                If.Is<Double, Double>(args, (y, x) => x / y) ??
                If.Is<Double, Double[,]>(args, (y, x) => x.Divide(y));
        }

        public static Object LDiv(Object[] args)
        {
            return Curry.MinTwo(StandardOperators.LDiv, args) ??
                If.Is<Double, Double>(args, (y, x) => y / x) ??
                If.Is<Double[,], Double>(args, (y, x) => y.Divide(x));
        }

        public static Object Pow(Object[] args)
        {
            return Curry.MinTwo(StandardOperators.Pow, args) ??
                If.Is<Double, Double>(args, (y, x) => Math.Pow(x, y)) ??
                If.Is<Double[,], Double[,]>(args, (y, x) => x.Pow(y)) ??
                If.Is<Double[,], Double>(args, (y, x) => x.Pow(y)) ??
                If.Is<Double, Double[,]>(args, (y, x) => x.Pow(y));
        }

        public static Object Mod(Object[] args)
        {
            return Curry.MinTwo(StandardOperators.Mod, args) ??
                If.Is<Double, Double>(args, (y, x) => x % y);
        }

        public static Object And(Object[] args)
        {
            return Curry.MinTwo(StandardOperators.And, args) ??
                If.Is<Double, Double>(args, (y, x) => x.ToBoolean() && y.ToBoolean()) ??
                If.Is<Boolean, Boolean>(args, (y, x) => x && y) ??
                If.Is<Double[,], Double[,]>(args, (y, x) => x.And(y)) ??
                If.Is<Double[,], Double>(args, (y, x) => y.And(x)) ??
                If.Is<Double, Double[,]>(args, (y, x) => x.And(y)) ??
                If.Is<Double[,], Boolean>(args, (y, x) => y.And(x.ToNumber())) ??
                If.Is<Boolean, Double[,]>(args, (y, x) => x.And(y.ToNumber())) ??
                (args[1].ToBoolean() && args[0].ToBoolean());
        }

        public static Object Or(Object[] args)
        {
            return Curry.MinTwo(StandardOperators.Or, args) ??
                If.Is<Double, Double>(args, (y, x) => x.ToBoolean() || y.ToBoolean()) ??
                If.Is<Boolean, Boolean>(args, (y, x) => x || y) ??
                If.Is<Double[,], Double[,]>(args, (y, x) => x.Or(y)) ??
                If.Is<Double[,], Double>(args, (y, x) => y.Or(x)) ??
                If.Is<Double, Double[,]>(args, (y, x) => x.Or(y)) ??
                If.Is<Double[,], Boolean>(args, (y, x) => y.Or(x.ToNumber())) ??
                If.Is<Boolean, Double[,]>(args, (y, x) => x.Or(y.ToNumber())) ??
                (args[1].ToBoolean() || args[0].ToBoolean());
        }

        public static Object Eq(Object[] args)
        {
            return Curry.MinTwo(StandardOperators.Eq, args) ??
                If.Is<Double, Double>(args, (y, x) => x == y) ??
                If.Is<Boolean, Boolean>(args, (y, x) => x == y) ??
                If.Is<Double[,], Double[,]>(args, (y, x) => x.AreEqual(y)) ??
                If.Is<Double[,], Double>(args, (y, x) => y.AreEqual(x)) ??
                If.Is<Double, Double[,]>(args, (y, x) => x.AreEqual(y)) ??
                If.Is<Double[,], Boolean>(args, (y, x) => y.AreEqual(x.ToNumber())) ??
                If.Is<Boolean, Double[,]>(args, (y, x) => x.AreEqual(y.ToNumber())) ??
                If.Is<String, String>(args, (y, x) => y.Equals(x)) ??
                If.Is<String, Object>(args, (y, x) => y.Equals(Stringify.This(x))) ??
                If.Is<Object, String>(args, (y, x) => x.Equals(Stringify.This(y))) ??
                Object.ReferenceEquals(args[1], args[0]);
        }

        public static Object Neq(Object[] args)
        {
            return Curry.MinTwo(StandardOperators.Neq, args) ??
                If.Is<Double, Double>(args, (y, x) => x != y) ??
                If.Is<Boolean, Boolean>(args, (y, x) => x != y) ??
                If.Is<Double[,], Double[,]>(args, (y, x) => x.AreNotEqual(y)) ??
                If.Is<Double[,], Double>(args, (y, x) => y.AreNotEqual(x)) ??
                If.Is<Double, Double[,]>(args, (y, x) => x.AreNotEqual(y)) ??
                If.Is<Double[,], Boolean>(args, (y, x) => y.AreNotEqual(x.ToNumber())) ??
                If.Is<Boolean, Double[,]>(args, (y, x) => x.AreNotEqual(y.ToNumber())) ??
                If.Is<String, String>(args, (y, x) => !x.Equals(y)) ??
                If.Is<String, Object>(args, (y, x) => !y.Equals(Stringify.This(x))) ??
                If.Is<Object, String>(args, (y, x) => !x.Equals(Stringify.This(y))) ??
                !Object.ReferenceEquals(args[1], args[0]);
        }

        public static Object Gt(Object[] args)
        {
            return Curry.MinTwo(StandardOperators.Gt, args) ??
                If.Is<Double, Double>(args, (y, x) => x > y) ??
                If.Is<Double[,], Double[,]>(args, Matrix.IsLessThan) ??
                If.Is<Double[,], Double>(args, Matrix.IsLessThan) ??
                If.Is<Double, Double[,]>(args, (y, x) => x.IsGreaterThan(y)) ??
                (args[1].ToNumber() > args[0].ToNumber());
        }

        public static Object Geq(Object[] args)
        {
            return Curry.MinTwo(StandardOperators.Geq, args) ??
                If.Is<Double, Double>(args, (y, x) => x >= y) ??
                If.Is<Double[,], Double[,]>(args, Matrix.IsLessOrEqual) ??
                If.Is<Double[,], Double>(args, Matrix.IsLessOrEqual) ??
                If.Is<Double, Double[,]>(args, (y, x) => x.IsGreaterOrEqual(y)) ??
                (args[1].ToNumber() >= args[0].ToNumber());
        }

        public static Object Lt(Object[] args)
        {
            return Curry.MinTwo(StandardOperators.Lt, args) ??
                If.Is<Double, Double>(args, (y, x) => x < y) ??
                If.Is<Double[,], Double[,]>(args, Matrix.IsGreaterThan) ??
                If.Is<Double[,], Double>(args, Matrix.IsGreaterThan) ??
                If.Is<Double, Double[,]>(args, (y, x) => x.IsLessThan(y)) ??
                (args[1].ToNumber() < args[0].ToNumber());
        }

        public static Object Leq(Object[] args)
        {
            return Curry.MinTwo(StandardOperators.Leq, args) ??
                If.Is<Double, Double>(args, (y, x) => x <= y) ??
                If.Is<Double[,], Double[,]>(args, Matrix.IsGreaterOrEqual) ??
                If.Is<Double[,], Double>(args, Matrix.IsGreaterOrEqual) ??
                If.Is<Double, Double[,]>(args, (y, x) => x.IsLessOrEqual(y)) ??
                (args[1].ToNumber() <= args[0].ToNumber());
        }

        public static Object Pipe(Object[] args)
        {
            return Curry.MinTwo(StandardOperators.Pipe, args) ??
                If.Is<Function>(args, f => f.Invoke(new[] { args[1] }));
        }
    }
}
