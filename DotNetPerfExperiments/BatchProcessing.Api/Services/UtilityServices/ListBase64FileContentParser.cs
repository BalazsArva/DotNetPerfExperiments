using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using BatchProcessing.Api.Services.DataStructures;

namespace BatchProcessing.Api.Services.UtilityServices
{
    /// <summary>
    /// An implementation of <see cref="IBase64FileContentParser"/> which adds the parsed lines to a <see
    /// cref="List&lt;FileEntry&gt;"/> result collection whose initial capacity is <see cref="DefaultResultCapacity"/>. This is
    /// expected to be feasible in the case of a relatively small number of entries but can be problematic if there are
    /// lots of entries and the result list needs to be resized many times. When the number of items is not too large, it
    /// can be more feasible than e.g. a linked list, as the linked list nodes would mean additional work for the GC.
    /// However, it can be problematic in case of lots of entries and high memory fragmentation.
    /// </summary>
    public class ListBase64FileContentParser : IBase64FileContentParser
    {
        public const int DefaultResultCapacity = 1000;

        public IEnumerable<FileEntry> ParseFileContent(string fileContentBase64)
        {
            var result = new List<FileEntry>(DefaultResultCapacity);

            using (var memoryStream = new MemoryStream(Encoding.Default.GetBytes(fileContentBase64)))
            using (var base64Decoder = new CryptoStream(memoryStream, new FromBase64Transform(), CryptoStreamMode.Read))
            using (var streamReader = new StreamReader(base64Decoder))
            {
                while (!streamReader.EndOfStream)
                {
                    var line = streamReader.ReadLine();
                    var columns = line.Split(',');

                    var entry = new FileEntry { Id = int.Parse(columns[0]), Name = columns[1], Email = columns[2], DateOfBirth = columns[3] };

                    result.Add(entry);
                }
            }

            return result;
        }
    }
}