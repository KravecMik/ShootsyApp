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
        int Id { get; set; }

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
        int User { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        string FileName { get; set; }

        /// <summary>
        /// Расширение
        /// </summary>
        string Extension { get; set; }

        /// <summary>
        /// Путь до содержимого
        /// </summary>
        string ContentPath { get; set; }
    }
}
