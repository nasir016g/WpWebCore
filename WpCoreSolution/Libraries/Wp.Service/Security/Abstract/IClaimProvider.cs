using System.Collections.Generic;

namespace Wp.Service.Security
{
    public interface IClaimProvider
    {
        IEnumerable<ClaimRecord> GetClaims();
        IEnumerable<DefaultClaimRecord> GetDefaultClaims();
    }
}