using System.ComponentModel.DataAnnotations;

namespace Wocablary_API.Domain.DTOs
{
    public class UpdateWordRequest
    {
        [Required] // ID обов'язкове для оновлення
        public long Id { get; set; } // Додаємо ID

        [Required(ErrorMessage = "Word text is required.")]
        [StringLength(100, ErrorMessage = "Word text cannot exceed 100 characters.")]
        public string WordText { get; set; }

        public string? WordTranslation { get; set; }
        public string? WordDescription { get; set; }
        public string? WordStory { get; set; }
        public string? WordType { get; set; }
        public string? WordImageURL { get; set; }
        // Примітки: Якщо ви хочете дозволити оновлення тегів, це вимагатиме додаткової логіки.
        // Це DTO має відображати лише ті поля, які ви дозволяєте оновлювати через API.
    }

}
