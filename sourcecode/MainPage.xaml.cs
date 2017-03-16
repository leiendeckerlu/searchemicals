using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Coding4Fun.Toolkit.Controls;
using System.IO.IsolatedStorage;
using BugSense;
using BugSense.Core.Model;
using Microsoft.Phone.Tasks;
using System.Threading.Tasks;
using System.Diagnostics;



namespace XML
{

    public partial class MainPage : PhoneApplicationPage
    {
        IsolatedStorageSettings appsettings = IsolatedStorageSettings.ApplicationSettings;
        //string StereoStatus;
        //string HydrogenStatus;
        //string CarbonStatus;

        // Konstruktor
        public MainPage()
        {
            InitializeComponent();

            //Tracking
            BugSenseLogResult result = BugSenseHandler.Instance.LogEvent("Main-Page geöffnet!");


            // Checkbox-Initialisierung
            CheckBox StereoCheckbox = new CheckBox();
            CheckBox WasserstoffCheckbox = new CheckBox();
            CheckBox KohlenstoffCheckbox = new CheckBox();

            // Tracking
            byte[] id = (byte[])Microsoft.Phone.Info.DeviceExtendedProperties.GetValue("DeviceUniqueId");
            string deviceID = Convert.ToBase64String(id);
            BugSenseHandler.Instance.UserIdentifier = deviceID;

        }


        // wenn diese view aufgerufen wird, passiert folgendes:
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {


            //BugSense Last Crashes

            this.Loaded += async (sender, args) =>
            {
                // Delay to run to give a chance for the pending requests to send on the InitAndStartSession.
                await Task.Delay(5000);
                int totalCrashes = await BugSenseHandler.Instance.GetTotalCrashesNum();
                Debug.WriteLine("TotalCrashes: {0}", totalCrashes);
                if (totalCrashes > 3)
                {
                    await BugSenseHandler.Instance.ClearTotalCrashesNum();
                    Debug.WriteLine("TotalCrashes after clear: {0}", await BugSenseHandler.Instance.GetTotalCrashesNum());
                }
                Debug.WriteLine("Last Crash Error ID: {0}", await BugSenseHandler.Instance.GetLastCrashId());
            };

            /////////






  
            AnfrageText.Text = "Type here...";


            // Initialiserung der Komponenten zur Festlegung der Settings in der MainView
            //IsolatedStorageSettings appsettings = IsolatedStorageSettings.ApplicationSettings;
            string StereoStatus;
            string HydrogenStatus;
            string CarbonStatus;


            if (appsettings.Contains("StereoToggle"))
            {


                StereoStatus = appsettings["StereoToggle"] as string;
                HydrogenStatus = appsettings["HydrogenToggle"] as string;
                CarbonStatus = appsettings["CarbonToggle"] as string;

                //// Stereochemie
                if (appsettings.Contains("StereoToggle"))
                {
                    if ("True" == StereoStatus)
                    {
                        StereoCheckbox.IsChecked = true;
                    }
                    else if (appsettings["StereoToggle"] as string == "False")
                    {
                        StereoCheckbox.IsChecked = false;
                    }
                    else if(!appsettings.Contains("StereoToggle"))
                    {
                        StereoCheckbox.IsChecked = false;
                    }

                }
                //// Wasserstoff
                if (appsettings.Contains("HydrogenToggle"))
                {

                    if ("True" == HydrogenStatus)
                    {
                        WasserstoffCheckbox.IsChecked = true;
                    }
                    else if (appsettings["HydrogenToggle"] as string == "False")
                    {
                        WasserstoffCheckbox.IsChecked = false;
                    }
                    else if (!appsettings.Contains("HydrogenToggle"))
                    {
                        WasserstoffCheckbox.IsChecked = false;
                    }


                }

                //// Kohlenstoff
                if (appsettings.Contains("CarbonToggle"))
                {

                    if ("True" == CarbonStatus)
                    {
                        KohlenstoffCheckbox.IsChecked = true;
                    }
                    else if (appsettings["CarbonToggle"] as string == "False")
                    {
                        KohlenstoffCheckbox.IsChecked = false;
                    }
                    else if (!appsettings.Contains("CarbonToggle"))
                    {
                        KohlenstoffCheckbox.IsChecked = false;
                    }


                }

            }
             
        } 


            //Beispielcode zur Lokalisierung der ApplicationBar
            //BuildLocalizedApplicationBar();
       

