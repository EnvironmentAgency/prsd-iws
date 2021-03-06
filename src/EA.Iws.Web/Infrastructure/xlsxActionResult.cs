﻿namespace EA.Iws.Web.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text.RegularExpressions;
    using System.Web;
    using System.Web.Mvc;
    using ClosedXML.Excel;

    public class XlsxActionResult<T> : FileResult
    {
        private readonly bool fixedWidthFormat;
        private const int MaxPixelColWidth = 150;
        private readonly string columnsToHide;

        protected IEnumerable<T> Data { get; set; }

        protected XLWorkbook WorkBook { get; set; }

        protected IXLWorksheet Worksheet { get; set; }

        public XlsxActionResult(IEnumerable<T> data,
            string fileDownloadName,
            bool fixedWidthFormat = false,
            string columnsToHide = null) : base(MimeTypes.MSExcelXml)
        {
            Data = data;
            FileDownloadName = fileDownloadName;
            this.fixedWidthFormat = fixedWidthFormat;
            this.columnsToHide = columnsToHide;
        }

        protected override void WriteFile(HttpResponseBase response)
        {
            var outputStream = response.OutputStream;
            using (var memoryStream = new MemoryStream())
            {
                WriteSheet();

                WorkBook.SaveAs(memoryStream);

                outputStream.Write(memoryStream.GetBuffer(), 0, (int)memoryStream.Length);
            }
        }

        protected void WriteSheet()
        {
            CreateWorkBook();

            Worksheet.Cell(2, 1).Value = Data.AsEnumerable();
           
            FormatWorkSheet();
        }

        protected void FormatWorkSheet(IList<PropertyInfo> properties = null, int maxPixelColWidth = MaxPixelColWidth)
        {
            if (properties == null)
            {
                properties = typeof(T).GetProperties();
            }

            AddHeaderRow(properties);

            FormatTitles(properties.Count);

            if (!string.IsNullOrEmpty(columnsToHide))
            {
                Worksheet.Columns(columnsToHide).Delete();
            }

            if (fixedWidthFormat)
            {
                Worksheet.Columns().Width = PixelWidthToExcel(maxPixelColWidth);
                Worksheet.Cells().Style.Alignment.WrapText = true;
            }
            else
            {
                Worksheet.Columns().AdjustToContents();
            }
        }

        private void CreateWorkBook()
        {
            WorkBook = new XLWorkbook();
            Worksheet = WorkBook.Worksheets.Add("Report");
        }

        private void AddHeaderRow(IList<PropertyInfo> properties)
        {
            for (var i = 0; i < properties.Count(); i++)
            {
                var property = properties[i];

                var attr = (DisplayNameAttribute)Attribute.GetCustomAttribute(property, typeof(DisplayNameAttribute));

                var columnName = attr == null ? SplitCamelCase(property.Name) : attr.DisplayName;

                Worksheet.Cell(1, i + 1).Value = columnName;
            }
        }

        private static string SplitCamelCase(string input)
        {
            return Regex.Replace(input, "(?<=[a-z])([A-Z])", " $1", RegexOptions.Compiled);
        }

        private void FormatTitles(int numberOfProperties)
        {
            Worksheet.Range(1, 1, 1, numberOfProperties).AddToNamed("Titles");

            WorkBook.NamedRanges.NamedRange("Titles").Ranges.Style.Font.Bold = true;
            WorkBook.NamedRanges.NamedRange("Titles").Ranges.Style.Fill.BackgroundColor = XLColor.LimeGreen;

            Worksheet.SheetView.FreezeRows(1);
        }

        private static double PixelWidthToExcel(int pixels)
        {
            if (pixels <= 0)
            {
                return 0;
            }

            return ((pixels * 256 / 7) - (128 / 7)) / 256;
        }
    }
}