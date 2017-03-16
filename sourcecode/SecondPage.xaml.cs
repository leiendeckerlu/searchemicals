using BugSense;
using BugSense.Core.Model;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Media.PhoneExtensions;
using System;
using System.IO;
using System.IO.IsolatedStorage;
using System.Net;
using System.Net.NetworkInformation;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Resources;
using System.Xml.Linq;
using Windows.ApplicationModel.DataTransfer;

namespace XML
{

    public partial class Page1 : PhoneApplicationPage
    {
        private WebClient ChemieClient;
        private WebClient BildClient;
        BitmapImage Bild = new BitmapImage();
        ProgressIndicator fortschritt;

        int FavoritHinzugefuegt = 0;
        int SpeicherHinzugefuegt = 0;

        const string tempJpeg = "TempJPEG";
        ShareMediaTask ShareMolecule = new ShareMediaTask();
        

        IsolatedStorageSettings appsettings = IsolatedStorageSettings.ApplicationSettings;

        string[] SchneideErgebnis;


        public Page1()
        {
            InitializeComponent();


            //Tracking
            BugSenseLogResult result = BugSenseHandler.Instance.LogEvent("Second-Page geöffnet!");


            // Webservice-Hinweis beim ersten Start nach Update!
             if (!appsettings.Contains("FirstStart"))
             {
                    appsettings.Add("FirstStart", "True");
                    MessageBox.Show("Please remember that SearCHemicals uses an webservice to look up chemical structures. This might lead in the inability to look up results due to server downtimes!");
                
                    //Tracking
                    BugSenseLogResult result2 = BugSenseHandler.Instance.LogEvent("Number of Users");
             }
             else if (appsettings.Contains("FirstStart"))
             {
                 //Tracking
                 BugSenseLogResult result2 = BugSenseHandler.Instance.LogEvent("Returning User");
             }
            appsettings.Save();



            if (NetworkInterface.GetIsNetworkAvailable())
            {

                // WebClient Initialisierung
                ChemieClient = new WebClient();
                ChemieClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(ChemieClient_DownloadStringCompleted);

                BildClient = new WebClient();
                BildClient.OpenReadCompleted += new OpenReadCompletedEventHandler(BildClient_OpenReadCompleted);
                SystemTray.SetIsVisible(this, true);
                SystemTray.SetOpacity(this, 1);  // unebdingt =1 lassen, sonst springt die View hoch!!!! ARGH -.-

                // fortschritts-balken initialisieren
                fortschritt = new ProgressIndicator();
                fortschritt.IsVisible = true;
                fortschritt.IsIndeterminate = true;
                fortschritt.Text = "Loading...";
                SystemTray.SetProgressIndicator(this, fortschritt);
            }
            else
            {
                MessageBox.Show("Please connect to the internet!");
                if (NavigationService.CanGoBack == true)
                {
                    NavigationService.GoBack();
                }
                else
                {
                    MessageBox.Show("You cannot go back!");
                }
            }


        }

        

        // NavigationService.Navigate(new Uri("/SecondPage.xaml?suchbegriff="+ Suchbegriff + "&stereo="+ StereoCheckbox.IsChecked + "&kohlenstoff=" + KohlenstoffCheckbox.IsChecked + "&wasserstoff=" +WasserstoffCheckbox.IsChecked, UriKind.Relative));
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            string suchbegriff;
            string stereo;
            string wasserstoff;
            string kohlenstoff;
         


