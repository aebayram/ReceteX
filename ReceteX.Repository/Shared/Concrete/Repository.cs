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
	public class Repository<T> : IRepository<T> where T : BaseModel
	{
		private readonly ApplicationDbContext _db;
		internal DbSet<T> dbSet;

		public Repository(ApplicationDbContext db)
		{
			_db = db;
			dbSet = db.Set<T>();
		}

		public void Add(T entity)
		{
			//    _db.Books.Add(entity);
			dbSet.Add(entity);
		}

		public void AddRange(IEnumerable<T> entities)
		{
			dbSet.AddRange(entities);
		}

		public virtual IQueryable<T> GetAll()
		{
			return dbSet.Where(t => t.isDeleted == false);
		}

		public IQueryable<T> GetAll(Expression<Func<T, bool>> filter)
		{
			return GetAll().Where(filter);
		}

		public IQueryable<T> GetAllDeleted(Expression<Func<T, bool>> filter)
		{
			return dbSet.Where(t => t.isDeleted==true).Where(filter);
		}

		public virtual IQueryable<T> GetAllDeleted()
		{
			return dbSet.Where(t => t.isDeleted == true);
		}

		public T GetById(Guid id)
		{
			return dbSet.Find(id);
			// return GetFirstOrDefault(t=>t.Id==id);
		}

		public virtual T GetFirstOrDefault(Expression<Func<T, bool>> filter)
		{
			return dbSet.Where(t => t.isDeleted==false).AsNoTracking().FirstOrDefault(filter);
		}

		public void Remove(Guid id)
		{
			T entity = GetFirstOrDefault(t => t.Id == id);

			entity.isDeleted = true;

			dbSet.Update(entity);

		}

		public void RemoveRange(IEnumerable<T> entities)
		{
			foreach (T item in entities)
			{
				item.isDeleted = true;

			}

			dbSet.UpdateRange(entities);

		}

		public void Update(T entity)
		{

			dbSet.Update(entity);
		}

		public void UpdateRange(IEnumerable<T> entities)
		{


			dbSet.UpdateRange(entities);
		}
	}
}