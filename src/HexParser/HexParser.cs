using System;
using System.Collections.Generic;
using System.IO;


namespace HexParser
{
    public class HexReader
    {

        public IEnumerable<string> hexContent;
        public HexReader(string fileName) {
            hexContent = File.ReadLines(fileName);
        }

        public HexRecord ParseLine(string hexRecordLine) {
            HexRecord record = new HexRecord();

            if (! hexRecordLine.StartsWith(':')) {
                throw new IOException("Line should start wit ':'");
            }

            try {
                record.ByteCount = Int32.Parse(hexRecordLine.Substring(1,2),System.Globalization.NumberStyles.HexNumber);
                record.Address = Int32.Parse(hexRecordLine.Substring(3,4),System.Globalization.NumberStyles.HexNumber);
                switch (hexRecordLine.Substring(7,2)) {
                    case "00":
                        record.RecordType = RecordType.Data;
                        break;
                    case "01":
                        record.RecordType = RecordType.EOF;
                        break;
                    case "02":
                        record.RecordType = RecordType.ExtendedSegmentAddress;
                        break;
                    case "03":
                        record.RecordType = RecordType.StartSegmentAddress;
                        break;
                    case "04":
                        record.RecordType = RecordType.ExtendedLinearAddress;
                        break;
                    case "05":
                        record.RecordType = RecordType.StartLinearAddress;
                        break;
                    default:
                        throw new FormatException();
                };
                record.Data = new byte[record.ByteCount];
                for (int i=0; i<record.ByteCount; i++) {
                    record.Data[i] = Byte.Parse(hexRecordLine.Substring(9+2*i,2),System.Globalization.NumberStyles.HexNumber);
                }

                record.Checksum = Int32.Parse(hexRecordLine.Substring(9+2*record.ByteCount,2),System.Globalization.NumberStyles.HexNumber);


            } catch (FormatException) {
                Console.WriteLine("Cannot parse line");
                throw;
            }

            return record;
        }       
    }
}
