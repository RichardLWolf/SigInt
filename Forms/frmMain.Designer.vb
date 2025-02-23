<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMain
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMain))
        lblSdrDevices = New Label()
        cboDeviceList = New ComboBox()
        Label1 = New Label()
        ComboBox1 = New ComboBox()
        picEdit = New PictureBox()
        picDelete = New PictureBox()
        CType(picEdit, ComponentModel.ISupportInitialize).BeginInit()
        CType(picDelete, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' lblSdrDevices
        ' 
        lblSdrDevices.AutoSize = True
        lblSdrDevices.ForeColor = Color.White
        lblSdrDevices.Location = New Point(12, 9)
        lblSdrDevices.Name = "lblSdrDevices"
        lblSdrDevices.Size = New Size(135, 21)
        lblSdrDevices.TabIndex = 3
        lblSdrDevices.Text = "Select SDR Device"
        ' 
        ' cboDeviceList
        ' 
        cboDeviceList.BackColor = Color.FromArgb(CByte(43), CByte(43), CByte(43))
        cboDeviceList.DropDownStyle = ComboBoxStyle.DropDownList
        cboDeviceList.ForeColor = Color.White
        cboDeviceList.FormattingEnabled = True
        cboDeviceList.Location = New Point(13, 34)
        cboDeviceList.Margin = New Padding(4)
        cboDeviceList.Name = "cboDeviceList"
        cboDeviceList.Size = New Size(253, 29)
        cboDeviceList.TabIndex = 2
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.ForeColor = Color.White
        Label1.Location = New Point(296, 9)
        Label1.Name = "Label1"
        Label1.Size = New Size(211, 21)
        Label1.TabIndex = 5
        Label1.Text = "Select Configuration Settings"
        ' 
        ' ComboBox1
        ' 
        ComboBox1.BackColor = Color.FromArgb(CByte(43), CByte(43), CByte(43))
        ComboBox1.DropDownStyle = ComboBoxStyle.DropDownList
        ComboBox1.ForeColor = Color.White
        ComboBox1.FormattingEnabled = True
        ComboBox1.Location = New Point(297, 34)
        ComboBox1.Margin = New Padding(4)
        ComboBox1.Name = "ComboBox1"
        ComboBox1.Size = New Size(253, 29)
        ComboBox1.TabIndex = 4
        ' 
        ' picEdit
        ' 
        picEdit.Image = My.Resources.Resources.gear_gray
        picEdit.Location = New Point(557, 34)
        picEdit.Name = "picEdit"
        picEdit.Size = New Size(29, 29)
        picEdit.SizeMode = PictureBoxSizeMode.StretchImage
        picEdit.TabIndex = 24
        picEdit.TabStop = False
        ' 
        ' picDelete
        ' 
        picDelete.Image = My.Resources.Resources.gear_gray
        picDelete.Location = New Point(592, 34)
        picDelete.Name = "picDelete"
        picDelete.Size = New Size(29, 29)
        picDelete.SizeMode = PictureBoxSizeMode.StretchImage
        picDelete.TabIndex = 25
        picDelete.TabStop = False
        ' 
        ' frmMain
        ' 
        AutoScaleDimensions = New SizeF(9F, 21F)
        AutoScaleMode = AutoScaleMode.Font
        BackColor = Color.FromArgb(CByte(43), CByte(43), CByte(43))
        ClientSize = New Size(1029, 630)
        Controls.Add(picDelete)
        Controls.Add(picEdit)
        Controls.Add(Label1)
        Controls.Add(ComboBox1)
        Controls.Add(lblSdrDevices)
        Controls.Add(cboDeviceList)
        Font = New Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        ForeColor = Color.White
        Icon = CType(resources.GetObject("$this.Icon"), Icon)
        Margin = New Padding(4, 4, 4, 4)
        Name = "frmMain"
        Text = "SigInt - RTL-SDR Signal Monitoring Utility"
        CType(picEdit, ComponentModel.ISupportInitialize).EndInit()
        CType(picDelete, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents lblSdrDevices As Label
    Friend WithEvents cboDeviceList As ComboBox
    Friend WithEvents Label1 As Label
    Friend WithEvents ComboBox1 As ComboBox
    Friend WithEvents picEdit As PictureBox
    Friend WithEvents picDelete As PictureBox
End Class
