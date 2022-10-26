using System;
using System.Collections.Generic;

namespace C6502
{
    public enum BrkFlagsMask : uint {
        RESET = 1,
        NMI = 2,
        IRQ = 4
    }
    public enum StatusFlagsMask : uint {
        C = 1, // Carry Flag
        Z = 2, // Zero Flag
        I = 4, // IRQ Disable Flag
        D = 8, // Decimal mode flag
        B = 16, // Break
        X = 32, // Unused flag
        V = 64, // Overflow
        N = 128 // Negative flag

    }

    public enum addressingModes {
        IMPLIED,
        IMPLIED_PUSH,
        IMPLIED_PULL,
        IMPLIED_BRK,
        IMPLIED_RTS,
        IMPLIED_RTI,
        ACCUMULATOR,
        IMMEDIATE,
        ZEROPAGE_R,
        ZEROPAGE_RMW,
        ZEROPAGE_W,
        ZEROPAGEX_R,
        ZEROPAGEX_RMW,
        ZEROPAGEX_W,
        ZEROPAGEY_R,
        ZEROPAGEY_RMW,
        ZEROPAGEY_W,
        RELATIVE,
        ABSOLUTE_R,
        ABSOLUTE_RMW,
        ABSOLUTE_W,
        ABSOLUTEX_R,
        ABSOLUTEX_RMW,
        ABSOLUTEX_W,
        ABSOLUTEY_R,
        ABSOLUTEY_RMW,
        ABSOLUTEY_W,
        INDIRECT,
        INDEXEDINDIRECT_R,
        INDEXEDINDIRECT_RMW,
        INDEXEDINDIRECT_W,
        INDIRECTINDEXED_R,
        INDIRECTINDEXED_RMW,
        INDIRECTINDEXED_W


    }



    public class Cpu
    {
        // Registers

        public Memory mem;
        private uint _A,_X,_Y,_S,_P,_PC;
        private uint _AddrPins,_DataPins,_IOPorts;
        //private uint _RW,_SYNC,_RD,_AEC,_IRQ,_NMI,_RES,_PHY1,_PHY2;

        public uint A { get => _A; set => _A = value & 0xFF;}
        public uint X { get => _X; set => _X = value & 0xFF;}
        public uint Y { get => _Y; set => _Y = value & 0xFF;}

        // Stack pointer
        //public uint S { get => _S; set => _S = value & 0xFF;}
        public uint S {
            get => _S;
            set
            {
                if (value == 0  || value == 0x100) {

                }
                _S = value & 0xFF;
            }
        }

        // Procesor status
        public uint P { get => _P; set => _P = value ;}

        // Program Counter
        public uint PC { get => _PC; set => _PC = value & 0xFFFF;}

        public uint AD { get ; set;}


        public uint AddrPins { get => _AddrPins; set => _AddrPins = value & 0xFFFF;}
        public uint DataPins { get => _DataPins; set => _DataPins = value & 0xFF;}

        public uint IOPorts { get => _IOPorts; set => _IOPorts = value & 0b0011_1111;}

        // Other pins
        public bool RW { get; set;}
        public bool SYNC { get; set;}
        public bool RDY { get; set;}
        public bool AEC { get; set;}
        public bool IRQ { get; set;}
        public bool NMI { get; set;}
        public bool RES { get; set;}
        public bool PHY1 { get; set;}
        public bool PHY2 { get; set;}

        public uint _opcycle;
        public Instruction IR { get; set;}
        private uint _tmp_stack;

        public uint BrkFlag { get; set;}

        private bool _processing = false;
 

        private void Init() {
            SYNC = true;
            RW = true;
            RES = false;
            BrkFlag = 0;
            AddrPins = 0x0000;
            // DataPins = 0xA9;
            // PC = 0x8000;
            S = 0xFF;
            A = 0x00;
            X = 0x00;
            Y = 0x00;
            // P = (uint) StatusFlagsMask.B | (uint) StatusFlagsMask.I;
            //P = (uint) StatusFlagsMask.B ;

            PHY2 = false;

        }
        private Dictionary<uint,Instruction> instructionSet;

