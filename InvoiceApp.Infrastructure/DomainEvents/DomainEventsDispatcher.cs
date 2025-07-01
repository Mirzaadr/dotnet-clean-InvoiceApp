using System.Collections.Concurrent;
using InvoiceApp.Application.Invoices.Send;
using InvoiceApp.Domain.Commons.Interfaces;
using InvoiceApp.Domain.Invoices;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace InvoiceApp.Infrastructure.DomainEvents;

internal sealed class DomainEventsDispatcher : IDomainEventsDispatcher
{
    private static readonly ConcurrentDictionary<Type, Type> HandlerTypeDictionary = new();
    private static readonly ConcurrentDictionary<Type, Type> WrapperTypeDictionary = new();
    private readonly IMediator _mediator;

    public DomainEventsDispatcher(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    public async Task DispatchAsync(IEnumerable<IDomainEvent> domainEvents, CancellationToken cancellationToken = default)
    {
        foreach (var domainEvent in domainEvents)
        {
            // Map domain event to INotification wrapper
            var notification = ToNotification(domainEvent);
            if (notification != null)
            {
                await _mediator.Publish(notification);
            }
        }
    }

    private INotification? ToNotification(IDomainEvent domainEvent)
    {
        // Use reflection or a switch pattern here
        return domainEvent switch
        {
            InvoiceSentEvent e => new InvoiceSentNotification(e),
            _ => null
        };
    }
}
