<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmViewLog
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmViewLog))
        ToolStrip1 = New ToolStrip()
        btnRefresh = New ToolStripButton()
        txtSearch = New ToolStripTextBox()
        ToolStripLabel1 = New ToolStripLabel()
        btnCopy = New ToolStripButton()
        lvwLog = New WatermarkListview()
        ToolStrip1.SuspendLayout()
        SuspendLayout()
        ' 
        ' ToolStrip1
        ' 
        ToolStrip1.BackColor = Color.FromArgb(CByte(50), CByte(50), CByte(55))
        ToolStrip1.GripStyle = ToolStripGripStyle.Hidden
        ToolStrip1.ImageScalingSize = New Size(32, 32)
        ToolStrip1.Items.AddRange(New ToolStripItem() {btnRefresh, txtSearch, ToolStripLabel1, btnCopy})
        ToolStrip1.Location = New Point(0, 0)
        ToolStrip1.Name = "ToolStrip1"
        ToolStrip1.Padding = New Padding(10, 0, 1, 0)
        ToolStrip1.Size = New Size(1029, 39)
        ToolStrip1.TabIndex = 0
        ToolStrip1.Text = "ToolStrip1"
        ' 
        ' btnRefresh
        ' 
        btnRefresh.DisplayStyle = ToolStripItemDisplayStyle.Image
        btnRefresh.Image = My.Resources.Resources.refresh
        btnRefresh.ImageTransparentColor = Color.Magenta
        btnRefresh.Margin = New Padding(0, 1, 10, 2)
        btnRefresh.Name = "btnRefresh"
        btnRefresh.Size = New Size(36, 36)
        btnRefresh.Text = "ToolStripButton1"
        btnRefresh.ToolTipText = "Refresh"
        ' 
        ' txtSearch
        ' 
        txtSearch.Alignment = ToolStripItemAlignment.Right
        txtSearch.Margin = New Padding(1, 0, 20, 0)
        txtSearch.Name = "txtSearch"
        txtSearch.Size = New Size(200, 39)
        ' 
        ' ToolStripLabel1
        ' 
        ToolStripLabel1.Alignment = ToolStripItemAlignment.Right
        ToolStripLabel1.Name = "ToolStripLabel1"
        ToolStripLabel1.Size = New Size(65, 36)
        ToolStripLabel1.Text = "Search text"
        ' 
        ' btnCopy
        ' 
        btnCopy.DisplayStyle = ToolStripItemDisplayStyle.Image
        btnCopy.Image = My.Resources.Resources.copy
        btnCopy.ImageTransparentColor = Color.Magenta
        btnCopy.Name = "btnCopy"
        btnCopy.Size = New Size(36, 36)
        btnCopy.ToolTipText = "Copy log to Windows Clipboard"
        ' 
        ' lvwLog
        ' 
        lvwLog.BackColor = Color.FromArgb(CByte(43), CByte(43), CByte(43))
        lvwLog.Dock = DockStyle.Fill
        lvwLog.ForeColor = Color.WhiteSmoke
        lvwLog.GridLines = True
        lvwLog.HeaderStyle = ColumnHeaderStyle.Nonclickable
        lvwLog.Location = New Point(0, 39)
        lvwLog.Name = "lvwLog"
        lvwLog.Size = New Size(1029, 591)
        lvwLog.SortColumn = 0
        lvwLog.SortDirection = SortOrder.None
        lvwLog.TabIndex = 1
        lvwLog.UseCompatibleStateImageBehavior = False
        lvwLog.WatermarkAlpha = 200
        lvwLog.WatermarkImage = Nothing
        ' 
        ' frmViewLog
        ' 
        AutoScaleDimensions = New SizeF(9F, 21F)
        AutoScaleMode = AutoScaleMode.Font
        BackColor = Color.FromArgb(CByte(43), CByte(43), CByte(43))
        ClientSize = New Size(1029, 630)
        Controls.Add(lvwLog)
        Controls.Add(ToolStrip1)
        Font = New Font("Segoe UI", 12F)
        ForeColor = Color.White
        Icon = CType(resources.GetObject("$this.Icon"), Icon)
        Margin = New Padding(4)
        Name = "frmViewLog"
        StartPosition = FormStartPosition.CenterScreen
        Text = "Sigint Application Log"
        ToolStrip1.ResumeLayout(False)
        ToolStrip1.PerformLayout()
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents ToolStrip1 As ToolStrip
    Friend WithEvents btnRefresh As ToolStripButton
    Friend WithEvents txtSearch As ToolStripTextBox
    Friend WithEvents ToolStripLabel1 As ToolStripLabel
    Friend WithEvents btnCopy As ToolStripButton
    Friend WithEvents lvwLog As WatermarkListview
End Class
