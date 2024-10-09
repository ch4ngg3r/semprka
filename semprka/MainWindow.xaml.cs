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
using System.Windows.Threading;

namespace semprka
{
    public partial class MainWindow : Window
    {
        private List<Rectangle> disks = new List<Rectangle>();
        private bool isDragging = false; // Флаг для отслеживания перетаскивания
        private Rectangle draggedDisk = null; // Диск, который перетаскивается
        private double offsetY;
        private bool canDrag = false; // Флаг для разрешения перетаскивания

        public MainWindow()
        {
            InitializeComponent();
            InitializeDisks();
        }

        private void InitializeDisks()
        {
            for (int i = 0; i < 3; i++)
            {
                Rectangle disk = new Rectangle
                {
                    Width = 40 + i * 30,
                    Height = 20,
                    Fill = new SolidColorBrush(Colors.Red),
                    Stroke = new SolidColorBrush(Colors.Black),
                    StrokeThickness = 1,
                };

                // Обработчики событий для перетаскивания дисков
                disk.MouseLeftButtonDown += Disk_MouseLeftButtonDown;
                disk.MouseMove += Disk_MouseMove;
                disk.MouseLeftButtonUp += Disk_MouseLeftButtonUp;

                disks.Add(disk);
                CanvasArea.Children.Add(disk);
                Canvas.SetLeft(disk, 100); // Положение на первом стержне
                Canvas.SetTop(disk, 150 - (i * 20)); // Сложение по высоте
            }
        }

        private void Disk_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (canDrag && sender is Rectangle disk)
            {
                draggedDisk = disk;
                offsetY = e.GetPosition(disk).Y; // Сохраняем смещение
                isDragging = true;
                disk.CaptureMouse(); // Зафиксировать мышь на диске
            }
        }

        private void Disk_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging && draggedDisk != null)
            {
                // Перемещение диска с учетом смещения
                var mousePos = e.GetPosition(CanvasArea);
                Canvas.SetLeft(draggedDisk, mousePos.X - draggedDisk.Width / 2);
                Canvas.SetTop(draggedDisk, mousePos.Y - offsetY);
            }
        }

        private void Disk_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (isDragging && draggedDisk != null)
            {
                isDragging = false;
                draggedDisk.ReleaseMouseCapture(); // Освободить захват мыши
                draggedDisk = null;
            }
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            canDrag = true; // Разрешаем перетаскивание дисков
        }
    }
}