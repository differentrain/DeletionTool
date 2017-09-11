using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DeletionTool
{
    public partial class FormFixPath : Form
    {
        private String _Dots;

        public String Dots => _Dots;

        public FormFixPath()
        {
            InitializeComponent();
            LabelDot.Text = FormMain.DotCount;
            LabelMsg.Text = FormMain.ErrorPathMsg;
            ButtonOK.Text = FormMain.ButtonOKText;
            ButtonCancle.Text = FormMain.ButtonCancleText;
            this.Text = FormMain.FormFixPathText;
            NumberBoxDot.Text = "2";
        }

        private void ButtonOK_Click(object sender, EventArgs e)
        {
            _Dots = new String('.', NumberBoxDot.Value);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void ButtonCancle_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
