namespace Shootsy.Enums
{
    /// <summary>
    /// Тип учетной записи
    /// </summary>
    public enum TypeEnum
    {
        /// <summary>
        /// Фотограф
        /// </summary>
        Photographer = 1,

        /// <summary>
        /// Модель
        /// </summary>
        Model = 2
    }

    /// <summary>
    /// Тип сотрудничества
    /// </summary>
    public enum CooperationTypeEnum
    {
        /// <summary>
        /// Расходы оплачивает модель
        /// </summary>
        ModelPaid = 1,

        /// <summary>
        /// Расходы оплачивает фотограф
        /// </summary>
        PhotographPaid = 2,

        /// <summary>
        /// Расходы оплачиваются поровну
        /// </summary>
        FiftyFifty = 3
    }

    /// <summary>
    /// Пол
    /// </summary>
    public enum GenderEnum
    {
        /// <summary>
        /// Мужской
        /// </summary>
        Male = 1,

        /// <summary>
        /// Женский
        /// </summary>
        Female = 2
    }
}
