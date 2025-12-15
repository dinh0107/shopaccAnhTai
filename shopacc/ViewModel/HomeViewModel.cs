using shopacc.Models;
using PagedList;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace shopacc.ViewModel
{
    public class HomeViewModel
    {
        public IEnumerable<Product> Products { get; set; }
        public IEnumerable<Banner> Banners { get; set; }
    }
    public class ProductFilterViewModel
    {
        public string Sort { get; set; }
        public string Price { get; set; }
        public IEnumerable<Product> Products { get; set; }
    }

    public class ProductDetail
    {
        public Product Product { get; set; }
    }
    public class TinhGopViewModel
    {
        public decimal Gia { get; set; }
        public decimal GiaGop { get; set; }
    }
    public class NapUcViewModel
    {
        public IEnumerable<Table> Tables { get; set; }
        public IEnumerable<Table> TablesQT { get; set; }

    }
}
