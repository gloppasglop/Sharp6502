using System;
using Xunit;
using CIA6526;

namespace CIA6526.Tests
{


    public class TimerA
    {

        private Board testBoard = new Board();
	private CIA6526.Chip cia;
        private int cycles = 4;

	private void initTimer(uint counter, bool oneShot, uint icr)
	{
		cia = testBoard.Cia;
            	cia.Init();
            	cia.CS = true;
            	cia.RES = false;

	    	// Set Timer to count 1000 PHY2
            	// Select TALO Register
            	cia.RS = 0b0100;
            	// Write to CIA  
            	cia.RW = false;
            	cia.DataPins = counter & 0xFF;
            	testBoard.Execute(2);

            	// Select TAHI Register
            	cia.RS = 0b0101;
            	// Write to CIA  
            	cia.RW = false;
            	cia.DataPins = counter >> 8;
            	testBoard.Execute(2);

            	// Select ICR Register
            	cia.RS = 0xD;
            	// Write to CIA  
            	cia.RW = false;
            	cia.DataPins = icr;
            	testBoard.Execute(2);

            	// Select CRA Register
            	cia.RS = 0b1110;
            	cia.RW = false;
	    	// Set Continuous mode, start timerA
		if (oneShot) {
	    		cia.DataPins = 0b0000_1001;
		} else {
	    		cia.DataPins = 0b0000_0001;
		}
            	testBoard.Execute(2);
	}


        [Fact]
        public void TimerContinousIRQNoOverflow_ShouldWork()
	{	

		var startCounter = 1000u;
		var count = 500;
		// Timer A, interrupt request
		var icr = 0b1000_0001u;

		initTimer(startCounter,false, icr);
            	cia.RW = true;


            	testBoard.Execute(count);

	    	var TALO = cia.TALO;
	    	var TAHI = cia.TAHI;

		// Timer should be started
	    	Assert.Equal(cia.CRA & 0b0000_0001u,0b0000_0001u);
	    	// 499 cycles should have passed
	    	Assert.Equal(499u,TALO+256*TAHI);
	    	// IRQ should be low
	    	Assert.Equal(cia.IRQ,false);
	}

        [Fact]
        public void TimerContinousIRQOverflow_ShouldOverflowWithIRQ()
	{	

		var startCounter = 1000u;
		var count = 1500;
		// Timer A, interrupt request
		var icr = 0b1000_0001u;

		initTimer(startCounter,false, icr);

            	testBoard.Execute(count);

	    	var TALO = cia.TALO;
	    	var TAHI = cia.TAHI;

		// Timer should be started
	    	Assert.Equal(cia.CRA & 0b0000_0001u,0b0000_0001u);
	    	// Counter should be 500
	    	Assert.Equal(500u,TALO+256*TAHI);
	    	// IRQ should be High
	    	Assert.Equal(cia.IRQ,true);
	}

        [Fact]
        public void TimerContinousNoIRQNoOverflow_ShouldWork()
	{	

		var startCounter = 1000u;
		var count = 500;
		// Timer A, no interrupt request
		var icr = 0b1000_0000u;

		initTimer(startCounter,false, icr);


            	testBoard.Execute(count);

	    	var TALO = cia.TALO;
	    	var TAHI = cia.TAHI;

		// Timer should be started
	    	Assert.Equal(cia.CRA & 0b0000_0001u,0b0000_0001u);
	    	// 499 cycles should have passed
	    	Assert.Equal(499u,TALO+256*TAHI);
	    	// IRQ should be low
	    	Assert.Equal(cia.IRQ,false);
	}
        [Fact]
        public void TimerContinousNoIRQOverflow_ShouldOverflowNoIRQ()
	{	

		var startCounter = 1000u;
		var count = 1500;
		// Timer A, no interrupt request
		var icr = 0b0000_0001u;

		initTimer(startCounter,false, icr);


            	testBoard.Execute(count);

	    	var TALO = cia.TALO;
	    	var TAHI = cia.TAHI;

		// Timer should be started
	    	Assert.Equal(cia.CRA & 0b0000_0001u,0b0000_0001u);
	    	// 500 cycles should have passed
	    	Assert.Equal(500u,TALO+256*TAHI);
	    	// IRQ should be low
	    	Assert.Equal(cia.IRQ,false);
	}

        [Fact]
        public void TimerOneShotIRQNoOverflow_ShouldWork()
	{	

		var startCounter = 1000u;
		var count = 500;
		// Timer A, interrupt request
		var icr = 0b1000_0001u;

		initTimer(startCounter,true, icr);


            	testBoard.Execute(count);

	    	var TALO = cia.TALO;
	    	var TAHI = cia.TAHI;

		// Timer should be started
	    	Assert.Equal(cia.CRA & 0b0000_0001u,0b0000_0001u);
	    	// 499 cycles should have passed
	    	Assert.Equal(499u,TALO+256*TAHI);
	    	// IRQ should be low
	    	Assert.Equal(cia.IRQ,false);
	}

        [Fact]
        public void TimerOneShotIRQOverflow_ShouldStopWithIRQ()
	{	

		var startCounter = 1000u;
		var count = 1500;
		// Timer A, interrupt request
		var icr = 0b1000_0001u;

		initTimer(startCounter,true, icr);
            	cia.RW = true;


            	testBoard.Execute(count);

	    	var TALO = cia.TALO;
	    	var TAHI = cia.TAHI;

		// Timer should be stopped
	    	Assert.Equal(cia.CRA & 0b0000_0001u,0b0000_0000u);
	    	// Counter should be 1000
	    	Assert.Equal(1000u,TALO+256*TAHI);
	    	// IRQ should be High
	    	Assert.Equal(cia.IRQ,true);
	}

        [Fact]
        public void TimerOneShotNoIRQNoOverflow_ShouldWork()
	{	

		var startCounter = 1000u;
		var count = 500;
		// Timer A, no interrupt request
		var icr = 0b1000_0000u;

		initTimer(startCounter,true, icr);
            	cia.RW = true;


            	testBoard.Execute(count);

	    	var TALO = cia.TALO;
	    	var TAHI = cia.TAHI;

		// Timer should be started
	    	Assert.Equal(cia.CRA & 0b0000_0001u,0b0000_0001u);
	    	// 499 cycles should have passed
	    	Assert.Equal(499u,TALO+256*TAHI);
	    	// IRQ should be low
	    	Assert.Equal(cia.IRQ,false);
	}
        [Fact]
        public void TimerOneShotNoIRQOverflow_ShouldStopWithNoIRQ()
	{	

		var startCounter = 1000u;
		var count = 1500;
		// Timer A, no interrupt request
		var icr = 0b0000_0001u;

		initTimer(startCounter,true, icr);
            	cia.RW = true;


            	testBoard.Execute(count);

	    	var TALO = cia.TALO;
	    	var TAHI = cia.TAHI;

		// Timer should be started
	    	Assert.Equal(cia.CRA & 0b0000_0001u,0b0000_0000u);
	    	// Counter reloaded with initial value 1000
	    	Assert.Equal(1000u,TALO+256*TAHI);
	    	// IRQ should be low
	    	Assert.Equal(cia.IRQ,false);
	}
    }
}
	