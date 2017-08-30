using ABB.Ability.SignalRHost.Core;
using ABB.Ability.SignalRHost.Hubs;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Cors;
using Microsoft.ServiceBus.Messaging;
using Owin;
using Owin.Security.AesDataProtectorProvider;
using System;

namespace ABB.Ability.SignalRHost
{
    public static class Startup
    {
        public static void ConfigureApp(IAppBuilder app)
        {
            ConfigureCors(app);
            ConfigureSignalR(app);
        }

        private static void ConfigureCors(IAppBuilder app)
        {
             app.UseCors(CorsOptions.AllowAll);
        }

        private static void ConfigureSignalR(IAppBuilder app)
        {
            app.UseAesDataProtectorProvider(SignalRHostConfiguration.EncryptionPassword);

            if (SignalRHostConfiguration.UseScaleout)
            {
                var serviceBusConfig = new ServiceBusScaleoutConfiguration(SignalRHostConfiguration.ServiceBusConnectionString, 
                    SignalRHostConfiguration.ServiceBusBackplaneTopic);

                GlobalHost.DependencyResolver.UseServiceBus(serviceBusConfig);
                app.MapSignalR();
            }

            var configuration = new HubConfiguration { EnableDetailedErrors = true, EnableJavaScriptProxies = false };
            app.MapSignalR(configuration);

            var connectionString = SignalRHostConfiguration.ServiceBusConnectionString;
            var topicName = SignalRHostConfiguration.ServiceBusNotificationTopic;
            var topicSubscriptionName = SignalRHostConfiguration.TopicSubscriptionName;

            var client = SubscriptionClient.CreateFromConnectionString(connectionString, topicName, topicSubscriptionName);

            client.OnMessage(message =>
            {
                var notificationHubContext = GlobalHost.ConnectionManager.GetHubContext<AbilityNotificationHub>();
                notificationHubContext.Clients.All.showNotificationInPage(message.GetBody<String>());
            });
        }
    }
}
