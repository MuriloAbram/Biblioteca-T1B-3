using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T1B_3Library.Application.DTOs;

namespace T1B_3Library.Application.ViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<BookDto> FeaturedBooks { get; set; } = new List<BookDto>();
        public IEnumerable<GenderDto> Categories { get; set; } = new List<GenderDto>();
        public IEnumerable<BookDto> RecentBooks { get; set; } = new List<BookDto>();
    }

    public class BookDetailsViewModel
    {
        public BookDto Book { get; set; } = new BookDto();
        public IEnumerable<BookDto> RelatedBooks { get; set; } = new List<BookDto>();
    }

    public class DashboardViewModel
    {
        public int TotalBooks { get; set; }
        public int TotalGender { get; set; }
        public int FeaturedBooks { get; set; }
        public IEnumerable<BookDto> RecentBooks { get; set; } = new List<BookDto>();
    }

    public class BookFormViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public int YearPublication { get; set; }
        public int GenderId { get; set; }
        public bool IsFeatured { get; set; }

        public IEnumerable<GenderDto> Gender { get; set; } = new List<GenderDto>();
    }

    public class BookListViewModel
    {
        public IEnumerable<BookDto> Book { get; set; } = new List<BookDto>();
        public IEnumerable<BookDto> Gender { get; set; } = new List<BookDto>();
        public int? SelectedCategoryId { get; set; }
    }
}
