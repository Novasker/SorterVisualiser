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
        if (isRunning)
        {   
            isRunning = false;
            rectangles.Clear();
            CanvasMain.Children.Clear();
        }
        else
        {
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
                heights = new int[15];
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
                case "Merge Sort":
                    InitializeMergeSort();
                    break;
                case "Quick Sort":
                    InitializeQuickSort();
                    break;
                case "Insertion Sort":
                    InitializeInsertionSort();
                    break;
                case "Cocktail Shaker Sort":
                    InitializeCocktailShakerSort();
                    break;
                case "Bogo Sort":
                    InitializeBogoSort();
                    break;
                case "Miracle Sort":
                    InitializeMiracleSort();
                    break;
            }
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
    private async Task InitializeMergeSort()
    {
        await Task.Delay(1000);
        await MergeSort(0, rectangles.Count - 1);
        isRunning = false;
        MessageBox.Show("Ordenação Merge Sort concluída!");
    }
    private async Task InitializeQuickSort()
    {
        await Task.Delay(1000); 
        await QuickSort(0, rectangles.Count - 1); 
        isRunning = false;
        MessageBox.Show("Ordenação Quick Sort concluída!");
    }
    private async Task InitializeInsertionSort()
    {
        await Task.Delay(1000); 
        
        for (int i = 1; i < rectangles.Count; i++)
        {
            var keyRect = rectangles[i];
            double keyHeight = GetHeight(keyRect);

            int j = i - 1;
            
            while (j >= 0 && GetHeight(rectangles[j]) > keyHeight)
            {
                await Swap(j, j + 1); 
                j--;
            }
        }

        isRunning = false;
        MessageBox.Show("Ordenação Insertion Sort concluída!");
    }
    private async Task InitializeCocktailShakerSort()
    {
        await Task.Delay(1000); 

        bool swapped = true;
        int start = 0;
        int end = rectangles.Count - 1;

        while (swapped)
        {
            swapped = false;

         
            for (int i = start; i < end; i++)
            {
                if (GetHeight(rectangles[i]) > GetHeight(rectangles[i + 1]))
                {
                    await Swap(i, i + 1); 
                    swapped = true; 
                }
            }

 
            if (!swapped)
                break;


            end--;

            swapped = false;


            for (int i = end - 1; i >= start; i--)
            {
                if (GetHeight(rectangles[i]) > GetHeight(rectangles[i + 1]))
                {
                    await Swap(i, i + 1); 
                    swapped = true; 
                }
            }
            
            start++;
        }

        isRunning = false;
        MessageBox.Show("Ordenação Cocktail Shaker Sort concluída!");
    }
    private async Task InitializeBogoSort()
    {
        await Task.Delay(1000);

        while (!IsSorted())
        {
            if (!isPaused)
            {
                ShuffleRectangles();
                
                await Task.Delay(1000); 
            }
            else
            {
                await Task.Delay(100); 
            }
        }

        isRunning = false;
        MessageBox.Show("Ordenação Bogo Sort concluída!");
    }
    private async Task InitializeMiracleSort()
    {
        await Task.Delay(1000);

        while (!IsSorted())
        {
            await Task.Delay(100);
        }

        isRunning = false;
        MessageBox.Show("Ordenação Miracle Sort concluída!");
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
    private double GetHeight(Rectangle rect)
    {
        return rect.Height;
    }
    private async Task MergeSort(int left, int right)
    {
        if (left < right)
        {
            int mid = (left + right) / 2;
            
            await MergeSort(left, mid);
            await MergeSort(mid + 1, right);
            
            await Merge(left, mid, right);
        }
    }

    private async Task Merge(int left, int mid, int right)
    {
        int leftIndex = left;
        int rightIndex = mid + 1;

        List<Rectangle> temp = new List<Rectangle>();
        
        while (leftIndex <= mid && rightIndex <= right)
        {
            if (GetHeight(rectangles[leftIndex]) <= GetHeight(rectangles[rightIndex]))
            {
                temp.Add(rectangles[leftIndex]);
                leftIndex++;
            }
            else
            {
                temp.Add(rectangles[rightIndex]);
                rightIndex++;
            }
        }
        
        while (leftIndex <= mid)
        {
            temp.Add(rectangles[leftIndex]);
            leftIndex++;
        }
        
        while (rightIndex <= right)
        {
            temp.Add(rectangles[rightIndex]);
            rightIndex++;
        }
        
        for (int i = left; i <= right; i++)
        {
            int tempIndex = i - left;

            if (rectangles[i] != temp[tempIndex])
            {
                await Swap(i, FindIndexOfRectangle(temp[tempIndex]));
            }
        }
    }
    private int FindIndexOfRectangle(Rectangle rect)
    {
        return rectangles.IndexOf(rect);
    }
    private bool IsSorted()
    {
        for (int i = 0; i < rectangles.Count - 1; i++)
        {
            if (GetHeight(rectangles[i]) > GetHeight(rectangles[i + 1]))
            {
                return false;
            }
        }
        return true;
    }
    private async void ShuffleRectangles()
    {
        for (int i = 0; i < rectangles.Count; i++)
        {
            int randomIndex = random.Next(0, rectangles.Count);
            await Swap(i, randomIndex);
        }
    }
    private async Task QuickSort(int low, int high)
    {
        if (low < high)
        {
            int pivotIndex = await Partition(low, high);
            
            await QuickSort(low, pivotIndex - 1);
            await QuickSort(pivotIndex + 1, high);
        }
    }
    private async Task<int> Partition(int low, int high)
    {
        var pivot = rectangles[high];
        double pivotHeight = GetHeight(pivot);

        int i = low - 1;

        for (int j = low; j < high; j++)
        {
            if (GetHeight(rectangles[j]) < pivotHeight)
            {
                i++;
                await Swap(i, j); 
            }
        }
        
        await Swap(i + 1, high);
        return i + 1; 
    }
}

