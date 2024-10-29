namespace NavigationWrapperExample
{
    public partial class ContentPageWrapper : ContentPage
    {

        public static readonly BindableProperty AnimatedContentProperty = BindableProperty.Create(nameof(AnimatedContent), typeof(View), typeof(ContentPageWrapper));
        public View AnimatedContent
        {
            get => (View)GetValue(AnimatedContentProperty);
            set => SetValue(AnimatedContentProperty, value);
        }

        public static readonly BindableProperty StaticContentProperty = BindableProperty.Create(nameof(StaticContent), typeof(View), typeof(ContentPageWrapper));
        public View StaticContent
        {
            get => (View)GetValue(StaticContentProperty);
            set => SetValue(StaticContentProperty, value);
        }

        ContentView _animatedContentView;
        ContentView _staticContentView;
        public ContentPageWrapper()
        {
            InitializeComponent();
            _animatedContentView = GetTemplateChild("animatedContentView") as ContentView;
            _staticContentView = GetTemplateChild("staticContentView") as ContentView;
            AnimateIn();
        }

        //you could make it into a BindableProperty
        uint animationLenght = 500;

        async void AnimateIn()
        {
            Animation animation = new((d) =>
            {
                _animatedContentView.TranslationX = (1 - d) * DeviceDisplay.MainDisplayInfo.Width;
                _animatedContentView.Rotation = (1 - d) * 720;
            });
            animation.Commit(this, nameof(ContentPageWrapper), length: animationLenght, easing: Easing.Linear);
        }

        async void AnimateOut()
        {
            Animation animation = new((d) =>
            {
                _animatedContentView.TranslationX = d * DeviceDisplay.MainDisplayInfo.Width;
                _animatedContentView.Rotation = d * 720;
            });
            animation.Commit(this, nameof(ContentPageWrapper), length: animationLenght, easing: Easing.Linear, finished: async (a, b) =>
            {
                //this is a little hack to workaround issue where using phone's back button doesnt't respect the Shell.PresentationMode
                _staticContentView.Opacity = 0;
                await Navigation.PopModalAsync();
            });
        }

        protected override bool OnBackButtonPressed()
        {
            AnimateOut();

            return true;

        }
    }
}
