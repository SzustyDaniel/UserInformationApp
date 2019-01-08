using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using UserInformationApp.ViewModel;

namespace UserInformationApp.View
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        public SettingsViewModel ViewModel { get; set; } = new SettingsViewModel();

        public SettingsWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Used to Check if settings were changed and if the user wish to save them if he hadn't
        /// committed to them
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void windowSettings_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (ViewModel.HasChanged)
            {
                switch (MessageBox.Show("Changes has been made to the settings but are not saved\nDo you wish to save them?", "Settings has been changed", MessageBoxButton.YesNoCancel, MessageBoxImage.Exclamation, MessageBoxResult.No))
                {
                    case MessageBoxResult.Cancel:
                        e.Cancel = true;
                        break;
                    case MessageBoxResult.Yes:
                        ViewModel.SaveSettingsCommand.Execute(true);
                        break;
                    case MessageBoxResult.No:
                        ViewModel.ResetSettings();
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
