using RestSharp.Deserializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DpApiSDK.Representation
{
    [DeserializeAs(Name = "Address")]
    public class DpAddress : BaseResponse
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string PostCode { get; set; }
        public string Street { get; set; }
        public BookingExtraFields BookingExtraFields { get; set; }
    }
}
