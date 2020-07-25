using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace CocktailMagician.Web.Middlewares
{
    public class NotFoundMiddleware
    {
        private readonly RequestDelegate next;

        public NotFoundMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            await this.next(httpContext);

            if (httpContext.Response.StatusCode == 404)
            {
                httpContext.Response.Redirect("/Home/NotFound");
            }
        }
    }
}
