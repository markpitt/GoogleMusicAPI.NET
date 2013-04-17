using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TestWPF.MVVM
{
    [Export(typeof(IMessageBoxService))]
    public class MessageBoxService : IMessageBoxService
    {
        private string _title;
        private string _message;

        public string Title
        {
            set { _title = value; }
        }

        public string Message
        {
            set { _message = value; }
        }

        public void ShowMessageBox()
        {
            MessageBox.Show(_message, _title, MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
