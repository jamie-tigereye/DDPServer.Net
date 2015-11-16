using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.DDP.Server
{
    /// <summary>
    /// Expando object that enables classes to subscribe to property changed notifications
    /// Inherits from the INotifyPropertyChanged interface
    /// </summary>
    public abstract class NotifiableExpandoObject : UnsealedExpando, INotifyPropertyChanged
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
}
