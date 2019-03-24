using VimbHistory;
using System.Windows.Input;
using System.Text.RegularExpressions;
using System;

namespace VimbHistory.items
{
    class NumControl
    {
        public static void DealTBChangedEventHandler(object sender, TextCompositionEventArgs e)
        {
            e.Handled = Regex.IsMatch(Convert.ToString(e.Text), "[^0-9\b]+");
        }
    }     
}