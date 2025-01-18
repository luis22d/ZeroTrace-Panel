using System;
using System.Drawing;
using System.Windows.Forms;

namespace ZeroTrace_Stealer
{
    internal class LiveChart : UserControl
    {
        private Timer updateTimer;
        private int[] chartData;
        private int maxDataPoints = 200; // Increase max data points
        private int dataIndex = 0;
        private int[] counts; // Array to store counts for different labels
        private string[] labels = { "uscount", "italycount", "canadacount", "germanycount", "romaniacount", "swedencount", "denmarkcount", "chinacount" };

        public LiveChart()
        {
            this.Size = new Size(800, 400);
            this.DoubleBuffered = true;

            // Initialize Timer
            updateTimer = new Timer();
            updateTimer.Interval = 1000; // Slower animation (1 second update interval)
            updateTimer.Tick += UpdateChart;

            // Initialize chart data array and counts array
            chartData = new int[maxDataPoints];
            counts = new int[labels.Length];

            // Start the timer
            updateTimer.Start();
        }

        private void UpdateChart(object sender, EventArgs e)
        {
            // Update counts for each label
            foreach (string label in labels)
            {
                // Replace the next line with actual logic to read count for the label
                // Example: counts[i] = GetLabelCount(label);
                counts[Array.IndexOf(labels, label)] = new Random().Next(0, this.Height);
            }

            // Update chart data with the sum of counts for visualization
            int sumCounts = 0;
            for (int i = 0; i < counts.Length; i++)
            {
                sumCounts += counts[i];
            }

            chartData[dataIndex] = sumCounts;

            // Move index
            dataIndex = (dataIndex + 1) % maxDataPoints;

            // Redraw the control
            this.Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            using (var brush = new SolidBrush(Color.FromArgb(77, 86, 99)))
            {
                Graphics g = e.Graphics;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                for (int i = 0; i < chartData.Length - 1; i++)
                {
                    int x1 = i * (this.Width / maxDataPoints);
                    int y1 = this.Height - chartData[i];
                    int x2 = (i + 1) * (this.Width / maxDataPoints);
                    int y2 = this.Height - chartData[(i + 1) % maxDataPoints];

                    g.DrawLine(new Pen(brush, 2), x1, y1, x2, y2); // Increase pen width
                }
            }
        }
    }
}
