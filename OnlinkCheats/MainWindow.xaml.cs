using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Utils;

namespace OnlinkCheats
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string AgentPath;
        private string Agent;
        private ColorChanger colorChanger;
        private OnlinkDatabase db;
        public MainWindow()
        {
            InitializeComponent();
            colorChanger = new ColorChanger(ThisWindow.Background, Dispatcher);
            LocatorDialog ld = new LocatorDialog();
            bool? ldSuccess = ld.ShowDialog();
            if (ldSuccess != true)
            {
                MessageBox.Show("Please select an agent.", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                Application.Current.Shutdown();


            }
            this.AgentPath = ld.AgentPath;
            this.Agent = ld.Agent;
            db = new OnlinkDatabase(AgentPath);
        }

        private void textBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {

            if (!char.IsDigit(e.Text, e.Text.Length - 1))
                e.Handled = true;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void send_Click(object sender, RoutedEventArgs e)
        {
            colorChanger.Start();
            try
            {
                
                db.Open();
                if (hackLevel.IsChecked.Value)
                {

                    int Ranking = db.GetUplinkRatingInt(((ComboBoxItem) uplinkRating.SelectedItem).Name);
                    //int Score = Int32.Parse(score.Text);
                    //int Credits = Int32.Parse(creditRanking.Text);
                    db.setUplinkRating(Ranking);
                }
                if (SetScore.IsChecked.Value)
                {

                    //int Ranking = db.GetUplinkRatingInt(((ComboBoxItem)uplinkRating.SelectedItem).Name);
                    int Score = Int32.Parse(score.Text);
                    //int Credits = Int32.Parse(creditRanking.Text);
                    db.setUplinkScore(Score);
                }
                if (SetCredits.IsChecked.Value)
                {

                    //int Ranking = db.GetUplinkRatingInt(((ComboBoxItem)uplinkRating.SelectedItem).Name);
                    //int Score = Int32.Parse(score.Text);
                    int Credit = Int32.Parse(creditRanking.Text);
                    db.setCreditRating(Credit);
                }
                if (SetMoney.IsChecked.Value)
                {
                    int Money = int.Parse(money.Text);
                    db.setMoney(Money, Agent);
                }
                db.Close();
                MessageBox.Show("Finished!", "Finished", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: "+ ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            colorChanger.Stop();
        }

        private void hackLevel_Click(object sender, RoutedEventArgs e)
        {
            if (hackLevel.IsChecked.Value)
            {
                uplinkRating.IsEnabled = true;
                //score.IsEnabled = true;
                //creditRanking.IsEnabled = true;
            }
            else
            {
                uplinkRating.IsEnabled = false;
                //score.IsEnabled = false;
                //creditRanking.IsEnabled = false;

            }
        }


        private void SetMoney_Click(object sender, RoutedEventArgs e)
        {
            if (SetMoney.IsChecked.Value)
            {
                money.IsEnabled = true;
            }
            else
            {
                money.IsEnabled = false;
            }
        }

        private void SetScore_Click(object sender, RoutedEventArgs e)
        {
            if (hackLevel.IsChecked.Value)
            {
                //uplinkRating.IsEnabled = true;
                score.IsEnabled = true;
                //creditRanking.IsEnabled = true;
            }
            else
            {
                //uplinkRating.IsEnabled = false;
                score.IsEnabled = false;
                //creditRanking.IsEnabled = false;

            }
        }

        private void SetCredits_Click(object sender, RoutedEventArgs e)
        {
            if (hackLevel.IsChecked.Value)
            {
                //uplinkRating.IsEnabled = true;
                //score.IsEnabled = true;
                creditRanking.IsEnabled = true;
            }
            else
            {
                //uplinkRating.IsEnabled = false;
                //score.IsEnabled = false;
                creditRanking.IsEnabled = false;

            }
        }

        private void setAllSecSysStatus_Click(object sender, RoutedEventArgs e)
        {
            db.Open();
            db.setAllSecuritySystemsStatus(AllSecSysStatusVal.IsChecked.Value);
            db.Close();
        }
        //TODO: Disable / Enable Security.
        //SQL Example:
        /*
        //Disable
        UPDATE secsystem
        SET enabled = 0;
        //Enable
        UPDATE secsystem
        SET enabled = 0;
        */
    }
}
