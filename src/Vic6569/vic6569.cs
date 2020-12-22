
using System;

namespace VIC6569
{
    public class VicRegister {
        public string Name { get; set; }
        public uint Address { get; set; }
        public uint Value { get; set;}
        public void WriteToMem(in C6502.Memory mem) {
            mem.Write(Address,Value);
        }

        public uint ReadFromMem(in C6502.Memory mem) {
            return mem.Read(Address);
        }

    }
    public class Chip
    {


        public C6502.Memory Mem;
        public uint AddrPins {get; set;}

        public uint DataPins {get; set;}

        public bool IRQ { get; set;}
        public bool BA { get; set;}
        public bool AEC {get; set;}
        public bool LP { get; set;}
        public bool PHYIN { get; set;}
        public bool PHY0 {get; set;}

        private uint[] _registers = new uint[47];

        private uint _bank;


        // Registers

        public uint M0X {
            get => Mem.Read(0xD000);
            set => Mem.Write(0xD0000,value);
        }

        public uint M0Y {
            get => Mem.Read(0xD001);
            set => Mem.Write(0xD0001,value);
        }

        public uint M1X {
            get => Mem.Read(0xD002);
            set => Mem.Write(0xD0002,value);
        }

        public uint M1Y {
            get => Mem.Read(0xD003);
            set => Mem.Write(0xD0003,value);
        }
        public uint M2X {
            get => Mem.Read(0xD004);
            set => Mem.Write(0xD0004,value);
        }
        public uint M2Y {
            get => Mem.Read(0xD005);
            set => Mem.Write(0xD0005,value);
        }

        public uint M3X {
            get => Mem.Read(0xD006);
            set => Mem.Write(0xD0006,value);
        }
        public uint M3Y {
            get => Mem.Read(0xD007);
            set => Mem.Write(0xD007,value);
        }

        public uint M4X {
            get => Mem.Read(0xD008);
            set => Mem.Write(0xD008,value);
        }
        public uint M4Y {
            get => Mem.Read(0xD009);
            set => Mem.Write(0xD009,value);
        }

        public uint M5X {
            get => Mem.Read(0xD00A);
            set => Mem.Write(0xD00A,value);
        }
        public uint M5Y {
            get => Mem.Read(0xD00B);
            set => Mem.Write(0xD00B,value);
        }

        public uint M6X {
            get => Mem.Read(0xD00C);
            set => Mem.Write(0xD00C,value);
        }
        public uint M6Y {
            get => Mem.Read(0xD00D);
            set => Mem.Write(0xD00D,value);
        }

        public uint M7X {
            get => Mem.Read(0xD00E);
            set => Mem.Write(0xD00E,value);
        }
        public uint M7Y  {
            get => Mem.Read(0xD00F);
            set => Mem.Write(0xD00F,value);
        }

        public uint MSBX {
            get => Mem.Read(0xD010);
            set => Mem.Write(0xD010,value);
        }
        public uint CR1 {
            get => Mem.Read(0xD011);
            set => Mem.Write(0xD011,value);
        }

        // Control Register 1
        //           8|   7|   6|   5|   4|       0|
        // $d011 |RST8| ECM| BMM| DEN|RSEL| YSCROLL|
        public uint RASTER {
            get => Mem.Read(0xD012);
            set => Mem.Write(0xD012,value);
        }
        public uint LPX {
            get => Mem.Read(0xD013);
            set => Mem.Write(0xD013,value);
        }
        public uint LPY {
            get => Mem.Read(0xD014);
            set => Mem.Write(0xD014,value);
        }

        // Sprite Enable
        public uint SE {
            get => Mem.Read(0xD015);
            set => Mem.Write(0xD015,value);
        }
        // Control Register 2
        public uint CR2 {
            get => Mem.Read(0xD016);
            set => Mem.Write(0xD016,value);
        }
        // SPrite Y Expansion
        public uint SPYE {
            get => Mem.Read(0xD017);
            set => Mem.Write(0xD017,value);
        }
        // Memory pointers
        public uint MP {
            get => Mem.Read(0xD018);
            set => Mem.Write(0xD018,value);
        }
        // Interrupt register
        public uint INTR {
            get => Mem.Read(0xD019);
            set => Mem.Write(0xD019,value);
        }
        // Interupt Enabled
        public uint INTE {
            get => Mem.Read(0xD01A);
            set => Mem.Write(0xD01A,value);
        }
        // Sprite Data Priority
        public uint SPDP {
            get => Mem.Read(0xD01B);
            set => Mem.Write(0xD01B,value);
        }
        // Sprite Multicolor
        public uint SPMC {
            get => Mem.Read(0xD001C);
            set => Mem.Write(0xD01c,value);
        }
        // SPrite X Expansion
        public uint SPXE {
            get => Mem.Read(0xD01D);
            set => Mem.Write(0xD01D,value);
        }
        // Sprite-Sprite collision
        public uint SPSPC {
            get => Mem.Read(0xD01E);
            set => Mem.Write(0xD01E,value);
        }
        // Sprite-Data collision
        public uint SPDTC {
            get => Mem.Read(0xD01F);
            set => Mem.Write(0xD01F,value);
        }
        // Border Color
        public uint EC {
            get => Mem.Read(0xD020);
            set => Mem.Write(0xD020,value);
        }
        // Background colors
        public uint B0C {
            get => Mem.Read(0xD021);
            set => Mem.Write(0xD021,value);
        }
        public uint B1C {
            get => Mem.Read(0xD022);
            set => Mem.Write(0xD022,value);
        }
        public uint B2C {
            get => Mem.Read(0xD023);
            set => Mem.Write(0xD023,value);
        }
        public uint B3C {
            get => Mem.Read(0xD024);
            set => Mem.Write(0xD024,value);
        }
        