        public Cpu(Memory m) {
            
            mem = m;
            Init();

            this.instructionSet = new Dictionary<uint, Instruction>();
            instructionSet.Add(0xA9,new LDA{ Opcode = 0xA9, AddressingMode = addressingModes.IMMEDIATE});
            instructionSet.Add(0xA5,new LDA{ Opcode = 0xA5,AddressingMode = addressingModes.ZEROPAGE_R});
            instructionSet.Add(0xB5,new LDA{ Opcode = 0xB5,AddressingMode = addressingModes.ZEROPAGEX_R});
            instructionSet.Add(0xAD,new LDA{ Opcode = 0xAD,AddressingMode = addressingModes.ABSOLUTE_R});
            instructionSet.Add(0xBD,new LDA{ Opcode = 0xBD,AddressingMode = addressingModes.ABSOLUTEX_R});
            instructionSet.Add(0xB9,new LDA{ Opcode = 0xB9,AddressingMode = addressingModes.ABSOLUTEY_R});
            instructionSet.Add(0xA1,new LDA{ Opcode = 0xA1,AddressingMode = addressingModes.INDEXEDINDIRECT_R});
            instructionSet.Add(0xB1,new LDA{ Opcode = 0xB1,AddressingMode = addressingModes.INDIRECTINDEXED_R});

            instructionSet.Add(0x69,new ADC{ Opcode = 0x69,AddressingMode = addressingModes.IMMEDIATE});
            instructionSet.Add(0x65,new ADC{ Opcode = 0x65,AddressingMode = addressingModes.ZEROPAGE_R});
            instructionSet.Add(0x75,new ADC{ Opcode = 0x75,AddressingMode = addressingModes.ZEROPAGEX_R});
            instructionSet.Add(0x6D,new ADC{ Opcode = 0x6D,AddressingMode = addressingModes.ABSOLUTE_R});
            instructionSet.Add(0x7D,new ADC{ Opcode = 0x7D,AddressingMode = addressingModes.ABSOLUTEX_R});
            instructionSet.Add(0x79,new ADC{ Opcode = 0x79,AddressingMode = addressingModes.ABSOLUTEY_R});
            instructionSet.Add(0x61,new ADC{ Opcode = 0x61,AddressingMode = addressingModes.INDEXEDINDIRECT_R});
            instructionSet.Add(0x71,new ADC{ Opcode = 0x71,AddressingMode = addressingModes.INDIRECTINDEXED_R});

            instructionSet.Add(0xE9,new SBC{ Opcode = 0xE9,AddressingMode = addressingModes.IMMEDIATE});
            instructionSet.Add(0xE5,new SBC{ Opcode = 0xE5,AddressingMode = addressingModes.ZEROPAGE_R});
            instructionSet.Add(0xF5,new SBC{ Opcode = 0xF5,AddressingMode = addressingModes.ZEROPAGEX_R});
            instructionSet.Add(0xED,new SBC{ Opcode = 0xED,AddressingMode = addressingModes.ABSOLUTE_R});
            instructionSet.Add(0xFD,new SBC{ Opcode = 0xFD,AddressingMode = addressingModes.ABSOLUTEX_R});
            instructionSet.Add(0xF9,new SBC{ Opcode = 0xF9,AddressingMode = addressingModes.ABSOLUTEY_R});
            instructionSet.Add(0xE1,new SBC{ Opcode = 0xE1,AddressingMode = addressingModes.INDEXEDINDIRECT_R});
            instructionSet.Add(0xF1,new SBC{ Opcode = 0xF1,AddressingMode = addressingModes.INDIRECTINDEXED_R});

            instructionSet.Add(0x24,new BIT{ Opcode = 0x24,AddressingMode = addressingModes.ZEROPAGE_R});
            instructionSet.Add(0x2C,new BIT{ Opcode = 0x2C,AddressingMode = addressingModes.ABSOLUTE_R});

            instructionSet.Add(0xC9,new CMP{ Opcode = 0xC9,AddressingMode = addressingModes.IMMEDIATE});
            instructionSet.Add(0xC5,new CMP{ Opcode = 0xC5,AddressingMode = addressingModes.ZEROPAGE_R});
            instructionSet.Add(0xD5,new CMP{ Opcode = 0xD5,AddressingMode = addressingModes.ZEROPAGEX_R});
            instructionSet.Add(0xCD,new CMP{ Opcode = 0xCD,AddressingMode = addressingModes.ABSOLUTE_R});
            instructionSet.Add(0xDD,new CMP{ Opcode = 0xDD,AddressingMode = addressingModes.ABSOLUTEX_R});
            instructionSet.Add(0xd9,new CMP{ Opcode = 0xD9,AddressingMode = addressingModes.ABSOLUTEY_R});
            instructionSet.Add(0xC1,new CMP{ Opcode = 0xC1,AddressingMode = addressingModes.INDEXEDINDIRECT_R});
            instructionSet.Add(0xD1,new CMP{ Opcode = 0xD1,AddressingMode = addressingModes.INDIRECTINDEXED_R});

            instructionSet.Add(0xC0,new CPY{ Opcode = 0xC0,AddressingMode = addressingModes.IMMEDIATE});
            instructionSet.Add(0xC4,new CPY{ Opcode = 0xC4,AddressingMode = addressingModes.ZEROPAGE_R});
            instructionSet.Add(0xCC,new CPY{ Opcode = 0xCC,AddressingMode = addressingModes.ABSOLUTE_R});

            instructionSet.Add(0xE0,new CPX{ Opcode = 0xE0,AddressingMode = addressingModes.IMMEDIATE});
            instructionSet.Add(0xE4,new CPX{ Opcode = 0xE4,AddressingMode = addressingModes.ZEROPAGE_R});
            instructionSet.Add(0xEC,new CPX{ Opcode = 0xEC,AddressingMode = addressingModes.ABSOLUTE_R});

            instructionSet.Add(0x29,new AND{ Opcode = 0x29,AddressingMode = addressingModes.IMMEDIATE});
            instructionSet.Add(0x25,new AND{ Opcode = 0x25,AddressingMode = addressingModes.ZEROPAGE_R});
            instructionSet.Add(0x35,new AND{ Opcode = 0x35,AddressingMode = addressingModes.ZEROPAGEX_R});
            instructionSet.Add(0x2D,new AND{ Opcode = 0x2D,AddressingMode = addressingModes.ABSOLUTE_R});
            instructionSet.Add(0x3D,new AND{ Opcode = 0x3D,AddressingMode = addressingModes.ABSOLUTEX_R});
            instructionSet.Add(0x39,new AND{ Opcode = 0x39,AddressingMode = addressingModes.ABSOLUTEY_R});
            instructionSet.Add(0x21,new AND{ Opcode = 0x21,AddressingMode = addressingModes.INDEXEDINDIRECT_R});
            instructionSet.Add(0x31,new AND{ Opcode = 0x31,AddressingMode = addressingModes.INDIRECTINDEXED_R});
 
            instructionSet.Add(0x09,new ORA{ Opcode = 0x09,AddressingMode = addressingModes.IMMEDIATE});
            instructionSet.Add(0x05,new ORA{ Opcode = 0x05,AddressingMode = addressingModes.ZEROPAGE_R});
            instructionSet.Add(0x15,new ORA{ Opcode = 0x15,AddressingMode = addressingModes.ZEROPAGEX_R});
            instructionSet.Add(0x0D,new ORA{ Opcode = 0x0D,AddressingMode = addressingModes.ABSOLUTE_R});
            instructionSet.Add(0x1D,new ORA{ Opcode = 0x1D,AddressingMode = addressingModes.ABSOLUTEX_R});
            instructionSet.Add(0x19,new ORA{ Opcode = 0x19,AddressingMode = addressingModes.ABSOLUTEY_R});
            instructionSet.Add(0x01,new ORA{ Opcode = 0x01,AddressingMode = addressingModes.INDEXEDINDIRECT_R});
            instructionSet.Add(0x11,new ORA{ Opcode = 0x11,AddressingMode = addressingModes.INDIRECTINDEXED_R});

            instructionSet.Add(0x49,new EOR{ Opcode = 0x49,AddressingMode = addressingModes.IMMEDIATE});
            instructionSet.Add(0x45,new EOR{ Opcode = 0x45,AddressingMode = addressingModes.ZEROPAGE_R});
            instructionSet.Add(0x55,new EOR{ Opcode = 0x35,AddressingMode = addressingModes.ZEROPAGEX_R});
            instructionSet.Add(0x4D,new EOR{ Opcode = 0x4D,AddressingMode = addressingModes.ABSOLUTE_R});
            instructionSet.Add(0x5D,new EOR{ Opcode = 0x5D,AddressingMode = addressingModes.ABSOLUTEX_R});
            instructionSet.Add(0x59,new EOR{ Opcode = 0x59,AddressingMode = addressingModes.ABSOLUTEY_R});
            instructionSet.Add(0x41,new EOR{ Opcode = 0x41,AddressingMode = addressingModes.INDEXEDINDIRECT_R});
            instructionSet.Add(0x51,new EOR{ Opcode = 0x51,AddressingMode = addressingModes.INDIRECTINDEXED_R});

 
            instructionSet.Add(0xA2,new LDX{ Opcode = 0xA2,AddressingMode = addressingModes.IMMEDIATE});
            instructionSet.Add(0xA6,new LDX{ Opcode = 0xA6,AddressingMode = addressingModes.ZEROPAGE_R});
            instructionSet.Add(0xB6,new LDX{ Opcode = 0xB6,AddressingMode = addressingModes.ZEROPAGEY_R});
            instructionSet.Add(0xAE,new LDX{ Opcode = 0xAE,AddressingMode = addressingModes.ABSOLUTE_R});
            instructionSet.Add(0xBE,new LDX{ Opcode = 0xBE,AddressingMode = addressingModes.ABSOLUTEY_R});

            instructionSet.Add(0xA0,new LDY{ Opcode = 0xA0,AddressingMode = addressingModes.IMMEDIATE});
            instructionSet.Add(0xA4,new LDY{ Opcode = 0xA4,AddressingMode = addressingModes.ZEROPAGE_R});
            instructionSet.Add(0xB4,new LDY{ Opcode = 0xB4,AddressingMode = addressingModes.ZEROPAGEX_R});
            instructionSet.Add(0xAC,new LDY{ Opcode = 0xAC,AddressingMode = addressingModes.ABSOLUTE_R});
            instructionSet.Add(0xBC,new LDY{ Opcode = 0xBC,AddressingMode = addressingModes.ABSOLUTEX_R});

            instructionSet.Add(0x0A,new ASL{ Opcode = 0x0A,AddressingMode = addressingModes.ACCUMULATOR});
            instructionSet.Add(0x06,new ASL{ Opcode = 0x06,AddressingMode = addressingModes.ZEROPAGE_RMW});
            instructionSet.Add(0x16,new ASL{ Opcode = 0x16,AddressingMode = addressingModes.ZEROPAGEX_RMW});
            instructionSet.Add(0x0E,new ASL{ Opcode = 0x0E,AddressingMode = addressingModes.ABSOLUTE_RMW});
            instructionSet.Add(0x1E,new ASL{ Opcode = 0x1E,AddressingMode = addressingModes.ABSOLUTEX_RMW});

            instructionSet.Add(0x2A,new ROL{ Opcode = 0x2A,AddressingMode = addressingModes.ACCUMULATOR});
            instructionSet.Add(0x26,new ROL{ Opcode = 0x26,AddressingMode = addressingModes.ZEROPAGE_RMW});
            instructionSet.Add(0x36,new ROL{ Opcode = 0x36,AddressingMode = addressingModes.ZEROPAGEX_RMW});
            instructionSet.Add(0x2E,new ROL{ Opcode = 0x2E,AddressingMode = addressingModes.ABSOLUTE_RMW});
            instructionSet.Add(0x3E,new ROL{ Opcode = 0x3E,AddressingMode = addressingModes.ABSOLUTEX_RMW});

            instructionSet.Add(0x6A,new ROR{ Opcode = 0x6A,AddressingMode = addressingModes.ACCUMULATOR});
            instructionSet.Add(0x66,new ROR{ Opcode = 0x66,AddressingMode = addressingModes.ZEROPAGE_RMW});
            instructionSet.Add(0x76,new ROR{ Opcode = 0x76,AddressingMode = addressingModes.ZEROPAGEX_RMW});
            instructionSet.Add(0x6E,new ROR{ Opcode = 0x6E,AddressingMode = addressingModes.ABSOLUTE_RMW});
            instructionSet.Add(0x7E,new ROR{ Opcode = 0x7E,AddressingMode = addressingModes.ABSOLUTEX_RMW});

            instructionSet.Add(0x4A,new LSR{ Opcode = 0x4A,AddressingMode = addressingModes.ACCUMULATOR});
            instructionSet.Add(0x46,new LSR{ Opcode = 0x46,AddressingMode = addressingModes.ZEROPAGE_RMW});
            instructionSet.Add(0x56,new LSR{ Opcode = 0x56,AddressingMode = addressingModes.ZEROPAGEX_RMW});
            instructionSet.Add(0x4E,new LSR{ Opcode = 0x4E,AddressingMode = addressingModes.ABSOLUTE_RMW});
            instructionSet.Add(0x5E,new LSR{ Opcode = 0x5E,AddressingMode = addressingModes.ABSOLUTEX_RMW});

            //instructionSet.Add(0x08,new PHP(addressingModes.IMPLIED));
            instructionSet.Add(0x18,new CLC{ Opcode = 0x18,AddressingMode = addressingModes.IMPLIED});
            //instructionSet.Add(0x28,new PLP(addressingModes.IMPLIED));
            instructionSet.Add(0x38,new SEC{ Opcode = 0x38,AddressingMode = addressingModes.IMPLIED});
            //instructionSet.Add(0x48,new PHA(addressingModes.IMPLIED));
            instructionSet.Add(0x58,new CLI{ Opcode = 0x58,AddressingMode = addressingModes.IMPLIED});
            //instructionSet.Add(0x68,new PLA(addressingModes.IMPLIED));
            instructionSet.Add(0x78,new SEI{ Opcode = 0x78,AddressingMode = addressingModes.IMPLIED});
            instructionSet.Add(0x88,new DEY{ Opcode = 0x88,AddressingMode = addressingModes.IMPLIED});
            instructionSet.Add(0x8A,new TXA{ Opcode = 0x8A,AddressingMode = addressingModes.IMPLIED});
            instructionSet.Add(0x98,new TYA{ Opcode = 0x98,AddressingMode = addressingModes.IMPLIED});
            instructionSet.Add(0x9A,new TXS{ Opcode = 0x9A,AddressingMode = addressingModes.IMPLIED});
            instructionSet.Add(0xA8,new TAY{ Opcode = 0xA8,AddressingMode = addressingModes.IMPLIED});
            instructionSet.Add(0xAA,new TAX{ Opcode = 0xAA,AddressingMode = addressingModes.IMPLIED});
            instructionSet.Add(0xB8,new CLV{ Opcode = 0xB8,AddressingMode = addressingModes.IMPLIED});
            instructionSet.Add(0xBA,new TSX{ Opcode = 0xBA,AddressingMode = addressingModes.IMPLIED});
            instructionSet.Add(0xC8,new INY{ Opcode = 0xC8,AddressingMode = addressingModes.IMPLIED});
            instructionSet.Add(0xD8,new CLD{ Opcode = 0xD8,AddressingMode = addressingModes.IMPLIED});
            instructionSet.Add(0xE8,new INX{ Opcode = 0xE8,AddressingMode = addressingModes.IMPLIED});
            instructionSet.Add(0xF8,new SED{ Opcode = 0xF8,AddressingMode = addressingModes.IMPLIED});

            instructionSet.Add(0xCA,new DEX{ Opcode = 0xCA,AddressingMode = addressingModes.IMPLIED});
            instructionSet.Add(0xEA,new NOP{ Opcode = 0xEA,AddressingMode = addressingModes.IMPLIED});

            instructionSet.Add(0x84,new STY{ Opcode = 0x84,AddressingMode = addressingModes.ZEROPAGE_W});
            instructionSet.Add(0x85,new STA{ Opcode = 0x85,AddressingMode = addressingModes.ZEROPAGE_W});
            instructionSet.Add(0x86,new STX{ Opcode = 0x86,AddressingMode = addressingModes.ZEROPAGE_W});

            instructionSet.Add(0x94,new STY{ Opcode = 0x94,AddressingMode = addressingModes.ZEROPAGEX_W});
            instructionSet.Add(0x95,new STA{ Opcode = 0x95,AddressingMode = addressingModes.ZEROPAGEX_W});
            instructionSet.Add(0x96,new STX{ Opcode = 0x96,AddressingMode = addressingModes.ZEROPAGEY_W});
            
            instructionSet.Add(0x8C,new STY{ Opcode = 0x8C,AddressingMode = addressingModes.ABSOLUTE_W});
            instructionSet.Add(0x8D,new STA{ Opcode = 0x8D,AddressingMode = addressingModes.ABSOLUTE_W});
            instructionSet.Add(0x8E,new STX{ Opcode = 0x8E,AddressingMode = addressingModes.ABSOLUTE_W});
            
            instructionSet.Add(0x9D,new STA{ Opcode = 0x9D,AddressingMode = addressingModes.ABSOLUTEX_W});
            instructionSet.Add(0x99,new STA{ Opcode = 0x99,AddressingMode = addressingModes.ABSOLUTEY_W});

            instructionSet.Add(0x81,new STA{ Opcode = 0x81,AddressingMode = addressingModes.INDEXEDINDIRECT_W});
            instructionSet.Add(0x91,new STA{ Opcode = 0x91,AddressingMode = addressingModes.INDIRECTINDEXED_W});

            instructionSet.Add(0xCE,new DEC{ Opcode = 0xCE,AddressingMode = addressingModes.ABSOLUTE_RMW});
            instructionSet.Add(0xC6,new DEC{ Opcode = 0xC6,AddressingMode = addressingModes.ZEROPAGE_RMW});
            instructionSet.Add(0xDE,new DEC{ Opcode = 0xDE,AddressingMode = addressingModes.ABSOLUTEX_RMW});
            instructionSet.Add(0xD6,new DEC{ Opcode = 0xD6,AddressingMode = addressingModes.ZEROPAGEX_RMW});

            instructionSet.Add(0xEE,new INC{ Opcode = 0xEE,AddressingMode = addressingModes.ABSOLUTE_RMW});
            instructionSet.Add(0xE6,new INC{ Opcode = 0xE6,AddressingMode = addressingModes.ZEROPAGE_RMW});
            instructionSet.Add(0xFE,new INC{ Opcode = 0xFE,AddressingMode = addressingModes.ABSOLUTEX_RMW});
            instructionSet.Add(0xF6,new INC{ Opcode = 0xF6,AddressingMode = addressingModes.ZEROPAGEX_RMW});

            instructionSet.Add(0x10,new BPL{ Opcode = 0x10,AddressingMode = addressingModes.RELATIVE});
            instructionSet.Add(0x30,new BMI{ Opcode = 0x30,AddressingMode = addressingModes.RELATIVE});
            instructionSet.Add(0x90,new BCC{ Opcode = 0x90,AddressingMode = addressingModes.RELATIVE});
            instructionSet.Add(0xB0,new BCS{ Opcode = 0xB0,AddressingMode = addressingModes.RELATIVE});
            instructionSet.Add(0x50,new BVC{ Opcode = 0x50,AddressingMode = addressingModes.RELATIVE});
            instructionSet.Add(0x70,new BVS{ Opcode = 0x70,AddressingMode = addressingModes.RELATIVE});
            instructionSet.Add(0xD0,new BNE{ Opcode = 0xD0,AddressingMode = addressingModes.RELATIVE});
            instructionSet.Add(0xF0,new BEQ{ Opcode = 0xF0,AddressingMode = addressingModes.RELATIVE});

            instructionSet.Add(0x4C,new JMP{ Opcode = 0x4C,AddressingMode = addressingModes.ABSOLUTE_R});
            instructionSet.Add(0x6C,new JMP{ Opcode = 0x6C,AddressingMode = addressingModes.INDIRECT});

            instructionSet.Add(0x20,new JSR{ Opcode = 0x20,AddressingMode = addressingModes.ABSOLUTE_R});
            instructionSet.Add(0x60,new RTS{ Opcode = 0x60,AddressingMode = addressingModes.IMPLIED});
            instructionSet.Add(0x40,new RTI{ Opcode = 0x40,AddressingMode = addressingModes.IMPLIED});
            instructionSet.Add(0x00,new BRK{ Opcode = 0x00,AddressingMode = addressingModes.IMPLIED});
            // Dummy IRQ
            //instructionSet.Add(0x100,new IRQ{ Opcode = 0x100,AddressingMode = addressingModes.IMPLIED});

            instructionSet.Add(0x08,new PHP{ Opcode = 0x08,AddressingMode = addressingModes.IMPLIED});
            instructionSet.Add(0x48,new PHA{ Opcode = 0x48,AddressingMode = addressingModes.IMPLIED});
            instructionSet.Add(0x68,new PLA{ Opcode = 0x68,AddressingMode = addressingModes.IMPLIED});
            instructionSet.Add(0x28,new PLP{ Opcode = 0x28,AddressingMode = addressingModes.IMPLIED});


        }
        public void setNZ(uint value) {
            if (value == 0) {
                _P |= (uint) StatusFlagsMask.Z;
            } else {
                _P &= (uint) ~StatusFlagsMask.Z;
            }
            if ( ( value >> 7 == 1 )) {
                _P |= (uint) StatusFlagsMask.N;
            } else {
                _P &= (uint) ~StatusFlagsMask.N;
            }
        }


