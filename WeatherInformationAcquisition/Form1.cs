using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeatherInformationAcquisition.DataManage;
using WeatherInformationAcquisition.Src;

namespace WeatherInformationAcquisition
{
    public partial class Form1 : Form
    {
        
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(WeatherRequestSystem.Param.CityID == default)
            {
                MessageBox.Show("未能找到该城市！");
                return;
            }
            //先请求
            WeatherRequestSystem.WeatherQuest();
            textBox1.Text = WeatherRequestSystem.Result.JsonData;  
            
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            string txt = textBox2.Text;
            WeatherRequestSystem.Param.City = txt;

            
            WeatherRequestSystem.CityIDRequest();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var data = WeatherRequestSystem.Result.Jobject.GetValue("data");
            var detail = data.ElementAt(0);
            var forcast = data.ElementAt(1);
            

            
        }

        string propertyName;
        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            propertyName = textBox3.Text;
        }
    }
}
