Imports System.Windows
Imports System.Windows.Controls

Public Class ConsultSchedule
    Private Enum JenisKonsultasi
        KonsultasiUmum
        KonsultasiGizi
        KonsultasiKebugaran
        KonsultasiKesehatanMental
    End Enum

    ' Konstanta Batasan Waktu
    Private Const JAM_BUKA As Integer = 8
    Private Const JAM_TUTUP As Integer = 16

    ' Event Menu Keluar
    Private Sub menuExit_Click(sender As Object, e As RoutedEventArgs)
        Dim result As MessageBoxResult = MessageBox.Show("Apakah Anda yakin ingin keluar?", "Konfirmasi", MessageBoxButton.YesNo, MessageBoxImage.Question)
        If result = MessageBoxResult.Yes Then
            Close()
        End If
    End Sub

    ' Event Menu Tentang
    Private Sub menuTentang_Click(sender As Object, e As RoutedEventArgs)
        MessageBox.Show("Aplikasi Jadwal Konsultasi" & vbNewLine & "Versi 1.0" & vbNewLine & "© 2024 Tim Pengembang", "Tentang Aplikasi", MessageBoxButton.OK, MessageBoxImage.Information)
    End Sub

    ' Tombol Daftar Konsultasi
    Private Sub btnDaftarKonsultasi_Click(sender As Object, e As RoutedEventArgs)
        If ValidasiInput() Then
            ' Operator Aritmatika untuk validasi waktu
            Dim jam As Integer = Integer.Parse(cmbJam.SelectedItem.Content)
            Dim menit As Integer = Integer.Parse(cmbMenit.SelectedItem.Content)

            ' Menggunakan Enumerasi untuk Jenis Konsultasi
            Dim jenisKonsultasi As JenisKonsultasi = CType(cmbJenisKonsultasi.SelectedIndex, JenisKonsultasi)

            ' Menampilkan Hasil Pendaftaran
            txtHasilKonsultasi.Text = $"Nama: {txtNamaPasien.Text}" & vbNewLine &
                                       $"Telepon: {txtNoTelepon.Text}" & vbNewLine &
                                       $"Email: {txtEmail.Text}" & vbNewLine &
                                       $"Jenis Konsultasi: {jenisKonsultasi}" & vbNewLine &
                                       $"Tanggal: {dpTanggalKonsultasi.SelectedDate.Value.ToShortDateString()}" & vbNewLine &
                                       $"Waktu: {jam:00}:{menit:00}"
        End If
    End Sub

    ' Tombol Reset
    Private Sub btnReset_Click(sender As Object, e As RoutedEventArgs)
        txtNamaPasien.Clear()
        txtNoTelepon.Clear()
        txtEmail.Clear()
        cmbJenisKonsultasi.SelectedIndex = -1
        dpTanggalKonsultasi.SelectedDate = Nothing
        cmbJam.SelectedIndex = -1
        cmbMenit.SelectedIndex = -1
        txtHasilKonsultasi.Text = String.Empty
    End Sub

    ' Validasi Input
    Private Function ValidasiInput() As Boolean
        ' Variabel untuk penyimpanan sementara
        Dim errorMessage As String = ""

        If String.IsNullOrWhiteSpace(txtNamaPasien.Text) Then
            errorMessage += "- Nama Pasien harus diisi" & vbCrLf
        End If

        If String.IsNullOrWhiteSpace(txtNoTelepon.Text) Then
            errorMessage += "- Nomor Telepon harus diisi" & vbCrLf
        End If

        If String.IsNullOrWhiteSpace(txtEmail.Text) Then
            errorMessage += "- Email harus diisi" & vbCrLf
        End If

        If cmbJenisKonsultasi.SelectedIndex = -1 Then
            errorMessage += "- Pilih Jenis Konsultasi" & vbCrLf
        End If

        If dpTanggalKonsultasi.SelectedDate Is Nothing Then
            errorMessage += "- Pilih Tanggal Konsultasi" & vbCrLf
        End If

        If cmbJam.SelectedIndex = -1 OrElse cmbMenit.SelectedIndex = -1 Then
            errorMessage += "- Pilih Waktu Konsultasi" & vbCrLf
        End If

        If Not String.IsNullOrEmpty(errorMessage) Then
            MessageBox.Show(errorMessage, "Peringatan", MessageBoxButton.OK, MessageBoxImage.Warning)
            Return False
        End If

        Return True
    End Function
End Class
