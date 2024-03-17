using System.Windows;
using System.Windows.Threading;

namespace ATCGameACC
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window 
    {
        private List<Aircraft> aircrafts = new List<Aircraft>();
        private List<Point> routes = new List<Point>();
        private List<string> airlineCodes = new List<string>();
        private string allowedChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        private int score = 0;
        private Random random = new Random();
        private DispatcherTimer gameTimer;
        private DateTime lastTickTime;
        public MainWindow()
        {
            InitializeComponent();
            InitializeRoutes();
            InitializeAirlineCodes();
            InitializeGameTimer();
            GenerateRandomCallsign();
            AddAircraft();
            AddAircraftButton_OnClick(null, null);
            lastTickTime = DateTime.Now;
            UpdateGame(gameTimer.Interval.TotalMilliseconds);
        }

      

        private void InitializeRoutes()
        {
            // Populate routes
            routes.Add(new Point(100, 100)); // Route 1: Diagonale von links oben nach rechts unten
            routes.Add(new Point(1100, 1100));
            // Populate other routes...
        }

        private void InitializeAirlineCodes()
        {
            // Populate airline codes
            airlineCodes.AddRange(new string[] { "DLH", "AUA", "SWR", /* Add other codes */ });
        }
        
        
        private void InitializeGameTimer()
        {
            // Initialize game timer
            gameTimer = new DispatcherTimer();
            gameTimer.Interval = TimeSpan.FromMilliseconds(100); // Set your desired interval
            gameTimer.Tick += GameTimer_Tick; // Add your event handler
            gameTimer.Start();
        }
        private void UpdateGame(double elapsedTime)
        {
            // Update aircraft positions and other game logic
            foreach (Aircraft aircraft in aircrafts)
            {
                aircraft.Update(elapsedTime); // Update the aircraft's position and other properties
            }

            // Check for collisions or other game events
            //CheckCollisions(); // Implement this method to check for collisions between aircraft
            // Update score or other game state based on events

            // Redraw the canvas to reflect the updated game state
            GameCanvas.Children.Clear(); // Clear the canvas
            foreach (Aircraft aircraft in aircrafts)
            {
                aircraft.Draw(GameCanvas); // Redraw each aircraft on the canvas
            }
        }

        private void GameTimer_Tick(object sender, EventArgs e)
        {
            TimeSpan elapsedTime = DateTime.Now - lastTickTime;

            // Update lastTickTime to the current time
            lastTickTime = DateTime.Now;

            // Call UpdateGame with the elapsed time
            UpdateGame(elapsedTime.TotalMilliseconds);
        }

        private string GenerateRandomCallsign()
        {
            string randomAirlineCode = airlineCodes[random.Next(airlineCodes.Count)];
            string callsign = randomAirlineCode + random.Next(10);
            for (int i = 0; i < 2; i++)
            {
                callsign += allowedChars[random.Next(allowedChars.Length)];
            }
            return callsign;
        }

        private void AddAircraft()
        {
            double startX = random.Next(2) == 0 ? -10 : GameCanvas.Width + 10;
            double startY = random.Next(50, (int)GameCanvas.Height - 50);
            int exitHeight = random.Next(2) * 1000 + 23000; // Random exit height: 23000 or 24000
            int altitude = random.Next(24000, 40000); // Random altitude between 24000 and 40000

            // Ensure the altitude difference is within the valid range
            int altitudeDiff = Math.Abs(exitHeight - altitude);
            if (altitudeDiff <= 4000)
            {
                double direction = random.Next(360);
                double speed = 0.03;

                if (random.NextDouble() < 0.9)
                {
                    direction += (random.Next(2) == 0 ? -1 : 1) * random.Next(180);
                }

                // Generate random callsign
                string callsign = GenerateRandomCallsign();

                // Create Aircraft object
                Aircraft aircraft = new Aircraft(startX, startY, altitude, speed, direction, exitHeight, callsign);

                // Draw the aircraft on the canvas
                aircraft.Draw(GameCanvas);

                // Add the aircraft to the list
                aircrafts.Add(aircraft);
            }
        }

        private void AddAircraftButton_OnClick(object sender, RoutedEventArgs e)
        {
           AddAircraft();
        }
    }
}
