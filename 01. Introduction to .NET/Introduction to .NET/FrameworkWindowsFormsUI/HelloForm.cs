using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FrameworkWindowsFormsUI
{
    public partial class HelloForm : Form
    {
        public HelloForm()
        {
            InitializeComponent();
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            string name = this.nameTextBox.Text;

            if (string.IsNullOrWhiteSpace(name))
                name = "anonim";  
            
            MessageBox.Show($"Hello, {name}!");
        }
    }
}
