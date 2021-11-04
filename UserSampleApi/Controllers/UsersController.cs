
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
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
    public class UsersController : ControllerBase
    {


        private readonly ILogger<UsersController> _logger;
        private readonly IRandomUserValidator _randomUserValidator;
        private readonly IConfiguration _configuration;
        private IHttpClientFactory _httpClient;
        private uint _numberOfUser = 50;
        private string _randomUserApiFilter = "?results={0}&inc=name,dob,id";
        private string _randomUserApiUrl ;


        public UsersController(ILogger<UsersController> logger, 
            IHttpClientFactory httpClient, 
            IRandomUserValidator randomUserValidator,
            IConfiguration Configuration)
        {
            _logger = logger;
            _httpClient = httpClient;
            _randomUserValidator = randomUserValidator;
            _configuration = Configuration;            
        }


        /// <summary>
        /// Gets the list of all Users.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        /// GET api/GetUsers
        ///     { 
        ///     
        ///         "id": 690780483,
        ///         "name": "Ben",
        ///         "surname": "Hicks",
        ///         "birthdate": "1945-08-18T18:10:35.164Z"
        ///     }
        /// </remarks>
        /// <param name="NumberOfUsers">Number of users you want to receive</param>
        /// <returns>The list of Users.</returns>
        /// <response code="200">Returns the list of Users</response>
        /// <response code="204">If there is not Users</response>     
        /// <response code="500">Internal error</response>     
        // GET: api/GetUsers
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [Produces("application/json")]
        [HttpGet]
        public async Task<IActionResult> GetUsers(uint NumberOfUsers = 50)
        {
            _logger.LogInformation($"Calling api{this.ControllerContext.RouteData.Values["controller"].ToString()}\\{this.ControllerContext.RouteData.Values["action"].ToString()} ");
            _numberOfUser = NumberOfUsers != 10 ? NumberOfUsers : _numberOfUser;
            _randomUserApiUrl = _configuration["RandomuserUrl"];
            _randomUserApiUrl += string.Format(_randomUserApiFilter, _numberOfUser);
            if (!Uri.IsWellFormedUriString(_randomUserApiUrl, UriKind.Absolute))//validating url format
            {
                _logger.LogError("MissingApiUrl");
                return Problem("Api is not available at momet");
            }
            return  await GetUsersFromRandomuser();
            
        }


        [NonAction]
        private async Task<IActionResult> GetUsersFromRandomuser()
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
