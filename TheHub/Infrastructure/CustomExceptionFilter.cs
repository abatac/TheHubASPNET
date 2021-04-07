using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;

namespace TheHub.Infrastructure
{
    public class CustomExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            var exceptionMessage = string.Empty;
            if (actionExecutedContext.Exception.InnerException == null)
                exceptionMessage = actionExecutedContext.Exception.Message;
            else
                exceptionMessage = actionExecutedContext.Exception.InnerException.Message;
            // We can log this exception message to the file or database.  
            var response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
            {
                Content = new StringContent("An unhandled exception was thrown by service: " + exceptionMessage),
                ReasonPhrase = "Internal Server Error. Please Contact your Administrator."
            };
            actionExecutedContext.Response = response;
        }
    }
}