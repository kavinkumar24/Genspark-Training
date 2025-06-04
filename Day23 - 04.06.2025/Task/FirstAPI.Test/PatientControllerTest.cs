using System.Threading.Tasks;
using FirstAPI.Controllers;
using FirstAPI.Interfaces;
using FirstAPI.Models;
using FirstAPI.Models.DTOs.DoctorSpecialities;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace FirstAPI.Test
{
    [TestFixture]
    public class PatientControllerTest
    {
        private Mock<IPatientService> _patientServiceMock;
        private PatientController _controller;

        [SetUp]
        public void Setup()
        {
            _patientServiceMock = new Mock<IPatientService>();
            _controller = new PatientController(_patientServiceMock.Object);
        }

        [Test]
        public async Task GetPatientById_ReturnsOk_WhenPatientExists()
        {
            var patient = new Patient { Id = 1, Name = "Test" };
            _patientServiceMock.Setup(s => s.GetPatientById(1)).ReturnsAsync(patient);

            var result = await _controller.GetPatientById(1);

            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.AreEqual(patient, okResult.Value);
        }

        [Test]
        public async Task GetPatientById_ReturnsBadRequest_WhenIdInvalid()
        {
            var result = await _controller.GetPatientById(0);

            Assert.IsInstanceOf<BadRequestObjectResult>(result.Result);
        }

        [Test]
        public async Task GetPatientById_ReturnsNotFound_WhenPatientNotFound()
        {
            _patientServiceMock.Setup(s => s.GetPatientById(2)).ReturnsAsync((Patient)null);

            var result = await _controller.GetPatientById(2);

            Assert.IsInstanceOf<NotFoundObjectResult>(result.Result);
        }

        [Test]
        public async Task PostPatient_ReturnsCreated_WhenSuccessful()
        {
            var dto = new PatientAddRequestDto { Name = "Test", Email = "test@gmail.com" };
            var patient = new Patient { Id = 1, Name = "Test", Email = "test@gmail.com" };
            _patientServiceMock.Setup(s => s.AddPatient(dto)).ReturnsAsync(patient);

            var result = await _controller.PostPatient(dto);

            Assert.IsInstanceOf<CreatedResult>(result.Result);
            var createdResult = result.Result as CreatedResult;
            Assert.AreEqual(patient, createdResult.Value);
        }

        [Test]
        public async Task PostPatient_ReturnsBadRequest_WhenServiceReturnsNull()
        {
            var dto = new PatientAddRequestDto { Name = "Test", Email = "test@gmail.com" };
            _patientServiceMock.Setup(s => s.AddPatient(dto)).ReturnsAsync((Patient)null);

            var result = await _controller.PostPatient(dto);

            Assert.IsInstanceOf<BadRequestObjectResult>(result.Result);
        }

        [Test]
        public async Task PostPatient_ReturnsBadRequest_WhenExceptionThrown()
        {
            var dto = new PatientAddRequestDto { Name = "Test", Email = "test@gmail.com" };
            _patientServiceMock.Setup(s => s.AddPatient(dto)).ThrowsAsync(new System.Exception("error"));

            var result = await _controller.PostPatient(dto);

            Assert.IsInstanceOf<BadRequestObjectResult>(result.Result);
        }
    }
}