using Excel;
using System.Data;
using System.Collections.Generic;
using System.IO;
using System.Configuration;

namespace SiteChecker
{
    class ExcelDataHandler
    {
        Dictionary<string, List<KeyValuePair<string, List<string>>>> mainData;
        Dictionary<string, List<KeyValuePair<string, List<string>>>> additionalData;
        Dictionary<string, List<KeyValuePair<string, List<string>>>> testCasesData;

        public ExcelDataHandler()
        {
            mainData = new Dictionary<string, List<KeyValuePair<string, List<string>>>>();
            additionalData = new Dictionary<string, List<KeyValuePair<string, List<string>>>>();
            testCasesData = new Dictionary<string, List<KeyValuePair<string, List<string>>>>();
        }
        
        DataSet GetDataFromFile()
        {
            var stream = File.Open(ConfigurationManager.AppSettings["FileName"], FileMode.Open, FileAccess.Read);
            var excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
            excelReader.IsFirstRowAsColumnNames = true;
            DataSet result = excelReader.AsDataSet();
            excelReader.Close();
            return result;
        }

        DataSet GetTestCasesDataFromFile()
        {
            var stream = File.Open(ConfigurationManager.AppSettings["TestCaseFileName"], FileMode.Open, FileAccess.Read);
            var excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
            excelReader.IsFirstRowAsColumnNames = true;
            DataSet result = excelReader.AsDataSet();
            excelReader.Close();
            return result;
        }

        List<string> GetCategoryPoints(List<string> tableColumn)
        {
            var result = new List<string>();
            for (int i = 0; i < tableColumn.Count; i++)
                if (i != 0)
                    result.Add(tableColumn[i]);
            return result;
        }

        #region Data Handling
        void AddInMainCategoriesList(DataTable table, List<KeyValuePair<string, List<string>>> mainCategoriesList)
        {
            for (int j = 0; j < table.Columns.Count; j++)
            {
                List<string> tableColumn = new List<string>();
                for (int index = 0; index < table.Rows.Count; index++)
                {
                    if (!table.Rows[index].ItemArray[j].ToString().Trim().Equals(""))
                        tableColumn.Add(table.Rows[index].ItemArray[j].ToString());
                    else
                        break;
                }
                if (tableColumn.Count != 0)
                    mainCategoriesList.Add(new KeyValuePair<string, List<string>>(tableColumn[0], GetCategoryPoints(tableColumn)));
            }
        }

        void AddInAdditionalCategoriesList(DataTable table, List<KeyValuePair<string, List<string>>> additionalCategoriesList)
        {
            int index = 0;
            for (int j = 0; j < table.Columns.Count; j++)
            {
                for (index = 0; index < table.Rows.Count; index++)
                    if (table.Rows[index].ItemArray[j].ToString().Trim().Equals(""))
                        break;
                List<string> tableColumn = new List<string>();
                for (index = index + 1; index < table.Rows.Count; index++)
                {
                    if (!table.Rows[index].ItemArray[j].ToString().Trim().Equals(""))
                        tableColumn.Add(table.Rows[index].ItemArray[j].ToString());
                    else
                        break;
                }
                if (tableColumn.Count != 0)
                    additionalCategoriesList.Add(new KeyValuePair<string, List<string>>(tableColumn[0], GetCategoryPoints(tableColumn)));
            }
        }

        void AddInTestCasesCategoriesList(DataTable table, List<KeyValuePair<string, List<string>>> testCasesCategoriesList)
        {
            int index = 0;
            for (int j = 0; j < table.Columns.Count; j++)
            {
                List<string> tableColumn = new List<string>();
                for (index = 0; index < table.Rows.Count; index++)
                {
                    if (!table.Rows[index].ItemArray[j].ToString().Trim().Equals(""))
                        tableColumn.Add(table.Rows[index].ItemArray[j].ToString());
                    else
                    {
                        tableColumn.Add("");
                        break;
                    }
                }
                testCasesCategoriesList.Add(new KeyValuePair<string, List<string>>(tableColumn[0], GetCategoryPoints(tableColumn)));
            }
        }

