using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BankService.Model;
using BankService.ResourceModel;
using BankService.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace BankService.Controllers
{
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    [Route("Customers")]
    [Authorize]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        private readonly ILogger<CustomerController> _logger;
        private readonly AppSettings _appSettings;
        private readonly IMapper _mapper;
        public CustomerController(ICustomerService customerService, ILogger<CustomerController> logger, IMapper mapper, IOptions<AppSettings> appSettings)
        {
            _customerService = customerService;
            _logger = logger;
            _appSettings = appSettings.Value;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] AuthenticateModel model)
        {
            var user = _customerService.Authenticate(model.Username, model.Password);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.id.ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            // return basic user info and authentication token
            return Ok(new
            {
                Id = user.id,
                Username = user.Username,
                Token = tokenString
            });
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult RegisterCustomer([FromBody] CustomerModel customerModel)
        {
            try
            {
                var customer = _mapper.Map<Customer>(customerModel);
                _customerService.AddCustomer(customer);
                
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(new { message = "Error occured in Add Customer. Please validate the input." });
            }
        }

        [HttpGet]
        public IActionResult GetAllCustomers()
        {
            IEnumerable<Customer> customers = _customerService.GetAllCustomers();
            return Ok(customers);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateCustomer([FromRoute]Guid id, [FromBody] CustomerModel customerModel)
        {
            try
            {
                var customer = _mapper.Map<Customer>(customerModel);
                
                customer.id = id;
                _customerService.UpdateCustomer(customer);
                
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}/LoanDetail")]
        public IActionResult AddLoadDetail([FromRoute] Guid id, [FromBody] LoanModel loanModel)
        {
            try
            {
                var loan = _mapper.Map<Loan>(loanModel);
                var customer = _customerService.GetById(id);
                
                customer.Loan.Add(loan);
                
                _customerService.UpdateCustomer(customer);
                
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetCustomerById(Guid id)
        {
            var customer = _customerService.GetById(id);

            return Ok(customer);
        }

        [AllowAnonymous]
        [HttpGet("GetVersion")]
        [MapToApiVersion("1.0")]
        public IEnumerable<string> GetVersion1()
        {
            return new string[] { "Version 1.0" };
        }

        [AllowAnonymous]
        [HttpGet("GetVersion")]
        [MapToApiVersion("2.0")]
        public IEnumerable<string> GetVersion2()
        {
            return new string[] { "Version 2.0" };
        }
    }
}