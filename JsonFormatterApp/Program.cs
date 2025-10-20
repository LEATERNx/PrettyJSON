using System;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;

namespace JsonFormatterApp
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }

    public class MainForm : Form
    {
        private RichTextBox inputTextBox = new RichTextBox();
        private RichTextBox outputTextBox = new RichTextBox();
        private Label statsLabel = new Label();
        private Label errorLabel = new Label();
        private Button formatBtn = new Button();
        private Button minifyBtn = new Button();
        private Button copyBtn = new Button();
        private Button downloadBtn = new Button();

        public MainForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "🎨 JSON Formatter";
            this.Size = new Size(925, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Font = new Font("Segoe UI", 9);
            this.BackColor = Color.FromArgb(15, 23, 42);
            this.ForeColor = Color.White;

            // Header Panel
            Panel headerPanel = new Panel();
            headerPanel.Dock = DockStyle.Top;
            headerPanel.Height = 51;
            headerPanel.BackColor = Color.FromArgb(30, 41, 59);

            Label titleLabel = new Label();
            titleLabel.Text = "🎨 JSON Formatter";
            titleLabel.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            titleLabel.ForeColor = Color.White;
            titleLabel.Location = new Point(15, 8);
            titleLabel.Width = 200;
            titleLabel.Height = 25;

            Label descLabel = new Label();
            descLabel.Text = "Format, validate and optimize your JSON data ✨";
            descLabel.Font = new Font("Segoe UI", 8);
            descLabel.ForeColor = Color.FromArgb(148, 163, 184);
            descLabel.Location = new Point(15, 35);
            descLabel.Width = 400;
            descLabel.Height = 12;

            Label creditLabel = new Label();
            creditLabel.Text = "built by: leatern";
            creditLabel.Font = new Font("Segoe UI", 9, FontStyle.Italic);
            creditLabel.ForeColor = Color.FromArgb(100, 200, 255);
            creditLabel.Location = new Point(700, 12);
            creditLabel.Width = 200;
            creditLabel.Height = 25;
            creditLabel.TextAlign = ContentAlignment.MiddleRight;

            headerPanel.Controls.Add(titleLabel);
            headerPanel.Controls.Add(descLabel);
            headerPanel.Controls.Add(creditLabel);

            // Main Container
            Panel mainPanel = new Panel();
            mainPanel.Dock = DockStyle.Fill;
            mainPanel.BackColor = Color.FromArgb(15, 23, 42);
            mainPanel.Padding = new Padding(10, 10, 10, 10);

            // Input Section
            Panel inputPanel = new Panel();
            inputPanel.Width = 440;
            inputPanel.Height = 400;
            inputPanel.Location = new Point(10, 50);
            inputPanel.BorderStyle = BorderStyle.FixedSingle;
            inputPanel.BackColor = Color.FromArgb(30, 41, 59);

            Panel inputHeaderPanel = new Panel();
            inputHeaderPanel.Dock = DockStyle.Top;
            inputHeaderPanel.Height = 35;
            inputHeaderPanel.BackColor = Color.FromArgb(99, 102, 241);

            Label inputTitleLabel = new Label();
            inputTitleLabel.Text = "📥 Input";
            inputTitleLabel.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            inputTitleLabel.ForeColor = Color.White;
            inputTitleLabel.Location = new Point(8, 8);
            inputTitleLabel.Width = 150;

            Button loadFileBtn = new Button();
            loadFileBtn.Text = "Load";
            loadFileBtn.Location = new Point(305, 6);
            loadFileBtn.Width = 60;
            loadFileBtn.Height = 22;
            loadFileBtn.BackColor = Color.FromArgb(79, 70, 229);
            loadFileBtn.ForeColor = Color.White;
            loadFileBtn.FlatStyle = FlatStyle.Flat;
            loadFileBtn.Font = new Font("Segoe UI", 8);
            loadFileBtn.Click += new EventHandler(LoadFile_Click);

            Button exampleBtn = new Button();
            exampleBtn.Text = "Example";
            exampleBtn.Location = new Point(370, 6);
            exampleBtn.Width = 60;
            exampleBtn.Height = 22;
            exampleBtn.BackColor = Color.FromArgb(79, 70, 229);
            exampleBtn.ForeColor = Color.White;
            exampleBtn.FlatStyle = FlatStyle.Flat;
            exampleBtn.Font = new Font("Segoe UI", 8);
            exampleBtn.Click += new EventHandler(LoadExample_Click);

            inputHeaderPanel.Controls.Add(inputTitleLabel);
            inputHeaderPanel.Controls.Add(loadFileBtn);
            inputHeaderPanel.Controls.Add(exampleBtn);

            inputTextBox = new RichTextBox();
            inputTextBox.Location = new Point(0, 35);
            inputTextBox.Width = 438;
            inputTextBox.Height = 280;
            inputTextBox.BackColor = Color.FromArgb(15, 23, 42);
            inputTextBox.ForeColor = Color.FromArgb(226, 232, 240);
            inputTextBox.Font = new Font("Consolas", 9);
            inputTextBox.BorderStyle = BorderStyle.None;
            inputTextBox.Padding = new Padding(8);
            inputTextBox.TextChanged += new EventHandler(Input_TextChanged);

            formatBtn = new Button();
            formatBtn.Text = "✨ Format";
            formatBtn.Location = new Point(8, 340);
            formatBtn.Width = 210;
            formatBtn.Height = 35;
            formatBtn.BackColor = Color.FromArgb(34, 197, 94);
            formatBtn.ForeColor = Color.White;
            formatBtn.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            formatBtn.FlatStyle = FlatStyle.Flat;
            formatBtn.Click += new EventHandler(FormatJson_Click);

            minifyBtn = new Button();
            minifyBtn.Text = "📦 Minify";
            minifyBtn.Location = new Point(222, 340);
            minifyBtn.Width = 210;
            minifyBtn.Height = 35;
            minifyBtn.BackColor = Color.FromArgb(6, 182, 212);
            minifyBtn.ForeColor = Color.White;
            minifyBtn.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            minifyBtn.FlatStyle = FlatStyle.Flat;
            minifyBtn.Click += new EventHandler(MinifyJson_Click);

            inputPanel.Controls.Add(inputHeaderPanel);
            inputPanel.Controls.Add(inputTextBox);
            inputPanel.Controls.Add(formatBtn);
            inputPanel.Controls.Add(minifyBtn);

            // Output Section
            Panel outputPanel = new Panel();
            outputPanel.Width = 440;
            outputPanel.Height = 400;
            outputPanel.Location = new Point(460, 50);
            outputPanel.BorderStyle = BorderStyle.FixedSingle;
            outputPanel.BackColor = Color.FromArgb(30, 41, 59);

            Panel outputHeaderPanel = new Panel();
            outputHeaderPanel.Dock = DockStyle.Top;
            outputHeaderPanel.Height = 35;
            outputHeaderPanel.BackColor = Color.FromArgb(16, 185, 129);

            Label outputTitleLabel = new Label();
            outputTitleLabel.Text = "✅ Output";
            outputTitleLabel.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            outputTitleLabel.ForeColor = Color.White;
            outputTitleLabel.Location = new Point(8, 8);
            outputTitleLabel.Width = 150;

            copyBtn = new Button();
            copyBtn.Text = "📋 Copy";
            copyBtn.Location = new Point(305, 6);
            copyBtn.Width = 60;
            copyBtn.Height = 22;
            copyBtn.BackColor = Color.FromArgb(30, 144, 255);
            copyBtn.ForeColor = Color.White;
            copyBtn.FlatStyle = FlatStyle.Flat;
            copyBtn.Font = new Font("Segoe UI", 8);
            copyBtn.Enabled = false;
            copyBtn.Click += new EventHandler(CopyToClipboard_Click);

            downloadBtn = new Button();
            downloadBtn.Text = "💾 Save";
            downloadBtn.Location = new Point(370, 6);
            downloadBtn.Width = 60;
            downloadBtn.Height = 22;
            downloadBtn.BackColor = Color.FromArgb(30, 144, 255);
            downloadBtn.ForeColor = Color.White;
            downloadBtn.FlatStyle = FlatStyle.Flat;
            downloadBtn.Font = new Font("Segoe UI", 8);
            downloadBtn.Enabled = false;
            downloadBtn.Click += new EventHandler(DownloadJson_Click);

            outputHeaderPanel.Controls.Add(outputTitleLabel);
            outputHeaderPanel.Controls.Add(copyBtn);
            outputHeaderPanel.Controls.Add(downloadBtn);

            outputTextBox = new RichTextBox();
            outputTextBox.Location = new Point(0, 35);
            outputTextBox.Width = 438;
            outputTextBox.Height = 280;
            outputTextBox.BackColor = Color.FromArgb(15, 23, 42);
            outputTextBox.ForeColor = Color.FromArgb(226, 232, 240);
            outputTextBox.Font = new Font("Consolas", 9);
            outputTextBox.BorderStyle = BorderStyle.None;
            outputTextBox.Padding = new Padding(8);
            outputTextBox.ReadOnly = true;

            errorLabel = new Label();
            errorLabel.Location = new Point(8, 35);
            errorLabel.Width = 422;
            errorLabel.Height = 280;
            errorLabel.ForeColor = Color.FromArgb(248, 113, 113);
            errorLabel.Font = new Font("Segoe UI", 10);
            errorLabel.BackColor = Color.FromArgb(15, 23, 42);
            errorLabel.Visible = false;
            errorLabel.Padding = new Padding(15);
            errorLabel.Text = "❌ Invalid JSON!";
            errorLabel.TextAlign = ContentAlignment.MiddleCenter;

            statsLabel = new Label();
            statsLabel.Location = new Point(8, 339);
            statsLabel.Width = 422;
            statsLabel.Height = 32;
            statsLabel.ForeColor = Color.FromArgb(34, 211, 238);
            statsLabel.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            statsLabel.BackColor = Color.FromArgb(30, 41, 59);
            statsLabel.TextAlign = ContentAlignment.MiddleCenter;
            statsLabel.Text = "📊 Keys: 0 | Lines: 0 | Size: 0 KB";

            outputPanel.Controls.Add(outputHeaderPanel);
            outputPanel.Controls.Add(outputTextBox);
            outputPanel.Controls.Add(errorLabel);
            outputPanel.Controls.Add(statsLabel);

            mainPanel.Controls.Add(inputPanel);
            mainPanel.Controls.Add(outputPanel);

            this.Controls.Add(mainPanel);
            this.Controls.Add(headerPanel);
        }

        private void Input_TextChanged(object sender, EventArgs e)
        {
        }

        private void LoadFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "JSON Files|*.json|All Files|*.*";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    inputTextBox.Text = File.ReadAllText(ofd.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void LoadExample_Click(object sender, EventArgs e)
        {
            string example = @"{""name"":""John Smith"",""age"":28,""city"":""New York"",""email"":""john@example.com"",""hobbies"":[""coding"",""gaming"",""reading""],""job"":{""title"":""Frontend Developer"",""company"":""Tech Corp"",""years"":5,""skills"":[""React"",""JavaScript"",""TypeScript""]},""social"":{""github"":""johnsmith"",""twitter"":""@john""}}";
            inputTextBox.Text = example;
        }

        private void FormatJson_Click(object sender, EventArgs e)
        {
            try
            {
                var parsed = JsonDocument.Parse(inputTextBox.Text);
                var formatted = JsonSerializer.Serialize(parsed, new JsonSerializerOptions
                {
                    WriteIndented = true,
                    Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                });

                outputTextBox.Text = formatted;
                outputTextBox.Visible = true;
                errorLabel.Visible = false;

                var stats = CountStats(formatted, parsed);
                statsLabel.Text = $"📊 Keys: {stats.keys} | Lines: {stats.lines} | Size: {stats.size} KB";

                copyBtn.Enabled = true;
                downloadBtn.Enabled = true;
            }
            catch (JsonException ex)
            {
                ShowError($"Invalid JSON: {ex.Message}");
            }
        }

        private void MinifyJson_Click(object sender, EventArgs e)
        {
            try
            {
                var parsed = JsonDocument.Parse(inputTextBox.Text);
                var minified = JsonSerializer.Serialize(parsed);

                outputTextBox.Text = minified;
                outputTextBox.Visible = true;
                errorLabel.Visible = false;

                var stats = CountStats(minified, parsed);
                statsLabel.Text = $"📊 Keys: {stats.keys} | Lines: 1 | Size: {stats.size} KB";

                copyBtn.Enabled = true;
                downloadBtn.Enabled = true;
            }
            catch (JsonException ex)
            {
                ShowError($"Invalid JSON: {ex.Message}");
            }
        }

        private void CopyToClipboard_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(outputTextBox.Text))
            {
                Clipboard.SetText(outputTextBox.Text);
                copyBtn.Text = "✅ Copied!";
                System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
                timer.Interval = 2000;
                timer.Tick += new EventHandler((s, ev) =>
                {
                    copyBtn.Text = "📋 Copy";
                    timer.Stop();
                });
                timer.Start();
            }
        }

        private void DownloadJson_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(outputTextBox.Text))
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "JSON Files|*.json";
                sfd.FileName = "formatted.json";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        File.WriteAllText(sfd.FileName, outputTextBox.Text);
                        MessageBox.Show("File saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void ShowError(string message)
        {
            outputTextBox.Visible = false;
            errorLabel.Visible = true;
            errorLabel.Text = message;
            statsLabel.Text = "❌ An error occurred!";
            copyBtn.Enabled = false;
            downloadBtn.Enabled = false;
        }

        private (int keys, int lines, string size) CountStats(string output, JsonDocument doc)
        {
            int keys = CountJsonKeys(doc.RootElement);
            int lines = output.Split('\n').Length;
            double sizeKb = new System.Text.UTF8Encoding().GetByteCount(output) / 1024.0;
            return (keys, lines, sizeKb.ToString("F2"));
        }

        private int CountJsonKeys(JsonElement element)
        {
            int count = 0;
            if (element.ValueKind == JsonValueKind.Object)
            {
                foreach (var prop in element.EnumerateObject())
                {
                    count++;
                    count += CountJsonKeys(prop.Value);
                }
            }
            else if (element.ValueKind == JsonValueKind.Array)
            {
                foreach (var item in element.EnumerateArray())
                {
                    count += CountJsonKeys(item);
                }
            }
            return count;
        }
    }
}