namespace CIA6526;
public class CIA6526
{
	// http://archive.6502.org/datasheets/mos_6526_cia_recreated.pdf

	public bool TOD { get;set;}
	// PHY2 Clock
	public bool PHI2 { get;set;}
	public bool CNT { get;set;}
	// Chip Select
	public bool CS { get;set;}
	// Rewa/Write
	public bool RW { get;set;}
	// PC
	public bool PC { get;set;}


	public uint PortA { get; set; }
	public uint PortB { get; set; }

	public uint DataPins { get; set;}

	public uint RS { get; set; }



	// Registers
	//PERIPHERAL DATA REGISTER A
	public uint PRA { get;set;}
	//PERIPHERAL DATA REGISTER B
	public uint PRB { get;set;}
	// DATA DIRECTION REG A
	public uint DDRA { get;set;}
	// DATA DIRECTION REG B
	public uint DDRB { get;set;}
	// TIMER A LOW
	public uint TALO { get;set;}
	// TIMER A HIGH
	public uint TAHI { get;set;}
	// TIMER B LOW
	public uint TBLO { get;set;}
	// TIMER B HIGH 
	public uint TBHI { get;set;}
	// TOD 10TH seconds
	public uint TOD10TH { get;set;}
	// TOD Seconds
	public uint TODSEC { get;set;}
	// TOD Minutes
	public uint TODMIN { get;set;}
	// TOD HOURS AM/PM
	public uint TODHR { get;set;}
	// SERIAL DATA REGISTER
	public uint SDR { get;set;}
	// INTERRUPT CONTROL REGISTER
	public uint ICR { get;set;}
	// CONTROL REG A
	public uint CRA { get;set;}
	// CONTROL REG B
	public uint CRB { get;set;}

	private uint _readRegister() {
		// return register value based on RS flags
		switch(RS) {
			case 0x0: {
				return PRA;	
			}

			case 0x1: {
				return PRB;
			}

			case 0x2: {
				return DDRA;
			}

			case 0x3: {
				return DDRB;
			}

			case 0x4: {
				return TALO;
			}

			case 0x5: {
				return TAHI;
			}

			case 0x6: {
				return TBLO;
			}

			case 0x7: {
				return TBHI;
			}

			case 0x8: {
				return TOD10TH;
			}

			case 0x9: {
				return TODSEC;
			}

			case 0xA: {
				return TODMIN;
			}

			case 0xB: {
				return TODHR;
			}

			case 0xC: {
				return SDR;
			}

			case 0xD: {
				return ICR;
			}

			case 0xE: {
				return CRA;
			}

			case 0xF: {
				return CRB;
			}

			default: {
				// TODO: Assert. Should not happen
				throw new ArgumentException("CSR must be between 0 and 15");
				//return 0xFFFF;
			}


		}
	}

	private void _writeRegister(uint value) {
		// return register value based on RS flags
		switch(RS) {
			case 0x0: {
				PRA = value;	
				break;
			}

			case 0x1: {
				PRB = value;
				break;
			}

			case 0x2: {
				DDRA = value;
				break;
			}

			case 0x3: {
				DDRB = value;
				break;
			}

			case 0x4: {
				TALO = value;
				break;
			}

			case 0x5: {
				TAHI = value;
				break;
			}

			case 0x6: {
				TBLO = value;
				break;
			}

			case 0x7: {
				TBHI = value;
				break;
			}

			case 0x8: {
				TOD10TH = value;
				break;
			}

			case 0x9: {
				TODSEC = value;
				break;
			}

			case 0xA: {
				TODMIN = value;
				break;
			}

			case 0xB: {
				TODHR = value;
				break;
			}

			case 0xC: {
				SDR = value;
				break;
			}

			case 0xD: {
				ICR = value;
				break;
			}

			case 0xE: {
				CRA = value;
				break;
			}

			case 0xF: {
				CRB = value;
				break;
			}

			default: {
				// TODO: Assert. Should not happen
				//throw new ArgumentException("CSR must be between 0 and 15");
				//return 0xFFFF;
				break;
			}


		}

	}

	public void Tick()
	{

	}

}
