namespace Mages.Core.Tests
{
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;

    [TestFixture]
    public class InterpretationTests
    {
        [Test]
        public void FactorialOfNegativeNumberShouldBeNegative()
        {
            Test("-3!", -6.0);
        }

        [Test]
        public void AddAndMultiplyNumbersShouldYieldRightResult()
        {
            Test("2+3*4", 14.0);
        }

        [Test]
        public void CallPreviouslyCreatedFunction()
        {
            Test("((x, y) => x + y)(2, 3)", 5.0);
        }

        [Test]
        public void CallFunctionCreatedFromFunction()
        {
            Test("(x => y => x + y)(2)(3)", 5.0);
        }

        [Test]
        public void CallFunctionStoredInGlobalVariable()
        {
            var scope = Test("A = (x, y) => x + y; B = A(2, 3); A(4, 3)", 7.0);
            Assert.AreEqual(5.0, scope["B"]);
            Assert.IsInstanceOf<Function>(scope["A"]);
        }

        [Test]
        public void CallFunctionStoredInGlobalFunctionAndCreatedFromFunction()
        {
            var scope = Test("A = x => y => x + y; B = A(2); C = B(3); B(4)", 6.0);
            Assert.AreEqual(5.0, scope["C"]);
            Assert.IsInstanceOf<Function>(scope["A"]);
            Assert.IsInstanceOf<Function>(scope["B"]);
        }

        [Test]
        public void CompilationWorksAndCanBeExecutedRepeaditly()
        {
            var engine = new Engine();
            var func = engine.Compile("A = 6; B = 7; A * B");
            var result1 = func.Invoke();
            var result2 = func.Invoke();
            Assert.AreEqual(42.0, result1);
            Assert.AreEqual(42.0, result2);
        }

        [Test]
        public void CompilationUsingScopeWorksAndCanBeExecutedRepeaditlyWithDifferentInputs()
        {
            var engine = new Engine();
            var func = engine.Compile("A * B - C");
            engine.Scope["A"] = 1.0;
            engine.Scope["B"] = 2.0;
            engine.Scope["C"] = 0.0;
            var result1 = func.Invoke();
            engine.Scope["B"] = 3.0;
            engine.Scope["C"] = 4.0;
            var result2 = func.Invoke();
            Assert.AreEqual(2.0, result1);
            Assert.AreEqual(-1.0, result2);
        }

        [Test]
        public void InitializeObjectAndAccessEntry()
        {
            var scope = Test("A = new { a: 4, b: 6 }; A.a", 4.0);
            var obj = scope["A"] as IDictionary<String, Object>;

            Assert.IsNotNull(obj);
            Assert.AreEqual(4.0, obj["a"]);
            Assert.AreEqual(6.0, obj["b"]);
        }

        [Test]
        public void InitializeObjectAndChangeEntry()
        {
            var scope = Test("A = new { a: 4, b: 6 }; A.b = 7.0", 7.0);
            var obj = scope["A"] as IDictionary<String, Object>;

            Assert.IsNotNull(obj);
            Assert.AreEqual(4.0, obj["a"]);
            Assert.AreEqual(7.0, obj["b"]);
        }

        [Test]
        public void InitializeObjectAndAddEntry()
        {
            var scope = Test("A = new { a: 4, b: 6 }; A.c = 9.0", 9.0);
            var obj = scope["A"] as IDictionary<String, Object>;

            Assert.IsNotNull(obj);
            Assert.AreEqual(4.0, obj["a"]);
            Assert.AreEqual(6.0, obj["b"]);
            Assert.AreEqual(9.0, obj["c"]);
        }

        [Test]
        public void SetMatrixEntryWithValidValueAndSingleIndexAfterCreatingIt()
        {
            var scope = Test("A = [1,2;3,4]; A(1) = 0.0", 0.0);
            var mat = scope["A"] as Double[,];

            Assert.IsNotNull(mat);
            Assert.AreEqual(1.0, mat[0, 0]);
            Assert.AreEqual(0.0, mat[0, 1]);
            Assert.AreEqual(3.0, mat[1, 0]);
            Assert.AreEqual(4.0, mat[1, 1]);
        }

