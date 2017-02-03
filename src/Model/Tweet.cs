using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Model
{
    public class Tweet
    {
        public string CorrelationId { get; set; }

        public string Message { get; set; }

        public Token Token { get; set; }
    }
}
