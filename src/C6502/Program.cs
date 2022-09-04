using System;
using System.Diagnostics;
using System.IO;
using HexParser;

namespace C6502
{
    class Program
    {

        static void DumpState(ulong tick, Cpu cpu) {
            Console.WriteLine("{0,20} {11,1} {10,2:X2} {1,8:X4} {2,4:X2} {3,2} {4,6:X4} {5,4:X2} {6,4:X2} {7,4:X2} {8,4:X2} {9}", 
                tick,
                cpu.AddrPins, 
                cpu.DataPins, 
                Convert.ToInt32(cpu.RW),
                cpu.PC,
                cpu.A,
                cpu.X,
                cpu.Y,
                cpu.S,
                Convert.ToString(cpu.P,2).PadLeft(8,'0'),
                cpu.IR.Opcode,
                cpu._opcycle
                );
        }
        
        static void Main(string[] args)
        {

            uint offset = 0;
            Cpu cpu = new Cpu();
            Memory mem = new Memory();

            /*
            string fileName = "../../tests/6502_functional_test.hex";
            HexReader parse = new HexReader(fileName);
            foreach (var line in parse.hexContent) {
                HexRecord record =  parse.ParseLine(line);
                if (record.RecordType == RecordType.Data) {
                    offset = (uint) record.Address;
                    for (var i = 0; i < record.Data.Length; i++ ) {
                        mem.Write(offset+(uint) i,record.Data[i]);
                    }
                }
            }
            */
      


            byte[] kernalBytes = File.ReadAllBytes("C:\\Users\\glopp\\Downloads\\GTK3VICE-3.4-win64-r39109\\GTK3VICE-3.4-win64-r39109\\C64\\kernal");
            offset = 0;
            mem.BankZones[6].BankType = BankType.ROM;
            foreach (uint data in kernalBytes) {
                mem.BankZones[6].Mem[offset++] = data;
            }
            
            byte[] basicBytes = File.ReadAllBytes("C:\\Users\\glopp\\Downloads\\GTK3VICE-3.4-win64-r39109\\GTK3VICE-3.4-win64-r39109\\C64\\basic");
            offset = 0;
            mem.BankZones[3].BankType = BankType.ROM;
            foreach (uint data in basicBytes) {
                mem.BankZones[3].Mem[offset++] = data;
            }


            byte[] fileBytes = File.ReadAllBytes("../../tests/6502_functional_test.bin");
            offset = 0x000A;

            foreach (uint data in fileBytes) {
                mem.Write(offset++,data);
            }

            cpu.RES = true;
            cpu.PC = 0x0400;
            cpu.AddrPins = cpu.PC;
            cpu.DataPins = mem.Read(cpu.PC);
            
            ulong tick = 0; 

            Console.WriteLine("Starting CPU!");
            Console.WriteLine("{0,20} {1,8:X4} {2,4:X2} {3,2} {4,6:X4} {5,4:X2} {6,4:X2} {7,4:X2} {8,4:X2}", 
                "tick", 
                "ab", 
                "db", 
                "rw",
                "pc",
                "a",
                "x",
                "y",
                "s"
                );

            //DumpState(tick,cpu);

            uint previousPC= 0x1FFFF;
            bool debug = false;
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            while(true) {
                
                if (cpu.PHY2) {
                    // 6502 Access
                    cpu.Tick();

                    if (debug) {
                    }
                    

                    if ( cpu.RW) {
                        // Read Data from memory and put them on the data bus
                        cpu.DataPins = mem.Read(cpu.AddrPins);

                    } else {
                        // Write Data from databus into memory
                        mem.Write(cpu.AddrPins,cpu.DataPins);
                    }

                    tick++;
                    if ( debug) {
                        DumpState(tick,cpu);
                    }

                    // For functional test end
                    //if (cpu.PC == 0x3375) {
                    //    break;
                    //}
                    
                    if (cpu.PC == 0xFF5F) {
                        Console.WriteLine("DEBUG");  
                        debug = true;                
                    }


                    previousPC = cpu.PC;
                    //Console.WriteLine("{0} {1} {2}",mem.Read(0x200),tick,cpu.PC);
                    

                } else {
                    // VIC 
                }

                cpu.PHY2 = !cpu.PHY2;
            }

            stopwatch.Stop();
            Console.WriteLine("Total number of cycles: {0}", tick);
            Console.WriteLine("Elapsed time: {0}", stopwatch.ElapsedMilliseconds);
            Console.WriteLine("Frequency: {0}", (long) tick/1000.0/stopwatch.ElapsedMilliseconds);
        }
    }
}