using System;
using System.Diagnostics;
using System.IO;
using C6502;

namespace C6502Test
{
    class Computer
    {

        public C6502.Memory Mem {get; set;}
        public Cpu Cpu { get; set; }
    	public ulong TickCount {get;private set;}

	public Boolean Debug = false;

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

    	public Computer(string programPath)
    	{
            uint breakPoint = 0x10000;
            uint offset = 0x0a;
	        Mem = new C6502.Memory();
	        Mem.ConfigureBanks(0);

            byte[] programBytes = File.ReadAllBytes(programPath);
            foreach (uint data in programBytes)
            {
                Mem.Write(offset++, data);
            }


            Cpu = new Cpu(Mem);
            
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
        Cpu.RDY = true;
       	//Cpu.AEC = Vic.AEC;
                
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


            	if (Debug) {
                	DumpState(TickCount, Cpu);
            	}

        	Cpu.PHY2 = ! Cpu.PHY2;

            	TickCount++;

        }
    }
}