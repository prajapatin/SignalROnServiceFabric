using ABB.Ability.NotificationDispatcher.Core;
using Microsoft.ServiceBus.Messaging;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using System;
using System.Collections.Generic;
using System.Fabric;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

namespace ABB.Ability.NotificationDispatcher
{
    /// <summary>
    /// An instance of this class is created for each service instance by the Service Fabric runtime.
    /// </summary>
    internal sealed class NotificationDispatcher : StatelessService
    {
        public NotificationDispatcher(StatelessServiceContext context)
            : base(context)
        { }

        /// <summary>
        /// Optional override to create listeners (e.g., TCP, HTTP) for this service replica to handle client or user requests.
        /// </summary>
        /// <returns>A collection of listeners.</returns>
        protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
        {
            return new ServiceInstanceListener[0];
        }

        /// <summary>
        /// This is the main entry point for your service instance.
        /// </summary>
        /// <param name="cancellationToken">Canceled when Service Fabric needs to shut down this service instance.</param>
        protected override async Task RunAsync(CancellationToken cancellationToken)
        {
           
            long iterations = 0;

            while (true)
            {
                cancellationToken.ThrowIfCancellationRequested();

                ServiceEventSource.Current.ServiceMessage(this.Context, "Working-{0}", ++iterations);

                var connectionString = DispatcherConfiguration.ServiceBusConnectionString;
                var topicName = DispatcherConfiguration.ServiceBusNotificationTopic;

                var client = TopicClient.CreateFromConnectionString(connectionString, topicName);
                var message = new BrokeredMessage("This is a test message generated on " + DateTime.Now.ToUniversalTime().ToString("MM/dd/yyyy HH:mm:ss.fff",
                                CultureInfo.InvariantCulture));
                client.Send(message);

                await Task.Delay(TimeSpan.FromSeconds(15), cancellationToken);
            }
        }
    }
}
