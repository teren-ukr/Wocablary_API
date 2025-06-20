using WocabWeb.API.Domain;

namespace WocabWeb.API.DAO
{
    public interface IWordDAO : IGenericDAO<Word>
    {
        //Специфічні методи для сутності Word


    }

    public interface ITagDAO : IGenericDAO<Tag>
    {
        //Специфічні методи для сутності Tag
        Task<Tag> GetByTagNameAsync(string tagName);

    }
}
