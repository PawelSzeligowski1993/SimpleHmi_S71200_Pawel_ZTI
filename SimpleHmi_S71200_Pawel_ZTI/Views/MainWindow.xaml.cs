using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using S7.Net;
using S7.Net.Types;

namespace SimpleHmi_S71200_Pawel_ZTI.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string text1;
        private string text2;


        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {

        }


        private void Set_level(object sender, RoutedEventArgs e)
        {
            string text1 = Set_Tank1_Level.Text;
            string text2 = Set_Tank2_Level.Text;
            int value1;
            int value2;
            value1 = int.Parse(text1);
            value2 = int.Parse(text2);


            using (var plc = new Plc(CpuType.S71200, "192.168.0.1", 0, 1))
            {
                plc.Open();
                if (value1 > 0 && value1 < 100 && value2 > 0 && value2 < 100) 
                {
                    

                    //TODO Write Value1 to DB in PLC
                    int db1DwordVariable = value1;
                    plc.Write("DB7.DBD6.0", db1DwordVariable.ConvertToUInt());
                    MessageBox.Show("Tank 1 Set "+text1+" %");

                    //TODO Write Value2 to DB in PLC
                    int db2DwordVariable = value2;
                    plc.Write("DB7.DBD10.0", db2DwordVariable.ConvertToUInt());
                    MessageBox.Show("Tank 2 Set " + text2 + " %");
                } else
                {
                    MessageBox.Show("Value1 and Value2 should be > 0 < 100");
                }
            } 
        }


    }
}
