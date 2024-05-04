using ElectronicJournal.Models;
using ElectronicJournal.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using XAct.Users;
using XSystem.Security.Cryptography;

namespace ElectronicJournal.Controllers
{
    public class HomeController : Controller
    {

        private ApplicationDbContext db;

        public HomeController(ApplicationDbContext context)
        {
            db = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginModel loginModel)
        {
            if (ModelState.IsValid)
            {
                Models.User user = await db.Users.FirstOrDefaultAsync(u => u.UserName == loginModel.userName && u.Password == loginModel.password);
                if (user != null)
                {
                    await Authenticate(user.UserName, user.RoleId);
                    return RedirectToAction("Main");
                }
                ModelState.AddModelError("", "Некоректные логин или пароль");
            }
            return View(loginModel);
        }


        [HttpGet]
        public async Task<IActionResult> CreateUser()
        {
            ViewBag.Groups = await db.Groups.ToListAsync();
            ViewBag.Roles = await db.Roles.ToListAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(String userName, int? groupId, int roleId)
        {
            if (ModelState.IsValid)
            {
                Models.User user = await db.Users.FirstOrDefaultAsync(u => u.UserName == userName);
                if (user == null)
                {
                    string password = GetHash(userName);
                    if (roleId == 3)
                    {
                        await db.Users.AddAsync(new Models.User { UserName = userName, Password = password, GroupId = groupId, RoleId = roleId });
                    } else
                    {
                        await db.Users.AddAsync(new Models.User { UserName = userName, Password = password, RoleId = roleId });
                    }
                    await db.SaveChangesAsync();
                    return RedirectToAction("UsersList");
                } else
                {
                    ModelState.AddModelError("", "Пользователь с таким логином уже добавлен");
                }
            }
            return View();
        }

        public async Task<IActionResult> DeleteUser(int userId)
        {
            var grades = await db.Grades.Where(g => g.UserId == userId).ToListAsync();
            db.Grades.RemoveRange(grades);

            var subjects = await db.Subjects.Where(s => s.UserId == userId).ToListAsync();
            db.Subjects.RemoveRange(subjects);

            var user = await db.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user != null)
            {
                db.Users.Remove(user);
            }
            await db.SaveChangesAsync();
            return RedirectToAction("UsersList");
        }

        public async Task<IActionResult> DeleteSubject(int subjectId)
        {
            var grades = await db.Grades.Where(g => g.SubjectId == subjectId).ToListAsync();
            db.Grades.RemoveRange(grades);

            var gradeDates = await db.GradesDates.Where(g => g.SubjectId == subjectId).ToListAsync();
            db.GradesDates.RemoveRange(gradeDates);

            var subjectGroups = await db.subjectGroups.Where(g => g.SubjectNameId == subjectId).ToListAsync();
            db.subjectGroups.RemoveRange(subjectGroups);

            var subject = await db.Subjects.FirstOrDefaultAsync(s => s.Id == subjectId);
            if (subject != null)
            {
                db.Subjects.Remove(subject);
            }

            await db.SaveChangesAsync();
            return RedirectToAction("Main");
        }

        [HttpGet]
        public async Task<IActionResult> UsersList()
        {
            return View(await db.Users.ToListAsync());
        }

