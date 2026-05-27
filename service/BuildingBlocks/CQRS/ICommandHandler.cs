using Stoxolio.Service.BuildingBlocks.Common;

namespace Stoxolio.Service.BuildingBlocks.CQRS;

public interface ICommandHandler<in TCommand, TResponse>
    where TCommand : ICommand<TResponse>
{
    Task<Result<TResponse>> Handle(TCommand request, CancellationToken cancellationToken);
}
