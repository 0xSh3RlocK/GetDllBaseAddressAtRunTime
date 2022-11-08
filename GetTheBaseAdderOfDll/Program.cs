using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using DWORD = System.UInt32;
namespace GetTheBaseAdderOfDll
{
    internal class Program
    {
        [StructLayout(LayoutKind.Sequential, CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        public struct MODULEENTRY32
        {
            internal uint dwSize;
            internal uint th32ModuleID;
            internal uint th32ProcessID;
            internal uint GlblcntUsage;
            internal uint ProccntUsage;
            internal IntPtr modBaseAddr;
            internal uint modBaseSize;
            internal IntPtr hModule;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            internal string szModule;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            internal string szExePath;
        }

        [DllImport("Kernel32.dll")]
        private static extern IntPtr CreateToolhelp32Snapshot(DWORD dwFlags, 
                                                            DWORD th32ProcessID);

        [DllImport("kernel32.dll")]
        static extern bool Module32FirstW(IntPtr hSnapshot, ref MODULEENTRY32 lpme);

        [DllImport("kernel32.dll")]
        static extern bool Module32NextW(IntPtr hSnapshot, ref MODULEENTRY32 lpme);

        static void Main(string[] args)
        {
            DWORD TH32CS_SNAPHEAPLIST = 1;
            DWORD TH32CS_SNAPMODULE = 8;
            DWORD TH32CS_SNAPPROCESS = 2;
            DWORD TH32CS_SNAPTHREAD = 4;

            IntPtr snapshotH = CreateToolhelp32Snapshot(1 | 2 | 4 | 8, 0);
            MODULEENTRY32 mODULEENTRY32 = new MODULEENTRY32();
            mODULEENTRY32.dwSize = (uint)Marshal.SizeOf(typeof(MODULEENTRY32));
            bool res = Module32FirstW(snapshotH, ref mODULEENTRY32);

            if (res == true)
            {
                Console.WriteLine(mODULEENTRY32.szExePath);
                Console.WriteLine(mODULEENTRY32.szModule);
            }
            while(Module32NextW(snapshotH, ref mODULEENTRY32))
            {
                Console.WriteLine($"mODULE {mODULEENTRY32.szModule} lOADED AT {mODULEENTRY32.modBaseSize.ToString("X")} ");
                //Console.WriteLine(mODULEENTRY32.szModule);
            }
            Console.ReadKey();
        }
    }
}
