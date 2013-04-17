using System;
using System.Windows.Input;

namespace TestWPF.MVVM
{
	public class RelayCommand : ICommand
	{
		#region Events

		public event EventHandler CanExecuteChanged;

		#endregion

		#region Fields

		private Predicate<object> _canExecute;
		private Action<object> _execute;

		#endregion

		#region Constructors

		public RelayCommand(Action<object> execute)
		{
			_execute = execute;
		}

		public RelayCommand(Predicate<object> canExecute, Action<object> execute)
		{
			_canExecute = canExecute;
			_execute = execute;
		}

		#endregion

		#region Public

		public bool CanExecute(object parameter)
		{
			if (_canExecute != null)
			{
				return _canExecute(parameter);
			}
			return true;
		}

		public void Execute(object parameter)
		{
			_execute(parameter);
		}

		public void RaiseCanExecuteChanged()
		{
			if (CanExecuteChanged != null)
			{
				CanExecuteChanged(this, new EventArgs());
			}
		}

		#endregion
	}
}
