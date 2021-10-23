using Decryptor.Forms;
using Decryptor.Utils;
using System;
using System.Windows.Forms;
using System.IO;
using Timer = System.Windows.Forms.Timer;

namespace Decryptor
{
    static class Program
    {
        private static Timer _timer = new Timer()
        {
            Interval = 3000
        };
        public static Settings Settings;
        public static readonly ThemeTool ThemeTool = new ThemeTool();
        public static Theme UsingTheme;
        private static readonly float _version = 1.3f;
        public static string WorkPath => Environment.CurrentDirectory;

        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if (!Directory.Exists(WorkPath + "\\Themes")) Directory.CreateDirectory(Program.WorkPath + "\\Themes");
            foreach (string path in Directory.GetFiles(WorkPath + "\\Themes"))
            {
                Theme theme = (Theme) JsonTool.Deserialize(typeof(Theme), path);
                ThemeTool.registeredThemes.Add(theme);
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            _timer.Tick += OnTick;
            _timer.Start();
            Application.Run(new StartUp());
        }

        private static void OnTick(object sender, EventArgs e)
        {
            Application.OpenForms[0].Hide();
            new Main().Show();
            _timer.Stop();
        }

    }
}
