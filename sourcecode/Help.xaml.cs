using BugSense;
using BugSense.Core.Model;
using Microsoft.Phone.Controls;

namespace XML
{
    public partial class Help : PhoneApplicationPage
    {
        public Help()
        {
            InitializeComponent();

            // Tracking
            BugSenseLogResult result = BugSenseHandler.Instance.LogEvent("Help-Page geöffnet!");
        }
    }
}