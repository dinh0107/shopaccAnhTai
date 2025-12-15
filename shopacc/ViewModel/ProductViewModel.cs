using shopacc.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace shopacc.ViewModel
{
    
    public class ListProductViewModel
    {
        public string Name { get; set; }
        public string Sort { get; set; }
        public PagedList.IPagedList<Product> Products { get; set; }

       
    }

    public class InsertProductViewModel
    {
        public Product Product { get; set; }
        [Display(Name = "Giá niêm yết"), UIHint("MoneyBox"), DisplayFormat(DataFormatString = "{0:N0}đ")]
        public string Price { get; set; }
        [Display(Name = "Giá khuyến mãi"), UIHint("MoneyBox"), DisplayFormat(DataFormatString = "{0:N0}đ")]
        public string PriceSale { get; set; }
    }
}
