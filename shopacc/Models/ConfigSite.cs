using System.ComponentModel.DataAnnotations;

namespace shopacc.Models
{
    public class ConfigSite
    {
        public int Id { get; set; }
        [StringLength(500, ErrorMessage = "Tối đa 500 ký tự"), Display(Name = "Đường dẫn Facebook"), Url(ErrorMessage = "Đường dẫn không chính xác"), UIHint("TextBox")]
        public string Facebook { get; set; }
        [StringLength(500, ErrorMessage = "Tối đa 500 ký tự"), Display(Name = "Đường dẫn Zalo"), Url(ErrorMessage = "Đường dẫn không chính xác"), UIHint("TextBox")]
        public string Zalo { get; set; }
        [StringLength(500, ErrorMessage = "Tối đa 500 ký tự"), Display(Name = "Đường dẫn Youtube"), Url(ErrorMessage = "Đường dẫn không chính xác"), UIHint("TextBox")]
        public string Youtube { get; set; }
        [StringLength(500, ErrorMessage = "Tối đa 500 ký tự"), Display(Name = "Đường dẫn Twitter"), Url(ErrorMessage = "Đường dẫn không chính xác"), UIHint("TextBox")]
        public string Twitter { get; set; }
        [StringLength(500, ErrorMessage = "Tối đa 500 ký tự"), Display(Name = "Đường dẫn Instagram"), Url(ErrorMessage = "Đường dẫn không chính xác"), UIHint("TextBox")]
        public string Instagram { get; set; }
        [StringLength(500, ErrorMessage = "Tối đa 4000 ký tự"), Display(Name = "Mã nhúng Messenger"), UIHint("TextBox")]
        public string Messenger { get; set; }
        [Display(Name = "Logo"), UIHint("ImageConfig")]
        public string Image { get; set; }

        [Display(Name = "Favicon"), UIHint("ImageConfig")]
        public string Favicon { get; set; }
        [StringLength(4000, ErrorMessage = "Tối đa 4000 ký tự"), Display(Name = "Mã Google Map"), UIHint("TextArea")]
        public string GoogleMap { get; set; }
        [StringLength(4000, ErrorMessage = "Tối đa 4000 ký tự"), Display(Name = "Mã Google Analytics"), UIHint("TextArea")]
        public string GoogleAnalytics { get; set; }
        [Display(Name = "Nội dung thông báo"), UIHint("EditorBox")]
        public string Body { get; set; }
        [Display(Name = "Thông tin chân trang"), UIHint("EditorBox")]
        public string Footer { get; set; }

        [Display(Name = "Thẻ title"), StringLength(200, ErrorMessage = "Tối đa 200 ký tự"), UIHint("TextBox")]
        public string Title { get; set; }
       
        [Display(Name = "Thẻ description"), StringLength(500, ErrorMessage = "Tối đa 500 ký tự"), UIHint("TextArea")]
        public string Description { get; set; }
        [Display(Name = "Hotline"), StringLength(50, ErrorMessage = "Tối đa 50 ký tự"), UIHint("TextBox")]
        public string Hotline { get; set; }
        [Display(Name = "Tên cá nhân"), UIHint("TextBox")]
        public string Name { get; set; }

        [Display(Name = "Avatar")]
        public string Avatar { get; set; }
        [Display(Name = "Mô tả Profile"), UIHint("EditorBox")]
        public string Profile { get; set; }

        [Display(Name = "Giao diện sự kiện"), StringLength(50)]
        public string ThemeEvent { get; set; }

        [Display(Name = "Bật hiệu ứng động sự kiện")]
        public bool EnableThemeEffect { get; set; }

        [Display(Name = "Lời chúc / Thông điệp sự kiện"), StringLength(200)]
        public string ThemeGreeting { get; set; }
    }
}
