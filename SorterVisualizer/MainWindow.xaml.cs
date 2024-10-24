using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SorterVisualizer;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private const int RECTANGLE_HEIGHT_FACTOR = 20;
    private const int RECTANGLE_WIDTH = 40;
    private const int RECTANGLE_MAX_HEIGHT = 300;
    private List<Rectangle> rectangles = new List<Rectangle>();
    private Random random = new Random();
    private int currentStep = 0;
    private int currentIndex = 0;
    private bool isPaused = false;
    private bool isRunning = false;
    
    
    public MainWindow()
    {
        InitializeComponent();
    }
    private void BtnStart_Click(object sender, RoutedEventArgs e)
    {
        if (isRunning) return;
        
        currentIndex = 0;
        currentStep = 0;
        isPaused = false; 
        isRunning = true;
        
        string[] input = TextBox.Text.Split(',');
        int[] heights = new int[input.Length];
        bool validInput = true;

        for (int i = 0; i < input.Length; i++)
        {
            if (int.TryParse(input[i].Trim(), out int height)) 
            {
                heights[i] = height * RECTANGLE_HEIGHT_FACTOR;
            }
            else
            {
                validInput = false; 
                break;
            }
        }
        
        if (!validInput)
        {
            heights = new int[10]; 
            for (int j = 0; j < heights.Length; j++)
            {
                heights[j] = random.Next(20, RECTANGLE_MAX_HEIGHT);
            }
        }
        
        InitializeRectangles(heights);
        
        string selectedAlgorithm = (AlgorithmSelector.SelectedItem as ComboBoxItem).Content.ToString();

        switch (selectedAlgorithm)
        {
            case "Bubble Sort":
                InitializeBubbleSort();
                break;
            case "Selection Sort":
                InitializeSelectionSort();
                break;
        }
    }
    
    private void BtnPause_Click(object sender, RoutedEventArgs e)
    {
        isPaused = !isPaused;
    }
    
    private void InitializeRectangles(int[] heights)
    {
        rectangles.Clear();
        CanvasMain.Children.Clear();
        
        for (int i = 0; i < heights.Length; i++)
        {
            var rect = new Rectangle
            {
                Width = RECTANGLE_WIDTH,
                Height = heights[i],
                Fill = Brushes.Blue
            };
            
            Canvas.SetLeft(rect, i * (RECTANGLE_WIDTH + 10));
            Canvas.SetBottom(rect, 0); 

            rectangles.Add(rect); 
            CanvasMain.Children.Add(rect); 
        }
    }
    
    private async Task InitializeBubbleSort()
    {
        await Task.Delay(1000);

        while (currentIndex < rectangles.Count - 1)
        {
            if (currentStep < rectangles.Count - currentIndex - 1)
            {
                if (!isPaused) 
                {
                    if (currentStep < rectangles.Count - 1 - currentIndex)
                    {
                        var rect1 = rectangles[currentStep];
                        var rect2 = rectangles[currentStep + 1];

                        if (GetHeight(rect1) > GetHeight(rect2))
                        {
                            await Swap(currentStep, currentStep + 1);
                        }

                        await Task.Delay(100); 
                    }
                    currentStep++;
                }
                else
                {
                    await Task.Delay(100);
                }
            }
            else
            {
                currentStep = 0; 
                currentIndex++; 
            }
        }

        isRunning = false; 
        MessageBox.Show("Ordenação Bubble Sort concluída!");
    }
    
    private async Task InitializeSelectionSort()
    {
        await Task.Delay(1000);

        while (currentIndex < rectangles.Count - 1)
        {
            if (!isPaused)
            {
                int minIndex = currentIndex;

                for (int j = currentIndex + 1; j < rectangles.Count; j++)
                {
                    if (GetHeight(rectangles[j]) < GetHeight(rectangles[minIndex]))
                    {
                        minIndex = j;
                    }
                }

                if (minIndex != currentIndex)
                {
                    await Swap(currentIndex, minIndex);
                }

                currentIndex++;
            }
            else
            {
                await Task.Delay(100); 
            }
        }

        isRunning = false;
        MessageBox.Show("Ordenação Selection Sort concluída!");
    }
    
    private double GetHeight(Rectangle rect)
    {
        return rect.Height;
    }
    
    private async Task Swap(int index1, int index2)
    {
        (rectangles[index1], rectangles[index2]) = (rectangles[index2], rectangles[index1]);

        DoubleAnimation animation1 = new DoubleAnimation
        {
            From = Canvas.GetLeft(rectangles[index1]),
            To = index1 * (RECTANGLE_WIDTH + 10),
            Duration = TimeSpan.FromSeconds(0.5)
        };

        DoubleAnimation animation2 = new DoubleAnimation
        {
            From = Canvas.GetLeft(rectangles[index2]),
            To = index2 * (RECTANGLE_WIDTH + 10),
            Duration = TimeSpan.FromSeconds(0.5)
        };
        
        rectangles[index1].BeginAnimation(Canvas.LeftProperty, animation1);
        
        rectangles[index2].BeginAnimation(Canvas.LeftProperty, animation2);
        
        await Task.Delay(500); 
    }
}

