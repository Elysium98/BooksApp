using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksApp.Services.Models
{
    public class UserLoginResponseModel: UserModel
    {
        public string Role { get; set; }
        public string Token { get; set; }
    }
}
