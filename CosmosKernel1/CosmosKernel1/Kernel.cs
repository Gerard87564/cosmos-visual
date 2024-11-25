using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Sys = Cosmos.System;
using Cosmos.System.Graphics;
using System.Drawing;
using static System.Net.Mime.MediaTypeNames;
using System.Diagnostics;
using Cosmos.HAL.Drivers.Audio;
using Cosmos.System.Audio.IO;
using Cosmos.System.Audio;
using IL2CPU.API.Attribs;
using System.Security.Cryptography.X509Certificates;
using Cosmos.HAL.Audio;


namespace CosmosKernel1
{
    public class Kernel : Sys.Kernel
    {
        Canvas canvas;

        static string ASC16Base64 = "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AAB4zMzM2MzGxsbMAAAAAAAA/sbGwMDAwMDAwAAAAAAAAAAA/mxsbGxsbGwAAAAAAAAA/sZgMBgwYMb+AAAAAAAAAAAAftjY2NjYcAAAAAAAAAAAZmZmZmZ8YGDAAAAAAAAAAHbcGBgYGBgYAAAAAAAAAH4YPGZmZjwYfgAAAAAAAAA4bMbG/sbGbDgAAAAAAAA4bMbGxmxsbGzuAAAAAAAAHjAYDD5mZmZmPAAAAAAAAAAAAH7b29t+AAAAAAAAAAAAAwZ+29vzfmDAAAAAAAAAHDBgYHxgYGAwHAAAAAAAAAB8xsbGxsbGxsYAAAAAAAAAAP4AAP4AAP4AAAAAAAAAAAAYGH4YGAAA/wAAAAAAAAAwGAwGDBgwAH4AAAAAAAAADBgwYDAYDAB+AAAAAAAADhsbGBgYGBgYGBgYGBgYGBgYGBgYGNjY2HAAAAAAAAAAABgYAH4AGBgAAAAAAAAAAAAAdtwAdtwAAAAAAAAAOGxsOAAAAAAAAAAAAAAAAAAAAAAAABgYAAAAAAAAAAAAAAAAAAAAGAAAAAAAAAAADwwMDAwM7GxsPBwAAAAAANhsbGxsbAAAAAAAAAAAAABw2DBgyPgAAAAAAAAAAAAAAAAAfHx8fHx8fAAAAAAAAAAAAAAAAAAAAAAAAAAAAA==";
        static MemoryStream ASC16FontMS = new MemoryStream(Convert.FromBase64String(ASC16Base64));

        public void DrawACSIIString(Canvas canvas, Color color, string s, int x, int y, int scale)
        {
            string[] lines = s.Split('\n');
            for (int l = 0; l < lines.Length; l++)
            {
                for (int c = 0; c < lines[l].Length; c++)
                {
                    int offset = (Encoding.ASCII.GetBytes(lines[l][c].ToString())[0] & 0xFF) * 16;
                    ASC16FontMS.Seek(offset, SeekOrigin.Begin);
                    byte[] fontbuf = new byte[16];
                    ASC16FontMS.Read(fontbuf, 0, fontbuf.Length);

                    for (int i = 0; i < 16 * scale; i++)
                    {
                        for (int j = 0; j < 8 * scale; j++)
                        {
                            if ((fontbuf[i / scale] & (0x80 >> (j / scale))) != 0)
                            {
                                canvas.DrawPoint(color, (x + j) + (c * 8 * scale), y + i + (l * 16 * scale));
                            }
                        }
                    }
                }
            }
        }

        private readonly Sys.Graphics.Bitmap bitmap = new Sys.Graphics.Bitmap(10, 10,
                new byte[] { 0, 255, 243, 255, 0, 255, 243, 255, 0, 255, 243, 255, 0, 255, 243, 255, 0, 255, 243, 255, 0,
                    255, 243, 255, 0, 255, 243, 255, 0, 255, 243, 255, 0, 255, 243, 255, 0, 255, 243, 255, 0, 255, 243, 255,
                    0, 255, 243, 255, 0, 255, 243, 255, 0, 255, 243, 255, 0, 255, 243, 255, 0, 255, 243, 255, 0, 255, 243, 255,
                    0, 255, 243, 255, 0, 255, 243, 255, 0, 255, 243, 255, 0, 255, 243, 255, 0, 255, 243, 255, 23, 59, 88, 255,
                    23, 59, 88, 255, 0, 255, 243, 255, 0, 255, 243, 255, 23, 59, 88, 255, 23, 59, 88, 255, 0, 255, 243, 255, 0,
                    255, 243, 255, 0, 255, 243, 255, 23, 59, 88, 255, 153, 57, 12, 255, 0, 255, 243, 255, 0, 255, 243, 255, 0, 255,
                    243, 255, 0, 255, 243, 255, 153, 57, 12, 255, 23, 59, 88, 255, 0, 255, 243, 255, 0, 255, 243, 255, 0, 255, 243,
                    255, 0, 255, 243, 255, 0, 255, 243, 255, 72, 72, 72, 255, 72, 72, 72, 255, 0, 255, 243, 255, 0, 255, 243, 255, 0,
                    255, 243, 255, 0, 255, 243, 255, 0, 255, 243, 255, 0, 255, 243, 255, 0, 255, 243, 255, 0, 255, 243, 255, 72, 72,
                    72, 255, 72, 72, 72, 255, 0, 255, 243, 255, 0, 255, 243, 255, 0, 255, 243, 255, 0, 255, 243, 255, 0, 255, 243, 255,
                    10, 66, 148, 255, 0, 255, 243, 255, 0, 255, 243, 255, 0, 255, 243, 255, 0, 255, 243, 255, 0, 255, 243, 255, 0, 255,
                    243, 255, 10, 66, 148, 255, 0, 255, 243, 255, 0, 255, 243, 255, 0, 255, 243, 255, 10, 66, 148, 255, 10, 66, 148, 255,
                    10, 66, 148, 255, 10, 66, 148, 255, 10, 66, 148, 255, 10, 66, 148, 255, 0, 255, 243, 255, 0, 255, 243, 255, 0, 255,
                    243, 255, 0, 255, 243, 255, 0, 255, 243, 255, 10, 66, 148, 255, 10, 66, 148, 255, 10, 66, 148, 255, 10, 66, 148,
                    255, 0, 255, 243, 255, 0, 255, 243, 255, 0, 255, 243, 255, 0, 255, 243, 255, 0, 255, 243, 255, 0, 255, 243, 255,
                    0, 255, 243, 255, 0, 255, 243, 255, 0, 255, 243, 255, 0, 255, 243, 255, 0, 255, 243, 255, 0, 255, 243, 255, 0, 255, 243, 255, }, ColorDepth.ColorDepth32);

