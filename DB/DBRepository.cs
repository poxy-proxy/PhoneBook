using Dapper;
using System.Collections.Generic;
using System.Data.SqlClient;
using PhoneBook.Models;

namespace PhoneBook.DB
{
    public class DbRepository
    {
        private readonly string _connectionString;

        public DbRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

       

        public IEnumerable<Abonent> GetAbonets()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.Query<Abonent>("SELECT * FROM Abonents"); 
            }
        }

        public Abonent GetAbonentById(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.QueryFirstOrDefault<Abonent>(
                    @"SELECT a.*, s.Name AS StreetName
                FROM Abonent a
                JOIN Address ad ON a.AddressId = ad.Id
                JOIN Streets s ON ad.StreetId = s.Id
                WHERE a.Id = @Id",
                     new { Id = id });
            }
        }

        public void AddAbonent(Abonent abonent)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var addressId = connection.QuerySingle<int>(
                    @"INSERT INTO Address (StreetId, HouseNumber)
                VALUES (@StreetId, @HouseNumber);
                SELECT CAST(SCOPE_IDENTITY() AS INT)",
                    new { abonent.Address.Street.Id, abonent.Address.HouseNumber });

                connection.Execute(
                    @"INSERT INTO Abonent (FullName, AddressId)
                VALUES (@FullName, @AddressId)",
                    new { FullName = abonent.FullName, AddressId = addressId });

                foreach (var phoneNumber in abonent.PhoneNumbers)
                {
                    connection.Execute(
                        @"INSERT INTO PhoneNumber (AbonentId, Number, Type)
                    VALUES (@AbonentId, @Number, @Type)",
                        new { AbonentId = addressId, Number = phoneNumber.Number, Type = phoneNumber.Type });
                }
            }
        }

        public void UpdateAbonent(Abonent abonent)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Execute(
                    @"UPDATE Abonent
                SET FullName = @FullName
                WHERE Id = @Id",
                    new { Id = abonent.Id, FullName = abonent.FullName });

                connection.Execute(
                    @"UPDATE Address
                SET StreetId = @StreetId, HouseNumber = @HouseNumber
                WHERE Id = @AddressId",
                    new { StreetId = abonent.Address.Street.Id, HouseNumber = abonent.Address.HouseNumber, AddressId = abonent.Address.Id });

                connection.Execute(@"DELETE FROM PhoneNumber WHERE AbonentId = @AbonentId", new { AbonentId = abonent.Id });

                foreach (var phoneNumber in abonent.PhoneNumbers)
                {
                    connection.Execute(
                        @"INSERT INTO PhoneNumber (AbonentId, Number, Type)
                    VALUES (@AbonentId, @Number, @Type)",
                        new { AbonentId = abonent.Id, Number = phoneNumber.Number, Type = phoneNumber.Type });
                }
            }
        }

        public IEnumerable<PhoneNumber> GetPhoneNumbersByAbonentId(int abonentId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.Query<PhoneNumber>(
                    @"SELECT p.*
            FROM PhoneNumber p
            WHERE p.AbonentId = @AbonentId",
                    new { AbonentId = abonentId });
            }
        }

        //public void AddPhoneNumber(PhoneNumber phoneNumber)
        //{
        //    using (var connection = new SqlConnection(_connectionString))
        //    {
        //        connection.Execute(
        //            @"INSERT INTO PhoneNumber (AbonentId, Number, Type)
        //    VALUES (@AbonentId, @Number, @Type)",
        //            new { phoneNumber.AbonentId, phoneNumber.Number, phoneNumber.Type });
        //    }
        //}

        public void UpdatePhoneNumber(PhoneNumber phoneNumber)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Execute(
                    @"UPDATE PhoneNumber
            SET Number = @Number, Type = @Type
            WHERE Id = @Id",
                    new { Id = phoneNumber.Id, Number = phoneNumber.Number, Type = phoneNumber.Type });
            }
        }

        public void DeletePhoneNumber(int phoneNumberId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Execute(@"DELETE FROM PhoneNumber WHERE Id = @Id", new { Id = phoneNumberId });
            }
        }

        public IEnumerable<Street> GetStreets()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.Query<Street>("SELECT * FROM Streets");
            }
        }

        public Street GetStreetById(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.QueryFirstOrDefault<Street>("SELECT * FROM Streets WHERE Id = @Id", new { Id = id });
            }
        }

        public void UpdateStreet(Street street)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Execute("UPDATE Streets SET Name = @Name WHERE Id = @Id", new { Id = street.Id, Name = street.Name });
            }
        }

        public void DeleteStreet(int streetId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Execute("DELETE FROM Streets WHERE Id = @Id", new { Id = streetId });
            }
        }
    }
 }
