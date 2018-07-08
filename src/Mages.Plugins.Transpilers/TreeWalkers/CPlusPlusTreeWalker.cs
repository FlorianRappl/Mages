namespace Mages.Plugins.Transpilers.TreeWalkers
{
    using System;

    public class CPlusPlusTreeWalker : TranspilerTreeWalker
    {
        protected override void InsertAdd(Action left, Action right)
        {
            throw new NotImplementedException();
        }

        protected override void InsertAnd(Action left, Action right)
        {
            throw new NotImplementedException();
        }

        protected override void InsertArguments(Int32 length, Action<Int32> argument)
        {
            throw new NotImplementedException();
        }

        protected override void InsertAssignment(Action slot, Action value)
        {
            throw new NotImplementedException();
        }

        protected override void InsertAwait(Action payload)
        {
            throw new NotImplementedException();
        }

        protected override void InsertBlock(Action body)
        {
            throw new NotImplementedException();
        }

        protected override void InsertBreak()
        {
            throw new NotImplementedException();
        }

        protected override void InsertCall(Boolean isAssigned, Action function, Action arguments)
        {
            throw new NotImplementedException();
        }

        protected override void InsertCase(Action condition, Action body)
        {
            throw new NotImplementedException();
        }

        protected override void InsertCondition(Action condition, Action primary, Action secondary)
        {
            throw new NotImplementedException();
        }

        protected override void InsertConstant(Object value)
        {
            throw new NotImplementedException();
        }

        protected override void InsertContinue()
        {
            throw new NotImplementedException();
        }

        protected override void InsertDefVariable(String name)
        {
            throw new NotImplementedException();
        }

        protected override void InsertDelMember(Action obj, String name)
        {
            throw new NotImplementedException();
        }

        protected override void InsertDelVariable(String name)
        {
            throw new NotImplementedException();
        }

        protected override void InsertDiv(Action left, Action right)
        {
            throw new NotImplementedException();
        }

        protected override void InsertEq(Action left, Action right)
        {
            throw new NotImplementedException();
        }

        protected override void InsertFactorial(Action expression)
        {
            throw new NotImplementedException();
        }

        protected override void InsertFor(Action init, Action condition, Action post, Action body)
        {
            throw new NotImplementedException();
        }

        protected override void InsertFunction(Boolean isMethod, Action parameters, Action body)
        {
            throw new NotImplementedException();
        }

        protected override void InsertGeq(Action left, Action right)
        {
            throw new NotImplementedException();
        }

        protected override void InsertGetMember(Action obj, Action property)
        {
            throw new NotImplementedException();
        }

        protected override void InsertGetVariable(String name)
        {
            throw new NotImplementedException();
        }

        protected override void InsertGt(Action left, Action right)
        {
            throw new NotImplementedException();
        }

        protected override void InsertIdentifier(String name)
        {
            throw new NotImplementedException();
        }

        protected override void InsertIf(Action condition, Action primary, Action secondary)
        {
            throw new NotImplementedException();
        }

        protected override void InsertMod(Action left, Action right)
        {
            throw new NotImplementedException();
        }

        protected override void InsertPow(Action left, Action right)
        {
            throw new NotImplementedException();
        }

        protected override void InsertInterpolatedString(String formattedString, Int32 length, Action<Int32> replacement)
        {
            throw new NotImplementedException();
        }

        protected override void InsertLeq(Action left, Action right)
        {
            throw new NotImplementedException();
        }

        protected override void InsertLookupType(Action expression)
        {
            throw new NotImplementedException();
        }

        protected override void InsertLt(Action left, Action right)
        {
            throw new NotImplementedException();
        }

        protected override void InsertMatch(Action reference, Action cases)
        {
            throw new NotImplementedException();
        }

        protected override void InsertMatrix(Int32 cols, Int32 rows, Action<Int32, Int32> cell)
        {
            throw new NotImplementedException();
        }

        protected override void InsertMul(Action left, Action right)
        {
            throw new NotImplementedException();
        }

        protected override void InsertNegative(Action expression)
        {
            throw new NotImplementedException();
        }

        protected override void InsertNeq(Action left, Action right)
        {
            throw new NotImplementedException();
        }

        protected override void InsertNot(Action expression)
        {
            throw new NotImplementedException();
        }

        protected override void InsertObject(Int32 length, Action<Int32> property)
        {
            throw new NotImplementedException();
        }

        protected override void InsertOptionalParameter(String name, Action defaultValue)
        {
            throw new NotImplementedException();
        }

        protected override void InsertOr(Action left, Action right)
        {
            throw new NotImplementedException();
        }

        protected override void InsertParameters(Int32 length, Action<Int32> parameter)
        {
            throw new NotImplementedException();
        }

        protected override void InsertPipe(Action left, Action right)
        {
            throw new NotImplementedException();
        }

        protected override void InsertPositive(Action expression)
        {
            throw new NotImplementedException();
        }

        protected override void InsertPostDecrement(Action expression)
        {
            throw new NotImplementedException();
        }

        protected override void InsertPostIncrement(Action expression)
        {
            throw new NotImplementedException();
        }

        protected override void InsertPreDecrement(Action expression)
        {
            throw new NotImplementedException();
        }

        protected override void InsertPreIncrement(Action expression)
        {
            throw new NotImplementedException();
        }

        protected override void InsertProperty(Action name, Action value)
        {
            throw new NotImplementedException();
        }

        protected override void InsertRange(Action from, Action to, Action step)
        {
            throw new NotImplementedException();
        }

        protected override void InsertRequiredParameters(String name)
        {
            throw new NotImplementedException();
        }

        protected override void InsertReturn(Action payload)
        {
            throw new NotImplementedException();
        }

        protected override void InsertReverseDiv(Action left, Action right)
        {
            throw new NotImplementedException();
        }

        protected override void InsertSetMember(Action obj, Action property)
        {
            throw new NotImplementedException();
        }

        protected override void InsertSetVariable(String name)
        {
            throw new NotImplementedException();
        }

        protected override void InsertStatement(Action body)
        {
            throw new NotImplementedException();
        }

        protected override void InsertSub(Action left, Action right)
        {
            throw new NotImplementedException();
        }

        protected override void InsertTranspose(Action expression)
        {
            throw new NotImplementedException();
        }

        protected override void InsertWhile(Action condition, Action body)
        {
            throw new NotImplementedException();
        }

        protected override String Stringify()
        {
            throw new NotImplementedException();
        }
    }
}
