using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sofos2ToDatawarehouse.Domain.DTOs.SIDCToken
{
    public class SIDCTokenRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
