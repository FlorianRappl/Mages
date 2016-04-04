namespace Mages.Core.Ast
{
    using System;

    public interface IParser
    {
        Boolean HasErrors { get; }

        Int32 ErrorCount { get; }
    }
}
