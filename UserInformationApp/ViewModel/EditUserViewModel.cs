using AppData;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using UserInformationApp.Cmd;
using UserInformationApp.View;

namespace UserInformationApp.ViewModel
{
    public class EditUserViewModel : ViewModelBase
    {

        #region Properties and fields

        //The list of users to edit
        private ObservableCollection<User> _usersToEdit;
        public ObservableCollection<User> UsersToEdit
        {
            get { return _usersToEdit; }
            private set
            {
                if (_usersToEdit == value) return;
                _usersToEdit = value;
                OnPropertyChanged();
            }
        }

        private ListCollectionView _usersToEditView;
        public ListCollectionView UsersToEditView
        {
            get { return _usersToEditView; }
            private set
            {
                if (_usersToEditView == value) return;
                _usersToEditView = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<User> _resetValues;
        public ObservableCollection<User> ResetValues
        {
            get { return _resetValues; }
            private set
            {
                if (_resetValues == value) return;
                _resetValues = value;
                OnPropertyChanged();
            }
        }

        //Current display id string
        private string _txtCurrentUserID;
        public string TxtCurrentUserID
        {
            get { return _txtCurrentUserID; }
            set
            {
                if (_txtCurrentUserID == value) return;
                _txtCurrentUserID = value;
                OnPropertyChanged();
            }
        }

        //Current disply position string
        private string _txtCurrentPosition;
        public string TxtCurrentPosition
        {
            get { return _txtCurrentPosition; }
            set
            {
                if (_txtCurrentPosition == value) return;
                _txtCurrentPosition = value;
                OnPropertyChanged();
            }
        }

        public bool IsNewUser { get; private set; }
        public bool CommandClose { get; private set; }


        #endregion

        #region Constructor

        public EditUserViewModel(ObservableCollection<User> users, bool isNewUser)
        {
            IsNewUser = isNewUser;
            UsersToEdit = new ObservableCollection<User>(users);
            UsersToEditView = (ListCollectionView)CollectionViewSource.GetDefaultView(UsersToEdit);
            UsersToEditView.CurrentChanged += UsersToEditView_CurrentChanged;
            UsersToEditView_CurrentChanged(UsersToEditView, new EventArgs());
            CommandClose = false;
        }

        /// <summary>
        /// Event for updating the textboxes that show the position and id of the currently worked on user
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UsersToEditView_CurrentChanged(object sender, EventArgs e)
        {
            TxtCurrentUserID = "Selected User ID: " + (UsersToEditView.CurrentItem as User).ID.ToString();
            TxtCurrentPosition = (UsersToEditView.CurrentPosition + 1).ToString() + " From Users " + (UsersToEditView.Count).ToString();
        }

        #endregion

        #region Commands and methods

        #region Movemonet Commands

        private RelayCommand _navigateFirst;
        private RelayCommand _navigatePrevious;
        private RelayCommand _navigateNext;
        private RelayCommand _navigateLast;

        public RelayCommand NavigateFirst => _navigateFirst ?? (_navigateFirst = new RelayCommand(GoToFirst, CanGoBack));
        public RelayCommand NavigatePrevious => _navigatePrevious ?? (_navigatePrevious = new RelayCommand(GoToPrevious, CanGoBack));
        public RelayCommand NavigateNext => _navigateNext ?? (_navigateNext = new RelayCommand(GoToNext, CanGoNext));
        public RelayCommand NavigateLast => _navigateLast ?? (_navigateLast = new RelayCommand(GoToLast, CanGoNext));

        private void GoToLast()
        {
            UsersToEditView.MoveCurrentToLast();
        }

        private void GoToNext()
        {
            UsersToEditView.MoveCurrentToNext();
        }

        private bool CanGoNext()
        {
            return UsersToEditView.CurrentPosition != UsersToEditView.Count - 1;
        }

        private void GoToPrevious()
        {
            UsersToEditView.MoveCurrentToPrevious();
        }

        private void GoToFirst()
        {
            UsersToEditView.MoveCurrentToFirst();
        }

        private bool CanGoBack()
        {
            return UsersToEditView.CurrentPosition != 0;
        }

        #endregion

        #region Confirm Command

        private RelayCommand<object> _confirmEditCmd;
        public RelayCommand<object> ConfirmEditCmd => _confirmEditCmd ?? (_confirmEditCmd = new RelayCommand<object>(ConfirmUserEdit, CanConfirm));

        /// <summary>
        /// Confirm the edit and close the window method
        /// </summary>
        /// <param name="obj"></param>
        private void ConfirmUserEdit(object obj)
        {
            var window = obj as EditUserWindow;
            string message;

            message = IsNewUser ? "New user will be created with the given information\nAre you sure?" : "Users will recive the edit\nare you sure?";

            if (MessageBox.Show(message, "Information edit confermation", MessageBoxButton.YesNo, MessageBoxImage.Exclamation, MessageBoxResult.No) == MessageBoxResult.Yes)
            {
                CommandClose = true;
                window.DialogResult = true;
                window.Close();
            }

        }

        /// <summary>
        /// Method that test for errors if the user 
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private bool CanConfirm(object arg)
        {
            var window = arg as EditUserWindow;
            if (window == null)
            {
                return false;
            }
            Dictionary<string, List<string>>.ValueCollection errorList = (Dictionary<string, List<string>>.ValueCollection)(window.lsbUsersToEdit.SelectedItem as User).GetErrors(null);

            if (errorList.Count > 0)
                return false;


            return true;
        }

        #endregion

        #region Cancel Command

        private RelayCommand<object> _cancelEditCmd;
        public RelayCommand<object> CancelEditCmd => _cancelEditCmd ?? (_cancelEditCmd = new RelayCommand<object>(CancelUserEdit, CanCancel));

        /// <summary>
        /// Method for canceling user creation or edits on selected users
        /// </summary>
        /// <param name="obj"></param>
        private void CancelUserEdit(object obj)
        {
            var window = obj as EditUserWindow;
            string windowMessage;

            windowMessage = IsNewUser ? "New user will be canceled\nAre you sure?" : "Any edit that has been made will be canceled\nAre you sure?";

            if (MessageBox.Show(windowMessage, "Cancel Operation", MessageBoxButton.YesNo, MessageBoxImage.Exclamation, MessageBoxResult.No) == MessageBoxResult.Yes)
            {
                CommandClose = true;
                window.DialogResult = false;
                window.Close();
            }

        }

        private bool CanCancel(object arg)
        {
            return true;
        }

        #endregion

        #endregion
    }
}
