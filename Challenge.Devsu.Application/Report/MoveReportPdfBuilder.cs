using Challenge.Devsu.Application.DTOs;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Globalization;

namespace Challenge.Devsu.Application.Report
{
    public static class MoveReportPdfBuilder
    {
        public static byte[] Generate(List<MoveReportResponseDto> moves, string clientName, DateTime start, DateTime end)
        {
            CultureInfo culture = CultureInfo.InvariantCulture;

            var doc = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(30);
                    page.Size(PageSizes.A4.Landscape());
                    page.DefaultTextStyle(x => x.FontSize(10).FontFamily("Helvetica"));

                    // Header
                    page.Header().Row(row =>
                    {
                        row.RelativeItem().Column(col =>
                        {
                            col.Item().Text("Reporte de Movimientos").FontSize(16).Bold();
                            col.Item().Text($"Cliente: {clientName}");
                            col.Item().Text($"Rango: {start:yyyy-MM-dd HH:mm} a {end:yyyy-MM-dd HH:mm}");
                        }); // opcional
                    });

                    // Content: Tabla
                    page.Content().Table(table =>
                    {
                        table.ColumnsDefinition(cols =>
                        {
                            cols.ConstantColumn(100); // Fecha
                            cols.ConstantColumn(180); // Cliente
                            cols.ConstantColumn(100); // N° Cuenta
                            cols.ConstantColumn(70);  // Tipo
                            cols.ConstantColumn(90);  // Saldo inicial
                            cols.ConstantColumn(60);  // Estado
                            cols.ConstantColumn(90);  // Movimiento
                            cols.ConstantColumn(90);  // Saldo final
                        });

                        // Header
                        table.Header(header =>
                        {
                            header.Cell().Element(CellHeader).Text("Fecha");
                            header.Cell().Element(CellHeader).Text("Cliente");
                            header.Cell().Element(CellHeader).Text("N° Cuenta");
                            header.Cell().Element(CellHeader).Text("Tipo");
                            header.Cell().Element(CellHeader).Text("Saldo inicial");
                            header.Cell().Element(CellHeader).Text("Estado");
                            header.Cell().Element(CellHeader).Text("Movimiento");
                            header.Cell().Element(CellHeader).Text("Saldo final");
                        });

                        // Rows
                        foreach (var m in moves)
                        {
                            table.Cell().Element(CellBody).Text(m.TransactionDate.ToString("yyyy-MM-dd HH:mm"));
                            table.Cell().Element(CellBody).Text(Trunc(m.Client, 26));
                            table.Cell().Element(CellBody).Text(m.Account);
                            table.Cell().Element(CellBody).Text(m.AccountType);
                            table.Cell().Element(CellBody).Text(m.InitialBalance.ToString("N2", culture));
                            table.Cell().Element(CellBody).Text(m.Success ? "Éxito" : "Falló");
                            table.Cell().Element(CellBody).Text(m.Amount.ToString("N2", culture));
                            table.Cell().Element(CellBody).Text(m.FinalBalance.ToString("N2", culture));
                        }
                    });

                    // Footer
                    page.Footer().AlignRight().Text(txt =>
                    {
                        txt.Span("Página ").FontSize(9);
                        txt.CurrentPageNumber().FontSize(9);
                        txt.Span(" / ").FontSize(9);
                        txt.TotalPages().FontSize(9);
                    });
                });
            });

            return doc.GeneratePdf();
        }

        private static IContainer CellHeader(IContainer container)
        {
            return container
                .PaddingVertical(4)
                .PaddingHorizontal(2)
                .BorderBottom(1)
                .Background("#E5E7EB")
                .AlignCenter()
                .DefaultTextStyle(x => x.Bold().FontSize(9));
        }

        private static IContainer CellBody(IContainer container)
        {
            return container.PaddingVertical(2).PaddingHorizontal(2);
        }

        static string Trunc(string s, int max) =>
            string.IsNullOrEmpty(s) || s.Length <= max ? s : s.Substring(0, max - 1) + "…";
    }
}
