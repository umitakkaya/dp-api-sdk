using RestSharp.Deserializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DpApiSDK.Representation
{
    public class ServiceItem : BaseResponse
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
