using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ATCGameACC
{
    public class Aircraft
    {
        public double X { get; set; }
        public double Y { get; set; }
        public int Altitude { get; set; }
        public double Speed { get; set; }
        public double Direction { get; set; }
        public int ExitHeight { get; set; }
        public string Callsign { get; set; }
        public Point Position { get; set; }
        public bool IsSelected { get; private set; }


        public readonly double Radius = 10;

        public Aircraft(double startX, double startY, int altitude, double speed, double direction, int exitHeight, string callsign)
        {
            X = startX;
            Y = startY;
            Altitude = altitude;
            Speed = speed;
            Direction = direction;
            ExitHeight = exitHeight;
            Callsign = callsign;
            Position = new Point(startX, startY);
        }
        public void Select()
        {
            IsSelected = true;
        }

        public void Deselect()
        {
            IsSelected = false;
        }
        public void Draw(Canvas canvas)
        {
            // Check if position is valid
            if (double.IsNaN(Position.X) || double.IsNaN(Position.Y))
            {
                return; // Skip drawing if position is NaN
            }

            const double length = 20; // Adjust the length of the aircraft line as needed
            const double degreesToRadians = Math.PI / 180.0;

            // Convert direction to radians
            double radians = Direction * degreesToRadians;

            // Calculate the end point based on the direction and length of the aircraft
            double endX = Position.X + length * Math.Cos(radians);
            double endY = Position.Y + length * Math.Sin(radians);

            // Draw the aircraft line
            Line aircraftLine = new Line
            {
                X1 = Position.X,
                Y1 = Position.Y,
                X2 = endX,
                Y2 = endY,
                Stroke = Brushes.GhostWhite,
                // Adjust the stroke thickness as needed
                StrokeThickness = 1, // Adjusted to 1
            };

            canvas.Children.Add(aircraftLine);

            // Draw text labels for aircraft data
            TextBlock textBlock = new TextBlock
            {
                Text = $"{Callsign} {Altitude} ft\n{Speed} kt \n {Direction}°",
                Foreground = Brushes.White,
                FontSize = 12,
                Margin = new Thickness(Position.X + 5, Position.Y + 5, 0, 0) // Adjust position of text
            };

            canvas.Children.Add(textBlock);
        }
        public void Update(double elapsedTime)
        {
            // Calculate the distance traveled based on speed and direction
            double distance = Speed * elapsedTime;

            // Calculate the new position based on the direction and distance
            double newX = Position.X + distance * Math.Cos(Direction * Math.PI / 180);
            double newY = Position.Y + distance * Math.Sin(Direction * Math.PI / 180);
            Position = new Point(newX, newY);
        }
    }
}