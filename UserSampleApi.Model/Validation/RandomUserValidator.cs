using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserSampleApi.Model
{

    /// <summary>
    /// Validate randomuser response
    /// </summary>
    public class RandomUserValidator : IRandomUserValidator
    {

        public RandomUserValidator(Ilogger)
        {

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
                name = !string.IsNullOrEmpty(rndUser?.name?.first);
                surname = !string.IsNullOrEmpty(rndUser?.name?.last);
                dob = rndUser?.dob?.date != null;
                if (id > 0 && name && surname && dob)
                {
                    userList.Add(
                        new User()
                        {
                            Id = id,
                            Name= rndUser.name.first,
                            Surname= rndUser.name.last,
                            Birthdate= rndUser.dob.date
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
        /// <returns>if the Result has valid data for compose the User </returns>
        IEnumerable<User> GetValidUsers(IEnumerable<Result> rndUsers);
    }


}
