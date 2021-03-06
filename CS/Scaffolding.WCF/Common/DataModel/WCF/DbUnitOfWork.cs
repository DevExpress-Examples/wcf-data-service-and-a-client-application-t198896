using System;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Data.Services.Client;

namespace Scaffolding.WCF.Common.DataModel.WCF {
    public abstract class DbUnitOfWork<TContext> : UnitOfWorkBase, IUnitOfWork where TContext : DataServiceContext {
        readonly Lazy<TContext> context;
        public TContext Context { get { return context.Value; } }
        public DbUnitOfWork(Func<TContext> contextFactory) {
            this.context = new Lazy<TContext>(contextFactory);
        }
        void IUnitOfWork.SaveChanges() {
            try {
                Context.SaveChanges();
            } catch (Exception ex) {
                throw DbExceptionsConverter.Convert(ex);
            }
        }

        bool IUnitOfWork.HasChanges() {
            return Context.Entities.Any(x => x.State != EntityStates.Unchanged) || Context.Links.Any(x => x.State != EntityStates.Unchanged);
        }
        protected IRepository<TEntity, TPrimaryKey>
            GetRepository<TEntity, TPrimaryKey>(Expression<Func<TContext, DataServiceQuery<TEntity>>> dbSetAccessor, Expression<Func<TEntity, TPrimaryKey>> getPrimaryKeyExpression, bool useExtendedDataQuery = false)
            where TEntity : class, new() {
            return GetRepositoryCore<IRepository<TEntity, TPrimaryKey>, TEntity>(() => new DbRepository<TEntity, TPrimaryKey, TContext>(this, dbSetAccessor, getPrimaryKeyExpression, useExtendedDataQuery));
        }
        protected IReadOnlyRepository<TEntity>
            GetReadOnlyRepository<TEntity>(Func<TContext, DataServiceQuery<TEntity>> dbSetAccessor, bool useExtendedDataQuery = false)
            where TEntity : class {
            return GetRepositoryCore<IReadOnlyRepository<TEntity>, TEntity>(() => new DbReadOnlyRepository<TEntity, TContext>(this, dbSetAccessor, useExtendedDataQuery));
        }
    }
}