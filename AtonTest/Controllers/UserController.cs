using AtonTest.Constants;
using AtonTest.Exceptions;
using AtonTest.Models;
using AtonTest.Services;
using Microsoft.AspNetCore.Mvc;

namespace AtonAssignment.Controllers
{

    [Route("[controller]/[action]")]
    public class UsersController : Controller
    {

        private readonly AtDbContext   _atDbContext;

        private readonly EntityService _entityService;

        public UsersController(AtDbContext context, EntityService service)
        {
            _atDbContext = context;
            _entityService = service;
        }


        [HttpPost]
        public IActionResult CreateUser(string login, string password, string newUserLogin, string newUserPassword, string newUserName, int newUserGender, DateTime newUserBirthday)
        {
            if (IsUserExist(login, password) is null)
            {
                return Unauthorized(TextConstants.IncorrectLoginOrPassword);
            }

            try
            {
                _entityService.CreateUser(newUserLogin, newUserPassword, newUserName, newUserGender, newUserBirthday, login);
            }
            catch (UserAlreadyExistsException ex)
            {
                return Conflict(ex.Message);
            }
            catch (ValidateLogPassNameException ex)
            {
                return Conflict(ex.Message);
            }

            return Ok("New user has been successfully created.");
        }


        [HttpPatch]
        public IActionResult ChangeUserName(string login, string password, string nameToChange)
        {
            User user;

            if ((user = IsUserExist(login, password)) is null)
            {
                return Unauthorized(TextConstants.IncorrectLoginOrPassword);
            }

            if (!string.IsNullOrEmpty(user.RevorkedBy))
            {
                return Conflict(TextConstants.UserIsAlreadyRevorked);
            }

            try
            {
                _entityService.ChangeUserName(login, nameToChange, login);
            }
            catch (ValidateLogPassNameException)
            {
                return Conflict("All characters except Latin letters and numbers in name field are prohibited");
            }

            return Ok("Name has been successfully changed.");
        }


        [HttpPatch]
        public IActionResult ChangeUserGender(string login, string password, int gender)
        {
            User user;

            if ((user = IsUserExist(login, password)) is null)
            {
                return Unauthorized(TextConstants.IncorrectLoginOrPassword);
            }

            if (!string.IsNullOrEmpty(user.RevorkedBy))
            {
                return Conflict(TextConstants.UserIsAlreadyRevorked);
            }

            _entityService.ChangeUserGender(login, gender, login);

            return Ok("Gender has been successfully changed.");
        }


        [HttpPatch]
        public IActionResult ChangeUserBirthday(string login, string password, DateTime birthday)
        {
            User user;

            if ((user = IsUserExist(login, password)) is null)
            {
                return Unauthorized(TextConstants.IncorrectLoginOrPassword);
            }

            if (!string.IsNullOrEmpty(user.RevorkedBy))
            {
                return Conflict(TextConstants.UserIsAlreadyRevorked);
            }

            _entityService.ChangeUserBirthday(login, birthday, login);

            return Ok("Birthday has been successfully changed.");
        }


        [HttpPatch]
        public IActionResult ChangeUserPassword(string login, string password, string newPassword)
        {
            User user;

            if ((user = IsUserExist(login, password)) is null)
            {
                return Unauthorized(TextConstants.IncorrectLoginOrPassword);
            }

            if (!string.IsNullOrEmpty(user.RevorkedBy))
            {
                return Conflict(TextConstants.UserIsAlreadyRevorked);
            }

            try
            {
                _entityService.ChangeUserPassword(login, newPassword, login);
            }
            catch (ValidateLogPassNameException)
            {
                return Conflict("All characters except Latin letters and numbers in password field are prohibited");
            }

            return Ok("Password has been successfully changed.");
        }


        [HttpPatch]
        public IActionResult ChangeUserLogin(string login, string password, string newLogin)
        {
            User user;

            if ((user = IsUserExist(login, password)) is null)
            {
                return Unauthorized(TextConstants.IncorrectLoginOrPassword);
            }

            if (!string.IsNullOrEmpty(user.RevorkedBy))
            {
                return Conflict(TextConstants.UserIsAlreadyRevorked);
            }

            try
            {
                _entityService.ChangeUserLogin(login, newLogin, newLogin);
            }
            catch (UserAlreadyExistsException ex)
            {
                return Conflict(ex.Message);
            }
            catch (ValidateLogPassNameException)
            {
                return Conflict("All characters except Latin letters and numbers in login field are prohibited");
            }

            return Ok("Login has been successfully changed.");
        }


        [HttpGet]
        public IActionResult GetUserByLoginAndPassword(string login, string password)
        {
            User user;

            if ((user = IsUserExist(login, password)) is null)
            {
                return Unauthorized(TextConstants.IncorrectLoginOrPassword);
            }

            if (!string.IsNullOrEmpty(user.RevorkedBy))
            {
                return Conflict(TextConstants.UserIsAlreadyRevorked);
            }

            return Json(user);
        }


        private User IsUserExist(string login, string password) => _atDbContext.Users.FirstOrDefault(u => u.Login == login && u.Password == password);

    }
}
