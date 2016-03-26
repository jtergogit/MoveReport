using System.Collections.Generic;

namespace MoveReport
{
    public class ParamModel
    {

        /// <summary>
        /// 类型
        /// </summary>
        public string Type { set; get; }
        /// <summary>
        /// 参数名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<ParamItem> ParamItems { get; set; }
        /// <summary>
        /// 选中值
        /// </summary>
        public string Value { set; get; }
        /// <summary>
        /// 选中文本
        /// </summary>
        public string Text { set; get; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

    }
}