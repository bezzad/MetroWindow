using System;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MetroWindow
{
    public class MetroWindow : Window
    {
        public static readonly DependencyProperty TitleBarBrushProperty = DependencyProperty.Register(nameof(TitleBarBrush), typeof(Brush), typeof(MetroWindow), new PropertyMetadata(default(Brush)));
        public static readonly DependencyProperty TitleBarVisibilityProperty = DependencyProperty.Register(nameof(TitleBarVisibility), typeof(Visibility), typeof(MetroWindow), new PropertyMetadata(Visibility.Visible));
        public static readonly DependencyProperty TitleBarTextVisibilityProperty = DependencyProperty.Register(nameof(TitleBarTextVisibility), typeof(Visibility), typeof(MetroWindow), new PropertyMetadata(default(Visibility)));
        public static readonly DependencyProperty IgnoreTaskbarOnMaximizeProperty = DependencyProperty.Register(nameof(IgnoreTaskbarOnMaximize), typeof(bool), typeof(MetroWindow), new PropertyMetadata(default(bool)));
        public static readonly DependencyProperty IsFullscreenProperty = DependencyProperty.Register(nameof(IsFullscreen), typeof(bool), typeof(MetroWindow), new PropertyMetadata(default(bool)));
        public static readonly DependencyProperty DeactivatedTitleBarBrushProperty = DependencyProperty.Register(nameof(DeactivatedTitleBarBrush), typeof(Brush), typeof(MetroWindow), new PropertyMetadata(default(Brush)));

        private WindowState _lastWindowState;
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
        public bool IsFullscreen
        {
            get => (bool)GetValue(IsFullscreenProperty);
            set
            {
                SetValue(IsFullscreenProperty, value);
                OnIsFullscreenChanged();
            }
        }
        public Brush DeactivatedTitleBarBrush
        {
            get => (Brush)GetValue(DeactivatedTitleBarBrushProperty);
            set => SetValue(DeactivatedTitleBarBrushProperty, value);
        }
        public Brush TitleBarBrush
        {
            get => (Brush)GetValue(TitleBarBrushProperty);
            set => SetValue(TitleBarBrushProperty, value);
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

        protected override void OnDeactivated(EventArgs e)
        {
            base.OnDeactivated(e);
            TitleBarBrush = DeactivatedTitleBarBrush;
        }

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
            TitleBarBrush = BorderBrush;
        }

        protected override void OnStateChanged(EventArgs e)
        {
            base.OnStateChanged(e);

            if (WindowState == WindowState.Maximized)
            {
                RestoreButton.Visibility = Visibility.Visible;
                MaximizeButton.Visibility = Visibility.Collapsed;
                ShowInTaskbar = !IgnoreTaskbarOnMaximize;
                if (LayoutRoot != null)
                {
                    LayoutRoot.Margin = GetDefaultMarginForDpi();
                }
            }
            else
            {
                RestoreButton.Visibility = Visibility.Collapsed;
                MaximizeButton.Visibility = Visibility.Visible;
                ShowInTaskbar = true;
                if (LayoutRoot != null)
                {
                    LayoutRoot.Margin = new Thickness(0);
                }
            }
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

        private void OnIsFullscreenChanged()
        {
            Topmost = true;
            
            if (IsFullscreen)
            {
                _lastWindowState = WindowState;
                SystemCommands.RestoreWindow(this); // Note: restore maximized windows
                WindowStyle = WindowStyle.None;
                ShowTitleBar = false;
                SystemCommands.MaximizeWindow(this);
            }
            else
            {
                WindowStyle = WindowStyle.SingleBorderWindow;
                ShowTitleBar = true;
                WindowState = _lastWindowState;
            }

            Topmost = false; // disabled top most to allow show notification on this window
            Activate();
        }
    }
}
