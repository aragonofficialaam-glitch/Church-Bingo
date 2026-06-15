using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;

namespace BingoWPF
{
    public static class ReporteBingoPDF
    {
        public static void GenerarReporte(
            string nombreSorteo,
            DateTime fechaInicio,
            DateTime fechaFin,
            string premioCartonLleno,
            List<string> jugadas,
            List<string> ganadores,
            List<int> numerosSorteados)
        {
            QuestPDF.Settings.License = LicenseType.Community;

            TimeSpan duracion = fechaFin - fechaInicio;

            string carpeta = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                "Downloads",
                "ReportesBINGO"
            );

            if (!Directory.Exists(carpeta))
            {
                Directory.CreateDirectory(carpeta);
            }

            string nombreArchivo = $"Reporte_Bingo_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.pdf";
            string ruta = Path.Combine(carpeta, nombreArchivo);

            Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(35);
                    page.DefaultTextStyle(x => x.FontSize(12));

                    page.Header()
                        .Text("REPORTE FINAL DEL BINGO")
                        .FontSize(22)
                        .Bold()
                        .AlignCenter();

                    page.Content().Column(col =>
                    {
                        col.Spacing(12);

                        col.Item().Text("DATOS GENERALES").FontSize(16).Bold();

                        col.Item().Text($"Fecha de inicio: {fechaInicio:dd/MM/yyyy HH:mm:ss}");
                        col.Item().Text($"Fecha de finalización: {fechaFin:dd/MM/yyyy HH:mm:ss}");
                        col.Item().Text($"Duración: {duracion.Hours}h {duracion.Minutes}m {duracion.Seconds}s");
                        col.Item().Text($"Sorteo: {nombreSorteo}");
                        col.Item().Text($"Premio Cartón Lleno: {premioCartonLleno}");

                        col.Item().LineHorizontal(1);

                        col.Item().Text("JUGADAS REGISTRADAS").FontSize(16).Bold();

                        foreach (string jugada in jugadas)
                        {
                            col.Item().Text("• " + jugada);
                        }

                        col.Item().LineHorizontal(1);

                        col.Item().Text("NÚMEROS SORTEADOS").FontSize(16).Bold();

                        string numeros = numerosSorteados.Count == 0
                            ? "No se registraron números."
                            : string.Join(" - ", numerosSorteados);

                        col.Item().Text(numeros);

                        col.Item().LineHorizontal(1);

                        col.Item().Text("GANADORES").FontSize(16).Bold();

                        if (ganadores.Count == 0)
                        {
                            col.Item().Text("No se registraron ganadores.");
                        }
                        else
                        {
                            for (int i = 0; i < ganadores.Count; i++)
                            {
                                col.Item().Text($"{i + 1}.").Bold();
                                col.Item().Text(ganadores[i]);
                            }
                        }
                    });

                    page.Footer()
                        .AlignCenter()
                        .Text(x =>
                        {
                            x.Span(" Página ");
                            x.CurrentPageNumber();
                        });
                });
            })
            .GeneratePdf(ruta);

            MessageBox.Show(
                "Reporte PDF guardado en:\n" + ruta,
                "Reporte generado",
                MessageBoxButton.OK,
                MessageBoxImage.Information
            );
        }
    }
}