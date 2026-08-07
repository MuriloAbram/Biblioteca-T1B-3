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
            var categories = await _genderRepository.GetAllAsync();
            return categories.Select(MapToDto);
        }



        private static GenderDto MapToDto(Gender category)
        {
            return new GenderDto
            {
                Id = category.Id,
                Name = category.Name,
                GameCount = category.Games?.Count ?? 0
            };
        }
    }
}
