using Microsoft.EntityFrameworkCore;
using ReceteX.Data;
using ReceteX.Models;
using ReceteX.Repository.Shared.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ReceteX.Repository.Shared.Concrete
{
    public class Repository <T> : IRepository<T> where T : BaseModel
    {
        private readonly ApplicationDbContext _db;

        //hangi contexte bağlanması gerektiğini runtime da kendisi karar vermesi gerekiyor.
        internal DbSet<T> dbSet;

        public Repository(ApplicationDbContext db)
        {
            _db = db;
            //dblerin içindeki setlerin içindeki generic olanlara
            dbSet = db.Set<T>();
        }

        public void Add(T entity)
        {
            dbSet.Add(entity);
        }

        public void AddRange(IEnumerable<T> entities)
        {

            dbSet.AddRange(entities);
        }

        //sadece silinmemiş olanlarla aktif olanları getir
        public virtual IQueryable<T> GetAll()
        {
            //burada geriye bir şey döndürmem gerekiyor.
            return dbSet.Where(x => x.isDeleted == false);
        }

        public IQueryable<T> GetAll(Expression<Func<T, bool>> filter)
        {
            //return dbSet.Where(filter);
            //önce parametresiz olan getall çalışacak sonra buna düşecek
            return GetAll().Where(filter);
        }

		public T GetById(Guid id)
		{
            return (T)dbSet.Find(id);

		}

		public T GetFirstOrDefault(Expression<Func<T, bool>> filter)
        {
            //aslında bu da filtre uyguluyor ama bir tane nesne gönderiyor bize
            return GetAll().FirstOrDefault(filter);

		}

        //public void Remove(T entity)
        //{
        //    entity.isDeleted = true;
        //    //entity.isActive = false;
        //    //entity.DateModified = DateTime.Now;
        //    dbSet.Update(entity);
        //}
         
        public void Remove(Guid id)
        
        {
            //programımızda bir şey silecekken id ile siliyoruz. Reposirtoryde düzenleme yaptık
            //T entity = GetFirstOrDefault(t=>t.Id== id);
            T entity = dbSet.Find(id);
            entity.isDeleted = true;
            dbSet.Update(entity);
        }

	public void RemoveRange(IEnumerable<T> entities)
        {
            foreach (T item in entities)
            {
                item.isDeleted = true;
                //item.DateModified = DateTime.Now;

            }
            dbSet.UpdateRange(entities);
        }

        public void Update(T entity)
        {
            //entity.DateModified = DateTime.Now;
            dbSet.Update(entity);
        }

        public void UpdateRange(IEnumerable<T> entities)
        {
           
            dbSet.UpdateRange(entities);
        }
    }
}
