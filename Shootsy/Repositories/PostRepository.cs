using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;
using MongoDB.Bson;
using MongoDB.Driver;
using Shootsy.Database.Mongo;
using Shootsy.Models.Enums;
using Shootsy.Repositories.Interfaces;
using Shootsy.Service;
using System.Text.RegularExpressions;

namespace Shootsy.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly IMongoCollection<PostEntity> _collection;
        private readonly IKafkaProducerService _kafkaProducer;

        public PostRepository(IMongoDatabase database, IKafkaProducerService kafkaProducer)
        { 
            _collection = database.GetCollection<PostEntity>("Post");
            _kafkaProducer = kafkaProducer;
        }

        public async Task<string> CreateAsync(PostEntity post, CancellationToken cancellationToken)
        {
            await _collection.InsertOneAsync(post);
            await _kafkaProducer.ProduceFileEventAsync("post.created", new { post.Id });

            return post.Id;
        }

        public async Task DeleteByIdAsync(string idPost, CancellationToken cancellationToken)
        {
            await _collection.DeleteOneAsync(x => x.Id == idPost, cancellationToken);
            await _kafkaProducer.ProduceFileEventAsync("post.deleted", new { idPost });
        }

        public async Task<PostEntity>? GetByIdAsync(string idPost, CancellationToken cancellationToken)
            => await _collection.Find(x => x.Id == idPost).FirstOrDefaultAsync(cancellationToken);

        public async Task<(IReadOnlyList<PostEntity>, long total)>? GetListAsync(PostFilterModel f, CancellationToken cancellationToken = default)
        {
            var fb = Builders<PostEntity>.Filter;
            var filters = new List<FilterDefinition<PostEntity>>();

            if (f.UserId is int userId)
                filters.Add(fb.Eq(x => x.IdUser, userId));

            if (f.PostIds is { Count: > 0 })
                filters.Add(fb.In(x => x.Id, f.PostIds));

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
                    fb.Regex(x => x.Text, regex)
                ));
            }

            var filter = filters.Count > 0 ? fb.And(filters) : FilterDefinition<PostEntity>.Empty;

            var sort = f.SortBy switch
            {
                PostSortByEnum.EditDate => f.SortDescending
                    ? Builders<PostEntity>.Sort.Descending(x => x.EditDate).Descending(x => x.CreateDate)
                    : Builders<PostEntity>.Sort.Ascending(x => x.EditDate).Ascending(x => x.CreateDate),
                PostSortByEnum.IdUser => f.SortDescending
                    ? Builders<PostEntity>.Sort.Descending(x => x.IdUser).Descending(x => x.CreateDate)
                    : Builders<PostEntity>.Sort.Ascending(x => x.IdUser).Ascending(x => x.CreateDate),
                _ => f.SortDescending
                    ? Builders<PostEntity>.Sort.Descending(x => x.CreateDate)
                    : Builders<PostEntity>.Sort.Ascending(x => x.CreateDate),
            };

            var total = await _collection.CountDocumentsAsync(filter, cancellationToken: cancellationToken);
            var items = await _collection.Find(filter)
                                    .Sort(sort)
                                    .Skip(Math.Max(0, f.Offset))
                                    .Limit(Math.Clamp(f.Limit, 1, 200))
                                    .ToListAsync(cancellationToken);

            return (items, total);
        }


        public async Task<bool> ReplaceAsync(PostEntity entity, CancellationToken cancellationToken = default)
        {
            var res = await _collection.ReplaceOneAsync(x => x.Id == entity.Id, entity, cancellationToken: cancellationToken);
            await _kafkaProducer.ProduceFileEventAsync("post.updated", new { entity.Id });
            return res.MatchedCount == 1;
        }

        private static string RegexEscapeForMongo(string input)
        {
            return Regex.Escape(input);
        }
    }
}
