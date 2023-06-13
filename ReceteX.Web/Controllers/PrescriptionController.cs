using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using ReceteX.Models;
using ReceteX.Repository.Shared.Abstract;
using ReceteX.Repository.Shared.Concrete;

namespace ReceteX.Web.Controllers
{
    public class PrescriptionController : Controller

        
    {
        private readonly IUnitOfWork unitOfWork;

        public PrescriptionController(IUnitOfWork unitOfWork)
    {
        this.unitOfWork = unitOfWork;
    }

    [HttpGet]
        [Route("ReceteYaz/{id?}")]

        public IActionResult Write(string id)
        {
            Prescription prescription = unitOfWork.Prescriptions.GetFirstOrDefault(p=>p.Id==Guid.Parse(id));
            return View(prescription);
           
        }

        [HttpPost]

        public IActionResult Create (Prescription prescription) 
        {
            unitOfWork.Prescriptions.Add(prescription);
            unitOfWork.Save();
            return View(prescription);
        }


        [HttpPost]
        //Reçeteye tanı ekleyeceğiz.
        public IActionResult AddDiagnosis(Guid prescriptionId, Guid diagnosisId)
        {
            Prescription asil = unitOfWork.Prescriptions.GetFirstOrDefault(p => p.Id == prescriptionId);
            Diagnosis asilDiagnosis = unitOfWork.Diagnoses.GetFirstOrDefault(d => d.Id == diagnosisId);

            asil.Diagnoses.Add(asilDiagnosis);
            unitOfWork.Prescriptions.Update(asil);
            unitOfWork.Save();

            return Ok();

        }

        [HttpPost]
        public IActionResult AddDescription(Guid prescriptionId, Description description)
        {
            Prescription asil = unitOfWork.Prescriptions.GetFirstOrDefault(p => p.Id == prescriptionId);
            asil.Descriptions.Add(description);
            unitOfWork.Prescriptions.Update(asil);
            unitOfWork.Save();
            return Ok();
        }

        [HttpPost]
        public IActionResult AddMedicine(Guid prescriptionId, PrescriptionMedicine prescriptionMedicine)
        {
            Prescription asil = unitOfWork.Prescriptions.GetFirstOrDefault(p => p.Id == prescriptionId);
            asil.PrescriptionMedicines.Add(prescriptionMedicine);
            unitOfWork.Prescriptions.Update(asil);
            unitOfWork.Save();
            return Ok();
        }

        [HttpPost]
        public IActionResult RemoveMedicine(Guid prescriptionId, Guid prescriptionMedicineId)
        {
            Prescription asil = unitOfWork.Prescriptions.GetFirstOrDefault(p => p.Id == prescriptionId);
            asil.PrescriptionMedicines.Remove(asil.PrescriptionMedicines.FirstOrDefault(pm => pm.Id == prescriptionMedicineId));
            unitOfWork.Prescriptions.Update(asil);
            unitOfWork.Save();
            return Ok();
        }

        [HttpPost]
        public IActionResult RemoveDescription(Guid prescriptionId, Guid descriptionId)
        {
            Prescription asil = unitOfWork.Prescriptions.GetFirstOrDefault(p => p.Id == prescriptionId);
            asil.Descriptions.Remove(asil.Descriptions.FirstOrDefault(d => d.Id == descriptionId));
            unitOfWork.Prescriptions.Update(asil);
            unitOfWork.Save();
            return Ok();
        }

        [HttpPost]
        public IActionResult RemoveDiagnosis(Guid prescriptionId, Guid diagnosisId)
        {
            Prescription asil = unitOfWork.Prescriptions.GetFirstOrDefault(p => p.Id == prescriptionId);
            asil.Descriptions.Remove(asil.Descriptions.FirstOrDefault(d => d.Id == diagnosisId));
            unitOfWork.Prescriptions.Update(asil);
            unitOfWork.Save();
            return Ok();
        }
        //projeye minible sayfası giydirilecek ve login sayfası yapılacak.Ödev..]

    }
}

//GET: GET isteği, belirtilen bir kaynağın(örneğin bir web sayfası veya bir veri) alınmasını istemek için kullanılır. Bu yöntem, sunucudan veriyi almak için kullanılır ve genellikle bir kaynağın okunması veya görüntülenmesi için kullanılır. GET istekleri genellikle URL üzerinden parametrelerle iletilir ve verilerin sunucudan alınmasını sağlar.

//POST: POST isteği, sunucuya veri göndermek için kullanılır. Bu yöntem, bir kaynağa yeni bir veri eklemek veya mevcut bir kaynağı değiştirmek için kullanılır. POST istekleri genellikle bir formun sunucuya gönderilmesi veya bir API'ye veri gönderilmesi gibi durumlarda kullanılır.

//PUT: PUT isteği, belirtilen bir kaynağı güncellemek için kullanılır. Bu yöntem, belirli bir URL'deki kaynağın tamamen değiştirilmesini sağlar. Bir PUT isteği gönderildiğinde, sunucu istekte belirtilen kaynağı günceller veya oluşturur.

//DELETE: DELETE isteği, belirtilen bir kaynağı silmek için kullanılır. Bu yöntem, bir URL'deki kaynağın sunucu tarafından silinmesini sağlar. DELETE isteği gönderildiğinde, sunucu istekte belirtilen kaynağı siler.