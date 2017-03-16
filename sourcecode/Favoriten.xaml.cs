using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using BugSense.Core.Model;
using BugSense;
using System.IO.IsolatedStorage;

namespace XML
{
    public partial class Favoriten : PhoneApplicationPage
    {



        string StereoStatus;
        string HydrogenStatus;
        string CarbonStatus;

        bool StereoSend;
        bool HydrogenSend;
        bool CarbonSend;




        public Favoriten()
        {
            InitializeComponent();


            //Tracking
            BugSenseLogResult result = BugSenseHandler.Instance.LogEvent("Favoriten-Page geöffnet!");

            List<FavoritenEintrag> source = new List<FavoritenEintrag>();


            if (IsolatedStorageSettings.ApplicationSettings.Contains("Favorit"))
            {
                source.Add(new FavoritenEintrag(IsolatedStorageSettings.ApplicationSettings["Favorit"] as string));
            }


            int r = 0;
            for (bool check; check = IsolatedStorageSettings.ApplicationSettings.Contains("Favorit" + r); r++)
            {
                source.Add(new FavoritenEintrag(IsolatedStorageSettings.ApplicationSettings["Favorit" + r] as string));
            }


            Favorit.ItemsSource = source;

        }




        public class FavoritenEintrag
        {
            public string Favorit
            {
                get;
                set;

            }

            public FavoritenEintrag(string favorit)
            {
                this.Favorit = favorit;
            }


        }

        private void Favorites_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Favorit.SelectedItem == null)
                return;


            var TabellenWert = ((FavoritenEintrag)Favorit.SelectedItem).Favorit;
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
                Favorit.SelectedItem = null;
            }
            else if (!IsolatedStorageSettings.ApplicationSettings.Contains("StereoToggle"))
            {
                StereoSend = false;
                CarbonSend = false;
                HydrogenSend = false;
                NavigationService.Navigate(new Uri("/SecondPage.xaml?suchbegriff=" + TabellenWert + "&stereo=" + StereoSend + "&kohlenstoff=" + CarbonSend + "&wasserstoff=" + HydrogenSend, UriKind.Relative));
                Favorit.SelectedItem = null;
            }


        }

        private void DeleteFavoritesClick(object sender, EventArgs e)
        {

            if (IsolatedStorageSettings.ApplicationSettings.Contains("Favorit"))
            {
                IsolatedStorageSettings.ApplicationSettings.Remove("Favorit");

                int s = 0;
                for (bool delete; delete = IsolatedStorageSettings.ApplicationSettings.Contains("Favorit" + s); s++)
                {
                    IsolatedStorageSettings.ApplicationSettings.Remove("Favorit" + s);
                }
                MessageBox.Show("Favorites deleted successfully!");
                if (NavigationService.CanGoBack == true)
                {
                    NavigationService.GoBack();
                }
                else
                {
                    MessageBox.Show("You cannot go back!");
                }
            }
            else if (!IsolatedStorageSettings.ApplicationSettings.Contains("Favorit"))
            {
                MessageBox.Show("Your favorites are empty!");
            }


        }

    }
}