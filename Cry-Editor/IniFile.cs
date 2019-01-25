using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace Crying
{
    public class IniFile
    {
        // TODO: Not P/Invoke.
        [DllImport("kernel32")]
        private static extern bool GetPrivateProfileString(
            string lpAppName,
            string lpKeyName,
            string lpDefault,
            StringBuilder lpReturnedString,
            int nSize,
            string lpFileName
        );

        public IniFile(string filename)
        {
            FileName = filename ?? throw new ArgumentNullException(nameof(filename));
            if (!File.Exists(filename))
                throw new FileNotFoundException();
        }

        public bool TryGetString(string section, string key, out string value)
        {
            var sb = new StringBuilder(255);

            if (GetPrivateProfileString(section, key, string.Empty, sb, 255, FileName))
            {
                value = sb.ToString();
                return true;
            }

            value = string.Empty;
            return false;
        }

        public bool TryGetInt32(string section, string key, out int value)
        {
            if (TryGetString(section, key, out string str))
            {
                if (str.StartsWith("0x") ||
                    str.StartsWith("0X") ||
                    str.StartsWith("&h") ||
                    str.StartsWith("&H"))
                {
                    if (int.TryParse(str.Substring(2), NumberStyles.HexNumber, null, out int i))
                    {
                        value = i;
                        return true;
                    }
                }
                else
                {
                    if (int.TryParse(str, out int i))
                    {
                        value = i;
                        return true;
                    }
                }
            }

            value = -1;
            return false;
        }

        public string GetString(string section, string key)
        {
            if (TryGetString(section, key, out string value))
            {
                return value;
            }

            throw new KeyNotFoundException($"Could not find {key} within [{section}].");
        }

        public int GetInt32(string section, string key)
        {
            if (TryGetInt32(section, key, out int value))
            {
                return value;
            }

            throw new KeyNotFoundException($"Could not find {key} within [{section}].");
        }

        private string FileName { get; }
    }
}
