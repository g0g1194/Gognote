using System;
using System.IO;
using System.Windows.Forms;

namespace Gognote
{
    public partial class Form1 : Form
    {
        uint counter = 1;

        public Form1()
        {
            InitializeComponent();
        }

        private void newToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            TabPage newTab = new TabPage("Файл " + counter);
            RichTextBox textBox = new RichTextBox();
            textBox.Dock = DockStyle.Fill;
            newTab.Controls.Add(textBox);
            tabControl.TabPages.Add(newTab);
            tabControl.SelectedTab = newTab;
            counter++;
        }

        private async void openToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            openFileDialog1.Title = "Выберите тектовый файл";
            openFileDialog1.Filter = "Текстовые файлы|*.txt";
            openFileDialog1.Multiselect = false;
            openFileDialog1.ValidateNames = true;
            try
            {
                using (openFileDialog1)
                {
                    if (openFileDialog1.ShowDialog() == DialogResult.OK)
                    {
                        using (StreamReader sr = new StreamReader(openFileDialog1.FileName))
                        {
                            TabPage fileTab = new TabPage(openFileDialog1.FileName);
                            RichTextBox textBox = new RichTextBox();
                            textBox.Text = await sr.ReadToEndAsync();
                            sr.Close();
                            textBox.Dock = DockStyle.Fill;
                            fileTab.Controls.Add(textBox);
                            tabControl.TabPages.Add(fileTab);
                            tabControl.SelectedTab = fileTab;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Title = "Выберите место сохранения";
            saveFileDialog1.Filter = "Текстовые файлы|*.txt";
            saveFileDialog1.ValidateNames = true;
            try
            {
                using (saveFileDialog1)
                {
                    if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                    {
                        using (StreamWriter sw = new StreamWriter(saveFileDialog1.FileName))
                        {
                            await sw.WriteAsync(tabControl.SelectedTab.Controls[0].Text);
                            sw.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cutToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            textWorker(0);
        }

        private void copyToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            textWorker(1);
        }

        private void pasteToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            textWorker(2);
        }

        private void textWorker(byte mode)
        {
            RichTextBox textBox;
            if (tabControl.TabCount > 0)
            {
                textBox = tabControl.SelectedTab.Controls[0] as RichTextBox;
                switch (mode)
                {
                    case 0:
                        textBox.Cut();
                        break;
                    case 1:
                        textBox.Copy();
                        break;
                    case 2:
                        textBox.Paste();
                        break;
                }
            }
        }
    }
}
