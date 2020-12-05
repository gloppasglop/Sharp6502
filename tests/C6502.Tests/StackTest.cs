using System;
using Xunit;
using C6502;

namespace C6502.Tests
{


    public class PHA_IMPLIED
    {

        private Computer testComputer = new Computer();
        private uint opcode = 0x48;
        private int cycles = 3;
        private int bytes = 1;


        [Fact]
        public void PHA_OneItemShouldWork()
        {
            testComputer.MemoryReset();
            
            uint A = 0x32;

            testComputer.mem.Write(0x0000,opcode);

            testComputer.CPUReset();

            testComputer.cpu.A = A;

            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles);

            Assert.Equal(cpuCopy.A,testComputer.cpu.A);
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.Y,testComputer.cpu.Y);
            // Stack pointer should be decremented by 1
            Assert.Equal(cpuCopy.S-1,testComputer.cpu.S);
            // memory pointed by stack pointer +1 should contain pushed value
            Assert.Equal(A,testComputer.mem.Read(0x0100+cpuCopy.S));
            Assert.Equal(cpuCopy.P,testComputer.cpu.P);
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }

        [Fact]
        public void PHA_MultipleItemsShouldWork()
        {
            testComputer.MemoryReset();
            
            uint A = 0x32;

            uint length = 5;
            for (uint i = 0; i < length; i++)
            {
                testComputer.mem.Write(i,opcode);
            }

            testComputer.CPUReset();

            testComputer.cpu.A = A;

            var cpuCopy = testComputer.Clone();

            for (uint i = 0; i < length; i++)
            {
                testComputer.cpu.A = A+i;
                testComputer.Execute(cycles);
            }
            // Stack pointer should be decremented by length
            Assert.Equal(cpuCopy.S-length,testComputer.cpu.S);
            // check values correcly pushed to stack
            for (uint i = 0; i < length; i++)
            {
                Assert.Equal(A+i,testComputer.mem.Read(0x100+cpuCopy.S-i));
            }
        }

    }
    public class PHP_IMPLIED
    {

        private Computer testComputer = new Computer();
        private uint opcode = 0x08;
        private int cycles = 3;
        private int bytes = 1;


        [Fact]
        public void PHP_OneItemShouldWork()
        {
            testComputer.MemoryReset();
            
            uint P = 0x32;

            testComputer.mem.Write(0x0000,opcode);

            testComputer.CPUReset();

            testComputer.cpu.P = P;

            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles);

            Assert.Equal(cpuCopy.A,testComputer.cpu.A);
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.Y,testComputer.cpu.Y);
            // Stack pointer should be decremented by 1
            Assert.Equal(cpuCopy.S-1,testComputer.cpu.S);
            // memory pointed by stack pointer +1 should contain pushed value
            Assert.Equal(P,testComputer.mem.Read(0x0100+cpuCopy.S));
            Assert.Equal(cpuCopy.P,testComputer.cpu.P);
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }

        [Fact]
        public void PHP_MultipleItemsShouldWork()
        {
            testComputer.MemoryReset();
            
            uint P = 0x32;

            uint length = 5;
            for (uint i = 0; i < length; i++)
            {
                testComputer.mem.Write(i,opcode);
            }

            testComputer.CPUReset();

            var cpuCopy = testComputer.Clone();

            for (uint i = 0; i < length; i++)
            {
                testComputer.cpu.P = P+i;
                testComputer.Execute(cycles);
            }
            // Stack pointer should be decremented by length
            Assert.Equal(cpuCopy.S-length,testComputer.cpu.S);
            // check values correcly pushed to stack
            for (uint i = 0; i < length; i++)
            {
                Assert.Equal(P+i,testComputer.mem.Read(0x100+cpuCopy.S-i));
            }
        }

    }

    public class PLA_IMPLIED
    {

        private Computer testComputer = new Computer();
        private uint opcode = 0x68;
        private int cycles = 4;
        private int bytes = 1;


        [Fact]
        public void PLA_MultipleItemsShouldWork()
        {
            testComputer.MemoryReset();
            
            uint A = 0x32;

            uint length = 5;
            for (uint i = 0; i < length; i++)
            {
                testComputer.mem.Write(i,opcode);
            }

            testComputer.CPUReset();

            for (uint i = 0; i < length; i++)
            {
                testComputer.mem.Write(0x01FF-i,A+i);
            }
            testComputer.cpu.S = 0xFF-length;

            var cpuCopy = testComputer.Clone();

            for (uint i = 0; i < length; i++)
            {
                testComputer.Execute(cycles);
                Assert.Equal(A+length-i-1,testComputer.cpu.A);
            }
            // Stack pointer should be decremented by length
            Assert.Equal(cpuCopy.S+length,testComputer.cpu.S);
        }

        [Fact]
        public void PLA_ZeroShouldSetZFlag()
        {
            testComputer.MemoryReset();
            
            uint value = 0x00;

            testComputer.mem.Write(0x0000,opcode);

            testComputer.mem.Write(0x01FF,0x32);
            testComputer.mem.Write(0x01FE,value);

            testComputer.CPUReset();
            testComputer.cpu.S = 0xFD;

            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles);

            Assert.Equal(value,testComputer.cpu.A);
            // Zero flag should be true
            Assert.Equal((uint) StatusFlagsMask.Z, testComputer.cpu.P & (uint) StatusFlagsMask.Z);
            // Negativeflag should be false
            Assert.Equal((uint) 0, testComputer.cpu.P & (uint) StatusFlagsMask.N);
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }

        [Fact]
        public void PLA_ZeroShouldSetNegativeFlag()
        {
            testComputer.MemoryReset();
            
            uint value = 0x84;

            testComputer.mem.Write(0x0000,opcode);

            testComputer.mem.Write(0x01FF,0x32);
            testComputer.mem.Write(0x01FE,value);

            testComputer.CPUReset();
            testComputer.cpu.S = 0xFD;

            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles);

            Assert.Equal(value,testComputer.cpu.A);
            // Zero flag should be false
            Assert.Equal((uint) 0, testComputer.cpu.P & (uint) StatusFlagsMask.Z);
            // Negativeflag should be false
            Assert.Equal((uint) StatusFlagsMask.N, testComputer.cpu.P & (uint) StatusFlagsMask.N);
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }
 
    }
   public class PLP_IMPLIED
    {

        private Computer testComputer = new Computer();
        private uint opcode = 0x28;
        private int cycles = 4;
        private int bytes = 1;


        [Fact]
        public void PLP_ShouldWork()
        {
            testComputer.MemoryReset();
            
            uint value = 0x84;

            testComputer.mem.Write(0x0000,opcode);

            testComputer.mem.Write(0x01FF,0x32);
            testComputer.mem.Write(0x01FE,value);

            testComputer.CPUReset();
            testComputer.cpu.S = 0xFD;

            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles);

            // Reading X and B flags set them high
            Assert.Equal(value |(uint) StatusFlagsMask.X | (uint) StatusFlagsMask.B,testComputer.cpu.P);
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }
 
    }


}
