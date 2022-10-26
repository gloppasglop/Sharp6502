using System;
using System.Collections.Generic;

namespace C6502
{

    public class Instruction
    {

        public List<Action<Cpu>> Steps;
        public uint Opcode { get; set;}
        public addressingModes AddressingMode {get; set;}

        public virtual void Execute(Cpu cpu) {}

        

    }

    public class Implied: Instruction {

        public Implied() {
            Steps = new List<Action<Cpu>>() {
                cpu => { 
                    cpu.PC++;
                    cpu.AddrPins = cpu.PC;
                },
                cpu => Execute(cpu)
            };
        }
    }


    public class Immediate: Instruction {
        
        public Immediate() {
            Steps = new List<Action<Cpu>>() {
                cpu => { 
                    cpu.PC++;
                    cpu.AddrPins = cpu.PC;
                },
                cpu => {
                    cpu.PC++;
                    Execute(cpu);
                }
            };
        }
    }

    public class PHP : Instruction { }

    public class PLP : Instruction { }

    public class PHA : Instruction { }

    public class PLA : Instruction { }

    public class RTS : Instruction { }

    public class RTI : Instruction { }
    public class BRK : Instruction { }

    public class JSR : Instruction { }
   
    public class BCC : Instruction {
        public override void Execute(Cpu cpu)
        {
            if ((cpu.P & (uint) StatusFlagsMask.C ) == 0) {
                if ( ( cpu.DataPins >> 7 ) == 0 ) {
                    cpu.AD = cpu.PC+cpu.DataPins;
                    cpu.PC = (cpu.PC & 0xFF00 ) | ((cpu.PC + cpu.DataPins) & 0xFF);
                } else {
                    cpu.AD = cpu.PC+cpu.DataPins-0x100;
                    cpu.PC = (cpu.PC & 0xFF00 ) | ((cpu.PC + cpu.DataPins-0x100) & 0xFF);
                }
            } else {
                cpu.SYNC = true;
            }
            cpu.AddrPins =  cpu.PC;
        }
    }
    public class BCS : Instruction {
        public override void Execute(Cpu cpu)
        {
            if ((cpu.P & (uint) StatusFlagsMask.C ) != 0) {
                if ( ( cpu.DataPins >> 7 ) == 0 ) {
                    cpu.AD = cpu.PC+cpu.DataPins;
                    cpu.PC = (cpu.PC & 0xFF00 ) | ((cpu.PC + cpu.DataPins) & 0xFF);
                } else {
                    cpu.AD = cpu.PC+cpu.DataPins-0x100;
                    cpu.PC = (cpu.PC & 0xFF00 ) | ((cpu.PC + cpu.DataPins-0x100) & 0xFF);
                }
            } else {
                cpu.SYNC = true;
            }
            cpu.AddrPins =  cpu.PC;
        }
    }
    public class BVC : Instruction {
        public override void Execute(Cpu cpu)
        {
            if ((cpu.P & (uint) StatusFlagsMask.V ) == 0) {
                if ( ( cpu.DataPins >> 7 ) == 0 ) {
                    cpu.AD = cpu.PC+cpu.DataPins;
                    cpu.PC = (cpu.PC & 0xFF00 ) | ((cpu.PC + cpu.DataPins) & 0xFF);
                } else {
                    cpu.AD = cpu.PC+cpu.DataPins-0x100;
                    cpu.PC = (cpu.PC & 0xFF00 ) | ((cpu.PC + cpu.DataPins-0x100) & 0xFF);
                }
            } else {
                cpu.SYNC = true;
            }
            cpu.AddrPins =  cpu.PC;
        }
    }
    public class BVS : Instruction {
        public override void Execute(Cpu cpu)
        {
            if ((cpu.P & (uint) StatusFlagsMask.V ) != 0) {
                if ( ( cpu.DataPins >> 7 ) == 0 ) {
                    cpu.AD = cpu.PC+cpu.DataPins;
                    cpu.PC = (cpu.PC & 0xFF00 ) | ((cpu.PC + cpu.DataPins) & 0xFF);
                } else {
                    cpu.AD = cpu.PC+cpu.DataPins-0x100;
                    cpu.PC = (cpu.PC & 0xFF00 ) | ((cpu.PC + cpu.DataPins-0x100) & 0xFF);
                }
            } else {
                cpu.SYNC = true;
            }
            cpu.AddrPins =  cpu.PC;
        }
    }
    public class BNE : Instruction {
        public override void Execute(Cpu cpu)
        {
            if ((cpu.P & (uint) StatusFlagsMask.Z ) == 0) {
                if ( ( cpu.DataPins >> 7 ) == 0 ) {
                    cpu.AD = cpu.PC+cpu.DataPins;
                    cpu.PC = (cpu.PC & 0xFF00 ) | ((cpu.PC + cpu.DataPins) & 0xFF);
                } else {
                    cpu.AD = cpu.PC+cpu.DataPins-0x100;
                    cpu.PC = (cpu.PC & 0xFF00 ) | ((cpu.PC + cpu.DataPins-0x100) & 0xFF);
                }
            } else {
                cpu.SYNC = true;
            }
            cpu.AddrPins =  cpu.PC;
        }
    }
    public class BEQ : Instruction {
        public override void Execute(Cpu cpu)
        {
            if ((cpu.P & (uint) StatusFlagsMask.Z ) != 0) {
                if ( ( cpu.DataPins >> 7 ) == 0 ) {
                    cpu.AD = cpu.PC+cpu.DataPins;
                    cpu.PC = (cpu.PC & 0xFF00 ) | ((cpu.PC + cpu.DataPins) & 0xFF);
                } else {
                    cpu.AD = cpu.PC+cpu.DataPins-0x100;
                    cpu.PC = (cpu.PC & 0xFF00 ) | ((cpu.PC + cpu.DataPins-0x100) & 0xFF);
                }
            } else {
                cpu.SYNC = true;
            }
            cpu.AddrPins =  cpu.PC;
        }
    }
    public class BPL : Instruction {
        public override void Execute(Cpu cpu)
        {
            if ((cpu.P & (uint) StatusFlagsMask.N ) == 0) {
                if ( ( cpu.DataPins >> 7 ) == 0 ) {
                    cpu.AD = cpu.PC+cpu.DataPins;
                    cpu.PC = (cpu.PC & 0xFF00 ) | ((cpu.PC + cpu.DataPins) & 0xFF);
                } else {
                    cpu.AD = cpu.PC+cpu.DataPins-0x100;
                    cpu.PC = (cpu.PC & 0xFF00 ) | ((cpu.PC + cpu.DataPins-0x100) & 0xFF);
                }
            } else {
                cpu.SYNC = true;
            }
            cpu.AddrPins =  cpu.PC;
        }
    }
    public class BMI : Instruction {
        public override void Execute(Cpu cpu)
        {
            if ((cpu.P & (uint) StatusFlagsMask.N ) != 0) {
                if ( ( cpu.DataPins >> 7 ) == 0 ) {
                    cpu.AD = cpu.PC+cpu.DataPins;
                    cpu.PC = (cpu.PC & 0xFF00 ) | ((cpu.PC + cpu.DataPins) & 0xFF);
                } else {
                    cpu.AD = cpu.PC+cpu.DataPins-0x100;
                    cpu.PC = (cpu.PC & 0xFF00 ) | ((cpu.PC + cpu.DataPins-0x100) & 0xFF);
                }
            } else {
                cpu.SYNC = true;
            }
            cpu.AddrPins =  cpu.PC;
        }
    }
    public class LDA : Instruction {

