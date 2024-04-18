#region Namespaces
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;

#endregion

namespace ExampleRevitPluginWPF
{
    [Transaction(TransactionMode.Manual)]
    public class Command : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Application app = uiapp.Application;
            Document doc = uidoc.Document;
            Example(doc);
            return Result.Succeeded;
        }

        void Example(Document doc)
        {
            Categories cats = doc.Settings.Categories;
            ObservableCollection<ExampleELementInfo> listCats = new ObservableCollection<ExampleELementInfo>();
            
            foreach (Category c in cats)
            {
                if(c.AllowsBoundParameters)
                {
                    ExampleELementInfo categoryInfo = new ExampleELementInfo() { Name = c.Name };
                    listCats.Add(categoryInfo);
                    using (FilteredElementCollector fec = new FilteredElementCollector(doc).OfCategory((BuiltInCategory)c.Id.IntegerValue).WhereElementIsElementType())
                    {
                    foreach (Element element in fec)
                        {
                            ExampleELementInfo elementInfo = new ExampleELementInfo() { Name = element.Name };
                            categoryInfo.Children.Add(elementInfo);
                            foreach (Parameter p in (element as ElementType).GetOrderedParameters())
                            {
                                ExampleELementInfo parameterInfo = new ExampleELementInfo() { Name = p.Definition.Name };
                                elementInfo.Children.Add(parameterInfo);
                            }

                        }
                    }
                }
            }
            MainWindow mainWindow = new MainWindow(listCats);
            mainWindow.ShowDialog();

        }
    }

    public class ExampleELementInfo
    {
        public string Name { get; set; }
        public ObservableCollection<ExampleELementInfo> Children { get; set; } = new ObservableCollection<ExampleELementInfo>();
        public override string ToString()
        {
            return Name;
        }
    }

}
