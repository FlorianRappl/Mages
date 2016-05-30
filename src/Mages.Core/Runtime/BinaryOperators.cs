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
                If.Is<Double, Double>(args, (x, y) => x + y) ??
                If.Is<Double[,], Double[,]>(args, Matrix.Add) ??
                If.Is<String, String>(args, String.Concat) ??
                If.Is<Object, String>(args, (x, y) => String.Concat(Stringify.This(x), y)) ??
                If.Is<String, Object>(args, (x, y) => String.Concat(x, Stringify.This(y)));
        }

        public static Object Sub(Object[] args)
        {
            return Curry.MinTwo(StandardOperators.Sub, args) ??
                If.Is<Double, Double>(args, (x, y) => x - y) ??
                If.Is<Double[,], Double[,]>(args, Matrix.Subtract);
        }
        
        public static Object Mul(Object[] args)
        {
            return Curry.MinTwo(StandardOperators.Mul, args) ??
                If.Is<Double, Double>(args, (x, y) => x * y) ??
                If.Is<Double[,], Double[,]>(args, Matrix.Multiply) ??
                If.Is<Double, Double[,]>(args, (y, x) => x.Multiply(y)) ??
                If.Is<Double[,], Double>(args, Matrix.Multiply);
        }

        public static Object RDiv(Object[] args)
        {
            return Curry.MinTwo(StandardOperators.RDiv, args) ??
                If.Is<Double, Double>(args, (x, y) => x / y) ??
                If.Is<Double[,], Double>(args, Matrix.Divide);
        }

        public static Object LDiv(Object[] args)
        {
            return Curry.MinTwo(StandardOperators.LDiv, args) ??
                If.Is<Double, Double>(args, (x, y) => y / x) ??
                If.Is<Double, Double[,]>(args, (y, x) => x.Divide(y));
        }

        public static Object Pow(Object[] args)
        {
            return Curry.MinTwo(StandardOperators.Pow, args) ??
                If.Is<Double, Double>(args, (x, y) => Math.Pow(x, y)) ??
                If.Is<Double[,], Double[,]>(args, Matrix.Pow) ??
                If.Is<Double[,], Double>(args, Matrix.Pow) ??
                If.Is<Double, Double[,]>(args, Matrix.Pow);
        }

        public static Object Mod(Object[] args)
        {
            return Curry.MinTwo(StandardOperators.Mod, args) ??
                If.Is<Double, Double>(args, (x, y) => x % y);
        }

        public static Object And(Object[] args)
        {
            return Curry.MinTwo(StandardOperators.And, args) ??
                If.Is<Double, Double>(args, (x, y) => x.ToBoolean() && y.ToBoolean()) ??
                If.Is<Boolean, Boolean>(args, (x, y) => x && y) ??
                If.Is<Double[,], Double[,]>(args, Matrix.And) ??
                If.Is<Double[,], Double>(args, Matrix.And) ??
                If.Is<Double, Double[,]>(args, (y, x) => x.And(y)) ??
                If.Is<Double[,], Boolean>(args, (x, y) => x.And(y.ToNumber())) ??
                If.Is<Boolean, Double[,]>(args, (y, x) => x.And(y.ToNumber())) ??
                (args[0].ToBoolean() && args[1].ToBoolean());
        }

        public static Object Or(Object[] args)
        {
            return Curry.MinTwo(StandardOperators.Or, args) ??
                If.Is<Double, Double>(args, (x, y) => x.ToBoolean() || y.ToBoolean()) ??
                If.Is<Boolean, Boolean>(args, (x, y) => x || y) ??
                If.Is<Double[,], Double[,]>(args, Matrix.Or) ??
                If.Is<Double[,], Double>(args, Matrix.Or) ??
                If.Is<Double, Double[,]>(args, (y, x) => x.Or(y)) ??
                If.Is<Double[,], Boolean>(args, (x, y) => x.Or(y.ToNumber())) ??
                If.Is<Boolean, Double[,]>(args, (y, x) => x.Or(y.ToNumber())) ??
                (args[0].ToBoolean() || args[1].ToBoolean());
        }

        public static Object Eq(Object[] args)
        {
            return Curry.MinTwo(StandardOperators.Eq, args) ??
                If.Is<Double, Double>(args, (x, y) => x == y) ??
                If.Is<Boolean, Boolean>(args, (x, y) => x == y) ??
                If.Is<Double[,], Double[,]>(args, Matrix.AreEqual) ??
                If.Is<Double[,], Double>(args, Matrix.AreEqual) ??
                If.Is<Double, Double[,]>(args, (y, x) => x.AreEqual(y)) ??
                If.Is<Double[,], Boolean>(args, (x, y) => x.AreEqual(y.ToNumber())) ??
                If.Is<Boolean, Double[,]>(args, (y, x) => x.AreEqual(y.ToNumber())) ??
                If.Is<Object, Object>(args, (x, y) => Object.ReferenceEquals(x, y));
        }

        public static Object Neq(Object[] args)
        {
            return Curry.MinTwo(StandardOperators.Neq, args) ??
                If.Is<Double, Double>(args, (x, y) => x != y) ??
                If.Is<Boolean, Boolean>(args, (x, y) => x != y) ??
                If.Is<Double[,], Double[,]>(args, Matrix.AreNotEqual) ??
                If.Is<Double[,], Double>(args, Matrix.AreNotEqual) ??
                If.Is<Double, Double[,]>(args, (y, x) => x.AreNotEqual(y)) ??
                If.Is<Double[,], Boolean>(args, (x, y) => x.AreNotEqual(y.ToNumber())) ??
                If.Is<Boolean, Double[,]>(args, (y, x) => x.AreNotEqual(y.ToNumber())) ??
                If.Is<Object, Object>(args, (x, y) => !Object.ReferenceEquals(x, y));
        }

        public static Object Gt(Object[] args)
        {
            return Curry.MinTwo(StandardOperators.Gt, args) ??
                If.Is<Double, Double>(args, (x, y) => x > y) ??
                If.Is<Double[,], Double[,]>(args, Matrix.IsGreaterThan) ??
                If.Is<Double[,], Double>(args, Matrix.IsGreaterThan) ??
                If.Is<Double, Double[,]>(args, (y, x) => x.IsLessThan(y)) ??
                (args[0].ToNumber() > args[1].ToNumber());
        }

        public static Object Geq(Object[] args)
        {
            return Curry.MinTwo(StandardOperators.Geq, args) ??
                If.Is<Double, Double>(args, (x, y) => x >= y) ??
                If.Is<Double[,], Double[,]>(args, Matrix.IsGreaterOrEqual) ??
                If.Is<Double[,], Double>(args, Matrix.IsGreaterOrEqual) ??
                If.Is<Double, Double[,]>(args, (y, x) => x.IsLessOrEqual(y)) ??
                (args[0].ToNumber() >= args[1].ToNumber());
        }

        public static Object Lt(Object[] args)
        {
            return Curry.MinTwo(StandardOperators.Lt, args) ??
                If.Is<Double, Double>(args, (x, y) => x < y) ??
                If.Is<Double[,], Double[,]>(args, Matrix.IsLessThan) ??
                If.Is<Double[,], Double>(args, Matrix.IsLessThan) ??
                If.Is<Double, Double[,]>(args, (y, x) => x.IsGreaterThan(y)) ??
                (args[0].ToNumber() < args[1].ToNumber());
        }

        public static Object Leq(Object[] args)
        {
            return Curry.MinTwo(StandardOperators.Leq, args) ??
                If.Is<Double, Double>(args, (x, y) => x <= y) ??
                If.Is<Double[,], Double[,]>(args, Matrix.IsLessOrEqual) ??
                If.Is<Double[,], Double>(args, Matrix.IsLessOrEqual) ??
                If.Is<Double, Double[,]>(args, (y, x) => x.IsGreaterOrEqual(y)) ??
                (args[0].ToNumber() <= args[1].ToNumber());
        }
    }
}