        // Sprite Multicolor
        public uint MM0 {
            get => Mem.Read(0xD025);
            set => Mem.Write(0xD025,value);
        }
        public uint MM1 {
            get => Mem.Read(0xD026);
            set => Mem.Write(0xD026,value);
        }
        // Sprite color
        public uint M0C {
            get => Mem.Read(0xD027);
            set => Mem.Write(0xD027,value);
        }

        public uint M1C {
            get => Mem.Read(0xD028);
            set => Mem.Write(0xD028,value);
        }

        public uint M2C {
            get => Mem.Read(0xD029);
            set => Mem.Write(0xD029,value);
        }

        public uint M3C {
            get => Mem.Read(0xD02A);
            set => Mem.Write(0xD02A,value);
        }

        public uint M4C {
            get => Mem.Read(0xD02B);
            set => Mem.Write(0xD02B,value);
        }

        public uint M5C {
            get => Mem.Read(0xD02C);
            set => Mem.Write(0xD02C,value);
        }

        public uint M6C {
            get => Mem.Read(0xD02D);
            set => Mem.Write(0xD02D,value);
        }

        public uint M7C {
            get => Mem.Read(0xD02E);
            set => Mem.Write(0xD02E,value);
        }

        public void SetRegistersFromMem(C6502.Memory mem) {

            for (uint i = 0; i<47; i++) {
                _registers[i] = mem.Read(0xD000+i);
            }
        }
        public void WriteRegistersToMem(C6502.Memory mem) {
            for (uint i = 0; i<47; i++) {
                mem.Write(0xD000+i, _registers[i]);
            }
        }

        // Video Matrix Counter
        private uint _VC { get; set;}
        private uint _VCBASE { get; set;}
        private uint _VMLI { get; set;}
        // Row Counter
        private uint _RC { get; set;}
        // MOB Data Counter
        private uint _MC { get;set;} 

        private bool _badLine;
        private uint _displayWindowsHeight;
        private uint _displayWindowsWidth;
        private uint _firstLine;
        private uint _lastLine;
        private uint _firstXCoord;
        private uint _lastXCoord;

        //          | Video  | # of  | Visible | Cycles/ |  Visible
        //   Type   | system | lines |  lines  |  line   | pixels/line
        // ---------+--------+-------+---------+---------+------------
        // 6567R56A | NTSC-M |  262  |   234   |   64    |    411
        //  6567R8  | NTSC-M |  263  |   235   |   65    |    418
        //   6569   |  PAL-B |  312  |   284   |   63    |    403
        private uint _numberLines = 312;
        private uint _visibleLines = 284;
        private uint _cyclesPerLines = 63;
        private uint _visiblePixelPerLines = 403;

                // Screen array or RGBA => 4 bytes per pixel
        public byte[] Screen;

        //          | First  |  Last  |              |   First    |   Last
        //          | vblank | vblank | First X coo. |  visible   |  visible
        //   Type   |  line  |  line  |  of a line   |   X coo.   |   X coo.
        // ---------+--------+--------+--------------+------------+-----------
        // 6567R56A |   13   |   40   |  412 ($19c)  | 488 ($1e8) | 388 ($184)
        //  6567R8  |   13   |   40   |  412 ($19c)  | 489 ($1e9) | 396 ($18c)
        //   6569   |  300   |   15   |  404 ($194)  | 480 ($1e0) | 380 ($17c)
        private uint _firstVblankLine = 300;
        private uint _lastVblankLine = 15;
        private uint _firstXCoordLine = 0x194;
        private uint _firstVisibleXCoord = 0x1E0;
        private uint _lastVisibleXCoord = 0x17C;

