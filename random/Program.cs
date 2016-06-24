using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using CommandLine;

namespace random
{
    /// <summary>
    /// Entry point.
    /// </summary>
    public class Program
    {
        public const int ExitSuccess = 0;
        public const int ExitFailure = 1;

        /// <summary>
        /// Entry point.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static int Main(string[] args)
        {
            int result;

            try
            {
                ParserResult<CommandLineOptions> parserResult = Parser.Default.ParseArguments<CommandLineOptions>(args);
                result = parserResult.MapResult<CommandLineOptions, int>(
                    options =>
                    {
                        GenerateBytes(options.OutputFormat, options.ByteCount);
                        return ExitSuccess;
                    },
                    errors =>
                    {
                        Console.Error.WriteLine(errors.FirstOrDefault());
                        return ExitFailure;
                    });
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                result = ExitFailure;
            }

            return result;
        }

        /// <summary>
        /// Generate random bytes and write them to <see cref="Console.Out"/>.
        /// </summary>
        /// <param name="outputEncoding">
        /// The output encoding, either hex or base 64.
        /// </param>
        /// <param name="byteCount">
        /// The number of bytes to generate.
        /// </param>
        public static void GenerateBytes(OutputEncoding outputEncoding, uint byteCount)
        {
            if (!Enum.IsDefined(typeof (OutputEncoding), outputEncoding))
            {
                throw new ArgumentOutOfRangeException(nameof(outputEncoding));
            }

            byte[] randomBytes;
            string output;

            using (RandomNumberGenerator randomNumberGenerator = RandomNumberGenerator.Create())
            {
                randomBytes = new byte[byteCount];
                randomNumberGenerator.GetBytes(randomBytes);
            }

            switch (outputEncoding)
            {
                case OutputEncoding.Base64:
                    output = Convert.ToBase64String(randomBytes);
                    break;
                case OutputEncoding.Hex:
                    output = randomBytes.Aggregate(
                        new StringBuilder(),
                            (stringBuilder, randomByte) => stringBuilder.Append(randomByte.ToString("X2"))).ToString();
                    break;
                default:
                    throw new ArgumentException("Unknown output format", nameof(outputEncoding));
            }

            Console.Out.WriteLine(output);

        }
    }
}