        [Test]
        public void SetMatrixEntryWithValidValueAfterCreatingIt()
        {
            var scope = Test("A = [1,2;3,4]; A(1,1) = 0.0", 0.0);
            var mat = scope["A"] as Double[,];

            Assert.IsNotNull(mat);
            Assert.AreEqual(1.0, mat[0, 0]);
            Assert.AreEqual(2.0, mat[0, 1]);
            Assert.AreEqual(3.0, mat[1, 0]);
            Assert.AreEqual(0.0, mat[1, 1]);
        }

        [Test]
        public void SetMatrixEntryWithInvalidValueAfterCreatingIt()
        {
            var scope = Test("A = [1,2;3,4]; A(1,1) = \"hi there\"; A(0,0)", 1.0);
            var mat = scope["A"] as Double[,];

            Assert.IsNotNull(mat);
            Assert.AreEqual(1.0, mat[0, 0]);
            Assert.AreEqual(2.0, mat[0, 1]);
            Assert.AreEqual(3.0, mat[1, 0]);
            Assert.AreEqual(Double.NaN, mat[1, 1]);
        }

        [Test]
        public void SetMatrixEntryWithConvertedStringValueAfterCreatingIt()
        {
            var scope = Test("A = [1,2;3,4]; A(1,1) = \"23\"; A(1, 0)", 3.0);
            var mat = scope["A"] as Double[,];

            Assert.IsNotNull(mat);
            Assert.AreEqual(1.0, mat[0, 0]);
            Assert.AreEqual(2.0, mat[0, 1]);
            Assert.AreEqual(3.0, mat[1, 0]);
            Assert.AreEqual(23.0, mat[1, 1]);
        }

        [Test]
        public void SetObjectWithValueAfterCreatingIt()
        {
            var scope = Test("A = new {}; A(\"a\") = 5", 5.0);
            var obj = scope["A"] as IDictionary<String, Object>;

            Assert.IsNotNull(obj);
            Assert.AreEqual(5.0, obj["a"]);
        }

        [Test]
        public void ModifyObjectWithValueAfterCreatingIt()
        {
            var scope = Test("A = new { a: 0 }; A(\"a\") = \"test\"; A(\"b\") = 17.3", 17.3);
            var obj = scope["A"] as IDictionary<String, Object>;

            Assert.IsNotNull(obj);
            Assert.AreEqual("test", obj["a"]);
            Assert.AreEqual(17.3, obj["b"]);
        }

        [Test]
        public void SetObjectWithNumericIndexAfterCreatingIt()
        {
            var scope = Test("A = new { }; A(2) = 17.3", 17.3);
            var obj = scope["A"] as IDictionary<String, Object>;

            Assert.IsNotNull(obj);
            Assert.AreEqual(17.3, obj["2"]);
        }

        [Test]
        public void SetObjectWithBooleanIndexAfterCreatingIt()
        {
            var scope = Test("A = new { }; A(true) = 17.3", 17.3);
            var obj = scope["A"] as IDictionary<String, Object>;

            Assert.IsNotNull(obj);
            Assert.AreEqual(17.3, obj["true"]);
        }

        [Test]
        public void FunctionLocalVariableRemainsLocal()
        {
            var scope = Test("(() => { var x = 5; x + 9; })()", 14.0);

            Assert.AreEqual(0, scope.Count);
        }

        [Test]
        public void FunctionLocalVariableRemainsLocalAndDoesNotRequireTrailingSemicolon()
        {
            var scope = Test("((x, y) => { var z = 5; x + y + z })(2, 3)", 10.0);

            Assert.AreEqual(0, scope.Count);
        }

        [Test]
        public void FunctionGlobalAssignmentChangesScope()
        {
            var scope = Test("(x => { y = 5; x + y })(2)", 7.0);

            Assert.AreEqual(1, scope.Count);
            Assert.AreEqual(5.0, scope["y"]);
        }

        private IDictionary<String, Object> Test(String sourceCode, Double expected, Double tolerance = 0.0)
        {
            var engine = new Engine();
            var result = engine.Interpret(sourceCode) as Double?;

            Assert.IsTrue(result.HasValue);
            Assert.AreEqual(expected, result.Value, tolerance);
            return engine.Scope;
        }
    }
}
