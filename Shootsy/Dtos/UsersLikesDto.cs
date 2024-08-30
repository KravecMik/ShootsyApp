using Shootsy.Enums;

namespace Shootsy.Dtos
{
    public class UserLikeDto
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public UserTypeEnums UserType { get; set; }

        public int LikedUserId { get; set; }
    }
}
