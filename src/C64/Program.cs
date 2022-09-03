using System;
using System.Diagnostics;
using System.IO;
using HexParser;
using C6502;

namespace C64
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

        public static void WriteBitmapToPPM(string file, int width, int height, uint[] screen)
        {
            //Use a streamwriter to write the text part of the encoding
            var writer = new StreamWriter(file);
            writer.WriteLine("P6");
            writer.WriteLine($"{width}  {height}");
            writer.WriteLine("255");
            writer.Close();
            //Switch to a binary writer to write the data
            var writerB = new BinaryWriter(new FileStream(file, FileMode.Append));
            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                {
                    byte red = (byte) ((screen[x + y * width] & 0xFF0000) >> 16);
                    byte green = (byte)((screen[x + y * width] & 0x00FF00) >> 8);
                    byte blue = (byte)((screen[x + y * width] & 0x0000FF));

                    writerB.Write(red);
                    writerB.Write(green);
                    writerB.Write(blue);
                }
            writerB.Close();
        }

        static void Main(string[] args)
        {

            uint breakPoint = 0x10000;
            uint offset = 0;
            
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
      

            //byte[] fileBytes = File.ReadAllBytes("../../tests/6502_functional_test.bin");

            byte[] kernalBytes = File.ReadAllBytes("C:\\Users\\glopp\\Downloads\\GTK3VICE-3.4-win64-r39109\\GTK3VICE-3.4-win64-r39109\\C64\\kernal");
            offset = 0;
            foreach (uint data in kernalBytes) {
                mem.KernalRom[offset++] = data;
            }
            
            byte[] basicBytes = File.ReadAllBytes("C:\\Users\\glopp\\Downloads\\GTK3VICE-3.4-win64-r39109\\GTK3VICE-3.4-win64-r39109\\C64\\basic");
            offset = 0;
            foreach (uint data in basicBytes) {
                mem.BasicRom[offset++] = data;
            }

            byte[] chargenBytes = File.ReadAllBytes("C:\\Users\\glopp\\Downloads\\GTK3VICE-3.4-win64-r39109\\GTK3VICE-3.4-win64-r39109\\C64\\chargen");
            offset = 0;
            foreach (uint data in chargenBytes) {
                mem.CharRom[offset++] = data;
            }

            Cpu cpu = new Cpu(mem);
            VIC6569.Chip vic = new VIC6569.Chip(0,mem);
            
            cpu.PC = 0x0400;
            cpu.AddrPins = cpu.PC;
            cpu.DataPins = mem.Read(cpu.PC);
            cpu.RES = true;
            
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
            debug = false;
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            while(true) {
                if ((tick %2) == 0) {
                    //Console.WriteLine("------ Half Cyle : {0,20} ------", tick/2);
                }
                cpu.RDY = vic.BA;
                cpu.PHY2 = vic.PHY0;
                cpu.AEC = vic.AEC;
                
                if (cpu.PHY2) {
                    // if RDY is low and we are not writing
                    // return
                    if ( cpu.RDY || !cpu.RW ) {
                        //Console.WriteLine("CPU: {0,2} {1}",cpu._opcycle,vic.PHY0);
                        // 6502 Access
                        cpu.Tick();

                        if (debug) {

                        }
                        
                        /*
                        if ( cpu.RW ) {
                            // Read Data from memory and put them on the data bus
                            cpu.DataPins = mem.Read(cpu.AddrPins);

                        } else {
                            // Write Data from databus into memory
                            mem.Write(cpu.AddrPins,cpu.DataPins);
                        }

                        */
                        if ( debug) { DumpState(tick,cpu); }

                        // For functional test end
                        //if (cpu.PC == 0x3375) {
                        //    break;
                        //}
                        
                        breakPoint = 0xE5CD; // Wait for a key press
                        //breakPoint = 0xE422; // BASIC POWERUP MESSAGE
                        // breakPoint = 0xFF5B; // CINIT
                        // breakPoint = 0xFD52; // RAMTAS
                        // breakPoint = 0xEA12; // End of clear screen 
                        //breakPoint = 0xE564; // Memory TEST
                        if (cpu.PC == breakPoint ) {
                           /* Console.WriteLine("DEBUG");  
                            for (int y=0; y<25; y++) {
                                for (int x=0; x<40;x++) {
                                    Console.Write("{0,2:X2} ", mem.Read( (uint) (1024+x+40*y)));
                                }
                                Console.WriteLine();
                            }
                           */

                            WriteBitmapToPPM("test.ppm", 504,312, vic.Screen);
                            debug = true;                
                            //break;
                        }
                    }

                    previousPC = cpu.PC;
                    //Console.WriteLine("{0} {1} {2}",mem.Read(0x200),tick,cpu.PC);
                    

                }
                if ( !cpu.PHY2 || !cpu.RDY) {
                    // VIC 
                    //vic.SetRegistersFromMem(mem);
                    //Console.WriteLine("VIC: {0,2} {1}",vic._opcycle,vic.PHY0);
                    vic.Tick();
                    // TODO: Hardcoding to Bank0 vic mapping for now

                    //vic.WriteRegistersToMem(mem);

                }

                vic.PHY0 = !vic.PHY0;
                if ( !vic.PHY0) {
                    vic._opcycle++;
                }
                tick++;

            }

            stopwatch.Stop();
            Console.WriteLine("Total number of cycles: {0}", tick);
            Console.WriteLine("Elapsed time: {0}", stopwatch.ElapsedMilliseconds);
            Console.WriteLine("Frequency: {0}", (long) tick/1000.0/stopwatch.ElapsedMilliseconds);
        }
    }
}