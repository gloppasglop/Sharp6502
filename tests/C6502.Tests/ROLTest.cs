using System;
using Xunit;
using C6502;

namespace C6502.Tests
{


    public class ROL_ACCUMULATOR
    {

        private Computer testComputer = new Computer();
        private uint opcode = 0x2A;
        private int cycles = 2;
        private int bytes = 1;


        [Theory]
        // A, Carry, expected result, expected N flag, expected Z flag, Expected C flag, 
        [InlineData(0b0000_1111,0,0b0001_1110,(uint) 0, (uint) 0, (uint) 0)]
        [InlineData(0b0000_1111,(uint) StatusFlagsMask.C,0b0001_1111,(uint) 0, (uint) 0, (uint) 0)]
        [InlineData(0b1000_1111,(uint) 0,0b0001_1110,(uint) 0, (uint) 0, (uint) StatusFlagsMask.C)]
        [InlineData(0b1000_1111,(uint) StatusFlagsMask.C,0b0001_1111,(uint) 0, (uint) 0, (uint) StatusFlagsMask.C)]
        [InlineData(0b0100_1111,(uint) 0,0b1001_1110,(uint) StatusFlagsMask.N, (uint) 0, (uint) 0)]
        [InlineData(0b0100_1111,(uint) StatusFlagsMask.C,0b1001_1111,(uint) StatusFlagsMask.N, (uint) 0, (uint) 0)]
        [InlineData(0b0000_0000,(uint) 0,0b0000_0000,(uint) 0, (uint) StatusFlagsMask.Z, (uint) 0)]
        [InlineData(0b1000_0000,(uint) 0,0b0000_0000,(uint) 0, (uint) StatusFlagsMask.Z, (uint) StatusFlagsMask.C)]
        public void ROL_ShouldWork(uint A, uint carry, uint expRes, uint expN, uint expZ,uint expC)
        {
            testComputer.MemoryReset();
            
            
            // ROL #$32
            testComputer.mem.Write(0x0000,opcode);

            testComputer.CPUReset();
            testComputer.cpu.A = A;
            testComputer.cpu.P |= carry;

            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles);

