namespace Demo.LeanCloud.WindowsForms
{
    partial class frm_AddGroup
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
            this.bt_CloseForm = new System.Windows.Forms.Button();
            this.tb_GroupName = new System.Windows.Forms.TextBox();
            this.bt_AddGroup = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.lb_Members = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // bt_CloseForm
            // 
            this.bt_CloseForm.Location = new System.Drawing.Point(309, 572);
            this.bt_CloseForm.Name = "bt_CloseForm";
            this.bt_CloseForm.Size = new System.Drawing.Size(104, 51);
            this.bt_CloseForm.TabIndex = 0;
            this.bt_CloseForm.Text = "關閉";
            this.bt_CloseForm.UseVisualStyleBackColor = true;
            this.bt_CloseForm.Click += new System.EventHandler(this.bt_CloseForm_Click);
            // 
            // tb_GroupName
            // 
            this.tb_GroupName.Location = new System.Drawing.Point(143, 56);
            this.tb_GroupName.Name = "tb_GroupName";
            this.tb_GroupName.Size = new System.Drawing.Size(502, 36);
            this.tb_GroupName.TabIndex = 1;
            // 
            // bt_AddGroup
            // 
            this.bt_AddGroup.Location = new System.Drawing.Point(143, 572);
            this.bt_AddGroup.Name = "bt_AddGroup";
            this.bt_AddGroup.Size = new System.Drawing.Size(160, 51);
            this.bt_AddGroup.TabIndex = 3;
            this.bt_AddGroup.Text = "新增群組";
            this.bt_AddGroup.UseVisualStyleBackColor = true;
            this.bt_AddGroup.Click += new System.EventHandler(this.bt_AddGroup_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 67);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(106, 24);
            this.label1.TabIndex = 4;
            this.label1.Text = "群組名稱";
            // 
            // lb_Members
            // 
            this.lb_Members.FormattingEnabled = true;
            this.lb_Members.ItemHeight = 24;
            this.lb_Members.Location = new System.Drawing.Point(143, 120);
            this.lb_Members.Name = "lb_Members";
            this.lb_Members.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.lb_Members.Size = new System.Drawing.Size(502, 436);
            this.lb_Members.TabIndex = 5;
            // 
            // frm_AddGroup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(812, 873);
            this.Controls.Add(this.lb_Members);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.bt_AddGroup);
            this.Controls.Add(this.tb_GroupName);
            this.Controls.Add(this.bt_CloseForm);
            this.Name = "frm_AddGroup";
            this.Text = "AddGroup";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button bt_CloseForm;
        private System.Windows.Forms.TextBox tb_GroupName;
        private System.Windows.Forms.Button bt_AddGroup;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox lb_Members;
    }
}