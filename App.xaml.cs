namespace Grimoire;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();
		// MainPage assignment deprecated in .NET 9 – navigation is handled via the Window created below.
	}

	protected override Window CreateWindow(IActivationState? activationState)
	{
		return new Window(new AppShell());
	}
}