        Sys.FileSystem.CosmosVFS fs = new Cosmos.System.FileSystem.CosmosVFS();
        [ManifestResourceStream(ResourceName = "CosmosKernel1.walking_polar_bear.wav")] public static byte[] music;
        protected override void BeforeRun()
        {
            Sys.FileSystem.VFS.VFSManager.RegisterVFS(fs);
            Sys.KeyboardManager.SetKeyLayout(new Sys.ScanMaps.ESStandardLayout());
            Console.WriteLine("Cosmos booted successfully. Let's go in Graphical Mode");
           
            var mixer = new AudioMixer();
            var audioStream = new MemoryAudioStream(new Cosmos.HAL.Audio.SampleFormat(AudioBitDepth.Bits16, 2, true), 48000, music);
            var driver = AC97.Initialize(bufferSize: 4096);
            mixer.Streams.Add(audioStream);
            var audioManager = new AudioManager()
            {
                Stream = mixer,
                Output = driver
            };
            audioManager.Enable();
            canvas = FullScreenCanvas.GetFullScreenCanvas(new Mode(640, 480, ColorDepth.ColorDepth32));
            canvas.Clear(Color.Blue);
        }

        protected override void Run()
        {
            int cursorY = 10;
            int initialCursorX = 10;
            int cursorX = initialCursorX;
            string inputText = "";
            bool inputActive = true;
            bool exit = false;
            string currentDirectory = @"0:\";

            DrawACSIIString(canvas, Color.White, "======================================", initialCursorX, cursorY, 2);
            DrawACSIIString(canvas, Color.White, "                                  ", initialCursorX, cursorY += 16, 2);
            DrawACSIIString(canvas, Color.White, "          GerardOS                ", initialCursorX, cursorY += 16, 3);
            DrawACSIIString(canvas, Color.White, "                                  ", initialCursorX, cursorY += 16, 2);
            DrawACSIIString(canvas, Color.White, "======================================", initialCursorX, cursorY += 16, 2);
            canvas.Display();

            while (!exit)
            {
                while (inputActive)
                {
                    var key = Console.ReadKey(intercept: true);

                    if (key.Key == ConsoleKey.Enter)
                    {
                        inputActive = false;
                    }
                    else if (key.Key == ConsoleKey.Backspace)
                    {
                        if (inputText.Length > 0)
                        {
                            inputText = inputText.Substring(0, inputText.Length - 1);
                            cursorX = initialCursorX + (inputText.Length * 8);
                        }
                    }
                    else
                    {
                        inputText += key.KeyChar;
                        cursorX = initialCursorX + (inputText.Length * 8);
                    }

                    canvas.Clear(Color.Blue);
                    cursorY = 10;
                    DrawACSIIString(canvas, Color.White, "Ingresa una comanda: " + inputText, initialCursorX, cursorY+=32, 1);
                    canvas.Display();
                }

                var commandParts = inputText.Trim().Split(' ');

                switch (commandParts[0])
                {
                    case "help":
                        ShowHelp(cursorX, cursorY + 16);
                        break;
                    case "about":
                        DrawACSIIString(canvas, Color.White, "This is a SO created and developed with Cosmos ", initialCursorX, cursorY += 16, 1);
                        break;
                    case "apagar":
                        if (commandParts.Length > 1 && commandParts[1] == "-a")
                            Cosmos.System.Power.Shutdown();
                        exit = true;
                        break;
                    case "reinicia":
                        Cosmos.System.Power.Reboot();
                        exit = true;
                        break;
                    case "delete":
                        break;
                    case "crear":
                        break;

                    case "llistam":
                        if (Directory.Exists(currentDirectory))
                        {
                            DrawACSIIString(canvas, Color.Green, "Directori actual: " + currentDirectory, initialCursorX, cursorY += 16, 1);

                            var directoryList = Directory.GetDirectories(currentDirectory);

                            if (directoryList.Length > 0)
                            {
                                foreach (var directory in directoryList)
                                {
                                    DrawACSIIString(canvas, Color.White, directory, initialCursorX, cursorY += 16, 1);
                                }
                            }
                            else
                            {
                                DrawACSIIString(canvas, Color.Yellow, "No hi ha subdirectoris en aquest directori.", initialCursorX, cursorY += 16, 1);
                            }
                        }
                        else
                        {
                            DrawACSIIString(canvas, Color.Red, "Error: Directori actual no trobat.", initialCursorX, cursorY += 16, 1);
                        }
                        break;
                    case "espai":
                        var available_space = fs.GetAvailableFreeSpace(@"0:\");
                        DrawACSIIString(canvas, Color.White, "Available Free Space: " + available_space, initialCursorX, cursorY += 16, 1);
                        break;

                    case "sysdisk":
                        var fs_type = fs.GetFileSystemType(@"0:\");
                        DrawACSIIString(canvas, Color.White, "File System Type: " + fs_type, initialCursorX, cursorY += 16, 1);
                        break;

                    case "files":
                        bool inputActive2 = true;
                        string inputText2 = "";
                        while (inputActive2)
                        {
                            DrawACSIIString(canvas, Color.White, "Introdueix nom del directori a mostrar el llistat dels seus fitxers: " + inputText2, initialCursorX, cursorY += 16, 1);
                            canvas.Display();
                            var key = Console.ReadKey(intercept: true);

                            if (key.Key == ConsoleKey.Enter)
                            {
                                inputActive2 = false;
                            }
                            else if (key.Key == ConsoleKey.Backspace)
                            {
                                if (inputText2.Length > 0)
                                {
                                    inputText2 = inputText2.Substring(0, inputText2.Length - 1);
                                    cursorX = initialCursorX + (inputText2.Length * 8);
                                }
                            }
                            else
                            {
                                inputText2 += key.KeyChar;
                                cursorX = initialCursorX + (inputText2.Length * 8);
                            }
                            canvas.Clear(Color.Blue);
                            cursorY = 10;
                            canvas.Display();
                        }

                        var dir = @"0:\" + inputText2;
                        var files_list = Directory.GetFiles(dir);

                        foreach (var file in files_list)
                        {
                            DrawACSIIString(canvas, Color.White, "Nom del fitxer: " + file, initialCursorX, cursorY += 16, 1);
                        }
                        break;

                    case "nfile":
                        bool inputActive3 = true;
                        string inputText3 = "";
                        while (inputActive3)
                        {
                            DrawACSIIString(canvas, Color.White, "Introdueix nom del nou fitxer: " + inputText3, initialCursorX, cursorY += 16, 1);
                            canvas.Display();
                            var key = Console.ReadKey(intercept: true);

                            if (key.Key == ConsoleKey.Enter)
                            {
                                inputActive3 = false;
                            }
                            else if (key.Key == ConsoleKey.Backspace)
                            {
                                if (inputText3.Length > 0)
                                {
                                    inputText3 = inputText3.Substring(0, inputText3.Length - 1);
                                    cursorX = initialCursorX + (inputText3.Length * 8);
                                }
                            }
                            else
                            {
                                inputText3 += key.KeyChar;
                                cursorX = initialCursorX + (inputText3.Length * 8);
                            }
                            canvas.Clear(Color.Blue);
                            cursorY = 10;
                            canvas.Display();
                        }

                        var filePath = @"0:\" + inputText3;
                        try
                        {
                            var file_stream = File.Create(filePath);
                            DrawACSIIString(canvas, Color.White, $"Fitxer '{inputText3}' creat correctament.", initialCursorX, cursorY += 16, 1);
                        }
                        catch (Exception e)
                        {
                            DrawACSIIString(canvas, Color.White, $"Error al crear el fitxer '{e.Message}'", initialCursorX, cursorY += 16, 1);
                        }
                        break;

                    case "ndir":
                        bool inputActive4 = true;
                        string inputText4 = "";
                        while (inputActive4)
                        {
                            DrawACSIIString(canvas, Color.White, "Introdueix nom del nou directori: " + inputText4, initialCursorX, cursorY += 16, 1);
                            canvas.Display();
                            var key = Console.ReadKey(intercept: true);

                            if (key.Key == ConsoleKey.Enter)
                            {
                                inputActive4 = false;
                            }
                            else if (key.Key == ConsoleKey.Backspace)
                            {
                                if (inputText4.Length > 0)
                                {
                                    inputText4 = inputText4.Substring(0, inputText4.Length - 1);
                                    cursorX = initialCursorX + (inputText4.Length * 8);
                                }
                            }
                            else
                            {
                                inputText4 += key.KeyChar;
                                cursorX = initialCursorX + (inputText4.Length * 8);
                            }
                            canvas.Clear(Color.Blue);
                            cursorY = 10;
                            canvas.Display();
                        }

                        var filePathdir = @"0:\" + inputText4;
                        try
                        {
                            Directory.CreateDirectory(filePathdir);
                            DrawACSIIString(canvas, Color.White, $"Directori '{inputText4}' creat correctament.", initialCursorX, cursorY += 16, 1);
                        }
                        catch (Exception e)
                        {
                            DrawACSIIString(canvas, Color.White, $"Error al crear el directori: {e.Message}", initialCursorX, cursorY += 16, 1);
                        }
                        break;

                    case "cambiarDir": 
                        bool inputactiveCD = true;
                        string newDirectory = "";

                        while (inputactiveCD)
                        {
                            DrawACSIIString(canvas, Color.White, "Introdueix nom del directori: " + newDirectory, initialCursorX, cursorY += 16, 1);
                            canvas.Display();

                            var key = Console.ReadKey(intercept: true);

                            if (key.Key == ConsoleKey.Enter)
                            {
                                inputactiveCD = false;
                            }
                            else if (key.Key == ConsoleKey.Backspace)
                            {
                                if (newDirectory.Length > 0)
                                {
                                    newDirectory = newDirectory.Substring(0, newDirectory.Length - 1);
                                    cursorX = initialCursorX + (newDirectory.Length * 8);
                                }
                            }
                            else
                            {
                                newDirectory += key.KeyChar;
                                cursorX = initialCursorX + (newDirectory.Length * 8);
                            }

                            canvas.Clear(Color.Blue);
                            cursorY = 10;
                            canvas.Display();
                        }

                        string potentialPath = Path.Combine(currentDirectory, newDirectory);

                        if (Directory.Exists(potentialPath))
                        {
                            currentDirectory = potentialPath;
                            DrawACSIIString(canvas, Color.Green, "Canviat al directori: " + currentDirectory, initialCursorX, cursorY += 16, 1);
                        }
                        else
                        {
                            DrawACSIIString(canvas, Color.Red, "Error: Directori no trobat.", initialCursorX, cursorY += 16, 1);
                        }
                        break;
                    case "delfdir":
                        bool inputActive5 = true;
                        string inputText5 = "";
                        while (inputActive5)
                        {
                            DrawACSIIString(canvas, Color.White, "Vols borrar fitxer o directori (f-d): " + inputText5, initialCursorX, cursorY += 16, 1);
                            canvas.Display();
                            var key = Console.ReadKey(intercept: true);

                            if (key.Key == ConsoleKey.Enter)
                            {
                                inputActive5 = false;
                            }
                            else if (key.Key == ConsoleKey.Backspace)
                            {
                                if (inputText5.Length > 0)
                                {
                                    inputText5 = inputText5.Substring(0, inputText5.Length - 1);
                                    cursorX = initialCursorX + (inputText5.Length * 8);
                                }
                            }
                            else
                            {
                                inputText5 += key.KeyChar;
                                cursorX = initialCursorX + (inputText5.Length * 8);
                            }
                            canvas.Clear(Color.Blue);
                            cursorY = 10;
                            canvas.Display();
                        }

                        if (inputText5 == "f")
                        {
                            bool inputActiveFile = true;
                            string inputTextFile = "";

                            while (inputActiveFile)
                            {
                                DrawACSIIString(canvas, Color.White, "Escriu el nom del fitxer a borrar: " + inputTextFile, initialCursorX, cursorY += 16, 1);
                                canvas.Display();
                                var key = Console.ReadKey(intercept: true);

                                if (key.Key == ConsoleKey.Enter)
                                {
                                    inputActiveFile = false;
                                }
                                else if (key.Key == ConsoleKey.Backspace)
                                {
                                    if (inputTextFile.Length > 0)
                                    {
                                        inputTextFile = inputTextFile.Substring(0, inputTextFile.Length - 1);
                                        cursorX = initialCursorX + (inputTextFile.Length * 8);
                                    }
                                }
                                else
                                {
                                    inputTextFile += key.KeyChar;
                                    cursorX = initialCursorX + (inputTextFile.Length * 8);
                                }
                                canvas.Clear(Color.Blue);
                                cursorY = 10;
                                canvas.Display();
                            }
                            
                            var filePathdelete = @"0:\" + inputTextFile;
                            try
                            {
                                File.Delete(filePathdelete);
                                DrawACSIIString(canvas, Color.White, $"Fitxer '{inputTextFile}' esborrat correctament.", initialCursorX, cursorY += 16, 1);
                            }
                            catch (Exception e)
                            {
                                DrawACSIIString(canvas, Color.White, $"Error esborrant el fitxer: {e.Message}", initialCursorX, cursorY += 16, 1);
                            }
                        }
                        else if (inputText5 == "d")
                        {
                            bool inputActiveDir = true;
                            string inputTextDir = "";

                            while (inputActiveDir)
                            {
                                DrawACSIIString(canvas, Color.White, "Escriu el nom del directori a borrar: " + inputTextDir, initialCursorX, cursorY += 16, 1);
                                canvas.Display();
                                var key = Console.ReadKey(intercept: true);

                                if (key.Key == ConsoleKey.Enter)
                                {
                                    inputActiveDir = false;
                                }
                                else if (key.Key == ConsoleKey.Backspace)
                                {
                                    if (inputTextDir.Length > 0)
                                    {
                                        inputTextDir = inputTextDir.Substring(0, inputTextDir.Length - 1);
                                        cursorX = initialCursorX + (inputTextDir.Length * 8);
                                    }
                                }
                                else
                                {
                                    inputTextDir += key.KeyChar;
                                    cursorX = initialCursorX + (inputTextDir.Length * 8);
                                }
                                canvas.Clear(Color.Blue);
                                cursorY = 10;
                                canvas.Display();
                            }

                            var dirPathdelete = @"0:\" + inputTextDir;
                            try
                            {
                                Directory.Delete(dirPathdelete);
                                DrawACSIIString(canvas, Color.White, $"Directori '{inputTextDir}' esborrat correctament.", initialCursorX, cursorY += 16, 1);
                            }
                            catch (Exception e)
                            {
                                DrawACSIIString(canvas, Color.White, $"Error esborrant el directori: {e.Message}", initialCursorX, cursorY += 16, 1);
                            }
                        }
                        else
                        {
                            DrawACSIIString(canvas, Color.White, "Opció no vàlida. Introdueix 'f' per fitxer o 'd' per directori.", initialCursorX, cursorY += 16, 1);
                        }
                        break;

                    case "wtofile":
                        bool inputActive6 = true;
                        string inputText6 = "";

                        while (inputActive6)
                        {
                            DrawACSIIString(canvas, Color.White, "Escriu el nom del fitxer a escriure: " + inputText6, initialCursorX, cursorY += 16, 1);
                            canvas.Display();
                            var key = Console.ReadKey(intercept: true);

                            if (key.Key == ConsoleKey.Enter)
                            {
                                inputActive6 = false;
                            }
                            else if (key.Key == ConsoleKey.Backspace)
                            {
                                if (inputText6.Length > 0)
                                {
                                    inputText6 = inputText6.Substring(0, inputText6.Length - 1);
                                    cursorX = initialCursorX + (inputText6.Length * 8);
                                }
                            }
                            else
                            {
                                inputText6 += key.KeyChar;
                                cursorX = initialCursorX + (inputText6.Length * 8);
                            }
                            canvas.Clear(Color.Blue);
                            cursorY = 10;
                            canvas.Display();
                        }
                        
                        var filewrute = @"0:\" + inputText6;
                        bool inputTextActive = true;
                        string inputContent = "";
                        while (inputTextActive)
                        {
                            DrawACSIIString(canvas, Color.White, "Escriu el que vols escriure:" + inputContent, initialCursorX, cursorY += 16, 1);
                            canvas.Display();
                            var key = Console.ReadKey(intercept: true);

                            if (key.Key == ConsoleKey.Enter)
                            {
                                inputTextActive = false;
                            }
                            else if (key.Key == ConsoleKey.Backspace)
                            {
                                if (inputContent.Length > 0)
                                {
                                    inputContent = inputContent.Substring(0, inputContent.Length - 1);
                                    cursorX = initialCursorX + (inputContent.Length * 8);
                                }
                            }
                            else
                            {
                                inputContent += key.KeyChar;
                                cursorX = initialCursorX + (inputContent.Length * 8);
                            }
                            canvas.Clear(Color.Blue);
                            cursorY = 10;
                            canvas.Display();
                        }

                        try
                        {
                            File.WriteAllText(filewrute, inputContent);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.ToString());
                        }
                        break;

                    case "mvfile":
                        bool inputActive7= true;
                        string filemv = "";
                        while (inputActive7)
                        {
                            DrawACSIIString(canvas, Color.White, "Introdueix la ruta del fitxer a moure: " + filemv, initialCursorX, cursorY += 16, 1);
                            canvas.Display();
                            var key = Console.ReadKey(intercept: true);

                            if (key.Key == ConsoleKey.Enter)
                            {
                                inputActive7 = false;
                            }
                            else if (key.Key == ConsoleKey.Backspace)
                            {
                                if (filemv.Length > 0)
                                {
                                    filemv = filemv.Substring(0, filemv.Length - 1);
                                    cursorX = initialCursorX + (filemv.Length * 8);
                                }
                            }
                            else
                            {
                                filemv += key.KeyChar;
                                cursorX = initialCursorX + (filemv.Length * 8);
                            }
                            canvas.Clear(Color.Blue);
                            cursorY = 10;
                            canvas.Display();
                        }

                        var filemvrute = @"0:\" + filemv;

                        bool inputActiveRuta = true;
                        string newpath = "";

                        while (inputActiveRuta)
                        {
                            DrawACSIIString(canvas, Color.White, "Introdueix la ruta final:  " + newpath, initialCursorX, cursorY += 16, 1);
                            canvas.Display();
                            var key = Console.ReadKey(intercept: true);

                            if (key.Key == ConsoleKey.Enter)
                            {
                                inputActiveRuta = false;
                            }
                            else if (key.Key == ConsoleKey.Backspace)
                            {
                                if (newpath.Length > 0)
                                {
                                    newpath = newpath.Substring(0, newpath.Length - 1);
                                    cursorX = initialCursorX + (newpath.Length * 8);
                                }
                            }
                            else
                            {
                                newpath += key.KeyChar;
                                cursorX = initialCursorX + (newpath.Length * 8);
                            }
                            canvas.Clear(Color.Blue);
                            cursorY = 10;
                            canvas.Display();
                        }

                        var newpath2 = @"0:\" + newpath + @"\" + filemv;
                        MoveFile(filemvrute, newpath2);
                        break;

                    case "rfile":
                        bool inputActive8 = true;
                        string rfile = "";

                        while (inputActive8)
                        {
                            DrawACSIIString(canvas, Color.White, "Introdueix el nom del fitxer a llegir tot el seu contingut: " + rfile, initialCursorX, cursorY += 16, 1);
                            canvas.Display();
                            var key = Console.ReadKey(intercept: true);

                            if (key.Key == ConsoleKey.Enter)
                            {
                                inputActive8 = false;
                            }
                            else if (key.Key == ConsoleKey.Backspace)
                            {
                                if (rfile.Length > 0)
                                {
                                    rfile = rfile.Substring(0, rfile.Length - 1);
                                    cursorX = initialCursorX + (rfile.Length * 8);
                                }
                            }
                            else
                            {
                                rfile += key.KeyChar;
                                cursorX = initialCursorX + (rfile.Length * 8);
                            }
                            canvas.Clear(Color.Blue);
                            cursorY = 10;
                            canvas.Display();
                        }
                        var readfile = @"0:\" + rfile;
                        try
                        {
                            DrawACSIIString(canvas, Color.White, File.ReadAllText(readfile), initialCursorX, cursorY += 16, 1);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.ToString());
                        }
                        break;

                    case "rbytesf":
                        bool inputActive9= true;
                        string rbytesf = "";

                        while (inputActive9)
                        {
                            DrawACSIIString(canvas, Color.White, "Introdueix el nom del fitxer a llegir els seus bytes: " + rbytesf, initialCursorX, cursorY += 16, 1);
                            canvas.Display();
                            var key = Console.ReadKey(intercept: true);

                            if (key.Key == ConsoleKey.Enter)
                            {
                                inputActive9 = false;
                            }
                            else if (key.Key == ConsoleKey.Backspace)
                            {
                                if (rbytesf.Length > 0)
                                {
                                    rbytesf = rbytesf.Substring(0, rbytesf.Length - 1);
                                    cursorX = initialCursorX + (rbytesf.Length * 8);
                                }
                            }
                            else
                            {
                                rbytesf += key.KeyChar;
                                cursorX = initialCursorX + (rbytesf.Length * 8);
                            }
                            canvas.Clear(Color.Blue);
                            cursorY = 10;
                            canvas.Display();
                        }

                        var readfilebyte = @"0:\" + rbytesf;
                        try
                        {
                            var fileContent = File.ReadAllBytes(readfilebyte);
                            var bits = BitConverter.ToString(fileContent).Replace("-", "");
                            DrawACSIIString(canvas, Color.White, bits, initialCursorX, cursorY += 16, 1);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.ToString());
                        }
                        break;

                    case "opMat":
                        bool inputActive10= true;
                        string operacio = "";

                        while (inputActive10)
                        {
                            DrawACSIIString(canvas, Color.White, "Introdueix el signe de operacio que vols escollir: " + operacio, initialCursorX, cursorY += 16, 1);
                            canvas.Display();
                            var key = Console.ReadKey(intercept: true);

                            if (key.Key == ConsoleKey.Enter)
                            {
                                inputActive10 = false;
                            }
                            else if (key.Key == ConsoleKey.Backspace)
                            {
                                if (operacio.Length > 0)
                                {
                                    operacio = operacio.Substring(0, operacio.Length - 1);
                                    cursorX = initialCursorX + (operacio.Length * 8);
                                }
                            }
                            else
                            {
                                operacio += key.KeyChar;
                                cursorX = initialCursorX + (operacio.Length * 8);
                            }
                            canvas.Clear(Color.Blue);
                            cursorY = 10;
                            canvas.Display();
                        }

                        switch (operacio)
                        {
                            case "x":
                                bool inputActiveMultiplicacio = true;
                                string numMult = "";
                                string numMult2 = "";
                                var count = 0;
                                while (inputActiveMultiplicacio)
                                {
                                    if (count==0)
                                    {
                                        DrawACSIIString(canvas, Color.White, "Introdueix el primer numero: " + numMult, initialCursorX, cursorY += 16, 1);
                                        canvas.Display();
                                    } else if (count>0)
                                    {
                                        DrawACSIIString(canvas, Color.White, "Introdueix el segon numero: " + numMult2, initialCursorX, cursorY += 16, 1);
                                        canvas.Display();
                                    }
                                    var key = Console.ReadKey(intercept: true);

                                    if (key.Key == ConsoleKey.Enter)
                                    {
                                        count++;
                                        if (count>1) {
                                            inputActiveMultiplicacio = false;
                                        }
                                    }
                                    else if (key.Key == ConsoleKey.Backspace)
                                    {
                                        if (count == 0 && numMult.Length > 0)
                                        {
                                            numMult = numMult.Substring(0, numMult.Length - 1);
                                            cursorX = initialCursorX + (numMult.Length * 8);
                                        }

                                        if (count > 0 && numMult2.Length > 0)
                                        {
                                            numMult2 = numMult2.Substring(0, numMult2.Length - 1);
                                            cursorX = initialCursorX + (numMult2.Length * 8);
                                        }
                                    }
                                    else
                                    {
                                        if (count == 0)
                                        {
                                            numMult += key.KeyChar;
                                            cursorX = initialCursorX + (numMult.Length * 8);
                                        } else if (count>0)
                                        {
                                            numMult2 += key.KeyChar;
                                            cursorX = initialCursorX + (numMult2.Length * 8);
                                        }
                                    }
                                    canvas.Clear(Color.Blue);
                                    cursorY = 10;
                                    canvas.Display();
                                }

                                var res = int.Parse(numMult);
                                var res2 = int.Parse(numMult2);
                                var resF = res * res2;
                                DrawACSIIString(canvas, Color.White, "El resultat és: " + resF, initialCursorX, cursorY += 16, 1);
                                break;
                            
                            case "+":
                                bool inputActiveSuma = true;
                                string numSum = "";
                                string numSum2 = "";
                                var count2 = 0;
                                while (inputActiveSuma)
                                {
                                    if (count2 == 0)
                                    {
                                        DrawACSIIString(canvas, Color.White, "Introdueix el primer numero: " + numSum, initialCursorX, cursorY += 16, 1);
                                        canvas.Display();
                                    }
                                    else if (count2 > 0)
                                    {
                                        DrawACSIIString(canvas, Color.White, "Introdueix el segon numero: " + numSum2, initialCursorX, cursorY += 16, 1);
                                        canvas.Display();
                                    }
                                    var key = Console.ReadKey(intercept: true);

                                    if (key.Key == ConsoleKey.Enter)
                                    {
                                        count2++;
                                        if (count2 > 1)
                                        {
                                            inputActiveSuma = false;
                                        }
                                    }
                                    else if (key.Key == ConsoleKey.Backspace)
                                    {
                                        if (count2 == 0 && numSum.Length > 0)
                                        {
                                            numSum = numSum.Substring(0, numSum.Length - 1);
                                            cursorX = initialCursorX + (numSum.Length * 8);
                                        }

                                        if (count2 > 0 && numSum2.Length > 0)
                                        {
                                            numSum2 = numSum2.Substring(0, numSum2.Length - 1);
                                            cursorX = initialCursorX + (numSum2.Length * 8);
                                        }
                                    }
                                    else
                                    {
                                        if (count2 == 0)
                                        {
                                            numSum += key.KeyChar;
                                            cursorX = initialCursorX + (numSum.Length * 8);
                                        }
                                        else if (count2 > 0)
                                        {
                                            numSum2 += key.KeyChar;
                                            cursorX = initialCursorX + (numSum2.Length * 8);
                                        }
                                    }
                                    canvas.Clear(Color.Blue);
                                    cursorY = 10;
                                    canvas.Display();
                                }

                                var resSum = int.Parse(numSum);
                                var resSum2 = int.Parse(numSum2);
                                var resSumF = resSum + resSum2;
                                DrawACSIIString(canvas, Color.White, "El resultat és: " + resSumF, initialCursorX, cursorY += 16, 1);
                                break;

                            case "-":
                                bool inputActiveRes = true;
                                string numRes = "";
                                string numRes2 = "";
                                var count3 = 0;
                                while (inputActiveRes)
                                {
                                    if (count3 == 0)
                                    {
                                        DrawACSIIString(canvas, Color.White, "Introdueix el primer numero: " + numRes, initialCursorX, cursorY += 16, 1);
                                        canvas.Display();
                                    }
                                    else if (count3 > 0)
                                    {
                                        DrawACSIIString(canvas, Color.White, "Introdueix el segon numero: " + numRes2, initialCursorX, cursorY += 16, 1);
                                        canvas.Display();
                                    }
                                    var key = Console.ReadKey(intercept: true);

                                    if (key.Key == ConsoleKey.Enter)
                                    {
                                        count3++;
                                        if (count3 > 1)
                                        {
                                            inputActiveRes = false;
                                        }
                                    }
                                    else if (key.Key == ConsoleKey.Backspace)
                                    {
                                        if (count3 == 0 && numRes.Length > 0)
                                        {
                                            numRes = numRes.Substring(0, numRes.Length - 1);
                                            cursorX = initialCursorX + (numRes.Length * 8);
                                        }

                                        if (count3 > 0 && numRes2.Length > 0)
                                        {
                                            numRes2 = numRes2.Substring(0, numRes2.Length - 1);
                                            cursorX = initialCursorX + (numRes2.Length * 8);
                                        }
                                    }
                                    else
                                    {
                                        if (count3 == 0)
                                        {
                                            numRes += key.KeyChar;
                                            cursorX = initialCursorX + (numRes.Length * 8);
                                        }
                                        else if (count3 > 0)
                                        {
                                            numRes2 += key.KeyChar;
                                            cursorX = initialCursorX + (numRes2.Length * 8);
                                        }
                                    }
                                    canvas.Clear(Color.Blue);
                                    cursorY = 10;
                                    canvas.Display();
                                }

                                var resRes= int.Parse(numRes);
                                var resRes2 = int.Parse(numRes2);
                                var resResRes= resRes - resRes2;
                                DrawACSIIString(canvas, Color.White, "El resultat és: " + resResRes, initialCursorX, cursorY += 16, 1);
                                break;

                            case "/":
                                bool inputActiveDiv = true;
                                string numDiv = "";
                                string numDiv2 = "";
                                var count4= 0;
                                while (inputActiveDiv)
                                {
                                    if (count4 == 0)
                                    {
                                        DrawACSIIString(canvas, Color.White, "Introdueix el primer numero: " + numDiv, initialCursorX, cursorY += 16, 1);
                                        canvas.Display();
                                    }
                                    else if (count4 > 0)
                                    {
                                        DrawACSIIString(canvas, Color.White, "Introdueix el segon numero: " + numDiv2, initialCursorX, cursorY += 16, 1);
                                        canvas.Display();
                                    }
                                    var key = Console.ReadKey(intercept: true);

                                    if (key.Key == ConsoleKey.Enter)
                                    {
                                        count4++;
                                        if (count4 > 1)
                                        {
                                            inputActiveDiv = false;
                                        }
                                    }
                                    else if (key.Key == ConsoleKey.Backspace)
                                    {
                                        if (count4 == 0 && numDiv.Length > 0)
                                        {
                                            numDiv = numDiv.Substring(0, numDiv.Length - 1);
                                            cursorX = initialCursorX + (numDiv.Length * 8);
                                        }

                                        if (count4 > 0 && numDiv2.Length > 0)
                                        {
                                            numDiv2 = numDiv2.Substring(0, numDiv2.Length - 1);
                                            cursorX = initialCursorX + (numDiv2.Length * 8);
                                        }
                                    }
                                    else
                                    {
                                        if (count4 == 0)
                                        {
                                            numDiv += key.KeyChar;
                                            cursorX = initialCursorX + (numDiv.Length * 8);
                                        }
                                        else if (count4 > 0)
                                        {
                                            numDiv2 += key.KeyChar;
                                            cursorX = initialCursorX + (numDiv2.Length * 8);
                                        }
                                    }
                                    canvas.Clear(Color.Blue);
                                    cursorY = 10;
                                    canvas.Display();
                                }

                                var resDiv= int.Parse(numDiv);
                                var resDiv2= int.Parse(numDiv2);
                                var resResDiv = resDiv / resDiv2;
                                DrawACSIIString(canvas, Color.White, "El resultat és: " + resResDiv, initialCursorX, cursorY += 16, 1);
                                break;
                        }
                        break;

                    case "netejar":
                        canvas.Clear(Color.Blue);
                        break;

                    default:
                        DrawACSIIString(canvas, Color.White, "Comanda no trobada. Escriu 'help' per veure totes les comandes.", initialCursorX, cursorY += 16, 1);
                        break;
                }

                inputActive = true;
                inputText = "";
                cursorX = initialCursorX;
                canvas.Display();
            }
        }

