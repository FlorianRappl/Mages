namespace Mages.Core.Runtime.Proxies
{
    using System;
    using System.Reflection;

    sealed class FieldProxy(WrapperObject obj, FieldInfo field) : BaseProxy(obj)
    {
        private readonly FieldInfo _field = field;

        protected override Object GetValue()
        {
            var target = _obj.Content;
            return _field.GetValue(target);
        }

        protected override void SetValue(Object value)
        {
            var target = _obj.Content;
            var result = Convert(value, _field.FieldType);

            try { _field.SetValue(target, result); } 
            catch { }
        }
    }
}
