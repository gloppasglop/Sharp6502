namespace C6502
{

    public enum BankType {
        RAM,
        ROM,
        IO,
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

        public uint[] CharRom = new uint[4*1024]; 

        public uint[] KernalRom = new uint[8*1024];
        public uint[] BasicRom = new uint[28*1024];
        private uint[] ColorRam = new uint[1024];

        public uint[] CartridgeRomLo = new uint[8*1024];
        public uint[] CartridgeRomHi = new uint[8*1024];

        public BankZone[] BankZones {get; set;}

        private BankZone[] MemoryMapping;

        public void ConfigureBanks(int mode) {

            switch(mode) {
                case 31: {
                    BankZones[0].BankType = BankType.RAM;
                    
                    BankZones[1].BankType = BankType.RAM;
                    
                    BankZones[2].BankType = BankType.RAM;
                    
                    BankZones[3].BankType = BankType.ROM;
                    BankZones[3].Mem = BasicRom;
                    
                    BankZones[4].BankType = BankType.RAM;
                    
                    BankZones[5].BankType = BankType.IO;
                    
                    BankZones[6].BankType = BankType.ROM;
                    BankZones[6].Mem = KernalRom;

                    break;
                }
                case 30: {
                    BankZones[0].BankType = BankType.RAM;
                    
                    BankZones[1].BankType = BankType.RAM;
                    
                    BankZones[2].BankType = BankType.RAM;
                    
                    BankZones[3].BankType = BankType.RAM;
                    
                    BankZones[4].BankType = BankType.RAM;
                    
                    BankZones[5].BankType = BankType.IO;
                    
                    BankZones[6].BankType = BankType.ROM;
                    BankZones[6].Mem = KernalRom;

                    break;
                }
                case 29: {
                    BankZones[0].BankType = BankType.RAM;
                    
                    BankZones[1].BankType = BankType.RAM;
                    
                    BankZones[2].BankType = BankType.RAM;
                    
                    BankZones[3].BankType = BankType.RAM;
                    
                    BankZones[4].BankType = BankType.RAM;
                    
                    BankZones[5].BankType = BankType.IO;
                    
                    BankZones[6].BankType = BankType.RAM;

                    break;
                }
                case 28: {
                    BankZones[0].BankType = BankType.RAM;
                    
                    BankZones[1].BankType = BankType.RAM;
                    
                    BankZones[2].BankType = BankType.RAM;
                    
                    BankZones[3].BankType = BankType.RAM;
                    
                    BankZones[4].BankType = BankType.RAM;
                    
                    BankZones[5].BankType = BankType.RAM;
                    
                    BankZones[6].BankType = BankType.RAM;
                    break;
                }
                case 27: {
                    BankZones[0].BankType = BankType.RAM;
                    
                    BankZones[1].BankType = BankType.RAM;
                    
                    BankZones[2].BankType = BankType.RAM;
                    
                    BankZones[3].BankType = BankType.ROM;
                    BankZones[3].Mem = BasicRom;
                    
                    BankZones[4].BankType = BankType.RAM;

                    BankZones[5].BankType = BankType.ROM;
                    BankZones[5].Mem = CharRom;
                    
                    BankZones[6].BankType = BankType.ROM;
                    BankZones[6].Mem = KernalRom;
                    break;
                }
                case 26: {
                    BankZones[0].BankType = BankType.RAM;
                    
                    BankZones[1].BankType = BankType.RAM;
                    
                    BankZones[2].BankType = BankType.RAM;
                    
                    BankZones[3].BankType = BankType.RAM;
                    
                    BankZones[4].BankType = BankType.RAM;

                    BankZones[5].BankType = BankType.ROM;
                    BankZones[5].Mem = CharRom;
                    
                    BankZones[6].BankType = BankType.ROM;
                    BankZones[6].Mem = KernalRom;
                    break;
                }
                case 25: {
                    BankZones[0].BankType = BankType.RAM;
                    
                    BankZones[1].BankType = BankType.RAM;
                    
                    BankZones[2].BankType = BankType.RAM;
                    
                    BankZones[3].BankType = BankType.RAM;
                    
                    BankZones[4].BankType = BankType.RAM;

                    BankZones[5].BankType = BankType.ROM;
                    BankZones[5].Mem = CharRom;
                    
                    BankZones[6].BankType = BankType.RAM;
                    break;
                }
                case 24: {
                    BankZones[0].BankType = BankType.RAM;
                    
                    BankZones[1].BankType = BankType.RAM;
                    
                    BankZones[2].BankType = BankType.RAM;
                    
                    BankZones[3].BankType = BankType.RAM;
                    
                    BankZones[4].BankType = BankType.RAM;

                    BankZones[5].BankType = BankType.RAM;
                    
                    BankZones[6].BankType = BankType.RAM;
                    break;
                }
                case 23:
                case 22:
                case 21:
                case 20:
                case 19:
                case 18:
                case 17:
                case 16: {
                    BankZones[0].BankType = BankType.RAM;
                    
                    BankZones[1].BankType = BankType.UNMAPPED;
                    
                    BankZones[2].BankType = BankType.ROM;
                    BankZones[2].Mem = CartridgeRomLo;
                    
                    BankZones[3].BankType = BankType.UNMAPPED;
                    
                    BankZones[4].BankType = BankType.UNMAPPED;

                    BankZones[5].BankType = BankType.IO;
                    
                    BankZones[6].BankType = BankType.ROM;
                    BankZones[6].Mem = CartridgeRomHi;

                    break;
                }
                case 15: {
                    BankZones[0].BankType = BankType.RAM;
                    
                    BankZones[1].BankType = BankType.RAM;
                    
                    BankZones[2].BankType = BankType.ROM;
                    BankZones[2].Mem = CartridgeRomLo;
                    
                    BankZones[3].BankType = BankType.ROM;
                    BankZones[3].Mem = BasicRom;
                    
                    BankZones[4].BankType = BankType.RAM;

                    BankZones[5].BankType = BankType.IO;
                    
                    BankZones[6].BankType = BankType.ROM;
                    BankZones[6].Mem = KernalRom;
                    break;
                }
                case 14: {
                    BankZones[0].BankType = BankType.RAM;
                    
                    BankZones[1].BankType = BankType.RAM;
                    
                    BankZones[2].BankType = BankType.RAM;
                    
                    BankZones[3].BankType = BankType.RAM;
                    
                    BankZones[4].BankType = BankType.RAM;

                    BankZones[5].BankType = BankType.IO;
                    
                    BankZones[6].BankType = BankType.ROM;
                    BankZones[6].Mem = KernalRom;
                    break;
                }
                case 13: {
                    BankZones[0].BankType = BankType.RAM;
                    
                    BankZones[1].BankType = BankType.RAM;
                    
                    BankZones[2].BankType = BankType.RAM;
                    
                    BankZones[3].BankType = BankType.RAM;
                    
                    BankZones[4].BankType = BankType.RAM;

                    BankZones[5].BankType = BankType.IO;
                    
                    BankZones[6].BankType = BankType.RAM;

                    break;

                }
                case 12: {
                    BankZones[0].BankType = BankType.RAM;
                    
                    BankZones[1].BankType = BankType.RAM;
                    
                    BankZones[2].BankType = BankType.RAM;
                    
                    BankZones[3].BankType = BankType.RAM;
                    
                    BankZones[4].BankType = BankType.RAM;

                    BankZones[5].BankType = BankType.RAM;
                    
                    BankZones[6].BankType = BankType.RAM;

                    break;
                }
                case 11: {
                    BankZones[0].BankType = BankType.RAM;
                    
                    BankZones[1].BankType = BankType.RAM;
                    
                    BankZones[2].BankType = BankType.ROM;
                    BankZones[2].Mem = CartridgeRomLo;

                    BankZones[3].BankType = BankType.ROM;
                    BankZones[3].Mem = BasicRom;
                    
                    BankZones[4].BankType = BankType.RAM;

                    BankZones[5].BankType = BankType.ROM;
                    BankZones[5].Mem = CharRom;
                    
                    BankZones[6].BankType = BankType.ROM;
                    BankZones[6].Mem = KernalRom;

                    break;
                }
                case 10: {
                    BankZones[0].BankType = BankType.RAM;
                    
                    BankZones[1].BankType = BankType.RAM;
                    
                    BankZones[2].BankType = BankType.RAM;
                    
                    BankZones[3].BankType = BankType.RAM;
                    
                    BankZones[4].BankType = BankType.RAM;

                    BankZones[5].BankType = BankType.ROM;
                    BankZones[5].Mem = CharRom;
                    
                    BankZones[6].BankType = BankType.RAM;

                    break;                    
                }
                case 09: {
                    BankZones[0].BankType = BankType.RAM;
                    
                    BankZones[1].BankType = BankType.RAM;
                    
                    BankZones[2].BankType = BankType.RAM;
                    
                    BankZones[3].BankType = BankType.RAM;
                    
                    BankZones[4].BankType = BankType.RAM;

                    BankZones[5].BankType = BankType.ROM;
                    BankZones[5].Mem = CharRom;
                    
                    BankZones[6].BankType = BankType.RAM;

                    break;
                }
                case 08: {
                    BankZones[0].BankType = BankType.RAM;
                    
                    BankZones[1].BankType = BankType.RAM;
                    
                    BankZones[2].BankType = BankType.RAM;
                    
                    BankZones[3].BankType = BankType.RAM;
                    
                    BankZones[4].BankType = BankType.RAM;

                    BankZones[5].BankType = BankType.RAM;
                    
                    BankZones[6].BankType = BankType.RAM;

                    break;
                }
                case 07: {
                    BankZones[0].BankType = BankType.RAM;
                    
                    BankZones[1].BankType = BankType.RAM;
                    
                    BankZones[2].BankType = BankType.ROM;
                    BankZones[2].Mem = CartridgeRomLo;

                    BankZones[3].BankType = BankType.ROM;
                    BankZones[3].Mem = CartridgeRomHi;

                    BankZones[4].BankType = BankType.RAM;

                    BankZones[5].BankType = BankType.IO;
                    
                    BankZones[6].BankType = BankType.ROM;
                    BankZones[6].Mem = KernalRom;

                    break;
                }
                case 06: {
                    BankZones[0].BankType = BankType.RAM;
                    
                    BankZones[1].BankType = BankType.RAM;
                    
                    BankZones[2].BankType = BankType.RAM;

                    BankZones[3].BankType = BankType.ROM;
                    BankZones[3].Mem = CartridgeRomHi;

                    BankZones[4].BankType = BankType.RAM;

                    BankZones[5].BankType = BankType.IO;
                    
                    BankZones[6].BankType = BankType.ROM;
                    BankZones[6].Mem = KernalRom;

                    break;
                }
                case 05: {
                    BankZones[0].BankType = BankType.RAM;
                    
                    BankZones[1].BankType = BankType.RAM;
                    
                    BankZones[2].BankType = BankType.RAM;

                    BankZones[3].BankType = BankType.RAM;

                    BankZones[4].BankType = BankType.RAM;

                    BankZones[5].BankType = BankType.IO;
                    
                    BankZones[6].BankType = BankType.RAM;

                    break;

                }
                case 04: {
                    BankZones[0].BankType = BankType.RAM;
                    
                    BankZones[1].BankType = BankType.RAM;
                    
                    BankZones[2].BankType = BankType.RAM;

                    BankZones[3].BankType = BankType.RAM;

                    BankZones[4].BankType = BankType.RAM;

                    BankZones[5].BankType = BankType.RAM;
                    
                    BankZones[6].BankType = BankType.RAM;

                    break;
                }
                case 03: {
                    BankZones[0].BankType = BankType.RAM;
                    
                    BankZones[1].BankType = BankType.RAM;
                    
                    BankZones[2].BankType = BankType.ROM;
                    BankZones[2].Mem = CartridgeRomLo;

                    BankZones[3].BankType = BankType.ROM;
                    BankZones[3].Mem = CartridgeRomHi;

                    BankZones[4].BankType = BankType.RAM;

                    BankZones[5].BankType = BankType.ROM;
                    BankZones[5].Mem = CharRom;
                    
                    BankZones[6].BankType = BankType.ROM;
                    BankZones[6].Mem = KernalRom;
                    break;
                }
                case 02: {
                    BankZones[0].BankType = BankType.RAM;
                    
                    BankZones[1].BankType = BankType.RAM;
                    
                    BankZones[2].BankType = BankType.RAM;

                    BankZones[3].BankType = BankType.ROM;
                    BankZones[3].Mem = CartridgeRomHi;

                    BankZones[4].BankType = BankType.RAM;

                    BankZones[5].BankType = BankType.ROM;
                    BankZones[5].Mem = CharRom;
                    
                    BankZones[6].BankType = BankType.ROM;
                    BankZones[6].Mem = KernalRom;

                    break;
                }
                case 01:
                case 00: {
                    BankZones[0].BankType = BankType.RAM;
                    
                    BankZones[1].BankType = BankType.RAM;
                    
                    BankZones[2].BankType = BankType.RAM;

                    BankZones[3].BankType = BankType.RAM;

                    BankZones[4].BankType = BankType.RAM;

                    BankZones[5].BankType = BankType.RAM;
                    
                    BankZones[6].BankType = BankType.RAM;

                    break;
                }

            }
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

        public uint ReadCharRom(uint addr) {
            return CharRom[addr];
        }
        
        public Memory(){
            Mem = new uint[65536];
            MemoryMapping = new BankZone[65536];

            BankZones = new BankZone[7];
            BankZones[0] = new BankZone{
                StartAddress = 0x0000,
                EndAddress = 0x0FFF,
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
                BankType = BankType.IO
            };
            BankZones[6] = new BankZone{
                StartAddress = 0xE000,
                EndAddress = 0xFFFF,
                Mem = new uint[8192],
                BankType = BankType.RAM
            };

        // TODO : Configure based on PLA Latch Bits
        // https://www.c64-wiki.com/wiki/Bank_Switching
        ConfigureBanks(31);
        
        }

        public uint Read(uint addr) {
            // Check Bank
            // If ROM, read from ROM bank
            // If RAM, read from Mem
            if (MemoryMapping[addr].BankType == BankType.ROM) {
                return MemoryMapping[addr].Mem[addr-MemoryMapping[addr].StartAddress];
            }
            if (MemoryMapping[addr].BankType == BankType.IO) {

                // TODO: Should we implement something more generic?

                // 0xD000-0xD3FF - VIC Registers
                if ( addr <= 0xD3FF) {
                    // Repeated every 64 bytes
                    // 0x2f to 0x3f return ff
                    if ( ( addr & 0x3F) >= 0x2F) {
                        return 0xFF;
                    }
                    addr = 0xD3FF & (addr & 0xFF3F);
                    return MemoryMapping[addr].Mem[addr-MemoryMapping[addr].StartAddress];
                }

                return MemoryMapping[addr].Mem[addr-MemoryMapping[addr].StartAddress];
            }
            return Mem[addr];
        }

        public void Write(uint addr, uint value) {
            // Check Bank
            // If ROM write to RAM under
            // If RAM, write to Mem
            if (MemoryMapping[addr].BankType == BankType.IO) {

                // TODO: Should we implement something more generic?

                // 0xD000-0xD3FF - VIC Registers
                if ( addr <= 0xD3FF) {
                    // Repeated every 64 bytes. handled by memory read
                    // so only write in the first 64 bytes range
                    addr = 0xD3FF & (addr & 0xFF3F);
                    MemoryMapping[addr].Mem[addr-MemoryMapping[addr].StartAddress]=value;
                }

                MemoryMapping[addr].Mem[addr-MemoryMapping[addr].StartAddress]=value;
            } else {
                // Not sure this is correct for RAM/ROM
                Mem[addr] = value;
            }
        }

    }
}