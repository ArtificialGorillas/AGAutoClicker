using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace AGAutoClicker
{
    public partial class MainForm : Form
    {
        private System.Windows.Forms.Timer clickTimer;
        private int interval;
        private bool isClicking;
        private Keys startKey = Keys.F6;
        private GlobalKeyboardHook globalHook;

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);

        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;

        public MainForm()
        {
            InitializeComponent();
            clickTimer = new System.Windows.Forms.Timer();
            clickTimer.Tick += new EventHandler(ClickTimer_Tick);
            btnStop.Enabled = false;
            textBoxKeybind.Text = startKey.ToString();

            globalHook = new GlobalKeyboardHook();
            globalHook.KeyDown += GlobalHook_KeyDown;
        }

        private void GlobalHook_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == startKey)
            {
                if (!isClicking)
                {
                    btnStart_Click(null, null);
                }
                else
                {
                    btnStop_Click(null, null);
                }
                e.Handled = true; 
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            interval = (int)numericUpDownInterval.Value;
            clickTimer.Interval = interval;
            isClicking = true;
            clickTimer.Start();
            btnStart.Enabled = false;
            btnStop.Enabled = true;
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            isClicking = false;
            clickTimer.Stop();
            btnStart.Enabled = true;
            btnStop.Enabled = false;
        }

        private void ClickTimer_Tick(object sender, EventArgs e)
        {
            mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
        }
    }
}
