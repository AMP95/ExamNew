using DTOs;
using DTOs.Dtos;
using Spire.Doc;
using Spire.Doc.Documents;
using Spire.Doc.Fields;
using Utilities;
using Utilities.Interfaces;

namespace Exam.FileManager
{
    public class SpireContractCreator : IContractCreator<ContractDto, CompanyDto>
    {
        private IFileManager _fileManager;

        public SpireContractCreator(IFileManager fileManager)
        {
            _fileManager = fileManager;
        }

        public string CreateContractDocument(ContractDto contract, CompanyDto company)
        {
            Document doc = new Document();
            Section s = doc.AddSection();

            Paragraph headerP = s.AddParagraph();
            headerP.Format.HorizontalAlignment = HorizontalAlignment.Center;
            headerP.Format.TextAlignment = TextAlignment.Center;

            TextRange headerText = headerP.AppendText($"ДОГОВОР-ЗАЯВКА №{contract.Number} от {contract.CreationDate.ToString("dd.MM.yyyy")}");
            headerText.CharacterFormat.Bold = true;
            headerText.CharacterFormat.FontSize = 14;
            headerText.CharacterFormat.FontName = "TimesNewRoman";

            Table table = s.AddTable(true);

            int rowCount = 18 + contract.Template.Additionals.Count + (contract.UnloadPoints.Count * 6);

            List<string[]> tableData = GetTableData(contract, company);

            table.ResetCells(tableData.Count, 2);

            for (int i = 0; i < tableData.Count; i++) 
            {
                TableRow row = table.Rows[i];

                Paragraph rowHeader = row.Cells[0].AddParagraph();
                TextRange rowHaderText = rowHeader.AppendText(tableData[i][0]);
                headerText.CharacterFormat.FontName = "TimesNewRoman";
                headerText.CharacterFormat.FontSize = 11;
                headerText.CharacterFormat.Bold = true;

                if (tableData[i][1] == string.Empty)
                {
                    table.ApplyHorizontalMerge(i, 0, 1);
                }
                else 
                {
                    Paragraph cellParagraph = row.Cells[1].AddParagraph();
                    headerP.Format.HorizontalAlignment = HorizontalAlignment.Center;
                    headerP.Format.TextAlignment = TextAlignment.Center;

                    TextRange cellText = cellParagraph.AppendText(tableData[i][1]);
                    headerText.CharacterFormat.FontName = "TimesNewRoman";
                    headerText.CharacterFormat.FontSize = 11;
                }
            }

            Paragraph spase = s.AddParagraph();

            Table bottomTable = s.AddTable(true);

            table.ResetCells(6, 4);
            table.ApplyHorizontalMerge(0, 0, 1);
            table.ApplyHorizontalMerge(0, 2, 3);

            TableRow bottomHeader = bottomTable.Rows[0];
            bottomHeader.IsHeader = true;

            Paragraph first = bottomHeader.Cells[0].AddParagraph();
            TextRange firstText = first.AppendText("Исполнитель");
            headerText.CharacterFormat.FontName = "TimesNewRoman";
            headerText.CharacterFormat.FontSize = 11;
            headerText.CharacterFormat.Bold = true;

            Paragraph second = bottomHeader.Cells[1].AddParagraph();
            TextRange secondText = second.AppendText("Заказчик");
            headerText.CharacterFormat.FontName = "TimesNewRoman";
            headerText.CharacterFormat.FontSize = 11;
            headerText.CharacterFormat.Bold = true;

            List<string[]> bottomData = GetBottomTableData(contract.Carrier, company);

            for (int i = 0; i < bottomData.Count; i++) 
            {
                TableRow row = bottomTable.Rows[i + 1];

                for (int j = 0; j < bottomData[i].Count(); j++) 
                {
                    Paragraph par = row.Cells[j].AddParagraph();
                    TextRange txt = first.AppendText(bottomData[i][j]);
                    headerText.CharacterFormat.FontName = "TimesNewRoman";
                    headerText.CharacterFormat.FontSize = 11;
                }
            }

            string fullPath = _fileManager.GetFullPath($"Contracts/{DateTime.Now.Year}/{contract.Number}.docx");

            doc.SaveToFile( fullPath, FileFormat.Docx2010);

            return string.Empty;
        }

        private List<string[]> GetTableData(ContractDto contract, CompanyDto company) 
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

        private List<string[]> GetBottomTableData(CarrierDto carrier, CompanyDto company) 
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
