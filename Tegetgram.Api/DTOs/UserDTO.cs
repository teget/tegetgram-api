﻿using System;
using System.ComponentModel.DataAnnotations;

namespace Tegetgram.Api.DTOs
{
    public class UserDTO
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
