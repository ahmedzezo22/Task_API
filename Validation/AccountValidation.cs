using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Task_API.Models;

namespace Task_API.Validation
{
    public class AccountValidation : IAccountValidation

    {
        private readonly UserManager<AppUser> _userManager;

        public AccountValidation(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<bool> CheckEmailExist(string Email)
        {
            var email = await _userManager.FindByEmailAsync(Email);
            if (email != null) { return false; }
            return true;
        }

        public async Task<bool> CheckUserNameExist(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName.ToLower());
            if (user != null) { return false; }
            return true;
        }

        public bool isEmailValid(string Email)
        {
            Regex reg = new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");
            if (!reg.IsMatch(Email))
            {
                return false;
            }
            return true;
        }

        public bool ValidatePassword(string password)
        {

            Regex reg = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[#$^+=!*()@%&]).{8,}$");
            if (!reg.IsMatch(password)) { return false; }
            return true;
        }
    }
}