        public override void Execute(Cpu cpu) {
            cpu.A = cpu.DataPins;
            cpu.setNZ(cpu.A);
            cpu.AddrPins = cpu.PC;
            cpu.SYNC = true;
        }
    }
    public class LDX : Instruction {
        public override void Execute(Cpu cpu) {
            cpu.X = cpu.DataPins;
            cpu.setNZ(cpu.X);
            cpu.AddrPins = cpu.PC;
            cpu.SYNC = true;
        }
    }
    public class LDY : Instruction {
        public override void Execute(Cpu cpu) {
            cpu.Y = cpu.DataPins;
            cpu.setNZ(cpu.Y);
            cpu.AddrPins = cpu.PC;
            cpu.SYNC = true;
        }
    }
    public class STA : Instruction {
        public override void Execute(Cpu cpu) {
            cpu.DataPins = cpu.A;
            cpu.RW = false;
        }
    }
    public class STX : Instruction {
        public override void Execute(Cpu cpu) {
            cpu.DataPins = cpu.X;
            cpu.RW = false;
        }
    }
    public class STY : Instruction {
        public override void Execute(Cpu cpu) {
            cpu.DataPins = cpu.Y;
            cpu.RW = false;
        }
    }

 
 
    public class TAX : Instruction {
        public override void Execute(Cpu cpu) {
            cpu.AddrPins = cpu.PC;
            cpu.X = cpu.A;
            cpu.setNZ(cpu.X);
            cpu.SYNC = true;
        }
    }
 
