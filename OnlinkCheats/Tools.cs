using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Microsoft.CSharp.RuntimeBinder;
using Brush = System.Windows.Media.Brush;
using Color = System.Windows.Media.Color;

namespace Utils
{
    class Tools
    {
        public static Process NewProcessWithPsi(ProcessStartInfo PSI, bool CreateNoWindow = true,
            bool UseShellExecute = false)
        {
            Process process = new Process();
            PSI.CreateNoWindow = CreateNoWindow;
            PSI.UseShellExecute = UseShellExecute;
            process.StartInfo = PSI;

            return process;
        }


        public static HttpStatusCode GetHeaders(string url)
        {

            HttpStatusCode result = default(HttpStatusCode);

            var request = HttpWebRequest.Create(url);

            request.Method = "HEAD";
            using (var response = request.GetResponse() as HttpWebResponse)
            {
                if (response != null)
                {
                    result = response.StatusCode;
                    response.Close();
                }
            }

            return result;


        }

        public static string CalculateMD5Hash(string input)

        {

            // step 1, calculate MD5 hash from input

            MD5 md5 = System.Security.Cryptography.MD5.Create();

            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);

            byte[] hash = md5.ComputeHash(inputBytes);


            // step 2, convert byte array to hex string

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < hash.Length; i++)

            {

                sb.Append(hash[i].ToString("X2"));

            }

            return sb.ToString().ToLower();

        }


    }

    class ColorChanger
    {

        private static Thread thread;
        private volatile static bool Changing = false;
        private volatile static bool ShouldRun = true;
        public ColorChanger(Brush bg, Dispatcher dispatcher)
        {

            Color defaultColor = ((SolidColorBrush)bg).Color;
            thread = new Thread(() => ThreadSys(bg, dispatcher, defaultColor));
        }

        private static void ThreadSys(Brush bg, Dispatcher dispatcher, Color defaultColor)
        {
            try
            {
                while (ShouldRun)
                {

                    while (Changing)
                    {
                        dispatcher.Invoke(() =>
                        {
                            Random random = new Random();
                            ColorAnimation animation = new ColorAnimation();
                            animation.From = ((SolidColorBrush) bg).Color;
                            animation.To = Color.FromRgb((byte) random.Next(0, 255), (byte) random.Next(0, 255),
                                (byte) random.Next(0, 255));
                            animation.Duration = new Duration(TimeSpan.FromSeconds(1));
                            bg.BeginAnimation(SolidColorBrush.ColorProperty, animation);
                        });
                        Thread.Sleep(1000);
                    }
                    if (!Changing)
                    {
                        dispatcher.Invoke(() =>
                        {
                            // Random random = new Random();
                            ColorAnimation animation = new ColorAnimation();
                            animation.From = ((SolidColorBrush) bg).Color;
                            animation.To = defaultColor;
                            animation.Duration = new Duration(TimeSpan.FromSeconds(1));
                            bg.BeginAnimation(SolidColorBrush.ColorProperty, animation);
                        });
                    }
                }
            }
            catch (TaskCanceledException) { }
        }

        public void Start()
        {

            Changing = true;
            try
            {
                thread.Start();
            }
            catch (ThreadStateException) { }
        }

        public void Stop()
        {
            Changing = false;


        }

        public bool isChanging()
        {
            return Changing;
        }

        public bool isAlive()
        {
            return ShouldRun;
        }


        public void Kill()
        {
            Changing = false;
            ShouldRun = false;
        }
    }

    class WindowUtil
    {
        [DllImport("user32.dll")]
        private static extern bool ShowWindowAsync(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        private static extern bool CloseWindow(IntPtr hWnd);

        private IntPtr hWnd = IntPtr.Zero;

        public enum WindowSize
        {
            Normal = 1,
            Minimized = 2,
            Maximized = 3
        }

        public WindowUtil(string Title)
        {
            Process[] processes = Process.GetProcesses();
            foreach (Process pList in processes)
            {
                if (pList.MainWindowTitle.Contains(Title))
                {
                    hWnd = pList.MainWindowHandle;
                }
            }
            if (hWnd == IntPtr.Zero)
            {
                throw new Exception("Not Found");
            }
        }

        public void setVisibility(WindowSize size)
        {
            bool success = ShowWindowAsync(hWnd, (int)size);
            if (!success)
            {
                throw new Exception("Win32API said that there was an error!");
            }
        }

        public void Close()
        {
            bool success = CloseWindow(hWnd);
            if (!success)
            {
                throw new Exception("Win32API said that there was an error!");
            }

        }

        public void Kill()
        {
            SendMessage(hWnd, 0x0010, IntPtr.Zero, IntPtr.Zero);
        }

    }

    static class Kiosk
    {
        private static bool isEnabledInternal = false;

        public static bool isEnabled()
        {
            return isEnabledInternal;
        }

        public static void EnableKiosk()
        {
            int killedCount = 0;
            if (!isEnabled())
            {
                Process[] explorerInstances = Process.GetProcessesByName("explorer.exe");
                foreach (Process explorer in explorerInstances)
                {
                    explorer.Kill();
                    killedCount++;
                }
                if (killedCount <= 0)
                {
                    //Using alternative metheod!
                    ProcessStartInfo cfg = new ProcessStartInfo("taskkill.exe", "/F /IM \"explorer.exe\"");
                    cfg.CreateNoWindow = true;
                    cfg.UseShellExecute = true;
                    Process alternativeKiller = new Process();
                    alternativeKiller.StartInfo = cfg;
                    alternativeKiller.Start();

                }
                isEnabledInternal = true;
            }

        }

        public static void DisableKiosk()
        {

            if (isEnabled())
            {
                System.Diagnostics.Process process = new System.Diagnostics.Process();
                System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
                startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                startInfo.FileName = Path.Combine(Environment.GetEnvironmentVariable("windir"), "explorer.exe"); ;

                process.StartInfo = startInfo;
                process.Start();
                isEnabledInternal = false;
            }
        }


    }




}
