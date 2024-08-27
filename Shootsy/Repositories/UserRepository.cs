using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Shootsy.Core.Interfaces;
using Shootsy.Database;
using Shootsy.Database.Entities;
using Shootsy.Dtos;
using System.Linq.Expressions;

namespace Shootsy.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationContext _context;
        private readonly IMapper _mapper;
        private readonly TimeProvider _timeProvider;

        public UserRepository(ApplicationContext context, IMapper mapper, TimeProvider timeProvider)
        {
            _context = context;
            _mapper = mapper;
            _timeProvider = timeProvider;
        }

        public async Task<int> CreateAsync(UserDto user, CancellationToken cancellationToken = default)
        {
            var userEntity = _mapper.Map<UserEntity>(user);

            var userEntityEntry = await _context.Users.AddAsync(userEntity, cancellationToken);

            UpdateDateTimeProperty(nameof(userEntity.CreateDate), userEntityEntry);
            UpdateDateTimeProperty(nameof(userEntity.EditDate), userEntityEntry);

            await _context.SaveChangesAsync(cancellationToken);

            userEntityEntry.State = EntityState.Detached;

            return userEntity.Id;
        }

        public async Task UpdateAsync(UserDto user, IEnumerable<string> updateProperties, CancellationToken cancellationToken = default)
        {
            using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
            var currUserEntity = await _context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == user.Id, cancellationToken);

            if (currUserEntity is null)
            {
                return;
            }

            var userEntity = _mapper.Map<UserEntity>(user);

            var userEntityEntry = _context.Users.Attach(userEntity);

            foreach (var property in updateProperties)
            {
                UpdateDateTimeProperty(property, userEntityEntry);
                userEntityEntry.Property(property).IsModified = true;
            }

            UpdateDateTimeProperty(nameof(userEntity.EditDate), userEntityEntry);

            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<UserDto> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            var user = _context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
            return _mapper.Map<UserDto>(user);
        }

        public async Task<IReadOnlyList<UserDto>> GetListAsync(
            int limit,
            int offset,
            CancellationToken cancellationToken = default)
        {
            var query = _context.Users.AsNoTracking().Select(x => x);
            var users = await query
                .Skip(offset)
                .Take(limit)
                .ToArrayAsync(cancellationToken);
            return _mapper.Map<IReadOnlyList<UserDto>>(users);
        }

        private void UpdateDateTimeProperty(string propertyName, EntityEntry<UserEntity> entityEntry)
        {
            switch (propertyName)
            {
                case nameof(UserEntity.CreateDate):
                    SetValueToProperty(entityEntry, x => x.CreateDate, _timeProvider.GetLocalNow().DateTime);
                    break;

                case nameof(UserEntity.EditDate):
                    SetValueToProperty(entityEntry, x => x.EditDate, _timeProvider.GetLocalNow().DateTime);
                    break;
                default:
                    break;
            }
        }

        private void SetValueToProperty(EntityEntry<UserEntity> entityEntry, Expression<Func<UserEntity, DateTime?>> property, DateTime? dateTime)
        {
            entityEntry.Property(property).CurrentValue = dateTime;
            entityEntry.Property(property).IsModified = true;
        }
    }
}
