namespace Mages.Core.Runtime.Proxies
{
    using System;
    using System.Linq;
    using System.Reflection;

    sealed class IndexProxy : BaseProxy
    {
        private readonly PropertyInfo[] _properties;
        private readonly Function _proxy;

        public IndexProxy(WrapperObject obj, PropertyInfo[] properties)
            : base(obj)
        {
            _properties = properties;
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
            var parameters = arguments.Select(m => m.GetType()).ToArray();
            var method = _properties.Where(m => m.CanRead).Select(m => m.GetGetMethod()).Find(arguments, parameters);
            var result = default(Object);

            if (method != null)
            {
                result = method.Call(_obj, arguments);
            }

            return result;
        }
    }
}
