using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace UserSampleApi.Model
{
    public class User
    {

        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("surname")]
        public string Surname { get; set; }

        [JsonPropertyName("birthdate")]
        public DateTime Birthdate { get; set; }

        public User():this(0,"","",DateTime.UtcNow) { } //in net6 there is the possibility to use DateOnly

        public User(long id, string name, string surname, DateTime birthDate)
        {
            Id = id;
            Name = name;
            Surname = surname;
            Birthdate = birthDate;
        }
    }

}



/// <summary>
/// Read a list of users
///// </summary>
///// <remarks>
///// </remarks>
///// <param name="NumOfUser">Number of users you would like to receive.</param>
////[ProducesResponseType(typeof(IEnumerable<User>), (int)HttpStatusCode.OK)]
//[HttpGet, Route("{NumOfUser}", Name = "GetUsers")]
