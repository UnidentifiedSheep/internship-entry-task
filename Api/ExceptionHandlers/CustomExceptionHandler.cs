using System.Text.Json;
using Core.Exceptions;
using Core.Interfaces;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Api.ExceptionHandlers
{
	public class CustomExceptionHandler(ILogger<CustomExceptionHandler> logger) : IExceptionHandler
	{
		public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken cancellationToken)
		{
			logger.LogTrace(context.TraceIdentifier, exception, "Error occurred at {time}", DateTime.UtcNow);

			var showDetails = exception is
				ValidationException or
				BadRequestException or
				BadHttpRequestException or
				JsonException or
				NotFoundException;

			var statusCode = exception switch
			{
				BadHttpRequestException => StatusCodes.Status400BadRequest,
				JsonException => StatusCodes.Status400BadRequest,
				InternalServerException => StatusCodes.Status500InternalServerError,
				ValidationException => StatusCodes.Status400BadRequest,
				BadRequestException => StatusCodes.Status400BadRequest,
				NotFoundException => StatusCodes.Status404NotFound,
				_ => StatusCodes.Status500InternalServerError
			};

			context.Response.StatusCode = statusCode;

			var problemDetails = new ProblemDetails
			{
				Title = showDetails ? exception.GetType().Name : "An unexpected error occurred",
				Detail = showDetails ? exception.Message : null,
				Status = statusCode,
				Instance = context.Request.Path,
				Type = $"https://httpstatuses.io/{statusCode}",
				Extensions =
				{
					["traceId"] = context.TraceIdentifier
				}
			};
			
			if (exception is IValuedException valuedEx)
			{
				var errorValues = valuedEx.GetErrorValues();
				if (errorValues != null)
					problemDetails.Extensions["errorRelatedData"] = errorValues;
			}

			await context.Response.WriteAsJsonAsync(problemDetails, cancellationToken: cancellationToken);
			return true;
		}
	}

}
