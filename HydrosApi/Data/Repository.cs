namespace HydrosApi.Data
{
    using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Linq.Expressions;
using HydrosApi.Models;
using Oracle.ManagedDataAccess.Client;
using System.Data.SqlClient;
using System.Reflection;
using System.Data.Entity.Core.Objects;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
    using System.Web.Http;

    public interface IRepository<TEntity>
    {

    }

    public class Repository<T> : IRepository<T> where T : class
    {
        public static List<T> GetAll()
        {
            using (var databaseContext = new OracleContext())
            {
                var query = databaseContext.Set<T>();
                return query.ToList();
            }
        }

        public static List<T> GetAll(OracleContext databaseContext)
        {
            var query = databaseContext.Set<T>();
            return query.ToList();
        }

        public static List<T> GetList(Expression<Func<T, bool>> predicate)
        {
            using (var databaseContext = new OracleContext())
            {
                return databaseContext.Set<T>().Where(predicate).ToList();
            }
        }
        public static void Delete(T entity)
        {
            using (var databaseContext = new OracleContext())
            {
                databaseContext.Entry(entity).State = EntityState.Deleted;
                databaseContext.SaveChanges();
            }
        }
        public static List<T> GetList(Expression<Func<T, bool>> predicate, Expression<Func<T, byte>> orderByPredicate)
        {
            using (var databaseContext = new OracleContext())
            {
                var query = databaseContext.Set<T>().Where(predicate).OrderBy(orderByPredicate);
                return query.ToList();
            }
        }

        public static List<T> GetList(Expression<Func<T, bool>> predicate, Expression<Func<T, decimal>> orderByPredicate, int count)
        {
            using (var databaseContext = new OracleContext())
            {
                var query = databaseContext.Set<T>().Where(predicate).OrderByDescending(orderByPredicate).Take(count);
                return query.ToList();
            }
        }

        public static IEnumerable<T> GetList(Expression<Func<T, bool>> predicate, OracleContext databaseContext)
        {
            var query = databaseContext.Set<T>().Where(predicate);
            return query;
        }

        public static T Get(Expression<Func<T, bool>> predicate)
        {
            using (var databaseContext = new OracleContext())
            {
                var query = databaseContext.Set<T>().FirstOrDefault(predicate);
                return query;
            }
        }

        public static T Get(Expression<Func<T, bool>> predicate, OracleContext databaseContext)
        {
            var query = databaseContext.Set<T>().FirstOrDefault(predicate);
            return query;
        }

        public static void Add(T entity)
        {
            using (var databaseContext = new OracleContext())
            {
                databaseContext.Set<T>().Add(entity);
                databaseContext.SaveChanges();
            }
        }

        public static void AddAll(List<T> entity)
        {
            using (var databaseContext = new OracleContext())
            {
                databaseContext.Set<T>().AddRange(entity);
                databaseContext.SaveChanges();
            }
        }

        public static List<T> AddAll(List<T> entity, OracleContext databaseContext)
        {            
            databaseContext.Set<T>().AddRange(entity);
            databaseContext.SaveChanges();
            return entity;
        }

        public static T Add(T entity, OracleContext databaseContext)
        {
            databaseContext.Set<T>().Add(entity);
            return entity;
        }

        public static void Remove(T entity, OracleContext databaseContext)
        {
            databaseContext.Set<T>().Remove(entity);
        }
         public static T Update(T entity) //updates all values
        {
            using (var databaseContext = new OracleContext())
            {
                databaseContext.Set<T>().Attach(entity);
                databaseContext.Entry(entity).State = EntityState.Modified;
                databaseContext.SaveChanges();                
                return entity;
            }
        }

        public static T UpdateSome(T entity, params Expression<Func<T, object>>[] updatedProperties)
        {         
             
            //Ensure only modified fields are updated.
            using (var databaseContext = new OracleContext())
            {
                var dbEntry = databaseContext.Entry(entity);

                foreach(var property in dbEntry.OriginalValues.PropertyNames)
                {
                    var original = dbEntry.OriginalValues.GetValue<object>(property);
                    var current = dbEntry.CurrentValues.GetValue<object>(property);

                    if(original != null && !original.Equals(current))
                    {
                        dbEntry.Property(property).IsModified = true;
                    }
                }

                return entity;
            }
        }

       /* public static T UpdateSome(T entity, Expression<Func<T, bool>> predicate) //Updates some values but not all
        {
            using (var databaseContext = new OracleContext())
            {
                var query = databaseContext.Set<T>().FirstOrDefault(predicate);
                databaseContext.Set<T>().Attach(entity);                 
                databaseContext.SaveChanges();
                return entity;
            }
        }*/

        public static List<T> ExecuteStoredProcedure(string sqlStatement, params object[] parameters)
        {
            using (var databaseContext = new OracleContext())
            {
                if (parameters == null)
                    return databaseContext.Database.SqlQuery<T>(sqlStatement).ToList();
                return databaseContext.Database.SqlQuery<T>(sqlStatement, parameters).ToList();
            }
        }
       
        public static void ExecuteEmptyStoredProcedure(string sqlStatement, params object[] parameters)
        {
            using (var databaseContext = new OracleContext())
            {
                if (parameters == null)
                    databaseContext.Database.ExecuteSqlCommand(sqlStatement);
                databaseContext.Database.ExecuteSqlCommand(sqlStatement, parameters);
            }
        }

    }
}
