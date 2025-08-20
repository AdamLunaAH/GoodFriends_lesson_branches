using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;

using Microsoft.Extensions.Options;
using Seido.Utilities.SeedGenerator;
using Configuration.Options;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AppWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class SecretController : Controller
    {
        readonly ILogger<SecretController> _logger;
        private readonly DbConnectionSetsOptions _dbSetOptions;
        readonly AesEncryptionOptions _aesOptions;
        readonly JwtOptions _jwtOptions;
        readonly VersionOptions _versionOptions;
        readonly MySecretOptions _mysecretOptions;
        readonly IConfiguration _configuration;

        //GET: api/secret/key
        [HttpGet()]
        [ActionName("Key")]
        [ProducesResponseType(200)]
        public IActionResult Key()
        {
            try
            {
                var keyOptions = new
                {
                    SecretStorage = _configuration["ApplicationSecrets:SecretStorage"],
                    MigrationUser = _configuration["DatabaseConnections:MigrationUser"],
                    DefaultDataUser = _configuration["DatabaseConnections:DefaultDataUser"],
                    UseDataSetWithTag = _configuration["DatabaseConnections:UseDataSetWithTag"],
                };
                return Ok(keyOptions);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //GET: api/secret/version
        [HttpGet()]
        [ActionName("Version")]
        [ProducesResponseType(typeof(VersionOptions), 200)]
        public IActionResult Version()
        {
            try
            {
                return Ok(_versionOptions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving version information");
                return BadRequest(ex.Message);
            }
        }

        //Get: api/secret/mysecret
        [HttpGet()]
        [ActionName("MySecret")]
        [ProducesResponseType(200)]
        public IActionResult mysecret()
        {
            try
            {
                var my = new
                {
                    Color = _configuration
                    ["MySecrets:Color"],
                    Number = _configuration
                    ["MySecrets:Number"],
                    Animal = _configuration
                    ["MySecrets:Animal"],


                };
                return Ok(my);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //Get: api/secret/mysecretoption
        [HttpGet()]
        [ActionName("MySecretOption")]
        [ProducesResponseType(200)]
        public IActionResult mysecretoption()
        {
            try
            {
                return Ok(_mysecretOptions);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public SecretController(ILogger<SecretController> logger,
                    IConfiguration configuration,
                    IOptions<DbConnectionSetsOptions> dbSetOptions,
                    IOptions<AesEncryptionOptions> aesOptions,
                    IOptions<JwtOptions> jwtOptions,
                    IOptions<VersionOptions> versionOptions,
                    IOptions<MySecretOptions> mysecretOptions)
        {
            _logger = logger;

            _dbSetOptions = dbSetOptions.Value;
            _aesOptions = aesOptions.Value;
            _jwtOptions = jwtOptions.Value;
            _versionOptions = versionOptions.Value;
            _configuration = configuration;
            _mysecretOptions = mysecretOptions.Value;
        }
    }
}

