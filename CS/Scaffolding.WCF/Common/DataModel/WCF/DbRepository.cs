using System;
using System.Linq;
using System.Data;
using System.Linq.Expressions;
using System.Collections.Generic;
using Scaffolding.WCF.Common.Utils;
using Scaffolding.WCF.Common.DataModel;
using Scaffolding.WCF.Common.DataModel.DesignTime;
using System.Collections.ObjectModel;
using System.Data.Services.Client;

namespace Scaffolding.WCF.Common.DataModel.WCF {
    public class DbRepository<TEntity, TPrimaryKey, TDbContext> : DbReadOnlyRepository<TEntity, TDbContext>, IRepository<TEntity, TPrimaryKey>
        where TEntity : class, new()
        where TDbContext : DataServiceContext {

        readonly string dbSetName;
        readonly Expression<Func<TEntity, TPrimaryKey>> getPrimaryKeyExpression;
        readonly EntityTraits<TEntity, TPrimaryKey> entityTraits;

        public DbRepository(DbUnitOfWork<TDbContext> unitOfWork, Expression<Func<TDbContext, DataServiceQuery<TEntity>>> dbSetAccessorExpression, Expression<Func<TEntity, TPrimaryKey>> getPrimaryKeyExpression, bool useExtendedDataQuery)
            : base(unitOfWork, dbSetAccessorExpression.Compile(), useExtendedDataQuery) {
            Expression body = dbSetAccessorExpression.Body;
            while(body is MethodCallExpression) {
                body = ((MethodCallExpression)body).Object;
            }
            this.dbSetName = ((MemberExpression)body).Member.Name;
            this.getPrimaryKeyExpression = getPrimaryKeyExpression;
            this.entityTraits = ExpressionHelper.GetEntityTraits(this, getPrimaryKeyExpression);
        }
        TEntity IRepository<TEntity, TPrimaryKey>.Find(TPrimaryKey primaryKey) {
            try {
                var entity = LocalCollection.SingleOrDefault(x => object.Equals(GetPrimaryKeyCore(x), primaryKey));
                if(entity != null)
                    return entity;
                entity = FindCore(primaryKey);
                if(entity != null)
                    LocalCollection.Load(entity);
                return entity;
            } catch (DataServiceQueryException) {
                return null;
            }
        }
        void IRepository<TEntity, TPrimaryKey>.Add(TEntity entity) {
            AddCore(entity);
        }
        void IRepository<TEntity, TPrimaryKey>.Remove(TEntity entity) {
            RemoveCore(entity);
        }
        TEntity IRepository<TEntity, TPrimaryKey>.Create(bool add) {
            return CreateCore(add);
        }
        void IRepository<TEntity, TPrimaryKey>.Update(TEntity entity) {
            UpdateCore(entity);
        }
        EntityState IRepository<TEntity, TPrimaryKey>.GetState(TEntity entity) {
            return GetStateCore(entity);
        }
        Expression<Func<TEntity, TPrimaryKey>> IRepository<TEntity, TPrimaryKey>.GetPrimaryKeyExpression {
            get { return this.getPrimaryKeyExpression; }
        }
        void IRepository<TEntity, TPrimaryKey>.SetPrimaryKey(TEntity entity, TPrimaryKey primaryKey) {
            SetPrimaryKeyCore(entity, primaryKey);
        }
        TPrimaryKey IRepository<TEntity, TPrimaryKey>.GetPrimaryKey(TEntity entity) {
            return GetPrimaryKeyCore(entity);
        }
        bool IRepository<TEntity, TPrimaryKey>.HasPrimaryKey(TEntity entity) {
            return entityTraits.HasPrimaryKey(entity);
        }
        protected virtual void AddCore(TEntity entity) {
            LocalCollection.Add(entity);
        }
        protected virtual TEntity CreateCore(bool add) {
            TEntity newEntity = new TEntity();
            if(add) {
                AddCore(newEntity);
            }
            return newEntity;
        }
        protected virtual void UpdateCore(TEntity entity) {
            UnitOfWork.Context.UpdateObject(entity);
        }
        protected virtual EntityState GetStateCore(TEntity entity) {
            var descriptor = UnitOfWork.Context.GetEntityDescriptor(entity);
            return descriptor != null ? GetEntityState(descriptor.State) : EntityState.Detached;
        }
        static EntityState GetEntityState(EntityStates entityStates) {
            switch(entityStates) {
                case EntityStates.Added:
                    return EntityState.Added;
                case EntityStates.Deleted:
                    return EntityState.Deleted;
                case EntityStates.Detached:
                    return EntityState.Detached;
                case EntityStates.Modified:
                    return EntityState.Modified;
                case EntityStates.Unchanged:
                    return EntityState.Unchanged;
                default:
                    throw new NotImplementedException();
            }
        }
        protected virtual TEntity FindCore(TPrimaryKey primaryKey) {
            return DbSet.Where(ExpressionHelper.GetKeyEqualsExpression<TEntity, TEntity, TPrimaryKey>(getPrimaryKeyExpression, primaryKey)).Take(1).ToArray().FirstOrDefault();
        }
        protected virtual void RemoveCore(TEntity entity) {
            try {
                LocalCollection.Remove(entity);
            } catch (Exception ex) {
                throw DbExceptionsConverter.Convert(ex);
            }
        }
        protected virtual TPrimaryKey GetPrimaryKeyCore(TEntity entity) {
            return entityTraits.GetPrimaryKey(entity);
        }
        protected virtual void SetPrimaryKeyCore(TEntity entity, TPrimaryKey primaryKey) {
            var setPrimaryKeyAction = entityTraits.SetPrimaryKey;
            setPrimaryKeyAction(entity, primaryKey);
        }
        TEntity IRepository<TEntity, TPrimaryKey>.Reload(TEntity entity) {
            int index = this.LocalCollection.IndexOf(entity);
            UnitOfWork.Context.Detach(entity);
            TEntity newEntity = FindCore(GetPrimaryKeyCore(entity));
            if(newEntity == null)
                LocalCollection.RemoveAt(index);
            else if(index >= 0)
                LocalCollection[index] = newEntity;
            return newEntity;
        }
    }
}