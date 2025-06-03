using System.Threading.Tasks;
using AutoMapper;
using FirstAPI.Interfaces;
using FirstAPI.Misc;
using FirstAPI.Models;
using FirstAPI.Models.DTOs.DoctorSpecialities;


namespace FirstAPI.Services
{
    public class PatientService : IPatientService
    {
        private readonly IRepository<int, Patient> _patientRepository;
        private readonly IRepository<string, User> _userRepository;
        private readonly IEncryptionService _encryptionService;
        private readonly IMapper _mapper;

        public PatientService(
            IRepository<int, Patient> patientRepository,
            IRepository<string, User> userRepository,
            IEncryptionService encryptionService,
            IMapper mapper)
        {
            _patientRepository = patientRepository;
            _userRepository = userRepository;
            _encryptionService = encryptionService;
            _mapper = mapper;
        }

        public async Task<Patient> AddPatient(PatientAddRequestDto patientDto)
        {
            try
            {
                var user = _mapper.Map<PatientAddRequestDto, User>(patientDto);
                var encryptedData = await _encryptionService.EncryptData(new EncryptModel
                {
                    Data = patientDto.Password
                });
                user.Password = encryptedData.EncryptedData;
                user.HashKey = encryptedData.HashKey;
                user.Role = "Patient";
                user = await _userRepository.Add(user);

                var patient = _mapper.Map<PatientAddRequestDto, Patient>(patientDto);
                patient.User = user;
                if(patient.User == null)
                    throw new Exception("User could not be added");
                patient = await _patientRepository.Add(patient);

                if (patient == null)
                    throw new Exception("Could not add patient");

                return patient;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public Task<Patient> GetPatientById(int id)
        {
            return _patientRepository.Get(id);
        }
    }
}