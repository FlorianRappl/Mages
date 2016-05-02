namespace Mages.Core.Ast
{
    using Mages.Core.Ast.Expressions;
    using Mages.Core.Ast.Statements;
    using Mages.Core.Tokens;
    using System;
    using System.Collections.Generic;

    sealed class ExpressionParser : IParser
    {
        private readonly AbstractScopeStack _scopes;

        public ExpressionParser()
        {
            var root = new AbstractScope(null);
            _scopes = new AbstractScopeStack(root);
        }

        public IStatement ParseStatement(IEnumerator<IToken> tokens)
        {
            tokens.NextNonIgnorable();
            var current = tokens.Current;

            if (current.Is(Keywords.Var))
            {
                return ParseVarStatement(tokens);
            }
            else if (current.Type == TokenType.OpenScope)
            {
                return ParseBlockStatement(tokens);
            }
            else
            {
                return ParseSimpleStatement(tokens);
            }
        }

        public IExpression ParseExpression(IEnumerator<IToken> tokens)
        {
            return ParseAssignment(tokens.NextNonIgnorable());
        }

        private SimpleStatement ParseSimpleStatement(IEnumerator<IToken> tokens)
        {
            var expr = ParseAssignment(tokens);
            var end = tokens.Current.End;
            return new SimpleStatement(expr, end);
        }

        private IStatement ParseVarStatement(IEnumerator<IToken> tokens)
        {
            var start = tokens.Current.Start;
            var expr = ParseAssignment(tokens.NextNonIgnorable());
            var end = tokens.Current.End;
            var stmt = new VarStatement(expr, start, end);

            if (_scopes.Current.Parent != null)
            {
                var assignment = expr as AssignmentExpression;

                if (assignment != null)
                {
                    var variable = assignment.VariableName;

                    if (variable != null)
                    {
                        _scopes.Current.Provide(variable, assignment);
                    }
                }
            }

            return stmt;
        }

        private BlockStatement ParseBlockStatement(IEnumerator<IToken> tokens)
        {
            var start = tokens.Current.Start;
            var current = tokens.NextNonIgnorable().Current;
            var statements = new List<IStatement>();
            _scopes.PushNew();

            while (current.IsNeither(TokenType.CloseScope, TokenType.End))
            {
                var statement = ParseStatement(tokens);
                statements.Add(statement);
                current = tokens.Current;

                if (current.Type == TokenType.SemiColon)
                {
                    current = tokens.NextNonIgnorable().Current;
                }
            }

            var end = tokens.Current.End;
            var scope = _scopes.PopCurrent();
            return new BlockStatement(statements.ToArray(), start, end);
        }

        private List<IExpression> ParseExpressions(IEnumerator<IToken> tokens)
        {
            var expressions = new List<IExpression>();
            var token = tokens.Current;
            var allowNext = true;

            while (allowNext && token.Type != TokenType.End)
            {
                var expression = ParseAssignment(tokens);
                allowNext = false;
                expressions.Add(expression);
                token = tokens.Current;

                if (token.Type == TokenType.Comma)
                {
                    allowNext = true;
                    tokens.NextNonIgnorable();
                    token = tokens.Current;
                }
            }

            return expressions;
        }

        private FunctionExpression ParseFunction(ParameterExpression parameters, IEnumerator<IToken> tokens)
        {
            _scopes.PushNew();
            parameters.BindTo(_scopes.Current);
            var body = ParseAssignment(tokens);
            var scope = _scopes.PopCurrent();
            return new FunctionExpression(parameters, body);
        }

        private IExpression ParseAssignment(IEnumerator<IToken> tokens)
        {
            var x = ParseRange(tokens);
            var token = tokens.Current;
            var mode = token.Type;

            if (mode == TokenType.Assignment)
            {
                var y = ParseExpression(tokens);
                var expr = new AssignmentExpression(x, y);

                if (_scopes.Current.Parent == null)
                {
                    var variable = expr.VariableName;

                    if (variable != null)
                    {
                        _scopes.Current.Provide(variable, expr);
                    }
                }

                return expr;
            }
            else if (mode == TokenType.Lambda)
            {
                if (x is ArgumentsExpression)
                {
                    var args = ((ArgumentsExpression)x);
                    var parameters = new ParameterExpression(args.Arguments, args.Start, args.End);
                    return ParseFunction(parameters, tokens.NextNonIgnorable());
                }
                else
                {
                    var parameters = new ParameterExpression(new[] { x }, x.Start, x.End);
                    return ParseFunction(parameters, tokens.NextNonIgnorable());
                }
            }

            return x;
        }

        private IExpression ParseRange(IEnumerator<IToken> tokens)
        {
            var x = ParseConditional(tokens);

            if (tokens.Current.Type == TokenType.Colon)
            {
                var z = ParseConditional(tokens.NextNonIgnorable());
                var y = z;

                if (tokens.Current.Type == TokenType.Colon)
                {
                    z = ParseConditional(tokens.NextNonIgnorable());
                }
                else
                {
                    y = new EmptyExpression(z.Start);
                }

                return new RangeExpression(x, y, z);
            }

            return x;
        }

        private IExpression ParseConditional(IEnumerator<IToken> tokens)
        {
            var x = ParseOr(tokens);

            if (tokens.Current.Type == TokenType.Condition)
            {
                var y = ParseConditional(tokens.NextNonIgnorable());
                var z = default(IExpression);

                if (tokens.Current.Type == TokenType.Colon)
                {
                    z = ParseConditional(tokens.NextNonIgnorable());
                }
                else
                {
                    z = ParseInvalid(ErrorCode.BranchMissing, tokens);
                }

                return new ConditionalExpression(x, y, z);
            }

            return x;
        }

        private IExpression ParseOr(IEnumerator<IToken> tokens)
        {
            var x = ParseAnd(tokens);

            while (tokens.Current.Type == TokenType.Or)
            {
                var y = ParseAnd(tokens.NextNonIgnorable());
                x = new BinaryExpression.Or(x, y);
            }

            return x;
        }

        private IExpression ParseAnd(IEnumerator<IToken> tokens)
        {
            var x = ParseEquality(tokens);

            while (tokens.Current.Type == TokenType.And)
            {
                var y = ParseEquality(tokens.NextNonIgnorable());
                x = new BinaryExpression.And(x, y);
            }

            return x;
        }

        private IExpression ParseEquality(IEnumerator<IToken> tokens)
        {
            var x = ParseRelational(tokens);

            while (tokens.Current.IsEither(TokenType.Equal, TokenType.NotEqual))
            {
                var mode = tokens.Current.Type;
                var y = ParseRelational(tokens.NextNonIgnorable());
                x = ExpressionCreators.Binary[mode].Invoke(x, y);
            }

            return x;
        }

        private IExpression ParseRelational(IEnumerator<IToken> tokens)
        {
            var x = ParseAdditive(tokens);

            while (tokens.Current.IsOneOf(TokenType.Greater, TokenType.GreaterEqual, TokenType.LessEqual, TokenType.Less))
            {
                var mode = tokens.Current.Type;
                var y = ParseAdditive(tokens.NextNonIgnorable());
                x = ExpressionCreators.Binary[mode].Invoke(x, y);
            }

            return x;
        }

        private IExpression ParseAdditive(IEnumerator<IToken> tokens)
        {
            var x = ParseMultiplicative(tokens);

            while (tokens.Current.IsEither(TokenType.Add, TokenType.Subtract))
            {
                var mode = tokens.Current.Type;
                var y = ParseMultiplicative(tokens.NextNonIgnorable());
                x = ExpressionCreators.Binary[mode].Invoke(x, y);
            }

            return x;
        }

        private IExpression ParseMultiplicative(IEnumerator<IToken> tokens)
        {
            var x = ParsePower(tokens);

            while (tokens.Current.IsOneOf(TokenType.Multiply, TokenType.Modulo, TokenType.LeftDivide, TokenType.RightDivide, TokenType.Number, TokenType.Identifier))
            {
                var implicitMultiply = tokens.Current.IsEither(TokenType.Number, TokenType.Identifier);
                var mode = implicitMultiply ? TokenType.Multiply : tokens.Current.Type;
                var y = ParsePower(implicitMultiply ? tokens : tokens.NextNonIgnorable());
                x = ExpressionCreators.Binary[mode].Invoke(x, y);
            }

            return x;
        }

        private IExpression ParsePower(IEnumerator<IToken> tokens)
        {
            var atom = ParseUnary(tokens);

            if (tokens.Current.Type == TokenType.Power)
            {
                var expressions = new Stack<IExpression>();
                expressions.Push(atom);

                do
                {
                    var x = ParseUnary(tokens.NextNonIgnorable());
                    expressions.Push(x);
                }
                while (tokens.Current.Type == TokenType.Power);

                do
                {
                    var right = expressions.Pop();
                    var left = expressions.Pop();
                    var x = new BinaryExpression.Power(left, right);
                    expressions.Push(x);
                }
                while (expressions.Count > 1);

                atom = expressions.Pop();
            }

            return atom;
        }

        private IExpression ParseUnary(IEnumerator<IToken> tokens)
        {
            var current = tokens.Current;
            var mode = current.Type;

            switch (mode)
            {
                case TokenType.Subtract:
                case TokenType.Add:
                case TokenType.Negate:
                case TokenType.Decrement:
                case TokenType.Increment:
                    var expr = ParseUnary(tokens.NextNonIgnorable());
                    return ExpressionCreators.PreUnary[mode].Invoke(current.Start, expr);
            }

            return ParsePrimary(tokens);
        }

        private IExpression ParsePrimary(IEnumerator<IToken> tokens)
        {
            var left = ParseAtomic(tokens);

            do
            {
                var current = tokens.Current;
                var mode = current.Type;

                if (current.IsOneOf(TokenType.Increment, TokenType.Decrement, TokenType.Factorial, TokenType.Transpose))
                {
                    left = ExpressionCreators.PostUnary[mode].Invoke(left, current.End);
                    tokens.NextNonIgnorable();
                }
                else if (mode == TokenType.Dot)
                {
                    var identifier = ParseIdentifier(tokens.NextNonIgnorable());
                    left = new MemberExpression(left, identifier);
                }
                else if (mode == TokenType.OpenGroup)
                {
                    var arguments = ParseArguments(tokens);
                    left = new CallExpression(left, arguments);
                }
                else
                {
                    return left;
                }
            }
            while (true);
        }

        private ArgumentsExpression ParseArguments(IEnumerator<IToken> tokens)
        {
            var start = tokens.Current.Start;
            var token = tokens.NextNonIgnorable().Current;
            var arguments = default(List<IExpression>);

            if (token.Type != TokenType.CloseGroup)
            {
                arguments = ParseExpressions(tokens);
                token = tokens.Current;
            }
            else
            {
                arguments = new List<IExpression>();
            }

            var end = token.End;

            if (token.Type == TokenType.CloseGroup)
            {
                tokens.NextNonIgnorable();
            }
            else
            {
                var expr = ParseInvalid(ErrorCode.BracketNotTerminated, tokens);
                arguments.Add(expr);
            }

            return new ArgumentsExpression(arguments.ToArray(), start, end);
        }

        private MatrixExpression ParseMatrix(IEnumerator<IToken> tokens)
        {
            var start = tokens.Current;
            var values = ParseRows(tokens.NextNonIgnorable());
            var end = tokens.Current;

            if (end.Type == TokenType.CloseList)
            {
                tokens.NextNonIgnorable();
            }
            else
            {
                var error = ParseInvalid(ErrorCode.MatrixNotTerminated, tokens);
                values.Add(new IExpression[] { error });
            }

            return new MatrixExpression(values.ToArray(), start.Start, end.End);
        }

        private List<IExpression> ParseColumns(IEnumerator<IToken> tokens)
        {
            var expressions = new List<IExpression>();

            do
            {
                var expression = ParseAssignment(tokens);
                expressions.Add(expression);
            } 
            while (tokens.Current.Type == TokenType.Comma && tokens.NextNonIgnorable().Current.Type != TokenType.End);

            return expressions;
        }

        private List<IExpression[]> ParseRows(IEnumerator<IToken> tokens)
        {
            var expressions = new List<IExpression[]>();

            if (tokens.Current.IsNeither(TokenType.CloseList, TokenType.End))
            {
                var columns = ParseColumns(tokens);
                expressions.Add(columns.ToArray());

                while (tokens.Current.Type == TokenType.SemiColon)
                {
                    if (tokens.NextNonIgnorable().Current.IsNeither(TokenType.CloseList, TokenType.End))
                    {
                        columns = ParseColumns(tokens);
                        expressions.Add(columns.ToArray());
                    }
                }
            }

            return expressions;
        }

        private IExpression ParseObject(IEnumerator<IToken> tokens)
        {
            var start = tokens.Current;
            var current = tokens.NextNonIgnorable().Current;

            if (current.Type == TokenType.OpenScope)
            {
                var values = ParseProperties(tokens.NextNonIgnorable());
                var end = tokens.Current;

                if (end.Type == TokenType.CloseScope)
                {
                    tokens.NextNonIgnorable();
                }
                else
                {
                    var error = ParseInvalid(ErrorCode.ObjectNotTerminated, tokens);
                    values.Add(error);
                }

                return new ObjectExpression(values.ToArray(), start.Start, end.End);
            }

            return ParseInvalid(ErrorCode.BracketNotTerminated, tokens);
        }

        private PropertyExpression ParseProperty(IEnumerator<IToken> tokens)
        {
            var identifier = ParseIdentifier(tokens);
            var value = default(IExpression);

            if (tokens.Current.Type != TokenType.Colon)
            {
                value = ParseInvalid(ErrorCode.ColonExpected, tokens);
            }
            else
            {
                value = ParseExpression(tokens);
            }

            return new PropertyExpression(identifier, value);
        }

        private List<IExpression> ParseProperties(IEnumerator<IToken> tokens)
        {
            var expressions = new List<IExpression>();

            if (tokens.Current.IsNeither(TokenType.CloseScope, TokenType.End))
            {
                var expression = ParseProperty(tokens);
                expressions.Add(expression);

                while (tokens.Current.Type == TokenType.Comma)
                {
                    if (tokens.NextNonIgnorable().Current.IsNeither(TokenType.CloseScope, TokenType.End))
                    {
                        expression = ParseProperty(tokens);
                        expressions.Add(expression);
                    }
                }
            }

            return expressions;
        }

        private IExpression ParseAtomic(IEnumerator<IToken> tokens)
        {
            switch (tokens.Current.Type)
            {
                case TokenType.OpenList:
                    return ParseMatrix(tokens);
                case TokenType.OpenGroup:
                    return ParseArguments(tokens);
                case TokenType.Keyword:
                    return ParseKeywordConstant(tokens);
                case TokenType.Identifier:
                    return ParseVariable(tokens);
                case TokenType.Number:
                    return ParseNumber(tokens);
                case TokenType.Text:
                    return ParseString(tokens);
                case TokenType.SemiColon:
                case TokenType.End:
                    return new EmptyExpression(tokens.Current.Start);
            }

            return ParseInvalid(ErrorCode.InvalidSymbol, tokens);
        }

        private InvalidExpression ParseInvalid(ErrorCode code, IEnumerator<IToken> tokens)
        {
            var expr = new InvalidExpression(code, tokens.Current);
            tokens.NextNonIgnorable();
            return expr;
        }

        private IExpression ParseVariable(IEnumerator<IToken> tokens)
        {
            var token = tokens.Current;

            if (token.Type == TokenType.Identifier)
            {
                var scope = _scopes.Current.Find(token.Payload);
                var expr = new VariableExpression(token.Payload, scope, token.Start, token.End);
                tokens.NextNonIgnorable();
                return expr;
            }

            return ParseInvalid(ErrorCode.IdentifierExpected, tokens);
        }

        private IExpression ParseIdentifier(IEnumerator<IToken> tokens)
        {
            var token = tokens.Current;

            if (token.Type == TokenType.Identifier)
            {
                var expr = new IdentifierExpression(token.Payload, token.Start, token.End);
                tokens.NextNonIgnorable();
                return expr;
            }

            return ParseInvalid(ErrorCode.IdentifierExpected, tokens);
        }

        private IExpression ParseKeywordConstant(IEnumerator<IToken> tokens)
        {
            var token = tokens.Current;

            if (token.Payload.Equals(Keywords.New, StringComparison.Ordinal))
            {
                return ParseObject(tokens);
            }
            else
            {
                var constant = default(Object);

                if (Keywords.TryGetConstant(token.Payload, out constant))
                {
                    var expr = ConstantExpression.From(constant, token);
                    tokens.NextNonIgnorable();
                    return expr;
                }
            }

            return ParseInvalid(ErrorCode.KeywordUnexpected, tokens);
        }

        private ConstantExpression ParseString(IEnumerator<IToken> tokens)
        {
            var token = (StringToken)tokens.Current;
            var expr = new ConstantExpression.Text(token.Payload, token, token.Errors);
            tokens.NextNonIgnorable();
            return expr;
        }

        private ConstantExpression ParseNumber(IEnumerator<IToken> tokens)
        {
            var token = (NumberToken)tokens.Current;
            var expr = new ConstantExpression.Number(token.Value, token, token.Errors);
            tokens.NextNonIgnorable();
            return expr;
        }
    }
}
