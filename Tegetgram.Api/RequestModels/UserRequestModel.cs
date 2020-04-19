using System;
using System.ComponentModel.DataAnnotations;

namespace Tegetgram.Api.RequestModels
{
    public class UserRequestModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
