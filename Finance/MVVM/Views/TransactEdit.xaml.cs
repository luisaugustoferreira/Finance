using CommunityToolkit.Mvvm.Messaging;
using Empreendedores.Data.Repositories;
using Empreendedores.MVVM.Enuns;
using Empreendedores.MVVM.Models;
using Finances.Libraries.Utils.FixBugs;
using System.Text;

namespace Empreendedores.MVVM.Views;

public partial class TransactEdit : ContentPage
{
    private readonly ITransactionRepository transactionRepository;
    private Transaction transaction;


    public TransactEdit(ITransactionRepository transactionRepository)
    {
        this.transactionRepository = transactionRepository;
        InitializeComponent();
    }



    public void SetTransactionToEdit(Transaction transaction)
    {
        this.transaction = transaction;
        if (transaction.Type == TransactionType.Income)
            RadioIncome.IsChecked = true;
        else
            RadioExpense.IsChecked = true;

        EntryName.Text = transaction.Name;
        DatePickerData.Date = transaction.Data.Date;
        EntryValor.Text = transaction.Value.ToString();

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
            Id = this.transaction.Id,
            Type = RadioIncome.IsChecked ? TransactionType.Income : TransactionType.Expenses,
            Name = EntryName.Text,
            Data = DatePickerData.Date,
            Value = Math.Abs(double.Parse(EntryValor.Text))
        };

        transactionRepository.Update(transaction);
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
    private void Fechar_Tapped(object sender, TappedEventArgs e)
    {
        KeyboardFixBugs.HideKeyboard();
        Navigation.PopModalAsync();

    }
}