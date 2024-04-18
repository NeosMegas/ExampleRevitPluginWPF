#region Namespaces
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Media;
using System.Windows.Media.Imaging;

#endregion

namespace ExampleRevitPluginWPF
{
    internal class App : IExternalApplication
    {
        static AddInId addInId = new AddInId(new Guid("f8fdb023-34ec-4cf3-8693-66f1374033df"));
        string helpURL = "https://www.google.com";

        public RibbonPanel AutomationPanel(UIControlledApplication application, string tabName = "", RibbonPanel ribbonPanel = null)
        {
            string assemblyPath = Assembly.GetExecutingAssembly().Location;
            if (ribbonPanel == null)
            {
                if (string.IsNullOrEmpty(tabName))
                    ribbonPanel = application.CreateRibbonPanel("ExampleRevitPluginWPF");
                else
                    ribbonPanel = application.CreateRibbonPanel(tabName, "ExampleRevitPluginWPF");
            }
            AddButton(ribbonPanel, "WPF", assemblyPath, nameof(ExampleRevitPluginWPF) + "." + nameof(Command), "WPF");
            return ribbonPanel;
        }

        private void AddButton(RibbonPanel ribbonPanel, string buttonName, string path, string linkToCommand, string toolTip)
        {
            PushButtonData buttonData = new PushButtonData(
               buttonName,
               buttonName,
               path,
               linkToCommand);
            ContextualHelp contextualHelp = new ContextualHelp(ContextualHelpType.Url, helpURL);
            buttonData.SetContextualHelp(contextualHelp);
            PushButton Button = ribbonPanel.AddItem(buttonData) as PushButton;
            Button.ToolTip = toolTip;
            ImageSource imageSource = new BitmapImage(new Uri($@"pack://application:,,,/{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name};component/Icon32.png", UriKind.RelativeOrAbsolute));
            if (imageSource != null)
                Button.LargeImage = imageSource;
        }

        public Result OnStartup(UIControlledApplication a)
        {
            RibbonPanel ribbonPanel = AutomationPanel(a);
            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication a)
        {
            return Result.Succeeded;
        }
    }
}
