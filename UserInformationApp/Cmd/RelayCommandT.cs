using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserInformationApp.Cmd
{
    public class RelayCommand<T> : CommandBase
    {
        #region Fields and Properties
        private Action<T> _execute;
        private Func<T, bool> _canExecute;
        #endregion

        #region Constructors
        public RelayCommand() { }
        public RelayCommand(Action<T> action, Func<T, bool> func)
        {
            _execute = action ?? throw new ArgumentNullException(nameof(action));
            _canExecute = func;
        }
        public RelayCommand(Action<T> action) : this(action, null) { }
        #endregion

        #region Methods
        public override bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute((T)parameter);
        }

        public override void Execute(object parameter)
        {
            _execute((T)parameter);
        }
        #endregion

    }//end of RelayCommand<T>
}
