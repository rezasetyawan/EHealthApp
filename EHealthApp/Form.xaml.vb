Class Form
    Inherits Window

    Public Sub New()
        InitializeComponent()
    End Sub

    Private Sub btnCalculate_Click(sender As Object, e As RoutedEventArgs)
        Try
            ' Mengambil input dari pengguna
            Dim age As Integer = Convert.ToInt32(txtAge.Text)
            Dim weight As Double = Convert.ToDouble(txtWeight.Text)
            Dim height As Double = Convert.ToDouble(txtHeight.Text)

            ' Memastikan jenis kelamin sudah dipilih
            If cmbGender.SelectedItem Is Nothing Then
                MessageBox.Show("Harap pilih jenis kelamin")
                Return
            End If

            ' Menentukan faktor aktivitas
            Dim activityLevel As Double

            If rbtnLow.IsChecked = True Then
                activityLevel = 1.2 ' Aktivitas rendah
            ElseIf rbtnModerate.IsChecked = True Then
                activityLevel = 1.55 ' Aktivitas sedang
            ElseIf rbtnHigh.IsChecked = True Then
                activityLevel = 1.9 ' Aktivitas tinggi
            Else
                MessageBox.Show("Pilih tingkat aktivitas.")
                Return
            End If

            ' Menghitung BMR (Basal Metabolic Rate) menggunakan rumus Mifflin-St Jeor
            Dim bmr As Double
            Dim gender As String = CType(cmbGender.SelectedItem, ComboBoxItem).Content.ToString()

            If gender = "Laki-laki" Then
                bmr = (88.4 + (13.4 * weight)) + (4.8 * height) - (5.68 * age) ' Rumus untuk pria
            ElseIf gender = "Perempuan" Then
                bmr = (447.6 + (9.25 * weight)) + (3.1 * height) - (4.33 * age) ' Rumus untuk wanita
            Else
                MessageBox.Show("Jenis kelamin tidak valid.")
                Return
            End If


            ' Menghitung Total Kalori Harian (TDEE) dengan mempertimbangkan tingkat aktivitas
            Dim totalCalories As Double = bmr * activityLevel

            ' Menampilkan hasil
            lblResult.Text = "Kebutuhan Kalori Harian: " & totalCalories.ToString("F2") & " Kalori"
        Catch ex As Exception
            MessageBox.Show("Pastikan semua input valid dan coba lagi.", "Error", MessageBoxButton.OK, MessageBoxImage.Error)
        End Try
    End Sub

End Class
