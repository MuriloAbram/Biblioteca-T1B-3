using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace T1B_3Library.Application.DTOs
{
    public class GenderDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int BookCount { get; set; }
    }

    public class CreateGenderDto
    {
        public string Name { get; set; } = string.Empty;
    }

    public class UpdateGenderDto 
    {
        public string Name { get; set; } = string.Empty;
    }
}
