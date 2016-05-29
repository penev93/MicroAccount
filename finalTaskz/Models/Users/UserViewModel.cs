using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace finalTaskz.Models.Users
{
    public class UserViewModel
    {
        public static Expression<Func<User, UserViewModel>> FromUsers
        {
            get
            {
                return user => new UserViewModel
                {
                    Username = user.Username,
                    Firstname = user.Firstname,
                    Lastname = user.Lastname,
                    Password=user.Password,
                    Email=user.Email,
                    IdUser = user.IdUser
                };
            }
        }
        public string IdUser { get; set; }
        public string Username { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        
    }
}