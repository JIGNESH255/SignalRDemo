public class SessionMiddleware
{
    private readonly RequestDelegate _next;

    public SessionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Session != null)
        {
            var userName = context.Session.GetString("UserName");
            if (!string.IsNullOrEmpty(userName))
            {
                context.Items["UserName"] = userName;
            }
        }
        await _next(context);
    }
}
