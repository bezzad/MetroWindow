namespace MetroWindowSample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow.MetroWindow
    {
        public MainWindow()
        {
            DataContext = this;
            InitializeComponent();
        }

        private bool _fullscreen;
        public bool Fullscreen
        {
            get => _fullscreen;
            set
            {
                _fullscreen = value;
                IsFullscreen = value;
            }
        }
    }
}
