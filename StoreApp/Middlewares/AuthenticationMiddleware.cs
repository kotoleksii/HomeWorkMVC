using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoreApp
{
    public class AuthenticationMiddleware
    {
        private RequestDelegate _next;
        public AuthenticationMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            var token = context.Request.Query["token"];

            if (token != "12345")
            {
                context.Response.StatusCode = 403;
                await context.Response.WriteAsync("Token is invalid\n");
            }
            else
            {
                //context.Response.Redirect("/Home/Index");
                await _next.Invoke(context);
            }
        }
    }
}
