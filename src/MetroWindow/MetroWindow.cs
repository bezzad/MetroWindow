using System;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace MetroWindow
{
    public class MetroWindow : Window
    {
        public static readonly DependencyProperty TitleBarVisibilityProperty = DependencyProperty.Register(nameof(TitleBarVisibility), typeof(Visibility), typeof(MetroWindow), new PropertyMetadata(Visibility.Visible));
        public static readonly DependencyProperty TitleBarTextVisibilityProperty = DependencyProperty.Register(nameof(TitleBarTextVisibility), typeof(Visibility), typeof(MetroWindow), new PropertyMetadata(default(Visibility)));
        public static readonly DependencyProperty IgnoreTaskbarOnMaximizeProperty = DependencyProperty.Register(nameof(IgnoreTaskbarOnMaximize), typeof(bool), typeof(MetroWindow), new PropertyMetadata(default(bool)));

        public double VirtualScreenWidth => SystemParameters.VirtualScreenWidth;
        public double VirtualScreenHeight => SystemParameters.VirtualScreenHeight;
        public Border LayoutRootBorder { get; private set; }
        public Grid LayoutRoot { get; private set; }
        public Grid HeaderBar { get; private set; }
        public Button MinimizeButton { get; private set; }
        public Button MaximizeButton { get; private set; }
        public Button RestoreButton { get; private set; }
        public Button CloseButton { get; private set; }
        public Visibility TitleBarVisibility
        {
            get => (Visibility)GetValue(TitleBarVisibilityProperty);
            set => SetValue(TitleBarVisibilityProperty, value);
        }
        public bool ShowTitleBar
        {
            get => TitleBarVisibility == Visibility.Visible;
            set => TitleBarVisibility = value ? Visibility.Visible : Visibility.Collapsed;
        }
        public Visibility TitleBarTextVisibility
        {
            get => (Visibility)GetValue(TitleBarTextVisibilityProperty);
            set => SetValue(TitleBarTextVisibilityProperty, value);
        }
        public bool IgnoreTaskbarOnMaximize
        {
            get => (bool)GetValue(IgnoreTaskbarOnMaximizeProperty);
            set
            {
                SetValue(IgnoreTaskbarOnMaximizeProperty, value);
                OnStateChanged(EventArgs.Empty);
            }
        }


        static MetroWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MetroWindow), new FrameworkPropertyMetadata(typeof(MetroWindow)));
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
                if (IgnoreTaskbarOnMaximize)
                {
                    ShowInTaskbar = false;
                }
            }
            else
            {
                LayoutRootBorder.BorderThickness = new Thickness(0);
                RestoreButton.Visibility = Visibility.Collapsed;
                MaximizeButton.Visibility = Visibility.Visible;
                ShowInTaskbar = true;
            }
        }
    }
}
