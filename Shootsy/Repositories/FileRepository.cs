using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using Shootsy.Database.Mongo;
using Shootsy.Service;

namespace Shootsy.Repositories
{
    public class FileRepository : IFileRepository
    {
        private readonly IMongoCollection<FileStorageEntity> _collection;
        private readonly IKafkaProducerService _kafkaProducer;

        public FileRepository(IMongoDatabase database, IKafkaProducerService kafkaProducer)
        {
            _collection = database.GetCollection<FileStorageEntity>("FileStorage");
            _kafkaProducer = kafkaProducer;
        }

        public async Task<string> CreateFileAsync(FileStorageEntity file, CancellationToken cancellationToken = default)
        {
            await _collection.InsertOneAsync(file);
            await _kafkaProducer.ProduceFileEventAsync("file.created", new { file.Id });
            return file.Id;
        }

        public async Task<FileStorageEntity?> GetFileByIdAsync(string idFile, CancellationToken cancellationToken)
        {
            return await _collection.Find(x => x.Id == idFile)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<FileStorageEntity?>> GetFilesListByUserIdAsync(int userId, CancellationToken cancellationToken = default)
        {
            return await _collection.Find(x => x.UserId == userId)
                .ToListAsync(cancellationToken);
        }

        public async Task<bool> UpdateFileAsync(FileStorageEntity entity, CancellationToken cancellationToken = default)
        {
            var res = await _collection.ReplaceOneAsync(x => x.Id == entity.Id, entity, cancellationToken: cancellationToken);
            await _kafkaProducer.ProduceFileEventAsync("file.updated", new { entity.Id });
            return res.MatchedCount == 1;
        }

        public async Task<bool> DeleteFileByIdAsync(string idFile, CancellationToken cancellationToken)
        {
            var res = await _collection.DeleteOneAsync(x => x.Id == idFile, cancellationToken);
            await _kafkaProducer.ProduceFileEventAsync("file.deleted", new { idFile });
            return res.DeletedCount == 1;
        }
    }
}