using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Collections;
using System.Data;
using Newtonsoft.Json;
using Dashboards_BusinessLayer.Models;
using Dashboards_BusinessLayer.Handlers.Interfaces;
using System.Threading.Tasks;
using System.Linq;
using System.Configuration;

namespace Dashboards_BusinessLayer.Handlers
{
    public class BvdHandler : IBvdHandler
    {
        public BvdHandler()
        {
        }
        string personalaccesstoken = ConfigurationManager.AppSettings["AccessToken"].ToString();
        public List<Pipeline.Value> GetBVDPipelines()
        {
            try
            {
                List<Pipeline.Value> pipelineData = new List<Pipeline.Value>();
                var msg = Task.Run(async () => await GetBVD()).Result;
                for (int i = 0; i < msg.Count(); i++)
                {
                    var data = msg.ToArray()[i].value.OrderByDescending(a => a.lastChangedDate).First();
                    pipelineData.Add(data);
                }
                return pipelineData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ReleaseDetail.Value> GetReleasesByBuild(int bldDefId)
        {
            try
            {
                var msg = Task.Run(async () => await GetReleasesById(bldDefId)).Result;
                return msg.ToList();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private async Task<IEnumerable<Pipeline.Root>> GetBVD()
        {
            try
            {
                //string jsondataclientBuilds = null;
                List<Pipeline.Root> data = new List<Pipeline.Root>();
                DataSet dsReleaseData = new DataSet();
                ArrayList al = new ArrayList();
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Accept.Add(
                        new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes(string.Format("{0}:{1}", "", personalaccesstoken))));
                al.Add(7739);//EERS Release ADORM
                al.Add(7614);//BVD Release ADORM 
                al.Add(7803);//BVD E2E Rolling Build
                al.Add(1922);//BVD Experimental
                al.Add(8677);//EERS TST Experimental
                al.Add(8955);//EERS Rolling Build
                al.Add(10449);//Testing BVD

                foreach (var i in al)
                {
                    var resultclientBuilds = await client.GetAsync("https://dev.azure.com/skype/SBS/_apis/build/builds?definitions=" + i.ToString() + "&api-version=6.0", HttpCompletionOption.ResponseContentRead);
                    var jsondataclientBuilds = resultclientBuilds.Content.ReadAsStringAsync().Result;
                    var BuildDetail = JsonConvert.DeserializeObject<Pipeline.Root>(jsondataclientBuilds);
                    data.Add(BuildDetail);
                }
                return data;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Deployments.Value> GetReleaseData(List<ReleaseDetail.Value> refinedList)
        {
            try
            {
                List<Deployments.Value> values = new List<Deployments.Value>();
                var data = Task.Run(async () => await GetBvdReleases(refinedList)).Result;

                foreach (var item in data)
                {
                    if (item.value.Count > 0)
                    {
                        var rec = item.value.OrderByDescending(a => a.completedOn).FirstOrDefault();
                        rec.queuedOn = GetStartDate(rec.release.id, item.value);
                        values.Add(rec);
                    }
                }
                return values;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private DateTime GetStartDate(int id, List<Deployments.Value> data)
        {
            try
            {
                return data.Where(a => a.release.id.Equals(id)).OrderBy(a => a.queuedOn).First().queuedOn;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private async Task<IEnumerable<Deployments.Root>> GetBvdReleases(List<ReleaseDetail.Value> refinedList)
        {
            try
            {
                List<Deployments.Root> releaseData = new List<Deployments.Root>();
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Accept.Add(
                        new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(string.Format("{0}:{1}", "", personalaccesstoken))));
                //Get Releases
                if (refinedList != null && refinedList.Count > 0)
                {
                    foreach (var item in refinedList)
                    {
                        // To get deployments status 
                        var resultclientDep = await client.GetAsync("https://vsrm.dev.azure.com/skype/SBS/_apis/release/deployments?definitionId=" + item.id + "&api-version=6.0", HttpCompletionOption.ResponseContentRead);
                        var jsondataclientDep = resultclientDep.Content.ReadAsStringAsync().Result;
                        var deploymentsDetail = JsonConvert.DeserializeObject<Deployments.Root>(jsondataclientDep);
                        releaseData.Add(deploymentsDetail);
                    }
                }
                return releaseData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async Task<IEnumerable<ReleaseDetail.Value>> GetReleasesById(int id)
        {
            try
            {
                List<Deployments.Root> releaseData = new List<Deployments.Root>();
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Accept.Add(
                        new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(string.Format("{0}:{1}", "", personalaccesstoken))));

                // To get deployments status 
                var resultclientAlert = await client.GetAsync("https://vsrm.dev.azure.com/skype/SBS/_apis/release/definitions?api-version=6.0", HttpCompletionOption.ResponseContentRead);
                var jsondataclientAlert = resultclientAlert.Content.ReadAsStringAsync();
                var ReleaseDetail = JsonConvert.DeserializeObject<ReleaseDetail.Root>(jsondataclientAlert.Result);
                return GetConditionByRelese(id, ReleaseDetail);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private List<ReleaseDetail.Value> GetConditionByRelese(int id, ReleaseDetail.Root root)
        {
            List<ReleaseDetail.Value> data = new List<ReleaseDetail.Value>();
            //Service :EERS Release ADORM
            if (id == 7739)
            {
                data = root.value.FindAll(s => (Convert.ToString(s.name).Contains("EERS PROD Public") || Convert.ToString(s.name).Contains("EERS PROD AzSc")) && !Convert.ToString(s.name).ToUpper().Contains("DEPRECATED") && !Convert.ToString(s.name).ToUpper().Contains("TEST"));
            }
            else if (id == 7614)
            {
                //BVD Release ADORM
                data = root.value.FindAll(s => (Convert.ToString(s.name).Contains("BVD PROD AzSc") || Convert.ToString(s.name).Contains("BVD PROD Public") || Convert.ToString(s.name).Contains("BVD DOD") || Convert.ToString(s.name).Contains("BVD GCCH")) && (!Convert.ToString(s.name).ToUpper().Contains("DEPRECATED")));
            }
            else if (id == 1922)
            {
                //BVD Experimental
                data = root.value.FindAll(s => Convert.ToString(s.name).Contains("Experimental.Release") && Convert.ToString(s.name).Contains("BVD") && !Convert.ToString(s.name).ToUpper().Contains("DEPRECATED") && !Convert.ToString(s.name).ToUpper().Contains("TEST"));
            }
            else if (id == 8677)
            {
                //EERS TST Experimental
                data = root.value.FindAll(s => Convert.ToString(s.name).Contains("EERS") && Convert.ToString(s.name).Contains("Experimental.Release") && !Convert.ToString(s.name).ToUpper().Contains("DEPRECATED") && !Convert.ToString(s.name).ToUpper().Contains("TEST"));
            }
            else if (id == 7803)
            {
                //BVD E2E Rolling
                data = root.value.FindAll(s => Convert.ToString(s.name).Contains("RollingBuild.Release") && !Convert.ToString(s.name).ToUpper().Contains("DEPRECATED") && !Convert.ToString(s.name).ToUpper().Contains("TEST"));
            }
            else if (id == 8955)
            {
                //EERS Rolling Build
                data = root.value.FindAll(s => Convert.ToString(s.name).Contains("EERS") && Convert.ToString(s.name).Contains("Rolling") && !Convert.ToString(s.name).ToUpper().Contains("DEPRECATED") && !Convert.ToString(s.name).ToUpper().Contains("TEST"));
            }
            else if (id == 10449)
            {
                //Testing BVD
                data = root.value.FindAll(s => Convert.ToString(s.name).Contains("BVD") && !Convert.ToString(s.name).ToUpper().Contains("DEPRECATED") && Convert.ToString(s.name).ToUpper().Contains("TEST"));
            }
            return data;
        }
    }
}
