namespace Shootsy.Core.Interfaces
{
    /// <summary>
    /// Единый интерфейс файлов
    /// </summary>
    public interface IFile
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        string Id { get; set; }

        /// <summary>
        /// Дата создания
        /// </summary>
        DateTime CreateDate { get; set; }

        /// <summary>
        /// Дата редактирования
        /// </summary>
        DateTime EditDate { get; set; }

        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        int IdUser { get; set; }

        /// <summary>
        /// Информация о файле
        /// </summary>
        public FileInfo FileInfo { get; set; }
    }
}