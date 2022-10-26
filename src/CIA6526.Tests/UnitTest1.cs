using System;
using Xunit;
using CIA6526;

namespace CIA6526.Tests
{

    public class Computer {

        public CIA6526.Chip cia1;

        public Computer() {
            cia1 = new CIA6526.Chip();
        }
        public int Execute(int cycle) {
            int tick = 0;
            while (tick < cycle) {
                cia1.Tick();

                tick++;
            }
            return tick;
        }
        /*
        public Cpu Clone() {
            var newCpu = new Cpu(new Memory());
            newCpu.PC = cpu.PC;
            newCpu.A = cpu.A;
            newCpu.X = cpu.X;
            newCpu.Y = cpu.Y;
            newCpu.S = cpu.S;
            newCpu.P = cpu.P;
            return newCpu;
        }
        */

    }

}
