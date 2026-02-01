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
public class StocksController : ControllerBase
{
    private readonly StoxolioDbContext _context;

    public StocksController(StoxolioDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<StockDto>>> GetStocks()
    {
        var stocks = await _context.Stocks.ToListAsync();

        var dtos = stocks.Select(s => new StockDto
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
        }).ToList();

        return Ok(dtos);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<StockDto>> GetStock(int id)
    {
        var stock = await _context.Stocks.FindAsync(id);

        if (stock == null)
            return NotFound();

        var dto = new StockDto
        {
            Id = stock.Id,
            Name = stock.Name,
            Ticker = stock.Ticker,
            Exchange = stock.Exchange,
            Sri = stock.Sri,
            Shares = stock.Shares,
            Price = stock.Price,
            Invest = stock.Invest,
            CategoryId = stock.CategoryId,
            PrevPrice = stock.PrevPrice,
            Value = stock.Value,
            PriceChange = stock.PriceChange,
            ValueChange = stock.ValueChange
        };

        return Ok(dto);
    }

    [HttpPost]
    public async Task<ActionResult<StockDto>> CreateStock([FromBody] StockDto stockDto)
    {
        var stock = new Stock
        {
            Name = stockDto.Name,
            Ticker = stockDto.Ticker,
            Exchange = stockDto.Exchange,
            Sri = stockDto.Sri,
            Shares = stockDto.Shares,
            Price = stockDto.Price,
            Invest = stockDto.Invest,
            CategoryId = stockDto.CategoryId,
            PrevPrice = stockDto.PrevPrice
        };

        _context.Stocks.Add(stock);
        await _context.SaveChangesAsync();

        stockDto.Id = stock.Id;
        return CreatedAtAction(nameof(GetStock), new { id = stock.Id }, stockDto);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateStock(int id, [FromBody] StockDto stockDto)
    {
        var stock = await _context.Stocks.FindAsync(id);

        if (stock == null)
            return NotFound();

        stock.Name = stockDto.Name;
        stock.Ticker = stockDto.Ticker;
        stock.Exchange = stockDto.Exchange;
        stock.Sri = stockDto.Sri;
        stock.Shares = stockDto.Shares;
        stock.Price = stockDto.Price;
        stock.Invest = stockDto.Invest;
        stock.CategoryId = stockDto.CategoryId;
        stock.PrevPrice = stockDto.PrevPrice;

        _context.Stocks.Update(stock);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteStock(int id)
    {
        var stock = await _context.Stocks.FindAsync(id);

        if (stock == null)
            return NotFound();

        _context.Stocks.Remove(stock);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
