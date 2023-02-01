using JodonDiary.Database;
using JodonDiary.Models;
using Microsoft.AspNetCore.Mvc;

namespace JodonDiary.Controllers;

public class UserController : Controller
{
    // GET
    [HttpGet("/register")]
    public IActionResult Register() => View();

    [HttpGet("/login")]
    public IActionResult Login() => View();

    [HttpGet("/logout")]
    public RedirectResult Logout()
    {
        HttpContext.Session.Remove("Jodon-Id");

        return Redirect("/");
    }

    [HttpPost("/register/post")]
    public RedirectResult RegisterPost()
    {
        var service = new UserService();

        var user = new User
        {
            user_id = Request.Form["Id"],
            user_password = Request.Form["Password"],
            user_name = Request.Form["Name"]
        };

        if (CheckRegister(user, Request.Form["PasswordCheck"]))
        {
            service.InsertUser(user);
            
            Console.WriteLine($"{user.id}.{user.user_name}님이 회원가입에 성공하셨습니다.");

            return Redirect("/login");
        }

        Console.WriteLine($"{user.user_id}(ID)님이 회원가입에 실패하셨습니다.");

        return Redirect("/register");
    }

    [HttpPost("/login/post")]
    public RedirectResult LoginPost()
    {
        var service = new UserService();

        var inputId = Request.Form["Id"];
        var inputPw = Request.Form["Password"];

        foreach (var user in service.GetUser().Where(user => user.user_id == inputId && user.user_password == inputPw))
        {
            Console.WriteLine($"{user.id}.{user.user_name}님이 로그인에 성공하셨습니다.");
            
            HttpContext.Session.SetInt32("Jodon-Id", user.id);

            return Redirect("/");
        }
        
        Console.WriteLine($"{inputId}(ID)님이 로그인의 실패하셨습니다.");
            
        return Redirect("/login");
    }

    private bool CheckRegister(User user, string passwordCheck)
    {
        if (user.user_id.Length < 5) return false;
        if (user.user_password.Length < 5) return false;
        if (user.user_name.Length == 0) return false;

        if (user.user_password != passwordCheck) return false;

        return true;
    }
}