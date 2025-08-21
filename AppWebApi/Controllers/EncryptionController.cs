using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;

using Configuration;
using Configuration.Options;

using Microsoft.Extensions.Options;
using AppWebApi.Models;

using Seido.Utilities.SeedGenerator;
using System.ComponentModel.Design;
using Microsoft.VisualBasic;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AppWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class EncryptionController : Controller
    {
        // private readonly Encryptions _encryptions = null;
        readonly DatabaseConnections _dbConnections = null;
        readonly ILogger<EncryptionController> _logger;
        readonly IConfiguration _configuration;
        readonly MySettingsOptions _mySettingsOptions;

        private readonly Encryptions _encryptMsg;

        //GET: api/encryption/message
        [HttpGet()]
        [ActionName("Message")]
        [ProducesResponseType(200)]
        public IActionResult Message()
        {
            try
            {

                return Ok(_mySettingsOptions);


            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //GET: api/encryption/encryptedmessage
        [HttpGet()]
        [ActionName("EncryptedMessage")]
        [ProducesResponseType(200)]
        public IActionResult EncryptedMessage()
        {
            try
            {

                _logger.LogInformation($"{nameof(EncryptedMessage)}");
                var msg2 = _encryptMsg.AesEncryptToBase64<MySettingsOptions>(_mySettingsOptions);

                return Ok(msg2);


            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //GET: api/encryption/decryptedmessage
        [HttpGet()]
        [ActionName("DecryptedMessage")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400, Type = typeof(string))]
        public IActionResult DecryptedMessage(string encryptedMessage)
        {
            try
            {

                _logger.LogInformation($"{nameof(DecryptedMessage)}");
                var msg = _encryptMsg.AesDecryptFromBase64<MySettingsOptions>(encryptedMessage);

                return Ok(msg);


            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //GET: api/encryption/encryptpassword
        [HttpGet()]
        [ActionName("EncryptPassword")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400, Type = typeof(string))]
        public IActionResult EncryptPassword(string password)
        {
            try
            {

                _logger.LogInformation($"{nameof(EncryptPassword)}");
                // var msg = _encryptMsg.EncryptPasswordToBase64(password);

                if (string.IsNullOrWhiteSpace(password))
                {
                    throw new ArgumentException("Password cannot be null or empty.");
                }
                else
                {
                    var msg = _encryptMsg.EncryptPasswordToBase64(password);
                    return Ok(msg);
                }

                // return Ok(msg);


            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }





        public EncryptionController(Encryptions encryptMsg, DatabaseConnections dbConnections, ILogger<EncryptionController> logger,
                    IConfiguration configuration,
                    IOptions<MySettingsOptions> mySettingsOptions)
        {
            _encryptMsg = encryptMsg;
            _dbConnections = dbConnections;
            _logger = logger;
            _configuration = configuration;
            _mySettingsOptions = mySettingsOptions.Value;
        }
    }
}

