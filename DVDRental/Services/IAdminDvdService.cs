using DVDRental.DTOs.RequestDTO;
using DVDRental.DTOs.ResponseDTO;
using DVDRental.Entities;

namespace DVDRental.Services
{
    public interface IAdminDvdService
    {
        Task<List<DVDResponseDTO>> GetAllAsync();
        Task<DVDResponseDTO> GetByIdAsync(string id);
        Task <DVDResponseDTO> AddAsync(DVDRequestDTO movieDvdRequest);
        Task UpdateAsync(string id, DVDRequestDTO movieDvdRequest);
        Task DeleteAsync(string id);
    }
}
