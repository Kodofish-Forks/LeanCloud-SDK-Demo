namespace Demo.LeanCloud.WindowsForms
{
    partial class Main
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
            this.tb_Console = new System.Windows.Forms.TextBox();
            this.tb_UserName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.bt_Login = new System.Windows.Forms.Button();
            this.lb_Friends = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tb_ConversationContent = new System.Windows.Forms.TextBox();
            this.lb_ConversationTitle = new System.Windows.Forms.Label();
            this.tb_Message = new System.Windows.Forms.TextBox();
            this.bt_SendMessage = new System.Windows.Forms.Button();
            this.lb_ConversationList = new System.Windows.Forms.ListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // tb_Console
            // 
            this.tb_Console.Location = new System.Drawing.Point(33, 658);
            this.tb_Console.Multiline = true;
            this.tb_Console.Name = "tb_Console";
            this.tb_Console.Size = new System.Drawing.Size(1685, 232);
            this.tb_Console.TabIndex = 0;
            // 
            // tb_UserName
            // 
            this.tb_UserName.Location = new System.Drawing.Point(177, 52);
            this.tb_UserName.Name = "tb_UserName";
            this.tb_UserName.Size = new System.Drawing.Size(296, 36);
            this.tb_UserName.TabIndex = 1;
            this.tb_UserName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tb_UserName_KeyPress);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(29, 52);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(130, 24);
            this.label1.TabIndex = 2;
            this.label1.Text = "使用者名稱";
            // 
            // bt_Login
            // 
            this.bt_Login.Location = new System.Drawing.Point(495, 52);
            this.bt_Login.Name = "bt_Login";
            this.bt_Login.Size = new System.Drawing.Size(92, 36);
            this.bt_Login.TabIndex = 3;
            this.bt_Login.Text = "登入";
            this.bt_Login.UseVisualStyleBackColor = true;
            this.bt_Login.Click += new System.EventHandler(this.bt_Login_Click);
            // 
            // lb_Friends
            // 
            this.lb_Friends.FormattingEnabled = true;
            this.lb_Friends.ItemHeight = 24;
            this.lb_Friends.Location = new System.Drawing.Point(33, 178);
            this.lb_Friends.Name = "lb_Friends";
            this.lb_Friends.Size = new System.Drawing.Size(278, 412);
            this.lb_Friends.TabIndex = 4;
            this.lb_Friends.SelectedIndexChanged += new System.EventHandler(this.lb_Friends_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(33, 134);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(106, 24);
            this.label2.TabIndex = 5;
            this.label2.Text = "好友名單";
            // 
            // tb_ConversationContent
            // 
            this.tb_ConversationContent.Location = new System.Drawing.Point(1032, 178);
            this.tb_ConversationContent.Multiline = true;
            this.tb_ConversationContent.Name = "tb_ConversationContent";
            this.tb_ConversationContent.Size = new System.Drawing.Size(686, 370);
            this.tb_ConversationContent.TabIndex = 6;
            // 
            // lb_ConversationTitle
            // 
            this.lb_ConversationTitle.AutoSize = true;
            this.lb_ConversationTitle.Location = new System.Drawing.Point(1028, 134);
            this.lb_ConversationTitle.Name = "lb_ConversationTitle";
            this.lb_ConversationTitle.Size = new System.Drawing.Size(106, 24);
            this.lb_ConversationTitle.TabIndex = 7;
            this.lb_ConversationTitle.Text = "對話內容";
            // 
            // tb_Message
            // 
            this.tb_Message.Location = new System.Drawing.Point(1032, 554);
            this.tb_Message.Name = "tb_Message";
            this.tb_Message.Size = new System.Drawing.Size(574, 36);
            this.tb_Message.TabIndex = 8;
            this.tb_Message.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tb_Message_KeyPress);
            // 
            // bt_SendMessage
            // 
            this.bt_SendMessage.Location = new System.Drawing.Point(1612, 554);
            this.bt_SendMessage.Name = "bt_SendMessage";
            this.bt_SendMessage.Size = new System.Drawing.Size(106, 36);
            this.bt_SendMessage.TabIndex = 9;
            this.bt_SendMessage.Text = "傳送";
            this.bt_SendMessage.UseVisualStyleBackColor = true;
            this.bt_SendMessage.Click += new System.EventHandler(this.bt_SendMessage_Click);
            // 
            // lb_ConversationList
            // 
            this.lb_ConversationList.FormattingEnabled = true;
            this.lb_ConversationList.ItemHeight = 24;
            this.lb_ConversationList.Location = new System.Drawing.Point(317, 178);
            this.lb_ConversationList.Name = "lb_ConversationList";
            this.lb_ConversationList.Size = new System.Drawing.Size(709, 412);
            this.lb_ConversationList.TabIndex = 10;
            this.lb_ConversationList.SelectedIndexChanged += new System.EventHandler(this.lb_ConversationList_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(317, 133);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(106, 24);
            this.label3.TabIndex = 11;
            this.label3.Text = "對話列表";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(33, 620);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(106, 24);
            this.label4.TabIndex = 12;
            this.label4.Text = "系統訊息";
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1728, 905);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lb_ConversationList);
            this.Controls.Add(this.bt_SendMessage);
            this.Controls.Add(this.tb_Message);
            this.Controls.Add(this.lb_ConversationTitle);
            this.Controls.Add(this.tb_ConversationContent);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lb_Friends);
            this.Controls.Add(this.bt_Login);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tb_UserName);
            this.Controls.Add(this.tb_Console);
            this.Name = "Main";
            this.Text = "Main";
            this.Load += new System.EventHandler(this.Main_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tb_Console;
        private System.Windows.Forms.TextBox tb_UserName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button bt_Login;
        private System.Windows.Forms.ListBox lb_Friends;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tb_ConversationContent;
        private System.Windows.Forms.Label lb_ConversationTitle;
        private System.Windows.Forms.TextBox tb_Message;
        private System.Windows.Forms.Button bt_SendMessage;
        private System.Windows.Forms.ListBox lb_ConversationList;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
    }
}