using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sofos2ToDatawarehouse.Domain.DTOs.SIDCToken
{
    public class SIDCTokenResponse : SIDCBaseResponse
    {
        public SIDCBaseResponseData Data { get; set; }
    }

    public class SIDCBaseResponseData
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public bool IsVerified { get; set; }
        public string JwToken { get; set; }
    }
}
