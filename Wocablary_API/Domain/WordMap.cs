using FluentNHibernate.Mapping;

namespace WocabWeb.API.Domain
{
    public class WordMap : ClassMap<Word>
    {
        public WordMap()
        {
            Table("Words");
            Id(x => x.Id); // Мапінг ID

            Map(x => x.WordText);
            Map(x => x.WordTranslation);
            Map(x => x.WordDescription);
            Map(x => x.WordStory);
            Map(x => x.WordType);
            Map(x => x.WordImageURL);

            // Мапінг зв'язку "багато-до-багатьох" до Tag
            HasManyToMany(x => x.WordTegs)
                .Table("WordTag")           // Вказуємо назву проміжної таблиці
                .ParentKeyColumn("WordId")  // Стовпець у проміжній таблиці, що вказує на Word
                .ChildKeyColumn("TagId")    // Стовпець у проміжній таблиці, що вказує на Tag
                .Cascade.All()              // Визначаємо поведінку каскадного збереження/видалення
                .Inverse();                 // Визначаємо, яка сторона (Word чи Tag) керує зв'язком (зазвичай керує Tag)
        }
    }
}
