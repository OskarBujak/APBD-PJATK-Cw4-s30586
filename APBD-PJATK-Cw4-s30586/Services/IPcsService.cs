using System.Collections.Generic;
using System.Threading.Tasks;

public interface IPcsService
{
    Task<IEnumerable<PcDto>> GetAllAsync();
    Task<PcDetailsDto?> GetByIdAsync(int id);
    Task<PcDto> CreateAsync(PcCreateUpdateDto dto);
    Task<bool> UpdateAsync(int id, PcCreateUpdateDto dto);
    Task<bool> DeleteAsync(int id);
}