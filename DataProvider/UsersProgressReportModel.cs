using AppData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataProvider
{
    /// <summary>
    /// Used as the model to test the progress of async methods in Provider class
    /// </summary>
    public class UsersProgressReportModel
    {
        public List<User> CompleatedUsersList { get; set; } = new List<User>();
        public int OverAllUsersCount { get; set; } = 0;
        public int UsersPrecentage { get; set; } = 0;
    }
}
