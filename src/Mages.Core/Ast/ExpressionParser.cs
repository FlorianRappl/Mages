namespace Mages.Core.Ast
{
    using Mages.Core.Ast.Expressions;
    using Mages.Core.Tokens;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    class ExpressionParser
    {
        public static readonly Dictionary<String, Object> KeywordConstants = new Dictionary<String, Object>
        {
            { "true", true },
            { "false", false },
        };

        private readonly AbstractScopeStack _scopes;

        public ExpressionParser()
        {
            var root = new AbstractScope(null);
            _scopes = new AbstractScopeStack(root);
        }

        public IExpression ParseExpression(IEnumerator<IToken> tokens)
        {
            return ParseAssignment(tokens);
        }

        private List<IExpression> ParseExpressions(IEnumerator<IToken> tokens)
        {
            var list = new List<IExpression>();
            var token = tokens.Current;
            var allowNext = true;

            while (allowNext && token.Type != TokenType.End)
            {
                var result = ParseExpression(tokens);
                allowNext = false;
                list.Add(result);
                tokens.NextNonIgnorable();
                token = tokens.Current;

                if (token.Type == TokenType.Comma)
                {
                    allowNext = true;
                    tokens.NextNonIgnorable();
                    token = tokens.Current;
                }
            }

            return list;
        }

        private FunctionExpression ParseFunction(ParameterExpression parameters, IEnumerator<IToken> tokens)
        {
            _scopes.PushNew();
            var body = ParseExpression(tokens.NextNonIgnorable());
            var scope = _scopes.PopCurrent();
            return new FunctionExpression(scope, parameters, body);
        }

        private IExpression ParseAssignment(IEnumerator<IToken> tokens)
        {
            var x = ParseConditional(tokens);
            var token = tokens.Current;
            var mode = token.Type;

            if (mode == TokenType.Assignment)
            {
                var y = ParseExpression(tokens.NextNonIgnorable());
                return new AssignmentExpression(x, y);
            }
            else if (mode == TokenType.Lambda)
            {
                if (x is ArgumentsExpression)
                {
                    var args = ((ArgumentsExpression)x);
                    var parameters = new ParameterExpression(args.Expressions, args.Start, args.End);
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

        private IExpression ParseConditional(IEnumerator<IToken> tokens)
        {
            var x = ParseOr(tokens);
            var current = tokens.Current;
            var mode = current.Type;

            if (mode == TokenType.Condition)
            {
                var y = ParseOr(tokens.NextNonIgnorable());
                mode = tokens.Current.Type;

                if (mode == TokenType.Colon)
                {
                    var z = ParseOr(tokens.NextNonIgnorable());
                    return new ConditionalExpression(x, y, z);
                }

                //TODO Report(new ParseError(ErrorCode.BranchMissing, tokens.Current.Start));
            }
            else if (mode == TokenType.Colon)
            {
                var z = ParseOr(tokens.NextNonIgnorable());
                var y = z;
                mode = tokens.Current.Type;

                if (mode == TokenType.Colon)
                {
                    z = ParseOr(tokens.NextNonIgnorable());
                }
                else
                {
                    y = new EmptyExpression(z.Start);
                }

                return new RangeExpression(x, y, z);
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

            while (tokens.Current.IsOneOf(TokenType.Multiply, TokenType.Modulo, TokenType.LeftDivide, TokenType.RightDivide))
            {
                var mode = tokens.Current.Type;
                var y = ParsePower(tokens.NextNonIgnorable());
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
                tokens.NextNonIgnorable();
                var current = tokens.Current;
                var mode = current.Type;

                if (current.IsOneOf(TokenType.Increment, TokenType.Decrement, TokenType.Factorial, TokenType.Transpose))
                {
                    left = ExpressionCreators.PostUnary[mode].Invoke(left, current.End);
                }
                else if (mode == TokenType.Dot)
                {
                    tokens.NextNonIgnorable();
                    current = tokens.Current;

                    if (current.Type == TokenType.Identifier)
                    {
                        var identifier = new IdentifierExpression(current.Payload, current.Start, current.End);
                        left = new MemberExpression(left, identifier);
                    }
                    else
                    {
                        //TODO Report(new ParseError(ErrorCode.IdentifierExpected, current.Start));
                        return left;
                    }
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
            var arguments = ParseExpressions(tokens.NextNonIgnorable()).ToArray();
            var token = tokens.Current;
            var end = token.End;

            if (token.Type == TokenType.CloseGroup)
            {
                tokens.NextNonIgnorable();
            }
            else
            {
                //TODO Report(new ParseError(ErrorCode.BracketNotTerminated, token));
            }

            return new ArgumentsExpression(arguments, start, end);
        }

        private MatrixExpression ParseMatrix(IEnumerator<IToken> tokens)
        {
            var start = tokens.Current;
            var buffer = new List<List<IExpression>>();
            var current = tokens.Current;

            do
            {
                tokens.NextNonIgnorable();
                current = tokens.Current;

                //TODO

                if (current.Type == TokenType.Comma)
                {
                    tokens.NextNonIgnorable();
                    current = tokens.Current;
                }
            }
            while (current.IsNeither(TokenType.CloseList, TokenType.End));

            if (current.Type == TokenType.End)
            {
                //TODO ...
            }

            var end = current;
            var values = buffer.Select(m => m.ToArray()).ToArray();
            return new MatrixExpression(values, start.Start, end.End);
        }

        private ObjectExpression ParseObject(IEnumerator<IToken> tokens)
        {
            var start = tokens.Current;
            var values = new Dictionary<String, IExpression>();
            var current = tokens.Current;

            do 
            {
                tokens.NextNonIgnorable();
                current = tokens.Current;

                //TODO

                if (current.Type == TokenType.Comma)
                {
                    tokens.NextNonIgnorable();
                    current = tokens.Current;
                }
            }
            while (current.IsNeither(TokenType.CloseScope, TokenType.End));

            if (current.Type == TokenType.End)
            {
                //TODO ...
            }

            var end = current;
            return new ObjectExpression(values, start.Start, end.End);
        }

        private IExpression ParseAtomic(IEnumerator<IToken> tokens)
        {
            switch (tokens.Current.Type)
            {
                case TokenType.OpenList:
                    return ParseMatrix(tokens);
                case TokenType.OpenScope:
                    return ParseObject(tokens);
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
            }

            return new EmptyExpression(tokens.Current.Start);
        }

        private VariableExpression ParseVariable(IEnumerator<IToken> tokens)
        {
            var token = tokens.Current as IdentToken;
            var scope = _scopes.Current.Find(token.Payload);
            var expr = new VariableExpression(token.Payload, scope, token.Start, token.End);
            tokens.NextNonIgnorable();
            return expr;
        }

        private IExpression ParseKeywordConstant(IEnumerator<IToken> tokens)
        {
            var token = tokens.Current as IdentToken;
            var constant = default(Object);

            if (KeywordConstants.TryGetValue(token.Payload, out constant))
            {
                var expr = new ConstantExpression(constant, token.Start, token.End);
                tokens.NextNonIgnorable();
                return expr;
            }

            return new EmptyExpression(token.Start);
        }

        private ConstantExpression ParseString(IEnumerator<IToken> tokens)
        {
            var token = tokens.Current as StringToken;
            var expr = new ConstantExpression(token.Payload, token.Start, token.End);
            tokens.NextNonIgnorable();
            return expr;
        }

        private ConstantExpression ParseNumber(IEnumerator<IToken> tokens)
        {
            var token = tokens.Current as NumberToken;
            var expr = new ConstantExpression(token.Value, token.Start, token.End);
            tokens.NextNonIgnorable();
            return expr;
        }
    }
}
