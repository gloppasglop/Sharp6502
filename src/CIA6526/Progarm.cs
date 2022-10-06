using System;
using System.IO;
using CIA6526;

namespace CIA6526
{

    class Program
    {
	static void DumpState()
	{

	}
	static void Main(string[] args)
        {
		var chip = new CIA6526.Chip();

		chip.PHI2 = true;
		chip.RES = false;
		chip.Tick();
		chip.RES = true;
		chip.PHI2 = ! chip.PHI2;

		chip.DataPins = 0xFF;
		chip.RS = 0xE;
		chip.RW = false;
		chip.Tick();
		chip.PHI2 = ! chip.PHI2;

		while ( true) {
			chip.Tick();
			chip.PHI2 = ! chip.PHI2;
			Console.WriteLine((chip.TAHI << 8) | chip.TALO );
		}
	}
    }
}