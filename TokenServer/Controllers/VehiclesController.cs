using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace TokenServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehiclesController : ControllerBase
    {
        IConfiguration _configuration;

        public VehiclesController(
            IConfiguration configuration
            )
        {
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> GetVehicles()
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString();
            var result = string.Empty;
            var apiRoot = _configuration["ApiUrl"];
            var request = new HttpRequestMessage(HttpMethod.Get, $"{apiRoot}/vehicles");
            request.Headers.Add("Authorization", token);
            using (var response = await new HttpClient().SendAsync(request))
            {
                result = await response.Content.ReadAsStringAsync();
            }
            return Ok(result);
        }
    }
}