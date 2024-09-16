using Shootsy.Core.Interfaces;

namespace Shootsy.Dtos
{
    public class UserSessionDto : IUserSession
    {
        public int Id { get; init; }
        public int User { get; init; }
        public DateTime SessionDateFrom { get; set; }
        public DateTime SessionDateTo { get; set; }
        public Guid Guid { get; init; }
        public bool isActive { get; set; }
    }
}
