using DTOs;
using DTOs.Dtos;
using Spire.Doc;
using Spire.Doc.Documents;
using Spire.Doc.Fields;
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

            Paragraph headerP = s.AddParagraph();
            headerP.Format.HorizontalAlignment = HorizontalAlignment.Center;
            headerP.Format.TextAlignment = TextAlignment.Center;

            TextRange headerText = headerP.AppendText($"ДОГОВОР-ЗАЯВКА №{contract.Number} от {contract.CreationDate.ToString("dd.MM.yyyy")}");
            headerText.CharacterFormat.FontSize = 14f;
            headerText.CharacterFormat.Bold = true;
            headerText.CharacterFormat.FontName = "TimesNewRoman";

            Paragraph spase1 = s.AddParagraph();

            Table table = s.AddTable(true);

            int rowCount = 18 + contract.Template.Additionals.Count + (contract.UnloadPoints.Count * 6);

            List<string[]> tableData = GetTableData(contract, company);

            table.ResetCells(tableData.Count, 2);

            for (int i = 0; i < tableData.Count; i++) 
            {
                TableRow row = table.Rows[i];

                row.Cells[0].SetCellWidth(25, CellWidthType.Percentage);
                Paragraph rowHeader = row.Cells[0].AddParagraph();
                rowHeader.Format.LineSpacing = 13.5f;
                TextRange rowHaderText = rowHeader.AppendText(tableData[i][0]);
                rowHaderText.CharacterFormat.Bold = true;
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

            string filePathWithoutRoot = Path.Combine("Contracts", contract.CreationDate.Year.ToString(), $"{contract.Number}.docx");
            string fullPath = _fileManager.GetFullPath(filePathWithoutRoot);

            doc.SaveToFile( fullPath, FileFormat.Docx2010);

            return filePathWithoutRoot;
        }

        private List<string[]> GetTableData(ContractDto contract, CompanyBaseDto company) 
        {
            string route = $"{contract.LoadPoint.Route} - {contract.UnloadPoints.Last().Route}";

            List<string[]> tableData = new List<string[]>()
            {
                new string[] { "Заказчик", company.Name },
                new string[] { "Исполнитель", contract.Carrier.Name },
                new string[] { "Маршрут", route },
                new string[] { "Груз", "" },
                new string[] { "Вес, т", contract.Weight.ToString() },
                new string[] { "Объем", contract.Volume.ToString() },
                new string[] { "Получение груза", "" },
                new string[] { "Грузоотправитель", contract.LoadPoint.Company },
                new string[] { "Адрес погрузки", $"{ contract.LoadPoint.Address }\n{string.Join(", ", contract.LoadPoint.Phones) }" },
                new string[] { "Дата и время", contract.LoadPoint.DateAndTime.ToString("dd.MM.yyyy, HH:mm") },
                new string[] { "Способ погрузки", contract.LoadPoint.Side.GetDescription() }
            };


            int count = 1;
            foreach (var point in contract.UnloadPoints)
            {
                tableData.Add(new string[] { $"Сдача груза {count}", "" });
                tableData.Add(new string[] { "Грузополучатель", point.Company });
                tableData.Add(new string[] { "Адрес выгрузки", point.Address });
                tableData.Add(new string[] { "Дата и время", point.DateAndTime.ToString("dd.MM.yyyy, HH:mm") });
                tableData.Add(new string[] { "Контакт", string.Join(", ", point.Phones) });
                tableData.Add(new string[] { "Способ выгрузки", point.Side.GetDescription() });

                count++;
            }


            foreach (AdditionalDto additional in contract.Template.Additionals)
            {
                tableData.Add(new string[] { additional.Name, additional.Description });
            }

            string payment = $"{contract.Payment.ToString()} руб. {contract.Carrier.Vat.GetDescription()}";

            if (contract.Prepayment != 0)
            {
                payment += $"\nПредоплата {contract.Prepayment} руб.";
            }

            tableData.Add(new string[] { $"Стоимость услуг", payment });
            tableData.Add(new string[] { "Условия оплаты", contract.PaymentCondition.GetDescription() });
            tableData.Add(new string[] { "Водитель", "" });
            tableData.Add(new string[] { "ФИО", contract.Driver.Name });
            tableData.Add(new string[] { "Паспортные данные", $"{contract.Driver.PassportSerial},выдан {contract.Driver.PassportIssuer}, {contract.Driver.PassportDateOfIssue.ToString("dd.MM.yyyy")}" });
            tableData.Add(new string[] { "Тел.", string.Join(", ", contract.Driver.Phones) });
            tableData.Add(new string[] { "ТС", $"{contract.Vehicle.TruckModel} {contract.Vehicle.TruckNumber}, {contract.Vehicle.TrailerNumber}" });

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
    }
}
