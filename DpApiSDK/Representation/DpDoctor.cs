using RestSharp.Deserializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DpApiSDK.Representation
{
    [DeserializeAs(Name = "Doctor")]
    public class DpDoctor : BaseResponse
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DpCollection<Specialization> Specializations { get; set; }

        public virtual string Fullname
        {
            get
            {
                return string.Concat(Name, " ", Surname);
            }
        }
    }
}
