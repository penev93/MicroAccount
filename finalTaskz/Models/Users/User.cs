using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace finalTaskz.Models.Users
{
    public class User
    {
      /*  [Required(ErrorMessage = "Username must contain only letters and length 3-16")]
        [RegularExpression(@"^[a-zA-Z0-9_-]{3,16}")]*/
        public string IdUser { get; set; }
        public string Username { get; set; }
/*        [Required(ErrorMessage = "Password must have length 3-16")]
        [RegularExpression(@"^[a-zA-Z]\w{3,14}$")]*/
        public string Password { get; set; }
        /*[Required(ErrorMessage = "First name must contain only letters and length 3-25")]
        [RegularExpression(@"([a-zA-Z]{3,25}\s*)+")]*/
        public string Firstname { get; set; }
        /*[Required(ErrorMessage = "Last name must contain only letters and length 3-25")]
        [RegularExpression(@"([a-zA-Z]{3,25}\s*)+")]*/
        public string Lastname { get; set; }
        /*[Required(ErrorMessage = "Please enter valid email")]
        [RegularExpression(@".*@.*")]*/
        public string Email { get; set; }
    }
}