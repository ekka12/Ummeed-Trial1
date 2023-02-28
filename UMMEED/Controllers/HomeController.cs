using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using UMMEED.Models;
using BOL;
using BLL;
namespace UMMEED.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public IActionResult ChildPage(){
        return View();
    }
    [HttpGet]
    public IActionResult Registration(){
        return View();
    }
    [HttpGet]
    public IActionResult AdminIndex(){
      IOManage im = new IOManage();
      List<ChildSts> chlist = im.GetChild();
      if(chlist!=null){
          ViewData["Child"]=chlist;
      }  
      return View();
    }
    [HttpGet]
    public IActionResult Login(){
        return View();
    }
    [HttpGet]
    public IActionResult Insert(){
        IOManage im = new IOManage();
        List<ChildSts>chlist = im.GetChild();
        foreach(ChildSts ch in chlist){
            if(chlist!=null){
                ViewData["Child"]=ch;
            }
        }
        
        return View();
    }
   
    [HttpGet]
    public IActionResult Delete(int chid){
        IOManage im=new IOManage();
        bool status=im.RemoveUser(chid);
        if(status){
            TempData["message"]="Removed SuccessFully";
            return RedirectToAction("AdminIndex","Home");
        }
        return View();
    }

    [HttpGet]
    public IActionResult Edit(){
        IOManage im = new IOManage();
        List<ChildSts>chlist = im.GetChild();
        foreach(ChildSts ch in chlist){
            if(chlist!=null){
                ViewData["Child"]=ch;
            }
        }

        return View();
    } 
    [HttpPost]
    public IActionResult Edit(int chid,string payment,string date){
        IOManage im = new IOManage();
        bool status = im.Update(chid,payment,date);
        if(status){
            TempData["message"]="Updated Successfully";
            return RedirectToAction("AdminIndex","Home");
        }
        return View();
    }

    [HttpPost]
    public IActionResult Insert(string password,string chname,string date,int chid){
        IOManage im = new IOManage();
        string role="child";
        string chkey = Guid.NewGuid().ToString();
        bool status = false;

        status = im.ChildInx(chname,date,password,role,chid,chkey);

        if(status){
            TempData["message"]="Registered SuccessFully";
           return RedirectToAction("AdminIndex","Home");
        }

        return View();

    }
    
    [HttpPost]
    public IActionResult Login(string uname,string password,string role){
        IOManage im = new IOManage();
        List<User>ulist  = im.GetAll();
        foreach(User u in ulist){
            if(u!=null){
                
                if(u.Uname.Equals(uname) && u.Password.Equals(password)){
                    if(u.Role==role){
                        if(role=="admin"){
                            return RedirectToAction("AdminIndex","Home");
                        }
                        if(role=="child"){
                            return RedirectToAction("ChildPage","Home");
                        }
                       TempData["message"]="Invalid Credentials";
                       return RedirectToAction("Login","Home");
                    }
                    else if(u.Role=="child" && role=="admin"){                    
                        TempData["message"]="sorry you are not admin";
                        return RedirectToAction("Login","Home");
                    }
                }
                
            }
        }
        TempData["message"]="Get Register First";
                return RedirectToAction("Login","Home");

        return View();
    }
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
