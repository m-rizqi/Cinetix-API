using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Cinetix_Api.Response
{
    public class ErrorResponse<T> : ActionResult
    {
        public readonly HttpStatusCode statusCode;
        public readonly string message;

        public ErrorResponse(HttpStatusCode statusCode, string message)
        {
            this.statusCode = statusCode;
            this.message = message;
        }

        public async Task ExecuteResultAsync(ActionContext context)
        {
            var objectResult = new ObjectResult(
                new
                {
                    errorMessage = message
                }
            )
            {
                StatusCode = (int)statusCode,
            };
            context.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = message;
            await objectResult.ExecuteResultAsync(context);
        }
    }
}
