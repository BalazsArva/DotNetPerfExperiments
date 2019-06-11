using System;
using System.Collections.Generic;
using System.Threading;
using BatchProcessing.Api.Services.DataStructures;
using BatchProcessing.Api.Services.UtilityServices;

namespace BatchProcessing.Api.Services
{
    public class BatchProcessorService : IBatchProcessorService
    {
        private readonly IBase64FileContentParser fileContentParser;

        public BatchProcessorService(IBase64FileContentParser fileContentParser)
        {
            this.fileContentParser = fileContentParser ?? throw new ArgumentNullException(nameof(fileContentParser));
        }

        public void Process(string fileContentBase64, CancellationToken cancellationToken)
        {
            var fileEntries = fileContentParser.ParseFileContent(fileContentBase64);

            if (fileEntries is List<FileEntry> fileEntryList)
            {
                for (var i = 0; i < fileEntryList.Count; ++i)
                {
                    var fileEntry = fileEntryList[i];

                    Console.WriteLine($"Id: {fileEntry.Id}, Name: {fileEntry.Name}, Email: {fileEntry.Email}, Date of birth: {fileEntry.DateOfBirth}");
                }
            }
            else
            {
                foreach (var fileEntry in fileEntries)
                {
                    Console.WriteLine($"Id: {fileEntry.Id}, Name: {fileEntry.Name}, Email: {fileEntry.Email}, Date of birth: {fileEntry.DateOfBirth}");
                }
            }
        }
    }
}