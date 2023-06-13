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

        //bu dışarıdan filtreleyebileceğim getall

        IQueryable<T> GetAll(Expression<Func<T, bool>> expression);

        //Geri dönüş değeri T
        T GetFirstOrDefault(Expression<Func<T, bool>> expression);


        T GetById(Guid id);





        //Dışarıdan bir adet nesne alacak geriye değer döndürmeyecek
        void Add(T entity);

        void Update(T entity);

		//programımızda bir şey silecekken id ile siliyoruz. Reposirtoryde düzenleme yaptık
		void Remove(Guid id);

        void RemoveRange(IEnumerable<T> entities);
        void UpdateRange(IEnumerable<T> entities);
        void AddRange(IEnumerable<T> entities);

       

    }
}
