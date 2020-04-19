using System;
namespace Tegetgram.Api.Models
{
    public class JWTSettings
    {
        public string SecretKey { get; set; }
        public string Issuer { get; set; }
    }
}
