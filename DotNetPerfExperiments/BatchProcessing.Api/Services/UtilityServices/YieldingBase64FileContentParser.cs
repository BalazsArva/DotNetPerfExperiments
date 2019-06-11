using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using BatchProcessing.Api.Services.DataStructures;

namespace BatchProcessing.Api.Services.UtilityServices
{
    /// <summary>
    /// An implementation of <see cref="IBase64FileContentParser"/> which directly enumerates the content of the base
    /// 64-decoded file. That is, it does not add the elements to a result data collecton, such as a list. This should
    /// result in less memory but more CPU utilization.
    /// </summary>
    public class YieldingBase64FileContentParser : IBase64FileContentParser
    {
        public IEnumerable<FileEntry> ParseFileContent(string fileContentBase64)
        {
            using (var memoryStream = new MemoryStream(Encoding.Default.GetBytes(fileContentBase64)))
            using (var base64Decoder = new CryptoStream(memoryStream, new FromBase64Transform(), CryptoStreamMode.Read))
            using (var streamReader = new StreamReader(base64Decoder))
            {
                while (!streamReader.EndOfStream)
                {
                    var line = streamReader.ReadLine();
                    var columns = line.Split(',');

                    var entry = new FileEntry { Id = int.Parse(columns[0]), Name = columns[1], Email = columns[2], DateOfBirth = columns[3] };

                    yield return entry;
                }
            }
        }
    }
}