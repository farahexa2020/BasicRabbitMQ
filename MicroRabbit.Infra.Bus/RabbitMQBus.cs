using MediatR;
using MicroRabbit.Domain.Core.Bus;
using MicroRabbit.Domain.Core.Commands;
using MicroRabbit.Domain.Core.Events;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MicroRabbit.Domain.Core.Bus.IEventHandler;

namespace MicroRabbit.Infra.Bus
{
    public class RabbitMQBus : IEventBus
    {
        private readonly IMediator _mediator;
        public readonly Dictionary<string, List<Type>> _handlers;
        public readonly List<Type> _eventTypes;
        private readonly IServiceScopeFactory _serviceScopedFactory;

        public RabbitMQBus(IMediator mediator, IServiceScopeFactory serviceScopeFactory)
        {
            this._mediator = mediator;
            this._serviceScopedFactory = serviceScopeFactory;
            this._handlers = new Dictionary<string, List<Type>>();
            this._eventTypes = new List<Type>();
        }

        public Task SendCommand<T>(T command) where T : Command
        {
            return this._mediator.Send(command);
        }

        public void Publish<T>(T @event) where T : Event
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                var eventName = @event.GetType().Name;

                channel.QueueDeclare(eventName, false, false, false, null);

                var message = JsonConvert.SerializeObject(@event);
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish("", eventName, null, body);
            }
        }

        public void Subscribe<T, TH>()
            where T : Event
            where TH : IEventHandler<T>
        {
            var eventName = typeof(T).Name;
            var handlerType = typeof(TH);

            if (!this._eventTypes.Contains(typeof(T)))
            {
                this._eventTypes.Add(typeof(T));
            }

            if (!this._handlers.ContainsKey(eventName))
            {
                this._handlers.Add(eventName, new List<Type>());
            }

            if (this._handlers[eventName].Any(s => s.GetType() == handlerType))
            {
                throw new ArgumentException(
                    $"Handler type {handlerType.Name} already is registered for '{eventName}'", nameof(handlerType));
            }

            this._handlers[eventName].Add(handlerType);

            StartBasicConsume<T>();
        }

        private void StartBasicConsume<T>() where T : Event
        {
            var factory = new ConnectionFactory()
            {
                HostName = "localhost",
                DispatchConsumersAsync = true
            };

            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            var eventName = typeof(T).Name;

            channel.QueueDeclare(eventName, false, false, false, null);

            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.Received += Consumer_Received;

            channel.BasicConsume(eventName, true, consumer);
        }

        private async Task Consumer_Received(object sender, BasicDeliverEventArgs @event)
        {
            var eventName = @event.RoutingKey;
            var message = Encoding.UTF8.GetString(@event.Body);

            try
            {
                await ProcessEvent(eventName, message).ConfigureAwait(false);
            }
            catch (Exception ex)
            {

            }
        }

        private async Task ProcessEvent(string eventName, string message)
        {
            if (this._handlers.ContainsKey(eventName))
            {
                using (var scope = this._serviceScopedFactory.CreateScope())
                {
                    var subscriptions = this._handlers[eventName];
                    foreach (var subscription in subscriptions)
                    {
                        var handler = scope.ServiceProvider.GetService(subscription);
                        if (handler == null)
                        {
                            continue;
                        }

                        var eventType = this._eventTypes.SingleOrDefault(t => t.Name == eventName);
                        var @event = JsonConvert.DeserializeObject(message, eventType);
                        var concreteType = typeof(IEventHandler<>).MakeGenericType(eventType);

                        await (Task)concreteType.GetMethod("Handle").Invoke(handler, new object[] { @event });
                    }
                }
            }
        }
    }
}
