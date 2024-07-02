  using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Windows.Forms.DataVisualization.Charting;

namespace Adilbek_Khiiasov_nr_albumu58981
{
    public partial class Adilbek_Khiiasov_58981 : Form
    {
        public Adilbek_Khiiasov_58981()
        {
            InitializeComponent();
        }

        bool PobranieDanychWejściowychDlaObliczeniaSumySzereguPotęgowego(out float Akx, out float Ak_Eps)
        {
            Akx = 0.0F;
            Ak_Eps = 0.0f;
            //wypisanie errorProider
            if (!float.TryParse(Ak_txt_x.Text, out Akx))
            {
                errorProvider1.SetError(Ak_txt_x, "ERROR: wystpoił niedozwolony znak w zapisie zmiennej niezależnej X");
                return false;
            }
            errorProvider1.Dispose();


            errorProvider1.Dispose();

            if (!float.TryParse(AktxtEps.Text, out Ak_Eps))
            {
                errorProvider1.SetError(AktxtEps, "ERROR: wystąpił niedozwolony znak w zapise doskonałości obliczeń Eps");
                return false;
            }
            errorProvider1.Dispose();

            if ((Ak_Eps >= 1.0f) || (Ak_Eps <= 0.0f))
            {
                errorProvider1.SetError(AktxtEps, "ERROR: wartość Eps musi spełniać warunek wejściowy 0<Eps<=1");
                return false;
            }
            errorProvider1.Dispose();


            return true;
        }
        bool PobranieDanychWejściowychDlaPotrzebCałkowania(out double AkXdCałk, out double Ak_XgCałk, out double AkEpsCałk, out double AkEpsSzeregu)
        {
           
            AkXdCałk = Ak_XgCałk = AkEpsCałk = AkEpsSzeregu = 0.0f;

            //pobranie dokładności obliczeń sumy szeregu
            if (!double.TryParse(AktxtEps.Text, out AkEpsSzeregu) || (AkEpsSzeregu > 1.0f) || (AkEpsSzeregu <= 0.0f))
            {
                errorProvider1.SetError(AktxtEps, "ERROR: wystąpił niedozwolony znak w zapise doskonałości obliczeń Eps");
                return false;
            }
            errorProvider1.Dispose();
            //pobranie dółnej granicy całkowania
            if (!double.TryParse(AktxtDolnaGranica.Text, out AkXdCałk))
            {
                //był błąd, to go sygnalizujemy
                errorProvider1.SetError(AktxtDolnaGranica, "ERROR: w zapisie wartości dolnej granicy całkowania wystąpił niedozwolony znak");
                return false;
            }
            else
                errorProvider1.Dispose();
            if (!double.TryParse(AktxtGórnaGranica.Text, out Ak_XgCałk) || (Ak_XgCałk < -1.0F) || (Ak_XgCałk > 1.0F))
            {
                //był błąd, to go sygnalizujemy
                errorProvider1.SetError(AktxtGórnaGranica, "ERROR: w zapisie wartości górnej granicy całkowania wystąpił niedozwolony znak");
                return false;
            }
            
            else
                if (Ak_XgCałk <= AkXdCałk)
            {
                //był błąd, to go sygnalizujemy
                errorProvider1.SetError(AktxtGórnaGranica, "ERROR: górna granica całkowania musi być większa od dolnej granicy całkowania");
                return false;
            }
            else
                errorProvider1.Dispose();
            if (!double.TryParse(AktxtDokładnośćCałkowania.Text, out AkEpsCałk) || (AkEpsCałk <= 0.0F) || (AkEpsCałk > 0.05F))
            {
                //był błąd, to go sygnalizujemy
                errorProvider1.SetError(AktxtDokładnośćCałkowania, "ERROR: w zapisie wartości dokładności obliczeń Eps wystąpił niedozwolony znak");
                return false;
            }
            else
                errorProvider1.Dispose();
            return true;
        }


