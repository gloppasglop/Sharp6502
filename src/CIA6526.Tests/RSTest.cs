using System;
using Xunit;
using CIA6526;

namespace CIA6526.Tests
{


    public class RegisterSelect
    {

        private Board testBoard = new Board();
        private int cycles = 4;


        [Fact]
        public void PRA_Read_ShouldWork()
        {
            cycles = 2;
            var cia = testBoard.Cia;
            cia.Init();
            cia.CS = true;
            cia.RES = false;

            // Select PRA Register
            cia.RS = 0b0000;
            // Read from CIA  
            cia.RW = true;
            var PRA = 42u;
            cia.PortA = PRA; 

            testBoard.Execute(cycles);

            
            // PRA should be on Datapins
            Assert.Equal(PRA,cia.DataPins);
        }

        [Theory]
        // Writing to PRA should only write bits where conrresponding bits 
        // are set in DDR, othe bits unchanged
        // Initial Value, Written Value, DDRA, Expected Result
        [InlineData(0b1111_1010,0b1001_1111,0b1111_1111,0b1001_1111)]
        [InlineData(0b1111_1010,0b1001_1111,0b1001_1001,0b1111_1111)]
        [InlineData(0b1111_0010,0b1001_1000,0b1001_1001,0b1111_1010)]
        public void PRA_Write_ShouldWork(uint initialValue,uint writtenValue,uint ddr, uint expectedResult)
        {
            cycles = 2;
            var cia = testBoard.Cia;
            cia.Init();
            cia.CS = true;
            cia.RES = false;

            // Select PRA Register
            cia.RS = 0b0000;
            // Write to CIA  
            cia.RW = false;
            // Initialize register with some value
            cia.PortA = initialValue; 
            cia.DDRA = ddr; 

            cia.DataPins = writtenValue;

            testBoard.Execute(cycles);

            // PRA should be set
            Assert.Equal(expectedResult,cia.PRA);
        }

        [Fact]
        public void PRB_Read_ShouldWork()
        {
            cycles = 2;
            var cia = testBoard.Cia;
            cia.Init();
            cia.CS = true;
            cia.RES = false;

            // Select PRB Register
            cia.RS = 0b0001;
            // Read from CIA  
            cia.RW = true;
            var PRB = 42u;
            cia.PortB = PRB; 

            testBoard.Execute(cycles);

            
            // PRB should be on Datapins
            Assert.Equal(PRB,cia.DataPins);
        }

        [Theory]
        // Writing to PRA should only write bits where conrresponding bits 
        // are set in DDR, othe bits unchanged
        // Initial Value, Written Value, DDRA, Expected Result
        [InlineData(0b1111_1010,0b1001_1111,0b1111_1111,0b1001_1111)]
        [InlineData(0b1111_1010,0b1001_1111,0b1001_1001,0b1111_1111)]
        [InlineData(0b1111_0010,0b1001_1000,0b1001_1001,0b1111_1010)]
        public void PRB_Write_ShouldWork(uint initialValue,uint writtenValue,uint ddr, uint expectedResult)
        {
            cycles = 2;
            var cia = testBoard.Cia;
            cia.Init();
            cia.CS = true;
            cia.RES = false;

            // Select PRB Register
            cia.RS = 0b0001;
            // Write to CIA  
            cia.RW = false;
            // Initialize register with some value
            cia.PortB = initialValue; 
            cia.DDRB = ddr; 

            cia.DataPins = writtenValue;

            testBoard.Execute(cycles);

            // PRB should be set
            Assert.Equal(expectedResult,cia.PRB);
        }

        [Fact]
        public void DDRA_Read_ShouldWork()
        {
            cycles = 2;
            var cia = testBoard.Cia;
            cia.Init();
            cia.CS = true;
            cia.RES = false;

            // Select DDRA Register
            cia.RS = 0b0010;
            // Read from CIA  
            cia.RW = true;
            var DDRA = 42u;
            cia.DDRA = DDRA; 

            testBoard.Execute(cycles);

            
            // DDRA should be on Datapins
            Assert.Equal(DDRA,cia.DataPins);
        }


        [Fact]
        public void DDRA_Write_ShouldWork()
        {
            cycles = 2;
            var cia = testBoard.Cia;
            cia.Init();
            cia.CS = true;
            cia.RES = false;

            // Select DDRA Register
            cia.RS = 0b0010;
            // Write to CIA  
            cia.RW = false;
            // Initialize register with some value

            cia.DDRA = 69u;
            var DDRA = 42u;

            cia.DataPins = DDRA;

            testBoard.Execute(cycles);

            // DDRA should be set
            Assert.Equal(DDRA,cia.DDRA);

        }

