using System;
using System.Windows;
using Microsoft.Practices.Prism.Commands;

namespace ITE.Infrastructure.Behaviors
{
    public class FrameworkElementDropCommandBehavior : CommandBehaviorBase<FrameworkElement>
    {
        public FrameworkElementDropCommandBehavior(FrameworkElement targetObject)
            : base(targetObject)
        {
            if (targetObject == null)
                throw new ArgumentNullException();

            targetObject.AllowDrop = true;
            targetObject.DragEnter += TargetObject_DragEnter;
            targetObject.DragOver += TargetObject_DragOver;
            targetObject.DragLeave += TargetObject_DragLeave;
            targetObject.Drop += TargetObject_Drop;
        }

        private void TargetObject_Drop(object sender, DragEventArgs e)
        {
            if (Command != null)
            {
                object data = null;
                foreach (var dataFormat in AllowableDataFormats.Return(x => x, new string[] { }))
                {
                    data = e.Data.GetData(dataFormat);
                    if (data != null)
                        break;
                }

                if (data == null)
                    return;

                object senderDataContext = null;
                var frameworkElement = sender as FrameworkElement;
                if (frameworkElement != null)
                    senderDataContext = frameworkElement.DataContext;

                CommandParameter = new[] { senderDataContext, data };

                if (Command.CanExecute(CommandParameter))
                {
                    ExecuteCommand();
                }
            }

            e.Handled = true;
        }

        private static void TargetObject_DragLeave(object sender, DragEventArgs e)
        {
            e.Handled = true;
        }

        private void TargetObject_DragOver(object sender, DragEventArgs e)
        {
            e.Effects = IsAllowDrop(e) ? DragDropEffects.Move : DragDropEffects.None;

            e.Handled = true;
        }

        private void TargetObject_DragEnter(object sender, DragEventArgs e)
        {
            e.Handled = true;
        }

        private bool IsAllowDrop(DragEventArgs e)
        {
            object parameter = null;
            foreach (var dataFormat in AllowableDataFormats.Return(x => x, new string[] { }))
            {
                parameter = e.Data.GetData(dataFormat);
                if (parameter != null)
                    break;
            }

            return parameter != null;
        }

        public string[] AllowableDataFormats { get; set; }
    }
}
