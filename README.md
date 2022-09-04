

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


http://www.zimmers.net/cbmpics/cbm/c64/vic-ii.txt


https://www.pagetable.com/c64ref/c64disasm/#D50

https://dustlayer.com/c64-architecture/2013/4/13/ram-under-rom

TODO:
- DECIMAL mode

## Compile functional test

```
cd tests
..\tools\as65.exe -l -m -w -h0  .\6502_functional_test.a65
```

