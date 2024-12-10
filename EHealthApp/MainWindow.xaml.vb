Imports System.Windows
Imports System.Windows.Controls

Public Class MainWindow
    ' ======= Konstanta dan Enum =======
    Private Const IMT_NORMAL_MIN As Double = 18.5
    Private Const IMT_NORMAL_MAX As Double = 24.9
    Private Const JAM_BUKA As Integer = 8
    Private Const JAM_TUTUP As Integer = 16

    Private Enum KategoriIMT
        Underweight
        Normal
        Overweight
        Obese
    End Enum

    Private Enum JenisKonsultasi
        KonsultasiUmum
        KonsultasiGizi
        KonsultasiKebugaran
        KonsultasiKesehatanMental
    End Enum

    ' ======= Event Menu =======
    Private Sub menuExit_Click(sender As Object, e As RoutedEventArgs)
        Dim result As MessageBoxResult = MessageBox.Show("Apakah Anda yakin ingin keluar?", "Konfirmasi", MessageBoxButton.YesNo, MessageBoxImage.Question)
        If result = MessageBoxResult.Yes Then Close()
    End Sub

    Private Sub menuTentang_Click(sender As Object, e As RoutedEventArgs)
        MessageBox.Show("Aplikasi E-Health" & vbCrLf & "Versi 1.0" & vbCrLf & "© 2024 Tim Pengembang", "Tentang Aplikasi", MessageBoxButton.OK, MessageBoxImage.Information)
    End Sub

    ' ======= Logika IMT =======
    Private Sub btnHitungIMT_Click(sender As Object, e As RoutedEventArgs)
        If ValidasiInputIMT() Then
            Dim berat As Double = Double.Parse(txtBeratBadan.Text)
            Dim tinggi As Double = Double.Parse(txtTinggiBadan.Text)

            Dim imt As Double = HitungIMT(berat, tinggi)
            Dim kategori As KategoriIMT = TentukanKategoriIMT(imt)

            txtHasilIMT.Text = $"IMT Anda: {imt:F2}" & vbNewLine & $"Kategori: {kategori}"
        End If
    End Sub

    Private Sub btnResetIMT_Click(sender As Object, e As RoutedEventArgs)
        txtNama.Clear()
        txtUsia.Clear()
        txtBeratBadan.Clear()
        txtTinggiBadan.Clear()
        cmbJenisKelamin.SelectedIndex = -1
        txtHasilIMT.Text = String.Empty
    End Sub


    Private Function HitungIMT(berat As Double, tinggi As Double) As Double
        Return berat / ((tinggi / 100) * (tinggi / 100))
    End Function

    Private Function TentukanKategoriIMT(imt As Double) As KategoriIMT
        If imt < IMT_NORMAL_MIN Then Return KategoriIMT.Underweight
        If imt <= IMT_NORMAL_MAX Then Return KategoriIMT.Normal
        If imt < 30 Then Return KategoriIMT.Overweight
        Return KategoriIMT.Obese
    End Function

    Private Function ValidasiInputIMT() As Boolean
        If String.IsNullOrWhiteSpace(txtNama.Text) OrElse
           String.IsNullOrWhiteSpace(txtBeratBadan.Text) OrElse
           String.IsNullOrWhiteSpace(txtTinggiBadan.Text) Then
            MessageBox.Show("Lengkapi semua data IMT!", "Peringatan", MessageBoxButton.OK, MessageBoxImage.Warning)
            Return False
        End If
        Return True
    End Function

    ' ======= Logika Jadwal Konsultasi =======
    Private Sub btnDaftarKonsultasi_Click(sender As Object, e As RoutedEventArgs)
        If ValidasiInputKonsultasi() Then
            Dim jam As Integer = Integer.Parse(cmbJam.SelectedItem.Content)
            Dim menit As Integer = Integer.Parse(cmbMenit.SelectedItem.Content)
            Dim jenis As JenisKonsultasi = CType(cmbJenisKonsultasi.SelectedIndex, JenisKonsultasi)

            txtHasilKonsultasi.Text = $"Nama: {txtNamaPasien.Text}" & vbNewLine &
                                      $"Telepon: {txtNoTelepon.Text}" & vbNewLine &
                                      $"Jenis Konsultasi: {jenis}" & vbNewLine &
                                      $"Tanggal: {dpTanggalKonsultasi.SelectedDate.Value.ToShortDateString()}" & vbNewLine &
                                      $"Waktu: {jam:00}:{menit:00}"
        End If
    End Sub

    Private Sub btnResetKonsultasi_Click(sender As Object, e As RoutedEventArgs)
        txtNamaPasien.Clear()
        txtNoTelepon.Clear()
        txtEmail.Clear()
        cmbJenisKonsultasi.SelectedIndex = -1
        dpTanggalKonsultasi.SelectedDate = Nothing
        cmbJam.SelectedIndex = -1
        cmbMenit.SelectedIndex = -1
        txtHasilKonsultasi.Text = String.Empty
    End Sub


    Private Function ValidasiInputKonsultasi() As Boolean
        Dim errors As String = ""

        If String.IsNullOrWhiteSpace(txtNamaPasien.Text) Then errors += "- Nama Pasien harus diisi" & vbCrLf
        If String.IsNullOrWhiteSpace(txtNoTelepon.Text) Then errors += "- Telepon harus diisi" & vbCrLf
        If dpTanggalKonsultasi.SelectedDate Is Nothing Then errors += "- Pilih tanggal konsultasi" & vbCrLf
        If cmbJenisKonsultasi.SelectedIndex = -1 Then errors += "- Pilih jenis konsultasi" & vbCrLf

        If Not String.IsNullOrEmpty(errors) Then
            MessageBox.Show(errors, "Peringatan", MessageBoxButton.OK, MessageBoxImage.Warning)
            Return False
        End If

        Return True
    End Function
End Class
