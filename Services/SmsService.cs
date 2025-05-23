using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace SAIS.Services
{

    public class SmsService : ISmsService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<SmsService> _logger;

        public SmsService(IConfiguration configuration, ILogger<SmsService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task SendApplicationSubmittedSms(string phoneNumber, string applicantName, string referenceNumber)
        {
            var message = $"Dear {applicantName}, your social assistance application has been submitted. Ref: {referenceNumber}";
            await SendSms(phoneNumber, message);
        }

        public async Task SendApplicationApprovedSms(string phoneNumber, string applicantName, string programNames, string referenceNumber)
        {
            var message = $"Dear {applicantName}, your application for {programNames} has been approved. Ref: {referenceNumber}";
            await SendSms(phoneNumber, message);
        }

        public async Task SendApplicationUpdatedSms(string phoneNumber, string applicantName, string referenceNumber)
        {
            var message = $"Dear {applicantName}, your application details have been updated. Ref: {referenceNumber}";
            await SendSms(phoneNumber, message);
        }

        public async Task SendStatusChangedSms(string phoneNumber, string applicantName, string newStatus, string referenceNumber)
        {
            var message = $"Dear {applicantName}, your application status has changed to {newStatus}. Ref: {referenceNumber}";
            await SendSms(phoneNumber, message);
        }

        private async Task SendSms(string phoneNumber, string message)
        {
            try
            {
                // In production, use a real SMS gateway API
                var smsGatewayEnabled = _configuration.GetValue<bool>("SmsGateway:Enabled");

                if (smsGatewayEnabled)
                {
                    // Implement actual SMS gateway integration here
                    // Example: await _smsGatewayClient.SendAsync(phoneNumber, message);
                    _logger.LogInformation($"SMS sent to {phoneNumber}: {message}");
                }
                else
                {
                    // Log to console in development
                    _logger.LogInformation($"SMS would be sent to {phoneNumber}: {message}");
                    Console.WriteLine($"SMS to: {phoneNumber}");
                    Console.WriteLine($"Message: {message}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending SMS");
                // Fail silently or implement retry logic as needed
            }
        }
    }
}