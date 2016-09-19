using RestSharp.Deserializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DpApiSDK.Representation
{
    public class BookingRequest
    {
        public string AddressServiceId { get; set; }
        [DeserializeAs(Name="IsReturning")]
        public bool IsReturningPatient { get; set; }
        public Patient Patient { get; set; }
    }
}
