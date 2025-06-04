using FirstAPI.Contexts;
using FirstAPI.Models;
using FirstAPI.Repositories;
using FirstAPI.Interfaces;
using FirstAPI.Services;
using FirstAPI.Models.DTOs;
using FirstAPI.Models.DTOs.DoctorSpecialities;
using FirstAPI.Misc;
using Microsoft.EntityFrameworkCore;
using Moq;
using AutoMapper;

namespace FirstAPI.Test;

public class DoctorServiceTest
{
    private ClinicContext _context;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<ClinicContext>()
                            .UseInMemoryDatabase("TestDb")
                            .Options;
        _context = new ClinicContext(options);
    }

    [TestCase("General")]
    [TestCase("Cardiology")]
    [TestCase("Dermatology")]
    public async Task TestGetDoctorBySpeciality(string speciality)
    {
        var doctorRepositoryMock = new Mock<DoctorRepository>(_context);
        var specialityRepositoryMock = new Mock<SpecialityRepository>(_context);
        var doctorSpecialityRepositoryMock = new Mock<DoctorSpecialityRepository>(_context);
        var userRepositoryMock = new Mock<UserRepository>(_context);
        var otherContextFunctionitiesMock = new Mock<OtherFuncinalitiesImplementation>(_context);
        var encryptionServiceMock = new Mock<EncryptionService>();
        var mapperMock = new Mock<IMapper>();

        otherContextFunctionitiesMock.Setup(ocf => ocf.GetDoctorsBySpeciality(It.IsAny<string>()))
            .ReturnsAsync((string spec) => new List<DoctorsBySpecialityResponseDto>
            {
                new DoctorsBySpecialityResponseDto
                {
                    Dname = "test",
                    Yoe = 2,
                    Id = 1
                }
            });

        IDoctorService doctorService = new DoctorService(
            doctorRepositoryMock.Object,
            specialityRepositoryMock.Object,
            doctorSpecialityRepositoryMock.Object,
            userRepositoryMock.Object,
            otherContextFunctionitiesMock.Object,
            encryptionServiceMock.Object,
            mapperMock.Object
        );

        // Act
        var result = await doctorService.GetDoctorsBySpeciality(speciality);

        // Assert
        Assert.That(result.Count(), Is.EqualTo(1));
    }

    [Test]
    public async Task AddDoctor_ShouldAddDoctorAndUser_WhenValidRequest()
    {
        // Arrange
        var doctorRepositoryMock = new Mock<IRepository<int, Doctor>>();
        var specialityRepositoryMock = new Mock<IRepository<int, Speciality>>();
        var doctorSpecialityRepositoryMock = new Mock<IRepository<int, DoctorSpeciality>>();
        var userRepositoryMock = new Mock<IRepository<string, User>>();
        var otherContextFunctionitiesMock = new Mock<IOtherContextFunctionities>();
        var encryptionServiceMock = new Mock<IEncryptionService>();
        var mapperMock = new Mock<IMapper>();

        var doctorAddRequest = new DoctorAddRequestDto
        {
            Name = "Dr. John",
            Password = "password123",
            Specialities = new List<SpecialityAddRequestDto>
            {
                new SpecialityAddRequestDto { Name = "Cardiology" }
            }
        };

        var user = new User
        {
            Username = "Dr. John",
            Password = System.Text.Encoding.UTF8.GetBytes("encrypted"),
            HashKey = System.Text.Encoding.UTF8.GetBytes("hash"),
            Role = "Doctor"
        };
        var encryptedModel = new EncryptModel
        {
            EncryptedData = System.Text.Encoding.UTF8.GetBytes("encrypted"),
            HashKey = System.Text.Encoding.UTF8.GetBytes("hash")
        };
        var doctor = new Doctor { Id = 1, Name = "Dr. John", User = user };

        mapperMock.Setup(m => m.Map<DoctorAddRequestDto, User>(It.IsAny<DoctorAddRequestDto>())).Returns(user);
        encryptionServiceMock.Setup(e => e.EncryptData(It.IsAny<EncryptModel>())).ReturnsAsync(encryptedModel);
        userRepositoryMock.Setup(u => u.Add(It.IsAny<User>())).ReturnsAsync(user);

        doctorRepositoryMock.Setup(d => d.Add(It.IsAny<Doctor>())).ReturnsAsync(doctor);
        specialityRepositoryMock.Setup(s => s.GetAll()).ReturnsAsync(new List<Speciality>());
        specialityRepositoryMock.Setup(s => s.Add(It.IsAny<Speciality>())).ReturnsAsync(new Speciality { Id = 2, Name = "Cardiology" });
        doctorSpecialityRepositoryMock.Setup(ds => ds.Add(It.IsAny<DoctorSpeciality>())).ReturnsAsync(new DoctorSpeciality { SerialNumber = 1, DoctorId = 1, SpecialityId = 2 });

        var doctorService = new DoctorService(
            doctorRepositoryMock.Object,
            specialityRepositoryMock.Object,
            doctorSpecialityRepositoryMock.Object,
            userRepositoryMock.Object,
            otherContextFunctionitiesMock.Object,
            encryptionServiceMock.Object,
            mapperMock.Object
        );

        // Act
        var result = await doctorService.AddDoctor(doctorAddRequest);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual("Dr. John", result.Name);
        userRepositoryMock.Verify(u => u.Add(It.IsAny<User>()), Times.Once);
        doctorRepositoryMock.Verify(d => d.Add(It.IsAny<Doctor>()), Times.Once);
        doctorSpecialityRepositoryMock.Verify(ds => ds.Add(It.IsAny<DoctorSpeciality>()), Times.Once);
    }

    [Test]
    public void AddDoctor_ShouldThrowException_WhenDoctorRepositoryReturnsNull()
    {
        // Arrange
        var doctorRepositoryMock = new Mock<IRepository<int, Doctor>>();
        var specialityRepositoryMock = new Mock<IRepository<int, Speciality>>();
        var doctorSpecialityRepositoryMock = new Mock<IRepository<int, DoctorSpeciality>>();
        var userRepositoryMock = new Mock<IRepository<string, User>>();
        var otherContextFunctionitiesMock = new Mock<IOtherContextFunctionities>();
        var encryptionServiceMock = new Mock<IEncryptionService>();
        var mapperMock = new Mock<IMapper>();

        var doctorAddRequest = new DoctorAddRequestDto
        {
            Name = "Dr. Jane",
            Password = "password123",
            Specialities = new List<SpecialityAddRequestDto>
            {
                new SpecialityAddRequestDto { Name = "Dermatology" }
            }
        };

        var user = new User
        {
            Username = "Dr. Jane",
            Password = System.Text.Encoding.UTF8.GetBytes("encrypted"),
            HashKey = System.Text.Encoding.UTF8.GetBytes("hash"),
            Role = "Doctor"
        };
        var encryptedModel = new EncryptModel
        {
            EncryptedData = System.Text.Encoding.UTF8.GetBytes("encrypted"),
            HashKey = System.Text.Encoding.UTF8.GetBytes("hash")
        };

        mapperMock.Setup(m => m.Map<DoctorAddRequestDto, User>(It.IsAny<DoctorAddRequestDto>())).Returns(user);
        encryptionServiceMock.Setup(e => e.EncryptData(It.IsAny<EncryptModel>())).ReturnsAsync(encryptedModel);
        userRepositoryMock.Setup(u => u.Add(It.IsAny<User>())).ReturnsAsync(user);
        doctorRepositoryMock.Setup(d => d.Add(It.IsAny<Doctor>())).ReturnsAsync((Doctor)null);

        var doctorService = new DoctorService(
            doctorRepositoryMock.Object,
            specialityRepositoryMock.Object,
            doctorSpecialityRepositoryMock.Object,
            userRepositoryMock.Object,
            otherContextFunctionitiesMock.Object,
            encryptionServiceMock.Object,
            mapperMock.Object
        );

        // Act & Assert
        var ex = Assert.ThrowsAsync<Exception>(async () => await doctorService.AddDoctor(doctorAddRequest));
        Assert.That(ex.Message, Is.EqualTo("Could not add doctor"));
    }

    [TearDown]
    public void TearDown()
    {
        _context.Dispose();
    }
}