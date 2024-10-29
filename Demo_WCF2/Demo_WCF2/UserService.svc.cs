using Demo_WCF2.Models;
using Demo_WCF2.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Demo_WCF2
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "UserService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select UserService.svc or UserService.svc.cs at the Solution Explorer and start debugging.
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
