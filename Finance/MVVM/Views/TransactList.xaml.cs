using CommunityToolkit.Mvvm.Messaging;
using Empreendedores.Data.Repositories;
using Empreendedores.MVVM.Enuns;
using Transaction = Empreendedores.MVVM.Models.Transaction;

namespace Empreendedores.MVVM.Views;

public partial class TransactList : ContentPage
{
    //private readonly TransactAdd transactAdd;
    //private readonly TransactEdit transactEdit;
    private readonly ITransactionRepository transactionRepository;


    //   public TransactList(TransactAdd transactAdd, TransactEdit transactEdit, ITransactionRepository transactionRepository)
    public TransactList(ITransactionRepository transactionRepository)

    {
        this.transactionRepository = transactionRepository;
        InitializeComponent();
        reload();
        WeakReferenceMessenger.Default.Register<string>(this, (e, msg) =>
        {
            reload();
        });

    }

    private void reload()
    {
        var itens = transactionRepository.GetAll();
        CollectionViewTransaction.ItemsSource = itens;

        var receita = itens.Where(c => c.Type == TransactionType.Income).Sum(c => c.Value);
        var despesas = itens.Where(c => c.Type == TransactionType.Expenses).Sum(c => c.Value);
        double balance = receita - despesas;
        LabelIncome.Text = receita.ToString("C");
        LabelExpense.Text = despesas.ToString("C");
        LabelBalance.Text = balance.ToString("C");
    }

    private void Button_Add(object sender, EventArgs e)
    {
        /*
         * Publisher - Subscribers
         * TransactionAdd -> Publisher > Cadastro
         * Subscribers -> TransactionList (Cadastro) 
         */
        // Navigation.PushAsync(new TransactAdd());
        var transactAdd = Handler.MauiContext.Services.GetService<TransactAdd>();
        Navigation.PushModalAsync(transactAdd);
    }

    private void Editar_Tapped(object sender, TappedEventArgs e)
    {
        var grid = (Grid)sender;
        var gesture = (TapGestureRecognizer)grid.GestureRecognizers[0];
        var transaction = (Transaction)gesture.CommandParameter;

        var transactEdit = Handler.MauiContext.Services.GetService<TransactEdit>();
        transactEdit.SetTransactionToEdit(transaction);
        Navigation.PushModalAsync(transactEdit);
    }

    private async void Excluir_Tapped(object sender, TappedEventArgs e)
    {
        await AnimationBorder((Border)sender, true);
        var trasaction = (Transaction)e.Parameter;
        bool result = await App.Current.MainPage.DisplayAlert("Excluir", "Tem certeza que deseja excluir?", "Sim", "Não");
        if (result)
        {
            transactionRepository.Delete(trasaction);
            reload();
        }
        else
        {
            await AnimationBorder((Border)sender, false);
        }

    }

    private Color _borderOriginalBackgroundColor;
    private string _labelOriginalTex;
    private async Task AnimationBorder(Border border, bool IsDeleteAnimation)
    {
        var label = (Label)border.Content;
        if (IsDeleteAnimation)
        {
            _borderOriginalBackgroundColor = border.BackgroundColor;
            _labelOriginalTex = label.Text;
            await border.RotateYTo(90, 500);
            border.BackgroundColor = Colors.Red;
          
            label.TextColor = Colors.White;
            label.Text = "X";
            await border.RotateYTo(180, 500);
        }
        else
        {
            await border.RotateYTo(90, 500);
            label.TextColor = Colors.Black;
            border.BackgroundColor = _borderOriginalBackgroundColor;
            label.Text = _labelOriginalTex;
            await border.RotateYTo(0, 500);
        }
    }
}