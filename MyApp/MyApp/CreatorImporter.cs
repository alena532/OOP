using MyApp.Figures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MyApp
{
    public class CreatorImporter
    {
        private List<FigureCreator> importedCreators;
        public List<FigureCreator> ImportedCreators { get { return importedCreators; } }
        public CreatorImporter()
        {
            importedCreators = new();
        }
        public ImportResult ImportFromDll(string fileName)
        {
            int prevImport = importedCreators.Count;
            try
            {
                Assembly assembly = Assembly.LoadFrom(fileName);
                Type[] types = assembly.GetTypes();
                foreach (Type type in types)
                {
                    if (type.IsSubclassOf(typeof(FigureCreator)))
                    {
                        var importCreator = assembly.CreateInstance(type.FullName);
                        if (importCreator != null)
                        {
                            importedCreators?.Add((FigureCreator)importCreator);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return ImportResult.IMPORT_ERROR;
            }
            if (importedCreators.Count == prevImport)
            {
                return ImportResult.IMPORT_ERROR;
            }
            return ImportResult.OK;
        }
        public void ClearImportData()
        {
            importedCreators.Clear();
        }
    }
    public enum ImportResult
    {
        OK = 0,
        IMPORT_ERROR = 1,
    }
}

