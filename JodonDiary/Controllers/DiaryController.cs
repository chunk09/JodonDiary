using JodonDiary.Database;
using JodonDiary.Models;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.Ocsp;

namespace JodonDiary.Controllers;

public class DiaryController : Controller
{
    // GET
    [HttpGet("/")]
    public IActionResult Index() => View();

    [HttpGet("/write")]
    public IActionResult Write() => View();

    [HttpGet("/another-list")]
    public IActionResult AnotherList()
    {
        var service = new DiaryService();

        ViewData["Diarys"] = service.GetDiary();

        return View();
    }

    [HttpGet("/my-list")]
    public IActionResult MyList()
    {
        var service = new DiaryService();

        ViewData["Diarys"] = service.GetDiary();

        return View();
    }

    [HttpGet("/diary/{id}")]
    public IActionResult Diary(string id)
    {
        var diaryService = new DiaryService();
        var userService = new UserService();

        foreach (var diary in diaryService.GetDiary().Where(diary => diary.id == Convert.ToInt32(id)))
        {
            ViewData["diary-title"] = diary.title;
            ViewData["content"] = diary.content;
            ViewData["id"] = diary.user_key;
            ViewData["diary-id"] = diary.id;

            foreach (var user in userService.GetUser().Where(user => user.id == diary.user_key))
            {
                ViewData["name"] = user.user_name;
            }
        }

        return View();
    }

    [HttpGet("/delete-diary/{id}")]
    public RedirectResult DeleteDiary(string id)
    {
        var service = new DiaryService();
        
        service.DeleteDiary(Convert.ToInt32(id));

        return Redirect("/");
    }
    
    [HttpGet("/update-diary/{id}")]
    public IActionResult Update(string id)
    {
        var service = new DiaryService();

        foreach (var diary in service.GetDiary().Where(diary => diary.id == Convert.ToInt32(id)))
        {
            ViewData["diary-title"] = diary.title;
            ViewData["content"] = diary.content;
            ViewData["id"] = id;

            Console.WriteLine($"private : {diary.private_check}");
            
            if (diary.private_check == 1)
            {
                ViewData["checked"] = true;
            }
            else
            {
                ViewData["checked"] = false;
            }
        }

        return View();
    }

    [HttpPost("/update-post/{id}")]
    public RedirectResult UpdatePost(string id)
    {
        var service = new DiaryService();
        
        Diary diary;

        var title = Request.Form["title"];
        var content = Request.Form["content"];
        var privateCheck = Request.Form["private"];
        
        Console.WriteLine($"privateCheck : {privateCheck}");

        if (privateCheck == "비공개")
        {
            diary = new Diary
            {
                title = title,
                content = content,
                date = DateTime.Now.ToString("yyyy-MM-dd"),
                user_key = HttpContext.Session.GetInt32("Jodon-Id"),
                private_check = 1
            };
        }
        else
        {
            diary = new Diary
            {
                title = title,
                content = content,
                date = DateTime.Now.ToString("yyyy-MM-dd"),
                user_key = HttpContext.Session.GetInt32("Jodon-Id"),
                private_check = 0
            };
        }

        if (CheckWrite(diary))
        {
            Console.WriteLine($"{diary.user_key}(KEY) 일기 업데이트 성공");
            
            service.UpdateData(Convert.ToInt32(id), diary);

            return Redirect("/");
        }

        return Redirect("/");
    }

    [HttpPost("/write/post")]
    public RedirectResult WritePost()
    {
        var service = new DiaryService();
        
        Diary diary;

        var title = Request.Form["title"];
        var content = Request.Form["content"];
        var privateCheck = Request.Form["private"];
        
        Console.WriteLine($"privateCheck : {privateCheck}");

        if (privateCheck == "비공개")
        {
            diary = new Diary
            {
                title = title,
                content = content,
                date = DateTime.Now.ToString("yyyy-MM-dd"),
                user_key = HttpContext.Session.GetInt32("Jodon-Id"),
                private_check = 1
            };
        }
        else
        {
            diary = new Diary
            {
                title = title,
                content = content,
                date = DateTime.Now.ToString("yyyy-MM-dd"),
                user_key = HttpContext.Session.GetInt32("Jodon-Id"),
                private_check = 0
            };
        }

        if (CheckWrite(diary))
        {
            Console.WriteLine($"{diary.user_key}(KEY) 일기 작성 성공");
            
            service.InsertDiary(diary);

            return Redirect("/");
        }

        return Redirect("/write");
    }

    private bool CheckWrite(Diary diary)
    {
        if (diary.title.Length == 0) return false;
        if (diary.content.Length == 0) return false;

        return true;
    }
}