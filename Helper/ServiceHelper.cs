using System;
using System.ServiceProcess;

namespace AMD3DConfigurator.Helper
{
    public static class ServiceHelper
    {
        #region Constants

        private const string SERVICE_NAME = "amd3dvcacheSvc";

        #endregion

        #region Methods

        public static void RestartAMDService() => RestartWindowsService(SERVICE_NAME);

        private static void RestartWindowsService(string serviceName)
        {
            ServiceController serviceController = new(serviceName);

            if (serviceController.Status.Equals(ServiceControllerStatus.Running) || (serviceController.Status.Equals(ServiceControllerStatus.StartPending)))
                serviceController.Stop();

            serviceController.WaitForStatus(ServiceControllerStatus.Stopped, TimeSpan.FromSeconds(10000));

            serviceController.Start();
            serviceController.WaitForStatus(ServiceControllerStatus.Running, TimeSpan.FromSeconds(10000));
        }

        #endregion
    }
}