        public static void MoveFile(string file, string newpath)
        {
            try
            {
                File.Copy(file, newpath);
                File.Delete(file);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private void ShowHelp(int cursorX, int cursorY)
        {
            try
            {
                DrawACSIIString(canvas, Color.White, "netejar: neteja la pantalla", cursorX, cursorY += 16, 1);
                DrawACSIIString(canvas, Color.White, "llistam: llista els subdirectoris", cursorX, cursorY += 16, 1);
                DrawACSIIString(canvas, Color.White, "apagar -a: apagar el SO", cursorX, cursorY += 16, 1);
                DrawACSIIString(canvas, Color.White, "cambiarDir <dir>: et permet cambiar de directori", cursorX, cursorY += 16, 1);
                DrawACSIIString(canvas, Color.White, "mostra <fitxer>: funció per mostar el contingut de un arxiu", cursorX, cursorY += 16, 1);
                DrawACSIIString(canvas, Color.White, "about: informació sobre el SO", cursorX, cursorY += 16, 1);
                DrawACSIIString(canvas, Color.White, "reinicia: reinicia el sistema", cursorX, cursorY += 16, 1);
                DrawACSIIString(canvas, Color.White, "espai: mira el espai del sistema", cursorX, cursorY += 16, 1);
                DrawACSIIString(canvas, Color.White, "sysdisk: Mira el sistema d'arxius", cursorX, cursorY += 16, 1);
                DrawACSIIString(canvas, Color.White, "files: Mostra el llistat dels fitxers del directori", cursorX, cursorY += 16, 1);
                DrawACSIIString(canvas, Color.White, "nfile: Crea un nou fitxer", cursorX, cursorY += 16, 1);
                DrawACSIIString(canvas, Color.White, "ndir: Crea un nou directori", cursorX, cursorY += 16, 1);
                DrawACSIIString(canvas, Color.White, "delfdir: Borra un fitxer o directori", cursorX, cursorY += 16, 1);
                DrawACSIIString(canvas, Color.White, "wtofile: Escriu al fitxer un contingut", cursorX, cursorY += 16, 1);
                DrawACSIIString(canvas, Color.White, "mvfile: Mou un fitxer de ruta", cursorX, cursorY += 16, 1);
                DrawACSIIString(canvas, Color.White, "rfile: Llegeix el contingut del fitxer", cursorX, cursorY += 16, 1);
                DrawACSIIString(canvas, Color.White, "rbytesf: Llegeix els bytes del fitxer", cursorX, cursorY += 16, 1);

                canvas.Display();
            }
            catch (Exception e)
            {
                Sys.Power.Shutdown();
            }
        }
    }
}