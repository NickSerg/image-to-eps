using System;
using System.Windows.Input;
using ITE.Infrastructure.Interfaces;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;
using Microsoft.Win32;

namespace ITE.ConvertModule.ViewModels
{
    public class ConvertCommandsViewModel : NotificationObject
    {
        private readonly IConvertService convertService;
        private readonly DelegateCommand convertCommand;

        public ConvertCommandsViewModel(IConvertService convertService)
        {
            if (convertService == null)
                throw new ArgumentNullException("convertService");

            this.convertService = convertService;
            convertCommand = new DelegateCommand(Convert);
        }

        private void Convert()
        {
            var openFileDialog = new OpenFileDialog {RestoreDirectory = false, Multiselect = true};
            if (openFileDialog.ShowDialog() != true)
                return;

            if (convertService.ConvertCommand.CanExecute(openFileDialog.FileNames))
                convertService.ConvertCommand.Execute(openFileDialog.FileNames);
        }

        public ICommand ConvertCommand { get { return convertCommand; } }
    }
}
