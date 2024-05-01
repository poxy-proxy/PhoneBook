using Dapper;
using System.Collections.Generic;
using System.Data.SqlClient;
using PhoneBook.Models;
using System;
using System.Data;
using System.Data.Entity;

namespace PhoneBook.DB
{
    public class DataContext : IDisposable
    {
        private readonly IDbConnection _connection;

        public DataContext(string connectionString)
        {
            _connection = new SqlConnection(connectionString); 
            _connection.Open();
        }



        public IEnumerable<T> Get<T>(string sql, object param = null)
        {
            return _connection.Query<T>(sql, param);
        }

        public void Insert<T>(T entity)
        {
            _connection.Insert(entity);

           
        }

        public void Update<T>(T entity)
        {
            _connection.Update(entity);
        }

        public void Delete<T>(T entity)
        {
            _connection.Delete(entity);
        }

        public IEnumerable<T> GetList<T>()
        {
           return  _connection.GetList<T>();
           
        }
       

        public void Dispose()
        {
            _connection.Close();
        }
    }
}
