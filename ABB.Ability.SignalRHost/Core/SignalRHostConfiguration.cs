using System;
using System.Fabric;

namespace ABB.Ability.SignalRHost.Core
{
    public static class SignalRHostConfiguration
    {
        public static bool UseScaleout { get; internal set; }
        public static string ServiceBusConnectionString { get; internal set; }
        public static string EncryptionPassword { get; internal set; }
        public static string ServiceBusBackplaneTopic { get; internal set; }
        public static string ServiceBusNotificationTopic { get; internal set; }
        public static string TopicSubscriptionName { get; internal set; }
        

        static SignalRHostConfiguration()
        {
            var codeContext = FabricRuntime.GetActivationContext();
            if (codeContext == null)
            {
                // sanity check
                throw new ApplicationException("CodePackageActivationContext is null");
            }

            ConfigurationPackage configurationPackage = codeContext.GetConfigurationPackageObject("Config");

            if (configurationPackage.Settings?.Sections == null || !configurationPackage.Settings.Sections.Contains("SignalRScaleout"))
            {
                return;
            }

            var param = configurationPackage.Settings.Sections["SignalRScaleout"].Parameters;

            if (param.Contains("UseScaleout"))
            {
                UseScaleout = bool.Parse(param["UseScaleout"].Value);
            }
            if (param.Contains("ServiceBusConnectionString"))
            {
                ServiceBusConnectionString = param["ServiceBusConnectionString"].Value;
            }
            if (param.Contains("ServiceBusBackplaneTopic"))
            {
                ServiceBusBackplaneTopic = param["ServiceBusBackplaneTopic"].Value;
            }
            if (param.Contains("ServiceBusNotificationTopic"))
            {
                ServiceBusNotificationTopic = param["ServiceBusNotificationTopic"].Value;
            }
            if (param.Contains("TopicSubscriptionName"))
            {
                TopicSubscriptionName = param["TopicSubscriptionName"].Value;
            }
            if (param.Contains("EncryptionPassword"))
            {
                EncryptionPassword = param["EncryptionPassword"].Value;
            }
        }
    }
}
