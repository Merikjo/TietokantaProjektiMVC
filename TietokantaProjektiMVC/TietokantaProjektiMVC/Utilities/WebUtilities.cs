using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace TietokantaProjekti2MVC.Utilities
{
    public static class WebUtilities
    {
        public static string ReadToEnd(this Stream stream)
        {
            int length = (int)stream.Length;
            byte[] buffer = new byte[length];
            int bytesRead = stream.Read(buffer, 0, length);
            string data = Encoding.UTF8.GetString(buffer, 0, bytesRead);
            return data;
        }
    }
}