﻿using ProlexNetUpdater.Library.Common;
using ProlexNetUpdater.Library.Script;
using ProlexNetUpdater.Library.Update;
using ProlexNetUpdater.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace ProlexNetUpdater
{
    internal class Program
    {
        public static int Main(string[] args)
        {
            var result = new List<Result>();
            var error = new List<object>();

            try
            {
                //Vasculha registro pela pasta do ProlexNet             
                Registry.LoadPath();

                //Download da lista 
                DownloadParameters.LoadApplicationList();

                result.Add(Result.Success);
            }
            catch (Exception ex)
            {
                error.Add(ex.Message);
                result.Add(Result.Failed);
            }

            if (!result.Contains(Result.Failed))
            {
                foreach (var item in args)
                {
                    //Método de atualização da aplicação
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

                    //Metódo de atualização de banco
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
            }

            var updateResult = 0;
            if (result.Contains(Result.Failed))
                updateResult = 1;

            //Gera a página de resultado da atualização
            var rootPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var htmlResult = Path.Combine(rootPath, "result.html");
            UpdateResult.Build(htmlResult, updateResult);
            System.Diagnostics.Process.Start(htmlResult);

            return updateResult;
        }
    }
}