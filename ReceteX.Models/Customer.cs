using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReceteX.Models
{
    //hangi müşteriye kaç kullanıcı olmuş 
    public class Customer : BaseModel
    {
        //bir müşterinin birden fazla kullanıcısı olabilir

        public ICollection<AppUser>? AppUsers { get; set; }
    }
}
