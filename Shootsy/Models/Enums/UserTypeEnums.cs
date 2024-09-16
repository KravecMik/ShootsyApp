using System.ComponentModel;

namespace Shootsy.Enums
{
    public enum UserTypeEnums
    {
        [Description("Фотограф")]
        Photographer = 1,

        [Description("Модель")]
        Model = 2,

        [Description("Визажист")]
        MakeupArtist = 3
    }
}
