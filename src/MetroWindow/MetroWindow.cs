using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MetroWindow
{
    public class MetroWindow : Window
    {
        public Border LayoutRootBorder { get; private set; }
        public Grid LayoutRoot { get; private set; }
        public Grid HeaderBar { get; private set; }
        public Button MinimizeButton { get; private set; }
        public Button MaximizeButton { get; private set; }
        public Button RestoreButton { get; private set; }
        public Button CloseButton { get; private set; }


        static MetroWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MetroWindow), new FrameworkPropertyMetadata(typeof(MetroWindow)));
        }

        public MetroWindow()
        {
        }

        public override void OnApplyTemplate()
        {
            LayoutRootBorder = GetRequiredTemplateChild<Border>("LayoutRootBorder");
            LayoutRoot = GetRequiredTemplateChild<Grid>("LayoutRoot");
            MinimizeButton = GetRequiredTemplateChild<Button>("MinimizeButton");
            MaximizeButton = GetRequiredTemplateChild<Button>("MaximizeButton");
            RestoreButton = GetRequiredTemplateChild<Button>("RestoreButton");
            CloseButton = GetRequiredTemplateChild<Button>("CloseButton");
            HeaderBar = GetRequiredTemplateChild<Grid>("PART_HeaderBar");

            if (LayoutRoot != null && WindowState == WindowState.Maximized)
            {
                //LayoutRoot.Margin = GetDefaultMarginForDpi();
            }

            if (CloseButton != null)
            {
                //CloseButton.Click += CloseButton_Click;
            }

            if (MinimizeButton != null)
            {
                //MinimizeButton.Click += MinimizeButton_Click;
            }

            if (RestoreButton != null)
            {
               // RestoreButton.Click += RestoreButton_Click;
            }

            if (MaximizeButton != null)
            {
               // MaximizeButton.Click += MaximizeButton_Click;
            }

            if (HeaderBar != null)
            {
                //HeaderBar.AddHandler(MouseLeftButtonDownEvent, new MouseButtonEventHandler(OnHeaderBarMouseLeftButtonDown));
            }

            base.OnApplyTemplate();
        }

        private T GetRequiredTemplateChild<T>(string childName) where T : DependencyObject
        {
            return (T)GetTemplateChild(childName);
        }

        protected override void OnStateChanged(EventArgs e)
        {
            base.OnStateChanged(e);

            if (WindowState == WindowState.Maximized)
            {
                LayoutRootBorder.BorderThickness = new Thickness(8);
                RestoreButton.Visibility = Visibility.Visible;
                MaximizeButton.Visibility = Visibility.Collapsed;
            }
            else
            {
                LayoutRootBorder.BorderThickness = new Thickness(0);
                RestoreButton.Visibility = Visibility.Collapsed;
                MaximizeButton.Visibility = Visibility.Visible;
            }
        }
    }
}
