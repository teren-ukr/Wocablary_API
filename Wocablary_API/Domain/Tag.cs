namespace WocabWeb.API.Domain
{
    public class Tag : EntityBase 
    {
        public virtual required string TagName { get; set; } // Наприклад, "їжа", "злічуване"   

        // Зворотний зв'язок: які слова мають цей тег
        public virtual IList<Word> Words { get; set; }

        public Tag()
        {
            Words = new List<Word>();
        }
    }
}