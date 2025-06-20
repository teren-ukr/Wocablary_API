namespace WocabWeb.API.Domain
{
    public class Word :EntityBase
    {
        public virtual required string WordText { get; set; }
        public virtual string? WordTranslation { get; set; }
        public virtual string? WordDescription { get; set; }
        public virtual string? WordStory { get; set; }
        public virtual string? WordType { get; set; }
        public virtual string? WordImageURL { get; set; }


        
        public virtual IList<Tag> WordTegs { get; set; } 

        public Word()
        {
            WordTegs = new List<Tag>(); // Ініціалізуємо колекцію
        }

    }
}
