using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STSH_OCR.Common
{
    public class ClsGoods
    {
        public string Code { get; set; }
        //public int Nouka { get; set; }    // 2020/08/04 コメント化 
        public double Nouka { get; set; }   // 小数点以下対応 2020/08/04 
        //public int Baika { get; set; }    // 2020/08/04 コメント化      
        public double Baika { get; set; }   // 小数点以下対応 2020/08/04 
        public string [] Suu { get; set; }
        public bool[] Target { get; set; }
        public int Syubai { get; set; }
    }
}
