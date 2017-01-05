namespace Mages.Core.Runtime
{
    using Mages.Core.Runtime.Converters;
    using Mages.Core.Runtime.Functions;
    using System;

    static class BinaryOperators
    {
        #region Converter Fields

        private static readonly Func<Object, Double> AsNumber = m => m.ToNumber();

        #endregion

        #region Add Fields

        private static readonly Func<Double, Double, Object> AddNumbers = (y, x) => x + y;
        private static readonly Func<Double[,], Double[,], Object> AddMatrices = (y, x) => x.Add(y);
        private static readonly Func<String, String, Object> AddStrings = (y, x) => String.Concat(x, y);
        private static readonly Func<Object, String, Object> AddAnyStr = (y, x) => String.Concat(x, Stringify.This(y));
        private static readonly Func<String, Object, Object> AddStrAny = (y, x) => String.Concat(Stringify.This(x), y);

        #endregion

        #region Sub Fields

        private static readonly Func<Double, Double, Object> SubNumbers = (y, x) => x - y;
        private static readonly Func<Double[,], Double[,], Object> SubMatrices = (y, x) => x.Subtract(y);

        #endregion

        #region Mul Fields

        private static readonly Func<Double, Double, Object> MulNumbers = (y, x) => x * y;
        private static readonly Func<Double[,], Double[,], Object> MulMatrices = (y, x) => x.Multiply(y);
        private static readonly Func<Double, Double[,], Object> MulNumMat = (y, x) => x.Multiply(y);
        private static readonly Func<Double[,], Double, Object> MulMatNum = (y, x) => y.Multiply(x);

        #endregion

        #region RDiv Fields

        private static readonly Func<Double, Double, Object> RDivNumbers = (y, x) => x / y;
        private static readonly Func<Double, Double[,], Object> RDivNumMat = (y, x) => x.Divide(y);

        #endregion

        #region LDiv Fields

        private static readonly Func<Double, Double, Object> LDivNumbers = (y, x) => y / x;
        private static readonly Func<Double[,], Double, Object> LDivMatNum = (y, x) => y.Divide(x);

        #endregion

        #region Pow Fields

        private static readonly Func<Double, Double, Object> PowNumbers = (y, x) => Math.Pow(x, y);
        private static readonly Func<Double[,], Double[,], Object> PowMatrices = (y, x) => x.Pow(y);
        private static readonly Func<Double[,], Double, Object> PowMatNum = (y, x) => x.Pow(y);
        private static readonly Func<Double, Double[,], Object> PowNumMat = (y, x) => x.Pow(y);

        #endregion

        #region Other Fields

        private static readonly Func<Double, Double, Object> ModNumbers = (y, x) => x % y;
        private static readonly Func<Function, Object, Object> InvokeFunction = (f, arg) => f.Invoke(new[] { arg });

        #endregion

        #region Functions

        public static Object Add(Object[] args)
        {
            return If.Is<Double, Double>(args, AddNumbers) ??
                If.Is<Double[,], Double[,]>(args, AddMatrices) ??
                If.Is<String, String>(args, AddStrings) ??
                If.Is<Object, String>(args, AddAnyStr) ??
                If.Is<String, Object>(args, AddStrAny) ??
                If.IsNotNull(args, AsNumber, AddNumbers);
        }

        public static Object Sub(Object[] args)
        {
            return If.Is<Double, Double>(args, SubNumbers) ??
                If.Is<Double[,], Double[,]>(args, SubMatrices) ??
                If.IsNotNull(args, AsNumber, SubNumbers);
        }
        
        public static Object Mul(Object[] args)
        {
            return If.Is<Double, Double>(args, MulNumbers) ??
                If.Is<Double[,], Double[,]>(args, MulMatrices) ??
                If.Is<Double, Double[,]>(args, MulNumMat) ??
                If.Is<Double[,], Double>(args, MulMatNum) ??
                If.IsNotNull(args, AsNumber, MulNumbers);
        }

        public static Object RDiv(Object[] args)
        {
            return If.Is<Double, Double>(args, RDivNumbers) ??
                If.Is<Double, Double[,]>(args, RDivNumMat) ??
                If.IsNotNull(args, AsNumber, RDivNumbers);
        }

        public static Object LDiv(Object[] args)
        {
            return If.Is<Double, Double>(args, LDivNumbers) ??
                If.Is<Double[,], Double>(args, LDivMatNum) ??
                If.IsNotNull(args, AsNumber, LDivNumbers);
        }

        public static Object Pow(Object[] args)
        {
            return If.Is<Double, Double>(args, PowNumbers) ??
                If.Is<Double[,], Double[,]>(args, PowMatrices) ??
                If.Is<Double[,], Double>(args, PowMatNum) ??
                If.Is<Double, Double[,]>(args, PowNumMat) ??
                If.IsNotNull(args, AsNumber, PowNumbers);
        }

        public static Object Mod(Object[] args)
        {
            return If.Is<Double, Double>(args, ModNumbers) ??
                If.IsNotNull(args, AsNumber, ModNumbers);
        }

        public static Object And(Object[] args)
        {
            return If.Is<Double, Double>(args, (y, x) => x.ToBoolean() && y.ToBoolean()) ??
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
            return If.Is<Double, Double>(args, (y, x) => x.ToBoolean() || y.ToBoolean()) ??
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
            return If.Is<Double, Double>(args, (y, x) => x == y) ??
                If.Is<Boolean, Boolean>(args, (y, x) => x == y) ??
                If.Is<Double[,], Double[,]>(args, (y, x) => x.AreEqual(y)) ??
                If.Is<Double[,], Double>(args, (y, x) => y.AreEqual(x)) ??
                If.Is<Double, Double[,]>(args, (y, x) => x.AreEqual(y)) ??
                If.Is<Double[,], Boolean>(args, (y, x) => y.AreEqual(x.ToNumber())) ??
                If.Is<Boolean, Double[,]>(args, (y, x) => x.AreEqual(y.ToNumber())) ??
                If.Is<String, String>(args, (y, x) => y.Equals(x)) ??
                Object.ReferenceEquals(args[1], args[0]);
        }

        public static Object Neq(Object[] args)
        {
            return If.Is<Double, Double>(args, (y, x) => x != y) ??
                If.Is<Boolean, Boolean>(args, (y, x) => x != y) ??
                If.Is<Double[,], Double[,]>(args, (y, x) => x.AreNotEqual(y)) ??
                If.Is<Double[,], Double>(args, (y, x) => y.AreNotEqual(x)) ??
                If.Is<Double, Double[,]>(args, (y, x) => x.AreNotEqual(y)) ??
                If.Is<Double[,], Boolean>(args, (y, x) => y.AreNotEqual(x.ToNumber())) ??
                If.Is<Boolean, Double[,]>(args, (y, x) => x.AreNotEqual(y.ToNumber())) ??
                If.Is<String, String>(args, (y, x) => !x.Equals(y)) ??
                !Object.ReferenceEquals(args[1], args[0]);
        }

        public static Object Gt(Object[] args)
        {
            return If.Is<Double, Double>(args, (y, x) => x > y) ??
                If.Is<Double[,], Double[,]>(args, Matrix.IsLessThan) ??
                If.Is<Double[,], Double>(args, Matrix.IsLessThan) ??
                If.Is<Double, Double[,]>(args, (y, x) => x.IsGreaterThan(y)) ??
                (args[1].ToNumber() > args[0].ToNumber());
        }

        public static Object Geq(Object[] args)
        {
            return If.Is<Double, Double>(args, (y, x) => x >= y) ??
                If.Is<Double[,], Double[,]>(args, Matrix.IsLessOrEqual) ??
                If.Is<Double[,], Double>(args, Matrix.IsLessOrEqual) ??
                If.Is<Double, Double[,]>(args, (y, x) => x.IsGreaterOrEqual(y)) ??
                (args[1].ToNumber() >= args[0].ToNumber());
        }

        public static Object Lt(Object[] args)
        {
            return If.Is<Double, Double>(args, (y, x) => x < y) ??
                If.Is<Double[,], Double[,]>(args, Matrix.IsGreaterThan) ??
                If.Is<Double[,], Double>(args, Matrix.IsGreaterThan) ??
                If.Is<Double, Double[,]>(args, (y, x) => x.IsLessThan(y)) ??
                (args[1].ToNumber() < args[0].ToNumber());
        }

        public static Object Leq(Object[] args)
        {
            return If.Is<Double, Double>(args, (y, x) => x <= y) ??
                If.Is<Double[,], Double[,]>(args, Matrix.IsGreaterOrEqual) ??
                If.Is<Double[,], Double>(args, Matrix.IsGreaterOrEqual) ??
                If.Is<Double, Double[,]>(args, (y, x) => x.IsLessOrEqual(y)) ??
                (args[1].ToNumber() <= args[0].ToNumber());
        }

        public static Object Pipe(Object[] args)
        {
            return If.Is<Function, Object>(args, InvokeFunction);
        }

        #endregion
    }
}
