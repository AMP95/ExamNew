using DTOs;
using DTOs.Dtos;
using Spire.Doc;
using Spire.Doc.Documents;
using Spire.Doc.Fields;
using Spire.Doc.Interface;
using System.Drawing;
using Utilities;
using Utilities.Interfaces;

namespace Exam.FileManager
{
    public class SpireContractCreator : IContractCreator<ContractDto, CompanyBaseDto>
    {
        private IFileManager _fileManager;

        public SpireContractCreator(IFileManager fileManager)
        {
            _fileManager = fileManager;
        }

        public string CreateContractDocument(ContractDto contract, CompanyBaseDto company)
        {
            Document doc = new Document();

            Section s = doc.AddSection();

            s.PageSetup.Margins.Top = 15f;
            s.PageSetup.Margins.Right = 15f;
            s.PageSetup.Margins.Bottom = 15f;

            Paragraph headerP = s.AddParagraph();
            headerP.Format.HorizontalAlignment = HorizontalAlignment.Center;
            headerP.Format.TextAlignment = TextAlignment.Center;

            TextRange headerText = headerP.AppendText($"ДОГОВОР-ЗАЯВКА №{contract.Number} от {contract.CreationDate.ToString("dd.MM.yyyy")}");
            headerText.CharacterFormat.FontSize = 14f;
            headerText.CharacterFormat.Bold = true;
            headerText.CharacterFormat.FontName = "TimesNewRoman";

            Paragraph spase1 = s.AddParagraph();

            Table table = s.AddTable(true);

            List<string[]> tableData = GetTableData(contract, company);

            table.ResetCells(tableData.Count, 2);

            for (int i = 0; i < tableData.Count; i++) 
            {
                TableRow row = table.Rows[i];

                row.Cells[0].SetCellWidth(25, CellWidthType.Percentage);
                Paragraph rowHeader = row.Cells[0].AddParagraph();
                TextRange rowHaderText = rowHeader.AppendText(tableData[i][0]);
                rowHaderText.CharacterFormat.Bold = bool.Parse(tableData[i][2]);
                rowHeader.Format.LineSpacing = 13.5f;
                rowHaderText.CharacterFormat.FontSize = 12f;

                if (tableData[i][1] == string.Empty)
                {
                    table.ApplyHorizontalMerge(i, 0, 1);
                }
                else 
                {
                    Paragraph cellParagraph = row.Cells[1].AddParagraph();
                    cellParagraph.Format.HorizontalAlignment = HorizontalAlignment.Justify;
                    cellParagraph.Format.LineSpacing = 11.15f;
                    cellParagraph.Format.BeforeSpacingLines = 0.1f;
                    cellParagraph.Format.AfterSpacingLines = 0.1f;
                    TextRange cellText = cellParagraph.AppendText(tableData[i][1]);
                    cellText.CharacterFormat.FontSize = 11f;

                    if (tableData[i].Count() == 4)
                    {
                        cellParagraph.Format.LineSpacing = 8.15f;
                        cellText.CharacterFormat.FontSize = 8f;
                    }
                }
            }

            Paragraph spase = s.AddParagraph();

            Table bottomTable = s.AddTable(true);

            bottomTable.ResetCells(7, 4);
            bottomTable.ApplyHorizontalMerge(0, 0, 1);
            bottomTable.ApplyHorizontalMerge(0, 2, 3);

            TableRow bottomHeader = bottomTable.Rows[0];
            bottomHeader.IsHeader = true;

            Paragraph first = bottomHeader.Cells[0].AddParagraph();
            TextRange firstText = first.AppendText("Исполнитель");
            firstText.CharacterFormat.Bold = true;

            Paragraph second = bottomHeader.Cells[2].AddParagraph();
            TextRange secondText = second.AppendText("Заказчик");
            secondText.CharacterFormat.Bold = true;

            List<string[]> bottomData = GetBottomTableData(contract.Carrier, company);

            for (int i = 0; i < bottomData.Count; i++) 
            {
                TableRow row = bottomTable.Rows[i + 1];

                for (int j = 0; j < bottomData[i].Length; j++) 
                {
                    Paragraph par = row.Cells[j].AddParagraph();
                    par.Format.LineSpacing = 9.15f;
                    par.Format.BeforeSpacingLines = 0.1f;
                    par.Format.AfterSpacingLines = 0.1f;
                    TextRange txt = par.AppendText(bottomData[i][j]);
                    txt.CharacterFormat.FontSize = 9f;
                }
            }

            string fileDocPathWithoutRoot = Path.Combine("Contracts", contract.CreationDate.Year.ToString(), $"{contract.Number}.docx");
            string fullDocPath = _fileManager.GetFullPath(fileDocPathWithoutRoot);

            

            doc.SaveToFile(fullDocPath, FileFormat.Docx2010);
            

            return fileDocPathWithoutRoot;
        }

