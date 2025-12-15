using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace shopacc.Models
{
    public class Table
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Giá"), UIHint("TextBox")]
        public string Price { get; set; }
        [Display(Name = "Lần 1"), UIHint("TextBox")]
        public string Lan1 { get; set; }
        [Display(Name = "Lần 2"), UIHint("TextBox")]
        public string Lan2 { get; set; }
        [Display(Name = "Lần 3"), UIHint("TextBox")]
        public string Lan3 { get; set; }
        public bool Hot { get; set; }
        [Display(Name = "Thứ tự"), Required(ErrorMessage = "Hãy nhập số thứ tự"), RegularExpression(@"\d+", ErrorMessage = "Chỉ nhập số nguyên dương"), UIHint("NumberBox")]
        public int Sort { get; set; }
        public TypeTable TypeTable { get; set; }
    }


    public enum TypeTable
    {
        [Display(Name = "BẢNG GIÁ NẠP UC VNG")]
        VNG,
        [Display(Name = "BẢNG GIÁ NẠP UC QUỐC TẾ")]
        QT
    }
}