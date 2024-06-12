using Microsoft.AspNetCore.Mvc;

public class AccountController : Controller
{
    private readonly IHttpContextAccessor httpContext;

    public AccountController(IHttpContextAccessor _httpContext)
    {
        httpContext = _httpContext;
    }

    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Login(string name)
    {
        if (!string.IsNullOrEmpty(name))
        {
            // Store the name in session or local storage
            var ipAddress = httpContext.HttpContext.Connection.RemoteIpAddress;

            HttpContext.Session.SetString("UserIPAddress", ipAddress.ToString());

            HttpContext.Session.SetString("UserName", name);

            return RedirectToAction("Index", "Home"); // Redirect to the main page
        }
        else
        {
            ModelState.AddModelError("", "Please enter your name.");
            return View();
        }
    }
}