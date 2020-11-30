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

    public class JMP_INDIRECT
    {

        private Computer testComputer = new Computer();
        private uint opcode = 0x6C;
        private int cycles = 5;
        private int bytes = 3;


        [Fact]
        public void JMP_shouldwork()
        {
            testComputer.MemoryReset();
            
            uint pointer = 0x8000;
            uint addr = 0xBEEF;

            testComputer.mem.Write(0x0000,opcode);
            testComputer.mem.Write(0x0001,pointer & 0x00FF);
            testComputer.mem.Write(0x0002,pointer >> 8);

            testComputer.mem.Write(pointer, addr & 0x00FF);
            testComputer.mem.Write(pointer+1, addr >> 8);

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

    public class JSR_ABSOLUTE
    {

        private Computer testComputer = new Computer();
        private uint opcode = 0x20;
        private int cycles = 6;
        private int bytes = 3;


        [Fact]
        public void JSR_shouldwork()
        {
            testComputer.MemoryReset();
            
            uint startAddr = 0x1080;
            uint addr = 0xBEEF;

            testComputer.mem.Write(startAddr,opcode);
            testComputer.mem.Write(startAddr+1,addr & 0xFF);
            testComputer.mem.Write(startAddr+2,addr >>8);

            testComputer.CPUReset();
            testComputer.cpu.PC = startAddr;
            testComputer.cpu.AddrPins = testComputer.cpu.PC;
            testComputer.cpu.DataPins = testComputer.mem.Read(testComputer.cpu.PC);

            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles);

            Assert.Equal(cpuCopy.A,testComputer.cpu.A);
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.Y,testComputer.cpu.Y);
            // Stack point should be decremented by 2
            Assert.Equal(cpuCopy.S-2,testComputer.cpu.S);
            Assert.Equal((startAddr+2) & 0xFF, testComputer.mem.Read(0x100+cpuCopy.S));
            Assert.Equal((startAddr+2) >> 8, testComputer.mem.Read(0x100+cpuCopy.S-1));


            Assert.Equal(cpuCopy.P,testComputer.cpu.P);
            Assert.Equal(addr,testComputer.cpu.PC);
        }
    }

    public class RTS
    {

        private Computer testComputer = new Computer();
        private uint opcode = 0x60;
        private int cycles = 6;
        private int bytes = 1;


        [Fact]
        public void RTS_shouldwork()
        {
            testComputer.MemoryReset();
            
            uint startAddr = 0x1080;
            uint addr = 0xBEEF;
            uint S = 0xF0;

            testComputer.mem.Write(startAddr,opcode);

            testComputer.mem.Write(0x100+S+1,addr & 0xFF);
            testComputer.mem.Write(0x100+S+2,addr >>8);

            testComputer.CPUReset();
            testComputer.cpu.PC = startAddr;
            testComputer.cpu.S = S;
            testComputer.cpu.AddrPins = testComputer.cpu.PC;
            testComputer.cpu.DataPins = testComputer.mem.Read(testComputer.cpu.PC);

            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles);

            Assert.Equal(cpuCopy.A,testComputer.cpu.A);
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.Y,testComputer.cpu.Y);
            // Stack point should be incremented by 2
            Assert.Equal(cpuCopy.S+2,testComputer.cpu.S);
            Assert.Equal(cpuCopy.P,testComputer.cpu.P);
            Assert.Equal(addr+1,testComputer.cpu.PC);
        }
    }
    public class RTI
    {

        private Computer testComputer = new Computer();
        private uint opcode = 0x40;
        private int cycles = 6;
        private int bytes = 1;


        [Fact]
        public void RTI_shouldwork()
        {
            testComputer.MemoryReset();
            
            uint startAddr = 0x1080;
            uint addr = 0xBEEF;
            uint S = 0xF0;
            uint P = 0b0011_0111;

            testComputer.mem.Write(startAddr,opcode);

            testComputer.mem.Write(0x100+S+1,P);
            testComputer.mem.Write(0x100+S+2,addr & 0xFF);
            testComputer.mem.Write(0x100+S+3,addr >>8);

            testComputer.CPUReset();
            testComputer.cpu.PC = startAddr;
            testComputer.cpu.S = S;
            testComputer.cpu.AddrPins = testComputer.cpu.PC;
            testComputer.cpu.DataPins = testComputer.mem.Read(testComputer.cpu.PC);

            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles);

            Assert.Equal(cpuCopy.A,testComputer.cpu.A);
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.Y,testComputer.cpu.Y);
            // Stack point should be incremented by 3
            Assert.Equal(cpuCopy.S+3,testComputer.cpu.S);
            Assert.Equal(P,testComputer.cpu.P);
            Assert.Equal(addr,testComputer.cpu.PC);
        }
    }
    public class BRK
    {

        private Computer testComputer = new Computer();
        private uint opcode = 0x00;
        private int cycles = 7;
        private int bytes = 1;


        [Fact]
        public void BRK_shouldwork()
        {
            testComputer.MemoryReset();
            
            uint addr = 0xBEEF;
            uint S = 0xFF;

            testComputer.mem.Write(0x0000,opcode);

            testComputer.mem.Write(0xFFFE,addr & 0xFF);
            testComputer.mem.Write(0xFFFF,addr >>8);

            testComputer.CPUReset();
            testComputer.cpu.S = S;

            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles);

            Assert.Equal(cpuCopy.A,testComputer.cpu.A);
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.Y,testComputer.cpu.Y);
            // Stack point should be decremented by 3
            Assert.Equal(cpuCopy.S-3,testComputer.cpu.S);
            // B flag should be set
            Assert.Equal((uint) StatusFlagsMask.B,testComputer.cpu.P & (uint) StatusFlagsMask.B);
            Assert.Equal(addr,testComputer.cpu.PC);
        }
    }
}
