using System.Windows;
using System.Reflection;
using WPCordovaClassLib;

namespace Fi.Avaus.Cordova
{
    class BouncyScrollBlocker
    {
        private const string BMHelper_filed = "bmHelper";

        public BouncyScrollBlocker()
        {
            var frame = Application.Current.RootVisual as Microsoft.Phone.Controls.PhoneApplicationFrame;
            var mainWindow = frame.Content as MainPage;
            if (mainWindow == null)
            {
                return;
            }

            var bmHelper = typeof(CordovaView).GetField(BMHelper_filed, BindingFlags.Instance | BindingFlags.NonPublic).GetValue(mainWindow.CordovaView) as BrowserMouseHelper;
            if (bmHelper == null)
            {
                return;
            }

            bmHelper.ScrollDisabled = true;
        }
    }
}