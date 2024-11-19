using EmailOtpModule.Services;
using FakeItEasy;

namespace EmailOtpModule.Tests
{
    public class EmailServiceTest
    {
        private EmailService CreateService()
        {
            return new EmailService();
        }

        [Fact]
        public void Test_EmailService_ValidateUserEmail_InvalidEmail()
        {
            //Arrange
            string email = "";

            var service = CreateService();

            //Act
            bool isValidEmail = service.ValidateUserEmail(email);


            //Assert
            Assert.False(isValidEmail);

        }

        [Fact]
        public void Test_EmailService_ValidateUserEmail_InvalidEmailDomain()
        {
            //Arrange
            string email = "sameera@gmail.com";

            var service = CreateService();

            //Act
            bool isValidEmail = service.ValidateUserEmail(email);


            //Assert
            Assert.False(isValidEmail);

        }

        [Fact]
        public void Test_EmailService_ValidateUserEmail_ValidEmailDomain()
        {
            //Arrange
            string email = "sameera@dso.org.sg";

            var service = CreateService();

            //Act
            bool isValidEmail = service.ValidateUserEmail(email);


            //Assert
            Assert.True(isValidEmail);

        }


        [Fact]
        public void Test_EmailService_SendEmail_Success()
        {
            //Arrange
            string email = "sameera@dso.org.sg";
            string otp = "123456";

            var service = CreateService();

            //Act
            bool isEmailSent = service.SendEmail(email, otp);


            //Assert
            Assert.True(isEmailSent);

        }

        [Fact]
        public void Test_EmailService_SendEmail_Unsuccess()
        {
            //Arrange
            var fakeEmailService = A.Fake<IEmailService>();

            // Mock the EmailSentOut method to throw an exception
            A.CallTo(() => fakeEmailService.EmailSentOut(A<string>.Ignored, A<string>.Ignored))
                .Throws(new Exception("Simulated email send failure"));

            var email = "sameera@dso.org.sg";
            var otp = "123456";

            // Act
            var result = fakeEmailService.SendEmail(email, otp);

            // Assert
            Assert.False(result);

        }

    }

}