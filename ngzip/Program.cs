using Microsoft.Extensions.CommandLineUtils;
using ngzip.Exceptions;
using ngzip.Infrastructure;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ngzip
{
    class Program
    {
        static void Main(string[] args)
        {
            var cancelToken = GetToken();

            CommandLineApplication commandLineApplication =
                new CommandLineApplication(throwOnUnexpectedArg: false);
            commandLineApplication.HelpOption("-? | -h | --help");

            RegisterCompressCommand(cancelToken, commandLineApplication);
            RegisterDecompressCommand(cancelToken, commandLineApplication);

            commandLineApplication.Execute(args);

            Console.ReadKey();
        }

        private static void RegisterDecompressCommand(ThreadCancelationToken cancelToken, CommandLineApplication commandLineApplication)
        {
            commandLineApplication.Command("decompress",
                          (target) =>
                          {
                              target.HelpOption("-? | -h | --help");
                              CommandArgument inputName = target.Argument(
                                  "archiveName",
                                  "Введите пути к архиву необходимо обработать.",
                              multipleValues: false);
                              CommandArgument outputName = target.Argument(
                                  "filePath",
                                  "Путь выходного архива.",
                              multipleValues: false);

                              target.OnExecute(() =>
                              {
                                  try
                                  {
                                      new DecompressCommandHandler().Handle(new DecompressCommand(inputName.Value, outputName.Value), cancelToken);
                                  }
                                  catch (Exception e)
                                  {
                                      Console.WriteLine("Что-то пошло не так....");
                                      Console.WriteLine(e);
                                  }

                                  return 0;
                              });
                          }, false);
        }

        private static void RegisterCompressCommand(ThreadCancelationToken cancelToken, CommandLineApplication commandLineApplication)
        {
            commandLineApplication.Command("compress",
                          (target) =>
                          {
                              target.HelpOption("-? | -h | --help");
                              CommandArgument inputName = target.Argument(
                                  "filePath",
                                  "Введите пути к файлу который необходимо обработать.",
                              multipleValues: false);
                              CommandArgument outputName = target.Argument(
                                  "archiveName",
                                  "Путь выходного архива.",
                              multipleValues: false);

                              target.OnExecute(() =>
                              {
                                  try
                                  {
                                      new CompressCommandHandler().Handle(new CompressCommand(inputName.Value, outputName.Value), cancelToken);
                                  }
                                  catch (Exception e)
                                  {
                                      Console.WriteLine("Что-то пошло не так....");
                                      Console.WriteLine(e);
                                  }

                                  return 0;
                              });
                          }, false);
        }

        private static ThreadCancelationToken GetToken()
        {
            var cancelToken = new ThreadCancelationToken();
            cancelToken.OnCancel += (e) =>
            {
                Console.WriteLine("Что-то пошло не так...");
                Console.WriteLine(e);
            };
            return cancelToken;
        }
    }
}
