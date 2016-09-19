using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DpApiSDK.Representation
{
    public class AddressService : BaseResponse
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool IsPriceFrom { get; set; }
        public int? Price { get; set; }
        public int? duration { get; set; }
    }
}
