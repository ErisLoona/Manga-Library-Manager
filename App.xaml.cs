namespace Manga_Library_Manager
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
        }

        protected override Window CreateWindow(IActivationState activationState)
        {
            Window window = base.CreateWindow(activationState);
            window.Title = "Manga Library Manager";
            window.Width = 1140;
            window.Height = 699;
            return window;
        }
    }
}
