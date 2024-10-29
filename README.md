# Demo_WCF_NetFramework (Service)
## Step 1:
Instal WCF template (Visual Studio Installer -> Modify -> Invidual Component -> Check the Windows Communication Foundation -> click modify )
## Step 2:
Create WCF Service Application project (As Adminator)
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
Note: Change connectionString to your connnectionStringconnnectionString
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
note: change the servicename and contract to match your namespace
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
# Demo_WCF_NetFramework_Client (Client)
## Step 1: Create New MVC Project (Web Application .Net Framework)
## Step 2: Create "User" model

```bash
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
```
## Step 3: Create "UserController" class

```bash
note: Change the CustomerApiUrl port to match your url (Run the server project to get it)
public class UserController : Controller
{
    private readonly HttpClient Client = new HttpClient();
    private string CustomerApiUrl = "http://localhost:52041/UserService.svc/User";
    private MediaTypeWithQualityHeaderValue contentType = new MediaTypeWithQualityHeaderValue("application/json");


    // GET: User
    public async Task<ActionResult> Index()
    {
        Client.DefaultRequestHeaders.Accept.Add(contentType);
        HttpResponseMessage response = await Client.GetAsync(CustomerApiUrl);
        string strData = await response.Content.ReadAsStringAsync();

        List<User> users = JsonConvert.DeserializeObject<List<User>>(strData);
        return View(users);
    }

    // GET: CustomerController/Details/5
    public async Task<ActionResult> Details(int Id)
    {
        User customer = await GetUserById(Id);
        return View(customer);
    }

    // GET: CustomerController/Create
    public ActionResult Create()
    {
        return View();
    }

    // POST: CustomerController/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Create(string Name)
    {
        if (ModelState.IsValid)
        {
            var user = new User
            {
                Name = Name
            };

            //Send Create request
            HttpRequestMessage request = new HttpRequestMessage();
            request.Headers.Add("Accept", "application/json");
            request.RequestUri = new Uri(CustomerApiUrl + "/Add");
            request.Content = new StringContent(
                Newtonsoft.Json.JsonConvert.SerializeObject(user),
                Encoding.UTF8,
                "application/json");
            request.Method = HttpMethod.Post;
            HttpResponseMessage apiResponse = await Client.SendAsync(request);

            return RedirectToAction("Index");
        }
        return View();
    }

    // GET: CustomerController/Edit/5
    public async Task<ActionResult> Edit(int Id)
    {
        User user = await GetUserById(Id);
        return View(user);
    }

    // POST: CustomerController/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Edit(int Id, string Name)
    {
        var user = new User
        {
            Id = Id,
            Name = Name
        };

        if (ModelState.IsValid)
        {


            //Send update request
            HttpRequestMessage request = new HttpRequestMessage();
            request.Headers.Add("Accept", "application/json");
            request.RequestUri = new Uri(CustomerApiUrl + "/Edit");
            request.Content = new StringContent(
                JsonConvert.SerializeObject(user),
                Encoding.UTF8,
                "application/json");
            request.Method = HttpMethod.Put;
            HttpResponseMessage apiResponse = await Client.SendAsync(request);
            return RedirectToAction("Index");
        }
        return View(user);
    }


    public async Task<ActionResult> Delete(int Id)
    {


        try
        {
            User user = await GetUserById(Id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }
        catch (Exception ex)
        {
            return View("Error", ex);
        }
    }

    [HttpPost]
    public async Task<ActionResult> DeleteConfirm(int Id)
    {
        try
        {
            if (Id == 0)
            {
                return HttpNotFound();
            }


            HttpRequestMessage request = new HttpRequestMessage();
            request.Headers.Add("Accept", "application/json");
            request.RequestUri = new Uri(CustomerApiUrl + $"/Delete/{Id}");
            request.Method = HttpMethod.Delete;
            HttpResponseMessage apiResponse = await Client.SendAsync(request);
            return RedirectToAction("Index");

        }
        catch (Exception ex)
        {

            return View("Error", ex);
        }
    }

    private async Task<User> GetUserById(int Id)
    {
        User user = null;
        HttpResponseMessage response = await Client.GetAsync(CustomerApiUrl + $"/{Id}");
        string strData = await response.Content.ReadAsStringAsync();

        user = JsonConvert.DeserializeObject<User>(strData);

        return user;
    }

}
```
## Step 4: Add View
Note: Change the model match your namespace
Create

