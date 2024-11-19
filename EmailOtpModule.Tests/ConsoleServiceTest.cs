using EmailOtpModule.Constants;
using EmailOtpModule.Services;
using FakeItEasy;

namespace EmailOtpModule.Tests
{
    public class ConsoleServiceTest
    {
        private IEmailService _emailService;
        private IOtpService _otpService;

        public ConsoleServiceTest()
        {
            _emailService = A.Fake<IEmailService>();
            _otpService = A.Fake<IOtpService>();
        }

        private ConsoleService CreateService(int seconds)
        {
            return new ConsoleService(_emailService, _otpService)
            {
                _currentOtp = "123456", // Mock the global variable
                _otpExpiryTime = DateTime.Now.AddSeconds(seconds) // Ensure OTP is not expired
            };
        }


        [Fact]
        public void Test_ConsoleService_ProcessEmailAndSendOtp_InvalidEmail()
        {
            //Arrange
            string userEmail = "sameera@test.com";

            var input = new StringReader(userEmail);
            Console.SetIn(input);
            A.CallTo(() => _emailService.ValidateUserEmail(userEmail)).Returns(false);

            var service = CreateService(10);

            //Act
            StatusEnum status = service.ProcessEmailAndSendOtp();

            //Assert
            Assert.Equal(StatusEnum.STATUS_EMAIL_INVALID, status);
        }

        [Fact]
        public void Test_ConsoleService_ProcessEmailAndSendOtp_SendFail()
        {
            //Arrange
            string userEmail = "sameera@dso.org.sg";
            string otp = "123456";
            var input = new StringReader(userEmail);
            Console.SetIn(input);

            A.CallTo(() => _emailService.ValidateUserEmail(userEmail)).Returns(true);
            A.CallTo(() => _otpService.GenerateOtp()).Returns(otp);
            A.CallTo(() => _emailService.SendEmail(userEmail, otp)).Returns(false);

            var service = CreateService(10);

            //Act
            StatusEnum status = service.ProcessEmailAndSendOtp();

            //Assert
            Assert.Equal(StatusEnum.STATUS_EMAIL_FAIL, status);
        }

        [Fact]
        public void Test_ConsoleService_ProcessEmailAndSendOtp_ValidEmail()
        {
            //Arrange
            string userEmail = "sameera@dso.org.sg";
            string otp = "123456";
            var input = new StringReader(userEmail);
            Console.SetIn(input);

            A.CallTo(() => _emailService.ValidateUserEmail(userEmail)).Returns(true);
            A.CallTo(() => _otpService.GenerateOtp()).Returns(otp);
            A.CallTo(() => _emailService.SendEmail(userEmail, otp)).Returns(true);

            var service = CreateService(10);

            //Act
            StatusEnum status = service.ProcessEmailAndSendOtp();

            //Assert
            Assert.Equal(StatusEnum.STATUS_EMAIL_OK, status);
        }



        [Fact]
        public void Test_ConsoleService_ProcessAndValidateOtp_InvalidOtpMaximumTries()
        {
            //Arrange
            string otp1 = "123455";
            string otp2 = "123457";
            string otp3 = "123458";
            string otp4 = "123459";
            string otp5 = "123460";
            string otp6 = "123461";
            string otp7 = "123462";
            string otp8 = "123463";
            string otp9 = "123464";
            string otp10 = "123465";

            string userInput = $"{otp1}\n{otp2}\n{otp3}\n{otp4}\n{otp5}\n{otp6}\n{otp7}\n{otp8}\n{otp9}\n{otp10}";

            var input = new StringReader(userInput);
            Console.SetIn(input);

            var service = CreateService(10);

            //Act
            StatusEnum status = service.ProcessAndValidateOtp();

            //Assert
            Assert.Equal(StatusEnum.STATUS_OTP_FAIL, status);
        }

        [Fact]
        public void Test_ConsoleService_ProcessAndValidateOtp_Timeout()
        {
            //Arrange

            var service = CreateService(-10);

            //Act
            StatusEnum status = service.ProcessAndValidateOtp();

            //Assert
            Assert.Equal(StatusEnum.STATUS_OTP_TIMEOUT, status);
        }

        [Fact]
        public void Test_ConsoleService_ProcessAndValidateOtp_ValidOtp()
        {
            //Arrange
            string otp1 = "123456";

            var input = new StringReader(otp1);
            Console.SetIn(input);

            var service = CreateService(10);

            //Act
            StatusEnum status = service.ProcessAndValidateOtp();

            //Assert
            Assert.Equal(StatusEnum.STATUS_OTP_OK, status);
        }

    }

}