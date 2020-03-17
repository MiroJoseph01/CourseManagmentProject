using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using CourseManagment.Web.Models;
using CourseManagment.BLL.Services.Interfaces;
using System.Collections.Generic;
using CourseManagment.Web.Models.ManageViewModels;
using System.Threading;
using CourseManagment.BLL.DTOModels;
using AutoMapper;
using CourseManagment.DAL.Entities.Courses;
using CourseManagment.Web.Models.HomeViewModels;
using CourseManagment.BLL.Infrastructure;
using System.Data.Entity;
using System.IO;
using CourseManagment.DAL.Entities.Lecturers;
using CourseManagment.Web.Filters;

namespace CourseManagment.Web.Controllers
{
    [Authorize]
    public class ManageController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private IStudentService studentService;
        private ILecturerService lecturerService;
        private ICourseService courseService;
        private IProgressService progressService;
        private IDepartmentService departmentService;
        public ManageController(IStudentService std, ILecturerService lec, ICourseService cor,
            IProgressService prog, IDepartmentService dep)
        {
            studentService = std;
            lecturerService = lec;
            courseService = cor;
            progressService = prog;
            departmentService = dep;
        }

        public ManageController(ApplicationUserManager userManager, ApplicationSignInManager signInManager, IStudentService std,
            ILecturerService lec, ICourseService cor, IProgressService prog, IDepartmentService dep)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            studentService = std;
            courseService = cor;
            progressService = prog;
            departmentService = dep;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        //

        // GET: /Manage/Index

        [ExceptionLogger]
        [Authorize(Roles = "student, lecturer, admin")]
        public async Task<ActionResult> Index(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Ваш пароль изменен."
                : message == ManageMessageId.SetPasswordSuccess ? "Пароль задан."
                : message == ManageMessageId.SetTwoFactorSuccess ? "Настроен поставщик двухфакторной проверки подлинности."
                : message == ManageMessageId.Error ? "Произошла ошибка."
                : message == ManageMessageId.AddPhoneSuccess ? "Ваш номер телефона добавлен."
                : message == ManageMessageId.RemovePhoneSuccess ? "Ваш номер телефона удален."
                : "";



            var userId = User.Identity.GetUserId();

            ApplicationUserManager userManager = HttpContext.GetOwinContext()
                                            .GetUserManager<ApplicationUserManager>();
            var email = userManager.GetEmail(userId);
            IList<string> roles = new List<string>();
            if (!string.IsNullOrEmpty(userId))
            {
                roles = userManager.GetRoles(userId);
            }
            var model = new IndexViewModel
            {
                HasPassword = HasPassword(),
                PhoneNumber = await UserManager.GetPhoneNumberAsync(userId),
                TwoFactor = await UserManager.GetTwoFactorEnabledAsync(userId),
                Logins = await UserManager.GetLoginsAsync(userId),
                BrowserRemembered = await AuthenticationManager.TwoFactorBrowserRememberedAsync(userId),
            };

            if (roles.Contains("admin"))
            {
                model.Email = email;
                model.Role = "admin";
            }
            else if (roles.Contains("lecturer"))
            {
                LecturerDTO lecturer = lecturerService.GetLecturers().First(x => x.LecturerEmail == email);
                model.FirstName = lecturer.FirstName;
                model.SecondName = lecturer.SecondName;
                model.Department = lecturer.Department.DepartmentName;
                model.Description = lecturer.Information;
                model.Role = "lecturer";
                model.Id = lecturer.LecturerId;
                model.Email = lecturer.LecturerEmail;
                model.Image = lecturer.Image;
                model.ImageName = lecturer.ImageName;
            }
            else if (roles.Contains("student"))
            {
                StudentDTO student = studentService.GetStudents().First(x => x.StudentEmail == email);
                model.FirstName = student.FirstName;
                model.SecondName = student.SecondName;
                model.StudentTerm = student.StudentTerm;
                model.Description = "";
                model.Role = "student";
                model.Id = student.StudentId;
                model.Email = student.StudentEmail;
                model.isBanned = student.IsBanned;

            }

            return View(model);
        }



