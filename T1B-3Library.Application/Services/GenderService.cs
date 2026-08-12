using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T1B_3Library.Application.DTOs;
using T1B_3Library.Application.Interfaces;
using T1B_3Library.Domain.Entities;
using T1B_3Library.Domain.Interfaces;

namespace T1B_3Library.Application.Services
{
    public class GenderService : IGenderService
    {
        private readonly IGenderRepository _genderRepository;

        public GenderService(IGenderRepository genderRepository)
        {
            _genderRepository = genderRepository;
        }

        public async Task<IEnumerable<GenderDto>> GetAllAsync()
        {
            var gender = await _genderRepository.GetAllAsync();
            return gender.Select(MapToDto);
        }

        public async Task<GenderDto?> GetByIdAsync(int id)
        {
            var gender = await _genderRepository.GetByIdAsync(id);
            return gender == null ? null : MapToDto(gender);
        }

        public async Task<GenderDto> CreateAsync(CreateGenderDto dto)
        {
            var gender = new Gender { Name = dto.Name };
            await _genderRepository.AddAsync(gender);
            return MapToDto(gender);
        }

        public async Task<GenderDto?> UpdateAsync(int id, UpdateGenderDto dto)
        {
            var gender = await _genderRepository.GetByIdAsync(id);
            if (gender == null) return null;

            gender.Name = dto.Name;
            await _genderRepository.UpdateAsync(gender);
            return MapToDto(gender);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var gender = await _genderRepository.GetByIdAsync(id);
            if (gender == null) return false;

            await _genderRepository.DeleteAsync(id);
            return true;
        }

        public async Task<int> CountAsync()
        {
            return await _genderRepository.CountAsync();
        }


        private static GenderDto MapToDto(Gender gender)
        {
            return new GenderDto
            {
                Id = gender.Id,
                Name = gender.Name,
                BookCount = gender.Books?.Count ?? 0
            };
        }
    }
}
