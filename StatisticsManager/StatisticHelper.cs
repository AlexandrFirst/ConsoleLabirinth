﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatisticsManager
{
    public static class StatisticHelper
    {
        public static void WriteResult()
        {
            try
            {
                using (StreamWriter sw = new StreamWriter("names.txt",true))
                {
                    if(ResultStatConfig.Type== "SinglePlayer")
                        sw.WriteLine("Type: {0}, Date: {1}, Name: {2}, steps: {3}, time: {4}, Result: {5}",ResultStatConfig.Type,ResultStatConfig.Date,
                            ResultStatConfig.Name,ResultStatConfig.Steps,ResultStatConfig.Time,ResultStatConfig.Result);
                    else
                        sw.WriteLine("Type: {0}, Date: {1}, Name: {2}, Opponent's name: {3}, steps: {4}, time: {5}, Result: {6}", ResultStatConfig.Type, ResultStatConfig.Date,
                           ResultStatConfig.Name,ResultStatConfig.Opponet_name ,ResultStatConfig.Steps, ResultStatConfig.Time, ResultStatConfig.Result);
                }
            }
            catch
            {
                Console.WriteLine("Sorry, can't write to file");
            }
        }

        public static List<string> ReadFromFile()
        {
            try
            {
                string line = "";
                List<string> res = new List<string>() ;
                using (StreamReader sr = new StreamReader("names.txt"))
                {
                    while ((line = sr.ReadLine()) != null)
                    {
                        res.Add(line);
                    }
                }
                return res;
            }
            catch
            {
                return null;
            }
        }
    }
}
