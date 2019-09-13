using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Subarashii.Core
{
    internal class FileUploader
    {
        private static string userFolder = @"c:\Users\alvar";
        private FileStream Fs { get; set; }

        public DecodedMessage<byte[]> SaveFileToTmpLocation(DecodedMessage<byte[]> decoded, Func<DecodedMessage<byte[]>> getMore)
        {
            int pathLength = BitConverter.ToInt32(decoded.Payload, 0);
            string path = Encoding.UTF8.GetString(decoded.Payload.Skip(sizeof(int)).Take(pathLength).ToArray());

            Fs = File.OpenWrite(Path.Combine(userFolder, path));

            while (true)
            {
                byte[] data = decoded.Payload.Skip(sizeof(int) + pathLength).ToArray();
                Fs.Write(data, 0, data.Length);

                decoded = getMore();

                if (!decoded.IsFile)
                {
                    Fs.Close();
                    return decoded;
                }
            }
        }
    }
}
