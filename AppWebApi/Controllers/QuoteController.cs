using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;

using Seido.Utilities.SeedGenerator;


namespace AppWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class QuotesController : Controller
    {
        readonly ILogger<QuotesController> _logger;
        readonly IWebHostEnvironment _environment;
        readonly SeedGenerator _seeder = new SeedGenerator();

        //GET: api/admin/allquoteslist
        [HttpGet()]
        [ActionName("AllQuotesList")]
        [ProducesResponseType(200)]
        public IActionResult AllQuotesList()
        {
            try
            {
                var quotes = _seeder.AllQuotes;

                return Ok(quotes);
                // var hi = new
                // {
                //     greeting = "Hello, World!"
                // };
                // return Ok(hi);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Something went wrong");
                return BadRequest(ex.Message);
            }
        }


        //GET: api/admin/randomquote
        [HttpGet()]
        [ActionName("RandomQuote")]
        [ProducesResponseType(200)]
        public IActionResult RandomQuote()
        {
            try
            {
                var quotes = _seeder.AllQuotes;
                var rng = new Random();
                var RandomQuoteSelect = quotes[rng.Next(quotes.Count)];

                return Ok(RandomQuoteSelect);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Something went wrong");
                return BadRequest(ex.Message);
            }
        }


        //GET: api/admin/searchquotes
        [HttpGet()]
        [ActionName("SearchQuotes")]
        [ProducesResponseType(200)]
        public IActionResult SearchQuotes()
        {
            try
            {
                var quotes = _seeder.AllQuotes;

                var searchtext = "would";

                var searchlist = quotes.Where(x => x.Quote.Contains(searchtext, StringComparison.OrdinalIgnoreCase));

                return Ok(searchlist);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Something went wrong");
                return BadRequest(ex.Message);
            }
        }



        public QuotesController(ILogger<QuotesController> logger, IWebHostEnvironment environment)
        {
            _logger = logger;
            _environment = environment;
        }
    }



}