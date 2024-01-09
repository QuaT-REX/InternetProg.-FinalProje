using System.ComponentModel.DataAnnotations;

namespace sorucevap.ViewModel
{
    public class LoginModel
    {
        [Display(Name = "Kullanıcı Adı")]
        [Required(ErrorMessage = "Kullanıcı Adı Giriniz!")]
        public string email { get; set; }


        [Display(Name = "Parola")]
        [Required(ErrorMessage = "Parola Giriniz!")]
        public string Sifre { get; set; }
    }
}
