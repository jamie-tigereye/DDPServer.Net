using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.DDP.Server
{
    public class NotifiableExpandoObject : UnsealedExpando, INotifyPropertyChanged
    {
        protected void OnPropertyChanged(string propertyName)
        {
            var eventHandler = PropertyChanged;
            eventHandler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            var result = base.TrySetMember(binder, value);
            OnPropertyChanged(binder.Name);
            return result;
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }

    public class UnsealedExpando : DynamicObject
    {
            private readonly Dictionary<string, object> _members = new Dictionary<string, object>();
        
            public override bool TrySetMember(SetMemberBinder binder, object value)
            {
                if (!_members.ContainsKey(binder.Name)) _members.Add(binder.Name, value);
                else _members[binder.Name] = value;
                return true;
            }
        
            public override bool TryGetMember(GetMemberBinder binder, out object result)
            {
                if (_members.ContainsKey(binder.Name))
                {
                    result = _members[binder.Name];
                    return true;
                }
                else
                {
                    return base.TryGetMember(binder, out result);
                }
            }
        
            public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
            {
                if (_members.ContainsKey(binder.Name) && _members[binder.Name] is Delegate)
                {
                    result = (_members[binder.Name] as Delegate).DynamicInvoke(args);
                    return true;
                }
                else
                {
                    return base.TryInvokeMember(binder, args, out result);
                }
            }
        
    }
}
