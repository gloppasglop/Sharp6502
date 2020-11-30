using System;
using Xunit;
using C6502;

namespace C6502.Tests
{

    public class BVS_RELATIVE
    {

        private Computer testComputer = new Computer();
        private uint opcode = 0x70;
        private int cycles = 2;
        private int bytes = 2;


        [Fact]
        public void BVS_NotTakenShouldWork()
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

            testComputer.cpu.P &= (uint) ~StatusFlagsMask.V;

            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles);

            Assert.Equal(cpuCopy.A,testComputer.cpu.A);
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.Y,testComputer.cpu.Y);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            Assert.Equal(cpuCopy.P,testComputer.cpu.P);
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }

        [Fact]
        public void BVS_ForwardTakenSamePageShouldWork()
        {
            testComputer.MemoryReset();
            
            uint startAddr = 0x10A0;
            uint offset = 0x0A;

            testComputer.mem.Write(startAddr,opcode);
            testComputer.mem.Write(startAddr+1,offset);

            testComputer.CPUReset();

            testComputer.cpu.PC = startAddr;
            testComputer.cpu.AddrPins = testComputer.cpu.PC;
            testComputer.cpu.DataPins = testComputer.mem.Read(testComputer.cpu.PC);
            testComputer.cpu.P |= (uint) StatusFlagsMask.V;

            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles+1);

            Assert.Equal(cpuCopy.A,testComputer.cpu.A);
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.Y,testComputer.cpu.Y);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            Assert.Equal(cpuCopy.P,testComputer.cpu.P);
            Assert.Equal((uint) 0x10AC,testComputer.cpu.PC);
        }

        [Fact]
        public void BVS_ForwardTakenDifferentPageShouldWork()
        {
            testComputer.MemoryReset();
            
            uint startAddr = 0x10A0;
            uint offset = 0x79;

            testComputer.mem.Write(startAddr,opcode);
            testComputer.mem.Write(startAddr+1,offset);

            testComputer.CPUReset();

            testComputer.cpu.PC = startAddr;
            testComputer.cpu.AddrPins = testComputer.cpu.PC;
            testComputer.cpu.DataPins = testComputer.mem.Read(testComputer.cpu.PC);
            testComputer.cpu.P |= (uint) StatusFlagsMask.V;

            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles+2);

            Assert.Equal(cpuCopy.A,testComputer.cpu.A);
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.Y,testComputer.cpu.Y);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            Assert.Equal(cpuCopy.P,testComputer.cpu.P);
            Assert.Equal((uint) 0x111C,testComputer.cpu.PC);
        }

 
        [Fact]
        public void BVS_BackwardTakenSamePageShouldWork()
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
            testComputer.cpu.P |= (uint) StatusFlagsMask.V;

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
        public void BVS_BackwardTakenDifferentPageShouldWork()
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
            testComputer.cpu.P |= (uint) StatusFlagsMask.V;

            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles+2);

            Assert.Equal(cpuCopy.A,testComputer.cpu.A);
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.Y,testComputer.cpu.Y);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            Assert.Equal(cpuCopy.P,testComputer.cpu.P);
            Assert.Equal((uint) 0x0FB9,testComputer.cpu.PC);
        }
 
   }

}
