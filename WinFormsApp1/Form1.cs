using System;
using System.Data.SqlClient;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml.Presentation;
using DocumentFormat.OpenXml;
using Newtonsoft.Json.Linq;
using A = DocumentFormat.OpenXml.Drawing;

namespace InvestigacionAI
{
    public partial class Form1 : Form
    {

        private string connectionString = "Server=DESKTOP-O3OIHTO\\SQLEXPRESS;Database=proyecto 1;Trusted_Connection=True;";
        private string apiUrl = "https://openrouter.ai/api/v1/chat/completions";
        private string apiKey = "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";

        public Form1()
        {
            InitializeComponent();
            ProbarConexion();
        }

        private void ProbarConexion()
        {
            try
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    MessageBox.Show("¡Conexión exitosa a la base de datos!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al conectar: " + ex.Message);
            }
        }

        private async void btnConsultar_Click(object sender, EventArgs e)
        {
            string prompt = txtPrompt.Text.Trim();
            if (string.IsNullOrEmpty(prompt))
            {
                MessageBox.Show("Por favor, ingresa un tema a investigar.");
                return;
            }

            txtResultado.Text = "Consultando AI, por favor espera...";
            string resultado = await ConsultarAIAsync(prompt);
            txtResultado.Text = resultado;
        }

        private async Task<string> ConsultarAIAsync(string prompt)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

                string json = $@"
                    {{
                    ""model"": ""meta-llama/llama-3-8b-instruct"",
                      ""messages"": [
                      {{
                       ""role"": ""user"",
                        ""content"": ""{prompt.Replace("\"", "\\\"")}""
                                                                         }}
                                                                                 ]
                                                                                    }}";

                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                var response = await client.PostAsync(apiUrl, content);
                string responseBody = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = JObject.Parse(responseBody);
                    string message = jsonResponse["choices"]?[0]?["message"]?["content"]?.ToString();
                    return message ?? "No se recibió respuesta del modelo.";
                }
                else
                {
                    return $"Error al consultar la API: {response.StatusCode}\n{responseBody}";
                }
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            string prompt = txtPrompt.Text.Trim();
            string resultado = txtResultado.Text.Trim();

            if (string.IsNullOrEmpty(prompt) || string.IsNullOrEmpty(resultado))
            {
                MessageBox.Show("Debe haber un tema y un resultado para guardar.");
                return;
            }

            GuardarEnBaseDeDatos(prompt, resultado);

            string carpeta = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "InvestigacionAI_" + DateTime.Now.ToString("yyyyMMdd_HHmmss"));
            Directory.CreateDirectory(carpeta);

            string wordPath = Path.Combine(carpeta, "Investigacion.docx");
            string pptxPath = Path.Combine(carpeta, "Investigacion.pptx");

            GenerarWord(wordPath, prompt, resultado);
            GenerarPowerPoint(pptxPath, prompt, resultado);

            MessageBox.Show("Documentos generados en: " + carpeta);
        }

        private void GuardarEnBaseDeDatos(string prompt, string resultado)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                var cmd = new SqlCommand("INSERT INTO Investigaciones (Prompt, Resultado, Fecha) VALUES (@Prompt, @Resultado, @Fecha)", conn);
                cmd.Parameters.AddWithValue("@Prompt", prompt);
                cmd.Parameters.AddWithValue("@Resultado", resultado);
                cmd.Parameters.AddWithValue("@Fecha", DateTime.Now);
                cmd.ExecuteNonQuery();
            }
        }

        private void GenerarWord(string filePath, string prompt, string resultado)
        {
            using (WordprocessingDocument doc = WordprocessingDocument.Create(filePath, WordprocessingDocumentType.Document))
            {
                MainDocumentPart mainPart = doc.AddMainDocumentPart();
                mainPart.Document = new Document();
                Body body = new Body();

                // Poner el prompt en negrita
                var boldPrompt = new Paragraph(
                    new Run(
                        new RunProperties(new Bold()),
                        new DocumentFormat.OpenXml.Wordprocessing.Text("Tema de investigación: " + prompt)
                    )
                );
                body.Append(boldPrompt);

                body.Append(new Paragraph(new Run(new DocumentFormat.OpenXml.Wordprocessing.Text("Resultado:"))));
                body.Append(new Paragraph(new Run(new DocumentFormat.OpenXml.Wordprocessing.Text(resultado))));

                mainPart.Document.Append(body);
                mainPart.Document.Save();
            }
        }


        private void GenerarPowerPoint(string filePath, string prompt, string resultado)
        {
            using (PresentationDocument presentationDocument = PresentationDocument.Create(filePath, PresentationDocumentType.Presentation))
            {
                PresentationPart presentationPart = presentationDocument.AddPresentationPart();
                presentationPart.Presentation = new Presentation();

                SlidePart slidePart = presentationPart.AddNewPart<SlidePart>();
                slidePart.Slide = new Slide(new CommonSlideData(new ShapeTree()));
                AddSlide(slidePart, "Tema de investigación", prompt);

                SlidePart slidePart2 = presentationPart.AddNewPart<SlidePart>();
                slidePart2.Slide = new Slide(new CommonSlideData(new ShapeTree()));
                AddSlide(slidePart2, "Resultado", resultado);

                presentationPart.Presentation.AppendChild(new SlideIdList(
                    new SlideId() { Id = 256U, RelationshipId = presentationPart.GetIdOfPart(slidePart) },
                    new SlideId() { Id = 257U, RelationshipId = presentationPart.GetIdOfPart(slidePart2) }
                ));

                presentationPart.Presentation.Save();
            }
        }

        private void AddSlide(SlidePart slidePart, string title, string content)
        {
            var slide = slidePart.Slide;
            var shapeTree = slide.CommonSlideData.ShapeTree;

            var titleShape = new Shape(
                new NonVisualShapeProperties(
                    new NonVisualDrawingProperties() { Id = 1, Name = "Title" },
                    new NonVisualShapeDrawingProperties(new A.ShapeLocks() { NoGrouping = true }),
                    new ApplicationNonVisualDrawingProperties()),
                new ShapeProperties(),
                new TextBody(
                    new A.BodyProperties(),
                    new A.ListStyle(),
                    new A.Paragraph(new A.Run(new A.Text(title)))
                )
            );
            shapeTree.Append(titleShape);

            var contentShape = new Shape(
                new NonVisualShapeProperties(
                    new NonVisualDrawingProperties() { Id = 2, Name = "Content" },
                    new NonVisualShapeDrawingProperties(new A.ShapeLocks() { NoGrouping = true }),
                    new ApplicationNonVisualDrawingProperties()),
                new ShapeProperties(),
                new TextBody(
                    new A.BodyProperties(),
                    new A.ListStyle(),
                    new A.Paragraph(new A.Run(new A.Text(content)))
                )
            );
            shapeTree.Append(contentShape);
        }

        private void txtResultado_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
