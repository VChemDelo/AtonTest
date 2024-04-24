namespace AtonTest.Exceptions
{
    public class UserAlreadyExistsException : Exception
    {
        public UserAlreadyExistsException() :base("User with this login is already exist"){ }
    }
}
