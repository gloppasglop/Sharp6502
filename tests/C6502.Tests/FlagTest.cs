using System;
using Xunit;
using C6502;

namespace C6502.Tests
{


    public class SEC
    {

        private Computer testComputer = new Computer();
        private uint opcode = 0x38;
        private int cycles = 2;
        private int bytes = 1;


        [Fact]
        public void SEC_ShouldSetCarry()
        {
            testComputer.MemoryReset();
            
            testComputer.mem.Write(0x0000,opcode);

            testComputer.CPUReset();

            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles);

            Assert.Equal(cpuCopy.A,testComputer.cpu.A);
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.Y,testComputer.cpu.Y);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            // Carry flag should be set
            Assert.Equal((uint) StatusFlagsMask.C, testComputer.cpu.P & (uint) StatusFlagsMask.C);
            // Other flags should be unchanged
            Assert.Equal(cpuCopy.P & (uint) ~StatusFlagsMask.C,testComputer.cpu.P & (uint) ~StatusFlagsMask.C);
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }
    }
    public class SED
    {

        private Computer testComputer = new Computer();
        private uint opcode = 0xF8;
        private int cycles = 2;
        private int bytes = 1;


        [Fact]
        public void SED_ShouldSetDecimal()
        {
            testComputer.MemoryReset();
            
            testComputer.mem.Write(0x0000,opcode);

            testComputer.CPUReset();

            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles);

            Assert.Equal(cpuCopy.A,testComputer.cpu.A);
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.Y,testComputer.cpu.Y);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            // Decimal flag should be set
            Assert.Equal((uint) StatusFlagsMask.D, testComputer.cpu.P & (uint) StatusFlagsMask.D);
            // Other flags should be unchanged
            Assert.Equal(cpuCopy.P & (uint) ~StatusFlagsMask.D,testComputer.cpu.P & (uint) ~StatusFlagsMask.D);
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }
    }
    public class SEI
    {

        private Computer testComputer = new Computer();
        private uint opcode = 0x78;
        private int cycles = 2;
        private int bytes = 1;


        [Fact]
        public void SEI_ShouldSetInterrupt()
        {
            testComputer.MemoryReset();
            
            testComputer.mem.Write(0x0000,opcode);

            testComputer.CPUReset();

            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles);

            Assert.Equal(cpuCopy.A,testComputer.cpu.A);
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.Y,testComputer.cpu.Y);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            // Interrupt flag should be set
            Assert.Equal((uint) StatusFlagsMask.I, testComputer.cpu.P & (uint) StatusFlagsMask.I);
            // Other flags should be unchanged
            Assert.Equal(cpuCopy.P & (uint) ~StatusFlagsMask.I,testComputer.cpu.P & (uint) ~StatusFlagsMask.I);
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }
    }

    public class CLC
    {

        private Computer testComputer = new Computer();
        private uint opcode = 0x18;
        private int cycles = 2;
        private int bytes = 1;


        [Fact]
        public void CLC_ShouldCLearCarry()
        {
            testComputer.MemoryReset();
            
            testComputer.mem.Write(0x0000,opcode);

            testComputer.CPUReset();

            // Set Carry
            testComputer.cpu.P &= (uint) StatusFlagsMask.C;

            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles);

            Assert.Equal(cpuCopy.A,testComputer.cpu.A);
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.Y,testComputer.cpu.Y);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            // Carry flag should be Cleared
            Assert.Equal((uint) 0, testComputer.cpu.P & (uint) StatusFlagsMask.C);
            // Other flags should be unchanged
            Assert.Equal(cpuCopy.P & (uint) ~StatusFlagsMask.C,testComputer.cpu.P & (uint) ~StatusFlagsMask.C);
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }
    }
    public class CLD
    {

        private Computer testComputer = new Computer();
        private uint opcode = 0xD8;
        private int cycles = 2;
        private int bytes = 1;


        [Fact]
        public void CLD_ShouldClearDecimal()
        {
            testComputer.MemoryReset();
            
            testComputer.mem.Write(0x0000,opcode);

            testComputer.CPUReset();

            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles);

            Assert.Equal(cpuCopy.A,testComputer.cpu.A);
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.Y,testComputer.cpu.Y);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            // Decimal flag should be set
            Assert.Equal((uint) 0, testComputer.cpu.P & (uint) StatusFlagsMask.D);
            // Other flags should be unchanged
            Assert.Equal(cpuCopy.P & (uint) ~StatusFlagsMask.D,testComputer.cpu.P & (uint) ~StatusFlagsMask.D);
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }
    }
    public class CLI
    {

        private Computer testComputer = new Computer();
        private uint opcode = 0x58;
        private int cycles = 2;
        private int bytes = 1;


        [Fact]
        public void CLI_ShouldClearInterrupt()
        {
            testComputer.MemoryReset();
            
            testComputer.mem.Write(0x0000,opcode);

            testComputer.CPUReset();

            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles);

            Assert.Equal(cpuCopy.A,testComputer.cpu.A);
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.Y,testComputer.cpu.Y);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            // Interrupt flag should be set
            Assert.Equal((uint) 0, testComputer.cpu.P & (uint) StatusFlagsMask.I);
            // Other flags should be unchanged
            Assert.Equal(cpuCopy.P & (uint) ~StatusFlagsMask.I,testComputer.cpu.P & (uint) ~StatusFlagsMask.I);
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }
    }

    public class CLV
    {

        private Computer testComputer = new Computer();
        private uint opcode = 0xB8;
        private int cycles = 2;
        private int bytes = 1;


        [Fact]
        public void CLV_ShouldClearOverflow()
        {
            testComputer.MemoryReset();
            
            testComputer.mem.Write(0x0000,opcode);

            testComputer.CPUReset();

            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles);

            Assert.Equal(cpuCopy.A,testComputer.cpu.A);
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.Y,testComputer.cpu.Y);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            // Interrupt flag should be set
            Assert.Equal((uint) 0, testComputer.cpu.P & (uint) StatusFlagsMask.I);
            // Other flags should be unchanged
            Assert.Equal(cpuCopy.P & (uint) ~StatusFlagsMask.I,testComputer.cpu.P & (uint) ~StatusFlagsMask.I);
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }
    }



}
