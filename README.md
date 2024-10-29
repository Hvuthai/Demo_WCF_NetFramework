# Demo_WCF_NetFramework (Service)
## Step 1:
Instal WCF template
## Step 2:
Create WCF Application project (As Adminator)
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
Create "UserService" service

IUserService
```bash
[ServiceContract]
public interface IUserService
{
    [OperationContract]
    [WebGet(UriTemplate = "User", ResponseFormat = WebMessageFormat.Json)]
    List<User> GetUsers();

    [OperationContract]
    [WebGet(UriTemplate = "User/{Id}", ResponseFormat = WebMessageFormat.Json)]
    User GetUserById(string Id);

    [OperationContract]
    [WebInvoke(UriTemplate = "User/Add", Method = "POST", RequestFormat = WebMessageFormat.Json)]
    void Add(User user);

    [OperationContract]
    [WebInvoke(UriTemplate = "User/Edit", Method = "PUT", RequestFormat = WebMessageFormat.Json)]
    void Edit(User user);

    [OperationContract]
    [WebInvoke(UriTemplate = "User/Delete/{Id}", Method = "DELETE")]
    void Delete(string Id);
}
```
UserService
```bash
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
```

## Step 8
Change the the Web.config content
```bash
<system.serviceModel>
	<services>
		<service name="Demo_WCF2.UserService">
			<!-- Service Endpoints -->
			<!-- Unless fully qualified, address is relative to base address supplied above -->
			<endpoint address="" behaviorConfiguration="restbehaviour" binding="webHttpBinding" contract="Demo_WCF2.IUserService">
				<!-- 
          Upon deployment, the following identity element should be removed or replaced to reflect the 
          identity under which the deployed service runs.  If removed, WCF will infer an appropriate identity 
          automatically.
      -->
			</endpoint>
			<!-- Metadata Endpoints -->
			<!-- The Metadata Exchange endpoint is used by the service to describe itself to clients. -->
			<!-- This endpoint does not use a secure binding and should be secured or removed before deployment -->
			<endpoint address="mex" binding="mexHttpBinding" contract="IMetadataExchange" />
		</service>
	</services>
	<behaviors>
		<endpointBehaviors>
			<behavior name="restbehaviour">
				<webHttp />
			</behavior>
		</endpointBehaviors>
		<serviceBehaviors>
			<behavior>
				<!-- To avoid disclosing metadata information, 
      set the values below to false before deployment -->
				<serviceMetadata httpGetEnabled="true" httpsGetEnabled="true" />
				<!-- To receive exception details in faults for debugging purposes, 
      set the value below to true.  Set to false before deployment 
      to avoid disclosing exception information -->
				<serviceDebug includeExceptionDetailInFaults="true" />
			</behavior>
		</serviceBehaviors>
	</behaviors>
	<protocolMapping>
		<add scheme="https" binding="basicHttpBinding" />
	</protocolMapping>
	<serviceHostingEnvironment aspNetCompatibilityEnabled="false" multipleSiteBindingsEnabled="true">
	</serviceHostingEnvironment>
</system.serviceModel>
```
