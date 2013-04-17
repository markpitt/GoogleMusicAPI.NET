using System;

namespace TestWPF.MVVM
{
    public interface IDispatchService
    {
        void Invoke(Action action);
    }
}