        [Fact]
        public void DDRB_Read_ShouldWork()
        {
            cycles = 2;
            var cia = testBoard.Cia;
            cia.Init();
            cia.CS = true;
            cia.RES = false;

            // Select DDRB Register
            cia.RS = 0b0011;
            // Read from CIA  
            cia.RW = true;
            var DDRB = 42u;
            cia.DDRB = DDRB; 

            testBoard.Execute(cycles);

            
            // DDRB should be on Datapins
            Assert.Equal(DDRB,cia.DataPins);
        }


        [Fact]
        public void DDRB_Write_ShouldWork()
        {
            cycles = 2;
            var cia = testBoard.Cia;
            cia.Init();
            cia.CS = true;
            cia.RES = false;

            // Select DDRB Register
            cia.RS = 0b0011;
            // Write to CIA  
            cia.RW = false;
            // Initialize register with some value

            cia.DDRB = 69u;
            var DDRB = 42u;

            cia.DataPins = DDRB;

            testBoard.Execute(cycles);

            // DDRB should be set
            Assert.Equal(DDRB,cia.DDRB);
        }

        //TODO: Not sure correct 
        [Fact]
        public void TALO_Read_ShouldWork()
        {
            cycles = 2;
            var cia = testBoard.Cia;
            cia.Init();
            cia.CS = true;
            cia.RES = false;

            // Select TALO Register
            cia.RS = 0b0100;
            // Read from CIA  
            cia.RW = true;
            var TALO = 42u;
            cia.TALO = TALO; 

            testBoard.Execute(cycles);

            
            // TALO should be on Datapins
            Assert.Equal(TALO,cia.DataPins);
        }


        [Fact]
        public void TALO_Write_ShouldWork()
        {
            cycles = 2;
            var cia = testBoard.Cia;
            cia.Init();
            cia.CS = true;
            cia.RES = false;

            // Select TALO Register
            cia.RS = 0b0100;
            // Write to CIA  
            cia.RW = false;
            // Initialize register with some value

            cia.TALO = 69u;
            var TALO = 42u;

            cia.DataPins = TALO;

            testBoard.Execute(cycles);

            // TALO should be set
            Assert.Equal(TALO,cia.TALO);
        }

        //TODO: Not sure correct 
        [Fact]
        public void TAHI_Read_ShouldWork()
        {
            cycles = 2;
            var cia = testBoard.Cia;
            cia.Init();
            cia.CS = true;
            cia.RES = false;

            // Select TAHI Register
            cia.RS = 0b0101;
            // Read from CIA  
            cia.RW = true;
            var TAHI = 42u;
            cia.TAHI = TAHI; 

            testBoard.Execute(cycles);

            
            // TAHI should be on Datapins
            Assert.Equal(TAHI,cia.DataPins);
        }


        [Fact]
        public void TAHI_Write_ShouldWork()
        {
            cycles = 2;
            var cia = testBoard.Cia;
            cia.Init();
            cia.CS = true;
            cia.RES = false;

            // Select TAHI Register
            cia.RS = 0b0101;
            // Write to CIA  
            cia.RW = false;
            // Initialize register with some value

            cia.TAHI = 69u;
            var TAHI = 42u;

            cia.DataPins = TAHI;

            testBoard.Execute(cycles);

            // TAHI should be set
            Assert.Equal(TAHI,cia.TAHI);
        }

        //TODO: Not sure correct 
        [Fact]
        public void TBLO_Read_ShouldWork()
        {
            cycles = 2;
            var cia = testBoard.Cia;
            cia.Init();
            cia.CS = true;
            cia.RES = false;

            // Select TBLO Register
            cia.RS = 0b0110;
            // Read from CIA  
            cia.RW = true;
            var TBLO = 42u;
            cia.TBLO = TBLO; 

            testBoard.Execute(cycles);

            
            // TBLO should be on Datapins
            Assert.Equal(TBLO,cia.DataPins);
        }


        [Fact]
        public void TBLO_Write_ShouldWork()
        {
            cycles = 2;
            var cia = testBoard.Cia;
            cia.Init();
            cia.CS = true;
            cia.RES = false;

            // Select TBLO Register
            cia.RS = 0b0110;
            // Write to CIA  
            cia.RW = false;
            // Initialize register with some value

            cia.TBLO = 69u;
            var TBLO = 42u;

            cia.DataPins = TBLO;

            testBoard.Execute(cycles);

            // TBLO should be set
            Assert.Equal(TBLO,cia.TBLO);
        }

