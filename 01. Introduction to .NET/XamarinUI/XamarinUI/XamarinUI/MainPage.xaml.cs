using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace XamarinUI
{
	public partial class MainPage : ContentPage
	{
		public MainPage()
		{
			InitializeComponent();
		}

        private void OnButtonClicked(object sender, EventArgs e)
        {
            string name = this.nameEntry.Text;
            if (string.IsNullOrWhiteSpace(name))
                name = "anonim";
            this.helloMessageLabel.Text = $"Hello, {name}!";
        }
    }
}
