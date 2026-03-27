using System.Diagnostics;
using System.Reflection;
using Ardalis.GuardClauses;
using Mediator;
using Microsoft.Extensions.Logging;

namespace Ardalis.Specification.EntityFrameworkCore.SharedKernel;

/// <summary>
/// Adds logging for all requests in Mediator pipeline.
/// Configure by adding the service with a scoped lifetime
/// 
/// Example for Autofac:
/// builder
///   .RegisterType&lt;Mediator&gt;()
///   .As&lt;IMediator&gt;()
///   .InstancePerLifetimeScope();
///
/// builder
///   .RegisterGeneric(typeof(LoggingBehavior&lt;,&gt;))
///      .As(typeof(IPipelineBehavior&lt;,&gt;))
///   .InstancePerLifetimeScope();
///
/// </summary>
/// <typeparam name="TMessage"></typeparam>
/// <typeparam name="TResponse"></typeparam>
public class LoggingBehavior<TMessage, TResponse> : IPipelineBehavior<TMessage, TResponse>
  where TMessage : IRequest<TResponse>
{
  private readonly ILogger<IMediator> _logger;

  public LoggingBehavior(ILogger<IMediator> logger)
  {
    _logger = logger;
  }

  public async ValueTask<TResponse> Handle(TMessage message, MessageHandlerDelegate<TMessage, TResponse> next, CancellationToken cancellationToken)
  {
    Guard.Against.Null(message);
    if (_logger.IsEnabled(LogLevel.Information))
    {
      _logger.LogInformation("Handling {RequestName}", typeof(TMessage).Name);

      // Reflection! Could be a performance concern
      //var props = new List<PropertyInfo>(typeof(message).GetProperties());
      //foreach (PropertyInfo prop in props)
      //{
      //  object? propValue = prop?.GetValue(message, null);
      //  _logger.LogInformation("Property {Property} : {@Value}", prop?.Name, propValue);
      //}
    }
    var sw = Stopwatch.StartNew();

    var response = await next(message, cancellationToken);

    _logger.LogInformation("Handled {RequestName} with {Response} in {ms} ms", typeof(TMessage).Name, response, sw.ElapsedMilliseconds);
    sw.Stop();
    return response;
  }

}

