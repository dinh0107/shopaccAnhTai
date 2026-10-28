using shopacc.DAL;
using shopacc.Models;
using shopacc.ViewModel;
using Helpers;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Data.Entity;
using System.Globalization;
using PagedList;

namespace shopacc.Controllers
{
    [Authorize, RoutePrefix("mms")]
    public class MmsController : Controller
    {
        public readonly UnitOfWork _unitOfWork = new UnitOfWork();

        #region Login
        [Route("dang-nhap")]
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }
        [Route("dang-nhap")]
        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login(AdminLoginModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var admin = _unitOfWork.AdminRepository.Get(a => a.Username == model.Username && a.Active).SingleOrDefault();

                if (admin != null && HtmlHelpers.VerifyHash(model.Password, "SHA256", admin.Password))
                {
                    var ticket = new FormsAuthenticationTicket(1, model.Username.ToLower(), DateTime.Now, DateTime.Now.AddDays(30), true,
                       FormsAuthentication.FormsCookiePath);

                    var encTicket = FormsAuthentication.Encrypt(ticket);
                    // Create the cookie.
                    Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encTicket));
                    if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                        && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                    {
                        return Redirect(returnUrl);
                    }
                    return RedirectToAction("Index", "Mms");
                }
                ModelState.AddModelError("", @"Tên đăng nhập hoặc mật khẩu không chính xác.");
            }
            return View(model);
        }
        [Route("dang-xuat")]
        public RedirectToRouteResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Mms");
        }
        #endregion
        [Route("")]
        public ActionResult Index()
        {
            var model = new InfoAdminViewModel
            {
                Admins = _unitOfWork.AdminRepository.GetQuery().Count(),
                Banners = _unitOfWork.BannerRepository.GetQuery().Count(),
                Products = _unitOfWork.ProductRepository.GetQuery().Count(),
            };
            return View(model);
        }

        #region Admin
        [ChildActionOnly]
        public PartialViewResult ListAdmin()
        {
            var admins = _unitOfWork.AdminRepository.Get();
            return PartialView("ListAdmin", admins);
        }
        [Route("them-tai-khoan")]
        public ActionResult CreateAdmin(string result = "")
        {
            ViewBag.Result = result;
            return View();
        }
        [Route("them-tai-khoan")]
        [HttpPost]
        public ActionResult CreateAdmin(Admin model)
        {
            if (ModelState.IsValid)
            {
                var admin = _unitOfWork.AdminRepository.GetQuery(a => a.Username.Equals(model.Username)).SingleOrDefault();
                if (admin != null)
                {
                    ModelState.AddModelError("", @"Tên đăng nhập này có rồi");
                }
                else
                {
                    var hashPass = HtmlHelpers.ComputeHash(model.Password, "SHA256", null);
                    _unitOfWork.AdminRepository.Insert(new Admin { Username = model.Username, Password = hashPass,Active = model.Active });
                    _unitOfWork.Save();
                    return RedirectToAction("CreateAdmin", new { result = "success" });
                }
            }
            return View();
        }
        [Route("sua-tai-khoan")]
        public ActionResult EditAdmin(int adminId = 0)
        {
            var admin = _unitOfWork.AdminRepository.GetById(adminId);
            if (admin == null)
            {
                return RedirectToAction("CreateAdmin");
            }

            var model = new EditAdminViewModel
            {
                Id = admin.Id,
                Username = admin.Username,
                Active = admin.Active,
            };

            return View(model);
        }
        [Route("sua-tai-khoan")]
        [HttpPost]
        public ActionResult EditAdmin(EditAdminViewModel model)
        {
            if (ModelState.IsValid)
            {
                var admin = _unitOfWork.AdminRepository.GetById(model.Id);
                if (admin == null)
                {
                    return RedirectToAction("CreateAdmin");
                }
                if (admin.Username != model.Username)
                {
                    var exists = _unitOfWork.AdminRepository.GetQuery(a => a.Username.Equals(model.Username)).SingleOrDefault();
                    if (exists != null)
                    {
                        ModelState.AddModelError("", @"Tên đăng nhập này có rồi");
                        return View(model);
                    }
                    admin.Username = model.Username;
                }
                admin.Active = model.Active;
                if (model.Password != null)
                {
                    admin.Password = HtmlHelpers.ComputeHash(model.Password, "SHA256", null);
                }
                _unitOfWork.Save();
                return RedirectToAction("CreateAdmin", new { result = "update" });
            }
            return View(model);
        }
        public bool DeleteAdmin(string username)
        {
            var admin = _unitOfWork.AdminRepository.GetQuery(a => a.Username.Equals(username)).SingleOrDefault();
            if (admin == null)
            {
                return false;
            }
            if (username == "admin")
            {
                return false;
            }
            _unitOfWork.AdminRepository.Delete(admin);
            _unitOfWork.Save();
            return true;
        }
        [Route("doi-mat-khau")]
        public ActionResult ChangePassword(int result = 0)
        {
            ViewBag.Result = result;
            return View();
        }
        [Route("doi-mat-khau")]
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {
                var admin = _unitOfWork.AdminRepository.GetQuery(a => a.Username.Equals(User.Identity.Name,
                StringComparison.OrdinalIgnoreCase)).SingleOrDefault();
                if (admin == null)
                {
                    return HttpNotFound();
                }
                if (HtmlHelpers.VerifyHash(model.OldPassword, "SHA256", admin.Password))
                {
                    admin.Password = HtmlHelpers.ComputeHash(model.Password, "SHA256", null);
                    _unitOfWork.Save();
                    return RedirectToAction("ChangePassword", new { result = 1 });
                }
                else
                {
                    ModelState.AddModelError("", @"Mật khẩu hiện tại không đúng!");
                    return View();
                }
            }
            return View(model);
        }
        #endregion

        #region ConfigSite
        [Route("thong-tin-chung")]
        public ActionResult ConfigSite(string result = "")
        {
            ViewBag.Result = result;
            var config = _unitOfWork.ConfigSiteRepository.Get().FirstOrDefault();
            return View(config);
        }
        [Route("thong-tin-chung")]
        [HttpPost, ValidateInput(false)]
        public ActionResult ConfigSite(ConfigSite model, FormCollection fc)
        {
            var config = _unitOfWork.ConfigSiteRepository.Get().FirstOrDefault();
            if (config == null)
            {
                _unitOfWork.ConfigSiteRepository.Insert(model);
            }
            else
            {
                for (var i = 0; i < Request.Files.Count; i++)
                {
                    if (Request.Files[i] == null || Request.Files[i].ContentLength <= 0) continue;
                    if (!HtmlHelpers.CheckFileExt(Request.Files[i].FileName, "jpg|jpeg|png|gif")) continue;
                    if (Request.Files[i].ContentLength > 1024 * 1024 * 4) continue;

                    var imgFileName = HtmlHelpers.ConvertToUnSign(null, Path.GetFileNameWithoutExtension(Request.Files[i].FileName)) +
                        "-" + DateTime.Now.Millisecond + Path.GetExtension(Request.Files[i].FileName);
                    var imgPath = "/images/configs/" + DateTime.Now.ToString("yyyy/MM/dd");
                    HtmlHelpers.CreateFolder(Server.MapPath(imgPath));

                    var imgFile = DateTime.Now.ToString("yyyy/MM/dd") + "/" + imgFileName;

                    var newImage = Image.FromStream(Request.Files[i].InputStream);
                    var fixSizeImage = HtmlHelpers.FixedSize(newImage, 1000, 1000, false);
                    HtmlHelpers.SaveJpeg(Server.MapPath(Path.Combine(imgPath, imgFileName)), fixSizeImage, 90);

                    if (Request.Files.Keys[i] == "Image")
                    {
                        config.Image = imgFile;
                    }
                    else if (Request.Files.Keys[i] == "Favicon")
                    {
                        config.Favicon = imgFile;
                    }
                    else if (Request.Files.Keys[i] == "Avatar")
                    {
                        config.Avatar = imgFile;
                    }
                }

                config.Facebook = model.Facebook;
                config.GoogleMap = model.GoogleMap;
                config.Youtube = model.Youtube;
                config.Instagram = model.Instagram;
                config.Title = model.Title;
                config.Description = model.Description;
                config.GoogleAnalytics = model.GoogleAnalytics;
                config.Hotline = model.Hotline;
                config.Messenger = model.Messenger;
                config.Footer = model.Footer;
                config.Body = model.Body;
                config.Name = model.Name;
                config.Profile = model.Profile;
                config.ThemeEvent = string.IsNullOrWhiteSpace(model.ThemeEvent) ? "none" : model.ThemeEvent;
                config.EnableThemeEffect = model.EnableThemeEffect;
                config.ThemeGreeting = model.ThemeGreeting;
                if (model.Zalo != null)
                {
                    config.Zalo = model.Zalo.Replace(" ", string.Empty);
                }

                _unitOfWork.Save();
                HttpContext.Application["ConfigSite"] = config;
                return RedirectToAction("ConfigSite", "Mms", new { result = "success" });
            }
            return View("ConfigSite", model);
        }
        #endregion


        #region Price
        public ActionResult ListPrice(string name = "")
        {
            var price = _unitOfWork.TableRepository.GetQuery(a => a.TypeTable == TypeTable.VNG, a => a.OrderBy(l => l.Sort));
            if (!string.IsNullOrEmpty(name))
            {
                price = price.Where(a => a.Price.Contains(name));
            }
            var model = new ListPriceViewModel
            {
                Name = name,
                Tables = price
            };
            return View(model);
        }
        public ActionResult AddPrice()
        {
            var model = new PriceViewModel
            {
                Table = new Table
                {
                    Sort = 1
                }
            };
            return View(model);

        }
        [HttpPost]
        public ActionResult AddPrice(PriceViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.Table.TypeTable = TypeTable.VNG;
                _unitOfWork.TableRepository.Insert(model.Table);
                _unitOfWork.Save();
                return RedirectToAction("ListPrice", new { result = "success" });
            }
            return View(model);
        }

        public ActionResult UpdatePrice(int id)
        {
            var price = _unitOfWork.TableRepository.GetById(id);
            if (price == null)
            {
                return RedirectToAction("CarService");
            }
            var model = new PriceViewModel
            {
                Table = price
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult UpdatePrice(PriceViewModel model)
        {
           
            var price = _unitOfWork.TableRepository.GetById(model.Table.Id);

            if (price == null)
                return RedirectToAction("AddPrice");

            if (ModelState.IsValid)
            {
                price.Sort = model.Table.Sort;
                price.Price = model.Table.Price;
                price.Lan1 = model.Table.Lan1;
                price.Lan2 = model.Table.Lan2;
                price.Lan3 = model.Table.Lan3;
                _unitOfWork.TableRepository.Update(price);
                _unitOfWork.Save();

                return RedirectToAction("ListPrice", new { result = "update" });
            }

            return View(model);
        }


        [HttpPost]
        public bool DeletePrice(int catId = 0)
        {

            var category = _unitOfWork.TableRepository.GetById(catId);
            if (category == null)
            {
                return false;
            }
            _unitOfWork.TableRepository.Delete(category);
            _unitOfWork.Save();
            return true;
        }
        #endregion
        #region Price2
        public ActionResult ListPrice2(string name = "")
        {
            var price = _unitOfWork.TableRepository.GetQuery(a => a.TypeTable == TypeTable.QT, a => a.OrderBy(l => l.Sort));
            if (!string.IsNullOrEmpty(name))
            {
                price = price.Where(a => a.Price.Contains(name));
            }
            var model = new ListPriceViewModel
            {
                Name = name,
                Tables = price
            };
            return View(model);
        }
        public ActionResult AddPrice2()
        {
            var model = new PriceViewModel
            {
                Table = new Table
                {
                    Sort = 1
                }
            };
            return View(model);

        }
        [HttpPost]
        public ActionResult AddPrice2(PriceViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.Table.TypeTable = TypeTable.QT;
                _unitOfWork.TableRepository.Insert(model.Table);
                _unitOfWork.Save();
                return RedirectToAction("ListPrice2", new { result = "success" });
            }
            return View(model);
        }

        public ActionResult UpdatePrice2(int id)
        {
            var price = _unitOfWork.TableRepository.GetById(id);
            if (price == null)
            {
                return RedirectToAction("AddPrice2");
            }
            var model = new PriceViewModel
            {
                Table = price
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult UpdatePrice2(PriceViewModel model)
        {

            var price = _unitOfWork.TableRepository.GetById(model.Table.Id);

            if (price == null)
                return RedirectToAction("AddPrice2");

            if (ModelState.IsValid)
            {
                price.Sort = model.Table.Sort;
                price.Price = model.Table.Price;
                price.Lan1 = model.Table.Lan1;
                _unitOfWork.TableRepository.Update(price);
                _unitOfWork.Save();

                return RedirectToAction("ListPrice2", new { result = "update" });
            }

            return View(model);
        }

        #endregion

        protected override void Dispose(bool disposing)
        {
            _unitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }
}
