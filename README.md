

# 6502 Emulator in C#

Trying to write a 6502 emulator in C# while learning C# at the same time.

Wanted to try that after seeing  [Dave Poo](https://www.youtube.com/c/DavePoo) Youtube videos [6502 Emulator in C++](https://www.youtube.com/playlist?list=PLLwK93hM93Z13TRzPx9JqTIn33feefl37)


## Links and reference documentaion

* https://www.masswerk.at/6502/6502_instruction_set.html
* https://floooh.github.io/2019/12/13/cycle-stepped-6502.html
* https://github.com/eteran/pretendo/blob/master/doc/cpu/6502.txt#L945
* https://floooh.github.io/tiny8bit/c64-ui.html

http://www.6502.org/tutorials/interrupts.html

http://www.unusedino.de/ec64/technical/project64/mapping_c64.html




https://www.pagetable.com/c64ref/c64disasm/#D50

https://dustlayer.com/c64-architecture/2013/4/13/ram-under-rom

## VIC Stuff

https://dustlayer.com/vic-ii/2013/4/25/vic-ii-for-beginners-beyond-the-screen-rasters-cycle

http://www.zimmers.net/cbmpics/cbm/c64/vic-ii.txt
http://unusedino.de/ec64/technical/misc/vic656x/colors/
https://codebase64.org/doku.php?id=base:vicii_memory_organizing


## TODO:
- INDEXEDINDERECT tests wre not sufficient
- BIT tests do not check V flag correctly
- VIC Implementation
- CIA Implementation
- Refactor Class structure
- ....