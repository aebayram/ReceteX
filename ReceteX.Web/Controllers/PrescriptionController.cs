using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReceteX.Models;
using ReceteX.Repository.Shared.Abstract;
using System.Security.Claims;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace ReceteX.Web.Controllers
{
    [Authorize]
    public class PrescriptionController : Controller
    {
        private readonly IUnitOfWork unitOfWork;

        public PrescriptionController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }


        public IActionResult Index()
        {
            Prescription prescription = new Prescription();
            prescription.StatusId = unitOfWork.Statuses.GetFirstOrDefault(s => s.Name == "Taslak").Id;
            prescription.AppUserId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            unitOfWork.Prescriptions.Add(prescription);
            unitOfWork.Save();
            return View(prescription);


        }




        [HttpPost]
        public IActionResult AddDiagnosis(Guid prescriptionId, Guid diagnosisId)
        {
            Prescription asil = unitOfWork.Prescriptions.GetAll(p => p.Id == prescriptionId).Include(p => p.Diagnoses).First();
            Diagnosis asilDiagnosis = unitOfWork.Diagnoses.GetFirstOrDefault(d => d.Id == diagnosisId);

            asil.Diagnoses.Add(asilDiagnosis);

            unitOfWork.Prescriptions.Update(asil);
            unitOfWork.Save();

            return Ok();

        }




        [HttpPost]
        public IActionResult GetDiagnoses(Guid prescriptionId)
        {
            return Json(unitOfWork.Prescriptions.GetAll(p => p.Id == prescriptionId).Include(p => p.Diagnoses));

        }
        [HttpPost]
        public IActionResult GetMedicines(Guid prescriptionId)
        {
            return Json(unitOfWork.Prescriptions.GetAll(p => p.Id == prescriptionId).Include(p => p.PrescriptionMedicines).ThenInclude(m => m.Medicine).Include(p => p.PrescriptionMedicines).ThenInclude(p => p.MedicineUsagePeriod).Include(p => p.PrescriptionMedicines).ThenInclude(p => p.MedicineUsageType).First());

        }


        [HttpPost]
        public IActionResult AddDescription(Description description)
        {
            unitOfWork.Descriptions.Add(description);
            unitOfWork.Save();
            return Ok();
        }



        [HttpPost]
        public JsonResult GetDescriptions(Guid prescriptionId)
        {
            return Json(unitOfWork.Descriptions.GetAll(d => d.PrescriptionId == prescriptionId).Include(d => d.DescriptionType));
        }

        [HttpPost]
        public IActionResult AddMedicine(Guid prescriptionId, PrescriptionMedicine prescriptionMedicine)
        {
            Prescription asil = unitOfWork.Prescriptions.GetAll(p => p.Id == prescriptionId).Include(p => p.PrescriptionMedicines).First();
            asil.PrescriptionMedicines.Add(prescriptionMedicine);
            unitOfWork.Prescriptions.Update(asil);
            unitOfWork.Save();
            return Ok();

        }

        [HttpPost]
        public IActionResult RemoveMedicine(Guid prescriptionId, Guid prescriptionMedicineId)
        {

            Prescription asil = unitOfWork.Prescriptions.GetAll(p => p.Id == prescriptionId).Include(p => p.PrescriptionMedicines).First();

            asil.PrescriptionMedicines.Remove(asil.PrescriptionMedicines.FirstOrDefault(pm => pm.Id == prescriptionMedicineId));

            unitOfWork.Prescriptions.Update(asil);
            unitOfWork.Save();
            return Ok();

        }

        [HttpPost]
        public IActionResult RemoveDescription(Guid descriptionId)
        {

            unitOfWork.Descriptions.Remove(descriptionId);


            unitOfWork.Save();
            return Ok();

        }

        [HttpPost]
        public IActionResult RemoveDiagnosis(Guid prescriptionId, Guid diagnosisId)
        {
            Prescription asil = unitOfWork.Prescriptions.GetAll(p => p.Id == prescriptionId).Include(p => p.Diagnoses).First();
            asil.Diagnoses.Remove(asil.Diagnoses.FirstOrDefault(d => d.Id == diagnosisId));

            unitOfWork.Prescriptions.Update(asil);
            unitOfWork.Save();
            return Ok();
        }
        [HttpPost]
        public IActionResult AddPatient(Guid prescriptionId, string patientTCK, string patientGSM)
        {
            Prescription asil = unitOfWork.Prescriptions.GetAll(p => p.Id == prescriptionId).First();
            asil.TCKN = patientTCK;
            asil.PatientGsm = patientGSM;
            unitOfWork.Prescriptions.Update(asil);
            unitOfWork.Save();
            return Ok();


        }

        [HttpPost]
        public XDocument GenerateXml(Guid prescriptionId)
        {
            Prescription asil = unitOfWork.Prescriptions.GetAll(p => p.Id == prescriptionId).Include(p => p.PrescriptionMedicines).ThenInclude(m => m.Medicine).Include(p => p.PrescriptionMedicines).ThenInclude(u => u.MedicineUsagePeriod).Include(p => p.PrescriptionMedicines).ThenInclude(t => t.MedicineUsageType).Include(p => p.Diagnoses).Include(p => p.AppUser).First();

            XElement ereceteBilgisi = new XElement("ereceteBilgisi",
                new XElement("doktorTcKimlikNo", asil.AppUser.TCKN),
                new XElement("receteTarihi", asil.DateCreated),
                new XElement("doktorAdi", asil.AppUser.Name),
                new XElement("doktorSoyadi", asil.AppUser.Surname),
            //her bir öge için geziyoruz
                     asil.PrescriptionMedicines.Select(med => new XElement("ereceteIlacBilgisi",
                            new XElement("adet", med.Quantity),
                            new XElement("barkod", med.Medicine.Barcode),
                            new XElement("kullanimDoz1", med.Dose1),
                            new XElement("kullanimDoz2", med.Dose2 + ".00"),
                            new XElement("kullanimPeriyot", med.Period),
                            new XElement("kullanimPeriyotBirimi", med.MedicineUsagePeriod.RemoteId),
                            new XElement("kullanimSekli", med.MedicineUsageType.RemoteId))
                    ),
                    //her bir öge için geziyoruz.
                    asil.Diagnoses.Select(diag => new XElement("ereceteTaniBilgisi",
                            new XElement("taniKodu", diag.Code))),
                        new XElement("kisiDVO",
                        new XElement("adi", asil.PatientFirstName),
                        new XElement("soyadi", asil.PatientLastName),
                        new XElement("cinsiyeti", asil.Gender),
                        new XElement("dogumTarihi", asil.BirthDate),
                        new XElement("tcKimlikNo", asil.TCKN))
                );

            XDeclaration dek = new XDeclaration("1.0", "utf-8", "yes");

            XDocument xDoc = new XDocument(dek, ereceteBilgisi);


            string xmlString = $"{xDoc.Declaration}\n{xDoc}";
            asil.XmlToSign = xmlString;
            unitOfWork.Prescriptions.Update(asil);
            unitOfWork.Save();


            return xDoc;
        }



    }
}


