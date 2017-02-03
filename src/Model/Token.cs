using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Model
{
    public class Token
    {
        public string Key { get; set; }

        public string Secret { get; set; }

        public static Token ReadFromFile(string name)
        {
            using (var stream = new FileStream(name, FileMode.Open))
            {
                using (var streamReader = new StreamReader(stream))
                {
                    return ReadToken(streamReader);
                }
            }
        }

        private static Token ReadToken(StreamReader streamReader)
        {
            var token = new Token
            {
                Key = streamReader.ReadLine(),
                Secret = streamReader.ReadLine()
            };
            return token;
        }

        public void WriteToFile(string name)
        {
            using (var stream = new FileStream(name, FileMode.OpenOrCreate))
            {
                using (var streamWriter = new StreamWriter(stream))
                {
                    streamWriter.WriteLine(Key);
                    streamWriter.WriteLine(Secret);
                }
            }
        }
    }
}
