namespace Factors.Feature.Email.Models
{
    public class EmailSendResult
    {
        public bool IsSuccess { get; set; }

        public string Message { get; set; }

        public string TrackingIdentifier { get; set; }
    }
}
