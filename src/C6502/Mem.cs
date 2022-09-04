namespace C6502
{

    public enum BankType {
        RAM,
        ROM,
        UNMAPPED
    }
    public class BankZone {
        public uint StartAddress { get; set;}
        public uint EndAddress { get; set; }
        public uint[] Mem {get; set;}

        public BankType BankType {get; set;}


    }

    public class Memory {
        private uint[] Mem { get; set;}

        public BankZone[] BankZones {get; set;}

        private BankZone[] MemoryMapping;
        
        public Memory(){
            Mem = new uint[65536];
            MemoryMapping = new BankZone[65536];

            BankZones = new BankZone[7];
            BankZones[0] = new BankZone{
                StartAddress = 0x0000,
                EndAddress = 0x0FFF,
                Mem = new uint[4096],
                BankType = BankType.RAM
            };
            BankZones[1] = new BankZone{
                StartAddress = 0x1000,
                EndAddress = 0x7FFF,
                Mem = new uint[28*1024],
                BankType = BankType.RAM
            };
            BankZones[2] = new BankZone{
                StartAddress = 0x8000,
                EndAddress = 0x9FFF,
                Mem = new uint[8192],
                BankType = BankType.RAM
            };
            BankZones[3] = new BankZone{
                StartAddress = 0xA000,
                EndAddress = 0xBFFF,
                Mem = new uint[8192],
                BankType = BankType.RAM
            };
            BankZones[4] = new BankZone{
                StartAddress = 0xC000,
                EndAddress = 0xCFFF,
                Mem = new uint[4096],
                BankType = BankType.RAM
            };
            BankZones[5] = new BankZone{
                StartAddress = 0xD000,
                EndAddress = 0xDFFF,
                Mem = new uint[4096],
                BankType = BankType.RAM
            };
            BankZones[6] = new BankZone{
                StartAddress = 0xE000,
                EndAddress = 0xFFFF,
                Mem = new uint[8192],
                BankType = BankType.RAM
            };
            /* BankZones[0] = new BankZone{
                StartAddress = 0x0000,
                EndAddress = 0x0FFF,
                Mem = new uint[4096],
                BankType = BankType.RAM
            };*/

            // Mapping address to bank
            for (uint addr=0; addr<=0x10000; addr++) {
                foreach (var bz in BankZones) {
                    if ( (addr >= bz.StartAddress ) && (addr <= bz.EndAddress)){
                        MemoryMapping[addr] = bz;
                        continue;
                    }
                }
            }          
        }

        public uint Read(uint addr) {
            // Check Bank
            // If ROM, read from ROM bank
            // If RAM, read from Mem
            if (MemoryMapping[addr].BankType == BankType.ROM) {
                return MemoryMapping[addr].Mem[addr-MemoryMapping[addr].StartAddress];
            }
            return Mem[addr];
        }

        public void Write(uint addr, uint value) {
            // Check Bank
            // If ROM write to RAM under
            // If RAM, write to Mem
            Mem[addr] = value;
        }

    }
}