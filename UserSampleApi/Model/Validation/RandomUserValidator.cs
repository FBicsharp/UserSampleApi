using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserSampleApi.Model.Validation
{

    /// <summary>
    /// Validate randomuser response
    /// </summary>
    public class RandomUserValidator : IRandomUserValidator
    {
        private readonly ILogger<RandomUserValidator> _logger;

        public RandomUserValidator(ILogger<RandomUserValidator> logger)
        {
            _logger = logger;
        }


        public IEnumerable<User> GetValidUsers(IEnumerable<Result> rndUsers)
        {
            var userList = new List<User>();
            long id;
            bool name, surname, dob;
            string idFromUserAsString = string.Empty;


            foreach (var rndUser in rndUsers)
            {
                id = 0;
                idFromUserAsString = rndUser?.id?.value?.ToString();
                long.TryParse(idFromUserAsString, out id);
                if (id == 0) 
                {//generate automatically an id for avoid empty list 
                    var rnd = new Random();
                    id =   rnd.Next(1, int.MaxValue);
                }
                name = !string.IsNullOrEmpty(rndUser?.name?.first);
                surname = !string.IsNullOrEmpty(rndUser?.name?.last);
                dob = rndUser?.dob?.date != null;
                if (id > 0 && name && surname && dob)
                {
                    userList.Add(
                        new User()
                        {
                            Id = id,
                            Name = rndUser.name.first,
                            Surname = rndUser.name.last,
                            Birthdate = rndUser.dob.date
                        }
                    );
                }
            }

            return userList;
        }

    }

    public interface IRandomUserValidator
    {
        /// <summary>
        /// Check Result properties values
        /// </summary>
        /// <param name="rndUser"></param>
        /// <returns>User list</returns>
        IEnumerable<User> GetValidUsers(IEnumerable<Result> rndUsers);
    }


}
