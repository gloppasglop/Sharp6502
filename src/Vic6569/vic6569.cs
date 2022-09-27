
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
        // Control Register 1
        //           8|   7|   6|   5|   4|       0|
        // $d011 |RST8| ECM| BMM| DEN|RSEL| YSCROLL|
        public uint CR1 {
            get => Mem.Read(0xD011);
            set => Mem.Write(0xD011,value);
        }

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
        //           8|   7|   6|   5|   4|       0|
        // $d016 |  - |  - | RES| MCM|CSEL| XSCROLL|
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
        // $d018 |VM13|VM12|VM11|VM10|CB13|CB12|CB11|  - | Memory pointers
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
        public uint _VC { get; set;}
        public uint _VCBASE { get; set;}
        public uint _VMLI { get; set;}
        // Row Counter
        public uint _RC { get; set;}

        // MOB Data Counter
        private uint _MC { get;set;} 

        public bool _badLine;
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
        private uint _numberRows;
        private uint _visiblePixelPerLines = 403;

        // Screen array or RGB => 3 bytes per pixel
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
        public uint _x;
        public uint _y;
        private uint _sequencer;
        private uint _color;


        // For DRAM access
        private uint _ref=0xFF;
        public bool _displayState;

        public uint[] VideoMatrixLine = new uint[40];

        public uint _opcycle;

        private uint _graphicMode() {

	        return ((CR1 & ( 0x40 | 0x20 ) ) | ((CR2 & 0x10) ) ) >> 4;
        }

        private uint _cdata;


        private void Init() {
            _opcycle = 0;
            _y = 0;
            //_x = _firstXCoord;
            _x = _firstXCoordLine+4;
            RASTER = 0;
            // Initialise CR1
            CR1 = 0b0001_1000;
            // Initialise CR2
            CR2 = 0b1100_1000;
            PHY0 = false;
            MP = 0x14;
            _numberRows = _cyclesPerLines*8;
            Screen = new byte[4*_cyclesPerLines*8*_numberLines];
        }

        private void _paccess(uint sprite){ }
        private void _caccess() {
            // TODO create flags enum
            //  3.7.3.1. Standard text mode (ECM/BMM/MCM=0/0/0)
            // 3.7.3.2. Multicolor text mode (ECM/BMM/MCM=0/0/1)
            // 3.7.3.3. Standard bitmap mode (ECM/BMM/MCM=0/1/0)
            // 3.7.3.4. Multicolor bitmap mode (ECM/BMM/MCM=0/1/1)
            // 3.7.3.5. ECM text mode (ECM/BMM/MCM=1/0/0)
            // 3.7.3.6. Invalid text mode (ECM/BMM/MCM=1/0/1)
            // 3.7.3.7. Invalid bitmap mode 1 (ECM/BMM/MCM=1/1/0)
            // 3.7.3.8. Invalid bitmap mode 2 (ECM/BMM/MCM=1/1/1)

            // ---------------------------
            // CR1: §Control Register 1
            //           8|   7|   6|   5|   4|       0|
            // $d011 |RST8| ECM| BMM| DEN|RSEL| YSCROLL|
            // ---------------------------
            // Control Register 2
            //           8|   7|   6|   5|   4|       0|
            // $d016 |  - |  - | RES| MCM|CSEL| XSCROLL|
            // ---------------------------
            // Addresses
            // +----+----+----+----+----+----+----+----+----+----+----+----+----+----+
            // | 13 | 12 | 11 | 10 |  9 |  8 |  7 |  6 |  5 |  4 |  3 |  2 |  1 |  0 |
            // +----+----+----+----+----+----+----+----+----+----+----+----+----+----+
            // |VM13|VM12|VM11|VM10| VC9| VC8| VC7| VC6| VC5| VC4| VC3| VC2| VC1| VC0|
            // +----+----+----+----+----+----+----+----+----+----+----+----+----+----+

            switch(_graphicMode()) {
                // Standard Text Mode
                // EMM/BMM/MCM = 0/0/0
                case 0x0:
                    AddrPins = (MP & 0xF0) <<6 | _VC;
                    DataPins = _fetchMemory(AddrPins);
                    _color =  Mem.Read(0xD800 | _VC) & 0xF ;
                    VideoMatrixLine[_VMLI] = DataPins;
                    break;

                // Multicolor Text Mode
                // EMM/BMM/MCM = 0/0/1
                case 0x1:
                    AddrPins = (MP & 0xF0) <<6 | _VC;
                    DataPins = _fetchMemory(AddrPins);
                    _color =  Mem.Read(0xD800 | _VC) & 0xF;
                    VideoMatrixLine[_VMLI] = DataPins;
                    break;
                // Standard Bitmap Mode
                // EMM/BMM/MCM = 0/1/0
                case 0x2:
                    AddrPins = (MP & 0xF0) <<6 | _VC;
                    DataPins = _fetchMemory(AddrPins);
                    // Bits 8-11 read from character map in text modes
                    // but unused for bitmap mode
                    // Should we still read it or just put dummy value
                    _color =  Mem.Read(0xD800 | _VC) & 0xF ;
                    VideoMatrixLine[_VMLI] = DataPins;
                    break;
                // multicolor Bitmap Mode
                // EMM/BMM/MCM = 0/1/1
                case 0x2|0x1:
                    AddrPins = (MP & 0xF0) <<6 | _VC;
                    DataPins = _fetchMemory(AddrPins);
                    // Bits 8-11 read from character map in text modes
                    // but unused for bitmap mode
                    // Should we still read it or just put dummy value
                    _color =  Mem.Read(0xD800 | _VC) & 0xF ;
                    VideoMatrixLine[_VMLI] = DataPins;
                    break;


                default:
                    Console.WriteLine($"Unhandled mode: CR1: {CR1}, CR2: {CR2}");
                    break;
            } 

            if ( ! _displayState) {
                _color = 0;
                VideoMatrixLine[_VMLI] = 0;
            }
            


        }
        private void _gaccess(){

	    
            if (_displayState) {
                switch(_graphicMode()) {
                    case 0x0:
                        // Standard Text Mode
                    case 0x1:
                        // Multicolor Text Mode
                        AddrPins = ((MP & 0b0000_1110) << 10 ) | (VideoMatrixLine[_VMLI] << 3) | _RC;
                        DataPins = _fetchMemory(AddrPins) ;
                        _sequencer = DataPins;
                        break;
                    case 0x2:
                        // Standard bitmap Mode
                        AddrPins = ((MP & 0b0000_1000) << 10 ) | (_VC << 3) | _RC;
                        DataPins = _fetchMemory(AddrPins) ;
                        _sequencer = DataPins;
                        break;
                    case 0x2|0x1:
                        // Multicolor bitmap Mode
                        AddrPins = ((MP & 0b0000_1000) << 10 ) | (_VC << 3) | _RC;
                        DataPins = _fetchMemory(AddrPins) ;
                        _sequencer = DataPins;
                        break;
                    default:
                        Console.WriteLine($"_gaccess - Unhandled mode: CR1: {CR1}, CR2: {CR2}");
                        break;

                }
                _VC = (_VC+1) & 0x3FF;
                // TODO : Reset of _sequencer should depend on XSCROLL
                _VMLI++;
            } else {
                // The access is always to address
                // $3fff ($39ff when the ECM bit in register $d016 is set
                if ( (CR1 & 0x40) == 0) {
                    AddrPins = 0x3FFF;
                    DataPins = _fetchMemory(AddrPins) ;
                    _sequencer = DataPins;
                } else {
                    AddrPins = 0x39FF;
                    DataPins = _fetchMemory(AddrPins) ;
                    _sequencer = DataPins;
                }
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
                // When vic reads from 0x1000-0x1fff (from vicii pint of view
                // read characted ROM)
                return Mem.ReadCharRom(addr&0x0FFF);
            } else {
                return Mem.Read((_bank<< 14) | addr);
            }
        }

        private (byte,byte,byte)  _getColorPalette(uint  color) {

            switch(color) {
                case 0:
                    return (0x00,0x00,0x00);
                case 1:
                    return (0xFF, 0xFF, 0xFF);
                case 2:
                    return (0x68, 0x37, 0x2B );
                case 3:
                    return (0x70, 0xA4, 0xB2);
                case 4:
                    return (0x6F, 0x3D, 0x86);
                case 5:
                    return (0x58, 0x8D, 0x43);
                case 6:
                    return (0x35, 0x28, 0x79);
                case 7:
                    return (0xB8, 0xC7, 0x6F);
                case 8:
                    return (0x6F, 0x4F, 0x25);
                case 9:
                    return (0x43, 0x39, 0x00);
                case 10:
                    return (0x9A, 0x67, 0x59);
                case 11:
                    return (0x44, 0x44, 0x44);
                case 12:
                    return (0x6C, 0x6C, 0x6C);
                case 13:
                    return (0x9A, 0xD2, 0x84);
                case 14:
                    return (0x6c, 0x5e,0xb5);
                case 15:
                    return (0x95, 0x95, 0x95);
            }
            return (0,0,0);

        }
        public void GraphicsDataSequencer() {
            var red = ( byte) 0;
            var green = (byte) 0;
            var blue = (byte) 0;
            var alpha = (byte) 0;

            for (var px = 0; px < 8; px++)
            {
                var single_pixel_value = (_sequencer & (0b1  << (7-px))) >> (7-px);
                var double_pixel_value = (_sequencer & (0b11 << (6-2*(px/2)))) >> (6-2*(px/2));


                if (_x == _lastXCoord)
                {
                    _mainBorderFlipFlop = true;
                }
                if (_x == _firstXCoord && _y == _lastLine)
                {
                    _verticalBorderFlipFlop = true;
                }

                if (_x == _firstXCoord && _y == _firstLine && ((CR1 & 0x10) == 0x10))
                {
                    _verticalBorderFlipFlop = false;
                }

                if (_x == _firstXCoord && !_verticalBorderFlipFlop)
                {
                    _mainBorderFlipFlop = false;
                }

                // Check if we are not in the borders
                if (!_verticalBorderFlipFlop && !_mainBorderFlipFlop)
                {
                    switch(_graphicMode()) {

                        case 0x0:
                            // For standard Text Mode
                            //    Each bit is a pixel
                            //if ( ( (_sequencer & (1 << (7-px))  & 0x80) == 0x80 )
                            if ( single_pixel_value == 1 )
                            {
                                (red, green ,blue) = _getColorPalette(_color);
                            }
                            else
                            {
                                (red,green,blue) = _getColorPalette(B0C);

                            }
                            break;
                        case 0x1:
                            // Multicolor text mode

                            // Check MC flag of color
                            // if set, 2 bits per pixel
                            // if not, same as standard text mode but only 7 colors 
                            // available
                            if ( (_color & 0x8) == 0x8) {
                                // set the color by pixel pairs
                                if ( px % 2 == 0) {
                                    //if ( (_sequencer & 0b1100_0000) == 0b1100_0000 )
                                    if ( double_pixel_value == 0b11 )
                                    {
                                        (red, green ,blue) = _getColorPalette(_color & 0x7);
                                    }
                                    // else if ( (_sequencer & 0b0100_0000) == 0b0100_0000 )
                                    else if ( double_pixel_value == 0b01 )
                                    {
                                        (red,green,blue) = _getColorPalette(B1C);
                                    }
                                    //else if ( (_sequencer & 0b1000_0000) == 0b1000_0000 )
                                    else if ( double_pixel_value == 0b10 )
                                    {
                                        (red,green,blue) = _getColorPalette(B2C);
                                    } else
                                    {
                                        (red,green,blue) = _getColorPalette(B0C);
                                    }
                                }
                            } else {
                                if ( single_pixel_value == 1 )
                                {
                                    (red, green ,blue) = _getColorPalette(_color);
                                }
                                else
                                {
                                    (red,green,blue) = _getColorPalette(B0C);

                                }
                            }
                            break;

                        case 0x2:
                            // For standard Bitmap Mode
                            //    Each bit is a pixel
                            // +----+----+----+----+----+----+----+----+
                            // |  7 |  6 |  5 |  4 |  3 |  2 |  1 |  0 |
                            // +----+----+----+----+----+----+----+----+
                            // |         8 pixels (1 bit/pixel)        |
                            // |                                       |
                            // | "0": Color from bits 0-3 of c-data    |
                            // | "1": Color from bits 4-7 of c-data    |
                            // +---------------------------------------+
                            if ( single_pixel_value == 1 )
                            {
                                (red, green ,blue) = _getColorPalette((VideoMatrixLine[(_VMLI ) % 40 ] & 0xF0) >> 4);
                            }
                            else
                            {
                                (red,green,blue) = _getColorPalette((VideoMatrixLine[(_VMLI) % 40] & 0x7));
                            }

                            break;

                        case 0x2|0x1:
                            // For Multicolor Bitmap Mode
                            // +----+----+----+----+----+----+----+----+
                            // |  7 |  6 |  5 |  4 |  3 |  2 |  1 |  0 |
                            // +----+----+----+----+----+----+----+----+
                            // |         4 pixels (2 bits/pixel)       |
                            // |                                       |
                            // | "00": Background color 0 ($d021)      |
                            // | "01": Color from bits 4-7 of c-data   |
                            // | "10": Color from bits 0-3 of c-data   |
                            // | "11": Color from bits 8-11 of c-data  |
                            //  +---------------------------------------+

			                if ( px % 2 == 0) {
                                if ( double_pixel_value == 0b11 )
                                {
                                    (red, green ,blue) = _getColorPalette(_color & 0x7);
                                }
                                else if ( double_pixel_value == 0b01 )
                                {
                                    (red, green ,blue) = _getColorPalette((VideoMatrixLine[(_VMLI ) % 40 ] & 0xF0) >> 4);
                                }
                                else if ( double_pixel_value == 0b10 )
                                {
                                    (red,green,blue) = _getColorPalette((VideoMatrixLine[(_VMLI) % 40] & 0x7));
                                } else
                                {
                                    (red,green,blue) = _getColorPalette(B0C);
                                }
                            }
                            break;

                    }
                    if ( !(_displayState) )
                    {
                        switch(_graphicMode()) {
                            case 0x0:
                            case 0x1:
                            case 0x4:
                                if (single_pixel_value == 1) {
                                    (red,green,blue) = (0x00, 0x00, 0x00);
                                } else {
                                    (red,green,blue) = _getColorPalette(B0C);
                                } 
                                break;
                            case 0x2:
                            case 0x1|0x4:
                            case 0x2|0x4:
                                (red,green,blue) = (0x00, 0x00, 0x00);
                                break;
                            case 0x1|0x2:
                                if ( double_pixel_value == 0) {
                                    (red,green,blue) = _getColorPalette(B0C);
                                } else {
                                    (red,green,blue) = (0x00, 0x00, 0x00);
                                }
                                break;
                            default:
                                (red,green,blue) = (0x00, 0x00, 0x00);
                                break;

                        }

                    }

                } else {
                    // Border Color
                    (red,green,blue) = _getColorPalette(EC);
                }

                //_sequencer = (_sequencer << 1) & 0xFF;

                //Screen[4*(_x+_y*_cyclesPerLines*8)] = pixel[0];

                Screen[4*(_x+(_numberLines-1-_y)*_numberRows)] = red;
                Screen[4*(_x+(_numberLines-1-_y)*_numberRows)+1] = green;
                Screen[4*(_x+(_numberLines-1-_y)*_numberRows)+2] = blue;

                _x++;
                if (_x == 0x1f8)
                {
                    _x = 0;
                }
            }            
            _sequencer = 0;
        }

        public void Tick()
        {

            if (!PHY0) {
                _opcycle++;
                if (_opcycle == _cyclesPerLines + 1) {
                    _opcycle = 1;
                }

            }


            // RSEL|  Display window height   | First line  | Last line
            // ----+--------------------------+-------------+----------
            //   0 | 24 text lines/192 pixels |   55 ($37)  | 246 ($f6)
            //   1 | 25 text lines/200 pixels |   51 ($33)  | 250 ($fa)
            _displayWindowsHeight = (CR1 & 0x08) == 0 ? 24U : 25U;
            _firstLine = (CR1 & 0x08) == 0 ? 0x37U : 0x33U;
            _lastLine = (CR1 & 0x08) == 0 ? 0xf6U : 0xfaU;

            // CSEL|   Display window width   | First X coo. | Last X coo.
            //  ----+--------------------------+--------------+------------
            //    0 | 38 characters/304 pixels |   31 ($1f)   |  334 ($14e)
            //    1 | 40 characters/320 pixels |   24 ($18)   |  343 ($157)
            _displayWindowsWidth = (CR2 & 0x08) == 0 ? 38U : 40U;
            _firstXCoord = (CR2 & 0x08) == 0 ? 0x1FU : 0x18U;
            _lastXCoord = (CR2 & 0x08) == 0 ? 0x14EU : 0x157U;


            if ( (CR1 & 0x07) != 0 )  
            {
                // 
                
                //Console.WriteLine("YSCROLL Set");
            }

            // Moved to the END
            // TODO: Remove comment if working
            //if (_opcycle == _cyclesPerLines + 1)
            //{
            //    _opcycle = 1;
            //}

            //Console.WriteLine("{0,4} {1,2} {2,3}",PHY0, _opcycle,_VMLI);

            AEC = PHY0;

            if (_opcycle >= 15 && _opcycle <= 54)
            {
                AEC = !_badLine;
            }
            else
            {
                AEC = PHY0;
            }

            if (_opcycle >= 16 && _opcycle <= 54)
            {
                if (!PHY0)
                {
                    _gaccess();
                }
                else
                {
                    if ( _badLine)
                    {
                        _caccess();
                    }
                }
            }
            switch (_opcycle)
            {
                case 1:
                    {
                        if (_y == 0)
                        {
                            _x = _firstXCoordLine+4;
                            _ref = 0xFF;
                            _VCBASE = 0;
                        }

                        RASTER = _y & 0xFF;
                        CR1 = (CR1 & (~0x80 & 0xFF)) | ((_y & 0x100) >> 1);

                        // Bad Line
                        // Full RASTER is RASTER ($d012) and MSB in bit 7 of CR1
                        _badLine = (((RASTER | ((CR1 & 0x80) <<1)) >= 0x30 && (RASTER | ((CR1 & 0x80) <<1)) <= 0xF7) &&
                            ((RASTER & 0x07) == (CR1 & 0x07)) &&
                            ((CR1 & 0x10) == 0x10));
                        if (_badLine)
                        {
                            _displayState = true;
                        }

                        // Compare Raster line with $d012/$d011(bit 7)
                        // if line is not 0
                        // if same, IRQ should be set and RST set as IRQ reason
                        if ((_y != 0) && _y == (RASTER | ((CR1 & 0x80) << 1)))
                        {
                            INTR |= 0x01;
                            // toDO: Should we also set INTE

                        }


                        // pAccess(3)
                        _paccess(3);
                        BA = true;


                        break;
                    }
                case 2:
                    {

                        // Compare Raster line with $d012/$d011(bit 7)
                        // if line is not 0
                        // if same, IRQ should be set and RST set as IRQ reason
                        if ((_y == 0) && _y == (RASTER | ((CR1 & 0x80) << 1)))
                        {
                            INTR |= 0x01;

                        }
                        _idleAccess();
                        break;
                    }

                case 3:
                    {
                        _paccess(4);
                        break;
                    }

                case 5:
                    {
                        _paccess(5);
                        break;
                    }
                case 7:
                    {
                        _paccess(6);
                        break;
                    }
                case 9:
                    {
                        _paccess(7);
                        break;
                    }

                case 4:
                case 6:
                case 8:
                case 10:
                    {
                        _idleAccess();
                        break;
                    }

                case 11:
                    {
                        _dramRefresh();
                        break;
                    }
                case 12:
                    {
                        if (!PHY0)
                        {
                            BA = !_badLine;
                            _dramRefresh();
                        }
                        break;
                    }

                case 13:
                    if (!PHY0)
                    {
                        _dramRefresh();
                    }
                    break;
                case 14:
                    if (!PHY0)
                    {
                        _VC = _VCBASE;
                        _VMLI = 0;
                        if (_badLine) { _RC = 0; }
                        _dramRefresh();
                    }
                    break;
                case 15:
                    {
                        if (!PHY0)
                        {
                            _dramRefresh();

                        }
                        else
                        {
                            _caccess();
                        }

                        break;
                    }
                case 16:
                case 54:
                    {
                        break;
                    }


                case 55:
                    {
                        BA = true;
                        _gaccess();
                        break;
                    }

                case 56:
                case 57:
                case 61:
                    {
                        _idleAccess();
                        break;
                    }

                case 58:
                    {
                        if (_RC == 7)
                        {
                            _VCBASE = (_VC & 0x3FF);
                            if (!_badLine)
                            {
                                _displayState = false;
                            }
                        }
                        if (_displayState)
                        {
                            _RC = (_RC + 1) & 0x07;
                        }
                        _paccess(0);
                        break;
                    }
                case 59:
                    {
                        _idleAccess();
                        break;
                    }
                case 60:
                    {
                        _paccess(1);
                        break;
                    }
                case 62:
                    {
                        _paccess(2);
                        break;
                    }

                case 63:
                    {
                        _idleAccess();
                        if (_y == _lastLine)
                        {
                            _verticalBorderFlipFlop = true;
                        }
                        if ((_y == _firstLine) && ((CR1 & 0x10) == 0x10))
                        {
                            _verticalBorderFlipFlop = false;
                        }
                        break;
                    }

            }


            if ( !PHY0)
            {

                GraphicsDataSequencer();

            }
            //GraphicsDataSequencer();

            if (_opcycle == 63) {
                _y++;
                if (_y == _numberLines)
                {
                    _y = 0;
                    //for (int i = 0; i< Screen.Length; i++) { Screen[i] = 255;}
                }

            }



        }
    }
}
