using System;
using System.Collections.Generic;
using log4net.ElasticSearch.Infrastructure;
using log4net.ElasticSearch.Tests.Infrastructure.Builders;
using Xunit;

namespace log4net.ElasticSearch.Tests.IntegrationTests
{
    public class HttpClientTests
    {
        const string UserName = "";
        const string Password = "";
        const string Host = "";
        const string Port = "";
        const string Index = "";
        
        [Fact(Skip = "Set creds to run for real")]
        public void Can_post_to_elastic_cloud_instance()
        {
            var date = new DateTime();
            string uri = $"https://{UserName}:{Password}@{Host}:{Port}/{Index}-{date:yyyy-MM-dd}/logEvent/_bulk";
            var httpClient = new HttpClient();

            var logEvent = LogEventBuilder.Default.LogEvent;

            httpClient.Post(new Uri(uri), logEvent);
        }

        private static readonly ILog Log = LogManager.GetLogger(typeof(HttpClientTests));

        [Fact(Skip = "Set creds to run for real")]
        public void Can_post_bulk_to_elastic_cloud_instance()
        {
            var date = new DateTime();

            string uri = $"https://{UserName}:{Password}@{Host}:{Port}/{Index}-{date:yyyy-MM-dd}/logEvent/_bulk";
            var httpClient = new HttpClient();

            var leb = LogEventBuilder.Default.LogEvent;
            var timeStamp = DateTime.UtcNow.ToString("O");
            leb.timeStamp = timeStamp;
            leb.message = "Unit test";
            leb.loggerName = this.GetType().FullName;
            leb.domain = "NOT AVAILABLE";
            leb.identity = "NOT AVAILABLE";
            leb.level = "INFO";
            leb.className = "?";
            leb.fileName = "?";
            leb.lineNumber = "?";
            leb.fullInfo = "?";
            leb.methodName = "";
            leb.fix = "LocationInfo, UserName, Identity, Partia";
            leb.properties = new Dictionary<string, string>
            {
                {"log4net:HostName", "UnitHost" },
                {"log4net:Identity", "NOT AVAILABLE" },
                {"log4net:UserName", "NOT AVAILABLE" },
                {"@timestamp", timeStamp }
            };
            leb.userName = "NOT AVAILABLE";
            leb.threadName = "10";
            leb.hostName = "UNITHOST";

            httpClient.PostBulk(new Uri(uri), new []{ leb });
        }
    }
}
