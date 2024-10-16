using System;
using System.Collections.Generic;
using System.IO;
using Sys = Cosmos.System;

namespace CosmosKernel1
{
    public class Kernel : Sys.Kernel
    {
        Sys.FileSystem.CosmosVFS fs = new Cosmos.System.FileSystem.CosmosVFS();
        protected override void BeforeRun()
        {
            Sys.FileSystem.VFS.VFSManager.RegisterVFS(fs);
            Sys.KeyboardManager.SetKeyLayout(new Sys.ScanMaps.ESStandardLayout());
            Console.WriteLine("Cosmos booted successfully.\n");
            Console.WriteLine("Welcome to Cosmos developer!\n");
            Console.WriteLine("Type a line of text to get it echoed back.\n");
            Console.WriteLine("Type help to get info about the commands of the OS\n");
        }

        protected override void Run()
        {
            Console.Write("Ingresa una comanda: ");
            var input = Console.ReadLine();
            var commandParts = input.Split(' ');

            switch (commandParts[0])
            {
                case "help":
                    ShowHelp();
                    break;
                case "about":
                    Console.WriteLine("\nThis is a SO created and developed with Cosmos");
                    break;
                case "apagar":
                    if (commandParts.Length > 1 && commandParts[1] == "-a")
                        Cosmos.System.Power.Shutdown();
                    break;
                case "reinicia":
                    Cosmos.System.Power.Reboot();
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
                    var files_list = Directory.GetFiles(dir);
                    foreach (var file in files_list)
                    {
                        Console.WriteLine(file);
                    }
                    break;

                case "rfiles":
                    Console.WriteLine("Introdueix nom del directori a llegir els fitxers: ");
                    var rfiles = Console.ReadLine();
                    var directory_list = Directory.GetDirectories(rfiles);
                    try
                    {
                        foreach (var file in directory_list)
                        {
                            var content = File.ReadAllText(file);

                            Console.WriteLine("File name: " + file);
                            Console.WriteLine("File size: " + content.Length);
                            Console.WriteLine("Content: " + content);
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.ToString());
                    }
                    break;

                case "nfile":
                    Console.WriteLine("Introdueix nom del nou fitxer: ");
                    var nfile = Console.ReadLine();
                    try
                    {
                        var file_stream = File.Create(nfile);
                        Console.WriteLine($"Fitxer '{file_stream}' creat correctament.");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Fitxer '{e.Message}' creat correctament.");
                    }
                    break;

                case "ndir":
                    Console.WriteLine("Introdueix nom del nou directori: ");
                    var ndir = Console.ReadLine();
                    try
                    {
                        Directory.CreateDirectory(ndir);
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
                        try
                        {
                            File.Delete(fileToDelete);
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
                        try
                        {
                            Directory.Delete(directoryToDelete);
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
                    var filew= Console.ReadLine();
                    Console.WriteLine("Escriu el que vols escriure:");
                    var contentw = Console.ReadLine();
                    try
                    {
                        File.WriteAllText(filew, contentw);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.ToString());
                    }
                    break;

                case "mvfile":
                    Console.WriteLine("Introdueix la ruta del fitxer a moure: ");
                    var filemv= Console.ReadLine();
                    Console.WriteLine("Introdueix la ruta final: ");
                    var newpath= Console.ReadLine();
                    MoveFile(filemv, newpath);
                    break;

                case "rfile":
                    Console.WriteLine("Introdueix el nom del fitxer a llegir tot el seu contingut: ");
                    var rfile= Console.ReadLine();
                    try
                    {
                        Console.WriteLine(File.ReadAllText(rfile));
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.ToString());
                    }
                    break;

                case "rbytesf":
                    Console.WriteLine("Introdueix el nom del fitxer a llegir els seus bytes: ");
                    var rbytesf = Console.ReadLine();
                    try
                    {
                        Console.WriteLine(File.ReadAllBytes(rbytesf));
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

        private void ShowHelp()
        {
            Console.WriteLine("\ndelete -u <nombre>: borra el usuari");
            Console.WriteLine("\nnetejar: neteja la pantalla");
            Console.WriteLine("\nllistam: llista els subdirectoris");
            Console.WriteLine("\nchoose -u <usuario>: cambiar de usuari");
            Console.WriteLine("\napagar -a: apagar el SO");
            Console.WriteLine("\ncambiarDir <dir>: et permet cambiar de directori");
            Console.WriteLine("\nmostra <fitxer>: funció per mostarr el contingut de un arxiu");
            Console.WriteLine("\nsuperUser: funció per tenir permisos de administrador i estar en super usuari");
            Console.WriteLine("\ncrea -u <nombre>: funció per a crear un usuari");
            Console.WriteLine("\nabout: informació sobre el SO");
            Console.WriteLine("\nreinicia: reinicia el sistema");
            Console.WriteLine("\nespai: mira el espai del sistema");
            Console.WriteLine("\nsysdisk: Mira el sistema d'arxius");
            Console.WriteLine("\nfiles: Mostra el llistat dels fitxers del directori");
            Console.WriteLine("\nrfiles: Llegeix els fitxers del directori");
            Console.WriteLine("\nnfile: Crea un nou fitxer");
            Console.WriteLine("\nndir: Crea un nou directori");
            Console.WriteLine("\ndelfdir: Borra un fitxer o directori");
            Console.WriteLine("\nwtofile: Escriu al fitxer un contingut");
            Console.WriteLine("\nmvfile: Mou un fitxer de ruta");
            Console.WriteLine("\nrfile: Llegeix el contingut del fitxer");
            Console.WriteLine("\nrbytesf: Llegeix els bytes del fitxer");
        }
    }
}