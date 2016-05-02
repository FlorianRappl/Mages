namespace Mages.Core
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Contains all the reserved keywords.
    /// </summary>
    public static class Keywords
    {
        public static readonly String True = "true";
        public static readonly String False = "false";
        public static readonly String Pi = "pi";
        public static readonly String Var = "var";
        public static readonly String Return = "return";
        public static readonly String Let = "let";
        public static readonly String Const = "const";
        public static readonly String For = "for";
        public static readonly String While = "while";
        public static readonly String Do = "do";
        public static readonly String Module = "module";
        public static readonly String If = "if";
        public static readonly String Else = "else";
        public static readonly String Break = "break";
        public static readonly String Yield = "yield";
        public static readonly String Async = "async";
        public static readonly String Await = "await";
        public static readonly String Class = "class";
        public static readonly String Static = "static";
        public static readonly String New = "new";
        public static readonly String Delete = "delete";

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
            Module,
            If,
            Else,
            Break,
            Yield,
            Async,
            Await,
            Class,
            Static,
            New,
            Delete,
            Pi
        };

        public static Boolean TryGetConstant(String keyword, out Object constant)
        {
            return KeywordConstants.TryGetValue(keyword, out constant);
        }

        public static Boolean IsKeyword(String identifier)
        {
            return KeywordNames.Contains(identifier);
        }
    }
}
