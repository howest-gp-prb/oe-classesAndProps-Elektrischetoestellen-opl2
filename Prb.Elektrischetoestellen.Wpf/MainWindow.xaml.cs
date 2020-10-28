using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
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
using Prb.Elektrischetoestellen.Core;

namespace Prb.Elektrischetoestellen.Wpf
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
        List<Appliance> appliances;
        bool isNew;
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            PopulateCombobox();
            ClearControls();
            ViewNormal();
            // ONDERSTAANDE IF-STATEMENT IS EEN EXTRA EN GEEN LEERSTOF
            if(!JSonRead())
                DoSeeding();
            PopulateListbox();
            DoStatistics();
        }
        private void PopulateCombobox()
        {
            cmbTypes.ItemsSource = ApplianceTypes.GetApplianceTypes();
        }
        private void ClearControls()
        {
            cmbTypes.SelectedIndex = -1;
            txtBrand.Text = "";
            txtPrice.Text = "";
            txtConsumption.Text = "";
        }
        private void ViewNormal()
        {
            lstAppliances.IsEnabled = true;
            grpOperations.Visibility = Visibility.Visible;
            grpDetails.IsEnabled = false;
            btnSave.Visibility = Visibility.Hidden;
            btnCancel.Visibility = Visibility.Hidden;
        }
        private void ViewOperation()
        {
            lstAppliances.IsEnabled = false;
            grpOperations.Visibility = Visibility.Hidden;
            grpDetails.IsEnabled = true;
            btnSave.Visibility = Visibility.Visible;
            btnCancel.Visibility = Visibility.Visible;
        }
        private void DoSeeding()
        {
            appliances = new List<Appliance>();
            appliances.Add(new Appliance("Vaatwasser", "Miele", 1800, 644.99M));
            appliances.Add(new Appliance("Vaatwasser", "AEG", 1750, 512M));
            appliances.Add(new Appliance("TV", "Samsung", 195, 399.99M));
        }
        private void PopulateListbox()
        {
            lstAppliances.ItemsSource = null;
            lstAppliances.ItemsSource = appliances;
        }

        private void lstAppliances_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ClearControls();
            if (lstAppliances.SelectedIndex < 0) return;

            Appliance appliance = (Appliance)lstAppliances.SelectedItem;
            txtBrand.Text = appliance.ApplianceBrand;
            txtPrice.Text = appliance.AppliancePrice.ToString("0.00");
            txtConsumption.Text = appliance.ApplianceConsumption.ToString();
            cmbTypes.SelectedItem = appliance.ApplianceType;
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            isNew = true;
            ViewOperation();
            ClearControls();
            cmbTypes.Focus();
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (lstAppliances.SelectedIndex == -1) return;
            isNew = false;
            ViewOperation();
            cmbTypes.Focus();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (lstAppliances.SelectedIndex == -1) return;
            ClearControls();
            Appliance appliance = (Appliance)lstAppliances.SelectedItem;
            appliances.Remove(appliance);
            PopulateListbox();
            DoStatistics();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (cmbTypes.SelectedIndex < 0)
            {
                MessageBox.Show("Je dient een type te selecteren !", "Fout");
                return;
            }
            string applienceType = cmbTypes.SelectedItem.ToString();
            string applienceBrand = txtBrand.Text;
            int applienceConsumption = 0;
            int.TryParse(txtConsumption.Text, out applienceConsumption);
            decimal appliencePrice = 0;
            decimal.TryParse(txtPrice.Text, out appliencePrice);

            Appliance appliance;

            if (isNew)
            {
                try
                {
                    appliance = new Appliance(applienceType, applienceBrand, applienceConsumption, appliencePrice);
                    appliances.Add(appliance);
                }
                catch (Exception fout)
                {
                    MessageBox.Show(fout.Message, "Fout");
                    return;
                }
            }
            else
            {
                appliance = (Appliance)lstAppliances.SelectedItem;
                try
                {
                    appliance.ApplianceType = applienceType;
                    appliance.ApplianceBrand = applienceBrand;
                    appliance.ApplianceConsumption = applienceConsumption;
                    appliance.AppliancePrice = appliencePrice;
                }
                catch (Exception fout)
                {
                    MessageBox.Show(fout.Message, "Fout");
                    return;
                }
            }

            ClearControls();
            ViewNormal();
            PopulateListbox();
            lstAppliances.SelectedItem = appliance;
            lstAppliances_SelectionChanged(null, null);
            DoStatistics();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            ClearControls();
            ViewNormal();
            if (lstAppliances.SelectedIndex >= 0)
                lstAppliances_SelectionChanged(null, null);
        }

        private void DoStatistics()
        {
            int applianceCount = appliances.Count;
            if (applianceCount == 0)
            {
                lblStats.Content = "-";
                return;
            }
            decimal totalPrice = 0;
            int totalConsumption = 0;

            foreach (Appliance appliance in appliances)
            {
                totalPrice += appliance.AppliancePrice;
                totalConsumption += appliance.ApplianceConsumption;
            }

            decimal avgPrice = totalPrice / applianceCount;
            double avgConsumption = totalConsumption / applianceCount;

            string statistiek = "Aantal toestellen = " + applianceCount.ToString() + Environment.NewLine;
            statistiek += "Totale prijs = €" + totalPrice.ToString("0.00") + Environment.NewLine;
            statistiek += "Gemiddelde prijs/toestel = €" + avgPrice.ToString("0.00") + Environment.NewLine;
            statistiek += "Totale vermogen = " + totalConsumption.ToString() + " W" + Environment.NewLine;
            statistiek += "Gemiddeld vermogen/toestel = " + avgConsumption.ToString("0.00") + " W";
            lblStats.Content = statistiek;
        }

        //ONDERSTAANDE CODE IS EEN EXTRA EN GEEN LEERSTOF !
        // bij het afsluiten proberen we onze list op te slaan in een json file
        // bij het opstarten proberen we deze json file weer uit te lezen;

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            JSonWrite();
        }
        private bool JSonRead()
        {
            try
            {
                string fileName = Directory.GetCurrentDirectory() + "\\appliances.json";
                StreamReader streamReader = new StreamReader(fileName);
                string content = streamReader.ReadToEnd();
                streamReader.Close();
                appliances = new List<Appliance>();
                appliances = JsonSerializer.Deserialize<List<Appliance>>(content);
                return true;
            }
            catch(Exception fout)
            {
                return false;
            }
        }
        private void JSonWrite()
        {
            try
            {
                string fileName = Directory.GetCurrentDirectory() + "\\appliances.json";
                string content = JsonSerializer.Serialize<List<Appliance>>(appliances);
                StreamWriter streamWriter = new StreamWriter(fileName);
                streamWriter.Write(content);
                streamWriter.Close();
            }
            catch { }
        }


    }
}
