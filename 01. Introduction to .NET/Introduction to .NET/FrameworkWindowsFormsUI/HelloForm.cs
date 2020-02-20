using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HelloMessageLogic;

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
                                  
            MessageBox.Show(HelloMessageService.HelloMessage(DateTime.Now, name));
        }
    }
}
