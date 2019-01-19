using System;
using Microsoft.AspNetCore.Mvc;

namespace DriveCentric.BaseService
{
    public static class ExceptionHelper
    {
        public static ObjectResult ProcessError(Exception exception)
            => new ObjectResult(
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