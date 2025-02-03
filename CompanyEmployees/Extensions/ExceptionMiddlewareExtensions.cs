﻿using Contracts;
using Entities.ErrorModel;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using System.Net;

namespace CompanyEmployees.Extensions
{
	public static class ExceptionMiddlewareExtensions
	{
		public static void ConfigExceptionHandler(this WebApplication webApplication,
			ILoggerManager loggerManager)
		{
			webApplication.UseExceptionHandler(appError =>
			{
				appError.Run(async context =>
				{
					context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
					context.Response.ContentType = "application/json";

					var contextFeature = context.Features.Get<IExceptionHandlerFeature>();

					if (contextFeature != null)
					{
						loggerManager.LogError($"Something went wrong {contextFeature.Error}");

						await context.Response.WriteAsync(new ErrorDetails
						{
							StatusCode = context.Response.StatusCode,
							Message = "Internal Server Error.",
						}.ToString());
					}
				});
			});
		}
	}
}
