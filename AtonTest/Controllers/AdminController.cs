using AtonTest.Constants;
using AtonTest.Exceptions;
using AtonTest.Models;
using AtonTest.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace AtonTest.Controllers
{
    [Route("[controller]/[action]")]
    public class AdminController : Controller
    {
        private readonly AtDbContext _dbContext;
        private readonly EntityService _entityService;

        public AdminController(AtDbContext dbContext, EntityService entityService)
        {
            _dbContext = dbContext;
            _entityService = entityService;
        }

        [HttpPost]
        public IActionResult CreateUserAdmin(string login, string password, string newUserLogin, string newUserPassword, string newUserName, int newUserGender, DateTime newUserBirthday, bool isAdmin)
        {
            if(IsAdminExist(login,password) is null)
            {
                return Unauthorized(TextConstants.IncorrectLoginOrPassword);
            }

            try
            {
                _entityService.CreateUser(newUserLogin, newUserPassword, newUserName, newUserGender, newUserBirthday, login, isAdmin);
            }
            catch (UserAlreadyExistsException ex)
            {
                return Conflict(ex.Message);
            }
            catch (ValidateLogPassNameException ex)
            {
                return Conflict(ex.Message);
            }
            return Ok("User created successfuly");

        }


        [HttpPatch]
        public IActionResult ChangeUserNameAdmin(string login, string password, string userLogin, string nameToChange)
        {
            if (IsAdminExist(login, password) is null)
            {
                return Unauthorized(TextConstants.IncorrectLoginOrPassword);
            }

            try
            {
                _entityService.ChangeUserName(userLogin, nameToChange, login);}
            
            catch (ValidateLogPassNameException)
            {
                return Conflict("All characters except Latin letters and numbers in name field are prohibited");
            }

            return Ok("Name has been successfully changed.");
        }


        [HttpPatch]
        public IActionResult ChangeUserGenderAdmin(string login, string password, string userLogin, int gender)
        {
            if (IsAdminExist(login, password) is null)
            {
                return Unauthorized(TextConstants.IncorrectLoginOrPassword);
            }

            _entityService.ChangeUserGender(userLogin, gender, login);

            return Ok("Gender has been successfully changed.");
        }


        [HttpPatch]
        public IActionResult ChangeUserBirthdayAdmin(string login, string password, string userLogin, DateTime birthday)
        {
            if (IsAdminExist(login, password) is null)
            {
                return Unauthorized(TextConstants.IncorrectLoginOrPassword);
            }

            _entityService.ChangeUserBirthday(userLogin, birthday, login);

            return Ok("Birthday has been successfully changed.");
        }


        [HttpPatch]
        public IActionResult ChangeUserPasswordAdmin(string login, string password, string userLogin, string newPassword)
        {
            if (IsAdminExist(login, password) is null)
            {
                return Unauthorized(TextConstants.IncorrectLoginOrPassword);
            }

            try
            {
                _entityService.ChangeUserPassword(userLogin, newPassword, login);
            }
            catch (ValidateLogPassNameException)
            {
                return Conflict("All characters except Latin letters and numbers in password field are prohibited");
            }

            return Ok("Password has been successfully changed.");
        }


        [HttpPatch]
        public IActionResult ChangeUserLoginAdmin(string login, string password, string userLogin, string newLogin)
        {
            if (IsAdminExist(login, password) is null)
            {
                return Unauthorized(TextConstants.IncorrectLoginOrPassword);
            }

            try
            {
                _entityService.ChangeUserLogin(userLogin, newLogin, login);
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
        public IActionResult GetAllActiveUsers(string login, string password)
        {
            if (IsAdminExist(login, password) is null)
            {
                return Unauthorized(TextConstants.IncorrectLoginOrPassword);
            }

            return Json(_dbContext.Users.Where(u => string.IsNullOrEmpty(u.RevorkedBy)).OrderBy(u => u.CreateOn).ToList());
        }


        [HttpGet]
        public IActionResult GetUserByLogin(string login, string password, string loginToSearch)
        {
            if (IsAdminExist(login, password) is null)
            {
                return Unauthorized(TextConstants.IncorrectLoginOrPassword);
            }

            var searchedUser = _dbContext.Users.FirstOrDefault(u => u.Login == loginToSearch);

            if (searchedUser is null)
            {
                return NotFound("User with this login is not founded.");
            }

            var activeStatus = string.IsNullOrEmpty(searchedUser.RevorkedBy) ? "Active" : "Revoked";

            return Json(new { searchedUser.Name, searchedUser.Gender, searchedUser.Birthday, activeStatus });
        }


        [HttpGet]
        public IActionResult GetUserOlderThan(string login, string password, int years)
        {
            if (IsAdminExist(login, password) is null)
            {
                return Unauthorized(TextConstants.IncorrectLoginOrPassword);
            }

            var yearFilter = DateTime.UtcNow.Year - years - 1;
            var currentDate = DateTime.UtcNow;

            var filteredUsers = _dbContext.Users.Where(u => yearFilter > u.Birthday.Value.Year
                || (yearFilter == u.Birthday.Value.Year && u.Birthday.Value.Month < currentDate.Month)
                || (yearFilter == u.Birthday.Value.Year && u.Birthday.Value.Month == currentDate.Month && currentDate.Day > u.Birthday.Value.Day));

            if (filteredUsers is null)
            {
                return NotFound($"Users older than {years} is not found.");
            }

            return Json(filteredUsers);
        }


        [HttpDelete]
        public IActionResult DeleteUserByLoginHard(string login, string password, string loginToDelete)
        {
            if (IsAdminExist(login, password) is null)
            {
                return Unauthorized(TextConstants.IncorrectLoginOrPassword);
            }

            var user = _dbContext.Users.FirstOrDefault(u => u.Login == loginToDelete);

            if (user is null)
            {
                return NotFound($"User with login:{loginToDelete} is not found.");
            }

            _dbContext.Users.Remove(user);
            _dbContext.SaveChanges();

            return Ok("User has been successfully deleted.");
        }


        [HttpDelete]
        public IActionResult DeleteUserByLoginSoft(string login, string password, string loginToDelete)
        {
            if (IsAdminExist(login, password) is null)
            {
                return Unauthorized(TextConstants.IncorrectLoginOrPassword);
            }

            var user = _dbContext.Users.FirstOrDefault(u => u.Login == loginToDelete);

            if (user is null)
            {
                return NotFound($"Users with login:{loginToDelete} is not found.");
            }

            user.RevorkedOn = DateTime.Now;
            user.RevorkedBy = login;

            _dbContext.SaveChanges();

            return Ok("User has been successfully deleted.");
        }


        [HttpPost]
        public IActionResult RestoreUser(string login, string password, string loginToRestore)
        {
            if (IsAdminExist(login, password) is null)
            {
                return Unauthorized(TextConstants.IncorrectLoginOrPassword);
            }

            var user = _dbContext.Users.FirstOrDefault(u => u.Login == loginToRestore);

            if (user is null)
            {
                return NotFound($"Users with login:{loginToRestore} is not found.");
            }

            user.RevorkedOn = DateTime.MaxValue;
            user.RevorkedBy = string.Empty;

            _dbContext.SaveChanges();

            return Ok("User has been successfully restored.");
        }


        private User IsAdminExist(string login, string password) => _dbContext.Users.FirstOrDefault(u => u.Login == login && u.Password == password && u.Admin == true);
    }
}
