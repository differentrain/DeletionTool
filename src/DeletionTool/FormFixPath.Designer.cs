namespace DeletionTool
{
    partial class FormFixPath
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.LabelMsg = new System.Windows.Forms.Label();
            this.ButtonOK = new System.Windows.Forms.Button();
            this.ButtonCancle = new System.Windows.Forms.Button();
            this.LabelDot = new System.Windows.Forms.Label();
            this.NumberBoxDot = new DeletionTool.NumberBox();
            this.SuspendLayout();
            // 
            // LabelMsg
            // 
            this.LabelMsg.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.LabelMsg.Location = new System.Drawing.Point(3, 9);
            this.LabelMsg.Name = "LabelMsg";
            this.LabelMsg.Size = new System.Drawing.Size(280, 27);
            this.LabelMsg.TabIndex = 0;
            // 
            // ButtonOK
            // 
            this.ButtonOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ButtonOK.Location = new System.Drawing.Point(147, 39);
            this.ButtonOK.Name = "ButtonOK";
            this.ButtonOK.Size = new System.Drawing.Size(53, 24);
            this.ButtonOK.TabIndex = 0;
            this.ButtonOK.TabStop = false;
            this.ButtonOK.UseVisualStyleBackColor = true;
            this.ButtonOK.Click += new System.EventHandler(this.ButtonOK_Click);
            // 
            // ButtonCancle
            // 
            this.ButtonCancle.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ButtonCancle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ButtonCancle.Location = new System.Drawing.Point(221, 39);
            this.ButtonCancle.Name = "ButtonCancle";
            this.ButtonCancle.Size = new System.Drawing.Size(53, 24);
            this.ButtonCancle.TabIndex = 0;
            this.ButtonCancle.TabStop = false;
            this.ButtonCancle.UseVisualStyleBackColor = true;
            this.ButtonCancle.Click += new System.EventHandler(this.ButtonCancle_Click);
            // 
            // LabelDot
            // 
            this.LabelDot.Location = new System.Drawing.Point(9, 44);
            this.LabelDot.Name = "LabelDot";
            this.LabelDot.Size = new System.Drawing.Size(82, 19);
            this.LabelDot.TabIndex = 4;
            // 
            // NumberBoxDot
            // 
            this.NumberBoxDot.Location = new System.Drawing.Point(97, 40);
            this.NumberBoxDot.MaxNum = 99;
            this.NumberBoxDot.MinNum = 2;
            this.NumberBoxDot.Name = "NumberBoxDot";
            this.NumberBoxDot.Size = new System.Drawing.Size(33, 21);
            this.NumberBoxDot.TabIndex = 5;
            // 
            // FormFixPath
            // 
            this.AcceptButton = this.ButtonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.ButtonCancle;
            this.ClientSize = new System.Drawing.Size(286, 69);
            this.ControlBox = false;
            this.Controls.Add(this.NumberBoxDot);
            this.Controls.Add(this.LabelDot);
            this.Controls.Add(this.ButtonCancle);
            this.Controls.Add(this.ButtonOK);
            this.Controls.Add(this.LabelMsg);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FormFixPath";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "FormFixPath";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label LabelMsg;
        private System.Windows.Forms.Button ButtonOK;
        private System.Windows.Forms.Button ButtonCancle;
        private System.Windows.Forms.Label LabelDot;
        private NumberBox NumberBoxDot;
    }
}