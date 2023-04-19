using System.Net;
using System.Net.Sockets;

namespace TelekomunikacjaZad2
{
    partial class GUI
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            ReadButton = new Button();
            SendButton = new Button();
            DecodeButton = new Button();
            SaveButton = new Button();
            contextMenuStrip1 = new ContextMenuStrip(components);
            StringInfo = new TextBox();
            StringText = new RichTextBox();
            BitInfo = new TextBox();
            BitText = new RichTextBox();
            DicionaryText = new RichTextBox();
            DictionaryInfo = new TextBox();
            CodeButton = new Button();
            RecieveButton = new Button();
            SenderIP = new TextBox();
            RecieverIP = new TextBox();
            IPAddr1 = new TextBox();
            IPAddr2 = new TextBox();
            Port1 = new TextBox();
            Port2 = new TextBox();
            RecieveTreeButton = new Button();
            SendTreeButton = new Button();
            SuspendLayout();
            // 
            // ReadButton
            // 
            ReadButton.BackColor = Color.IndianRed;
            ReadButton.Location = new Point(13, 33);
            ReadButton.Margin = new Padding(4);
            ReadButton.Name = "ReadButton";
            ReadButton.Size = new Size(224, 91);
            ReadButton.TabIndex = 1;
            ReadButton.Text = "Wczytaj plik tekstowy";
            ReadButton.UseVisualStyleBackColor = false;
            ReadButton.Click += ReadButton_Click;
            // 
            // SendButton
            // 
            SendButton.BackColor = Color.IndianRed;
            SendButton.Location = new Point(519, 33);
            SendButton.Margin = new Padding(4);
            SendButton.Name = "SendButton";
            SendButton.Size = new Size(242, 91);
            SendButton.TabIndex = 2;
            SendButton.Text = "Skompresuj i wyślij plik";
            SendButton.UseVisualStyleBackColor = false;
            SendButton.Click += SendButton_Click;
            // 
            // DecodeButton
            // 
            DecodeButton.BackColor = Color.IndianRed;
            DecodeButton.Location = new Point(126, 132);
            DecodeButton.Margin = new Padding(4);
            DecodeButton.Name = "DecodeButton";
            DecodeButton.Size = new Size(239, 87);
            DecodeButton.TabIndex = 3;
            DecodeButton.Text = "Odkoduj i zapisz plik";
            DecodeButton.UseVisualStyleBackColor = false;
            DecodeButton.Click += DecodeButton_Click;
            // 
            // SaveButton
            // 
            SaveButton.BackColor = SystemColors.ActiveCaption;
            SaveButton.Location = new Point(2044, 18);
            SaveButton.Margin = new Padding(4);
            SaveButton.Name = "SaveButton";
            SaveButton.Size = new Size(360, 197);
            SaveButton.TabIndex = 4;
            SaveButton.Text = "Zapisz plik";
            SaveButton.UseVisualStyleBackColor = false;
            SaveButton.Click += SaveButton_Click;
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.ImageScalingSize = new Size(20, 20);
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new Size(61, 4);
            // 
            // StringInfo
            // 
            StringInfo.BackColor = Color.IndianRed;
            StringInfo.Location = new Point(159, 283);
            StringInfo.Margin = new Padding(4);
            StringInfo.Name = "StringInfo";
            StringInfo.ReadOnly = true;
            StringInfo.Size = new Size(87, 34);
            StringInfo.TabIndex = 7;
            StringInfo.Text = "String:";
            // 
            // StringText
            // 
            StringText.BackColor = SystemColors.ButtonHighlight;
            StringText.Location = new Point(22, 325);
            StringText.Margin = new Padding(4);
            StringText.Name = "StringText";
            StringText.ReadOnly = true;
            StringText.Size = new Size(414, 161);
            StringText.TabIndex = 8;
            StringText.Text = "";
            // 
            // BitInfo
            // 
            BitInfo.BackColor = Color.IndianRed;
            BitInfo.Location = new Point(684, 283);
            BitInfo.Margin = new Padding(4);
            BitInfo.Name = "BitInfo";
            BitInfo.ReadOnly = true;
            BitInfo.Size = new Size(104, 34);
            BitInfo.TabIndex = 9;
            BitInfo.Text = "Binarnie:";
            BitInfo.TextChanged += BitInfo_TextChanged;
            // 
            // BitText
            // 
            BitText.BackColor = SystemColors.ButtonHighlight;
            BitText.Location = new Point(519, 325);
            BitText.Margin = new Padding(4);
            BitText.Name = "BitText";
            BitText.ReadOnly = true;
            BitText.Size = new Size(451, 161);
            BitText.TabIndex = 10;
            BitText.Text = "";
            // 
            // DicionaryText
            // 
            DicionaryText.BackColor = SystemColors.ButtonHighlight;
            DicionaryText.Location = new Point(1099, 325);
            DicionaryText.Margin = new Padding(4);
            DicionaryText.Name = "DicionaryText";
            DicionaryText.ReadOnly = true;
            DicionaryText.Size = new Size(482, 161);
            DicionaryText.TabIndex = 11;
            DicionaryText.Text = "";
            DicionaryText.TextChanged += DicionaryText_TextChanged;
            // 
            // DictionaryInfo
            // 
            DictionaryInfo.BackColor = Color.IndianRed;
            DictionaryInfo.Location = new Point(1296, 283);
            DictionaryInfo.Margin = new Padding(4);
            DictionaryInfo.Name = "DictionaryInfo";
            DictionaryInfo.ReadOnly = true;
            DictionaryInfo.Size = new Size(91, 34);
            DictionaryInfo.TabIndex = 12;
            DictionaryInfo.Text = "Słownik:";
            DictionaryInfo.TextChanged += DictionaryInfo_TextChanged;
            // 
            // CodeButton
            // 
            CodeButton.BackColor = Color.IndianRed;
            CodeButton.Location = new Point(245, 33);
            CodeButton.Margin = new Padding(4);
            CodeButton.Name = "CodeButton";
            CodeButton.Size = new Size(239, 91);
            CodeButton.TabIndex = 2;
            CodeButton.Text = "Zakoduj binarnie i zapisz do pliku";
            CodeButton.UseVisualStyleBackColor = false;
            CodeButton.Click += CodeButton_Click;
            // 
            // RecieveButton
            // 
            RecieveButton.BackColor = Color.IndianRed;
            RecieveButton.Location = new Point(1089, 33);
            RecieveButton.Margin = new Padding(4);
            RecieveButton.Name = "RecieveButton";
            RecieveButton.Size = new Size(245, 91);
            RecieveButton.TabIndex = 14;
            RecieveButton.Text = "Otwórz port do nasłuchu (plik)";
            RecieveButton.UseVisualStyleBackColor = false;
            RecieveButton.Click += RecieveButtonClick;
            // 
            // SenderIP
            // 
            SenderIP.BackColor = Color.DarkViolet;
            SenderIP.Location = new Point(589, 145);
            SenderIP.Margin = new Padding(4);
            SenderIP.Name = "SenderIP";
            SenderIP.ReadOnly = true;
            SenderIP.Size = new Size(356, 34);
            SenderIP.TabIndex = 15;
            SenderIP.Text = "Podaj IP odbiorcy i nr portu:";
            // 
            // RecieverIP
            // 
            RecieverIP.BackColor = Color.DarkViolet;
            RecieverIP.Location = new Point(1173, 145);
            RecieverIP.Margin = new Padding(4);
            RecieverIP.Name = "RecieverIP";
            RecieverIP.ReadOnly = true;
            RecieverIP.Size = new Size(356, 34);
            RecieverIP.TabIndex = 16;
            RecieverIP.Text = "IP odbiorcy i nr portu:";
            // 
            // IPAddr1
            // 
            IPAddr1.BackColor = Color.DeepSkyBlue;
            IPAddr1.Location = new Point(589, 185);
            IPAddr1.Margin = new Padding(4);
            IPAddr1.Name = "IPAddr1";
            IPAddr1.Size = new Size(356, 34);
            IPAddr1.TabIndex = 17;
            IPAddr1.Text = "ip";
            // 
            // IPAddr2
            // 
            IPAddr2.BackColor = Color.DeepSkyBlue;
            IPAddr2.Location = new Point(1173, 187);
            IPAddr2.Margin = new Padding(4);
            IPAddr2.Name = "IPAddr2";
            IPAddr2.ReadOnly = true;
            IPAddr2.Size = new Size(356, 34);
            IPAddr2.TabIndex = 18;
            // 
            // Port1
            // 
            Port1.BackColor = Color.DeepSkyBlue;
            Port1.Location = new Point(589, 227);
            Port1.Margin = new Padding(4);
            Port1.Name = "Port1";
            Port1.Size = new Size(356, 34);
            Port1.TabIndex = 19;
            Port1.Text = "port";
            // 
            // Port2
            // 
            Port2.BackColor = Color.DeepSkyBlue;
            Port2.Location = new Point(1173, 227);
            Port2.Margin = new Padding(4);
            Port2.Name = "Port2";
            Port2.Size = new Size(356, 34);
            Port2.TabIndex = 20;
            // 
            // RecieveTreeButton
            // 
            RecieveTreeButton.BackColor = Color.IndianRed;
            RecieveTreeButton.Location = new Point(1353, 33);
            RecieveTreeButton.Margin = new Padding(4);
            RecieveTreeButton.Name = "RecieveTreeButton";
            RecieveTreeButton.Size = new Size(247, 91);
            RecieveTreeButton.TabIndex = 22;
            RecieveTreeButton.Text = "Otwórz port do nasłuchu (drzewo)";
            RecieveTreeButton.UseVisualStyleBackColor = false;
            RecieveTreeButton.Click += RecieveTreeButton_Click;
            // 
            // SendTreeButton
            // 
            SendTreeButton.BackColor = Color.IndianRed;
            SendTreeButton.Location = new Point(769, 33);
            SendTreeButton.Margin = new Padding(4);
            SendTreeButton.Name = "SendTreeButton";
            SendTreeButton.Size = new Size(275, 91);
            SendTreeButton.TabIndex = 23;
            SendTreeButton.Text = "Wyślij drzewo";
            SendTreeButton.UseVisualStyleBackColor = false;
            SendTreeButton.Click += SendTreeButton_Click;
            // 
            // GUI
            // 
            AutoScaleDimensions = new SizeF(12F, 22F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.LightSeaGreen;
            ClientSize = new Size(1609, 504);
            Controls.Add(SendTreeButton);
            Controls.Add(RecieveTreeButton);
            Controls.Add(Port2);
            Controls.Add(Port1);
            Controls.Add(IPAddr2);
            Controls.Add(IPAddr1);
            Controls.Add(RecieverIP);
            Controls.Add(SenderIP);
            Controls.Add(RecieveButton);
            Controls.Add(CodeButton);
            Controls.Add(DictionaryInfo);
            Controls.Add(DicionaryText);
            Controls.Add(BitText);
            Controls.Add(BitInfo);
            Controls.Add(StringText);
            Controls.Add(StringInfo);
            Controls.Add(SaveButton);
            Controls.Add(DecodeButton);
            Controls.Add(SendButton);
            Controls.Add(ReadButton);
            Font = new Font("Algerian", 12F, FontStyle.Regular, GraphicsUnit.Point);
            Margin = new Padding(4);
            Name = "GUI";
            Text = "Form1";
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Button ReadButton;
        private Button SendButton;
        private Button DecodeButton;
        private Button SaveButton;
        private ContextMenuStrip contextMenuStrip1;
        private TextBox StringInfo;
        private RichTextBox StringText;
        private TextBox BitInfo;
        private RichTextBox BitText;
        private RichTextBox DicionaryText;
        private TextBox DictionaryInfo;
        private Button CodeButton;
        private Button RecieveButton;
        private TextBox SenderIP;
        private TextBox RecieverIP;
        private TextBox IPAddr1;
        private TextBox IPAddr2;
        private TextBox Port1;
        private TextBox Port2;
        private Button RecieveTreeButton;
        private Button SendTreeButton;
    }
}