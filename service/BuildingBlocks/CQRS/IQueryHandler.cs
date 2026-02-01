namespace Stoxolio.Service.BuildingBlocks.CQRS;

public interface IQueryHandler<in TQuery, TResponse>
    where TQuery : IQuery<TResponse>
{
    Task<TResponse?> Handle(TQuery request, CancellationToken cancellationToken);
}
