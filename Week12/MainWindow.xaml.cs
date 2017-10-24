using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Week12
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Color[] colours = new Color[10] {Colors.White, Colors.Yellow,
            Colors.Green,Colors.LightBlue, Colors.LightGreen,
            Colors.LightCyan, Colors.LightGray, Colors.LightPink,
            Colors.Purple, Colors.Red};

        // Store the active lines, each of which corresponds to a place 
        // the user is currently touching down.
        Dictionary<int, UserLine> userLines = new Dictionary<int, UserLine>();

        int counter = 0;

        // For mouse
        bool isMouseDown = false;
        int strokeThickness = 4;
        SolidColorBrush stroke;
        Point lastPosition;

        public MainWindow()
        {
            InitializeComponent();
        }


        private void canvas_TouchDown(object sender, TouchEventArgs e)
        {
            // Get a number between 1 and 10 to select colour
            counter = (counter + 1) % 10;
            // Add new object for this id
            userLines[e.TouchDevice.Id] = new UserLine(colours[counter], e.GetTouchPoint(canvas));
        }

        private void canvas_TouchMove(object sender, TouchEventArgs e)
        {
            TouchPoint currentPoint = e.GetTouchPoint(canvas);
            canvas.Children.Add(userLines[e.TouchDevice.Id].DrawLine(currentPoint));
        }

        private void canvas_TouchUp(object sender, TouchEventArgs e)
        {
            // Remove the line 
            userLines.Remove(e.TouchDevice.Id);
        }

        private void canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (lastPosition == null)
                lastPosition = e.GetPosition(canvas);

            isMouseDown = true;
            counter = (counter + 1) % 10;
            strokeThickness = 4;
            stroke = new SolidColorBrush(colours[counter]);
        }

        private void canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (isMouseDown) // draw line only when mouse is down
            {
                // Move the line to the new mouse-down point.
                Point mousePoint = e.GetPosition(canvas);
                Line line = new Line
                {
                    StrokeThickness = strokeThickness,
                    Stroke = stroke,
                    X1 = lastPosition.X,
                    Y1 = lastPosition.Y,
                    X2 = mousePoint.X,
                    Y2 = mousePoint.Y
                };

                lastPosition = mousePoint;

                canvas.Children.Add(line);
            }
        }

        private void canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            isMouseDown = false; // Stop drawing line
        }

    }

    public class UserLine
    {
        int strokeThickness = 4;

        SolidColorBrush Brush { get; set; }
        Color LineColor { get; set; }
        TouchPoint InitialPosition { get; set; }

        public UserLine(Color color, TouchPoint initialPosition)
        {
            LineColor = color;
            Brush = new SolidColorBrush(LineColor);
            InitialPosition = initialPosition;
        }

        public Line DrawLine(TouchPoint currentPosition)
        {
            Line line = new Line
            {
                StrokeThickness = strokeThickness,
                Stroke = Brush,

                X1 = InitialPosition.Position.X,
                Y1 = InitialPosition.Position.Y,
                X2 = currentPosition.Position.X,
                Y2 = currentPosition.Position.Y
            };

            InitialPosition = currentPosition;

            return line;
        }

    }
}
