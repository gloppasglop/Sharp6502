using System;
using System.IO;
namespace CIA6526;
public class Chip
{
	// http://archive.6502.org/datasheets/mos_6526_cia_recreated.pdf
	public bool TOD { get;set;}
	// PHY2 Clock
	public bool PHI2 { get;set;}
	private bool _CNT;
	private bool _CNT_POSITIVE_TRANSITION = false;
	public bool CNT {
		get {
			return _CNT;
		}
		set {
			// Track positive CNT transition
			_CNT_POSITIVE_TRANSITION = (value) && (! _CNT);
			_CNT = value;
		}
	}
	// Chip Select
	public bool CS { get;set;}
	// Rewa/Write
	public bool RW { get;set;}
	// PC
	public bool PC { get;set;}

	public bool RES { get; set; }

	public bool IRQ { get; set;}


	public uint PortA { get; set; }
	public uint PortB { get; set; }

	public uint DataPins { get; set;}

	public uint RS { get; set; }



	// Registers
	//PERIPHERAL DATA REGISTER A

	
	public uint PRA {
		get {
			// Returns what is on PortA whatever the value of DDRA
			return PortA;
		}
		set {
			// Only write bits if corresponding DDRA bit is set
			PortA = value | (PortA & (~DDRA & 0xFF));
		}
	}
	//PERIPHERAL DATA REGISTER B
	public uint PRB {
		get {
			// Returns what is on PortA whatever the value of DDRA
			return PortB;
		}
		set {
			// Only write bits if corresponding DDRA bit is set
			PortB = value | (PortB & (~DDRB & 0xFF));
		}
	}
	// DATA DIRECTION REG A
	public uint DDRA { get; set; }
	// DATA DIRECTION REG B
	public uint DDRB { get; set; }
	// TIMER A LOW
	private uint _TALOLATCH;
	private uint _TALO;
	public uint TALO { 
		get
		{
			return _TALO;
		} 
		set
		{
			_TALO = value;
			_TALOLATCH = value;
		}
	}
	// TIMER A HIGH
	private uint _TAHILATCH;
	private uint _TAHI;
	public uint TAHI { 
		get
		{
			return _TAHI;
		} 
		set
		{
			_TAHI = value;
			_TAHILATCH = value;
		}
	}
	// TIMER B LOW
	private uint _TBLO;
	private uint _TBLOLATCH;
	public uint TBLO {
		get
		{
			return _TBLO;
		}
		set
		{
			_TBLO = value;
			_TBLOLATCH = value;
		}
	}
	// TIMER B HIGH 
	private uint _TBHI;
	private uint _TBHILATCH;
	public uint TBHI {
		get
		{
			return _TBHI;
		}
		set
		{
			_TBHI = value;
			_TBHILATCH = value;
		}
	}
	// Read:
	//Bit 0..3: Tenth seconds in BCD-format ($0-$9)
	//Bit 4..7: always 0
	//Writing:
	//Bit 0..3: if CRB-Bit7=0: Set the tenth seconds in BCD-format
	//Bit 0..3: if CRB-Bit7=1: Set the tenth seconds of the alarm time in BCD-format
	public uint TOD10TH { get; set; }

	// TOD Seconds
	//Bit 0..3: Single seconds in BCD-format ($0-$9)
	//Bit 4..6: Ten seconds in BCD-format ($0-$5)
	//Bit 7: always 0
	public uint TODSEC { get; set; }

	// TOD Minutes
	//Bit 0..3: Single minutes in BCD-format ($0-$9)
	//Bit 4..6: Ten minutes in BCD-format ($0-$5)
	//Bit 7: always 0
	public uint TODMIN { get; set; }

	// TOD HOURS AM/PM
	// Bit 0..3: Single hours in BCD-format ($0-$9)
	// Bit 4..6: Ten hours in BCD-format ($0-$5)
	// Bit 7: Differentiation AM/PM, 0=AM, 1=PM
	// Writing into this register stops TOD, until register 8 (TOD 10THS) will be read.
	public uint TODHR { get; set; }

	// SERIAL DATA REGISTER
	// The byte within this register will be shifted bitwise to or from the SP-pin with every positive slope at the CNT-pin.
	public uint SDR { get; set; }

	// INTERRUPT CONTROL REGISTER
	private uint _ICR;
	public uint ICR {
		get
		{

			// The interrupt DATA register is
			// cleared and the /IRQ line returns high following a
			// read of the DATA register
			var tmpICR = _ICR;
			// Not clear if we should reset the whole register to 0
			// or only bit 7 (IR bit)
			//_ICR = 0;
			_ICR &= 0b0111_1111;
			IRQ = false;
			
			return tmpICR;

		}
		set
		{
			// When writing to the MASK
			// register, if bit 7 (SET/CLEAR) of data written is a
			// ZERO, any mask bit written with a on will be
			// cleared, while those mask bits written with a zero
			// will be unaffected. If bit 7 of the data written is a
			// ONE, any mask bit written with a one will be set,
			// while those mask bits written with a zero will be
			// unaffected
			if ( (value & 0b1000_0000) == 0b1000_0000) {
				_ICR |= value;
				//PortA = value | (PortA & (~DDRA & 0xFF));
			} else {
				_ICR = _ICR & (~value & 0xFF);

			}
			//_ICR = value;
		}
	}

	// CONTROL REG A
	public uint CRA { get; set; }

	// CONTROL REG B
	public uint CRB { get; set; }

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

