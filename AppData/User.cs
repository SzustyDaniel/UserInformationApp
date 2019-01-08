using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData
{
    //id	first_name	last_name	email	gender	country	city	street	phone_number
    public class User : IComparable<User>, INotifyDataErrorInfo
    {

        #region Properties
        public int ID { get; set; }

        private string _firstName;
        public string FirstName
        {
            get { return _firstName; }
            set
            {
                _firstName = value;
                CheckEmpty("FirstName", _firstName);
            }
        }

        private string _lastName;
        public string LastName
        {
            get { return _lastName; }
            set
            {
                _lastName = value;
                CheckEmpty("LastName", _lastName);
            }
        }

        private string _email;
        public string Email
        {
            get { return _email; }
            set
            {
                _email = value;
                CheckEmpty("Email", _email);
            }
        }

        private string _gender;
        public string Gender
        {
            get { return _gender; }
            set
            {
                bool valid = true;
                _gender = value;

                if (_gender == null)
                    valid = false;
                else if (_gender.ToLower() != "male" && _gender.ToLower() != "female")
                    valid = false;

                if (!valid)
                {
                    List<string> errors = new List<string>();
                    errors.Add("Gender can only be Male or Female.");
                    SetErros("Gender", errors);
                }
                else
                {
                    ClearErrors("Gender");
                }
            }
        }

        private string _country;
        public string Country
        {
            get { return _country; }
            set
            {
                _country = value;
                CheckEmpty("Country", _country);
            }
        }

        private string _city;
        public string City
        {
            get { return _city; }
            set
            {
                _city = value;
                CheckEmpty("City", _city);
            }
        }

        private string _street;
        public string Street
        {
            get { return _street; }
            set
            {
                _street = value;
                CheckEmpty("Street", _street);
            }
        }

        private string _phoneNumber;
        public string PhoneNumber
        {
            get { return _phoneNumber; }
            set
            {
                _phoneNumber = value;
                CheckEmpty("PhoneNumber", _phoneNumber);
            }
        }

        public bool HasErrors { get { return (errors.Count > 0); } }
        private Dictionary<string, List<string>> errors = new Dictionary<string, List<string>>();
        public event EventHandler<DataErrorsChangedEventArgs> ErrorChanged;
        #endregion

        #region Constructors
        public User()
        {

        }

        public User(int id)
        {
            this.ID = id;
            this.FirstName = null;
            this.LastName = null;
            this.Email = null;
            this.Gender = null;
            this.Country = null;
            this.City = null;
            this.Street = null;
            this.PhoneNumber = null;
        }

        /// <summary>
        /// Brand new user addition constructor
        /// </summary>
        /// <param name="id">The ID in table of the user</param>
        /// <param name="firstName">The first name of the user</param>
        /// <param name="lastName">The last name of the user</param>
        /// <param name="email">The email of the user</param>
        /// <param name="gender">The gender of the user</param>
        /// <param name="country">The country of the user</param>
        /// <param name="city">The city of the user</param>
        /// <param name="street">The street where the user lives</param>
        /// <param name="phoneNumber">The phone number of the user</param>
        public User(int id, string firstName, string lastName, string email, string gender, string country, string city, string street, string phoneNumber)
        {
            this.ID = id;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Email = email;
            this.Gender = gender;
            this.Country = country;
            this.City = city;
            this.Street = street;
            this.PhoneNumber = phoneNumber;
        }

        /// <summary>
        /// Copy constructor of the class
        /// </summary>
        /// <param name="other">The class to copy the information from</param>
        public User(User other) : this(other.ID, other.FirstName, other.LastName, other.Email, other.Gender, other.Country, other.City, other.Street, other.PhoneNumber) { }

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;


        #endregion

        #region Methods

        #region Base methods
        public int CompareTo(User other)
        {
            return this.ID.CompareTo(other.ID);
        }

        /// <summary>
        /// String representation of the class 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"User information -> Id: {this.ID}, First Name: {this.FirstName}, Last Name: {this.LastName}, Email: {this.Email}\n" + $"Gender: {this.Gender}, Country: {this.Country}, City: {this.City}, Street: {this.Street}, Phone Number: {this.PhoneNumber}\n" + "*****************************************************************";
        }

        /// <summary>
        /// Check for equality of User objects
        /// </summary>
        /// <param name="obj">The other object to compare to</param>
        /// <returns>if the users have the same information</returns>
        public override bool Equals(object obj)
        {
            User other = obj as User;
            if (other == null)
                return base.Equals(obj);

            if (this.FirstName == other.FirstName && this.LastName == other.LastName && this.Email == other.Email && this.Gender == other.Gender && this.Country == other.Country && this.City == other.City && this.Street == other.Street && this.PhoneNumber == other.PhoneNumber)
                return true;

            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        #endregion

        #region Error Handelning

        /// <summary>
        /// Check if a field is empty or null which raise error validation
        /// </summary>
        /// <param name="sender">The name of property that is validated</param>
        /// <param name="value">The value that is validated</param>
        private void CheckEmpty(string sender, string value)
        {
            bool valid = true;

            if (string.IsNullOrWhiteSpace(value))
                valid = false;

            if (!valid)
            {
                List<string> errors = new List<string>();
                errors.Add($"This field must have a value");
                SetErros(sender, errors);
            }
            else
            {
                ClearErrors(sender);
            }
        }

        private void SetErros(string propertyName, List<string> propertyErrors)
        {
            //Clear any existing error from the property
            errors.Remove(propertyName);

            //Add the list collection for the property
            errors.Add(propertyName, propertyErrors);

            //Raise the error-notification event
            if (ErrorChanged != null)
                ErrorChanged(this, new DataErrorsChangedEventArgs(propertyName));
        }

        private void ClearErrors(string propertyName)
        {
            //Remove the error list for the property
            errors.Remove(propertyName);

            if (ErrorChanged != null)
                ErrorChanged(this, new DataErrorsChangedEventArgs(propertyName));
        }

        public IEnumerable GetErrors(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                // Provide all the error collection for the requested property
                return (errors.Values);
            }
            else
            {
                //return if a singel field has an error
                if (errors.ContainsKey(propertyName))
                    return (errors[propertyName]);
                else
                    return null;
            }
        }
        #endregion

        #endregion

    }
}
