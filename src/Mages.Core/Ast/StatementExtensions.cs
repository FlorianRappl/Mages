namespace Mages.Core.Ast
{
    using Mages.Core.Ast.Expressions;
    using Mages.Core.Ast.Statements;
    using Mages.Core.Ast.Walkers;
    using Mages.Core.Vm;
    using System.Collections.Generic;

    /// <summary>
    /// A collection of statement extensions.
    /// </summary>
    public static class StatementExtensions
    {
        /// <summary>
        /// Looks for missing symbols in the provided statement.
        /// </summary>
        /// <param name="statement">The statement.</param>
        /// <returns>The found list of missing symbols.</returns>
        public static List<VariableExpression> FindMissingSymbols(this IStatement statement)
        {
            var missingSymbols = new List<VariableExpression>();
            statement.CollectMissingSymbols(missingSymbols);
            return missingSymbols;
        }

        /// <summary>
        /// Looks for missing symbols in the provided statements.
        /// </summary>
        /// <param name="statements">The statements.</param>
        /// <returns>The found list of missing symbols.</returns>
        public static List<VariableExpression> FindMissingSymbols(this IEnumerable<IStatement> statements)
        {
            var block = statements.ToBlock();
            return block.FindMissingSymbols();
        }

        /// <summary>
        /// Converts the given statements to a single block statement.
        /// </summary>
        /// <param name="statements">The statements.</param>
        /// <returns>The single block statement containing all statements.</returns>
        public static BlockStatement ToBlock(this IEnumerable<IStatement> statements)
        {
            var list = new List<IStatement>(statements);
            var start = list.Count > 0 ? list[0].Start : new TextPosition();
            var end = list.Count > 0 ? list[list.Count - 1].End : start;
            return new BlockStatement(list.ToArray(), start, end);
        }

        /// <summary>
        /// Collects the missing symbols in the provided statement.
        /// </summary>
        /// <param name="statement">The statement.</param>
        /// <param name="missingSymbols">The list of missing symbols to populate.</param>
        public static void CollectMissingSymbols(this IStatement statement, List<VariableExpression> missingSymbols)
        {
            var walker = new SymbolTreeWalker(missingSymbols);
            statement.Accept(walker);
        }

        /// <summary>
        /// Transforms the statements to an array of operations.
        /// </summary>
        /// <param name="statements">The statements.</param>
        /// <returns>The operations that can be run.</returns>
        public static IOperation[] MakeRunnable(this IEnumerable<IStatement> statements)
        {
            var operations = new List<IOperation>();
            var walker = new OperationTreeWalker(operations);
            statements.ToBlock().Accept(walker);
            return operations.ToArray();
        }
    }
}
