using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace AndroidGeneratorUI
{
    public partial class ConfigEditor : Form
    {

        Config config;
        public ConfigEditor()
        {
            InitializeComponent();
        }

        private void BtnColorPrimary_Click(object sender, EventArgs e)
        {
            var color = ShowColorDialog();
            if (color == "")
                return;
            txtColorPrimary.Text =  color ;
        }

        private String ShowColorDialog()
        {
            colorDialog1.ShowDialog();
            if (colorDialog1.Color.IsEmpty)
            {
                return "";
            }
            return "#" + (colorDialog1.Color.ToArgb() & 0x00FFFFFF).ToString("X6");
        }

        private String ShowFileDialog()
        {
            openFileDialog1.FileName = "";
            openFileDialog1.ShowDialog();
            
            return openFileDialog1.FileName;
        }

        private void BtnSecondaryColor_Click(object sender, EventArgs e)
        {
            var color = ShowColorDialog();
            if (color == "")
                return;
            txtSecondaryColor.Text = color;
        }

        private void BtnColorAccent_Click(object sender, EventArgs e)
        {
            var color = ShowColorDialog();
            if (color == "")
                return;
            txtColorAcent.Text = color;
        }

        private void BtnIcon_Click(object sender, EventArgs e)
        {
            var file = ShowFileDialog();
            if (file == "")
                return;
            txtIcon.Text = file;
        }

        private void ConfigEditor_Load(object sender, EventArgs e)
        {
            config = ReadConfig<Config>("config.xml");
            txtOrgName.Text = config.VariableList.Where(x => x.Name.ToLower() == "org_name".ToLower()).SingleOrDefault().Value;
            txtProjectName.Text = config.VariableList.Where(x => x.Name.ToLower() == "project_name".ToLower()).SingleOrDefault().Value;
            txtAppName.Text = config.VariableList.Where(x => x.Name.ToLower() == "app_name".ToLower()).SingleOrDefault().Value;
            txtAppTitle.Text = config.VariableList.Where(x => x.Name.ToLower() == "App_title".ToLower()).SingleOrDefault().Value;
            txtColorPrimary.Text = config.VariableList.Where(x => x.Name.ToLower() == "colorPrimary".ToLower()).SingleOrDefault().Value;
            txtSecondaryColor.Text = config.VariableList.Where(x => x.Name.ToLower() == "colorPrimaryDark".ToLower()).SingleOrDefault().Value;
            txtColorAcent.Text = config.VariableList.Where(x => x.Name.ToLower() == "colorAccent".ToLower()).SingleOrDefault().Value;
            txtFCMSender.Text = config.VariableList.Where(x => x.Name.ToLower() == "fcm_sender_id".ToLower()).SingleOrDefault().Value;
            txtServerClientID.Text = config.VariableList.Where(x => x.Name.ToLower() == "server_client_id".ToLower()).SingleOrDefault().Value;
            txtIcon.Text = config.CopyFileList.Where(x => x.Name.ToLower() == "icon".ToLower()).SingleOrDefault().SourcePath;
            txtBackground.Text = config.CopyFileList.Where(x => x.Name.ToLower() == "background".ToLower()).SingleOrDefault().SourcePath;
            txtGoogleService.Text = config.CopyFileList.Where(x => x.Name.ToLower() == "google_service".ToLower()).SingleOrDefault().SourcePath;
            txtFacebookId.Text = config.VariableList.Where(x => x.Name.ToLower() == "facebook_id".ToLower()).SingleOrDefault().Value;

        }

        public T ReadConfig<T>(string path)
        {
            var serializer = new XmlSerializer(typeof(T));
            var fileConfig = new StreamReader(path);
            T a = (T)serializer.Deserialize(fileConfig);
            fileConfig.Close();
            return a;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {

            config.VariableList.Where(x => x.Name.ToLower() == "org_name".ToLower()).SingleOrDefault().Value = txtOrgName.Text;
            config.VariableList.Where(x => x.Name.ToLower() == "project_name".ToLower()).SingleOrDefault().Value = txtProjectName.Text;
            config.VariableList.Where(x => x.Name.ToLower() == "app_name".ToLower()).SingleOrDefault().Value = txtAppName.Text;
            config.VariableList.Where(x => x.Name.ToLower() == "App_title".ToLower()).SingleOrDefault().Value = txtAppTitle.Text;
            config.VariableList.Where(x => x.Name.ToLower() == "colorPrimary".ToLower()).SingleOrDefault().Value = txtColorPrimary.Text;
            config.VariableList.Where(x => x.Name.ToLower() == "colorPrimaryDark".ToLower()).SingleOrDefault().Value = txtSecondaryColor.Text;
            config.VariableList.Where(x => x.Name.ToLower() == "colorAccent".ToLower()).SingleOrDefault().Value = txtColorAcent.Text;
            config.VariableList.Where(x => x.Name.ToLower() == "facebook_id".ToLower()).SingleOrDefault().Value = txtFacebookId.Text;
            config.VariableList.Where(x => x.Name.ToLower() == "fcm_sender_id".ToLower()).SingleOrDefault().Value = txtFCMSender.Text;
            config.VariableList.Where(x => x.Name.ToLower() == "server_client_id".ToLower()).SingleOrDefault().Value = txtServerClientID.Text;
            config.CopyFileList.Where(x => x.Name.ToLower() == "icon".ToLower()).SingleOrDefault().SourcePath = txtIcon.Text;
            config.CopyFileList.Where(x => x.Name.ToLower() == "background".ToLower()).SingleOrDefault().SourcePath = txtBackground.Text;
            config.CopyFileList.Where(x => x.Name.ToLower() == "google_service".ToLower()).SingleOrDefault().SourcePath = txtGoogleService.Text;
            

            WriteConfig(config, "config.xml");
            MessageBox.Show("Save Success","Info");

        }

        public void WriteConfig<T>(T config, string path)
        {
            var serializer = new XmlSerializer(typeof(T));
            var fileConfig = new StreamWriter(path);
            serializer.Serialize(fileConfig, config);
            fileConfig.Close();
            Console.WriteLine($"{path} Created ");
        }

        private void BtnGenerate_Click(object sender, EventArgs e)
        {           
                CallProgramExternal("run.bat", "");            
        }

        public void CallProgramExternal(string programName, String argument, String workingDirectory = "")
        {
            Process build = new Process();
            build.StartInfo.WorkingDirectory = workingDirectory;
            build.StartInfo.Arguments = argument;
            build.StartInfo.FileName = programName;
            build.StartInfo.UseShellExecute = true;
            build.Start();
            build.WaitForExit();

        }

        private void BtnInitate_Click(object sender, EventArgs e)
        { 
            CallProgramExternal("init_git.bat", "", config.OutputPath);
            CallProgramExternal("create_jks.bat", "", config.OutputPath);
            CallProgramExternal("build.bat", "", config.OutputPath);
        }

        private void BtnBuild_Click(object sender, EventArgs e)
        {
            CallProgramExternal("build.bat", "", config.OutputPath);
        }

        private void BtnCreateKeyStore_Click(object sender, EventArgs e)
        {
            CallProgramExternal("create_jks.bat", "", config.OutputPath);
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            CallProgramExternal("open_apk.bat", "", config.OutputPath);
        }

        private void MenuSetting_Click(object sender, EventArgs e)
        {
            var setting = new Setting();
            setting.ShowDialog();
            config = setting.config;
        }

        private void MenuExit_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            var file = ShowFileDialog();
            if (file == "")
                return;
            txtBackground.Text = file;
        }

        private void BtnGoogleService_Click(object sender, EventArgs e)
        {
            var file = ShowFileDialog();
            if (file == "")
                return;
            txtGoogleService.Text = file;
        }
    }
}
