using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using TRABAJO_GRUPAL_AVALONIA.View;

namespace Hotel;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow();
            desktop.ShutdownMode = Avalonia.Controls.ShutdownMode.OnLastWindowClose;
        }

        base.OnFrameworkInitializationCompleted();
    }
}