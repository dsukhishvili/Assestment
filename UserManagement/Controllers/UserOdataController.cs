using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserManagement.Service.DAL;
using UserManagement.Service.Models;

namespace UserManagement.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class UserOdataController : ODataController
    {
        private UserContext _userContext { get; set; }
        public UserOdataController(UserContext context)
        {
            _userContext = context;
        }
        [ODataRoute("Users")]
        [EnableQuery(PageSize = 10, AllowedQueryOptions = Microsoft.AspNet.OData.Query.AllowedQueryOptions.All)]
        public IEnumerable<User> Get()
        {
            return this._userContext.Users;
        }
    }
}