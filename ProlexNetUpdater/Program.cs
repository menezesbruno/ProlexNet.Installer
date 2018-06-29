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

            foreach (var item in args)
            {
                // Métodos de atualização da aplicação
                if (item == "/update")
                {
                    try
                    {
                        Update.Run();
                        result.Add(Result.Finished);
                    }
                    catch
                    {
                        result.Add(Result.Failed);
                    }
                }

                // Metódos de atualização de banco
                if (item == "/script")
                {
                    try
                    {
                        Script.Run();
                        result.Add(Result.Finished);
                    }
                    catch
                    {
                        result.Add(Result.Failed);
                    }
                }
            }
            var updateResult = 0;
            if (result.Contains(Result.Failed))
                updateResult = 2;

            Console.WriteLine(updateResult);
            return updateResult;
        }
    }
}