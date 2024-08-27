namespace Shootsy.Models
{
    public class CreateUserModel
    {
        public string Login { get; set; }

        public string Password { get; set; }

        public string Contact { get; set; }

        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public string? Patronymic { get; set; }

        public string Discription { get; set; }

        public int CooperationTypeId { get; set; }

        public int RoleId { get; set; }

        public bool? isNude { get; set; } = false;

        public int GenderId { get; set; }

        public int CityId { get; set; }

        public string? Fullname { get; set; }

        public int TypeId { get; set; }

        public bool? isDelete { get; set; } = false;

        public bool? isHasActiveSubscribe { get; set; } = false;

        public DateTime? CreateDate { get; set; }

        public DateTime? EditDate { get; set; }
    }
}
