
using Database.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;


namespace Database.Services
{
    public class DbReadService : IDbReadService
    {
        #region Properties
        private VODContext _db;
        #endregion

        #region Constructor
        public DbReadService(VODContext db)
        {
            _db = db;
        }
        #endregion

        #region Read Methods
        public async Task<List<TEntity>> GetAsync<TEntity>() where TEntity : class
        {
            return await _db.Set<TEntity>().ToListAsync();
        }

        public async Task<List<TEntity>> GetAsync<TEntity>(Expression<Func<TEntity, bool>> expression) where TEntity : class
        {
            return await _db.Set<TEntity>().Where(expression).ToListAsync();
        }

        public async Task<TEntity> SingleAsync<TEntity>(Expression<Func<TEntity, bool>> expression) where TEntity : class
        {
            return await _db.Set<TEntity>().SingleOrDefaultAsync(expression);

        }
        public async Task<bool> AnyAsync<TEntity>(Expression<Func<TEntity, bool>> expression) where TEntity : class
        {
            return await _db.Set<TEntity>().AnyAsync(expression);
        }

        public void Include<TEntity>() where TEntity : class
        {
            var propertyNames = _db.Model.FindEntityType(typeof(TEntity)).GetNavigations().Select(e => e.Name);

            foreach (var name in propertyNames)
                _db.Set<TEntity>().Include(name).Load();
        }

        public void Include<TEntity1, TEntity2>() where TEntity1 : class where TEntity2 : class
        {
            Include<TEntity1>();
            Include<TEntity2>();
        }
        public (int topics, int downloads, int topicTypes, int modules, int medias, int users) Count()
        {
            return (
                topics: _db.topics.Count(),
                downloads: _db.Downloads.Count(),
                topicTypes: _db.topicTypes.Count(),
                modules: _db.Modules.Count(),
                medias: _db.medias.Count(),
                users: _db.Users.Count());

        }

        #endregion
    }
}
