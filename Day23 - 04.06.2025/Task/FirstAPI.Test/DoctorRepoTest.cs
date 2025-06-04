
using FirstAPI.Contexts;
using FirstAPI.Models;
using FirstAPI.Repositories;
using FirstAPI.Interfaces;
using Microsoft.EntityFrameworkCore;



namespace FirstAPI.Test;

public class Tests
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

    /*[Test]
    public async Task AddDoctorTest()
    {
        //arrange
        var email = " test@gmail.com";
        var password = System.Text.Encoding.UTF8.GetBytes("test123");
        var key = Guid.NewGuid().ToByteArray();
        var user = new User
        {
            Username = email,
            Password = password,
            HashKey = key,
            Role = "Doctor"
        };
        _context.Add(user);
        await _context.SaveChangesAsync();
        var doctor = new Doctor
        {
            Name = "test",
            YearsOfExperience = 2,
            Email = email
        };
        IRepository<int, Doctor> _doctorRepository = new DoctorRepository(_context);
        //action
        var result = await _doctorRepository.Add(doctor);
        //assert
        Assert.That(result, Is.Not.Null, "Doctor IS not addeed");
        Assert.That(result.Id, Is.EqualTo(1));
    }
    [TestCase(1)]
    [TestCase(2)]
    public async Task GetDoctorPassTest(int id)
    {
        //arrange
        var email = " test@gmail.com";
        var password = System.Text.Encoding.UTF8.GetBytes("test123");
        var key = Guid.NewGuid().ToByteArray();
        var user = new User
        {
            Username = email,
            Password = password,
            HashKey = key,
            Role = "Doctor"
        };
        _context.Add(user);
        await _context.SaveChangesAsync();
        var doctor = new Doctor
        {
            Name = "test",
            YearsOfExperience = 2,
            Email = email
        };
        IRepository<int, Doctor> _doctorRepository = new DoctorRepository(_context);
        //action
        await _doctorRepository.Add(doctor);

        //action
        var result = _doctorRepository.Get(id);
        //assert
        Assert.That(result.Id, Is.EqualTo(id));

    }*/

    [Test]
    public async Task AddDoctor_Success()
    {
        //arrange
        var email = "test1@gmail.com";
        var password = System.Text.Encoding.UTF8.GetBytes("test123");
        var key = Guid.NewGuid().ToByteArray();
        var user = new User
        {
            Username = email,
            Password = password,
            HashKey = key,
            Role = "Doctor"
        };
        _context.Add(user);
        await _context.SaveChangesAsync();
        var doctor = new Doctor
        {
            Name = "test",
            YearsOfExperience = 2,
            Email = email
        };

        IRepository<int, Doctor> _doctorRepository = new DoctorRepository(_context);
        //action
        var result = await _doctorRepository.Add(doctor);
        //assert
        Assert.That(result, Is.Not.Null, "Doctor IS not addeed");
        Assert.That(result.Id, Is.EqualTo(1));
    }

    
    [Test]
    public async Task AddDoctor_ThrowsException_WhenDuplicateEmail()
    {
        // Arrange
        var email = "test2@gmail.com";
        var password = System.Text.Encoding.UTF8.GetBytes("test123");
        var key = Guid.NewGuid().ToByteArray();
        var user = new User
        {
            Username = email,
            Password = password,
            HashKey = key,
            Role = "Doctor"
        };
        _context.Add(user);
        await _context.SaveChangesAsync();
        var doctor = new Doctor
        {
            Name = "test",
            YearsOfExperience = 2,
            Email = email
        };

        IRepository<int, Doctor> _doctorRepository = new DoctorRepository(_context);
        await _doctorRepository.Add(doctor);

        // Act & Assert: Adding the same doctor again should throw an exception
        var ex = Assert.ThrowsAsync<Exception>(async () => await _doctorRepository.Add(doctor));
        Assert.That(ex.Message, Is.EqualTo("Item already exists in the database"));
    }

    
    [Test]
    public async Task GetDoctor_Success()
    {
        // Arrange
        var email = "getdoctor@gmail.com";
        var password = System.Text.Encoding.UTF8.GetBytes("test123");
        var key = Guid.NewGuid().ToByteArray();
        var user = new User
        {
            Username = email,
            Password = password,
            HashKey = key,
            Role = "Doctor"
        };
        _context.Add(user);
        await _context.SaveChangesAsync();
        var doctor = new Doctor
        {
            Name = "GetDoctorTest",
            YearsOfExperience = 5,
            Email = email
        };
        IRepository<int, Doctor> _doctorRepository = new DoctorRepository(_context);
        var addedDoctor = await _doctorRepository.Add(doctor);

        // Act
        var result = await _doctorRepository.Get(addedDoctor.Id);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Id, Is.EqualTo(addedDoctor.Id));
        Assert.That(result.Name, Is.EqualTo("GetDoctorTest"));
    }

    [TestCase(1)]
    public async Task GetDoctorById_Success(int id)
    {
        // Arrange
        IRepository<int, Doctor> _doctorRepository = new DoctorRepository(_context);
        // Act
        var result = await _doctorRepository.Get(id);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Id, Is.EqualTo(id));
    }

    [Test]
    public async Task UpdateDoctor_Success()
    {
        // Arrange
        var email = "test@gmail.com";
        var password = System.Text.Encoding.UTF8.GetBytes("test123");
        var key = Guid.NewGuid().ToByteArray();
        var user = new User
        {
            Username = email,
            Password = password,
            HashKey = key,
            Role = "Doctor"
        };
        _context.Add(user);
        await _context.SaveChangesAsync();
        var doctor = new Doctor
        {
            Name = "test",
            YearsOfExperience = 2,
            Email = email
        };
        IRepository<int, Doctor> _doctorRepository = new DoctorRepository(_context);
        var addedDoctor = await _doctorRepository.Add(doctor);

        // Act
        addedDoctor.Name = "updated";
        var updatedDoctor = await _doctorRepository.Update(addedDoctor.Id, addedDoctor);

        // Assert
        Assert.That(updatedDoctor, Is.Not.Null);
        Assert.That(updatedDoctor.Name, Is.EqualTo("updated"));
    }

    [Test]
    public void UpdateDoctor_ThrowsException_WhenNotFound()
    {
        // Arrange
        IRepository<int, Doctor> _doctorRepository = new DoctorRepository(_context);
        var nonExistentDoctor = new Doctor
        {
            Id = 999,
            Name = "Dummy",
            YearsOfExperience = 0,
            Email = "dummy@gmail.com"
        };

        // Act & Assert
        var ex = Assert.ThrowsAsync<Exception>(async () => await _doctorRepository.Update(nonExistentDoctor.Id, nonExistentDoctor));
        Assert.That(ex.Message, Is.EqualTo("No doctor with the given ID"));
    }

    [Test]
    public async Task DeleteDoctor_Success()
    {
        // Arrange
        var email = "delete@gmail.com";
        var password = System.Text.Encoding.UTF8.GetBytes("test123");
        var key = Guid.NewGuid().ToByteArray();
        var user = new User
        {
            Username = email,
            Password = password,
            HashKey = key,
            Role = "Doctor"
        };
        _context.Add(user);
        await _context.SaveChangesAsync();
        var doctor = new Doctor
        {
            Name = "deleteTest",
            YearsOfExperience = 1,
            Email = email
        };
        IRepository<int, Doctor> _doctorRepository = new DoctorRepository(_context);
        var addedDoctor = await _doctorRepository.Add(doctor);

        // Act
        var deletedDoctor = await _doctorRepository.Delete(addedDoctor.Id);

        // Assert
        Assert.That(deletedDoctor, Is.Not.Null);
        Assert.That(deletedDoctor.Id, Is.EqualTo(addedDoctor.Id));
    }

    [Test]
    public void DeleteDoctor_ThrowsException_WhenNotFound()
    {
        // Arrange
        IRepository<int, Doctor> _doctorRepository = new DoctorRepository(_context);
        int nonExistentId = 999;

        // Act & Assert
        var ex = Assert.ThrowsAsync<Exception>(async () => await _doctorRepository.Delete(nonExistentId));
        Assert.That(ex.Message, Is.EqualTo("No doctor with the given ID"));
    }


    
    [TearDown]
    public void TearDown() 
    {
        _context.Dispose();
    }
}