        public void Tick() {

            addressingModes addressing;
            uint opcode;
            if (SYNC) {
                // Data pins should contain opcode
                try {
                    IR = instructionSet[DataPins];
                }
                catch (KeyNotFoundException) {
                    throw new System.InvalidOperationException(String.Format("Unhandled opcode {0,2:X2}",DataPins));
                }
                SYNC = false;
                _opcycle = 0;
                if (RES) {
                    BrkFlag |= (uint) BrkFlagsMask.RESET;
                }

                if (IRQ && (( P & (uint) StatusFlagsMask.I) == 0) ) {
                    BrkFlag |= (uint) BrkFlagsMask.IRQ;                    
                    IR = instructionSet[0x00];
                }

                // TODO NMI

                if (BrkFlag != 0 ) {
                    IR = instructionSet[0x00];
                    //P &= (uint) ~StatusFlagsMask.B ;
                    //P |= (uint) StatusFlagsMask.I;
                    RES = false;
                }
            }
            addressing = IR.AddressingMode;
            opcode = IR.Opcode;

            if ( opcode == 0x6C) {

            }
            RW = true;
            switch (opcode) {
                // Special cases for
                //   BRK
                //   RTI,RTS
                //   PHA,PLA, PHP,PLP
                //   JSR
                
                //JSR
                case 0x20: {
                    switch(_opcycle) {
                    
                        case 0: {
                            PC++;
                            AddrPins = PC;
                            break;
                        }

                        case 1: {
                            PC++;
                            // From running visual6502 step by step
                            _tmp_stack =S;
                            AddrPins = 0x100+_tmp_stack;
                            S = DataPins;
                            break;
                        }

                        case 2: {
                            AddrPins = 0x100+_tmp_stack;
                            DataPins = PC >> 8;
                            RW = false;
                            break;
                        }

                        case 3: {
                            _tmp_stack--;
                            DataPins = PC & 0xFF;
                            AddrPins = 0x100+_tmp_stack;
                            RW = false;

                            break;
                        }

                        case 4: {
                            _tmp_stack--;
                            AddrPins = PC;
                            break;
                        }

                        case 5: {
                            AddrPins = DataPins << 8 | S;
                            PC = AddrPins;
                            S = _tmp_stack;
                            SYNC = true;
                            break;
                        }
                        default: {
                            break;
                        }
                    }
                    break;
                }
                //BRK and IRQ
                case 0x00: {
                    switch(_opcycle) {
                    
                        case 0: {
                            PC++;
                            AddrPins = PC;
                            _tmp_stack = S;
                            break;
                        }

                        case 1: {
                            PC++;
                            AddrPins = 0x100+_tmp_stack;
                            DataPins = (PC-2) >> 8;
                            // If reset do not write to stack
                            if ( (BrkFlag & (uint) BrkFlagsMask.RESET) != 0) {
                            } else {
                                _tmp_stack--;
                                RW = false;
                            }
                            break;
                        }

                        case 2: {
                            // FIXME - Not sure correct
                            //PC++;
                            // End FIXME
                            AddrPins = 0x100+_tmp_stack;
                            //DataPins = (PC) & 0xFF;
                            DataPins = (PC-2) & 0xFF;
                            if ( (BrkFlag & (uint) BrkFlagsMask.RESET) != 0) {
                            } else {
                                RW = false;
                                _tmp_stack--;
                            }
                            break;
                        }

                        case 3: {
                            AddrPins = 0x100+_tmp_stack;
                            // Set B status flag only for "normal" BRK
                            // Clear it for NMI/IRQ/RESET
                            if ( BrkFlag == 0) { 
                                P |= (uint) StatusFlagsMask.B;
                            } else {
                                P &= (uint) ~StatusFlagsMask.B;
                            }
                            P |= (uint) StatusFlagsMask.X;
                            //P |= (uint) StatusFlagsMask.I;
                            DataPins = P;
                            // Only write if not RESET
                            if ( (BrkFlag & (uint) BrkFlagsMask.RESET) != 0) {
                            } else {
                                P |= (uint) StatusFlagsMask.I;
                                RW = false;
                                _tmp_stack--;
                            }
                            break;
                        }

                        case 4: {
                            S = _tmp_stack;
                            if ( ( BrkFlag & (uint) BrkFlagsMask.RESET) != 0 ) {
                                AddrPins = 0xFFFC;
                            } else if (( BrkFlag & (uint) BrkFlagsMask.NMI) != 0) {
                                AddrPins = 0xFFFA;
                            } else {
                                AddrPins = 0xFFFE;
                            }
                            break;
                        }

                        case 5: {
                            AD = DataPins;
                            if ( ( BrkFlag & (uint) BrkFlagsMask.RESET) != 0 ) {
                                AddrPins = 0xFFFD;
                            } else if (( BrkFlag & (uint) BrkFlagsMask.NMI) != 0) {
                                AddrPins = 0xFFFB;
                            } else {
                                AddrPins = 0xFFFF;
                            }
                            // TODO: Check if correct cycle to clear BrkFlag
                            BrkFlag = 0;
                            break;
                        }

                        case 6: {
                            AD = DataPins <<8 | AD;
                            PC = AD;
                            AddrPins = PC;
                            SYNC = true;
                            break;
                        }

                        default: {
                            break;
                        }
                    }
                    break;
                }
                // RTI
                case 0x40: {
                    switch(_opcycle) {
                    
                        case 0: {
                            PC++;
                            AddrPins = PC;
                            break;
                        }

                        case 1: {
                            break;
                        }

                        case 2: {
                            S++;
                            AddrPins = 0x100+S;
                            break;
                        }

                        case 3: {
                            S++;
                            P = DataPins;
                            AddrPins = 0x100+S;
                            break;
                        }

                        case 4: {
                            AD = DataPins;
                            S++;
                            AddrPins = 0x100+S;
                            break;
                        }

                        case 5: {
                            AD = DataPins << 8 | AD;
                            PC = AD;
                            AddrPins = PC;
                            SYNC = true;
                            break;
                        }
                        default: {
                            break;
                        }
                    }
                    break;
                }
                // RTS
                case 0x60: {
                    switch(_opcycle) {
                    
                        case 0: {
                            PC++;
                            AddrPins = PC;
                            break;
                        }

                        case 1: {
                            break;
                        }

                        case 2: {
                            S++;
                            AddrPins = 0x100+S;
                            break;
                        }

                        case 3: {
                            AD = DataPins;
                            S++;
                            AddrPins = 0x100+S;
                            break;
                        }

                        case 4: {
                            AD = DataPins << 8 | AD;
                            AddrPins = 0x100+S;
                            break;
                        }

                        case 5: {
                            PC = AD;
                            PC++;
                            AddrPins = PC;
                            SYNC = true;
                            break;
                        }
                        default: {
                            break;
                        }
                    }
                    break;
                }
                // PHA
                case 0x48: {
                    switch(_opcycle) {
                        case 0: {
                            PC++;
                            AddrPins = PC;
                            break;
                        }
                        case 1: {
                            AddrPins = 0x0100+S;
                            DataPins = A;
                            RW = false;
                            break;
                        }
                        case 2: {
                            AddrPins = PC;
                            S -=1;
                            SYNC = true;
                            break;
                        }
                        default: {
                            break;
                        }
                    }
                    break;
                }
                // PLA
                case 0x68: {
                    switch(_opcycle) {
                        case 0: {
                            PC++;
                            AddrPins = PC;
                            break;
                        }
                        case 1: {
                            AddrPins = 0x0100+S;
                            break;
                        }
                        case 2: {
                            S++;
                            AddrPins = 0x0100+S;
                            break;
                        }
                        case 3: {
                            AddrPins = PC;
                            A = DataPins;
                            setNZ(A);
                            SYNC = true;
                            break;
                        }
                        default: {
                            break;
                        }
                    }
                    break;
                }
                // PHP
                case 0x08: {
                    switch(_opcycle) {
                        case 0: {
                            PC++;
                            AddrPins = PC;
                            break;
                        }
                        case 1: {
                            AddrPins = 0x0100+S;
                            DataPins = P;
                            RW = false;
                            break;
                        }
                        case 2: {
                            AddrPins = PC;
                            S -=1;
                            SYNC = true;
                            break;
                        }
                        default: {
                            break;
                        }
                    }
                    break;
                }
                // PLP
                case 0x28: {
                    switch(_opcycle) {
                        case 0: {
                            PC++;
                            AddrPins = PC;
                            break;
                        }
                        case 1: {
                            AddrPins = 0x0100+S;
                            break;
                        }
                        case 2: {
                            S++;
                            AddrPins = 0x0100+S;
                            break;
                        }
                        case 3: {
                            AddrPins = PC;
                            P = DataPins | (uint) StatusFlagsMask.X | (uint) StatusFlagsMask.B;
                            SYNC = true;
                            break;
                        }
                        default: {
                            break;
                        }
                    }
                    break;
                }

                default: {
                    switch(addressing) {
                        case addressingModes.IMPLIED or addressingModes.ACCUMULATOR: {
                            switch(_opcycle) {
                            
                                case 0: {
                                    PC++;
                                    AddrPins = PC;
                                    break;
                                }

                                case 1: {
                                    IR.Execute(this);
                                    break;
                                }

                                default: {
                                    break;
                                }
                            }
                            break;
                        }
                        case addressingModes.IMMEDIATE:
                            switch(_opcycle) {
                                case 0: {            
                                    PC++;
                                    AddrPins = PC;
                                    break;
                                }
                                case 1: {
                                    PC++;
                                    IR.Execute(this);
                                    break;
                                }
                                default: {
                                    break;
                                }
                            }
                            break;
                        case addressingModes.ZEROPAGE_R : {
                        switch(_opcycle) {
                            
                                case 0: {
                                    PC++;
                                    AddrPins = PC;
                                    break;
                                }

                                case 1: {
                                    PC++;
                                    AddrPins = DataPins;
                                    break;
                                }

                                case 2: {
                                    IR.Execute(this);
                                    break;
                                }

                                default: {
                                    break;
                                }
                            }
                            break;
                        }
                        case addressingModes.ZEROPAGE_W : {
                            switch(_opcycle) {
                            
                                case 0: {
                                    PC++;
                                    AddrPins = PC;
                                    break;
                                }

                                case 1: {
                                    PC++;
                                    AddrPins = DataPins;
                                    IR.Execute(this);
                                    break;
                                }

                                case 2: {
                                    AddrPins = PC;
                                    SYNC = true;
                                    break;
                                }

                                default: {
                                    break;
                                }
                            }
                            break;
                        }
                        case addressingModes.ZEROPAGEX_R : {
                            switch(_opcycle) {
                            
                                case 0: {
                                    PC++;
                                    AddrPins = PC;
                                    break;
                                }

                                case 1: {
                                    PC++;
                                    AddrPins = AddrPins;
                                    break;
                                }

                                case 2: {
                                    AddrPins = ( DataPins+X )  & 0x00FF;
                                    break;
                                }

                                case 3: {
                                    IR.Execute(this);
                                    break;
                                }

                                default: {
                                    break;
                                }
                            }
                            break;
                        }
                        case addressingModes.ZEROPAGEX_W : {
                        switch(_opcycle) {
                            
                                case 0: {
                                    PC++;
                                    AddrPins = PC;
                                    break;
                                }

                                case 1: {
                                    PC++;
                                    AddrPins = DataPins;
                                    AD = AddrPins;
                                    break;
                                }

                                case 2: {
                                    AddrPins = ( AD+X )  & 0x00FF;
                                    IR.Execute(this);
                                    break;
                                }

                                case 3: {
                                    AddrPins = PC;
                                    SYNC = true;
                                    break;
                                }

                                default: {
                                    break;
                                }
                            }
                            break;
                        }
                        case addressingModes.ZEROPAGEY_R : {
                            switch(_opcycle) {
                            
                                case 0: {
                                    PC++;
                                    AddrPins = PC;
                                    break;
                                }

                                case 1: {
                                    PC++;
                                    AddrPins = AddrPins;
                                    break;
                                }

                                case 2: {
                                    AddrPins = ( DataPins+Y )  & 0x00FF;
                                    break;
                                }

                                case 3: {
                                    IR.Execute(this);
                                    break;
                                }

                                default: {
                                    break;
                                }
                            }
                            break;
                        }
                        case addressingModes.ZEROPAGEY_W : {
                        switch(_opcycle) {
                            
                                case 0: {
                                    PC++;
                                    AddrPins = PC;
                                    break;
                                }

                                case 1: {
                                    PC++;
                                    AddrPins = DataPins;
                                    AD = AddrPins;
                                    break;
                                }

                                case 2: {
                                    AddrPins = ( AD+Y )  & 0x00FF;
                                    IR.Execute(this);
                                    break;
                                }

                                case 3: {
                                    AddrPins = PC;
                                    SYNC = true;
                                    break;
                                }

                                default: {
                                    break;
                                }
                            }
                            break;
                        }
                        case addressingModes.ZEROPAGE_RMW: {
                            switch(_opcycle) {
                            
                                case 0: {
                                    PC++;
                                    AddrPins = PC;
                                    break;
                                }

                                case 1: {
                                    PC++;
                                    AddrPins = DataPins;
                                    break;
                                }

                                case 2: {
                                    DataPins = DataPins;
                                    RW = false;
                                    break;
                                }

                                case 3: {
                                    IR.Execute(this);
                                    break;
                                }

                                case 4: {
                                    AddrPins = PC;
                                    SYNC = true;
                                    break;
                                }

                                default: {
                                    break;
                                }
                            }                    
                            break;
                        }
                        case addressingModes.ZEROPAGEX_RMW: {
                            switch(_opcycle) {
                            
                                case 0: {
                                    PC++;
                                    AddrPins = PC;
                                    break;
                                }

                                case 1: {
                                    PC++;
                                    AddrPins = DataPins;
                                    break;
                                }

                                case 2: {
                                    AddrPins = (AddrPins + X) & 0xFF;
                                    break;
                                }

                                case 3: {
                                    DataPins = DataPins;
                                    RW = false;
                                    break;
                                }

                                case 4: {
                                    IR.Execute(this);
                                    break;
                                }

                                case 5: {
                                    AddrPins = PC;
                                    SYNC = true;
                                    break;
                                }

                                default: {
                                    break;
                                }
                            }                    
                            break;
                        }
                        case addressingModes.ZEROPAGEY_RMW: {
                            switch(_opcycle) {
                            
                                case 0: {
                                    PC++;
                                    AddrPins = PC;
                                    break;
                                }

                                case 1: {
                                    PC++;
                                    AddrPins = DataPins;
                                    break;
                                }

                                case 2: {
                                    AddrPins = (AddrPins + Y) & 0xFF;
                                    break;
                                }

                                case 3: {
                                    DataPins = DataPins;
                                    RW = false;
                                    break;
                                }

                                case 4: {
                                    IR.Execute(this);
                                    break;
                                }

                                case 5: {
                                    AddrPins = PC;
                                    SYNC = true;
                                    break;
                                }

                                default: {
                                    break;
                                }
                            }                    
                            break;
                        }
                        case addressingModes.ABSOLUTE_R : {
                            switch(_opcycle) {
                                case 0: {
                                    PC++;
                                    AddrPins = PC;
                                    break;
                                }

                                case 1: {
                                    PC++;
                                    AddrPins = PC;
                                    AD = DataPins;
                                    break;
                                }

                                case 2: {
                                    PC++;
                                    AD = DataPins <<8 | AD;
                                    AddrPins = AD;
                                    if (opcode == 0x4C) {
                                        IR.Execute(this);
                                    }
                                    break;
                                }

                                case 3: {
                                    IR.Execute(this);
                                    break;
                                }

                                default: {
                                    break;
                                }
                            }
                            break;
                        }
                        case addressingModes.ABSOLUTE_W : {
                            switch(_opcycle) {
                            
                                case 0: {
                                    PC++;
                                    AddrPins = PC;
                                    break;
                                }

                                case 1: {
                                    PC++;
                                    AddrPins = PC;
                                    AD = DataPins;
                                    break;
                                }

                                case 2: {
                                    PC++;
                                    AD = DataPins <<8 | AD;
                                    AddrPins = AD;
                                    IR.Execute(this);
                                    break;
                                }

                                case 3: {
                                    AddrPins = PC;
                                    SYNC = true;
                                    break;
                                }

                                default: {
                                    break;
                                }
                            }
                            break;
                        }
                        case addressingModes.ABSOLUTE_RMW: {
                            switch(_opcycle) {
                            
                                case 0: {
                                    PC++;
                                    AddrPins = PC;
                                    break;
                                }

                                case 1: {
                                    PC++;
                                    AddrPins = PC;
                                    AD = DataPins;
                                    break;
                                }

                                case 2: {
                                    PC++;
                                    AD = DataPins <<8 | AD;
                                    AddrPins = AD;
                                    break;
                                }

                                case 3: {
                                    DataPins = DataPins;
                                    RW = false;
                                    break;
                                }

                                case 4: {
                                    IR.Execute(this);
                                    break;
                                }

                                case 5: {
                                    AddrPins = PC;
                                    SYNC = true;
                                    break;
                                }

                                default: {
                                    break;
                                }
                            }                    
                            break;
                        }
                        case addressingModes.ABSOLUTEX_R : {
                            switch(_opcycle) {
                                case 0: {
                                    PC++;
                                    AddrPins = PC;
                                    break;
                                }

                                case 1: {
                                    PC++;
                                    AddrPins = PC;
                                    AD = DataPins;
                                    break;
                                }

                                case 2: {
                                    PC++;
                                    AddrPins =  DataPins <<8 | (AD + X) &0xFF;
                                    AD = DataPins <<8 | AD ;
                                    break;
                                }

                                case 3: {
                                    if ( AddrPins >= AD ) {
                                        IR.Execute(this);
                                    } else {
                                        AddrPins = AddrPins + 0x0100;
                                    }
                                    break;
                                }

                                case 4: {
                                    IR.Execute(this);
                                    break;
                                }

                                default: {
                                    break;
                                }
                            }
                            break;
                        }
                        case addressingModes.ABSOLUTEX_W : {
                            switch(_opcycle) {
                            
                                case 0: {
                                    PC++;
                                    AddrPins = PC;
                                    break;
                                }

                                case 1: {
                                    PC++;
                                    AddrPins = PC;
                                    AD = DataPins;
                                    break;
                                }

                                case 2: {
                                    PC++;
                                    AddrPins =  DataPins <<8 | (AD + X) &0xFF;
                                    AD = DataPins <<8 | AD ;
                                    break;
                                }

                                case 3: {
                                    if ( AddrPins < AD ) {
                                        AddrPins = AddrPins + 0x0100;
                                    }
                                    IR.Execute(this);
                                    break;
                                }

                                case 4: {
                                    AddrPins = PC;
                                    SYNC = true;
                                    break;
                                }

                                default: {
                                    break;
                                }
                            }
                            break;
                        }
                        case addressingModes.ABSOLUTEX_RMW: {
                            switch(_opcycle) {
                            
                                case 0: {
                                    PC++;
                                    AddrPins = PC;
                                    break;
                                }

                                case 1: {
                                    PC++;
                                    AddrPins = PC;
                                    AD = DataPins;
                                    break;
                                }

                                case 2: {
                                    PC++;
                                    AddrPins = DataPins <<8 | (AD+X) & 0xFF;
                                    AD =  DataPins <<8 | AD ;
                                    break;
                                }

                                case 3: {
                                    if ( AddrPins < AD) {
                                        AddrPins = AddrPins + 0x0100;
                                    }
                                    break;
                                }

                                case 4: {
                                    RW = false;
                                    break;
                                }

                                case 5: {
                                    IR.Execute(this);
                                    break;
                                }

                                case 6: {
                                    AddrPins = PC;
                                    SYNC = true;
                                    break;
                                }

                                default: {
                                    break;
                                }
                            }                    
                            break;
                        }
                        case addressingModes.ABSOLUTEY_R : {
                            switch(_opcycle) {
                            
                                case 0: {
                                    PC++;
                                    AddrPins = PC;
                                    break;
                                }

                                case 1: {
                                    PC++;
                                    AddrPins = PC;
                                    AD = DataPins;
                                    break;
                                }

                                case 2: {
                                    PC++;
                                    AddrPins =  DataPins <<8 | (AD + Y) &0xFF;
                                    AD = DataPins <<8 | AD ;
                                    break;
                                }

                                case 3: {
                                    if ( AddrPins >= AD ) {
                                        IR.Execute(this);
                                    } else {
                                        AddrPins = AddrPins + 0x0100;
                                    }
                                    break;
                                }

                                case 4: {
                                    IR.Execute(this);
                                    break;
                                }

                                default: {
                                    break;
                                }
                            }
                            break;
                        }
                        case addressingModes.ABSOLUTEY_W : {
                            switch(_opcycle) {
                            
                                case 0: {
                                    PC++;
                                    AddrPins = PC;
                                    break;
                                }

                                case 1: {
                                    PC++;
                                    AddrPins = PC;
                                    AD = DataPins;
                                    break;
                                }

                                case 2: {
                                    PC++;
                                    AddrPins =  DataPins <<8 | (AD + Y) &0xFF;
                                    AD = DataPins <<8 | AD ;
                                    break;
                                }

                                case 3: {
                                    if ( AddrPins < AD ) {
                                        AddrPins = AddrPins + 0x0100;
                                    }
                                    IR.Execute(this);
                                    break;
                                }

                                case 4: {
                                    AddrPins = PC;
                                    SYNC = true;
                                    break;
                                }

                                default: {
                                    break;
                                }
                            }
                            break;
                        }
                        case addressingModes.INDEXEDINDIRECT_R : {
                            switch(_opcycle) {
                            
                                case 0: {
                                    PC++;
                                    AddrPins = PC;
                                    break;
                                }

                                case 1: {
                                    PC++;
                                    AddrPins = DataPins;
                                    break;
                                }

                                case 2: {
                                    AddrPins =  ( AddrPins + X) &0xFF;
                                    break;
                                }

                                case 3: {
                                    AD = DataPins;
                                    AddrPins =  ( AddrPins + 1) &0xFF;
                                    break;
                                }

                                case 4: {
                                    AD = DataPins <<8 | AD;
                                    AddrPins = AD;
                                    break;
                                }

                                case 5: {
                                    IR.Execute(this);
                                    break;
                                }

                                default: {
                                    break;
                                }
                            }
                            break;
                        }
                        case addressingModes.INDIRECTINDEXED_R : {
                            switch(_opcycle) {
                            
                                case 0: {
                                    PC++;
                                    AddrPins = PC;
                                    break;
                                }

                                case 1: {
                                    PC++;
                                    AddrPins = DataPins;
                                    break;
                                }
                                case 2: {
                                    AD = DataPins;
                                    AddrPins = (AddrPins+1) & 0xFF;
                                    break;
                                }

                                case 3: {
                                    AddrPins =  DataPins <<8 | (AD + Y) & 0xFF;
                                    AD = DataPins << 8 | AD;
                                    break;
                                }

                                case 4: {
                                    if ( AddrPins >= AD) {
                                        IR.Execute(this);
                                    } else {
                                        AddrPins += 0x0100;
                                    }

                                    break;
                                }

                                case 5: {
                                    IR.Execute(this);
                                    break;
                                }

                                default: {
                                    break;
                                }
                            }
                            break;
                        }
                        case addressingModes.INDEXEDINDIRECT_W : {
                            switch(_opcycle) {
                            
                                case 0: {
                                    PC++;
                                    AddrPins = PC;
                                    break;
                                }

                                case 1: {
                                    PC++;
                                    AddrPins = DataPins;
                                    break;
                                }

                                case 2: {
                                    AddrPins =  ( AddrPins + X) &0xFF;
                                    break;
                                }

                                case 3: {
                                    AD = DataPins;
                                    AddrPins =  ( AddrPins + 1) &0xFF;
                                    break;
                                }

                                case 4: {
                                    AD = DataPins <<8 | AD;
                                    AddrPins = AD;
                                    IR.Execute(this);
                                    break;
                                }

                                case 5: {
                                    AddrPins = PC;
                                    SYNC = true;
                                    break;
                                }

                                default: {
                                    break;
                                }
                            }
                            break;
                        }
        
                        case addressingModes.INDIRECTINDEXED_W : {
                        switch(_opcycle) {
                            
                                case 0: {
                                    PC++;
                                    AddrPins = PC;
                                    break;
                                }

                                case 1: {
                                    PC++;
                                    AddrPins = DataPins;
                                    break;
                                }
                                case 2: {
                                    AD = DataPins;
                                    AddrPins = (AddrPins+1) & 0xFF;
                                    break;
                                }

                                case 3: {
                                    AddrPins =  DataPins <<8 | (AD + Y) & 0xFF;
                                    AD = DataPins << 8 | AD;
                                    break;
                                }

                                case 4: {
                                    if ( AddrPins < AD) {
                                        AddrPins = (AddrPins + 0x0100) & 0xFFFF;
                                    }
                                    IR.Execute(this);
                                    break;
                                }

                                case 5: {
                                    AddrPins = PC;
                                    SYNC = true;
                                    break;
                                }

                                default: {
                                    break;
                                }
                            }
                            break;
                        } 
                        case addressingModes.RELATIVE : {
                            switch(_opcycle) {
                                case 0: {
                                    PC++;
                                    AddrPins = PC;
                                    break;
                                }

                                case 1: {
                                    PC++;
                                    IR.Execute(this);
                                    break;
                                }

                                case 2: {
                                    if ( ( AddrPins >> 8) == (AD >> 8) ) {
                                        AddrPins = PC;
                                        SYNC = true;
                                    } else {
                                        PC = ( ( AddrPins >> 8) > (AD >> 8) ) ? PC - 0x0100 : PC + 0x100;
                                        AddrPins = PC;
                                    }
                                    break;
                                }

                                case 3: {
                                    AddrPins = PC;
                                    SYNC = true;
                                    break;
                                }

                                default: {
                                    break;
                                }
                            }
                            break;
                        }
                        // Only for JUMP ???
                        case addressingModes.INDIRECT : {
                            switch(_opcycle) {
                                case 0: {
                                    PC++;
                                    AddrPins = PC;
                                    break;
                                }

                                case 1: {
                                    PC++;
                                    AddrPins = PC;
                                    AD = DataPins;
                                    break;
                                }

                                case 2: {
                                    PC++;
                                    AD =  DataPins <<8 | AD ;
                                    AddrPins = AD ;
                                    break;
                                }

                                case 3: {
                                    AD = DataPins;
                                    AddrPins = (AddrPins & 0xFF00) | ((AddrPins+1) & 0xFF);
                                    break;
                                }

                                case 4: {
                                    IR.Execute(this);
                                    break;
                                }

                                default: {
                                    break;
                                }
                            }
                            break;
                        }
        
                    default: {
                            throw new System.InvalidOperationException($"Unhandled addressing mode {addressing.ToString()}");
                        }
                    }
                    //_opcycle++;
                    break;
                }
                
            }

            
            if ( RW ) {
                // Read Data from memory and put them on the data bus
                DataPins = mem.Read(AddrPins);

            } else {
                // Write Data from databus into memory
                mem.Write(AddrPins,DataPins);
            }
            _opcycle++;
        }
    }
}
