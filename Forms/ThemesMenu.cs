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

namespace Decryptor.Forms
{
    public partial class ThemesMenu : Form
    {
        public ThemeResult Result = ThemeResult.AbortedAction;
        private readonly List<Theme> _localThemes = Program.ThemeTool.registeredThemes;

        public ThemesMenu()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(treeView1.SelectedNode != null)
            {
                Program.UsingTheme = Program.ThemeTool.getThemeByName(treeView1.SelectedNode.Text);
                Result = ThemeResult.ChangeTheme;
                this.Close();
                return;
            }
            MessageBox.Show("Please select a theme!", "You have not chosen a theme", MessageBoxButtons.OK);
        }

        private void treeView2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                if(treeView2.SelectedNode != null)
                {
                    if(!(treeView2.SelectedNode.Text.Equals("White") || treeView2.SelectedNode.Text.Equals("Dark")))
                    {
                        TreeNode selected = treeView2.SelectedNode;
                        _localThemes.Remove(Program.ThemeTool.getThemeByName(selected.Text));
                        treeView2.Nodes.Remove(selected);
                        return;
                    }
                    MessageBox.Show("You can't remove the default themes!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else if (e.KeyCode == Keys.Oemplus)
            {
                NewTheme newTheme = new NewTheme();
                newTheme.ShowDialog();
                if(newTheme.result == DialogResult.OK)
                {
                    Theme theme = new Theme()
                    {
                        FontForeColor = newTheme.FontForeColor,
                        textBoxBackcolor = newTheme.textBoxBackcolor,
                        FormBackColor = newTheme.FormBackColor,
                        name = newTheme.name
                    };
                    _localThemes.Add(theme);
                    TreeNode node = new TreeNode(theme.name);
                    treeView2.Nodes.Add(node);
                }
            }
        }

        private void ThemesMenu_Load(object sender, EventArgs e)
        {
            foreach(Theme t in _localThemes)
            {
                TreeNode node = new TreeNode(t.name);
                treeView1.Nodes.Add(node);
                treeView2.Nodes.Add((TreeNode) node.Clone());
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Result = ThemeResult.EditedTheme;
            Program.ThemeTool.registeredThemes = _localThemes;
            this.Close();
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }

        private void treeView2_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            NewTheme newTheme = new NewTheme();
            newTheme.ShowDialog();
            if (newTheme.result == DialogResult.OK)
            {
                Theme t = new Theme()
                {
                    FontForeColor = newTheme.FontForeColor,
                    textBoxBackcolor = newTheme.textBoxBackcolor,
                    FormBackColor = newTheme.FormBackColor,
                    name = newTheme.name
                };
                _localThemes.Add(t);
                TreeNode node = new TreeNode(t.name);
                treeView2.Nodes.Add(node);
            }
        }
    }
    
    public enum ThemeResult
    {
        ChangeTheme,
        EditedTheme,
        AbortedAction
    }

}

