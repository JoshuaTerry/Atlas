using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace DriveCentric.BaseService
{
    public static class ExceptionHelper
    {
        public static ObjectResult ProcessError(Exception exception)
        {
            if (exception.GetType() == typeof(KeyNotFoundException))
            {
                return new NotFoundObjectResult(new
                {
                    error = new
                    {
                        code = exception.HResult,
                        message = exception.Message
                    }
                });
            }

            return new ObjectResult(
                new
                {
                    error = new
                    {
                        code = exception.HResult,
                        message = exception.Message
                    }
                });
        }
    }
}
