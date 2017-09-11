namespace DeletionTool
{
    partial class FormMain
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.ButtonOK = new System.Windows.Forms.Button();
            this.CheckBoxDelete = new System.Windows.Forms.CheckBox();
            this.LabelTips = new System.Windows.Forms.Label();
            this.LabelPath = new System.Windows.Forms.Label();
            this.BackgroundWorkerDel = new System.ComponentModel.BackgroundWorker();
            this.SuspendLayout();
            // 
            // ButtonOK
            // 
            this.ButtonOK.BackColor = System.Drawing.SystemColors.WindowFrame;
            this.ButtonOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ButtonOK.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ButtonOK.ForeColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.ButtonOK.Location = new System.Drawing.Point(228, 37);
            this.ButtonOK.Name = "ButtonOK";
            this.ButtonOK.Size = new System.Drawing.Size(50, 24);
            this.ButtonOK.TabIndex = 0;
            this.ButtonOK.TabStop = false;
            this.ButtonOK.UseVisualStyleBackColor = false;
            this.ButtonOK.Click += new System.EventHandler(this.ButtonOK_Click);
            // 
            // CheckBoxDelete
            // 
            this.CheckBoxDelete.AutoSize = true;
            this.CheckBoxDelete.BackColor = System.Drawing.SystemColors.WindowFrame;
            this.CheckBoxDelete.ForeColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.CheckBoxDelete.Location = new System.Drawing.Point(168, 42);
            this.CheckBoxDelete.Name = "CheckBoxDelete";
            this.CheckBoxDelete.Size = new System.Drawing.Size(15, 14);
            this.CheckBoxDelete.TabIndex = 0;
            this.CheckBoxDelete.TabStop = false;
            this.CheckBoxDelete.UseVisualStyleBackColor = false;
            this.CheckBoxDelete.CheckedChanged += new System.EventHandler(this.CheckBoxDelete_CheckedChanged);
            // 
            // LabelTips
            // 
            this.LabelTips.ForeColor = System.Drawing.SystemColors.Info;
            this.LabelTips.Location = new System.Drawing.Point(12, 41);
            this.LabelTips.Name = "LabelTips";
            this.LabelTips.Size = new System.Drawing.Size(113, 17);
            this.LabelTips.TabIndex = 4;
            this.LabelTips.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // LabelPath
            // 
            this.LabelPath.AllowDrop = true;
            this.LabelPath.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.LabelPath.Location = new System.Drawing.Point(6, 9);
            this.LabelPath.Name = "LabelPath";
            this.LabelPath.Size = new System.Drawing.Size(270, 16);
            this.LabelPath.TabIndex = 5;
            this.LabelPath.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.LabelPath.DragDrop += new System.Windows.Forms.DragEventHandler(this.LabelPath_DragDrop);
            this.LabelPath.DragEnter += new System.Windows.Forms.DragEventHandler(this.LabelPath_DragEnter);
            // 
            // BackgroundWorkerDel
            // 
            this.BackgroundWorkerDel.DoWork += new System.ComponentModel.DoWorkEventHandler(this.BackgroundWorkerDel_DoWork);
            this.BackgroundWorkerDel.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.BackgroundWorkerDel_RunWorkerCompleted);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.WindowFrame;
            this.ClientSize = new System.Drawing.Size(283, 68);
            this.Controls.Add(this.LabelPath);
            this.Controls.Add(this.LabelTips);
            this.Controls.Add(this.CheckBoxDelete);
            this.Controls.Add(this.ButtonOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FormMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "DeletionTool";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ButtonOK;
        private System.Windows.Forms.CheckBox CheckBoxDelete;
        private System.Windows.Forms.Label LabelTips;
        private System.Windows.Forms.Label LabelPath;
        private System.ComponentModel.BackgroundWorker BackgroundWorkerDel;
    }
}

