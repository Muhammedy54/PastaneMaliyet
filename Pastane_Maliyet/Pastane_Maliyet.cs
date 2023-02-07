using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Pastane_Maliyet
{
    public partial class Pastane_Maliyet : Form
    {
        public Pastane_Maliyet()
        {
            InitializeComponent();
        }

        SqlConnection baglanti = new SqlConnection(@"Data Source=MUHAMMEDYILMAZ\SQLEXPRESS;Initial Catalog=TestMaliyet;Integrated Security=True");

        void malzemelistele()
        {
            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM TBLMALZEMELER", baglanti);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
        }

        void ürünlistele()
        {
            SqlDataAdapter da2 = new SqlDataAdapter("SELECT * FROM TBLURUNLER", baglanti);
            DataTable dt2 = new DataTable();
            da2.Fill(dt2);
            dataGridView1.DataSource = dt2;
            
        }
        void temizle()
        {
            TxtMalzemeId.Text = "";
            TxtMalzemeAd.Text = "";
            TxtMalzemeFiyat.Text = "";
            TxtMalzemeStok.Text = "";
            TxtMalzemeNot.Text = "";
            TxtUrunId.Text = "";
            TxtUrunAd.Text = "";
            TxtUrunMaliyetFiyat.Text = "";
            TxtUrunSatisFiyat.Text = "";
            TxtUrunStok.Text = "";
        }

        void kasa()
        {
            SqlDataAdapter da3 = new SqlDataAdapter("SELECT * FROM TBLKASA",baglanti);
            DataTable dt3 = new DataTable();
            da3.Fill(dt3);
            dataGridView1.DataSource = dt3;
        }

        void Urunler()
        {
            baglanti.Open();
            SqlDataAdapter da4 = new SqlDataAdapter("SELECT * FROM TBLURUNLER", baglanti);
            DataTable dt4 = new DataTable();
            da4.Fill(dt4);
            CmbUrun.ValueMember = "URUNID";
            CmbUrun.DisplayMember = "AD";
            CmbUrun.DataSource = dt4;
            baglanti.Close();
        }

        void Malzemeler()
        {
            baglanti.Open();
            SqlDataAdapter da5 = new SqlDataAdapter("SELECT * FROM TBLMALZEMELER", baglanti);
            DataTable dt5 = new DataTable();
            da5.Fill(dt5);
            CmbUrunMalzeme.ValueMember = "MALZEMEID";
            CmbUrunMalzeme.DisplayMember = "AD";
            CmbUrunMalzeme.DataSource = dt5;
            baglanti.Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            malzemelistele();

            Urunler();

            Malzemeler();

            BtnUrunEkle.Enabled = false;

            BtnGuncelle.Enabled = false;
        }

        private void BtnÜrünListele_Click(object sender, EventArgs e)
        {
            ürünlistele();

            temizle();

            BtnUrunEkle.Enabled = true;

            BtnGuncelle.Enabled = true;

            BtnMalzemeGuncelle.Enabled = false;

            BtnMalzemeEkle.Enabled = false;

            BtnGetir.Enabled = false;

        }

        private void BtnMalzemeListele_Click(object sender, EventArgs e)
        {
            malzemelistele();

            temizle();

            BtnUrunEkle.Enabled = false;

            BtnGuncelle.Enabled = false;

            BtnMalzemeGuncelle.Enabled = true;

            BtnMalzemeEkle.Enabled = true;

            BtnGetir.Enabled = true;

        }

        private void BtnKasa_Click(object sender, EventArgs e)
        {
            kasa();

            temizle();
        }

        private void BtnÇıkış_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnMalzemeEkle_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            SqlCommand cmd = new SqlCommand("insert into TBLMALZEMELER (AD,STOK,FIYAT,NOTLAR) VALUES (@P1,@P2,@P3,@P4)", baglanti);
            cmd.Parameters.AddWithValue("@P1", TxtMalzemeAd.Text);
            cmd.Parameters.AddWithValue("@P2",decimal.Parse(TxtMalzemeStok.Text));
            cmd.Parameters.AddWithValue("@P3",decimal.Parse(TxtMalzemeFiyat.Text));
            cmd.Parameters.AddWithValue("@P4", TxtMalzemeNot.Text);
            cmd.ExecuteNonQuery();
            baglanti.Close();
            MessageBox.Show("Malzeme Eklendi", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            malzemelistele();
        }

        private void BtnUrunEkle_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            SqlCommand cmd = new SqlCommand("insert into TBLURUNLER (AD) VALUES (@P1)", baglanti);
            cmd.Parameters.AddWithValue("@P1", TxtUrunAd.Text);
            cmd.ExecuteNonQuery();
            baglanti.Close();
            MessageBox.Show("Ürün Eklendi", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            ürünlistele();
        }

        private void BtnEkle_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            SqlCommand cmd = new SqlCommand("insert into TBLFIRIN (URUNID,MALZEMEID,MIKTAR,MALIYET) VALUES (@P1,@P2,@P3,@P4)", baglanti);
            cmd.Parameters.AddWithValue("@P1",CmbUrun.SelectedValue);
            cmd.Parameters.AddWithValue("@P2",CmbUrunMalzeme.SelectedValue);
            cmd.Parameters.AddWithValue("@P3",decimal.Parse(TxtUrunMiktar.Text));
            cmd.Parameters.AddWithValue("@P4",decimal.Parse(TxtUrunMaliyet.Text));
            cmd.ExecuteNonQuery();
            baglanti.Close();
            MessageBox.Show("Malzeme Eklendi", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);

            listBox1.Items.Add(CmbUrunMalzeme.Text + " - " + TxtUrunMaliyet.Text);
        }

        private void TxtUrunMiktar_TextChanged(object sender, EventArgs e)
        {
            double maliyet;

            if (TxtUrunMiktar.Text == "")
            {

                TxtUrunMiktar.Text = "0";
            }
            baglanti.Open();
            SqlCommand cmd = new SqlCommand("SELECT * FROM TBLMALZEMELER WHERE MALZEMEID=@P1", baglanti);
            cmd.Parameters.AddWithValue("@P1", CmbUrunMalzeme.SelectedValue);
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                TxtUrunMaliyet.Text = dr[3].ToString();
            }
            baglanti.Close();

            maliyet = Convert.ToDouble(TxtUrunMaliyet.Text) * Convert.ToDouble(TxtUrunMiktar.Text);

            TxtUrunMaliyet.Text = maliyet.ToString();
            if (CmbUrun.SelectedItem == "YUMURTA")
            {
                baglanti.Open();
                SqlCommand cmd2 = new SqlCommand("SELECT * FROM TBLMALZEMELER WHERE MALZEMEID=@P1", baglanti);
                cmd2.Parameters.AddWithValue("@P1", CmbUrunMalzeme.SelectedValue);
                SqlDataReader dr2 = cmd2.ExecuteReader();
                while (dr2.Read())
                {
                    TxtUrunMaliyet.Text = dr2[3].ToString();
                }
                baglanti.Close();

                maliyet = Convert.ToDouble(TxtUrunMaliyet.Text) /10000 * Convert.ToDouble(TxtUrunMiktar.Text);

                TxtUrunMaliyet.Text = maliyet.ToString();
            }
        }

       

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int secilen = dataGridView1.SelectedCells[0].RowIndex;

            TxtUrunId.Text = dataGridView1.Rows[secilen].Cells[0].Value.ToString();
            TxtUrunAd.Text = dataGridView1.Rows[secilen].Cells[1].Value.ToString();
            TxtUrunMaliyetFiyat.Text = dataGridView1.Rows[secilen].Cells[2].Value.ToString();
            TxtUrunSatisFiyat.Text = dataGridView1.Rows[secilen].Cells[3].Value.ToString();
            TxtUrunStok.Text = dataGridView1.Rows[secilen].Cells[4].Value.ToString();
            

            baglanti.Open();
            SqlCommand cmd = new SqlCommand("SELECT SUM(MALIYET) FROM TBLFIRIN WHERE URUNID=@P1",baglanti);
            cmd.Parameters.AddWithValue("@P1", CmbUrun.SelectedValue);
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                TxtUrunMaliyetFiyat.Text = dr[0].ToString();
            }
            baglanti.Close();

        }
        

        private void BtnGuncelle_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            SqlCommand cmd = new SqlCommand("update TBLURUNLER SET AD=@P1,MFIYAT=@P2,SFIYAT=@P3,STOK=@P4 WHERE URUNID=@P5", baglanti);
            cmd.Parameters.AddWithValue("@P1", TxtUrunAd.Text);
            cmd.Parameters.AddWithValue("@P2", decimal.Parse(TxtUrunMaliyetFiyat.Text));
            cmd.Parameters.AddWithValue("@P3", decimal.Parse(TxtUrunSatisFiyat.Text));
            cmd.Parameters.AddWithValue("@P4", TxtUrunStok.Text);
            cmd.Parameters.AddWithValue("@P5", TxtUrunId.Text);
            cmd.ExecuteNonQuery();
            baglanti.Close();
            MessageBox.Show("Ürün Bilgisi Güncellendi", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            ürünlistele();
            temizle();
        }

        private void BtnMalzemeGuncelle_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            SqlCommand cmd = new SqlCommand("update TBLMALZEMELER SET AD=@P1,STOK=@P2,FIYAT=@P3,NOTLAR=@P4 WHERE MALZEMEID=@P5", baglanti);
            cmd.Parameters.AddWithValue("@P1",TxtMalzemeAd.Text);
            cmd.Parameters.AddWithValue("@P2",decimal.Parse(TxtMalzemeStok.Text));
            cmd.Parameters.AddWithValue("@P3",decimal.Parse(TxtMalzemeFiyat.Text));
            cmd.Parameters.AddWithValue("@P4",TxtMalzemeNot.Text);
            cmd.Parameters.AddWithValue("@P5",TxtMalzemeId.Text);
            cmd.ExecuteNonQuery();
            baglanti.Close();
            MessageBox.Show("Malzeme Bilgisi Güncellendi", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            malzemelistele();
            temizle();
        }

        private void BtnGetir_Click(object sender, EventArgs e)
        {
            int secilen2 = dataGridView1.SelectedCells[0].RowIndex;

            TxtMalzemeId.Text = dataGridView1.Rows[secilen2].Cells[0].Value.ToString();
            TxtMalzemeAd.Text = dataGridView1.Rows[secilen2].Cells[1].Value.ToString();
            TxtMalzemeStok.Text = dataGridView1.Rows[secilen2].Cells[2].Value.ToString();
            TxtMalzemeFiyat.Text = dataGridView1.Rows[secilen2].Cells[3].Value.ToString();
            TxtMalzemeNot.Text = dataGridView1.Rows[secilen2].Cells[4].Value.ToString();
        }
    }
}