        private void AkbtnGraficznaWizualizacja_Click(object sender, EventArgs e)
        {


            double Ak_X, Ak_Xg, Ak_Xd, Ak_Eps, Ak_h;
            double ALicznikZsumowanychWyrazówSzeregu;
            if (!PobranieDanychDlaTablicowania(out Ak_Xd, out Ak_Xg, out Ak_Eps, out Ak_h))
                return;
            Ak_X = Ak_Xd;
            akchtGraficznaWizualizacjaFunkcji.Series.Clear();
            akchtGraficznaWizualizacjaFunkcji.Series.Add("Wartość funkcji F(X)");
            akchtGraficznaWizualizacjaFunkcji.ChartAreas[0].AxisX.Title = "Wartość X";
            akchtGraficznaWizualizacjaFunkcji.ChartAreas[0].AxisY.Title = "Wartości F(x)";
            akchtGraficznaWizualizacjaFunkcji.ChartAreas[0].Name = "Wykres funkcji";
            akchtGraficznaWizualizacjaFunkcji.Series[0].ChartType = SeriesChartType.Line;
            akchtGraficznaWizualizacjaFunkcji.Series[0].Color = Color.Black;
            akchtGraficznaWizualizacjaFunkcji.Series[0].BorderDashStyle = ChartDashStyle.Solid;
            akchtGraficznaWizualizacjaFunkcji.Series[0].BorderWidth = 2;
            akchtGraficznaWizualizacjaFunkcji.Series[0].IsVisibleInLegend = true;
            akchtGraficznaWizualizacjaFunkcji.Legends.FindByName("Legend1").Docking = Docking.Bottom;
            akchtGraficznaWizualizacjaFunkcji.BackColor = Color.White;
        
            for (int Ak_i = 0; Ak_X <= Ak_Xg; Ak_i++, Ak_X = Ak_Xd + Ak_i * Ak_h)
            {
                akchtGraficznaWizualizacjaFunkcji.Series[0].Points.AddXY(string.Format("{0:0.00}", Ak_X), Ak_SumaSzereguPotęgowego(Ak_X, Ak_Eps, out ALicznikZsumowanychWyrazówSzeregu));

            }
            akchtGraficznaWizualizacjaFunkcji.Visible = true;
            AkdgvTabelarycznaWizualizacja.Visible = false;
            AkgbGrubość.Visible = true;
            AkgbStyl.Visible = true;
            AktxtEps.Enabled = false;
            AktxtXd.Enabled = false;
            AktxtXg.Enabled = false;
            Aktxth.Enabled = false;
            AkbtnGraficznaWizualizacja.Enabled = false;
            AkbtnTabelarycznaWizualizacja.Enabled = true;
        }

        private void AkbtnObliczenie_Click(object sender, EventArgs e)
        {
            float AkX, AkEps;
            double AkLicznikZsumowanychWyrazówSzeregu;

            if (!PobranieDanychWejściowychDlaObliczeniaSumySzereguPotęgowego(out AkX, out AkEps))
                return;

            AktxtObliczonaWartośćSzeregu.Text =
                string.Format("{0:0.00}",
                Ak_SumaSzereguPotęgowego(AkX, AkEps, out AkLicznikZsumowanychWyrazówSzeregu));
            AktxtLicznikZsumowanychWyrazów.Text = AkLicznikZsumowanychWyrazówSzeregu.ToString();
            AklblLicznikZsumowanychWyrazów.Visible = true;
            AklblObliczonaWartośćSzeregu.Visible = true;
            AktxtLicznikZsumowanychWyrazów.Visible = true;
            AktxtObliczonaWartośćSzeregu.Visible = true;
            Ak_txt_x.Enabled = false;
            AktxtEps.Enabled = false;
            AkbtnObliczenie.Enabled = false;
        }

 

