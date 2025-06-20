using NHibernate.Linq;
using WocabWeb.API.Domain;
using ISession = NHibernate.ISession; // Посилання на EntityBase


namespace WocabWeb.API.DAO
{

    //--------------------------------------------------------------------------------------------------------
    public class WordDAO : GenericDAO<Word>, IWordDAO
    {
        public WordDAO(ISession session) : base(session) { }



    }


    //--------------------------------------------------------------------------------------------------------
    public class TagDAO : GenericDAO<Tag>, ITagDAO
    {
        public TagDAO(ISession session) : base(session) { }

        public async Task<Tag> GetByTagNameAsync(string tagName)
        {
            return await _session.Query<Tag>()
                .Where(t => t.TagName.ToLower() == tagName.ToLower())
                .SingleOrDefaultAsync();
        }
    }
}
