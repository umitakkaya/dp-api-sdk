using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DpApiSDK.Representation
{
    public class NotificationData
    {
        public DpFacility Facility { get; set; }
        public DpAddress Address { get; set; }
        public DpDoctor Doctor { get; set; }
        public Booking VisitBooking { get; set; }
        public RealtimeBooking VisitBookingRequest { get; set; }
        public Booking OldVisitBooking { get; set; }
        public Booking NewVisitBooking { get; set; }
    }
}
