using System;

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
        ACCUMULATOR,
        IMMEDIATE,
        ZEROPAGE,
        ZEROPAGEX,
        ZEROPAGEY,
        RELATIVE,
        ABSOLUTE,
        ABSOLUTEX,
        ABSOLUTEY,
        INDIRECT,
        INDEXEDINDIRECT,
        INDIRECTINDEXED


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
        public uint IR { get; private set;}

 

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
        public Cpu() {
            Init();
            
        }
        private void setNZ(uint value) {
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

            if (SYNC) {
                // Data pins should contain opcode
                IR = DataPins;
                SYNC = false;
                _opcycle = 0;
            }
            RW=true;
            switch(IR) {
                // LDA
                case 0xA5:
                    _LDA(addressingModes.ZEROPAGE,_opcycle);
                    break;
                case 0xB5:
                    _LDA(addressingModes.ZEROPAGEX,_opcycle);
                    break;
                case 0xA9:
                    _LDA(addressingModes.IMMEDIATE,_opcycle);
                    break;
                case 0xAD:
                    _LDA(addressingModes.ABSOLUTE,_opcycle);
                    break;
                case 0xBD:
                    _LDA(addressingModes.ABSOLUTEX,_opcycle);
                    break;
                case 0xB9:
                    _LDA(addressingModes.ABSOLUTEY,_opcycle);
                    break;
                case 0xA1:
                    _LDA(addressingModes.INDEXEDINDIRECT,_opcycle);
                    break;
                case 0xB1:
                    _LDA(addressingModes.INDIRECTINDEXED,_opcycle);
                    break;
                // LDX
                case 0xA6:
                    _LDX(addressingModes.ZEROPAGE,_opcycle);
                    break;
                case 0xB6:
                    _LDX(addressingModes.ZEROPAGEY,_opcycle);
                    break;
                case 0xA2:
                    _LDX(addressingModes.IMMEDIATE,_opcycle);
                    break;
                case 0xAE:
                    _LDX(addressingModes.ABSOLUTE,_opcycle);
                    break;
                case 0xBE:
                    _LDX(addressingModes.ABSOLUTEY,_opcycle);
                    break;

                // LDY
                case 0xA4:
                    _LDY(addressingModes.ZEROPAGE,_opcycle);
                    break;
                case 0xB4:
                    _LDY(addressingModes.ZEROPAGEX,_opcycle);
                    break;
                case 0xA0:
                    _LDY(addressingModes.IMMEDIATE,_opcycle);
                    break;
                case 0xAC:
                    _LDY(addressingModes.ABSOLUTE,_opcycle);
                    break;
                case 0xBC:
                    _LDY(addressingModes.ABSOLUTEX,_opcycle);
                    break;

                case 0x20:
                    _JSR(addressingModes.ABSOLUTE,_opcycle);
                    break;
                case 0x4C:
                    _JMP(addressingModes.ABSOLUTE,_opcycle);
                    break;
                case 0xE8:
                    _INX(addressingModes.IMPLIED,_opcycle);
                    break;
                case 0xC8:
                    _INY(addressingModes.IMPLIED,_opcycle);
                    break;
                case 0x88:
                    _DEY(addressingModes.IMPLIED,_opcycle);
                    break;
                case 0xE6:
                    _INC(addressingModes.ZEROPAGE,_opcycle);
                    break;
                case 0xF6:
                    _INC(addressingModes.ZEROPAGEX,_opcycle);
                    break;
                case 0xEE:
                    _INC(addressingModes.ABSOLUTE,_opcycle);
                    break;
                case 0xFE:
                    _INC(addressingModes.ABSOLUTEX,_opcycle);
                    break;
                case 0xC6:
                    _DEC(addressingModes.ZEROPAGE,_opcycle);
                    break;
                case 0xD6:
                    _DEC(addressingModes.ZEROPAGEX,_opcycle);
                    break;
                case 0xCE:
                    _DEC(addressingModes.ABSOLUTE,_opcycle);
                    break;
                case 0xDE:
                    _DEC(addressingModes.ABSOLUTEX,_opcycle);
                    break;
                case 0xEA:
                    _NOP(addressingModes.IMPLIED,_opcycle);
                    break;

                // STA
                case 0x85:
                    _STA(addressingModes.ZEROPAGE,_opcycle);
                    break;
                case 0x95:
                    _STA(addressingModes.ZEROPAGEX,_opcycle);
                    break;
                case 0x8D:
                    _STA(addressingModes.ABSOLUTE,_opcycle);
                    break;
                case 0x9D:
                    _STA(addressingModes.ABSOLUTEX,_opcycle);
                    break;
                case 0x99:
                    _STA(addressingModes.ABSOLUTEY,_opcycle);
                    break;
                case 0x81:
                    _STA(addressingModes.INDEXEDINDIRECT,_opcycle);
                    break;

                // STX
                case 0x86:
                    _STX(addressingModes.ZEROPAGE,_opcycle);
                    break;
                case 0x96:
                    _STX(addressingModes.ZEROPAGEY,_opcycle);
                    break;
                case 0x8E:
                    _STX(addressingModes.ABSOLUTE,_opcycle);
                    break;

                // STY
                case 0x84:
                    _STY(addressingModes.ZEROPAGE,_opcycle);
                    break;
                case 0x94:
                    _STY(addressingModes.ZEROPAGEX,_opcycle);
                    break;
                case 0x8C:
                    _STY(addressingModes.ABSOLUTE,_opcycle);
                    break;

                case 0x38:
                    _SEC(addressingModes.IMPLIED,_opcycle);
                    break;

                case 0xF8:
                    _SED(addressingModes.IMPLIED,_opcycle);
                    break;

                case 0x78:
                    _SEI(addressingModes.IMPLIED,_opcycle);
                    break;

                case 0x18:
                    _CLC(addressingModes.IMPLIED,_opcycle);
                    break;

                case 0xD8:
                    _CLD(addressingModes.IMPLIED,_opcycle);
                    break;

                case 0x58:
                    _CLI(addressingModes.IMPLIED,_opcycle);
                    break;

                case 0xB8:
                    _CLV(addressingModes.IMPLIED,_opcycle);
                    break;

                case 0xCA:
                    _DEX(addressingModes.IMPLIED,_opcycle);
                    break;

                case 0x48:
                    _PHA(addressingModes.IMPLIED,_opcycle);
                    break;
                case 0x08:
                    _PHP(addressingModes.IMPLIED,_opcycle);
                    break;
                case 0x68:
                    _PLA(addressingModes.IMPLIED,_opcycle);
                    break;
                case 0x28:
                    _PLP(addressingModes.IMPLIED,_opcycle);
                    break;

                case 0x25:
                    _AND(addressingModes.ZEROPAGE,_opcycle);
                    break;
                case 0x35:
                    _AND(addressingModes.ZEROPAGEX,_opcycle);
                    break;
                case 0x29:
                    _AND(addressingModes.IMMEDIATE,_opcycle);
                    break;
                case 0x2D:
                    _AND(addressingModes.ABSOLUTE,_opcycle);
                    break;
                case 0x3D:
                    _AND(addressingModes.ABSOLUTEX,_opcycle);
                    break;
                case 0x39:
                    _AND(addressingModes.ABSOLUTEY,_opcycle);
                    break;
                case 0x21:
                    _AND(addressingModes.INDEXEDINDIRECT,_opcycle);
                    break;
                case 0x31:
                    _AND(addressingModes.INDIRECTINDEXED,_opcycle);
                    break;

                case 0x05:
                    _ORA(addressingModes.ZEROPAGE,_opcycle);
                    break;
                case 0x15:
                    _ORA(addressingModes.ZEROPAGEX,_opcycle);
                    break;
                case 0x09:
                    _ORA(addressingModes.IMMEDIATE,_opcycle);
                    break;
                case 0x0D:
                    _ORA(addressingModes.ABSOLUTE,_opcycle);
                    break;
                case 0x1D:
                    _ORA(addressingModes.ABSOLUTEX,_opcycle);
                    break;
                case 0x19:
                    _ORA(addressingModes.ABSOLUTEY,_opcycle);
                    break;
                case 0x01:
                    _ORA(addressingModes.INDEXEDINDIRECT,_opcycle);
                    break;
                case 0x11:
                    _ORA(addressingModes.INDIRECTINDEXED,_opcycle);
                    break;

                case 0x45:
                    _EOR(addressingModes.ZEROPAGE,_opcycle);
                    break;
                case 0x55:
                    _EOR(addressingModes.ZEROPAGEX,_opcycle);
                    break;
                case 0x49:
                    _EOR(addressingModes.IMMEDIATE,_opcycle);
                    break;
                case 0x4D:
                    _EOR(addressingModes.ABSOLUTE,_opcycle);
                    break;
                case 0x5D:
                    _EOR(addressingModes.ABSOLUTEX,_opcycle);
                    break;
                case 0x59:
                    _EOR(addressingModes.ABSOLUTEY,_opcycle);
                    break;
                case 0x41:
                    _EOR(addressingModes.INDEXEDINDIRECT,_opcycle);
                    break;
                case 0x51:
                    _EOR(addressingModes.INDIRECTINDEXED,_opcycle);
                    break;
 
 
 
                default:
                    Console.WriteLine($"Unhandled opcode {IR}");
                    throw new System.InvalidOperationException(String.Format("Unhandled opcode {0,2:X2}",IR));
            }

            _opcycle++;

        }




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

        /*
        INX  Increment Index X by One
        bub
        X + 1 -> X                       N Z C I D V
                                        + + - - - -

        addressing    assembler    opc  bytes  cyles
        --------------------------------------------
        implied       INX           E8    1     2
        */
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

    }
}
