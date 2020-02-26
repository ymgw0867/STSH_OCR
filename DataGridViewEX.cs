using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace STSH_OCR
{

    /// <summary>
    /// Enterキーが押された時に、Tabキーが押されたのと同じ動作をする
    /// （現在のセルを隣のセルに移動する）DataGridView
    /// </summary>
    public class DataGridViewEx : DataGridView
    {
        //直前に押したキーがEnterならtrue
        bool isEnterLast = false;
        bool suppressCellValidating = false;

        [System.Security.Permissions.UIPermission(
            System.Security.Permissions.SecurityAction.LinkDemand,
            Window = System.Security.Permissions.UIPermissionWindow.AllWindows)]
        protected override bool ProcessDialogKey(Keys keyData)
        {
            isEnterLast = false;
            suppressCellValidating = false;

            // ①Enterキー、→キーが押された時は、Tabキーが押されたようにする
            // ②ReadOnlyのセルはスキップする
            if ((keyData & Keys.KeyCode) == Keys.Enter || (keyData & Keys.KeyCode) == Keys.Right)
            {
                isEnterLast = true;
                DataGridViewCell _CurrentCell = this.CurrentCell;
                //cellValidatingCancel = false;

                // 次のセルがReadonlyのときReadOnlyではないセルまで検証
                int Col = this.CurrentCell.ColumnIndex + 1;
                while (Col < this.Columns.Count)
                {
                    if (this.Columns[Col].ReadOnly == false)
                    {
                        break;
                    }
                    Col++;
                }

                // 同じ行内での移動
                if (Col < this.Columns.Count)
                {
                    this.CurrentCell = this[this.Columns[Col].Name, this.CurrentRow.Index];
                }
                // 次の行へ移動（ダイレクトに指定カラムへ遷移）
                else if (this.CurrentRow.Index < this.Rows.Count - 1)
                {
                    this.CurrentCell = this[STSH_OCR.Common.global.NEXT_COLUMN, this.CurrentRow.Index + 1];
                }

                if (this.cellValidatingCancel)
                {
                    suppressCellValidating = true;
                    this.CurrentCell = _CurrentCell;
                    BeginEdit(false);
                    this.NotifyCurrentCellDirty(true);
                }

                return true;
            }

            return base.ProcessDialogKey(keyData);
        }


        [System.Security.Permissions.SecurityPermission(
            System.Security.Permissions.SecurityAction.LinkDemand,
            Flags = System.Security.Permissions.SecurityPermissionFlag.UnmanagedCode)]
        protected override bool ProcessDataGridViewKey(KeyEventArgs e)
        {
            isEnterLast = false;
            suppressCellValidating = false;

            // ①Enterキー、→キーが押された時は、Tabキーが押されたようにする
            // ②ReadOnlyのセルはスキップする
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Right)
            {
                isEnterLast = true;
                DataGridViewCell _CurrentCell = this.CurrentCell;
                //cellValidatingCancel = false;

                // 次のセルがReadonlyのときReadOnlyではないセルまで検証
                int Col = this.CurrentCell.ColumnIndex + 1;
                while (Col < this.Columns.Count)
                {
                    if (this.Columns[Col].ReadOnly == false)
                    {
                        break;
                    }

                    Col++;
                }

                // 同じ行内での移動
                if (Col < this.Columns.Count)
                {
                    this.CurrentCell = this[this.Columns[Col].Name, this.CurrentRow.Index];
                }
                else if (this.CurrentRow.Index < this.Rows.Count - 1)
                {
                    // 次の行へ移動（ダイレクトにカラム[1]へ遷移）
                    this.CurrentCell = this[STSH_OCR.Common.global.NEXT_COLUMN, this.CurrentRow.Index + 1];
                }

                if (this.cellValidatingCancel)
                {
                    suppressCellValidating = true;
                    this.CurrentCell = _CurrentCell;
                    BeginEdit(false);
                    this.NotifyCurrentCellDirty(true);
                }

                return true;
            }
            return base.ProcessDataGridViewKey(e);
        }

        bool cellValidatingCancel = false;


        //[System.Security.Permissions.SecurityPermission(
        //    System.Security.Permissions.SecurityAction.LinkDemand,
        //    Flags = System.Security.Permissions.SecurityPermissionFlag.UnmanagedCode)]
        //protected override bool ProcessDataGridViewKey(KeyEventArgs e)
        //{
        //    //Enterキーが押された時は、Tabキーが押されたようにする
        //    if (e.KeyCode == Keys.Enter)
        //    {
        //        return this.ProcessTabKey(e.KeyCode);
        //    }
        //    return base.ProcessDataGridViewKey(e);
        //}


        protected override void OnCellValidating(DataGridViewCellValidatingEventArgs e)
        {
            if (suppressCellValidating) return;

            base.OnCellValidating(e);

            cellValidatingCancel = e.Cancel;

            if (isEnterLast) e.Cancel = false;
        }

    }
}