        void AddInCategoriesList(DataTable table, List<KeyValuePair<string, List<string>>> categoriesList)
        {
            int index = 0;
            for (int j = 0; j < table.Columns.Count; j++)
            {
                List<string> tableColumn = new List<string>();
                for (index = 0; index < table.Rows.Count; index++)
                {
                    if (!table.Rows[index].ItemArray[j].ToString().Trim().Equals(""))
                        tableColumn.Add(table.Rows[index].ItemArray[j].ToString());
                    else
                        break;
                }
                if (tableColumn.Count != 0)
                    categoriesList.Add(new KeyValuePair<string, List<string>>(tableColumn[0], GetCategoryPoints(tableColumn)));
                tableColumn = new List<string>();
                for (index = index + 1; index < table.Rows.Count; index++)
                    if (!table.Rows[index].ItemArray[j].ToString().Trim().Equals(""))
                        tableColumn.Add(table.Rows[index].ItemArray[j].ToString());
                if (tableColumn.Count != 0)
                    categoriesList.Add(new KeyValuePair<string, List<string>>(tableColumn[0], GetCategoryPoints(tableColumn)));
            }
        }

        void SetMainData(DataTable table)
        {
            List<KeyValuePair<string, List<string>>> mainCategoriesList = new List<KeyValuePair<string, List<string>>>();
            AddInMainCategoriesList(table, mainCategoriesList);
            if (table.TableName.Equals("professionalcasemanagementjourn"))
                mainData.Add($"{table.TableName}al", mainCategoriesList);
            else
                mainData.Add(table.TableName, mainCategoriesList);
        }

        void SetAdditionalData(DataTable table)
        {
            List<KeyValuePair<string, List<string>>> additionalCategoriesList = new List<KeyValuePair<string, List<string>>>();
            AddInAdditionalCategoriesList(table, additionalCategoriesList);
            if (table.TableName.Equals("professionalcasemanagementjourn"))
                additionalData.Add($"{table.TableName}al", additionalCategoriesList);
            else
                additionalData.Add(table.TableName, additionalCategoriesList);
        }

        void SetTestCasesData(DataTable table)
        {
            List<KeyValuePair<string, List<string>>> testCasesCategoriesList = new List<KeyValuePair<string, List<string>>>();
            AddInTestCasesCategoriesList(table, testCasesCategoriesList);
            if (table.TableName.Equals("professionalcasemanagementjourn"))
                testCasesData.Add($"{table.TableName}al", testCasesCategoriesList);
            else
                testCasesData.Add(table.TableName, testCasesCategoriesList);
        }

        public void SetData()
        {
            DataSet result = GetDataFromFile();
            foreach (DataTable table in result.Tables)
            {
                SetMainData(table);
                SetAdditionalData(table);
            }

            DataSet testCases = GetTestCasesDataFromFile();
            foreach (DataTable table in testCases.Tables)
                SetTestCasesData(table);
        }

        public Dictionary<string, List<KeyValuePair<string, List<string>>>> GetMainData() => mainData;

        public Dictionary<string, List<KeyValuePair<string, List<string>>>> GetAdditionalData() => additionalData;

        public List<KeyValuePair<string, List<string>>> GetMainCategories(string listName)
        {
            foreach (KeyValuePair<string, List<KeyValuePair<string, List<string>>>> categories in mainData)
                if (categories.Key.Equals(listName))
                    return categories.Value;
            return null;
        }

        public List<KeyValuePair<string, List<string>>> GetTestCasesCategories(string listName)
        {
            foreach (KeyValuePair<string, List<KeyValuePair<string, List<string>>>> categories in testCasesData)
                if (categories.Key.Equals(listName))
                    return categories.Value;
            return null;
        }
        #endregion
    }
}