            Assert.Equal(expRes,testComputer.cpu.A);
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            // N,Z,C Flags should be correct
            Assert.Equal(expN | expZ | expC , testComputer.cpu.P & (uint) (StatusFlagsMask.N | StatusFlagsMask.Z |  StatusFlagsMask.C));
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }

    }



    public class ROL_ZEROPAGE_RMW
    {

        private Computer testComputer = new Computer();
        private uint opcode = 0x26;
        private int cycles = 5;
        private int bytes = 2;


        [Theory]
        // value, Carry, expected result, expected N flag, expected Z flag, Expected C flag, 
        [InlineData(0b0000_1111,0,0b0001_1110,(uint) 0, (uint) 0, (uint) 0)]
        [InlineData(0b0000_1111,(uint) StatusFlagsMask.C,0b0001_1111,(uint) 0, (uint) 0, (uint) 0)]
        [InlineData(0b1000_1111,(uint) 0,0b0001_1110,(uint) 0, (uint) 0, (uint) StatusFlagsMask.C)]
        [InlineData(0b1000_1111,(uint) StatusFlagsMask.C,0b0001_1111,(uint) 0, (uint) 0, (uint) StatusFlagsMask.C)]
        [InlineData(0b0100_1111,(uint) 0,0b1001_1110,(uint) StatusFlagsMask.N, (uint) 0, (uint) 0)]
        [InlineData(0b0100_1111,(uint) StatusFlagsMask.C,0b1001_1111,(uint) StatusFlagsMask.N, (uint) 0, (uint) 0)]
        [InlineData(0b0000_0000,(uint) 0,0b0000_0000,(uint) 0, (uint) StatusFlagsMask.Z, (uint) 0)]
        [InlineData(0b1000_0000,(uint) 0,0b0000_0000,(uint) 0, (uint) StatusFlagsMask.Z, (uint) StatusFlagsMask.C)]
        public void ROL_ShouldWork(uint value, uint carry, uint expRes, uint expN, uint expZ, uint expC)
        {
            testComputer.MemoryReset();
            
            // ROL #AA
            uint addr = 0xAA;

            testComputer.mem.Write(0x0000,opcode);
            testComputer.mem.Write(0x0001,addr);
            testComputer.mem.Write(addr,value);

            testComputer.CPUReset();
            testComputer.cpu.P |= carry;

            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles);

            Assert.Equal(expRes,testComputer.mem.Read(addr));
            Assert.Equal(cpuCopy.A,testComputer.cpu.A);
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.Y,testComputer.cpu.Y);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            // N,Z,C Flags should be correct
            Assert.Equal(expN | expZ | expC , testComputer.cpu.P & (uint) (StatusFlagsMask.N | StatusFlagsMask.Z |  StatusFlagsMask.C));
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }

    }

   public class ROL_ZEROPAGEX
    {

        private Computer testComputer = new Computer();
        private uint opcode = 0x36;
        private int cycles = 6;
        private int bytes = 2;


        [Theory]
        // A, Carry, expected result, expected N flag, expected Z flag, Expected C flag, 
        [InlineData(0b0000_1111,0,0b0001_1110,(uint) 0, (uint) 0, (uint) 0)]
        [InlineData(0b0000_1111,(uint) StatusFlagsMask.C,0b0001_1111,(uint) 0, (uint) 0, (uint) 0)]
        [InlineData(0b1000_1111,(uint) 0,0b0001_1110,(uint) 0, (uint) 0, (uint) StatusFlagsMask.C)]
        [InlineData(0b1000_1111,(uint) StatusFlagsMask.C,0b0001_1111,(uint) 0, (uint) 0, (uint) StatusFlagsMask.C)]
        [InlineData(0b0100_1111,(uint) 0,0b1001_1110,(uint) StatusFlagsMask.N, (uint) 0, (uint) 0)]
        [InlineData(0b0100_1111,(uint) StatusFlagsMask.C,0b1001_1111,(uint) StatusFlagsMask.N, (uint) 0, (uint) 0)]
        [InlineData(0b0000_0000,(uint) 0,0b0000_0000,(uint) 0, (uint) StatusFlagsMask.Z, (uint) 0)]
        [InlineData(0b1000_0000,(uint) 0,0b0000_0000,(uint) 0, (uint) StatusFlagsMask.Z, (uint) StatusFlagsMask.C)]
        public void ROL_ShouldWork(uint value, uint carry, uint expRes, uint expN, uint expZ, uint expC)
        {
            testComputer.MemoryReset();
            
            uint X = 0x0A;
            uint addr = 0xAA;

            testComputer.mem.Write(0x0000,opcode);
            testComputer.mem.Write(0x0001,addr);
            testComputer.mem.Write(addr+X,value);

            testComputer.CPUReset();
            testComputer.cpu.X = X;
            testComputer.cpu.P |= carry;

            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles);

            Assert.Equal(expRes,testComputer.mem.Read(addr+X));
            Assert.Equal(cpuCopy.A,testComputer.cpu.A);
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.Y,testComputer.cpu.Y);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            // N,Z,C Flags should be correct
            Assert.Equal(expN | expZ | expC , testComputer.cpu.P & (uint) (StatusFlagsMask.N | StatusFlagsMask.Z |  StatusFlagsMask.C));
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }

        [Theory]
        // A, Carry, expected result, expected N flag, expected Z flag, Expected C flag, 
        [InlineData(0b0000_1111,0,0b0001_1110,(uint) 0, (uint) 0, (uint) 0)]
        [InlineData(0b0000_1111,(uint) StatusFlagsMask.C,0b0001_1111,(uint) 0, (uint) 0, (uint) 0)]
        [InlineData(0b1000_1111,(uint) 0,0b0001_1110,(uint) 0, (uint) 0, (uint) StatusFlagsMask.C)]
        [InlineData(0b1000_1111,(uint) StatusFlagsMask.C,0b0001_1111,(uint) 0, (uint) 0, (uint) StatusFlagsMask.C)]
        [InlineData(0b0100_1111,(uint) 0,0b1001_1110,(uint) StatusFlagsMask.N, (uint) 0, (uint) 0)]
        [InlineData(0b0100_1111,(uint) StatusFlagsMask.C,0b1001_1111,(uint) StatusFlagsMask.N, (uint) 0, (uint) 0)]
        [InlineData(0b0000_0000,(uint) 0,0b0000_0000,(uint) 0, (uint) StatusFlagsMask.Z, (uint) 0)]
        [InlineData(0b1000_0000,(uint) 0,0b0000_0000,(uint) 0, (uint) StatusFlagsMask.Z, (uint) StatusFlagsMask.C)]
        public void ROL_IndexWrapShouldWork(uint value, uint carry, uint expRes, uint expN, uint expZ, uint expC)
        {
            testComputer.MemoryReset();
            
            // x = A
            // ROL #A0,X
            uint X = 0xBB;
            uint addr = 0xAA;

            testComputer.mem.Write(0x0000,opcode);
            testComputer.mem.Write(0x0001,addr);
            testComputer.mem.Write( (addr+X) & 0xFF,value);

            testComputer.CPUReset();
            testComputer.cpu.X = X;
            testComputer.cpu.P |= carry;


            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles);

            Assert.Equal(expRes,testComputer.mem.Read((addr+X) & 0xFF));
            Assert.Equal(cpuCopy.A,testComputer.cpu.A);
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.Y,testComputer.cpu.Y);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            // N,Z,C Flags should be correct
            Assert.Equal(expN | expZ | expC , testComputer.cpu.P & (uint) (StatusFlagsMask.N | StatusFlagsMask.Z |  StatusFlagsMask.C));
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }
    }


    public class ROL_ABSOLUTE
    {

        private Computer testComputer = new Computer();
        private uint opcode = 0x2E;
        private int cycles = 6;
        private int bytes = 3;


        [Theory]
        // A, Carry, expected result, expected N flag, expected Z flag, Expected C flag, 
        [InlineData(0b0000_1111,0,0b0001_1110,(uint) 0, (uint) 0, (uint) 0)]
        [InlineData(0b0000_1111,(uint) StatusFlagsMask.C,0b0001_1111,(uint) 0, (uint) 0, (uint) 0)]
        [InlineData(0b1000_1111,(uint) 0,0b0001_1110,(uint) 0, (uint) 0, (uint) StatusFlagsMask.C)]
        [InlineData(0b1000_1111,(uint) StatusFlagsMask.C,0b0001_1111,(uint) 0, (uint) 0, (uint) StatusFlagsMask.C)]
        [InlineData(0b0100_1111,(uint) 0,0b1001_1110,(uint) StatusFlagsMask.N, (uint) 0, (uint) 0)]
        [InlineData(0b0100_1111,(uint) StatusFlagsMask.C,0b1001_1111,(uint) StatusFlagsMask.N, (uint) 0, (uint) 0)]
        [InlineData(0b0000_0000,(uint) 0,0b0000_0000,(uint) 0, (uint) StatusFlagsMask.Z, (uint) 0)]
        [InlineData(0b1000_0000,(uint) 0,0b0000_0000,(uint) 0, (uint) StatusFlagsMask.Z, (uint) StatusFlagsMask.C)]
        public void ROL_ShouldWork(uint value, uint carry, uint expRes, uint expN, uint expZ, uint expC)
        {
            testComputer.MemoryReset();
            
            uint addr = 0xBEEF;

            testComputer.mem.Write(0x0000,opcode);
            testComputer.mem.Write(0x0001,addr & 0x00FF);
            testComputer.mem.Write(0x0002,addr >> 8);       
            testComputer.mem.Write(addr,value);

            testComputer.CPUReset();
            testComputer.cpu.P |= carry;

            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles);

            Assert.Equal(expRes,testComputer.mem.Read(addr));
            Assert.Equal(cpuCopy.A,testComputer.cpu.A);
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.Y,testComputer.cpu.Y);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            // N,Z,C Flags should be correct
            Assert.Equal(expN | expZ | expC , testComputer.cpu.P & (uint) (StatusFlagsMask.N | StatusFlagsMask.Z |  StatusFlagsMask.C));
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }

    }
    

    public class ROL_ABSOLUTEX
    {

        private Computer testComputer = new Computer();
        private uint opcode = 0x3E;
        private int cycles = 7;
        private int bytes = 3;


        [Theory]
        // A, Carry, expected result, expected N flag, expected Z flag, Expected C flag, 
        [InlineData(0b0000_1111,0,0b0001_1110,(uint) 0, (uint) 0, (uint) 0)]
        [InlineData(0b0000_1111,(uint) StatusFlagsMask.C,0b0001_1111,(uint) 0, (uint) 0, (uint) 0)]
        [InlineData(0b1000_1111,(uint) 0,0b0001_1110,(uint) 0, (uint) 0, (uint) StatusFlagsMask.C)]
        [InlineData(0b1000_1111,(uint) StatusFlagsMask.C,0b0001_1111,(uint) 0, (uint) 0, (uint) StatusFlagsMask.C)]
        [InlineData(0b0100_1111,(uint) 0,0b1001_1110,(uint) StatusFlagsMask.N, (uint) 0, (uint) 0)]
        [InlineData(0b0100_1111,(uint) StatusFlagsMask.C,0b1001_1111,(uint) StatusFlagsMask.N, (uint) 0, (uint) 0)]
        [InlineData(0b0000_0000,(uint) 0,0b0000_0000,(uint) 0, (uint) StatusFlagsMask.Z, (uint) 0)]
        [InlineData(0b1000_0000,(uint) 0,0b0000_0000,(uint) 0, (uint) StatusFlagsMask.Z, (uint) StatusFlagsMask.C)]
        public void ROL_ShouldWork(uint value, uint carry, uint expRes, uint expN, uint expZ, uint expC)
        {
            testComputer.MemoryReset();
            
            // ROL #0xBEE0,X
            uint X = 0x0F;
            uint addr = 0xBEE0;

            testComputer.mem.Write(0x0000,opcode);
            testComputer.mem.Write(0x0001,addr & 0x00FF);
            testComputer.mem.Write(0x0002,addr >> 8);       
            testComputer.mem.Write(addr+X,value);

            testComputer.CPUReset();
            testComputer.cpu.X = X;
            testComputer.cpu.P |= carry;

            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles);

            Assert.Equal(expRes,testComputer.mem.Read(addr+X));
            Assert.Equal(cpuCopy.A,testComputer.cpu.A);
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.Y,testComputer.cpu.Y);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            // N,Z,C Flags should be correct
            Assert.Equal(expN | expZ | expC , testComputer.cpu.P & (uint) (StatusFlagsMask.N | StatusFlagsMask.Z |  StatusFlagsMask.C));
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }

        [Theory]
        // A, Carry, expected result, expected N flag, expected Z flag, Expected C flag, 
        [InlineData(0b0000_1111,0,0b0001_1110,(uint) 0, (uint) 0, (uint) 0)]
        [InlineData(0b0000_1111,(uint) StatusFlagsMask.C,0b0001_1111,(uint) 0, (uint) 0, (uint) 0)]
        [InlineData(0b1000_1111,(uint) 0,0b0001_1110,(uint) 0, (uint) 0, (uint) StatusFlagsMask.C)]
        [InlineData(0b1000_1111,(uint) StatusFlagsMask.C,0b0001_1111,(uint) 0, (uint) 0, (uint) StatusFlagsMask.C)]
        [InlineData(0b0100_1111,(uint) 0,0b1001_1110,(uint) StatusFlagsMask.N, (uint) 0, (uint) 0)]
        [InlineData(0b0100_1111,(uint) StatusFlagsMask.C,0b1001_1111,(uint) StatusFlagsMask.N, (uint) 0, (uint) 0)]
        [InlineData(0b0000_0000,(uint) 0,0b0000_0000,(uint) 0, (uint) StatusFlagsMask.Z, (uint) 0)]
        [InlineData(0b1000_0000,(uint) 0,0b0000_0000,(uint) 0, (uint) StatusFlagsMask.Z, (uint) StatusFlagsMask.C)]
        public void ROL_PageBoundaryCrossShouldWork(uint value, uint carry,uint expRes, uint expN, uint expZ, uint expC)
        {
            testComputer.MemoryReset();
            
            // ROL #0xBEE0,Y
            uint X = 0xFA;
            uint addr = 0xBEE0;

            testComputer.mem.Write(0x0000,opcode);
            testComputer.mem.Write(0x0001,addr & 0x00FF);
            testComputer.mem.Write(0x0002,addr >> 8);       
            testComputer.mem.Write(addr+X,value);

            testComputer.CPUReset();
            testComputer.cpu.X = X;
            testComputer.cpu.P |= carry;

            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles);

            Assert.Equal(expRes,testComputer.mem.Read(addr+X));
            Assert.Equal(cpuCopy.A,testComputer.cpu.A);
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.Y,testComputer.cpu.Y);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            // N,Z,C Flags should be correct
            Assert.Equal(expN | expZ | expC , testComputer.cpu.P & (uint) (StatusFlagsMask.N | StatusFlagsMask.Z |  StatusFlagsMask.C));
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }

    }

}
