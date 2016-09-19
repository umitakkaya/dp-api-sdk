using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DpApiSDK.Representation
{
    public class BaseResponse : AuthenticationErrorResponse
    {
        public dynamic Errors { get; set; }
        public string Message { get; set; }
    }
}
