using FluentNHibernate.Mapping;

namespace WocabWeb.API.Domain
{
    public class TagMap : ClassMap<Tag>
    {
        public TagMap()
        {
            Table("Tags");
            Id(x => x.Id); // Мапінг ID
            Map(x => x.TagName); // Мапінг назви тега

            // Мапінг зворотного зв'язку "багато-до-багатьох" до Word
            HasManyToMany(x => x.Words)
                .Table("WordTag") // Вказуємо ту саму проміжну таблицю
                .ParentKeyColumn("TagId") // Стовпець у проміжній таблиці, що вказує на Tag
                .ChildKeyColumn("WordId") // Стовпець у проміжній таблиці, що вказує на Word
                .Cascade.All(); // Каскадна поведінка
                                // Тут НЕ ставимо .Inverse(), оскільки Word вже має Inverse().
                                // Тільки одна сторона зв'язку "багато-до-багатьох" може бути інверсною.
        }
    }
}
