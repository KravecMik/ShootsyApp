using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using Shootsy.Database;
using Shootsy.Database.Entities;
using Shootsy.Dtos;

namespace Shootsy.Repositories
{
    public class FileRepository : IFileRepository
    {
        private readonly ApplicationContext _context;
        private readonly IMapper _mapper;

        public FileRepository(ApplicationContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> CreateAsync(FileDto file, CancellationToken cancellationToken)
        {
            var fileEntity = _mapper.Map<FileEntity>(file);
            var fileEntityEntry = await _context.Files.AddAsync(fileEntity, cancellationToken);

            fileEntity.CreateDate = DateTime.UtcNow;

            await _context.SaveChangesAsync(cancellationToken);
            fileEntityEntry.State = EntityState.Detached;

            return fileEntity.Id;
        }

        public async Task<FileDto>? GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            var fileEntity = _context.Files
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
            if (fileEntity.Result is null)
                return null;
            return _mapper.Map<FileDto>(fileEntity.Result);
        }

        public async Task<IReadOnlyList<FileDto>>? GetListAsync(
            int limit,
            int offset,
            int? userId,
            CancellationToken cancellationToken = default)
        {
            var query = _context.Files.AsNoTracking().Select(x => x);
            var userEntities = await query
                .Where(x => x.User == userId)
                .Skip(offset)
                .Take(limit)
                .ToArrayAsync(cancellationToken);
            return _mapper.Map<IReadOnlyList<FileDto>>(userEntities);
        }

        public async Task UpdateAsync(FileDto fileDto, JsonPatchDocument<FileDto> jsonPatchDocument, CancellationToken cancellationToken = default)
        {
            jsonPatchDocument.ApplyTo(fileDto);
            var fileEntity = _mapper.Map<FileEntity>(fileDto);

            fileEntity.EditDate = DateTime.UtcNow;
            _context.Update(fileEntity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteByIdAsync(int id, CancellationToken cancellationToken)
        {
            var fileEntity = await _context.Files.Where(x => x.Id == id).ExecuteDeleteAsync();
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteManyAsync(IEnumerable<int> ids, CancellationToken cancellationToken)
        {
            var fileEntity = await _context.Files
                .Where(x => ids.Any(y => y == x.Id))
                .ExecuteDeleteAsync();
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
