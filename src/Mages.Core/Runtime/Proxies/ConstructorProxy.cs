namespace Mages.Core.Runtime.Proxies
{
    using System;
    using System.Linq;
    using System.Reflection;

    sealed class ConstructorProxy : BaseProxy
    {
        private readonly ConstructorInfo[] _ctors;
        private readonly Function _proxy;

        public ConstructorProxy(WrapperObject obj, ConstructorInfo[] ctors)
            : base(obj)
        {
            _ctors = ctors;
            _proxy = new Function(Invoke);
        }

        protected override Object GetValue()
        {
            return _proxy;
        }

        protected override void SetValue(Object value)
        {
        }

        private Object Invoke(Object[] arguments)
        {
            var types = arguments.Select(m => m.GetType()).ToArray();
            var ctor = _ctors.Find(arguments, types) as ConstructorInfo;

            if (ctor != null)
            {
                return ctor.Call(arguments);
            }

            return null;
        }
    }
}
