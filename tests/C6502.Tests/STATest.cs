using System;
using Xunit;
using C6502;

namespace C6502.Tests
{


    public class STA_ZEROPAGE
    {

        private Computer testComputer = new Computer();
        private uint opcode = 0x85;
        private int cycles = 3;
        private int bytes = 2;


        [Fact]
        public void STA_ShouldWork()
        {
            testComputer.MemoryReset();
            
            // STA #AA
            uint A = 0x32;
            uint addr = 0xAA;

            testComputer.mem.Write(0x0000,opcode);
            testComputer.mem.Write(0x0001,addr);

            testComputer.CPUReset();
            testComputer.cpu.A = A;

            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles);

            Assert.Equal(A,testComputer.mem.Read(addr));
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.Y,testComputer.cpu.Y);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            Assert.Equal(cpuCopy.P,testComputer.cpu.P);
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }
    }
   public class STA_ZEROPAGEX
    {

        private Computer testComputer = new Computer();
        private uint opcode = 0x95;
        private int cycles = 4;
        private int bytes = 2;


        [Fact]
        public void STA_ShouldWork()
        {
            testComputer.MemoryReset();
            
            // STA #A0,X
            uint X = 0x0A;
            uint A = 0x32;
            uint addr = 0xA0;

            testComputer.mem.Write(0x0000,opcode);
            testComputer.mem.Write(0x0001,addr);

            testComputer.CPUReset();
            testComputer.cpu.X = X;
            testComputer.cpu.A = A;

            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles);

            Assert.Equal(A,testComputer.mem.Read(addr+X));
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.Y,testComputer.cpu.Y);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            Assert.Equal(cpuCopy.P,testComputer.cpu.P);
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }

        [Fact]
        public void STA_IndexWrapShouldWork()
        {
            testComputer.MemoryReset();
            
            // x = A
            // LDA #A0,X
            uint X = 0xBB;
            uint A = 0x42;
            uint addr = 0xAA;

            testComputer.mem.Write(0x0000,opcode);
            testComputer.mem.Write(0x0001,addr);

            testComputer.CPUReset();
            testComputer.cpu.X = X;
            testComputer.cpu.A = A;


            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles);

            Assert.Equal(A,testComputer.mem.Read((addr+X)& 0xFF));
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }

    }


    public class STA_ABSOLUTE
    {

        private Computer testComputer = new Computer();
        private uint opcode = 0x8D;
        private int cycles = 4;
        private int bytes = 3;


        [Fact]
        public void STA_ShouldWork()
        {
            testComputer.MemoryReset();
            
            // LDA #BEEF
            uint A = 0x32;
            uint addr = 0xBEEF;

            testComputer.mem.Write(0x0000,opcode);
            testComputer.mem.Write(0x0001,addr & 0x00FF);
            testComputer.mem.Write(0x0002,addr >> 8);       

            testComputer.CPUReset();
            testComputer.cpu.A = A;

            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles);

            Assert.Equal(A,testComputer.mem.Read(addr));
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.Y,testComputer.cpu.Y);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            Assert.Equal(cpuCopy.P,testComputer.cpu.P);
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }

    }
    
    public class STA_ABSOLUTEX
    {

        private Computer testComputer = new Computer();
        private uint opcode = 0x9D;
        private int cycles = 5;
        private int bytes = 3;


        [Fact]
        public void STA_ShouldWork()
        {
            testComputer.MemoryReset();
            
            // LDA #0xBEE0,X
            uint A = 0x32;
            uint X = 0x0F;
            uint addr = 0xBEE0;

            testComputer.mem.Write(0x0000,opcode);
            testComputer.mem.Write(0x0001,addr & 0x00FF);
            testComputer.mem.Write(0x0002,addr >> 8);       

            testComputer.CPUReset();
            testComputer.cpu.X = X;
            testComputer.cpu.A = A;

            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles);

            Assert.Equal(A,testComputer.mem.Read(addr+X));
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.Y,testComputer.cpu.Y);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            Assert.Equal(cpuCopy.P,testComputer.cpu.P);
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }
        [Fact]
        public void STA_CrossPageBoundaryShouldWork()
        {
            testComputer.MemoryReset();
            
            // LDA #0xBEE0,X
            uint A = 0x32;
            uint X = 0xAF;
            uint addr = 0xBEE0;

            testComputer.mem.Write(0x0000,opcode);
            testComputer.mem.Write(0x0001,addr & 0x00FF);
            testComputer.mem.Write(0x0002,addr >> 8);       

            testComputer.CPUReset();
            testComputer.cpu.X = X;
            testComputer.cpu.A = A;

            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles);

            Assert.Equal(A,testComputer.mem.Read(addr+X));
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.Y,testComputer.cpu.Y);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            Assert.Equal(cpuCopy.P,testComputer.cpu.P);
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }
    }


    public class STA_ABSOLUTEY
    {

        private Computer testComputer = new Computer();
        private uint opcode = 0x99;
        private int cycles = 5;
        private int bytes = 3;


        [Fact]
        public void STA_ShouldWork()
        {
            testComputer.MemoryReset();
            
            // STA #0xBEE0,Y
            uint A = 0x32;
            uint Y = 0x0F;
            uint addr = 0xBEE0;

            testComputer.mem.Write(0x0000,opcode);
            testComputer.mem.Write(0x0001,addr & 0x00FF);
            testComputer.mem.Write(0x0002,addr >> 8);       

            testComputer.CPUReset();
            testComputer.cpu.Y = Y;
            testComputer.cpu.A = A;

            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles);

            Assert.Equal(A,testComputer.mem.Read(addr+Y));
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.Y,testComputer.cpu.Y);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            Assert.Equal(cpuCopy.P,testComputer.cpu.P);
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }
        [Fact]
        public void STA_CrossPageBoundaryShouldWork()
        {
            testComputer.MemoryReset();
            
            // STA #0xBEE0,Y
            uint A = 0x32;
            uint Y = 0xAF;
            uint addr = 0xBEE0;

            testComputer.mem.Write(0x0000,opcode);
            testComputer.mem.Write(0x0001,addr & 0x00FF);
            testComputer.mem.Write(0x0002,addr >> 8);       

            testComputer.CPUReset();
            testComputer.cpu.Y = Y;
            testComputer.cpu.A = A;

            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles);

            Assert.Equal(A,testComputer.mem.Read(addr+Y));
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.Y,testComputer.cpu.Y);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            Assert.Equal(cpuCopy.P,testComputer.cpu.P);
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }

    }
    public class STA_INDEXEDINDIRECT
    {

        private Computer testComputer = new Computer();
        private uint opcode = 0x81;
        private int cycles = 6;
        private int bytes = 2;


        [Fact]
        public void STA_ShouldWork()
        {
            testComputer.MemoryReset();
            
            // STA (#0xAA,X)
            uint pointer = 0x40;
            uint A = 0x32;
            uint X = 0x0F;
            uint addr = 0xBEE0;

            testComputer.mem.Write(0x0000,opcode);
            testComputer.mem.Write(0x0001,pointer);

            testComputer.mem.Write((pointer+X) & 0xFF,addr & 0x00FF);
            testComputer.mem.Write((pointer+X+1) & 0xFF,addr >> 8);       

            testComputer.CPUReset();
            testComputer.cpu.X = X;
            testComputer.cpu.A = A;

            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles);

            Assert.Equal(A,testComputer.mem.Read(addr));
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.Y,testComputer.cpu.Y);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            Assert.Equal(cpuCopy.P,testComputer.cpu.P);
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }
        [Fact]
        public void STA_CrossPageBoundaryShouldWork()
        {
            testComputer.MemoryReset();

             // STA (#0xAA,X)
            uint pointer = 0x60;
            uint A = 0x32;
            uint X = 0xBF;
            uint addr = 0xBEE0;

            testComputer.mem.Write(0x0000,opcode);
            testComputer.mem.Write(0x0001,pointer);

            testComputer.mem.Write((pointer+X) & 0xFF,addr & 0x00FF);
            testComputer.mem.Write((pointer+X+1) & 0xFF,addr >> 8);       


            testComputer.CPUReset();
            testComputer.cpu.X = X;
            testComputer.cpu.A = A;

            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles);

            Assert.Equal(A,testComputer.mem.Read(addr));
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.Y,testComputer.cpu.Y);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            Assert.Equal(cpuCopy.P,testComputer.cpu.P);
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }
    }
    public class STA_INDIRECTINDEXED
    {

        private Computer testComputer = new Computer();
        private uint opcode = 0x81;
        private int cycles = 6;
        private int bytes = 2;


        [Fact]
        public void STA_ShouldWork()
        {
            testComputer.MemoryReset();
            
            // STA (#0xAA,X)
            uint pointer = 0xAA;
            uint A = 0x32;
            uint Y = 0x0F;
            uint addr = 0xBEE0;

            testComputer.mem.Write(0x0000,opcode);
            testComputer.mem.Write(0x0001,pointer);

            testComputer.mem.Write(pointer,addr & 0x00FF);
            testComputer.mem.Write(pointer+1,addr >> 8);       

            testComputer.mem.Write(addr+Y,A);

            testComputer.CPUReset();
            testComputer.cpu.Y = Y;
            testComputer.cpu.A = A;

            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles);

            Assert.Equal(A,testComputer.mem.Read(addr+Y));
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.Y,testComputer.cpu.Y);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            Assert.Equal(cpuCopy.P,testComputer.cpu.P);
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }
        [Fact]
        public void STA_CrossPageBoundaryShouldWork()
        {
            testComputer.MemoryReset();

            // STA (#0xAA,X)
            uint pointer = 0xAA;
            uint A = 0x32;
            uint Y = 0xBF;
            uint addr = 0xBEE0;

            testComputer.mem.Write(0x0000,opcode);
            testComputer.mem.Write(0x0001,pointer);

            testComputer.mem.Write(pointer,addr & 0x00FF);
            testComputer.mem.Write(pointer+1,addr >> 8);       

            testComputer.mem.Write(addr+Y,A);

            testComputer.CPUReset();
            testComputer.cpu.Y = Y;
            testComputer.cpu.A = A;

            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles);

            Assert.Equal(A,testComputer.mem.Read(addr+Y));
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.Y,testComputer.cpu.Y);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            Assert.Equal(cpuCopy.P,testComputer.cpu.P);
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }
   }
}
