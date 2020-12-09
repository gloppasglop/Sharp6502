using System;
using HexParser;

namespace HexParserTest
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1) {
                Console.WriteLine("Missing filename");
                Environment.Exit(22);
            }
            HexReader parse = new HexReader(args[0]);
            foreach (var line in parse.hexContent) {
                parse.ParseLine(line);
            }
        }
    }
}
