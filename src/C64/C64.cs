using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using System;
using System.Diagnostics;
using System.IO;
using HexParser;
using C6502;
using CIA6526;

namespace C64
{
    class Computer
    {

        public Memory Mem {get; set;}
        public Cpu Cpu { get; set; }
        public VIC6569.Chip Vic { get; set; }
        public CIA6526.Chip CIA1 { get; set; }
        public CIA6526.Chip CIA2 { get; set; }
    	public ulong TickCount {get;private set;}

	    public Boolean Debug = true;

	    public byte[] Screen()
        {
		    return Vic.Screen;
	    }

        static void DumpState(ulong tick, Cpu cpu) {
            Console.WriteLine("{0,20} {11,1} {10,2:X2} {1,8:X4} {2,4:X2} {3,2} {4,6:X4} {5,4:X2} {6,4:X2} {7,4:X2} {8,4:X2} {9} {12,6}", 
                        tick,
                        cpu.AddrPins, 
                        cpu.DataPins, 
                        cpu.RW,
                        cpu.PC,
                        cpu.A,
                        cpu.X,
                        cpu.Y,
                        cpu.S,
                        Convert.ToString(cpu.P,2).PadLeft(8,'0'),
                        cpu.IR?.Opcode,
                        cpu._opcycle,
                        cpu.IRQ
                    );
        }

    	public Computer(string kernalPath, string basicPath, string chargenPath)
    	{
            uint breakPoint = 0x10000;
            uint offset = 0;
	        Mem = new Memory();

            byte[] kernalBytes = File.ReadAllBytes(kernalPath);
            offset = 0;
            foreach (uint data in kernalBytes)
            {
                Mem.KernalRom[offset++] = data;
            }
            
            byte[] basicBytes = File.ReadAllBytes(basicPath);
            offset = 0;
            foreach (uint data in basicBytes)
            {
                Mem.BasicRom[offset++] = data;
            }

            byte[] chargenBytes = File.ReadAllBytes(chargenPath);
            offset = 0;
            foreach (uint data in chargenBytes)
            {
                Mem.CharRom[offset++] = data;
            }

            Cpu = new Cpu(Mem);
            Vic = new VIC6569.Chip(0,Mem);
            CIA1 = new CIA6526.Chip();
            CIA1.Init();

            CIA2 = new CIA6526.Chip();
            CIA2.Init();
            
            Cpu.PC = 0x0400;
            Cpu.AddrPins = Cpu.PC;
            Cpu.DataPins = Mem.Read(Cpu.PC);
            Cpu.RES = true;
            
            TickCount = 0; 

            //Console.WriteLine("Starting CPU!");
            /*Console.WriteLine("{0,20} {1,8:X4} {2,4:X2} {3,2} {4,6:X4} {5,4:X2} {6,4:X2} {7,4:X2} {8,4:X2}", 
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
	    */

            uint previousPC= 0x1FFFF;
            Debug = false;
            //var stopwatch = new Stopwatch();
            //stopwatch.Start();
	}

    	public void Tick()
	    {
            if ( (TickCount % 1_000_000U) == 0) {
                Console.WriteLine("TIC");
            }
            if ((TickCount %2) == 0)
		    {
                    //Console.WriteLine("------ Half Cyle : {0,20} ------", tick/2);
            }
            Cpu.RDY = Vic.BA;
            Cpu.PHY2 = Vic.PHY0;
            Cpu.AEC = Vic.AEC;
                
            if (Cpu.PHY2)
		    {
			    // if RDY is low and we are not writing
			    // return
                if ( Cpu.RDY || !Cpu.RW )
		        {
				    Cpu.Tick();

                }

            var previousPC = Cpu.PC;
            //Console.WriteLine("{0} {1} {2}",mem.Read(0x200),tick,cpu.PC);
                    

            }
            if ( !Cpu.PHY2 || !Cpu.RDY)
            {
                // VIC 
                //vic.SetRegistersFromMem(mem);
                //Console.WriteLine("VIC: {0,2} {1}",vic._opcycle,vic.PHY0);
                Vic.Tick();
                // TODO: Hardcoding to Bank0 vic mapping for now
                //vic.WriteRegistersToMem(mem);

            }


            CIA1.PHI2 = Cpu.PHY2;
            CIA1.RW = Cpu.RW;
            CIA1.RS = Cpu.AddrPins & 0x000F;
            CIA1.CS = (Cpu.AddrPins & 0xFF00) == 0xDC00;

            CIA2.PHI2 = Cpu.PHY2;
            CIA2.RW = Cpu.RW;
            CIA2.RS = Cpu.AddrPins & 0x000F;
            CIA2.CS = (Cpu.AddrPins & 0xFF00) == 0xDD00;

            if (Cpu.RW) {
                CIA1.Tick(); 
                if (CIA1.CS) { 
                    Cpu.mem.Write(0xDC00 | CIA1.RS, CIA1.DataPins);
                }
            } else {
                if (CIA1.CS) {
                    CIA1.DataPins = Cpu.mem.Read(0xDC00 | CIA1.RS);
                }
                CIA1.Tick(); 
            }
            Cpu.IRQ = CIA1.IRQ;

            if (Cpu.RW) {
                CIA2.Tick(); 
                if (CIA2.CS) { 
                    Cpu.mem.Write(0xDD00 | CIA2.RS, CIA2.DataPins);
                }
            } else {
                if (CIA2.CS) {
                    CIA2.DataPins = Cpu.mem.Read(0xDD00 | CIA2.RS);
                }
                CIA2.Tick(); 
            }

            if (Debug) {
                DumpState(TickCount, Cpu);
            }


            //Vic.GraphicsDataSequencer();
            Vic.PHY0 = !Vic.PHY0;
            //if ( !Vic.PHY0)
            //{
                //Vic._opcycle++;
            //    Vic.Tick();
            //}
            TickCount++;

            //stopwatch.Stop();
            //Console.WriteLine("Total number of cycles: {0}", tick);
            //Console.WriteLine("Elapsed time: {0}", stopwatch.ElapsedMilliseconds);
            //Console.WriteLine("Frequency: {0}", (long) tick/1000.0/stopwatch.ElapsedMilliseconds);
        }
    }
}