using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace AndroidGeneratorUI
{
    public partial class Setting : Form
    {
        public Setting()
        {
            InitializeComponent();
        }

        public Config config { get; set; }
        private void BtnSave_Click(object sender, EventArgs e)
        {
            config.OutputPath = txtOutput.Text;
            config.TemplatePath = txtTemplate.Text;
            WriteConfig<Config>(config,"config.xml");
            Close();
        }

        public T ReadConfig<T>(string path)
        {
            var serializer = new XmlSerializer(typeof(T));
            var fileConfig = new StreamReader(path);
            T a = (T)serializer.Deserialize(fileConfig);
            fileConfig.Close();
            return a;
        }

        public void WriteConfig<T>(T config, string path)
        {
            var serializer = new XmlSerializer(typeof(T));
            var fileConfig = new StreamWriter(path);
            serializer.Serialize(fileConfig, config);
            fileConfig.Close();
            Console.WriteLine($"{path} Created ");
        }

        private void Setting_Load(object sender, EventArgs e)
        {
            config = ReadConfig<Config>("Config.xml");
            txtOutput.Text = config.OutputPath;
            txtTemplate.Text = config.TemplatePath;
        }

        private void BtnTemplate_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.ShowDialog();
            if (folderBrowserDialog1.SelectedPath != "")
            {
                txtTemplate.Text = folderBrowserDialog1.SelectedPath;
            }
          
        }

        private void BtnOutput_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.ShowDialog();
            if (folderBrowserDialog1.SelectedPath != "")
            {
                txtOutput.Text = folderBrowserDialog1.SelectedPath;
            }
        }
    }
}
