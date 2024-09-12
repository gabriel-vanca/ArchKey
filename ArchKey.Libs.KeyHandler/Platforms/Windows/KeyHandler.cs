using System.Diagnostics;
using System.Runtime.InteropServices;
using SharpHook;
using Windows.System;

namespace ArchKey.Libs.KeyHandler.Platforms.Windows
{
    public class KeyHandler
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        private static extern short GetKeyState(int keyCode);
        public static extern short GetAsyncKeyState(int keyCode);



        //This function is useful to simulate Key presses to the window with focus.
        //[DllImport("user32.dll")]
        //static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, int dwExtraInfo);

        //[DllImport("System.Windows.Forms.dll", CharSet = CharSet.Auto, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]




        bool CapsLock = false;
        bool NumLock = false;
        bool ScrollLock = false;
        bool KeyPressed = false;

        public KeyHandler()
        {
            UpdateKeyState();

            var hook = new TaskPoolGlobalHook();

            hook.HookEnabled += OnHookEnabled;     // EventHandler<HookEventArgs>
                                                   //hook.HookDisabled += OnHookDisabled;   // EventHandler<HookEventArgs>

            // CapsLock, NumLock activate on KeyPress
            hook.KeyPressed += OnKeyPressed;       // EventHandler<KeyboardHookEventArgs>
            hook.KeyReleased += OnKeyReleased;

            hook.RunAsync();

        }


        private void OnHookEnabled(object? sender, HookEventArgs e)
        {
            Debug.WriteLine("\n\n\n\n\n\n\n\n");
            Debug.WriteLine($"NumLock: {NumLock}, CapsLock: {CapsLock}, ScrollLock: {ScrollLock}");
            Debug.WriteLine("Hook enabled");
        }

        private void OnKeyPressed(object? sender, KeyboardHookEventArgs e)
        {
            if (KeyPressed)
                return;
            //KeyCode.VcE
            KeyPressed = true;
            Debug.WriteLine($"Key Pressed: {e.RawEvent.Keyboard.KeyChar} | {e.RawEvent.Keyboard.KeyCode}  | {e.RawEvent.Keyboard.RawCode} | {e.RawEvent.Keyboard.RawKeyChar}");

            Debug.WriteLine("State after press");
            UpdateKeyState();

        }

        private void OnKeyReleased(object? sender, KeyboardHookEventArgs e)
        {
            KeyPressed = false;
            Debug.WriteLine("State after release");
            UpdateKeyState();
            Debug.WriteLine("\n\n");
        }

        public void UpdateKeyState()
        {
            var y = VirtualKey.CapitalLock;
            var z = SharpHook.Native.KeyCode.VcCapsLock;
            NumLock = System.Console.NumberLock;
            CapsLock = System.Console.CapsLock;
            //Control.IsKeyLocked
            //Keyboard.ScrollLock

            var VK_CAPITAL = (int) z;
            var CapsLock2 = (((ushort) GetKeyState(VK_CAPITAL)) & 0xffff) != 0;
            var CapsLock3 = (GetKeyState(VK_CAPITAL) & 0x0001) != 0;
            //NumLock = (((ushort) GetKeyState(0x90)) & 0xffff) != 0;
            ScrollLock = (((ushort) GetKeyState(0x91)) & 0xffff) != 0;
            //Key_Handler.GetKeyState()

            // MacOS
            //bool CapsLock3 = (CGEventSourceFlagsState(1) & 0x00010000) != 0;

            //Debug.WriteLine($"NumLock: {NumLock}, CapsLock: {CapsLock}, ScrollLock: {ScrollLock}");

            //var CapsLock3 = System.Windows.Forms.Control.IsKeyLocked(Keys.CapsLock);

            Debug.WriteLine($"CAPS LOCK: 1: {CapsLock}, 2: {CapsLock2}, 3: {CapsLock3}");

        }

    }
}
