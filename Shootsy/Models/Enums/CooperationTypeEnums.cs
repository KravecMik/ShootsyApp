using System.ComponentModel;

namespace Shootsy.Enums
{
    public enum CooperationTypeEnums
    {
        [Description("Не указано")]
        None = 1,

        [Description("Расходы оплачивает модель")]
        ModelPaid = 2,

        [Description("Расходы оплачивает фотограф")]
        PhotographPaid = 3,

        [Description("Расходы оплачиваются поровну")]
        FiftyFifty = 4
    }
}
