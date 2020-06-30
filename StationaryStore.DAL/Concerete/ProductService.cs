using MongoDB.Bson;
using MongoDB.Driver;
using StationaryStore.DAL.Abstractions;
using StationaryStore.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StationaryStore.DAL.Concerete
{
    public class ProductService: IProductService
    {
        private readonly IMongoCollection<Product> _mongoService;
        public ProductService(IMongoDbSettings settings)
        {
            MongoClient client = new MongoClient(settings.ConnectionString);
            var db = client.GetDatabase(settings.Database);
            _mongoService = db.GetCollection<Product>(settings.Collection);
        }

        public async Task<bool> CreateAsync(Product model)
        {
            try
            {
                await _mongoService.InsertOneAsync(model);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Something went wrong: ", e);
                return false;
            }
        }

        public async Task<bool> DeleteAsync(string id)
        {
            try
            {
                var docId = new ObjectId(id);
                await _mongoService.DeleteOneAsync(m => m.Id == docId.ToString());
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Something went wrong: ", e);
                return false;
            }
        }

        public async Task<List<Product>> GetAllAsync()
        {
            return await _mongoService.Find(x => true).ToListAsync();
        }

        public async Task<Product> GetByIdAsync(string id)
        {
            try
            {
                var docId = new ObjectId(id);
                return await _mongoService.Find<Product>(m => m.Id == docId.ToString()).FirstOrDefaultAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine("Someting went wrong: ", e);
                return null;
            }
        }

        public async Task<bool> UpdateAsync(string id, Product model)
        {
            try
            {
                var docId = new ObjectId(id);
                await _mongoService.ReplaceOneAsync(m => m.Id == docId.ToString(), model);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Someting went wrong: ", e);
                return false;
            }
            
        }
    }
}
