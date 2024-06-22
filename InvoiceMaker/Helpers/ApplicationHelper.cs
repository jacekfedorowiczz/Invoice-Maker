using Castle.Windsor;
using Castle.Windsor.Installer;

namespace InvoiceMaker.Domain.Helpers
{
    public static class ApplicationHelper
    {
        public static WindsorContainer Container { get; set; }

        public static void InitializeDependencies()
        {
            Container = new WindsorContainer();
            Container.Install(FromAssembly.InThisApplication());
            Console.WriteLine("Zarejestrowano zależności!");
        }
    }
}
