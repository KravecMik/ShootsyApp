using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shootsy.Database;
using Shootsy.Database.Entities;
using Shootsy.Dtos;
using System.Linq.Dynamic.Core;

namespace Shootsy.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationContext _context;
        private readonly IMapper _mapper;
        InternalConstants _internalConstants;

        public UserRepository(ApplicationContext context, IMapper mapper, InternalConstants internalConstants)
        {
            _context = context;
            _mapper = mapper;
            _internalConstants = internalConstants;
        }

        public async Task<int> CreateAsync(UserDto user, CancellationToken cancellationToken = default)
        {
            var userEntity = _mapper.Map<UserEntity>(user);
            var userEntityEntry = await _context.Users.AddAsync(userEntity, cancellationToken);

            userEntity.CreateDate = DateTime.UtcNow;
            userEntity.EditDate = DateTime.UtcNow;

            await _context.SaveChangesAsync(cancellationToken);
            userEntityEntry.State = EntityState.Detached;

            return userEntity.Id;
        }

        public async Task<UserDto>? GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            var userEntity = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
            if (userEntity is null)
                return null;
            return _mapper.Map<UserDto>(userEntity);
        }

        public async Task<UserDto>? GetByLoginAsync(string login, CancellationToken cancellationToken)
        {
            var userEntity = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Login == login, cancellationToken);
            if (userEntity is null)
                return null;
            return _mapper.Map<UserDto>(userEntity);
        }

        public async Task<UserDto>? GetByGuidAsync(Guid guid, CancellationToken cancellationToken)
        {
            var sessionEntity = await _context.UserSessions
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Guid == guid, cancellationToken);
            if (sessionEntity is null)
                return null;

            var user = await GetByIdAsync(sessionEntity.User, cancellationToken);
            if (user is null)
                return null;

            return user;
        }

        public async Task<Guid>? GetLastSessionAsync(int userId, CancellationToken cancellationToken)
        {
            var query = _context.UserSessions.AsNoTracking().Select(x => x).Where($"user eq {userId}");
            var sessionEntities = await query
                .Skip(0)
                .Take(1)
                .OrderBy("id desc")
                .ToArrayAsync(cancellationToken);
            var session = sessionEntities.FirstOrDefault();
            if (session is null)
                return Guid.Empty;

            return session.Guid;
        }

        public async Task<IReadOnlyList<UserDto>> GetListAsync(
            int limit,
            int offset,
            string filter,
            string sort,
            CancellationToken cancellationToken = default)
        {
            var query = _context.Users.AsNoTracking().Select(x => x).Where(filter);
            var userEntities = await query
                .Skip(offset)
                .Take(limit)
                .OrderBy(sort)
                .ToArrayAsync(cancellationToken);
            return _mapper.Map<IReadOnlyList<UserDto>>(userEntities);
        }

        public async Task UpdateAsync(UserDto userDto, JsonPatchDocument<UserDto> jsonPatchDocument, CancellationToken cancellationToken = default)
        {
            jsonPatchDocument.ApplyTo(userDto);
            var userEntity = _mapper.Map<UserEntity>(userDto);

            userEntity.EditDate = DateTime.UtcNow;
            _context.Update(userEntity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteByIdAsync(int id, CancellationToken cancellationToken)
        {
            var userEntityEntry = await _context.Users
                .Where(x => x.Id == id)
                .ExecuteDeleteAsync();
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<Guid> CreateSessionAsync(int userId, CancellationToken cancellationToken)
        {
            var sessionEntity = new UserSessionEntity
            {
                User = userId,
                Guid = Guid.NewGuid(),
                isActive = true,
            };
            var sessionEntityEntry = await _context.UserSessions.AddAsync(sessionEntity, cancellationToken);

            sessionEntity.SessionDateFrom = DateTime.UtcNow;
            sessionEntity.SessionDateTo = DateTime.UtcNow.AddDays(_internalConstants.SessionActivityTime);

            await _context.SaveChangesAsync(cancellationToken);
            sessionEntityEntry.State = EntityState.Detached;

            return sessionEntity.Guid;
        }

        public async Task UpdateSessionIsActiveStatusAsync(Guid guid, bool status, CancellationToken cancellationToken)
        {
            var userSessionEntity = _context.UserSessions.AsNoTracking().Where(x => x.Guid == guid).First();
            userSessionEntity.isActive = status;
            _context.Update(userSessionEntity);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeactivateUserSessions(int userId, CancellationToken cancellationToken)
        {
            var userSessionEntitys = await _context.UserSessions.AsNoTracking().Where(x => x.User == userId).ToArrayAsync(cancellationToken);
            foreach (var userSessionEntity in userSessionEntitys)
                await UpdateSessionIsActiveStatusAsync(userSessionEntity.Guid, false, cancellationToken);
        }

        public async Task<UserSessionDto>? GetSessionByGuidAsync(Guid guid, CancellationToken cancellationToken)
        {
            var sessionEntity = await _context.UserSessions
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Guid == guid, cancellationToken);

            if (sessionEntity is null)
            {
                return null;
            }
            return _mapper.Map<UserSessionDto>(sessionEntity);
        }

        public async Task<bool> IsAuthorized(string? session, CancellationToken cancellationToken)
        {
            if (session is null)
                return false;

            var sessionParseRes = Guid.TryParse(session, out Guid guid);
            if (!sessionParseRes)
                return false;

            var sessionDto = await GetSessionByGuidAsync(guid, cancellationToken);

            if (sessionDto is null)
                return false;

            if (!sessionDto.isActive)
                return false;

            var IsAuthorized = sessionDto.SessionDateTo >= DateTime.UtcNow;

            if (!IsAuthorized)
                await UpdateSessionIsActiveStatusAsync(guid, false, cancellationToken);

            return IsAuthorized;
        }

        public async Task<bool> IsForbidden(string session, int needAccsessToId, CancellationToken cancellationToken)
        {
            var guid = Guid.Parse(session);
            var user = await GetByGuidAsync(guid, cancellationToken);
            var isHaveSuperUser = _context.UserRoles
                .Where(x => x.User == user.Id & x.RoleEntity.isSuperUser == true & x.isActive == true);
            if (isHaveSuperUser.Count() == 0)
                if (user.Id != needAccsessToId)
                    return false;
            return true;
        }
    }
}