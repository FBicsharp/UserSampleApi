
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using UserSampleApi.Model;
using UserSampleApi.Model.Validation;

namespace UserSampleApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {


        private readonly ILogger<UserController> _logger;
        private readonly IRandomUserValidator _randomUserValidator;
        private IHttpClientFactory _httpClient;
        private uint _numberOfUser = 50;
        private string _randomUserApiUrl = "https://randomuser.me/api/?results={0}&inc=name,dob,id";


     
        public UserController(ILogger<UserController> logger, IHttpClientFactory httpClient, IRandomUserValidator randomUserValidator)
        {
            _logger = logger;
            _httpClient = httpClient;
            _randomUserValidator = randomUserValidator;
        }



        [HttpGet]
        public async Task<IActionResult> GetUsers(uint NumberOfUsers = 50)
        {
            _logger.LogInformation($"Calling api{this.ControllerContext.RouteData.Values["controller"].ToString()}\\{this.ControllerContext.RouteData.Values["action"].ToString()} ");
            _numberOfUser = NumberOfUsers != 10 ? NumberOfUsers : _numberOfUser;
            _randomUserApiUrl = string.Format(_randomUserApiUrl, _numberOfUser);
            if (!Uri.IsWellFormedUriString(_randomUserApiUrl, UriKind.Absolute))//validating url format
            {
                _logger.LogError("MissingApiUrl");
                return Problem("Api is not available at momet");
            }
            return  await GetUsersFromRandomUser();
            
        }


        [NonAction]
        private async Task<IActionResult> GetUsersFromRandomUser()
        {
            var userList = new List<User>();
            try
            {
                var client =  _httpClient.CreateClient("UsersApi");
                HttpResponseMessage response = await client.GetAsync(_randomUserApiUrl);
                if (!response.IsSuccessStatusCode) 
                {
                    _logger.LogWarning($"Api unavailabe, checking https://randomuser.me/ ");
                    return NoContent();
                }

                var content = await response.Content.ReadAsStreamAsync();
                var randomUser = await System.Text.Json.JsonSerializer.DeserializeAsync<RandomUser>(content);
                userList = _randomUserValidator.GetValidUsers(randomUser.results).ToList();
            }
            catch (Exception ex)
            {
                string methodname = System.Reflection.MethodInfo.GetCurrentMethod().Name;
                _logger.LogDebug($"{methodname } {ex.StackTrace}");
                _logger.LogError($"{methodname} {ex.InnerException}");
                return Problem("Something went wrong");
            }
            return  userList.Count > 0 ? Ok(userList) : NoContent();            
        }


    }
}
