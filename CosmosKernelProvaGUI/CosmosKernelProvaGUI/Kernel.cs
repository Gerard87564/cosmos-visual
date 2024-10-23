using System;
using Sys = Cosmos.System;
using Cosmos.System.Graphics;
using System.Drawing;
using Cosmos.System.Graphics.Fonts;

namespace GraphicTest
{
    public class Kernel : Sys.Kernel
    {
        Canvas canvas;

        private readonly Sys.Graphics.Bitmap bitmap = new Sys.Graphics.Bitmap(10, 10,
                new byte[] { 0, 255, 243, 255, 0, 255, 243, 255, 0, 255, 243, 255, 0, 255, 243, 255, 0, 255, 243, 255, 0,
                    255, 243, 255, 0, 255, 243, 255, 0, 255, 243, 255, 0, 255, 243, 255, 0, 255, 243, 255, 0, 255, 243, 255,
                    0, 255, 243, 255, 0, 255, 243, 255, 0, 255, 243, 255, 0, 255, 243, 255, 0, 255, 243, 255, 0, 255, 243, 255,
                    0, 255, 243, 255, 0, 255, 243, 255, 0, 255, 243, 255, 0, 255, 243, 255, 23, 59, 88, 255,
                    23, 59, 88, 255, 0, 255, 243, 255, 0, 255, 243, 255, 23, 59, 88, 255, 23, 59, 88, 255, 0, 255, 243, 255, 0,
                    255, 243, 255, 0, 255, 243, 255, 23, 59, 88, 255, 153, 57, 12, 255, 0, 255, 243, 255, 0, 255, 243, 255, 0, 255,
                    243, 255, 0, 255, 243, 255, 153, 57, 12, 255, 23, 59, 88, 255, 0, 255, 243, 255, 0, 255, 243, 255, 0, 255, 243,
                    255, 0, 255, 243, 255, 0, 255, 243, 255, 0, 255, 243, 255, 72, 72, 72, 255, 72, 72, 72, 255, 0, 255, 243, 255, 0,
                    255, 243, 255, 0, 255, 243, 255, 0, 255, 243, 255, 0, 255, 243, 255, 0, 255, 243, 255, 0, 255, 243, 255, 72, 72,
                    72, 255, 72, 72, 72, 255, 0, 255, 243, 255, 0, 255, 243, 255, 0, 255, 243, 255, 0, 255, 243, 255, 0, 255, 243, 255,
                    10, 66, 148, 255, 0, 255, 243, 255, 0, 255, 243, 255, 0, 255, 243, 255, 0, 255, 243, 255, 0, 255, 243, 255, 0, 255,
                    243, 255, 10, 66, 148, 255, 0, 255, 243, 255, 0, 255, 243, 255, 0, 255, 243, 255, 10, 66, 148, 255, 10, 66, 148, 255,
                    10, 66, 148, 255, 10, 66, 148, 255, 10, 66, 148, 255, 10, 66, 148, 255, 0, 255, 243, 255, 0, 255, 243, 255, 0, 255,
                    243, 255, 0, 255, 243, 255, 0, 255, 243, 255, 10, 66, 148, 255, 10, 66, 148, 255, 10, 66, 148, 255, 10, 66, 148,
                    255, 0, 255, 243, 255, 0, 255, 243, 255, 0, 255, 243, 255, 0, 255, 243, 255, 0, 255, 243, 255, 0, 255, 243, 255,
                    0, 255, 243, 255, 0, 255, 243, 255, 0, 255, 243, 255, 0, 255, 243, 255, 0, 255, 243, 255, 0, 255, 243, 255, 0, 255, 243, 255,
                }, ColorDepth.ColorDepth32);

        protected override void BeforeRun()
        {
            Console.WriteLine("Cosmos booted successfully. Let's go in Graphical Mode");

            canvas = FullScreenCanvas.GetFullScreenCanvas(new Mode(640, 480, ColorDepth.ColorDepth32));

            canvas.Clear(Color.Blue);
        }

        protected override void Run()
        {
            try
            {
                canvas.Clear(Color.Blue);

                canvas.DrawFilledRectangle(Color.Blue, 100, 100, 300, 200);

                var font = PCScreenFont.Default;

                string[] commands = { "delete -u <nombre>: borra el usuari", "netejar: neteja la pantalla",
                    "llistam: llista els subdirectoris", "choose -u <usuario>: cambiar de usuari",
                    "apagar -a: apagar el SO", "cambiarDir <dir>: et permet cambiar de directori",
                    "mostra <fitxer>: funció per mostarr el contingut de un arxiu", "superUser: funció per tenir permisos de administrador i estar en super usuari",
                    "crea -u <nombre>: funció per a crear un usuari", "about: informació sobre el SO", "reinicia: reinicia el sistema",
                    "espai: mira el espai del sistema", "sysdisk: Mira el sistema d'arxius", "files: Mostra el llistat dels fitxers del directori",
                    "nfile: Crea un nou fitxer", "ndir: Crea un nou directori", "delfdir: Borra un fitxer o directori", "wtofile: Escriu al fitxer un contingut",
                    "mvfile: Mou un fitxer de ruta", "rfile: Llegeix el contingut del fitxer", "rbytesf: Llegeix els bytes del fitxer"
                };

                int yPosition = 120;

                foreach (var command in commands)
                {
                    canvas.DrawString(command, font, Color.White, 120, yPosition);
                    yPosition += 40;
                }

                canvas.Display();

                Console.ReadKey();
                Sys.Power.Shutdown();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception occurred: " + e.Message);
                Sys.Power.Shutdown();
            }
        }
    }
}