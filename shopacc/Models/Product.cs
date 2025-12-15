using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace shopacc.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Display(Name = "Mã acc"), UIHint("TextBox")]
        public string Name { get; set; }
        [Display(Name = "Ảnh đại diện"), StringLength(500)]
        public string Image { get; set; }
        [Display(Name = "Danh sách ảnh"), UIHint("UploadMultiFile")]
        public string ListImage { get; set; }

        [Display(Name = "Thông tin acc"), UIHint("EditorBox")]
        public string Body { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}")]
        [Display(Name = "Ngày đăng")]
        public DateTime CreateDate { get; set; }
        [Display(Name = "Thứ tự"), Required(ErrorMessage = "Hãy nhập số thứ tự"), RegularExpression(@"\d+", ErrorMessage = "Chỉ nhập số nguyên dương"), UIHint("NumberBox")]
        public int Sort { get; set; }
        [Display(Name = "Hoạt động")]
        public bool Active { get; set; } = true;

        [Display(Name = "Đã bán")]
        public bool ChoThue { get; set; } = false;
        [Display(Name = "Ngoại tệ"), UIHint("TextBox")]
        public string NgoaiTe { get; set; }
        [Display(Name = "Giá"), DisplayFormat(DataFormatString = "{0:N0}đ")]
        public decimal Price { get; set; }
        [Display(Name = "Đường dẫn"), StringLength(300), UIHint("TextBox")]
        public string Url { get; set; }
        [Display(Name = "Thẻ tiêu đề"), StringLength(100, ErrorMessage = "Tối đa 100 ký tự"), UIHint("TextBox")]
        public string TitleMeta { get; set; }
        [Display(Name = "Thẻ mô tả"), StringLength(500, ErrorMessage = "Tối đa 500 ký tự"), UIHint("TextArea")]
        public string DescriptionMeta { get; set; }
        [Display(Name = "Loại acc")]
        public TypeAcc TypeAcc { get; set; }
        public Product()
        {
            CreateDate = DateTime.Now;
        }
    }


    public enum TypeAcc
    {
        [Display(Name = "Acc cho thuê")]
        ChoThue,
        [Display(Name = "Acc cho thường")]
        AccThuong
    }
}
