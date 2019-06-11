using System.Threading;

namespace BatchProcessing.Api.Services
{
    public interface IBatchProcessorService
    {
        void Process(string fileContentBase64, CancellationToken cancellationToken);
    }
}