        private void AkbtnResetuj_Click(object sender, EventArgs e)
        {
            AklblLicznikZsumowanychWyrazów.Visible = false;
            AklblObliczonaWartośćSzeregu.Visible = false;
            AktxtLicznikZsumowanychWyrazów.Visible = false;
            AktxtObliczonaWartośćSzeregu.Visible = false;
            Ak_txt_x.Enabled = true;
            AktxtEps.Enabled = true;
            AkbtnObliczenie.Enabled = true;
            akchtGraficznaWizualizacjaFunkcji.Visible = false;
            AkdgvTabelarycznaWizualizacja.Visible = false;
            AkgbGrubość.Visible = false;
            AkgbStyl.Visible = false;
            AktxtXd.Enabled = true;
            AktxtXg.Enabled = true;
            Aktxth.Enabled = true;
            AkbtnGraficznaWizualizacja.Enabled = true;
            AkbtnTabelarycznaWizualizacja.Enabled = true;
            AktxtDolnaGranica.Enabled = true;
            AktxtGórnaGranica.Enabled = true;
            AktxtDokładnośćCałkowania.Enabled = true;
            AkbtnObliczenieCałki.Enabled = true;
            AkdgvWizualizacjaObliczaniaCałki.Visible = false;
        }

       
        static double Ak_SumaSzereguPotęgowego(double x, double AkEps, out double Ak_n)//LicznikWyrazówSzeregu
        {//deklaracje lokalne
            double Ak_Suma;
            double AK_W;  //WyrazSzeregu

            //ustalenie początkowego stanu obliczeń
            AK_W = 1.0F; Ak_Suma = 0.0F; Ak_n = 0;
            do
            {
                //obliczenie kolejnej sumy częściowej szeregu potęgowego
                if (Ak_n == 0)
                {
                    AK_W = x;
                    Ak_n++;
                    Ak_Suma += AK_W;
                }
                else
                {
                    AK_W = AK_W *(Math.Pow(-1,Ak_n) * ((1/((2*Ak_n) + 1) * Math.Pow(x,2*Ak_n + 1))));//kolejny, czyli k-ty wyraz szeregu potęgowego
                }

                Ak_Suma = Ak_Suma + AK_W;// Suma+=W;
                Ak_n++;//zwiększenie licznika wyrazów szeregu potęgowego
                       //lub k=k+1;
            } while (Math.Abs(AK_W) > AkEps);
            //zwrotne przekazanie wyniku obliczeń
            return Ak_Suma;
        }


        bool PobranieDanychDlaTablicowania(out double AkXd, out double AkXg, out double AkEps, out double Akh)
        {
            AkXd = 0.0F;
            AkXg = 0.0F;
            Akh = 0.0F;
            AkEps = 0.0f;
            //wypisanie errorProider
            if (!double.TryParse(AktxtXd.Text, out AkXd))
            {
                errorProvider1.SetError(AktxtXd, "ERROR: wystpoił niedozwolony znak w zapisie zmiennej niezależnej X");
                return false;
            }
            errorProvider1.Dispose();

            if ((AkXd >= 1.0F) || (-1.0F >= AkXd))
            {
                errorProvider1.SetError(AktxtXd, "ERROR: wartość zmiennej niezależnej X musi spełniać warunek wejściowy ");
                return false;
            }
            errorProvider1.Dispose();
            if (!double.TryParse(AktxtXg.Text, out AkXg))
            {
                errorProvider1.SetError(AktxtXg, "ERROR: wystpoił niedozwolony znak w zapisie zmiennej niezależnej X");
                return false;
            }
            errorProvider1.Dispose();

            if ((AkXg > 1.0F) || (-1.0f > AkXg))
            {
                errorProvider1.SetError(AktxtXg, "ERROR: wartość zmiennej niezależnej X musi spełniać warunek wejściowy ");
                return false;
            }
            errorProvider1.Dispose();
            if (AkXd >= AkXg)
            {
                //był bład, to go sygnalizujemy
                errorProvider1.SetError(AktxtXd, "ERROR: wartość zmiennej Xd musi być mniejsza niż Xg");
                errorProvider1.SetError(AktxtXg, "ERROR: wartość zmiennej Xd musi być większa niż Xd");
                return false;
            }
            else
                errorProvider1.Dispose();

            if (!double.TryParse(AktxtEps.Text, out AkEps))
            {
                errorProvider1.SetError(AktxtEps, "ERROR: wystąpił niedozwolony znak w zapise doskonałości obliczeń Eps");
                return false;
            }
            errorProvider1.Dispose();

            if ((AkEps > 1.0f) || (AkEps <= 0.0f))
            {
                errorProvider1.SetError(AktxtEps, "ERROR: wartość Eps musi spełniać warunek wejściowy 0<Eps<=1");
                return false;
            }
            errorProvider1.Dispose();

            if (!double.TryParse(Aktxth.Text, out Akh))
            {
                //był bład, to go sygnalizujemy
                errorProvider1.SetError(Aktxth, "ERROR: w zapisie wartości kroku zmiennej wystąpił niedozwolony znak");
                return false;
            }
            else
                errorProvider1.Dispose();
            if ((Akh <= 0.0F) || (Akh >= 1.0F))
            {
                //był bład, to go sygnalizujemy
                errorProvider1.SetError(Aktxth, "ERROR: krok(przyrost) musi spełniać warunek: 0.0 < h < 1.0");
                return false;
            }
            else
                errorProvider1.Dispose();
            if (Akh > (Math.Abs(AkXg) + Math.Abs(AkXd)) / 2)
            {
                //był bład, to go sygnalizujemy
                errorProvider1.SetError(Aktxth, "ERROR: krok(przyrost) musi spełniać warunek: h =< (Xg + Xd) / 2");
                return false;
            }
            else
                errorProvider1.Dispose();
            return true;
        }
        private void AkdgvWizualizacjaObliczaniaCałki_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void AkgbGrubość_Enter(object sender, EventArgs e)
        {

        }

