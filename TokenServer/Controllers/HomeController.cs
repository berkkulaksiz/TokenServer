using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Text;

namespace TokenServer.Controllers
{
    [Route("/")]
    public class HomeController : ControllerBase
    {
        public IActionResult Index()
        {
            return Redirect("index.html");
        }
    }
}
