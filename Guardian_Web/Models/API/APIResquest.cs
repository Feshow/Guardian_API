﻿using static Guardian_Utility.SD;

namespace Guardian_Web.Models.API
{
    public class APIResquest
    {
        public ApiType ApiType { get; set; } = ApiType.GET;
        public string Url { get; set; }
        public object Data { get; set; }
    }
}