    public class TXA : Instruction {
        public override void Execute(Cpu cpu) {
            cpu.AddrPins = cpu.PC;
            cpu.A = cpu.X;
            cpu.setNZ(cpu.A);
            cpu.SYNC = true;
        }
    }

     public class TXS : Instruction {
        public override void Execute(Cpu cpu) {
            cpu.AddrPins = cpu.PC;
            cpu.S = cpu.X;
            cpu.SYNC = true;
        }
    }
 
    public class TSX : Instruction {
        public override void Execute(Cpu cpu) {
            cpu.AddrPins = cpu.PC;
            cpu.X = cpu.S;
            cpu.setNZ(cpu.X);
            cpu.SYNC = true;
        }
    }
 
 
    public class TAY : Instruction {
        public override void Execute(Cpu cpu) {
            cpu.AddrPins = cpu.PC;
            cpu.Y = cpu.A;
            cpu.setNZ(cpu.Y);
            cpu.SYNC = true;
        }
    }

     public class TYA : Instruction {
        public override void Execute(Cpu cpu) {
            cpu.AddrPins = cpu.PC;
            cpu.A = cpu.Y;
            cpu.setNZ(cpu.A);
            cpu.SYNC = true;
        }
    }
    
    public class DEY : Instruction {
        public override void Execute(Cpu cpu) {
            cpu.AddrPins = cpu.PC;
            cpu.Y = (cpu.Y - 1 ) & 0xFF;
            cpu.setNZ(cpu.Y);
            cpu.SYNC = true;
        }
    }
    public class DEX : Instruction {
        public override void Execute(Cpu cpu) {
            cpu.AddrPins = cpu.PC;
            cpu.X = (cpu.X - 1 ) & 0xFF;
            cpu.setNZ(cpu.X);
            cpu.SYNC = true;
        }
    }
    public class INY : Instruction {
        public override void Execute(Cpu cpu) {
            cpu.AddrPins = cpu.PC;
            cpu.Y = (cpu.Y + 1 ) & 0xFF;
            cpu.setNZ(cpu.Y);
            cpu.SYNC = true;
        }
    }
    public class INX : Instruction {
        public override void Execute(Cpu cpu) {
            cpu.AddrPins = cpu.PC;
            cpu.X = (cpu.X + 1 ) & 0xFF;
            cpu.setNZ(cpu.X);
            cpu.SYNC = true;
        }
    }
    public class CLC : Instruction {
        public override void Execute(Cpu cpu) {
            cpu.AddrPins = cpu.PC;
            cpu.P &= (uint) ~StatusFlagsMask.C;
            cpu.SYNC = true;
        }
    }

