using DVDRental.DTOs.RequestDTO;
using DVDRental.DTOs.ResponseDTO;
using DVDRental.Entities;
using DVDRental.Repositories;

namespace DVDRental.Services
{
    public class AdminDvdService:IAdminDvdService
    {
        private readonly IAdminDvdRepository _dvdRepository;
        private readonly IAdminCategoriesRepository _categoryRepository;

        public AdminDvdService(IAdminDvdRepository dvdRepository, IAdminCategoriesRepository categoryRepository)
        {
            _dvdRepository = dvdRepository;
            _categoryRepository = categoryRepository;
        }

        // Get all DVDs
        public async Task<List<DVDResponseDTO>> GetAllAsync()
        {
            var movieDvds = await _dvdRepository.GetAllAsync();

            return movieDvds.Select(movieDvd => new DVDResponseDTO
            {
                ID = movieDvd.ID,
                MovieName = movieDvd.Title,
                ReleaseDate = movieDvd.ReleaseDate,
                Director = movieDvd.Director,
                Copies = movieDvd.Copies,
                Categories = movieDvd.Categories.Select(c => c.CategoryID).ToList(),
                ImagePath = movieDvd.ImagePath
            }).ToList();
            Console.WriteLine(movieDvds);
            Console.ReadLine();
        }

        // Get DVD by ID
        public async Task<DVDResponseDTO> GetByIdAsync(string id)
        {
            var movieDvd = await _dvdRepository.GetByIdAsync(id);

            if (movieDvd == null) return null;

            return new DVDResponseDTO
            {
                ID = movieDvd.ID,
                MovieName = movieDvd.Title,
                ReleaseDate = movieDvd.ReleaseDate,
                Director = movieDvd.Director,
                Copies = movieDvd.Copies,
                Categories = movieDvd.Categories.Select(c => c.CategoryID).ToList(),
                ImagePath = movieDvd.ImagePath
            };
        }

        // Add a new DVD
        public async Task<DVDResponseDTO> AddAsync(DVDRequestDTO movieDvdRequest)
        {
           
            var movieDvd = new MovieDvd
            {
                Title = movieDvdRequest.Title,
                ReleaseDate = movieDvdRequest.ReleaseDate,
                Director = movieDvdRequest.Director,
                Copies = movieDvdRequest.Copies,
                ImagePath = movieDvdRequest.Image.ToString(),
                Categories = new List<Categories>()
            };
           

            foreach (var categoryId in movieDvdRequest.CategoryIds)
            {
                var category = await _categoryRepository.GetByIdAsync(categoryId);
                if (category != null)
                {
                    movieDvd.Categories.Add(category);
                }
            }
            var data = await _dvdRepository.AddAsync(movieDvd);
            var responseDVD = new DVDResponseDTO
            { 
                ID=data.ID,
                MovieName= data.Title,
                Director=data.Director,
                ReleaseDate= data.ReleaseDate,
                Categories= data.Categories.Select(c => c.CategoryID).ToList(),
                Copies=data.Copies,
                ImagePath=data.ImagePath
            };
            return responseDVD;


        }





        // Update an existing DVD
        public async Task UpdateAsync(string id, DVDRequestDTO movieDvdRequest)
        {
            var movieDvd = await _dvdRepository.GetByIdAsync(id);

            if (movieDvd == null) return;

            movieDvd.Title = movieDvdRequest.Title;
            movieDvd.ReleaseDate = movieDvdRequest.ReleaseDate;
            movieDvd.Director = movieDvdRequest.Director;
            movieDvd.Copies = movieDvdRequest.Copies;
            movieDvd.ImagePath = movieDvdRequest.Image.ToString();

          
            movieDvd.Categories.Clear();

            foreach (var categoryId in movieDvdRequest.CategoryIds)
            {
                var category = await _categoryRepository.GetByIdAsync(categoryId);
                if (category != null)
                {
                    movieDvd.Categories.Add(category);
                }
            }

            await _dvdRepository.UpdateAsync(movieDvd);
        }

        // Delete a DVD
        public async Task DeleteAsync(string id)
        {
            await _dvdRepository.DeleteAsync(id);
        }



        private string GenerateNewDvdId(string lastDvdId)
        {
            if (string.IsNullOrEmpty(lastDvdId))
            {
               
                return "dvd001";
            }

            string numericPart = lastDvdId.Substring(3);
            int numericId = int.Parse(numericPart) + 1;

            return $"dvd{numericId.ToString("D3")}";
        }
    }
}
