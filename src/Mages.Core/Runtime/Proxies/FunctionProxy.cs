namespace Mages.Core.Runtime.Proxies
{
    using Mages.Core.Runtime.Functions;
    using System;
    using System.Reflection;

    abstract class FunctionProxy(WrapperObject obj, MethodBase[] methods) : BaseProxy(obj)
    {
        protected readonly MethodBase[] _methods = methods;
        private readonly Int32 _maxParameters = methods.MaxParameters();
        protected Function _proxy;

        protected Object TryCurry(Object[] arguments)
        {
            return Curry.Min(_maxParameters, _proxy, arguments);
        }

        protected override Object GetValue()
        {
            return _proxy;
        }

        protected override void SetValue(Object value)
        {
        }
    }
}
