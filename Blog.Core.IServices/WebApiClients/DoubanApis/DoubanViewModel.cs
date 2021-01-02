namespace Blog.Core.Common.WebApiClients.HttpApis
{
    public class Data
    {
        /// <summary>
        /// 
        /// </summary>
        public string isbn { get; set; }
        /// <summary>
        /// 解忧杂货店
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// ナミヤ雑貨店の奇蹟
        /// </summary>
        public string origintitle { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string subtitle { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string image { get; set; }
        /// <summary>
        /// [日]东野圭吾
        /// </summary>
        public string author { get; set; }
        /// <summary>
        /// 李盈春
        /// </summary>
        public string translator { get; set; }
        /// <summary>
        /// 南海出版公司
        /// </summary>
        public string publisher { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string pubdate { get; set; }
        /// <summary>
        /// <东野圭吾><治愈><温暖><小说><日本><日本文学><東野圭吾><推理>
        /// </summary>
        public string tags { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string kaiben { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string zhizhang { get; set; }
        /// <summary>
        /// 精装
        /// </summary>
        public string binding { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string taozhuang { get; set; }
        /// <summary>
        /// 新经典文库·东野圭吾作品
        /// </summary>
        public string series { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string pages { get; set; }
        /// <summary>
        /// 39.50元
        /// </summary>
        public string price { get; set; }

        public string author_intro { get; set; }

        public string summary { get; set; }

        public string catalog { get; set; }
    }

    public class DoubanViewModel
    {
        /// <summary>
        /// 
        /// </summary>
        public string status { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Data data { get; set; }
        /// <summary>
        /// 获取图书数据成功
        /// </summary>
        public string msg { get; set; }
    }
}
