using EmailOtpModule.Services;

namespace EmailOtpModule.Tests
{
    public class OtpServiceTest
    {
        private OtpService CreateService()
        {
            return new OtpService();
        }

        [Fact]
        public void Test_OtpService_GenerateOtp_Positive()
        {
            //Arrange
            var service = CreateService();

            //Act
            string generatedOtp = service.GenerateOtp();


            //Asssert
            Assert.Equal(6, generatedOtp.Length);

        }
    }

}