        private void AkbtnTabelarycznaWizualizacja_Click(object sender, EventArgs e)
        {
            double AkLicznikZsumowanychWyrazówSzeregu;
            if (!PobranieDanychDlaTablicowania(out double AkXd, out double AkXg, out double AkEps, out double Akh))//pobranie danych
            {
                return;
            }
            double AkX = AkXd;
            AkdgvTabelarycznaWizualizacja.Rows.Clear();
            for (int Aki = 0; AkX <= AkXg; Aki++, AkX = AkXd + Aki * Akh)//wypisanie tabeli
            {
                AkdgvTabelarycznaWizualizacja.Rows.Add();
                AkdgvTabelarycznaWizualizacja.Rows[Aki].Cells[0].Value = string.Format("{0:0.00}", AkX);
                AkdgvTabelarycznaWizualizacja.Rows[Aki].Cells[1].Value = string.Format("{0:0.00}", Ak_SumaSzereguPotęgowego(AkX, AkEps, out AkLicznikZsumowanychWyrazówSzeregu));
                AkdgvTabelarycznaWizualizacja.Rows[Aki].Cells[2].Value = AkLicznikZsumowanychWyrazówSzeregu;
            }
            AkdgvTabelarycznaWizualizacja.Visible = true;
            AkbtnTabelarycznaWizualizacja.Enabled = false;
            akchtGraficznaWizualizacjaFunkcji.Visible = false;
            AkbtnGraficznaWizualizacja.Enabled = true;
            AktxtEps.Enabled = false;
            AktxtXd.Enabled = false;
            AktxtXg.Enabled = false;
            Aktxth.Enabled = false;
            AkgbStyl.Visible = false;
            AkgbGrubość.Visible = false;
         
        }

      

        private void AkbtnwybierzKolorLiniiWykresu_Click(object sender, EventArgs e)
        {
            if(kolorLiniiWykresu.ShowDialog() == DialogResult.OK)
            {
                AktxtWziernikKoloruLinii.BackColor = kolorLiniiWykresu.Color;
                akchtGraficznaWizualizacjaFunkcji.Series[0].Color = kolorLiniiWykresu.Color;
            }
        }

     
        private void AkbtnwybierzKolorTłaWykresu_Click(object sender, EventArgs e)
        {
            if (kolorTłaWykresu.ShowDialog() == DialogResult.OK)
            {
                AktxtWziernikKołoruTła.BackColor = kolorTłaWykresu.Color;
                akchtGraficznaWizualizacjaFunkcji.BackColor = kolorTłaWykresu.Color;
            }
           
        }

        private void AktbGrubośćLiniiWykresu_Scroll(object sender, EventArgs e)
        {
            akchtGraficznaWizualizacjaFunkcji.Series[0].BorderWidth = AktbGrubośćLiniiWykresu.Value;
        }

