namespace Mages.Core
{
    using System;
    using System.Collections.Generic;

    public static class Keywords
    {
        private static readonly Dictionary<String, Object> KeywordConstants = new Dictionary<String, Object>
        {
            { "true", true },
            { "false", false },
        };

        private static readonly HashSet<String> KeywordNames = new HashSet<String>
        {
            "true",
            "false",
            "var",
            "return",
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
