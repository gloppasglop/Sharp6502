using System;
using Xunit;
using C6502;

namespace C6502.Tests
{

    public class NOP
    {

        private Cpu cpu;
        private Memory mem;
        public NOP() {
            cpu = new Cpu();
            mem = new Memory();

        }

        private void CPUReset(){

            cpu.A = cpu.X = cpu.Y = 0;
            cpu.S = 0xFF;
            cpu.PC = 0x0000;
            cpu.AddrPins = cpu.PC;
            cpu.DataPins = mem.Read(cpu.PC);
        }

        private void MemoryReset(){
            for ( uint offset=0; offset < 65536; offset++) {
                mem.Write(offset++,0x0);
            }
        }

        private int Execute() {
            int tick = 0;
            while (cpu.DataPins != 0x0) {
                cpu.Tick();

                if ( cpu.RW) {
                    // Read Data from memory and put them on the data bus
                    cpu.DataPins = mem.Read(cpu.AddrPins);

                } else {
                    // Write Data from databus into memory
                    mem.Write(cpu.AddrPins,cpu.DataPins);
                }

                tick++;
            }
            return tick;
        }

        private Cpu Clone(Cpu oldCpu) {
            var newCpu = new Cpu();
            newCpu.PC = oldCpu.PC;
            newCpu.A = oldCpu.A;
            newCpu.X = oldCpu.X;
            newCpu.Y = oldCpu.Y;
            newCpu.S = oldCpu.S;
            newCpu.P = oldCpu.P;
            return newCpu;
        }

        [Fact]
        public void NOP_ShouldDoNothingAndConsumeOnecycle()
        {
            MemoryReset();
            mem.Write(0x0000,0xEA);
            CPUReset();
            var cpuCopy = Clone(cpu);


            int tick = Execute();

            Assert.Equal(1,tick);
            Assert.Equal(cpuCopy.A,cpu.A);
            Assert.Equal(cpuCopy.X,cpu.X);
            Assert.Equal(cpuCopy.Y,cpu.Y);
            Assert.Equal(cpuCopy.S,cpu.S);
            Assert.Equal(cpuCopy.P,cpu.P);
            Assert.Equal(cpuCopy.PC+1,cpu.PC);
        }
    }

    public class UnitTest2
    {
        [Fact]
        public void Test2()
        {

        }
    }
}
