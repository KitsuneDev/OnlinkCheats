using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace OnlinkCheats
{
    /// <summary>
    /// Interaction logic for LocatorDialog.xaml
    /// </summary>
    public partial class LocatorDialog : Window
    {
        public string AgentPath { get; set; }
        public string Agent { get; set; }
        public LocatorDialog()
        {
            InitializeComponent();
        }

        private void agent_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {

            Path.Text = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "Onlink/Users/", agent.Text +e.Text+ ".db");
        }

        private void submit_Click(object sender, RoutedEventArgs e)
        {
            if (!File.Exists(Path.Text))
            {
                MessageBox.Show("Agent not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (agent.Text == string.Empty)
            {
                MessageBox.Show("Agent must not be empty.", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            DialogResult = true;
            AgentPath = Path.Text;
            Agent = agent.Text;
            this.Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (DialogResult != true) DialogResult = false;
        }
        
    }
}
