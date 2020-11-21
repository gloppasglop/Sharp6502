using System;
using Xunit;
using C6502;

namespace C6502.Tests
{

    public class DEX
    {

        private Computer testComputer = new Computer();
        private uint opcode = 0xCA;
        private int cycles = 2;
        private int bytes = 1;


        [Fact]
        public void DEX_ShouldDecreaseXByOne()
        {
            testComputer.MemoryReset();
            
            testComputer.mem.Write(0x0000,opcode);

            testComputer.CPUReset();
            testComputer.cpu.X = 0xA0;
            testComputer.cpu.P |= (uint) StatusFlagsMask.N;
            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles);

            Assert.Equal(cpuCopy.A,testComputer.cpu.A);
            Assert.Equal(cpuCopy.X-1,testComputer.cpu.X);
            Assert.Equal(cpuCopy.Y,testComputer.cpu.Y);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            Assert.Equal(cpuCopy.P,testComputer.cpu.P);
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }
        [Fact]
        public void DEX_ShouldSetNegativeFlag()
        {
            testComputer.MemoryReset();
            
            testComputer.mem.Write(0x0000,opcode);

            testComputer.CPUReset();
            testComputer.cpu.X = 0x00;

            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles);

            Assert.Equal(cpuCopy.A,testComputer.cpu.A);
            Assert.Equal((uint) 0xFF,testComputer.cpu.X);
            Assert.Equal(cpuCopy.Y,testComputer.cpu.Y);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            Assert.Equal((uint) StatusFlagsMask.N, testComputer.cpu.P & (uint) StatusFlagsMask.N);
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }
        [Fact]
        public void DEX_ShouldSetZeroFlag()
        {
            testComputer.MemoryReset();
            
            testComputer.mem.Write(0x0000,opcode);

            testComputer.CPUReset();
            testComputer.cpu.X = 0x01;

            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles);

            Assert.Equal(cpuCopy.A,testComputer.cpu.A);
            Assert.Equal((uint) 0x00,testComputer.cpu.X);
            Assert.Equal(cpuCopy.Y,testComputer.cpu.Y);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            Assert.Equal((uint) StatusFlagsMask.Z, testComputer.cpu.P & (uint) StatusFlagsMask.Z);
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }
    }

    public class DEY
    {

        private Computer testComputer = new Computer();
        private uint opcode = 0x88;
        private int cycles = 2;
        private int bytes = 1;


        [Fact]
        public void DEY_ShouldDecreaseYByOne()
        {
            testComputer.MemoryReset();
            
            testComputer.mem.Write(0x0000,opcode);

            testComputer.CPUReset();
            testComputer.cpu.Y = 0xA0;
            testComputer.cpu.P |= (uint) StatusFlagsMask.N;
            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles);

            Assert.Equal(cpuCopy.A,testComputer.cpu.A);
            Assert.Equal(cpuCopy.Y-1,testComputer.cpu.Y);
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            Assert.Equal(cpuCopy.P,testComputer.cpu.P);
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }
        [Fact]
        public void DEY_ShouldSetNegativeFlag()
        {
            testComputer.MemoryReset();
            
            testComputer.mem.Write(0x0000,opcode);

            testComputer.CPUReset();
            testComputer.cpu.Y = 0x00;

            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles);

            Assert.Equal(cpuCopy.A,testComputer.cpu.A);
            Assert.Equal((uint) 0xFF,testComputer.cpu.Y);
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            Assert.Equal((uint) StatusFlagsMask.N, testComputer.cpu.P & (uint) StatusFlagsMask.N);
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }
        [Fact]
        public void DEY_ShouldSetZeroFlag()
        {
            testComputer.MemoryReset();
            
            testComputer.mem.Write(0x0000,opcode);

            testComputer.CPUReset();
            testComputer.cpu.Y = 0x01;

            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles);

            Assert.Equal(cpuCopy.A,testComputer.cpu.A);
            Assert.Equal((uint) 0x00,testComputer.cpu.Y);
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            Assert.Equal((uint) StatusFlagsMask.Z, testComputer.cpu.P & (uint) StatusFlagsMask.Z);
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }

    }
    public class INX
    {

        private Computer testComputer = new Computer();
        private uint opcode = 0xE8;
        private int cycles = 2;
        private int bytes = 1;


        [Fact]
        public void INX_ShouldIncreaseXByOne()
        {
            testComputer.MemoryReset();
            
            testComputer.mem.Write(0x0000,opcode);

            testComputer.CPUReset();
            testComputer.cpu.X = 0xA0;
            testComputer.cpu.P |= (uint) StatusFlagsMask.N;
            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles);

            Assert.Equal(cpuCopy.A,testComputer.cpu.A);
            Assert.Equal(cpuCopy.X+1,testComputer.cpu.X);
            Assert.Equal(cpuCopy.Y,testComputer.cpu.Y);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            Assert.Equal(cpuCopy.P,testComputer.cpu.P);
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }
        [Fact]
        public void INX_ShouldSetNegativeFlag()
        {
            testComputer.MemoryReset();
            
            testComputer.mem.Write(0x0000,opcode);

            testComputer.CPUReset();
            testComputer.cpu.X = 0x7F;

            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles);

            Assert.Equal(cpuCopy.A,testComputer.cpu.A);
            Assert.Equal((uint) 0x80,testComputer.cpu.X);
            Assert.Equal(cpuCopy.Y,testComputer.cpu.Y);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            Assert.Equal((uint) StatusFlagsMask.N, testComputer.cpu.P & (uint) StatusFlagsMask.N);
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }
        [Fact]
        public void INX_ShouldSetZeroFlag()
        {
            testComputer.MemoryReset();
            
            testComputer.mem.Write(0x0000,opcode);

            testComputer.CPUReset();
            testComputer.cpu.X = 0xFF;

            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles);

            Assert.Equal(cpuCopy.A,testComputer.cpu.A);
            Assert.Equal((uint) 0x00,testComputer.cpu.X);
            Assert.Equal(cpuCopy.Y,testComputer.cpu.Y);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            Assert.Equal((uint) StatusFlagsMask.Z, testComputer.cpu.P & (uint) StatusFlagsMask.Z);
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }
    }

    public class INY
    {

        private Computer testComputer = new Computer();
        private uint opcode = 0xC8;
        private int cycles = 2;
        private int bytes = 1;


        [Fact]
        public void INY_ShouldIncreaseYByOne()
        {
            testComputer.MemoryReset();
            
            testComputer.mem.Write(0x0000,opcode);

            testComputer.CPUReset();
            testComputer.cpu.Y = 0xA0;
            testComputer.cpu.P |= (uint) StatusFlagsMask.N;
            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles);

            Assert.Equal(cpuCopy.A,testComputer.cpu.A);
            Assert.Equal(cpuCopy.Y+1,testComputer.cpu.Y);
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            Assert.Equal(cpuCopy.P,testComputer.cpu.P);
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }
        [Fact]
        public void INY_ShouldSetNegativeFlag()
        {
            testComputer.MemoryReset();
            
            testComputer.mem.Write(0x0000,opcode);

            testComputer.CPUReset();
            testComputer.cpu.Y = 0x7F;

            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles);

            Assert.Equal(cpuCopy.A,testComputer.cpu.A);
            Assert.Equal((uint) 0x80,testComputer.cpu.Y);
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            Assert.Equal((uint) StatusFlagsMask.N, testComputer.cpu.P & (uint) StatusFlagsMask.N);
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }
        [Fact]
        public void INY_ShouldSetZeroFlag()
        {
            testComputer.MemoryReset();
            
            testComputer.mem.Write(0x0000,opcode);

            testComputer.CPUReset();
            testComputer.cpu.Y = 0xFF;

            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles);

            Assert.Equal(cpuCopy.A,testComputer.cpu.A);
            Assert.Equal((uint) 0x00,testComputer.cpu.Y);
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            Assert.Equal((uint) StatusFlagsMask.Z, testComputer.cpu.P & (uint) StatusFlagsMask.Z);
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }

    }

    public class INC_ZEROPAGE
    {

        private Computer testComputer = new Computer();
        private uint opcode = 0xE6;
        private int cycles = 5;
        private int bytes = 2;


        [Fact]
        public void INC_ShouldIncreaseByOne()
        {
            testComputer.MemoryReset();
            
            uint value = 0x85;
            uint addr = 0xAA;

            testComputer.mem.Write(0x0000,opcode);
            testComputer.mem.Write(0x0001,addr);
            testComputer.mem.Write(addr,value);

            testComputer.CPUReset();
            testComputer.cpu.P |= (uint) StatusFlagsMask.N;
            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles);

            Assert.Equal(value+1,testComputer.mem.Read(addr));
            Assert.Equal(cpuCopy.A,testComputer.cpu.A);
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.Y,testComputer.cpu.Y);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            Assert.Equal(cpuCopy.P,testComputer.cpu.P);
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }
        [Fact]
        public void INC_ShouldSetNegativeFlag()
        {
            testComputer.MemoryReset();

            uint value = 0x7F;
            uint addr = 0xAA;

            testComputer.mem.Write(0x0000,opcode);
            testComputer.mem.Write(0x0001,addr);
            testComputer.mem.Write(addr,value);
            

            testComputer.CPUReset();

            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles);

            Assert.Equal(value+1,testComputer.mem.Read(addr));
            Assert.Equal(cpuCopy.A,testComputer.cpu.A);
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.Y,testComputer.cpu.Y);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            Assert.Equal((uint) StatusFlagsMask.N, testComputer.cpu.P & (uint) StatusFlagsMask.N);
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }
        [Fact]
        public void INC_ShouldSetZeroFlag()
        {
            testComputer.MemoryReset();

            uint value = 0xFF;
            uint addr = 0xAA;

            testComputer.mem.Write(0x0000,opcode);
            testComputer.mem.Write(0x0001,addr);
            testComputer.mem.Write(addr,value);
            

            testComputer.CPUReset();

            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles);

            Assert.Equal((uint) 0x00,testComputer.mem.Read(addr));
            Assert.Equal(cpuCopy.A,testComputer.cpu.A);
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.Y,testComputer.cpu.Y);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            Assert.Equal((uint) StatusFlagsMask.Z, testComputer.cpu.P & (uint) StatusFlagsMask.Z);
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }

    }
    public class INC_ZEROPAGEX
    {

        private Computer testComputer = new Computer();
        private uint opcode = 0xF6;
        private int cycles = 6;
        private int bytes = 2;


        [Fact]
        public void INC_ShouldIncreaseByOne()
        {
            testComputer.MemoryReset();
            
            uint value = 0x85;
            uint X = 0x0A;
            uint addr = 0xAA;

            testComputer.mem.Write(0x0000,opcode);
            testComputer.mem.Write(0x0001,addr);
            testComputer.mem.Write(addr+X,value);

            testComputer.CPUReset();
            testComputer.cpu.X = X;
            testComputer.cpu.P |= (uint) StatusFlagsMask.N;
            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles);

            Assert.Equal(value+1,testComputer.mem.Read(addr+X));
            Assert.Equal(cpuCopy.A,testComputer.cpu.A);
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.Y,testComputer.cpu.Y);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            Assert.Equal(cpuCopy.P,testComputer.cpu.P);
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }
        [Fact]
        public void INC_ShouldSetNegativeFlag()
        {
            testComputer.MemoryReset();

            uint value = 0x7F;
            uint X = 0x0A;
            uint addr = 0xAA;

            testComputer.mem.Write(0x0000,opcode);
            testComputer.mem.Write(0x0001,addr);
            testComputer.mem.Write(addr+X,value);
            

            testComputer.CPUReset();
            testComputer.cpu.X = X;

            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles);

            Assert.Equal(value+1,testComputer.mem.Read(addr+X));
            Assert.Equal(cpuCopy.A,testComputer.cpu.A);
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.Y,testComputer.cpu.Y);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            Assert.Equal((uint) StatusFlagsMask.N, testComputer.cpu.P & (uint) StatusFlagsMask.N);
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }
        [Fact]
        public void INC_ShouldSetZeroFlag()
        {
            testComputer.MemoryReset();

            uint value = 0xFF;
            uint X = 0x0A;
            uint addr = 0xAA;

            testComputer.mem.Write(0x0000,opcode);
            testComputer.mem.Write(0x0001,addr);
            testComputer.mem.Write(addr+X,value);
            

            testComputer.CPUReset();
            testComputer.cpu.X = X;

            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles);

            Assert.Equal((uint) 0x00,testComputer.mem.Read(addr+X));
            Assert.Equal(cpuCopy.A,testComputer.cpu.A);
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.Y,testComputer.cpu.Y);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            Assert.Equal((uint) StatusFlagsMask.Z, testComputer.cpu.P & (uint) StatusFlagsMask.Z);
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }

        [Fact]
        public void INC_CrossPageBoundaryShouldWork()
        {
            testComputer.MemoryReset();

            uint value = 0x5F;
            uint X = 0xBA;
            uint addr = 0xAA;

            testComputer.mem.Write(0x0000,opcode);
            testComputer.mem.Write(0x0001,addr);
            testComputer.mem.Write((addr+X) & 0xFF,value);
            

            testComputer.CPUReset();
            testComputer.cpu.X = X;

            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles);

            Assert.Equal(value+1,testComputer.mem.Read((addr+X) & 0xFF));
            Assert.Equal(cpuCopy.A,testComputer.cpu.A);
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.Y,testComputer.cpu.Y);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            Assert.Equal(cpuCopy.P,testComputer.cpu.P);
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }
    }
    public class INC_ABSOLUTE
    {

        private Computer testComputer = new Computer();
        private uint opcode = 0xEE;
        private int cycles = 6;
        private int bytes = 3;


        [Fact]
        public void INC_ShouldIncreaseByOne()
        {
            testComputer.MemoryReset();
            
            uint value = 0x85;
            uint addr = 0xBEE0;

            testComputer.mem.Write(0x0000,opcode);
            testComputer.mem.Write(0x0001,addr & 0x00FF);
            testComputer.mem.Write(0x0002,addr >> 8);

            testComputer.mem.Write(addr,value);

            testComputer.CPUReset();
            testComputer.cpu.P |= (uint) StatusFlagsMask.N;
            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles);

            Assert.Equal(value+1,testComputer.mem.Read(addr));
            Assert.Equal(cpuCopy.A,testComputer.cpu.A);
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.Y,testComputer.cpu.Y);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            Assert.Equal(cpuCopy.P,testComputer.cpu.P);
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }
        [Fact]
        public void INC_ShouldSetNegativeFlag()
        {
            testComputer.MemoryReset();

            uint value = 0x7F;
            uint addr = 0xBEE0;

            testComputer.mem.Write(0x0000,opcode);
            testComputer.mem.Write(0x0001,addr & 0x00FF);
            testComputer.mem.Write(0x0002,addr >> 8);

            testComputer.mem.Write(addr,value);

            testComputer.CPUReset();

            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles);

            Assert.Equal(value+1,testComputer.mem.Read(addr));
            Assert.Equal(cpuCopy.A,testComputer.cpu.A);
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.Y,testComputer.cpu.Y);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            Assert.Equal((uint) StatusFlagsMask.N, testComputer.cpu.P & (uint) StatusFlagsMask.N);
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }
        [Fact]
        public void INC_ShouldSetZeroFlag()
        {
            testComputer.MemoryReset();

            uint value = 0xFF;
            uint addr = 0xBEE0;

            testComputer.mem.Write(0x0000,opcode);
            testComputer.mem.Write(0x0001,addr & 0x00FF);
            testComputer.mem.Write(0x0002,addr >> 8);

            testComputer.mem.Write(addr,value);

            testComputer.CPUReset();

            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles);

            Assert.Equal((uint) 0x00,testComputer.mem.Read(addr));
            Assert.Equal(cpuCopy.A,testComputer.cpu.A);
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.Y,testComputer.cpu.Y);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            Assert.Equal((uint) StatusFlagsMask.Z, testComputer.cpu.P & (uint) StatusFlagsMask.Z);
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }

    }
    public class INC_ABSOLUTEX
    {

        private Computer testComputer = new Computer();
        private uint opcode = 0xFE;
        private int cycles = 7;
        private int bytes = 3;


        [Fact]
        public void INC_ShouldIncreaseByOne()
        {
            testComputer.MemoryReset();
            
            uint value = 0x85;
            uint X = 0x0F;
            uint addr = 0xBEE0;

            testComputer.mem.Write(0x0000,opcode);
            testComputer.mem.Write(0x0001,addr & 0x00FF);
            testComputer.mem.Write(0x0002,addr >> 8);

            testComputer.mem.Write(addr+X,value);

            testComputer.CPUReset();
            testComputer.cpu.X = X;
            testComputer.cpu.P |= (uint) StatusFlagsMask.N;
            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles);

            Assert.Equal(value+1,testComputer.mem.Read(addr+X));
            Assert.Equal(cpuCopy.A,testComputer.cpu.A);
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.Y,testComputer.cpu.Y);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            Assert.Equal(cpuCopy.P,testComputer.cpu.P);
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }
        [Fact]
        public void INC_ShouldSetNegativeFlag()
        {
            testComputer.MemoryReset();

            uint value = 0x7F;
            uint X = 0x0F;
            uint addr = 0xBEE0;

            testComputer.mem.Write(0x0000,opcode);
            testComputer.mem.Write(0x0001,addr & 0x00FF);
            testComputer.mem.Write(0x0002,addr >> 8);

            testComputer.mem.Write(addr+X,value);

            testComputer.CPUReset();
            testComputer.cpu.X = X;

            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles);

            Assert.Equal(value+1,testComputer.mem.Read(addr+X));
            Assert.Equal(cpuCopy.A,testComputer.cpu.A);
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.Y,testComputer.cpu.Y);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            Assert.Equal((uint) StatusFlagsMask.N, testComputer.cpu.P & (uint) StatusFlagsMask.N);
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }
        [Fact]
        public void INC_ShouldSetZeroFlag()
        {
            testComputer.MemoryReset();

            uint value = 0xFF;
            uint X = 0x0F;
            uint addr = 0xBEE0;

            testComputer.mem.Write(0x0000,opcode);
            testComputer.mem.Write(0x0001,addr & 0x00FF);
            testComputer.mem.Write(0x0002,addr >> 8);

            testComputer.mem.Write(addr+X,value);

            testComputer.CPUReset();
            testComputer.cpu.X = X;

            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles);

            Assert.Equal((uint) 0x00,testComputer.mem.Read(addr+X));
            Assert.Equal(cpuCopy.A,testComputer.cpu.A);
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.Y,testComputer.cpu.Y);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            Assert.Equal((uint) StatusFlagsMask.Z, testComputer.cpu.P & (uint) StatusFlagsMask.Z);
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }

        [Fact]
        public void INC_CrossPageBoundaryShouldWork()
        {
            testComputer.MemoryReset();
            
            uint value = 0x85;
            uint X = 0xAF;
            uint addr = 0xBEE0;

            testComputer.mem.Write(0x0000,opcode);
            testComputer.mem.Write(0x0001,addr & 0x00FF);
            testComputer.mem.Write(0x0002,addr >> 8);

            testComputer.mem.Write(addr+X,value);

            testComputer.CPUReset();
            testComputer.cpu.X = X;
            testComputer.cpu.P |= (uint) StatusFlagsMask.N;
            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles);

            Assert.Equal(value+1,testComputer.mem.Read(addr+X));
            Assert.Equal(cpuCopy.A,testComputer.cpu.A);
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.Y,testComputer.cpu.Y);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            Assert.Equal(cpuCopy.P,testComputer.cpu.P);
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }
 
    }
    public class DEC_ZEROPAGE
    {

        private Computer testComputer = new Computer();
        private uint opcode = 0xC6;
        private int cycles = 5;
        private int bytes = 2;


        [Fact]
        public void DEC_ShouldDecreaseByOne()
        {
            testComputer.MemoryReset();
            
            uint value = 0x85;
            uint addr = 0xAA;

            testComputer.mem.Write(0x0000,opcode);
            testComputer.mem.Write(0x0001,addr);
            testComputer.mem.Write(addr,value);

            testComputer.CPUReset();
            testComputer.cpu.P |= (uint) StatusFlagsMask.N;
            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles);

            Assert.Equal(value-1,testComputer.mem.Read(addr));
            Assert.Equal(cpuCopy.A,testComputer.cpu.A);
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.Y,testComputer.cpu.Y);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            Assert.Equal(cpuCopy.P,testComputer.cpu.P);
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }
        [Fact]
        public void DEC_ShouldSetNegativeFlag()
        {
            testComputer.MemoryReset();

            uint value = 0x00;
            uint addr = 0xAA;

            testComputer.mem.Write(0x0000,opcode);
            testComputer.mem.Write(0x0001,addr);
            testComputer.mem.Write(addr,value);
            

            testComputer.CPUReset();

            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles);

            Assert.Equal((uint)0xFF,testComputer.mem.Read(addr));
            Assert.Equal(cpuCopy.A,testComputer.cpu.A);
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.Y,testComputer.cpu.Y);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            Assert.Equal((uint) StatusFlagsMask.N, testComputer.cpu.P & (uint) StatusFlagsMask.N);
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }
        [Fact]
        public void DEC_ShouldSetZeroFlag()
        {
            testComputer.MemoryReset();

            uint value = 0x01;
            uint addr = 0xAA;

            testComputer.mem.Write(0x0000,opcode);
            testComputer.mem.Write(0x0001,addr);
            testComputer.mem.Write(addr,value);
            

            testComputer.CPUReset();

            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles);

            Assert.Equal((uint) 0x00,testComputer.mem.Read(addr));
            Assert.Equal(cpuCopy.A,testComputer.cpu.A);
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.Y,testComputer.cpu.Y);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            Assert.Equal((uint) StatusFlagsMask.Z, testComputer.cpu.P & (uint) StatusFlagsMask.Z);
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }

    }
    public class DEC_ZEROPAGEX
    {

        private Computer testComputer = new Computer();
        private uint opcode = 0xD6;
        private int cycles = 6;
        private int bytes = 2;


        [Fact]
        public void DEC_ShouldDecreaseByOne()
        {
            testComputer.MemoryReset();
            
            uint value = 0x85;
            uint X = 0x0A;
            uint addr = 0xAA;

            testComputer.mem.Write(0x0000,opcode);
            testComputer.mem.Write(0x0001,addr);
            testComputer.mem.Write(addr+X,value);

            testComputer.CPUReset();
            testComputer.cpu.X = X;
            testComputer.cpu.P |= (uint) StatusFlagsMask.N;
            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles);

            Assert.Equal(value-1,testComputer.mem.Read(addr+X));
            Assert.Equal(cpuCopy.A,testComputer.cpu.A);
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.Y,testComputer.cpu.Y);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            Assert.Equal(cpuCopy.P,testComputer.cpu.P);
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }
        [Fact]
        public void DEC_ShouldSetNegativeFlag()
        {
            testComputer.MemoryReset();

            uint value = 0x00;
            uint X = 0x0A;
            uint addr = 0xAA;

            testComputer.mem.Write(0x0000,opcode);
            testComputer.mem.Write(0x0001,addr);
            testComputer.mem.Write(addr+X,value);
            

            testComputer.CPUReset();
            testComputer.cpu.X = X;

            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles);

            Assert.Equal((uint)0xFF,testComputer.mem.Read(addr+X));
            Assert.Equal(cpuCopy.A,testComputer.cpu.A);
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.Y,testComputer.cpu.Y);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            Assert.Equal((uint) StatusFlagsMask.N, testComputer.cpu.P & (uint) StatusFlagsMask.N);
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }
        [Fact]
        public void DEC_ShouldSetZeroFlag()
        {
            testComputer.MemoryReset();

            uint value = 0x01;
            uint X = 0x0A;
            uint addr = 0xAA;

            testComputer.mem.Write(0x0000,opcode);
            testComputer.mem.Write(0x0001,addr);
            testComputer.mem.Write(addr+X,value);
            

            testComputer.CPUReset();
            testComputer.cpu.X = X;

            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles);

            Assert.Equal((uint) 0x00,testComputer.mem.Read(addr+X));
            Assert.Equal(cpuCopy.A,testComputer.cpu.A);
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.Y,testComputer.cpu.Y);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            Assert.Equal((uint) StatusFlagsMask.Z, testComputer.cpu.P & (uint) StatusFlagsMask.Z);
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }

        [Fact]
        public void DEC_CrossPageBoundaryShouldWork()
        {
            testComputer.MemoryReset();

            uint value = 0x5F;
            uint X = 0xBA;
            uint addr = 0xAA;

            testComputer.mem.Write(0x0000,opcode);
            testComputer.mem.Write(0x0001,addr);
            testComputer.mem.Write((addr+X) & 0xFF,value);
            

            testComputer.CPUReset();
            testComputer.cpu.X = X;

            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles);

            Assert.Equal(value-1,testComputer.mem.Read((addr+X) & 0xFF));
            Assert.Equal(cpuCopy.A,testComputer.cpu.A);
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.Y,testComputer.cpu.Y);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            Assert.Equal(cpuCopy.P,testComputer.cpu.P);
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }
    }
    public class DEC_ABSOLUTE
    {

        private Computer testComputer = new Computer();
        private uint opcode = 0xCE;
        private int cycles = 6;
        private int bytes = 3;


        [Fact]
        public void DEC_ShouldDecreaseByOne()
        {
            testComputer.MemoryReset();
            
            uint value = 0x85;
            uint addr = 0xBEE0;

            testComputer.mem.Write(0x0000,opcode);
            testComputer.mem.Write(0x0001,addr & 0x00FF);
            testComputer.mem.Write(0x0002,addr >> 8);

            testComputer.mem.Write(addr,value);

            testComputer.CPUReset();
            testComputer.cpu.P |= (uint) StatusFlagsMask.N;
            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles);

            Assert.Equal(value-1,testComputer.mem.Read(addr));
            Assert.Equal(cpuCopy.A,testComputer.cpu.A);
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.Y,testComputer.cpu.Y);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            Assert.Equal(cpuCopy.P,testComputer.cpu.P);
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }
        [Fact]
        public void DEC_ShouldSetNegativeFlag()
        {
            testComputer.MemoryReset();

            uint value = 0x00;
            uint addr = 0xBEE0;

            testComputer.mem.Write(0x0000,opcode);
            testComputer.mem.Write(0x0001,addr & 0x00FF);
            testComputer.mem.Write(0x0002,addr >> 8);

            testComputer.mem.Write(addr,value);

            testComputer.CPUReset();

            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles);

            Assert.Equal((uint) 0xFF,testComputer.mem.Read(addr));
            Assert.Equal(cpuCopy.A,testComputer.cpu.A);
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.Y,testComputer.cpu.Y);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            Assert.Equal((uint) StatusFlagsMask.N, testComputer.cpu.P & (uint) StatusFlagsMask.N);
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }
        [Fact]
        public void DEC_ShouldSetZeroFlag()
        {
            testComputer.MemoryReset();

            uint value = 0x01;
            uint addr = 0xBEE0;

            testComputer.mem.Write(0x0000,opcode);
            testComputer.mem.Write(0x0001,addr & 0x00FF);
            testComputer.mem.Write(0x0002,addr >> 8);

            testComputer.mem.Write(addr,value);

            testComputer.CPUReset();

            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles);

            Assert.Equal((uint) 0x00,testComputer.mem.Read(addr));
            Assert.Equal(cpuCopy.A,testComputer.cpu.A);
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.Y,testComputer.cpu.Y);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            Assert.Equal((uint) StatusFlagsMask.Z, testComputer.cpu.P & (uint) StatusFlagsMask.Z);
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }

    }
    public class DEC_ABSOLUTEX
    {

        private Computer testComputer = new Computer();
        private uint opcode = 0xDE;
        private int cycles = 7;
        private int bytes = 3;


        [Fact]
        public void DEC_ShouldDecreaseByOne()
        {
            testComputer.MemoryReset();
            
            uint value = 0x85;
            uint X = 0x0F;
            uint addr = 0xBEE0;

            testComputer.mem.Write(0x0000,opcode);
            testComputer.mem.Write(0x0001,addr & 0x00FF);
            testComputer.mem.Write(0x0002,addr >> 8);

            testComputer.mem.Write(addr+X,value);

            testComputer.CPUReset();
            testComputer.cpu.X = X;
            testComputer.cpu.P |= (uint) StatusFlagsMask.N;
            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles);

            Assert.Equal(value-1,testComputer.mem.Read(addr+X));
            Assert.Equal(cpuCopy.A,testComputer.cpu.A);
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.Y,testComputer.cpu.Y);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            Assert.Equal(cpuCopy.P,testComputer.cpu.P);
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }
        [Fact]
        public void DEC_ShouldSetNegativeFlag()
        {
            testComputer.MemoryReset();

            uint value = 0x00;
            uint X = 0x0F;
            uint addr = 0xBEE0;

            testComputer.mem.Write(0x0000,opcode);
            testComputer.mem.Write(0x0001,addr & 0x00FF);
            testComputer.mem.Write(0x0002,addr >> 8);

            testComputer.mem.Write(addr+X,value);

            testComputer.CPUReset();
            testComputer.cpu.X = X;

            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles);

            Assert.Equal((uint) 0xFF,testComputer.mem.Read(addr+X));
            Assert.Equal(cpuCopy.A,testComputer.cpu.A);
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.Y,testComputer.cpu.Y);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            Assert.Equal((uint) StatusFlagsMask.N, testComputer.cpu.P & (uint) StatusFlagsMask.N);
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }
        [Fact]
        public void DEC_ShouldSetZeroFlag()
        {
            testComputer.MemoryReset();

            uint value = 0x01;
            uint X = 0x0F;
            uint addr = 0xBEE0;

            testComputer.mem.Write(0x0000,opcode);
            testComputer.mem.Write(0x0001,addr & 0x00FF);
            testComputer.mem.Write(0x0002,addr >> 8);

            testComputer.mem.Write(addr+X,value);

            testComputer.CPUReset();
            testComputer.cpu.X = X;

            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles);

            Assert.Equal((uint) 0x00,testComputer.mem.Read(addr+X));
            Assert.Equal(cpuCopy.A,testComputer.cpu.A);
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.Y,testComputer.cpu.Y);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            Assert.Equal((uint) StatusFlagsMask.Z, testComputer.cpu.P & (uint) StatusFlagsMask.Z);
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }

        [Fact]
        public void DEC_CrossPageBoundaryShouldWork()
        {
            testComputer.MemoryReset();
            
            uint value = 0x85;
            uint X = 0xAF;
            uint addr = 0xBEE0;

            testComputer.mem.Write(0x0000,opcode);
            testComputer.mem.Write(0x0001,addr & 0x00FF);
            testComputer.mem.Write(0x0002,addr >> 8);

            testComputer.mem.Write(addr+X,value);

            testComputer.CPUReset();
            testComputer.cpu.X = X;
            testComputer.cpu.P |= (uint) StatusFlagsMask.N;
            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles);

            Assert.Equal(value-1,testComputer.mem.Read(addr+X));
            Assert.Equal(cpuCopy.A,testComputer.cpu.A);
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.Y,testComputer.cpu.Y);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            Assert.Equal(cpuCopy.P,testComputer.cpu.P);
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }
 
    }
}