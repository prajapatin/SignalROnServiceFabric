using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Threading.Tasks;

namespace ABB.Ability.NotificationListener.Core
{
    internal static class NotificationListenerConfiguration
    {
        public static string SignalRHost { get; internal set; }
      
        static NotificationListenerConfiguration()
        {
            var codeContext = FabricRuntime.GetActivationContext();
            if (codeContext == null)
            {
                // sanity check
                throw new ApplicationException("CodePackageActivationContext is null");
            }

            ConfigurationPackage configurationPackage = codeContext.GetConfigurationPackageObject("Config");

            if (configurationPackage.Settings?.Sections == null || !configurationPackage.Settings.Sections.Contains("NotificationListenerConfig"))
            {
                return;
            }

            var param = configurationPackage.Settings.Sections["NotificationListenerConfig"].Parameters;

            if (param.Contains("SignalRHost"))
            {
                SignalRHost = param["SignalRHost"].Value;
            }

        }
    }
}
