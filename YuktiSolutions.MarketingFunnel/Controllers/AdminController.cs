using Google.Apis.AnalyticsReporting.v4;
using Google.Apis.AnalyticsReporting.v4.Data;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using YuktiSolutions.MarketingFunnel.Models;
using YuktiSolutions.MarketingFunnel.Models.Database;
namespace YuktiSolutions.MarketingFunnel.Controllers
{
    [Compress]
    //[Authorize(Roles ="Admin")]
    [Authorize]
    public class AdminController : BaseController
    {

        #region Dashboard
        [AllowAnonymousAttribute]
        [Route("~/admin/index")]
        public ActionResult Index()
        {
            var getGAList = context.AnalyticsCredentials.OrderByDescending(m => m.CreatedOn).ToList();
            return View(getGAList);
        }
        #endregion

        #region Team
        [Route("~/admin/users")]
        public ActionResult Users()
        {
            return View();
        }
        [HttpGet]
        [Route("~/admin/CreateUser")]
        public ActionResult CreateUser(Guid? ID)
        {
            Models.UI.UserModel user;
            if (ID.HasValue)
            {
                String userID = ID.Value.ToString();
                var storedUser = context.Users.FirstOrDefault(x => x.Id.Equals(userID, StringComparison.CurrentCultureIgnoreCase));

                IdentityRole userRole = null;
                if (storedUser.Roles.Any())
                {
                    String roleId = storedUser.Roles.FirstOrDefault().RoleId;
                    userRole = context.Roles.FirstOrDefault(x => x.Id.Equals(roleId, StringComparison.CurrentCultureIgnoreCase));
                }

                user = new Models.UI.UserModel
                {
                    ID = Guid.Parse(storedUser.Id),
                    DisplayName = storedUser.DisplayName,
                    Email = storedUser.Email,
                    IsActive = storedUser.IsActive,
                    IsPasswordChanged = false,
                    PhoneNumber = storedUser.PhoneNumber,
                    Role = userRole == null ? "Data Entry Executive" : userRole.Name
                };
            }
            else
            {
                user = new Models.UI.UserModel();
                user.IsPasswordChanged = true;
                user.IsActive = true;
                user.Role = "Data Entry Executive";//Default role.
                user.ID = Guid.NewGuid();
            }
            return View(user);
        }

