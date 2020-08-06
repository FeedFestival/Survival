using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Utils
{
    public static class utils
    {
        private static CanvasController _canvasController;
        public static CanvasController CanvasController
        {
            get
            {
                return _canvasController;
            }
            set { _canvasController = value; }
        }

        public static string GetDataValue(string data, string index)
        {
            string value = data.Substring(data.IndexOf(index, StringComparison.Ordinal) + index.Length);
            if (value.Contains("|"))
                value = value.Remove(value.IndexOf('|'));
            return value;
        }
        public static int GetIntDataValue(string data, string index)
        {
            int numb;
            var success = int.TryParse(GetDataValue(data, index), out numb);

            return success ? numb : 0;
        }
        public static bool GetBoolDataValue(string data, string index)
        {
            var value = GetDataValue(data, index);
            if (string.IsNullOrEmpty(value) || value.Equals("0"))
                return false;
            return true;
        }
        public static long GetLongDataValue(string data, string index)
        {
            long numb;
            var success = long.TryParse(GetDataValue(data, index), out numb);

            return success ? numb : 0;
        }

        public static long GetLongDataValue(string data)
        {
            long numb;
            var success = long.TryParse(data, out numb);

            return success ? numb : 0;
        }

        public static Color white;
        public static Color grey;
        public static Color black;
        public static Color gold;

        public static void InitColors()
        {
            ColorUtility.TryParseHtmlString("#E6E6E6FF", out white);
            ColorUtility.TryParseHtmlString("#18191AFF", out black);
            ColorUtility.TryParseHtmlString("#FF3232", out gold);
            ColorUtility.TryParseHtmlString("#909090FF", out grey);
        }

        public static Texture2D LoadLightmapFromDisk(string filePath)
        {

            Texture2D tex = null;
            byte[] fileData;

            if (File.Exists(filePath))
            {
                fileData = File.ReadAllBytes(filePath);
                tex = new Texture2D(256, 256, TextureFormat.RGBAFloat, false);
                tex.LoadImage(fileData); //..this will auto-resize the texture dimensions.
                var exrBytes = tex.EncodeToEXR(Texture2D.EXRFlags.CompressZIP);
                tex.LoadRawTextureData(exrBytes);

                //tex = new Texture2D(256, 256, TextureFormat.RGBA32, false);
                //tex.LoadRawTextureData(File.ReadAllBytes(filePath));
            }
            return tex;
        }
    }

    public enum NavbarButton
    {
        HomeButton,
        FriendsButton,
        HistoryButton
    }
}
