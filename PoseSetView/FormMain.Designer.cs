namespace StudioMeowToon.PoseSetView {
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
            this.buttonFileDrop = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonFileDrop
            // 
            this.buttonFileDrop.AllowDrop = true;
            this.buttonFileDrop.Location = new System.Drawing.Point(12, 12);
            this.buttonFileDrop.Name = "buttonFileDrop";
            this.buttonFileDrop.Size = new System.Drawing.Size(107, 55);
            this.buttonFileDrop.TabIndex = 0;
            this.buttonFileDrop.Text = "Drop Files here!";
            this.buttonFileDrop.UseVisualStyleBackColor = true;
            this.buttonFileDrop.DragDrop += new System.Windows.Forms.DragEventHandler(this.buttonFileDrop_DragDrop);
            this.buttonFileDrop.DragEnter += new System.Windows.Forms.DragEventHandler(this.buttonFileDrop_DragEnter);
            this.buttonFileDrop.DragOver += new System.Windows.Forms.DragEventHandler(this.buttonFileDrop_DragOver);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(291, 83);
            this.Controls.Add(this.buttonFileDrop);
            this.Name = "FormMain";
            this.Text = "Main";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonFileDrop;
    }
}

