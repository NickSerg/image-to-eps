using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Input;
using ITE.Infrastructure;
using ITE.Infrastructure.Helpers;
using ITE.Infrastructure.Interfaces;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Logging;
using Microsoft.Practices.Unity;

namespace ITE.ConvertModule.Services
{
    public class ConvertService: IConvertService
    {
        private readonly ObservableCollection<string> processedFiles;
        private readonly DelegateCommand<string[]> convertCommand; 

        public ConvertService()
        {
            processedFiles = new ObservableCollection<string>();
            convertCommand = new DelegateCommand<string[]>(Convert);
        }

        private void Convert(string[] files)
        {
            processedFiles.Clear();

            foreach (var imageFile in files.Where(FileHelper.IsImageFile).ToList())
            {
                ConvertImageFile(imageFile);
            }
        }

        private void ConvertImageFile(string imageFile)
        {
            var epsFile = Path.ChangeExtension(imageFile, ".eps");
            if (File.Exists(epsFile))
                File.Delete(epsFile);

            var process = new Process
            {
                StartInfo =
                {
                    FileName = "convert.exe",
                    Arguments = string.Format("\"{0}\" \"{1}\"", imageFile, epsFile),
                    UseShellExecute = false,
                    ErrorDialog = true
                }
            };

            try
            {
                process.Start();
                processedFiles.Add(imageFile);
            }
            catch (Exception exception)
            {
                Logger.Do(x=>x.Log(exception.Message, Category.Exception, Priority.High));
            }
        }

        public ObservableCollection<string> RetrieveProcessedFiles()
        {
            return processedFiles;
        }

        public ICommand ConvertCommand { get { return convertCommand; } }

        [Dependency]
        public ILoggerFacade Logger { get; set; }
    }
}
