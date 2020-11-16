using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string _mouseActivity = string.Empty;

        public MainWindow()
        {
            InitializeComponent();
            SetFICommandBinding();
        }

        private void SetFICommandBinding()
        {
            CommandBinding helpBinding = new CommandBinding(ApplicationCommands.Help);
            helpBinding.CanExecute += CanHelpExecute;
            helpBinding.Executed += HelpExecuted;
            CommandBindings.Add(helpBinding);
        }

        private void HelpExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show("Look, it is not that difficult. Just type something!", "Help!");
        }

        private void CanHelpExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            // Если нужно предотвратить выполнение команды,
            //то можно установить CanExecute в false.
            e.CanExecute = true;
        }

        protected void FileExit_Click(object sender, RoutedEventArgs args)
        {
            // Закрыть это окно.
            this.Close();
        }

        protected void ToolsSpellingHints_Click(object sender, RoutedEventArgs args)
        {
            string spellingHints = string.Empty;
            // Попробовать получить ошибку правописания в текущем положении курсора ввода.
            SpellingError error = txtData.GetSpellingError(txtData.CaretIndex);
            if (error != null)
            {
                // Построить строку с предполагаемыми вариантами правописания.
                foreach (string s in error.Suggestions)
                {
                    spellingHints += $"{s}\n";
                    // Отобразить предполагаемые варианты и раскрыть элемент Expander.
                    lblSpellingHints.Content = spellingHints;
                    expanderSpelling.IsExpanded = true;
                }
            }
        }

        protected void MouseEnterExitArea(object sender, RoutedEventArgs args)
        {
            statBarText.Text = "Exit the Application";
        }

        protected void MouseEnterToolsHintsArea(object sender, RoutedEventArgs args)
        {
            statBarText.Text = "Show Spelling Suggestions";
        }

        protected void MouseLeaveArea(object sender, RoutedEventArgs args)
        {
            statBarText.Text = "Ready";
        }

        private void OpenCmdCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void SaveCmdCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void OpenCmdExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            // Создать диалоговое окно открытия файла
            // и показать в нем только текстовые файлы.
            var openDlg = new OpenFileDialog { Filter = "Text Files |*.txt" };
            // Был ли совершен щелчок на кнопке ОК?
            if (true == openDlg.ShowDialog())
            {
                // Загрузить содержимое выбранного файла.
                string dataFromFile = File.ReadAllText(openDlg.FileName);
                // Отобразить строку в TextBox.
                txtData.Text = dataFromFile;
            }
        }
        private void SaveCmdExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            var saveDlg = new SaveFileDialog { Filter = "Text Files |*.txt" };
            // Был ли совершен щелчок на кнопке ОК?
            if (true == saveDlg.ShowDialog())
            {
                // Сохранить данные из TextBox в указанном файле.
                File.WriteAllText(saveDlg.FileName, txtData.Text);
            }
        }

        public void btnClickMe_Clicked(object sender, RoutedEventArgs e)
        {
            // Делать что-нибудь, когда на кнопке произведен щелчок.
            MessageBox.Show("Clicked the button");
        }

        public void outerEllipse_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // Изменить заголовок окна.
            MessageBox.Show("You clicked the outer ellipse!");

            // Остановить пузырьковое распространение.
            e.Handled = true;
        }

        public void btn2_Clicked(object sender, RoutedEventArgs e)
        {
            AddEventlnfo(sender, e);
            MessageBox.Show(_mouseActivity, "Your Event Info");
            // Очистить строку для следующего цикла.
            _mouseActivity = "";
        }

        private void AddEventlnfo(object sender, RoutedEventArgs e)
        {
            _mouseActivity += string.Format(
            "{0} sent a {1} event named {2}.\n", sender,
            e.RoutedEvent.RoutingStrategy,
            e.RoutedEvent.Name);
        }

        private void outerEllipse2_MouseDown(object sender, MouseButtonEventArgs e)
        {
            AddEventlnfo(sender, e);
        }

        private void outerEllipse2_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            AddEventlnfo(sender, e);
        }
    }
}