        [HttpPost]
        [Route("~/admin/CreateUser")]
        public ActionResult CreateUser(Models.UI.UserModel userInfo)
        {
            String userId = userInfo.ID.ToString();
            var user = context.Users.FirstOrDefault(x => x.Id.Equals(userId, StringComparison.CurrentCultureIgnoreCase));
            var role = context.Roles.FirstOrDefault(x => x.Name.Equals(userInfo.Role, StringComparison.CurrentCultureIgnoreCase));
            try
            {
                if (user == null)
                {
                    if (context.Users.Any(x => x.Email == userInfo.Email && x.UserName == userInfo.Email))
                    {
                        return Json(new Models.AjaxResponse { Success = false, Message = "Email already exists. Duplicate Email are not allowed." });
                    }
                    if (!string.IsNullOrEmpty(userInfo.Email))
                    {
                        if (string.IsNullOrEmpty(userInfo.Password))
                        {

                            return Json(new Models.AjaxResponse { Success = false, Message = "Password can not be blank." });
                        }
                    }
                    //Create new user
                    user = context.Users.Add(new ApplicationUser
                    {
                        Id = userInfo.ID.ToString(),
                        DisplayName = userInfo.DisplayName,
                        Email = userInfo.Email,
                        UserName = userInfo.Email,
                        PasswordHash = (new PasswordHasher()).HashPassword(userInfo.Password),
                        SecurityStamp = Guid.NewGuid().ToString(),
                        LockoutEnabled = false,
                        PhoneNumber = userInfo.PhoneNumber,
                        EmailConfirmed = true,
                        PhoneNumberConfirmed = true,
                        IsActive = userInfo.IsActive
                    });
                    user.Roles.Add(new Microsoft.AspNet.Identity.EntityFramework.IdentityUserRole { UserId = user.Id, RoleId = role.Id });
                    /*Alert should be raised to inform the admin users.*/
                    Models.EmailManager.SendAlert(String.Format("New user has been created :{0} {1} ", user.DisplayName, user.Email), String.Format("A new user has been created with the name:{0}, email:{1} by {2}", user.DisplayName, user.Email, User.Identity.GetUserName()));
                }
                else
                {
                    //Update existing user.
                    user.DisplayName = userInfo.DisplayName;
                    user.Email = userInfo.Email;
                    user.UserName = userInfo.Email;
                    user.PhoneNumber = userInfo.PhoneNumber;
                    user.IsActive = userInfo.IsActive;
                    if (user.Roles.Any(x => x.RoleId.Equals(role.Id, StringComparison.CurrentCultureIgnoreCase)) == false)
                    {
                        user.Roles.Clear(); //Remove all previous roles.
                        //Assign new role.
                        user.Roles.Add(new Microsoft.AspNet.Identity.EntityFramework.IdentityUserRole { UserId = user.Id, RoleId = role.Id });
                    }
                    if (userInfo.IsPasswordChanged == true)
                    {
                        if (string.IsNullOrEmpty(userInfo.Password))
                        {
                            return Json(new Models.AjaxResponse { Success = false, Message = "Password can not be blank." });
                        }
                        user.PasswordHash = (new PasswordHasher()).HashPassword(userInfo.Password);
                    }
                }
                context.SaveChanges();
                return Json(new Models.AjaxResponse { Success = true, Message = "User saved successfully." });
            }
            catch (DbUpdateException)
            {
                return Json(new Models.AjaxResponse { Success = false, Message = "User already exists." });
            }
            catch (Exception ex)
            {
                return Json(new Models.AjaxResponse { Success = false, Message = ex.InnerException == null ? ex.Message : ex.InnerException.Message });
            }
        }
        [HttpPost]
        [Route("~/admin/DeleteUsers")]
        public ActionResult DeleteUsers(String IDs, String AllocateTo)
        {
            String[] selectedIDs = IDs.Split(new String[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
            List<Guid> recordIDs = new List<Guid>();
            foreach (var item in selectedIDs)
            {
                recordIDs.Add(Guid.Parse(item));
            }
            /*First of all move all the data of the users being deleted to another user and then delete the user. It should be done in one transaction.*/
            try
            {
                var users = context.Users.Where(x => selectedIDs.Contains(x.Id));
                foreach (var user in users)
                {
                    //user.Roles.Clear();
                    context.Users.Remove(user);
                }
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                return Json(new Models.AjaxResponse { Success = false, Message = ex.InnerException == null ? ex.Message : ex.InnerException.Message });
            }
            return Json(new Models.AjaxResponse { Success = true, Message = "Records deleted successfully" });
        }
        [Route("~/admin/ReadOtherUsers")]
        public ActionResult ReadOtherUsers(String SelectedUsers)
        {
            String[] selectedIDs = SelectedUsers.Split(new String[] { ";" }, StringSplitOptions.RemoveEmptyEntries);

            return Json(context.Users.Where(x => selectedIDs.Contains(x.Id) == false).Select(x => new Models.UI.UserListItem
            {
                ID = x.Id,
                Email = x.Email,
                PhoneNumber = x.PhoneNumber,
                Name = x.DisplayName == null ? x.Email : x.DisplayName,
                Roles = context.Roles.Where(y => x.Roles.FirstOrDefault().RoleId.Equals(y.Id, StringComparison.CurrentCultureIgnoreCase)).Select(a => a.Name).FirstOrDefault()
            }), JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        [Route("~/admin/Users_Read")]
        public ActionResult Users_Read([DataSourceRequest] DataSourceRequest request, String SearchText)
        {
            var query = context.Users.Select(x => new Models.UI.UserListItem
            {
                ID = x.Id,
                Email = x.Email,
                PhoneNumber = x.PhoneNumber,
                Name = x.DisplayName == null ? x.Email : x.DisplayName,
                Roles = context.Roles.Where(y => x.Roles.FirstOrDefault().RoleId.Equals(y.Id, StringComparison.CurrentCultureIgnoreCase)).Select(a => a.Name).FirstOrDefault()
            });
            if (String.IsNullOrEmpty(SearchText) == false)
            {
                query = query.Where(x => x.Name.Contains(SearchText) || x.Email.Contains(SearchText) || x.PhoneNumber.Contains(SearchText) || x.Roles.Contains(SearchText));
            }
            return Json(query.ToDataSourceResult(request));
        }
        #endregion


        #region Google Analytics
        [Route("~/admin/add-new-project")]
        public ActionResult AddNewGAProject()
        {
            return View();
        }
        public ActionResult CreateAnalyticsCredential(Guid? ID)
        {
            Models.Database.AnalyticsCredential AnalyticsCredential = new Models.Database.AnalyticsCredential();
            if (ID.HasValue)
            {
                AnalyticsCredential = context.AnalyticsCredentials.FirstOrDefault(x => x.ID == ID.Value);
            }
            else
            {
                AnalyticsCredential.ID = Guid.NewGuid();
            }
            return View(AnalyticsCredential);
        }


        public ActionResult SaveAnalyticsCredential(Models.Database.AnalyticsCredential analyticsCredential, HttpPostedFileBase GAP12File, HttpPostedFileBase GAJsonFile)
        {
            try
            {
                if (GAP12File.ContentLength < 0)
                {
                    return Json(new Models.AjaxResponse { Success = false, Message = "Please select .p12 extension file." });
                }
                else
                {
                    string getGAP12FileExtention = Path.GetExtension(GAP12File.FileName);
                    if (!getGAP12FileExtention.Equals(".p12", StringComparison.OrdinalIgnoreCase))
                    {
                        return Json(new Models.AjaxResponse { Success = false, Message = "Please choose .p12 extension file." });
                    }
                    string path = Server.MapPath("~/YsplDoc/");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    GAP12File.SaveAs(Server.MapPath("~/ysplDoc/") + GAP12File.FileName);
                    string gaP12FilePath = @"http://localhost:56791/ysplDoc/" + GAP12File.FileName;
                    analyticsCredential.GAP12FilePath = gaP12FilePath;
                }
                if (GAJsonFile.ContentLength < 0)
                {
                    return Json(new Models.AjaxResponse { Success = false, Message = "Please select .json file." });
                }
                else
                {
                    string getGAJsonFileExtention = Path.GetExtension(GAJsonFile.FileName);
                    if (!getGAJsonFileExtention.Equals(".json", StringComparison.OrdinalIgnoreCase))
                    {
                        return Json(new Models.AjaxResponse { Success = false, Message = "Please choose .json extension file." });
                    }
                    string path = Server.MapPath("~/YsplDoc/");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    GAP12File.SaveAs(Server.MapPath("~/ysplDoc/") + GAJsonFile.FileName);
                    string gaJsonFilePath = @"http://localhost:56791/ysplDoc/" + GAJsonFile.FileName;
                    analyticsCredential.GAJsonFilePath = gaJsonFilePath;
                }

                analyticsCredential.ID = Guid.NewGuid();
                analyticsCredential.CreatedBy = Guid.Parse(User.Identity.GetUserId());
                analyticsCredential.Save();
                return Json(new Models.AjaxResponse { Success = true, Message = "Changes saved." });
            }
            catch (Exception ex)
            {
                return Json(new Models.AjaxResponse { Success = false, Message = ex.Message });
            }
        }
        [HttpPost]
        public ActionResult AnalyticsCredentialRead([DataSourceRequest] DataSourceRequest request, String SearchText)
        {

            var _userId = Guid.Parse(User.Identity.GetUserId());
            var AnalyticsCredentials = context.AnalyticsCredentials.Where(m => m.CreatedBy == _userId);
            if (String.IsNullOrEmpty(SearchText) == false)
            {
                AnalyticsCredentials = AnalyticsCredentials.Where(x => x.ApplicationName.Contains(SearchText));
            }
            return Json(AnalyticsCredentials.Select(x => new Models.UI.AnalyticsCredentialListItem

            {
                ID = x.ID,
                ApplicationName = x.ApplicationName,
                GAEmail = x.GAEmail,
                GAViewID = x.GAViewID
            }).ToDataSourceResult(request));
        }
        public ActionResult AnalyticsCredentialDelete(String IDs)
        {
            foreach (var AnalyticsCredential in IDs.Split(new String[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
            {
                var AnalyticsCredentialID = Guid.Parse(AnalyticsCredential);
                var selectedAnalyticsCredential = context.AnalyticsCredentials.FirstOrDefault(x => x.ID == AnalyticsCredentialID);
                if (selectedAnalyticsCredential != null)
                {
                    context.AnalyticsCredentials.Remove(selectedAnalyticsCredential);
                }
            }
            try
            {
                context.SaveChanges();
                return Json(new Models.AjaxResponse { Success = true, Message = "Records deleted successfully" });
            }
            catch (Exception ex)
            {
                return Json(new Models.AjaxResponse { Success = false, Message = ex.InnerException == null ? ex.Message : ex.InnerException.Message });
            }
        }

        [Route("~/admin/google-analytic-detail/{id}")]
        public ActionResult GoogleAnalyticDetail(Guid id, AnalyticsCredential analyticsCredential)
        {
            try
            {
                //if(id==null)
                //{
                //    //return new ()
                //}
                var getACredential = context.AnalyticsCredentials.FirstOrDefault(m => m.ID == id);
                var credential = GetCredential(getACredential.GAEmail, getACredential.GAJsonFilePath).Result;
                using (var svc = new AnalyticsReportingService(
                    new BaseClientService.Initializer
                    {
                        HttpClientInitializer = credential,
                        ApplicationName = "Google Api"
                    }))
                {
                    var dateRange = new DateRange
                    {
                        StartDate = "2021-02-09",
                        EndDate = "2021-02-25"
                    };
                    var sessions = new Metric
                    {
                        Expression = "ga:sessions",
                        Alias = "Sessions"
                    };
                    var users = new Metric
                    {
                        Expression = "ga:users",
                        Alias = "Users"
                    };
                    var bounceRates = new Metric
                    {
                        Expression = "ga:bouncerate",
                        Alias = "Bounce Rate"
                    };
                    var sessionDurations = new Metric
                    {
                        Expression = "ga:sessionduration",
                        Alias = "Session Duration"
                    };
                    var pageViews = new Metric
                    {
                        Expression = "ga:pageviews",
                        Alias = "Page View"
                    };

                    var date = new Dimension { Name = "ga:date" };

                    var reportRequest = new ReportRequest
                    {
                        DateRanges = new List<DateRange> { dateRange },
                        Dimensions = new List<Dimension> { date },
                        Metrics = new List<Metric> { sessions, users, bounceRates, sessionDurations, pageViews },
                        ViewId = getACredential.GAViewID
                    };
                    //ViewBag.Session = sessions;

                    var getReportsRequest = new GetReportsRequest
                    {
                        ReportRequests = new List<ReportRequest> { reportRequest }
                    };
                    var batchRequest = svc.Reports.BatchGet(getReportsRequest);
                    var response = batchRequest.Execute();
                    var galistViewModel = new List<GAListViewModel>();
                    foreach (var x in response.Reports.First().Data.Rows)
                    {
                        galistViewModel.Add(new GAListViewModel(string.Join(", ", x.Dimensions), string.Join(", ", x.Metrics.First().Values)));
                        Console.WriteLine(string.Join(", ", x.Dimensions) +
                        "   " + string.Join(", ", x.Metrics.First().Values));


                    }
                    return View(galistViewModel);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);

            }

            return View();
        }

        static async Task<UserCredential> GetCredential(string gaEmail, string gaJsonFilePath)
        {
            using (var stream = new FileStream(@"C:\Users\Bilal_Khan\Downloads\client_secret_1096586895070-10eu6js9m7ak5pl21tlhn1m3u74k5rfu.apps.googleusercontent.com.json", FileMode.Open, FileAccess.Read))
            {
                string loginEmailAddress = gaEmail;
                return await GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    new[] { AnalyticsReportingService.Scope.Analytics },
                    loginEmailAddress, CancellationToken.None,
                    new FileDataStore("GoogleAnalyticsApiConsole"));
            }
        }
    }
    #endregion
}
