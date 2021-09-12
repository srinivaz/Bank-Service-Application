using AutoFixture;
using AutoMapper;
using BankService.Controllers;
using BankService.Model;
using BankService.ResourceModel;
using BankService.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace BankService.Test
{
    public class CustomerControllerTest
    {
        private readonly Mock<ICustomerService> _service;
        private readonly Mock<ILogger<CustomerController>> _logger;
        private readonly Mock<IOptions<AppSettings>> _appSettings;

        public CustomerControllerTest()

        {
            _service = new Mock<ICustomerService>();
            _logger = new Mock<ILogger<CustomerController>>();
            _appSettings = new Mock<IOptions<AppSettings>>();
        }

        [Fact]
        public void GetAllCustomers_ReturnsExpectedResult()
        {
            //arrange
            var employees = GetSampleCustomer();

            _service.Setup(x => x.GetAllCustomers()).Returns(employees);

            var controller = new CustomerController(_service.Object, _logger.Object, AutomapperSingleton.Mapper, _appSettings.Object);

            //act
            var actionResult = controller.GetAllCustomers();
            var result = actionResult as OkObjectResult;
            var actual = result.Value as IEnumerable<Customer>;


            //assert
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal(employees.Count(), actual.Count());
        }

        [Fact]
        public void GetCustomerById_ReturnsExpectedResult()
        {
            //arrange
            var employee = GetSampleCustomer().First();

            _service.Setup(x => x.GetById(It.IsAny<Guid>())).Returns(employee);

            var controller = new CustomerController(_service.Object, _logger.Object, AutomapperSingleton.Mapper, _appSettings.Object);

            //act
            var actionResult = controller.GetCustomerById(employee.id);
            var result = actionResult as OkObjectResult;
            var actual = result.Value as Customer;


            //assert
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal(employee.id, actual.id);
        }

        [Fact]
        public void GetVersion1_ReturnsExpectedResult()
        {
            //arrange
            var controller = new CustomerController(_service.Object, _logger.Object, AutomapperSingleton.Mapper, _appSettings.Object);

            //act
            var result = controller.GetVersion1();
                       
            //assert
            Assert.Equal("Version 1.0" , result.First());
        }

        [Fact]
        public void GetVersion2_ReturnsExpectedResult()
        {
            //arrange
            var controller = new CustomerController(_service.Object, _logger.Object, AutomapperSingleton.Mapper, _appSettings.Object);

            //act
            var result = controller.GetVersion2();

            //assert
            Assert.Equal("Version 2.0", result.First());
        }

        [Fact]
        public void Authenticate_WithValidCredential_ReturnsValidTokens()
        {
            //arrange
            var employee = GetSampleCustomer().First();

            _service.Setup(x => x.Authenticate(It.IsAny<string>(), It.IsAny<string>())).Returns(employee);

            var appSettings = new AppSettings() { Secret = "This is my Secret key for Bank services" };
            _appSettings.Setup(x => x.Value).Returns(appSettings);

            var controller = new CustomerController(_service.Object, _logger.Object, AutomapperSingleton.Mapper, _appSettings.Object);
            var inputModel = new AuthenticateModel() { Username = employee.Username, Password = employee.Password };

            //act
            var actionResult = controller.Authenticate(inputModel);
            var result = actionResult as OkObjectResult;
            var actual = result.Value as dynamic;

            //assert
            Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(actual);
        }

        [Fact]
        public void Authenticate_WithInValidCredential_ReturnsBadRequest()
        {
            //arrange
            _service.Setup(x => x.Authenticate(It.IsAny<string>(), It.IsAny<string>())).Returns<Customer>(null);

            var appSettings = new AppSettings() { Secret = "This is my Secret key for Bank services" };
            _appSettings.Setup(x => x.Value).Returns(appSettings);

            var controller = new CustomerController(_service.Object, _logger.Object, AutomapperSingleton.Mapper, _appSettings.Object);
            var inputModel = new AuthenticateModel();

            //act
            var actionResult = controller.Authenticate(inputModel);
            var result = actionResult as BadRequestObjectResult;

            //assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void RegisterCustomer_WithValid_SavesSuccessfully()
        {
            //arrange
            var employee = GetSampleCustomer().First();

            _service.Setup(x => x.AddCustomer(It.IsAny<Customer>())).ReturnsAsync(employee);

            var controller = new CustomerController(_service.Object, _logger.Object, AutomapperSingleton.Mapper, _appSettings.Object);
            var inputModel = new CustomerModel()
            {
                AccountType = 1,
                Address = "test Address ",
                ContactNo = 123456789,
                Country = "Test Country",
                DOB = DateTime.Today,
                EmailAddress = "test@cts.com",
                Name = "Test name",
                PAN = "Test PAN",
                Password = "pass@word1",
                State = "Test State",
                Username = "TestName"
            };

            //act
            var actionResult = controller.RegisterCustomer(inputModel);
            var result = actionResult as OkResult;

            //assert
            Assert.IsType<OkResult>(result);
            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public void RegisterCustomer_WhenUnExpectedError_ThrowsBadRequestException()
        {
            //arrange
            var employee = GetSampleCustomer().First();

            _service.Setup(x => x.AddCustomer(It.IsAny<Customer>())).ReturnsAsync(employee);

            var controller = new CustomerController(_service.Object, _logger.Object, null, _appSettings.Object);
            var inputModel = new CustomerModel()
            {
                AccountType = 1,
                Address = "test Address ",
                ContactNo = 123456789,
                Country = "Test Country",
                DOB = DateTime.Today,
                EmailAddress = "test@cts.com",
                Name = "Test name",
                PAN = "Test PAN",
                Password = "pass@word1",
                State = "Test State",
                Username = "TestName"
            };

            //act
            var actionResult = controller.RegisterCustomer(inputModel);
            var result = actionResult as BadRequestObjectResult;

            //assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void UpdateCustomer_WithValid_SavesSuccessfully()
        {
            //arrange
            var employee = GetSampleCustomer().First();

            _service.Setup(x => x.UpdateCustomer(It.IsAny<Customer>())).Returns(true);

            var controller = new CustomerController(_service.Object, _logger.Object, AutomapperSingleton.Mapper, _appSettings.Object);
            var inputModel = new CustomerModel()
            {
                AccountType = 1,
                Address = "test Address ",
                ContactNo = 123456789,
                Country = "Test Country",
                DOB = DateTime.Today,
                EmailAddress = "test@cts.com",
                Name = "Test name",
                PAN = "Test PAN",
                Password = "pass@word1",
                State = "Test State",
                Username = "TestName"
            };

            //act
            var actionResult = controller.UpdateCustomer(employee.id, inputModel);
            var result = actionResult as OkResult;

            //assert
            Assert.IsType<OkResult>(result);
            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public void UpdateCustomer_WhenUnExpectedError_ThrowsBadRequestException()
        {
            //arrange
            var employee = GetSampleCustomer().First();

            _service.Setup(x => x.UpdateCustomer(It.IsAny<Customer>())).Returns(true);

            var controller = new CustomerController(_service.Object, _logger.Object, null, _appSettings.Object);
            var inputModel = new CustomerModel()
            {
                AccountType = 1,
                Address = "test Address ",
                ContactNo = 123456789,
                Country = "Test Country",
                DOB = DateTime.Today,
                EmailAddress = "test@cts.com",
                Name = "Test name",
                PAN = "Test PAN",
                Password = "pass@word1",
                State = "Test State",
                Username = "TestName"
            };

            //act
            var actionResult = controller.UpdateCustomer(employee.id, inputModel);
            var result = actionResult as BadRequestObjectResult;

            //assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void AddLoadDetail_WithValid_SavesSuccessfully()
        {
            //arrange
            var employee = GetSampleCustomer().First();

            _service.Setup(x => x.GetById(It.IsAny<Guid>())).Returns(employee);
            _service.Setup(x => x.UpdateCustomer(It.IsAny<Customer>())).Returns(true);

            var controller = new CustomerController(_service.Object, _logger.Object, AutomapperSingleton.Mapper, _appSettings.Object);
            var inputModel = new LoanModel()
            {
                Date = DateTime.Today,
                Duration = 48,
                LoanAmount = 1000000,
                LoanType = 1,
                RateOfInterest = 1
            };

            //act
            var actionResult = controller.AddLoadDetail(employee.id, inputModel);
            var result = actionResult as OkResult;

            //assert
            Assert.IsType<OkResult>(result);
            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public void AddLoadDetail_WhenUnExpectedError_ThrowsBadRequestException()
        {
            //arrange
            var employee = GetSampleCustomer().First();

            _service.Setup(x => x.GetById(It.IsAny<Guid>())).Returns(employee);
            _service.Setup(x => x.UpdateCustomer(It.IsAny<Customer>())).Returns(true);

            var controller = new CustomerController(_service.Object, _logger.Object, null, _appSettings.Object);
            var inputModel = new LoanModel()
            {
                Date = DateTime.Today,
                Duration = 48,
                LoanAmount = 1000000,
                LoanType = 1,
                RateOfInterest = 1
            };

            //act
            var actionResult = controller.AddLoadDetail(employee.id, inputModel);
            var result = actionResult as BadRequestObjectResult;

            //assert
            Assert.IsType<BadRequestObjectResult>(result);
        }


        private IEnumerable<Customer> GetSampleCustomer()
        {
            return new List<Customer>
            {
                new Customer()
                {
                    id = Guid.NewGuid(),
                    AccountType = 1,
                    Address = "test Address ",
                    ContactNo = 123456789,
                    Country ="Test Country",
                    DOB = DateTime.Today,
                    EmailAddress = "test@cts.com",
                    Name = "Test name",
                    PAN = "Test PAN",
                    Password ="pass@word1",
                    State = "Test State",
                    Username = "TestName"
                }
            };
        }

        public class AutomapperSingleton
        {
            private static IMapper _mapper;
            public static IMapper Mapper
            {
                get
                {
                    if (_mapper == null)
                    {
                        // Auto Mapper Configurations
                        var mappingConfig = new MapperConfiguration(mc =>
                        {
                            mc.AddProfile(new AutoMapperProfile());
                        });
                        IMapper mapper = mappingConfig.CreateMapper();
                        _mapper = mapper;
                    }
                    return _mapper;
                }
            }
        }
    }
}
