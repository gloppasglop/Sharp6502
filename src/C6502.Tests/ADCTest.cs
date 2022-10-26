using System;
using Xunit;
using C6502;

namespace C6502.Tests
{

    public class ADC_IMMEDIATE
    {

        private Computer testComputer = new Computer();
        private uint opcode = 0x69;
        private int cycles = 2;
        private int bytes = 2;


        [Theory]
        [InlineData(0x32,0x32)]
        [InlineData(0x0,0x32)]
        [InlineData(0x32,0x0)]
        [InlineData(0x32,0x83)]
        [InlineData(0x83,0x32)]
        [InlineData(0x83,0x0)]
        [InlineData(0x0,0x83)]
        public void ADC_WithoutCarryShouldWork(uint A,uint value)
        {
            testComputer.MemoryReset();
            
            
            testComputer.mem.Write(0x0000,opcode);
            testComputer.mem.Write(0x0001,value);

            testComputer.CPUReset();
            testComputer.cpu.A = A;

            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles);

            Assert.Equal(A+value,testComputer.cpu.A);
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.Y,testComputer.cpu.Y);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            // Zero flag should be false
            Assert.Equal((uint) 0, testComputer.cpu.P & (uint) StatusFlagsMask.Z);
            // Overflow flag should be false
            Assert.Equal((uint) 0, testComputer.cpu.P & (uint) StatusFlagsMask.V);
            // Carry flag should be false
            Assert.Equal((uint) 0, testComputer.cpu.P & (uint) StatusFlagsMask.C);
            // Negative flag should be correct
            Assert.Equal((A+value) & (uint) StatusFlagsMask.N, testComputer.cpu.P & (uint) StatusFlagsMask.N);
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }

        [Theory]
        [InlineData(0x32,0xD0)]
        [InlineData(0xd0,0x32)]
        [InlineData(0xd0,0xd0)]
        public void ADC_ShouldSetCarry(uint A, uint value)
        {
            testComputer.MemoryReset();
            
            testComputer.mem.Write(0x0000,opcode);
            testComputer.mem.Write(0x0001,value);

            testComputer.CPUReset();
            testComputer.cpu.A = A;

            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles);

            Assert.Equal((A+value) & 0xFF,testComputer.cpu.A);
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.Y,testComputer.cpu.Y);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            // Zero flag should be false
            Assert.Equal((uint) 0, testComputer.cpu.P & (uint) StatusFlagsMask.Z);
            // Overflow flag should be false
            Assert.Equal((uint) 0, testComputer.cpu.P & (uint) StatusFlagsMask.V);
            // Carry flag should be true
            Assert.Equal((uint) StatusFlagsMask.C, testComputer.cpu.P & (uint) StatusFlagsMask.C);
            // Negative flag should be correct
            Assert.Equal((A+value) & (uint) StatusFlagsMask.N, testComputer.cpu.P & (uint) StatusFlagsMask.N);
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }

        [Theory]
        [InlineData(0x50,0x50)]
        public void ADC_OverflowWithoutCarryShouldWork(uint A, uint value)
        {
            testComputer.MemoryReset();
            
            testComputer.mem.Write(0x0000,opcode);
            testComputer.mem.Write(0x0001,value);

            testComputer.CPUReset();
            testComputer.cpu.A = A;

            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles);

            Assert.Equal((A+value) & 0xFF,testComputer.cpu.A);
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.Y,testComputer.cpu.Y);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            // Zero flag should be false
            Assert.Equal((uint) 0, testComputer.cpu.P & (uint) StatusFlagsMask.Z);
            // Overflow flag should betrue 
            Assert.Equal((uint) StatusFlagsMask.V, testComputer.cpu.P & (uint) StatusFlagsMask.V);
            // Carry flag should be false
            Assert.Equal((uint) 0, testComputer.cpu.P & (uint) StatusFlagsMask.C);
            // Negative flag should be correct
            Assert.Equal((A+value) & (uint) StatusFlagsMask.N, testComputer.cpu.P & (uint) StatusFlagsMask.N);
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }

        [Theory]
        [InlineData(0xd0,0x90)]
        public void ADC_OverflowWithCarryShouldWork(uint A, uint value)
        {
            testComputer.MemoryReset();
            
            testComputer.mem.Write(0x0000,opcode);
            testComputer.mem.Write(0x0001,value);

            testComputer.CPUReset();
            testComputer.cpu.A = A;

            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles);

            Assert.Equal((A+value) & 0xFF,testComputer.cpu.A);
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.Y,testComputer.cpu.Y);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            // Zero flag should be false
            Assert.Equal((uint) 0, testComputer.cpu.P & (uint) StatusFlagsMask.Z);
            // Overflow flag should betrue 
            Assert.Equal((uint) StatusFlagsMask.V, testComputer.cpu.P & (uint) StatusFlagsMask.V);
            // Carry flag should be true
            Assert.Equal((uint) StatusFlagsMask.C, testComputer.cpu.P & (uint) StatusFlagsMask.C);
            // Negative flag should be correct
            Assert.Equal((A+value) & (uint) StatusFlagsMask.N, testComputer.cpu.P & (uint) StatusFlagsMask.N);
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }


 
    }

    public class ADC_ZEROPAGE
    {

        private Computer testComputer = new Computer();
        private uint opcode = 0x65;
        private int cycles = 3;
        private int bytes = 2;


        [Theory]
        [InlineData(0x32,0x32)]
        [InlineData(0x0,0x32)]
        [InlineData(0x32,0x0)]
        [InlineData(0x32,0x83)]
        [InlineData(0x83,0x32)]
        [InlineData(0x83,0x0)]
        [InlineData(0x0,0x83)]
        public void ADC_WithoutCarryShouldWork(uint A,uint value)
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

            Assert.Equal(A+value,testComputer.cpu.A);
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.Y,testComputer.cpu.Y);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            // Zero flag should be false
            Assert.Equal((uint) 0, testComputer.cpu.P & (uint) StatusFlagsMask.Z);
            // Overflow flag should be false
            Assert.Equal((uint) 0, testComputer.cpu.P & (uint) StatusFlagsMask.V);
            // Carry flag should be false
            Assert.Equal((uint) 0, testComputer.cpu.P & (uint) StatusFlagsMask.C);
            // Negative flag should be correct
            Assert.Equal((A+value) & (uint) StatusFlagsMask.N, testComputer.cpu.P & (uint) StatusFlagsMask.N);
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }

        [Theory]
        [InlineData(0x32,0xD0)]
        [InlineData(0xd0,0x32)]
        [InlineData(0xd0,0xd0)]
        public void ADC_ShouldSetCarry(uint A, uint value)
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

            Assert.Equal((A+value) & 0xFF,testComputer.cpu.A);
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.Y,testComputer.cpu.Y);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            // Zero flag should be false
            Assert.Equal((uint) 0, testComputer.cpu.P & (uint) StatusFlagsMask.Z);
            // Overflow flag should be false
            Assert.Equal((uint) 0, testComputer.cpu.P & (uint) StatusFlagsMask.V);
            // Carry flag should be true
            Assert.Equal((uint) StatusFlagsMask.C, testComputer.cpu.P & (uint) StatusFlagsMask.C);
            // Negative flag should be correct
            Assert.Equal((A+value) & (uint) StatusFlagsMask.N, testComputer.cpu.P & (uint) StatusFlagsMask.N);
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }

        [Theory]
        [InlineData(0x50,0x50)]
        public void ADC_OverflowWithoutCarryShouldWork(uint A, uint value)
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

            Assert.Equal((A+value) & 0xFF,testComputer.cpu.A);
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.Y,testComputer.cpu.Y);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            // Zero flag should be false
            Assert.Equal((uint) 0, testComputer.cpu.P & (uint) StatusFlagsMask.Z);
            // Overflow flag should betrue 
            Assert.Equal((uint) StatusFlagsMask.V, testComputer.cpu.P & (uint) StatusFlagsMask.V);
            // Carry flag should be false
            Assert.Equal((uint) 0, testComputer.cpu.P & (uint) StatusFlagsMask.C);
            // Negative flag should be correct
            Assert.Equal((A+value) & (uint) StatusFlagsMask.N, testComputer.cpu.P & (uint) StatusFlagsMask.N);
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }

        [Theory]
        [InlineData(0xd0,0x90)]
        public void ADC_OverflowWithCarryShouldWork(uint A, uint value)
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

            Assert.Equal((A+value) & 0xFF,testComputer.cpu.A);
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.Y,testComputer.cpu.Y);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            // Zero flag should be false
            Assert.Equal((uint) 0, testComputer.cpu.P & (uint) StatusFlagsMask.Z);
            // Overflow flag should betrue 
            Assert.Equal((uint) StatusFlagsMask.V, testComputer.cpu.P & (uint) StatusFlagsMask.V);
            // Carry flag should be true
            Assert.Equal((uint) StatusFlagsMask.C, testComputer.cpu.P & (uint) StatusFlagsMask.C);
            // Negative flag should be correct
            Assert.Equal((A+value) & (uint) StatusFlagsMask.N, testComputer.cpu.P & (uint) StatusFlagsMask.N);
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }

    }

    public class ADC_ZEROPAGEX
    {

        private Computer testComputer = new Computer();
        private uint opcode = 0x75;
        private int cycles = 4;
        private int bytes = 2;


        [Theory]
        [InlineData(0x32,0x32)]
        [InlineData(0x0,0x32)]
        [InlineData(0x32,0x0)]
        [InlineData(0x32,0x83)]
        [InlineData(0x83,0x32)]
        [InlineData(0x83,0x0)]
        [InlineData(0x0,0x83)]
        public void ADC_WithoutCarryShouldWork(uint A,uint value)
        {
            testComputer.MemoryReset();
            
            
            uint addr = 0xAA;
            uint X = 0x12;
            testComputer.mem.Write(0x0000,opcode);
            testComputer.mem.Write(0x0001,addr);
            testComputer.mem.Write(addr+X,value);

            testComputer.CPUReset();
            testComputer.cpu.X = X;
            testComputer.cpu.A = A;

            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles);

            Assert.Equal(A+value,testComputer.cpu.A);
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.Y,testComputer.cpu.Y);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            // Zero flag should be false
            Assert.Equal((uint) 0, testComputer.cpu.P & (uint) StatusFlagsMask.Z);
            // Overflow flag should be false
            Assert.Equal((uint) 0, testComputer.cpu.P & (uint) StatusFlagsMask.V);
            // Carry flag should be false
            Assert.Equal((uint) 0, testComputer.cpu.P & (uint) StatusFlagsMask.C);
            // Negative flag should be correct
            Assert.Equal((A+value) & (uint) StatusFlagsMask.N, testComputer.cpu.P & (uint) StatusFlagsMask.N);
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }

        [Theory]
        [InlineData(0x32,0xD0)]
        [InlineData(0xd0,0x32)]
        [InlineData(0xd0,0xd0)]
        public void ADC_ShouldSetCarry(uint A, uint value)
        {
            testComputer.MemoryReset();
            
            uint addr = 0xAA;
            uint X = 0x0A;
            testComputer.mem.Write(0x0000,opcode);
            testComputer.mem.Write(0x0001,addr);
            testComputer.mem.Write(addr+X,value);

            testComputer.CPUReset();
            testComputer.cpu.A = A;
            testComputer.cpu.X = X;

            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles);

            Assert.Equal((A+value) & 0xFF,testComputer.cpu.A);
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.Y,testComputer.cpu.Y);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            // Zero flag should be false
            Assert.Equal((uint) 0, testComputer.cpu.P & (uint) StatusFlagsMask.Z);
            // Overflow flag should be false
            Assert.Equal((uint) 0, testComputer.cpu.P & (uint) StatusFlagsMask.V);
            // Carry flag should be true
            Assert.Equal((uint) StatusFlagsMask.C, testComputer.cpu.P & (uint) StatusFlagsMask.C);
            // Negative flag should be correct
            Assert.Equal((A+value) & (uint) StatusFlagsMask.N, testComputer.cpu.P & (uint) StatusFlagsMask.N);
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }

        [Theory]
        [InlineData(0x50,0x50)]
        public void ADC_OverflowWithoutCarryShouldWork(uint A, uint value)
        {
            testComputer.MemoryReset();
            
            uint addr = 0xAA;
            uint X = 0x0A;
            testComputer.mem.Write(0x0000,opcode);
            testComputer.mem.Write(0x0001,addr);
            testComputer.mem.Write(addr+X,value);

            testComputer.CPUReset();
            testComputer.cpu.A = A;
            testComputer.cpu.X = X;

            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles);

            Assert.Equal((A+value) & 0xFF,testComputer.cpu.A);
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.Y,testComputer.cpu.Y);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            // Zero flag should be false
            Assert.Equal((uint) 0, testComputer.cpu.P & (uint) StatusFlagsMask.Z);
            // Overflow flag should betrue 
            Assert.Equal((uint) StatusFlagsMask.V, testComputer.cpu.P & (uint) StatusFlagsMask.V);
            // Carry flag should be false
            Assert.Equal((uint) 0, testComputer.cpu.P & (uint) StatusFlagsMask.C);
            // Negative flag should be correct
            Assert.Equal((A+value) & (uint) StatusFlagsMask.N, testComputer.cpu.P & (uint) StatusFlagsMask.N);
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }

        [Theory]
        [InlineData(0xd0,0x90)]
        public void ADC_OverflowWithCarryShouldWork(uint A, uint value)
        {
            testComputer.MemoryReset();
            
            uint addr = 0xAA;
            uint X = 0x0A;
            testComputer.mem.Write(0x0000,opcode);
            testComputer.mem.Write(0x0001,addr);
            testComputer.mem.Write(addr+X,value);

            testComputer.CPUReset();
            testComputer.cpu.A = A;
            testComputer.cpu.X = X;

            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles);

            Assert.Equal((A+value) & 0xFF,testComputer.cpu.A);
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.Y,testComputer.cpu.Y);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            // Zero flag should be false
            Assert.Equal((uint) 0, testComputer.cpu.P & (uint) StatusFlagsMask.Z);
            // Overflow flag should betrue 
            Assert.Equal((uint) StatusFlagsMask.V, testComputer.cpu.P & (uint) StatusFlagsMask.V);
            // Carry flag should be true
            Assert.Equal((uint) StatusFlagsMask.C, testComputer.cpu.P & (uint) StatusFlagsMask.C);
            // Negative flag should be correct
            Assert.Equal((A+value) & (uint) StatusFlagsMask.N, testComputer.cpu.P & (uint) StatusFlagsMask.N);
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }


        [Fact]
        public void ADC_IndexWrapShouldWork()
        {
            testComputer.MemoryReset();
            
            // x = A
            // LDA #A0,X
            uint X = 0x32;
            uint A = 0x32;
            uint value = 0x32;
            uint addr = 0xAA;

            testComputer.mem.Write(0x0000,opcode);
            testComputer.mem.Write(0x0001,addr);
            testComputer.mem.Write( (addr+X) & 0xFF,value);

            testComputer.CPUReset();
            testComputer.cpu.X = X;
            testComputer.cpu.A = A;


            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles);

            Assert.Equal(A+value,testComputer.cpu.A);
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }

    }

    public class ADC_ABSOLUTE
    {

        private Computer testComputer = new Computer();
        private uint opcode = 0x6D;
        private int cycles = 4;
        private int bytes = 3;


        [Theory]
        [InlineData(0x32,0x32)]
        [InlineData(0x0,0x32)]
        [InlineData(0x32,0x0)]
        [InlineData(0x32,0x83)]
        [InlineData(0x83,0x32)]
        [InlineData(0x83,0x0)]
        [InlineData(0x0,0x83)]
        public void ADC_WithoutCarryShouldWork(uint A,uint value)
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

            Assert.Equal(A+value,testComputer.cpu.A);
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.Y,testComputer.cpu.Y);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            // Zero flag should be false
            Assert.Equal((uint) 0, testComputer.cpu.P & (uint) StatusFlagsMask.Z);
            // Overflow flag should be false
            Assert.Equal((uint) 0, testComputer.cpu.P & (uint) StatusFlagsMask.V);
            // Carry flag should be false
            Assert.Equal((uint) 0, testComputer.cpu.P & (uint) StatusFlagsMask.C);
            // Negative flag should be correct
            Assert.Equal((A+value) & (uint) StatusFlagsMask.N, testComputer.cpu.P & (uint) StatusFlagsMask.N);
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }

        [Theory]
        [InlineData(0x32,0xD0)]
        [InlineData(0xd0,0x32)]
        [InlineData(0xd0,0xd0)]
        public void ADC_ShouldSetCarry(uint A, uint value)
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

            Assert.Equal((A+value) & 0xFF,testComputer.cpu.A);
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.Y,testComputer.cpu.Y);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            // Zero flag should be false
            Assert.Equal((uint) 0, testComputer.cpu.P & (uint) StatusFlagsMask.Z);
            // Overflow flag should be false
            Assert.Equal((uint) 0, testComputer.cpu.P & (uint) StatusFlagsMask.V);
            // Carry flag should be true
            Assert.Equal((uint) StatusFlagsMask.C, testComputer.cpu.P & (uint) StatusFlagsMask.C);
            // Negative flag should be correct
            Assert.Equal((A+value) & (uint) StatusFlagsMask.N, testComputer.cpu.P & (uint) StatusFlagsMask.N);
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }

        [Theory]
        [InlineData(0x50,0x50)]
        public void ADC_OverflowWithoutCarryShouldWork(uint A, uint value)
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

            Assert.Equal((A+value) & 0xFF,testComputer.cpu.A);
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.Y,testComputer.cpu.Y);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            // Zero flag should be false
            Assert.Equal((uint) 0, testComputer.cpu.P & (uint) StatusFlagsMask.Z);
            // Overflow flag should betrue 
            Assert.Equal((uint) StatusFlagsMask.V, testComputer.cpu.P & (uint) StatusFlagsMask.V);
            // Carry flag should be false
            Assert.Equal((uint) 0, testComputer.cpu.P & (uint) StatusFlagsMask.C);
            // Negative flag should be correct
            Assert.Equal((A+value) & (uint) StatusFlagsMask.N, testComputer.cpu.P & (uint) StatusFlagsMask.N);
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }

        [Theory]
        [InlineData(0xd0,0x90)]
        public void ADC_OverflowWithCarryShouldWork(uint A, uint value)
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

            Assert.Equal((A+value) & 0xFF,testComputer.cpu.A);
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.Y,testComputer.cpu.Y);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            // Zero flag should be false
            Assert.Equal((uint) 0, testComputer.cpu.P & (uint) StatusFlagsMask.Z);
            // Overflow flag should betrue 
            Assert.Equal((uint) StatusFlagsMask.V, testComputer.cpu.P & (uint) StatusFlagsMask.V);
            // Carry flag should be true
            Assert.Equal((uint) StatusFlagsMask.C, testComputer.cpu.P & (uint) StatusFlagsMask.C);
            // Negative flag should be correct
            Assert.Equal((A+value) & (uint) StatusFlagsMask.N, testComputer.cpu.P & (uint) StatusFlagsMask.N);
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }

    }

    public class ADC_ABSOLUTEX
    {

        private Computer testComputer = new Computer();
        private uint opcode = 0x7D;
        private int cycles = 4;
        private int bytes = 3;


        [Theory]
        [InlineData(0x32,0x32)]
        [InlineData(0x0,0x32)]
        [InlineData(0x32,0x0)]
        [InlineData(0x32,0x83)]
        [InlineData(0x83,0x32)]
        [InlineData(0x83,0x0)]
        [InlineData(0x0,0x83)]
        public void ADC_WithoutCarryShouldWork(uint A,uint value)
        {
            testComputer.MemoryReset();
            
            
            uint addr = 0xBEEF;
            uint X = 0x0F;

            testComputer.mem.Write(0x0000,opcode);
            testComputer.mem.Write(0x0001,addr & 0x00FF);
            testComputer.mem.Write(0x0002,addr >> 8);       
            testComputer.mem.Write(addr+X,value);

            testComputer.CPUReset();
            testComputer.cpu.A = A;
            testComputer.cpu.X = X;

            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles);

            Assert.Equal(A+value,testComputer.cpu.A);
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.Y,testComputer.cpu.Y);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            // Zero flag should be false
            Assert.Equal((uint) 0, testComputer.cpu.P & (uint) StatusFlagsMask.Z);
            // Overflow flag should be false
            Assert.Equal((uint) 0, testComputer.cpu.P & (uint) StatusFlagsMask.V);
            // Carry flag should be false
            Assert.Equal((uint) 0, testComputer.cpu.P & (uint) StatusFlagsMask.C);
            // Negative flag should be correct
            Assert.Equal((A+value) & (uint) StatusFlagsMask.N, testComputer.cpu.P & (uint) StatusFlagsMask.N);
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }

        [Theory]
        [InlineData(0x32,0xD0)]
        [InlineData(0xd0,0x32)]
        [InlineData(0xd0,0xd0)]
        public void ADC_ShouldSetCarry(uint A, uint value)
        {
            testComputer.MemoryReset();
            
            uint addr = 0xBEEF;
            uint X = 0x0F;

            testComputer.mem.Write(0x0000,opcode);
            testComputer.mem.Write(0x0001,addr & 0x00FF);
            testComputer.mem.Write(0x0002,addr >> 8);       
            testComputer.mem.Write(addr+X,value);

            testComputer.CPUReset();
            testComputer.cpu.A = A;
            testComputer.cpu.X = X;

            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles);

            Assert.Equal((A+value) & 0xFF,testComputer.cpu.A);
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.Y,testComputer.cpu.Y);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            // Zero flag should be false
            Assert.Equal((uint) 0, testComputer.cpu.P & (uint) StatusFlagsMask.Z);
            // Overflow flag should be false
            Assert.Equal((uint) 0, testComputer.cpu.P & (uint) StatusFlagsMask.V);
            // Carry flag should be true
            Assert.Equal((uint) StatusFlagsMask.C, testComputer.cpu.P & (uint) StatusFlagsMask.C);
            // Negative flag should be correct
            Assert.Equal((A+value) & (uint) StatusFlagsMask.N, testComputer.cpu.P & (uint) StatusFlagsMask.N);
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }

        [Theory]
        [InlineData(0x50,0x50)]
        public void ADC_OverflowWithoutCarryShouldWork(uint A, uint value)
        {
            testComputer.MemoryReset();
            
            uint addr = 0xBEEF;
            uint X = 0x0F;

            testComputer.mem.Write(0x0000,opcode);
            testComputer.mem.Write(0x0001,addr & 0x00FF);
            testComputer.mem.Write(0x0002,addr >> 8);       
            testComputer.mem.Write(addr+X,value);

            testComputer.CPUReset();
            testComputer.cpu.A = A;
            testComputer.cpu.X = X;

            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles);

            Assert.Equal((A+value) & 0xFF,testComputer.cpu.A);
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.Y,testComputer.cpu.Y);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            // Zero flag should be false
            Assert.Equal((uint) 0, testComputer.cpu.P & (uint) StatusFlagsMask.Z);
            // Overflow flag should betrue 
            Assert.Equal((uint) StatusFlagsMask.V, testComputer.cpu.P & (uint) StatusFlagsMask.V);
            // Carry flag should be false
            Assert.Equal((uint) 0, testComputer.cpu.P & (uint) StatusFlagsMask.C);
            // Negative flag should be correct
            Assert.Equal((A+value) & (uint) StatusFlagsMask.N, testComputer.cpu.P & (uint) StatusFlagsMask.N);
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }

        [Theory]
        [InlineData(0xd0,0x90)]
        public void ADC_OverflowWithCarryShouldWork(uint A, uint value)
        {
            testComputer.MemoryReset();
            
            uint addr = 0xBEEF;
            uint X = 0x0F;

            testComputer.mem.Write(0x0000,opcode);
            testComputer.mem.Write(0x0001,addr & 0x00FF);
            testComputer.mem.Write(0x0002,addr >> 8);       
            testComputer.mem.Write(addr+X,value);

            testComputer.CPUReset();
            testComputer.cpu.A = A;
            testComputer.cpu.X = X;

            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles);

            Assert.Equal((A+value) & 0xFF,testComputer.cpu.A);
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.Y,testComputer.cpu.Y);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            // Zero flag should be false
            Assert.Equal((uint) 0, testComputer.cpu.P & (uint) StatusFlagsMask.Z);
            // Overflow flag should betrue 
            Assert.Equal((uint) StatusFlagsMask.V, testComputer.cpu.P & (uint) StatusFlagsMask.V);
            // Carry flag should be true
            Assert.Equal((uint) StatusFlagsMask.C, testComputer.cpu.P & (uint) StatusFlagsMask.C);
            // Negative flag should be correct
            Assert.Equal((A+value) & (uint) StatusFlagsMask.N, testComputer.cpu.P & (uint) StatusFlagsMask.N);
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }
        [Theory]
        [InlineData(0x32,0xD0)]
        [InlineData(0xd0,0x32)]
        [InlineData(0xd0,0xd0)]
        public void ADC_CrossPageBoundaryShouldAddACycle(uint A, uint value)
        {
            testComputer.MemoryReset();
            
            uint addr = 0xBEEF;
            uint X = 0xFF;

            testComputer.mem.Write(0x0000,opcode);
            testComputer.mem.Write(0x0001,addr & 0x00FF);
            testComputer.mem.Write(0x0002,addr >> 8);       
            testComputer.mem.Write(addr+X,value);

            testComputer.CPUReset();
            testComputer.cpu.A = A;
            testComputer.cpu.X = X;

            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles+1);

            Assert.Equal((A+value) & 0xFF,testComputer.cpu.A);
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.Y,testComputer.cpu.Y);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            // Zero flag should be false
            Assert.Equal((uint) 0, testComputer.cpu.P & (uint) StatusFlagsMask.Z);
            // Overflow flag should be false
            Assert.Equal((uint) 0, testComputer.cpu.P & (uint) StatusFlagsMask.V);
            // Carry flag should be true
            Assert.Equal((uint) StatusFlagsMask.C, testComputer.cpu.P & (uint) StatusFlagsMask.C);
            // Negative flag should be correct
            Assert.Equal((A+value) & (uint) StatusFlagsMask.N, testComputer.cpu.P & (uint) StatusFlagsMask.N);
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }

 
    }

    public class ADC_ABSOLUTEY
    {

        private Computer testComputer = new Computer();
        private uint opcode = 0x79;
        private int cycles = 4;
        private int bytes = 3;


        [Theory]
        [InlineData(0x32,0x32)]
        [InlineData(0x0,0x32)]
        [InlineData(0x32,0x0)]
        [InlineData(0x32,0x83)]
        [InlineData(0x83,0x32)]
        [InlineData(0x83,0x0)]
        [InlineData(0x0,0x83)]
        public void ADC_WithoutCarryShouldWork(uint A,uint value)
        {
            testComputer.MemoryReset();
            
            
            uint addr = 0xBEEF;
            uint Y = 0x0F;

            testComputer.mem.Write(0x0000,opcode);
            testComputer.mem.Write(0x0001,addr & 0x00FF);
            testComputer.mem.Write(0x0002,addr >> 8);       
            testComputer.mem.Write(addr+Y,value);

            testComputer.CPUReset();
            testComputer.cpu.A = A;
            testComputer.cpu.Y = Y;

            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles);

            Assert.Equal(A+value,testComputer.cpu.A);
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.Y,testComputer.cpu.Y);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            // Zero flag should be false
            Assert.Equal((uint) 0, testComputer.cpu.P & (uint) StatusFlagsMask.Z);
            // Overflow flag should be false
            Assert.Equal((uint) 0, testComputer.cpu.P & (uint) StatusFlagsMask.V);
            // Carry flag should be false
            Assert.Equal((uint) 0, testComputer.cpu.P & (uint) StatusFlagsMask.C);
            // Negative flag should be correct
            Assert.Equal((A+value) & (uint) StatusFlagsMask.N, testComputer.cpu.P & (uint) StatusFlagsMask.N);
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }

        [Theory]
        [InlineData(0x32,0xD0)]
        [InlineData(0xd0,0x32)]
        [InlineData(0xd0,0xd0)]
        public void ADC_ShouldSetCarry(uint A, uint value)
        {
            testComputer.MemoryReset();
            
            uint addr = 0xBEEF;
            uint Y = 0x0F;

            testComputer.mem.Write(0x0000,opcode);
            testComputer.mem.Write(0x0001,addr & 0x00FF);
            testComputer.mem.Write(0x0002,addr >> 8);       
            testComputer.mem.Write(addr+Y,value);

            testComputer.CPUReset();
            testComputer.cpu.A = A;
            testComputer.cpu.Y = Y;

            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles);

            Assert.Equal((A+value) & 0xFF,testComputer.cpu.A);
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.Y,testComputer.cpu.Y);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            // Zero flag should be false
            Assert.Equal((uint) 0, testComputer.cpu.P & (uint) StatusFlagsMask.Z);
            // Overflow flag should be false
            Assert.Equal((uint) 0, testComputer.cpu.P & (uint) StatusFlagsMask.V);
            // Carry flag should be true
            Assert.Equal((uint) StatusFlagsMask.C, testComputer.cpu.P & (uint) StatusFlagsMask.C);
            // Negative flag should be correct
            Assert.Equal((A+value) & (uint) StatusFlagsMask.N, testComputer.cpu.P & (uint) StatusFlagsMask.N);
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }

        [Theory]
        [InlineData(0x50,0x50)]
        public void ADC_OverflowWithoutCarryShouldWork(uint A, uint value)
        {
            testComputer.MemoryReset();
            
            uint addr = 0xBEEF;
            uint Y = 0x0F;

            testComputer.mem.Write(0x0000,opcode);
            testComputer.mem.Write(0x0001,addr & 0x00FF);
            testComputer.mem.Write(0x0002,addr >> 8);       
            testComputer.mem.Write(addr+Y,value);

            testComputer.CPUReset();
            testComputer.cpu.A = A;
            testComputer.cpu.Y = Y;

            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles);

            Assert.Equal((A+value) & 0xFF,testComputer.cpu.A);
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.Y,testComputer.cpu.Y);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            // Zero flag should be false
            Assert.Equal((uint) 0, testComputer.cpu.P & (uint) StatusFlagsMask.Z);
            // Overflow flag should betrue 
            Assert.Equal((uint) StatusFlagsMask.V, testComputer.cpu.P & (uint) StatusFlagsMask.V);
            // Carry flag should be false
            Assert.Equal((uint) 0, testComputer.cpu.P & (uint) StatusFlagsMask.C);
            // Negative flag should be correct
            Assert.Equal((A+value) & (uint) StatusFlagsMask.N, testComputer.cpu.P & (uint) StatusFlagsMask.N);
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }

        [Theory]
        [InlineData(0xd0,0x90)]
        public void ADC_OverflowWithCarryShouldWork(uint A, uint value)
        {
            testComputer.MemoryReset();
            
            uint addr = 0xBEEF;
            uint Y = 0x0F;

            testComputer.mem.Write(0x0000,opcode);
            testComputer.mem.Write(0x0001,addr & 0x00FF);
            testComputer.mem.Write(0x0002,addr >> 8);       
            testComputer.mem.Write(addr+Y,value);

            testComputer.CPUReset();
            testComputer.cpu.A = A;
            testComputer.cpu.Y = Y;

            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles);

            Assert.Equal((A+value) & 0xFF,testComputer.cpu.A);
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.Y,testComputer.cpu.Y);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            // Zero flag should be false
            Assert.Equal((uint) 0, testComputer.cpu.P & (uint) StatusFlagsMask.Z);
            // Overflow flag should betrue 
            Assert.Equal((uint) StatusFlagsMask.V, testComputer.cpu.P & (uint) StatusFlagsMask.V);
            // Carry flag should be true
            Assert.Equal((uint) StatusFlagsMask.C, testComputer.cpu.P & (uint) StatusFlagsMask.C);
            // Negative flag should be correct
            Assert.Equal((A+value) & (uint) StatusFlagsMask.N, testComputer.cpu.P & (uint) StatusFlagsMask.N);
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }
        [Theory]
        [InlineData(0x32,0xD0)]
        [InlineData(0xd0,0x32)]
        [InlineData(0xd0,0xd0)]
        public void ADC_CrossPageBoundaryShouldAddACycle(uint A, uint value)
        {
            testComputer.MemoryReset();
            
            uint addr = 0xBEEF;
            uint Y = 0xFF;

            testComputer.mem.Write(0x0000,opcode);
            testComputer.mem.Write(0x0001,addr & 0x00FF);
            testComputer.mem.Write(0x0002,addr >> 8);       
            testComputer.mem.Write(addr+Y,value);

            testComputer.CPUReset();
            testComputer.cpu.A = A;
            testComputer.cpu.Y = Y;

            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles+1);

            Assert.Equal((A+value) & 0xFF,testComputer.cpu.A);
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.Y,testComputer.cpu.Y);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            // Zero flag should be false
            Assert.Equal((uint) 0, testComputer.cpu.P & (uint) StatusFlagsMask.Z);
            // Overflow flag should be false
            Assert.Equal((uint) 0, testComputer.cpu.P & (uint) StatusFlagsMask.V);
            // Carry flag should be true
            Assert.Equal((uint) StatusFlagsMask.C, testComputer.cpu.P & (uint) StatusFlagsMask.C);
            // Negative flag should be correct
            Assert.Equal((A+value) & (uint) StatusFlagsMask.N, testComputer.cpu.P & (uint) StatusFlagsMask.N);
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }

 
    }

    public class ADC_INDEXEDINDIRECT
    {

        private Computer testComputer = new Computer();
        private uint opcode = 0x61;
        private int cycles = 6;
        private int bytes = 2;


        [Theory]
        [InlineData(0x32,0x32)]
        [InlineData(0x0,0x32)]
        [InlineData(0x32,0x0)]
        [InlineData(0x32,0x83)]
        [InlineData(0x83,0x32)]
        [InlineData(0x83,0x0)]
        [InlineData(0x0,0x83)]
        public void ADC_WithoutCarryShouldWork(uint A,uint value)
        {
            testComputer.MemoryReset();
            
            uint pointer = 0x40;
            uint X = 0x0F;
            uint addr = 0xBEE0;

            testComputer.mem.Write(0x0000,opcode);
            testComputer.mem.Write(0x0001,pointer);

            testComputer.mem.Write((pointer+X) & 0xFF,addr & 0x00FF);
            testComputer.mem.Write((pointer+X+1) & 0xFF,addr >> 8);       
            testComputer.mem.Write(addr,value);

            testComputer.CPUReset();
            testComputer.cpu.A = A;
            testComputer.cpu.X = X;

            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles);

            Assert.Equal(A+value,testComputer.cpu.A);
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.Y,testComputer.cpu.Y);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            // Zero flag should be false
            Assert.Equal((uint) 0, testComputer.cpu.P & (uint) StatusFlagsMask.Z);
            // Overflow flag should be false
            Assert.Equal((uint) 0, testComputer.cpu.P & (uint) StatusFlagsMask.V);
            // Carry flag should be false
            Assert.Equal((uint) 0, testComputer.cpu.P & (uint) StatusFlagsMask.C);
            // Negative flag should be correct
            Assert.Equal((A+value) & (uint) StatusFlagsMask.N, testComputer.cpu.P & (uint) StatusFlagsMask.N);
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }

        [Theory]
        [InlineData(0x32,0xD0)]
        [InlineData(0xd0,0x32)]
        [InlineData(0xd0,0xd0)]
        public void ADC_ShouldSetCarry(uint A, uint value)
        {
            testComputer.MemoryReset();

            uint pointer = 0x40;
            uint X = 0x0F;
            uint addr = 0xBEE0;

            testComputer.mem.Write(0x0000,opcode);
            testComputer.mem.Write(0x0001,pointer);

            testComputer.mem.Write((pointer+X) & 0xFF,addr & 0x00FF);
            testComputer.mem.Write((pointer+X+1) & 0xFF,addr >> 8);       
            testComputer.mem.Write(addr,value);

            testComputer.CPUReset();
            testComputer.cpu.A = A;
            testComputer.cpu.X = X;

            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles);

            Assert.Equal((A+value) & 0xFF,testComputer.cpu.A);
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.Y,testComputer.cpu.Y);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            // Zero flag should be false
            Assert.Equal((uint) 0, testComputer.cpu.P & (uint) StatusFlagsMask.Z);
            // Overflow flag should be false
            Assert.Equal((uint) 0, testComputer.cpu.P & (uint) StatusFlagsMask.V);
            // Carry flag should be true
            Assert.Equal((uint) StatusFlagsMask.C, testComputer.cpu.P & (uint) StatusFlagsMask.C);
            // Negative flag should be correct
            Assert.Equal((A+value) & (uint) StatusFlagsMask.N, testComputer.cpu.P & (uint) StatusFlagsMask.N);
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }

        [Theory]
        [InlineData(0x50,0x50)]
        public void ADC_OverflowWithoutCarryShouldWork(uint A, uint value)
        {
            testComputer.MemoryReset();

            uint pointer = 0x40;
            uint X = 0x0F;
            uint addr = 0xBEE0;

            testComputer.mem.Write(0x0000,opcode);
            testComputer.mem.Write(0x0001,pointer);

            testComputer.mem.Write((pointer+X) & 0xFF,addr & 0x00FF);
            testComputer.mem.Write((pointer+X+1) & 0xFF,addr >> 8);       
            testComputer.mem.Write(addr,value);

            testComputer.CPUReset();
            testComputer.cpu.A = A;
            testComputer.cpu.X = X;

            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles);

            Assert.Equal((A+value) & 0xFF,testComputer.cpu.A);
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.Y,testComputer.cpu.Y);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            // Zero flag should be false
            Assert.Equal((uint) 0, testComputer.cpu.P & (uint) StatusFlagsMask.Z);
            // Overflow flag should betrue 
            Assert.Equal((uint) StatusFlagsMask.V, testComputer.cpu.P & (uint) StatusFlagsMask.V);
            // Carry flag should be false
            Assert.Equal((uint) 0, testComputer.cpu.P & (uint) StatusFlagsMask.C);
            // Negative flag should be correct
            Assert.Equal((A+value) & (uint) StatusFlagsMask.N, testComputer.cpu.P & (uint) StatusFlagsMask.N);
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }

        [Theory]
        [InlineData(0xd0,0x90)]
        public void ADC_OverflowWithCarryShouldWork(uint A, uint value)
        {
            testComputer.MemoryReset();

            uint pointer = 0x40;
            uint X = 0x0F;
            uint addr = 0xBEE0;

            testComputer.mem.Write(0x0000,opcode);
            testComputer.mem.Write(0x0001,pointer);

            testComputer.mem.Write((pointer+X) & 0xFF,addr & 0x00FF);
            testComputer.mem.Write((pointer+X+1) & 0xFF,addr >> 8);       
            testComputer.mem.Write(addr,value);

            testComputer.CPUReset();
            testComputer.cpu.A = A;
            testComputer.cpu.X = X;

            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles);

            Assert.Equal((A+value) & 0xFF,testComputer.cpu.A);
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.Y,testComputer.cpu.Y);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            // Zero flag should be false
            Assert.Equal((uint) 0, testComputer.cpu.P & (uint) StatusFlagsMask.Z);
            // Overflow flag should betrue 
            Assert.Equal((uint) StatusFlagsMask.V, testComputer.cpu.P & (uint) StatusFlagsMask.V);
            // Carry flag should be true
            Assert.Equal((uint) StatusFlagsMask.C, testComputer.cpu.P & (uint) StatusFlagsMask.C);
            // Negative flag should be correct
            Assert.Equal((A+value) & (uint) StatusFlagsMask.N, testComputer.cpu.P & (uint) StatusFlagsMask.N);
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }

        [Theory]
        [InlineData(0x32,0xD0)]
        [InlineData(0xd0,0x32)]
        [InlineData(0xd0,0xd0)]
        public void ADC_CrossPageBoundaryShouldWork(uint A, uint value)
        {
            testComputer.MemoryReset();

            uint pointer = 0x40;
            uint X = 0xFF;
            uint addr = 0xBEE0;

            testComputer.mem.Write(0x0000,opcode);
            testComputer.mem.Write(0x0001,pointer);

            testComputer.mem.Write((pointer+X) & 0xFF,addr & 0x00FF);
            testComputer.mem.Write((pointer+X+1) & 0xFF,addr >> 8);       
            testComputer.mem.Write(addr,value);

            testComputer.CPUReset();
            testComputer.cpu.A = A;
            testComputer.cpu.X = X;

            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles);

            Assert.Equal((A+value) & 0xFF,testComputer.cpu.A);
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.Y,testComputer.cpu.Y);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            // Zero flag should be false
            Assert.Equal((uint) 0, testComputer.cpu.P & (uint) StatusFlagsMask.Z);
            // Overflow flag should be false
            Assert.Equal((uint) 0, testComputer.cpu.P & (uint) StatusFlagsMask.V);
            // Carry flag should be true
            Assert.Equal((uint) StatusFlagsMask.C, testComputer.cpu.P & (uint) StatusFlagsMask.C);
            // Negative flag should be correct
            Assert.Equal((A+value) & (uint) StatusFlagsMask.N, testComputer.cpu.P & (uint) StatusFlagsMask.N);
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }


    }

    public class ADC_INDIRECTINDEXED
    {

        private Computer testComputer = new Computer();
        private uint opcode = 0x71;
        private int cycles = 5;
        private int bytes = 2;


        [Theory]
        [InlineData(0x32,0x32)]
        [InlineData(0x0,0x32)]
        [InlineData(0x32,0x0)]
        [InlineData(0x32,0x83)]
        [InlineData(0x83,0x32)]
        [InlineData(0x83,0x0)]
        [InlineData(0x0,0x83)]
        public void ADC_WithoutCarryShouldWork(uint A,uint value)
        {
            testComputer.MemoryReset();
            
            uint pointer = 0x40;
            uint Y = 0x0F;
            uint addr = 0xBEE0;

            testComputer.mem.Write(0x0000,opcode);
            testComputer.mem.Write(0x0001,pointer);

            testComputer.mem.Write( pointer ,addr & 0x00FF);
            testComputer.mem.Write( pointer+1,addr >> 8);       
            testComputer.mem.Write(addr+Y,value);

            testComputer.CPUReset();
            testComputer.cpu.A = A;
            testComputer.cpu.Y = Y;

            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles);

            Assert.Equal(A+value,testComputer.cpu.A);
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.Y,testComputer.cpu.Y);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            // Zero flag should be false
            Assert.Equal((uint) 0, testComputer.cpu.P & (uint) StatusFlagsMask.Z);
            // Overflow flag should be false
            Assert.Equal((uint) 0, testComputer.cpu.P & (uint) StatusFlagsMask.V);
            // Carry flag should be false
            Assert.Equal((uint) 0, testComputer.cpu.P & (uint) StatusFlagsMask.C);
            // Negative flag should be correct
            Assert.Equal((A+value) & (uint) StatusFlagsMask.N, testComputer.cpu.P & (uint) StatusFlagsMask.N);
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }

        [Theory]
        [InlineData(0x32,0xD0)]
        [InlineData(0xd0,0x32)]
        [InlineData(0xd0,0xd0)]
        public void ADC_ShouldSetCarry(uint A, uint value)
        {
            testComputer.MemoryReset();

            uint pointer = 0x40;
            uint Y = 0x0F;
            uint addr = 0xBEE0;

            testComputer.mem.Write(0x0000,opcode);
            testComputer.mem.Write(0x0001,pointer);

            testComputer.mem.Write( pointer ,addr & 0x00FF);
            testComputer.mem.Write( pointer+1,addr >> 8);       
            testComputer.mem.Write(addr+Y,value);

            testComputer.CPUReset();
            testComputer.cpu.A = A;
            testComputer.cpu.Y = Y;

            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles);

            Assert.Equal((A+value) & 0xFF,testComputer.cpu.A);
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.Y,testComputer.cpu.Y);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            // Zero flag should be false
            Assert.Equal((uint) 0, testComputer.cpu.P & (uint) StatusFlagsMask.Z);
            // Overflow flag should be false
            Assert.Equal((uint) 0, testComputer.cpu.P & (uint) StatusFlagsMask.V);
            // Carry flag should be true
            Assert.Equal((uint) StatusFlagsMask.C, testComputer.cpu.P & (uint) StatusFlagsMask.C);
            // Negative flag should be correct
            Assert.Equal((A+value) & (uint) StatusFlagsMask.N, testComputer.cpu.P & (uint) StatusFlagsMask.N);
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }

        [Theory]
        [InlineData(0x50,0x50)]
        public void ADC_OverflowWithoutCarryShouldWork(uint A, uint value)
        {
            testComputer.MemoryReset();

            uint pointer = 0x40;
            uint Y = 0x0F;
            uint addr = 0xBEE0;

            testComputer.mem.Write(0x0000,opcode);
            testComputer.mem.Write(0x0001,pointer);

            testComputer.mem.Write( pointer ,addr & 0x00FF);
            testComputer.mem.Write( pointer+1,addr >> 8);       
            testComputer.mem.Write(addr+Y,value);

            testComputer.CPUReset();
            testComputer.cpu.A = A;
            testComputer.cpu.Y = Y;

            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles);

            Assert.Equal((A+value) & 0xFF,testComputer.cpu.A);
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.Y,testComputer.cpu.Y);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            // Zero flag should be false
            Assert.Equal((uint) 0, testComputer.cpu.P & (uint) StatusFlagsMask.Z);
            // Overflow flag should betrue 
            Assert.Equal((uint) StatusFlagsMask.V, testComputer.cpu.P & (uint) StatusFlagsMask.V);
            // Carry flag should be false
            Assert.Equal((uint) 0, testComputer.cpu.P & (uint) StatusFlagsMask.C);
            // Negative flag should be correct
            Assert.Equal((A+value) & (uint) StatusFlagsMask.N, testComputer.cpu.P & (uint) StatusFlagsMask.N);
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }

        [Theory]
        [InlineData(0xd0,0x90)]
        public void ADC_OverflowWithCarryShouldWork(uint A, uint value)
        {
            testComputer.MemoryReset();

            uint pointer = 0x40;
            uint Y = 0x0F;
            uint addr = 0xBEE0;

            testComputer.mem.Write(0x0000,opcode);
            testComputer.mem.Write(0x0001,pointer);

            testComputer.mem.Write( pointer ,addr & 0x00FF);
            testComputer.mem.Write( pointer+1,addr >> 8);       
            testComputer.mem.Write(addr+Y,value);

            testComputer.CPUReset();
            testComputer.cpu.A = A;
            testComputer.cpu.Y = Y;

            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles);

            Assert.Equal((A+value) & 0xFF,testComputer.cpu.A);
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.Y,testComputer.cpu.Y);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            // Zero flag should be false
            Assert.Equal((uint) 0, testComputer.cpu.P & (uint) StatusFlagsMask.Z);
            // Overflow flag should betrue 
            Assert.Equal((uint) StatusFlagsMask.V, testComputer.cpu.P & (uint) StatusFlagsMask.V);
            // Carry flag should be true
            Assert.Equal((uint) StatusFlagsMask.C, testComputer.cpu.P & (uint) StatusFlagsMask.C);
            // Negative flag should be correct
            Assert.Equal((A+value) & (uint) StatusFlagsMask.N, testComputer.cpu.P & (uint) StatusFlagsMask.N);
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }

        public void ADC_CrossPageBoundaryShouldAddACycle(uint A, uint value)
        {
            testComputer.MemoryReset();

            uint pointer = 0x40;
            uint Y = 0xFF;
            uint addr = 0xBEE0;

            testComputer.mem.Write(0x0000,opcode);
            testComputer.mem.Write(0x0001,pointer);

            testComputer.mem.Write( pointer ,addr & 0x00FF);
            testComputer.mem.Write( pointer+1,addr >> 8);       
            testComputer.mem.Write(addr+Y,value);

            testComputer.CPUReset();
            testComputer.cpu.A = A;
            testComputer.cpu.Y = Y;

            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles);

            Assert.Equal((A+value) & 0xFF,testComputer.cpu.A);
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.Y,testComputer.cpu.Y);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            // Zero flag should be false
            Assert.Equal((uint) 0, testComputer.cpu.P & (uint) StatusFlagsMask.Z);
            // Overflow flag should be false
            Assert.Equal((uint) 0, testComputer.cpu.P & (uint) StatusFlagsMask.V);
            // Carry flag should be true
            Assert.Equal((uint) StatusFlagsMask.C, testComputer.cpu.P & (uint) StatusFlagsMask.C);
            // Negative flag should be correct
            Assert.Equal((A+value) & (uint) StatusFlagsMask.N, testComputer.cpu.P & (uint) StatusFlagsMask.N);
            Assert.Equal(cpuCopy.PC+bytes+1,testComputer.cpu.PC);
        }


    }


    public class ADC_INDIRECTINDEXEDT
    {

        private Computer testComputer = new Computer();
        private uint opcode = 0xB1;
        private int cycles = 5;
        private int bytes = 2;


        [Fact]
        public void ADC_ShouldWork()
        {
            testComputer.MemoryReset();
            
            // ADC (#0xAA),Y
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

            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles);

            Assert.Equal(A,testComputer.cpu.A);
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.Y,testComputer.cpu.Y);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            Assert.Equal(cpuCopy.P,testComputer.cpu.P);
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }

        [Fact]
        public void ADC_ZeroShouldSetZFlag()
        {
            testComputer.MemoryReset();

             // ADC (#0xAA),Y
            uint pointer = 0xAA;
            uint A = 0x00;
            uint Y = 0x0F;
            uint addr = 0xBEE0;

            testComputer.mem.Write(0x0000,opcode);
            testComputer.mem.Write(0x0001,pointer);

            testComputer.mem.Write(pointer,addr & 0x00FF);
            testComputer.mem.Write(pointer+1,addr >> 8);       

            testComputer.mem.Write(addr+Y,A);

            testComputer.CPUReset();
            testComputer.cpu.Y = Y;

            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles);

            Assert.Equal(A,testComputer.cpu.A);
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
        public void ADC_NegativeShouldSetNFlag()
        {
            testComputer.MemoryReset();

              // ADC (#0xAA),Y
            uint pointer = 0xAA;
            uint A = 0x84;
            uint Y = 0x0F;
            uint addr = 0xBEE0;

            testComputer.mem.Write(0x0000,opcode);
            testComputer.mem.Write(0x0001,pointer);

            testComputer.mem.Write(pointer,addr & 0x00FF);
            testComputer.mem.Write(pointer+1,addr >> 8);       

            testComputer.mem.Write(addr+Y,A);

            testComputer.CPUReset();
            testComputer.cpu.Y = Y;

            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles);

            Assert.Equal(A,testComputer.cpu.A);
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
        public void ADC_PageBoundryCrossShouldAddACycle()
        {
            testComputer.MemoryReset();

               // ADC (#0xAA),Y
            uint pointer = 0xAA;
            uint A = 0x44;
            uint Y = 0xAF;
            uint addr = 0xBEE0;

            testComputer.mem.Write(0x0000,opcode);
            testComputer.mem.Write(0x0001,pointer);

            testComputer.mem.Write(pointer,addr & 0x00FF);
            testComputer.mem.Write(pointer+1,addr >> 8);       

            testComputer.mem.Write(addr+Y,A);

            testComputer.CPUReset();
            testComputer.cpu.Y = Y;

            var cpuCopy = testComputer.Clone();

            int tick = testComputer.Execute(cycles+1);

            Assert.Equal(A,testComputer.cpu.A);
            Assert.Equal(cpuCopy.X,testComputer.cpu.X);
            Assert.Equal(cpuCopy.Y,testComputer.cpu.Y);
            Assert.Equal(cpuCopy.S,testComputer.cpu.S);
            Assert.Equal(cpuCopy.P,testComputer.cpu.P);
            Assert.Equal(cpuCopy.PC+bytes,testComputer.cpu.PC);
        }

    }

}
