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
using WeatherInformationAcquisition.DataManage;
using WeatherInformationAcquisition.DataManage.APIS;
using WeatherInformationAcquisition.Src;

namespace WeatherInformationAcquisition
{
    public partial class Form1 : Form
    {
        private IWeatherRequest requestMethod = new NMCWeather();
        private IWeatherRequest[] methods ={ new NMCWeather(), new MojiWeather() };
        public IWeatherRequest RequestMethod
        {
            get => requestMethod; set
            {
                if (value != null)
                {
                    requestMethod = value;
                    WeatherRequestSystem.RequestMethod = requestMethod;
                }
            }
        }
        public Form1()
        {
            InitializeComponent();           
        }


        private void btnReq_Click(object sender, EventArgs e)
        {

            if (WeatherRequestSystem.Param.CityCode == default)
            {
                MessageBox.Show("未能找到该城市！");
                return;
            }
            //先请求
            WeatherRequestSystem.WeatherForcastQuest();
            tbInfo.Text = WeatherRequestSystem.Result.ForecastWeather.ToString();  

        }

        private void tbName_TextChanged(object sender, EventArgs e)
        {

            string txt = tbName.Text;
            WeatherRequestSystem.Param.City = txt;

            WeatherRequestSystem.CityIDRequest();
        }

        private void btnDoc_Click(object sender, EventArgs e)
        {
            if (WeatherRequestSystem.Result.ForecastWeather == null)
            {
                MessageBox.Show("未请求到任何数据");
                return;
            }

            FolderBrowserDialog dlg = new FolderBrowserDialog();
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            //打开的文件夹浏览对话框上的描述  
            dialog.Description = "请选择一个文件夹";
            //是否显示对话框左下角 新建文件夹 按钮，默认为 true  
            dialog.ShowNewFolderButton = false;

            //按下确定选择的按钮  
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                //记录选中的目录  
                string dir = dialog.SelectedPath;
                WeatherRequestSystem.Result.ExportDataAsDocx(dir);
            }

            MessageBox.Show("保存成功！");
        }

        private void btnTxt_Click(object sender, EventArgs e)
        {
            if(WeatherRequestSystem.Result.ForecastWeather == null)
            {
                MessageBox.Show("未请求到任何数据");
                return;
            }

            FolderBrowserDialog dlg = new FolderBrowserDialog();
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            //打开的文件夹浏览对话框上的描述  
            dialog.Description = "请选择一个文件夹";
            //是否显示对话框左下角 新建文件夹 按钮，默认为 true  
            dialog.ShowNewFolderButton = false;
           
            //按下确定选择的按钮  
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                //记录选中的目录  
                string dir = dialog.SelectedPath;
                WeatherRequestSystem.Result.ExportDataAsTxt(dir);
            }

            MessageBox.Show("保存成功！");
        }

        private void btnXml_Click(object sender, EventArgs e)
        {
            if (WeatherRequestSystem.Result.ForecastWeather == null)
            {
                MessageBox.Show("未请求到任何数据");
                return;
            }

            FolderBrowserDialog dlg = new FolderBrowserDialog();
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            //打开的文件夹浏览对话框上的描述  
            dialog.Description = "请选择一个文件夹";
            //是否显示对话框左下角 新建文件夹 按钮，默认为 true  
            dialog.ShowNewFolderButton = false;

            //按下确定选择的按钮  
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                //记录选中的目录  
                string dir = dialog.SelectedPath;
                WeatherRequestSystem.Result.ExportDataAsXML(dir);
            }

            MessageBox.Show("保存成功！");
        }

        private void button1_Click(object sender, EventArgs e)
        {
      
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBox1.Items.AddRange(methods);
            comboBox1.SelectedIndex = 0;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            RequestMethod = (IWeatherRequest)comboBox1.SelectedItem;
        }
    }
}
