using System;
using System.ComponentModel;
using System.Linq.Expressions;

namespace TestWPF.MVVM
{
    public class ViewModelBase : INotifyPropertyChanged, IViewModel
    {
        private void RaisePropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

        protected void RaisePropertyChanged<T>(Expression<Func<T>> propertyExpression)
        {
            if (propertyExpression.Body.NodeType == ExpressionType.MemberAccess)
            {
                var memberExpression = propertyExpression.Body as MemberExpression;
                string propertyName = memberExpression.Member.Name;
                RaisePropertyChanged(propertyName);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
