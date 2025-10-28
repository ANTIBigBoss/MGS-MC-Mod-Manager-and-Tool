using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;

namespace ANTIBigBoss_MGS_Mod_Manager
{
    public static class MGS2MemoryManager
    {
        // The process name must match exactly (without ".exe")
        private const string PROCESS_NAME_MGS2 = "METAL GEAR SOLID2";
        private const int PROCESS_ALL_ACCESS = 0x1F0FFF;

        // New static offsets (within the EXE)
        private const int OFFSET_X = 0x153F190;
        private const int OFFSET_Y = 0x153F194;
        private const int OFFSET_Z = 0x153F198;

        /// <summary>
        /// Reads the player's X position as a float (Base + OFFSET_X).
        /// </summary>
        public static string ReadMGS2PlayerPositionX()
        {
            Process proc = Process.GetProcessesByName(PROCESS_NAME_MGS2).FirstOrDefault();
            if (proc == null || proc.HasExited)
                return "0";

            IntPtr hProc = OpenProcess(proc);
            if (hProc == IntPtr.Zero)
                return "0";

            try
            {
                // Calculate the final address for X
                IntPtr address = proc.MainModule.BaseAddress + OFFSET_X;
                float xVal = ReadFloat(hProc, address);
                return xVal.ToString("F6");
            }
            finally
            {
                NativeMethods.CloseHandle(hProc);
            }
        }

        /// <summary>
        /// Reads the player's Y position as a float (Base + OFFSET_Y).
        /// </summary>
        public static string ReadMGS2PlayerPositionY()
        {
            Process proc = Process.GetProcessesByName(PROCESS_NAME_MGS2).FirstOrDefault();
            if (proc == null || proc.HasExited)
                return "0";

            IntPtr hProc = OpenProcess(proc);
            if (hProc == IntPtr.Zero)
                return "0";

            try
            {
                IntPtr address = proc.MainModule.BaseAddress + OFFSET_Y;
                float yVal = ReadFloat(hProc, address);
                return yVal.ToString("F6");
            }
            finally
            {
                NativeMethods.CloseHandle(hProc);
            }
        }

        /// <summary>
        /// Reads the player's Z position as a float (Base + OFFSET_Z).
        /// </summary>
        public static string ReadMGS2PlayerPositionZ()
        {
            Process proc = Process.GetProcessesByName(PROCESS_NAME_MGS2).FirstOrDefault();
            if (proc == null || proc.HasExited)
                return "0";

            IntPtr hProc = OpenProcess(proc);
            if (hProc == IntPtr.Zero)
                return "0";

            try
            {
                IntPtr address = proc.MainModule.BaseAddress + OFFSET_Z;
                float zVal = ReadFloat(hProc, address);
                return zVal.ToString("F6");
            }
            finally
            {
                NativeMethods.CloseHandle(hProc);
            }
        }

        // ------------------------------------------------------------------------
        // Internal helper methods

        private static IntPtr OpenProcess(Process proc)
        {
            return NativeMethods.OpenProcess(PROCESS_ALL_ACCESS, false, proc.Id);
        }

        private static float ReadFloat(IntPtr hProcess, IntPtr address)
        {
            byte[] buffer = new byte[4];
            if (NativeMethods.ReadProcessMemory(hProcess, address, buffer, (uint)buffer.Length, out int bytesRead)
                && bytesRead == buffer.Length)
            {
                return BitConverter.ToSingle(buffer, 0);
            }
            return 0f;
        }
    }

    internal static class NativeMethods
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool ReadProcessMemory(
            IntPtr hProcess,
            IntPtr lpBaseAddress,
            [Out] byte[] lpBuffer,
            uint dwSize,
            out int lpNumberOfBytesRead
        );

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool CloseHandle(IntPtr hObject);
    }
}
