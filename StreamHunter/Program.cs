using System;
using System.Collections.Generic;

namespace StreamHunter
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Hunter Hunter = new Hunter("C:/Users/Onur/Google Drive/workspace/C++/TTV/Resources/playlist.tivi");
            Hunter.Schedule();
        }
    }
}