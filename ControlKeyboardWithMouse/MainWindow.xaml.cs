using System;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Windows.Forms;
using System.Drawing;
using System.Windows;
using System.Runtime.InteropServices;

namespace ControlKeyboardWithMouse
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

         static int screenWidth = 1920;
         static int screenHeight = 1080;
         static System.Drawing.Point center = new System.Drawing.Point(screenWidth/2, screenHeight/2);

         System.Drawing.Point currentPosition = new System.Drawing.Point(screenWidth/2, screenHeight/2);
         int korakHoriz = 1;
         int korakVret = 1;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            this.Dispatcher.BeginInvoke((Action)(() =>
            {
                KeyboardHook.CreateHook();
                KeyboardHook.KeyPressed += (sender, e) =>
                {
                    switch (e.KeyCode)
                    {
                        case System.Windows.Forms.Keys.NumPad8: //toogle

                            MoveUp();

                        break;

                        case System.Windows.Forms.Keys.NumPad4: //toogle

                            MoveLeft();

                            break;

                        case System.Windows.Forms.Keys.NumPad6: //toogle

                            MoveRight();

                            break;

                        case System.Windows.Forms.Keys.NumPad2: //toogle

                            MoveDown();

                            break;

                        case System.Windows.Forms.Keys.NumPad0:

                            DoMouseClick();

                            break;
                        case Keys.LControlKey:

                            SetCursor(screenWidth / 2, screenHeight / 2);
                            korakHoriz = 1;
                            korakVret = 1;

                            break;
                    }

                };

                //       KeyboardHook.DisposeHook();
            }), DispatcherPriority.ContextIdle);

        }

        private void MoveDown()
        { 
            SetCursor(System.Windows.Forms.Cursor.Position.X, (int) (System.Windows.Forms.Cursor.Position.Y + screenHeight/2 * (1.0/Math.Pow(2,korakVret))));
            korakVret++;
        }

        private void MoveRight()
        {
            SetCursor((int)(System.Windows.Forms.Cursor.Position.X + screenWidth / 2 * (1.0 / Math.Pow(2, korakHoriz))), System.Windows.Forms.Cursor.Position.Y);
            korakHoriz++;

        }

        private void MoveLeft()
        {
            SetCursor((int)(System.Windows.Forms.Cursor.Position.X - screenWidth / 2 * (1.0 / Math.Pow(2, korakHoriz))), System.Windows.Forms.Cursor.Position.Y);
            korakHoriz++;

        }

        private void MoveUp()
        {
            SetCursor(System.Windows.Forms.Cursor.Position.X, (int)(System.Windows.Forms.Cursor.Position.Y - screenHeight / 2 * (1.0 / Math.Pow(2, korakVret))));
            korakVret++;
        }

        private void SetCursor(int x,int y)
        {
            System.Windows.Forms.Cursor.Position = new System.Drawing.Point(x,y);
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);
        //Mouse actions
        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;
        private const int MOUSEEVENTF_RIGHTDOWN = 0x08;
        private const int MOUSEEVENTF_RIGHTUP = 0x10;

        public void DoMouseClick()
        {
            //Call the imported function with the cursor's current position
            uint X = (uint)System.Windows.Forms.Cursor.Position.X;
            uint Y = (uint)System.Windows.Forms.Cursor.Position.Y;
            mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, X, Y, 0, 0);
        }
    }
}
