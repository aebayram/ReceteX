using Microsoft.AspNetCore.Mvc;
using ReceteX.Models;
using ReceteX.Repository.Shared.Abstract;

namespace ReceteX.Web.Controllers
{
	public class CustomerController : Controller
	{
		private readonly IUnitOfWork unitOfWork;

		public CustomerController(IUnitOfWork unitOfWork)
		{
			this.unitOfWork = unitOfWork;
		}

		public IActionResult Index()
		{
			return View();
		}

		[HttpPost]
		public IActionResult Create(Customer customer)
		{
			if (customer.Name != null)
			{
				unitOfWork.Customers.Add(customer);
				unitOfWork.Save();
				return Ok();
			}
			else
				return BadRequest();
		}

		public IActionResult GetAll()
		{
			return Json(new { data = unitOfWork.Customers.GetAllWithUserCount() });
		}

		public IActionResult GetById(Guid id)
		{
			return Json(unitOfWork.Customers.GetById(id));
		}

		[HttpPost]
		public IActionResult Update(Customer cust)
		{
			Customer asil = unitOfWork.Customers.GetFirstOrDefault(c => c.Id == cust.Id);
			asil.Name = cust.Name;
			unitOfWork.Customers.Update(asil);
			unitOfWork.Save();
			return Ok();
		}

		[HttpPost]
		public IActionResult Delete(Guid id)
		{
			unitOfWork.Customers.Remove(id);
			unitOfWork.Save();
			return Ok();
		}
	}
}