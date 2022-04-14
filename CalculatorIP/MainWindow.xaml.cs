using System.Windows;
using System.Windows.Input;
using static CalculatorIP.Session;

namespace CalculatorIP
{
    public partial class MainWindow : Window
    {
        private Session _session = new Session();
        string _result;
        private SelectedTask _currentTask = SelectedTask.None;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void BorderMouseDown(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            if (mouseButtonEventArgs.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void HideApplication(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.WindowState = WindowState.Minimized;
        }

        private void CloseApplication(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void SelectFirstTask(object sender, RoutedEventArgs e)
        {
            _currentTask = SelectedTask.First;
            UpdateInformation(_session.GetNewTask(_currentTask));
        }

        private void SelectSecondTask(object sender, RoutedEventArgs e)
        {
            _currentTask = SelectedTask.Second;
            UpdateInformation(_session.GetNewTask(_currentTask));
        }

        private void SelectThirdTask(object sender, RoutedEventArgs e)
        {
            _currentTask = SelectedTask.Third;
            UpdateInformation(_session.GetNewTask(_currentTask));
        }

        private void UpdateInformation(GeneratedTask generatedTask)
        {
            TaskLable.Content = generatedTask.Task;
            ResultLable.Content = generatedTask.Description;
            _result = generatedTask.Result;
        }

        private void CopyTask(object sender, RoutedEventArgs e)
        {
            if (TaskLable.Content is null == false)
                Clipboard.SetText(TaskLable.Content.ToString());
        }

        private void CopyResult(object sender, RoutedEventArgs e)
        {
            if (_result is null == false)
                Clipboard.SetText(_result);
        }
    }
}
