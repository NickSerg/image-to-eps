using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ITE.Infrastructure.Behaviors;

namespace ITE.Infrastructure
{
    public static class DragDrop
    {
        public static readonly DependencyProperty DropCommandProperty =
            DependencyProperty.RegisterAttached("DropCommand",
                                                typeof(ICommand),
                                                typeof(DragDrop),
                                                new PropertyMetadata(OnSetDropCommandCallback));

        public static readonly DependencyProperty AllowableDataFormatsProperty =
            DependencyProperty.RegisterAttached("AllowableDataFormats",
                                                typeof(string),
                                                typeof(DragDrop),
                                                new PropertyMetadata(OnSetAllowableDataFormatsCallback));

        public static readonly DependencyProperty DragCommandProperty =
            DependencyProperty.RegisterAttached("DragCommand",
                                                typeof(ICommand),
                                                typeof(DragDrop),
                                                new PropertyMetadata(OnSetDragCommandCallback));

        public static readonly DependencyProperty AllowDragPropery =
            DependencyProperty.RegisterAttached("AllowDrag",
                                                typeof(bool),
                                                typeof(DragDrop),
                                                new PropertyMetadata(OnSetAllowDragCallback));

        private static readonly DependencyProperty ItemsControlDropCommandBehaviorProperty =
            DependencyProperty.RegisterAttached("ItemsControlDropCommandBehavior",
                                                typeof(ItemsControlDropCommandBehavior),
                                                typeof(DragDrop),
                                                null);

        private static readonly DependencyProperty FrameworkElementDropCommandBehaviorProperty =
            DependencyProperty.RegisterAttached("FrameworkElementDropCommandBehavior",
                                                typeof(FrameworkElementDropCommandBehavior),
                                                typeof(DragDrop),
                                                null);

        private static readonly DependencyProperty FrameworkElementDragCommandBehaviorPropery =
            DependencyProperty.RegisterAttached("FrameworkElementDragCommandBehavior",
                                                typeof(FrameworkElementDragCommandBehavior),
                                                typeof(DragDrop),
                                                null);

        private static readonly Dictionary<object, ICommand> mapDragCommandDataCotext = new Dictionary<object, ICommand>();

        public static void SetDropCommand(FrameworkElement frameworkElement, ICommand command)
        {
            if (frameworkElement == null)
                throw new ArgumentNullException("frameworkElement");

            frameworkElement.SetValue(DropCommandProperty, command);
        }

        public static ICommand GetDropCommand(FrameworkElement frameworkElement)
        {
            if (frameworkElement == null)
                throw new ArgumentNullException("frameworkElement");

            return frameworkElement.GetValue(DropCommandProperty) as ICommand;
        }

        public static void SetAllowableDataFormats(FrameworkElement frameworkElement, string allowableDataFormats)
        {
            if (frameworkElement == null)
                throw new ArgumentNullException("frameworkElement");

            frameworkElement.SetValue(AllowableDataFormatsProperty, allowableDataFormats);
        }

        public static string GetAllowableDataFormats(FrameworkElement frameworkElement)
        {
            if (frameworkElement == null)
                throw new ArgumentNullException("frameworkElement");

            return frameworkElement.GetValue(AllowableDataFormatsProperty) as string;
        }

        public static void SetDragCommand(FrameworkElement frameworkElement, ICommand command)
        {
            if (frameworkElement == null)
                throw new ArgumentNullException("frameworkElement");

            frameworkElement.SetValue(DragCommandProperty, command);
        }

        public static ICommand GetDragCommand(FrameworkElement frameworkElement)
        {
            if (frameworkElement == null)
                throw new ArgumentNullException("frameworkElement");

            return frameworkElement.GetValue(DragCommandProperty) as ICommand;
        }

        public static void SetAllowDrag(FrameworkElement frameworkElement, bool value)
        {
            if (frameworkElement == null)
                throw new ArgumentNullException("frameworkElement");

            frameworkElement.SetValue(AllowDragPropery, value);
        }

