using AppData;
using FileHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DataProvider
{
    public static class Provider
    {
        private static FileHelperEngine<UserEngine> _userEngine = new FileHelperEngine<UserEngine>();
        private static FileHelperAsyncEngine<UserEngine> _userEngineAsync = new FileHelperAsyncEngine<UserEngine>();

        /// <summary>
        /// Gets the user list from a csv file
        /// </summary>
        /// <param name="path">The path to the file</param>
        /// <returns>list of users</returns>  
        public static List<User> GetUsers(string path)
        {
            try
            {
                var records = _userEngine.ReadFile(path);
                List<User> userlist = new List<User>();
                UsersFactory factory = new UsersFactory();

                foreach (UserEngine user in records)
                {
                    userlist.Add(factory.CreateUser(user.ID, user.FirstName, user.LastName, user.Email, user.Gender, user.Country, user.City, user.Street, user.PhoneNumber));
                }

                return userlist;
            }
            catch (ArgumentException e)
            {
                throw new ArgumentException(e.Message);
            }

        }

        /// <summary>
        /// Reads the users from the file async
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static async Task<List<User>> GetUsersAsync(string path)
        {

            using (_userEngineAsync.BeginReadFile(path))
            {
                List<Task<User>> tasks = new List<Task<User>>();
                UsersFactory factory = new UsersFactory();

                foreach (var i in _userEngineAsync)
                {
                    tasks.Add(Task.Run(() => factory.CreateUser(i.ID, i.FirstName, i.LastName, i.Email, i.Gender, i.Country, i.City, i.Street, i.PhoneNumber)));
                }

                _userEngineAsync.Close();
                return new List<User>(await Task.WhenAll(tasks));
            }
        }

        public static async Task<List<User>> GetUsersAsyncV2(string filePath, IProgress<UsersProgressReportModel> progress, CancellationToken token)
        {
            UsersFactory factory = new UsersFactory();
            List<User> users = new List<User>();
            UsersProgressReportModel progressReport = new UsersProgressReportModel();
            List<UserEngine> usersRows;
            object locker = new object(); //to prevent lockdown of the list when adding users
            ParallelOptions parallelOptions = new ParallelOptions(); //To handel the cancelation and other problems
            parallelOptions.CancellationToken = token;
            parallelOptions.MaxDegreeOfParallelism = System.Environment.ProcessorCount;

            using (_userEngineAsync.BeginReadFile(filePath))
            {
                usersRows = new List<UserEngine>(_userEngineAsync);

                progressReport.OverAllUsersCount = usersRows.Count;
                try
                {
                    await Task.Run(() => Parallel.ForEach<UserEngine>(usersRows, parallelOptions, (userRow) =>
                    {
                        parallelOptions.CancellationToken.ThrowIfCancellationRequested();
                        lock (locker)
                        {
                            users.Add(new User(userRow.ID, userRow.FirstName, userRow.LastName, userRow.Email,
                            userRow.Gender, userRow.Country, userRow.City, userRow.Street, userRow.PhoneNumber));
                        }

                        progressReport.CompleatedUsersList = users;
                        progressReport.UsersPrecentage = (users.Count * 100) / progressReport.OverAllUsersCount;
                        progress.Report(progressReport);
                        //Thread.Sleep(100); // For debugging long lists
                    }));
                }
                catch (OperationCanceledException)
                {
                    throw new OperationCanceledException("Getting Users operation was canceled");
                }
                finally
                {
                    _userEngineAsync.Close();
                }

                return users;
            }
        }

        public static void WriteUsersFile(string path, List<User> users)
        {
            var temp = new List<UserEngine>();

            foreach (var item in users)
            {
                temp.Add(new UserEngine(item.ID, item.FirstName, item.LastName, item.Email, item.Gender, item.Country, item.City, item.Street, item.PhoneNumber));
            }
            _userEngine.HeaderText = _userEngine.GetFileHeader();
            _userEngine.WriteFile(path, temp);

        }
    }
}
