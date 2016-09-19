using Newtonsoft.Json;
using RestSharp.Deserializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DpApiSDK.Representation
{
    public class Patient
    {
        /// <summary>
        /// National Identity Number:
        /// For Turkey: T.C. Kimlik No,
        /// For Poland: PESEL
        /// </summary>
        public string Nin { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public DateTimeOffset? BirthDate { get; set; }

        /// <summary>
        /// "m" stands for male, "f" stands for female
        /// </summary>
        /// <value>"m" stands for male, "f" stands for female</value>
        public string Gender { get; set; }

        [JsonIgnore]
        public virtual string Fullname
        {
            get
            {
                return string.Concat(Name, " ", Surname);
            }
        }
    }
}
