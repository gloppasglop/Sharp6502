using System;
using Xunit;
using C6502;

namespace C6502.Tests
{


    public class STX_ZEROPAGE
    {

        private Computer testComputer = new Computer();
        private uint opcode = 0x86;
        private int cycles = 3;
        private int bytes = 2;


        [Fact]
        public void STX_ShouldWork()
        {
            testComputer.MemoryReset();
            
            // STX #AA
            uint X = 0x32;
            uint addr = 0xAA;

            testComputer.mem.Write(0x0000,opcode);
            testComputer.mem.Write(0x0001,addr);

            testComputer.CPUReset();
            testComputer.cpu.X = X;

            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles);

            Assert.Equal(X,testComputer.mem.Read(addr));
            Assert.Equal(cpuCopy.A,testComputer.cpu.A);
            Assert.Equal(cpuCopy.Y,testComputer.cpu.Y);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            Assert.Equal(cpuCopy.P,testComputer.cpu.P);
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }
    }
   public class STX_ZEROPAGEY
    {

        private Computer testComputer = new Computer();
        private uint opcode = 0x96;
        private int cycles = 4;
        private int bytes = 2;


        [Fact]
        public void STX_ShouldWork()
        {
            testComputer.MemoryReset();
            
            // STX #A0,Y
            uint Y = 0x0A;
            uint X = 0x32;
            uint addr = 0xA0;

            testComputer.mem.Write(0x0000,opcode);
            testComputer.mem.Write(0x0001,addr);

            testComputer.CPUReset();
            testComputer.cpu.Y = Y;
            testComputer.cpu.X = X;

            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles);

            Assert.Equal(X,testComputer.mem.Read(addr+Y));
            Assert.Equal(cpuCopy.A,testComputer.cpu.A);
            Assert.Equal(cpuCopy.Y,testComputer.cpu.Y);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            Assert.Equal(cpuCopy.P,testComputer.cpu.P);
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }

        [Fact]
        public void STX_IndexWrapShouldWork()
        {
            testComputer.MemoryReset();
            
            uint Y = 0xBB;
            uint X = 0x42;
            uint addr = 0xAA;

            testComputer.mem.Write(0x0000,opcode);
            testComputer.mem.Write(0x0001,addr);

            testComputer.CPUReset();
            testComputer.cpu.Y = Y;
            testComputer.cpu.X = X;


            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles);

            Assert.Equal(X,testComputer.mem.Read((addr+Y)& 0xFF));
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }

    }


    public class STX_ABSOLUTE
    {

        private Computer testComputer = new Computer();
        private uint opcode = 0x8E;
        private int cycles = 4;
        private int bytes = 3;


        [Fact]
        public void STX_ShouldWork()
        {
            testComputer.MemoryReset();
            
            uint X = 0x32;
            uint addr = 0xBEEF;

            testComputer.mem.Write(0x0000,opcode);
            testComputer.mem.Write(0x0001,addr & 0x00FF);
            testComputer.mem.Write(0x0002,addr >> 8);       

            testComputer.CPUReset();
            testComputer.cpu.X = X;

            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles);

            Assert.Equal(X,testComputer.mem.Read(addr));
            Assert.Equal(cpuCopy.A,testComputer.cpu.A);
            Assert.Equal(cpuCopy.Y,testComputer.cpu.Y);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            Assert.Equal(cpuCopy.P,testComputer.cpu.P);
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }

    }
    
}
