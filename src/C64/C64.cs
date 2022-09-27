using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using System;
using System.Diagnostics;
using System.IO;
using HexParser;
using C6502;

namespace C64
{
    class Computer
    {

        public Memory Mem {get; set;}
        public Cpu Cpu { get; set; }
        public VIC6569.Chip Vic { get; set; }
    	public ulong TickCount {get;private set;}

	    public Boolean Debug = true;

	    public byte[] Screen()
        {
		    return Vic.Screen;
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
				    //Console.WriteLine("CPU: {0,2} {1}",cpu._opcycle,vic.PHY0);
				    // 6502 Access
				    Cpu.Tick();

				    if (Debug)
				    {

				    }
				
				    var breakPoint = 0xE5CD; // Wait for a key press
				    //breakPoint = 0xE422; // BASIC POWERUP MESSAGE
				    // breakPoint = 0xFF5B; // CINIT
				    // breakPoint = 0xFD52; // RAMTAS
				    // breakPoint = 0xEA12; // End of clear screen 
				    //breakPoint = 0xE564; // Memory TEST
				    if (Cpu.PC == breakPoint )
				    {
				        /* Console.WriteLine("DEBUG");  
				        for (int y=0; y<25; y++) {
				    	    for (int x=0; x<40;x++) {
				        	Console.Write("{0,2:X2} ", mem.Read( (uint) (1024+x+40*y)));
				        	}
				        	Console.WriteLine();
				        }
				        */

        				//WriteBitmapToPPM("test.ppm", 504,312, Vic.Screen);
        				//Debug = true;                
        				//break;
    				}
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