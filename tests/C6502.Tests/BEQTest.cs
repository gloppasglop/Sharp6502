using System;
using Xunit;
using C6502;

namespace C6502.Tests
{

    public class BEQ_RELATIVE
    {

        private Computer testComputer = new Computer();
        private uint opcode = 0xF0;
        private int cycles = 2;
        private int bytes = 2;


        [Fact]
        public void BEQ_NotTakenShouldWork()
        {
            testComputer.MemoryReset();
            
            uint startAddr = 0x1080;
            uint offset = 0x0A;

            testComputer.mem.Write(startAddr,opcode);
            testComputer.mem.Write(startAddr+1,offset);

            testComputer.CPUReset();
            testComputer.cpu.PC = startAddr;
            testComputer.cpu.AddrPins = testComputer.cpu.PC;
            testComputer.cpu.DataPins = testComputer.mem.Read(testComputer.cpu.PC);

            testComputer.cpu.P &= (uint) ~StatusFlagsMask.Z;

            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles);

            Assert.Equal(cpuCopy.A,testComputer.cpu.A);
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.Y,testComputer.cpu.Y);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            Assert.Equal(cpuCopy.P,testComputer.cpu.P);
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }

        [Theory]
        [InlineData(0x1000,0x00,0x1002)]
        [InlineData(0x1000,0x0A,0x100C)]
        [InlineData(0x1000,0x66,0x1068)]
        [InlineData(0x1000,0x78,0x107A)]
        [InlineData(0x1000,0x7F,0x1081)]
        [InlineData(0x10FF,0x01,0x1102)]
        [InlineData(0x10FE,0x01,0x1101)]

        public void BEQ_ForwardTakenSamePageShouldWork(uint startAddr, uint offset, uint endAddr)
        {
            testComputer.MemoryReset();

            testComputer.mem.Write(startAddr,opcode);
            testComputer.mem.Write(startAddr+1,offset);

            testComputer.CPUReset();

            testComputer.cpu.PC = startAddr;
            testComputer.cpu.AddrPins = testComputer.cpu.PC;
            testComputer.cpu.DataPins = testComputer.mem.Read(testComputer.cpu.PC);
            testComputer.cpu.P |= (uint) StatusFlagsMask.Z;

            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles+1);

            Assert.True(testComputer.cpu.SYNC);
            Assert.Equal(cpuCopy.A,testComputer.cpu.A);
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.Y,testComputer.cpu.Y);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            Assert.Equal(cpuCopy.P,testComputer.cpu.P);
            Assert.Equal(endAddr,testComputer.cpu.PC);
        }

        [Theory]
        [InlineData(0x10F0,0x0E,0x1100)]
        [InlineData(0x10F0,0x66,0x1158)]
        [InlineData(0x10F0,0x79,0x116B)]
        [InlineData(0x10F0,0x7F,0x1171)]

        public void BEQ_ForwardTakenDifferentPageShouldWork(uint startAddr, uint offset, uint endAddr)
        {
            testComputer.MemoryReset();

            testComputer.mem.Write(startAddr,opcode);
            testComputer.mem.Write(startAddr+1,offset);

            testComputer.CPUReset();

            testComputer.cpu.PC = startAddr;
            testComputer.cpu.AddrPins = testComputer.cpu.PC;
            testComputer.cpu.DataPins = testComputer.mem.Read(testComputer.cpu.PC);
            testComputer.cpu.P |= (uint) StatusFlagsMask.Z;

            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles+2);

            Assert.True(testComputer.cpu.SYNC);
            Assert.Equal(cpuCopy.A,testComputer.cpu.A);
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.Y,testComputer.cpu.Y);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            Assert.Equal(cpuCopy.P,testComputer.cpu.P);
            Assert.Equal(endAddr,testComputer.cpu.PC);
        }

 
        [Fact]
        public void BEQ_BackwardTakenSamePageShouldWork()
        {
            testComputer.MemoryReset();
            
            uint startAddr = 0x1080;
            uint offset = 0xF6;

            testComputer.mem.Write(startAddr,opcode);
            testComputer.mem.Write(startAddr+1,offset);

            testComputer.CPUReset();

            testComputer.cpu.PC = startAddr;
            testComputer.cpu.AddrPins = testComputer.cpu.PC;
            testComputer.cpu.DataPins = testComputer.mem.Read(testComputer.cpu.PC);
            testComputer.cpu.P |= (uint) StatusFlagsMask.Z;

            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles+1);

            Assert.Equal(cpuCopy.A,testComputer.cpu.A);
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.Y,testComputer.cpu.Y);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            Assert.Equal(cpuCopy.P,testComputer.cpu.P);
            Assert.Equal((uint) 0x1078,testComputer.cpu.PC);
        }

        [Fact]
        public void BEQ_BackwardTakenDifferentPageShouldWork()
        {
            testComputer.MemoryReset();
            
            uint startAddr = 0x1010;
            uint offset = 0xA6;

            testComputer.mem.Write(startAddr,opcode);
            testComputer.mem.Write(startAddr+1,offset);

            testComputer.CPUReset();

            testComputer.cpu.PC = startAddr;
            testComputer.cpu.AddrPins = testComputer.cpu.PC;
            testComputer.cpu.DataPins = testComputer.mem.Read(testComputer.cpu.PC);
            testComputer.cpu.P |= (uint) StatusFlagsMask.Z;

            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles+2);

            Assert.Equal(cpuCopy.A,testComputer.cpu.A);
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.Y,testComputer.cpu.Y);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            Assert.Equal(cpuCopy.P,testComputer.cpu.P);
            Assert.Equal((uint) 0x0FB8,testComputer.cpu.PC);
        }
 
   }

}
