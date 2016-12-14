using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DpApiSDK.Representation
{
    public class CancelingBooking : BaseResponse
    {
        public string Id { get; set; }
        public DateTimeOffset StartAt { get; set; }
        public DateTimeOffset EndAt { get; set; }
        public int Duration { get; set; }
        public string BookedBy { get; set; }
        public string BookedAt { get; set; }
        public string CancelingBy { get; set; }
        public string CancelingAt { get; set; }
        public AddressService AddressService { get; set; }
    }
}
