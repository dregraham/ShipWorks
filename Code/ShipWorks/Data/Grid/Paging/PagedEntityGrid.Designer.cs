namespace ShipWorks.Data.Grid.Paging
{
    partial class PagedEntityGrid
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // EntityGrid
            // 
            this.EnableSearching = false;
            this.GridLines = Divelements.SandGrid.GridLinesDisplayType.Both;
            this.NullRepresentation = "";
            this.RowHighlightType = Divelements.SandGrid.RowHighlightType.Full;
            this.ShadeAlternateRows = true;
            this.DataError += new Divelements.SandGrid.GridDataErrorEventHandler(this.OnGridDataError);
            this.ResumeLayout(false);

        }

        #endregion



    }
}