        //TODO: Not sure correct 
        [Fact]
        public void TBHI_Read_ShouldWork()
        {
            cycles = 2;
            var cia = testBoard.Cia;
            cia.Init();
            cia.CS = true;
            cia.RES = false;

            // Select TBHI Register
            cia.RS = 0b0111;
            // Read from CIA  
            cia.RW = true;
            var TBHI = 42u;
            cia.TBHI = TBHI; 

            testBoard.Execute(cycles);

            
            // PRA should be on Datapins
            Assert.Equal(TBHI,cia.DataPins);
        }


        [Fact]
        public void TBHI_Write_ShouldWork()
        {
            cycles = 2;
            var cia = testBoard.Cia;
            cia.Init();
            cia.CS = true;
            cia.RES = false;

            // Select TBHI Register
            cia.RS = 0b0111;
            // Write to CIA  
            cia.RW = false;
            // Initialize register with some value

            cia.TBHI = 69u;
            var TBHI = 42u;

            cia.DataPins = TBHI;

            testBoard.Execute(cycles);

            // PRA should be set
            Assert.Equal(TBHI,cia.TBHI);
        }

        //TODO: Not sure correct 
        [Fact]
        public void TOD10TH_Read_ShouldWork()
        {
            cycles = 2;
            var cia = testBoard.Cia;
            cia.Init();
            cia.CS = true;
            cia.RES = false;

            // Select TOD10TH Register
            cia.RS = 0b1000;
            // Read from CIA  
            cia.RW = true;
            var TOD10TH = 42u;
            cia.TOD10TH = TOD10TH; 

            testBoard.Execute(cycles);

            
            // TOD10TH should be on Datapins
            Assert.Equal(TOD10TH,cia.DataPins);
        }


        [Fact]
        public void TOD10TH_Write_ShouldWork()
        {
            cycles = 2;
            var cia = testBoard.Cia;
            cia.Init();
            cia.CS = true;
            cia.RES = false;

            // Select TOD10TH Register
            cia.RS = 0b1000;
            // Write to CIA  
            cia.RW = false;
            // Initialize register with some value

            cia.TOD10TH = 69u;
            var TOD10TH = 42u;

            cia.DataPins = TOD10TH;

            testBoard.Execute(cycles);

            // TOD10TH should be set
            Assert.Equal(TOD10TH,cia.TOD10TH);
        }

        //TODO: Not sure correct 
        [Fact]
        public void TODSEC_Read_ShouldWork()
        {
            cycles = 2;
            var cia = testBoard.Cia;
            cia.Init();
            cia.CS = true;
            cia.RES = false;

            // Select TODSEC Register
            cia.RS = 0b1001;
            // Read from CIA  
            cia.RW = true;
            var TODSEC = 42u;
            cia.TODSEC = TODSEC; 

            testBoard.Execute(cycles);

            
            // TODSEC should be on Datapins
            Assert.Equal(TODSEC,cia.DataPins);
        }


        [Fact]
        public void TODSEC_Write_ShouldWork()
        {
            cycles = 2;
            var cia = testBoard.Cia;
            cia.Init();
            cia.CS = true;
            cia.RES = false;

            // Select TODSEC Register
            cia.RS = 0b1001;
            // Write to CIA  
            cia.RW = false;
            // Initialize register with some value

            cia.TODSEC = 69u;
            var TODSEC = 42u;

            cia.DataPins = TODSEC;

            testBoard.Execute(cycles);

            // TODSEC should be set
            Assert.Equal(TODSEC,cia.TODSEC);
        }

        //TODO: Not sure correct 
        [Fact]
        public void TODMIN_Read_ShouldWork()
        {
            cycles = 2;
            var cia = testBoard.Cia;
            cia.Init();
            cia.CS = true;
            cia.RES = false;

            // Select TODMIN Register
            cia.RS = 0b1010;
            // Read from CIA  
            cia.RW = true;
            var TODMIN = 42u;
            cia.TODMIN = TODMIN; 

            testBoard.Execute(cycles);

            
            // TODMIN should be on Datapins
            Assert.Equal(TODMIN,cia.DataPins);
        }


        [Fact]
        public void TODMIN_Write_ShouldWork()
        {
            cycles = 2;
            var cia = testBoard.Cia;
            cia.Init();
            cia.CS = true;
            cia.RES = false;

            // Select TODMIN Register
            cia.RS = 0b1010;
            // Write to CIA  
            cia.RW = false;
            // Initialize register with some value

            cia.TODMIN = 69u;
            var TODMIN = 42u;

            cia.DataPins = TODMIN;

            testBoard.Execute(cycles);

            // TODMIN should be set
            Assert.Equal(TODMIN,cia.TODMIN);
        }

