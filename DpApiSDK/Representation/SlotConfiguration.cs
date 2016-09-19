using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DpApiSDK.Representation
{
    public class SlotConfiguration : Slot
    {
        public DateTimeOffset End { get; set; }


        private List<SlotAddressService> _addressServices = new List<SlotAddressService>();
        public List<SlotAddressService> AddressServices
        {
            get { return _addressServices; }
            set { _addressServices = value; }
        }

    }
}