        public static bool GetAllowDrag(FrameworkElement frameworkElement)
        {
            if (frameworkElement == null)
                throw new ArgumentNullException("frameworkElement");

            return frameworkElement.GetValue(AllowDragPropery).Return(x => (bool)x, false);
        }

        public static ICommand FindDragCommand(object dataContext)
        {
            return mapDragCommandDataCotext.ContainsKey(dataContext)
                       ? mapDragCommandDataCotext[dataContext]
                       : null;
        }

        private static void OnSetDropCommandCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            var itemsControl = dependencyObject as ItemsControl;
            if (itemsControl != null)
            {
                var behavior = GetOrCreateItemsControlDropBehavior(itemsControl);
                behavior.Command = e.NewValue as ICommand;
            }
            else
            {
                var frameworkElement = dependencyObject as FrameworkElement;
                if (frameworkElement != null)
                {
                    var behavior = GetOrCreateFrameworkElementDropBehavior(frameworkElement);
                    behavior.Command = e.NewValue as ICommand;
                }
            }
        }

        private static void OnSetAllowableDataFormatsCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            var itemsControl = dependencyObject as ItemsControl;
            var allowableDataFormats = (e.NewValue as string).Return(x => x.Split(','), new string[0]);
            if (itemsControl != null)
            {
                var behavior = GetOrCreateItemsControlDropBehavior(itemsControl);
                behavior.AllowableDataFormats = allowableDataFormats;
            }
            else
            {
                var frameworkElement = dependencyObject as FrameworkElement;
                if (frameworkElement != null)
                {
                    var behavior = GetOrCreateFrameworkElementDropBehavior(frameworkElement);
                    behavior.AllowableDataFormats = allowableDataFormats;
                }
            }
        }

        private static void OnSetDragCommandCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            var frameworkElement = dependencyObject as FrameworkElement;
            if (frameworkElement != null)
            {
                var behavior = GetOrCreateFrameworkElementDragBehavior(frameworkElement);
                behavior.Command = e.NewValue as ICommand;

                mapDragCommandDataCotext[frameworkElement.DataContext] = behavior.Command;
            }
        }

        private static void OnSetAllowDragCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            var frameworkElement = dependencyObject as FrameworkElement;
            if (frameworkElement != null)
            {
                var behavior = GetOrCreateFrameworkElementDragBehavior(frameworkElement);
            }
        }

        private static FrameworkElementDropCommandBehavior GetOrCreateFrameworkElementDropBehavior(FrameworkElement frameworkElement)
        {
            var behavior = frameworkElement.GetValue(FrameworkElementDropCommandBehaviorProperty) as FrameworkElementDropCommandBehavior;
            if (behavior == null)
            {
                behavior = new FrameworkElementDropCommandBehavior(frameworkElement);
                frameworkElement.SetValue(FrameworkElementDropCommandBehaviorProperty, behavior);
            }

            return behavior;
        }

        private static ItemsControlDropCommandBehavior GetOrCreateItemsControlDropBehavior(ItemsControl itemsControl)
        {
            var behavior = itemsControl.GetValue(ItemsControlDropCommandBehaviorProperty) as ItemsControlDropCommandBehavior;
            if (behavior == null)
            {
                behavior = new ItemsControlDropCommandBehavior(itemsControl);
                itemsControl.SetValue(ItemsControlDropCommandBehaviorProperty, behavior);
            }

            return behavior;
        }

        private static FrameworkElementDragCommandBehavior GetOrCreateFrameworkElementDragBehavior(FrameworkElement frameworkElement)
        {
            var behavior = frameworkElement.GetValue(FrameworkElementDragCommandBehaviorPropery) as FrameworkElementDragCommandBehavior;
            if (behavior == null)
            {
                behavior = new FrameworkElementDragCommandBehavior(frameworkElement);
                frameworkElement.SetValue(FrameworkElementDragCommandBehaviorPropery, behavior);
            }

            return behavior;
        }
    }
}
