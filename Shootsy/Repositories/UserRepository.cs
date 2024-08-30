using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using Shootsy.Database;
using Shootsy.Database.Entities;
using Shootsy.Dtos;

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

            userEntity.CreateDate = DateTime.UtcNow;
            userEntity.EditDate = DateTime.UtcNow;

            await _context.SaveChangesAsync(cancellationToken);

            userEntityEntry.State = EntityState.Detached;

            return userEntity.Id;
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
            var userEntityEntry = await _context.Users.Where(x => x.Id == id).ExecuteDeleteAsync();
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<UserDto>? GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            var user = _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
            if (user.Result is null)
                return null;
            return _mapper.Map<UserDto>(user.Result);
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
    }
}