```bash
@model _Demo_WCF2_Client.Models.User

@{
    ViewBag.Title = "Create";
}

<h2>Create</h2>


@using (Html.BeginForm()) 
{
    @Html.AntiForgeryToken()
    
    <div class="form-horizontal">
        <h4>User</h4>
        <hr />
        @Html.ValidationSummary(true, "", new { @class = "text-danger" })
        <div class="form-group">
            @Html.LabelFor(model => model.Name, htmlAttributes: new { @class = "control-label col-md-2" })
            <div class="col-md-10">
                @Html.EditorFor(model => model.Name, new { htmlAttributes = new { @class = "form-control" } })
                @Html.ValidationMessageFor(model => model.Name, "", new { @class = "text-danger" })
            </div>
        </div>

        <div class="form-group">
            <div class="col-md-offset-2 col-md-10">
                <input type="submit" value="Create" class="btn btn-default" />
            </div>
        </div>
    </div>
}

<div>
    @Html.ActionLink("Back to List", "Index")
</div>

@section Scripts {
    @Scripts.Render("~/bundles/jqueryval")
}
```

 Details

```bash
@model _Demo_WCF2_Client.Models.User

@{
    ViewBag.Title = "Details";
}

<h2>Details</h2>

<div>
    <h4>User</h4>
    <hr />
    <dl class="dl-horizontal">
        <dt>
            @Html.DisplayNameFor(model => model.Name)
        </dt>

        <dd>
            @Html.DisplayFor(model => model.Name)
        </dd>

    </dl>
</div>
<p>
    @Html.ActionLink("Edit", "Edit", new { id = Model.Id }) |
    @Html.ActionLink("Back to List", "Index")
</p>
```

Index
```bash
@model IEnumerable<_Demo_WCF2_Client.Models.User>

@{
    ViewBag.Title = "Index";
}

<h2>Index</h2>

<p>
    @Html.ActionLink("Create New", "Create")
</p>
<table class="table">
    <tr>
        <th>
            @Html.DisplayNameFor(model => model.Name)
        </th>
        <th></th>
    </tr>

@foreach (var item in Model) {
    <tr>
        <td>
            @Html.DisplayFor(modelItem => item.Name)
        </td>
        <td>
            @Html.ActionLink("Edit", "Edit", new { id=item.Id }) |
            @Html.ActionLink("Details", "Details", new { id=item.Id }) |
            @Html.ActionLink("Delete", "Delete", new { id=item.Id })
        </td>
    </tr>
}

</table>
```
Edit
```bash
@model _Demo_WCF2_Client.Models.User

@{
    ViewBag.Title = "Edit";
}

<h2>Edit</h2>


@using (Html.BeginForm())
{
    @Html.AntiForgeryToken()
    
    <div class="form-horizontal">
        <h4>User</h4>
        <hr />
        @Html.ValidationSummary(true, "", new { @class = "text-danger" })
        @Html.HiddenFor(model => model.Id)

        <div class="form-group">
            @Html.LabelFor(model => model.Name, htmlAttributes: new { @class = "control-label col-md-2" })
            <div class="col-md-10">
                @Html.EditorFor(model => model.Name, new { htmlAttributes = new { @class = "form-control" } })
                @Html.ValidationMessageFor(model => model.Name, "", new { @class = "text-danger" })
            </div>
        </div>

        <div class="form-group">
            <div class="col-md-offset-2 col-md-10">
                <input type="submit" value="Save" class="btn btn-default" />
            </div>
        </div>
    </div>
}

<div>
    @Html.ActionLink("Back to List", "Index")
</div>

@section Scripts {
    @Scripts.Render("~/bundles/jqueryval")
}
```

Delete
```bash
@model _Demo_WCF2_Client.Models.User

@{
    ViewBag.Title = "Delete";
}

<h2>Delete</h2>

<h3>Are you sure you want to delete this?</h3>
<div>
    <h4>User</h4>
    <hr />
    <dl class="dl-horizontal">
        <dt>
            @Html.DisplayNameFor(model => model.Name)
        </dt>

        <dd>
            @Html.DisplayFor(model => model.Name)
        </dd>

    </dl>

    @using (Html.BeginForm("DeleteConfirm", "User", FormMethod.Post))
    {
        @Html.AntiForgeryToken()
        <div class="form-actions no-color">
            @Html.HiddenFor(model => model.Id)
            <input type="submit" value="Delete" class="btn btn-default" /> |
            @Html.ActionLink("Back to List", "Index")
        </div>
    }
</div>
```
# Finish. Run server and client project to use.
