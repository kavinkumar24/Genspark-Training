using System.Collections.Generic;
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
    public class DoctorControllerTest
    {
        private Mock<IDoctorService> _doctorServiceMock;
        private DoctorController _controller;

        [SetUp]
        public void Setup()
        {
            _doctorServiceMock = new Mock<IDoctorService>();
            _controller = new DoctorController(_doctorServiceMock.Object);
        }

        [Test]
        public async Task GetDoctors_ReturnsOkResult_WithDoctorsList()
        {
            // Arrange
            var speciality = "Cardiology";
            var doctorsList = new List<DoctorsBySpecialityResponseDto>
            {
                new DoctorsBySpecialityResponseDto { Dname = "Dr. John", Yoe = 5, Id = 1 }
            };
            _doctorServiceMock.Setup(s => s.GetDoctorsBySpeciality(speciality)).ReturnsAsync(doctorsList);

            // Act
            var result = await _controller.GetDoctors(speciality);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(doctorsList, okResult.Value);
        }

        [Test]
        public async Task PostDoctor_ReturnsCreated_WhenDoctorAdded()
        {
            // Arrange
            var doctorRequest = new DoctorAddRequestDto { Name = "Dr. John", Password = "pass" };
            var doctor = new Doctor { Id = 1, Name = "Dr. John" };
            _doctorServiceMock.Setup(s => s.AddDoctor(doctorRequest)).ReturnsAsync(doctor);

            // Act
            var result = await _controller.PostDoctor(doctorRequest);

            // Assert
            Assert.IsInstanceOf<CreatedResult>(result.Result);
            var createdResult = result.Result as CreatedResult;
            Assert.IsNotNull(createdResult);
            Assert.AreEqual(doctor, createdResult.Value);
        }

        [Test]
        public async Task PostDoctor_ReturnsBadRequest_WhenServiceReturnsNull()
        {
            // Arrange
            var doctorRequest = new DoctorAddRequestDto { Name = "Dr. Jane", Password = "pass" };
            _doctorServiceMock.Setup(s => s.AddDoctor(doctorRequest)).ReturnsAsync((Doctor)null);

            // Act
            var result = await _controller.PostDoctor(doctorRequest);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result.Result);
        }

        [Test]
        public async Task PostDoctor_ReturnsBadRequest_WhenExceptionThrown()
        {
            // Arrange
            var doctorRequest = new DoctorAddRequestDto { Name = "Dr. Error", Password = "pass" };
            _doctorServiceMock.Setup(s => s.AddDoctor(doctorRequest)).ThrowsAsync(new System.Exception("Some error"));

            // Act
            var result = await _controller.PostDoctor(doctorRequest);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result.Result);
            var badRequest = result.Result as BadRequestObjectResult;
            Assert.AreEqual("Some error", badRequest.Value);
        }
    }
}