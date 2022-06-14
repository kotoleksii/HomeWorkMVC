using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoreApp
{
    public class RoutingMiddleware
    {
        private readonly RequestDelegate _next;
        public RoutingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task InvokeAsync(HttpContext context)
        {
            string path = context.Request.Path.Value.ToLower();

            if (path == "/goods")
            {
                context.Response.Redirect("/Home/Goods");
            }
            else if (path == "/about")
            {
                context.Response.Redirect("/Home/About");
            }
            else
            {
                context.Response.StatusCode = 404;
            }

            return Task.CompletedTask;
        }
    }
}
