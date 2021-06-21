using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Task_API.Validation
{
  public interface IAccountValidation
    {
         bool ValidatePassword(string password);
         bool isEmailValid(string Email);
        Task<bool> CheckEmailExist(string Email);
        Task<bool> CheckUserNameExist(string userName);


        }
}
