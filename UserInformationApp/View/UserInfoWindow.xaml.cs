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
    /// Interaction logic for UserInfoWindow.xaml
    /// </summary>
    public partial class UserInfoWindow : Window
    {
        public UserInfoViewModel ViewModel { get; private set; }

        public UserInfoWindow()
        {
            ViewModel = new UserInfoViewModel();
            InitializeComponent();
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

        private void windowUserInfo_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            string message = "Changes where made on the list and were not saved\nDo you want to save it now?";

            if (ViewModel.ChangesToList && ViewModel.UserList.Count != 0)
            {
                switch (MessageBox.Show(message, "Closing without saving", MessageBoxButton.YesNoCancel, MessageBoxImage.Exclamation))
                {

                    case MessageBoxResult.Cancel:
                        e.Cancel = true;
                        break;
                    case MessageBoxResult.Yes:
                        ViewModel.SaveChangesCmd.Execute(true);
                        break;
                    case MessageBoxResult.No:
                        break;
                    default:
                        break;
                }
            }
        }

    }
}
