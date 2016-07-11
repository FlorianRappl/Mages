namespace Mages.Core
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Contains all the reserved keywords.
    /// </summary>
    public static class Keywords
    {
        /// <summary>
        /// The true keyword.
        /// </summary>
        public const String True = "true";

        /// <summary>
        /// The false keyword.
        /// </summary>
        public const String False = "false";

        /// <summary>
        /// The pi constant.
        /// </summary>
        public const String Pi = "pi";

        /// <summary>
        /// The var keyword.
        /// </summary>
        public const String Var = "var";

        /// <summary>
        /// The return keyword.
        /// </summary>
        public const String Return = "return";

        /// <summary>
        /// The let keyword.
        /// </summary>
        public const String Let = "let";

        /// <summary>
        /// The const keyword.
        /// </summary>
        public const String Const = "const";

        /// <summary>
        /// The for keyword.
        /// </summary>
        public const String For = "for";

        /// <summary>
        /// The while keyword.
        /// </summary>
        public const String While = "while";

        /// <summary>
        /// The do keyword.
        /// </summary>
        public const String Do = "do";


        /// <summary>
        /// The do keyword.
        /// </summary>
        public const String Each = "each";

        /// <summary>
        /// The module keyword.
        /// </summary>
        public const String Module = "module";

        /// <summary>
        /// The if keyword.
        /// </summary>
        public const String If = "if";

        /// <summary>
        /// The else keyword.
        /// </summary>
        public const String Else = "else";

        /// <summary>
        /// The break keyword.
        /// </summary>
        public const String Break = "break";

        /// <summary>
        /// The continue keyword.
        /// </summary>
        public const String Continue = "continue";

        /// <summary>
        /// The yield keyword.
        /// </summary>
        public const String Yield = "yield";

        /// <summary>
        /// The async keyword.
        /// </summary>
        public const String Async = "async";

        /// <summary>
        /// The await keyword.
        /// </summary>
        public const String Await = "await";

        /// <summary>
        /// The class keyword.
        /// </summary>
        public const String Class = "class";

        /// <summary>
        /// The static keyword.
        /// </summary>
        public const String Static = "static";

        /// <summary>
        /// The new keyword.
        /// </summary>
        public const String New = "new";

        /// <summary>
        /// The delete keyword.
        /// </summary>
        public const String Delete = "delete";


        private static readonly Dictionary<String, Object> KeywordConstants = new Dictionary<String, Object>
        {
            { True, true },
            { False, false },
            { Pi, Math.PI }
        };

        private static readonly HashSet<String> KeywordNames = new HashSet<String>
        {
            True,
            False,
            Var,
            Return,
            Let,
            Const,
            For,
            While,
            Do,
            Each,
            Module,
            If,
            Else,
            Break,
            Continue,
            Yield,
            Async,
            Await,
            Class,
            Static,
            New,
            Delete,
            Pi
        };

        /// <summary>
        /// Gets the globally available statement keywords.
        /// </summary>
        public static readonly String[] GlobalStatementKeywords = new []
        {
            Var,
            Return,
            While,
            If
        };

        /// <summary>
        /// Gets the within-loop available statement keywords.
        /// </summary>
        public static readonly String[] LoopControlKeywords = new[]
        {
            Break,
            Continue
        };

        /// <summary>
        /// Gets the available expression keywords.
        /// </summary>
        public static readonly String[] ExpressionKeywords = new[]
        {
            True,
            False,
            New,
            Pi
        };

        /// <summary>
        /// Tries to get the constant's value.
        /// </summary>
        /// <param name="keyword">The name of the constant.</param>
        /// <param name="constant">The value of the constant.</param>
        /// <returns>True if the constant could be resolved, otherwise false.</returns>
        public static Boolean TryGetConstant(String keyword, out Object constant)
        {
            return KeywordConstants.TryGetValue(keyword, out constant);
        }

        /// <summary>
        /// Checks if the given identifier is actually a keyword.
        /// </summary>
        /// <param name="identifier">The identifier to check.</param>
        /// <returns>True if the identifier is a keyword, otherwise false.</returns>
        public static Boolean IsKeyword(String identifier)
        {
            return KeywordNames.Contains(identifier);
        }
    }
}
