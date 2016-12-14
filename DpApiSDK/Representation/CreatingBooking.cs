using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DpApiSDK.Representation
{
    public class CreatingBooking : BaseResponse
    {
        public DateTimeOffset StartAt { get; set; }
        public DateTimeOffset EndAt { get; set; }
        public int Duration { get; set; }
        public string BookingBy { get; set; }
        public string BookedAt { get; set; }
        public AddressService AddressService { get; set; }
    }
}
