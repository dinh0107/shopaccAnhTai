using Helpers;
using PagedList;
using shopacc.DAL;
using shopacc.Models;
using shopacc.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web.Mvc;


namespace shopacc.Controllers
{
    [Authorize, RoutePrefix("mms")]
    public class ProductController : Controller
    {
        private readonly UnitOfWork _unitOfWork = new UnitOfWork();
     

        #region Product
        [Route("danh-sach-acc")]
        public ActionResult ListProduct(int? page, string name, int? childId, int rootId = 0, string sort = "date-desc", string result = "")
        {
            ViewBag.Result = result;
            var pageNumber = page ?? 1;
            const int pageSize = 15;
            var products = _unitOfWork.ProductRepository.GetQuery().AsNoTracking();

            if (!string.IsNullOrEmpty(name))
            {
                products = products.Where(l => l.Name.Contains(name));
            }

            switch (sort)
            {
                case "sort-asc":
                    products = products.OrderBy(a => a.Sort);
                    break;
                case "sort-desc":
                    products = products.OrderByDescending(a => a.Sort);
                    break;
                case "hot":
                    products = products.OrderByDescending(a => a.Sort);
                    break;
                case "date-asc":
                    products = products.OrderBy(a => a.CreateDate);
                    break;
                default:
                    products = products.OrderByDescending(a => a.CreateDate);
                    break;
            }
            var model = new ListProductViewModel
            {
                Products = products.ToPagedList(pageNumber, pageSize),
                Name = name,
                Sort = sort
            };
            return View(model);
        }
        [Route("them-acc")]
        public ActionResult Product()
        {
            var model = new InsertProductViewModel
            {
                Product = new Product { Sort = 1, Active = true },
            };
            return View(model);
        }
        [Route("them-acc")]
        [HttpPost, ValidateInput(false)]
        public ActionResult Product(InsertProductViewModel model, FormCollection fc)
        {
            if (ModelState.IsValid)
            {
                if (model.Price != null)
                {
                    model.Product.Price = Convert.ToDecimal(model.Price.Replace(",", ""));
                }
                model.Product.ListImage = fc["Pictures"];
                model.Product.Name = GenerateRandomString();
                model.Product.Url = HtmlHelpers.ConvertToUnSign(null, model.Product.Url ?? model.Product.Name);

                var count = _unitOfWork.ProductRepository.GetQuery(a => a.Url == model.Product.Url).Count();
                if (count > 1)
                {
                    model.Product.Url += "-" + DateTime.Now.Millisecond;
                    _unitOfWork.Save();
                }

                _unitOfWork.ProductRepository.Insert(model.Product);
                _unitOfWork.Save();

                return RedirectToAction("ListProduct", new { result = "success" });
            }

            return View(model);
        }
        [Route("sua-acc")]
        public ActionResult UpdateProduct(int proId = 0)
        {
            var product = _unitOfWork.ProductRepository.GetById(proId);
            if (product == null)
            {
                return RedirectToAction("ListProduct");
            }


            var model = new InsertProductViewModel
            {
                Product = product,
                Price = product.Price.ToString("N0"),
            };

            return View(model);
        }
        [Route("sua-acc")]
        [HttpPost, ValidateInput(false)]
        public ActionResult UpdateProduct(InsertProductViewModel model, FormCollection fc)
        {
            var product = _unitOfWork.ProductRepository.GetById(model.Product.Id);
            if (product == null)
            {
                return RedirectToAction("ListProduct");
            }

            if (ModelState.IsValid)
            {
                if (model.Price != null)
                {
                    product.Price = Convert.ToDecimal(model.Price.Replace(",", ""));
                }
                product.ListImage = fc["Pictures"] == "" ? null : fc["Pictures"];

                product.Url = HtmlHelpers.ConvertToUnSign(null, model.Product.Url ?? model.Product.Name);
                product.Name = model.Product.Name;
                product.Body = model.Product.Body;
                product.Active = model.Product.Active;
                product.ChoThue = model.Product.ChoThue;
                product.TypeAcc = model.Product.TypeAcc;
                product.TitleMeta = model.Product.TitleMeta;
                product.DescriptionMeta = model.Product.DescriptionMeta;
                product.Sort = model.Product.Sort;
                product.NgoaiTe = model.Product.NgoaiTe;
                _unitOfWork.Save();

                var count = _unitOfWork.ProductRepository.GetQuery(a => a.Url == product.Url).Count();
                if (count > 1)
                {
                    product.Url += "-" + DateTime.Now.Millisecond;
                    _unitOfWork.Save();
                }

                return RedirectToAction("ListProduct", new { result = "update" });
            }


            return View(model);
        }
        [HttpPost]
        public bool DeleteProduct(int proId = 0)
        {
            var product = _unitOfWork.ProductRepository.GetById(proId);
            if (product == null)
            {
                return false;
            }
            _unitOfWork.ProductRepository.Delete(product);
            _unitOfWork.Save();
            return true;
        }
        [HttpPost]
        public bool QuickUpdate(int? quantity, bool? status, bool active, int sort = 0, int proId = 0)
        {
            var product = _unitOfWork.ProductRepository.GetById(proId);
            if (product == null)
            {
                return false;
            }
            if (status != null)
            {
                product.Active = Convert.ToBoolean(status);
            }
            if (sort >= 0)
            {
                product.Sort = sort;
            }
            product.Active = active;
            _unitOfWork.Save();
            return true;
        }
        #endregion

        public string GenerateRandomString(int length = 12)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        protected override void Dispose(bool disposing)
        {
            _unitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }
}