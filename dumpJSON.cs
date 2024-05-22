
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