            if (NavigationContext.QueryString.TryGetValue("suchbegriff", out suchbegriff))
            {
                string TextUrl = "http://cactus.nci.nih.gov/chemical/structure/" + suchbegriff + "/iupac_name/xml";
                ChemieClient.DownloadStringAsync(new System.Uri(TextUrl));
            }
            if (NavigationContext.QueryString.TryGetValue("stereo", out stereo) && NavigationContext.QueryString.TryGetValue("kohlenstoff", out kohlenstoff) && NavigationContext.QueryString.TryGetValue("wasserstoff", out wasserstoff))
            {


                if (stereo == "True" && wasserstoff == "True" && kohlenstoff == "True")
                {
                    string BildUrl1 = "http://cactus.nci.nih.gov/chemical/structure/" + suchbegriff + "/image?showstereo=1&hsymbol=all&csymbol=all&width=600&height=600&linewidth=2&symbolfontsize=16";
                    BildClient.OpenReadAsync(new System.Uri(BildUrl1));
                }
                else if (stereo == "False" && wasserstoff == "False" && kohlenstoff == "False")
                {
                    string BildUrl2 = "http://cactus.nci.nih.gov/chemical/structure/" + suchbegriff + "/image?showstereo=0&hsymbol=special&csymbol=special&width=600&height=600&linewidth=2&symbolfontsize=16";
                    BildClient.OpenReadAsync(new System.Uri(BildUrl2));
                }
                else if (stereo == "True" && wasserstoff == "False" && kohlenstoff == "False")
                {
                    string BildUrl3 = "http://cactus.nci.nih.gov/chemical/structure/" + suchbegriff + "/image?showstereo=1&hsymbol=special&csymbol=special&width=600&height=600&linewidth=2&symbolfontsize=16";
                    BildClient.OpenReadAsync(new System.Uri(BildUrl3));
                }
                else if (stereo == "False" && wasserstoff == "True" && kohlenstoff == "False")
                {
                    string BildUrl4 = "http://cactus.nci.nih.gov/chemical/structure/" + suchbegriff + "/image?showstereo=0&hsymbol=all&csymbol=special&width=600&height=600&linewidth=2&symbolfontsize=16";
                    BildClient.OpenReadAsync(new System.Uri(BildUrl4));
                }
                else if (stereo == "False" && wasserstoff == "False" && kohlenstoff == "True")
                {
                    string BildUrl5 = "http://cactus.nci.nih.gov/chemical/structure/" + suchbegriff + "/image?showstereo=0&hsymbol=special&csymbol=all&width=600&height=600&linewidth=2&symbolfontsize=16";
                    BildClient.OpenReadAsync(new System.Uri(BildUrl5));
                }
                else if (stereo == "False" && wasserstoff == "True" && kohlenstoff == "True")
                {
                    string BildUrl6 = "http://cactus.nci.nih.gov/chemical/structure/" + suchbegriff + "/image?showstereo=0&hsymbol=all&csymbol=all&width=600&height=600&linewidth=2&symbolfontsize=16";
                    BildClient.OpenReadAsync(new System.Uri(BildUrl6));
                }
                else if (stereo == "True" && wasserstoff == "False" && kohlenstoff == "True")
                {
                    string BildUrl7 = "http://cactus.nci.nih.gov/chemical/structure/" + suchbegriff + "/image?showstereo=1&hsymbol=special&csymbol=all&width=600&height=600&linewidth=2&symbolfontsize=16";
                    BildClient.OpenReadAsync(new System.Uri(BildUrl7));
                }
                else if (stereo == "True" && wasserstoff == "True" && kohlenstoff == "False")
                {
                    string BildUrl8 = "http://cactus.nci.nih.gov/chemical/structure/" + suchbegriff + "/image?showstereo=1&hsymbol=all&csymbol=special&width=600&height=600&linewidth=2&symbolfontsize=16";
                    BildClient.OpenReadAsync(new System.Uri(BildUrl8));
                }


            }
                

        }
     
        //downlad des bildes fertig und speicherung desselben
        private void BildClient_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            
         
