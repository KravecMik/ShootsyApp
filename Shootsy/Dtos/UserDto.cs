using Shootsy.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shootsy.Dtos
{
    [Table("users")]
    public class UserDto
    {
        [Column("user_id")]
        public int Id { get; init; }

        [Column("usermame")]
        public string Usermame { get; init; }

        [Column("contact")]
        public string Contact { get; init; }

        [Column("first_name")]
        public string Firstname { get; init; }

        [Column("last_name")]
        public string Lastname { get; init; }

        [Column("patronymic")]
        public string? Patronymic { get; init; }

        [Column("full_name")]
        public string? Fullname { get; init; }

        [Column("discription")]
        public string? Discription { get; init; }

        [Column("cooperation_type")]
        public int? CooperationType { get; init; }

        [Column("type")]
        public int? Type { get; init; }

        [Column("is_nude")]
        public bool? isNude { get; init; }

        [Column("city")]
        public int? City { get; init; }

        [Column("is_has_active_subscribe")]
        public bool? isHasActiveSubscribe { get; init; }

        [Column("gender")]
        public int? Gender { get; init; }

        [Column("create_date")]
        public DateTime? CreateDate { get; init; }

        [Column("edit_date")]
        public DateTime? EditDate { get; init; }
    }
}
