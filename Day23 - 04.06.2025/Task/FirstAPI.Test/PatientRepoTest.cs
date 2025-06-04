using FirstAPI.Contexts;
using FirstAPI.Models;
using FirstAPI.Repositories;
using FirstAPI.Interfaces;
using Microsoft.EntityFrameworkCore;
using Moq;


namespace FirstAPI.Test;

public class PatientRepoTest
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
    public async Task AddPatient_Success()
    {
        IRepository<int, Patient> _patientRepository = new PatinetRepository(_context);
        var patientRepoMock = new Mock<PatinetRepository>(_context);
        var email = "testing@gmail.com";
        var password = System.Text.Encoding.UTF8.GetBytes("test123");
        var key = Guid.NewGuid().ToByteArray();
        var user = new User
        {
            Username = email,
            Password = password,
            HashKey = key,
            Role = "Patient"
        };
        _context.Add(user);
        await _context.SaveChangesAsync();
        var patient = new Patient
        {
            Name = "test",
            Age = 30,
            Email = email,
            Phone = "1234567890",
            Appointmnets = new List<Appointmnet>(),

        };
        
        //action
        var result = await _patientRepository.Add(patient);
        //assert
        Assert.That(result, Is.Not.Null, "Patient is not addeed");
        Assert.That(result.Id, Is.EqualTo(1));
    }
    
    [Test]
    public async Task AddPatient_ThrowsException_WhenDuplicateEmail()
    {
        IRepository<int, Patient> _patientRepository = new PatinetRepository(_context);
        var email = "testing1@gmail.com";
        var password = System.Text.Encoding.UTF8.GetBytes("test123");
        var key = Guid.NewGuid().ToByteArray();
        var user = new User
        {
            Username = email,
            Password = password,
            HashKey = key,
            Role = "Patient"
        };
        _context.Add(user);
        await _context.SaveChangesAsync();
        var patient = new Patient
        {
            Name = "test",
            Age = 30,
            Email = email,
            Phone = "1234567890",
            Appointmnets = new List<Appointmnet>(),
        };

        // Add the patient once
        await _patientRepository.Add(patient);

        // Act & Assert: Adding the same patient again should throw an exception
        var ex = Assert.ThrowsAsync<Exception>(async () => await _patientRepository.Add(patient));
        Assert.That(ex.Message, Is.EqualTo("Item already exists in the database")); 
    }

    [Test]
    public async Task UpdatePatient_Success()
    {
        // Arrange
        IRepository<int, Patient> _patientRepository = new PatinetRepository(_context);
        var email = "update@gmail.com";
        var password = System.Text.Encoding.UTF8.GetBytes("test123");
        var key = Guid.NewGuid().ToByteArray();
        var user = new User
        {
            Username = email,
            Password = password,
            HashKey = key,
            Role = "Patient"
        };
        _context.Add(user);
        await _context.SaveChangesAsync();
        var patient = new Patient
        {
            Name = "test",
            Age = 30,
            Email = email,
            Phone = "1234567890",
            Appointmnets = new List<Appointmnet>(),
        };
        var addedPatient = await _patientRepository.Add(patient);

        // Act
        addedPatient.Name = "Kim";
        var updatedPatient = await _patientRepository.Update(addedPatient.Id, addedPatient);

        // Assert
        Assert.That(updatedPatient, Is.Not.Null);
        Assert.That(updatedPatient.Name, Is.EqualTo("Kim"));
    }

    [Test]
    public void UpdatePatient_ThrowsException_WhenNotFound()
    {
        // Arrange
        IRepository<int, Patient> _patientRepository = new PatinetRepository(_context);
        var patient = new Patient
        {
            Id = 999,
            Name = "notfound",
            Age = 40,
            Email = "notfound@gmail.com",
            Phone = "0000000000",
            Appointmnets = new List<Appointmnet>(),
        };

        // Act & Assert
        var ex = Assert.ThrowsAsync<Exception>(async () => await _patientRepository.Update(patient.Id, patient));
        Assert.That(ex.Message, Is.EqualTo("No patient with the given ID")); 
    }


    [TestCase(1)]
    public async Task GetPatientById_Success(int id)
    {
        // Arrange
        IRepository<int, Patient> _patientRepository = new PatinetRepository(_context);
        var email = $"getbyid{id}@gmail.com";
        var password = System.Text.Encoding.UTF8.GetBytes("test123");
        var key = Guid.NewGuid().ToByteArray();
        var user = new User
        {
            Username = email,
            Password = password,
            HashKey = key,
            Role = "Patient"
        };
        _context.Add(user);
        await _context.SaveChangesAsync();
        var patient = new Patient
        {
            Name = "test",
            Age = 30,
            Email = email,
            Phone = "1234567890",
            Appointmnets = new List<Appointmnet>(),
        };
        var addedPatient = await _patientRepository.Add(patient);

        // Act
        var result = await _patientRepository.Get(addedPatient.Id);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Id, Is.EqualTo(addedPatient.Id));
    }


    [TestCase(999)]
    [TestCase(12345)]
    public void GetPatientById_ThrowsException_WhenNotFound(int id)
    {
        // Arrange
        IRepository<int, Patient> _patientRepository = new PatinetRepository(_context);

        // Act & Assert
        var ex = Assert.ThrowsAsync<Exception>(async () => await _patientRepository.Get(id));
        Assert.That(ex.Message, Is.EqualTo("No patient with the given ID"));
    }


    [TestCase(1)]
    public async Task DeletePatient_Success(int id)
    {
        // Arrange
        IRepository<int, Patient> _patientRepository = new PatinetRepository(_context);
        var email = $"delete{id}@gmail.com";
        var password = System.Text.Encoding.UTF8.GetBytes("test123");
        var key = Guid.NewGuid().ToByteArray();
        var user = new User
        {
            Username = email,
            Password = password,
            HashKey = key,
            Role = "Patient"
        };
        _context.Add(user);
        await _context.SaveChangesAsync();
        var patient = new Patient
        {
            Name = "test",
            Age = 30,
            Email = email,
            Phone = "1234567890",
            Appointmnets = new List<Appointmnet>(),
        };
        var addedPatient = await _patientRepository.Add(patient);

        // Act
        var deletedPatient = await _patientRepository.Delete(addedPatient.Id);

        // Assert
        Assert.That(deletedPatient, Is.Not.Null);
        Assert.That(deletedPatient.Id, Is.EqualTo(addedPatient.Id));
    }

    
    [TestCase(999)]
    [TestCase(12345)]
    public void DeletePatient_ThrowsException_WhenNotFound(int id)
    {
        // Arrange
        IRepository<int, Patient> _patientRepository = new PatinetRepository(_context);

        // Act & Assert
        var ex = Assert.ThrowsAsync<Exception>(async () => await _patientRepository.Delete(id));
        Assert.That(ex.Message, Is.EqualTo("No patient with the given ID"));
    }


    [TearDown]
    public void TearDown()
    {
        _context.Dispose();
    }
}