using Demo_WCF_NetFramework.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace Demo_WCF_NetFramework
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IUserService
    {
        [OperationContract]
        [WebGet(UriTemplate = "User", ResponseFormat = WebMessageFormat.Json)]
        List<User> GetUsers();

        [OperationContract]
        [WebGet(UriTemplate="User/{Id}",ResponseFormat=WebMessageFormat.Json)]
        User GetUserById(string Id);

        [OperationContract]
        [WebInvoke(UriTemplate="User/Add",Method = "POST", RequestFormat = WebMessageFormat.Json)]
        void Add(User user);

        [OperationContract]
        [WebInvoke(UriTemplate = "User/Edit", Method = "PUT", RequestFormat = WebMessageFormat.Json)]
        void Edit(User user);

        [OperationContract]
        [WebInvoke(UriTemplate = "User/Delete/{Id}", Method = "DELETE")]
        void Delete(string Id);
    }

}
