using System.Windows;
using System.Windows.Input;
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
        private Aircraft selectedAircraft;
        
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
            GameCanvas.MouseLeftButtonDown += GameCanvas_MouseLeftButtonDown;
            // Add event handler for key down
            this.KeyDown += MainWindow_KeyDown;
            this.Focus();
            
            CheckCollisions(); // Implement this method to check for collisions between aircraft
            UpdateGameState(); // Implement this method to update the game state based on events

// Redraw the canvas to reflect the updated game state
            UpdateCanvas();
          
            UpdateCanvas();
            UpdateGame(gameTimer.Interval.TotalMilliseconds);
            
            
        }

        private void GameCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Point clickPoint = e.GetPosition(GameCanvas);

            // Check if any aircraft is clicked
            foreach (Aircraft aircraft in aircrafts)
            {
                if (IsPointWithinAircraft(clickPoint, aircraft))
                {
                    // Toggle selection
                    aircraft.IsSelected = !aircraft.IsSelected;
                    break; // No need to check further
                }
            }
            UpdateCanvas();
        }

        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            // Handle key events only if an aircraft is selected
            Aircraft selectedAircraft = aircrafts.FirstOrDefault(a => a.IsSelected);
            if (selectedAircraft != null)
            {
                // Handle altitude adjustment
                if (e.Key == Key.Up)
                {
                    selectedAircraft.Altitude += 1000; // Increase altitude by 1000 ft
                }
                else if (e.Key == Key.Down)
                {
                    selectedAircraft.Altitude -= 1000; // Decrease altitude by 1000 ft
                }
                // Handle speed adjustment
                else if (e.Key == Key.Left)
                {
                    selectedAircraft.Speed -= 10; // Decrease speed by 10 knots
                }
                else if (e.Key == Key.Right)
                {
                    selectedAircraft.Speed += 10; // Increase speed by 10 knots
                }

                // Redraw canvas
                UpdateCanvas();
            }
        }
        private void UpdateCanvas()
        {
            GameCanvas.Children.Clear();
            foreach (Aircraft aircraft in aircrafts)
            {
                aircraft.Draw(GameCanvas);
            }
        }
        
        private bool IsPointWithinAircraft(Point point, Aircraft aircraft)
        {
            // Implement logic to check if the point is within the aircraft's bounds
            return false; // Placeholder
        }
        private void InitializeRoutes()
        {
            // Populate routes
            routes.Add(new Point(100, 100));
            routes.Add(new Point(1100, 1100));

            // Route 2: Example of another route
            routes.Add(new Point(500, 200));
            routes.Add(new Point(800, 600));

            // Route 3: Example of yet another route
            routes.Add(new Point(300, 400));
            routes.Add(new Point(1000, 200));

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

        private void CheckCollisions()
        {
            for (int i = 0; i < aircrafts.Count; i++)
            {
                for (int j = i + 1; j < aircrafts.Count; j++)
                {
                    if (AircraftsCollided(aircrafts[i], aircrafts[j]))
                    {
                        // Handle collision between aircrafts[i] and aircrafts[j]
                        // For example, you can remove both aircrafts from the list, update score, etc.
                        aircrafts.RemoveAt(i);
                        aircrafts.RemoveAt(j - 1); // Adjust the index since removing the previous item shifts the index
                        // Adjust score or other game state based on collision
                        score += 100; // Increase score by 100 for each collision
                        return; // Exit the method after handling the first collision
                    }
                }
            }
        }

        private bool AircraftsCollided(Aircraft aircraft1, Aircraft aircraft2)
        {
            // Implement collision detection logic here
            // For example, you can check if the distance between the aircrafts is less than a certain threshold
            // If the distance is less than the threshold, consider it a collision
            double distance = Math.Sqrt(Math.Pow(aircraft1.X - aircraft2.X, 2) + Math.Pow(aircraft1.Y - aircraft2.Y, 2));
            return distance < (2 * aircraft1.Radius); // Considered a collision if the distance is less than twice the radius of an aircraft
        }

        private void UpdateGameState()
        {
            // Implement game state update logic here
            // For example, you can update the score based on time, collisions, etc.
            score += 10; // Increase score by 10 in each game tick
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
            {
                double startX, startY;

                // Randomly select whether the aircraft will spawn on the left or right edge
                if (random.Next(2) == 0)
                {
                    // Spawn on the left edge
                    startX = -10;
                    startY = random.Next(50, (int)GameCanvas.Height - 50); // Random Y coordinate within canvas height
                }
                else
                {
                    // Spawn on the right edge
                    startX = GameCanvas.Width + 10;
                    startY = random.Next(50, (int)GameCanvas.Height - 50); // Random Y coordinate within canvas height
                }

                int exitHeight = random.Next(2) * 1000 + 23000; // Random exit height: 23000 or 24000
                int altitude = random.Next(24000, 40000); 
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
                
                aircrafts.Add(aircraft); 
            }
        }
        
        private void AddAircraftButton_OnClick(object sender, RoutedEventArgs e)
        {
           AddAircraft();
        }
    }
}
