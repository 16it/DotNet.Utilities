using System;
using System.Diagnostics;
using System.Windows.Input;

namespace YanZhiwei.DotNet3._5.Utilities.WPF
{
    public class RelayCommand : ICommand
    {
        #region Members

        private readonly Func<Boolean> canExecute;
        private readonly Action execute;

        #endregion Members

        #region Constructors

        public RelayCommand(Action execute)
            : this(execute, null)
        {
        }

        public RelayCommand(Action execute, Func<Boolean> canExecute)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");
            this.execute = execute;
            this.canExecute = canExecute;
        }

        #endregion Constructors

        #region ICommand Members

        public event EventHandler CanExecuteChanged
        {
            add
            {
                if (canExecute != null)
                    CommandManager.RequerySuggested += value;
            }
            remove
            {
                if (canExecute != null)
                    CommandManager.RequerySuggested -= value;
            }
        }

        [DebuggerStepThrough]
        public Boolean CanExecute(Object parameter)
        {
            return canExecute == null ? true : canExecute();
        }

        public void Execute(Object parameter)
        {
            execute();
        }

        #endregion ICommand Members
    }
}