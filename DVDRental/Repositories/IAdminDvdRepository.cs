using DVDRental.Entities;

namespace DVDRental.Repositories
{
    public interface IAdminDvdRepository
    {
        Task<List<MovieDvd>> GetAllAsync();
        Task<MovieDvd> GetByIdAsync(string id);
        Task<MovieDvd> AddAsync(MovieDvd dvd);
        Task UpdateAsync(MovieDvd dvd);
        Task DeleteAsync(String id);
       
    }
}
