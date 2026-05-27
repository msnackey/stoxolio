using Stoxolio.Service.BuildingBlocks.Common;

namespace Stoxolio.Service.BuildingBlocks.CQRS;

public interface IQueryHandler<in TQuery, TResponse>
    where TQuery : IQuery<TResponse>
{
    Task<Result<TResponse>> Handle(TQuery request, CancellationToken cancellationToken);
}
