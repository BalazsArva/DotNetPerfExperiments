using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using BatchProcessing.Api.Services.DataStructures;

namespace BatchProcessing.Api.Services.UtilityServices
{
    /// <summary>
    /// An implementation of <see cref="IBase64FileContentParser"/> which adds the parsed lines to a <see
    /// cref="LinkedList&lt;FileEntry&gt;"/>. This means more work for the GC (because of the nodes) and also makes
    /// enumeration slower compared to a regular list, but this could be more feasible under high memory fragmentation as
    /// well as eliminates the need for internal storage resizing which a regular list needs.
    /// </summary>
    public class LinkedListBase64FileContentParser : IBase64FileContentParser
    {
        public IEnumerable<FileEntry> ParseFileContent(string fileContentBase64)
        {
            var result = new LinkedList<FileEntry>();

            using (var memoryStream = new MemoryStream(Encoding.Default.GetBytes(fileContentBase64)))
            using (var base64Decoder = new CryptoStream(memoryStream, new FromBase64Transform(), CryptoStreamMode.Read))
            using (var streamReader = new StreamReader(base64Decoder))
            {
                while (!streamReader.EndOfStream)
                {
                    var line = streamReader.ReadLine();
                    var columns = line.Split(',');

                    var entry = new FileEntry { Id = int.Parse(columns[0]), Name = columns[1], Email = columns[2], DateOfBirth = columns[3] };

                    result.AddLast(entry);
                }
            }

            return result;
        }
    }
}