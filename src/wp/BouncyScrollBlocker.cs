using System.Windows;
using System.Reflection;
using WPCordovaClassLib;
using Microsoft.Phone.Controls;
using System.Diagnostics;
using WPCordovaClassLib.Cordova.Commands;
using System.Windows.Threading;
using System;

namespace Cordova.Extension.Commands
{
    public class BouncyScrollBlocker : BaseCommand
    {
        public BouncyScrollBlocker()
        {
            Debug.WriteLine("BouncyScrollBlocker - constructor");
        }

        public void block(string options)
        {
            UIThread.Invoke(DisableBouncyScroll);
        }

        private void DisableBouncyScroll()
        {
            var frame = Application.Current.RootVisual as PhoneApplicationFrame;

            var mainWindow = frame.Content as PhoneApplicationPage;
            if (mainWindow == null)
            {
                return;
            }

            var cordovaView = GetNonPublic<CordovaView>(mainWindow, "CordovaView");
            var bmHelper = GetNonPublic<BrowserMouseHelper>(cordovaView, "bmHelper");
            
            if (bmHelper != null)
            {
                bmHelper.ScrollDisabled = true;
            }
        }

        private static T GetNonPublic<T>(object obj, string method) where T: class
        {
            if (obj != null)
            {
                var type = obj.GetType();

                var field = type.GetField(method, BindingFlags.Instance | BindingFlags.NonPublic);
                if (field == null)
                {
                    Debug.WriteLine("Type {0} doesn't have a non-public, instance method {1}", type.FullName, method);
                }
                else
                {
                    return field.GetValue(obj) as T;
                }
            }

            return default(T);
        }
    }

    static class UIThread
    {
        private static readonly Dispatcher Dispatcher;

        static UIThread()
        {
            // Store a reference to the current Dispatcher once per application
            Dispatcher = Deployment.Current.Dispatcher;
        }

        /// <summary>
        ///   Invokes the given action on the UI thread - if the current thread is the UI thread this will just invoke the action directly on
        ///   the current thread so it can be safely called without the calling method being aware of which thread it is on.
        /// </summary>
        public static void Invoke(Action action)
        {
            if (Dispatcher.CheckAccess())
                action.Invoke();
            else
                Dispatcher.BeginInvoke(action);
        }
    }
}