    public class CLD : Instruction {
        public override void Execute(Cpu cpu) {
            cpu.AddrPins = cpu.PC;
            cpu.P &= (uint) ~StatusFlagsMask.D;
            cpu.SYNC = true;
        }
    }
    public class CLI : Instruction {
        public override void Execute(Cpu cpu) {
            cpu.AddrPins = cpu.PC;
            cpu.P &= (uint) ~StatusFlagsMask.I;
            cpu.SYNC = true;
        }
    }
    public class CLV : Instruction {
        public override void Execute(Cpu cpu) {
            cpu.AddrPins = cpu.PC;
            cpu.P &= (uint) ~StatusFlagsMask.V;
            cpu.SYNC = true;
        }
    }

    public class NOP : Instruction {
        public override void Execute(Cpu cpu) {
            cpu.AddrPins = cpu.PC;
            cpu.SYNC = true;
        }
    }

    public class SEC : Instruction {
        public override void Execute(Cpu cpu) {
            cpu.AddrPins = cpu.PC;
            cpu.P |= (uint) StatusFlagsMask.C;
            cpu.SYNC = true;
        }
    }

    public class SED : Instruction {
        public override void Execute(Cpu cpu) {
            cpu.AddrPins = cpu.PC;
            cpu.P |= (uint) StatusFlagsMask.D;
            cpu.SYNC = true;
        }
    }
    public class SEI : Instruction {
        public override void Execute(Cpu cpu) {
            cpu.AddrPins = cpu.PC;
            cpu.P |= (uint) StatusFlagsMask.I;
            cpu.SYNC = true;
        }
    }
    public class ORA : Instruction {

        public override void Execute(Cpu cpu) {
            cpu.A |= cpu.DataPins;
            cpu.setNZ(cpu.A);
            cpu.AddrPins = cpu.PC;
            cpu.SYNC = true;
        }
    }

    public class AND : Instruction {

        public override void Execute(Cpu cpu) {
            cpu.A &= cpu.DataPins;
            cpu.setNZ(cpu.A);
            cpu.AddrPins = cpu.PC;
            cpu.SYNC = true;
        }
    }

    public class ASL : Instruction {

        public override void Execute(Cpu cpu) {
            if ( AddressingMode == addressingModes.ACCUMULATOR) {
                uint tmp = (cpu.A << 1) & 0xFF;
                if ( (cpu.A & (uint) StatusFlagsMask.N) != 0) {
                    cpu.P |= (uint) StatusFlagsMask.C;
                } else {
                    cpu.P &= (uint) ~StatusFlagsMask.C;
                }
                cpu.A = tmp;
                cpu.setNZ(cpu.A);
                cpu.AddrPins = cpu.PC;
                cpu.SYNC = true;
                
            } else {
                uint tmp = (cpu.DataPins << 1) & 0xFF;
                if ( (cpu.DataPins & (uint) StatusFlagsMask.N) != 0) {
                    cpu.P |= (uint) StatusFlagsMask.C;
                } else {
                    cpu.P &= (uint) ~StatusFlagsMask.C;
                }
                cpu.DataPins = tmp;
                cpu.setNZ(tmp);
                cpu.RW = false;
            }
        }
    }

    public class LSR : Instruction {

        public override void Execute(Cpu cpu) {
            if ( AddressingMode == addressingModes.ACCUMULATOR) {
                uint tmp = (cpu.A >> 1) & 0xFF;
                if ( (cpu.A & 0x01) != 0) {
                    cpu.P |= (uint) StatusFlagsMask.C;
                } else {
                    cpu.P &= (uint) ~StatusFlagsMask.C;
                }
                cpu.A = tmp;
                cpu.setNZ(cpu.A);
                cpu.AddrPins = cpu.PC;
                cpu.SYNC = true;
                
            } else {
                uint tmp = (cpu.DataPins >> 1) & 0xFF;
                if ( (cpu.DataPins & 0x01) != 0) {
                    cpu.P |= (uint) StatusFlagsMask.C;
                } else {
                    cpu.P &= (uint) ~StatusFlagsMask.C;
                }
                cpu.DataPins = tmp;
                cpu.setNZ(tmp);
                cpu.RW = false;
            }
        }
    }

