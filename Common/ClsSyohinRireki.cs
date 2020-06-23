using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STSH_OCR.Common
{
    class ClsSyohinRireki
    {
        // 商品コード
        public string SYOHIN_CD { get; set; }

        // 商品名
        public string SYOHIN_NM { get; set; }
        
        // 仕入先名
        public string SIRESAKI_NM { get; set; }

        // 規格
        public string SYOHIN_KIKAKU { get; set; }

        // ケース入数（バラ換算）
        public double CASE_IRISU { get; set; }

        // 期間発注数
        public int Suu { get; set; }

        // 終売
        public bool Shubai { get; set; }

        // 大分類 : 2020/06/22
        public string SYOHIN_KIND_L_CD { get; set; }

        // 中分類 : 2020/06/22
        public string SYOHIN_KIND_M_CD { get; set; }

        // 小分類 : 2020/06/22
        public string SYOHIN_KIND_S_CD { get; set; }
    }
}
