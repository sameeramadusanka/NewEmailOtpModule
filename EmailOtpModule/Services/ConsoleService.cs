
using EmailOtpModule.Constants;

namespace EmailOtpModule.Services
{
    public class ConsoleService : IConsoleService
    {
        private readonly IEmailService _emailService;
        private readonly IOtpService _otpService;

        public string? _currentOtp;
        public DateTime _otpExpiryTime;
        private const int _timeoutDurationSeconds = 60;

        public ConsoleService(IEmailService emailService, IOtpService otpService)
        {
            _emailService = emailService;
            _otpService = otpService;
        }

        public void Start()
        {
            StatusEnum emailStatus = ProcessEmailAndSendOtp();

            string emailStatusMessage = GetMessageForStatus(emailStatus);
            Console.WriteLine(emailStatusMessage);
            if (emailStatus != StatusEnum.STATUS_EMAIL_OK)
            {
                return;
            }

            StatusEnum otpStatus = ProcessAndValidateOtp();
            string otpStatusMessage = GetMessageForStatus(otpStatus);
            Console.WriteLine(otpStatusMessage);
            return;
        }




        public StatusEnum ProcessEmailAndSendOtp()
        {
            //call user email input
            string? userEmail = GetUserEmail();

            //call email service and validate email
            bool isValidEmail = _emailService.ValidateUserEmail(userEmail);

            if (!isValidEmail)
            {
                return StatusEnum.STATUS_EMAIL_INVALID;
            }


            //call otp service and generate
            string otp = _otpService.GenerateOtp();
            _currentOtp = otp;
            _otpExpiryTime = DateTime.Now.AddSeconds(_timeoutDurationSeconds);
            //call email service and send email
            bool isEmailSent = _emailService.SendEmail(userEmail!, otp);

            if (!isEmailSent)
            {
                return StatusEnum.STATUS_EMAIL_FAIL;
            }

            return StatusEnum.STATUS_EMAIL_OK;
        }


        private string? GetUserEmail()
        {
            Console.WriteLine("Please enter your email?");

            string? userEmail = Console.ReadLine();
            return userEmail;
        }

        private static string GetMessageForStatus(StatusEnum status)
        {
            return status switch
            {
                StatusEnum.STATUS_EMAIL_OK => EmailConstants.STATUS_EMAIL_OK,
                StatusEnum.STATUS_EMAIL_FAIL => EmailConstants.STATUS_EMAIL_FAIL,
                StatusEnum.STATUS_EMAIL_INVALID => EmailConstants.STATUS_EMAIL_INVALID,
                StatusEnum.STATUS_OTP_OK => EmailConstants.STATUS_OTP_OK,
                StatusEnum.STATUS_OTP_FAIL => EmailConstants.STATUS_OTP_FAIL,
                StatusEnum.STATUS_OTP_TIMEOUT => EmailConstants.STATUS_OTP_TIMEOUT,
                _ => "Unknown status."
            };
        }


        public StatusEnum ProcessAndValidateOtp()
        {

            if (_currentOtp == null || DateTime.Now > _otpExpiryTime)
            {
                return StatusEnum.STATUS_OTP_TIMEOUT;
            }

            int attempts = 0;
            while (attempts < 10)
            {
                try
                {
                    // Wait for OTP input with timeout
                    Console.WriteLine("Please enter OTP");
                    string userOtp = ReadUserInputWithTimeout();

                    if (userOtp == _currentOtp)
                    {
                        return StatusEnum.STATUS_OTP_OK;
                    }
                    else
                    {
                        attempts++;
                    }
                }
                catch (TimeoutException)
                {
                    return StatusEnum.STATUS_OTP_TIMEOUT;
                }
            }
            return StatusEnum.STATUS_OTP_FAIL;
        }

        private string ReadUserInputWithTimeout()
        {
            string? userOtp = null;

            // Create a task to read input
            Task inputTask = Task.Run(() =>
            {
                userOtp = Console.ReadLine(); // Blocking call
            });

            // Wait for input or timeout
            TimeSpan delayDuration = _otpExpiryTime - DateTime.Now;
            if (Task.WaitAny(inputTask, Task.Delay(delayDuration)) == 0)
            {
                // Input was provided before timeout
                return userOtp ?? string.Empty;
            }
            else
            {
                // Timeout occurred
                throw new TimeoutException();
            }
        }

    }

}