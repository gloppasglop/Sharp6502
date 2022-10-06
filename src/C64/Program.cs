using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using HexParser;
using C6502;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;
using Silk.NET.Input;
using Silk.NET.OpenGL.Extensions.ImGui;


namespace C64
{

    class Program
    {

        private static IWindow window;
        private static GL Gl;

        private static uint Vao;
        private static uint Vertices;
        private static uint Indices;
        //private static uint Colors;
        
        
        private static readonly float[] VertexArray =
        {
            //X    Y      Z     U   V
            // Position         Texture coordinates
             1.0f,  1.0f, 0.0f, 1.0f, 1.0f, // top right
             1.0f, -1.0f, 0.0f, 1.0f, 0.0f, // bottom right
            -1.0f, -1.0f, 0.0f, 0.0f, 0.0f, // bottom left
            -1.0f,  1.0f, 0.0f, 0.0f, 1.0f  // top left
        };
        
        /* 
        private static readonly float[] VertexArray =
        {
            //X    Y      Z     U   V
             0.5f,  0.5f, 0.0f,
             0.5f, -0.5f, 0.0f,
            -0.5f, -0.5f, 0.0f,
            -0.5f,  0.5f, 0.0f
        };
        */

        private static readonly uint[] IndexArray =
        {
            0, 1, 3,
            1, 2, 3
        };

	    /*
        private static readonly uint[] IndexArray =
        {
            0, 1, 2
        };
        */

        private static Shader Shader;
        private static Texture Texture;
        private static C64.Computer c64;
        private static uint _numberOfTicks = 32833;
        private static byte[] _pixels;
        private static ImGuiController Controller = null;

        private static bool StepMode = false;
        private static bool NextStep = false;
        private static bool _debugDisplayMemory = false;

        private static LinkedList<string> _DebugExecutionStack;
        private static int _DebugExecutionStackCapacity = 10;

        private static bool _ecm = false;

        static void Main(string[] args)
        {

            c64 = new C64.Computer(
                "C:\\Users\\glopp\\Downloads\\GTK3VICE-3.4-win64-r39109\\GTK3VICE-3.4-win64-r39109\\C64\\kernal",
                "C:\\Users\\glopp\\Downloads\\GTK3VICE-3.4-win64-r39109\\GTK3VICE-3.4-win64-r39109\\C64\\basic",
                "C:\\Users\\glopp\\Downloads\\GTK3VICE-3.4-win64-r39109\\GTK3VICE-3.4-win64-r39109\\C64\\chargen");

            c64.Debug = false;

            _DebugExecutionStack = new LinkedList<string>();
            for (int i = 0; i < _DebugExecutionStackCapacity; i++ )
            {
                _DebugExecutionStack.AddFirst(" dsdjsdjjsd");
            }

            var options = WindowOptions.Default;
            options.Size = new Vector2D<int>(x: 1280,y: 720);
            options.Title = "CSharp64";
            options.VSync = true;

            window = Window.Create(options);

            //Assign events.
            window.Load += OnLoad;
            window.Update += OnUpdate;
            window.Render += OnRender;
            window.Resize += OnResize;
            window.Closing += OnClose;

            //Run the window.
            window.Run();
        }


