using Empreendedores.MVVM.Views;

namespace Empreendedores;

public partial class App : Application
{
    public App(TransactList listPage)
    {
        InitializeComponent();
        MainPage = new NavigationPage(listPage);
    }
}
