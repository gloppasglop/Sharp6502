using System;
using Xunit;
using CIA6526;

namespace CIA6526.Tests
{


    public class ICRTest
    {

        private Board testBoard = new Board();
	private CIA6526.Chip cia;
        private int cycles = 4;

	private void initBoard()
	{
		cia = testBoard.Cia;
            	cia.Init();
            	cia.CS = true;
            	cia.RES = false;
	}


        [Fact]
        public void ICR_ReadAfterInit_ShouldWork()
        {
		initBoard();
		cycles = 2;
            	cia.RS = 0b1101;
            	// Select ICR Register
            	// Read from CIA  
            	cia.RW = true;

            	testBoard.Execute(cycles);
            
            	// ICR should be on Datapins
            	Assert.Equal<uint>(0,cia.DataPins);
		// Reading ICR should clear IRQ
            	Assert.Equal<bool>(cia.IRQ,false);
        }

        [Fact]
        public void ICR_Read_ShouldClearIRQ()
        {
		initBoard();
            	cia.RS = 0b1101;
            	// Select ICR Register
            	// Read from CIA  
            	cia.RW = true;
		cia.IRQ = true;

            	testBoard.Execute(2);
            
		// Reading ICR should clear IRQ
            	Assert.Equal<bool>(cia.IRQ,false);
	}

	
        [Fact]
        public void ICR_WriteWithSCAfterInit_ShouldWork()
        {
		initBoard();

            	// Select ICR Register
            	cia.RS = 0b1101;
            	// Write to CIA  
            	cia.RW = false;

	     	// If bit 7 of the data written is a
		// ONE, any mask bit written with a one will be set,
		// while those mask bits written with a zero will be
		// unaffected. 

            	var ICR = 0b1010_0101u;
            	cia.DataPins = ICR;
            	testBoard.Execute(2);

            	// As ICR is 0 after init, we should now have ICR in cia.ICR
            	Assert.Equal(ICR,cia.ICR);
        }

        [Fact]
        public void ICR_WriteWithSC_ShouldWork()
        {
		initBoard();

            	// Select ICR Register
            	cia.RS = 0b1101;
            	// Write to CIA  
            	cia.RW = false;

	        // Initialize ICR register
            	var ICR = 0b1010_0101u;
            	cia.DataPins = ICR;
            	testBoard.Execute(2);

	     	// If bit 7 of the data written is a
		// ONE, any mask bit written with a one will be set,
		// while those mask bits written with a zero will be
		// unaffected. 

            	ICR = 0b1100_0000u;
            	cia.DataPins = ICR;
            	testBoard.Execute(2);

            	// Bit 6 should now be set while other bits should be unchanged
            	Assert.Equal(0b1110_0101u,cia.ICR);

        }

        [Fact]
        public void ICR_ReadWithSC_ShouldClearIRbit()
        {
		initBoard();

            	// Select ICR Register
            	cia.RS = 0b1101;
            	// Write to CIA  
            	cia.RW = false;

	        // Initialize ICR register
            	var ICR = 0b1010_0101u;
            	cia.DataPins = ICR;
            	testBoard.Execute(2);

	     	// If bit 7 of the data written is a
		// ONE, any mask bit written with a one will be set,
		// while those mask bits written with a zero will be
		// unaffected. 

            	ICR = 0b1100_0000u;
            	cia.DataPins = ICR;
            	testBoard.Execute(2);

		// Read ICR
	     	_ = cia.ICR;	
            	// Bit 6 should now be cleared
            	Assert.Equal(0b0110_0101u,cia.ICR);

        }

        [Fact]
        public void ICR_WriteWithoutSC_ShouldWork()
        {
		initBoard();

            	// Select ICR Register
            	cia.RS = 0b1101;
            	// Write to CIA  
            	cia.RW = false;

	        // Initialize ICR register
            	var ICR = 0b1010_0101u;
            	cia.DataPins = ICR;
            	testBoard.Execute(2);

	       	// if bit 7 (SET/CLEAR) of data written is a
		// ZERO, any mask bit written with a one will be
		// cleared, while those mask bits written with a zero
		// will be unaffected.

            	ICR = 0b0111_0000u;
            	cia.DataPins = ICR;
            	testBoard.Execute(2);

            	// Bit with 1 should now be cleared while other bits should be unchanged
            	Assert.Equal(0b1000_0101u,cia.ICR);
	}

        [Fact]
        public void ICR_readWithoutSC_ShouldClearIR()
        {
		initBoard();

            	// Select ICR Register
            	cia.RS = 0b1101;
            	// Write to CIA  
            	cia.RW = false;

	        // Initialize ICR register
            	var ICR = 0b1010_0101u;
            	cia.DataPins = ICR;
            	testBoard.Execute(2);

	       	// if bit 7 (SET/CLEAR) of data written is a
		// ZERO, any mask bit written with a one will be
		// cleared, while those mask bits written with a zero
		// will be unaffected.

            	ICR = 0b0111_0000u;
            	cia.DataPins = ICR;
            	testBoard.Execute(2);

	     	_ = cia.ICR;	
            	// Bit with 1 should now be cleared while other bits should be unchanged
		// and bit7 should be 0
            	Assert.Equal(0b0000_0101u,cia.ICR);
	}

    }
}