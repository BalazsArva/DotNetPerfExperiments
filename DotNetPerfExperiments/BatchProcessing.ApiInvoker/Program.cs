using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using BatchProcessing.ApiClient;

namespace BatchProcessing.ApiInvoker
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            var fileContentBase64 = await CreateFileIfNotExists(100 * 1000).ConfigureAwait(false);

            await UploadBatchFileAsync(fileContentBase64).ConfigureAwait(false);
        }

        private static async Task<string> CreateFileIfNotExists(int entryCount)
        {
            var fileName = $".\\sample-{entryCount}-items.txt";

            if (File.Exists(fileName))
            {
                return await File.ReadAllTextAsync(fileName).ConfigureAwait(false);
            }

            var stringBuilder = new StringBuilder();

            for (var i = 0; i < entryCount; ++i)
            {
                var id = i;
                var name = $"FirstName{i} LastName{i}";
                var email = $"$firstName{i}_lastName{i}@example.com";
                var dateOfBirth = $"1980-01-01";

                stringBuilder.Append($"{id},{name},{email},{dateOfBirth}");
            }

            var fileContentRaw = stringBuilder.ToString();
            var fileContentBytes = Encoding.Default.GetBytes(fileContentRaw);
            var fileContentBase64 = Convert.ToBase64String(fileContentBytes);

            await File.WriteAllTextAsync(fileName, fileContentBase64).ConfigureAwait(false);

            return fileContentBase64;
        }

        private static async Task UploadBatchFileAsync(string fileContentBase64)
        {
            var apiClient = new BatchApiClient(new HttpClient { BaseAddress = new Uri("http://localhost:5000", UriKind.Absolute) });

            await apiClient.PostAsync(new UploadBatchRequest { ContentBase64 = fileContentBase64 }).ConfigureAwait(false);
        }
    }
}