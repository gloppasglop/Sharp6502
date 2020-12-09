using HexParser;

namespace HexParserTest
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            HexReader parse = new HexReader(args[0]);
        }
    }
}