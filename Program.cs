using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace ImageMoji
{
    static class Program
    {
        // windows form 이용 진입
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
        private TextBox keywordTextBox;
        private TextBox pathTextBox;
        private Button addButton;
        private ListBox listBox;
        private Button saveButton;
        private Button loadButton;

        private Dictionary<string, string> imageDict = new Dictionary<string, string>();

        public MainForm()
        {
            this.Text = "ImageMoji";
            this.Size = new Size(400, 300);

            Label keywordLabel = new Label() { Text = "Keyword:", Location = new Point(10, 10), AutoSize = true };
            keywordTextBox = new TextBox() { Location = new Point(70, 10), Width = 200 };

            Label pathLabel = new Label() { Text = "Image Path:", Location = new Point(10, 40), AutoSize = true };
            pathTextBox = new TextBox() { Location = new Point(70, 40), Width = 200 };

            addButton = new Button() { Text = "Add", Location = new Point(280, 25), Width = 80 };
            addButton.Click += new EventHandler(AddButton_Click);

            listBox = new ListBox() { Location = new Point(10, 70), Width = 350, Height = 150 };

            saveButton = new Button() { Text = "Save", Location = new Point(10, 230), Width = 80 };
            saveButton.Click += new EventHandler(SaveButton_Click);

            loadButton = new Button() { Text = "Load", Location = new Point(100, 230), Width = 80 };
            loadButton.Click += new EventHandler(LoadButton_Click);

            this.Controls.Add(keywordLabel);
            this.Controls.Add(keywordTextBox);
            this.Controls.Add(pathLabel);
            this.Controls.Add(pathTextBox);
            this.Controls.Add(addButton);
            this.Controls.Add(listBox);
            this.Controls.Add(saveButton);
            this.Controls.Add(loadButton);
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            string keyword = keywordTextBox.Text;
            string path = pathTextBox.Text;

            if (!string.IsNullOrEmpty(keyword) && !string.IsNullOrEmpty(path) && File.Exists(path))
            {
                imageDict[keyword] = path;
                listBox.Items.Add($"{keyword}: {path}");
                keywordTextBox.Clear();
                pathTextBox.Clear();
            }
            else
            {
                MessageBox.Show("Please enter a valid keyword and image path.");
            }
        }


        // Save Load 시 매핑 txt 불러오기. .csv로 바꿀까?
        private void SaveButton_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    using (StreamWriter writer = new StreamWriter(saveFileDialog.FileName))
                    {
                        foreach (var pair in imageDict)
                        {
                            writer.WriteLine($"{pair.Key}:{pair.Value}");
                        }
                    }
                }
            }
        }

        private void LoadButton_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    using (StreamReader reader = new StreamReader(openFileDialog.FileName))
                    {
                        imageDict.Clear();
                        listBox.Items.Clear();
                        string line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            var parts = line.Split(':');
                            if (parts.Length == 2)
                            {
                                imageDict[parts[0]] = parts[1];
                                listBox.Items.Add(line);
                            }
                        }
                    }
                }
            }
        }

        // Ctrl + Space 입력시 이미지 복사
        // TODO: HotKey로 하는 거 추가

        // KMP로 포함된거 리스트 띄울 수 있게 작업중
        // 근데 이모티콘 n개, 문자 길이 m이면?
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Control | Keys.Space))
            {
                string input = keywordTextBox.Text.Trim();
                if (imageDict.ContainsKey(input))
                {
                    CopyImageToClipboard(imageDict[input]);
                    MessageBox.Show("Image copied to clipboard.");
                    keywordTextBox.Clear();
                }
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void CopyImageToClipboard(string imagePath)
        {
            Image image = Image.FromFile(imagePath);
            Clipboard.SetImage(image);
        }
    }
}
