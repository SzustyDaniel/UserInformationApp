using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace UserInformationApp.Resources.Themes
{
    /// <summary>
    /// Responsible for the current theme of the application
    /// </summary>
    public class ThemesDictionary : ResourceDictionary
    {
        #region Properties and Fields
        private Uri _lightThemeSource;
        public Uri LightThemeSource
        {
            get { return _lightThemeSource; }
            set
            {
                if (_lightThemeSource == value) return;
                _lightThemeSource = value;
                UpdateSource();
            }
        }

        private Uri _DarkThemeSource;
        public Uri DarkThemeSource
        {
            get { return _DarkThemeSource; }
            set
            {
                if (_DarkThemeSource == value) return;
                _DarkThemeSource = value;
                UpdateSource();
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Change Theme Uri ditionary
        /// </summary>
        private void UpdateSource()
        {
            Uri selectedTheme;
            switch (App.AppSkin)
            {
                case UserInformationApp.Skins.Light_Theme:
                    selectedTheme = LightThemeSource;
                    break;
                case UserInformationApp.Skins.Dark_Theme:
                    selectedTheme = DarkThemeSource;
                    break;
                default:
                    selectedTheme = null;
                    break;
            }

            // if the uri is valid and is not same to the one that is used change it
            if (selectedTheme != null && base.Source != selectedTheme)
                base.Source = selectedTheme;

        }
        #endregion
    }
}
