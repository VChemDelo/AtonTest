namespace AtonTest.Exceptions
{
    public class ValidateLoginPasswordNameException : Exception
    {   
        public ValidateLoginPasswordNameException():base("All characters except Latin letters and numbers in login, password and name fields are prohibited") { }
    }
}
