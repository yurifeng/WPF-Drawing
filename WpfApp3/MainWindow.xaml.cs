using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Win32;

namespace WpfApp3
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        //引入ViewModel 字段
        private ViewModel viewModel;
        //引入Point字段
        private Point iniP;
        public MainWindow()
        {
            InitializeComponent();

            //初始化绘画属性
            DrawingAttributes drawingAttributes = new DrawingAttributes
            {
                Color = Colors.Red,
                Width = 2,
                Height = 2,
                StylusTip = StylusTip.Rectangle,
                //FitToCurve = true,
                IsHighlighter = false,
                IgnorePressure = true,

            };
            //将初始化的绘画属性赋值给画板的默认属性
            inkCanvasMeasure.DefaultDrawingAttributes = drawingAttributes;

            viewModel = new ViewModel
            {
                MeaInfo = "测试······",
                InkStrokes = new StrokeCollection(),
            };

            DataContext = viewModel;
        }

        //xaml层的打开文件事件触发的方法
        private void OpenFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog
            {
                Filter = "Image Files (*.jpg)|*.jpg|Image Files (*.png)|*.png|Image Files (*.bmp)|*.bmp",
                Title = "Open Image File"
            };

            if (openDialog.ShowDialog() == true)
            {
                BitmapImage image = new BitmapImage();
                image.BeginInit();
                image.UriSource = new Uri(openDialog.FileName, UriKind.RelativeOrAbsolute);
                image.EndInit();
                imgMeasure.Source = image;
            }
        }

        //xaml层绘制矩形的事件触发的方法
        private void DrawSquare_Click(object sender, RoutedEventArgs e)
        {
            if (btnSquare.IsChecked == true)
            {
                btnEllipse.IsChecked = false;
            }
        }

        //xaml层绘制椭圆的事件触发的方法
        private void DrawEllipse_Click(object sender, RoutedEventArgs e)
        {
            if (btnEllipse.IsChecked == true)
            {
                btnSquare.IsChecked = false;
            }
        }


        //鼠标左键按下触发事件
        private void InkCanvasMeasure_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                iniP = e.GetPosition(inkCanvasMeasure);
            }
        }

        //绘制图形的主要方法
        private void InkCanvasMeasure_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                //绘制矩形
                if (btnSquare.IsChecked == true)
                {
                    Point endP = e.GetPosition(inkCanvasMeasure);
                    List<Point> pointList = new List<Point>
                    {
                        new Point(iniP.X, iniP.Y),
                        new Point(iniP.X, endP.Y),
                        new Point(endP.X, endP.Y),
                        new Point(endP.X, iniP.Y),
                        new Point(iniP.X, iniP.Y),
                    };
                    StylusPointCollection point = new StylusPointCollection(pointList);
                    Stroke stroke = new Stroke(point)
                    {
                        DrawingAttributes = inkCanvasMeasure.DefaultDrawingAttributes.Clone()
                    };
                    viewModel.InkStrokes.Clear();
                    viewModel.InkStrokes.Add(stroke);
                }
                
                //绘制椭圆形
                else if (btnEllipse.IsChecked == true)
                {
                    Point endP = e.GetPosition(inkCanvasMeasure);
                    List<Point> pointList = GenerateEclipseGeometry(iniP, endP);
                    StylusPointCollection point = new StylusPointCollection(pointList);
                    Stroke stroke = new Stroke(point)
                    {
                        DrawingAttributes = inkCanvasMeasure.DefaultDrawingAttributes.Clone()
                    };
                    viewModel.InkStrokes.Clear();
                    viewModel.InkStrokes.Add(stroke);
                }
            }
        }

        //绘制椭圆所需的复杂坐标方法
        private List<Point> GenerateEclipseGeometry(Point st, Point ed)
        {
            double a = 0.5 * (ed.X - st.X);
            double b = 0.5 * (ed.Y - st.Y);
            List<Point> pointList = new List<Point>();
            for (double r = 0; r <= 2 * Math.PI; r = r + 0.01)
            {
                pointList.Add(new Point(0.5 * (st.X + ed.X) + a * Math.Cos(r), 0.5 * (st.Y + ed.Y) + b * Math.Sin(r)));
            }
            return pointList;
        }
    }
}
