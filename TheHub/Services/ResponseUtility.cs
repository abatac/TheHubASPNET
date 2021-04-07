using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using TheHub.Models;

namespace TheHub.Services
{
    public static class ResponseUtility
    {

        public static HttpResponseMessage CreateHttpResponseMessage(List<string> errors, HttpStatusCode statusCode)
        {

            OrderResponse orderResponseWithError = CreateOrderResponseWithError(errors);

            var resultResponse = JsonConvert.SerializeObject(
                         orderResponseWithError, Formatting.Indented,
                         new JsonSerializerSettings()
                         {
                             ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                         });
            return new HttpResponseMessage
            {
                StatusCode = statusCode,
                Content = new StringContent(
                    resultResponse, System.Text.Encoding.UTF8,
                    "application/json"),
            };

        }

        public static HttpResponseMessage CreateHttpErrorResponse(List<string> errors, HttpStatusCode statusCode)
        {

            ErrorResponse errorResponse = CreateErrorResponse(errors);

            var resultResponse = JsonConvert.SerializeObject(
                         errorResponse, Formatting.Indented,
                         new JsonSerializerSettings()
                         {
                             ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                         });
            return new HttpResponseMessage
            {
                StatusCode = statusCode,
                Content = new StringContent(
                    resultResponse, System.Text.Encoding.UTF8,
                    "application/json"),
            };

        }

        private static OrderResponse CreateOrderResponseWithError(List<string> errors)
        {
            OrderResponse orderResponse = new OrderResponse();
            orderResponse.Messages = new List<ErrorMessage>();
            foreach (string error in errors)
            {
                orderResponse.Messages.Add(new ErrorMessage(error));
            }
            return orderResponse;

        }

        private static ErrorResponse CreateErrorResponse(List<string> errors)
        {
            ErrorResponse errorResponse = new ErrorResponse();
            errorResponse.Messages = new List<ErrorMessage>();
            foreach (string error in errors)
            {
                errorResponse.Messages.Add(new ErrorMessage(error));
            }
            return errorResponse;

        }
    }
}