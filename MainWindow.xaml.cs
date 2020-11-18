using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;

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

            // Установить режим Ink в качестве стандартного.
            this.MyInkCanvas.EditingMode = InkCanvasEditingMode.Ink;
            this.inkRadio.IsChecked = true;
            this.comboColors.SelectedIndex = 0;
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

        private void RadioButtonClicked(object sender, RoutedEventArgs e)
        {
            // В зависимости от того, какая кнопка отправила событие,
            // поместить InkCanvas в нужный режим оперирования.
            switch ((sender as RadioButton)?.Content.ToString())
            {
                // Эти строки должны совпадать со значениями свойства Content
                // каждого элемента RadioButton.
                case "Ink Mode!":
                    this.MyInkCanvas.EditingMode = InkCanvasEditingMode.Ink;
                    break;
                case "Erase Mode!":
                    this.MyInkCanvas.EditingMode = InkCanvasEditingMode.EraseByStroke;
                    break;
                case "Select Mode!":
                    this.MyInkCanvas.EditingMode = InkCanvasEditingMode.Select;
                    break;
            }
        }

        private void ColorChanged(object sender, SelectionChangedEventArgs e)
        {
            // Получить выбранный элемент в раскрывающемся списке.
            string colorToUse = (this.comboColors.SelectedItem as ComboBoxItem)?.Content.ToString();
            // Изменить цвет, используемый для визуализации штрихов.
            this.MyInkCanvas.DefaultDrawingAttributes.Color = (Color)ColorConverter.ConvertFromString(colorToUse);
        }

        private void ColorChanged2(object sender, SelectionChangedEventArgs e)
        {
            // Получить свойство Tag выбранного элемента StackPanel.
            string colorToUse = (this.comboColors2.SelectedItem as StackPanel)?.Tag.ToString();
            // Изменить цвет, используемый для визуализации штрихов.
            this.MyInkCanvas.DefaultDrawingAttributes.Color = (Color)ColorConverter.ConvertFromString(colorToUse);
        }

        private void SaveData(object sender, RoutedEventArgs e)
        {
            // Сохранить все данные InkCanvas в локальном файле.
            using (FileStream fs = new FileStream("StrokeData.bin", FileMode.Create))
            {
                this.MyInkCanvas.Strokes.Save(fs);
                fs.Close();
            }
        }

        private void LoadData(object sender, RoutedEventArgs e)
        {
            // Наполнить StrokeCollection из файла.
            using (FileStream fs = new FileStream("StrokeData.bin",
            FileMode.Open, FileAccess.Read))
            {
                StrokeCollection strokes = new StrokeCollection(fs);
                this.MyInkCanvas.Strokes = strokes;
            }
        }

        private void Clear(object sender, RoutedEventArgs e)
        {
            // Очистить все штрихи.
            this.MyInkCanvas.Strokes.Clear();
        }
    }
}
