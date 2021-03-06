using System;
using Xunit;
using C6502;

namespace C6502.Tests
{


    public class ASL_ACCUMULATOR
    {

        private Computer testComputer = new Computer();
        private uint opcode = 0x0A;
        private int cycles = 2;
        private int bytes = 1;


        [Theory]
        // A, expected result, expected N flag, expected Z flag, Expected C flag, 
        [InlineData(0b0000_1111,0b0001_1110,(uint) 0, (uint) 0, (uint) 0)]
        [InlineData(0b0100_1111,0b1001_1110, (uint) StatusFlagsMask.N, (uint) 0, (uint) 0)]
        [InlineData(0b1000_1111,0b0001_1110, (uint) 0, (uint) 0, (uint) StatusFlagsMask.C)]
        [InlineData(0b1000_0000,0b0000_0000, (uint) 0, (uint) StatusFlagsMask.Z, (uint) StatusFlagsMask.C)]
        public void ASL_ShouldWork(uint A, uint expRes, uint expN, uint expZ,uint expC)
        {
            testComputer.MemoryReset();
            
            
            // ASL #$32
            testComputer.mem.Write(0x0000,opcode);

            testComputer.CPUReset();
            testComputer.cpu.A = A;

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



    public class ASL_ZEROPAGE_RMW
    {

        private Computer testComputer = new Computer();
        private uint opcode = 0x06;
        private int cycles = 5;
        private int bytes = 2;


        [Theory]
        // value, expected result, expected N flag, expected Z flag, Expected C flag, 
        [InlineData(0b0000_1111,0b0001_1110,(uint) 0, (uint) 0, (uint) 0)]
        [InlineData(0b0100_1111,0b1001_1110, (uint) StatusFlagsMask.N, (uint) 0, (uint) 0)]
        [InlineData(0b1000_1111,0b0001_1110, (uint) 0, (uint) 0, (uint) StatusFlagsMask.C)]
        [InlineData(0b1000_0000,0b0000_0000, (uint) 0, (uint) StatusFlagsMask.Z, (uint) StatusFlagsMask.C)]
        public void ASL_ShouldWork(uint value, uint expRes, uint expN, uint expZ, uint expC)
        {
            testComputer.MemoryReset();
            
            // ASL #AA
            uint addr = 0xAA;

            testComputer.mem.Write(0x0000,opcode);
            testComputer.mem.Write(0x0001,addr);
            testComputer.mem.Write(addr,value);

            testComputer.CPUReset();

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

   public class ASL_ZEROPAGEX
    {

        private Computer testComputer = new Computer();
        private uint opcode = 0x16;
        private int cycles = 6;
        private int bytes = 2;


        [Theory]
        // value, expected result, expected N flag, expected Z flag, Expected C flag, 
        [InlineData(0b0000_1111,0b0001_1110,(uint) 0, (uint) 0, (uint) 0)]
        [InlineData(0b0100_1111,0b1001_1110, (uint) StatusFlagsMask.N, (uint) 0, (uint) 0)]
        [InlineData(0b1000_1111,0b0001_1110, (uint) 0, (uint) 0, (uint) StatusFlagsMask.C)]
        [InlineData(0b1000_0000,0b0000_0000, (uint) 0, (uint) StatusFlagsMask.Z, (uint) StatusFlagsMask.C)]
        public void ASL_ShouldWork(uint value, uint expRes, uint expN, uint expZ, uint expC)
        {
            testComputer.MemoryReset();
            
            uint X = 0x0A;
            uint addr = 0xAA;

            testComputer.mem.Write(0x0000,opcode);
            testComputer.mem.Write(0x0001,addr);
            testComputer.mem.Write(addr+X,value);

            testComputer.CPUReset();
            testComputer.cpu.X = X;

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
        // value, expected result, expected N flag, expected Z flag, Expected C flag, 
        [InlineData(0b0000_1111,0b0001_1110,(uint) 0, (uint) 0, (uint) 0)]
        [InlineData(0b0100_1111,0b1001_1110, (uint) StatusFlagsMask.N, (uint) 0, (uint) 0)]
        [InlineData(0b1000_1111,0b0001_1110, (uint) 0, (uint) 0, (uint) StatusFlagsMask.C)]
        [InlineData(0b1000_0000,0b0000_0000, (uint) 0, (uint) StatusFlagsMask.Z, (uint) StatusFlagsMask.C)]
        public void ASL_IndexWrapShouldWork(uint value, uint expRes, uint expN, uint expZ, uint expC)
        {
            testComputer.MemoryReset();
            
            // x = A
            // ASL #A0,X
            uint X = 0xBB;
            uint addr = 0xAA;

            testComputer.mem.Write(0x0000,opcode);
            testComputer.mem.Write(0x0001,addr);
            testComputer.mem.Write( (addr+X) & 0xFF,value);

            testComputer.CPUReset();
            testComputer.cpu.X = X;


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


    public class ASL_ABSOLUTE
    {

        private Computer testComputer = new Computer();
        private uint opcode = 0x0E;
        private int cycles = 6;
        private int bytes = 3;


        [Theory]
        // value, expected result, expected N flag, expected Z flag, Expected C flag, 
        [InlineData(0b0000_1111,0b0001_1110,(uint) 0, (uint) 0, (uint) 0)]
        [InlineData(0b0100_1111,0b1001_1110, (uint) StatusFlagsMask.N, (uint) 0, (uint) 0)]
        [InlineData(0b1000_1111,0b0001_1110, (uint) 0, (uint) 0, (uint) StatusFlagsMask.C)]
        [InlineData(0b1000_0000,0b0000_0000, (uint) 0, (uint) StatusFlagsMask.Z, (uint) StatusFlagsMask.C)]
        public void ASL_ShouldWork(uint value, uint expRes, uint expN, uint expZ, uint expC)
        {
            testComputer.MemoryReset();
            
            uint addr = 0xBEEF;

            testComputer.mem.Write(0x0000,opcode);
            testComputer.mem.Write(0x0001,addr & 0x00FF);
            testComputer.mem.Write(0x0002,addr >> 8);       
            testComputer.mem.Write(addr,value);

            testComputer.CPUReset();

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
    

    public class ASL_ABSOLUTEX
    {

        private Computer testComputer = new Computer();
        private uint opcode = 0x1E;
        private int cycles = 7;
        private int bytes = 3;


        [Theory]
        // value, expected result, expected N flag, expected Z flag, Expected C flag, 
        [InlineData(0b0000_1111,0b0001_1110,(uint) 0, (uint) 0, (uint) 0)]
        [InlineData(0b0100_1111,0b1001_1110, (uint) StatusFlagsMask.N, (uint) 0, (uint) 0)]
        [InlineData(0b1000_1111,0b0001_1110, (uint) 0, (uint) 0, (uint) StatusFlagsMask.C)]
        [InlineData(0b1000_0000,0b0000_0000, (uint) 0, (uint) StatusFlagsMask.Z, (uint) StatusFlagsMask.C)]
        public void ASL_ShouldWork(uint value, uint expRes, uint expN, uint expZ, uint expC)
        {
            testComputer.MemoryReset();
            
            // ASL #0xBEE0,X
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
        // value, expected result, expected N flag, expected Z flag, Expected C flag, 
        [InlineData(0b0000_1111,0b0001_1110,(uint) 0, (uint) 0, (uint) 0)]
        [InlineData(0b0100_1111,0b1001_1110, (uint) StatusFlagsMask.N, (uint) 0, (uint) 0)]
        [InlineData(0b1000_1111,0b0001_1110, (uint) 0, (uint) 0, (uint) StatusFlagsMask.C)]
        [InlineData(0b1000_0000,0b0000_0000, (uint) 0, (uint) StatusFlagsMask.Z, (uint) StatusFlagsMask.C)]
        public void ASL_PageBoundaryCrossShouldWork(uint value, uint expRes, uint expN, uint expZ, uint expC)
        {
            testComputer.MemoryReset();
            
            // ASL #0xBEE0,Y
            uint X = 0xFA;
            uint addr = 0xBEE0;

            testComputer.mem.Write(0x0000,opcode);
            testComputer.mem.Write(0x0001,addr & 0x00FF);
            testComputer.mem.Write(0x0002,addr >> 8);       
            testComputer.mem.Write(addr+X,value);

            testComputer.CPUReset();
            testComputer.cpu.X = X;

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
