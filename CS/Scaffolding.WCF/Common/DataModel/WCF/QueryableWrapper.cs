using System;
using System.Linq;
using System.Collections;
using System.Linq.Expressions;
using System.Collections.Generic;

namespace Scaffolding.WCF.Common.DataModel.WCF {
    public class QueryableWrapper<T, TEntity> : IQueryable<T> where TEntity : class {
        class EnumeratorWrapper : IEnumerator<T> {
            readonly IEnumerator<T> enumerator;
            readonly Action<TEntity> loadItemCallback;
            public EnumeratorWrapper(IEnumerator<T> enumerable, Action<TEntity> loadItemCallback) {
                this.enumerator = enumerable;
                this.loadItemCallback = loadItemCallback;
            }
            T IEnumerator<T>.Current {
                get { return enumerator.Current; }
            }

            void IDisposable.Dispose() {
                enumerator.Dispose();
            }

            object IEnumerator.Current {
                get { return enumerator.Current; }
            }
            bool IEnumerator.MoveNext() {
                bool result = enumerator.MoveNext();
                if(result) {
                    object item = enumerator.Current;
                    if(item is TEntity)
                        loadItemCallback(item as TEntity);
                }
                return result;
            }
            void IEnumerator.Reset() {
                enumerator.Reset();
            }
        }

        class QueryProviderWrapper : IQueryProvider {
            readonly IQueryProvider queryProvider;
            readonly Action<TEntity> loadItemCallback;
            public QueryProviderWrapper(IQueryProvider queryProvider, Action<TEntity> loadItemCallback) {
                this.loadItemCallback = loadItemCallback;
                this.queryProvider = queryProvider;
            }
            IQueryable<TElement> IQueryProvider.CreateQuery<TElement>(Expression expression) {
                return new QueryableWrapper<TElement, TEntity>(() => queryProvider.CreateQuery<TElement>(expression), loadItemCallback);
            }

            TResult IQueryProvider.Execute<TResult>(Expression expression) {
                var result = queryProvider.Execute<TResult>(expression);
                if(result is TEntity) {
                    object o = result;
                    loadItemCallback(o as TEntity);
                }
                return result;
            }
            IQueryable IQueryProvider.CreateQuery(Expression expression) {
                throw new NotImplementedException();
            }
            object IQueryProvider.Execute(Expression expression) {
                throw new NotImplementedException();
            }
        }

        readonly Lazy<IQueryable<T>> queryable;
        IQueryable<T> Queryable { get { return queryable.Value; } }
        readonly Lazy<IQueryProvider> queryProvider;
        IQueryProvider QueryProvider { get { return queryProvider.Value; } }
        protected Action<TEntity> LoadItemCallback { get; private set; }
        public QueryableWrapper(Func<IQueryable<T>> getQueryable, Action<TEntity> loadItemCallback) {
            this.queryable = new Lazy<IQueryable<T>>(getQueryable);
            this.queryProvider = new Lazy<IQueryProvider>(() => new QueryProviderWrapper(Queryable.Provider, this.LoadItemCallback));
            this.LoadItemCallback = loadItemCallback ?? (x => LoadItem(x));
        }
        protected virtual void LoadItem(TEntity entity) {
            throw new NotSupportedException();
        }
        IEnumerator<T> IEnumerable<T>.GetEnumerator() {
            return new EnumeratorWrapper(Queryable.GetEnumerator(), LoadItemCallback);
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return new EnumeratorWrapper(Queryable.GetEnumerator(), LoadItemCallback);
        }

        Type IQueryable.ElementType {
            get { return Queryable.ElementType; }
        }

        Expression IQueryable.Expression {
            get { return Queryable.Expression; }
        }

        IQueryProvider IQueryable.Provider {
            get { return QueryProvider; }
        }
    }
}