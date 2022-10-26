using System;
using Xunit;
using C6502;

namespace C6502.Tests
{

    public class BIT_ZEROPAGE
    {

        private Computer testComputer = new Computer();
        private uint opcode = 0x24;
        private int cycles = 3;
        private int bytes = 2;


        [Theory]
        // register, Memory, expected Z, Expected N, expected V
        [InlineData(0b0001_1111,0b0001_1111,(uint) 0,(uint) 0,(uint) 0)]
        [InlineData(0b0000_0011,0b0000_1100,(uint) StatusFlagsMask.Z,(uint) 0,(uint) 0)]
        [InlineData(0b0100_0011,0b0100_1100,(uint) 0,(uint) 0,(uint) StatusFlagsMask.V)]
        [InlineData(0b1000_0011,0b1000_1100,(uint) 0,(uint) StatusFlagsMask.N,(uint) 0)]
        public void BIT_ShouldWork(uint A,uint value, uint expZ, uint expN, uint expV)
        {
            testComputer.MemoryReset();
            
            uint addr = 0xAA;
            testComputer.mem.Write(0x0000,opcode);
            testComputer.mem.Write(0x0001,addr);
            testComputer.mem.Write(addr,value);

            testComputer.CPUReset();
            testComputer.cpu.A = A;

            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles);

            Assert.Equal(cpuCopy.A,testComputer.cpu.A);
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.Y,testComputer.cpu.Y);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            // Zero flag should be correct
            Assert.Equal(expZ  , testComputer.cpu.P & (uint) StatusFlagsMask.Z);
            // Overflow flag should be set with bit 6
            Assert.Equal( expV , testComputer.cpu.P & (uint) StatusFlagsMask.V);
            // Carry flag should be unchanged
            Assert.Equal(cpuCopy.P & (uint) StatusFlagsMask.C, testComputer.cpu.P & (uint) StatusFlagsMask.C);
            // Negative flag should be correct
            Assert.Equal(expN, testComputer.cpu.P & (uint) StatusFlagsMask.N);
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }
    }

    public class BIT_ABSOLUTE
    {

        private Computer testComputer = new Computer();
        private uint opcode = 0x2C;
        private int cycles = 4;
        private int bytes = 3;


        [Theory]
        [InlineData(0b0001_1111,0b0001_1111,(uint) 0,(uint) 0,(uint) 0)]
        [InlineData(0b0000_0011,0b0000_1100,(uint) StatusFlagsMask.Z,(uint) 0,(uint) 0)]
        [InlineData(0b0100_0011,0b0100_1100,(uint) 0,(uint) 0,(uint) StatusFlagsMask.V)]
        [InlineData(0b1000_0011,0b1000_1100,(uint) 0,(uint) StatusFlagsMask.N,(uint) 0)]
        public void BIT_ShouldWork(uint A,uint value, uint expZ, uint expN, uint expV)
        {
            testComputer.MemoryReset();
            
            
            uint addr = 0xBEEF;

            testComputer.mem.Write(0x0000,opcode);
            testComputer.mem.Write(0x0001,addr & 0x00FF);
            testComputer.mem.Write(0x0002,addr >> 8);       
            testComputer.mem.Write(addr,value);

            testComputer.CPUReset();
            testComputer.cpu.A = A;

            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles);

            Assert.Equal(cpuCopy.A,testComputer.cpu.A);
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.Y,testComputer.cpu.Y);
             // Zero flag should be correct
            Assert.Equal(expZ  , testComputer.cpu.P & (uint) StatusFlagsMask.Z);
            // Overflow flag should be set with bit 6
            Assert.Equal( expV , testComputer.cpu.P & (uint) StatusFlagsMask.V);
            // Carry flag should be unchanged
            Assert.Equal(cpuCopy.P & (uint) StatusFlagsMask.C, testComputer.cpu.P & (uint) StatusFlagsMask.C);
            // Negative flag should be correct
            Assert.Equal(expN, testComputer.cpu.P & (uint) StatusFlagsMask.N);
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }

    }

}