            if (e.Error != null)
            {

                //Tracking
                BugSenseLogResult result = BugSenseHandler.Instance.LogEvent("Picture load error!");


              //MessageBox.Show("Picture load error!");
                

            }
            else if (e.Error == null && !e.Cancelled)
            {
                try
                {
                    Bild.SetSource(e.Result);
                    ImageBox.Source = Bild;

                            ////////////////////////////
                            // MSDN-blog(ck)
                            const string tempJpeg = "TempJPEG";
                            var streamResourceInfo = new StreamResourceInfo(e.Result, null);

                            var userStoreForApplication = IsolatedStorageFile.GetUserStoreForApplication();
                            if (userStoreForApplication.FileExists(tempJpeg))
                            {
                                userStoreForApplication.DeleteFile(tempJpeg);
                            }

                            var isolatedStorageFileStream = userStoreForApplication.CreateFile(tempJpeg);
                            var bitmapImage = new BitmapImage { CreateOptions = BitmapCreateOptions.None };
                            bitmapImage.SetSource(streamResourceInfo.Stream);

                            var writeableBitmap = new WriteableBitmap(bitmapImage);
                            writeableBitmap.SaveJpeg(isolatedStorageFileStream, writeableBitmap.PixelWidth, writeableBitmap.PixelHeight, 0, 85);
                          
                            isolatedStorageFileStream.Close();

                            //MessageBox.Show("Bild in IsolatedStorage!");

                            // ende MSDN-blog(ck)


                    // stoppe fortschrittsbalken kurz nach beendigung des ladevorganges
                    fortschritt.IsVisible = false;
                    fortschritt.IsIndeterminate = false;
                    SystemTray.SetProgressIndicator(this, fortschritt);    
              

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Download-Error!");
                }
              
            }

        }


        // Sind die Daten gedownloadet wird auf Fehler gechecked und dargestellt:
        private void ChemieClient_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                //Tracking
                BugSenseLogResult result = BugSenseHandler.Instance.LogEvent("Fehler beim Download des Bildes");

                MessageBox.Show("An error occurred: It seems as if the Chemical Identifier Resolver is down. Please try later again!");
                if (NavigationService.CanGoBack == true)
                {
                    NavigationService.GoBack();
                }
                else
                {
                    MessageBox.Show("You cannot go back!");
                }


                //MessageBox.Show("Error");
            }
            else
            {
                var xml = XDocument.Parse(e.Result, LoadOptions.None);

                var request = xml.Descendants("request");
                var item = xml.Descendants("item");



                // request-data-notation
                foreach (var requestElement in request.Descendants("data"))
                {
                    RequestBlock.Text = "Search term: " + requestElement.Attribute("notation").Value;

                }
                
                string Rohdaten = xml.ToString();

                if (Rohdaten.Length >= 200)
                {

                    string[] SchneideZwischenText = new string[] { @"pubchem_iupac_name"">" };

                    string[] SchneideZwischenErgebnis = Rohdaten.Split(SchneideZwischenText, StringSplitOptions.None);


                    string[] SchneideText = new string[] { "</item>" };


                    // Im header global verfügbar gemacht, damit auch favoriten add darauf zugreifen kann!
                    //string[] SchneideErgebnis;
                        
                    SchneideErgebnis = SchneideZwischenErgebnis[1].Split(SchneideText, StringSplitOptions.None);

                    SuchBlock.Text = "IUPAC nomenclature: " + SchneideErgebnis[0];



                    IsolatedStorageSettings history = IsolatedStorageSettings.ApplicationSettings;

                    if (!history.Contains("Suchbegriff"))
                    {
                        history.Add("Suchbegriff", SchneideErgebnis[0]);
                        //MessageBox.Show("Suchbegriff");
                    }
                    else if (history.Contains("Suchbegriff"))
                    {
                        int i = 0;
                        for (bool check; check = history.Contains("Suchbegriff" + i); )
                        {
                            i++;
                        }

                        history.Add("Suchbegriff" + i, SchneideErgebnis[0]);
                        //MessageBox.Show("Suchbegriff" + i);
                    }

                    history.Save();



                }
                else if (Rohdaten.Length <= 200)
                {
                    MessageBox.Show("Invalid search term. Please try again!");
                    if (NavigationService.CanGoBack == true)
                    {
                        NavigationService.GoBack();
                    }
                    else
                    {
                        MessageBox.Show("You cannot go back!");
                    }
                }

            }
        }

        // speichert molekül in mediathek
        private void SaveMoleculeClick(object sender, EventArgs e)
        {
            if (SpeicherHinzugefuegt == 0)
            {
                //Tracking
                BugSenseLogResult result = BugSenseHandler.Instance.LogEvent("Bild gespeichert!");


                if (false == BildClient.IsBusy)
                {

                    var userStoreForApplication = IsolatedStorageFile.GetUserStoreForApplication();

                    var testisolatedStorageFileStream = userStoreForApplication.OpenFile(tempJpeg, FileMode.Open, FileAccess.Read);


                    var mediaLibrary = new MediaLibrary();
                    var picture = mediaLibrary.SavePicture(string.Format("SavedPicture{0}.jpg", DateTime.Now), testisolatedStorageFileStream);

                    ShareMolecule.FilePath = picture.GetPath();

                    MessageBox.Show("Picture of structural formula saved successfully to your media library!");

                    testisolatedStorageFileStream.Close();

                }
                else if (true == BildClient.IsBusy)
                {
                    MessageBox.Show("Please wait until the picture of the structural formula is downloaded!");
                }
                SpeicherHinzugefuegt = 1;
            }
            else if (SpeicherHinzugefuegt == 1)
            { 
                MessageBox.Show("You already have saved this molecule to your media library!");
            }

        }

        // bild in sozialen netzwerken teilen
        private void ShareMoleculeClick(object sender, EventArgs e)
        {

            //Tracking
            BugSenseLogResult result = BugSenseHandler.Instance.LogEvent("Bild geteilt!");


      
            if(string.IsNullOrEmpty(ShareMolecule.FilePath))
            {
                MessageBox.Show("Please save the structural formula before sharing it!");
            } 
            else
            {
                ShareMolecule.Show();
            }
        }

        // neue suchanfrage starten
        private void DeleteMoleculeClick(object sender, EventArgs e)
        {

            //Tracking
            BugSenseLogResult result = BugSenseHandler.Instance.LogEvent("Suchanfrage gelöscht!");


            if (NavigationService.CanGoBack == true)
            {
                NavigationService.GoBack();
            }
            else
            {
                MessageBox.Show("You cannot go back!");
            }
        }


        // füge aktuelles molekül zu favoriten hinzu!
        private void FavoriteAddClick(object sender, EventArgs e)
        {

            if (FavoritHinzugefuegt == 0)
            {
                IsolatedStorageSettings favorite = IsolatedStorageSettings.ApplicationSettings;

                if (!favorite.Contains("Favorit"))
                {
                    favorite.Add("Favorit", SchneideErgebnis[0]);
                    //MessageBox.Show("Suchbegriff");
                }
                else if (favorite.Contains("Favorit"))
                {
                    int i = 0;
                    for (bool check; check = favorite.Contains("Favorit" + i); )
                    {
                        i++;
                    }

                    favorite.Add("Favorit" + i, SchneideErgebnis[0]);
                    //MessageBox.Show("Suchbegriff" + i);
                }

                favorite.Save();
                FavoritHinzugefuegt = 1;
                MessageBox.Show("Molecule added to favorites!");
            }
            else if (FavoritHinzugefuegt == 1)
            {
                MessageBox.Show("You already have added this molecule to your favorites!");
            }
        }

        // kopiere iupac-name ins clipboard!
        /* private void CopyIupacName(object sender, EventArgs e)
        {
            string ClipBoardContent = Clipboard.GetText();
            if (ClipBoardContent == SchneideErgebnis[0])
            {
                MessageBox.Show("IUPAC nomenclature already copied to clipboard!");
            }
            else if(ClipBoardContent != SchneideErgebnis[0])
            {
                Clipboard.SetText(SchneideErgebnis[0]);
                MessageBox.Show("IUPAC nomenclature copied to clipboard!");
            }
        } */





    }
}