        private bool _mainBorderFlipFlop;
        private bool _verticalBorderFlipFlop;
        
        //TODO: Confir if this is how X position
        // is handled
        private uint _x;
        private uint _y;

        // For DRAM access
        private uint _ref=0xFF;
        private bool _displayState;

        public uint[] VideoMatrixLine = new uint[40];

        public uint _opcycle;

        private void Init() {
            _opcycle = 1;
            _y = 0;
            RASTER = 0;
            // Set RST8 to 0
            CR1 &= ( ~0x80 & 0xFF);
            _x = _firstXCoord;
            // Set DEN
            CR1 |= 0x10;
            PHY0 = false;
            MP = 0x14;
            Screen = new byte[_visiblePixelPerLines*_numberLines*4];
        }

        private void _paccess(uint sprite){ }
        private void _caccess() {
            // TODO create flags enum
            // EMM/BMM/MCM = 0/0/0
            // Addresses
            // +----+----+----+----+----+----+----+----+----+----+----+----+----+----+
            // | 13 | 12 | 11 | 10 |  9 |  8 |  7 |  6 |  5 |  4 |  3 |  2 |  1 |  0 |
            // +----+----+----+----+----+----+----+----+----+----+----+----+----+----+
            // |VM13|VM12|VM11|VM10| VC9| VC8| VC7| VC6| VC5| VC4| VC3| VC2| VC1| VC0|
            // +----+----+----+----+----+----+----+----+----+----+----+----+----+----+
            if ( ((CR1 & ( 0x40 | 0x20 ))  == 0 ) && ((CR2 & 0x10) == 0 )) {
                AddrPins = (MP & 0xF0) <<6 | _VC;
                DataPins = _fetchMemory(AddrPins);
                VideoMatrixLine[_VMLI] = DataPins;
            }


        }
        private void _gaccess(){

            if (_displayState) {
                AddrPins = ((MP & 0b0000_1110) << 10 ) | (VideoMatrixLine[_VMLI] << 3) | _RC;
                // TODO: Set Datapins by reading memory
                DataPins = _fetchMemory(AddrPins);
                Console.WriteLine("{0,3:X2}",DataPins);
                _VC = (_VC+1) & 0x3FF;
                if ( _VC >= 1000) {
                    //
                }
                _VMLI++;
            } else {
                // TODO handle ECM and read 0x39FF
                AddrPins = 0x3FFF;
            }
        }
        private void _saccess(){ }
        private void _dramRefresh() {
            AddrPins = 0x3F00 | _ref;
            _ref = (_ref-1) & 0xFF;

        }
        private void _idleAccess(){ }

        public Chip(uint bank,C6502.Memory mem) {
            Mem = mem;
            _bank = bank;
            Init();

        }

        private uint _fetchMemory(uint addr) { 
            if ( (_bank == 0 || _bank == 2) && (addr & 0x1000) == 0x1000 ) {
                // When vic reads from 0x1000-0x1fff (from vicii pint of vie
                // read characted ROM)
                return Mem.ReadCharRom(addr&0x0FFF);
            } else {
                return Mem.Read((_bank<< 14) | addr);
            }
        }

        public void GraphicsDataSequencer() {
        
            // Only draw if flipflop not set
            // output background color otherwise
            if (!_verticalBorderFlipFlop) {

            } else {

            }
            
        }

