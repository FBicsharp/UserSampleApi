
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using UserSampleApi.Model;

namespace UserSampleApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {


        private readonly ILogger<UserController> _logger;
        private readonly string _controllerName;
        private HttpClient _client;
        private uint _numberOfUser = 10;
        private string _randomUserApiUrl = $"https://randomuser.me/api/?results={0}&inc=name,dob,id";

        public UserController(ILogger<UserController> logger)
        {
            _logger = logger;
            //_controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            _controllerName = "User";
            _logger.LogInformation($"Incoming call for{_controllerName} ");
            _client = new HttpClient();
        }



        [HttpGet]        
        public async Task<IEnumerable<User>> GetUsers(uint NumberOfUser =10)
        {
            _logger.LogInformation($"Calling api{_controllerName}\\{this.ControllerContext.RouteData.Values["action"].ToString()} ");
            _numberOfUser = NumberOfUser != 10? NumberOfUser : _numberOfUser;
            var listUser = await GetUsersFromRandomUser();
            return listUser;
        }


        [NonAction]
        private async Task<IEnumerable<User>> GetUsersFromRandomUser( )
        {
            var userList = new List<User>();
            HttpResponseMessage response;
            try
            {
                _randomUserApiUrl = string.Format(_randomUserApiUrl, _numberOfUser);
                response = await _client.GetAsync(_randomUserApiUrl);

                if (!response.IsSuccessStatusCode)
                {
                    return userList;
                }
                var content = await response.Content.ReadAsStreamAsync();                
                var randomUser =  await System.Text.Json.JsonSerializer.DeserializeAsync<RandomUser>(content);
                var rndUserValidator = new RandomUserValidator();//TO using the  DI
                userList = rndUserValidator.GetValidUsers(randomUser.results).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError($"{_controllerName} {ex.InnerException}");
            }

            return userList;
        }


    }
}
