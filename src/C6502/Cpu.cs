using System;
using System.Collections.Generic;

namespace C6502
{
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
        ABSOLUTE_JMP,
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

        private uint _A,_X,_Y,_S,_P,_PC;
        private uint _AddrPins,_DataPins,_IOPorts;
        //private uint _RW,_SYNC,_RD,_AEC,_IRQ,_NMI,_RES,_PHY1,_PHY2;

        public uint A { get => _A; set => _A = value & 0xFF;}
        public uint X { get => _X; set => _X = value & 0xFF;}
        public uint Y { get => _Y; set => _Y = value & 0xFF;}

        // Stack pointer
        public uint S { get => _S; set => _S = value & 0xFF;}

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

        private uint _opcycle;
        public Instruction IR { get; set;}

 

        private void Init() {
            SYNC = true;
            RW = true;
            RES = true;

            AddrPins = 0x0000;
            DataPins = 0xA9;
            PC = 0x8000;
            S = 0xFD;
            A = 0xAA;
            X = 0x00;
            Y = 0x00;

        }
        private Dictionary<uint,Instruction> instructionSet;

        public Cpu() {
            Init();

            this.instructionSet = new Dictionary<uint, Instruction>();
            instructionSet.Add(0xA9,new LDA{ AddressingMode = addressingModes.IMMEDIATE});
            instructionSet.Add(0xA5,new LDA{ AddressingMode = addressingModes.ZEROPAGE_R});
            instructionSet.Add(0xB5,new LDA{ AddressingMode = addressingModes.ZEROPAGEX_R});
            instructionSet.Add(0xAD,new LDA{ AddressingMode = addressingModes.ABSOLUTE_R});
            instructionSet.Add(0xBD,new LDA{ AddressingMode = addressingModes.ABSOLUTEX_R});
            instructionSet.Add(0xB9,new LDA{ AddressingMode = addressingModes.ABSOLUTEY_R});
            instructionSet.Add(0xA1,new LDA{ AddressingMode = addressingModes.INDEXEDINDIRECT_R});
            instructionSet.Add(0xB1,new LDA{ AddressingMode = addressingModes.INDIRECTINDEXED_R});

            instructionSet.Add(0x69,new ADC{ AddressingMode = addressingModes.IMMEDIATE});
            instructionSet.Add(0x65,new ADC{ AddressingMode = addressingModes.ZEROPAGE_R});
            instructionSet.Add(0x75,new ADC{ AddressingMode = addressingModes.ZEROPAGEX_R});
            instructionSet.Add(0x6D,new ADC{ AddressingMode = addressingModes.ABSOLUTE_R});
            instructionSet.Add(0x7D,new ADC{ AddressingMode = addressingModes.ABSOLUTEX_R});
            instructionSet.Add(0x79,new ADC{ AddressingMode = addressingModes.ABSOLUTEY_R});
            instructionSet.Add(0x61,new ADC{ AddressingMode = addressingModes.INDEXEDINDIRECT_R});
            instructionSet.Add(0x71,new ADC{ AddressingMode = addressingModes.INDIRECTINDEXED_R});

            instructionSet.Add(0xC9,new CMP{ AddressingMode = addressingModes.IMMEDIATE});
            instructionSet.Add(0xC5,new CMP{ AddressingMode = addressingModes.ZEROPAGE_R});
            instructionSet.Add(0xD5,new CMP{ AddressingMode = addressingModes.ZEROPAGEX_R});
            instructionSet.Add(0xCD,new CMP{ AddressingMode = addressingModes.ABSOLUTE_R});
            instructionSet.Add(0xDD,new CMP{ AddressingMode = addressingModes.ABSOLUTEX_R});
            instructionSet.Add(0xd9,new CMP{ AddressingMode = addressingModes.ABSOLUTEY_R});
            instructionSet.Add(0xC1,new CMP{ AddressingMode = addressingModes.INDEXEDINDIRECT_R});
            instructionSet.Add(0xD1,new CMP{ AddressingMode = addressingModes.INDIRECTINDEXED_R});

            instructionSet.Add(0x29,new AND{ AddressingMode = addressingModes.IMMEDIATE});
            instructionSet.Add(0x25,new AND{ AddressingMode = addressingModes.ZEROPAGE_R});
            instructionSet.Add(0x35,new AND{ AddressingMode = addressingModes.ZEROPAGEX_R});
            instructionSet.Add(0x2D,new AND{ AddressingMode = addressingModes.ABSOLUTE_R});
            instructionSet.Add(0x3D,new AND{ AddressingMode = addressingModes.ABSOLUTEX_R});
            instructionSet.Add(0x39,new AND{ AddressingMode = addressingModes.ABSOLUTEY_R});
            instructionSet.Add(0x21,new AND{ AddressingMode = addressingModes.INDEXEDINDIRECT_R});
            instructionSet.Add(0x31,new AND{ AddressingMode = addressingModes.INDIRECTINDEXED_R});
 
            instructionSet.Add(0x09,new ORA{ AddressingMode = addressingModes.IMMEDIATE});
            instructionSet.Add(0x05,new ORA{ AddressingMode = addressingModes.ZEROPAGE_R});
            instructionSet.Add(0x15,new ORA{ AddressingMode = addressingModes.ZEROPAGEX_R});
            instructionSet.Add(0x0D,new ORA{ AddressingMode = addressingModes.ABSOLUTE_R});
            instructionSet.Add(0x1D,new ORA{ AddressingMode = addressingModes.ABSOLUTEX_R});
            instructionSet.Add(0x19,new ORA{ AddressingMode = addressingModes.ABSOLUTEY_R});
            instructionSet.Add(0x01,new ORA{ AddressingMode = addressingModes.INDEXEDINDIRECT_R});
            instructionSet.Add(0x11,new ORA{ AddressingMode = addressingModes.INDIRECTINDEXED_R});

            instructionSet.Add(0x49,new EOR{ AddressingMode = addressingModes.IMMEDIATE});
            instructionSet.Add(0x45,new EOR{ AddressingMode = addressingModes.ZEROPAGE_R});
            instructionSet.Add(0x55,new EOR{ AddressingMode = addressingModes.ZEROPAGEX_R});
            instructionSet.Add(0x4D,new EOR{ AddressingMode = addressingModes.ABSOLUTE_R});
            instructionSet.Add(0x5D,new EOR{ AddressingMode = addressingModes.ABSOLUTEX_R});
            instructionSet.Add(0x59,new EOR{ AddressingMode = addressingModes.ABSOLUTEY_R});
            instructionSet.Add(0x41,new EOR{ AddressingMode = addressingModes.INDEXEDINDIRECT_R});
            instructionSet.Add(0x51,new EOR{ AddressingMode = addressingModes.INDIRECTINDEXED_R});

 
            instructionSet.Add(0xA2,new LDX{ AddressingMode = addressingModes.IMMEDIATE});
            instructionSet.Add(0xA6,new LDX{ AddressingMode = addressingModes.ZEROPAGE_R});
            instructionSet.Add(0xB6,new LDX{ AddressingMode = addressingModes.ZEROPAGEY_R});
            instructionSet.Add(0xAE,new LDX{ AddressingMode = addressingModes.ABSOLUTE_R});
            instructionSet.Add(0xBE,new LDX{ AddressingMode = addressingModes.ABSOLUTEY_R});

            instructionSet.Add(0xA0,new LDY{ AddressingMode = addressingModes.IMMEDIATE});
            instructionSet.Add(0xA4,new LDY{ AddressingMode = addressingModes.ZEROPAGE_R});
            instructionSet.Add(0xB4,new LDY{ AddressingMode = addressingModes.ZEROPAGEX_R});
            instructionSet.Add(0xAC,new LDY{ AddressingMode = addressingModes.ABSOLUTE_R});
            instructionSet.Add(0xBC,new LDY{ AddressingMode = addressingModes.ABSOLUTEX_R});

            //instructionSet.Add(0x08,new PHP(addressingModes.IMPLIED));
            instructionSet.Add(0x18,new CLC{ AddressingMode = addressingModes.IMPLIED});
            //instructionSet.Add(0x28,new PLP(addressingModes.IMPLIED));
            instructionSet.Add(0x38,new SEC{ AddressingMode = addressingModes.IMPLIED});
            //instructionSet.Add(0x48,new PHA(addressingModes.IMPLIED));
            instructionSet.Add(0x58,new CLI{ AddressingMode = addressingModes.IMPLIED});
            //instructionSet.Add(0x68,new PLA(addressingModes.IMPLIED));
            instructionSet.Add(0x78,new SEI{ AddressingMode = addressingModes.IMPLIED});
            instructionSet.Add(0x88,new DEY{ AddressingMode = addressingModes.IMPLIED});
            instructionSet.Add(0x8A,new TXA{ AddressingMode = addressingModes.IMPLIED});
            instructionSet.Add(0x98,new TYA{ AddressingMode = addressingModes.IMPLIED});
            instructionSet.Add(0x9A,new TXS{ AddressingMode = addressingModes.IMPLIED});
            instructionSet.Add(0xA8,new TAY{ AddressingMode = addressingModes.IMPLIED});
            instructionSet.Add(0xAA,new TAX{ AddressingMode = addressingModes.IMPLIED});
            instructionSet.Add(0xB8,new CLV{ AddressingMode = addressingModes.IMPLIED});
            instructionSet.Add(0xBA,new TSX{ AddressingMode = addressingModes.IMPLIED});
            instructionSet.Add(0xC8,new INY{ AddressingMode = addressingModes.IMPLIED});
            instructionSet.Add(0xD8,new CLD{ AddressingMode = addressingModes.IMPLIED});
            instructionSet.Add(0xE8,new INX{ AddressingMode = addressingModes.IMPLIED});
            instructionSet.Add(0xF8,new SED{ AddressingMode = addressingModes.IMPLIED});

            instructionSet.Add(0xCA,new DEX{ AddressingMode = addressingModes.IMPLIED});
            instructionSet.Add(0xEA,new NOP{ AddressingMode = addressingModes.IMPLIED});

            instructionSet.Add(0x84,new STY{ AddressingMode = addressingModes.ZEROPAGE_W});
            instructionSet.Add(0x85,new STA{ AddressingMode = addressingModes.ZEROPAGE_W});
            instructionSet.Add(0x86,new STX{ AddressingMode = addressingModes.ZEROPAGE_W});

            instructionSet.Add(0x94,new STY{ AddressingMode = addressingModes.ZEROPAGEX_W});
            instructionSet.Add(0x95,new STA{ AddressingMode = addressingModes.ZEROPAGEX_W});
            instructionSet.Add(0x96,new STX{ AddressingMode = addressingModes.ZEROPAGEY_W});
            
            instructionSet.Add(0x8C,new STY{ AddressingMode = addressingModes.ABSOLUTE_W});
            instructionSet.Add(0x8D,new STA{ AddressingMode = addressingModes.ABSOLUTE_W});
            instructionSet.Add(0x8E,new STX{ AddressingMode = addressingModes.ABSOLUTE_W});
            
            instructionSet.Add(0x9D,new STA{ AddressingMode = addressingModes.ABSOLUTEX_W});
            instructionSet.Add(0x99,new STA{ AddressingMode = addressingModes.ABSOLUTEY_W});

            instructionSet.Add(0x81,new STA{ AddressingMode = addressingModes.INDEXEDINDIRECT_W});
            instructionSet.Add(0x91,new STA{ AddressingMode = addressingModes.INDIRECTINDEXED_W});

            instructionSet.Add(0xCE,new DEC{ AddressingMode = addressingModes.ABSOLUTE_RMW});
            instructionSet.Add(0xC6,new DEC{ AddressingMode = addressingModes.ZEROPAGE_RMW});
            instructionSet.Add(0xDE,new DEC{ AddressingMode = addressingModes.ABSOLUTEX_RMW});
            instructionSet.Add(0xD6,new DEC{ AddressingMode = addressingModes.ZEROPAGEX_RMW});

            instructionSet.Add(0xEE,new INC{ AddressingMode = addressingModes.ABSOLUTE_RMW});
            instructionSet.Add(0xE6,new INC{ AddressingMode = addressingModes.ZEROPAGE_RMW});
            instructionSet.Add(0xFE,new INC{ AddressingMode = addressingModes.ABSOLUTEX_RMW});
            instructionSet.Add(0xF6,new INC{ AddressingMode = addressingModes.ZEROPAGEX_RMW});



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
            if (SYNC) {
                // Data pins should contain opcode
                try {
                    IR = instructionSet[DataPins];
                }
                catch (KeyNotFoundException){
                    throw new System.InvalidOperationException(String.Format("Unhandled opcode {0,2:X2}",DataPins));
                }
                SYNC = false;
                _opcycle = 0;
            }
            addressing = IR.AddressingMode;
            RW=true;
            switch(addressing) {
                case addressingModes.IMPLIED : {
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
                            AddrPins =  ( DataPins + X) &0xFF;
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
                            AddrPins =  ( DataPins + X) &0xFF;
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
 
               default: {
                    throw new System.InvalidOperationException($"Unhandled addressing mode {addressing.ToString()}");
                }
            }
             _opcycle++;
        }

        /*

        private void _LDA(addressingModes addressing, uint opcycle) {
            switch(addressing) {
                case addressingModes.IMMEDIATE: {
                    switch(opcycle) {
                        case 0: {            
                            PC++;
                            AddrPins = PC;
                            PC++;
                            break;
                        }
                        case 1: {
                            A = DataPins;
                            setNZ(A);
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
                case addressingModes.ZEROPAGE : {
                  switch(opcycle) {
                    
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
                            A = DataPins;
                            setNZ(A);

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
                case addressingModes.ZEROPAGEX : {
                  switch(opcycle) {
                    
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
                            A = DataPins;
                            setNZ(A);
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
                case addressingModes.ABSOLUTE : {
                  switch(opcycle) {
                    
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
                            A = DataPins;
                            setNZ(A);
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
                case addressingModes.ABSOLUTEX : {
                  switch(opcycle) {
                    
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
                                A = DataPins;
                                setNZ(A);
                                AddrPins = PC;
                                SYNC = true;
                            } else {
                                AddrPins = AddrPins + 0x0100;
                            }
                            break;
                        }

                        case 4: {
                            A = DataPins;
                            setNZ(A);
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
                case addressingModes.ABSOLUTEY : {
                  switch(opcycle) {
                    
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
                                A = DataPins;
                                setNZ(A);
                                AddrPins = PC;
                                SYNC = true;
                            } else {
                                AddrPins = AddrPins + 0x0100;
                            }
                            break;
                        }

                        case 4: {
                            A = DataPins;
                            setNZ(A);
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
                case addressingModes.INDEXEDINDIRECT : {
                  switch(opcycle) {
                    
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
                            AddrPins =  ( DataPins + X) &0xFF;
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
                            A = DataPins;
                            setNZ(A);
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
                case addressingModes.INDIRECTINDEXED : {
                  switch(opcycle) {
                    
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
                                A = DataPins;
                                setNZ(A);
                                AddrPins = PC;
                                SYNC = true;
                            } else {
                                AddrPins += 0x0100;
                            }

                            break;
                        }

                        case 5: {
                            A = DataPins;
                            setNZ(A);
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
                default: {
                    throw new System.InvalidOperationException($"Unhandled addressing mode {addressing.ToString()}");
                }
            }

        }

        private void _LDX(addressingModes addressing, uint opcycle) {
            switch(addressing) {
                case addressingModes.IMMEDIATE: {
                    switch(opcycle) {
                        case 0: {            
                            PC++;
                            AddrPins = PC;
                            PC++;
                            break;
                        }
                        case 1: {
                            X = DataPins;
                            setNZ(X);
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
                case addressingModes.ZEROPAGE : {
                  switch(opcycle) {
                    
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
                            X = DataPins;
                            setNZ(X);

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
                case addressingModes.ZEROPAGEY : {
                  switch(opcycle) {
                    
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
                            X = DataPins;
                            setNZ(X);
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
                case addressingModes.ABSOLUTE : {
                  switch(opcycle) {
                    
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
                            X = DataPins;
                            setNZ(X);
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

                case addressingModes.ABSOLUTEY : {
                  switch(opcycle) {
                    
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
                                X = DataPins;
                                setNZ(X);
                                AddrPins = PC;
                                SYNC = true;
                            } else {
                                AddrPins = AddrPins + 0x0100;
                            }
                            break;
                        }

                        case 4: {
                            X = DataPins;
                            setNZ(X);
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
                default: {
                    throw new System.InvalidOperationException($"Unhandled addressing mode {addressing.ToString()}");
                }
            }

        }

        private void _LDY(addressingModes addressing, uint opcycle) {
            switch(addressing) {
                case addressingModes.IMMEDIATE: {
                    switch(opcycle) {
                        case 0: {            
                            PC++;
                            AddrPins = PC;
                            PC++;
                            break;
                        }
                        case 1: {
                            Y = DataPins;
                            setNZ(Y);
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
                case addressingModes.ZEROPAGE : {
                  switch(opcycle) {
                    
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
                            Y = DataPins;
                            setNZ(Y);

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
                case addressingModes.ZEROPAGEX : {
                  switch(opcycle) {
                    
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
                            Y = DataPins;
                            setNZ(Y);
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
                case addressingModes.ABSOLUTE : {
                  switch(opcycle) {
                    
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
                            Y = DataPins;
                            setNZ(Y);
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

                case addressingModes.ABSOLUTEX : {
                  switch(opcycle) {
                    
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
                                Y = DataPins;
                                setNZ(Y);
                                AddrPins = PC;
                                SYNC = true;
                            } else {
                                AddrPins = AddrPins + 0x0100;
                            }
                            break;
                        }

                        case 4: {
                            Y = DataPins;
                            setNZ(Y);
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
                default: {
                    throw new System.InvalidOperationException($"Unhandled addressing mode {addressing.ToString()}");
                }
            }

        }

         private void _STA(addressingModes addressing, uint opcycle) {
            switch(addressing) {
                case addressingModes.ZEROPAGE : {
                  switch(opcycle) {
                    
                        case 0: {
                            PC++;
                            AddrPins = PC;
                            break;
                        }

                        case 1: {
                            PC++;
                            AddrPins = DataPins;
                            DataPins = A;
                            RW = false;
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
                case addressingModes.ZEROPAGEX : {
                  switch(opcycle) {
                    
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
                            DataPins = A;
                            RW = false;
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
                case addressingModes.ABSOLUTE : {
                  switch(opcycle) {
                    
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
                            DataPins = A;
                            RW = false;
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
                case addressingModes.ABSOLUTEX : {
                  switch(opcycle) {
                    
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
                            DataPins = A;
                            RW = false;
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
                case addressingModes.ABSOLUTEY : {
                  switch(opcycle) {
                    
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
                            DataPins = A;
                            RW = false;
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
                case addressingModes.INDEXEDINDIRECT : {
                  switch(opcycle) {
                    
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
                            AddrPins =  ( DataPins + X) &0xFF;
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
                            DataPins = A;
                            RW = false;
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
 
                case addressingModes.INDIRECTINDEXED : {
                  switch(opcycle) {
                    
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
                            DataPins = A;
                            RW = true;
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
 
                default: {
                    throw new System.InvalidOperationException($"Unhandled addressing mode {addressing.ToString()}");
                }
            }

        }

        private void _STX(addressingModes addressing, uint opcycle) {
            switch(addressing) {
                case addressingModes.ZEROPAGE : {
                  switch(opcycle) {
                    
                        case 0: {
                            PC++;
                            AddrPins = PC;
                            break;
                        }

                        case 1: {
                            PC++;
                            AddrPins = DataPins;
                            DataPins = X;
                            RW = false;
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
                case addressingModes.ZEROPAGEY : {
                  switch(opcycle) {
                    
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
                            DataPins = X;
                            RW = false;
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
                case addressingModes.ABSOLUTE : {
                  switch(opcycle) {
                    
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
                            DataPins = X;
                            RW = false;
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
 
                default: {
                    throw new System.InvalidOperationException($"Unhandled addressing mode {addressing.ToString()}");
                }
            }

        }

        private void _STY(addressingModes addressing, uint opcycle) {
            switch(addressing) {
                case addressingModes.ZEROPAGE : {
                  switch(opcycle) {
                    
                        case 0: {
                            PC++;
                            AddrPins = PC;
                            break;
                        }

                        case 1: {
                            PC++;
                            AddrPins = DataPins;
                            DataPins = Y;
                            RW = false;
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
                case addressingModes.ZEROPAGEX : {
                  switch(opcycle) {
                    
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
                            DataPins = Y;
                            RW = false;
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
                case addressingModes.ABSOLUTE : {
                  switch(opcycle) {
                    
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
                            DataPins = Y;
                            RW = false;
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
 
                default: {
                    throw new System.InvalidOperationException($"Unhandled addressing mode {addressing.ToString()}");
                }
            }

        }

        private void _SEC(addressingModes addressing, uint opcycle) {
            switch(addressing) {
                case addressingModes.IMPLIED : {
                  switch(opcycle) {
                    
                        case 0: {
                            PC++;
                            AddrPins = PC;
                            break;
                        }

                        case 1: {
                            P |= (uint) StatusFlagsMask.C;
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
                default: {
                    throw new System.InvalidOperationException($"Unhandled addressing mode {addressing.ToString()}");
                }
            }
        }

        private void _SED(addressingModes addressing, uint opcycle) {
            switch(addressing) {
                case addressingModes.IMPLIED : {
                  switch(opcycle) {
                    
                        case 0: {
                            PC++;
                            AddrPins = PC;
                            break;
                        }

                        case 1: {
                            P |= (uint) StatusFlagsMask.D;
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
                default: {
                    throw new System.InvalidOperationException($"Unhandled addressing mode {addressing.ToString()}");
                }
            }
        }

        private void _SEI(addressingModes addressing, uint opcycle) {
            switch(addressing) {
                case addressingModes.IMPLIED : {
                  switch(opcycle) {
                    
                        case 0: {
                            PC++;
                            AddrPins = PC;
                            break;
                        }

                        case 1: {
                            P |= (uint) StatusFlagsMask.I;
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
                default: {
                    throw new System.InvalidOperationException($"Unhandled addressing mode {addressing.ToString()}");
                }
            }
        }

        private void _CLC(addressingModes addressing, uint opcycle) {
            switch(addressing) {
                case addressingModes.IMPLIED : {
                  switch(opcycle) {
                    
                        case 0: {
                            PC++;
                            AddrPins = PC;
                            break;
                        }

                        case 1: {
                            P &= (uint) ~StatusFlagsMask.C;
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
                default: {
                    throw new System.InvalidOperationException($"Unhandled addressing mode {addressing.ToString()}");
                }
            }
        }
        private void _CLD(addressingModes addressing, uint opcycle) {
            switch(addressing) {
                case addressingModes.IMPLIED : {
                  switch(opcycle) {
                    
                        case 0: {
                            PC++;
                            AddrPins = PC;
                            break;
                        }

                        case 1: {
                            P &= (uint) ~StatusFlagsMask.D;
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
                default: {
                    throw new System.InvalidOperationException($"Unhandled addressing mode {addressing.ToString()}");
                }
            }
        }
        private void _CLI(addressingModes addressing, uint opcycle) {
            switch(addressing) {
                case addressingModes.IMPLIED : {
                  switch(opcycle) {
                    
                        case 0: {
                            PC++;
                            AddrPins = PC;
                            break;
                        }

                        case 1: {
                            P &= (uint) ~StatusFlagsMask.I;
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
                default: {
                    throw new System.InvalidOperationException($"Unhandled addressing mode {addressing.ToString()}");
                }
            }
        }

        private void _CLV(addressingModes addressing, uint opcycle) {
            switch(addressing) {
                case addressingModes.IMPLIED : {
                  switch(opcycle) {
                    
                        case 0: {
                            PC++;
                            AddrPins = PC;
                            break;
                        }

                        case 1: {
                            P &= (uint) ~StatusFlagsMask.V;
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
                default: {
                    throw new System.InvalidOperationException($"Unhandled addressing mode {addressing.ToString()}");
                }
            }
        }

        private void _DEX(addressingModes addressing, uint opcycle) {
            switch(addressing) {
                case addressingModes.IMPLIED : {
                  switch(opcycle) {
                    
                        case 0: {
                            PC++;
                            AddrPins = PC;
                            break;
                        }

                        case 1: {
                            X = ( X -1 ) & 0xFF;
                            setNZ(X);
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
                default: {
                    throw new System.InvalidOperationException($"Unhandled addressing mode {addressing.ToString()}");
                }
            }
        }


        private void _JMP(addressingModes addressing, uint opcycle) {
            switch(addressing) {
                case addressingModes.ABSOLUTE: {
                    switch(opcycle) {
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
                            PC = DataPins <<8 | AD;
                            AddrPins = PC;
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
        }

        private void _JSR(addressingModes addressing, uint opcycle) {
            switch(opcycle) {
                case 0: {
                    PC++;
                    AddrPins = PC;
                    break;
                }
                case 1: {
                    PC++;
                    AD = DataPins;
                    AddrPins = PC;
                    break;
                }
                case 2: {
                   AddrPins = 0x0100|S;
                   DataPins = PC >> 8; // PCH to be pushed 
                   RW = false;
                   break;
                }
                case 3: {
                    AddrPins = 0x0100|S--;
                    DataPins = PC;
                    RW = false;
                    break;
                }
                case 4: {
                    S--;
                    AddrPins = PC;
                    break;
                }

                case 5: {
                    PC = (DataPins << 8) | AD;
                    AddrPins = PC;
                    SYNC = true;
                    break;
                }
                
                default: {
                    break;
                }
            }
        }

        */
        /*
        INX  Increment Index X by One
        bub
        X + 1 -> X                       N Z C I D V
                                        + + - - - -

        addressing    assembler    opc  bytes  cyles
        --------------------------------------------
        implied       INX           E8    1     2
        */

        /*
        private void _INX(addressingModes addressing , uint opcycle) {
            switch(opcycle) {
                case 0: {
                    PC++;
                    AddrPins = PC;
                    break;
                }
                case 1: {
                    AddrPins = PC;
                    X = (X + 1 ) & 0xFF;
                    setNZ(X);
                    SYNC = true;
                    break;
                }
                default: {
                    break;
                }
            }
        }
        private void _INY(addressingModes addressing , uint opcycle) {
            switch(opcycle) {
                case 0: {
                    PC++;
                    AddrPins = PC;
                    break;
                }
                case 1: {
                    AddrPins = PC;
                    Y = (Y + 1 ) & 0xFF;
                    setNZ(Y);
                    SYNC = true;
                    break;
                }
                default: {
                    break;
                }
            }
        }


        private void _DEY(addressingModes addressing , uint opcycle) {
            switch(opcycle) {
                case 0: {
                    PC++;
                    AddrPins = PC;
                    break;
                }
                case 1: {
                    AddrPins = PC;
                    Y = (Y - 1 ) & 0xFF;
                    setNZ(Y);
                    SYNC = true;
                    break;
                }
                default: {
                    break;
                }
            }
        }

        private void _INC(addressingModes addressing , uint opcycle) {
            switch(addressing) {
                case addressingModes.ZEROPAGE: {
                  switch(opcycle) {
                    
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
                            DataPins = (DataPins + 1) & 0xFF;
                            setNZ(DataPins);
                            RW = false;
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
                case addressingModes.ZEROPAGEX: {
                  switch(opcycle) {
                    
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
                            DataPins = (DataPins + 1) & 0xFF;
                            setNZ(DataPins);
                            RW = false;
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
                case addressingModes.ABSOLUTE: {
                  switch(opcycle) {
                    
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
                            DataPins = (DataPins + 1) & 0xFF;
                            setNZ(DataPins);
                            RW = false;
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
                case addressingModes.ABSOLUTEX: {
                  switch(opcycle) {
                    
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
                            DataPins = (DataPins + 1) & 0xFF;
                            setNZ(DataPins);
                            RW = false;
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
                default: {
                    throw new System.InvalidOperationException($"Unhandled addressing mode {addressing.ToString()}");
                }
            }
        }
        private void _DEC(addressingModes addressing , uint opcycle) {
            switch(addressing) {
                case addressingModes.ZEROPAGE: {
                  switch(opcycle) {
                    
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
                            DataPins = (DataPins - 1) & 0xFF;
                            setNZ(DataPins);
                            RW = false;
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
                case addressingModes.ZEROPAGEX: {
                  switch(opcycle) {
                    
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
                            DataPins = (DataPins - 1) & 0xFF;
                            setNZ(DataPins);
                            RW = false;
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
                case addressingModes.ABSOLUTE: {
                  switch(opcycle) {
                    
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
                            DataPins = (DataPins - 1) & 0xFF;
                            setNZ(DataPins);
                            RW = false;
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
                case addressingModes.ABSOLUTEX: {
                  switch(opcycle) {
                    
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
                            DataPins = (DataPins - 1) & 0xFF;
                            setNZ(DataPins);
                            RW = false;
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
                default: {
                    throw new System.InvalidOperationException($"Unhandled addressing mode {addressing.ToString()}");
                }
            }
        }

        private void _PHA(addressingModes addressing , uint opcycle) {
            switch(opcycle) {
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
        }
        private void _PHP(addressingModes addressing , uint opcycle) {
            switch(opcycle) {
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
        }
        private void _PLP(addressingModes addressing , uint opcycle) {
            switch(opcycle) {
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
                    P = DataPins;
                    SYNC = true;
                    break;
                }
                default: {
                    break;
                }
            }
        }
        private void _PLA(addressingModes addressing , uint opcycle) {
            switch(opcycle) {
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
        }
        private void _AND(addressingModes addressing, uint opcycle) {
            switch(addressing) {
                case addressingModes.IMMEDIATE: {
                    switch(opcycle) {
                        case 0: {            
                            PC++;
                            AddrPins = PC;
                            PC++;
                            break;
                        }
                        case 1: {
                            A &= DataPins;
                            setNZ(A);
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
                case addressingModes.ZEROPAGE : {
                  switch(opcycle) {
                    
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
                            A &= DataPins;
                            setNZ(A);

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
                case addressingModes.ZEROPAGEX : {
                  switch(opcycle) {
                    
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
                            A &= DataPins;
                            setNZ(A);
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
                case addressingModes.ABSOLUTE : {
                  switch(opcycle) {
                    
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
                            A &= DataPins;
                            setNZ(A);
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
                case addressingModes.ABSOLUTEX : {
                  switch(opcycle) {
                    
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
                                A &= DataPins;
                                setNZ(A);
                                AddrPins = PC;
                                SYNC = true;
                            } else {
                                AddrPins = AddrPins + 0x0100;
                            }
                            break;
                        }

                        case 4: {
                            A &= DataPins;
                            setNZ(A);
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
                case addressingModes.ABSOLUTEY : {
                  switch(opcycle) {
                    
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
                                A &= DataPins;
                                setNZ(A);
                                AddrPins = PC;
                                SYNC = true;
                            } else {
                                AddrPins = AddrPins + 0x0100;
                            }
                            break;
                        }

                        case 4: {
                            A &= DataPins;
                            setNZ(A);
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
                case addressingModes.INDEXEDINDIRECT : {
                  switch(opcycle) {
                    
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
                            AddrPins =  ( DataPins + X) &0xFF;
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
                            A &= DataPins;
                            setNZ(A);
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
                case addressingModes.INDIRECTINDEXED : {
                  switch(opcycle) {
                    
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
                                A &= DataPins;
                                setNZ(A);
                                AddrPins = PC;
                                SYNC = true;
                            } else {
                                AddrPins += 0x0100;
                            }

                            break;
                        }

                        case 5: {
                            A &= DataPins;
                            setNZ(A);
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
                default: {
                    throw new System.InvalidOperationException($"Unhandled addressing mode {addressing.ToString()}");
                }
            }

        }
       private void _ORA(addressingModes addressing, uint opcycle) {
            switch(addressing) {
                case addressingModes.IMMEDIATE: {
                    switch(opcycle) {
                        case 0: {            
                            PC++;
                            AddrPins = PC;
                            PC++;
                            break;
                        }
                        case 1: {
                            A |= DataPins;
                            setNZ(A);
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
                case addressingModes.ZEROPAGE : {
                  switch(opcycle) {
                    
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
                            A |= DataPins;
                            setNZ(A);

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
                case addressingModes.ZEROPAGEX : {
                  switch(opcycle) {
                    
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
                            A |= DataPins;
                            setNZ(A);
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
                case addressingModes.ABSOLUTE : {
                  switch(opcycle) {
                    
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
                            A |= DataPins;
                            setNZ(A);
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
                case addressingModes.ABSOLUTEX : {
                  switch(opcycle) {
                    
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
                                A |= DataPins;
                                setNZ(A);
                                AddrPins = PC;
                                SYNC = true;
                            } else {
                                AddrPins = AddrPins + 0x0100;
                            }
                            break;
                        }

                        case 4: {
                            A |= DataPins;
                            setNZ(A);
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
                case addressingModes.ABSOLUTEY : {
                  switch(opcycle) {
                    
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
                                A |= DataPins;
                                setNZ(A);
                                AddrPins = PC;
                                SYNC = true;
                            } else {
                                AddrPins = AddrPins + 0x0100;
                            }
                            break;
                        }

                        case 4: {
                            A |= DataPins;
                            setNZ(A);
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
                case addressingModes.INDEXEDINDIRECT : {
                  switch(opcycle) {
                    
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
                            AddrPins =  ( DataPins + X) &0xFF;
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
                            A |= DataPins;
                            setNZ(A);
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
                case addressingModes.INDIRECTINDEXED : {
                  switch(opcycle) {
                    
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
                                A |= DataPins;
                                setNZ(A);
                                AddrPins = PC;
                                SYNC = true;
                            } else {
                                AddrPins += 0x0100;
                            }

                            break;
                        }

                        case 5: {
                            A |= DataPins;
                            setNZ(A);
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
                default: {
                    throw new System.InvalidOperationException($"Unhandled addressing mode {addressing.ToString()}");
                }
            }

        }

       private void _EOR(addressingModes addressing, uint opcycle) {
            switch(addressing) {
                case addressingModes.IMMEDIATE: {
                    switch(opcycle) {
                        case 0: {            
                            PC++;
                            AddrPins = PC;
                            PC++;
                            break;
                        }
                        case 1: {
                            A ^= DataPins;
                            setNZ(A);
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
                case addressingModes.ZEROPAGE : {
                  switch(opcycle) {
                    
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
                            A ^= DataPins;
                            setNZ(A);

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
                case addressingModes.ZEROPAGEX : {
                  switch(opcycle) {
                    
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
                            A ^= DataPins;
                            setNZ(A);
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
                case addressingModes.ABSOLUTE : {
                  switch(opcycle) {
                    
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
                            A ^= DataPins;
                            setNZ(A);
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
                case addressingModes.ABSOLUTEX : {
                  switch(opcycle) {
                    
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
                                A ^= DataPins;
                                setNZ(A);
                                AddrPins = PC;
                                SYNC = true;
                            } else {
                                AddrPins = AddrPins + 0x0100;
                            }
                            break;
                        }

                        case 4: {
                            A ^= DataPins;
                            setNZ(A);
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
                case addressingModes.ABSOLUTEY : {
                  switch(opcycle) {
                    
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
                                A ^= DataPins;
                                setNZ(A);
                                AddrPins = PC;
                                SYNC = true;
                            } else {
                                AddrPins = AddrPins + 0x0100;
                            }
                            break;
                        }

                        case 4: {
                            A ^= DataPins;
                            setNZ(A);
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
                case addressingModes.INDEXEDINDIRECT : {
                  switch(opcycle) {
                    
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
                            AddrPins =  ( DataPins + X) &0xFF;
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
                            A ^= DataPins;
                            setNZ(A);
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
                case addressingModes.INDIRECTINDEXED : {
                  switch(opcycle) {
                    
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
                                A ^= DataPins;
                                setNZ(A);
                                AddrPins = PC;
                                SYNC = true;
                            } else {
                                AddrPins += 0x0100;
                            }

                            break;
                        }

                        case 5: {
                            A ^= DataPins;
                            setNZ(A);
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
                default: {
                    throw new System.InvalidOperationException($"Unhandled addressing mode {addressing.ToString()}");
                }
            }

        }


       private void _NOP(addressingModes addressing , uint opcycle) {
            switch(opcycle) {
                case 0: {
                    PC++;
                    AddrPins = PC;
                    break;
                }
                case 1: {
                    AddrPins = PC;
                    SYNC = true;
                    break;
                }
                default: {
                    break;
                }
            }
        }
        */
    }
}
