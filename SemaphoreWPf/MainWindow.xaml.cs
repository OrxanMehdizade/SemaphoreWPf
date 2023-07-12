using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SemaphoreWPf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
    }

    //public void Semaphorprint(string name, short İnitialCount_, short MaximumCount_)
    //{
    //    Semaphore semaphore = new Semaphore(İnitialCount_, MaximumCount_, name);

    //}
    //private void ButtonSpinner_Spin(
    //    object sender, Microsoft.Windows.Controls.SpinEventArgs e)
    //{
    //    ButtonSpinner spinner = (ButtonSpinner)sender;
    //    TextBox txtBox = (TextBox)spinner.Content;
    //    int value = String.IsNullOrEmpty(txtBox.Text) ? 0 : Convert.ToInt32(txtBox.Text);
    //    if (e.Direction == Windows.Controls.SpinDirection.Increase)
    //        value++;
    //    else
    //    {
    //        value--;
    //    }
    //    txtBox.Text = value.ToString();
    //}
}
