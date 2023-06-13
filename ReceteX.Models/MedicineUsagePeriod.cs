using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReceteX.Models
{
    public class MedicineUsagePeriod:BaseModel
    {
        //günde, haftada, ayda, yılda ne kadar aralıklarla kullanılacağı
        //Bu kolonu kendimiz gireceğiz
        public int? RemoteId { get; set; }
    }
}
