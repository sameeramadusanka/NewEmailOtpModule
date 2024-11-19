using System.Text.RegularExpressions;
using EmailOtpModule.Constants;

namespace EmailOtpModule.Services
{
    public class EmailService : IEmailService
    {

        public bool ValidateUserEmail(string? userEmail)
        {

            if (string.IsNullOrEmpty(userEmail))
            {
                return false;
            }
            userEmail = userEmail.Trim();

            //validate with regex
            string emailPattern = @"^[a-zA-Z0-9.]+@dso\.org\.sg$";

            if (!Regex.IsMatch(userEmail, emailPattern))
            {
                return false;
            }
            return true;
        }

        public bool SendEmail(string email, string otp)
        {
            try
            {
                string emailBody = $"{EmailConstants.YOUR_OTP}{otp}{EmailConstants.CODE_IS_VALID}";

                EmailSentOut(email, emailBody);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                //Current implementstion there will be no exceptions
                return false;
            }
        }

        public void EmailSentOut(string email, string emailBody)
        {
            //Assume email is sent is implemented
            Console.WriteLine(emailBody);
        }

    }

}