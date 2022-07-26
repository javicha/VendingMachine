using Application.DTO;
using Application.Exceptions;

namespace Application.Extensions
{
    /// <summary>
    /// Extension to transform validation errors into a custom DTO in response to the request
    /// </summary>
    public static class ValidationExceptionExtension
    {
        public static List<ErrorResponse> ToErrorResponse(this ValidationException exception)
        {
            var result = new List<ErrorResponse>();

            exception.Errors.ToList().ForEach(e =>
            {
                result.Add(new ErrorResponse($"Validation error in property {e.Key}", e.Value));
            });

            return result;
        }
    }
}
