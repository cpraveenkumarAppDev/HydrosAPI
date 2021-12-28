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
    using System.ComponentModel;

    public interface IAdwrRepository<TEntity>
    {

    }

    public class AdwrRepository<T> : IAdwrRepository<T> where T : class
    {
        public static List<T> GetAll()
        {
            using (var databaseContext = new ADWRContext())
            {
                var query = databaseContext.Set<T>();
                return query.ToList();
            }
        }

        public static List<T> GetAll(ADWRContext databaseContext)
        {
            var query = databaseContext.Set<T>();
            return query.ToList();
        }

        public static List<T> GetList(Expression<Func<T, bool>> predicate)
        {
            using (var databaseContext = new ADWRContext())
            {
                return databaseContext.Set<T>().Where(predicate).ToList();
            }
        }

        public static List<T> GetList(Expression<Func<T, bool>> predicate, Expression<Func<T, byte>> orderByPredicate)
        {
            using (var databaseContext = new ADWRContext())
            {
                var query = databaseContext.Set<T>().Where(predicate).OrderBy(orderByPredicate);
                return query.ToList();
            }
        }

        public static List<T> GetList(Expression<Func<T, bool>> predicate, Expression<Func<T, decimal>> orderByPredicate, int count)
        {
            using (var databaseContext = new ADWRContext())
            {
                var query = databaseContext.Set<T>().Where(predicate).OrderByDescending(orderByPredicate).Take(count);
                return query.ToList();
            }
        }

        public static IEnumerable<T> GetList(Expression<Func<T, bool>> predicate, ADWRContext databaseContext)
        {
            var query = databaseContext.Set<T>().Where(predicate);
            return query;
        }

        public static T Get(Expression<Func<T, bool>> predicate)
        {
            using (var databaseContext = new ADWRContext())
            {
                var query = databaseContext.Set<T>().FirstOrDefault(predicate);
                return query;
            }
        }

        public static T Get(Expression<Func<T, bool>> predicate, ADWRContext databaseContext)
        {
            var query = databaseContext.Set<T>().FirstOrDefault(predicate);
            return query;
        }

        //testing something out here

        public static T FormToModel(T model,HandleForm provider)
        {
            var form = provider.FormData;
            var prop = model.GetType();
            var files = provider.Files;

            foreach (var key in form)
            {
                var keyValue = form.GetValues(key.ToString())?.FirstOrDefault();

                if (keyValue != null)
                {
                    var fieldName = key.ToString().Trim('\"');
                    var property = model.GetType().GetProperty(fieldName);




                    //Convert the form value to the correct data type

                    if (property != null)
                    {
                        var propertyTypeName = property.PropertyType.Name;
                        var converter = TypeDescriptor.GetConverter(property.PropertyType);

                        if(property.PropertyType.ToString().IndexOf("byte",StringComparison.OrdinalIgnoreCase) > 0)
                        {
                            byte[] fileData = files[0].ReadAsByteArrayAsync().Result;
                            property.SetValue(model, fileData);
                        }
                        else
                        {
                            property.SetValue(model, converter.ConvertFrom(keyValue));
                        }



                        

                    }

                    //if (ffProp == property.PropertyType)
                    //{
                    //    property.SetValue(model, keyValue);
                   // }
                }
            }

            return model;
           
            
        }

        public static T Add(T entity)
        {
            using (var databaseContext = new ADWRContext())
            {
                databaseContext.Entry(entity).State = EntityState.Added;
                databaseContext.SaveChanges();

                return entity;
            }
        }

        public static void Delete(T entity)
        {
            using (var databaseContext = new ADWRContext())
            {
                databaseContext.Entry(entity).State = EntityState.Deleted;
                databaseContext.SaveChanges();
            }
        }

        public static void Delete(List<T> entity)
        {
            using (var databaseContext = new ADWRContext())
            {
                databaseContext.Entry(entity).State = EntityState.Deleted;                
                databaseContext.Set<T>().RemoveRange(entity);
                databaseContext.SaveChanges();
            }
        }

        public static void AddAll(List<T> entity)
        {
            using (var databaseContext = new ADWRContext())
            {
                databaseContext.Set<T>().AddRange(entity);
                databaseContext.SaveChanges();
            }
        }

        public static void Add(T entity, ADWRContext databaseContext)
        {
            databaseContext.Set<T>().Add(entity);
        }

        public static void Remove(T entity, ADWRContext databaseContext)
        {
            databaseContext.Set<T>().Remove(entity);
        }

        public static T Update(T entity)
        {
            using (var databaseContext = new ADWRContext())
            {
                databaseContext.Set<T>().Attach(entity);
                databaseContext.Entry(entity).State = EntityState.Modified;
                databaseContext.SaveChanges();
                
                return entity;
            }
        }

      

        public static List<T> ExecuteStoredProcedure(string sqlStatement, params object[] parameters)
        {
            using (var databaseContext = new ADWRContext())
            {
                if (parameters == null)
                    return databaseContext.Database.SqlQuery<T>(sqlStatement).ToList();
                return databaseContext.Database.SqlQuery<T>(sqlStatement, parameters).ToList();
            }
        }
        public static void ExecuteEmptyStoredProcedure(string sqlStatement, params object[] parameters)
        {
            using (var databaseContext = new ADWRContext())
            {
                if (parameters == null)
                    databaseContext.Database.ExecuteSqlCommand(sqlStatement);
                databaseContext.Database.ExecuteSqlCommand(sqlStatement, parameters);
            }
        }



    }
}