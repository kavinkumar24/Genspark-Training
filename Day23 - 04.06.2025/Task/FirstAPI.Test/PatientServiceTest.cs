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

public class EncryptResult
{
    public byte[] EncryptedData { get; set; }
    public byte[] HashKey { get; set; }
}
public class PatientServiceTest
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


    [Test]
    public async Task AddPatient_ReturnsPatient_WhenSuccessful()
    {
        // Arrange
        var patientDto = new PatientAddRequestDto
        {
            Name = "Kim",
            Age = 30,
            Email = "kim@gmail.com",
            Password = "test123",
            Phone = "1234567890"
        };

        var user = new User
        {
            Username = patientDto.Email,
            Role = "Patient",
            Password = new byte[] { 1, 2, 3 },
            HashKey = new byte[] { 4, 5, 6 }
        };

        var patient = new Patient
        {
            Id = 1,
            Name = patientDto.Name,
            Age = patientDto.Age,
            Email = patientDto.Email,
            Phone = patientDto.Phone,
            User = user
        };

        var encryptResult = new EncryptModel
        {
            EncryptedData = new byte[] { 1, 2, 3 },
            HashKey = new byte[] { 4, 5, 6 }
        };

        // Mock the interfaces, not the concrete classes!
        var patientRepositoryMock = new Mock<IRepository<int, Patient>>();
        var userRepositoryMock = new Mock<IRepository<string, User>>();
        var encryptionServiceMock = new Mock<IEncryptionService>();
        var mapperMock = new Mock<IMapper>();

        mapperMock.Setup(m => m.Map<PatientAddRequestDto, User>(patientDto)).Returns(user);
        mapperMock.Setup(m => m.Map<PatientAddRequestDto, Patient>(patientDto)).Returns(patient);

        encryptionServiceMock.Setup(e => e.EncryptData(It.IsAny<EncryptModel>()))
            .ReturnsAsync(encryptResult);

        userRepositoryMock.Setup(r => r.Add(It.IsAny<User>())).ReturnsAsync(user);
        patientRepositoryMock.Setup(r => r.Add(It.IsAny<Patient>())).ReturnsAsync(patient);

        var patientService = new PatientService(
            patientRepositoryMock.Object,
            userRepositoryMock.Object,
            encryptionServiceMock.Object,
            mapperMock.Object
        );

        // Act
        var result = await patientService.AddPatient(patientDto);

        // Assert
        Assert.IsNotNull(result);
        Assert.That(result.Name, Is.EqualTo(patientDto.Name));
        Assert.That(result.Email, Is.EqualTo(patientDto.Email));
        Assert.That(result.User, Is.Not.Null);
        Assert.That(result.User.Username, Is.EqualTo(patientDto.Email));
    }
    [TestCase(1)]
    // [TestCase(2)]
    // [TestCase(-3)]
    public async Task TestGetPatientById_ReturnsPatient(int patientId)
    {
        // Arrange
        var expectedPatient = new Patient { Id = patientId, Name = "Kim", Age = 30 };

        var patientRepositoryMock = new Mock<PatinetRepository>(_context);
        var userRepositoryMock = new Mock<UserRepository>(_context);
        var encryptionServiceMock = new Mock<EncryptionService>();
        var mapperMock = new Mock<IMapper>();

        patientRepositoryMock.Setup(repo => repo.Get(patientId))
                            .ReturnsAsync(expectedPatient);

        var patientService = new PatientService(
            patientRepositoryMock.Object,
            userRepositoryMock.Object,
            encryptionServiceMock.Object,
            mapperMock.Object
        );

        // Act
        var result = await patientService.GetPatientById(patientId);

        // Assert
        Assert.IsNotNull(result);
        Assert.That(result.Id, Is.EqualTo(patientId));
        Assert.That(result.Name, Is.EqualTo("Kim"));
    }

    
    [TearDown]
    public void TearDown() 
    {
        _context.Dispose();
    }
}