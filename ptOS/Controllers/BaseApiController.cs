using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ptOS.Core;

namespace ptOS.Controllers
{
    public class BaseApiController : ApiController
    {
        public ptOSContainer Context { get; set; }

        public BaseApiController()
        {
            Context = new ptOSContainer();
        }
    }
}
