﻿using System;
using System.IO;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;

namespace Com.Tradecloud1.SDK.Client
{
    class SetSettings
    {   
        // https://swagger-ui.accp.tradecloud1.com/?url=https://api.accp.tradecloud1.com/v2/authentication/specs.yaml#/authentication/
        const string authenticationUrl = "https://tc-8934-sap-update-delivery-schedule.t.tradecloud1.com/v2/authentication/";

        const string username = "supportuser@tradecloud1.com";
        // Fill in mandatory password
        const string password = "SupportSecret1";

        // https://swagger-ui.accp.tradecloud1.com/?url=https://api.accp.tradecloud1.com/v2/sap-soap-connector/private/specs.yaml#/sap-soap-connector/upsertSapSettings
        const string settingsUrl = "https://tc-8934-sap-update-delivery-schedule.t.tradecloud1.com/v2/sap-soap-connector/company/7091d1c3-6a84-4480-be26-8c45b8e59daa/settings";   

        static async Task Main(string[] args)
        {
            Console.WriteLine("Tradecloud SAP SOAP Connector set settings example.");

            var jsonContent = File.ReadAllText(@"voortman_settings.json");

            HttpClient httpClient = new HttpClient();
            var authenticationClient = new Authentication(httpClient, authenticationUrl);
            var (accessToken, refreshToken) = await authenticationClient.Login(username, password);
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
            await SetSettings();

            async Task SetSettings()
            {                
                var stringContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var started = DateTime.Now;
                var watch = System.Diagnostics.Stopwatch.StartNew();
                var response = await httpClient.PostAsync(settingsUrl, stringContent);
                watch.Stop();
                Console.WriteLine("SetSettings started: " + started +  " elapsed: " + watch.ElapsedMilliseconds);

                string responseString = await response.Content.ReadAsStringAsync();
                Console.WriteLine("SetSettings response StatusCode: " + (int)response.StatusCode + ", body: " +  responseString);  
            }
        }
    }
}