        private void AkcbStylLinii_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (AkcbStylLinii.Text == "Ciągła (Solid)")
            {
                akchtGraficznaWizualizacjaFunkcji.Series[0].BorderDashStyle = ChartDashStyle.Solid;//styl linii ciągła
            }
            else if (AkcbStylLinii.Text == "Kropkowa")
            {
                akchtGraficznaWizualizacjaFunkcji.Series[0].BorderDashStyle = ChartDashStyle.Dot;//styl linii kropkowa
            }
            else if (AkcbStylLinii.Text == "Kreskowa")
            {
                akchtGraficznaWizualizacjaFunkcji.Series[0].BorderDashStyle = ChartDashStyle.Dash;//styl linii kreskowa
            }
            else if (AkcbStylLinii.Text == "Kreskowo-Kropkowa")
            {
                akchtGraficznaWizualizacjaFunkcji.Series[0].BorderDashStyle = ChartDashStyle.DashDot;//styl linii kreskowo-Kropkowa
            }
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void grubośćLiniiToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult Ak_Pytanie_Do_Użytkownika_Aplikacji =
          MessageBox.Show("Czy rzewiście chcesz zamknąć ten formularz",//Sprawdzanie czy użytkownik chce wyłączyć program
              this.Text,
           MessageBoxButtons.YesNoCancel,
           MessageBoxIcon.Question,
           MessageBoxDefaultButton.Button3);
            switch (Ak_Pytanie_Do_Użytkownika_Aplikacji)
            {
                case DialogResult.Yes:
                    MessageBox.Show("Teraz nastąpił zamknięcia formularza" + this.Text);
                    Application.ExitThread();
                    break;
            }
        }

