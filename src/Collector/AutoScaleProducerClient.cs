namespace Collector
{
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using Collector.Configuration;

    public class AutoScaleProducerClient : IAutoScaleProducerClient
    {
        private readonly HttpClient httpClient;

        public AutoScaleProducerClient()
        {
            this.httpClient = new HttpClient
            {
                BaseAddress = ConfigurationReader.Instance.Settings.AutoScaleProducerBaseUri
            };
        }

        public virtual async Task<int?> GetWaitTimeAsync(CancellationToken cancellationToken)
        {
            const string meterEndpointSuffix = "api/meter";

            var response = await this.httpClient.GetAsync(meterEndpointSuffix, cancellationToken).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            switch (response.StatusCode)
            {
                case System.Net.HttpStatusCode.OK:
                    return await response.Content.ReadAsAsync<int?>(cancellationToken).ConfigureAwait(false);

                case System.Net.HttpStatusCode.NoContent:
                default:
                    return null;
            }
        }
    }
}