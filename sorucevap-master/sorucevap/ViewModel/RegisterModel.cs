using System.ComponentModel.DataAnnotations;

namespace sorucevap.ViewModel
{
    public class RegisterModel
    {
        [Display(Name = "Adı")]
        [Required(ErrorMessage = "Adınızı Giriniz!")]
        public string Adi { get; set; }

        [Display(Name = "Soyadi")]
        [Required(ErrorMessage = "Soyadınızı Giriniz!")]
        public string Soyadi { get; set; }

        [Display(Name = "Kullanici Adi")]
        [Required(ErrorMessage = "Kullanıcı Adını Giriniz!")]
        public string KullaniciAdi { get; set; }

        [Display(Name = "E-Posta Adresi")]
        [Required(ErrorMessage = "E-Posta Adresi Giriniz!")]
        [EmailAddress(ErrorMessage = "Geçerli bir E-Posta Adresi Giriniz!")]
        public string Email { get; set; }

        [Display(Name = "Parola")]
        [Required(ErrorMessage = "Parola Giriniz!")]
        public string Sifre { get; set; }

        [Display(Name = "Parola Tekrar")]
        [Required(ErrorMessage = "Parola Tekrar Giriniz!")]
        [Compare("Sifre", ErrorMessage = "Parola Tekrarı Tutarsızdır!")]
        public string SifreTekrar { get; set; }
    }
}
