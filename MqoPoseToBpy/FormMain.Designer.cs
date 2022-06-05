namespace MqoPoseToBpy {
    partial class FormMain {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent() {
            this._buttonFileDrop = new System.Windows.Forms.Button();
            this._labelPrefix = new System.Windows.Forms.Label();
            this._labelFrame = new System.Windows.Forms.Label();
            this._textBoxPrefix = new System.Windows.Forms.TextBox();
            this._textBoxFrame = new System.Windows.Forms.TextBox();
            this._textBoxTarget = new System.Windows.Forms.TextBox();
            this._labelTarget = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // _buttonFileDrop
            // 
            this._buttonFileDrop.AllowDrop = true;
            this._buttonFileDrop.BackColor = System.Drawing.Color.Gray;
            this._buttonFileDrop.ForeColor = System.Drawing.SystemColors.Control;
            this._buttonFileDrop.Location = new System.Drawing.Point(12, 16);
            this._buttonFileDrop.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this._buttonFileDrop.Name = "_buttonFileDrop";
            this._buttonFileDrop.Size = new System.Drawing.Size(149, 48);
            this._buttonFileDrop.TabIndex = 0;
            this._buttonFileDrop.Text = "ここにドロップ";
            this._buttonFileDrop.UseVisualStyleBackColor = false;
            this._buttonFileDrop.DragDrop += new System.Windows.Forms.DragEventHandler(this.buttonFileDrop_DragDrop);
            this._buttonFileDrop.DragEnter += new System.Windows.Forms.DragEventHandler(this.buttonFileDrop_DragEnter);
            this._buttonFileDrop.DragOver += new System.Windows.Forms.DragEventHandler(this.buttonFileDrop_DragOver);
            // 
            // _labelPrefix
            // 
            this._labelPrefix.AutoSize = true;
            this._labelPrefix.ForeColor = System.Drawing.SystemColors.Control;
            this._labelPrefix.Location = new System.Drawing.Point(13, 113);
            this._labelPrefix.Name = "_labelPrefix";
            this._labelPrefix.Size = new System.Drawing.Size(49, 20);
            this._labelPrefix.TabIndex = 3;
            this._labelPrefix.Text = "Prefix:";
            // 
            // _labelFrame
            // 
            this._labelFrame.AutoSize = true;
            this._labelFrame.ForeColor = System.Drawing.SystemColors.Control;
            this._labelFrame.Location = new System.Drawing.Point(13, 148);
            this._labelFrame.Name = "_labelFrame";
            this._labelFrame.Size = new System.Drawing.Size(52, 20);
            this._labelFrame.TabIndex = 5;
            this._labelFrame.Text = "Frame:";
            // 
            // _textBoxPrefix
            // 
            this._textBoxPrefix.Location = new System.Drawing.Point(68, 113);
            this._textBoxPrefix.Name = "_textBoxPrefix";
            this._textBoxPrefix.Size = new System.Drawing.Size(93, 27);
            this._textBoxPrefix.TabIndex = 4;
            // 
            // _textBoxFrame
            // 
            this._textBoxFrame.Location = new System.Drawing.Point(68, 148);
            this._textBoxFrame.Name = "_textBoxFrame";
            this._textBoxFrame.Size = new System.Drawing.Size(93, 27);
            this._textBoxFrame.TabIndex = 6;
            // 
            // _textBoxTarget
            // 
            this._textBoxTarget.Location = new System.Drawing.Point(67, 78);
            this._textBoxTarget.Name = "_textBoxTarget";
            this._textBoxTarget.Size = new System.Drawing.Size(93, 27);
            this._textBoxTarget.TabIndex = 2;
            // 
            // _labelTarget
            // 
            this._labelTarget.AutoSize = true;
            this._labelTarget.ForeColor = System.Drawing.SystemColors.Control;
            this._labelTarget.Location = new System.Drawing.Point(12, 78);
            this._labelTarget.Name = "_labelTarget";
            this._labelTarget.Size = new System.Drawing.Size(53, 20);
            this._labelTarget.TabIndex = 1;
            this._labelTarget.Text = "Target:";
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.WindowFrame;
            this.ClientSize = new System.Drawing.Size(173, 188);
            this.Controls.Add(this._textBoxTarget);
            this.Controls.Add(this._labelTarget);
            this.Controls.Add(this._textBoxFrame);
            this.Controls.Add(this._textBoxPrefix);
            this.Controls.Add(this._labelFrame);
            this.Controls.Add(this._labelPrefix);
            this.Controls.Add(this._buttonFileDrop);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormMain";
            this.Text = "アニメーション";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button _buttonFileDrop;
        private System.Windows.Forms.Label _labelTarget;
        private System.Windows.Forms.Label _labelPrefix;
        private System.Windows.Forms.Label _labelFrame;
        private System.Windows.Forms.TextBox _textBoxTarget;
        private System.Windows.Forms.TextBox _textBoxPrefix;
        private System.Windows.Forms.TextBox _textBoxFrame;
    }
}

