using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.ServiceModel;
using Contract;

namespace Host
{
    public partial class HostService : ServiceBase
    {
        private ServiceHost host;
        public HostService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            CloseHost();

            host = new ServiceHost(typeof(EresService));
            host.Open();
        }

        protected override void OnStop()
        {
            CloseHost();
        }

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
