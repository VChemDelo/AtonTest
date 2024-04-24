namespace AtonTest.Exceptions
{
    public class ValidateLogPassNameException : Exception
    {   
        public ValidateLogPassNameException():base("All characters except Latin letters and numbers in login, password and name fields are prohibited") { }
    }
}
