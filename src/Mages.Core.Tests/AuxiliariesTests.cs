namespace Mages.Core.Tests
{
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;

    [TestFixture]
    public class AuxiliariesTests
    {
        [Test]
        public void ReduceWithNumberShouldYieldSingleCall()
        {
            var result = "reduce(add, 2, 3)".Eval();
            Assert.AreEqual(5.0, result);
        }

        [Test]
        public void ReduceWithBooleanShouldYieldSingleCall()
        {
            var result = "reduce(and, true, false)".Eval();
            Assert.AreEqual(false, result);
        }

        [Test]
        public void ReduceWithMatrixShouldYieldOneValue()
        {
            var result = "reduce(add, 0, [1, 2, 3, 4, 5, 6])".Eval();
            Assert.AreEqual(21.0, result);
        }

        [Test]
        public void ReduceWitObjectShouldYieldOneValue()
        {
            var result = "reduce(multiply, 1, new { a: 5, b: 4, c: 3 })".Eval();
            Assert.AreEqual(60.0, result);
        }

        [Test]
        public void WhereWithNumberSatisfiedShouldYieldValue()
        {
            var result = "where(greater(3), 5)".Eval();
            Assert.AreEqual(5.0, result);
        }

        [Test]
        public void WhereWithNumberNotSatisfiedShouldYieldNull()
        {
            var result = "where(less(3), 5)".Eval();
            Assert.AreEqual(null, result);
        }

        [Test]
        public void WhereWithMatrixPartiallySatisfiedShouldYieldVector()
        {
            var result = "where(greater(4), [1, 2, 3, 4, 5, 6])".Eval();
            var vector = result as Double[,];
            Assert.IsNotNull(vector);
            Assert.AreEqual(2, vector.Length);
            Assert.AreEqual(5.0, vector[0, 0]);
            Assert.AreEqual(6.0, vector[0, 1]);
        }

        [Test]
        public void WhereWithMatrixFullySatisfiedShouldYieldVector()
        {
            var result = "where(greater(0), [1, 2; 3, 4; 5, 6])".Eval();
            var vector = result as Double[,];
            Assert.IsNotNull(vector);
            Assert.AreEqual(6, vector.Length);
            Assert.AreEqual(1.0, vector[0, 0]);
            Assert.AreEqual(2.0, vector[0, 1]);
            Assert.AreEqual(3.0, vector[0, 2]);
            Assert.AreEqual(4.0, vector[0, 3]);
            Assert.AreEqual(5.0, vector[0, 4]);
            Assert.AreEqual(6.0, vector[0, 5]);
        }

        [Test]
        public void WhereWitObjectNotSatisfiedShouldYieldEmptyObject()
        {
            var result = "where(less(0), new { a: 5, b: 4, c: 3 })".Eval();
            var array = result as IDictionary<String, Object>;
            Assert.IsNotNull(array);
            Assert.AreEqual(0, array.Count);
        }

        [Test]
        public void WhereWitObjectPartiallySatisfiedShouldYieldObject()
        {
            var result = "where(less(5), new { a: 5, b: 4, c: 3 })".Eval();
            var array = result as IDictionary<String, Object>;
            Assert.IsNotNull(array);
            Assert.AreEqual(2, array.Count);
            Assert.AreEqual(4.0, array["b"]);
            Assert.AreEqual(3.0, array["c"]);
        }

        [Test]
        public void WhereWitObjectFullySatisfiedShouldYieldObject()
        {
            var result = "where(greater(0), new { a: 5, b: 4, c: 3 })".Eval();
            var array = result as IDictionary<String, Object>;
            Assert.IsNotNull(array);
            Assert.AreEqual(3, array.Count);
            Assert.AreEqual(5.0, array["a"]);
            Assert.AreEqual(4.0, array["b"]);
            Assert.AreEqual(3.0, array["c"]);
        }
    }
}
