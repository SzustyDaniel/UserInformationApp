using AppData;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for EditUserWindow.xaml
    /// </summary>
    public partial class EditUserWindow : Window
    {
        public EditUserViewModel ViewModel { get; private set; }

        public EditUserWindow(ObservableCollection<User> users, bool isNewUser)
        {
            ViewModel = new EditUserViewModel(users, isNewUser);
            InitializeComponent();
            spNavigationPanel.Visibility = isNewUser ? Visibility.Collapsed : Visibility.Visible;
        }



        /// <summary>
        /// Marks the Text in the textboxes when they get focused
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBoxGotFocus(object sender, RoutedEventArgs e)
        {
            TextBox focusedObject = e.OriginalSource as TextBox;

            if (focusedObject != null)
                focusedObject.SelectAll();

        }

        /// <summary>
        /// Event that is used to prevent the uesr from accidently closing the window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void windowEditUser_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!ViewModel.CommandClose)
            {
                switch (MessageBox.Show("Are you sure that you want to close the window?", "Closing Window without conforming", MessageBoxButton.YesNo, MessageBoxImage.Exclamation, MessageBoxResult.No))
                {
                    case MessageBoxResult.Yes:
                        DialogResult = false;
                        break;
                    case MessageBoxResult.No:
                        e.Cancel = true;
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
