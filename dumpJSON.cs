using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Manga_Library_Manager
{
    public partial class dumpJSON : Form
    {
        public dumpJSON()
        {
            InitializeComponent();
        }

        private void dumpJSON_Load(object sender, EventArgs e)
        {
            textBox.Text = mainMenu.jsonDump;
        }

        private void copyButton_Click(object sender, EventArgs e)
        {
            try
            {
                Clipboard.SetText(mainMenu.jsonDump);
            }
            catch { }
        }
    }
}