        // wird der Such-Button geklickt passiert folgendes:
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(AnfrageText.Text))
            {
                MessageBox.Show("Please enter a valid search term. For example: ethanol, methan, aspirin, ...");
            }
            else if ("Type here..." == AnfrageText.Text)
            {
                MessageBox.Show("Please enter a valid search term. For example: ethanol, methan, aspirin, ...");
            }
            else
            {
                string Suchbegriff = AnfrageText.Text;
                NavigationService.Navigate(new Uri("/SecondPage.xaml?suchbegriff=" + Suchbegriff + "&stereo=" + StereoCheckbox.IsChecked + "&kohlenstoff=" + KohlenstoffCheckbox.IsChecked + "&wasserstoff=" + WasserstoffCheckbox.IsChecked, UriKind.Relative));
            }

        } 

        //öffnet about-view der app
        private void AboutClick(object sender, EventArgs e)
        {
            //Tracking
            BugSenseLogResult result = BugSenseHandler.Instance.LogEvent("About geöffnet!");
 

            AboutPrompt aboutMe = new AboutPrompt();
            aboutMe.Show("Lukas Leiendecker", "@searchemicals", "searchemicals@outlook.com");
        }

        // öffnet privacy&legal-infos
        private void PrivacyClick(object sender, EventArgs e)
        {
            //Tracking
            BugSenseLogResult result = BugSenseHandler.Instance.LogEvent("Privacy geöffnet!");

            AboutPrompt PrivacyPrompt = new AboutPrompt();
            PrivacyPrompt.Title = "Privacy Statement";
            PrivacyPrompt.Body = new TextBlock { Text = "This application uses the CADD Group Chemoinformatics Tools and User Services. The server only stores what every other web server logs: IP address and name of the page requested. For some of their services the server also logs the structures submitted and the queries entered. These latter logs are kept only to allow them to evaluate and improve the services. The log files are not, and cannot be, cross-referenced since they do not contain data that would allow such mutual cross-referencing. The application itself works as an front end to the above mentioned web services.Simply speaking, there is no interested at all in knowing who conducts what kind of searches on their servers or with this application.There is no further connection between the developer of this application and the provider of the above mentioned web services. There is no warranty for the program, to the extent permitted by applicable law. Except when otherwise stated in writing the copyright holders and/or other parties provide the program “as is” without warranty of any kind, either expressed or implied, including, but not limited to, the implied warranties of merchantability and fitness for a particular purpose. The entire risk as to the quality and performance of the program is with you. Should the program prove defective, you assume the cost of all necessary servicing, repair or correction. In no event unless required by applicable law or agreed to in writing will any copyright holder, or any other party who modifies and/or conveys the program as permitted above, be liable to you for damages, including any general, special, incidental or consequential damages arising out of the use or inability to use the program (including but not limited to loss of data or data being rendered inaccurate or losses sustained by you or third parties or a failure of the program to operate with any other programs), even if such holder or other party has been advised of the possibility of such damages.Further information regarding the policy of the used web services can be obtained at the link below.", TextWrapping = TextWrapping.Wrap };
            PrivacyPrompt.Footer = "http://cactus.nci.nih.gov/ncicadd/privacy.html";
            PrivacyPrompt.Show();
        }

        // öffne settings-view
        private void SettingsClick(object sender, EventArgs e)
        {

            NavigationService.Navigate(new Uri("/Settings.xaml", UriKind.Relative));

        }


        //öffne help-view mit erklärungen
        private void HelpClick(object sender, EventArgs e)
        {

            NavigationService.Navigate(new Uri("/Help.xaml", UriKind.Relative));

        }

        //öffne verlauf-view mit (erfolgreichem)-suchverlauf
        private void HistoryClick(object sender, EventArgs e)
        {

            NavigationService.Navigate(new Uri("/Verlauf.xaml", UriKind.Relative));

        }


        //managing von textfeld-eingaben
        private void AnfrageText_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key.Equals(System.Windows.Input.Key.Enter))
            {
                if (string.IsNullOrEmpty(AnfrageText.Text))
                {
                    MessageBox.Show("Please enter a valid search term. For example: ethanol, methan, aspirin, ...");
                }
                else
                {
                    string Suchbegriff = AnfrageText.Text;
                    NavigationService.Navigate(new Uri("/SecondPage.xaml?suchbegriff=" + Suchbegriff + "&stereo=" + StereoCheckbox.IsChecked + "&kohlenstoff=" + KohlenstoffCheckbox.IsChecked + "&wasserstoff=" + WasserstoffCheckbox.IsChecked, UriKind.Relative));
                }
                }
        }

        private void AnfrageText_GotFocus(object sender, RoutedEventArgs e)
        {
            if (AnfrageText.Text.Equals("Type here...", StringComparison.OrdinalIgnoreCase))
            {
                AnfrageText.Text = string.Empty;
            } 
        }

        private void AnfrageText_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(AnfrageText.Text))
            {
                AnfrageText.Text = "Type here...";
            }
        }

        //Implementing Review-Task
        private void RateClick(object sender, EventArgs e)
        {
            //Tracking
            BugSenseLogResult result = BugSenseHandler.Instance.LogEvent("Rate this App geöffnet!");
            //Review-Task
            MarketplaceReviewTask marketplaceReviewTask = new MarketplaceReviewTask();
            marketplaceReviewTask.Show();
        }

        //AppIconBar SearchIcon clicked
        private void SearchClick(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(AnfrageText.Text))
            {
                MessageBox.Show("Please enter a valid search term. For example: ethanol, methan, aspirin, ...");
            }
            else if ("Type here..." == AnfrageText.Text)
            {
                MessageBox.Show("Please enter a valid search term. For example: ethanol, methan, aspirin, ...");
            }
            else
            {
                string Suchbegriff = AnfrageText.Text;
                NavigationService.Navigate(new Uri("/SecondPage.xaml?suchbegriff=" + Suchbegriff + "&stereo=" + StereoCheckbox.IsChecked + "&kohlenstoff=" + KohlenstoffCheckbox.IsChecked + "&wasserstoff=" + WasserstoffCheckbox.IsChecked, UriKind.Relative));
            }
        }


        // wechsel zu favoriten-liste
        private void FavoriteClick(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Favoriten.xaml", UriKind.Relative));
        }

        private void StereoCheckbox_Click(object sender, RoutedEventArgs e)
        {

            ///// Stereochemie
            if (!appsettings.Contains("StereoToggle"))
            {

                if (StereoCheckbox.IsChecked == true)
                {
                    appsettings.Add("StereoToggle", "True");
                }
                else if (StereoCheckbox.IsChecked == false)
                {
                    appsettings.Add("StereoToggle", "False");
                }
            }
            else
            {
                if (StereoCheckbox.IsChecked == true)
                {
                    appsettings["StereoToggle"] = "True";
                }
                else if (StereoCheckbox.IsChecked == false)
                {
                    appsettings["StereoToggle"] = "False";
                }
            }
            appsettings.Save();
        }

        private void WasserstoffCheckbox_Click(object sender, RoutedEventArgs e)
        {
            ///// Wasserstoff
            if (!appsettings.Contains("HydrogenToggle"))
            {

                if (WasserstoffCheckbox.IsChecked == true)
                {
                    appsettings.Add("HydrogenToggle", "True");
                }
                else if (WasserstoffCheckbox.IsChecked == false)
                {
                    appsettings.Add("HydrogenToggle", "False");
                }
            }
            else
            {
                if (WasserstoffCheckbox.IsChecked == true)
                {
                    appsettings["HydrogenToggle"] = "True";
                }
                else if (WasserstoffCheckbox.IsChecked == false)
                {
                    appsettings["HydrogenToggle"] = "False";
                }
            }
            appsettings.Save();
        }

        private void KohlenstoffCheckbox_Click(object sender, RoutedEventArgs e)
        {
            ///// Kohlenstoff
            if (!appsettings.Contains("CarbonToggle"))
            {

                if (KohlenstoffCheckbox.IsChecked == true)
                {
                    appsettings.Add("CarbonToggle", "True");
                }
                else if (KohlenstoffCheckbox.IsChecked == false)
                {
                    appsettings.Add("CarbonToggle", "False");
                }
            }
            else
            {
                if (KohlenstoffCheckbox.IsChecked == true)
                {
                    appsettings["CarbonToggle"] = "True";
                }
                else if (KohlenstoffCheckbox.IsChecked == false)
                {
                    appsettings["CarbonToggle"] = "False";
                }
            }
            appsettings.Save();
        }



    }
}