//GET: GET isteği, belirtilen bir kaynağın(örneğin bir web sayfası veya bir veri) alınmasını istemek için kullanılır. Bu yöntem, sunucudan veriyi almak için kullanılır ve genellikle bir kaynağın okunması veya görüntülenmesi için kullanılır. GET istekleri genellikle URL üzerinden parametrelerle iletilir ve verilerin sunucudan alınmasını sağlar.

//POST: POST isteği, sunucuya veri göndermek için kullanılır. Bu yöntem, bir kaynağa yeni bir veri eklemek veya mevcut bir kaynağı değiştirmek için kullanılır. POST istekleri genellikle bir formun sunucuya gönderilmesi veya bir API'ye veri gönderilmesi gibi durumlarda kullanılır.

//PUT: PUT isteği, belirtilen bir kaynağı güncellemek için kullanılır. Bu yöntem, belirli bir URL'deki kaynağın tamamen değiştirilmesini sağlar. Bir PUT isteği gönderildiğinde, sunucu istekte belirtilen kaynağı günceller veya oluşturur.

//DELETE: DELETE isteği, belirtilen bir kaynağı silmek için kullanılır. Bu yöntem, bir URL'deki kaynağın sunucu tarafından silinmesini sağlar. DELETE isteği gönderildiğinde, sunucu istekte belirtilen kaynağı siler.