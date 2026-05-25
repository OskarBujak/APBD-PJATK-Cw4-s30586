using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class PcsController : ControllerBase
{
    private readonly IPcsService _pcsService;

    public PcsController(IPcsService pcsService)
    {
        _pcsService = pcsService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var pcs = await _pcsService.GetAllAsync();
        return Ok(pcs);
    }

    [HttpGet("{id}/components")]
    public async Task<IActionResult> GetComponents(int id)
    {
        var pc = await _pcsService.GetByIdAsync(id);
        
        if (pc == null)
        {
            return NotFound();
        }
        
        return Ok(pc);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] PcCreateUpdateDto dto)
    {
        var createdPc = await _pcsService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetComponents), new { id = createdPc.Id }, createdPc);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] PcCreateUpdateDto dto)
    {
        var isUpdated = await _pcsService.UpdateAsync(id, dto);
        
        if (!isUpdated)
        {
            return NotFound();
        }
        
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var isDeleted = await _pcsService.DeleteAsync(id);
        
        if (!isDeleted)
        {
            return NotFound();
        }
        
        return NoContent();
    }
}