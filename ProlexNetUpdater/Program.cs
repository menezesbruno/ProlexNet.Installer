using ProlexNetUpdater.Library.Common;
using ProlexNetUpdater.Library.Script;
using ProlexNetUpdater.Library.Update;
using ProlexNetUpdater.Models;
using System;
using System.Collections.Generic;

namespace ProlexNetUpdater
{
    internal class Program
    {
        public static int Main(string[] args)
        {
            var result = new List<Result>();
            var error = new List<object>();

            //Vasculha registro pela pasta do ProlexNet             
            Registry.LoadPath();

            //Download da lista 
            DownloadParameters.LoadApplicationList();

            foreach (var item in args)
            {
                // Método de atualização da aplicação
                if (item == "/update")
                {
                    try
                    {
                        Update.Run();
                        result.Add(Result.Success);
                    }
                    catch (Exception ex)
                    {
                        error.Add(ex.Message);
                        result.Add(Result.Failed);
                    }
                }

                // Metódo de atualização de banco
                if (item == "/script")
                {
                    try
                    {
                        Script.Run();
                        result.Add(Result.Success);
                    }
                    catch (Exception ex)
                    {
                        error.Add(ex.Message);
                        result.Add(Result.Failed);
                    }
                }
            }

            var updateResult = 0;
            if (result.Contains(Result.Failed))
            {
                updateResult = 1;
                Console.Write(error);
            }

            Console.WriteLine(updateResult);
            return updateResult;
        }
    }
}