using AppData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserInformationApp.ViewModel.Filters
{
    public enum GenderSelect
    {
        All, Male, Female
    }

    public class UsersFilterViewModel : ViewModel.ViewModelBase
    {

        #region Properties and Fields
        /// <summary>
        /// Controls for filtering binding
        /// </summary>

        #region First Name Properties

        private string _txtFirstName;
        public string TxtFirstName
        {
            get { return _txtFirstName; }
            set
            {
                if (_txtFirstName == value) return;
                _txtFirstName = value;
                OnPropertyChanged();
            }
        }

        private bool _chbFirstName;
        public bool ChbFirstName
        {
            get { return _chbFirstName; }
            set
            {
                if (_chbFirstName == value) return;
                _chbFirstName = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Last Name Properties
        private string _txtLastName;
        public string TxtLastName
        {
            get { return _txtLastName; }
            set
            {
                if (_txtLastName == value) return;
                _txtLastName = value;
                OnPropertyChanged();
            }
        }

        private bool _chbLastName;
        public bool ChbLastName
        {
            get { return _chbLastName; }
            set
            {
                if (_chbLastName == value) return;
                _chbLastName = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Email Properties
        private string _txtEmail;
        public string TxtEmail
        {
            get { return _txtEmail; }
            set
            {
                if (_txtEmail == value) return;
                _txtEmail = value;
                OnPropertyChanged();
            }
        }

        private bool _chbEmail;
        public bool ChbEmail
        {
            get { return _chbEmail; }
            set
            {
                if (_chbEmail == value) return;
                _chbEmail = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Gender Properties
        private GenderSelect _currentGender;
        public GenderSelect CurrentGender
        {
            get { return _currentGender; }
            set
            {
                if (_currentGender == value) return;
                _currentGender = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Country Properties
        private string _txtCountry;
        public string TxtCountry
        {
            get { return _txtCountry; }
            set
            {
                if (_txtCountry == value) return;
                _txtCountry = value;
                OnPropertyChanged();
            }
        }

        private bool _chbCountry;
        public bool ChbCountry
        {
            get { return _chbCountry; }
            set
            {
                if (_chbCountry == value) return;
                _chbCountry = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region City Properties
        private string _txtCity;
        public string TxtCity
        {
            get { return _txtCity; }
            set
            {
                if (_txtCity == value) return;
                _txtCity = value;
                OnPropertyChanged();
            }
        }

        private bool _chbCity;
        public bool ChbCity
        {
            get { return _chbCity; }
            set
            {
                if (_chbCity == value) return;
                _chbCity = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Street Properties
        private string _txtStreet;
        public string TxtStreet
        {
            get { return _txtStreet; }
            set
            {
                if (_txtStreet == value) return;
                _txtStreet = value;
                OnPropertyChanged();
            }
        }

        private bool _chbStreet;
        public bool ChbStreet
        {
            get { return _chbStreet; }
            set
            {
                if (_chbStreet == value) return;
                _chbStreet = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Phone number Properties
        private string _txtPhoneNumber;
        public string TxtPhoneNumber
        {
            get { return _txtPhoneNumber; }
            set
            {
                if (_txtPhoneNumber == value) return;
                _txtPhoneNumber = value;
                OnPropertyChanged();
            }
        }

        private bool _chbPhoneNumber;
        public bool ChbPhoneNumber
        {
            get { return _chbPhoneNumber; }
            set
            {
                if (_chbPhoneNumber == value) return;
                _chbPhoneNumber = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #endregion

        #region Methods
        /// <summary>
        /// Filters Users based on the given citirions
        /// </summary>
        /// <param name="item"></param>
        /// <returns>if the given object is valid by filtering</returns>
        public bool FilterUsers(object item)
        {
            User user = item as User;
            int numOfevaluated = PropertiesToCheck();
            int numOfValid = 0;
            if (user != null)
            {

                if (ChbFirstName)
                    if (user.FirstName.ToLower().Contains(TxtFirstName.ToLower()))
                        numOfValid++;
                if (ChbLastName)
                    if (user.LastName.ToLower().Contains(TxtLastName.ToLower()))
                        numOfValid++;
                if (ChbEmail)
                    if (user.Email.ToLower().Contains(TxtEmail.ToLower()))
                        numOfValid++;
                if (CurrentGender.ToString() == user.Gender)
                    numOfValid++;
                if (ChbCountry)
                    if (user.Country.ToLower().Contains(TxtCountry.ToLower()))
                        numOfValid++;
                if (ChbCity)
                    if (user.City.ToLower().Contains(TxtCity.ToLower()))
                        numOfValid++;
                if (ChbStreet)
                    if (user.Street.ToLower().Contains(TxtStreet.ToLower()))
                        numOfValid++;
                if (ChbPhoneNumber)
                    if (user.PhoneNumber.ToLower().Contains(TxtPhoneNumber.ToLower()))
                        numOfValid++;

                return numOfevaluated == numOfValid;
            }

            return false;
        }

        /// <summary>
        /// Check how many variables to check in the filtering
        /// </summary>
        /// <returns>the number of properties to check</returns>
        private int PropertiesToCheck()
        {
            int numToCheck = 0;

            if (ChbFirstName)
                numToCheck++;
            if (ChbLastName)
                numToCheck++;
            if (ChbEmail)
                numToCheck++;
            if (CurrentGender != GenderSelect.All)
                numToCheck++;
            if (ChbCountry)
                numToCheck++;
            if (ChbCity)
                numToCheck++;
            if (ChbStreet)
                numToCheck++;
            if (ChbPhoneNumber)
                numToCheck++;

            return numToCheck;
        }

        /// <summary>
        /// Clears the filter properties
        /// </summary>
        public void ResetFilter()
        {
            ChbFirstName = false;
            TxtFirstName = null;

            ChbLastName = false;
            TxtLastName = null;

            ChbEmail = false;
            TxtEmail = null;

            CurrentGender = GenderSelect.All;

            ChbCountry = false;
            TxtCountry = null;

            ChbCity = false;
            TxtCity = null;

            ChbStreet = false;
            TxtStreet = null;

            ChbPhoneNumber = false;
            TxtPhoneNumber = null;
        }
        #endregion

    }
}
