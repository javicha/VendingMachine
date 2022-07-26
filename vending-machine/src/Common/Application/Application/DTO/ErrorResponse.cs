namespace Application.DTO
{
    /// <summary>
    /// Custom DTO with error messages in response to request
    /// </summary>
    public sealed class ErrorResponse
    {
        public string ErrorCode { get; set; }
        public IEnumerable<object> Details { get; set; }

        public ErrorResponse() { }

        public ErrorResponse(string errorCode, IEnumerable<object> details)
        {
            ErrorCode = errorCode ?? throw new ArgumentNullException(nameof(errorCode));
            Details = details ?? throw new ArgumentNullException(nameof(details));
        }

        public ErrorResponse(string errorCode)
        {
            ErrorCode = errorCode ?? throw new ArgumentNullException(nameof(errorCode));
        }
    }
}