        public void Tick() {

            // RSEL|  Display window height   | First line  | Last line
            // ----+--------------------------+-------------+----------
            //   0 | 24 text lines/192 pixels |   55 ($37)  | 246 ($f6)
            //   1 | 25 text lines/200 pixels |   51 ($33)  | 250 ($fa)
            _displayWindowsHeight = (CR1 & 0x08) == 0 ? 24 : 25;
            _firstLine = (CR1 & 0x08) == 0 ? 0x37 : 0x33;
            _lastLine = (CR1 & 0x08) == 0 ? 0xf6 : 0xfa;

            // CSEL|   Display window width   | First X coo. | Last X coo.
            //  ----+--------------------------+--------------+------------
            //    0 | 38 characters/304 pixels |   31 ($1f)   |  334 ($14e)
            //    1 | 40 characters/320 pixels |   24 ($18)   |  343 ($157)
            _displayWindowsWidth = (CR2 & 0x08) == 0 ? 38 : 40;
            _firstXCoord = (CR2 & 0x08) == 0 ? 0x1F : 0x18;
            _lastXCoord = (CR2 & 0x08) == 0 ? 0x14E : 0x157;

            // Bad Line
            _badLine = ( ( RASTER >= 0x30 && RASTER <= 0xF7) && 
                ( (RASTER & 0x07) == (CR1 & 0x07) ) &&
                ( (CR1 & 0x10) == 0x10));
            if (_badLine) {
                _displayState = true;
            }

            if (_opcycle == _cyclesPerLines+1) {
                    _opcycle = 1;
            }

            //Console.WriteLine("{0,4} {1,2} {2,3}",PHY0, _opcycle,_VMLI);

            AEC = PHY0;

            if (_opcycle >= 15 && _opcycle <= 54 ) {
                AEC = ! _badLine;
           } else {
                AEC = PHY0;
            }

            if (_opcycle >= 16 && _opcycle <= 54 ) {
                if (!PHY0) {
                    _gaccess();    
                } else {
                    _caccess();
                }
            }
             switch(_opcycle) {
                case 1: {
                    _y++;
                    if ( _y == _numberLines) {
                        _y = 0;
                        _x = _firstXCoord;
                        _ref = 0xFF;
                        _VCBASE = 0;
                    }

                    // Compare Raster line with $d012/$d011(bit 7)
                    // if line is not 0
                    // if same, IRQ should be set and RST set as IRQ reason
                    if ( (_y != 0 ) && _y == (RASTER | ((CR1 & 0x80) <<1))) {
                        INTR |= 0x01;
                        // toDO: Should we also set INTE

                    }
                    RASTER = _y & 0xFF;
                    CR1 = (CR1 & (~0x80 & 0xFF)) |  ( (_y & 0x100) >> 1);

                    // pAccess(3)
                    _paccess(3);
                    BA = true;


                    break;
                }
                case 2: {

                    // Compare Raster line with $d012/$d011(bit 7)
                    // if line is not 0
                    // if same, IRQ should be set and RST set as IRQ reason
                    if ( (_y == 0 ) && _y == (RASTER | ((CR1 & 0x80) <<1))) {
                        INTR |= 0x01;

                    }
                    _idleAccess();
                    break;
                }

                case 3: {
                    _paccess(4);
                    break;
                }

                case 5: {
                    _paccess(5);
                    break;
                }
                case 7: {
                    _paccess(6);
                    break;
                }
                case 9: {
                    _paccess(7);
                    break;
                }

                case 4:
                case 6:
                case 8:
                case 10: {
                    _idleAccess();
                    break;
                }

                case 11: {
                    _dramRefresh();
                    break;
                }
                case 12: {
                    if (!PHY0) {
                        BA = ! _badLine;
                    }
                    break;
                }

                case 13:
                    if (!PHY0) {
                        _dramRefresh();
                    }
                    break;
                case 14:
                    if (!PHY0) {
                        _VC = _VCBASE;
                        _VMLI = 0;
                        if (_badLine) {_RC = 0;}
                        _dramRefresh();
                    }
                    break;
                case 15: {
                    if (!PHY0) {
                        _dramRefresh();

                    } else {
                        _caccess();
                    }

                    break;
                }
                case 16:
                case 54: {
                    break;
                }


                case 55: {
                    BA = true;
                    _gaccess();
                    break;
                }

                case 56:
                case 57:
                case 61: {
                    _idleAccess();
                    break;
                }

                case 58: {
                    if (_RC == 7 ) {
                        _VCBASE = (_VC & 0x3FF);
                        if ( !_badLine) {
                            _displayState = false;
                        }
                    }
                    if (_displayState) {
                        _RC = (_RC+1) &0x07;
                    }
                    _paccess(0);
                    break;
                }
                case 59: {
                    _idleAccess();
                    break;
                }
                case 60: {
                    _paccess(1);
                    break;
                }
                case 62: {
                    _paccess(2);
                    break;
                }

                case 63: {
                    _idleAccess();
                    if (_y == _lastLine) {
                        _verticalBorderFlipFlop = true;
                    }
                    if ( (_y == _firstLine) && ( (CR1 & 0x10) == 0x10 ) ) {
                        _verticalBorderFlipFlop = false;
                    }
                    break;
                }

            }


            for (var px = 0; px <8; px++) {
                _x++;

                if ( _x == _lastXCoord) {
                    _mainBorderFlipFlop = true;
                } 
                if ( _x == _firstXCoord && _y == _lastLine ) {
                    _verticalBorderFlipFlop = true;
                }

                if ( _x == _firstXCoord && _y == _firstLine && ( (CR1 & 0x10) == 0x10 ) ) {
                    _verticalBorderFlipFlop = false;
                }

                if (_x == _firstXCoord && !_verticalBorderFlipFlop) {
                    _mainBorderFlipFlop = false;
                }

                if ( _x == 0x1f8) {
                    _x = 0;
                }
            }



        }
    }
}
