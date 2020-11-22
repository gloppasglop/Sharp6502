using System;
using Xunit;
using C6502;

namespace C6502.Tests
{


    public class JMP_ABSOLUTE
    {

        private Computer testComputer = new Computer();
        private uint opcode = 0x4C;
        private int cycles = 3;
        private int bytes = 3;


        [Fact]
        public void JMP_shouldwork()
        {
            testComputer.MemoryReset();
            
            uint A = 0x32;
            uint addr = 0x8000;

            testComputer.mem.Write(0x0000,opcode);
            testComputer.mem.Write(0x0001,addr & 0x00FF);
            testComputer.mem.Write(0x0002,addr >> 8);

            testComputer.CPUReset();

            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles);

            Assert.Equal(cpuCopy.A,testComputer.cpu.A);
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.Y,testComputer.cpu.Y);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            Assert.Equal(cpuCopy.P,testComputer.cpu.P);
            Assert.Equal(addr,testComputer.cpu.PC);
        }

    }
}
