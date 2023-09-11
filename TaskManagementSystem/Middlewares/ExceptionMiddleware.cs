using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using TaskManagementSystem.Core.Exceptions;

namespace TaskManagementSystem.Middlewares
{
    public static class ExceptionMiddleware
    {
        public static void UseExceptionHandler(this WebApplication app)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.ContentType = "application/json";
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {
                        context.Response.StatusCode = contextFeature.Error switch
                        {
                            NotFoundException => StatusCodes.Status404NotFound,
                            BadRequestException => StatusCodes.Status400BadRequest,
                            ArgumentNullException => StatusCodes.Status400BadRequest,
                            _ => StatusCodes.Status500InternalServerError
                        };
                      //  logger.LogError($"Something went wrong: {contextFeature.Error}");
                        context.Response.ContentType = "application/json";
                        var response = new ProblemDetails()
                        {
                            Status = context.Response.StatusCode,
                            Detail = context.Response.StatusCode == StatusCodes.Status500InternalServerError ? new string("Error occurred, please contact support") : contextFeature.Error.Message
                        };
                        var option = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                        var json = JsonSerializer.Serialize(response, option);
                        await context.Response.WriteAsync(json);
                    }
                });
            });
        }
        //public async Task InvokeAsync(HttpContext context)
        //{
        //    try
        //    {
        //        await _next(context);

        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, ex.Message);
        //        context.Response.ContentType = "application/json";
        //        var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
        //        if (contextFeature != null)
        //        {
        //            context.Response.StatusCode = contextFeature.Error switch
        //            {
        //                BadRequestException => StatusCodes.Status400BadRequest,
        //                NotFoundException => StatusCodes.Status404NotFound,
        //                _ => StatusCodes.Status500InternalServerError
        //            };
        //            var response = new ProblemDetails()
        //            {
        //                Status = context.Response.StatusCode,
        //                Detail = context.Response.StatusCode == StatusCodes.Status500InternalServerError ? new string("Error occurred, please contact support") : contextFeature.Error.Message
        //            };
        //            var option = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        //            var json = JsonSerializer.Serialize(response, option);
        //            await context.Response.WriteAsync(json);


        //        }

        //    }
        //}
    }
}


