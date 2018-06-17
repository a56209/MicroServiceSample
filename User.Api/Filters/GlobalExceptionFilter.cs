﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace User.Api.Filters
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        private readonly IHostingEnvironment _evn;
        private readonly ILogger<GlobalExceptionFilter> _logger;

        public GlobalExceptionFilter(IHostingEnvironment evn, ILogger<GlobalExceptionFilter> logger)
        {
            _evn = evn;
            _logger = logger;
        }      

        public void OnException(ExceptionContext context)
        {
            var json = new JsonErrorResponse();
            if(context.Exception.GetType() == typeof(UserOperationException))
            {

                json.Message = context.Exception.Message;                
                context.Result = new BadRequestObjectResult(json);
            }
            else
            {
                json.Message = "发生了未知内部错误";                
                if(_evn.IsDevelopment())
                {
                    json.DeveloperMessage = context.Exception.StackTrace;
                }
                context.Result = new InternalServerErrorObjectResult(json);
            }

            _logger.LogError(context.Exception, context.Exception.Message);
            context.ExceptionHandled = true;
        }

        public class InternalServerErrorObjectResult : ObjectResult
        {
            public InternalServerErrorObjectResult(object error) : base(error)
            {
                StatusCode = StatusCodes.Status500InternalServerError;
               
            }
        }
    }
}
