using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using Shootsy.Database;
using Shootsy.Database.Entities;
using Shootsy.Models.Dtos;
using Shootsy.Service;

namespace Shootsy.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationContext _context;
        InternalConstants _internalConstants;
        private readonly IKafkaProducerService _kafkaProducer;

        public UserRepository(ApplicationContext context, InternalConstants internalConstants, IKafkaProducerService kafkaProducer)
        {
            _context = context;
            _internalConstants = internalConstants;
            _kafkaProducer = kafkaProducer;
        }

        public async Task<int> CreateUserAsync(UserEntity user, CancellationToken cancellationToken = default)
        {
            var userEntityEntry = await _context.Users.AddAsync(user, cancellationToken);
            user.CreateDate = DateTime.UtcNow;
            user.EditDate = DateTime.UtcNow;

            await _context.SaveChangesAsync(cancellationToken);
            userEntityEntry.State = EntityState.Detached;
            await _kafkaProducer.ProduceFileEventAsync("user.created", new { user.Id });

            return user.Id;
        }

        public async Task<UserEntity?> GetUserByIdAsync(int userId, CancellationToken cancellationToken)
        {
            return await _context.Users.AsNoTracking()
                .Include(u => u.ProfessionEntity)
                .Include(u => u.CityEntity)
                .Include(u => u.GenderEntity)
                .FirstOrDefaultAsync(x => x.Id == userId, cancellationToken);
        }

        public async Task<UserEntity?> GetUserByLoginAsync(string userLogin, CancellationToken cancellationToken)
        {
            return await _context.Users.AsNoTracking()
                .Include(u => u.ProfessionEntity)
                .Include(u => u.CityEntity)
                .Include(u => u.GenderEntity)
                .FirstOrDefaultAsync(x => x.Login == userLogin, cancellationToken);
        }

        public async Task<UserEntity?> GetUserByGuidAsync(Guid guid, CancellationToken cancellationToken)
        {
            var sessionEntity = await _context.UserSessions.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Guid == guid, cancellationToken);

            if (sessionEntity is null)
                return null;

            return await GetUserByIdAsync(sessionEntity.UserId, cancellationToken);
        }

        public async Task<(IReadOnlyList<UserEntity>, int)> GetUsersListAsync(UserFilterDto filter, CancellationToken cancellationToken = default)
        {
            var query = _context.Users
                   .Include(u => u.ProfessionEntity)
                   .Include(u => u.CityEntity)
                   .Include(u => u.GenderEntity)
                   .AsNoTracking();

            if (!string.IsNullOrEmpty(filter.City))
                query = query.Where(u => u.CityEntity.CityName.ToLower() == filter.City.ToLower());

            if (!string.IsNullOrEmpty(filter.Profession))
                query = query.Where(u => u.ProfessionEntity.Name.ToLower() == filter.Profession.ToLower());

            if (!string.IsNullOrEmpty(filter.Category))
                query = query.Where(u => u.ProfessionEntity.Category.ToLower() == filter.Category.ToLower());

            if (!string.IsNullOrEmpty(filter.Gender))
                query = query.Where(u => u.GenderEntity.GenderName.ToLower() == filter.Gender.ToLower());

            if (!string.IsNullOrEmpty(filter.Search))
            {
                query = query.Where(u =>
                    u.Firstname.Contains(filter.Search) ||
                    u.Lastname.Contains(filter.Search) ||
                    u.Login.Contains(filter.Search));
            }

            var totalCount = await query.CountAsync(cancellationToken);

            var users = await query
                .OrderBy(u => u.Id)
                .Skip((filter.Page - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToListAsync(cancellationToken);

            return (users, totalCount);
        }

        public async Task UpdateUserAsync(UserEntity user, JsonPatchDocument<UserEntity> jsonPatchDocument, CancellationToken cancellationToken = default)
        {
            jsonPatchDocument.ApplyTo(user);

            user.EditDate = DateTime.UtcNow;
            _context.Update(user);
            await _context.SaveChangesAsync();
            await _kafkaProducer.ProduceFileEventAsync("user.updated", new { user.Id });
        }

        public async Task DeleteUserByIdAsync(int userId, CancellationToken cancellationToken)
        {
            var userEntityEntry = await _context.Users.Where(x => x.Id == userId)
                .ExecuteDeleteAsync();
            await _context.SaveChangesAsync(cancellationToken);
            await _kafkaProducer.ProduceFileEventAsync("user.deleted", new { userId });
        }

        public async Task<Guid> CreateSessionAsync(int userId, CancellationToken cancellationToken)
        {
            var sessionEntity = new UserSessionEntity
            {
                UserId = userId,
                Guid = Guid.NewGuid()
            };
            var sessionEntityEntry = await _context.UserSessions.AddAsync(sessionEntity, cancellationToken);

            sessionEntity.StartDate = DateTime.UtcNow;
            sessionEntity.EndDate = DateTime.UtcNow.AddDays(_internalConstants.SessionActivityTime);

            await _context.SaveChangesAsync(cancellationToken);
            sessionEntityEntry.State = EntityState.Detached;

            return sessionEntity.Guid;
        }

        public async Task<Guid?> GetLastSessionAsync(int userId, CancellationToken cancellationToken)
        {
            var session = await _context.UserSessions.AsNoTracking()
                .Where(x => x.UserId.Equals(userId))
                .OrderByDescending(x => x.Id)
                .FirstOrDefaultAsync(cancellationToken);

            if (session is null)
                return Guid.Empty;

            return session.Guid;
        }

        public async Task<UserSessionEntity?> GetSessionByGuidAsync(Guid guid, CancellationToken cancellationToken)
        {
            return await _context.UserSessions.AsNoTracking()
                .Where(x => x.Guid == guid)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<bool> IsAuthorizedAsync(string? session, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(session) || !Guid.TryParse(session, out Guid guid))
                return false;

            var sessionEntity = await GetSessionByGuidAsync(guid, cancellationToken);
            return sessionEntity?.EndDate >= DateTime.UtcNow;
        }
    }
}