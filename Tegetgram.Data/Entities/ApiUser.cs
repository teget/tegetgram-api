using System;
using Microsoft.AspNetCore.Identity;

namespace Tegetgram.Data.Entities
{
public class ApiUser : IdentityUser
    {
        public ApiUser()
        {
        }

        public ApiUser(string userName) : base(userName)
        {
            this.TegetgramUser = new TegetgramUser();
        }

        public TegetgramUser TegetgramUser { get; set; }
    }
}
