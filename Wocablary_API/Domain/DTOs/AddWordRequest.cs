using System.ComponentModel.DataAnnotations; // Для атрибутів валідації

namespace WocabWeb.API.Models.DTOs
{
    public class AddWordRequest
    {
        [Required(ErrorMessage = "Word text is required.")] // Зробимо це поле обов'язковим
        [StringLength(100, ErrorMessage = "Word text cannot exceed 100 characters.")]
        public string WordText { get; set; }

        public string? WordTranslation { get; set; } // Може бути null
        public string? WordDescription { get; set; }
        public string? WordStory { get; set; }
        public string? WordType { get; set; }
        public string? WordImageURL { get; set; }
        // Не включаємо Id, оскільки він генерується базою даних
        // Не включаємо WordTags тут, це буде складніша логіка, якщо потрібно додавати теги при створенні слова
    }
}