        //TODO: Not sure correct 
        [Fact]
        public void TODHR_Read_ShouldWork()
        {
            cycles = 2;
            var cia = testBoard.Cia;
            cia.Init();
            cia.CS = true;
            cia.RES = false;

            // Select TODHR Register
            cia.RS = 0b1011;
            // Read from CIA  
            cia.RW = true;
            var TODHR = 42u;
            cia.TODHR = TODHR; 

            testBoard.Execute(cycles);

            
            // TODHR should be on Datapins
            Assert.Equal(TODHR,cia.DataPins);
        }


        [Fact]
        public void TODHR_Write_ShouldWork()
        {
            cycles = 2;
            var cia = testBoard.Cia;
            cia.Init();
            cia.CS = true;
            cia.RES = false;

            // Select TODHR Register
            cia.RS = 0b1011;
            // Write to CIA  
            cia.RW = false;
            // Initialize register with some value

            cia.TODHR = 69u;
            var TODHR = 42u;

            cia.DataPins = TODHR;

            testBoard.Execute(cycles);

            // TODHR should be set
            Assert.Equal(TODHR,cia.TODHR);
        }

        //TODO: Not sure correct 
        [Fact]
        public void SDR_Read_ShouldWork()
        {
            cycles = 2;
            var cia = testBoard.Cia;
            cia.Init();
            cia.CS = true;
            cia.RES = false;

            // Select SDR Register
            cia.RS = 0b1100;
            // Read from CIA  
            cia.RW = true;
            var SDR = 42u;
            cia.SDR = SDR; 

            testBoard.Execute(cycles);

            
            // SDR should be on Datapins
            Assert.Equal(SDR,cia.DataPins);
        }


        [Fact]
        public void SDR_Write_ShouldWork()
        {
            cycles = 2;
            var cia = testBoard.Cia;
            cia.Init();
            cia.CS = true;
            cia.RES = false;

            // Select SDR Register
            cia.RS = 0b1100;
            // Write to CIA  
            cia.RW = false;
            // Initialize register with some value

            cia.SDR = 69u;
            var SDR = 42u;

            cia.DataPins = SDR;

            testBoard.Execute(cycles);

            // SDR should be set
            Assert.Equal(SDR,cia.SDR);
        }

        // ICR Test done in separate file
        //public void ICR_Read_ShouldWork()
        //public void ICR_Write_ShouldWork()

        //TODO: Not sure correct 
        [Fact]
        public void CRA_Read_ShouldWork()
        {
            cycles = 2;
            var cia = testBoard.Cia;
            cia.Init();
            cia.CS = true;
            cia.RES = false;

            // Select CRA Register
            cia.RS = 0b1110;
            // Read from CIA  
            cia.RW = true;
            var CRA = 42u;
            cia.CRA = CRA; 

            testBoard.Execute(cycles);

            
            // CRA should be on Datapins
            Assert.Equal(CRA,cia.DataPins);
        }


        [Fact]
        public void CRA_Write_ShouldWork()
        {
            cycles = 2;
            var cia = testBoard.Cia;
            cia.Init();
            cia.CS = true;
            cia.RES = false;

            // Select CRA Register
            cia.RS = 0b1110;
            // Write to CIA  
            cia.RW = false;
            // Initialize register with some value

            cia.CRA = 69u;
            var CRA = 42u;

            cia.DataPins = CRA;

            testBoard.Execute(cycles);

            // CRA should be set
            Assert.Equal(CRA,cia.CRA);
        }

        //TODO: Not sure correct 
        [Fact]
        public void CRB_Read_ShouldWork()
        {
            cycles = 2;
            var cia = testBoard.Cia;
            cia.Init();
            cia.CS = true;
            cia.RES = false;

            // Select CRB Register
            cia.RS = 0b1111;
            // Read from CIA  
            cia.RW = true;
            var CRB = 42u;
            cia.CRB = CRB; 

            testBoard.Execute(cycles);

            
            // CRB should be on Datapins
            Assert.Equal(CRB,cia.DataPins);
        }


        [Fact]
        public void CRB_Write_ShouldWork()
        {
            cycles = 2;
            var cia = testBoard.Cia;
            cia.Init();
            cia.CS = true;
            cia.RES = false;

            // Select CRB Register
            cia.RS = 0b1111;
            // Write to CIA  
            cia.RW = false;
            // Initialize register with some value

            cia.CRB = 69u;
            var CRB = 42u;

            cia.DataPins = CRB;

            testBoard.Execute(cycles);

            // CRB should be set
            Assert.Equal(CRB,cia.CRB);
        }
    }
}
