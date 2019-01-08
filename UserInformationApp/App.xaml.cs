using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using UserInformationApp.Model;

namespace UserInformationApp
{
    public enum Skins { Light_Theme, Dark_Theme }

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static Skins AppSkin { get; set; }

        public App()
        {
            AppSkin = AppSettings.Instance.SelectedTheme;
        }

    }
}
