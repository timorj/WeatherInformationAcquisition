using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using WeatherInformationAcquisition.Src;
using static WeatherInformationAcquisition.Src.ClassHelper;
using System.Reflection;

namespace WeatherInformationAcquisition.Tests
{
    public class ClassHelperTests
    {
        private static object myClass;
        [Theory]
        [InlineData("test1")]
        [InlineData("My Class nMa")]
        public static void CreateInstance_InitAClass_ReturnClassProperty(string proValue)
        {
            InitTestClass();

            string expect = proValue;

            InitPropertyValue(proValue);
            PropertyInfo s1 = myClass.GetType().GetProperties().First(g => g.Name == "S1");

            string actual = s1.GetValue(myClass).ToString();

            Assert.Equal(actual, expect);
            

        }

        private static void InitTestClass()
        {
            CustPropertyInfo cpi1 = new CustPropertyInfo("System.String", "S1");
            CustPropertyInfo cpi2 = new CustPropertyInfo("System.String", "S2");
            List<CustPropertyInfo> lcpi = new List<CustPropertyInfo>{ cpi1, cpi2 };

            myClass = CreatInstance("MyClass", lcpi);

        }
        private static void InitPropertyValue(string proValue)
        {
            Type t = myClass.GetType();

            var pros = t.GetProperties();

            PropertyInfo pro = pros.First(g => g.Name == "S1");

            pro.SetValue(myClass, proValue);
        }
    }
}
