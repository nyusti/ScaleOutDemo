namespace Collector.Client
{
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using Collector.Configuration;

    /// <summary>
    /// Auto scale producer service client
    /// </summary>
    public class AutoScaleProducerClient : IAutoScaleProducerClient
    {
        private readonly HttpClient httpClient;

        /// <summary>
        /// Creates an instance of the client.
        /// </summary>
        public AutoScaleProducerClient()
        {
            this.httpClient = new HttpClient
            {
                BaseAddress = ConfigurationReader.Instance.Settings.AutoScaleProducerBaseUri
            };
        }

        /// <inheritdoc/>
        public virtual async Task<int?> GetWaitTimeAsync(CancellationToken cancellationToken)
        {
            const string meterEndpointSuffix = "api/meter";

            var response = await this.httpClient.GetAsync(meterEndpointSuffix, cancellationToken).ConfigureAwait(false);

            // filter out faild status codes
            response.EnsureSuccessStatusCode();

            switch (response.StatusCode)
            {
                // deserialize response if there should be one
                case System.Net.HttpStatusCode.OK:
                    return await response.Content.ReadAsAsync<int?>(cancellationToken).ConfigureAwait(false);

                // no content response should return null
                case System.Net.HttpStatusCode.NoContent:
                    return null;

                default:
                    throw new HttpRequestException("Unknown status code");
            }
        }
    }
}