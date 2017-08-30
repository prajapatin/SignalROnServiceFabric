using System;
using System.Fabric;

namespace ABB.Ability.NotificationDispatcher.Core
{
    internal static class DispatcherConfiguration
    {
        public static string ServiceBusConnectionString { get; internal set; }
        public static string ServiceBusNotificationTopic { get; internal set; }

        static DispatcherConfiguration()
        {
            var codeContext = FabricRuntime.GetActivationContext();
            if (codeContext == null)
            {
                // sanity check
                throw new ApplicationException("CodePackageActivationContext is null");
            }

            ConfigurationPackage configurationPackage = codeContext.GetConfigurationPackageObject("Config");

            if (configurationPackage.Settings?.Sections == null || !configurationPackage.Settings.Sections.Contains("DispatcherConfig"))
            {
                return;
            }

            var param = configurationPackage.Settings.Sections["DispatcherConfig"].Parameters;

            if (param.Contains("ServiceBusConnectionString"))
            {
                ServiceBusConnectionString = param["ServiceBusConnectionString"].Value;
            }
            if (param.Contains("ServiceBusNotificationTopic"))
            {
                ServiceBusNotificationTopic = param["ServiceBusNotificationTopic"].Value;
            }
            
        }
    }
}
