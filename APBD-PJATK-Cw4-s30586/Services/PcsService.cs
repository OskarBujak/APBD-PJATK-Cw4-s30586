using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

public class PcsService : IPcsService
{
    private readonly AppDbContext _context;

    public PcsService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<PcDto>> GetAllAsync()
    {
        return await _context.PCs
            .Select(p => new PcDto
            {
                Id = p.Id,
                Name = p.Name,
                Weight = p.Weight,
                Warranty = p.Warranty,
                CreatedAt = p.CreatedAt,
                Stock = p.Stock
            }).ToListAsync();
    }

    public async Task<PcDetailsDto?> GetByIdAsync(int id)
    {
        var pc = await _context.PCs
            .Include(p => p.PCComponents)
                .ThenInclude(pc => pc.Component)
                    .ThenInclude(c => c.ComponentManufacturer)
            .Include(p => p.PCComponents)
                .ThenInclude(pc => pc.Component)
                    .ThenInclude(c => c.ComponentType)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (pc == null) return null;

        return new PcDetailsDto
        {
            Id = pc.Id,
            Name = pc.Name,
            Weight = pc.Weight,
            Warranty = pc.Warranty,
            CreatedAt = pc.CreatedAt,
            Stock = pc.Stock,
            Components = pc.PCComponents.Select(c => new PcComponentDto
            {
                Amount = c.Amount,
                Component = new ComponentDto
                {
                    Code = c.Component.Code,
                    Name = c.Component.Name,
                    Description = c.Component.Description,
                    Manufacturer = new ManufacturerDto
                    {
                        Id = c.Component.ComponentManufacturer.Id,
                        Abbreviation = c.Component.ComponentManufacturer.Abbreviation,
                        FullName = c.Component.ComponentManufacturer.FullName,
                        FoundationDate = c.Component.ComponentManufacturer.FoundationDate
                    },
                    Type = new TypeDto
                    {
                        Id = c.Component.ComponentType.Id,
                        Abbreviation = c.Component.ComponentType.Abbreviation,
                        Name = c.Component.ComponentType.Name
                    }
                }
            }).ToList()
        };
    }

    public async Task<PcDto> CreateAsync(PcCreateUpdateDto dto)
    {
        var pc = new PC
        {
            Name = dto.Name,
            Weight = dto.Weight,
            Warranty = dto.Warranty,
            CreatedAt = dto.CreatedAt,
            Stock = dto.Stock
        };

        _context.PCs.Add(pc);
        await _context.SaveChangesAsync();

        return new PcDto
        {
            Id = pc.Id,
            Name = pc.Name,
            Weight = pc.Weight,
            Warranty = pc.Warranty,
            CreatedAt = pc.CreatedAt,
            Stock = pc.Stock
        };
    }

    public async Task<bool> UpdateAsync(int id, PcCreateUpdateDto dto)
    {
        var pc = await _context.PCs.FindAsync(id);
        if (pc == null) return false;

        pc.Name = dto.Name;
        pc.Weight = dto.Weight;
        pc.Warranty = dto.Warranty;
        pc.CreatedAt = dto.CreatedAt;
        pc.Stock = dto.Stock;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var pc = await _context.PCs.FindAsync(id);
        if (pc == null) return false;

        _context.PCs.Remove(pc);
        await _context.SaveChangesAsync();
        return true;
    }
}