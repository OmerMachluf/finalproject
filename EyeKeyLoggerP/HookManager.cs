using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Utilities;

namespace EyeKeyLoggerP
{
    class HookManager
    {

        
        globalKeyboardHook gkh = new globalKeyboardHook();

        public HookManager(Action<object, KeyEventArgs> keyDownHook, Action<object, KeyEventArgs> keyUpHook)
        {
            setUphook();
            gkh.KeyDown += new KeyEventHandler(gkh_KeyDown);
            gkh.KeyUp += new KeyEventHandler(keyUpHook);
            gkh.hook();
        }

        void setUphook()
        {
            foreach (Keys key in Enum.GetValues(typeof(Keys)))
            {
                gkh.HookedKeys.Add(key);
            }
        }

        private void setUpKeyBoardHook()
        {
            gkh.HookedKeys.Add(Keys.LButton);
            gkh.HookedKeys.Add(Keys.RButton);
            gkh.HookedKeys.Add(Keys.Back);
            gkh.HookedKeys.Add(Keys.Space);
            gkh.HookedKeys.Add(Keys.Return);
            gkh.HookedKeys.Add(Keys.ControlKey);
            gkh.HookedKeys.Add(Keys.ShiftKey);
            gkh.HookedKeys.Add(Keys.LShiftKey);
            gkh.HookedKeys.Add(Keys.Delete);
            gkh.HookedKeys.Add(Keys.Insert);
            gkh.HookedKeys.Add(Keys.Home);
            gkh.HookedKeys.Add(Keys.End);
            gkh.HookedKeys.Add(Keys.Tab);
            gkh.HookedKeys.Add(Keys.Prior);
            gkh.HookedKeys.Add(Keys.PageDown);
            gkh.HookedKeys.Add(Keys.LWin);
            gkh.HookedKeys.Add(Keys.RWin);
            gkh.HookedKeys.Add(Keys.PageDown);
            gkh.HookedKeys.Add(Keys.Cancel);
            gkh.HookedKeys.Add(Keys.MButton);
            gkh.HookedKeys.Add(Keys.XButton1);
            gkh.HookedKeys.Add(Keys.XButton2);
            gkh.HookedKeys.Add(Keys.Back);
            gkh.HookedKeys.Add(Keys.LineFeed);
            gkh.HookedKeys.Add(Keys.Clear);
            gkh.HookedKeys.Add(Keys.Return);
            gkh.HookedKeys.Add(Keys.Enter);
            gkh.HookedKeys.Add(Keys.Menu);
            gkh.HookedKeys.Add(Keys.Pause);
            gkh.HookedKeys.Add(Keys.Capital);
            gkh.HookedKeys.Add(Keys.CapsLock);
            gkh.HookedKeys.Add(Keys.KanaMode);
            gkh.HookedKeys.Add(Keys.HanguelMode);
            gkh.HookedKeys.Add(Keys.HangulMode);
            gkh.HookedKeys.Add(Keys.JunjaMode);
            gkh.HookedKeys.Add(Keys.FinalMode);
            gkh.HookedKeys.Add(Keys.HanjaMode);
            gkh.HookedKeys.Add(Keys.KanjiMode);
            gkh.HookedKeys.Add(Keys.Escape);
            gkh.HookedKeys.Add(Keys.IMEConvert);
            gkh.HookedKeys.Add(Keys.IMENonconvert);
            gkh.HookedKeys.Add(Keys.IMEAccept);
            gkh.HookedKeys.Add(Keys.IMEAceept);
            gkh.HookedKeys.Add(Keys.IMEModeChange);
            gkh.HookedKeys.Add(Keys.PageUp);
            gkh.HookedKeys.Add(Keys.Next);
            gkh.HookedKeys.Add(Keys.End);
            gkh.HookedKeys.Add(Keys.Home);
            gkh.HookedKeys.Add(Keys.Left);
            gkh.HookedKeys.Add(Keys.Up);
            gkh.HookedKeys.Add(Keys.Right);
            gkh.HookedKeys.Add(Keys.Down);
            gkh.HookedKeys.Add(Keys.Select);
            gkh.HookedKeys.Add(Keys.Print);
            gkh.HookedKeys.Add(Keys.Execute);
            gkh.HookedKeys.Add(Keys.Snapshot);
            gkh.HookedKeys.Add(Keys.PrintScreen);
            gkh.HookedKeys.Add(Keys.Insert);
            gkh.HookedKeys.Add(Keys.Delete);
            gkh.HookedKeys.Add(Keys.Help);
            gkh.HookedKeys.Add(Keys.D0);
            gkh.HookedKeys.Add(Keys.D1);
            gkh.HookedKeys.Add(Keys.D2);
            gkh.HookedKeys.Add(Keys.D3);
            gkh.HookedKeys.Add(Keys.D4);
            gkh.HookedKeys.Add(Keys.D5);
            gkh.HookedKeys.Add(Keys.D6);
            gkh.HookedKeys.Add(Keys.D7);
            gkh.HookedKeys.Add(Keys.D8);
            gkh.HookedKeys.Add(Keys.D9);

            gkh.HookedKeys.Add(Keys.A);//65,
            gkh.HookedKeys.Add(Keys.B);//66,
            gkh.HookedKeys.Add(Keys.C);//67,
            gkh.HookedKeys.Add(Keys.D);//68,
            gkh.HookedKeys.Add(Keys.E);//69,
            gkh.HookedKeys.Add(Keys.F);//70,
            gkh.HookedKeys.Add(Keys.G);//71,
            gkh.HookedKeys.Add(Keys.H);//72,
            gkh.HookedKeys.Add(Keys.I);//73,
            gkh.HookedKeys.Add(Keys.J);//74,
            gkh.HookedKeys.Add(Keys.K);//75,
            gkh.HookedKeys.Add(Keys.L);//76,
            gkh.HookedKeys.Add(Keys.M);//77,
            gkh.HookedKeys.Add(Keys.N);//78,
            gkh.HookedKeys.Add(Keys.O);//79,
            gkh.HookedKeys.Add(Keys.P);//80,
            gkh.HookedKeys.Add(Keys.Q);//81,
            gkh.HookedKeys.Add(Keys.R);//82,
            gkh.HookedKeys.Add(Keys.S);//83,
            gkh.HookedKeys.Add(Keys.T);//84,
            gkh.HookedKeys.Add(Keys.U);//85,
            gkh.HookedKeys.Add(Keys.V);//86,
            gkh.HookedKeys.Add(Keys.W);//87,
            gkh.HookedKeys.Add(Keys.X);//88,
            gkh.HookedKeys.Add(Keys.Y);//89,
            gkh.HookedKeys.Add(Keys.Z);//90,
            gkh.HookedKeys.Add(Keys.LWin);//91,
            gkh.HookedKeys.Add(Keys.RWin);//92,
            gkh.HookedKeys.Add(Keys.Apps);//93,
            gkh.HookedKeys.Add(Keys.Sleep);//95,
            gkh.HookedKeys.Add(Keys.NumPad0);//96,
            gkh.HookedKeys.Add(Keys.NumPad1);//97,
            gkh.HookedKeys.Add(Keys.NumPad2);//98,
            gkh.HookedKeys.Add(Keys.NumPad3);//99,
            gkh.HookedKeys.Add(Keys.NumPad4);//100,
            gkh.HookedKeys.Add(Keys.NumPad5);//101,
            gkh.HookedKeys.Add(Keys.NumPad6);//102,
            gkh.HookedKeys.Add(Keys.NumPad7);//103,
            gkh.HookedKeys.Add(Keys.NumPad8);//104,
            gkh.HookedKeys.Add(Keys.NumPad9);//105,
            gkh.HookedKeys.Add(Keys.Multiply); //106,
            gkh.HookedKeys.Add(Keys.Add); //107,
            gkh.HookedKeys.Add(Keys.Separator); //108,
            gkh.HookedKeys.Add(Keys.Subtract); //109,
            gkh.HookedKeys.Add(Keys.Decimal); //110,
            gkh.HookedKeys.Add(Keys.Divide); //111,
            gkh.HookedKeys.Add(Keys.F1); //112,
            gkh.HookedKeys.Add(Keys.F2); //113,
            gkh.HookedKeys.Add(Keys.F3); //114,
            gkh.HookedKeys.Add(Keys.F4); //115,
            gkh.HookedKeys.Add(Keys.F5); //116,
            gkh.HookedKeys.Add(Keys.F6); //117,
            gkh.HookedKeys.Add(Keys.F7); //118,
            gkh.HookedKeys.Add(Keys.F8); //119,
            gkh.HookedKeys.Add(Keys.F9); //120,
            gkh.HookedKeys.Add(Keys.F10); //121,
            gkh.HookedKeys.Add(Keys.F11); //122,
            gkh.HookedKeys.Add(Keys.F12); //123,
            gkh.HookedKeys.Add(Keys.F13); //124,
            gkh.HookedKeys.Add(Keys.F14); //125,
            gkh.HookedKeys.Add(Keys.F15); //126,
            gkh.HookedKeys.Add(Keys.F16); //127,
            gkh.HookedKeys.Add(Keys.F17); //128,
            gkh.HookedKeys.Add(Keys.F18); //129,
            gkh.HookedKeys.Add(Keys.F19); //130,
            gkh.HookedKeys.Add(Keys.F20); //131,
            gkh.HookedKeys.Add(Keys.F21); //132,
            gkh.HookedKeys.Add(Keys.F22); //133,
            gkh.HookedKeys.Add(Keys.F23); //134,
            gkh.HookedKeys.Add(Keys.F24); //135,
            gkh.HookedKeys.Add(Keys.NumLock); //144,
            gkh.HookedKeys.Add(Keys.Scroll); //145,
            gkh.HookedKeys.Add(Keys.LShiftKey); //160,
            gkh.HookedKeys.Add(Keys.RShiftKey); //161,
            gkh.HookedKeys.Add(Keys.LControlKey); //162,
            gkh.HookedKeys.Add(Keys.RControlKey); //163,
            gkh.HookedKeys.Add(Keys.LMenu); //164,
            gkh.HookedKeys.Add(Keys.RMenu); //165,
            gkh.HookedKeys.Add(Keys.BrowserBack); //166,
            gkh.HookedKeys.Add(Keys.BrowserForward); //167,
            gkh.HookedKeys.Add(Keys.BrowserRefresh); //168,
            gkh.HookedKeys.Add(Keys.BrowserStop); //169,
            gkh.HookedKeys.Add(Keys.BrowserSearch); //170,
            gkh.HookedKeys.Add(Keys.BrowserFavorites); //171,
            gkh.HookedKeys.Add(Keys.BrowserHome); //172,
            gkh.HookedKeys.Add(Keys.VolumeMute); //173,
            gkh.HookedKeys.Add(Keys.VolumeDown); //174,
            gkh.HookedKeys.Add(Keys.VolumeUp); //175,
            gkh.HookedKeys.Add(Keys.MediaNextTrack); //176,
            gkh.HookedKeys.Add(Keys.MediaPreviousTrack); //177,
            gkh.HookedKeys.Add(Keys.MediaStop); //178,
            gkh.HookedKeys.Add(Keys.MediaPlayPause); //179,
            gkh.HookedKeys.Add(Keys.LaunchMail); //180,
            gkh.HookedKeys.Add(Keys.SelectMedia); //181,
            gkh.HookedKeys.Add(Keys.LaunchApplication1); //182,
            gkh.HookedKeys.Add(Keys.LaunchApplication2); //183,
            gkh.HookedKeys.Add(Keys.OemSemicolon); //186,
            gkh.HookedKeys.Add(Keys.Oem1); //186,
            gkh.HookedKeys.Add(Keys.Oemplus); //187,
            gkh.HookedKeys.Add(Keys.Oemcomma); //188,
            gkh.HookedKeys.Add(Keys.OemMinus); //189,
            gkh.HookedKeys.Add(Keys.OemPeriod); //190,
            gkh.HookedKeys.Add(Keys.OemQuestion); //191,
            gkh.HookedKeys.Add(Keys.Oem2); //191,
            gkh.HookedKeys.Add(Keys.Oemtilde); //192,
            gkh.HookedKeys.Add(Keys.Oem3); //192,
            gkh.HookedKeys.Add(Keys.OemOpenBrackets); //219,
            gkh.HookedKeys.Add(Keys.Oem4); //219,
            gkh.HookedKeys.Add(Keys.OemPipe); //220,
            gkh.HookedKeys.Add(Keys.Oem5); //220,
            gkh.HookedKeys.Add(Keys.OemCloseBrackets); //221,
            gkh.HookedKeys.Add(Keys.Oem6); //221,
            gkh.HookedKeys.Add(Keys.OemQuotes); //222,
            gkh.HookedKeys.Add(Keys.Oem7); //222,
            gkh.HookedKeys.Add(Keys.Oem8); //223,
            gkh.HookedKeys.Add(Keys.OemBackslash); //226,
            gkh.HookedKeys.Add(Keys.Oem102); //226,
            gkh.HookedKeys.Add(Keys.ProcessKey); //229,
            gkh.HookedKeys.Add(Keys.Packet); //231,
            gkh.HookedKeys.Add(Keys.Attn); //246,
            gkh.HookedKeys.Add(Keys.Crsel); //247,
            gkh.HookedKeys.Add(Keys.Exsel); //248,
            gkh.HookedKeys.Add(Keys.EraseEof); //249,
            gkh.HookedKeys.Add(Keys.Play); //250,
            gkh.HookedKeys.Add(Keys.Zoom); //251,
            gkh.HookedKeys.Add(Keys.NoName); //252,
            gkh.HookedKeys.Add(Keys.Pa1); //253,
            gkh.HookedKeys.Add(Keys.OemClear); //254,
            gkh.HookedKeys.Add(Keys.KeyCode); //65535,
            gkh.HookedKeys.Add(Keys.Shift); //65536,
            gkh.HookedKeys.Add(Keys.Control); //131072,
            gkh.HookedKeys.Add(Keys.Alt); //262144
        }

        void gkh_KeyUp(object sender, KeyEventArgs e)
        {
           // lstLog.Items.Add("Up\t" + e.KeyCode.ToString());
            e.Handled = true;
        }

        void gkh_KeyDown(object sender, KeyEventArgs e)
        {
          //  lstLog.Items.Add("Down\t" + e.KeyCode.ToString());
            e.Handled = true;
        }
    }
}
