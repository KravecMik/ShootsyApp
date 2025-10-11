using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using MongoDB.Driver;
using Shootsy.Database.Mongo;
using Shootsy.Models.Enums;
using System.Text.RegularExpressions;

namespace Shootsy.Repositories
{
    public class FileRepository : IFileRepository
    {
        private readonly IMongoCollection<FileStorageEntity> _collection;

        public FileRepository(IMongoCollection<FileStorageEntity> collection) => _collection = collection;

        public async Task<string> CreateAsync(FileStorageEntity fileItem, CancellationToken cancellationToken = default)
        {
            await _collection.InsertOneAsync(fileItem);
            return fileItem.Id;
        }

        public async Task<FileStorageEntity>? GetByIdAsync(string idFile, CancellationToken cancellationToken)
            => await _collection.Find(x => x.Id == idFile).FirstOrDefaultAsync(cancellationToken);

        public async Task<(IReadOnlyList<FileStorageEntity>, long total)>? GetListAsync(FileStorageFilter f, CancellationToken cancellationToken = default)
        {
            var fb = Builders<FileStorageEntity>.Filter;
            var filters = new List<FilterDefinition<FileStorageEntity>>();

            if (f.UserId is int userId)
                filters.Add(fb.Eq(x => x.IdUser, userId));

            if (f.FileIds is { Count: > 0 })
                filters.Add(fb.In(x => x.Id, f.FileIds));

            if (f.CreatedDateFrom is DateTime cFrom)
                filters.Add(fb.Gte(x => x.CreateDate, DateTime.SpecifyKind(cFrom, DateTimeKind.Utc)));
            if (f.CreatedDateTo is DateTime cTo)
                filters.Add(fb.Lte(x => x.CreateDate, DateTime.SpecifyKind(cTo, DateTimeKind.Utc)));

            if (f.EditDateFrom is DateTime uFrom)
                filters.Add(fb.Gte(x => x.EditDate, DateTime.SpecifyKind(uFrom, DateTimeKind.Utc)));
            if (f.EditDateTo is DateTime uTo)
                filters.Add(fb.Lte(x => x.EditDate, DateTime.SpecifyKind(uTo, DateTimeKind.Utc)));

            if (!string.IsNullOrWhiteSpace(f.Search))
            {
                var pattern = RegexEscapeForMongo(f.Search.Trim());
                var regex = new BsonRegularExpression(pattern, "i");

                filters.Add(fb.Or(
                    fb.Regex(x => x.FileInfo.FileName, regex),
                    fb.Regex(x => x.FileInfo.Extension, regex),
                    fb.Regex(x => x.FileInfo.ContentPath, regex)
                ));
            }

            var filter = filters.Count > 0 ? fb.And(filters) : FilterDefinition<FileStorageEntity>.Empty;

            var sort = f.SortBy switch
            {
                SortByEnum.EditDate => f.SortDescending
                    ? Builders<FileStorageEntity>.Sort.Descending(x => x.EditDate).Descending(x => x.CreateDate)
                    : Builders<FileStorageEntity>.Sort.Ascending(x => x.EditDate).Ascending(x => x.CreateDate),
                SortByEnum.IdUser => f.SortDescending
                    ? Builders<FileStorageEntity>.Sort.Descending(x => x.IdUser).Descending(x => x.CreateDate)
                    : Builders<FileStorageEntity>.Sort.Ascending(x => x.IdUser).Ascending(x => x.CreateDate),
                SortByEnum.FileName => f.SortDescending
                    ? Builders<FileStorageEntity>.Sort.Descending(x => x.FileInfo.FileName)
                    : Builders<FileStorageEntity>.Sort.Ascending(x => x.FileInfo.FileName),
                SortByEnum.Extension => f.SortDescending
                    ? Builders<FileStorageEntity>.Sort.Descending(x => x.FileInfo.Extension)
                    : Builders<FileStorageEntity>.Sort.Ascending(x => x.FileInfo.Extension),
                _ => f.SortDescending
                    ? Builders<FileStorageEntity>.Sort.Descending(x => x.CreateDate)
                    : Builders<FileStorageEntity>.Sort.Ascending(x => x.CreateDate),
            };

            var total = await _collection.CountDocumentsAsync(filter, cancellationToken: cancellationToken);
            var items = await _collection.Find(filter)
                                    .Sort(sort)
                                    .Skip(Math.Max(0, f.Offset))
                                    .Limit(Math.Clamp(f.Limit, 1, 200))
                                    .ToListAsync(cancellationToken);

            return (items, total);
        }

        public async Task<bool> ReplaceAsync(FileStorageEntity entity, CancellationToken cancellationToken = default)
        {
            var res = await _collection.ReplaceOneAsync(x => x.Id == entity.Id, entity, cancellationToken: cancellationToken);
            return res.MatchedCount == 1; 
        }

        public async Task DeleteByIdAsync(string idFile, CancellationToken cancellationToken)
            => await _collection.DeleteOneAsync(x => x.Id == idFile, cancellationToken);

        private static string RegexEscapeForMongo(string input)
        {
            return Regex.Escape(input);
        }
    }
}