using AppData;
using DataProvider;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Shell;
using UserInformationApp.Cmd;
using UserInformationApp.Model;
using UserInformationApp.View;
using UserInformationApp.ViewModel.Filters;

namespace UserInformationApp.ViewModel
{
    public class UserInfoViewModel : ViewModelBase
    {
        #region Properties and Fields

        #region ViewModel Collections
        //The list of users that is worked on
        private ObservableCollection<User> _usersList;
        public ObservableCollection<User> UserList
        {
            get { return _usersList; }
            private set
            {
                if (_usersList == value) return;
                _usersList = value;
                OnPropertyChanged();
            }
        }

        // Used for Navigation filtering and sorting
        private ListCollectionView _usersListView;
        public ListCollectionView UserListView
        {
            get { return _usersListView; }
            private set
            {
                if (_usersListView == value) return;
                _usersListView = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Async Related Properties
        private Progress<UsersProgressReportModel> _usersProgress;

        private bool _isBusy; //used for BusyIndicator object
        public bool IsBusy
        {
            get { return _isBusy; }
            private set
            {
                if (_isBusy == value) return;
                _isBusy = value;
                OnPropertyChanged();
            }
        }

        private int _progressPrecentage; //used for traking task complition
        public int ProgressPrecentage
        {
            get { return _progressPrecentage; }
            private set
            {
                if (_progressPrecentage == value) return;
                _progressPrecentage = value;
                OnPropertyChanged();
            }
        }

        private string _progressMessage; //used to write message based on the progress of a task
        public string ProgressMessage
        {
            get { return _progressMessage; }
            private set
            {
                if (_progressMessage == value) return;
                _progressMessage = value;
                OnPropertyChanged();
            }
        }

        private string _taskName; //used to write the task name
        public string TaskName
        {
            get { return _taskName; }
            set
            {
                if (_taskName == value) return;
                _taskName = value;
                OnPropertyChanged();
            }
        }

        private TaskbarItemProgressState _taskInProgress;
        public TaskbarItemProgressState TaskInProgress
        {
            get { return _taskInProgress; }
            set { if (_taskInProgress == value) return; _taskInProgress = value; OnPropertyChanged(); }
        }

        private double _progressValue;
        public double ProgressValue
        {
            get { return _progressValue; }
            set { if (_progressValue == value) return; _progressValue = value; OnPropertyChanged(); }
        }

        private string _taskVerb;
        private CancellationTokenSource _taskCancellationToken;
        private object _lock;
        #endregion

        //Used to get user defined settings
        private AppSettings settingsInstance;


        //The filter class for the list
        public UsersFilterViewModel ListFilter { get; set; }

        private ListSortDirection sortDirection;

        public bool ChangesToList { get; private set; } // bool used to track if any changes where made to the list



        #endregion

        #region Constructor

        public UserInfoViewModel()
        {
            settingsInstance = AppSettings.Instance;
            IsBusy = false;
            ChangesToList = false;
            ListFilter = new UsersFilterViewModel();
            sortDirection = ListSortDirection.Ascending;
            _taskCancellationToken = new CancellationTokenSource();
            _usersProgress = new Progress<UsersProgressReportModel>();
            _usersProgress.ProgressChanged += UesrsProgress_ProgressChanged;
            _lock = new object();
            _taskInProgress = TaskbarItemProgressState.None;
            _progressValue = 0;
        }

        #endregion

        #region Events
        /// <summary>
        /// Updates the BusyIndicator of the Task progress
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UesrsProgress_ProgressChanged(object sender, UsersProgressReportModel e)
        {
            ProgressPrecentage = e.UsersPrecentage;
            ProgressMessage = "Completed " + _taskVerb + " " + e.CompleatedUsersList.Count + " users from " + e.OverAllUsersCount;
            ProgressValue = (double)e.UsersPrecentage / 100;
        }


        #endregion

        #region Methods & Commands


        #region Get Users Command
        //Fill the user list box command
        private RelayCommand _getUsersCmd = null;
        public RelayCommand GetUsersCommand => _getUsersCmd ?? (_getUsersCmd = new RelayCommand(GetUserListAsync, CanGetUsersCommand));

        /* Save for backup!!!!!!!
        /// <summary>
        /// Gets The list from the data provider
        /// </summary>
        private void GetUserList()
        {
            try
            {
                UserList = new ObservableCollection<User>(Provider.GetUsers(settingsInstance.FileUri));
                UserListView = (ListCollectionView)CollectionViewSource.GetDefaultView(UserList);
            }
            catch (ArgumentException)
            {
                MessageBox.Show("Error empty path to data file\nCheck Help for more information", "File Path Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (Exception)
            {
                MessageBox.Show("Error data file not matching acceptable parameters\nCheck Help(F1) for more information", "Data File Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            
        }
        */
        /*
        /// <summary>
        /// Get users from the database Asynchronously
        /// </summary>
        private async void GetUserListAsync()
        {
            if(UserList != null)
            {
                //Clear List if there is one already present
                switch (MessageBox.Show("There is already a list loaded \ndo you want to load another one?", "Warning!", MessageBoxButton.YesNo, MessageBoxImage.Error))
                {
                    case MessageBoxResult.Yes:
                        UserList = null;
                        break;
                    case MessageBoxResult.No:
                        return;
                    default:
                        break;
                }
            }

            try
            {
                UserList = new ObservableCollection<User>(await Task.Run(() => Provider.GetUsersAsync(settingsInstance.FileUri)));
                UserListView = (ListCollectionView)CollectionViewSource.GetDefaultView(UserList);
            }
            catch (ArgumentException)
            {
                MessageBox.Show("Error empty path to data file\nCheck Help for more information", "File Path Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (Exception)
            {
                MessageBox.Show("Error data file not matching acceptable parameters\nCheck Help(F1) for more information", "Data File Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        /// <summary>
        /// Get users from the database Asynchronously
        /// </summary>
        private async void GetUserListAsync()
        {
            if (UserList != null)
            {
                //Clear List if there is one already present
                switch (MessageBox.Show("There is already a list loaded \ndo you want to load another one?", "Warning!", MessageBoxButton.YesNo, MessageBoxImage.Error))
                {
                    case MessageBoxResult.Yes:
                        UserList = null;
                        break;
                    case MessageBoxResult.No:
                        return;
                    default:
                        break;
                }
            }

            try
            {
                UserList = new ObservableCollection<User>(await Task.Run(() => Provider.GetUsersAsync(settingsInstance.FileUri)));
                UserListView = (ListCollectionView)CollectionViewSource.GetDefaultView(UserList);
            }
            catch (ArgumentException)
            {
                MessageBox.Show("Error empty path to data file\nCheck Help for more information", "File Path Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (Exception)
            {
                MessageBox.Show("Error data file not matching acceptable parameters\nCheck Help(F1) for more information", "Data File Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        */

        /// <summary>
        /// Get users from the database Asynchronously
        /// </summary>
        private async void GetUserListAsync()
        {
            if (UserList != null)
            {
                string message = "Warning there is a list already loaded.\nDo you wish to load another one?";
                switch (MessageBox.Show(message, "Warning", MessageBoxButton.YesNo, MessageBoxImage.Exclamation))
                {
                    case MessageBoxResult.Yes:
                        break;
                    case MessageBoxResult.No:
                        return;
                    default:
                        break;
                }
            }

            IsBusy = true;
            _taskVerb = "adding";
            TaskName = "Getting Users";
            UserList = null;
            TaskInProgress = TaskbarItemProgressState.Normal;
            try
            {
                UserList = new ObservableCollection<User>(await Task.Run(() => Provider.GetUsersAsyncV2(settingsInstance.FileUri, _usersProgress, _taskCancellationToken.Token)));
                UserListView = (ListCollectionView)CollectionViewSource.GetDefaultView(UserList);
                BindingOperations.EnableCollectionSynchronization(UserList, _lock);
            }
            catch (ArgumentException)
            {
                MessageBox.Show("Error empty path to data file\nCheck Help for more information", "File Path Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (OperationCanceledException e)
            {
                MessageBox.Show(e.Message, "Operation Canceled", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            catch (Exception)
            {
                MessageBox.Show("File Not matching requested pattern", "File Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                _taskCancellationToken.Dispose();
                _taskCancellationToken = new CancellationTokenSource();
                IsBusy = false;
                TaskInProgress = TaskbarItemProgressState.None;
            }

        }

        /// <summary>
        /// Checks if there is need to load the users
        /// </summary>
        /// <returns>if the users list is not empty</returns>
        private bool CanGetUsersCommand()
        {
            return !IsBusy;
        }
        #endregion

        #region Remove Users Commands
        // Remove selected user command
        private RelayCommand<IList<object>> _removeUserCommand = null;
        public RelayCommand<IList<object>> RemoveUserCommand => _removeUserCommand ?? (_removeUserCommand = new RelayCommand<IList<object>>(RemoveSelectedUser, CanRemoveSelectedUser));

        /// <summary>
        /// Checks if there is a user to remove
        /// </summary>
        /// <param name="arg">The user object to remove can be null</param>
        /// <returns>bool for if there is selected item to remove</returns>
        private bool CanRemoveSelectedUser(IList<object> args)
        {
            if (UserList == null)
                return false;
            if (args == null)
                return false;
            else if (args.Count > 0)
                return true;

            return false;
        }

        /// <summary>
        /// Remove a selected users in the user list
        /// </summary>
        /// <param name="obj">The user to remove</param>
        private void RemoveSelectedUser(IList<object> obj)
        {
            string message = $"Are you sure that you want to remove { obj.Count} ";
            message += (obj.Count > 1) ? "users" : "user";
            List<object> temp = new List<object>(obj);

            //Used to allow the user the reconsider removal of users from the list
            switch (MessageBox.Show(message, "Warning Delete Users!", MessageBoxButton.YesNo, MessageBoxImage.Exclamation, MessageBoxResult.No))
            {
                case MessageBoxResult.Yes:
                    foreach (var item in temp)
                    {
                        UserList.Remove(item as User);
                    }
                    ChangesToList = true;
                    break;
                case MessageBoxResult.No:
                    break;
                default:
                    break;
            }

        }


        #endregion

        #region Add User Command
        private RelayCommand _addUserCommand = null;
        public RelayCommand AddUserCommand => _addUserCommand ?? (_addUserCommand = new RelayCommand(AddUserToList, CanAddUser));

        /// <summary>
        /// Checks if the adding user is vaiable 
        /// </summary>
        /// <returns>if there is a user list to add to</returns>
        private bool CanAddUser()
        {
            return UserList != null;
        }

        /// <summary>
        /// Adds a new user to the list and opens the editing window to update it's information
        /// </summary>
        private void AddUserToList()
        {

            UsersFactory factory = new UsersFactory();
            var usersCollection = new ObservableCollection<User>() { factory.CreateUser(CalculateID()) };
            EditUserWindow editUserWindow = new EditUserWindow(usersCollection, true);
            editUserWindow.ShowDialog();

            if ((bool)editUserWindow.DialogResult) //Check if the resault from the window is ture if so add the user to the list
            {
                UserList.Add(editUserWindow.ViewModel.UsersToEdit[0]);
                ChangesToList = true;
            }
        }

        /// <summary>
        /// Claculates the id to give the user in the list
        /// all of the id's are the rows which the user is written
        /// </summary>
        /// <returns></returns>
        private int CalculateID()
        {
            int id = 1;
            UserList = new ObservableCollection<User>(UserList.OrderBy(i => i)); //sort the list in order to place the new user in correct index
            UserListView = (ListCollectionView)CollectionViewSource.GetDefaultView(UserList); // refresh the collection view to allow sorting and filtering
            ListFilter.ResetFilter();

            for (int i = 0; i < UserList.Count; i++, id++)
            {
                if (id != UserList[i].ID)
                {
                    id = i + 1;
                    break;
                }
            }
            return id;

        }

        #endregion

        #region Edit Command

        private RelayCommand<IList<object>> _editCommand;
        public RelayCommand<IList<object>> EditCommand => _editCommand ?? (_editCommand = new RelayCommand<IList<object>>(EditUser, CanEdit));

        /// <summary>
        /// Test if there is any item selected for editing
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private bool CanEdit(IList<object> arg)
        {
            if (UserList == null)
                return false;
            if (arg == null)
                return false;
            else if (arg.Count > 0)
                return true;

            return false;
        }

        /// <summary>
        /// Method used for opening the edit user dialog used to edit selected items from an observable collection given
        /// </summary>
        /// <param name="obj"></param>
        private void EditUser(IList<object> obj)
        {
            var factory = new UsersFactory();
            List<User> temp = new List<User>();
            foreach (var item in obj)
            {
                temp.Add(item as User);
            }

            //Creat copies of the selected items list
            ObservableCollection<User> usersToEdit = new ObservableCollection<User>(temp.ConvertAll(user => factory.CreateUser(user)));
            ObservableCollection<User> copyForLater = new ObservableCollection<User>(temp.ConvertAll(user => factory.CreateUser(user)));

            EditUserWindow window = new EditUserWindow(usersToEdit, false);
            if (window.ShowDialog() == true)
            {
                foreach (var item in copyForLater)
                {
                    UserList.Remove(item);
                }
                foreach (var item in window.ViewModel.UsersToEdit)
                {
                    UserList.Add(item);
                }

                UserList = new ObservableCollection<User>(UserList.OrderBy(i => i)); //sort the list in order to place the new user in correct index
                UserListView = (ListCollectionView)CollectionViewSource.GetDefaultView(UserList); // refresh the collection view to allow sorting and filtering
                ChangesToList = true;
            }
        }

        #endregion

        #region Open Settings Command
        private RelayCommand<Window> _openSettingsCommand = null;
        public RelayCommand<Window> OpenSettingsCommand => _openSettingsCommand ?? (_openSettingsCommand = new RelayCommand<Window>(OpenSettingsWindow));

        /// <summary>
        /// Open settings window as a new dialog
        /// </summary>
        /// <param name="owner">set the window that is calling as the owner of settings</param>
        private void OpenSettingsWindow(Window owner)
        {
            var settingsWindow = new SettingsWindow();
            settingsWindow.Owner = owner;
            settingsWindow.ShowDialog();
        }
        #endregion

        #region Filter Commands

        #region Applay Filter
        private RelayCommand _filterListCmd;
        public RelayCommand FilterListCmd => _filterListCmd ?? (_filterListCmd = new RelayCommand(FilterUserList, CanFilterUserList));

        /// <summary>
        /// Sets a filter on the view of the user collection
        /// </summary>
        private void FilterUserList()
        {
            UserListView.Filter = new Predicate<object>(ListFilter.FilterUsers);
        }

        /// <summary>
        /// Checks if there is a list to filter
        /// </summary>
        /// <returns></returns>
        private bool CanFilterUserList()
        {
            return UserList != null && !IsBusy;
        }

        #endregion

        #region Clear Filter
        //used for reseting the filters
        private RelayCommand _clearFilterCmd;
        public RelayCommand ClearFilterCmd => _clearFilterCmd ?? (_clearFilterCmd = new RelayCommand(ClearFilter, CanClearFilter));

        /// <summary>
        /// Clears any applied filters to the collection view object
        /// </summary>
        private void ClearFilter()
        {
            UserListView.Filter = null;
            ListFilter.ResetFilter();
        }

        /// <summary>
        /// Checks if there are any applied filters
        /// </summary>
        /// <returns></returns>
        private bool CanClearFilter()
        {
            if (UserListView == null) return false;
            return UserListView.Filter != null;

        }
        #endregion

        #endregion

        #region Sorting Commands
        private RelayCommand<string> _sortListByCmd;
        public RelayCommand<string> SortListByCmd => _sortListByCmd ?? (_sortListByCmd = new RelayCommand<string>(SortList, CanSortList));

        /// <summary>
        /// Checks if there is a list to sort
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private bool CanSortList(string arg)
        {
            return UserList != null;
        }

        /// <summary>
        /// Sort the list based on the given property
        /// </summary>
        /// <param name="obj"></param>
        private void SortList(string obj)
        {
            string propertyToSort = null;

            if (obj != null)
            {
                propertyToSort = SetUpSorting(obj);

                UserListView.SortDescriptions.Add(new SortDescription(propertyToSort, sortDirection)); //set the sorting to the view

                UserListView.Refresh(); // force refresh to ensure gui update
            }

        }

        /// <summary>
        /// Parse and sorts the porperty and the direction to sort the list
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private string SetUpSorting(string obj)
        {
            string propertyToSort = null;



            if (obj.Contains(" ")) // since most of the properties use cammel case if the stirng has white space fix it
            {
                var temp = obj.Split(' ');
                for (int i = 0; i < temp.Length; i++)
                {
                    propertyToSort += temp[i];
                }
            }
            else
            {
                propertyToSort = obj;
            }

            SortDescription description = new SortDescription(propertyToSort, sortDirection);

            if (UserListView.SortDescriptions.Count > 0) //checks if the property is sorted already and sets up the direction
            {
                sortDirection = (description.Direction == ListSortDirection.Ascending) ? ListSortDirection.Descending : ListSortDirection.Ascending;
                UserListView.SortDescriptions.Clear();
            }

            return propertyToSort;
        }
        #endregion

        #region Save Changes Command

        private RelayCommand _saveChangesCmd;
        public RelayCommand SaveChangesCmd => _saveChangesCmd ?? (_saveChangesCmd = new RelayCommand(SaveChangesToList, CanSaveChanges));

        private void SaveChangesToList()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Csv File (*.csv)|*.csv";
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    List<User> temp = new List<User>(UserList);
                    Provider.WriteUsersFile(saveFileDialog.FileName, temp);
                    MessageBox.Show("File Saved");
                    ChangesToList = false;
                }
                catch (Exception)
                {
                    MessageBox.Show("Problem with saving the file");
                }
            }
        }

        private bool CanSaveChanges()
        {
            if (UserList == null) return false;
            if (!ChangesToList) return false;
            if (UserList.Count == 0) return false;
            return true;
        }

        #endregion

        #region Cancel Command

        private RelayCommand _cancelTaskCmd;
        public RelayCommand CancelTaskCommand => _cancelTaskCmd ?? (_cancelTaskCmd = new RelayCommand(CancelCurrentTask, null));

        private void CancelCurrentTask()
        {
            _taskCancellationToken.Cancel();
        }

        #endregion

        #endregion
    }
}
