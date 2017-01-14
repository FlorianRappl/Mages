namespace Mages.Plugins.Transpilers.TreeWalkers
{
    using Mages.Core;
    using Mages.Core.Ast;
    using Mages.Core.Ast.Expressions;
    using Mages.Core.Ast.Statements;
    using System;
    using System.Collections.Generic;

    public abstract class TranspilerTreeWalker : ITreeWalker, IValidationContext
    {
        #region Operator Mappings

        private static readonly Dictionary<String, Action<TranspilerTreeWalker, Action>> PreUnaryOperatorMapping = new Dictionary<String, Action<TranspilerTreeWalker, Action>>
        {
            { "~", (walker, expr) => walker.InsertNot(expr) },
            { "+", (walker, expr) => walker.InsertPositive(expr) },
            { "-", (walker, expr) => walker.InsertNegative(expr) },
            { "&", (walker, expr) => walker.InsertLookupType(expr) },
            { "++", (walker, expr) => walker.InsertPreIncrement(expr) },
            { "--", (walker, expr) => walker.InsertPreDecrement(expr) }
        };

        private static readonly Dictionary<String, Action<TranspilerTreeWalker, Action>> PostUnaryOperatorMapping = new Dictionary<String, Action<TranspilerTreeWalker, Action>>
        {
            { "!", (walker, expr) => walker.InsertFactorial(expr) },
            { "'", (walker, expr) => walker.InsertTranspose(expr) },
            { "++", (walker, expr) => walker.InsertPostIncrement(expr) },
            { "--", (walker, expr) => walker.InsertPostDecrement(expr) }
        };

        private static readonly Dictionary<String, Action<TranspilerTreeWalker, Action, Action>> BinaryOperatorMapping = new Dictionary<String, Action<TranspilerTreeWalker, Action, Action>>
        {
            { "&&", (walker, left, right) => walker.InsertAnd(left, right) },
            { "||", (walker, left, right) => walker.InsertOr(left, right) },
            { "==", (walker, left, right) => walker.InsertEq(left, right) },
            { "~=", (walker, left, right) => walker.InsertNeq(left, right) },
            { ">", (walker, left, right) => walker.InsertGt(left, right) },
            { "<", (walker, left, right) => walker.InsertLt(left, right) },
            { ">=", (walker, left, right) => walker.InsertGeq(left, right) },
            { "<=", (walker, left, right) => walker.InsertLeq(left, right) },
            { "+", (walker, left, right) => walker.InsertAdd(left, right) },
            { "-", (walker, left, right) => walker.InsertSub(left, right) },
            { "*", (walker, left, right) => walker.InsertMul(left, right) },
            { "/", (walker, left, right) => walker.InsertDiv(left, right) },
            { "\\", (walker, left, right) => walker.InsertReverseDiv(left, right) },
            { "^", (walker, left, right) => walker.InsertPow(left, right) },
            { "%", (walker, left, right) => walker.InsertMod(left, right) },
            { "|", (walker, left, right) => walker.InsertPipe(left, right) }
        };

        #endregion

        #region Fields

        private readonly Stack<Boolean> _loops;
        private Boolean _assigning;
        private Boolean _declaring;
        private Boolean _member;

        #endregion

        #region ctor

        public TranspilerTreeWalker()
        {
            _loops = new Stack<Boolean>();
        }

        #endregion

        #region Properties

        public Boolean IsInLoop
        {
            get { return _loops.Count > 0 && _loops.Peek(); }
        }

        public String Result
        {
            get { return Stringify(); }
        }

        #endregion

        #region Methods

        public String Transform(IEnumerable<IStatement> statements)
        {
            foreach (var statement in statements)
            {
                statement.Accept(this);
            }

            return Result;
        }

        #endregion

        #region Visitors

        void ITreeWalker.Visit(BlockStatement block)
        {
            block.Validate(this);

            InsertBlock(() =>
            {
                foreach (var statement in block.Statements)
                {
                    statement.Accept(this);
                }
            });
        }

        void ITreeWalker.Visit(SimpleStatement statement)
        {
            statement.Validate(this);
            InsertStatement(() => statement.Expression.Accept(this));
        }

        void ITreeWalker.Visit(VarStatement statement)
        {
            _declaring = true;
            statement.Validate(this);
            statement.Assignment.Accept(this);
            _declaring = false;
        }

        void ITreeWalker.Visit(ReturnStatement statement)
        {
            statement.Validate(this);
            InsertReturn(() => statement.Expression.Accept(this));
        }

        void ITreeWalker.Visit(DeleteExpression expression)
        {
            expression.Validate(this);
            var member = expression.Payload as MemberExpression;

            if (member != null)
            {
                var variable = member.Member as IdentifierExpression;
                member.Validate(this);

                if (variable != null)
                {
                    variable.Validate(this);
                    InsertDelMember(() => member.Object.Accept(this), variable.Name);
                }
            }
            else
            {
                var variable = expression.Payload as VariableExpression;

                if (variable != null)
                {
                    variable.Validate(this);
                    InsertDelVariable(variable.Name);
                }
            }
        }

        void ITreeWalker.Visit(WhileStatement statement)
        {
            statement.Validate(this);

            InsertWhile(() => statement.Condition.Accept(this), () =>
            {
                _loops.Push(true);
                statement.Body.Accept(this);
                _loops.Pop();
            });
        }

        void ITreeWalker.Visit(ForStatement statement)
        {
            statement.Validate(this);

            InsertFor(() =>
            {
                _declaring = statement.IsDeclared;
                statement.Initialization.Accept(this);
                _declaring = false;
            }, () => statement.Condition.Accept(this), () => statement.AfterThought.Accept(this), () =>
            {
                _loops.Push(true);
                statement.Body.Accept(this);
                _loops.Pop();
            });
        }

        void ITreeWalker.Visit(IfStatement statement)
        {
            statement.Validate(this);
            InsertIf(() => statement.Condition.Accept(this), () => statement.Primary.Accept(this), () => statement.Secondary.Accept(this));
        }

        void ITreeWalker.Visit(MatchStatement statement)
        {
            statement.Validate(this);
            InsertMatch(() => statement.Reference.Accept(this), () =>
            {
                _loops.Push(true);
                statement.Cases.Accept(this);
                _loops.Pop();
            });
        }

        void ITreeWalker.Visit(CaseStatement statement)
        {
            statement.Validate(this);
            InsertCase(() => statement.Condition.Accept(this), () => statement.Body.Accept(this));
        }

        void ITreeWalker.Visit(BreakStatement statement)
        {
            statement.Validate(this);
            InsertBreak();
        }

        void ITreeWalker.Visit(ContinueStatement statement)
        {
            statement.Validate(this);
            InsertContinue();
        }

        void ITreeWalker.Visit(EmptyExpression expression)
        {
            expression.Validate(this);
        }

        void ITreeWalker.Visit(ConstantExpression expression)
        {
            expression.Validate(this);
            InsertConstant(expression.Value);
        }

        void ITreeWalker.Visit(AwaitExpression expression)
        {
            expression.Validate(this);
            InsertAwait(() => expression.Payload.Accept(this));
        }

        void ITreeWalker.Visit(ArgumentsExpression expression)
        {
            var arguments = expression.Arguments;
            expression.Validate(this);
            InsertArguments(arguments.Length, i => arguments[i].Accept(this));
        }

        void ITreeWalker.Visit(AssignmentExpression expression)
        {
            expression.Validate(this);
            InsertAssignment(() =>
            {
                _assigning = true;
                expression.Variable.Accept(this);
                _assigning = false;
            }, () => expression.Value.Accept(this));
        }

        void ITreeWalker.Visit(BinaryExpression expression)
        {
            var action = default(Action<TranspilerTreeWalker, Action, Action>);
            expression.Validate(this);
            BinaryOperatorMapping.TryGetValue(expression.Operator, out action);
            action.Invoke(this, () => expression.LValue.Accept(this), () => expression.RValue.Accept(this));
        }

        void ITreeWalker.Visit(PreUnaryExpression expression)
        {
            var action = default(Action<TranspilerTreeWalker, Action>);
            expression.Validate(this);
            PreUnaryOperatorMapping.TryGetValue(expression.Operator, out action);
            action.Invoke(this, () => expression.Value.Accept(this));
        }

        void ITreeWalker.Visit(PostUnaryExpression expression)
        {
            var action = default(Action<TranspilerTreeWalker, Action>);
            expression.Validate(this);
            PostUnaryOperatorMapping.TryGetValue(expression.Operator, out action);
            action.Invoke(this, () => expression.Value.Accept(this));
        }

        void ITreeWalker.Visit(RangeExpression expression)
        {
            expression.Validate(this);
            InsertRange(() => expression.From.Accept(this), () => expression.To.Accept(this), () => expression.Step.Accept(this));
        }

        void ITreeWalker.Visit(ConditionalExpression expression)
        {
            expression.Validate(this);
            InsertCondition(() => expression.Condition.Accept(this), () => expression.Primary.Accept(this), () => expression.Secondary.Accept(this));
        }

        void ITreeWalker.Visit(CallExpression expression)
        {
            var assigning = _assigning;
            _assigning = false;
            expression.Validate(this);
            InsertCall(assigning, () => expression.Function.Accept(this), () => expression.Arguments.Accept(this));
            _assigning = assigning;
        }

        void ITreeWalker.Visit(ObjectExpression expression)
        {
            expression.Validate(this);
            var properties = expression.Values;
            InsertObject(properties.Length, i => properties[i].Accept(this));
        }

        void ITreeWalker.Visit(PropertyExpression expression)
        {
            expression.Validate(this);
            InsertProperty(() => expression.Name.Accept(this), () =>
            {
                _member = true;
                expression.Value.Accept(this);
                _member = false;
            });
        }

        void ITreeWalker.Visit(MatrixExpression expression)
        {
            var values = expression.Values;
            var rows = values.Length;
            var cols = rows > 0 ? values[0].Length : 0;
            expression.Validate(this);
            InsertMatrix(cols, rows, (i, j) => values[j][i].Accept(this));
        }

        void ITreeWalker.Visit(FunctionExpression expression)
        {
            var member = _member;
            _member = false;
            expression.Validate(this);

            InsertFunction(member, () => expression.Parameters.Accept(this), () =>
            {
                _loops.Push(false);
                expression.Body.Accept(this);
                _loops.Pop();
            });
        }

        void ITreeWalker.Visit(InvalidExpression expression)
        {
            expression.Validate(this);
        }

        void ITreeWalker.Visit(IdentifierExpression expression)
        {
            var name = expression.Name;
            expression.Validate(this);
            InsertIdentifier(name);
        }

        void ITreeWalker.Visit(MemberExpression expression)
        {
            var assigning = _assigning;
            _assigning = false;
            expression.Validate(this);

            if (assigning)
            {
                InsertSetMember(() => expression.Object.Accept(this), () => expression.Member.Accept(this));
            }
            else
            {
                InsertGetMember(() => expression.Object.Accept(this), () => expression.Member.Accept(this));
            }

            _assigning = assigning;
        }

        void ITreeWalker.Visit(ParameterExpression expression)
        {
            var expressions = expression.Parameters;
            expression.Validate(this);

            InsertParameters(expressions.Length, i =>
            {
                var identifier = expressions[i] as VariableExpression;

                if (identifier == null)
                {
                    var assignment = expressions[i] as AssignmentExpression;
                    identifier = (VariableExpression)assignment.Variable;
                    InsertOptionalParameter(identifier.Name, () => assignment.Value.Accept(this));
                }
                else
                {
                    InsertRequiredParameters(identifier.Name);
                }
            });
        }

        void ITreeWalker.Visit(VariableExpression expression)
        {
            var name = expression.Name;
            expression.Validate(this);

            if (_assigning)
            {
                if (_declaring)
                {
                    InsertDefVariable(name);
                }
                else
                {
                    InsertSetVariable(name);
                }
            }
            else
            {
                InsertGetVariable(name);
            }
        }

        void ITreeWalker.Visit(InterpolatedExpression expression)
        {
            var replacements = expression.Replacements;
            expression.Validate(this);
            InsertInterpolatedString(expression.Format.Value as String, replacements.Length, i => replacements[i].Accept(this));
        }

        #endregion

        #region Error Reporting

        void IValidationContext.Report(ParseError error)
        {
            throw new ParseException(error);
        }

        #endregion

        #region Helpers

        protected abstract String Stringify();

        protected abstract void InsertReturn(Action payload);

        protected abstract void InsertBlock(Action body);

        protected abstract void InsertStatement(Action body);

        protected abstract void InsertWhile(Action condition, Action body);

        protected abstract void InsertFor(Action init, Action condition, Action post, Action body);

        protected abstract void InsertIf(Action condition, Action primary, Action secondary);

        protected abstract void InsertAssignment(Action slot, Action value);

        protected abstract void InsertArguments(Int32 length, Action<Int32> argument);

        protected abstract void InsertFunction(Boolean isMethod, Action parameters, Action body);

        protected abstract void InsertParameters(Int32 length, Action<Int32> parameter);

        protected abstract void InsertObject(Int32 length, Action<Int32> property);

        protected abstract void InsertProperty(Action name, Action value);

        protected abstract void InsertMatrix(Int32 cols, Int32 rows, Action<Int32, Int32> cell);

        protected abstract void InsertInterpolatedString(String formattedString, Int32 length, Action<Int32> replacement);

        protected abstract void InsertOptionalParameter(String name, Action defaultValue);

        protected abstract void InsertRequiredParameters(String name);

        protected abstract void InsertMatch(Action reference, Action cases);

        protected abstract void InsertSetMember(Action obj, Action property);

        protected abstract void InsertGetMember(Action obj, Action property);

        protected abstract void InsertDelMember(Action obj, String name);

        protected abstract void InsertDelVariable(String name);

        protected abstract void InsertGetVariable(String name);

        protected abstract void InsertSetVariable(String name);

        protected abstract void InsertDefVariable(String name);

        protected abstract void InsertIdentifier(String name);

        protected abstract void InsertRange(Action from, Action to, Action step);

        protected abstract void InsertCondition(Action condition, Action primary, Action secondary);

        protected abstract void InsertCase(Action condition, Action body);

        protected abstract void InsertBreak();

        protected abstract void InsertContinue();

        protected abstract void InsertConstant(Object value);

        protected abstract void InsertAwait(Action payload);

        protected abstract void InsertCall(Boolean isAssigned, Action function, Action arguments);

        protected abstract void InsertDiv(Action left, Action right);

        protected abstract void InsertReverseDiv(Action left, Action right);

        protected abstract void InsertMul(Action left, Action right);

        protected abstract void InsertSub(Action left, Action right);

        protected abstract void InsertAdd(Action left, Action right);

        protected abstract void InsertLeq(Action left, Action right);

        protected abstract void InsertGeq(Action left, Action right);

        protected abstract void InsertLt(Action left, Action right);

        protected abstract void InsertGt(Action left, Action right);

        protected abstract void InsertNeq(Action left, Action right);

        protected abstract void InsertEq(Action left, Action right);

        protected abstract void InsertOr(Action left, Action right);

        protected abstract void InsertAnd(Action left, Action right);

        protected abstract void InsertPow(Action left, Action right);

        protected abstract void InsertMod(Action left, Action right);

        protected abstract void InsertPipe(Action left, Action right);

        protected abstract void InsertNot(Action expression);

        protected abstract void InsertPositive(Action expression);

        protected abstract void InsertNegative(Action expression);

        protected abstract void InsertLookupType(Action expression);

        protected abstract void InsertPreDecrement(Action expression);

        protected abstract void InsertPreIncrement(Action expression);

        protected abstract void InsertPostDecrement(Action expression);

        protected abstract void InsertPostIncrement(Action expression);

        protected abstract void InsertTranspose(Action expression);

        protected abstract void InsertFactorial(Action expression);

        #endregion
    }
}
