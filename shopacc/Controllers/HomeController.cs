using shopacc.DAL;
using shopacc.Models;
using shopacc.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace shopacc.Controllers
{
    public class HomeController : Controller
    {

        private readonly UnitOfWork _unitOfWork = new UnitOfWork();
        public ConfigSite ConfigSite => (ConfigSite)HttpContext.Application["ConfigSite"];
        public PartialViewResult Header()
        {
            return PartialView();
        }

       
        public PartialViewResult Footer()
        {
            return PartialView();
        }

        public ActionResult Index()
        {
            var accs = _unitOfWork.ProductRepository.GetQuery(a => a.Active, o => o.OrderByDescending(a => a.CreateDate));
            var model = new HomeViewModel
            {
                Products = accs,
                Banners = _unitOfWork.BannerRepository.GetQuery(a => a.Active, o => o.OrderBy(a => a.Sort))
            };
            return View(model);
        }

        public PartialViewResult ProductSort(string price = "all", string sort = "newest" , int typePage = 0)
        {
            IQueryable<Product> query = _unitOfWork.ProductRepository.GetQuery(a => a.Active);
            if(typePage == 1)
            {
                query = query.Where(a => a.TypeAcc == TypeAcc.ChoThue);
            }
            switch (price)
            {
                case "under-1m":
                    query = query.Where(a => a.Price <= 1000000);
                    break;
                case "1m-5m":
                    query = query.Where(a => a.Price >= 1000000 && a.Price <= 5000000);
                    break;
                case "5m-10m":
                    query = query.Where(a => a.Price >= 5000000 && a.Price <= 10000000);
                    break;
                case "10m-15m":
                    query = query.Where(a => a.Price >= 10000000 && a.Price <= 15000000);
                    break;
                case "15m-20m":
                    query = query.Where(a => a.Price >= 15000000 && a.Price <= 20000000);
                    break;
                case "20m-50m":
                    query = query.Where(a => a.Price >= 20000000 && a.Price <= 50000000);
                    break;
                case "50m+":
                    query = query.Where(a => a.Price >= 50000000);
                    break;
            }

            switch (sort)
            {
                case "newest":
                    query = query.OrderByDescending(a => a.CreateDate);
                    break;

                case "oldest":
                    query = query.OrderBy(a => a.CreateDate);
                    break;

                case "price-low":
                    query = query.OrderBy(a => a.Price);
                    break;

                case "price-high":
                    query = query.OrderByDescending(a => a.Price);
                    break;

                default:
                    query = query.OrderByDescending(a => a.CreateDate);
                    break;
            }


            var productList = query.ToList();

            var model = new ProductFilterViewModel
            {
                Price = price,
                Sort = sort,
                Products = productList
            };

            return PartialView(model);
        }

        [Route("thong-tin/{url}")]
        public ActionResult ProductDetail(string url)
        {
            var product = _unitOfWork.ProductRepository.GetQuery(a => a.Url == url).FirstOrDefault();
            if(product == null)
            {
                return Redirect("Index");
            }
            var model = new ProductDetail
            {
                Product = product,
            };  
            return View(model);
        }



        [Route("acc-cho-thue")]
        public ActionResult ProductList()
        {
            return View();
        }

        
        [Route("tinh-gop")]
        public ActionResult TinhGop()
        {
            return View(new TinhGopViewModel());
        }
        [HttpPost]
        [Route("tinh-gop")]
        public ActionResult TinhGop(TinhGopViewModel model)
        {
            if (model.Gia <= 0)
            {
                ModelState.AddModelError("Gia", "Giá phải lớn hơn 0");
                return View(model);
            }
            model.GiaGop = model.Gia * 0.3m;
            return View(model);
        }

        [Route("nap-uc")]
        public ActionResult NapUc()
        {
            var model = new NapUcViewModel
            {
                Tables = _unitOfWork.TableRepository.GetQuery(a => a.TypeTable == TypeTable.VNG, v => v.OrderBy(a => a.Sort)),
                TablesQT = _unitOfWork.TableRepository.GetQuery(a => a.TypeTable == TypeTable.QT, v => v.OrderBy(a => a.Sort)),
            };
            return View(model);
        }
    }
}