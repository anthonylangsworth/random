using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;

namespace random
{
    internal class CommandLineOptions
    {
        [Option('f', "format", Default = OutputEncoding.Hex, MetaValue = "<Hex | Base64>", Required = false)]
        public OutputEncoding OutputFormat
        {
            get;
            set;
        }

        [Value(0, Required = true, HelpText = "Count of random bytes", MetaName = "Count of random bytes", MetaValue = "<count>")]
        public uint ByteCount
        {
            get;
            set;
        }
    }
}
