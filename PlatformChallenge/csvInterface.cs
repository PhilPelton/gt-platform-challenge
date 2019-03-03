using System;
using System.Collections.Generic;
using System.IO;

namespace PlatformChallenge
{
    class CsvInterface
    {
        public List<CsvLine> csvData;

        public void LoadCVSData()
        {
            csvData = new List<CsvLine>();
            try
            {
                using (StreamReader reader = new StreamReader(Config.csvFileLocation))
                {
                    string csvLine;

                    while ((csvLine = reader.ReadLine()) != null)
                    {
                        if (csvLine.Trim(' ') != "" && csvLine.Substring(0, 2) != "Id") //ignore last line and column headers.
                        {
                            csvLine = csvLine.Replace("Minute,Hour", "Minute--Hour").Replace("\"", ""); //remove quotes and put in a placeholder for tricky comma.
                            string[] csvLineArray = csvLine.Split(',');
                            CsvLine csvDataItem = new CsvLine();
                            csvDataItem.Id = csvLineArray[0];
                            csvDataItem.UtcDate = csvLineArray[1];
                            csvDataItem.Topic = csvLineArray[2];
                            csvDataItem.Kind = csvLineArray[3].Replace("Minute--Hour", "Minute,Hour"); //put comma back
                            //csvDataItem.UtcProcessedDate = csvLineArray[4]; //this isn't used in this challenge.

                            csvData.Add(csvDataItem);
                        }

                    }
                }
            }
            catch (Exception e)
            {
                Logging.HandleError("loadCVSData", e);
            }
        }

    }
}
