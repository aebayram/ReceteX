using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReceteX.Models
{
    public class PrescriptionMedicine : BaseModel
    {
        //bir ilaç bir reçetenin içinde bir defa varolabilir.
        //bu model bir reçetenin içindeki bir ilacı temsil eden bir model
        public Guid PrescriptionId { get; set; }

        public virtual Prescription Prescription { get; set; }
        public Guid MedicineId { get; set; }
        public virtual Medicine Medicine { get; set; }

        public int Quantity { get; set; }

        //günde kaç kere kaç adet kullanılacağı

        public int Dose1 { get; set; }
        public int Dose2 { get; set; }

        //kaç günde bir
        public int Period { get; set; }

        //günde, haftada, ayda, yılda ne kadar aralıklarla kullanılacağı
       public Guid MedicineUsagePeriodId { get; set; }
        public virtual MedicineUsagePeriod? MedicineUsagePeriod { get; set; }

        //ağız, kol vs..
        public Guid MedicineUsageTypeId {get; set;} 

        public virtual MedicineUsageType? MedicineUsageType { get; set; }

         

    }
}
