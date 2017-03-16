using BugSense;
using BugSense.Core.Model;
using Microsoft.Phone.Controls;
using System.IO.IsolatedStorage;
using System.Windows;

namespace XML
{
    public partial class Settings : PhoneApplicationPage
    {

        IsolatedStorageSettings appsettings = IsolatedStorageSettings.ApplicationSettings;
        string StereoStatus;
        string HydrogenStatus;
        string CarbonStatus;
            
        public Settings()
        {
            InitializeComponent();
     
            //Tracking
            BugSenseLogResult result = BugSenseHandler.Instance.LogEvent("Settings-Page geöffnet!");

            //// Stereochemie
            if (appsettings.Contains("StereoToggle"))
            {
                StereoStatus = appsettings["StereoToggle"] as string;
             
                if ("True" == StereoStatus)
                {
                    StereoToggle.IsChecked = true;
                    StereoToggle.Content = "On";
                }
                else if(appsettings["StereoToggle"] as string == "False")
                {
                    StereoToggle.IsChecked = false;
                    StereoToggle.Content = "Off";
                }
  
            }
            //// Wasserstoff
            if (appsettings.Contains("HydrogenToggle"))
            {
                HydrogenStatus = appsettings["HydrogenToggle"] as string;
                if ("True" == HydrogenStatus)
                {
                    HydrogenToggle.IsChecked = true;
                    HydrogenToggle.Content = "On";
                }
                else if (appsettings["HydrogenToggle"] as string == "False")
                {
                    HydrogenToggle.IsChecked = false;
                    HydrogenToggle.Content = "Off";
                }

            }

            //// Kohlenstoff
            if (appsettings.Contains("CarbonToggle"))
            {
                CarbonStatus = appsettings["CarbonToggle"] as string;
                if ("True" == CarbonStatus)
                {
                    CarbonToggle.IsChecked = true;
                    CarbonToggle.Content = "On";
                }
                else if (appsettings["CarbonToggle"] as string == "False")
                {
                    CarbonToggle.IsChecked = false;
                    CarbonToggle.Content = "Off";
                }

            }


        }


        //Toggle-Events um Text zw. On <--> Off zu switchen
        private void StereoToggle_Click(object sender, RoutedEventArgs e)
        {
            if (StereoToggle.IsChecked == true)
            {
                StereoToggle.Content = "On";
            }
            else
            {
                StereoToggle.Content = "Off";
            }


            ///// Stereochemie
            if (!appsettings.Contains("StereoToggle"))
            {



                if (StereoToggle.IsChecked == true)
                {
                    appsettings.Add("StereoToggle", "True");
                }
                else if (StereoToggle.IsChecked == false)
                {
                    appsettings.Add("StereoToggle", "False");
                }
            }
            else
            {
                if (StereoToggle.IsChecked == true)
                {
                    appsettings["StereoToggle"] = "True";
                }
                else if (StereoToggle.IsChecked == false)
                {
                    appsettings["StereoToggle"] = "False";
                }
            }
            appsettings.Save();

        }

        private void HydrogenToggle_Click(object sender, RoutedEventArgs e)
        {
            if (HydrogenToggle.IsChecked == true)
            {
                HydrogenToggle.Content = "On";
            }
            else
            {
                HydrogenToggle.Content = "Off";
            }


            ///// Wasserstoff
            if (!appsettings.Contains("HydrogenToggle"))
            {



                if (HydrogenToggle.IsChecked == true)
                {
                    appsettings.Add("HydrogenToggle", "True");
                }
                else if (HydrogenToggle.IsChecked == false)
                {
                    appsettings.Add("HydrogenToggle", "False");
                }
            }
            else
            {
                if (HydrogenToggle.IsChecked == true)
                {
                    appsettings["HydrogenToggle"] = "True";
                }
                else if (HydrogenToggle.IsChecked == false)
                {
                    appsettings["HydrogenToggle"] = "False";
                }
            }
            appsettings.Save();

        }

        private void CarbonToggle_Click(object sender, RoutedEventArgs e)
        {
            if (CarbonToggle.IsChecked == true)
            {
                CarbonToggle.Content = "On";
            }
            else
            {
                CarbonToggle.Content = "Off";
            }


            ///// Kohlenstoff
            if (!appsettings.Contains("CarbonToggle"))
            {



                if (CarbonToggle.IsChecked == true)
                {
                    appsettings.Add("CarbonToggle", "True");
                }
                else if (CarbonToggle.IsChecked == false)
                {
                    appsettings.Add("CarbonToggle", "False");
                }
            }
            else
            {
                if (CarbonToggle.IsChecked == true)
                {
                    appsettings["CarbonToggle"] = "True";
                }
                else if (CarbonToggle.IsChecked == false)
                {
                    appsettings["CarbonToggle"] = "False";
                }
            }
            appsettings.Save();


        }

        private void Button_Click(object sender, RoutedEventArgs e)
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
            }
            else if (!IsolatedStorageSettings.ApplicationSettings.Contains("Suchbegriff"))
            {
                MessageBox.Show("Your search history is empty!");
            }
            
        }


    }
}