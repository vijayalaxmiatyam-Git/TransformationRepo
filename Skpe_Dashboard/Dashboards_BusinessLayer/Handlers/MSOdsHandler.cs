using Dashboards_BusinessLayer.Handlers.Interfaces;
using Dashboards_BusinessLayer.Models;
using Microsoft.Cloud.Metrics.Client;
using Microsoft.Cloud.Metrics.Client.Metrics;
using Microsoft.Cloud.Metrics.Client.Query;
using Microsoft.Online.Metrics.Serialization.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Dashboards_BusinessLayer.Handlers
{
    public class MSOdsHandler : IMsodsHandler
    {
        [Obsolete]
        public List<MSODSData> ReadMSODSMetricData(MetricReader reader, MetricIdentifier id, double threshold, double Interval, string metricName)
        {
            DimensionFilter[] dimensionFilters = new DimensionFilter[5];
            return GetSyncAndDimension(ref dimensionFilters, reader, id, threshold, Interval, metricName);

        }
        [Obsolete]
        private List<MSODSData> GetSyncAndDimension(ref DimensionFilter[] dimensionFilters, MetricReader reader, MetricIdentifier id, double threshold, double Interval, string metric)
        {
            List<MSODSData> dataList = new List<MSODSData>();
            string[] sync;
            switch (metric)
            {
                case "ForestA":
                    sync = new string[] {"36de647a-c70e-4ec1-9397-c0f21b175eab","0c2bc27a-9701-4265-82ad-e9d76532d05d","f97d5c0b-45d9-440a-98b7-29230496ee1a","3b3117fc-20aa-41e0-9584-762b43254251","141fccf2-eb66-4103-ba6f-a98e839b4bec","e25e46e2-630b-4542-9a91-9689f4359843","76667330-4ecd-4969-aa9c-de89b0f25b44","bcde860e-4ded-4fab-83b4-3517a435c465",
                            "b1541ff8-42ed-4fa1-9698-074e44b41fa7","9c0334b7-bdfa-4d83-a1d3-0fb6604e13fb","dfc2c060-c8fb-4f1f-a7c9-d0c7fc5335c2",
                            "3b3a64c8-96e0-4d40-aec6-b1bdfc731914","1e9c37d5-15cc-4e01-9862-487437380bcb","fcb45890-f92c-4804-b141-67924e420628",
                            "1a7f9c60-b127-4749-9116-0c36c7f7b76b","4b9ea16a-7c09-4dfa-8373-23e321ab42a4","ed514b2a-d52f-43ff-8445-270af14192a2",
                            "b84a245f-55f5-4a28-8164-98163aa85384","4262eaba-1d47-4307-85d9-448363b504d4","a0d30388-46cd-414e-b526-5e242f75e877",
                            "e9601fa6-4b6f-445a-8418-316913dbd828" };
                    dimensionFilters = new DimensionFilter[] { "servicedescriptorsuffix", "serviceinstance", DimensionFilter.CreateIncludeFilter("servicetype", "microsoftcommunicationsonline"), DimensionFilter.CreateIncludeFilter("syncstreamidentifier", sync) };
                    dataList = FillMsodsData(reader, id, threshold, Interval, dimensionFilters, metric, SamplingType.Sum);
                    break;
                case "Forest1A":
                    sync = new string[] {"151afa3a-7a15-4e31-aaa9-83a86a440690","1909374f-bf3b-4cf2-9233-42a30028a0c5","1f55e4f8-8b09-4352-9c4a-33ba0527b37c",
                        "2092f21e-f077-4b31-8b84-fe26d433f8fa","21ddf311-62da-4105-a576-154aa176e0ab",
                        "3111cd5f-5114-4ba7-9fa0-165c5ea2a8e2",
                        "33ba4d84-d2be-4ca1-9761-2452dfd793de",
                        "51526a73-812f-4b76-90d0-7479661e43bc",
                        "75bee480-4225-40a3-9c64-6e5395a5e8d8",
                        "89da07cb-da89-4fd8-8ceb-9a420f87db96",
                        "8c64c7bb-ee4c-4db0-8b8b-ebb4643d94a8",
                        "ac3a4e17-d881-4b52-be00-eee5efb0e56c",
                        "babda68e-683c-4849-865d-f39ccaf54ccc",
                        "d2a3c77f-c1bf-4782-8c2f-d179fc924a45",
                        "ea07df2d-94be-4e7d-8d2d-d6aaa3b4d8de",
                        "f2c05394-7bbe-46a9-bb88-6c5132ccc13b",
                        "f49249d0-d549-4ffc-b503-86abaaa3fb53" };
                    dimensionFilters = new DimensionFilter[] { "servicedescriptorsuffix", "serviceinstance", "servicetype", DimensionFilter.CreateIncludeFilter("syncstreamidentifier", sync) };
                    dataList = FillMsodsData(reader, id, threshold, Interval, dimensionFilters, metric, SamplingType.Average);
                    break;
                case "Forest2A":
                    sync = new string[] {"d42e84ef-bc60-4dcb-b711-fe2fd55ea7e8",
                        "093dc78a-7620-4303-9937-ad9639d78148",
                        "b9d3bfd9-d316-4dc2-a13f-0c8c484c72a2",
                        "3f4ad1a4-e389-4b10-9cb4-507c15d70b26",
                        "3d94cce3-f9fd-4606-b295-98ff8b9a8cdf",
                        "02ead937-3969-42f6-92ae-02b19e29ef1d",
                        "319ec7bf-02dc-4a8e-8788-e533984a9c0b",
                        "ff908f15-39c0-4c06-bbbd-7f3f80621d0e",
                        "d4809b54-a16c-4982-a647-bc71fcc25ac8",
                        "f49d37db-0575-41f4-b115-1765e16905ab",
                        "c974d44e-07c1-498a-8cb7-472db83facb3"};
                    dimensionFilters = new DimensionFilter[] { "servicedescriptorsuffix", "serviceinstance", "servicetype", DimensionFilter.CreateIncludeFilter("syncstreamidentifier", sync) };
                    dataList = FillMsodsData(reader, id, threshold, Interval, dimensionFilters, metric, SamplingType.Average);
                    break;
                case "Forest3A":
                    sync = new string[] {"1eff2c64-3a75-4c0e-bb20-47c177735cce",
                        "92ceaef8-dd50-47d5-ba02-d6f3940aae06",
                        "232e8852-e0ac-4a4f-a1b0-9b3aec222669",
                        "a8743df8-d6d1-4158-9a70-d6618bda5da6",
                        "3f6fd971-80eb-46b2-a2cc-29e3922d2fcd",
                        "6c658d77-b238-4529-a60f-d79f5c73bec5",
                        "8fa6f3d3-10fe-4d59-800b-eb44fc9ea022",
                        "aa3fb219-fc94-4050-ae49-b104e64f01b8",
                        "cc790762-d747-463a-88a0-f95dc1dfdaa2",
                        "8231b358-2a39-4a77-9fd5-7afd19d94ff1",
                        "91a88d64-f3e5-4c31-9026-2d6666b961a6",
                        "fbf45551-0972-49d2-8718-6975651edf26",
                        "5e9ce4d3-8f23-4d0e-8997-876522ad91f2",
                        "4e9cc1ce-56c1-48a6-a578-4e45bc8e0e8b",
                        "35cc8841-a09d-4bde-9dc8-adf5e434ed5a",
                        "93c54a30-fc89-455b-a1d0-8336f4449781",
                        "e5946164-3d7d-4749-8415-eaa5a9ad813a"};
                    dimensionFilters = new DimensionFilter[] { "servicedescriptorsuffix", "serviceinstance", "servicetype", DimensionFilter.CreateIncludeFilter("syncstreamidentifier", sync) };
                    dataList = FillMsodsData(reader, id, threshold, Interval, dimensionFilters, metric, SamplingType.Average);
                    break;
                case "Forest4A":
                    sync = new string[] { "fcaad9d4-7de1-4285-b6d1-f82cec9d75c8", "19988b8f-1363-4a48-9691-2fbc9bd9511a", "ca3543fa-bb30-46a1-a3f5-327c687b84ca", "ddeb703e-459a-4652-8304-79d21c7f03b6",
                    "fa687285-a261-4c0c-8047-9a46681a75d1","1029b8d5-ea69-479d-beb9-322e13ef5450","c4360b02-18ae-4243-95bc-901a096ba790","6383c883-2629-4b54-a1ea-5ff901cd88c3",
                    "08fc7ecc-b7b7-483b-9092-ca2bb3b2ad73","35b0782f-5ea2-4a51-9136-d607af3b0451","31286f19-a8de-482b-a341-cc4398d96b98","ae87a5e8-7c98-4d94-b7f8-59f4fcde599b",
                    "0130b1cf-40dd-4ece-a1f4-1615f3ca148a","188d408d-c2fd-43fa-9610-872b8f4f5c27","c8db233d-84b4-4dfb-83f8-890d51e6a88d","a51f169a-b1d8-4c1c-bc59-b6111c6c663b","cc43a72c-1726-407f-9925-1d97f3f91863"
                    };
                    dimensionFilters = new DimensionFilter[] { "servicedescriptorsuffix", "serviceinstance", "servicetype", DimensionFilter.CreateIncludeFilter("syncstreamidentifier", sync) };
                    dataList = FillMsodsData(reader, id, threshold, Interval, dimensionFilters, metric, SamplingType.Average);
                    break;
                case "ForestB":
                    sync = new string[] { "b66a2beb-78f4-44d6-8c68-3f6f4b87429f","93e22de4-1b8b-4cd3-b62b-69c2b57af53e","fdb909e3-9e45-4939-886c-cb149411bc5f","0ccb14d4-92fe-4ba0-96bd-60c2608c440c",
                        "28d852c7-9acf-486a-bde3-50155ee6da63","6b88a615-8e06-43d4-b375-88a585746808","063ba662-b7fa-489a-b467-21b68d97b23c","361694d2-8902-40b1-a380-2621e477c64c",
                        "a0f7cd83-3b2a-4e75-9f38-f070b7b553b1"
                    };
                    dimensionFilters = new DimensionFilter[] { "servicedescriptorsuffix", "serviceinstance", "servicetype", DimensionFilter.CreateIncludeFilter("syncstreamidentifier", sync) };
                    dataList = FillMsodsData(reader, id, threshold, Interval, dimensionFilters, metric, SamplingType.Average);
                    break;
                case "Forest1B":
                    sync = new string[] { "a8877bee-a32a-4807-8940-088e057e47f0","eead5eaf-950b-45c8-b565-2e61400ba1ac","9764c97c-5510-4ce3-99bf-e90e2e43fb45","5225bec7-91d8-4a79-8866-05bd9e076857","d6971421-bc68-4304-be4f-64c49995cc5a",
                        "be50f406-b362-4a1f-86dd-3a7aef429e26"
                    };
                    dimensionFilters = new DimensionFilter[] { "servicedescriptorsuffix", "serviceinstance", "servicetype", DimensionFilter.CreateIncludeFilter("syncstreamidentifier", sync) };
                    dataList = FillMsodsData(reader, id, threshold, Interval, dimensionFilters, metric, SamplingType.Average);
                    break;
                case "ForestE":
                    sync = new string[] { "6763df2d-f4bd-4138-8747-4350fd9c6e91","059fcdc3-c216-46f6-883e-778aab450b31","8bfeb20d-e7a2-4edc-a55b-6a8b9929e0f5","f55942b5-e169-4faa-8443-1c6282d44076",
                        "ed9eaaf5-5d5f-4de4-a6fd-fe73e7ae5451","b67f8906-946f-4f72-9af4-8781d9a94674","3edb28aa-924f-4ee5-9f8c-d47bbdbd8697","b2e51077-5200-4ad8-84bd-71e84878eab9",
                        "9de690f6-485e-486f-a4f5-ef0e9cffedbb"
                    };
                    dimensionFilters = new DimensionFilter[] { "servicedescriptorsuffix", "serviceinstance", "servicetype", DimensionFilter.CreateIncludeFilter("syncstreamidentifier", sync) };
                    dataList = FillMsodsData(reader, id, threshold, Interval, dimensionFilters, metric, SamplingType.Average);
                    break;
                case "Forest1E":
                    sync = new string[] { "501bd8c1-6325-4483-b302-b67cac451ea9","377643b8-0c67-447d-84e0-7407b089d900","dac5f80f-8655-493a-b6ff-839cf0275e92","ac49c575-814d-42fe-a6ae-eb9c7f1b464d",
                        "2aae52ee-5635-433a-8b9c-3ceaa52a1947","17cd9f08-3f9e-47f2-8865-8c2fa5b55413","5df8f862-b4d9-4308-bf5b-b4be2a5b020c","feedb696-a413-4566-b96f-36fef439f479",
                        "e9c9029e-6735-47b5-bcfe-6af6785fa79c","d74a0b4f-c38e-4006-a223-53f7d9fd0f93","29cf9a3b-1ee4-4b6b-986f-f59038ebaf8f"
                    };
                    dimensionFilters = new DimensionFilter[] { "servicedescriptorsuffix", "serviceinstance", "servicetype", DimensionFilter.CreateIncludeFilter("syncstreamidentifier", sync) };
                    dataList = FillMsodsData(reader, id, threshold, Interval, dimensionFilters, metric, SamplingType.Average);
                    break;
                case "Forest2E":
                    sync = new string[] { "19b38161-cf99-4a7c-b41b-cc3af8df4b27","b8955d71-bcdd-4fde-974e-806952c96810","79c6d105-c65a-4b38-9292-8168a35bb7d4","666a2684-5696-46de-91b3-ab4c2842ab12","06b701e8-b2d6-4c04-aee6-bd8b1612ba1f",
                        "963a2be8-6896-4dcb-bbdf-26ddfff0b68f","a320d409-3989-4f51-ade1-2c3400242291","e76c6dbc-6322-4d0a-9e2b-4205d6cc2f30","5242dc3a-cba8-4c53-af96-b43605bde6b0",
                        "107f17b2-a937-47d8-aecf-ea8c3dd356ec","df4fc10c-1eac-4f74-989a-2fa83183487d"
                    };
                    dimensionFilters = new DimensionFilter[] { "servicedescriptorsuffix", "serviceinstance", "servicetype", DimensionFilter.CreateIncludeFilter("syncstreamidentifier", sync) };
                    dataList = FillMsodsData(reader, id, threshold, Interval, dimensionFilters, metric, SamplingType.Average);
                    break;
                case "Forest3E":
                    sync = new string[] { "1385dd15-e6e7-4c90-933c-0acc0443d5fb","5ad470cd-7a69-4d02-955a-40eb250f9217","b2884fff-cf62-4b8b-9147-81fb93cddaf4","c1e8f0e9-1578-4e1f-939e-4bf542b9d52f",
                        "be54d0f6-b240-4de4-a0c7-c373b6f1e877","c253afba-88c5-4190-bf8e-11a878cf21e7","2768c26e-5bc7-4c26-a937-dd0e2dacaca5","f1f11154-63ed-4ccb-bb2f-20a6a685953e",
                        "eaff28dd-d05b-4ef6-8758-7aaa6f1cf7d3","04bcf801-0e4d-4015-ad23-c8826f22fce5","ba602ce6-3ba3-42c0-bf03-16a3465eb156"
                    };
                    sync = new string[] { "be54d0f6-b240-4de4-a0c7-c373b6f1e877" };
                    dimensionFilters = new DimensionFilter[] { "servicedescriptorsuffix", "serviceinstance", "servicetype", DimensionFilter.CreateIncludeFilter("syncstreamidentifier", sync) };
                    dataList = FillMsodsData(reader, id, threshold, Interval, dimensionFilters, metric, SamplingType.Average);
                    break;
                case "Forest4E":
                    sync = new string[] { "9d4bb46d-11b2-4a06-8149-17fefee5fd66","22f8adb5-eed0-4b2a-8845-24825e070f43","0a222314-632f-470a-9065-032cb6cdb694","5d07cc15-e5b2-4355-aef6-4e29c35a1239",
                        "141a0404-da43-4ef0-b6c7-61f46923fd54","a47ca71d-8aa5-4803-9981-db92a4de7878","f0b430b2-d593-44d3-ab00-0eb552719a74","283e67cc-799f-4352-8ba1-adf2ff94bee8",
                        "cd3c3549-8da4-4ed6-904f-0aa6416455ee","ba9ac51f-d93e-44c6-ba18-eca9006a70bb","5aa3cacf-533a-4026-af31-d464f21809fd"
                    };
                    dimensionFilters = new DimensionFilter[] { "servicedescriptorsuffix", "serviceinstance", "servicetype", DimensionFilter.CreateIncludeFilter("syncstreamidentifier", sync) };
                    dataList = FillMsodsData(reader, id, threshold, Interval, dimensionFilters, metric, SamplingType.Average);
                    break;
                case "ForestF":
                    sync = new string[] { "2c5407c1-f130-4a68-9b24-6c58b5b13a5b","62bc6936-b289-4b71-9c72-54711f289608","bd96e582-1b1a-41af-83c1-7427ccc1a8e9","2040f14b-d78a-462a-a0cc-78a0a2699619",
                        "52299e2f-8311-4deb-8a35-fd90a68948c3","85087c71-6d9a-4035-abef-bc15047611a6","cb16b573-93c4-49f1-9452-efa097393c2c","21817135-8b0d-4a31-a156-a1efb272ddbb"
                    };
                    dimensionFilters = new DimensionFilter[] { "servicedescriptorsuffix", "serviceinstance", "servicetype", DimensionFilter.CreateIncludeFilter("syncstreamidentifier", sync) };
                    dataList = FillMsodsData(reader, id, threshold, Interval, dimensionFilters, metric, SamplingType.Average);
                    break;
                case "Forest0G":
                    sync = new string[] { "8bd46acb-922e-4adb-958f-bfe1764bbdeb","dcd56e04-81cc-4233-af3a-9d259dbe6c72","32c7da36-fd95-43a5-ad30-514a429163e7"
                    };
                    dimensionFilters = new DimensionFilter[] { "servicedescriptorsuffix", "serviceinstance", "servicetype", DimensionFilter.CreateIncludeFilter("syncstreamidentifier", sync) };
                    dataList = FillMsodsData(reader, id, threshold, Interval, dimensionFilters, metric, SamplingType.Average);
                    break;
                case "Forest1G":
                    sync = new string[] { "81181a8a-6dc5-4c27-bcba-77e0f7012866","90ea5d31-a75b-4748-8e5a-137c55a91bbd"
                    };
                    dimensionFilters = new DimensionFilter[] { "servicedescriptorsuffix", "serviceinstance", "servicetype", DimensionFilter.CreateIncludeFilter("syncstreamidentifier", sync) };
                    dataList = FillMsodsData(reader, id, threshold, Interval, dimensionFilters, metric, SamplingType.Average);
                    break;
                case "Forest2G":
                    sync = new string[] { "b45f4782-b222-449c-b7c9-31e8b3068cbe","cb5683c4-f66b-4f03-ad34-23fb40ba2dd9"
                    };
                    dimensionFilters = new DimensionFilter[] { "servicedescriptorsuffix", "serviceinstance", "servicetype", DimensionFilter.CreateIncludeFilter("syncstreamidentifier", sync) };
                    dataList = FillMsodsData(reader, id, threshold, Interval, dimensionFilters, metric, SamplingType.Average);
                    break;
                case "Forest0M":
                    sync = new string[] { "9c9cf584-5648-4b89-a4ef-451e479b930d","e310a705-e098-4672-b434-c48dc13540a1","8e5da6e7-2a28-48a5-921d-cd34c811fa4f","8c90f85c-1661-41fa-9b13-8f8e9b889009",
                        "830eb069-2567-487b-ae25-1b0e1fe757da"
                    };
                    dimensionFilters = new DimensionFilter[] { "servicedescriptorsuffix", "serviceinstance", "servicetype", DimensionFilter.CreateIncludeFilter("syncstreamidentifier", sync) };
                    dataList = FillMsodsData(reader, id, threshold, Interval, dimensionFilters, metric, SamplingType.Average);
                    break;
                case "Forest1M":
                    sync = new string[] { "77bc4a73-5b4d-4bc7-8d94-0f2fdfd1bf4e","3ac50486-d901-4537-87d8-845298faf8f4","eaf2bf3f-054c-41bb-9ee4-d00dcc67b3d9"
                    };
                    dimensionFilters = new DimensionFilter[] { "servicedescriptorsuffix", "serviceinstance", "servicetype", DimensionFilter.CreateIncludeFilter("syncstreamidentifier", sync) };
                    dataList = FillMsodsData(reader, id, threshold, Interval, dimensionFilters, metric, SamplingType.Average);
                    break;
                case "ForestAN1":
                    sync = new string[] { "4170019b-d9ba-4a22-9404-0bc284e431d0","1cfc530c-536b-4f35-9698-f2eb9df682c5","ebf8ffe4-c43c-4067-9b93-9c0f14b74c88","b42fb1f7-eead-4150-a324-bfbd83ea5185",
                        "efb52cba-95e7-408b-b127-75239572a2a8","c6f3fcd2-673f-46fa-913b-6b20694761b3","dfbae4f7-c6fa-4ecc-a270-3d556f1e5c8c","697acdef-309b-40be-9276-aa88cd70ee2d",
                        "b6d40387-6e3f-438d-acd0-fc89cccda036","13621def-e037-4a04-8fd2-5a4ae6e7e45e","bc881d08-6042-4433-b880-21d2bdb0ed19"
                    };
                    dimensionFilters = new DimensionFilter[] { "servicedescriptorsuffix", "serviceinstance", "servicetype", DimensionFilter.CreateIncludeFilter("syncstreamidentifier", sync) };
                    dataList = FillMsodsData(reader, id, threshold, Interval, dimensionFilters, metric, SamplingType.Average);
                    break;
                case "ForestAU1":
                    sync = new string[] { "33b24d81-3dfb-48ee-878b-6a285beab3e0","1fc1b3e4-07b5-49ac-8ff0-50e1d9900cc4","0ac5930d-791e-4db5-8722-b5855c2c37e1","dcb4a87d-6891-4b3b-9700-db918ba3a3f1",
                        "b1e38d49-b0b5-4c05-9c56-52b66a96780e","94165994-12f0-4007-8f39-c93f5d1a0114","4e26a9f8-0de0-40b7-8ef8-0bbb2e4ad248","fe631a25-2efd-4b62-b07c-f3b75a27dfd0"
                    };
                    dimensionFilters = new DimensionFilter[] { "servicedescriptorsuffix", "serviceinstance", "servicetype", DimensionFilter.CreateIncludeFilter("syncstreamidentifier", sync) };
                    dataList = FillMsodsData(reader, id, threshold, Interval, dimensionFilters, metric, SamplingType.Average);
                    break;
                case "ForestCA1":
                    sync = new string[] { "e3fd1494-eeb6-4f07-ad9f-cbe29d6a5004","ce2aad84-e5fc-446a-9cab-1ec320259df3","6c541d67-6b79-43c9-b470-04480a43b700","6975a0ce-2dac-4953-aad9-fb5fe210c083",
                        "56549396-ea24-4116-8c29-d0e9c5583ef4","25bfe4f9-58b8-4ded-94cf-0ebef5221bbb","0eabac21-15b8-4cbf-98ce-5949f8dcedb9","d285795e-3a1f-4f25-9100-cdf7c6f046c9",
                        "562eeb9c-0df4-4b55-9a97-8d30e542cd52","52e8db3f-d9cf-4f0b-8219-8af2b00eeed9"
                    };
                    dimensionFilters = new DimensionFilter[] { "servicedescriptorsuffix", "serviceinstance", "servicetype", DimensionFilter.CreateIncludeFilter("syncstreamidentifier", sync) };
                    dataList = FillMsodsData(reader, id, threshold, Interval, dimensionFilters, metric, SamplingType.Average);
                    break;
                case "ForestDE1":
                    sync = new string[] { "0f9338e3-ba0c-4b20-9af4-47d9b6d1f788","9cc90a95-2ec3-4dc8-9673-8b862fefdaac","1395c1a3-17ca-4ab9-ae3e-930c06c3e031","ea070fe5-d817-41f1-84dc-a69d1ae73e2e"
                    };
                    dimensionFilters = new DimensionFilter[] { "servicedescriptorsuffix", "serviceinstance", "servicetype", DimensionFilter.CreateIncludeFilter("syncstreamidentifier", sync) };
                    dataList = FillMsodsData(reader, id, threshold, Interval, dimensionFilters, metric, SamplingType.Average);
                    break;
                case "ForestED1":
                    sync = new string[] { "36006244-99c0-4b37-bc52-6f7c600bbd98","4a2ced74-f526-4925-8d8d-c337b7f4f142","de806e0c-dce4-4698-ab20-f48ccda54339","3276e764-cbd8-48ce-ac31-d34191886935",
                        "37fe77b6-3e93-4890-a1f2-f0b2920184ff","f6a53f62-c750-4364-a7cb-b5052e986508","fd7cf7b2-f2d6-435f-8ae2-b0c350bf6509","60397a1c-245b-489c-a01c-e47bfdc0595c",
                        "759992ad-ec6f-4e6b-ba21-0341724d05e9","9431b3d9-7058-4043-a6ac-b20c869244c2"
                    };
                    dimensionFilters = new DimensionFilter[] { "servicedescriptorsuffix", "serviceinstance", "servicetype", DimensionFilter.CreateIncludeFilter("syncstreamidentifier", sync) };
                    dataList = FillMsodsData(reader, id, threshold, Interval, dimensionFilters, metric, SamplingType.Average);
                    break;
                case "ForestED2":
                    sync = new string[] { "a9a2a804-759c-4b64-8c31-ec7954e92f7b","f1c7bec5-638e-4d2e-88ec-c21f3dd10f2f","63939ce1-2299-4b08-a8be-8256372216a7","c9ebb341-9a67-40f5-9cc9-cc9d89fe1440",
                        "8db38ef5-b98e-4480-9087-c9264d560c78","b9de1ca8-f53a-42a1-878d-e4b3dfee511a","03309cd7-cb5f-48de-b1ac-a099f9aead89","3162c707-d4a6-47f7-9a08-a88c6dd360a9",
                        "b1ccd083-2194-46b8-bfcf-3009f64f6aef","ff75e6da-dc94-4a2d-b39f-4e345136ca94","3b23a1ad-7987-4ca2-8df5-0f639f0d3e2b"
                    };
                    dimensionFilters = new DimensionFilter[] { "servicedescriptorsuffix", "serviceinstance", "servicetype", DimensionFilter.CreateIncludeFilter("syncstreamidentifier", sync) };
                    dataList = FillMsodsData(reader, id, threshold, Interval, dimensionFilters, metric, SamplingType.Average);
                    break;
                case "ForestED3":
                    sync = new string[] { "9689deda-9f5e-49f1-94ed-7c2a4db3982d","9807cf8a-91ad-4aa4-9b68-62b4fcf7a6d4","cf327ed4-6491-4578-89cb-363927cf6d98","10541a29-43af-41f2-8096-0e051b126026",
                        "6fa2f441-e9eb-462e-b6cd-971538c8e3d7","7423d245-9cd6-4802-8ab7-021952d645e5","5925a677-7011-4a80-a6ca-47821ee33179","ab173ada-2306-47e8-9271-109a8dffd694",
                        "4ddd4cc7-f11f-44e1-b203-287a6bb26c66","ceb56b0e-e105-43f6-9d26-a8187bf00c7a","852cb3b7-c0af-401e-9d81-87877567a8c8"
                    };
                    dimensionFilters = new DimensionFilter[] { "servicedescriptorsuffix", "serviceinstance", DimensionFilter.CreateIncludeFilter("servicetype", "microsoftcommunicationsonline"), DimensionFilter.CreateIncludeFilter("syncstreamidentifier", sync) };
                    dataList = FillMsodsData(reader, id, threshold, Interval, dimensionFilters, metric, SamplingType.Average);
                    break;
                case "ForestED4":
                    sync = new string[] { "4e0f6404-52e4-41f0-9f2c-e6ab36d0cc09","03b46521-0e01-4c2a-87ac-a3b0d12adae0","08d16d22-b5a1-4932-aa15-f601ad1333f5","e2f5e613-4118-42d0-bdb3-afa6897c7fd9",
                        "0503f857-a405-4540-a83e-53c3ae7d8602","51fc169b-970b-4c4f-a901-e3631052118c","990c87d8-cb26-450a-804f-b702fb222813","7183aee7-5075-4e4d-a288-12dfdc97fe43",
                        "9fdcb76a-467f-4881-a01e-1c6fe48bb0f6","5585ce27-60ab-49aa-8624-5056653b2852"
                    };
                    dimensionFilters = new DimensionFilter[] { "servicedescriptorsuffix", "serviceinstance", DimensionFilter.CreateIncludeFilter("servicetype", "microsoftcommunicationsonline"), DimensionFilter.CreateIncludeFilter("syncstreamidentifier", sync) };
                    dataList = FillMsodsData(reader, id, threshold, Interval, dimensionFilters, metric, SamplingType.Average);
                    break;
                case "ForestED5":
                    sync = new string[] { "ae5a76ef-a7a9-43ab-a9b4-4349cb1b4b61","02af30f4-8a18-4fc7-8f85-b768588dbe47","7d2f9d0c-9e5f-418d-b7e6-397a778d67da","fb12460c-c0de-47c6-a020-0a67950f9691",
                        "c108b3ad-6421-4716-aa3e-85cf6095fbb0","68d3f91c-0a4f-4958-abd2-d13bd343c971","10bb7005-9493-4e7b-b235-e31e0bd569f6","581b8031-f8d7-41c5-91bb-4e3fe9134d95",
                        "4df3e2d0-b66e-4ac1-b2a9-c95f84458c43","b26bfd29-973b-44db-acac-c1f0872d3e8f"
                    };
                    dimensionFilters = new DimensionFilter[] { "servicedescriptorsuffix", "serviceinstance", DimensionFilter.CreateIncludeFilter("servicetype", "microsoftcommunicationsonline"), DimensionFilter.CreateIncludeFilter("syncstreamidentifier", sync) };
                    dataList = FillMsodsData(reader, id, threshold, Interval, dimensionFilters, metric, SamplingType.Average);
                    break;
                case "ForestED6":
                    sync = new string[] { "35b773c2-5779-4278-a9c6-862eff6ffd2d","a195f744-0def-4ce3-b1da-1b5853c212d0","a1a23548-5e98-4891-82b5-5e2614264396","48a2c61a-efc0-4623-aa1c-dd658a869574",
                        "b1954544-8df6-43df-883a-798d967de57d","e9fa6a14-fdbb-4d61-98ba-b7db0e612dd9","128c0ba0-542f-4a3e-8d3c-26bd3f681453","1bb2d91c-f50b-422d-afa6-34b5d0d86a7a",
                        "cd287dc8-7ae2-4704-8e12-72966e9b479d","f90feae2-6786-4e8f-a4fd-fd46c367e796","e67c4cd4-078c-48a5-a3cb-bf36123e77f2"
                    };
                    dimensionFilters = new DimensionFilter[] { "servicedescriptorsuffix", "serviceinstance", DimensionFilter.CreateIncludeFilter("servicetype", "microsoftcommunicationsonline"), DimensionFilter.CreateIncludeFilter("syncstreamidentifier", sync) };
                    dataList = FillMsodsData(reader, id, threshold, Interval, dimensionFilters, metric, SamplingType.Average);
                    break;
                case "ForestGB1":
                    sync = new string[] { "e79d3431-0546-4ffe-9c09-4f8a09bee622","8c03e804-6eab-4835-85f6-f56847ef1f8c","384ff626-376c-4108-9988-5eaab649f49b","f8da5f95-16d5-474d-8a73-5523bf99a6fa",
                        "06d062c7-37b4-42c9-9be0-7c01535b3761","1bc46959-0dee-4826-b852-6d4af5923ef3","b7f74b1f-febe-4298-8fc0-d138a713f0f6","560fe01c-9ef4-4eb0-8473-58c346acdf46",
                        "afdce499-fa72-48f9-834f-476f8cfd2c4e","dfea5753-ccd0-4c8f-a3cb-6131d2872b9a"
                    };
                    dimensionFilters = new DimensionFilter[] { "servicedescriptorsuffix", "serviceinstance", "servicetype", DimensionFilter.CreateIncludeFilter("syncstreamidentifier", sync) };
                    dataList = FillMsodsData(reader, id, threshold, Interval, dimensionFilters, metric, SamplingType.Average);
                    break;
                case "ForestIN1":
                    sync = new string[] { "a0b37065-1a63-486f-92f5-5776510d3dc1","929a1cee-dee5-4d81-8ba9-f4347322ac68","fff3cfd2-6e37-40c8-afda-9067bdcfc0f4","e779d0c2-94f8-4875-82cc-08402317ee51",
                        "fff8a1f5-f300-4d55-8b2e-a44783224152","20548728-897f-4b83-be60-07aa8be78e72","428dd276-b5ba-4691-9c60-61c2795d8927","3f752093-929e-4245-889c-78a0aaf37f3e",
                        "53175551-c944-4548-9c18-c4324ca727f3","9894305c-8cbd-4ff2-a938-a046e9e26db7"
                    };
                    dimensionFilters = new DimensionFilter[] { "servicedescriptorsuffix", "serviceinstance", "servicetype", DimensionFilter.CreateIncludeFilter("syncstreamidentifier", sync) };
                    dataList = FillMsodsData(reader, id, threshold, Interval, dimensionFilters, metric, SamplingType.Average);
                    break;
                case "ForestJP1":
                    sync = new string[] { "7d3170f4-964b-48f3-9843-de6afaf8e710","d2714581-2bbc-4a2d-bf46-5cf1e816a63d","8d116554-526b-4d7d-b624-302d1d2e19ef","d13c6ba9-988f-420e-afec-fbc020eab307",
                        "628be1df-fe41-4531-a90b-668f31f6322a","d6090efb-bcf4-4a14-ab15-2dba0e3f44a9","ed294760-73fb-4dc9-9463-b5f7c443015b","33467ed4-9dd0-4a7b-8c42-4227da7fb444",
                        "fcd43aff-de81-4c29-acfe-49bb99e36bf6","b277056d-c9e0-4f2c-b70c-d0ffc823958c","17ddb28c-84ac-4cdc-aa01-77c7ae9df708","9fd317d2-2d61-45c6-9c67-77bc5c02e415",
                        "3e3a670a-4795-4162-851b-d229057faee7","1422a176-d06e-49b9-b044-0debbde6343d","f0bcef88-3020-4926-8e00-1805d9b51ff0","1ff8d5e3-b422-42c0-8c4a-ab4b633732a0",
                        "0646dbf4-e48c-4be3-9ffb-858efa6569c1"
                    };
                    dimensionFilters = new DimensionFilter[] { "servicedescriptorsuffix", "serviceinstance", "servicetype", DimensionFilter.CreateIncludeFilter("syncstreamidentifier", sync) };
                    dataList = FillMsodsData(reader, id, threshold, Interval, dimensionFilters, metric, SamplingType.Average);
                    break;
                case "ForestKR1":
                    sync = new string[] { "33508e1d-4dc0-46cf-97e1-7230df51eee2","3ac920e5-6075-4df1-9e63-4d015197ef7b","335244d2-0851-495b-8355-be44afb28ea1","da65620e-0a70-415e-a232-5acf774295ba",
                        "8ffe50b4-dcfd-4244-aed6-fc2656125c44","ee4d8117-745d-4122-8956-d3368495ab1e","32ad7836-00dc-417d-8ed9-bbfdeac2508a","df27fa39-9fae-474f-848f-6564bb7f878a",
                        "1582ad7e-1b2b-4fe9-893d-1b9f8f815535","a92fda01-60fa-41e0-9db5-9238c966a16d"
                    };
                    dimensionFilters = new DimensionFilter[] { "servicedescriptorsuffix", "serviceinstance", "servicetype", DimensionFilter.CreateIncludeFilter("syncstreamidentifier", sync) };
                    dataList = FillMsodsData(reader, id, threshold, Interval, dimensionFilters, metric, SamplingType.Average);
                    break;
                default:
                    break;
            }
            return dataList;
        }

        [Obsolete]
        private List<MSODSData> FillMsodsData(MetricReader reader, MetricIdentifier id, double threshold, double Interval, DimensionFilter[] dimensionFilters, string divName, SamplingType samplingType)
        {
            List<MSODSData> divlist = new List<MSODSData>();
            try
            {
                IEnumerable<IQueryResult> results = reader.GetFilteredDimensionValuesAsync(
                id,
                dimensionFilters,
                DateTime.UtcNow.AddMinutes(-(Interval)),
                DateTime.UtcNow,
                samplingType,
                Reducer.Max,
                new QueryFilter(Operator.GreaterThanOrEqual, threshold),
                true,
                new SelectionClause(SelectionType.TopValues, 200, OrderBy.Descending)
                ).Result;

                MSODSData ObjOds = new MSODSData();
                int i = 1;

                if (results.Count() > 0)
                {
                    foreach (var r in results)
                    {
                        //ObjOds.ServiceDescriptorSuffix = r.DimensionList[0].Value.ToString();
                        ObjOds.ServiceInstance = r.DimensionList[1].Value.ToString();
                        ObjOds.ServiceType = r.DimensionList[2].Value.ToString();
                        ObjOds.SyncStreamIdentifier = r.DimensionList[3].Value.ToString();
                        ObjOds.EvaluatedResult = Convert.ToDouble(r.EvaluatedResult);
                        ObjOds.Datapoints = new List<Models.Datapoints>();
                        var d = r.TimeSeries.ToList();
                        d.RemoveAll(item => item == null);

                        var definition = new TimeSeriesDefinition<MetricIdentifier>(
                        id,
                        new Dictionary<string, string> { { "ServiceInstance", ObjOds.ServiceInstance }, { "ServiceType", ObjOds.ServiceType }, { "SyncStreamIdentifier", ObjOds.SyncStreamIdentifier } });
                        TimeSeries<MetricIdentifier, double?> result =
                            reader.GetTimeSeriesAsync(DateTime.UtcNow.AddMinutes(-Interval), DateTime.UtcNow, SamplingType.Average, definition).Result;

                        var re = JsonConvert.SerializeObject(result.Datapoints);
                        var thresholdWiseDatapoints = JsonConvert.DeserializeObject<List<Datapoints>>(re).Where(val => val.Value >= threshold);
                        ObjOds.Datapoints.AddRange((IEnumerable<Models.Datapoints>)thresholdWiseDatapoints);
                        MSODSData list;
                        if (thresholdWiseDatapoints.Count() >= Convert.ToInt32(ConfigurationManager.AppSettings["CutOffTime"]))// Interval  360 // Orange/ Red/ Green
                        {
                            double highThreshold;
                            if (ObjOds.ServiceInstance.Contains("-") && ObjOds.ServiceInstance.Split('-')[ObjOds.ServiceInstance.Split('-').Count() - 1].StartsWith("a"))
                            {
                                highThreshold = Convert.ToDouble(ConfigurationManager.AppSettings["MSODSThresholdHigh_a"]);
                            }
                            else
                            {
                                highThreshold = Convert.ToDouble(ConfigurationManager.AppSettings["MSODSThresholdHigh_others"]);
                            }
                            //continous any where 6more than 50 occurence0 came or not by using 
                            list = new MSODSData { divId = divName + i, Datapoints = ObjOds.Datapoints, ServiceInstance = ObjOds.ServiceInstance, SyncStreamIdentifier = ObjOds.SyncStreamIdentifier, ServiceType = ObjOds.ServiceType, EvaluatedResult = ObjOds.EvaluatedResult, Color = ObjOds.Color };
                            GetMsodsDataByThreshold(ref list, JsonConvert.DeserializeObject<List<DatapointsTimeWise>>(re), Convert.ToDouble(ConfigurationManager.AppSettings["ADSThresholdLow"]), highThreshold, Convert.ToInt16(ConfigurationManager.AppSettings["MSODSTimeSpan"]));
                            divlist.Add(list);
                        }
                        i++;
                    }
                }
                return divlist;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public void GetMsodsDataByThreshold(ref MSODSData obj, IEnumerable<DatapointsTimeWise> datapointsTimeWise, double lowThrshld, double highThrshld, int timeSpan)
        {
            try
            {
                var datapoint = datapointsTimeWise.Where(a => a.Value != 0).ToList();
                int dataCountCrl = 0;
                foreach (var item in datapoint)
                {
                    if (item.Value >= highThrshld)
                    {
                        dataCountCrl++;
                    }
                }
                if (dataCountCrl >= timeSpan)
                {
                    obj.Color = "red";
                }
                else
                {
                    obj.Color = "orange";
                }
            }
            catch (Exception)
            {
                throw;
            }
        }


    }
}
