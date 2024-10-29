using Demo_WCF2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Demo_WCF2.Repository
{
    public class UserRepository
    {
        public MyDbContext Context { get; set; }

        public UserRepository()
        {
            Context = new MyDbContext();
        }

        public List<User> GetAll()
        {
            return Context.Users.ToList();
        }

        public void AddUser(User user)
        {
            Context.Users.Add(user);
            Context.SaveChanges();
        }

        public void Update(User user)
        {
            var newUser = Context.Users.Find(user.Id);
            if (newUser != null)
            {

                newUser.Name = user.Name;
                Context.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            var user = Context.Users.Find(id);
            if (user != null)
            {

                Context.Users.Remove(user);
                Context.SaveChanges();
            }

        }

        public User GetUserById(int id)
        {
            var user = Context.Users.Find(id);
            return user;

        }
    }
}