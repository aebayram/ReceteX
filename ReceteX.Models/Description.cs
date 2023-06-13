using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReceteX.Models
{
    public class Description : BaseModel
    {
        public string? Text { get; set; }

        //birden fazla açıklaması olabilir o yüzden description type oluşturduk
        public Guid DescriptionTypeId { get; set; }
        public virtual DescriptionType? DescriptionType { get; set; }
    }
}
