using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherInformationAcquisition.Src
{
    public class WordHelper
    {
        public static void CreatWordDocument(string fileName, string content)
        {
            using (WordprocessingDocument wordDocument = WordprocessingDocument.Create(fileName, DocumentFormat.OpenXml.WordprocessingDocumentType.Document))
            {
                MainDocumentPart mainPart = wordDocument.AddMainDocumentPart();
                mainPart.Document = new DocumentFormat.OpenXml.Wordprocessing.Document();
                Body docBody = mainPart.Document.AppendChild(new Body());

                docBody.AppendChild(
                    new SectionProperties
                    (
                        new PageSize(){Width = 16839U, Height = 11907U, Orient = PageOrientationValues.Landscape}
                    ));
                Paragraph contentPara = docBody.AppendChild(new Paragraph());

                Run contentRun = contentPara.AppendChild(new Run());
                contentRun.AppendChild(new Text(content));

                RunProperties contentRunProp = contentRun.AppendChild(new RunProperties());

                RunFonts contentFonts = new RunFonts() { Ascii = "宋体", HighAnsi = "宋体" , EastAsia = "宋体"};
                FontSize contentSize = new FontSize() { Val = "36" };
                Color contentColor = new Color() { Val = "365F91" };

                contentRunProp.AppendChild(contentFonts);
                contentRunProp.AppendChild(contentSize);
                contentRunProp.AppendChild(contentColor);
            }
        }
    }
}
