using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReceteX.Models
{
    public class Prescription:BaseModel
    {
        //sadece kendi yazdığı reçeteleri görüntüleyebilecek diğer doktorların yazdığı reçeteleri görmeyecek

        public Guid AppUserId { get; set; }

        //navigational property : doktor bilgisiine ulaşabilmek için 

        public virtual AppUser? AppUser { get; set; }

        //reçetenin durumu
        public Guid StatusId { get; set; }
        public virtual Status? Status { get; set; }





        //bir reçetenin içerisinde birden fazla reçete ilacı olabilir
        //ama bu şekilde yazılmıi ilaç sadece bir reçetede olabilir
        //bu şekilde otomatik olarak relation tablosu oluşturacak
        public ICollection<PrescriptionMedicine>? PrescriptionMedicines { get; set; }

        //bir reçetenin birden fazla açıklaması olabiliyor(açıklama türü)

        public ICollection<Description>? Descriptions { get; set; }

        //bir reçetenin birden fazla tanısı olabilir

        public ICollection<Diagnosis>? Diagnoses { get; set; }




        //hasta bilgilerini kvkk'dan dolayı kayıt etmeyeceğiz reçetenin içine yazacağız

        public string? PatientFirstName { get; set; }

        public string? PatientLastName { get;set; }

        //base model'deki doktor için, buradaki hasta için  
        public string? PatientGsm { get;set; }

        public string? TCKN { get; set; }

        //bakanlığın oluşturduğu e-reçete numarası
        public string? PrescriptionNo { get; set;}

        //Cinsiyet default değeri : Erkek olsun
        public string? Gender { get; set; } = "E";

        //Default doğum tarihi : "01.01.1985"
        public string? BirthDate { get; set; } = "01.01.1985";

    }
}
