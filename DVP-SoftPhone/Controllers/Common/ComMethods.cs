using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuoSoftware.DuoSoftPhone.Controllers.Common
{
    public static class ComMethods
    {
        public class SwitchOnType<T1>
        {
            bool _break = false;
            private T1 _object;

            public SwitchOnType(T1 obj)
            {
                _object = obj;
            }

            public SwitchOnType<T1> Case<T2>(Action<T2> action) where T2 : class
            {
                if (_break == false)
                {
                    T2 t = _object as T2;
                    if (t != null)
                    {
                        action(t);
                        _break = true;
                    }
                }
                return this as SwitchOnType<T1>;
            }
            public void Default<T2>(Action<T2> action) where T2 : class
            {
                if (_break == false)
                {
                    T2 t = _object as T2;
                    if (t != null)
                        action(t);
                }
            }
        }

    }
}
