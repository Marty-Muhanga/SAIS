namespace SAIS.Services
{
    public interface ISmsService
    {
        Task SendApplicationSubmittedSms(string phoneNumber, string applicantName, string referenceNumber);
        Task SendApplicationApprovedSms(string phoneNumber, string applicantName, string programNames, string referenceNumber);
        Task SendApplicationUpdatedSms(string phoneNumber, string applicantName, string referenceNumber);
        Task SendStatusChangedSms(string phoneNumber, string applicantName, string newStatus, string referenceNumber);
    }
}
