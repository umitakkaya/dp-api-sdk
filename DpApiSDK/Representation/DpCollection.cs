using RestSharp.Deserializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DpApiSDK.Representation
{
    public class DpCollection<T> : BaseResponse
    {
        [DeserializeAs(Name = "_items")]
        public List<T> Items { get; set; }
    }
}