        private static unsafe void OnLoad() {
                //Set-up input context.
                IInputContext input = window.CreateInput();
                foreach (IKeyboard keyboard in input.Keyboards)
                {
                    keyboard.KeyDown += KeyDown;
                }

                Gl = window.CreateOpenGL();

                Gl.ClearColor(red: 0.1f,green:0.2f, blue:0.3f,alpha: 1.0f);

                Controller = new ImGuiController(Gl,window,input);

                //Gl.ClearColor(red: 0.0f,green:0.0f, blue:0.0f,alpha: 1.0f);

                //Shader = new Shader(Gl, "Shaders/shader_test.vert", "Shaders/shader_test.frag");
                Shader = new Shader(Gl, "Shaders/shader.vert", "Shaders/shader.frag");

                Vao = Gl.GenVertexArray();
                Gl.BindVertexArray(Vao);

                Vertices = Gl.GenBuffer();
                // Colors = Gl.GenBuffer();
                Indices = Gl.GenBuffer();

                Gl.BindBuffer(GLEnum.ArrayBuffer,Vertices);
                Gl.BufferData(GLEnum.ArrayBuffer,(ReadOnlySpan<float>) VertexArray.AsSpan(), GLEnum.StaticDraw);
                //Gl.VertexAttribPointer(0,3,GLEnum.Float,false,5,null);
                Gl.VertexAttribPointer(0,3,GLEnum.Float,false,5*sizeof(float),null);
                Gl.EnableVertexAttribArray(0);
                Gl.VertexAttribPointer(1,2,GLEnum.Float,false,5*sizeof(float),null);
                Gl.EnableVertexAttribArray(1);

                Gl.BindBuffer(GLEnum.ElementArrayBuffer, Indices);
                Gl.BufferData(GLEnum.ElementArrayBuffer,(ReadOnlySpan<uint>) IndexArray.AsSpan(),GLEnum.StaticDraw);

		        var height = 312u;
                var width = 504u;
                _pixels = c64.Screen();

                Texture = new Texture(Gl, _pixels.AsSpan<Byte>(),width,height);
                Shader.SetUniform("uTexture0", 0);
                


        }
        private static void KeyDown(IKeyboard arg1, Key arg2, int arg3)
        {
            switch(arg2)
            {
                case Key.Escape:
                    window.Close();
                    break;
                case Key.Up:
                    _numberOfTicks++;
                    Console.WriteLine(_numberOfTicks);
                    break;
                case Key.PageUp:
                    _numberOfTicks += 1000;
                    Console.WriteLine(_numberOfTicks);
                    break;
                case Key.PageDown:
                    if (_numberOfTicks > 1001) {
                        _numberOfTicks -= 1000;
                    }
                    Console.WriteLine(_numberOfTicks);
                    break;
                case Key.Down:
                    if (_numberOfTicks > 2) {
                        _numberOfTicks--;
                    }
                    Console.WriteLine(_numberOfTicks);
                    break;
                case Key.Home:
                    _numberOfTicks = 32833;
                    Console.WriteLine(_numberOfTicks);
                    break;
                
                case Key.End:
                    c64.Debug = !c64.Debug;
                    Console.WriteLine($"Debug: {c64.Debug}");
                    break;

                case Key.S:
                    StepMode = ! StepMode; 
                    NextStep = false;
                    break;

                case Key.M:
                    _debugDisplayMemory = ! _debugDisplayMemory;
                    break;

                case Key.N:
                    if (StepMode) {
                        NextStep = true;
                    }
                    break;

                default:
                    break;

            }

        }

        private static void OnUpdate(double delta)
        {
            //return;
            //numberOfTicks = (uint) (985000.0*delta);
            if (!StepMode)
            {
                for (int i = 0; i < _numberOfTicks; i++)
	            {
		            c64.Tick();
                    if (c64.Cpu.PC == 0xFF48) {
                        StepMode = true;
                    }
                    _DebugExecutionStack.AddFirst(String.Format("{0,20} {11,1} {10,2:X2} {1,8:X4} {2,4:X2} {3,2} {4,6:X4} {5,4:X2} {6,4:X2} {7,4:X2} {8,4:X2} {9}", 
                        c64.TickCount,
                        c64.Cpu.AddrPins, 
                        c64.Cpu.DataPins, 
                        Convert.ToInt32(c64.Cpu.RW),
                        c64.Cpu.PC,
                        c64.Cpu.A,
                        c64.Cpu.X,
                        c64.Cpu.Y,
                        c64.Cpu.S,
                        Convert.ToString(c64.Cpu.P,2).PadLeft(8,'0'),
                        c64.Cpu.IR?.Opcode,
                        c64.Cpu._opcycle
                    ));

                    // TODO: Should I put this in C64 class
                    _DebugExecutionStack.RemoveLast();

	            }
            } else  {
                if (NextStep)
                {
		            c64.Tick();
                    _DebugExecutionStack.AddFirst(String.Format("{0,20} {11,1} {10,2:X2} {1,8:X4} {2,4:X2} {3,2} {4,6:X4} {5,4:X2} {6,4:X2} {7,4:X2} {8,4:X2} {9}", 
                        c64.TickCount,
                        c64.Cpu.AddrPins, 
                        c64.Cpu.DataPins, 
                        Convert.ToInt32(c64.Cpu.RW),
                        c64.Cpu.PC,
                        c64.Cpu.A,
                        c64.Cpu.X,
                        c64.Cpu.Y,
                        c64.Cpu.S,
                        Convert.ToString(c64.Cpu.P,2).PadLeft(8,'0'),
                        c64.Cpu.IR?.Opcode,
                        c64.Cpu._opcycle
                    ));
                    _DebugExecutionStack.RemoveLast();
                    NextStep = false;
                }
            }

            /*
            if (delta > 1.0/20) {
                Console.WriteLine($"FPS : {1/delta}, Ticks: {_numberOfTicks}");

            }
            */
        }
        
