using BugSense;
using BugSense.Core.Model;
using Microsoft.Phone.Controls;
using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace XML
{


    public partial class Verlauf : PhoneApplicationPage
    {

        string StereoStatus;
        string HydrogenStatus;
        string CarbonStatus;

        bool StereoSend;
        bool HydrogenSend;
        bool CarbonSend;


        public Verlauf()
        {
            InitializeComponent();

            //Tracking
            BugSenseLogResult result = BugSenseHandler.Instance.LogEvent("History-Page geöffnet!");

            List<VerlaufEintrag> source = new List<VerlaufEintrag>();
            
            
            if (IsolatedStorageSettings.ApplicationSettings.Contains("Suchbegriff"))
            {
                source.Add(new VerlaufEintrag(IsolatedStorageSettings.ApplicationSettings["Suchbegriff"] as string));
            }


            int r = 0;
            for (bool check; check = IsolatedStorageSettings.ApplicationSettings.Contains("Suchbegriff" + r); r++)
            {
                source.Add(new VerlaufEintrag(IsolatedStorageSettings.ApplicationSettings["Suchbegriff" + r] as string));
            }


            Suchverlauf.ItemsSource = source;


        }



        public class VerlaufEintrag
        {
            public string Suchbegriff
            {
                get;
                set;

            }

            public VerlaufEintrag(string suchbegriff)
            {
                this.Suchbegriff = suchbegriff;
            }


        }

        private void Suchverlauf_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Suchverlauf.SelectedItem == null)
                return;


            var TabellenWert = ((VerlaufEintrag)Suchverlauf.SelectedItem).Suchbegriff;
            //MessageBox.Show(TabellenWert);


            if (IsolatedStorageSettings.ApplicationSettings.Contains("StereoToggle"))
            {


                StereoStatus = IsolatedStorageSettings.ApplicationSettings["StereoToggle"] as string;
                HydrogenStatus = IsolatedStorageSettings.ApplicationSettings["HydrogenToggle"] as string;
                CarbonStatus = IsolatedStorageSettings.ApplicationSettings["CarbonToggle"] as string;

                //// Stereochemie
                if (IsolatedStorageSettings.ApplicationSettings.Contains("StereoToggle"))
                {
                    if ("True" == StereoStatus)
                    {
                        StereoSend = true;
                    }
                    else if (IsolatedStorageSettings.ApplicationSettings["StereoToggle"] as string == "False")
                    {
                        StereoSend = false;
                    }

                }
                //// Wasserstoff
                if (IsolatedStorageSettings.ApplicationSettings.Contains("HydrogenToggle"))
                {

                    if ("True" == HydrogenStatus)
                    {
                        HydrogenSend = true;
                    }
                    else if (IsolatedStorageSettings.ApplicationSettings["HydrogenToggle"] as string == "False")
                    {
                       HydrogenSend = false;
                    }

                }

                //// Kohlenstoff
                if (IsolatedStorageSettings.ApplicationSettings.Contains("CarbonToggle"))
                {

                    if ("True" == CarbonStatus)
                    {
                        CarbonSend = true;
                    }
                    else if (IsolatedStorageSettings.ApplicationSettings["CarbonToggle"] as string == "False")
                    {
                        CarbonSend = false;
                    }

                }



                NavigationService.Navigate(new Uri("/SecondPage.xaml?suchbegriff=" + TabellenWert + "&stereo=" + StereoSend + "&kohlenstoff=" + CarbonSend + "&wasserstoff=" + HydrogenSend, UriKind.Relative));
                Suchverlauf.SelectedItem = null;
            }
            else if (!IsolatedStorageSettings.ApplicationSettings.Contains("StereoToggle"))
            {
                StereoSend = false;
                CarbonSend = false;
                HydrogenSend = false;
                NavigationService.Navigate(new Uri("/SecondPage.xaml?suchbegriff=" + TabellenWert + "&stereo=" + StereoSend + "&kohlenstoff=" + CarbonSend + "&wasserstoff=" + HydrogenSend, UriKind.Relative));
                Suchverlauf.SelectedItem = null;
            }


        }

        private void DeleteHistoryClick(object sender, EventArgs e)
        {
            if (IsolatedStorageSettings.ApplicationSettings.Contains("Suchbegriff"))
            {
                IsolatedStorageSettings.ApplicationSettings.Remove("Suchbegriff");

                int s = 0;
                for (bool delete; delete = IsolatedStorageSettings.ApplicationSettings.Contains("Suchbegriff" + s); s++)
                {
                    IsolatedStorageSettings.ApplicationSettings.Remove("Suchbegriff" + s);
                }
                MessageBox.Show("Search history deleted successfully!");
                if (NavigationService.CanGoBack == true)
                {
                    NavigationService.GoBack();
                }
                else
                {
                    MessageBox.Show("You cannot go back!");
                }
            }
            else if (!IsolatedStorageSettings.ApplicationSettings.Contains("Suchbegriff"))
            {
                MessageBox.Show("Your search history is empty!");
            }


        }

    }
}