using System.ServiceModel;
using System.ServiceProcess;
using Contract;

namespace Host
{
    public partial class HostService : ServiceBase
    {
        private ServiceHost host; // host

        /// <summary>
        /// Konstruktor
        /// </summary>
        public HostService()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Rozpoczynanie pracu serwisu
        /// </summary>
        /// <param name="args">Parametry uruchomienia</param>
        protected override void OnStart(string[] args)
        {
            //zamyka host jeśli istnieje
            CloseHost();

            host = new ServiceHost(typeof(EresService));
            host.Open();
        }

        /// <summary>
        /// Zatrzymywanie pracy serwisu
        /// </summary>
        protected override void OnStop()
        {
            CloseHost();
        }

        /// <summary>
        /// Wyłączanie hosta
        /// </summary>
        private void CloseHost()
        {
            if (host != null)
            {
                host.Close();
                host = null;
            }
        }
    }
}
