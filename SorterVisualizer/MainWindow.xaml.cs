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
    private bool isPaused = false;
    
    
    public MainWindow()
    {
        InitializeComponent();
    }
    private void BtnStart_Click(object sender, RoutedEventArgs e)
    {
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
                Fill = System.Windows.Media.Brushes.Blue
            };
            
            Canvas.SetLeft(rect, i * (RECTANGLE_WIDTH + 10));
            Canvas.SetBottom(rect, 0); 

            rectangles.Add(rect); 
            CanvasMain.Children.Add(rect); 
        }
    }

    
}