    public class ROL : Instruction {

        public override void Execute(Cpu cpu) {
            if ( AddressingMode == addressingModes.ACCUMULATOR) {
                uint tmp = ( (cpu.A << 1) & 0xFF )| (cpu.P & (uint) StatusFlagsMask.C);
                if ( (cpu.A & 0x80) != 0) {
                    cpu.P |= (uint) StatusFlagsMask.C;
                } else {
                    cpu.P &= (uint) ~StatusFlagsMask.C;
                }
                cpu.A = tmp;
                cpu.setNZ(cpu.A);
                cpu.AddrPins = cpu.PC;
                cpu.SYNC = true;
                
            } else {
                uint tmp = ( (cpu.DataPins << 1) & 0xFF )| (cpu.P & (uint) StatusFlagsMask.C);
                if ( (cpu.DataPins & 0x80) != 0) {
                    cpu.P |= (uint) StatusFlagsMask.C;
                } else {
                    cpu.P &= (uint) ~StatusFlagsMask.C;
                }
                cpu.DataPins = tmp;
                cpu.setNZ(tmp);
                cpu.RW = false;
            }
        }
    }

    public class ROR : Instruction {

        public override void Execute(Cpu cpu) {
            if ( AddressingMode == addressingModes.ACCUMULATOR) {
                uint tmp = ( (cpu.A >> 1) & 0xFF )| ( (cpu.P & (uint) StatusFlagsMask.C) << 7);
                cpu.P = (cpu.P & (uint) ~StatusFlagsMask.C ) | (cpu.A & 0x01);
                cpu.A = tmp;
                cpu.setNZ(cpu.A);
                cpu.AddrPins = cpu.PC;
                cpu.SYNC = true;
                
            } else {
                uint tmp = ( (cpu.DataPins >> 1) & 0xFF )| ( (cpu.P & (uint) StatusFlagsMask.C) << 7);
                cpu.P = (cpu.P & (uint) ~StatusFlagsMask.C ) | (cpu.DataPins & 0x01);
                cpu.DataPins = tmp;
                cpu.setNZ(tmp);
                cpu.RW = false;
            }
        }
    }


    public class BIT : Instruction {

        public override void Execute(Cpu cpu) {
            uint tmp = cpu.A & cpu.DataPins;
            cpu.setNZ(tmp);
            // Set V flag to valud of bit 6 of tested address
            cpu.P = (cpu.P & (uint) ~StatusFlagsMask.V) | ( cpu.DataPins & (uint) StatusFlagsMask.V ) ;
            // Set N flag to valud of bit 6 of tested address
            cpu.P = (cpu.P & (uint) ~StatusFlagsMask.N ) | ( cpu.DataPins & (uint) StatusFlagsMask.N ) ;

            cpu.AddrPins = cpu.PC;
            cpu.SYNC = true;
        }
    }

    public class JMP : Instruction {
        public override void Execute(Cpu cpu)
        {
                cpu.AD = cpu.DataPins <<8 | cpu.AD;
                cpu.PC = cpu.AD;
                cpu.AddrPins = cpu.AD;
                cpu.SYNC = true; 
        }
    }
 
    public class EOR : Instruction {

        public override void Execute(Cpu cpu) {
            cpu.A ^= cpu.DataPins;
            cpu.setNZ(cpu.A);
            cpu.AddrPins = cpu.PC;
            cpu.SYNC = true;
        }
    }
 
