'PlayVideoFileVB
'frmMain.vb
'
'Emgu CV 3.0.0
'
'form components:
'tableLayoutPanel
'btnOpenFile
'lblChosenFile
'ibOriginal
'txtInfo
'openFileDialog

Option Explicit On      'require explicit declaration of variables, this is NOT Python !!
Option Strict On        'restrict implicit data type conversions to only widening conversions

Imports Emgu.CV                 '
Imports Emgu.CV.CvEnum          'usual Emgu Cv imports
Imports Emgu.CV.Structure       '
Imports Emgu.CV.UI              '
Imports Emgu.CV.Util            '

'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Public Class frmMain

    ' member variables ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Dim capVideo As Capture
    Dim imgFrame As Mat

    Dim blnFormClosing As Boolean = False

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Private Sub frmMain_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        blnFormClosing = True
    End Sub

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Private Sub btnOpenFile_Click(sender As Object, e As EventArgs) Handles btnOpenFile.Click
        Dim drChosenFile As DialogResult

        drChosenFile = openFileDialog.ShowDialog()                 'open file dialog

        If (drChosenFile <> DialogResult.OK Or openFileDialog.FileName = "") Then    'if user chose Cancel or filename is blank . . .
            lblChosenFile.Text = "file not chosen"              'show error message on label
            Return                                              'and exit function
        End If

        Try
            capVideo = New Capture(openFileDialog.FileName)        'attempt to open chosen video file
        Catch ex As Exception                                   'catch error if unsuccessful
                                                                'show error via message box
            MessageBox.Show("unable to read video file, error: " + ex.Message)
            Return
        End Try

        playVideo()
        
    End Sub

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Sub playVideo()

        Dim blnFirstFrame As Boolean = True

        While (blnFormClosing = False)
            imgFrame = capVideo.QueryFrame()

            If (imgFrame Is Nothing And blnFirstFrame = True) Then          'if we did not get an image and we are on the first frame,
                                                                            'then the file probably could not be opened at all,
                txtInfo.AppendText("unable to read frame from video file" + vbCrLf)     'so show an error message
                Return                                                                  'and return
            ElseIf (imgFrame Is Nothing And blnFirstFrame = False) Then     'else if we are past the first frame and any successive frame could not be read
                txtInfo.AppendText("end of video")                          'most likely we have reached the end of the video
                Return
            End If
            
            ibOriginal.Image = imgFrame

            Application.DoEvents()

            blnFirstFrame = False

        End While
        
    End Sub

End Class
