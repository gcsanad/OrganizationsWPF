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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OrgWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Organization> szervezetek = new List<Organization>();
        private void Betoltes(string fajlNeve)
        {
            foreach (var sor in File.ReadAllLines(fajlNeve).Skip(1))
            {
                szervezetek.Add(new Organization(sor.Split(';')));
            }
        }
        public MainWindow()
        {
            InitializeComponent();
            Betoltes("organizations-100000.csv");
            dgAdatok.ItemsSource = szervezetek;
            cbOrszag.ItemsSource = szervezetek.Select(x => x.Country).OrderBy(x => x).Distinct().ToList();
            cbOrszag.Items.Add("Minden ország");
            cbEv.ItemsSource = szervezetek.Select(x => x.Founded).OrderBy(x => x).Distinct().ToList();
            cbEv.Items.Add("Minden év");
            lblOsszesDolgozo.Content = szervezetek.Sum(x => x.EmployeesNumber);
        }

        private void dgAdatok_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgAdatok.SelectedItem is Organization)
            {
                Organization valasztottMezo = dgAdatok.SelectedItem as Organization;
                lblAzon.Content = $"Azonosító: {valasztottMezo.Id.ToString()}";
                lblWeb.Content = $"Webhely: {valasztottMezo.Website}";
                lblLeiras.Content = $"Leírás: {valasztottMezo.Description}";
            }
            else
            {
                MessageBox.Show("Hiba!");
            }
        }

        private void cbEv_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var szurtLista = szervezetek.Where(x => x.Founded.ToString() == cbEv.SelectedItem.ToString());
            dgAdatok.ItemsSource = szurtLista;
            lblOsszesDolgozo.Content = szurtLista.Sum(x => x.EmployeesNumber);
            
            if (cbEv.SelectedIndex == cbEv.Items.Count - 1)
            {
                dgAdatok.ItemsSource = szervezetek;
            }
            else if (cbOrszag.SelectedIndex != null || cbOrszag.SelectedIndex != cbOrszag.Items.Count - 1)
            {
                szurtLista = szervezetek.Where(x => x.Founded.ToString() == cbEv.SelectedItem.ToString() && x.Country.ToString() == cbOrszag.SelectedItem.ToString());
            }
        }

        private void cbOrszag_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var szurtLista = szervezetek.Where(x => x.Country.ToString() == cbOrszag.SelectedItem.ToString());
            dgAdatok.ItemsSource = szurtLista;
            lblOsszesDolgozo.Content = szurtLista.Sum(x => x.EmployeesNumber);
            if (cbOrszag.SelectedIndex == cbOrszag.Items.Count - 1)
            {
                dgAdatok.ItemsSource = szervezetek;
            }
            else if (cbEv.SelectedIndex != null || cbEv.SelectedIndex != cbEv.Items.Count - 1)
            {
                szurtLista = szervezetek.Where(x => x.Founded.ToString() == cbEv.SelectedItem.ToString() && x.Country.ToString() == cbOrszag.SelectedItem.ToString());
            }
        }
    }
}
