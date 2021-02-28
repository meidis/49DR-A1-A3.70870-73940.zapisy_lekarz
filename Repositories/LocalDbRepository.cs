using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using Newtonsoft.Json;
using Zadanie_4.Factories;
using Zadanie_4.Interfaces;

namespace Zadanie_4.Repositories
{
    public class LocalDbRepository<T> where T : ILocalDbEntity
    {
        private readonly Dictionary<Guid, T> _currentData = new Dictionary<Guid, T>();

        public LocalDbRepository()
        {
            var databaseName = InitDatabase();
            this.LoadDatabaseData(databaseName);
        }

        private static string InitDatabase()
        {
            try
            {
                var databaseName = GetDatabaseName();

                if (File.Exists(databaseName)) return databaseName;
                
                using (File.Create(databaseName))
                {
                        
                }

                return databaseName;
            }
            catch (Exception e)
            {
                MessageBox.Show("Error while creating database");
                throw;
            }
        }

        /// <summary>
        ///     Loads database data to memory
        /// </summary>
        /// <param name="databaseName"></param>
        private void LoadDatabaseData(string databaseName)
        {
            try
            {
                var databaseData = File.ReadAllText(databaseName);
                var jsonData = JsonConvert.DeserializeObject<IEnumerable<T>>(databaseData);

                if (jsonData == null) return;
                foreach (var dataEntity in jsonData) this._currentData.Add(dataEntity.Id, dataEntity);
            }
            catch (Exception)
            {
                MessageBox.Show("Error while loading database data");
            }
        }

        private void SaveDatabaseData()
        {
            try
            {
                var databaseName = GetDatabaseName();
                var currentEntities = this._currentData.Select(x => x.Value);
                var jsonString = JsonConvert.SerializeObject(currentEntities);

                if (string.IsNullOrEmpty(jsonString)) return;
                if (!File.Exists(databaseName)) throw new Exception("Could not find database file");
                File.WriteAllText(databaseName, jsonString);
            }
            catch (Exception)
            {
                MessageBox.Show("Error while saving data to database");
            }
        }

        private static string GetDatabaseName()
        {
            var databaseName = LocalDbNamesFactory.ResolveLocalDbName(typeof(T));
            if (string.IsNullOrEmpty(databaseName)) throw new Exception("Could not resolve database name");
            return databaseName;
        }

        public IEnumerable<T> GetAll()
        {
            return this._currentData.Select(x => x.Value);
        }

        public T GetById(Guid id)
        {
            return this._currentData
                .Where(x => x.Key == id)
                .Select(x => x.Value)
                .FirstOrDefault();
        }

        public void Add(T entity)
        {
            if (this._currentData.ContainsKey(entity.Id)) throw new Exception("This id already exists in database");
            
            this._currentData.Add(entity.Id, entity);
            
            this.SaveDatabaseData();
        }

        public T UpdateById(T entity)
        {
            if (!this._currentData.ContainsKey(entity.Id)) return default;

            this._currentData[entity.Id] = entity;

            this.SaveDatabaseData();
            return this._currentData[entity.Id];
        }

        public void Delete(Guid id)
        {
            if (!this._currentData.ContainsKey(id)) return;

            this._currentData.Remove(id);
            this.SaveDatabaseData();
        }
    }
}