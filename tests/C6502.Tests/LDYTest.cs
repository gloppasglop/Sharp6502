using System;
using Xunit;
using C6502;

namespace C6502.Tests
{


    public class LDY_IMMEDIATE
    {

        private Computer testComputer = new Computer();
        private uint opcode = 0xA0;
        private int cycles = 2;
        private int bytes = 2;


        [Fact]
        public void LDY_ShouldWork()
        {
            testComputer.MemoryReset();
            
            
            uint Y = 0x32;
            // LDY #$32
            testComputer.mem.Write(0x0000,opcode);
            testComputer.mem.Write(0x0001,Y);

            testComputer.CPUReset();

            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles);

            Assert.Equal(Y,testComputer.cpu.Y);
            Assert.Equal(cpuCopy.A,testComputer.cpu.A);
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            Assert.Equal(cpuCopy.P,testComputer.cpu.P);
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }

        [Fact]
        public void LDY_ZeroShouldSetZFlag()
        {
            testComputer.MemoryReset();
            
            
            uint Y = 0x00;
            // LDY #$00
            testComputer.mem.Write(0x0000,opcode);
            testComputer.mem.Write(0x0001,Y);

            testComputer.CPUReset();


            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles);

            Assert.Equal(Y,testComputer.cpu.Y);
            Assert.Equal(cpuCopy.A,testComputer.cpu.A);
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            // Zero flag should be true
            Assert.Equal((uint) StatusFlagsMask.Z, testComputer.cpu.P & (uint) StatusFlagsMask.Z);
            // Negativeflag should be false
            Assert.Equal((uint) 0, testComputer.cpu.P & (uint) StatusFlagsMask.N);
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }

        [Fact]
        public void LDY_NegativeShouldSetNFlag()
        {
            testComputer.MemoryReset();
            
            
            uint Y = 0x85;
            // LDY #$85
            testComputer.mem.Write(0x0000,opcode);
            testComputer.mem.Write(0x0001,Y);

            testComputer.CPUReset();


            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles);

            Assert.Equal(Y,testComputer.cpu.Y);
            Assert.Equal(cpuCopy.A,testComputer.cpu.A);
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            // Zero flag should be false
            Assert.Equal((uint) 0, testComputer.cpu.P & (uint) StatusFlagsMask.Z);
            // Negativeflag should be true
            Assert.Equal((uint) StatusFlagsMask.N, testComputer.cpu.P & (uint) StatusFlagsMask.N);
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }

    }



    public class LDY_ZEROPAGE
    {

        private Computer testComputer = new Computer();
        private uint opcode = 0xA4;
        private int cycles = 3;
        private int bytes = 2;


        [Fact]
        public void LDY_ShouldWork()
        {
            testComputer.MemoryReset();
            
            // LDY #AA
            uint Y = 0x32;
            uint addr = 0xAA;

            testComputer.mem.Write(0x0000,opcode);
            testComputer.mem.Write(0x0001,addr);
            testComputer.mem.Write(addr,Y);

            testComputer.CPUReset();

            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles);

            Assert.Equal(Y,testComputer.cpu.Y);
            Assert.Equal(cpuCopy.A,testComputer.cpu.A);
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            Assert.Equal(cpuCopy.P,testComputer.cpu.P);
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }

        [Fact]
        public void LDY_ZeroShouldSetZFlag()
        {
            testComputer.MemoryReset();
            
            // LDY #AA
            uint Y = 0x00;
            uint addr = 0xAA;

            testComputer.mem.Write(0x0000,opcode);
            testComputer.mem.Write(0x0001,addr);
            testComputer.mem.Write(addr,Y);

            testComputer.CPUReset();


            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles);

            Assert.Equal(Y,testComputer.cpu.Y);
            Assert.Equal(cpuCopy.A,testComputer.cpu.A);
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            // Zero flag should be true
            Assert.Equal((uint) StatusFlagsMask.Z, testComputer.cpu.P & (uint) StatusFlagsMask.Z);
            // Negativeflag should be false
            Assert.Equal((uint) 0, testComputer.cpu.P & (uint) StatusFlagsMask.N);
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }

        [Fact]
        public void LDY_NegativeShouldSetNFlag()
        {
            testComputer.MemoryReset();
            
            // LDY #AA
            uint Y = 0x85;
            uint addr = 0xAA;

            testComputer.mem.Write(0x0000,opcode);
            testComputer.mem.Write(0x0001,addr);
            testComputer.mem.Write(addr,Y);

            testComputer.CPUReset();


            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles);

            Assert.Equal(Y,testComputer.cpu.Y);
            Assert.Equal(cpuCopy.A,testComputer.cpu.A);
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            // Zero flag should be false
            Assert.Equal((uint) 0, testComputer.cpu.P & (uint) StatusFlagsMask.Z);
            // Negativeflag should be true
            Assert.Equal((uint) StatusFlagsMask.N, testComputer.cpu.P & (uint) StatusFlagsMask.N);
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }

    }

   public class LDY_ZEROPAGEX
    {

        private Computer testComputer = new Computer();
        private uint opcode = 0xB4;
        private int cycles = 4;
        private int bytes = 2;


        [Fact]
        public void LDY_ShouldWork()
        {
            testComputer.MemoryReset();
            
            // x = A
            // LDY #A0,Y
            uint X = 0x0A;
            uint Y = 0x32;
            uint addr = 0xAA;

            testComputer.mem.Write(0x0000,opcode);
            testComputer.mem.Write(0x0001,addr);
            testComputer.mem.Write(addr+X,Y);

            testComputer.CPUReset();
            testComputer.cpu.X = X;

            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles);

            Assert.Equal(Y,testComputer.cpu.Y);
            Assert.Equal(cpuCopy.A,testComputer.cpu.A);
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            Assert.Equal(cpuCopy.P,testComputer.cpu.P);
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }

        [Fact]
        public void LDY_ZeroShouldSetZFlag()
        {
            testComputer.MemoryReset();
            
            // LDY #A0,X
            uint X = 0x0A;
            uint Y = 0x00;
            uint addr = 0xAA;

            testComputer.mem.Write(0x0000,opcode);
            testComputer.mem.Write(0x0001,addr);
            testComputer.mem.Write(addr+X,Y);

            testComputer.CPUReset();
            testComputer.cpu.X = X;


            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles);

            Assert.Equal(Y,testComputer.cpu.Y);
            Assert.Equal(cpuCopy.A,testComputer.cpu.A);
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            // Zero flag should be true
            Assert.Equal((uint) StatusFlagsMask.Z, testComputer.cpu.P & (uint) StatusFlagsMask.Z);
            // Negativeflag should be false
            Assert.Equal((uint) 0, testComputer.cpu.P & (uint) StatusFlagsMask.N);
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }

        [Fact]
        public void LDY_NegativeShouldSetNFlag()
        {
            testComputer.MemoryReset();
            
            // x = A
            // LDY #A0,X
            uint X = 0x0A;
            uint Y = 0x85;
            uint addr = 0xAA;

            testComputer.mem.Write(0x0000,opcode);
            testComputer.mem.Write(0x0001,addr);
            testComputer.mem.Write(addr+X,Y);

            testComputer.CPUReset();
            testComputer.cpu.X = X;


            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles);

            Assert.Equal(Y,testComputer.cpu.Y);
            Assert.Equal(cpuCopy.A,testComputer.cpu.A);
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            // Zero flag should be false
            Assert.Equal((uint) 0, testComputer.cpu.P & (uint) StatusFlagsMask.Z);
            // Negativeflag should be true
            Assert.Equal((uint) StatusFlagsMask.N, testComputer.cpu.P & (uint) StatusFlagsMask.N);
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }

        [Fact]
        public void LDY_IndexWrapShouldWork()
        {
            testComputer.MemoryReset();
            
            // x = A
            // LDY #A0,X
            uint X = 0xBB;
            uint Y = 0x42;
            uint addr = 0xAA;

            testComputer.mem.Write(0x0000,opcode);
            testComputer.mem.Write(0x0001,addr);
            testComputer.mem.Write( (addr+X) & 0xFF,Y);

            testComputer.CPUReset();
            testComputer.cpu.X = X;


            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles);

            Assert.Equal(Y,testComputer.cpu.Y);
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }

    }


    public class LDY_ABSOLUTE
    {

        private Computer testComputer = new Computer();
        private uint opcode = 0xAC;
        private int cycles = 4;
        private int bytes = 3;


        [Fact]
        public void LDY_ShouldWork()
        {
            testComputer.MemoryReset();
            
            // LDY #BEEF
            uint Y = 0x32;
            uint addr = 0xBEEF;

            testComputer.mem.Write(0x0000,opcode);
            testComputer.mem.Write(0x0001,addr & 0x00FF);
            testComputer.mem.Write(0x0002,addr >> 8);       
            testComputer.mem.Write(addr,Y);

            testComputer.CPUReset();

            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles);

            Assert.Equal(Y,testComputer.cpu.Y);
            Assert.Equal(cpuCopy.A,testComputer.cpu.A);
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            Assert.Equal(cpuCopy.P,testComputer.cpu.P);
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }

        [Fact]
        public void LDY_ZeroShouldSetZFlag()
        {
            testComputer.MemoryReset();
            
            // LDY #BEEF
            uint Y = 0x00;
            uint addr = 0xBEEF;

            testComputer.mem.Write(0x0000,opcode);
            testComputer.mem.Write(0x0001,addr & 0x00FF);
            testComputer.mem.Write(0x0002,addr >> 8);       
            testComputer.mem.Write(addr,Y);

            testComputer.CPUReset();


            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles);

            Assert.Equal(Y,testComputer.cpu.Y);
            Assert.Equal(cpuCopy.A,testComputer.cpu.A);
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            // Zero flag should be true
            Assert.Equal((uint) StatusFlagsMask.Z, testComputer.cpu.P & (uint) StatusFlagsMask.Z);
            // Negativeflag should be false
            Assert.Equal((uint) 0, testComputer.cpu.P & (uint) StatusFlagsMask.N);
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }

        [Fact]
        public void LDY_NegativeShouldSetNFlag()
        {
            testComputer.MemoryReset();
            
            // LDY #BEEF
            uint Y = 0x85;
            uint addr = 0xBEEF;

            testComputer.mem.Write(0x0000,opcode);
            testComputer.mem.Write(0x0001,addr & 0x00FF);
            testComputer.mem.Write(0x0002,addr >> 8);       
            testComputer.mem.Write(addr,Y);

            testComputer.CPUReset();


            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles);

            Assert.Equal(Y,testComputer.cpu.Y);
            Assert.Equal(cpuCopy.A,testComputer.cpu.A);
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            // Zero flag should be false
            Assert.Equal((uint) 0, testComputer.cpu.P & (uint) StatusFlagsMask.Z);
            // Negativeflag should be true
            Assert.Equal((uint) StatusFlagsMask.N, testComputer.cpu.P & (uint) StatusFlagsMask.N);
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }

    }
    

    public class LDY_ABSOLUTEX
    {

        private Computer testComputer = new Computer();
        private uint opcode = 0xBC;
        private int cycles = 4;
        private int bytes = 3;


        [Fact]
        public void LDY_ShouldWork()
        {
            testComputer.MemoryReset();
            
            // LDY #0xBEE0,X
            uint Y = 0x32;
            uint X = 0x0F;
            uint addr = 0xBEE0;

            testComputer.mem.Write(0x0000,opcode);
            testComputer.mem.Write(0x0001,addr & 0x00FF);
            testComputer.mem.Write(0x0002,addr >> 8);       
            testComputer.mem.Write(addr+X,Y);

            testComputer.CPUReset();
            testComputer.cpu.X = X;

            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles);

            Assert.Equal(Y,testComputer.cpu.Y);
            Assert.Equal(cpuCopy.A,testComputer.cpu.A);
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            Assert.Equal(cpuCopy.P,testComputer.cpu.P);
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }

        [Fact]
        public void LDY_ZeroShouldSetZFlag()
        {
            testComputer.MemoryReset();
            
            // LDY #0xBEE0,Y
            uint Y = 0x00;
            uint X = 0x0F;
            uint addr = 0xBEE0;

            testComputer.mem.Write(0x0000,opcode);
            testComputer.mem.Write(0x0001,addr & 0x00FF);
            testComputer.mem.Write(0x0002,addr >> 8);       
            testComputer.mem.Write(addr+X,Y);

            testComputer.CPUReset();
            testComputer.cpu.X = X;


            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles);

            Assert.Equal(Y,testComputer.cpu.Y);
            Assert.Equal(cpuCopy.A,testComputer.cpu.A);
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            // Zero flag should be true
            Assert.Equal((uint) StatusFlagsMask.Z, testComputer.cpu.P & (uint) StatusFlagsMask.Z);
            // Negativeflag should be false
            Assert.Equal((uint) 0, testComputer.cpu.P & (uint) StatusFlagsMask.N);
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }

        [Fact]
        public void LDY_NegativeShouldSetNFlag()
        {
            testComputer.MemoryReset();
            
            // LDY #0xBEE0,X
            uint Y = 0x82;
            uint X = 0x0F;
            uint addr = 0xBEE0;

            testComputer.mem.Write(0x0000,opcode);
            testComputer.mem.Write(0x0001,addr & 0x00FF);
            testComputer.mem.Write(0x0002,addr >> 8);       
            testComputer.mem.Write(addr+X,Y);

            testComputer.CPUReset();
            testComputer.cpu.X = X;


            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles);

            Assert.Equal(Y,testComputer.cpu.Y);
            Assert.Equal(cpuCopy.A,testComputer.cpu.A);
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            // Zero flag should be false
            Assert.Equal((uint) 0, testComputer.cpu.P & (uint) StatusFlagsMask.Z);
            // Negativeflag should be true
            Assert.Equal((uint) StatusFlagsMask.N, testComputer.cpu.P & (uint) StatusFlagsMask.N);
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }

        [Fact]
        public void LDY_PageBoundryCrossShouLDYddACycle()
        {
            testComputer.MemoryReset();
            
            // LDY #0xBEE0,Y
            uint Y = 0x32;
            uint X = 0xFA;
            uint addr = 0xBEE0;

            testComputer.mem.Write(0x0000,opcode);
            testComputer.mem.Write(0x0001,addr & 0x00FF);
            testComputer.mem.Write(0x0002,addr >> 8);       
            testComputer.mem.Write(addr+X,Y);

            testComputer.CPUReset();
            testComputer.cpu.X = X;

            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles+1);

            Assert.Equal(Y,testComputer.cpu.Y);
            Assert.Equal(cpuCopy.A,testComputer.cpu.A);
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            Assert.Equal(cpuCopy.P,testComputer.cpu.P);
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }

    }

}
