using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestWPF.MVVM
{
    public interface IMessageBoxService
    {
        string Title { set; }
        string Message { set; }
        void ShowMessageBox();
    }
}