	public void Init() {
		// /RES - Reset Input
		// A low on the /RES pin resets all internal registers.
		// The port pins are set as inputs and port registers to
		// zero (although a read of the ports will return all high
		// because of passive pullups). The timer control
		// registers are set to zero and the timer latches to all
		// ones. All other registers are reset to zero.

		PRA = 0;
		PRB = 0;
		DDRA = 0;
		DDRB = 0;
		TALO = 0;
		_TALOLATCH = 0xFF;
		TAHI = 0;
		_TAHILATCH = 0xFF;
		TBHI = 0;
		_TBHILATCH = 0xFF;
		TBLO = 0;
		_TBLOLATCH = 0xFF;
		TOD10TH = 0;
		TODHR = 0;
		TODSEC = 0;
		TODMIN = 0;
		SDR = 0;
		ICR = 0;
		CRA = 0;
		CRB = 0;

	}
	// TODO:
	//	Input Mode:
	// 	  Control bits allow selection of the clock used to
	// 	  decrement the timer. TIMER A can count phi2 clock
	// 	  pulses or external pulses applied to the CNT pin.
	// 	  TIMER B can count phi2 pulses, external CNT
	// 	  pulses, TIMER A underflow pulses or TIMER A
	// 	  underflow pulses while the CNT pin is held high.
	//    	  The timer latch is loaded into the timer on any
	// 	  timer underflow, on a force load or following a write
	//   	  to the high byte of the prescaler while the timer is
	// 	  stopped. If the timer is running, a write to the high
	// 	  byte will load the timer latch, but not reload the
	// 	  counter.
	//	Handshaking
	//	Forceload
	//	PB ON/OFF
	//	Toggle/PULSE
	//	SDR
	//	TOD
	//	I/O Ports PRA/PRB/DDRA/DDRB
	public void Tick()
	{

		if ( PHI2) {
			// The /CS input controls the activity of the 6526. A
			// low level on /CS while phi2 is high causes the
			// device to respond to signals on the R/W and
			// address (RSx) lines. A high on /CS prevents these
			// lines from controlling the 6526. The /CS line is
			// normally activated (low)

			// The R/W signal is normally supplied by the
			// microprocessor and controls the direction of data &
			// transfers of the 6526. A high on R/W indicates a
			// read (data transfer out of the 6526), while a low B
			// indicates a write (data transfer into the 6526).
			if (RW & CS) {
				// READ (means we need to put data on data pins to be read)
				DataPins = _readRegister();	

			}
			if ((!RW) & CS) {
				// WRITE
				_writeRegister(DataPins);	

			}

		}
		
		// TIMER A Started
		// Only increment if INNMODE = 0 or (INNMODE=1 and positive CNT transition)
		// TODO: Shouldn't we delay by one PHY2!! 
		//       Because now time starts in the exact same cycle as when writing
		//       to the CRA register 
		if ( ( (CRA & 0b0000_0001) == 0b0000_0001) ) {
			if ( ((CRA & 0b0010_0000) == 0) || (((CRA & 0b0010_0000) == 0b0010_0000) && _CNT_POSITIVE_TRANSITION))
			{
				if (_TALO == 0) {
					if (_TAHI > 0) {
						_TALO = 0xFF;
						_TAHI -= 1;
					} else {
						// We reached 0

						// Reload with Latched value
						TALO = _TALOLATCH;
						TAHI = _TAHILATCH;
						// Console.WriteLine("BIPA");
						
						// Set the Interrup control register bit
						var tmpICR = _ICR;
						_ICR |= 0b0000_0001;
						if ((tmpICR & 0b0000_0001) == 0b0000_0001) {
							_ICR |= 0b1000_0000;	
							IRQ = true;
						}

						// If ONE SHOT Mode, stop the timer
						if (( (CRA & 0b0000_1000) == 0b0000_1000)) {
							Console.WriteLine("ONESHOT");
							CRA &= (~0b0000_0001u & 0xFF);
						}

					}
				} else {
					_TALO -= 1;
				}
			}

		}

		// PBON
		if (( (CRA & 0b0000_0010) == 0b0000_0010)) {
			Console.WriteLine("TIMERA PBON Not implemented");
		}
		if (( (CRB & 0b0000_0001) == 0b0000_0001)) {
			if ( ((CRB & 0b0010_0000) == 0) || (((CRB & 0b0010_0000) == 0b0010_0000) && _CNT_POSITIVE_TRANSITION))
			{
				if (_TBLO == 0) {
					if (_TBHI > 0) {
						_TBLO = 0xFF;
						_TBHI -= 1;
					} else {
						// We reached 0

						// Reload with Latched value
						TBLO = _TBLOLATCH;
						TBHI = _TBHILATCH;
						// Console.WriteLine("BIPB");
						
						// Set the Interrup control register bit
						var tmpICR = _ICR;
						_ICR |= 0b0000_0001;
						if ((tmpICR & 0b0000_0010) == 0b0000_0010) {
							_ICR |= 0b1000_0000;	
							IRQ = true;
						}
						// If ONE SHOT Mode, stop the timer
						if (( (CRB & 0b0000_1000) == 0b0000_1000)) {
							Console.WriteLine("ONESHOT");
							CRB &= ((uint) ~0b0000_0001u) & 0xFF;
						}

					}
				} else {
					_TBLO -= 1;
				}
			}
		}
		if (( (CRB & 0b0000_0010) == 0b0000_0010)) {
			Console.WriteLine("TIMERB PBON Not implemented");
		}

		if ( ! RES) {
		}

	}

}
