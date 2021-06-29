using System;
using System.Configuration;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Cloud.Metrics.Client;

namespace Dashboards_DataLayer
{
    public class SkypeDAL
    {
         
       public ConnectionInfo GetConnectionString()
        {
            ConnectionInfo connectionInfo;
            string testCertificateThumbprint = ConfigurationManager.AppSettings["Thumbprint"].ToString();
            connectionInfo = new ConnectionInfo(testCertificateThumbprint, StoreLocation.CurrentUser, MdmEnvironment.Production);
            return connectionInfo;


        }
    }

}
