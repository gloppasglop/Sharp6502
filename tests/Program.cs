using System;
using System.IO;
using C6502;

namespace C6502Test
{

	class Program
    	{
		static void Main(string[] args)
		{
			Console.WriteLine("Running program");
			var computer = new C6502Test.Computer("6502_interrupt_essai.bin");
			computer.Debug = false;
			computer.Cpu.RES = false;
			computer.Cpu.IRQ = false;
			while (true) {
				computer.Tick();
				if (computer.TickCount == 2000-50) {
					computer.Debug = true;
				}
				if (computer.TickCount == 2000-20+5) {
					computer.Cpu.IRQ = true;
				}
			}
		}
        }
}
