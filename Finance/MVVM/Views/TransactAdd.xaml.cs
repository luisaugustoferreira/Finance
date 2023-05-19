using CommunityToolkit.Mvvm.Messaging;
using Empreendedores.Data.Repositories;
using Empreendedores.MVVM.Enuns;
using Empreendedores.MVVM.Models;
using Finances.Libraries.Utils.FixBugs;
using Microsoft.Maui.Platform;
using System.Text;

namespace Empreendedores.MVVM.Views;

public partial class TransactAdd : ContentPage
{
    private readonly ITransactionRepository _transactionRepository;
    public TransactAdd(ITransactionRepository transactionRepository)
    {
        _transactionRepository = transactionRepository;
        InitializeComponent();
    }

    private void Fechar_Tapped(object sender, TappedEventArgs e)
    {
        KeyboardFixBugs.HideKeyboard();

        Navigation.PopModalAsync();
    }

    private void Salvar_Clicked(object sender, EventArgs e)
    {
        //exemplo como capiturar dados program 
        //var repository = this.Handler.MauiContext.Services.GetService<ITransactionRepository>();

        if (IsValidData() == false)
            return;
        SaveTransactionInDatabase();

        //TODO - Fechar Tela*
        Navigation.PopModalAsync();
        KeyboardFixBugs.HideKeyboard();
        WeakReferenceMessenger.Default.Send<string>("");

    }

    private void SaveTransactionInDatabase()
    {
        Transaction transaction = new Transaction()
        {
            Type = RadioIncome.IsChecked ? TransactionType.Income : TransactionType.Expenses,
            Name = EntryName.Text,
            Data = DatePickerData.Date,
            Value = double.Parse(EntryValor.Text)
        };

        _transactionRepository.Add(transaction);
    }

    private bool IsValidData()
    {
        bool valid = true;
        StringBuilder sb = new StringBuilder();

        if (string.IsNullOrEmpty(EntryName.Text) || string.IsNullOrWhiteSpace(EntryName.Text))
        {
            sb.AppendLine("O campo 'Nome' precisa ser preenchido!");
            valid = false;
        }

        if (string.IsNullOrEmpty(EntryValor.Text) || string.IsNullOrWhiteSpace(EntryValor.Text))
        {
            sb.AppendLine("O campo 'Valor' precisa ser preenchido!");
            valid = false;
        }
        double result;
        if (!string.IsNullOrEmpty(EntryValor.Text) && !double.TryParse(EntryValor.Text, out result))
        {
            sb.AppendLine("O campo 'Valor' é inválido!");
            valid = false;
        }
        if (valid == false)
        {
            LabelError.IsVisible = true;
            LabelError.Text = sb.ToString();
        }
        return valid;
    }
}