using AtonTest.Models;
using AtonTest.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace AtonTest.Services
{
    public class EntityService
    {

        private readonly AtDbContext _dbContext;


        private readonly Regex latinLettersAndNumbers = new Regex(@"^[a-zA-Z0-9]+$");

        public EntityService (AtDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public void CreateUser(string newUserLogin, string newUserPassword, string newUserName, int newUserGender, DateTime newUserBirthday, string createdBy, bool isAdmin = false)
        {
            IsUserLoginUnique(newUserLogin);

            if(!latinLettersAndNumbers.Match(newUserLogin).Success || !latinLettersAndNumbers.Match(newUserPassword).Success || !latinLettersAndNumbers.Match(newUserName).Success) 
                throw new ValidateLogPassNameException();


            User userToCreate = new User()
            {
                Login = newUserLogin,
                Password = newUserPassword,
                Name = newUserName,
                Gender = newUserGender,
                Birthday = newUserBirthday,
                CreatedBy = createdBy,
                Admin = isAdmin
            };


            if(userToCreate.Birthday == DateTime.MinValue)
            {
                userToCreate.Birthday = DateTime.MaxValue;
            }

            try
            {
                _dbContext.Users.Add(userToCreate);
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.Out.WriteLine(ex.Message);
                throw;
            }
        }
        public void ChangeUserName(string userLogin, string nameTochange, string modifiedBy)
        {
            var user = _dbContext.Users.First(u => u.Login == userLogin);

            if (!latinLettersAndNumbers.Match(nameTochange).Success)
                throw new ValidateLogPassNameException();

            user.Name = nameTochange;
            user.ModifiedBy = modifiedBy;
            user.ModifiedOn = DateTime.UtcNow;

            _dbContext.SaveChanges();
        }


        public void ChangeUserGender(string userLogin, int genderToChange, string modifiedBy)
        {
            var user = _dbContext.Users.First(u => u.Login == userLogin);

            user.Gender = genderToChange;
            user.ModifiedBy = modifiedBy;
            user.ModifiedOn = DateTime.UtcNow;

            _dbContext.SaveChanges();
        }


        public void ChangeUserBirthday(string userLogin, DateTime birthdayToChange, string modifiedBy)
        {
            var user = _dbContext.Users.First(u => u.Login == userLogin);

            user.Birthday = birthdayToChange;
            user.ModifiedBy = modifiedBy;
            user.ModifiedOn = DateTime.UtcNow;

            _dbContext.SaveChanges();
        }


        public void ChangeUserPassword(string userLogin, string password, string modifiedBy)
        {
            if (!latinLettersAndNumbers.Match(password).Success)
                throw new ValidateLogPassNameException();

            var user = _dbContext.Users.First(u => u.Login == userLogin);

            user.Password = password;
            user.ModifiedBy = modifiedBy;
            user.ModifiedOn = DateTime.UtcNow;

            _dbContext.SaveChanges();
        }


        public void ChangeUserLogin(string userLogin, string newLogin, string modifiedBy)
        {
            IsUserLoginUnique(newLogin);

            if (!latinLettersAndNumbers.Match(newLogin).Success)
                throw new ValidateLogPassNameException();

            var user = _dbContext.Users.First(u => u.Login == userLogin);

            user.Login = newLogin;
            user.ModifiedBy = modifiedBy;
            user.ModifiedOn = DateTime.UtcNow;

            _dbContext.SaveChanges();
        }


        private void IsUserLoginUnique(string userLogin)
        {
            if(_dbContext.Users.Any(e => e.Login == userLogin))
            {
                throw new UserAlreadyExistsException();
            }
        }
    }
}
