using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using UserInformationApp.Cmd;
using UserInformationApp.Model;

namespace UserInformationApp.ViewModel
{
    public class SettingsViewModel : ViewModelBase
    {
        #region Properties and fields

        private AppSettings settingsModel = AppSettings.Instance;
        private bool hasChanged = false; //used to flag changes in the properties
        public bool HasChanged { get { return hasChanged; } }

        public string UsersFileUri
        {
            get { return settingsModel.FileUri; }
            set
            {
                if (settingsModel.FileUri == value) return;
                settingsModel.FileUri = value;
                hasChanged = true;
                OnPropertyChanged();
            }
        }

        public Skins SelectedAppTheme
        {
            get { return settingsModel.SelectedTheme; }
            set
            {
                if (settingsModel.SelectedTheme == value) return;
                settingsModel.SelectedTheme = value;
                hasChanged = true;
                OnPropertyChanged();
            }
        }

        public List<Skins> ThemesList { get { return Enum.GetValues(typeof(Skins)).Cast<Skins>().ToList(); } }

        #endregion

        #region Commands

        #region FileCommands
        /// <summary>
        /// File Path command
        /// </summary>
        private RelayCommand _getUriCommand = null;
        public RelayCommand GetUriCommand => _getUriCommand ?? (_getUriCommand = new RelayCommand(GetFilePath));

        /// <summary>
        /// Sets the path for the data source file
        /// </summary>
        private void GetFilePath()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Csv files (*.csv)|*.csv";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (openFileDialog.ShowDialog() == true)
            {
                UsersFileUri = Path.GetFullPath(openFileDialog.FileName);
            }


        }
        #endregion

        #region Save commands
        private RelayCommand _saveSettingsCommand = null;
        public RelayCommand SaveSettingsCommand => _saveSettingsCommand ?? (_saveSettingsCommand = new RelayCommand(SaveSettings, CanSaveSettings));

        private bool CanSaveSettings()
        {
            return hasChanged;
        }

        /// <summary>
        /// Saves the current values from the properties in settings to Application Settings
        /// </summary>
        private void SaveSettings()
        {
            Properties.Settings.Default.AppTheme = settingsModel.SelectedTheme.ToString();
            Properties.Settings.Default.UserDataPath = settingsModel.FileUri;
            Properties.Settings.Default.Save();
            hasChanged = false;
        }

        #endregion

        #region Revets to Defaults Commands
        private RelayCommand _revetToDefaultsCommand = null;
        public RelayCommand RevetToDefaultsCommand => _revetToDefaultsCommand ?? (_revetToDefaultsCommand = new RelayCommand(ReadDefaults));

        /// <summary>
        /// This method reads from Application settings the program default values for the settings
        /// </summary>
        private void ReadDefaults()
        {
            if (MessageBox.Show("Reveting to program default settings are you sure?", "Reset settings", MessageBoxButton.OKCancel, MessageBoxImage.Question, MessageBoxResult.Cancel) == MessageBoxResult.OK)
            {
                UsersFileUri = Properties.Settings.Default.UserDataPathDefault;
                SelectedAppTheme = (Skins)Enum.Parse(typeof(Skins), Properties.Settings.Default.AppDefaultTheme);
            }
        }

        #endregion

        #endregion

        #region Methods
        /// <summary>
        /// Used when changes happend to the properties of the settings instance but they are not committed
        /// </summary>
        public void ResetSettings()
        {
            settingsModel.FileUri = Properties.Settings.Default.UserDataPath;
            settingsModel.SelectedTheme = (Skins)Enum.Parse(typeof(Skins), Properties.Settings.Default.AppTheme);
        }

        #endregion
    }
}
