using System;
using Xunit;
using C6502;

namespace C6502.Tests
{

    public class AND_IMMEDIATE
    {

        private Computer testComputer = new Computer();
        private uint opcode = 0x29;
        private int cycles = 2;
        private int bytes = 2;


        [Fact]
        public void AND_ShouldWork()
        {
            testComputer.MemoryReset();
            
            
            uint A = 0x32;
            uint value = 0x83;

            // LDA #$32
            testComputer.mem.Write(0x0000,opcode);
            testComputer.mem.Write(0x0001,value);

            testComputer.CPUReset();

            testComputer.cpu.A = A;

            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles);

            Assert.Equal(A & value,testComputer.cpu.A);
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.Y,testComputer.cpu.Y);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            Assert.Equal(cpuCopy.P,testComputer.cpu.P);
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }

        [Fact]
        public void AND_ZeroShouldSetZFlag()
        {
            testComputer.MemoryReset();
            
            
            uint A = 0x32;
            uint value = 0x00;

            testComputer.mem.Write(0x0000,opcode);
            testComputer.mem.Write(0x0001,value);

            testComputer.CPUReset();
            testComputer.cpu.A = A;


            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles);

            Assert.Equal(A & value,testComputer.cpu.A);
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.Y,testComputer.cpu.Y);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            // Zero flag should be true
            Assert.Equal((uint) StatusFlagsMask.Z, testComputer.cpu.P & (uint) StatusFlagsMask.Z);
            // Negativeflag should be false
            Assert.Equal((uint) 0, testComputer.cpu.P & (uint) StatusFlagsMask.N);
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }

        [Fact]
        public void AND_NegativeShouldSetNFlag()
        {
            testComputer.MemoryReset();
            
            
            uint A = 0x85;
            uint value = 0xFF;

            // LDA #$85
            testComputer.mem.Write(0x0000,opcode);
            testComputer.mem.Write(0x0001,value);

            testComputer.CPUReset();
            testComputer.cpu.A = A;


            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles);

            Assert.Equal(A & value,testComputer.cpu.A);
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.Y,testComputer.cpu.Y);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            // Zero flag should be false
            Assert.Equal((uint) 0, testComputer.cpu.P & (uint) StatusFlagsMask.Z);
            // Negativeflag should be true
            Assert.Equal((uint) StatusFlagsMask.N, testComputer.cpu.P & (uint) StatusFlagsMask.N);
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }

    }



    public class AND_ZEROPAGE
    {

        private Computer testComputer = new Computer();
        private uint opcode = 0x25;
        private int cycles = 3;
        private int bytes = 2;


        [Fact]
        public void AND_ShouldWork()
        {
            testComputer.MemoryReset();
            
            uint A = 0x32;
            uint addr = 0xAA;
            uint value = 0x83;

            testComputer.mem.Write(0x0000,opcode);
            testComputer.mem.Write(0x0001,addr);
            testComputer.mem.Write(addr,value);

            testComputer.CPUReset();

            testComputer.cpu.A = A;

            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles);

            Assert.Equal(A & value,testComputer.cpu.A);
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.Y,testComputer.cpu.Y);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            Assert.Equal(cpuCopy.P,testComputer.cpu.P);
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }

        [Fact]
        public void AND_ZeroShouldSetZFlag()
        {
            testComputer.MemoryReset();
            
            uint A = 0x32;
            uint addr = 0xAA;
            uint value = 0x00;

            testComputer.mem.Write(0x0000,opcode);
            testComputer.mem.Write(0x0001,addr);
            testComputer.mem.Write(addr,value);

            testComputer.CPUReset();
            testComputer.cpu.A = A;


            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles);

            Assert.Equal(A & value,testComputer.cpu.A);
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.Y,testComputer.cpu.Y);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            // Zero flag should be true
            Assert.Equal((uint) StatusFlagsMask.Z, testComputer.cpu.P & (uint) StatusFlagsMask.Z);
            // Negativeflag should be false
            Assert.Equal((uint) 0, testComputer.cpu.P & (uint) StatusFlagsMask.N);
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }

        [Fact]
        public void AND_NegativeShouldSetNFlag()
        {
            testComputer.MemoryReset();
            
            // LDA #AA
            uint A = 0x85;
            uint addr = 0xAA;
            uint value = 0xFF;

            testComputer.mem.Write(0x0000,opcode);
            testComputer.mem.Write(0x0001,addr);
            testComputer.mem.Write(addr,value);

            testComputer.CPUReset();
            testComputer.cpu.A = A;


            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles);

            Assert.Equal(A & value,testComputer.cpu.A);
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.Y,testComputer.cpu.Y);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            // Zero flag should be false
            Assert.Equal((uint) 0, testComputer.cpu.P & (uint) StatusFlagsMask.Z);
            // Negativeflag should be true
            Assert.Equal((uint) StatusFlagsMask.N, testComputer.cpu.P & (uint) StatusFlagsMask.N);
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }

    }

   public class AND_ZEROPAGEX
    {

        private Computer testComputer = new Computer();
        private uint opcode = 0x35;
        private int cycles = 4;
        private int bytes = 2;


        [Fact]
        public void AND_ShouldWork()
        {
            testComputer.MemoryReset();
            
            // x = A
            // LDA #A0,X
            uint X = 0x0A;
            uint A = 0x32;
            uint addr = 0xAA;
            uint value = 0x83;

            testComputer.mem.Write(0x0000,opcode);
            testComputer.mem.Write(0x0001,addr);
            testComputer.mem.Write(addr+X,value);

            testComputer.CPUReset();
            testComputer.cpu.X = X;
            testComputer.cpu.A = A;

            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles);

            Assert.Equal(A & value,testComputer.cpu.A);
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.Y,testComputer.cpu.Y);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            Assert.Equal(cpuCopy.P,testComputer.cpu.P);
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }

        [Fact]
        public void AND_ZeroShouldSetZFlag()
        {
            testComputer.MemoryReset();
            
            uint X = 0x0A;
            uint A = 0x32;
            uint addr = 0xAA;
            uint value = 0x00;

            testComputer.mem.Write(0x0000,opcode);
            testComputer.mem.Write(0x0001,addr);
            testComputer.mem.Write(addr+X,value);

            testComputer.CPUReset();
            testComputer.cpu.X = X;
            testComputer.cpu.A = A;


            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles);

            Assert.Equal(A & value,testComputer.cpu.A);
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.Y,testComputer.cpu.Y);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            // Zero flag should be true
            Assert.Equal((uint) StatusFlagsMask.Z, testComputer.cpu.P & (uint) StatusFlagsMask.Z);
            // Negativeflag should be false
            Assert.Equal((uint) 0, testComputer.cpu.P & (uint) StatusFlagsMask.N);
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }

        [Fact]
        public void AND_NegativeShouldSetNFlag()
        {
            testComputer.MemoryReset();
            
            // x = A
            // LDA #A0,X
            uint X = 0x0A;
            uint A = 0x85;
            uint addr = 0xAA;
            uint value = 0xFF;

            testComputer.mem.Write(0x0000,opcode);
            testComputer.mem.Write(0x0001,addr);
            testComputer.mem.Write(addr+X,value);

            testComputer.CPUReset();
            testComputer.cpu.X = X;
            testComputer.cpu.A = A;


            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles);

            Assert.Equal(A & value,testComputer.cpu.A);
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.Y,testComputer.cpu.Y);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            // Zero flag should be false
            Assert.Equal((uint) 0, testComputer.cpu.P & (uint) StatusFlagsMask.Z);
            // Negativeflag should be true
            Assert.Equal((uint) StatusFlagsMask.N, testComputer.cpu.P & (uint) StatusFlagsMask.N);
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }

        [Fact]
        public void AND_IndexWrapShouldWork()
        {
            testComputer.MemoryReset();
            
            // x = A
            // LDA #A0,X
            uint X = 0xBB;
            uint A = 0x42;
            uint addr = 0xAA;
            uint value = 0x83;

            testComputer.mem.Write(0x0000,opcode);
            testComputer.mem.Write(0x0001,addr);
            testComputer.mem.Write( (addr+X) & 0xFF,value);

            testComputer.CPUReset();
            testComputer.cpu.X = X;
            testComputer.cpu.A = A;


            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles);

            Assert.Equal(A & value,testComputer.cpu.A);
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }

    }


    public class AND_ABSOLUTE
    {

        private Computer testComputer = new Computer();
        private uint opcode = 0x2D;
        private int cycles = 4;
        private int bytes = 3;


        [Fact]
        public void AND_ShouldWork()
        {
            testComputer.MemoryReset();
            
            // LDA #BEEF
            uint A = 0x32;
            uint addr = 0xBEEF;
            uint value = 0x83;

            testComputer.mem.Write(0x0000,opcode);
            testComputer.mem.Write(0x0001,addr & 0x00FF);
            testComputer.mem.Write(0x0002,addr >> 8);       
            testComputer.mem.Write(addr,value);

            testComputer.CPUReset();
            testComputer.cpu.A = A;

            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles);

            Assert.Equal(A & value,testComputer.cpu.A);
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.Y,testComputer.cpu.Y);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            Assert.Equal(cpuCopy.P,testComputer.cpu.P);
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }

        [Fact]
        public void AND_ZeroShouldSetZFlag()
        {
            testComputer.MemoryReset();
            
            // LDA #BEEF
            uint A = 0x32;
            uint addr = 0xBEEF;
            uint value = 0x00;

            testComputer.mem.Write(0x0000,opcode);
            testComputer.mem.Write(0x0001,addr & 0x00FF);
            testComputer.mem.Write(0x0002,addr >> 8);       
            testComputer.mem.Write(addr,value);

            testComputer.CPUReset();
            testComputer.cpu.A = A;


            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles);

            Assert.Equal(A & value,testComputer.cpu.A);
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.Y,testComputer.cpu.Y);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            // Zero flag should be true
            Assert.Equal((uint) StatusFlagsMask.Z, testComputer.cpu.P & (uint) StatusFlagsMask.Z);
            // Negativeflag should be false
            Assert.Equal((uint) 0, testComputer.cpu.P & (uint) StatusFlagsMask.N);
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }

        [Fact]
        public void AND_NegativeShouldSetNFlag()
        {
            testComputer.MemoryReset();
            
            // LDA #BEEF
            uint A = 0x85;
            uint addr = 0xBEEF;
            uint value = 0xFF;

            testComputer.mem.Write(0x0000,opcode);
            testComputer.mem.Write(0x0001,addr & 0x00FF);
            testComputer.mem.Write(0x0002,addr >> 8);       
            testComputer.mem.Write(addr,value);

            testComputer.CPUReset();
            testComputer.cpu.A = A;


            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles);

            Assert.Equal(A & value,testComputer.cpu.A);
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.Y,testComputer.cpu.Y);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            // Zero flag should be false
            Assert.Equal((uint) 0, testComputer.cpu.P & (uint) StatusFlagsMask.Z);
            // Negativeflag should be true
            Assert.Equal((uint) StatusFlagsMask.N, testComputer.cpu.P & (uint) StatusFlagsMask.N);
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }

    }
    
    public class AND_ABSOLUTEX
    {

        private Computer testComputer = new Computer();
        private uint opcode = 0x3D;
        private int cycles = 4;
        private int bytes = 3;


        [Fact]
        public void AND_ShouldWork()
        {
            testComputer.MemoryReset();
            
            // LDA #0xBEE0,X
            uint A = 0x32;
            uint X = 0x0F;
            uint addr = 0xBEE0;
            uint value = 0x83;

            testComputer.mem.Write(0x0000,opcode);
            testComputer.mem.Write(0x0001,addr & 0x00FF);
            testComputer.mem.Write(0x0002,addr >> 8);       
            testComputer.mem.Write(addr+X,value);

            testComputer.CPUReset();
            testComputer.cpu.X = X;
            testComputer.cpu.A = A;

            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles);

            Assert.Equal(A & value,testComputer.cpu.A);
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.Y,testComputer.cpu.Y);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            Assert.Equal(cpuCopy.P,testComputer.cpu.P);
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }

        [Fact]
        public void AND_ZeroShouldSetZFlag()
        {
            testComputer.MemoryReset();
            
            // LDA #0xBEE0,X
            uint A = 0x32;
            uint X = 0x0F;
            uint addr = 0xBEE0;
            uint value = 0x00;

            testComputer.mem.Write(0x0000,opcode);
            testComputer.mem.Write(0x0001,addr & 0x00FF);
            testComputer.mem.Write(0x0002,addr >> 8);       
            testComputer.mem.Write(addr+X,value);

            testComputer.CPUReset();
            testComputer.cpu.X = X;
            testComputer.cpu.A = A;


            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles);

            Assert.Equal(A & value,testComputer.cpu.A);
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.Y,testComputer.cpu.Y);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            // Zero flag should be true
            Assert.Equal((uint) StatusFlagsMask.Z, testComputer.cpu.P & (uint) StatusFlagsMask.Z);
            // Negativeflag should be false
            Assert.Equal((uint) 0, testComputer.cpu.P & (uint) StatusFlagsMask.N);
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }

        [Fact]
        public void AND_NegativeShouldSetNFlag()
        {
            testComputer.MemoryReset();
            
            // LDA #0xBEE0,X
            uint A = 0x82;
            uint X = 0x0F;
            uint addr = 0xBEE0;
            uint value = 0xFF;

            testComputer.mem.Write(0x0000,opcode);
            testComputer.mem.Write(0x0001,addr & 0x00FF);
            testComputer.mem.Write(0x0002,addr >> 8);       
            testComputer.mem.Write(addr+X,value);

            testComputer.CPUReset();
            testComputer.cpu.X = X;
            testComputer.cpu.A = A;


            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles);

            Assert.Equal(A & value,testComputer.cpu.A);
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.Y,testComputer.cpu.Y);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            // Zero flag should be false
            Assert.Equal((uint) 0, testComputer.cpu.P & (uint) StatusFlagsMask.Z);
            // Negativeflag should be true
            Assert.Equal((uint) StatusFlagsMask.N, testComputer.cpu.P & (uint) StatusFlagsMask.N);
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }

        [Fact]
        public void AND_PageBoundryCrossShouldAddACycle()
        {
            testComputer.MemoryReset();
            
            // LDA #0xBEE0,X
            uint A = 0x32;
            uint X = 0xFA;
            uint addr = 0xBEE0;
            uint value = 0x83;

            testComputer.mem.Write(0x0000,opcode);
            testComputer.mem.Write(0x0001,addr & 0x00FF);
            testComputer.mem.Write(0x0002,addr >> 8);       
            testComputer.mem.Write(addr+X,value);

            testComputer.CPUReset();
            testComputer.cpu.X = X;
            testComputer.cpu.A = A;

            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles+1);

            Assert.Equal(A & value,testComputer.cpu.A);
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.Y,testComputer.cpu.Y);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            Assert.Equal(cpuCopy.P,testComputer.cpu.P);
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }

    }


    public class AND_ABSOLUTEY
    {

        private Computer testComputer = new Computer();
        private uint opcode = 0x39;
        private int cycles = 4;
        private int bytes = 3;


        [Fact]
        public void AND_ShouldWork()
        {
            testComputer.MemoryReset();
            
            // LDA #0xBEE0,Y
            uint A = 0x32;
            uint Y = 0x0F;
            uint addr = 0xBEE0;
            uint value = 0x83;

            testComputer.mem.Write(0x0000,opcode);
            testComputer.mem.Write(0x0001,addr & 0x00FF);
            testComputer.mem.Write(0x0002,addr >> 8);       
            testComputer.mem.Write(addr+Y,value);

            testComputer.CPUReset();
            testComputer.cpu.Y = Y;
            testComputer.cpu.A = A;

            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles);

            Assert.Equal(A & value,testComputer.cpu.A);
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.Y,testComputer.cpu.Y);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            Assert.Equal(cpuCopy.P,testComputer.cpu.P);
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }

        [Fact]
        public void AND_ZeroShouldSetZFlag()
        {
            testComputer.MemoryReset();
            
            // LDA #0xBEE0,Y
            uint A = 0x32;
            uint Y = 0x0F;
            uint addr = 0xBEE0;
            uint value = 0x00;

            testComputer.mem.Write(0x0000,opcode);
            testComputer.mem.Write(0x0001,addr & 0x00FF);
            testComputer.mem.Write(0x0002,addr >> 8);       
            testComputer.mem.Write(addr+Y,value);

            testComputer.CPUReset();
            testComputer.cpu.Y = Y;
            testComputer.cpu.A = A;


            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles);

            Assert.Equal(A & value,testComputer.cpu.A);
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.Y,testComputer.cpu.Y);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            // Zero flag should be true
            Assert.Equal((uint) StatusFlagsMask.Z, testComputer.cpu.P & (uint) StatusFlagsMask.Z);
            // Negativeflag should be false
            Assert.Equal((uint) 0, testComputer.cpu.P & (uint) StatusFlagsMask.N);
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }

        [Fact]
        public void AND_NegativeShouldSetNFlag()
        {
            testComputer.MemoryReset();
            
            // LDA #0xBEE0,X
            uint A = 0x82;
            uint Y = 0x0F;
            uint addr = 0xBEE0;
            uint value = 0x83;

            testComputer.mem.Write(0x0000,opcode);
            testComputer.mem.Write(0x0001,addr & 0x00FF);
            testComputer.mem.Write(0x0002,addr >> 8);       
            testComputer.mem.Write(addr+Y,value);

            testComputer.CPUReset();
            testComputer.cpu.Y = Y;
            testComputer.cpu.A = A;


            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles);

            Assert.Equal(A & value,testComputer.cpu.A);
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.Y,testComputer.cpu.Y);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            // Zero flag should be false
            Assert.Equal((uint) 0, testComputer.cpu.P & (uint) StatusFlagsMask.Z);
            // Negativeflag should be true
            Assert.Equal((uint) StatusFlagsMask.N, testComputer.cpu.P & (uint) StatusFlagsMask.N);
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }

        [Fact]
        public void AND_PageBoundryCrossShouldAddACycle()
        {
            testComputer.MemoryReset();
            
            // LDA #0xBEE0,Y
            uint A = 0x32;
            uint Y = 0xFA;
            uint addr = 0xBEE0;
            uint value = 0x83;

            testComputer.mem.Write(0x0000,opcode);
            testComputer.mem.Write(0x0001,addr & 0x00FF);
            testComputer.mem.Write(0x0002,addr >> 8);       
            testComputer.mem.Write(addr+Y,value);

            testComputer.CPUReset();
            testComputer.cpu.Y = Y;
            testComputer.cpu.A = A;

            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles+1);

            Assert.Equal(A & value,testComputer.cpu.A);
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.Y,testComputer.cpu.Y);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            Assert.Equal(cpuCopy.P,testComputer.cpu.P);
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }

    }
    public class AND_INDEXEDINDIRECT
    {

        private Computer testComputer = new Computer();
        private uint opcode = 0x21;
        private int cycles = 6;
        private int bytes = 2;


        [Fact]
        public void AND_ShouldWork()
        {
            testComputer.MemoryReset();
            
            uint pointer = 0x40;
            uint pointerValue = 0xA0;
            uint A = 0x32;
            uint X = 0x0F;
            uint addr = 0xBEE0;
            uint value = 0x83;

            testComputer.mem.Write(0x0000,opcode);
            testComputer.mem.Write(0x0001,pointer);
            testComputer.mem.Write(pointer,pointerValue);

            testComputer.mem.Write((pointerValue+X) & 0xFF,addr & 0x00FF);
            testComputer.mem.Write((pointerValue+X+1) & 0xFF,addr >> 8);       
            testComputer.mem.Write(addr,value);

            testComputer.CPUReset();
            testComputer.cpu.X = X;
            testComputer.cpu.A = A;

            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles);

            Assert.Equal(A & value,testComputer.cpu.A);
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.Y,testComputer.cpu.Y);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            Assert.Equal(cpuCopy.P,testComputer.cpu.P);
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }

        [Fact]
        public void AND_ZeroShouldSetZFlag()
        {
            testComputer.MemoryReset();

            uint pointer = 0x40;
            uint pointerValue = 0xA0;
            uint A = 0x32;
            uint X = 0x0F;
            uint addr = 0xBEE0;
            uint value = 0x00;

            testComputer.mem.Write(0x0000,opcode);
            testComputer.mem.Write(0x0001,pointer);
            testComputer.mem.Write(pointer,pointerValue);

            testComputer.mem.Write((pointerValue+X) & 0xFF,addr & 0x00FF);
            testComputer.mem.Write((pointerValue+X+1) & 0xFF,addr >> 8);       
            testComputer.mem.Write(addr,value);

            testComputer.CPUReset();
            testComputer.cpu.X = X;
            testComputer.cpu.A = A;

            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles);

            Assert.Equal(A & value,testComputer.cpu.A);
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.Y,testComputer.cpu.Y);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            // Zero flag should be true
            Assert.Equal((uint) StatusFlagsMask.Z, testComputer.cpu.P & (uint) StatusFlagsMask.Z);
            // Negativeflag should be false
            Assert.Equal((uint) 0, testComputer.cpu.P & (uint) StatusFlagsMask.N);
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }

        [Fact]
        public void AND_NegativeShouldSetNFlag()
        {
            testComputer.MemoryReset();
            
              // LDA (#0xAA,X)
            uint pointer = 0x40;
            uint pointerValue = 0xA0;
            uint A = 0x84;
            uint X = 0x0F;
            uint addr = 0xBEE0;
            uint value = 0xFF;

            testComputer.mem.Write(0x0000,opcode);
            testComputer.mem.Write(0x0001,pointer);
            testComputer.mem.Write(pointer,pointerValue);

            testComputer.mem.Write((pointerValue+X) & 0xFF,addr & 0x00FF);
            testComputer.mem.Write((pointerValue+X+1) & 0xFF,addr >> 8);       
            testComputer.mem.Write(addr,value);

            testComputer.CPUReset();
            testComputer.cpu.X = X;
            testComputer.cpu.A = A;

            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles);

            Assert.Equal(A & value,testComputer.cpu.A);
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.Y,testComputer.cpu.Y);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            // Zero flag should be false
            Assert.Equal((uint) 0, testComputer.cpu.P & (uint) StatusFlagsMask.Z);
            // Negativeflag should be true
            Assert.Equal((uint) StatusFlagsMask.N, testComputer.cpu.P & (uint) StatusFlagsMask.N);
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }

        [Fact]
        public void AND_CrossPageBoundaryShouldWork()
        {
            testComputer.MemoryReset();
            
            uint pointer = 0x40;
            uint pointerValue = 0xA0;
            uint A = 0x42;
            uint X = 0xAA;
            uint addr = 0xBEE0;
            uint value = 0x83;

            testComputer.mem.Write(0x0000,opcode);
            testComputer.mem.Write(0x0001,pointer);
            testComputer.mem.Write(pointer,pointerValue);

            testComputer.mem.Write((pointerValue+X) & 0xFF,addr & 0x00FF);
            testComputer.mem.Write((pointerValue+X+1) & 0xFF,addr >> 8);       
            testComputer.mem.Write(addr,value);

            testComputer.CPUReset();
            testComputer.cpu.X = X;
            testComputer.cpu.A = A;

            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles);

            Assert.Equal(A & value,testComputer.cpu.A);
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.Y,testComputer.cpu.Y);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            Assert.Equal(cpuCopy.P,testComputer.cpu.P);
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }

    }
    public class AND_INDIRECTINDEXED
    {

        private Computer testComputer = new Computer();
        private uint opcode = 0x31;
        private int cycles = 5;
        private int bytes = 2;


        [Fact]
        public void AND_ShouldWork()
        {
            testComputer.MemoryReset();
            
            uint pointer = 0xAA;
            uint A = 0x32;
            uint Y = 0x0F;
            uint addr = 0xBEE0;
            uint value = 0x83;

            testComputer.mem.Write(0x0000,opcode);
            testComputer.mem.Write(0x0001,pointer);

            testComputer.mem.Write(pointer,addr & 0x00FF);
            testComputer.mem.Write(pointer+1,addr >> 8);       

            testComputer.mem.Write(addr+Y,value);

            testComputer.CPUReset();
            testComputer.cpu.Y = Y;
            testComputer.cpu.A = A;

            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles);

            Assert.Equal(A & value,testComputer.cpu.A);
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.Y,testComputer.cpu.Y);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            Assert.Equal(cpuCopy.P,testComputer.cpu.P);
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }

        [Fact]
        public void AND_ZeroShouldSetZFlag()
        {
            testComputer.MemoryReset();

            uint pointer = 0xAA;
            uint A = 0x32;
            uint Y = 0x0F;
            uint addr = 0xBEE0;
            uint value = 0x00;

            testComputer.mem.Write(0x0000,opcode);
            testComputer.mem.Write(0x0001,pointer);

            testComputer.mem.Write(pointer,addr & 0x00FF);
            testComputer.mem.Write(pointer+1,addr >> 8);       

            testComputer.mem.Write(addr+Y,value);

            testComputer.CPUReset();
            testComputer.cpu.Y = Y;
            testComputer.cpu.A = A;

            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles);

            Assert.Equal(A & value,testComputer.cpu.A);
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.Y,testComputer.cpu.Y);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            // Zero flag should be true
            Assert.Equal((uint) StatusFlagsMask.Z, testComputer.cpu.P & (uint) StatusFlagsMask.Z);
            // Negativeflag should be false
            Assert.Equal((uint) 0, testComputer.cpu.P & (uint) StatusFlagsMask.N);
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }

        [Fact]
        public void AND_NegativeShouldSetNFlag()
        {
            testComputer.MemoryReset();

              // LDA (#0xAA),Y
            uint pointer = 0xAA;
            uint A = 0x84;
            uint Y = 0x0F;
            uint addr = 0xBEE0;
            uint value = 0xFF;

            testComputer.mem.Write(0x0000,opcode);
            testComputer.mem.Write(0x0001,pointer);

            testComputer.mem.Write(pointer,addr & 0x00FF);
            testComputer.mem.Write(pointer+1,addr >> 8);       

            testComputer.mem.Write(addr+Y,value);

            testComputer.CPUReset();
            testComputer.cpu.Y = Y;
            testComputer.cpu.A = A;

            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles);

            Assert.Equal(A & value,testComputer.cpu.A);
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.Y,testComputer.cpu.Y);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            // Zero flag should be false
            Assert.Equal((uint) 0, testComputer.cpu.P & (uint) StatusFlagsMask.Z);
            // Negativeflag should be true
            Assert.Equal((uint) StatusFlagsMask.N, testComputer.cpu.P & (uint) StatusFlagsMask.N);
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }

        [Fact]
        public void AND_PageBoundryCrossShouldAddACycle()
        {
            testComputer.MemoryReset();

            uint pointer = 0xAA;
            uint A = 0x32;
            uint Y = 0xAF;
            uint addr = 0xBEE0;
            uint value = 0x83;

            testComputer.mem.Write(0x0000,opcode);
            testComputer.mem.Write(0x0001,pointer);

            testComputer.mem.Write(pointer,addr & 0x00FF);
            testComputer.mem.Write(pointer+1,addr >> 8);       

            testComputer.mem.Write(addr+Y,value);

            testComputer.CPUReset();
            testComputer.cpu.Y = Y;
            testComputer.cpu.A = A;

            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles+1);

            Assert.Equal(A & value,testComputer.cpu.A);
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.Y,testComputer.cpu.Y);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            Assert.Equal(cpuCopy.P,testComputer.cpu.P);
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }
    }

}