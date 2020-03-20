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
    }
}
