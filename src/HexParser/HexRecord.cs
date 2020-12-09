using System;

namespace HexParser
{
    public enum RecordType {
        Data,
        EOF,
        ExtendedSegmentAddress,
        StartSegmentAddress,
        ExtendedLinearAddress,
        StartLinearAddress
    }
    public class HexRecord
    {
        public int ByteCount {get; set;}
        public int Address {get; set; }
        public RecordType RecordType {get; set;} 
        public byte[] Data { get; set;}
        public int Checksum {get; set;}
    }
}
