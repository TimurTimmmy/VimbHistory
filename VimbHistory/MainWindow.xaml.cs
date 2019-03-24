using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using VimbHistory.items;

namespace VimbHistory
{
   
    public partial class MainWindow : Window
    {       
        public MainWindow()
        {
            InitializeComponent();
            
            this.DealTB.PreviewTextInput += new TextCompositionEventHandler(NumControl.DealTBChangedEventHandler);
        }

        private void LoadButton_Click(object sender, RoutedEventArgs e)
        {            
            ResultGrid.ItemsSource = SQLConnect.SQLout(DealTB.Text);            
        }
        private void DealTB_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Command == ApplicationCommands.Paste)  {e.Handled = true;}
        }
    }
}
