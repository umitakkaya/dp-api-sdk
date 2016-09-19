using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DpApiSDK.Representation
{
    public class AuthorizationToken : BaseResponse
    {
        public string AccessToken { get; set; }
        public int ExpiresIn { get; set; }
        public string TokenType { get; set; }
        public string Scope { get; set; }

        public virtual DateTime ExpiresAt
        {
            get
            {
                return DateTime.Now.AddSeconds(ExpiresIn);
            }
        }

    }
}
