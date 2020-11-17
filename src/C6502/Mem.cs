namespace C6502
{

    public class Memory {
        private uint[] Mem { get; set;}
        
        public Memory(){
            Mem = new uint[65536];
        }

        public uint Read(uint addr) {
            return Mem[addr];
        }

        public void Write(uint addr, uint value) {
            Mem[addr] = value;
        }

    }
}