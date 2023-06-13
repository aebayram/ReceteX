using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReceteX.Models
{
    public class AppUser : BaseModel
    {

        //app user müşteriye bağlı olacak

        public Guid CustomerId { get; set; }

        public virtual Customer? Customer { get; set; }

        //doktor adı base modelden geliyor surname'i burada alalım.
        public string? Surname { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Gsm { get; set; }
        public string? TCKN { get; set; }
        //e-signature pin code
        public string? PinCode { get; set; }
        //İstanbul = 34 
        public int? CityCode { get; set;}
        //numbers and letters mixed
        public string? MedulaPassword { get; set; }


        public bool isAdmin { get; set; } = false;

        public bool isActive { get; set; } = true;

        public bool isRememberMe { get; set; }  =false;

    }
}
