using Kusto.Data;
using Kusto.Data.Common;
using Kusto.Data.Net.Client;
using System;
using System.Configuration;
using System.Data;
using System.Net;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;

namespace Dashboards_DataLayer
{
    public class IcmDAL
    {
        public static KustoConnectionStringBuilder GetKustoConnection()
        {

            //IConfigurationBuilder builder = new ConfigurationBuilder();
            //builder.AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json"));

            //var root = builder.Build();
            //var sampleConnectionString = root.GetConnectionString("AppId");

            KustoConnectionStringBuilder kcsb = new KustoConnectionStringBuilder(ConfigurationManager.AppSettings["KustoCluster"])
            {
                FederatedSecurity = true,
                InitialCatalog = "IcMDataWarehouse",
                ApplicationClientId = ConfigurationManager.AppSettings["ida:ClientId"],
                ApplicationKey = ConfigurationManager.AppSettings["ida:ClientSecret"],
                Authority = ConfigurationManager.AppSettings["ida:TenantId"]


            };
            return kcsb;
        }
        public static KustoConnectionStringBuilder GetKustoConnectionTest()
        {
            KustoConnectionStringBuilder kcsb = new KustoConnectionStringBuilder(ConfigurationManager.AppSettings["KustoCluster"])
            {
                FederatedSecurity = true,
                InitialCatalog = "IcMDataWarehouse",
                ApplicationClientId = ConfigurationManager.AppSettings["ida:TestClientId"],
                ApplicationKey = ConfigurationManager.AppSettings["ida:TestClientSecret"],
                Authority = ConfigurationManager.AppSettings["ida:TenantId"]


            };
            return kcsb;
        }

        public DataTable ExecuteKustoQuery(string kustoQuery)
        {
            var dataTable = new DataTable();
            var clientRequestProperties = new ClientRequestProperties() { ClientRequestId = Guid.NewGuid().ToString() };
            using (var queryProvider = KustoClientFactory.CreateCslQueryProvider(GetKustoConnection()))
            {
                try
                {
                    var query = kustoQuery;
                    using (var reader = queryProvider.ExecuteQuery(query, clientRequestProperties))
                    {
                        dataTable.Load(reader);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return dataTable;
        }

        public DataTable ExecuteKustoQueryForTest(string kustoQuery)
        {
            var dataTable = new DataTable();
            var clientRequestProperties = new ClientRequestProperties() { ClientRequestId = Guid.NewGuid().ToString() };
            using (var queryProvider = KustoClientFactory.CreateCslQueryProvider(GetKustoConnectionTest()))
            {
                try
                {
                    var query = kustoQuery;
                    using (var reader = queryProvider.ExecuteQuery(query, clientRequestProperties))
                    {
                        dataTable.Load(reader);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return dataTable;
        }

        public HttpWebResponse GetIncident(string filter, string thumbprint)
        {
            try
            {
                string searhUrl = string.Format("https://portal.microsofticm.com/api/cert/incidents?" + filter); 

                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(searhUrl);

                request.ClientCertificates.Add(GetCert(thumbprint));
                request.Method = "GET";

                const SslProtocols _Tls12 = (SslProtocols)0x00000C00;
                const SecurityProtocolType Tls12 = (SecurityProtocolType)_Tls12;
                ServicePointManager.SecurityProtocol = Tls12;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;

                return (HttpWebResponse)request.GetResponse();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static X509Certificate2 GetCert(string certId)
        {
            if (string.IsNullOrEmpty(certId))
            {
                return null;
            }
            return GetCert(certId, StoreLocation.CurrentUser) ?? GetCert(certId, StoreLocation.LocalMachine);
        }
        private static X509Certificate2 GetCert(string certId, StoreLocation location)
        {
            X509Certificate2 result = null;
            X509Store certStore;
            certStore = new X509Store(StoreName.My, location);
            certStore.Open(OpenFlags.OpenExistingOnly | OpenFlags.ReadOnly);

            try
            {
                X509Certificate2Collection set = certStore.Certificates.Find(
                X509FindType.FindByThumbprint, certId, true);
                if (set.Count > 0 && set[0] != null && set[0].HasPrivateKey)
                {
                    result = set[0];
                }
            }
            finally
            {
                certStore.Close();
            }
            return result;
        }
    }
}