        private static void DumpState()
	    {

            ImGuiNET.ImGui.Text($"CIA1 Timer A: {c64.CIA1.TAHI*256 + c64.CIA1.TALO}");
            ImGuiNET.ImGui.Text($"CIA1 Timer B: {c64.CIA1.TBHI*256 + c64.CIA1.TBLO}");
            foreach (string dbg in _DebugExecutionStack)
            {
                ImGuiNET.ImGui.Text(dbg);
            }
        }

        private static void DumpMemory(uint address,uint size, uint width)
	    {
            ImGuiNET.ImGui.Begin("Memory");
            for (uint i = 0; i < size; i++)
            {
                if ( i % width != 0)
                {
                    ImGuiNET.ImGui.SameLine();

                }
                ImGuiNET.ImGui.Text(String.Format("{0,2:X2}",c64.Mem.Read(address+i)));
            }
            ImGuiNET.ImGui.End();
        }

	
        private static void DumpVic()
        {
            
            ImGuiNET.ImGui.Begin("VIC");
            ImGuiNET.ImGui.BeginGroup();
            ImGuiNET.ImGui.Text("$D000 Sprite #0 X-coordinate                "+c64.Mem.Read(0xD000));
            ImGuiNET.ImGui.Text("$D001 Sprite #0 Y-coordinate                "+c64.Mem.Read(0xD001));
            ImGuiNET.ImGui.Text("$D002 Sprite #1 X-coordinate                "+c64.Mem.Read(0xD002));
            ImGuiNET.ImGui.Text("$D003 Sprite #1 Y-coordinate                "+c64.Mem.Read(0xD003));
            ImGuiNET.ImGui.Text("$D004 Sprite #2 X-coordinate                "+c64.Mem.Read(0xD004));
            ImGuiNET.ImGui.Text("$D005 Sprite #2 Y-coordinate                "+c64.Mem.Read(0xD005));
            ImGuiNET.ImGui.Text("$D006 Sprite #3 X-coordinate                "+c64.Mem.Read(0xD006));
            ImGuiNET.ImGui.Text("$D007 Sprite #3 Y-coordinate                "+c64.Mem.Read(0xD007));
            ImGuiNET.ImGui.Text("$D008 Sprite #4 X-coordinate                "+c64.Mem.Read(0xD008));
            ImGuiNET.ImGui.Text("$D009 Sprite #4 Y-coordinate                "+c64.Mem.Read(0xD009));
            ImGuiNET.ImGui.Text("$D00A Sprite #5 X-coordinate                "+c64.Mem.Read(0xD00A));
            ImGuiNET.ImGui.Text("$D00B Sprite #5 Y-coordinate                "+c64.Mem.Read(0xD00B));
            ImGuiNET.ImGui.Text("$D00C Sprite #6 X-coordinate                "+c64.Mem.Read(0xD00C));
            ImGuiNET.ImGui.Text("$D00D Sprite #6 Y-coordinate                "+c64.Mem.Read(0xD00D));
            ImGuiNET.ImGui.Text("$D00E Sprite #7 X-coordinate                "+c64.Mem.Read(0xD00E));
            ImGuiNET.ImGui.Text("$D00F Sprite #7 Y-coordinate                "+c64.Mem.Read(0xD00F));
            ImGuiNET.ImGui.EndGroup();
            ImGuiNET.ImGui.SameLine();
            ImGuiNET.ImGui.BeginGroup();
            ImGuiNET.ImGui.Text("$D010 Sprite #0-#7 X-coordinates            "+c64.Mem.Read(0xD010));
            ImGuiNET.ImGui.Text("$D011 Screen control register               "+c64.Mem.Read(0xD011));
            ImGuiNET.ImGui.Text("$D012 Raster line                           "+c64.Mem.Read(0xD012));
            ImGuiNET.ImGui.Text("$D013 Light pen X-coordinate                "+c64.Mem.Read(0xD013));
            ImGuiNET.ImGui.Text("$D014 Light pen Y-coordinate                "+c64.Mem.Read(0xD014));
            ImGuiNET.ImGui.Text("$D015 Sprite enable register                "+c64.Mem.Read(0xD015));
            ImGuiNET.ImGui.Text("$D016 Screen control register #2            "+c64.Mem.Read(0xD016));
            ImGuiNET.ImGui.Text("$D017 Sprite double height register         "+c64.Mem.Read(0xD017));
            ImGuiNET.ImGui.Text("$D018 Memory setup register                 "+c64.Mem.Read(0xD018));
            ImGuiNET.ImGui.Text("$D019 Interrupt status register             "+c64.Mem.Read(0xD019));
            ImGuiNET.ImGui.Text("$D01A Interrupt control register            "+c64.Mem.Read(0xD01A));
            ImGuiNET.ImGui.Text("$D01B Sprite priority register              "+c64.Mem.Read(0xD01B));
            ImGuiNET.ImGui.Text("$D01C Sprite multicolor mode register       "+c64.Mem.Read(0xD01C));
            ImGuiNET.ImGui.Text("$D01D Sprite double width register          "+c64.Mem.Read(0xD01D));
            ImGuiNET.ImGui.Text("$D01E Sprite-sprite collision register      "+c64.Mem.Read(0xD01E));
            ImGuiNET.ImGui.Text("$D01F Sprite-background collision register  "+c64.Mem.Read(0xD01F));
            ImGuiNET.ImGui.Text("$D020 Border color                          "+c64.Mem.Read(0xD020));
            ImGuiNET.ImGui.Text("$D021 Background color                      "+c64.Mem.Read(0xD021));
            ImGuiNET.ImGui.Text("$D022 Extra background color #1             "+c64.Mem.Read(0xD022));
            ImGuiNET.ImGui.Text("$D023 Extra background color #2             "+c64.Mem.Read(0xD023));
            ImGuiNET.ImGui.Text("$D024 Extra background color #3             "+c64.Mem.Read(0xD024));
            /*
            ImGuiNET.ImGui.Text("$D025 Sprite extra color #1                 "+c64.Mem.Read(0xD025));
            ImGuiNET.ImGui.Text("$D026 Sprite extra color #2                 "+c64.Mem.Read(0xD026));
            ImGuiNET.ImGui.Text("$D027 Sprite #0 color                       "+c64.Mem.Read(0xD027));
            ImGuiNET.ImGui.Text("$D028 Sprite #1 color                       "+c64.Mem.Read(0xD028));
            ImGuiNET.ImGui.Text("$D029 Sprite #2 color                       "+c64.Mem.Read(0xD029));
            ImGuiNET.ImGui.Text("$D02A Sprite #3 color                       "+c64.Mem.Read(0xD02A));
            ImGuiNET.ImGui.Text("$D02B Sprite #4 color                       "+c64.Mem.Read(0xD02B));
            ImGuiNET.ImGui.Text("$D02C Sprite #5 color                       "+c64.Mem.Read(0xD02C));
            ImGuiNET.ImGui.Text("$D02D Sprite #6 color                       "+c64.Mem.Read(0xD02D));
            ImGuiNET.ImGui.Text("$D02E Sprite #7 color                       "+c64.Mem.Read(0xD02E));
            */
            ImGuiNET.ImGui.EndGroup();
            ImGuiNET.ImGui.Text("Raster X: "+c64.Vic._x.ToString("X3"));
            ImGuiNET.ImGui.SameLine();
            ImGuiNET.ImGui.Text("Raster Y: "+c64.Vic._y.ToString("X3"));

            ImGuiNET.ImGui.Text("AddrPins: "+c64.Vic.AddrPins.ToString("X4"));
            ImGuiNET.ImGui.Text("DataPins: "+c64.Vic.DataPins.ToString("X2"));
            ImGuiNET.ImGui.SameLine();
            ImGuiNET.ImGui.Text("Opcycle: "+c64.Vic._opcycle);
            ImGuiNET.ImGui.Text("VMLI: "+c64.Vic._VMLI);
            ImGuiNET.ImGui.Text("VBASE: "+c64.Vic._VCBASE);
            ImGuiNET.ImGui.Text("VC: "+c64.Vic._VC);
            ImGuiNET.ImGui.Text("RC: "+c64.Vic._RC);
            ImGuiNET.ImGui.Text("Badline: "+c64.Vic._badLine);
            ImGuiNET.ImGui.Text("Displaystate: "+c64.Vic._displayState);

            foreach (uint c in c64.Vic.VideoMatrixLine) {
                ImGuiNET.ImGui.Text(c.ToString("X2"));
                ImGuiNET.ImGui.SameLine();
            }

            ImGuiNET.ImGui.End();
        }

