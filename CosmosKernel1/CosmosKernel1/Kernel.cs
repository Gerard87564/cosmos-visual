using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Sys = Cosmos.System;
using Cosmos.System.Graphics;
using System.Drawing;


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
        protected override void BeforeRun()
        {
            Sys.FileSystem.VFS.VFSManager.RegisterVFS(fs);
            Sys.KeyboardManager.SetKeyLayout(new Sys.ScanMaps.ESStandardLayout());
            Console.WriteLine("Cosmos booted successfully. Let's go in Graphical Mode");

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

            while (!exit)
            {
                DrawACSIIString(canvas, Color.White, "Ingresa una comanda: ", initialCursorX, cursorY, 1);
                canvas.Display();

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
                    DrawACSIIString(canvas, Color.White, "Ingresa una comanda: " + inputText, initialCursorX, cursorY, 1);
                    canvas.Display();
                }

                var commandParts = inputText.Trim().Split(' ');

                switch (commandParts[0])
                {
                    case "help":
                        ShowHelp(cursorX, cursorY + 16);
                        break;
                    case "about":
                        Console.WriteLine("\nThis is a SO created and developed with Cosmos");
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

                    case "espai":
                        var available_space = fs.GetAvailableFreeSpace(@"0:\");
                        Console.WriteLine("Available Free Space: " + available_space);
                        break;

                    case "sysdisk":
                        var fs_type = fs.GetFileSystemType(@"0:\");
                        Console.WriteLine("File System Type: " + fs_type);
                        break;

                    case "files":
                        Console.WriteLine("Introdueix nom del directori a mostrar el llistat dels seus fitxers: ");
                        var dir = Console.ReadLine();
                        var dir2 = @"0:\" + dir;
                        var files_list = Directory.GetFiles(dir2);
                        foreach (var file in files_list)
                        {
                            Console.WriteLine("Nom del fitxer: " + file);
                        }
                        break;

                    case "nfile":
                        Console.WriteLine("Introdueix nom del nou fitxer: ");
                        var nfile = Console.ReadLine();
                        var filePath = @"0:\" + nfile;
                        try
                        {
                            var file_stream = File.Create(filePath);
                            Console.WriteLine($"Fitxer '{nfile}' creat correctament.");
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine($"Fitxer '{e.Message}' creat correctament.");
                        }
                        break;

                    case "ndir":
                        Console.WriteLine("Introdueix nom del nou directori: ");
                        var ndir = Console.ReadLine();
                        var filePathdir = @"0:\" + ndir;
                        try
                        {
                            Directory.CreateDirectory(filePathdir);
                            Console.WriteLine($"Directori '{ndir}' creat correctament.");
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine($"Error al crear el directori: {e.Message}");
                        }
                        break;

                    case "delfdir":
                        Console.WriteLine("Vols borrar fitxer o directori (f-d): ");
                        var option = Console.ReadLine();

                        if (option == "f")
                        {
                            Console.WriteLine("Escriu el nom del fitxer a borrar:");
                            var fileToDelete = Console.ReadLine();
                            var filePathdelete = @"0:\" + fileToDelete;
                            try
                            {
                                File.Delete(filePathdelete);
                                Console.WriteLine($"Fitxer '{fileToDelete}' esborrat correctament.");
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine($"Error esborrant el fitxer: {e.Message}");
                            }
                        }
                        else if (option == "d")
                        {
                            Console.WriteLine("Escriu el nom del directori a borrar:");
                            var directoryToDelete = Console.ReadLine();
                            var filePathdelete = @"0:\" + directoryToDelete;
                            try
                            {
                                Directory.Delete(filePathdelete);
                                Console.WriteLine($"Directori '{directoryToDelete}' esborrat correctament.");
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine($"Error esborrant el directori: {e.Message}");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Opció no vàlida. Introdueix 'f' per fitxer o 'd' per directori.");
                        }
                        break;

                    case "wtofile":
                        Console.WriteLine("Escriu el nom del fitxer a escriure:");
                        var filew = Console.ReadLine();
                        var filewrute = @"0:\" + filew;
                        Console.WriteLine("Escriu el que vols escriure:");
                        var contentw = Console.ReadLine();
                        try
                        {
                            File.WriteAllText(filewrute, contentw);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.ToString());
                        }
                        break;

                    case "mvfile":
                        Console.WriteLine("Introdueix la ruta del fitxer a moure: ");
                        var filemv = Console.ReadLine();
                        var filemvrute = @"0:\" + filemv;
                        Console.WriteLine("Introdueix la ruta final: ");
                        var newpath = Console.ReadLine();
                        var newpath2 = @"0:\" + newpath + @"\" + filemv;
                        MoveFile(filemvrute, newpath2);
                        break;

                    case "rfile":
                        Console.WriteLine("Introdueix el nom del fitxer a llegir tot el seu contingut: ");
                        var rfile = Console.ReadLine();
                        var readfile = @"0:\" + rfile;
                        try
                        {
                            Console.WriteLine(File.ReadAllText(readfile));
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.ToString());
                        }
                        break;

                    case "rbytesf":
                        Console.WriteLine("Introdueix el nom del fitxer a llegir els seus bytes: ");
                        var rbytesf = Console.ReadLine();
                        var readfilebyte = @"0:\" + rbytesf;
                        try
                        {
                            Console.WriteLine(File.ReadAllBytes(readfilebyte));
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.ToString());
                        }
                        break;
                    default:
                        Console.WriteLine("\nComanda no trobada. Escriu 'help' per veure totes les comandes.");
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
                DrawACSIIString(canvas, Color.White, "delete -u <nombre>: borra el usuari", cursorX, cursorY += 16, 1);
                DrawACSIIString(canvas, Color.White, "netejar: neteja la pantalla", cursorX, cursorY += 16, 1);
                DrawACSIIString(canvas, Color.White, "llistam: llista els subdirectoris", cursorX, cursorY += 16, 1);
                DrawACSIIString(canvas, Color.White, "choose -u <usuario>: cambiar de usuari", cursorX, cursorY += 16, 1);
                DrawACSIIString(canvas, Color.White, "apagar -a: apagar el SO", cursorX, cursorY += 16, 1);
                DrawACSIIString(canvas, Color.White, "cambiarDir <dir>: et permet cambiar de directori", cursorX, cursorY += 16, 1);
                DrawACSIIString(canvas, Color.White, "mostra <fitxer>: funció per mostar el contingut de un arxiu", cursorX, cursorY += 16, 1);
                DrawACSIIString(canvas, Color.White, "superUser: funció per tenir permisos de administrador i estar en super usuari", cursorX, cursorY += 16, 1);
                DrawACSIIString(canvas, Color.White, "crea -u <nombre>: funció per a crear un usuari", cursorX, cursorY += 16, 1);
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