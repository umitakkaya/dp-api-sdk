using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DpApiSDK.Representation
{
    public class CalendarBreak : BaseResponse
    {
        public string Id { get; set; }
        public DateTimeOffset Since { get; set; }
        public DateTimeOffset Till { get; set; }
    }
}
