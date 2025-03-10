using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceComparisonMVCAdmin.Models.Configuration
{
    public class JwtConfiguration
    {
        public const string Position = "JWT";
        public string? Audience { get; set; }
        public string? Issuer { get; set; }
        public string? Key { get; set; }
        public double AccessTokenLifetimeMin { get; set; }
        public double DefaultRefreshTokenLifetimeHours { get; set; }
        public double RememberMeRefreshTokenLifetimeHours { get; set; }
    }
}
