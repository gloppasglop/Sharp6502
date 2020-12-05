using System;
using System.Diagnostics;
using System.IO;

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
            Cpu cpu = new Cpu();
            Memory mem = new Memory();

    


            uint offset = 0;
            mem.Write(offset++,0xA9);
            mem.Write(offset++,0x00);
            mem.Write(offset++,0x20);
            mem.Write(offset++,0x10);
            mem.Write(offset++,0x00);
            mem.Write(offset++,0x4C);
            mem.Write(offset++,0x02);
            mem.Write(offset++,0x00);

            mem.Write(0x0F,0x40);

            


            offset = 0x10;
            mem.Write(offset++,0xE8);
            mem.Write(offset++,0x88);
            mem.Write(offset++,0xE6);
            mem.Write(offset++,0x0F);
            mem.Write(offset++,0x38);
            mem.Write(offset++,0x69);
            mem.Write(offset++,0x02);
            mem.Write(offset++,0x60);


            byte[] fileBytes = File.ReadAllBytes("../../tests/6502_functional_test.bin");

            //byte[] fileBytes = File.ReadAllBytes("../../tests/test.bin");
            offset = 0x000A;

            foreach (uint data in fileBytes) {
                mem.Write(offset++,data);
            }

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
                cpu.Tick();

                if ( cpu.RW) {
                    // Read Data from memory and put them on the data bus
                    cpu.DataPins = mem.Read(cpu.AddrPins);

                } else {
                    // Write Data from databus into memory
                    mem.Write(cpu.AddrPins,cpu.DataPins);
                }

                tick++;
                if ( debug) { DumpState(tick,cpu); }
                if ((cpu._opcycle == 1) && (previousPC == cpu.PC)) {
                    Console.WriteLine("PC di not change");
                }
                if (cpu.PC == 0x3375) {
                    break;
                }
                if (cpu.PC == 0x09c5) {
                    debug = true;
                }
                previousPC = cpu.PC;
            }
            stopwatch.Stop();
            Console.WriteLine("Total number of cycles: {0}", tick);
            Console.WriteLine("Elapsed time: {0}", stopwatch.ElapsedMilliseconds);
            Console.WriteLine("Frequency: {0}", (long) tick/1000.0/stopwatch.ElapsedMilliseconds);

        }
   }
}