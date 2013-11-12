namespace RSSPollingService
{
    partial class RSSPollingService
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.D3RSSEventLogs = new System.Diagnostics.EventLog();
            ((System.ComponentModel.ISupportInitialize)(this.D3RSSEventLogs)).BeginInit();
            // 
            // D3RSSEventLogs
            // 
            this.D3RSSEventLogs.Log = "Application";
            this.D3RSSEventLogs.Source = "D3RssPollingSrvc";
            // 
            // RSSPollingService
            // 
            this.ServiceName = "RSSPollingService";
            ((System.ComponentModel.ISupportInitialize)(this.D3RSSEventLogs)).EndInit();

        }

        #endregion

        private System.Diagnostics.EventLog D3RSSEventLogs;
    }
}
