using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Shootsy.Database;
using Shootsy.Database.Entities;
using Shootsy.Dtos;

namespace Shootsy.Repositories
{
    public class UserSessionRepository : IUserSessionRepository
    {
        private readonly ApplicationContext _context;
        private readonly IMapper _mapper;
        InternalConstants _internalConstants;

        public UserSessionRepository(ApplicationContext context, IMapper mapper, InternalConstants internalConstants)
        {
            _context = context;
            _mapper = mapper;
            _internalConstants = internalConstants;
        }

        public async Task<Guid> CreateAsync(int userId, CancellationToken cancellationToken)
        {
            var sessionEntity = new UserSessionEntity
            {
                User = userId,
                Guid = Guid.NewGuid()
            };
            var sessionEntityEntry = await _context.UserSessions.AddAsync(sessionEntity, cancellationToken);

            sessionEntity.SessionDateFrom = DateTime.UtcNow;
            sessionEntity.SessionDateTo = DateTime.UtcNow.AddDays(_internalConstants.SessionActivityTime);

            await _context.SaveChangesAsync(cancellationToken);
            sessionEntityEntry.State = EntityState.Detached;

            return sessionEntity.Guid;
        }

        public async Task<UserSessionDto>? GetByGuidAsync(Guid guid, CancellationToken cancellationToken)
        {
            var sessionEntity = await _context.UserSessions
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Guid == guid, cancellationToken);
            if (sessionEntity is null)
                return null;
            return _mapper.Map<UserSessionDto>(sessionEntity);
        }

        public async Task<bool> isSessionActive(Guid guid, CancellationToken cancellationToken)
        {
            var sessionDto = await GetByGuidAsync(guid, cancellationToken);
            if (sessionDto is null)
                return false;
            var sessionVerification = sessionDto.SessionDateTo >= DateTime.UtcNow;
            return sessionVerification;
        }
    }
}