        //
        // POST: /Manage/RemoveLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveLogin(string loginProvider, string providerKey)
        {
            ManageMessageId? message;
            var result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(), new UserLoginInfo(loginProvider, providerKey));
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                message = ManageMessageId.RemoveLoginSuccess;
            }
            else
            {
                message = ManageMessageId.Error;
            }
            return RedirectToAction("ManageLogins", new { Message = message });
        }

        //
        // GET: /Manage/AddPhoneNumber
        public ActionResult AddPhoneNumber()
        {
            return View();
        }

        //
        // POST: /Manage/AddPhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddPhoneNumber(AddPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            // Создание и отправка маркера
            var code = await UserManager.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId(), model.Number);
            if (UserManager.SmsService != null)
            {
                var message = new IdentityMessage
                {
                    Destination = model.Number,
                    Body = "Ваш код безопасности: " + code
                };
                await UserManager.SmsService.SendAsync(message);
            }
            return RedirectToAction("VerifyPhoneNumber", new { PhoneNumber = model.Number });
        }

        //
        // POST: /Manage/EnableTwoFactorAuthentication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EnableTwoFactorAuthentication()
        {
            await UserManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId(), true);
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", "Manage");
        }

        //
        // POST: /Manage/DisableTwoFactorAuthentication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DisableTwoFactorAuthentication()
        {
            await UserManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId(), false);
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", "Manage");
        }

        //
        // GET: /Manage/VerifyPhoneNumber
        public async Task<ActionResult> VerifyPhoneNumber(string phoneNumber)
        {
            var code = await UserManager.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId(), phoneNumber);
            // Отправка SMS через поставщик SMS для проверки номера телефона
            return phoneNumber == null ? View("Error") : View(new VerifyPhoneNumberViewModel { PhoneNumber = phoneNumber });
        }

        //
        // POST: /Manage/VerifyPhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyPhoneNumber(VerifyPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await UserManager.ChangePhoneNumberAsync(User.Identity.GetUserId(), model.PhoneNumber, model.Code);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                return RedirectToAction("Index", new { Message = ManageMessageId.AddPhoneSuccess });
            }
            // Это сообщение означает наличие ошибки; повторное отображение формы
            ModelState.AddModelError("", "Не удалось проверить телефон");
            return View(model);
        }

        //
        // POST: /Manage/RemovePhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemovePhoneNumber()
        {
            var result = await UserManager.SetPhoneNumberAsync(User.Identity.GetUserId(), null);
            if (!result.Succeeded)
            {
                return RedirectToAction("Index", new { Message = ManageMessageId.Error });
            }
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", new { Message = ManageMessageId.RemovePhoneSuccess });
        }

        //
        // GET: /Manage/ChangePassword
        public ActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Manage/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                return RedirectToAction("Index", new { Message = ManageMessageId.ChangePasswordSuccess });
            }
            AddErrors(result);
            return View(model);
        }

        //
        // GET: /Manage/SetPassword
        public ActionResult SetPassword()
        {
            return View();
        }

        //
        // POST: /Manage/SetPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SetPassword(SetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
                if (result.Succeeded)
                {
                    var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                    if (user != null)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    }
                    return RedirectToAction("Index", new { Message = ManageMessageId.SetPasswordSuccess });
                }
                AddErrors(result);
            }

            // Это сообщение означает наличие ошибки; повторное отображение формы
            return View(model);
        }

        //
        // GET: /Manage/ManageLogins
        public async Task<ActionResult> ManageLogins(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.RemoveLoginSuccess ? "Внешнее имя входа удалено."
                : message == ManageMessageId.Error ? "Произошла ошибка."
                : "";
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user == null)
            {
                return View("Error");
            }
            var userLogins = await UserManager.GetLoginsAsync(User.Identity.GetUserId());
            var otherLogins = AuthenticationManager.GetExternalAuthenticationTypes().Where(auth => userLogins.All(ul => auth.AuthenticationType != ul.LoginProvider)).ToList();
            ViewBag.ShowRemoveButton = user.PasswordHash != null || userLogins.Count > 1;
            return View(new ManageLoginsViewModel
            {
                CurrentLogins = userLogins,
                OtherLogins = otherLogins
            });
        }

        //
        // POST: /Manage/LinkLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LinkLogin(string provider)
        {
            // Запрос перенаправления к внешнему поставщику входа для связывания имени входа текущего пользователя
            return new AccountController.ChallengeResult(provider, Url.Action("LinkLoginCallback", "Manage"), User.Identity.GetUserId());
        }

        //
        // GET: /Manage/LinkLoginCallback
        public async Task<ActionResult> LinkLoginCallback()
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync(XsrfKey, User.Identity.GetUserId());
            if (loginInfo == null)
            {
                return RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
            }
            var result = await UserManager.AddLoginAsync(User.Identity.GetUserId(), loginInfo.Login);
            return result.Succeeded ? RedirectToAction("ManageLogins") : RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
            }
            studentService.Dispose();
            lecturerService.Dispose();
            courseService.Dispose();
            progressService.Dispose();
            departmentService.Dispose();
            base.Dispose(disposing);
        }

        #region Вспомогательные приложения
        // Используется для защиты от XSRF-атак при добавлении внешних имен входа
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private bool HasPassword()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

        private bool HasPhoneNumber()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PhoneNumber != null;
            }
            return false;
        }

        public enum ManageMessageId
        {
            AddPhoneSuccess,
            ChangePasswordSuccess,
            SetTwoFactorSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            RemovePhoneSuccess,
            Error
        }

        #endregion

        //Shows Courses to Student
        [ExceptionLogger]
        public PartialViewResult Courses(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new ValidationException("id is null", "");
            var courses = studentService.GetStudent(Convert.ToInt32(id)).Courses;
            if (courses != null)
            {
                ViewBag.Error = "";
                var config = new MapperConfiguration(cfg => cfg.CreateMap<Course, CourseDTO>()
                   .ForMember("NumberOfStudents", opt => opt.MapFrom(c => c.Students.Count)));
                var mapper = new Mapper(config);
                var courseDtos = mapper.Map<List<CourseDTO>>(courses);

                foreach (var i in courseDtos)
                {
                    if (i.CourseEnd < DateTime.Now)
                    {
                        i.Position = -1;
                        var progress = progressService.GetProgressByStudentIdAndCourseId(Convert.ToInt32(id), i.CourseId);
                        if (progress != null)
                            i.Mark = progress.GradeBookMark;
                        else
                            i.Mark = null;
                    }

                    else if (i.CourseEnd >= DateTime.Now && i.CourseStart <= DateTime.Now)
                    {
                        i.Position = 0;
                        i.Mark = null;
                    }

                    else
                    {


                        i.Position = 1;
                        i.Mark = null;
                    }
                }

                var coursesOrdered = courseDtos.OrderByDescending(x => x.Position).ThenBy(x => x.CourseStart);
                List<CoursesForStudents> resCourses = new List<CoursesForStudents>();
                foreach (var i in coursesOrdered)
                {
                    resCourses.Add(new CoursesForStudents
                    {
                        CourseName = i.CourseName,
                        Lecturer = i.Lecturer,
                        CourseStart = i.CourseStart.ToString().Split(' ')[0],
                        CourseEnd = i.CourseEnd.ToString().Split(' ')[0],
                        CourseId = i.CourseId,
                        Position = i.Position,
                        ForTerm = i.ForTerm,
                        IsDeleted = i.IsDeleted,
                        Mark = i.Mark
                    });
                }

                return PartialView(resCourses);
            }
            else
            {
                ViewBag.Error = "You don't have courses yet";
                return PartialView();
            }
        }

        //Allows Student to join Courses up
        [ExceptionLogger]
        [HttpGet]
        public PartialViewResult AddCourses(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new ValidationException("id is null", "");
            var student = studentService.GetStudent(Convert.ToInt32(id));
            var studentsCourses = student.Courses;

            var config = new MapperConfiguration(cfg => cfg.CreateMap<Course, CourseDTO>()
                  .ForMember("NumberOfStudents", opt => opt.MapFrom(c => c.Students.Count)));
            var mapper = new Mapper(config);
            var courseDtosOfStudent = mapper.Map<List<CourseDTO>>(studentsCourses);

            if (student == null)
                throw new ValidationException("Student with tihis id doesn't exist", "");

            var courses = courseService.GetCourses().ToList();

            List<CourseAddForStudent> res = new List<CourseAddForStudent>();

            List<int> idC = new List<int>();
            List<int> idCS = new List<int>();
            foreach (var i in courses)
            {
                idC.Add(i.CourseId);
            }

            foreach (var i in courseDtosOfStudent)
            {
                idCS.Add(i.CourseId);
            }

            List<int> idRes = idC.Except(idCS).ToList();


            foreach (var i in courses)
            {
                if (idRes.Contains(i.CourseId)
                    && i.ForTerm == student.StudentTerm
                    && i.CourseStart >= DateTime.Now
                    && i.IsDeleted == false)
                {
                    res.Add(new CourseAddForStudent
                    {
                        CourseId = i.CourseId,
                        CourseName = i.CourseName,
                        Lecturer = i.Lecturer,
                        Agree = false,
                        CourseStart = i.CourseStart.ToString().Split(' ')[0],
                        CourseEnd = i.CourseEnd.ToString().Split(' ')[0]
                    });
                }
            }

            CourseAddForStudentID resId = new CourseAddForStudentID
            {
                Id = Convert.ToInt32(id),
                Courses = res
            };

            return PartialView(resId);

        }

        //Shows result if Student has joined up
        [ExceptionLogger]
        [HttpPost]
        public PartialViewResult AddCourses(FormCollection form)
        {

            if (string.IsNullOrEmpty(form["StudentId"]))
                throw new ValidationException("id is null", "");
            if (string.IsNullOrEmpty(form["CourseId"]))
                throw new ValidationException("id is null", "");

            int studentId = Convert.ToInt32(form["StudentId"]);
            int courseId = Convert.ToInt32(form["CourseId"]);
            var student = studentService.GetStudent(studentId);
            var course = courseService.GetCourse(courseId);

            studentService.AddCourse(studentId, courseId);

            Thread.Sleep(1000);
            var studentsCourses = student.Courses;

            var config = new MapperConfiguration(cfg => cfg.CreateMap<Course, CourseDTO>()
                  .ForMember("NumberOfStudents", opt => opt.MapFrom(c => c.Students.Count)));
            var mapper = new Mapper(config);
            var courseDtosOfStudent = mapper.Map<List<CourseDTO>>(studentsCourses);

            if (student == null)
                throw new ValidationException("Student with tihis id doesn't exist", "");

            var courses = courseService.GetCourses().ToList();

            List<CourseAddForStudent> res = new List<CourseAddForStudent>();

            List<int> idC = new List<int> { };
            List<int> idCS = new List<int> { courseId };
            foreach (var i in courses)
            {
                idC.Add(i.CourseId);
            }

            foreach (var i in courseDtosOfStudent)
            {
                idCS.Add(i.CourseId);
            }

            List<int> idRes = idC.Except(idCS).ToList();


            foreach (var i in courses)
            {
                if (idRes.Contains(i.CourseId)
                    && i.ForTerm == student.StudentTerm
                    && i.CourseStart >= DateTime.Now
                    && i.IsDeleted == false)
                {
                    res.Add(new CourseAddForStudent
                    {
                        CourseId = i.CourseId,
                        CourseName = i.CourseName,
                        Lecturer = i.Lecturer,
                        Agree = false,
                        CourseStart = i.CourseStart.ToString().Split(' ')[0],
                        CourseEnd = i.CourseEnd.ToString().Split(' ')[0]
                    });
                }
            }

            CourseAddForStudentID resId = new CourseAddForStudentID
            {
                Id = Convert.ToInt32(studentId),
                Courses = res
            };



            ViewBag.Info = "Successfully! You will see this course on your profile page with your next visit!";


            return PartialView(resId);

        }

        //Shows to Lecturer his Courses
        [ExceptionLogger]
        [HttpGet]
        public PartialViewResult CoursesForLecturer(int id)
        {
            var courses = courseService.GetCourses().Where(x => x.Lecturer.LecturerId == id);
            if (courses == null)
                throw new ValidationException("Courses don't exist", "");
            List<CoursesForLecturer> coursesView = new List<CoursesForLecturer>();
            foreach (var i in courses)
            {
                int position;
                bool isMarked;
                if (i.CourseEnd < DateTime.Now
                    && i.CourseEnd.AddDays(5) > DateTime.Now)
                {
                    position = -2;

                }
                else if (i.CourseStart > DateTime.Now)
                {
                    position = 1;
                    isMarked = true;
                }
                else if (i.CourseStart < DateTime.Now && i.CourseEnd > DateTime.Now)
                {
                    position = 0;
                }
                else if (i.CourseEnd < DateTime.Now)
                {
                    position = -1;
                }
                else
                {
                    position = -10;
                    isMarked = false;
                }
                coursesView.Add(new CoursesForLecturer
                {
                    CourseId = i.CourseId,
                    CourseName = i.CourseName,
                    CourseStart = i.CourseStart,
                    CourseEnd = i.CourseEnd,
                    NumberOfStudents = i.NumberOfStudents,
                    Position = position
                });

            }
            return PartialView(coursesView);
        }

        // GET: /Manage/MarksOfCourse
        //Shows page to rate Students of current Course
        [ExceptionLogger]
        public ActionResult MarksOfCourse(int? id, int? position)
        {
            if (id == null || position == null)
                throw new ValidationException("Id or Position don't exist", "");
            if (position < -2 || position > 1)
                return HttpNotFound();
            var course = courseService.GetCourse(id);
           

            List<StudentMark> studentMarks = new List<StudentMark>();
            foreach (var i in course.Students)
            {
                int? Mark;
                try { Mark = progressService.GetProgressByStudentIdAndCourseId(i.StudentId, id).GradeBookMark; }
                catch { Mark = null; }

                studentMarks.Add(new StudentMark
                {
                    StudentId = i.StudentId,
                    StudentName = i.FirstName + " " + i.SecondName,
                    Mark = Mark
                });
            }


            return View(new Marks
            {
                CourseId = id.Value,
                CourseName = course.CourseName,
                Students = studentMarks,
                CourseEnd = course.CourseEnd,
                CourseStart = course.CourseStart,
                Position = position.Value
            });
        }

        //POST: /Manage/MarksOfCourse
        //Process information about Course marks
        [ExceptionLogger]
        [HttpPost]
        public ActionResult MarksOfCourse(FormCollection form)
        {

            var courseId = Convert.ToInt32(form["CourseId"]);
            var studentId = form["StudentId"].Split(',');
            var marks = form["StudentMark"].Split(',');

            Dictionary<int, int> stdId = new Dictionary<int, int>();
            for (int i = 0; i < studentId.Count(); i++)
            {
                var mark = Convert.ToInt32(marks[i]);
                if (mark > 100 || mark < 0)
                {
                    throw new ValidationException("Out of range", "");
                }
                stdId.Add(Convert.ToInt32(studentId[i]), Convert.ToInt32(mark));
            }
            foreach (var i in stdId)
            {
                progressService.Rate(i.Key, courseId, i.Value);
            }



            return Redirect("/Manage/Index");
        }


        public PartialViewResult Departments()
        {
            ViewBag.Info = "";
            return PartialView();
        }

        //Create new Department
        [ExceptionLogger]
        [HttpPost]
        public PartialViewResult Departments(DepartmentDTO departmentDTO)
        {
            if (ModelState.IsValid)
            {
                ViewBag.Info = "Successfully!";
                departmentService.CreateDepartment(departmentDTO);
                departmentDTO.DepartmentName = "";
                return PartialView(departmentDTO);
            }
            else
                return PartialView(departmentDTO);
        }

        //Shows form to delete Departments
        public PartialViewResult DelDep()
        {

            var result = departmentService.GetDepartments().ToList();
            ViewDelDep res = new ViewDelDep { departments = result };
            ViewBag.del = "";
            return PartialView(res);
        }

        //Check if it's possible to delete Department and do it if it's possible
        [HttpPost]
        public PartialViewResult DelDep(ViewDelDep viewDelDep)
        {
            try
            {
                departmentService.DeleteDepartment(viewDelDep.SelectedDepId);
                ViewBag.del = "Successfully!";
                viewDelDep.departments = departmentService.GetDepartments().ToList();
            }
            catch
            {
                ModelState.AddModelError("DepartmentId", "Can't Delete this Department because it's in use");
                viewDelDep.departments = departmentService.GetDepartments().ToList();
            }
            if (ModelState.IsValid)
            {
                return PartialView(viewDelDep);
            }
            else
                return PartialView(viewDelDep);

        }

        //GET: /Manage/CreateLec
        //Upload page to create Lecturer
        [ExceptionLogger]
        public ActionResult CreateLec()
        {
            var result = departmentService.GetDepartments().ToList();
            CreateLec res = new CreateLec { departments = result };
            ViewBag.CL = "";
            return View(res);
        }

        //POST: /Manage/CreateLec
        //Check if it's possible to create Lecturer and do it if it's possible
        [ExceptionLogger]
        [HttpPost]
        public async Task<ActionResult> CreateLec(CreateLec createLec, HttpPostedFileBase uploadImage)
        {
            createLec.departments = departmentService.GetDepartments().ToList();
            if (uploadImage == null)
                ModelState.AddModelError("Image", "Add image");
            if (ModelState.IsValid)
            {
                byte[] imageData = null;
                using (var binaryReader = new BinaryReader(uploadImage.InputStream))
                {
                    imageData = binaryReader.ReadBytes(uploadImage.ContentLength);
                }
                createLec.Image = imageData;


                var user = new ApplicationUser { UserName = createLec.FirstName + " " + createLec.SecondName, Email = createLec.LecturerEmail };
               



                var result = await UserManager.CreateAsync(user, createLec.Password);
                if (result.Succeeded)
                {
                    await UserManager.AddToRoleAsync(user.Id, "lecturer");

                    lecturerService.AddLecturer(new BLL.DTOModels.LecturerDTO
                    {
                        FirstName = createLec.FirstName,
                        SecondName = createLec.SecondName,
                        LecturerEmail = createLec.LecturerEmail,
                        Image = createLec.Image,
                        ImageName = createLec.FirstName + " " + createLec.SecondName,
                        Information = createLec.Information
                    }, createLec.SelectedDepId);
                    return Redirect("/Manage/Index");
                }
                AddErrors(result);
            }
            return View(createLec);

        }

        //GET: /Manage/CreateCour
        //Upload page to create Course
        [ExceptionLogger]
        public ActionResult CreateCour()
        {
            var result = lecturerService.GetLecturers().ToList();
            CreateCourse res = new CreateCourse { lecturers = result };
            return View(res);
        }

        //POST: /Manage/CreateCour
        //Check if it's possible to create Course and do it if it's possible
        [ExceptionLogger]
        [HttpPost]
        public ActionResult CreateCour(CreateCourse createCourse, HttpPostedFileBase uploadImage)
        {

            createCourse.lecturers = lecturerService.GetLecturers().ToList();
            if (uploadImage == null)
                ModelState.AddModelError("ImageName", "Add image");
            if (ModelState.IsValid)
            {
                byte[] imageData = null;
                using (var binaryReader = new BinaryReader(uploadImage.InputStream))
                {
                    imageData = binaryReader.ReadBytes(uploadImage.ContentLength);
                }
                createCourse.Image = imageData;

                var lec = lecturerService.GetLecturer(createCourse.SelectedLecId);
                Lecturer res = new Lecturer
                {
                    LecturerId = lec.LecturerId,
                    Courses = lec.Courses,
                    Department = lec.Department,
                    FirstName = lec.FirstName,
                    SecondName = lec.SecondName,
                    Image = lec.Image,
                    ImageName = lec.ImageName,
                    Information = lec.Information,
                    LecturerEmail = lec.LecturerEmail
                };

                try
                {
                    courseService.AddCourse(new CourseDTO
                    {
                        CourseName = createCourse.CourseName,
                        Description = createCourse.Description,
                        CourseStart = createCourse.CourseStart,
                        CourseEnd = createCourse.CourseEnd,
                        ForTerm = createCourse.ForTerm,
                        Image = createCourse.Image,
                        ImageName = createCourse.CourseName,
                        IsDeleted = createCourse.IsDeleted,
                        Lecturer=res
                    }, createCourse.SelectedLecId,  createCourse.Topics);
                }
                catch (ValidationException ex)
                {
                    ModelState.AddModelError("CourseStart", ex.Message);
                }
                if (ModelState.IsValid)
                    return Redirect("/Manage/Index");
                else
                {
                    createCourse.lecturers = lecturerService.GetLecturers().ToList();
                    return View(createCourse);
                }

            }
            return View(createCourse);

        }

        //GET: /Manage/CoursesForEdit
        //Upload page all Courses to choose one for editing
        [ExceptionLogger]
        public ActionResult CoursesForEdit()
        {
            var coursesDTO = courseService.GetCourses();
            List<LecturerForEdit> res = new List<LecturerForEdit>();
            foreach (var i in coursesDTO)
            {
                res.Add(new LecturerForEdit
                {
                    CourseId = i.CourseId,
                    CourseName = i.CourseName,
                    Lecturer = i.Lecturer
                });
            }
            res.OrderBy(x => x.CourseName);
            return View(res);
        }

        //GET: /Manage/EditCourse
        //Upload page to edit Course
        public ActionResult EditCourse(int? id)
        {
            if (id == null)
                throw new ValidationException("id doesn't exist", "");

            var course = courseService.GetCourse(id.Value);
            if (course == null)
                throw new ValidationException("course doesn't exist", "");
            string res = "";
            foreach (var i in course.Topics)
            {
                res += i.TopicName+",";
            }
            int resLen = res.Length - 1;
            var topics = res.Substring(0, resLen);
            var lec = lecturerService.GetLecturers().ToList();

            EditCours editCourse = new EditCours
            {
                CourseId = id.Value,
                CourseName = course.CourseName,
                CourseStart = course.CourseStart,
                CourseEnd = course.CourseEnd,
                Description = course.Description,
                ForTerm = course.ForTerm,
                Image = course.Image,
                ImageName = course.ImageName,
                IsDeleted = course.IsDeleted,
                Topics = topics,
                lecturers = lec
            };

            return View(editCourse);
        }

        //POST: /Manage/EditCourse
        //Check if it's possible to edit Course and do it if it's possible
        [HttpPost]
        [ExceptionLogger]
        public ActionResult EditCourse(EditCours editCourse, HttpPostedFileBase uploadImage)
        {
            editCourse.lecturers = lecturerService.GetLecturers().ToList();
            
            if (ModelState.IsValid)
            {
                byte[] img;
                if (uploadImage == null)
                {
                    img = courseService.GetCourse(editCourse.CourseId).Image;
                }
                else
                {
                    byte[] imageData = null;
                    using (var binaryReader = new BinaryReader(uploadImage.InputStream))
                    {
                        imageData = binaryReader.ReadBytes(uploadImage.ContentLength);
                    }
                    img = imageData;
                }



                try
                {
                    courseService.EditCourse(new CourseDTO
                    {
                        CourseId=editCourse.CourseId,
                        CourseName = editCourse.CourseName,
                        Description = editCourse.Description,
                        CourseStart = editCourse.CourseStart,
                        CourseEnd = editCourse.CourseEnd,
                        ForTerm = editCourse.ForTerm,
                        Image = img,
                        ImageName = editCourse.CourseName,
                        IsDeleted = editCourse.IsDeleted,
                        
                    }, editCourse.SelectedLecId, editCourse.Topics);
                }
                catch (ValidationException ex)
                {
                    ModelState.AddModelError("CourseStart", ex.Message);
                }
                if (ModelState.IsValid)
                    return Redirect("/Manage/Index");
                else
                {
                    editCourse.lecturers = lecturerService.GetLecturers().ToList();
                    return View(editCourse);
                }

            }
            return View(editCourse);

        }

        //GET: /Manage/LecturersForEdit
        //Upload page all Lecturers to choose one for editing
        [ExceptionLogger]
        public ActionResult LecturersForEdit()
        {
            var lecturersDTO = lecturerService.GetLecturers();
            List<LecturersForEdit> res = new List<LecturersForEdit>();
            foreach (var i in lecturersDTO)
            {
                res.Add(new LecturersForEdit
                {
                    LecturerId = i.LecturerId,
                    LecturerName = i.FirstName + " " + i.SecondName,
                    Department = i.Department
                });
            }
            res.OrderBy(x => x.LecturerName);
            return View(res);
        }

        //GET: /Manage/EditLec
        //Upload page to edit Lecturer
        public ActionResult EditLec(int? id)
        {
            if (id == null)
                throw new ValidationException("id doesn't exist", "");

            var lec = lecturerService.GetLecturer(id.Value);
            if (lec == null)
                throw new ValidationException("course doesn't exist", "");
            string res = "";

            var dep = departmentService.GetDepartments();
            EditLec editLec= new EditLec
            {
                LecturerId = id.Value,
                FirstName = lec.FirstName,
                SecondName=lec.SecondName,
                Image=lec.Image,
                ImageName=lec.ImageName,
                Information=lec.Information,
                departments=dep.ToList(),
                LecturerEmail=lec.LecturerEmail,
                
            };

            return View(editLec);
        }

        //POST: /Manage/EditLec
        //Upload page to edit Lecturer
        [HttpPost]
        public ActionResult EditLec(EditLec editLec, HttpPostedFileBase uploadImage)
        {
            editLec.departments = departmentService.GetDepartments().ToList();

            if (ModelState.IsValid)
            {
                byte[] img;
                if (uploadImage == null)
                {
                    img = courseService.GetCourse(editLec.LecturerId).Image;
                }
                else
                {
                    byte[] imageData = null;
                    using (var binaryReader = new BinaryReader(uploadImage.InputStream))
                    {
                        imageData = binaryReader.ReadBytes(uploadImage.ContentLength);
                    }
                    img = imageData;
                }


                ApplicationUserManager userManager = HttpContext.GetOwinContext()
                                            .GetUserManager<ApplicationUserManager>();
                try
                {
                    lecturerService.EditLecturer(new LecturerDTO
                    {
                        LecturerId = editLec.LecturerId,
                        Information = editLec.Information,
                        FirstName = editLec.FirstName,
                        SecondName = editLec.SecondName,
                        Image = img,
                        ImageName=editLec.FirstName+" "+editLec.SecondName,
                        LecturerEmail=editLec.LecturerEmail

                    }, editLec.SelectedDepId);
                    var user = UserManager.FindByEmail(editLec.LecturerEmail);
                    user.UserName = editLec.FirstName + " " + editLec.SecondName;
                    userManager.Update(user);
                }
                catch (ValidationException ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
                if (ModelState.IsValid)
                    return Redirect("/Manage/Index");
                else
                {
                    editLec.departments = departmentService.GetDepartments().ToList();
                    return View(editLec);
                }

            }
            return View(editLec);


        }

        //GET: /Manage/BanStudent
        //Upload page to Block/Unblock Stiudents
        [ExceptionLogger]
        public ActionResult BanStudent()
        {
            var std = studentService.GetStudents().ToList();
            List<BanStudentView> res = new List<BanStudentView>();
            foreach (var i in std)
            {
                res.Add(new BanStudentView
                {
                    StudentName = i.FirstName + " " + i.SecondName,
                    StudentId = i.StudentId,
                    isBanned = i.IsBanned,
                    Term = i.StudentTerm
                });
            }
            res.OrderBy(x => x.isBanned).ThenBy(x => x.StudentName);
            return View(res);
        }

        //POST: /Manage/BanStudent
        //Process information and change information about Student
        [ExceptionLogger]
        [HttpPost]
        public ActionResult BanStudent(FormCollection form)
        {
            if (string.IsNullOrEmpty(form["StudentId"]) || string.IsNullOrEmpty(form["Pos"]))
                throw new ValidationException("id or vale doesn't exist","");
            int stdId = Convert.ToInt32(form["StudentId"]);
            int pos= Convert.ToInt32(form["Pos"]);
            if (studentService.GetStudent(stdId) != null)
            {
                if(pos==1)
                 studentService.BlockUnblock(stdId, true); 
                else
                 studentService.BlockUnblock(stdId, false);

            }
            
            var std = studentService.GetStudents().ToList();
            
            List<BanStudentView> res = new List<BanStudentView>();
            foreach (var i in std)
            {
                res.Add(new BanStudentView
                {
                    StudentName = i.FirstName + " " + i.SecondName,
                    StudentId = i.StudentId,
                    isBanned = i.IsBanned,
                    Term = i.StudentTerm
                });
            }
            res.OrderBy(x => x.isBanned).ThenBy(x => x.StudentName);
            return View(res);

        }

        //GET: /Manage/Logs
        //Upload page with information about Logs
        public ActionResult Logs()
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                return View(db.ExceptionDetails.OrderByDescending(x => x.Date).ToList());
            }

        }

       
    }
}