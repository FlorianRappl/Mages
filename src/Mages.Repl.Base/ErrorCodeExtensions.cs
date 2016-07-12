namespace Mages.Repl
{
    using Mages.Core;
    using System;
    using System.ComponentModel;

    static class ErrorCodeExtensions
    {
        public static String GetMessage(this ErrorCode code)
        {
            var type = typeof(ErrorCode);
            var members = type.GetMember(code.ToString());
            var attributes = members[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
            return ((DescriptionAttribute)attributes[0]).Description;
        }
    }
}
