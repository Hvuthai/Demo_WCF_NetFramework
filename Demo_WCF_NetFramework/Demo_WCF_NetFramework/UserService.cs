using Demo_WCF_NetFramework.Model;
using Demo_WCF_NetFramework.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Demo_WCF_NetFramework
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in both code and config file together.
    public class UserService : IUserService
    {
        UserRepository repository = new UserRepository();

        public void Add(User user)
        {
            repository.AddUser(user);
        }

        public void Delete(string Id)
        {
            if (int.TryParse(Id, out int userId))
            {
                repository.Delete(userId);
            }
            else
            {
                throw new ArgumentException("Invalid user ID format.");
            }
        }

        public void Edit(User user)
        {
            repository.Update(user);
        }

        public User GetUserById(string Id)
        {
            if (int.TryParse(Id, out int userId))
            {
                return repository.GetUserById(userId);
            }
            else
            {
                throw new ArgumentException("Invalid user ID format.");
            }

        }

        public List<User> GetUsers()
        {
            return repository.GetAll();
        }
    }
}
