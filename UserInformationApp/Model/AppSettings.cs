using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserInformationApp.Model
{
    public sealed class AppSettings
    {
        #region Properties and Fields
        private static AppSettings _instance = null;

        public string FileUri { get; set; }

        public Skins SelectedTheme { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Default empty constructor
        /// </summary>
        private AppSettings()
        {
            SelectedTheme = (Skins)Enum.Parse(typeof(Skins), Properties.Settings.Default.AppTheme);
            FileUri = Properties.Settings.Default.UserDataPath;
        }
        #endregion

        #region Singleton
        public static AppSettings Instance
        {
            get
            {
                if (_instance != null)
                {
                    return _instance;
                }
                else
                {
                    _instance = new AppSettings();
                    return _instance;
                }
            }
        }
        #endregion

    }//end of settings
}
