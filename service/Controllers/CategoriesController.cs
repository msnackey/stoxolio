using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stoxolio.Service.Data;
using Stoxolio.Service.DTOs;
using Stoxolio.Service.Models;

namespace Stoxolio.Service.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CategoriesController : ControllerBase
{
    private readonly StoxolioDbContext _context;

    public CategoriesController(StoxolioDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoryDto>>> GetCategories()
    {
        var categories = await _context.Categories
            .Include(c => c.Stocks)
            .ToListAsync();

        var dtos = categories.Select(c => new CategoryDto
        {
            Id = c.Id,
            Name = c.Name,
            Target = c.Target,
            Stocks = c.Stocks.Select(s => new StockDto
            {
                Id = s.Id,
                Name = s.Name,
                Ticker = s.Ticker,
                Exchange = s.Exchange,
                Sri = s.Sri,
                Shares = s.Shares,
                Price = s.Price,
                Invest = s.Invest,
                CategoryId = s.CategoryId,
                PrevPrice = s.PrevPrice,
                Value = s.Value,
                PriceChange = s.PriceChange,
                ValueChange = s.ValueChange
            }).ToList()
        }).ToList();

        return Ok(dtos);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CategoryDto>> GetCategory(int id)
    {
        var category = await _context.Categories
            .Include(c => c.Stocks)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (category == null)
            return NotFound();

        var dto = new CategoryDto
        {
            Id = category.Id,
            Name = category.Name,
            Target = category.Target,
            Stocks = category.Stocks.Select(s => new StockDto
            {
                Id = s.Id,
                Name = s.Name,
                Ticker = s.Ticker,
                Exchange = s.Exchange,
                Sri = s.Sri,
                Shares = s.Shares,
                Price = s.Price,
                Invest = s.Invest,
                CategoryId = s.CategoryId,
                PrevPrice = s.PrevPrice,
                Value = s.Value,
                PriceChange = s.PriceChange,
                ValueChange = s.ValueChange
            }).ToList()
        };

        return Ok(dto);
    }

    [HttpPost]
    public async Task<ActionResult<CategoryDto>> CreateCategory([FromBody] CategoryDto categoryDto)
    {
        var category = new Category
        {
            Name = categoryDto.Name,
            Target = categoryDto.Target
        };

        _context.Categories.Add(category);
        await _context.SaveChangesAsync();

        categoryDto.Id = category.Id;
        return CreatedAtAction(nameof(GetCategory), new { id = category.Id }, categoryDto);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCategory(int id, [FromBody] CategoryDto categoryDto)
    {
        var category = await _context.Categories.FindAsync(id);

        if (category == null)
            return NotFound();

        category.Name = categoryDto.Name;
        category.Target = categoryDto.Target;

        _context.Categories.Update(category);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        var category = await _context.Categories.FindAsync(id);

        if (category == null)
            return NotFound();

        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
