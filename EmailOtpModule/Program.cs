// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.DependencyInjection;
using EmailOtpModule.Services;


Console.WriteLine("======================Email OTP Module======================");

var services = new ServiceCollection();

#region Register services
services.AddTransient<IConsoleService, ConsoleService>();
services.AddTransient<IEmailService, EmailService>();
services.AddTransient<IOtpService, OtpService>();
#endregion

var serviceBuilder = services.BuildServiceProvider();
var consoleService = serviceBuilder.GetRequiredService<IConsoleService>();
consoleService.Start();
Console.WriteLine("======================Email OTP Module======================");