using Infrastructure.Dtos;
using Infrastructure.Services;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Presentation.WpfApp
{
    public partial class MainWindow : Window
    {

        private readonly TaskService _taskService;

        public MainWindow(TaskService taskService)
        {
                InitializeComponent();
                 _taskService = taskService;

        }

    }
}