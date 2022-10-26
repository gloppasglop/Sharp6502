using System;
using Xunit;
using C6502;

namespace C6502.Tests
{


    public class STY_ZEROPAGE
    {

        private Computer testComputer = new Computer();
        private uint opcode = 0x84;
        private int cycles = 3;
        private int bytes = 2;


        [Fact]
        public void STY_ShouldWork()
        {
            testComputer.MemoryReset();
            
            // STY #AA
            uint Y = 0x32;
            uint addr = 0xAA;

            testComputer.mem.Write(0x0000,opcode);
            testComputer.mem.Write(0x0001,addr);

            testComputer.CPUReset();
            testComputer.cpu.Y = Y;

            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles);

            Assert.Equal(Y,testComputer.mem.Read(addr));
            Assert.Equal(cpuCopy.A,testComputer.cpu.A);
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            Assert.Equal(cpuCopy.P,testComputer.cpu.P);
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }
    }
   public class STY_ZEROPAGEX
    {

        private Computer testComputer = new Computer();
        private uint opcode = 0x94;
        private int cycles = 4;
        private int bytes = 2;


        [Fact]
        public void STY_ShouldWork()
        {
            testComputer.MemoryReset();
            
            // STY #A0,Y
            uint X = 0x0A;
            uint Y = 0x32;
            uint addr = 0xA0;

            testComputer.mem.Write(0x0000,opcode);
            testComputer.mem.Write(0x0001,addr);

            testComputer.CPUReset();
            testComputer.cpu.Y = Y;
            testComputer.cpu.X = X;

            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles);

            Assert.Equal(Y,testComputer.mem.Read(addr+X));
            Assert.Equal(cpuCopy.A,testComputer.cpu.A);
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            Assert.Equal(cpuCopy.P,testComputer.cpu.P);
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }

        [Fact]
        public void STY_IndexWrapShouldWork()
        {
            testComputer.MemoryReset();
            
            uint X = 0xBB;
            uint Y = 0x42;
            uint addr = 0xAA;

            testComputer.mem.Write(0x0000,opcode);
            testComputer.mem.Write(0x0001,addr);

            testComputer.CPUReset();
            testComputer.cpu.Y = Y;
            testComputer.cpu.X = X;


            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles);

            Assert.Equal(Y,testComputer.mem.Read((addr+X)& 0xFF));
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }

    }


    public class STY_ABSOLUTE
    {

        private Computer testComputer = new Computer();
        private uint opcode = 0x8C;
        private int cycles = 4;
        private int bytes = 3;


        [Fact]
        public void STY_ShouldWork()
        {
            testComputer.MemoryReset();
            
            uint Y = 0x32;
            uint addr = 0xBEEF;

            testComputer.mem.Write(0x0000,opcode);
            testComputer.mem.Write(0x0001,addr & 0x00FF);
            testComputer.mem.Write(0x0002,addr >> 8);       

            testComputer.CPUReset();
            testComputer.cpu.Y = Y;

            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles);

            Assert.Equal(Y,testComputer.mem.Read(addr));
            Assert.Equal(cpuCopy.A,testComputer.cpu.A);
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            Assert.Equal(cpuCopy.P,testComputer.cpu.P);
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }

    }
    
}
