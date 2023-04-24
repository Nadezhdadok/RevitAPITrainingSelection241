using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitAPITrainingSelection241
{
    [Transaction(TransactionMode.Manual)]
    public class Main : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            FilteredElementCollector collector = new FilteredElementCollector(doc);
            FilteredElementCollector collector2 = new FilteredElementCollector(doc);
            ICollection<Element> levels = collector
                .OfClass(typeof(Level))
                .ToElements();
            ICollection<Element> levels2 = collector2
                .OfClass(typeof(Level))
                .ToElements();
            var query = from element in collector where element.Name == "Level 1" select element;
            var query2 = from element in collector2 where element.Name == "Level 2" select element;

            List<Element> level1 = query.ToList<Element>();
            List<Element> level2 = query2.ToList<Element>();
            ElementId levelId = level1[0].Id;
            ElementId levelId2 = level2[0].Id;

            ElementLevelFilter levelFilter = new ElementLevelFilter(levelId);
            ElementLevelFilter levelFilter2 = new ElementLevelFilter(levelId2);
            collector = new FilteredElementCollector(doc);
            collector2 = new FilteredElementCollector(doc);
            ICollection<Element> allWallsOnLevel1 = collector
                .OfClass(typeof(Wall))
                .WherePasses(levelFilter)
                .ToElements();
            ICollection<Element> allWallsOnLevel2 = collector2
                .OfClass(typeof(Wall))
                .WherePasses(levelFilter2)
                .ToElements();


            TaskDialog.Show("1ый и 2ой", $"Количество стен на 1ом этаже {allWallsOnLevel1.Count.ToString()}\n" + $"Количество стен на 2ом этаже {allWallsOnLevel2.Count.ToString()}");
            return Result.Succeeded;
        }
    }
}
