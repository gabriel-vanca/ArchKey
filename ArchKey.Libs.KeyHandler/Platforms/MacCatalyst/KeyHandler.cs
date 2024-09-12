using System.Runtime.InteropServices;

namespace ArchKey.Libs.KeyHandler.Platforms.MacCatalyst
{
    public class KeyHandler
    {
        [DllImport("/System/Library/Frameworks/ApplicationServices.framework/ApplicationServices")]
        public static extern long CGEventSourceFlagsState(int keyCode);
    }
}