        private void plokToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void koloryToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void stylLiniiToolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void stylLiniiToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void kreskowaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            akchtGraficznaWizualizacjaFunkcji.Series[0].BorderDashStyle = ChartDashStyle.Dash;
        }

        private void kropkowaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            akchtGraficznaWizualizacjaFunkcji.Series[0].BorderDashStyle = ChartDashStyle.Dot;

        }

        private void kreskowokropkowaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            akchtGraficznaWizualizacjaFunkcji.Series[0].BorderDashStyle = ChartDashStyle.DashDot;

        }

        private void ciągłaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            akchtGraficznaWizualizacjaFunkcji.Series[0].BorderDashStyle = ChartDashStyle.Solid;

        }

        private void koloryToolStripMenuItem1_Click(object sender, EventArgs e)
        {
      
        }

        private void kolorTłaWykresuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog AkOknoKolorów = new ColorDialog();
            AkOknoKolorów.Color = akchtGraficznaWizualizacjaFunkcji.BackColor;
            if (AkOknoKolorów.ShowDialog() == DialogResult.OK)
                akchtGraficznaWizualizacjaFunkcji.BackColor = AkOknoKolorów.Color;
                 AktxtWziernikKoloruLinii.BackColor = AkOknoKolorów.Color;
        }

        private void kołorLiniiWykresuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog AkOknoKolorów = new ColorDialog();
            AkOknoKolorów.Color = akchtGraficznaWizualizacjaFunkcji.Series[0].Color = kolorLiniiWykresu.Color;
            if (AkOknoKolorów.ShowDialog() == DialogResult.OK)
                akchtGraficznaWizualizacjaFunkcji.Series[0].Color = AkOknoKolorów.Color;
            AktxtWziernikKoloruLinii.BackColor = AkOknoKolorów.Color;

        }

        private void kołorCzcionkiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog AkOknoKolorów = new ColorDialog();
            AkOknoKolorów.Color = this.ForeColor;
            if (AkOknoKolorów.ShowDialog() == DialogResult.OK)
                this.ForeColor = AkOknoKolorów.Color;
        }

      

       

   
        double Ak_MetodaTrapezów(double Ak_a, double Ak_b, double Ak_EpsCałk, double Ak_EpsSzeregu, out double Ak_LicznikPrzedziałów, out double Ak_SzerokośćPrzedziału)
        {
            //deklaracje pomocnicze
            Ak_SzerokośćPrzedziału = Ak_b - Ak_a;
            double Ak_Ci, Ak_Ci_1, Ak_SumaFx;
            //ustalenie początkowego stanu obliczeń: pierwsze przybliżenie całki
            double AkSumaFaFb = Ak_SumaSzereguPotęgowego(Ak_a, Ak_EpsSzeregu, out Ak_LicznikPrzedziałów) +
                               Ak_SumaSzereguPotęgowego(Ak_b, Ak_EpsSzeregu, out Ak_LicznikPrzedziałów);
            Ak_Ci = Ak_SzerokośćPrzedziału * AkSumaFaFb / 2.0F;

            Ak_LicznikPrzedziałów = 1;
            AkdgvWizualizacjaObliczaniaCałki.Rows.Clear();
            AkdgvWizualizacjaObliczaniaCałki.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            AkdgvWizualizacjaObliczaniaCałki.Rows.Add();
            AkdgvWizualizacjaObliczaniaCałki.Rows[0].Cells[0].Value = Ak_LicznikPrzedziałów.ToString();
            AkdgvWizualizacjaObliczaniaCałki.Rows[0].Cells[1].Value = string.Format("{0:0.000}", Ak_Ci);
            AkdgvWizualizacjaObliczaniaCałki.Rows[0].Cells[2].Value = string.Format("{0:0.000}", Ak_SzerokośćPrzedziału);

            do
            {
                //przechowanie i-tego przybliżenia w Ci_
                Ak_Ci_1 = Ak_Ci;
                Ak_LicznikPrzedziałów++;
                //obliczenie szerokości podprzedziałów przy zwiększonej ich liczbie
                Ak_SzerokośćPrzedziału = (Ak_b - Ak_a) / (float)Ak_LicznikPrzedziałów;
                Ak_SumaFx = 0;
                //obliczenie sumy wysokości (wartości szeregu) trapezów w ich punkcie śrovsowych
                for (int vsi = 0; vsi < Ak_LicznikPrzedziałów; vsi++)
                    Ak_SumaFx += Ak_SumaSzereguPotęgowego(Ak_a + Ak_LicznikPrzedziałów * Ak_SzerokośćPrzedziału, Ak_EpsSzeregu, out double Ak_n);

                //obliczenie kolejnego przybliżenia całki
                Ak_Ci = Ak_SzerokośćPrzedziału * (AkSumaFaFb / 2.0f + Ak_SumaFx);
                AkdgvWizualizacjaObliczaniaCałki.Rows.Add();
                AkdgvWizualizacjaObliczaniaCałki.Rows[(int)Ak_LicznikPrzedziałów - 1].Cells[0].Value = Ak_LicznikPrzedziałów.ToString();
                AkdgvWizualizacjaObliczaniaCałki.Rows[(int)Ak_LicznikPrzedziałów - 1].Cells[1].Value = string.Format("{0:0.000}", Ak_Ci);
                AkdgvWizualizacjaObliczaniaCałki.Rows[(int)Ak_LicznikPrzedziałów - 1].Cells[2].Value = string.Format("{0:0.000}", Ak_SzerokośćPrzedziału);
            }
            while (Math.Abs(Ak_Ci - Ak_Ci_1) > Ak_EpsCałk);
            //zwrotne przekazanie ostatniego podziału na trapezów
            //zwrotne przekazanie wartości obliczonej całki
            return Ak_Ci;
        }

        private void AkbtnObliczenieCałki_Click(object sender, EventArgs e)
        {
            //deklaracje dla przechowania pobranych danych wejściowych dla potrzeb całkowania
            double Ak_Xd_Całk, AkXgCałk, AkEpsCałk, AkEpsSzeregu, Ak_SzerokośćPrzedziału;
            double AkLicznikPrzedziałów;
            //pobranie danyh wejściowych
            if (!PobranieDanychWejściowychDlaPotrzebCałkowania(out Ak_Xd_Całk, out AkXgCałk, out AkEpsCałk, out AkEpsSzeregu))
                //gdy był błąd przy pobieraniu danych, to przerywamy obsługę zdarzenia Click od przycisku poleceń: vsbtnObliczenieCałki
                return;
            //deklaracje dla przechowania wyników metody całkowania
            double Ak_WartośćCałki;
            //wywołanie metody obliczenia całki metoda prostokątów
         
                Ak_WartośćCałki = Ak_MetodaTrapezów(Ak_Xd_Całk, AkXgCałk, AkEpsCałk, AkEpsSzeregu, out AkLicznikPrzedziałów, out Ak_SzerokośćPrzedziału);
                //wizualiazacja wyników obliczeń
                AktxtObliczonaWartośćCałki.Text = string.Format("{0:0.000}", Ak_WartośćCałki);
                AktxtLicznikPrzedziałów.Text = AkLicznikPrzedziałów.ToString();
                AktxtSzerokość.Text = string.Format("{0:0.000}", Ak_SzerokośćPrzedziału);
                AktxtGórnaGranica.Enabled = false;
                AktxtDolnaGranica.Enabled = false;
                AktxtDokładnośćCałkowania.Enabled = false;
                AkbtnObliczenieCałki.Enabled = false;
                AkdgvWizualizacjaObliczaniaCałki.Visible = true;
          
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            akchtGraficznaWizualizacjaFunkcji.Series[0].BorderWidth = (int)Ak_numericUpDown1.Value;
            if(Ak_numericUpDown1.Value > 15)
            {
                Ak_numericUpDown1.Value = 0;
            }
        }

        private void wyberszToolStripMenuItem_Click(object sender, EventArgs e)
        {
           


        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {

        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            akchtGraficznaWizualizacjaFunkcji.Series[0].BorderWidth = 1;
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            akchtGraficznaWizualizacjaFunkcji.Series[0].BorderWidth = 2;
        }

        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            akchtGraficznaWizualizacjaFunkcji.Series[0].BorderWidth = 3;
        }

        private void toolStripMenuItem6_Click(object sender, EventArgs e)
        {
            akchtGraficznaWizualizacjaFunkcji.Series[0].BorderWidth = 4;
        }

        private void toolStripMenuItem7_Click(object sender, EventArgs e)
        {
            akchtGraficznaWizualizacjaFunkcji.Series[0].BorderWidth = 5;
        }

        private void toolStripMenuItem8_Click(object sender, EventArgs e)
        {
            akchtGraficznaWizualizacjaFunkcji.Series[0].BorderWidth = 10;
        }

        private void toolStripMenuItem14_Click(object sender, EventArgs e)
        {
            akchtGraficznaWizualizacjaFunkcji.Series[0].BorderWidth = 6;

        }

        private void toolStripMenuItem15_Click(object sender, EventArgs e)
        {
            akchtGraficznaWizualizacjaFunkcji.Series[0].BorderWidth = 7;
        }

        private void toolStripMenuItem16_Click(object sender, EventArgs e)
        {
            akchtGraficznaWizualizacjaFunkcji.Series[0].BorderWidth = 8;
        }

        private void toolStripMenuItem17_Click(object sender, EventArgs e)
        {
            akchtGraficznaWizualizacjaFunkcji.Series[0].BorderWidth = 9;
        }

        private void toolStripMenuItem18_Click(object sender, EventArgs e)
        {
            akchtGraficznaWizualizacjaFunkcji.Series[0].BorderWidth = 10;
        }

        private void toolStripMenuItem9_Click(object sender, EventArgs e)
        {
            akchtGraficznaWizualizacjaFunkcji.Series[0].BorderWidth = 11;
        }

        private void toolStripMenuItem10_Click(object sender, EventArgs e)
        {
            akchtGraficznaWizualizacjaFunkcji.Series[0].BorderWidth = 12;
        }

        private void toolStripMenuItem11_Click(object sender, EventArgs e)
        {
            akchtGraficznaWizualizacjaFunkcji.Series[0].BorderWidth = 13;
        }

        private void toolStripMenuItem12_Click(object sender, EventArgs e)
        {
            akchtGraficznaWizualizacjaFunkcji.Series[0].BorderWidth = 14;
        }

        private void toolStripMenuItem13_Click(object sender, EventArgs e)
        {
            akchtGraficznaWizualizacjaFunkcji.Series[0].BorderWidth = 15;
        }

        private void stylCzionkiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FontDialog Ak_Czionka = new FontDialog();
            Ak_Czionka.Font = this.Font;
            if (Ak_Czionka.ShowDialog() == DialogResult.OK) 
            {
                this.Font = Ak_Czionka.Font;
            }
                
        }



        private void AktxtEps_TextChanged(object sender, EventArgs e)
        {

        }

        private void Ak_txt_x_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