    public class DEC : Instruction {
        public override void Execute(Cpu cpu) {
            cpu.DataPins = (cpu.DataPins - 1) & 0xFF;
            cpu.setNZ(cpu.DataPins);
            cpu.RW = false;
        }
    }

    public class INC : Instruction {
        public override void Execute(Cpu cpu) {
            cpu.DataPins = (cpu.DataPins + 1) & 0xFF;
            cpu.setNZ(cpu.DataPins);
            cpu.RW = false;
        }
    }

    public class ADC : Instruction {
        public override void Execute(Cpu cpu)
        {
            // TODO: Implemend Decimal Mode
            uint sum = cpu.DataPins + cpu.A + (cpu.P & (uint) StatusFlagsMask.C);
            // If M and A are same sign but sum is different sign, set the overflowe
            if ( ((~(cpu.DataPins ^ cpu.A) & 0xFF ) & (sum ^ cpu.A) & 0x80 ) == 0 ) {
                cpu.P &= (uint) ~StatusFlagsMask.V;
            } else {
                cpu.P |= (uint) StatusFlagsMask.V;
            }
            cpu.A = sum & 0xFF;
            cpu.setNZ(cpu.A);
            //Set Carry
            cpu.P = ( cpu.P & (uint) ~StatusFlagsMask.C) | ( sum >> 8 );
            cpu.AddrPins = cpu.PC;
            cpu.SYNC = true;
            
        }
    }

     public class SBC : Instruction {
        public override void Execute(Cpu cpu)
        {
            // TODO: Implemend Decimal Mode
            // Do same as ADC but with ~cpu.DataPins
            uint sum = (0xFF-cpu.DataPins) + cpu.A + (cpu.P & (uint) StatusFlagsMask.C);
            // If M and A are same sign but sum is different sign, set the overflowe
            if ( ((~(( 0xFF -cpu.DataPins) ^ cpu.A) & 0xFF ) & (sum ^ cpu.A) & 0x80 ) == 0 ) {
                cpu.P &= (uint) ~StatusFlagsMask.V;
            } else {
                cpu.P |= (uint) StatusFlagsMask.V;
            }
            cpu.A = sum & 0xFF;
            cpu.setNZ(cpu.A);
            //Set Carry
            cpu.P = ( cpu.P & (uint) ~StatusFlagsMask.C) | ( sum >> 8 );
            cpu.AddrPins = cpu.PC;
            cpu.SYNC = true;

        }
    }
    public class CMP : Instruction {
        public override void Execute(Cpu cpu) {
            uint diff = (cpu.A - cpu.DataPins) & 0xFF;
            cpu.setNZ(diff);
            if ( cpu.A >= cpu.DataPins) {
                cpu.P |= (uint) StatusFlagsMask.C;
            } else {
                cpu.P &= (uint) ~StatusFlagsMask.C;
            }
            cpu.AddrPins = cpu.PC;
            cpu.SYNC = true;
        }
    }

    public class CPY : Instruction {
        public override void Execute(Cpu cpu) {
            uint diff = (cpu.Y - cpu.DataPins) & 0xFF;
            cpu.setNZ(diff);
            if ( cpu.Y >= cpu.DataPins) {
                cpu.P |= (uint) StatusFlagsMask.C;
            } else {
                cpu.P &= (uint) ~StatusFlagsMask.C;
            }
            cpu.AddrPins = cpu.PC;
            cpu.SYNC = true;
        }
    }

   public class CPX : Instruction {
        public override void Execute(Cpu cpu) {
            uint diff = (cpu.X - cpu.DataPins) & 0xFF;
            cpu.setNZ(diff);
            if ( cpu.X >= cpu.DataPins) {
                cpu.P |= (uint) StatusFlagsMask.C;
            } else {
                cpu.P &= (uint) ~StatusFlagsMask.C;
            }
            cpu.AddrPins = cpu.PC;
            cpu.SYNC = true;
        }
    }

} 