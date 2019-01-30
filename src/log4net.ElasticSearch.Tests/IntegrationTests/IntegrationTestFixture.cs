using System;
using Nest;
using Xunit;

namespace log4net.ElasticSearch.Tests.IntegrationTests
{
    public class IntegrationTestFixture : IDisposable
    {
        readonly string _defaultIndex;

        public IntegrationTestFixture()
        {
            _defaultIndex = Environment.GetEnvironmentVariable("log4net_ElasticSearch_Index") ?? GetDefaultIndex();

            Client = new ElasticClient(ConnectionSettings(_defaultIndex));

            DeleteDefaultIndex();
        }

        public ElasticClient Client { get; private set; }

        public void Dispose()
        {
            DeleteDefaultIndex();            
        }

        static string GetDefaultIndex()
        {
            return "log_test";
        }

        static ConnectionSettings ConnectionSettings(string index)
        {
            var esUri = Environment.GetEnvironmentVariable("log4net_ElasticSearch_Uri") ?? GetDefaultUri();

            var defaultConnectionSettings = new ConnectionSettings(new Uri(esUri)).
                DefaultIndex(index).                
                DefaultTypeNameInferrer(t => t.Name).
                DefaultFieldNameInferrer(p => p);

            return !AppSettings.Instance.UseFiddler()
                       ? defaultConnectionSettings
                       : defaultConnectionSettings.
                             DisableAutomaticProxyDetection(false).
                             Proxy(new Uri("http://localhost:8888"), "", "");
        }

        static string GetDefaultUri()
        {
            return "http://127.0.0.1:9200";
        }

        void DeleteDefaultIndex()
        {
            Client.DeleteIndex(new DeleteIndexRequest(_defaultIndex));
        }
    }

    [CollectionDefinition("IndexCollection")]
    public class DatabaseCollection : ICollectionFixture<IntegrationTestFixture>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }
}