using DepsWebApp.Enums;
using DepsWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Net;

namespace DepsWebApp.Filters
{
    public class ExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (context.ExceptionHandled) return;

            var code = GetCode(context.Exception);
            var message = GetMessage(code);
            var result = new JsonResult(new ExceptionResult
                {
                    Code = (int)code,
                    Message = message ?? context.Exception.Message
                })
            {
                StatusCode = (int)HttpStatusCode.Unauthorized
            };
            context.ExceptionHandled = true;
            context.Result = result;
        }

        private static ExceptionsCode GetCode(Exception ex)
        {
            return ex switch
            {
                NotImplementedException => ExceptionsCode.FailedAuthorization,
                _ => ExceptionsCode.UnknownException,
            };
        }

        private static string GetMessage(ExceptionsCode code)
        {
            return code switch
            {
                ExceptionsCode.FailedAuthorization => "Failed authorization",
                _ => null,
            };
        }
    }
}
