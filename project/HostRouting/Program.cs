using System;
using System.ServiceModel;

namespace HostCache
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceHost svcHost = null;
            try
            {
                svcHost = new ServiceHost(typeof(Routing.Routing));
                try
                {
                    /*
                    ServiceMetadataBehavior smb = new ServiceMetadataBehavior();
                    smb.HttpGetEnabled = true;
                    smb.MetadataExporter.PolicyVersion = PolicyVersion.Policy15;
                    svcHost.Description.Behaviors.Add(smb);

                    WSHttpBinding webHttpBinding = new WSHttpBinding();
                    BasicHttpBinding basicHttpBinding = new BasicHttpBinding();
                    svcHost.AddServiceEndpoint(typeof(Routing.IRouting), webHttpBinding, baseaddr + "rest");
                    svcHost.AddServiceEndpoint(typeof(Routing.IRouting), basicHttpBinding, baseaddr + "soap");*/
                    svcHost.Open();

                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
                Console.WriteLine("\n\nService is Running  at following address");
                Console.WriteLine("\nhttp://localhost:8734/Design_Time_Addresses/Routing/rest");
            }
            catch (Exception eX)
            {
                svcHost = null;
                Console.WriteLine("Service can not be started \n\nError Message [" + eX.Message + "]");
            }
            if (svcHost != null)
            {
                Console.WriteLine("\nPress any key to close the Service");
                Console.ReadKey();
                svcHost.Close();
                svcHost = null;
            }
        }
    }
}