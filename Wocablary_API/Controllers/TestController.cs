using Microsoft.AspNetCore.Mvc;
using Wocablary_API.Domain.DTOs;
using WocabWeb.API.DAO;
using WocabWeb.API.Domain;
using WocabWeb.API.Models.DTOs;

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


    //--------------------------------------------------------------------------------------------------------
    [HttpPost("AddWord")]

    public async Task<IActionResult> AddWord([FromBody] AddWordRequest request)
    {// [FromBody] вказує ASP.NET Core, що дані потрібно брати з тіла запиту (зазвичай JSON).

        // Автоматична валідація моделі (завдяки [ApiController] атрибуту)
        // Якщо валідація провалиться, [ApiController] автоматично поверне 400 Bad Request
        // з деталями помилок ModelState.
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var word = new Word
            {
                WordText = request.WordText,
                WordTranslation = request.WordTranslation,
                WordDescription = request.WordDescription,
                WordStory = request.WordStory,
                WordType = request.WordType,
                WordImageURL = request.WordImageURL
            };

            await _wordDao.AddAsync(word);

            // Після успішного додавання повертаємо 201 Created
            // та URL-адресу, за якою можна отримати щойно створений ресурс.
            // Припускаємо, що GetWordById - це GET-метод для отримання слова за Id.
            //return CreatedAtAction(nameof(GetWordById), new { id = word.Id }, word);

            return Ok("Word inserted");
        }
        catch (Exception e)
        {
            return StatusCode(500, $"Internal Server Error: {e.Message}");
        }

    }


    //--------------------------------------------------------------------------------------------------------
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteWord(long id)
    {
        if (id <= 0)
        {
            return BadRequest("Invalid word ID provided.");
        }


        try
        {
            //створити метод для пошуку по id
            var wordToDelete = await _wordDao.GetByIdAsync(id);
            
            if (wordToDelete == null)
            {
                return NotFound($"Word with id {id} not found");
            }

            await _wordDao.DeleteAsync(id);
            return NoContent();
            
        }
        catch (Exception e)
        {
            return StatusCode(500, $"Internal Server Error: Failed to delete word with ID {id}. Details: {e.Message}");
        }

    }

    //--------------------------------------------------------------------------------------------------------
    [HttpPut]
    public async Task<IActionResult> UpdateWord(long id, [FromBody] UpdateWordRequest request)
    {

        // Автоматична валідація моделі (завдяки [ApiController] атрибуту)
        // Якщо валідація провалиться, [ApiController] автоматично поверне 400 Bad Request
        // з деталями помилок ModelState.
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var existingWord = await _wordDao.GetByIdAsync(id);

            existingWord.WordText = request.WordText;
            existingWord.WordTranslation = request.WordTranslation;
            existingWord.WordDescription = request.WordDescription;
            existingWord.WordStory = request.WordStory;
            existingWord.WordType = request.WordType;
            existingWord.WordImageURL = request.WordImageURL;

            await _wordDao.UpdateAsync(existingWord);

            // Після успішного додавання повертаємо 201 Created
            // та URL-адресу, за якою можна отримати щойно створений ресурс.
            // Припускаємо, що GetWordById - це GET-метод для отримання слова за Id.
            //return CreatedAtAction(nameof(GetWordById), new { id = word.Id }, word);

            return Ok("Word updated");
        }
        catch (Exception e)
        {
            return StatusCode(500, $"Internal Server Error: {e.Message}");
        }


    }


    //--------------------------------------------------------------------------------------------------------
    [HttpGet("{id}")] // Це дозволить звертатися до нього як api/Test/{id}
    public async Task<IActionResult> GetWordById(long id)
    {
        if (id <= 0)
        {
            return BadRequest("Invalid word ID provided.");
        }

        try
        {
            var word = await _wordDao.GetByIdAsync(id);

            if (word == null)
            {
                return NotFound($"Word with id {id} not found.");
            }

            return Ok(word);
        }
        catch (Exception e)
        {
            return StatusCode(500, $"Internal Server Error: Failed to retrieve word with ID {id}. Details: {e.Message}");
        }
    }



}