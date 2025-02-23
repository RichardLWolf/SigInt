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
        components = New ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMain))
        lblSdrDevices = New Label()
        cboDeviceList = New ComboBox()
        Label1 = New Label()
        ComboBox1 = New ComboBox()
        picEdit = New PictureBox()
        picDelete = New PictureBox()
        ToolTip1 = New ToolTip(components)
        PictureBox1 = New PictureBox()
        btnCancel = New Button()
        btnApply = New Button()
        lnkSigIntRepository = New LinkLabel()
        CType(picEdit, ComponentModel.ISupportInitialize).BeginInit()
        CType(picDelete, ComponentModel.ISupportInitialize).BeginInit()
        CType(PictureBox1, ComponentModel.ISupportInitialize).BeginInit()
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
        picEdit.Image = My.Resources.Resources.pencil2
        picEdit.Location = New Point(557, 34)
        picEdit.Name = "picEdit"
        picEdit.Size = New Size(29, 29)
        picEdit.SizeMode = PictureBoxSizeMode.StretchImage
        picEdit.TabIndex = 24
        picEdit.TabStop = False
        ToolTip1.SetToolTip(picEdit, "Edit selected configuration")
        ' 
        ' picDelete
        ' 
        picDelete.Image = My.Resources.Resources.trash_red
        picDelete.Location = New Point(643, 34)
        picDelete.Name = "picDelete"
        picDelete.Size = New Size(29, 29)
        picDelete.SizeMode = PictureBoxSizeMode.StretchImage
        picDelete.TabIndex = 25
        picDelete.TabStop = False
        ToolTip1.SetToolTip(picDelete, "Remove selected configuration")
        ' 
        ' PictureBox1
        ' 
        PictureBox1.Image = My.Resources.Resources.gear_add
        PictureBox1.Location = New Point(592, 34)
        PictureBox1.Name = "PictureBox1"
        PictureBox1.Size = New Size(29, 29)
        PictureBox1.SizeMode = PictureBoxSizeMode.StretchImage
        PictureBox1.TabIndex = 26
        PictureBox1.TabStop = False
        ToolTip1.SetToolTip(PictureBox1, "Add new configuration")
        ' 
        ' btnCancel
        ' 
        btnCancel.BackColor = Color.Pink
        btnCancel.DialogResult = DialogResult.Cancel
        btnCancel.FlatAppearance.BorderColor = Color.Black
        btnCancel.FlatAppearance.BorderSize = 0
        btnCancel.FlatAppearance.MouseOverBackColor = Color.CornflowerBlue
        btnCancel.FlatStyle = FlatStyle.Flat
        btnCancel.ForeColor = Color.Black
        btnCancel.Location = New Point(572, 98)
        btnCancel.Name = "btnCancel"
        btnCancel.Size = New Size(100, 50)
        btnCancel.TabIndex = 28
        btnCancel.Text = "E&XIT"
        btnCancel.UseVisualStyleBackColor = False
        ' 
        ' btnApply
        ' 
        btnApply.BackColor = Color.LightBlue
        btnApply.FlatAppearance.BorderColor = Color.Black
        btnApply.FlatAppearance.BorderSize = 0
        btnApply.FlatAppearance.MouseOverBackColor = Color.CornflowerBlue
        btnApply.FlatStyle = FlatStyle.Flat
        btnApply.ForeColor = Color.Black
        btnApply.Location = New Point(437, 98)
        btnApply.Name = "btnApply"
        btnApply.Size = New Size(100, 50)
        btnApply.TabIndex = 27
        btnApply.Text = "&MONITOR"
        btnApply.UseVisualStyleBackColor = False
        ' 
        ' lnkSigIntRepository
        ' 
        lnkSigIntRepository.AutoSize = True
        lnkSigIntRepository.LinkColor = Color.CornflowerBlue
        lnkSigIntRepository.Location = New Point(12, 127)
        lnkSigIntRepository.Name = "lnkSigIntRepository"
        lnkSigIntRepository.Size = New Size(163, 21)
        lnkSigIntRepository.TabIndex = 29
        lnkSigIntRepository.TabStop = True
        lnkSigIntRepository.Text = "Visit SigInt Repository"
        lnkSigIntRepository.VisitedLinkColor = Color.FromArgb(CByte(224), CByte(224), CByte(224))
        ' 
        ' frmMain
        ' 
        AutoScaleDimensions = New SizeF(9F, 21F)
        AutoScaleMode = AutoScaleMode.Font
        BackColor = Color.FromArgb(CByte(43), CByte(43), CByte(43))
        ClientSize = New Size(684, 175)
        Controls.Add(lnkSigIntRepository)
        Controls.Add(btnCancel)
        Controls.Add(btnApply)
        Controls.Add(PictureBox1)
        Controls.Add(picDelete)
        Controls.Add(picEdit)
        Controls.Add(Label1)
        Controls.Add(ComboBox1)
        Controls.Add(lblSdrDevices)
        Controls.Add(cboDeviceList)
        Font = New Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        ForeColor = Color.White
        Icon = CType(resources.GetObject("$this.Icon"), Icon)
        Margin = New Padding(4)
        Name = "frmMain"
        Text = "SigInt - RTL-SDR Signal Monitoring Utility"
        CType(picEdit, ComponentModel.ISupportInitialize).EndInit()
        CType(picDelete, ComponentModel.ISupportInitialize).EndInit()
        CType(PictureBox1, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents lblSdrDevices As Label
    Friend WithEvents cboDeviceList As ComboBox
    Friend WithEvents Label1 As Label
    Friend WithEvents ComboBox1 As ComboBox
    Friend WithEvents picEdit As PictureBox
    Friend WithEvents picDelete As PictureBox
    Friend WithEvents ToolTip1 As ToolTip
    Friend WithEvents PictureBox1 As PictureBox
    Friend WithEvents btnCancel As Button
    Friend WithEvents btnApply As Button
    Friend WithEvents lnkSigIntRepository As LinkLabel
End Class
