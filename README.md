# Demo_WCF_NetFramework (Service)
## Step 1:
Instal WCF template
## Step 2:
Create WCF Service Library project (As Adminator)
## Step 3:
Add Nuget Package:
EntityFrameWork
## Step 4:
Create folder "Model" and class "User", "MyDbContext"

User
```bash
    [DataContract]
    public class User
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Name { get; set; }

    }
```
MyDbContext
```bash
    public class MyDbContext: DbContext
    {
        public MyDbContext() : base("MyDbContext")
        {

        }

        public DbSet<User> Users { get; set; }


    }
```
## Step 5:
Add this piece of code below row 6:

```bash
	<connectionStrings>
		<add name="MyDbContext" connectionString="Data Source=LAPTOP-TR2UOAHI\SQLEXPRESS;Initial Catalog=Demo_WCF_NetFramework;Persist Security Info=True;User ID=sa; Password=123;TrustServerCertificate=True" providerName="System.Data.SqlClient"/>
	</connectionStrings>
```
Enter this command in Package Manager Console:
```bash
enable-migrations
```
Then:
```bash
add-migration Initial
```
Add this code to class "Configuration" to seed data:
```bash
context.Users.AddOrUpdate(
    new User { Id = 1, Name = "Thai" },
    new User { Id = 2, Name = "Lam" },
    new User { Id = 3, Name = "Phat" },
    new User { Id = 4, Name = "Tam" },
    new User { Id = 5, Name = "Sao" }
);
```

Enter command:
```bash
update-database
```

## Step 6:
Create "Repository" folder and "UserRepository" class.

```bash
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
            if(newUser != null)
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
```

## Step 7
Create "IUserService" interface and "UserService" class

IUserService
```bash
