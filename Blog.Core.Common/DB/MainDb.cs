using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Core.Common.DB
{
    public class MainDb
    {
        public MainDb(int currentDbID)
        {
            CurrentDbID = currentDbID;
        }
        public int CurrentDbID { get; set; } = 0;
    }
}
