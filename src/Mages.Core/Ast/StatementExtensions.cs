namespace Mages.Core.Ast
{
    using Mages.Core.Ast.Expressions;
    using Mages.Core.Ast.Statements;
    using Mages.Core.Ast.Walkers;
    using Mages.Core.Vm;
    using System.Collections.Generic;

    public static class StatementExtensions
    {
        public static List<VariableExpression> FindMissingSymbols(this IStatement statement)
        {
            var missingSymbols = new List<VariableExpression>();
            statement.CollectMissingSymbols(missingSymbols);
            return missingSymbols;
        }

        public static BlockStatement ToBlock(this IEnumerable<IStatement> statements)
        {
            var list = new List<IStatement>(statements);
            var start = list.Count > 0 ? list[0].Start : new TextPosition();
            var end = list.Count > 0 ? list[list.Count - 1].End : start;
            return new BlockStatement(list.ToArray(), start, end);
        }

        public static void CollectMissingSymbols(this IStatement statement, List<VariableExpression> missingSymbols)
        {
            var symbols = new List<VariableExpression>();
            var walker = new SymbolTreeWalker(symbols);
            statement.Accept(walker);

            foreach (var symbol in symbols)
            {
                if (symbol.Scope == null)
                {
                    missingSymbols.Add(symbol);
                }
            }
        }

        public static List<VariableExpression> FindMissingSymbols(this IEnumerable<IStatement> statements)
        {
            var missingSymbols = new List<VariableExpression>();
            statements.ToBlock().CollectMissingSymbols(missingSymbols);
            return missingSymbols;
        }

        public static ExecutionContext MakeRunnable(this IEnumerable<IStatement> statements)
        {
            var operations = new List<IOperation>();
            var walker = new OperationTreeWalker(operations);
            statements.ToBlock().Accept(walker);
            return new ExecutionContext(operations.ToArray());
        }
    }
}
