using System.Collections.Generic;
using BatchProcessing.Api.Services.DataStructures;

namespace BatchProcessing.Api.Services.UtilityServices
{
    public interface IBase64FileContentParser
    {
        IEnumerable<FileEntry> ParseFileContent(string fileContentBase64);
    }
}