        private List<string[]> GetTableData(ContractDto contract, CompanyBaseDto company) 
        {
            string route = $"{contract.LoadPoint.Route} - {contract.UnloadPoints.Last().Route}";

            List<string[]> tableData = new List<string[]>()
            {
                new string[] { "Заказчик", company.Name, true.ToString() },
                new string[] { "Исполнитель", contract.Carrier.Name, true.ToString() },
                new string[] { "Маршрут", route, true.ToString() },
                new string[] { "Груз", "", true.ToString() },
                new string[] { "Вес, т", contract.Weight.ToString(), false.ToString() },
                new string[] { "Объем", contract.Volume.ToString(), false.ToString()  },
                new string[] { "Получение груза", "", true.ToString(), false.ToString()  },
                new string[] { "Грузоотправитель", contract.LoadPoint.Company, false.ToString()  },
                new string[] { "Адрес погрузки", $"{ contract.LoadPoint.Address }\n{string.Join(", ", contract.LoadPoint.Phones) }", false.ToString()  },
                new string[] { "Дата и время", contract.LoadPoint.DateAndTime.ToString("dd.MM.yyyy, HH:mm") , false.ToString() },
                new string[] { "Способ погрузки", contract.LoadPoint.Side.GetDescription(), false.ToString() }
            };


            int count = 1;
            foreach (var point in contract.UnloadPoints)
            {
                tableData.Add(new string[] { $"Сдача груза {count}", "", true.ToString() });
                tableData.Add(new string[] { "Грузополучатель", point.Company, false.ToString() });
                tableData.Add(new string[] { "Адрес выгрузки", point.Address, false.ToString() });
                tableData.Add(new string[] { "Дата и время", point.DateAndTime.ToString("dd.MM.yyyy, HH:mm"), false.ToString() });
                tableData.Add(new string[] { "Контакт", string.Join(", ", point.Phones), false.ToString() });
                tableData.Add(new string[] { "Способ выгрузки", point.Side.GetDescription(), false.ToString() });

                count++;
            }

            if (contract.Template.Additionals.Any())
            {
                tableData.Add(new string[] { "Условия", "", true.ToString() });

                foreach (AdditionalDto additional in contract.Template.Additionals)
                {
                    tableData.Add(new string[] { additional.Name, additional.Description, false.ToString(), "" });
                }
            }

            string payment = $"{contract.Payment.ToString()} руб. {contract.Carrier.Vat.GetDescription()}";

            if (contract.Prepayment != 0)
            {
                payment += $"\nПредоплата {contract.Prepayment} руб.";
            }

            tableData.Add(new string[] { $"Стоимость услуг", payment, true.ToString() });
            tableData.Add(new string[] { "Условия оплаты", $"{contract.PaymentCondition.GetDescription()} документов, {contract.PayPriority.GetDescription()}", true.ToString() });
            tableData.Add(new string[] { "Водитель", "", true.ToString() });
            tableData.Add(new string[] { "ФИО", contract.Driver.Name, false.ToString() });
            tableData.Add(new string[] { "Паспортные данные", $"{contract.Driver.PassportSerial},выдан {contract.Driver.PassportIssuer}, {contract.Driver.PassportDateOfIssue.ToString("dd.MM.yyyy")}", false.ToString() });
            tableData.Add(new string[] { "Тел.", string.Join(", ", contract.Driver.Phones) , false.ToString() });
            tableData.Add(new string[] { "ТС", $"{contract.Vehicle.TruckModel} {contract.Vehicle.TruckNumber}, {contract.Vehicle.TrailerNumber}", true.ToString() });

            return tableData;
        }

        private List<string[]> GetBottomTableData(CarrierDto carrier, CompanyBaseDto company) 
        {
            List<string[]> tableData = new List<string[]>()
            {
                new string[] { "Название", carrier.Name , "Название", company.Name  },
                new string[] { "Адрес", carrier.Address, "Адрес" , company.Address},
                new string[] { "ИНН/КПП", carrier.InnKpp, "ИНН/КПП", company.InnKpp },
                new string[] { "Тел.:", string.Join(", ", carrier.Phones) , "Тел.:", string.Join(", ", company.Phones) },
                new string[] { "E-mail:", string.Join(", ", carrier.Emails), "E-mail", string.Join(", ", company.Emails) },
                new string[] { "Печать, подпись", "", "Печать, подпись" , "" },
            };

            return tableData;
        }

        public string CreateContractPdf(string docPath, string stampPath)
        {
            string fullDocPath = _fileManager.GetFullPath(docPath);
            Document doc = new Document(fullDocPath);

            string stampFullPath = _fileManager.GetFullPath(stampPath);

            Bitmap p = new Bitmap(Image.FromFile(stampFullPath));

            ITable table = doc.Sections[0].Tables[1];

            TableRow lastRow = table.LastRow;

            TableCell lastCell = lastRow.LastChild as TableCell;

            IParagraph imagPar = lastCell.LastParagraph;

            DocPicture picture = imagPar.AppendPicture(p);
            picture.Width = 150;
            picture.Height = 150;
            picture.VerticalPosition = -75f;
            picture.HorizontalPosition = -20f;
            picture.FillTransparency = 0.6;
            picture.BehindText = true;
            picture.TextWrappingStyle = TextWrappingStyle.Behind;

            string name = Path.GetFileNameWithoutExtension(docPath);
            string catalog = Path.GetDirectoryName(docPath);

            string PdfDirectory = Path.Combine(catalog, "PDF");

            Directory.CreateDirectory(PdfDirectory);

            string filePdfPathWithoutRoot = Path.Combine(PdfDirectory, $"{name}.pdf");
            string fullPdfPath = _fileManager.GetFullPath(filePdfPathWithoutRoot);

            doc.SaveToFile(fullPdfPath, FileFormat.PDF);

            return filePdfPathWithoutRoot;
        }
    }
}
