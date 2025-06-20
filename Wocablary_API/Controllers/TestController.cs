using Microsoft.AspNetCore.Mvc;
using WocabWeb.API.DAO;
using WocabWeb.API.Domain;

[ApiController]
[Route("api/[controller]")]
public class TestController : ControllerBase
{
    private readonly IWordDAO _wordDao;

    public TestController(IWordDAO wordDao)
    {
        _wordDao = wordDao;
    }

    //--------------------------------------------------------------------------------------------------------
    [HttpPost("save")]
    public async Task<IActionResult> SaveTestWord()
    {
        try
        {
            var word = new Word { WordText = "adolf" };
            await _wordDao.AddAsync(word);
            return Ok("Word inserted");
        }
        catch (Exception e)
        {
            return StatusCode(500, "Internal Error when adding word");
        }

    }

    //--------------------------------------------------------------------------------------------------------
    [HttpGet("getAll")]
    public async  Task<IActionResult> GetAllWords()
    {
        try
        {
            IEnumerable<Word> words = await _wordDao.GetAllAsync();
            return Ok(words);
        }
        catch (Exception e)
        {
            return StatusCode(500, "Internal Error when adding word");
        }
    }


}