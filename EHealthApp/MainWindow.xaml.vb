Imports System.Windows
Imports System.Windows.Controls
Imports System.IO
Imports System.Text
Imports Newtonsoft.Json

Public Class MainWindow
    ' ======= Konstanta dan Enum =======
    Private Const IMT_NORMAL_MIN As Double = 18.5
    Private Const IMT_NORMAL_MAX As Double = 24.9

    Private Enum KategoriIMT
        KekuranganBeratBadan
        Normal
        KelebihanBeratBadan
        Obesitas
    End Enum

    ' ======= Event Menu =======
    Private Sub menuExit_Click(sender As Object, e As RoutedEventArgs)
        Dim result As MessageBoxResult = MessageBox.Show("Apakah Anda yakin ingin keluar?", "Konfirmasi", MessageBoxButton.YesNo, MessageBoxImage.Question)
        If result = MessageBoxResult.Yes Then Close()
    End Sub

    Private Sub menuTentang_Click(sender As Object, e As RoutedEventArgs)
        MessageBox.Show("Aplikasi E-Health" & System.Environment.NewLine & "Versi 1.0" & System.Environment.NewLine & "© 2024 Tim Pengembang", "Tentang Aplikasi", MessageBoxButton.OK, MessageBoxImage.Information)
    End Sub

    ' ======= Logika IMT =======
    Private Sub btnHitungIMT_Click(sender As Object, e As RoutedEventArgs)
        If ValidasiInputIMT() Then
            Dim berat As Double = Double.Parse(txtBeratBadan.Text)
            Dim tinggi As Double = Double.Parse(txtTinggiBadan.Text)

            Dim imt As Double = HitungIMT(berat, tinggi)
            Dim kategori As String = TentukanKategoriIMT(imt)

            ' Hitung berat badan ideal dengan IMT 22
            Dim beratIdeal As Double = HitungBeratIdeal(tinggi, 22)

            ' Menampilkan hasil IMT
            txtHasilIMT.Text = $"IMT Anda: {imt:F2}" & Environment.NewLine &
                               $"Kategori: {kategori}" & Environment.NewLine &
                               $"Berat Badan Ideal Anda: {beratIdeal:F2} kg" & Environment.NewLine &
                               $"Saran: {TentukanSaranIMT(kategori)}"

            ' Data IMT
            Dim hasilIMT As New Dictionary(Of String, Object) From {
                {"Nama", txtNama.Text},
                {"Usia", txtUsia.Text},
                {"JenisKelamin", cmbJenisKelamin.Text},
                {"BeratBadan", berat},
                {"TinggiBadan", tinggi},
                {"IMT", imt},
                {"Kategori", kategori},
                {"BeratIdeal", beratIdeal},
                {"Saran", TentukanSaranIMT(kategori)}
            }

            ' Gabungkan data IMT ke dalam objek dengan format yang diinginkan
            Dim data As New Dictionary(Of String, Object) From {
                {"type", "Hitung IMT"},
                {"data", hasilIMT}
            }

            ' Simpan ke file JSON
            Dim filePath As String = "database.json"
            Dim jsonOutput As String = ReadAndAppendJson(filePath, data)

            ' Simpan ke file JSON
            File.WriteAllText(filePath, jsonOutput, Encoding.UTF8)

            ' Tampilkan pesan berhasil
            MessageBox.Show("Data telah ditambahkan.", "Informasi", MessageBoxButton.OK, MessageBoxImage.Information)
        End If
    End Sub

    Private Function TentukanSaranIMT(kategori As String) As String
        Select Case kategori
            Case KategoriIMT.KekuranganBeratBadan.ToString()
                Return "Perbaiki asupan gizi Anda dan pertimbangkan untuk meningkatkan konsumsi makanan bergizi."
            Case KategoriIMT.KelebihanBeratBadan.ToString()
                Return "Mulailah berolahraga secara teratur dan perhatikan pola makan sehat untuk menurunkan berat badan."
            Case KategoriIMT.Obesitas.ToString()
                Return "Mulailah dulu dari jalan sehat dan perhatikan pola makan yang lebih seimbang."
            Case Else
                Return "Jaga pola makan sehat dan gaya hidup aktif untuk menjaga kesehatan."
        End Select
    End Function

    Private Function HitungBeratIdeal(tinggi As Double, imtNormal As Double) As Double
        Return imtNormal * ((tinggi / 100) * (tinggi / 100))
    End Function

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

    Private Function TentukanKategoriIMT(imt As Double) As String
        If imt < IMT_NORMAL_MIN Then Return KategoriIMT.KekuranganBeratBadan.ToString()
        If imt <= IMT_NORMAL_MAX Then Return KategoriIMT.Normal.ToString()
        If imt < 30 Then Return KategoriIMT.KelebihanBeratBadan.ToString()
        Return KategoriIMT.Obesitas.ToString()
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

    ' ======= Logika Air Putih =======
    Private Sub btnHitungAirPutih_Click(sender As Object, e As RoutedEventArgs)
        ' Mengambil input dari TextBox
        Dim namaPasien As String = txtNamaPasienAir.Text
        Dim beratBadanInput As String = txtBeratBadanAir.Text

        ' Validasi input berat badan dan nama pasien
        If String.IsNullOrWhiteSpace(namaPasien) OrElse String.IsNullOrWhiteSpace(beratBadanInput) Then
            MessageBox.Show("Harap lengkapi semua kolom.")
            Return
        End If

        If cmbFaktorAktivitas.SelectedItem Is Nothing Then
            MessageBox.Show("Harap pilih faktor aktivitas terlebih dahulu.")
            Return
        End If

        Dim beratBadan As Double
        If Not Double.TryParse(beratBadanInput, beratBadan) Then
            MessageBox.Show("Harap masukkan berat badan yang valid.")
            Return
        End If

        'Mendapatkan faktor aktivitas pasien dengan fakator aktivitas yang dipilih, untuk kurang aktif 35ml, aktif 45ml, dan sangat aktif 55ml
        Dim selectedItem As ComboBoxItem = CType(cmbFaktorAktivitas.SelectedItem, ComboBoxItem)
        Dim faktorAktivitas As Double = Double.Parse(selectedItem.Tag.ToString())

        ' Menghitung kebutuhan air putih sesuai 
        Dim kebutuhanAirPutih As Double = beratBadan * faktorAktivitas

        ' Membulatkan hasil perhitungan ke atas
        kebutuhanAirPutih = Math.Ceiling(kebutuhanAirPutih)

        ' Menampilkan hasil perhitungan di TextBlock
        txtHasilAirPutih.Text = String.Format("Kebutuhan air putih harian untuk {0} adalah {1} mililiter.", namaPasien, kebutuhanAirPutih)

        ' Data Air Putih
        Dim hasilAirPutih As New Dictionary(Of String, Object) From {
            {"NamaPasien", namaPasien},
            {"BeratBadan", beratBadanInput},
            {"KebutuhanAirPutih", kebutuhanAirPutih}
        }

        ' Gabungkan data Air Putih ke dalam objek dengan format yang diinginkan
        Dim data As New Dictionary(Of String, Object) From {
            {"type", "Kalkulasi Air Minum Harian"},
            {"data", hasilAirPutih}
        }

        ' Simpan ke file JSON
        Dim filePath As String = "database.json"
        Dim jsonOutput As String = ReadAndAppendJson(filePath, data)

        ' Simpan ke file JSON
        File.WriteAllText(filePath, jsonOutput, Encoding.UTF8)

        ' Tampilkan pesan berhasil
        MessageBox.Show("Data telah ditambahkan.", "Informasi", MessageBoxButton.OK, MessageBoxImage.Information)
    End Sub

    ' Fungsi untuk mereset form
    Private Sub btnResetAirPutih_Click(sender As Object, e As RoutedEventArgs)
        txtNamaPasienAir.Clear()
        txtBeratBadanAir.Clear()
        txtHasilAirPutih.Text = String.Empty

    End Sub

    ' Fungsi untuk membaca dan menambah data ke file JSON
    Private Function ReadAndAppendJson(filePath As String, newData As Dictionary(Of String, Object)) As String
        Dim json As String = ""
        If File.Exists(filePath) Then
            ' Membaca data JSON yang sudah ada
            json = File.ReadAllText(filePath)
            Dim jsonArray As List(Of Object) = JsonConvert.DeserializeObject(Of List(Of Object))(json)

            ' Menambahkan data baru ke dalam array
            jsonArray.Add(newData)

            ' Mengonversi array menjadi JSON
            json = JsonConvert.SerializeObject(jsonArray, Formatting.Indented)
        Else
            ' Membuat file baru jika belum ada
            Dim jsonArray As New List(Of Object) From {newData}
            json = JsonConvert.SerializeObject(jsonArray, Formatting.Indented)
        End If

        Return json
    End Function

    'Private Sub txtBeratBadan_TextChanged(sender As Object, e As EventArgs) Handles txtBeratBadan.TextChanged

    'End Sub

    Private Sub txtUsia_TextChanged(sender As Object, e As TextChangedEventArgs) Handles txtUsia.TextChanged
        ' Memeriksa apakah input hanya angka atau titik desimal
        If Not IsNumeric(txtUsia.Text) AndAlso txtUsia.Text <> "" Then
            MessageBox.Show("Masukkan angka!!", "Peringatan", MessageBoxButton.OK, MessageBoxImage.Warning)
        End If
    End Sub

    Private Sub txtBeratBadan_TextChanged_1(sender As Object, e As TextChangedEventArgs) Handles txtBeratBadan.TextChanged
        ' Memeriksa apakah input hanya angka atau titik desimal
        If Not IsNumeric(txtBeratBadan.Text) AndAlso txtBeratBadan.Text <> "" Then
            MessageBox.Show("Masukkan angka!!", "Peringatan", MessageBoxButton.OK, MessageBoxImage.Warning)
        End If
    End Sub

    Private Sub txtTinggiBadan_TextChanged(sender As Object, e As TextChangedEventArgs) Handles txtTinggiBadan.TextChanged
        ' Memeriksa apakah input hanya angka atau titik desimal
        If Not IsNumeric(txtTinggiBadan.Text) AndAlso txtTinggiBadan.Text <> "" Then
            MessageBox.Show("Masukkan angka!!", "Peringatan", MessageBoxButton.OK, MessageBoxImage.Warning)
        End If
    End Sub

    Private Sub txtBeratBadanAir_TextChanged(sender As Object, e As TextChangedEventArgs) Handles txtBeratBadanAir.TextChanged
        ' Memeriksa apakah input hanya angka atau titik desimal
        If Not IsNumeric(txtBeratBadanAir.Text) AndAlso txtBeratBadanAir.Text <> "" Then
            MessageBox.Show("Masukkan angka!!", "Peringatan", MessageBoxButton.OK, MessageBoxImage.Warning)
        End If
    End Sub
End Class
