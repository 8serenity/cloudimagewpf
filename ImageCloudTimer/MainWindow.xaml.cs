using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
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

namespace ImageCloudTimer
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            int num = 0;
            TimerCallback tm = new TimerCallback(GetImage);
            Timer timer = new Timer(tm, num, 0, 2000);
        }

        private void GetImage(object obj)
        {

            byte[] buffer = new byte[16];

            using (FileStream stream = File.Open(@"C:\testC\fileInfo.txt", FileMode.OpenOrCreate))
            {
                stream.Read(buffer, 0, 16);
            }

            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(@"C:\testC\image1.jpg"))
                {
                    var result = md5.ComputeHash(stream);

                    for (int i = 0; i < result.Length; i++)
                    {
                        if (result[i] != buffer[i])
                        {
                            using (WebClient webClient = new WebClient())
                            {
                                webClient.DownloadFile("https://www.dropbox.com/s/tj36momzb5ozhzu/global-franchise-blue-header.jpg?dl=1", @"C:\testC\image1.jpg");
                            }
                            using (FileStream streamWrite = File.OpenWrite(@"C:\testC\fileInfo.txt"))
                            {
                                streamWrite.Write(result, 0, result.Length);
                            }

                            File.Copy(@"C:\testC\image1.jpg", @"C:\testC\image2.jpg", true);
                        }
                    }
                }
            }

            Dispatcher.Invoke(() =>
            {
                imageInUI.Source = new BitmapImage(new Uri(@"C:\testC\image2.jpg"));
            });
        }
    }
}
