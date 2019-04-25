using Microsoft.Extensions.CommandLineUtils;
using ngzip.Exceptions;
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
            //long fileSize = 1024 * 1024 * 1024;
            //fileSize *= 32;
            //var f = new FileInfo(Path.Combine(".", "data2.txt"));
            //FileStream stream;
            //if (!f.Exists)
            //    stream = f.Create();
            //else
            //    stream = f.OpenWrite();
            //using (stream)
            //using (var textStream = new StreamWriter(stream))
            //    while (stream.Length < fileSize)
            //        textStream.WriteLine("!!!TEST TEST TEST TEST TEST TEST TEST TEST TEST TEST!!!");
            //return;
            CommandLineApplication commandLineApplication =
                new CommandLineApplication(throwOnUnexpectedArg: false);

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
                          new CompressCommandHandler().Handle(new CompressCommand(inputName.Value, outputName.Value));
                      }
                      catch (NGZipExceptionBase e)
                      {
                          Console.WriteLine(e);
                      }
                      catch (Exception e)
                      {
                          Console.WriteLine("Что-то пошло не так....");
                          Console.WriteLine(e);
                      }

                      return 0;
                  });
              }, false);


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
                          new DecompressCommandHandler().Handle(new DecompressCommand(inputName.Value, outputName.Value));
                      }
                      catch (NGZipExceptionBase e)
                      {
                          Console.WriteLine(e);
                      }
                      catch (Exception e)
                      {
                          Console.WriteLine("Что-то пошло не так....");
                          Console.WriteLine(e);
                      }

                      return 0;
                  });
              }, false);

            commandLineApplication.HelpOption("-? | -h | --help");
            commandLineApplication.Execute(args);
        }
    }
}
