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

} 