namespace Mages.Plugins.Transpilers
{
    using Core.Runtime.Functions;
    using Mages.Core;
    using System;
    using System.Collections.Generic;

    public class TranspilersPlugin
    {
        public static readonly String Name = "Transpilers";
        public static readonly String Version = "1.0.0";
        public static readonly String Author = "Florian Rappl";
        private readonly IDictionary<String, Object> _transpiler;

        public TranspilersPlugin(Engine engine)
        {
            var transpiler = new Transpiler(engine);

            _transpiler = new Dictionary<String, Object>
            {
                { "toJavaScript", Wrap(transpiler.Js) },
            };
        }

        public IDictionary<String, Object> Transpile
        {
            get { return _transpiler; }
        }

        private static Function Wrap(Func<String, String> transformer)
        {
            var function = default(Function);
            function = args => Curry.MinOne(function, args) ?? If.Is<String>(args, transformer);
            return function;
        }
    }
}
