using Dashboards_BusinessLayer.Handlers.Interfaces;
using Dashboards_BusinessLayer.Models;
using Dashboards_DataLayer;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Dashboards_BusinessLayer.Handlers
{
    public class IC3EdgeHandler : IIc3EdgeHandler
    {
        Ic3EdgeDAL dalObj = new Ic3EdgeDAL();
        string personalaccesstoken = ConfigurationManager.AppSettings["AccessToken"].ToString();
        public IC3EdgeHandler()
        {
        }

        public bool UpdateTestCaseDetais(DataTable dt, List<TestPoints.Value> rootVal)
        {
            try
            {
                var msg = Task.Run(async () => await GetTestCases(dt, rootVal)).Result;
            }
            catch (Exception)
            {
                //throw ex;
                return false;
            }
            return true;
        }

        public List<TestPoints.Value> GetTestCaseDetais(int testPlnId, int suitId)
        {
            try
            {
                return Task.Run(async () => await GetTestDetailsForUpdate(testPlnId, suitId)).Result;
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
                var data =  Task.Run(async () => await GetIc3ReleaseData(refinedList)).Result;
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
        private async Task<List<TestPoints.Value>> GetTestDetailsForUpdate(int testPlnId, int suiteId)
        {
            try
            {
                List<TestPoints.Value> testData = new List<TestPoints.Value>();

                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(string.Format("{0}:{1}", "", personalaccesstoken))));

                //Get Test Points
                var resultclientAlert = await client.GetAsync("https://dev.azure.com/skype/SBS/_apis/testplan/Plans/" + testPlnId + "/Suites/" + suiteId + "/TestPoint?api-version=6.1-preview.2", HttpCompletionOption.ResponseContentRead);
                var jsondataclientAlert = resultclientAlert.Content.ReadAsStringAsync();
                var TPDetail = JsonConvert.DeserializeObject<TestPoints.Root>(jsondataclientAlert.Result);

                return TPDetail.value;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetTestcasesFromDB(int suitId)
        {
            try
            {
                DataTable dt = dalObj.GetUnprocessedTestData(suitId);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async Task<bool> GetTestCases(DataTable dt, List<TestPoints.Value> rootVal)
        {
            try
            {
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(string.Format("{0}:{1}", "", personalaccesstoken))));
                //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", personalaccesstoken);
                //Get Test Cases
                //var resultclientAlert = await client.GetAsync("https://dev.azure.com/skype/SBS/_apis/testplan/Plans/2288983/Suites/2459903/TestCase?api-version=6.1-preview.3", HttpCompletionOption.ResponseContentRead);
                //var jsondataclientAlert = resultclientAlert.Content.ReadAsStringAsync();
                //var TCDetail = JsonConvert.DeserializeObject<TestCases.Root>(jsondataclientAlert.Result);
                //if (TCDetail != null && TCDetail.count > 0)
                //{
                // foreach (var item in TCDetail.value)
                // {
                // }
                //}

                var resultSet = GetIdAndVal(dt, rootVal);

                if (resultSet.Count > 0)//if (TPDetail != null && TPDetail.count > 0)
                {
                    foreach (var item in resultSet) //TPDetail.value)
                    {
                        //if (item.id == 7259247)//2308975
                        //{
                        if (item.Result.ToUpper().Equals("PASS"))
                        {
                            item.Result = "passed";
                        }
                        else if (item.Result.ToUpper().Equals("NA") || item.Result.ToUpper().Equals("NOT APPLICABLE"))
                        {
                            item.Result = "notapplicable";
                        }
                        else if (item.Result.ToUpper().Equals("FAIL"))
                        {
                            item.Result = "failed";
                        }
                        else if (item.Result.ToUpper().Equals("BLOCK"))
                        {
                            item.Result = "blocked";
                        }

                        var data = new
                        {
                            outcome = item.Result //"passed"
                        };
                        // JsonContent Bodycontent = JsonContent.Create(data);
                        var json = JsonConvert.SerializeObject(data);
                        var content = new StringContent(json, Encoding.UTF8, "application/json");
                        //Update Test Points
                        var request = new HttpRequestMessage(new HttpMethod("PATCH"), "https://dev.azure.com/skype/SBS/_apis/test/Plans/" + item.TestPlanId + "/Suites/" + item.TestSuiteId + "/points/" + item.TestPointId + "?api-version=5.1");
                        request.Content = content;
                        var response1 = await client.SendAsync(request);
                        var content1 = await response1.Content.ReadAsStringAsync();
                        if (response1.IsSuccessStatusCode)
                        {
                            item.IsProcessed = true;
                        }
                        else
                        {
                            item.IsProcessed = false;
                        }
                        //}
                    }
                    DataTable table = ToDataTable(resultSet);
                    return dalObj.UpdateProcessedTestData(table,resultSet.FirstOrDefault().TestSuiteId);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return false;
        }
        private DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            //Get all the properties by using reflection   
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names  
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {

                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }

            return dataTable;
        }
        private List<TestResult> GetIdAndVal(DataTable dt, List<TestPoints.Value> rootVal)
        {
            List<TestResult> idRes = new List<TestResult>();
            foreach (DataRow item in dt.Rows)
            {
                TestResult res = new TestResult();
                res.TestId = Convert.ToInt32(item["TestCaseID"]);
                res.Result = item["Result"].ToString();
                idRes.Add(res);
            }
            List<int> testCaseId = rootVal.Select(a => a.testCaseReference.id).ToList();
            foreach (var item in idRes)
            {
                if (testCaseId.Contains(item.TestId))
                {
                    item.TestPointId = rootVal.First(a => a.testCaseReference.id.Equals(item.TestId)).id;
                    item.TestPlanId = rootVal.First(a => a.testCaseReference.id.Equals(item.TestId)).testPlan.id;
                    item.TestSuiteId = rootVal.First(a => a.testCaseReference.id.Equals(item.TestId)).testSuite.id;
                }
            }
            return idRes;
        }

        public List<Pipeline.Value> GetIc3EdgeBuildData()
        {
            try
            {
                List<Pipeline.Value> pipelineData = new List<Pipeline.Value>();
                var msg = Task.Run(async () => await GetIc3EdgeBuilds());
                for (int i = 0; i < msg.Result.Count(); i++)
                {
                    if (msg.Result.ToArray()[i].value.Count > 0)
                    {
                        var data = msg.Result.ToArray()[i].value.OrderByDescending(a => a.lastChangedDate).First();
                        pipelineData.Add(data);
                    }
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
                List<ReleaseDetail.Value> deplData = new List<ReleaseDetail.Value>();
                var msg = Task.Run(async () => await GetIc3EdgeReleasesById(bldDefId)).Result;
                return msg.ToList();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private async Task<List<Pipeline.Root>> GetIc3EdgeBuilds()
        {
            try
            {
                List<Pipeline.Root> data = new List<Pipeline.Root>();
                DataSet dsReleaseData = new DataSet();
                ArrayList al = new ArrayList();
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Accept.Add(
                        new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes(string.Format("{0}:{1}", "", personalaccesstoken))));
                al.Add(3594);//Service :SFB User BE
                al.Add(3565);//Service :CGW
                al.Add(3919);//Service :LGW
                al.Add(4126); //Service: DevicePinAuth

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

        private async Task<IEnumerable<ReleaseDetail.Value>> GetIc3EdgeReleasesById(int id)
        {
            try
            {
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Accept.Add(
                        new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(string.Format("{0}:{1}", "", personalaccesstoken))));

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
        private async Task<IEnumerable<Deployments.Root>> GetIc3ReleaseData(List<ReleaseDetail.Value> refinedList)
        {
            try
            {
                List<Deployments.Root> releaseData = new List<Deployments.Root>();
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Accept.Add(
                        new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(string.Format("{0}:{1}", "", personalaccesstoken))));
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
        private List<ReleaseDetail.Value> GetConditionByRelese(int id, ReleaseDetail.Root root)
        {
            List<ReleaseDetail.Value> data = new List<ReleaseDetail.Value>();
            //Service :SFB User BE
            if (id == 3594)
            {
                data = root.value.FindAll(s => (Convert.ToString(s.name).Contains("[Torus] Sfb UserBE Prod Pipeline")) && (!Convert.ToString(s.name).ToUpper().Contains("DEPRECATED") && !Convert.ToString(s.name).ToUpper().Contains("TEST")));
            }
            else if (id == 3565)
            {
                //Service :CGW
                data = root.value.FindAll(s => (Convert.ToString(s.name).Contains("[Torus] Sfb Conferencing Gateway Prod Pipeline")) && (!Convert.ToString(s.name).ToUpper().Contains("DEPRECATED") && !Convert.ToString(s.name).ToUpper().Contains("TEST")));
            }
            else if (id == 3919)
            {
                //Service :LGW
                data = root.value.FindAll(s => (Convert.ToString(s.name).Contains("[Torus] Lync Gateway 01 Prod Pipeline")) && (!Convert.ToString(s.name).ToUpper().Contains("DEPRECATED") && !Convert.ToString(s.name).ToUpper().Contains("TEST")));
            }
            else if (id == 4126)
            {
                //Service: DevicePinAuth
                data = root.value.FindAll(s => (Convert.ToString(s.name).Contains("[Torus] PinAuth Prod Pipeline")) && (!Convert.ToString(s.name).ToUpper().Contains("DEPRECATED") && !Convert.ToString(s.name).ToUpper().Contains("TEST")));
            }
            return data;
        }
    }
}
