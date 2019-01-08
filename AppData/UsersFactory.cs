using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData
{
    public class UsersFactory
    {
        //id	first_name	last_name	email	gender	country	city	street	phone_number
        public User CreateUser(int id, string fisrtName, string lastName, string email, string gender, string country, string city, string street, string phoneNumber)
        {
            return new User(id, fisrtName, lastName, email, gender, country, city, street, phoneNumber);
        }

        public User CreateUser(int id)
        {
            return new User(id);
        }

        public User CreateUser(User user)
        {
            return new User(user);
        }

    }
}
