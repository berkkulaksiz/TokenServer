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
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public TokenController(
            IConfiguration configuration
        )
        {
            _configuration = configuration;
        }

        [HttpGet("Auth")]
        public async Task<IActionResult> Get()
        {
            //var secret = _configuration.GetSection("Secret").Value;
            var secret = _configuration["Secret"];
            var clientId = _configuration["ClientId"];
            var redirectionUrl = _configuration["RedirectionURL"];
            var scope = _configuration["Scope"];
            var baseUrl = $"https://{Request.Host}/api/token";

            redirectionUrl = string.Format(redirectionUrl, clientId, baseUrl, scope);

            //return Ok($"{redirectionUrl}");
            return Redirect(redirectionUrl);
        }

        [HttpGet]
        public async Task<IActionResult> GetCode(string code)
        {
            var result = string.Empty;
            var tokenUrl = _configuration["TokenURL"];

            var secret = _configuration["Secret"];
            var clientId = _configuration["ClientId"];

            var secretClientIdBase64 = this.Base64Encode($"{clientId}:{secret}");

            var baseUrl = $"https://{Request.Host}/api/token";
            // var values = new Dictionary<string, string>
            // {
            //     { "grant_type", "authorization_code" },
            //     { "code", code },
            //     { "redirect_uri", baseUrl }
            // };

            var requestContent = $"grant_type=authorization_code&code={code}&redirect_uri={baseUrl}";
            var request = new HttpRequestMessage(HttpMethod.Post, tokenUrl);
            request.Headers.Add("Authorization", $"Basic {secretClientIdBase64}");
            request.Content = new StringContent(requestContent, Encoding.UTF8, "application/x-www-form-urlencoded");
            using (var response = await new HttpClient().SendAsync(request))
            {
                result = await response.Content.ReadAsStringAsync();
            }

            return Ok(result);
        }

        private string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
    }
}