        public async Task<IActionResult> Main()
        {
            var allModels = new AllModels();
            if (User.IsInRole("Student"))
            {
                Models.User user = await db.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);
                allModels.SubjectGroupsList = await db.subjectGroups.Include(s => s.Subjects).Where(s => s.GroupId == user.GroupId).ToListAsync();
                allModels.SubjectsList = await db.Subjects.Where(s => s.UserId == user.Id).ToListAsync();
                return View(allModels);
            } else if (User.IsInRole("Teacher"))
            {
                Models.User user = await db.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);
                allModels.SubjectGroupsList = await db.subjectGroups.Where(s => s.GroupId == user.GroupId).ToListAsync();
                allModels.SubjectsList = await db.Subjects.Where(s => s.UserId == user.Id).ToListAsync();
                return View(allModels);
            } else
            {
                allModels.SubjectGroupsList = await db.subjectGroups.ToListAsync();
                allModels.SubjectsList = await db.Subjects.ToListAsync();
                return View(allModels);
            }
        }


        public async Task<IActionResult> SubjectJournal(int? groupId, int subjectId)
        {
            var allModels = new AllModels();
            allModels.SubjectsList = await db.Subjects.ToListAsync();
            allModels.RolesList = await db.Roles.ToListAsync();
            allModels.GroupsList = await db.Groups.ToListAsync();
            if (groupId == null)
            {
                Models.User user = await db.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);
                allModels.UsersList = await db.Users.Where(u => u.GroupId == user.GroupId).ToListAsync();
                allModels.DateList = await db.GradesDates.Where(d => d.GroupId == user.GroupId && d.SubjectId == subjectId).ToListAsync();
                allModels.GradesList = await db.Grades.Where(g => g.GroupId == user.GroupId && g.SubjectId == subjectId).ToListAsync();
                ViewData["groupId"] = user.GroupId;
                ViewData["dateCount"] = db.GradesDates.Where(g => g.GroupId == user.GroupId).Count() + 1;
            }
            else
            {
                allModels.DateList = await db.GradesDates.Where(d => d.GroupId == groupId && d.SubjectId == subjectId).ToListAsync();
                allModels.UsersList = await db.Users.Where(u => u.GroupId == groupId).ToListAsync();
                allModels.GradesList = await db.Grades.Where(g => g.GroupId == groupId && g.SubjectId == subjectId).ToListAsync();
                ViewData["groupId"] = groupId;
                ViewData["dateCount"] = db.GradesDates.Where(g => g.GroupId == groupId).Count() + 1;
            }
            ViewData["subjectId"] = subjectId;
            Console.WriteLine(subjectId);
            return View(allModels);
        }

        public async Task<IActionResult> GroupList(int subjectId)
        {
            ViewData["subjectId"] = subjectId;
            return View(await db.subjectGroups.Include(s => s.Group).Where(s => s.SubjectNameId == subjectId).ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> AddGroup(int subjectId)
        {
            ViewData["subjectId"] = subjectId;
            var allModels = new AllModels();
            allModels.GroupsList = await db.Groups.ToListAsync();
            return View(allModels);
        }

        public async Task<IActionResult> AddGroup(int? groupId, int? subjectId)
        {
            await db.subjectGroups.AddAsync(new SubjectGroup { GroupId = groupId, SubjectNameId = subjectId });
            await db.SaveChangesAsync();
            return RedirectToAction("GroupList", new { subjectId = subjectId});
        }


        public async Task<IActionResult> EditJournal(int? groupId, int subjectId)
        {
            var allModels = new AllModels();
            allModels.SubjectsList = await db.Subjects.ToListAsync();
            allModels.RolesList = await db.Roles.ToListAsync();
            allModels.GroupsList = await db.Groups.ToListAsync();
            allModels.DateList = await db.GradesDates.Where(d => d.GroupId == groupId && d.SubjectId == subjectId ).ToListAsync();
            if (groupId == null)
            {
                Models.User user = await db.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);
                allModels.UsersList = await db.Users.Where(u => u.GroupId == user.GroupId).ToListAsync();
                allModels.GradesList = await db.Grades.Where(g => g.GroupId == user.GroupId && g.SubjectId == subjectId).ToListAsync();
            }
            else
            {
                allModels.UsersList = await db.Users.Where(u => u.GroupId == groupId).ToListAsync();
                allModels.GradesList = await db.Grades.Where(g => g.GroupId == groupId && g.SubjectId == subjectId).ToListAsync();
            }
            ViewData["subjectId"] = subjectId;
            ViewData["groupId"] = groupId;
            ViewData["dateCount"] = db.GradesDates.Where(g => g.GroupId == groupId && g.SubjectId == subjectId).Count() + 1;
            return View(allModels);
        }

        public async Task Authenticate(string userName, int roleId)
        {
            Role role = await db.Roles.FirstOrDefaultAsync(r => r.Id == roleId);
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userName),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, role.RoleName)
            };
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> CreateSubject()
        {
            var allModels = new AllModels();
            allModels.SubjectsList = await db.Subjects.ToListAsync();
            allModels.UsersList = await db.Users.Where(u => u.RoleId == 2).ToListAsync();
            return View(allModels);
        }


        [HttpPost]
        public async Task<IActionResult> CreateSubject(String subjectName, int groupId, int userId)
        {
            if (ModelState.IsValid)
            {
                Subject subject = await db.Subjects.FirstOrDefaultAsync(s => s.SubjectName == subjectName && s.UserId == userId);
                if (subject == null)
                {
                    await db.Subjects.AddAsync(new Subject { SubjectName = subjectName, UserId = userId});
                    await db.SaveChangesAsync();
                    return RedirectToAction("Main");
                }
                ModelState.AddModelError("", "Предмет уже добавлен");
            }
            return View();
        }

        public string GetHash(string text)
        {
            byte[] bytes = Encoding.Unicode.GetBytes(text);
            MD5CryptoServiceProvider CSP = new MD5CryptoServiceProvider();
            byte[] byteHash = CSP.ComputeHash(bytes);
            string hash = string.Empty;
            foreach (byte b in byteHash)
            {
                hash += string.Format("{0:x2}", b);
            }
            return hash;
        }

        [HttpGet]
        public async Task<IActionResult> CreateGroup()
        {
            var allModels = new AllModels();
            allModels.GroupsList = await db.Groups.ToListAsync();
            return View(allModels);
        }


        [HttpPost]
        public async Task<IActionResult> CreateGroup(string groupName)
        {
            await db.Groups.AddAsync(new Group { GroupName = groupName});
            await db.SaveChangesAsync();
            return RedirectToAction("Main");
        }


        public async Task<IActionResult> AddDate(string date, int subjectId, int groupId)
        {
            await db.GradesDates.AddAsync(new GradesDate { Date = date, GroupId = groupId, SubjectId = subjectId });
            await db.SaveChangesAsync();
            foreach(var student in db.Users.Where(s => s.GroupId == groupId))
            {
                await db.Grades.AddAsync(new Grade { Date = date, GroupId = groupId, UserId = student.Id, SubjectId = subjectId });
            }
            await db.SaveChangesAsync();
            Console.WriteLine(db.GradesDates.Where(g => g.GroupId == groupId).Count() + 1);
            return RedirectToAction("EditJournal", new { groupId = groupId, subjectId = subjectId });
        }


        public JsonResult EditGrade(int gradeId, int grade)
        {
            var updateGrade = db.Grades.Where(g => g.Id == gradeId).AsQueryable().FirstOrDefault();
            updateGrade.grade = grade;
            db.Grades.UpdateRange(updateGrade);
            db.SaveChanges();
            return Json(true);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
