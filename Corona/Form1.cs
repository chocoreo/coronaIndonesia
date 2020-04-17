using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Script.Serialization;

namespace Corona
{
    public partial class FrmMenuUtama : System.Windows.Forms.Form
    {
        List<ValueData> listValueData;
        public FrmMenuUtama()
        {
            InitializeComponent();
        }


        private async Task<List<ValueData>> getAllDataCovid19() 
        {
            // API CORONA DI INDONESIA
            string url = "https://api.kawalcorona.com/indonesia/provinsi/";

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            using (HttpResponseMessage responseMessage = await client.GetAsync(url)) 
            {
                if (responseMessage.IsSuccessStatusCode)
                {
                    string data = await responseMessage.Content.ReadAsStringAsync();
                    JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
                    List<ValueData> valueData = javaScriptSerializer.Deserialize<List<ValueData>>(data);
                    return valueData;
                }
                else 
                {
                    throw new Exception(responseMessage.ReasonPhrase);
                }


            }
            
        }

       
        private async void FrmMenuUtama_LoadAsync(object sender, EventArgs e)
        {
            listValueData = await getAllDataCovid19();
            

            if (listValueData.Count > 0)
            {
                for (int i = 0; i < listValueData.Count; i++)
                {
                    Button btnData = new Button();
                    flpCorona.Controls.Add(btnData);
                    btnData.Location = new Point(150*i,0);
                    btnData.Name = "btnData" + (i+1).ToString();
                    btnData.TabIndex = i;
                    btnData.Text = "";
                    btnData.Size = new Size(150, 120);
                    btnData.UseVisualStyleBackColor = true;
                    btnData.FlatStyle = FlatStyle.Flat;
                    
                    
                    Bitmap bmp = new Bitmap(btnData.ClientRectangle.Width,btnData.ClientRectangle.Height);

                    using (Graphics G = Graphics.FromImage(bmp))
                    {
                        G.Clear(btnData.BackColor);
                        string Provinsi = listValueData[i].attributes.Provinsi;
                        string Kasus_Posi = "Kasus Positif : " + listValueData[i].attributes.Kasus_Posi;
                        string Kasus_Semb = "Kasus Sembuh : " + listValueData[i].attributes.Kasus_Semb;
                        string Kasus_Meni = "Kasus Meninggal : " + listValueData[i].attributes.Kasus_Meni;

                        StringFormat SF = new StringFormat();
                        SF.Alignment = StringAlignment.Center;
                        SF.LineAlignment = StringAlignment.Center;
                        Rectangle RC = btnData.ClientRectangle;

                        Font courier = new Font("MS Courier", 10.0F,FontStyle.Regular);
                        Font mSSansSerif = new Font("Segue UI", 13.0F,FontStyle.Bold);

                        float x = -35;
                        RC.Y = (int)x;
                        G.DrawString(Provinsi, mSSansSerif, Brushes.Black, RC, SF);

                        x += 15 + G.MeasureString(Provinsi,courier).Height;
                        RC.Y = (int)x;
                        G.DrawString(Kasus_Posi,courier,Brushes.Red,RC,SF);

                        x += G.MeasureString(Kasus_Posi,courier).Height;
                        RC.Y = (int)x;
                        G.DrawString(Kasus_Semb,courier,Brushes.Green,RC,SF);

                        x += G.MeasureString(Kasus_Semb,courier).Height;
                        RC.Y = (int)x;
                        G.DrawString(Kasus_Meni,courier,Brushes.DarkOrange,RC,SF);


                    } 
                    btnData.Image = bmp;
                    btnData.ImageAlign = ContentAlignment.MiddleCenter;
                }
            }
        }

        private void lblTitle_Click(object sender, EventArgs e)
        {

        }
    }
}
