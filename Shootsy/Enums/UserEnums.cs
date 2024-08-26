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
        Photographer = 0,

        /// <summary>
        /// Модель
        /// </summary>
        Model = 1
    }

    /// <summary>
    /// Тип сотрудничества
    /// </summary>
    public enum CooperationTypeEnum
    {
        /// <summary>
        /// Расходы оплачивает модель
        /// </summary>
        ModelPaid = 0,

        /// <summary>
        /// Расходы оплачивает фотограф
        /// </summary>
        PhotographPaid = 1,

        /// <summary>
        /// Расходы оплачиваются пополам
        /// </summary>
        FiftyFifty = 2
    }

    /// <summary>
    /// Пол
    /// </summary>
    public enum GenderEnum
    {
        /// <summary>
        /// Мужской
        /// </summary>
        Male = 0,

        /// <summary>
        /// Женский
        /// </summary>
        Female = 1
    }
}
