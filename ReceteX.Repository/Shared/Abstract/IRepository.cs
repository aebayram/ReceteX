using ReceteX.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ReceteX.Repository.Shared.Abstract
{
	public interface IRepository<T> where T : BaseModel
	{

		//önce interfacelerden başladık
		//dışarıdan herhangi bir parametre almıyor
		//bu dışarıdan ne var ne yok getirebileceğim getall
		IQueryable<T> GetAll();
		IQueryable<T> GetAll(Expression<Func<T, bool>> filter);

		//bu dışarıdan filtreleyebileceğim getall

		IQueryable<T> GetAllDeleted();
		IQueryable<T> GetAllDeleted(Expression<Func<T, bool>> filter);

		T GetById(Guid id);
		void Add(T entity);
		void Update(T entity);
		void Remove(Guid id);


		void AddRange(IEnumerable<T> entities);
		void UpdateRange(IEnumerable<T> entities);
		void RemoveRange(IEnumerable<T> entities);

		T GetFirstOrDefault(Expression<Func<T, bool>> filter);
	}
}



//Dışarıdan bir adet nesne alacak geriye değer döndürmeyecek


//programımızda bir şey silecekken id ile siliyoruz. Reposirtoryde düzenleme yaptık