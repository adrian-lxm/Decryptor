using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Decryptor.Utils;
using System.Diagnostics;
using System.Net;
using System.IO;
using static Decryptor.Forms.ThemesMenu;

namespace Decryptor.Forms
{
    public partial class Main : Form
    {
        private bool isClosing = false;

        public Main()
        {
            InitializeComponent();
        }

        private void Main_SizeChanged(object sender, EventArgs e)
        {
            richTextBox1.SetBounds(0, 48, (this.Width / 2), (this.Height - 88));
            richTextBox2.SetBounds((this.Width / 2 + 7), 48, (this.Width / 2 - 25), (this.Height - 88));
            label2.SetBounds((this.Width / 2 + 7), 29, 72, 13);
        }

        private void Main_Load(object sender, EventArgs e)
        {
            richTextBox1.ScrollBars = RichTextBoxScrollBars.Both;
            richTextBox2.ScrollBars = RichTextBoxScrollBars.Both;
            richTextBox1.SetBounds(0, 48, (this.Width / 2), (this.Height - 88));
            richTextBox2.SetBounds((this.Width / 2 + 7), 48, (this.Width / 2 - 25), (this.Height - 88));
            label2.SetBounds((this.Width / 2 + 7), 29, 72, 13);
            Settings set = (Settings) JsonTool.Deserialize(typeof(Settings), Program.WorkPath + "\\settings.json");
            enableTheme(Program.ThemeTool.getThemeByName(set.settedTheme));
            Program.UsingTheme = Program.ThemeTool.getThemeByName(set.settedTheme);
            richTextBox1.Font = set.font;
            richTextBox2.Font = set.font;
            Program.Settings = set;
        }

        private void neuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Do you really want to delete everything", "New", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                richTextBox1.Text = "";
                richTextBox2.Text = "";
            }
        }

        private void beendenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string message = "Do you really want close Decryptor?";
            if (MessageBox.Show(message, "Exit?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                isClosing = true;
                this.Close();
            }
        }

        private void überDecryptorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            About about = new About();
            about.ShowDialog();
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            if (!(richTextBox1.Text.Equals("")))
            {
                Choose cho = new Choose("crypted.");
                cho.ShowDialog();
                if(cho.choosed > -1)
                {
                    if(cho.choosed == 0)
                    {
                        richTextBox2.Text = Crypting.encryptToSaikoC(richTextBox1.Text);
                    }
                    else
                    {
                        richTextBox2.Text = Crypting.encryptToCaeser(richTextBox1.Text);
                    }
                    MessageBox.Show("Encryption done.", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("The text to be encrypted should not be empty!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void entschlüsslenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!(richTextBox1.Text.Equals("")))
            {
                Choose cho = new Choose("entschlüsselt werden.");
                cho.ShowDialog();
                if (cho.choosed > -1)
                {
                    if (cho.choosed == 0)
                    {
                        richTextBox2.Text = Crypting.decryptFromSaikoC(richTextBox1.Text);
                    }
                    else
                    {
                        richTextBox2.Text = Crypting.decryptFromCaeser(richTextBox1.Text);
                    }
                    MessageBox.Show("Decryption done.", "Ready", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("The text to be decrypted must not be empty!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void textKopierenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.SetDataObject(richTextBox2.Text, true);
            MessageBox.Show("Successfully copied.", "Success!", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void toolStripMenuItem2_MouseHover(object sender, EventArgs e)
        {
            label1.Text = "Unverschlüsselt:";
            label2.Text = "Verschlüsselt:";
        }

        private void entschlüsslenToolStripMenuItem_MouseHover(object sender, EventArgs e)
        {
            label2.Text = "Unverschlüsselt:";
            label1.Text = "Verschlüsselt:";
        }

        private void Main_FormClosed(object sender, FormClosedEventArgs e)
        {
            JsonTool.Serialize(Program.Settings, Program.WorkPath + "\\settings.json");
            foreach(string path in Directory.GetFiles(Program.WorkPath + "\\Themes"))
            {
                File.Delete(path);
            }
            int i = 0;
            foreach(Theme t in Program.ThemeTool.registeredThemes)
            {
                if (!(t.name.Equals("White") || t.name.Equals("Dark")))
                {
                    i++;
                    JsonTool.Serialize(t, Program.WorkPath + "\\Themes\\" + i + ".json");
                }
            }
            Application.Exit();
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(!isClosing)
            {
                string message = "Do you really want to close Decryptor?";
                if(MessageBox.Show(message, "Exit?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    e.Cancel = true;
                }
            }
        }

        private void enableTheme(Theme t)
        {
            richTextBox1.ForeColor = t.FontForeColor;
            richTextBox2.ForeColor = t.FontForeColor;
            label1.ForeColor = t.FontForeColor;
            label2.ForeColor = t.FontForeColor;
            richTextBox1.BackColor = t.textBoxBackcolor;
            richTextBox2.BackColor = t.textBoxBackcolor;
            this.BackColor = t.FormBackColor;
        }

        private void themesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ThemesMenu menu = new ThemesMenu();
            menu.ShowDialog();
            if (menu.Result == ThemeResult.ChangeTheme)
            {
                enableTheme(Program.UsingTheme);
                Program.Settings.settedTheme = Program.UsingTheme.name;
            }
            else if(menu.Result == ThemeResult.EditedTheme)
            {
                if(!(Program.ThemeTool.registeredThemes.Contains(Program.UsingTheme)))
                {
                    Program.UsingTheme = Program.ThemeTool.getThemeByName("White");
                    enableTheme(Program.ThemeTool.getThemeByName("White"));
                    Program.Settings.settedTheme = "White";
                }
            }
        }

        private void einstellungenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FontDialog font = new FontDialog();
            font.Font = Program.Settings.font;
            if (font.ShowDialog() == DialogResult.OK)
            {
                Program.Settings.font = font.Font;
                richTextBox1.Font = font.Font;
                richTextBox2.Font = font.Font;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {

        }

        private void encryptToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
