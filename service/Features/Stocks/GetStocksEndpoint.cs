using Microsoft.EntityFrameworkCore;
using Stoxolio.Service.BuildingBlocks.Common;
using Stoxolio.Service.BuildingBlocks.CQRS;
using Stoxolio.Service.Data;
using Stoxolio.Service.DTOs;
using Stoxolio.Service.Mappings;

namespace Stoxolio.Service.Features.Stocks;

public sealed record GetStocksQuery : IQuery<GetStocksResponse>;

public sealed record GetStocksResponse(List<StockDto> Stocks);

public class GetStocksHandler(StoxolioDbContext context) : IQueryHandler<GetStocksQuery, GetStocksResponse>
{
    public async Task<Result<GetStocksResponse>> Handle(GetStocksQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var stocks = await context.Stocks.ToListAsync(cancellationToken);

            return Result.Success(new GetStocksResponse(stocks.Select(s => s.ToDto()).ToList()));
        }
        catch
        {
            return Result.Failure<GetStocksResponse>(
                new Error(
                    "GetStocks.Failed",
                    "Failed trying to get stocks.",
                    ErrorType.Problem));
        }
    }
}