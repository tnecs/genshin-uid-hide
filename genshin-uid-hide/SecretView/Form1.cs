using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace SecretView
{
    public partial class Form1 : Form
    {
        HatchBrush H_brush;

        [DllImport("user32.dll", SetLastError = true)]
        static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        static extern int SetWindowLong(IntPtr hWnd, int nIndex, uint dwNewLong);

        public Form1()
        {
            InitializeComponent();

            this.TopMost = true;
            this.ControlBox = false;
            //this.StartPosition = FormStartPosition.CenterScreen;
            this.Opacity = 1;
            this.ShowInTaskbar = false;
			//this.의 시작위치 5,5 로 설정할수 있게 해줘
			this.StartPosition = FormStartPosition.Manual;
			this.Location = new Point(1688, 1054);
			Array obj = Enum.GetValues(typeof(HatchStyle));
            HatchStyle h_Style = (HatchStyle)obj.GetValue(7);//0~10
            H_brush = new HatchBrush(h_Style, Color.Gray, Color.Gray);

            timer1.Start();
        }

        protected override CreateParams CreateParams // alt+tab에 표시 없애기
        {
            get
            {
                CreateParams pm = base.CreateParams;
                pm.ExStyle |= 0x80;
                return pm;
            }
        }

        Font arial = new Font("arial",7);
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            if (tickCount == 0)
            {
                Graphics _graphics = this.CreateGraphics();
                //_graphics.Clear(Color.White);

                _graphics.FillRectangle(H_brush, 0, 0, this.ClientSize.Width, this.ClientSize.Height);
				
				_graphics.Dispose();
            }
            else
            {
                Graphics _graphics = this.CreateGraphics();

                _graphics.DrawString("창 위치와 크기를 15초 내로 조절하세요.\n이후 프로그램 종료 때까지 위치와 크기를 변경할 수 없습니다.\n종료는 우측하단 트레이 아이콘을 클릭하면 이 프로그램은 종료.",arial,Brushes.Black,0,0);

                _graphics.Dispose();
            }
        }

        int tickCount = 15;
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (tickCount > 0)
            {
                this.Text = string.Format("{0}초 후 화면에 완전 고정됩니다.", tickCount);
                tickCount--;
            }
            else
            {
                timer1.Stop();
                timer2.Start();
                this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
                this.Text = "고정됨 (우측하단 아이콘으로 종료)";
                this.Opacity = 1;
                this.Invalidate();

                //창 통과 클릭 활성화
                int initialStyle = GetWindowLong(this.Handle, -20);
                SetWindowLong(this.Handle, -20, (uint)(initialStyle | 0x80000 | 0x20));
            }
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                this.WindowState = FormWindowState.Normal;
                this.Activate();
                System.Diagnostics.Debug.WriteLine("mini");
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            if (this.RectangleToScreen(this.ClientRectangle).Contains(System.Windows.Forms.Control.MousePosition) == true)
            {//투명도조절
                //System.Diagnostics.Debug.WriteLine("enter");
                //this.Opacity = 0.1;
				this.Opacity = 1;

			}
            else
            {
                this.Opacity =1;
            }
        }

        private void notifyIcon1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
