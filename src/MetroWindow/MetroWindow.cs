using System;
using System.Reflection;
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
                LayoutRoot.Margin = GetDefaultMarginForDpi();
            }

            if (CloseButton != null)
            {
                CloseButton.Click += delegate { Close(); };
            }

            if (MinimizeButton != null)
            {
                MinimizeButton.Click += delegate { WindowState = WindowState.Minimized; };
            }

            if (RestoreButton != null)
            {
                RestoreButton.Click += delegate { WindowState = WindowState.Normal; };
            }

            if (MaximizeButton != null)
            {
                MaximizeButton.Click += delegate { WindowState = WindowState.Maximized; };
            }

            base.OnApplyTemplate();
        }

        protected virtual Thickness GetDefaultMarginForDpi()
        {
            int currentDpi = GetCurrentDpi();
            Thickness thickness = new Thickness(8, 8, 8, 8);
            if (currentDpi == 120)
            {
                thickness = new Thickness(7, 7, 4, 5);
            }
            else if (currentDpi == 144)
            {
                thickness = new Thickness(7, 7, 3, 1);
            }
            else if (currentDpi == 168)
            {
                thickness = new Thickness(6, 6, 2, 0);
            }
            else if (currentDpi == 192)
            {
                thickness = new Thickness(6, 6, 0, 0);
            }
            else if (currentDpi == 240)
            {
                thickness = new Thickness(6, 6, 0, 0);
            }
            return thickness;
        }

        private static int GetCurrentDpi()
        {
            return (int)typeof(SystemParameters).GetProperty("Dpi", BindingFlags.Static | BindingFlags.NonPublic)
                .GetValue(null, null);
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
