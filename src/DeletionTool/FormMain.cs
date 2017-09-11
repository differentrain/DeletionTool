using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using YYProject.AdvancedDeletion;

namespace DeletionTool
{


    public partial class FormMain : Form
    {

        private FormFixPath FPForm = new FormFixPath();

        #region  Initialize

        private static readonly String Processing;
        private static readonly String Done;
        private static readonly String LableBoxTextEmpty;
        private static readonly String CheckBoxDeleteText;
        public static readonly String ButtonOKText;
        public static readonly String ButtonCancleText;
        public static readonly String ErrorPathMsg;
        public static readonly String DotCount;
        public static readonly String FormFixPathText;
        //MultiLang (English and Chinese Simplified)
        static FormMain()
        {
            if (System.Globalization.CultureInfo.InstalledUICulture.ThreeLetterISOLanguageName == "zho")
            {
                // Chinese Simplified
                Processing = "*** 处理中 ***";
                Done = "*** 完成 ***";
                LableBoxTextEmpty = "拖拽单个文件或文件夹到此处";
                CheckBoxDeleteText = "删除";
                ButtonOKText = "确定";
                ButtonCancleText = "取消";
                ErrorPathMsg = "路径未找到，它可能是一个以多个点号(.)结尾的文件夹，请输出点号数量：";
                DotCount = "数量(2-99)：";
                FormFixPathText = "修改路径";
            }
            else
            {
                Processing = "*** Processing ***";
                Done = "*** Done ***";
                LableBoxTextEmpty = "Drag and drop a file or a folder into me";
                CheckBoxDeleteText = "Delete";
                ButtonOKText = "OK";
                ButtonCancleText = "Cancle";
                ErrorPathMsg = "Path was not found, it may be a Directory end with Dot(.), please enter the count of Dot.";
                DotCount = "Count(2-99):";
                FormFixPathText = "Modify the path";
            }
        }

        public FormMain()
        {
            InitializeComponent();
            ButtonOK.Text = ButtonOKText;
            CheckBoxDelete.Text = CheckBoxDeleteText;
            StatusChange();
        }
        #endregion


        private enum Status
        {
            Start,
            Dropped,
            Processing,
            Finished
        }
        private void StatusChange(Status status = Status.Start, String advText = null)
        {
            switch (status)
            {
                case Status.Dropped:
                    LabelPath.Text = advText;
                    LabelTips.Text = string.Empty;
                    CheckBoxDelete.Enabled = true;
                    CheckBoxDelete.Checked = false;
                    ButtonOK.Enabled = false;
                    break;
                case Status.Processing:
                    LabelTips.Focus();
                    LabelTips.Text = Processing;
                    LabelPath.Enabled = false;
                    CheckBoxDelete.Enabled = false;
                    ButtonOK.Enabled = false;
                    break;
                case Status.Finished:
                    LabelTips.Text = Done;
                    goto default;
                default:
                    LabelPath.Enabled = true;
                    LabelPath.Text = LableBoxTextEmpty;
                    CheckBoxDelete.Enabled = false;
                    CheckBoxDelete.Checked = false;
                    ButtonOK.Enabled = false;
                    break;
            }
        }

        private void CheckBoxDelete_CheckedChanged(object sender, EventArgs e)
        {
            ButtonOK.Enabled = CheckBoxDelete.Checked;
        }

        private String Path;

        private void LabelPath_DragDrop(object sender, DragEventArgs e)
        {
            var list = (String[])e.Data.GetData(DataFormats.FileDrop);
            if (list.Length == 1)
            {

                if (AdvancedDeletion.IsExists(list[0], out var b))
                {
                    Path = list[0];
                    StatusChange(Status.Dropped, Path);
                    return;
                }
                else if (b && FPForm.ShowDialog(this) == DialogResult.OK)
                {
                    Path = list[0] + FPForm.Dots;
                    if (AdvancedDeletion.IsExists(Path, out b))
                    {
                        StatusChange(Status.Dropped, Path);
                        return;
                    }
                }

            }
            StatusChange();
        }
        private void LabelPath_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Link;
            }
            else
            {
               StatusChange();
                e.Effect = DragDropEffects.None;
            }
        }

        private void ButtonOK_Click(object sender, EventArgs e)
        {
            StatusChange(Status.Processing);
            BackgroundWorkerDel.RunWorkerAsync();
        }

        private void BackgroundWorkerDel_DoWork(object sender, DoWorkEventArgs e)
        {
            AdvancedDeletion.Delete(Path);
        }

        private void BackgroundWorkerDel_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            StatusChange(Status.Finished);
        }

     
    }
}