        private static void _displayDebug (double delta) {
            //TODO:Move this to a Display Debug function
            var glyphWidth = ((float) ImGuiNET.ImGui.CalcTextSize("F").X) + 1.0f;
            ImGuiNET.ImGui.Begin("Debug");
            ImGuiNET.ImGui.Text("FPS: "+1/delta);
            ImGuiNET.ImGui.Text("MHz: "+_numberOfTicks/delta/1_000_000);

            var CR2 = c64.Mem.Read(0xD016);
            if (ImGuiNET.ImGui.RadioButton("MCM ON", (CR2 & 0x10 ) == 0x10 )) { 
                c64.Mem.Write(0xD016,CR2 | 0x10); 
            }
            ImGuiNET.ImGui.SameLine();            
            if (ImGuiNET.ImGui.RadioButton("MCM OFF", (CR2 & 0x10 ) != 0x10 )) {
                c64.Mem.Write(0xD016,(uint) (CR2 &  ~0x10) & 0xFF); 
            };

            var CR1 = c64.Mem.Read(0xD011);
            if (ImGuiNET.ImGui.RadioButton("BMM ON", (CR1 & 0x20 ) == 0x20 )) { 
                c64.Mem.Write(0xD011,CR1 | 0x20); 
            }
            ImGuiNET.ImGui.SameLine();            
            if (ImGuiNET.ImGui.RadioButton("BMM OFF", (CR1 & 0x20 ) != 0x20 )) {
                c64.Mem.Write(0xD011,(uint) (CR1 &  ~0x20) & 0xFF); 
            };

            DumpState();
            ImGuiNET.ImGui.End();

            if ( _debugDisplayMemory )
            {
                DumpMemory(0xDC00,16,16);
                DumpMemory(0xDD00,16,16);
            }

            DumpVic();

        }
        private static unsafe void OnRender(double delta)
        {

	        Controller.Update((float) delta);
            Gl.Clear(ClearBufferMask.ColorBufferBit);
            //Gl.BindVertexArray(Vao);
            Shader.Use();
            var height = 312u;
    	    var width = 504u;

            Texture = new Texture(Gl, _pixels.AsSpan<Byte>(),width,height);

            Texture.Bind(TextureUnit.Texture0);
            Shader.SetUniform("uTexture0", 0);
            Gl.DrawElements(GLEnum.Triangles,(uint) IndexArray.Length,GLEnum.UnsignedInt,null);
            /*
            Gl.Clear(ClearBufferMask.ColorBufferBit);
            Gl.BindVertexArray(Vao);
            Shader.Use();
            //Texture.Bind(TextureUyynit.Texture0);
            //Shader.SetUniform("uTexture0", 0);
            Gl.DrawElements(GLEnum.Triangles,(uint) IndexArray.Length,GLEnum.UnsignedInt,null);
            */
            Texture.Dispose();

            _displayDebug(delta);

            Controller.Render();

        }

        public static void OnResize(Vector2D<int> size) {
            Gl.Viewport(size);
        }

        public static void OnClose() 
        {

            Controller?.Dispose();
            // Unload OpenGL
            Gl?.Dispose();
        }

    }
}