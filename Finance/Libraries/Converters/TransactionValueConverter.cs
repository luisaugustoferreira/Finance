using Empreendedores.MVVM.Enuns;
using Empreendedores.MVVM.Models;
using System.Globalization;


namespace Finances.Libraries.Converters
{
    public class TransactionValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Transaction transaction = (Transaction)value;
            if (transaction == null)
            {
                return "";
            }
            if (transaction.Type == TransactionType.Income)
                return transaction.Value.ToString("C");
            else
                return $"- {transaction.Value.ToString("C")}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
