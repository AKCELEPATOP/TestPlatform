using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using TestService.BindingModels;
using TestService.Interfaces;
using TestService.ViewModels;

namespace TestService.Implementations
{
    public class StatService : IStatService
    {
        private ApplicationDbContext context;

        public StatService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public static StatService Create(ApplicationDbContext context)
        {
            return new StatService(context);
        }

        public async Task<List<StatViewModel>> GetList(GetListModel model)
        {
            if (model.DateFrom.HasValue && model.DateTo.HasValue)
            {
                return await context.Stats.Where(rec => rec.DateCreate >= model.DateFrom && rec.DateCreate <= model.DateTo)
                    .Include(rec => rec.Pattern).Select(rec => new StatViewModel
                    {
                        UserName = rec.User.FIO,
                        Total = rec.Total,
                        Right = rec.Right,
                        PatternName = rec.Pattern.Name,
                        Mark = (int)((double)rec.Right / rec.Total * 100),
                        DateCreate = SqlFunctions.DateName("dd", rec.DateCreate) + " " +
                                            SqlFunctions.DateName("mm", rec.DateCreate) + " " +
                                            SqlFunctions.DateName("yyyy", rec.DateCreate)
                    }).ToListAsync();
            }

            return await context.Stats/*.Skip(model.Skip).Take(model.Take)*/.Include(rec => rec.Pattern).Select(rec => new StatViewModel
            {
                Total = rec.Total,
                Right = rec.Right,
                PatternName = rec.Pattern.Name,
                UserName = rec.User.FIO,
                Mark = (int)((double)rec.Right / rec.Total * 100),
                DateCreate = SqlFunctions.DateName("dd", rec.DateCreate) + " " +
                                            SqlFunctions.DateName("mm", rec.DateCreate) + " " +
                                            SqlFunctions.DateName("yyyy", rec.DateCreate)
            }).ToListAsync();
        }

        public async Task<StatChartViewModel> GetUserChart(GetListModel model)
        {
            return new StatChartViewModel
            {
                Results = await context.Stats.Where(rec => rec.UserId == model.UserId)
                .Reverse()
                .Take(model.Take)
                .Select(rec => (double)rec.Right / rec.Total)
                .ToListAsync()
            };
        }

        public async Task<List<StatViewModel>> GetUserList(GetListModel model)
        {
            if (model.DateFrom.HasValue && model.DateTo.HasValue)
            {
                if (model.Skip != -1 && model.Take != -1)
                {
                    return await context.Stats.Where(rec => rec.UserId == model.UserId)
                    .Where(rec => rec.DateCreate >= model.DateFrom.Value && rec.DateCreate <= model.DateTo.Value)
                    .Skip(model.Skip).Take(model.Take)
                    .Include(rec => rec.Pattern).Select(rec => new StatViewModel
                    {
                        PatternName = rec.Pattern.Name,
                        Right = rec.Right,
                        Total = rec.Total,
                        Mark = (int)((double)rec.Right / rec.Total * 100),
                        DateCreate = SqlFunctions.DateName("dd", rec.DateCreate) + " " +
                                            SqlFunctions.DateName("mm", rec.DateCreate) + " " +
                                            SqlFunctions.DateName("yyyy", rec.DateCreate),
                        UserName = rec.User.FIO
                    }).ToListAsync();
                }
                return await context.Stats.Where(rec => rec.UserId == model.UserId)
                    .Where(rec => rec.DateCreate >= model.DateFrom.Value && rec.DateCreate <= model.DateTo.Value)
                    .Include(rec => rec.Pattern).Select(rec => new StatViewModel
                    {
                        PatternName = rec.Pattern.Name,
                        Right = rec.Right,
                        Total = rec.Total,
                        Mark = (int)((double)rec.Right / rec.Total * 100),
                        DateCreate = SqlFunctions.DateName("dd", rec.DateCreate) + " " +
                                            SqlFunctions.DateName("mm", rec.DateCreate) + " " +
                                            SqlFunctions.DateName("yyyy", rec.DateCreate),
                        UserName = rec.User.FIO
                    }).ToListAsync();
            }
            return await context.Stats.Where(rec => rec.UserId == model.UserId)/*.Skip(model.Skip).Take(model.Take)*/.Include(rec => rec.Pattern).Select(rec => new StatViewModel
            {
                PatternName = rec.Pattern.Name,
                Right = rec.Right,
                Total = rec.Total,
                Mark = (int)((double)rec.Right / rec.Total * 100),
                UserName = rec.User.FIO,
                DateCreate = SqlFunctions.DateName("dd", rec.DateCreate) + " " +
                                            SqlFunctions.DateName("mm", rec.DateCreate) + " " +
                                            SqlFunctions.DateName("yyyy", rec.DateCreate),
            }).ToListAsync();
        }


        public async Task SaveToPdfAdmin(ReportBindingModel model)
        {
            List<StatViewModel> list = null;

            Task loadList = Task.Run(async () => list = await GetList(new GetListModel
            {
                DateFrom = model.DateFrom,
                DateTo = model.DateTo,
            }));

            //открываем файл для работы
            FileStream fs = new FileStream(model.FilePath, FileMode.OpenOrCreate, FileAccess.Write);
            //создаем документ, задаем границы, связываем документ и поток
            iTextSharp.text.Document doc = new iTextSharp.text.Document();
            doc.SetMargins(0.5f, 0.5f, 0.5f, 0.5f);
            PdfWriter writer = PdfWriter.GetInstance(doc, fs);

            doc.Open();
            BaseFont baseFont = (model.FontPath != null && model.FontPath != string.Empty) ?
                BaseFont.CreateFont(model.FontPath, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED) : BaseFont.CreateFont();

            //вставляем заголовок
            var phraseTitle = new Phrase("Отчет по тестам пользователей",
                new iTextSharp.text.Font(baseFont, 16, iTextSharp.text.Font.BOLD));
            iTextSharp.text.Paragraph paragraph = new iTextSharp.text.Paragraph(phraseTitle)
            {
                Alignment = Element.ALIGN_CENTER,
                SpacingAfter = 12
            };
            doc.Add(paragraph);

            if (model.DateFrom.HasValue && model.DateTo.HasValue)
            {
                var phrasePeriod = new Phrase("c " + model.DateFrom.Value.ToShortDateString() +
                                        " по " + model.DateTo.Value.ToShortDateString(),
                                        new iTextSharp.text.Font(baseFont, 14, iTextSharp.text.Font.BOLD));
                paragraph = new iTextSharp.text.Paragraph(phrasePeriod)
                {
                    Alignment = Element.ALIGN_CENTER,
                    SpacingAfter = 12
                };
                doc.Add(paragraph);
            }
            PdfPTable table = new PdfPTable(6)
            {
                TotalWidth = 800F
            };
            PdfPCell cell = new PdfPCell();
            var fontForCellBold = new iTextSharp.text.Font(baseFont, 10, iTextSharp.text.Font.BOLD);
            table.AddCell(new PdfPCell(new Phrase("ФИО", fontForCellBold))
            {
                HorizontalAlignment = Element.ALIGN_CENTER
            });
            table.AddCell(new PdfPCell(new Phrase("Шаблон", fontForCellBold))
            {
                HorizontalAlignment = Element.ALIGN_CENTER
            });
            table.AddCell(new PdfPCell(new Phrase("Дата", fontForCellBold))
            {
                HorizontalAlignment = Element.ALIGN_CENTER
            });
            table.AddCell(new PdfPCell(new Phrase("Всего вопросов", fontForCellBold))
            {
                HorizontalAlignment = Element.ALIGN_CENTER
            });
            table.AddCell(new PdfPCell(new Phrase("Правильных", fontForCellBold))
            {
                HorizontalAlignment = Element.ALIGN_CENTER
            });
            table.AddCell(new PdfPCell(new Phrase("Оценка", fontForCellBold))
            {
                HorizontalAlignment = Element.ALIGN_CENTER
            });
            var fontForCells = new iTextSharp.text.Font(baseFont, 10);

            await loadList;

            if (list == null)
            {
                throw new Exception("Нет данных");
            }
            for (int i = 0; i < list.Count; i++)
            {
                cell = new PdfPCell(new Phrase(list[i].UserName, fontForCells));
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase(list[i].PatternName, fontForCells));
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase(list[i].DateCreate, fontForCells));
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase(list[i].Total.ToString(), fontForCells));
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase(list[i].Right.ToString(), fontForCells));
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase(list[i].Mark.ToString(), fontForCells));
                table.AddCell(cell);
            }
            cell = new PdfPCell(new Phrase("Итого:", fontForCellBold))
            {
                HorizontalAlignment = Element.ALIGN_RIGHT,
                Colspan = 3,
                Border = 0
            };
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase(list.Select(rec => rec.Mark).DefaultIfEmpty(0).Average().ToString(), fontForCellBold))
            {
                HorizontalAlignment = Element.ALIGN_RIGHT,
                Border = 0
            };
            table.AddCell(cell);

            doc.Add(table);

            doc.Close();
        }


        public async Task SaveToPdf(ReportBindingModel model)
        {
            List<StatViewModel> list = null;
            Task loadList = Task.Run(async () => list = await GetUserList(new GetListModel
            {
                UserId = model.UserId,
                DateFrom = model.DateFrom,
                DateTo = model.DateTo,
                Skip = -1,
                Take = -1
            }));


            //открываем файл для работы
            FileStream fs = new FileStream(model.FilePath, FileMode.OpenOrCreate, FileAccess.Write);
            //создаем документ, задаем границы, связываем документ и поток
            iTextSharp.text.Document doc = new iTextSharp.text.Document();
            doc.SetMargins(0.5f, 0.5f, 0.5f, 0.5f);
            PdfWriter writer = PdfWriter.GetInstance(doc, fs);

            doc.Open();
            BaseFont baseFont = (model.FontPath != null && model.FontPath != string.Empty) ?
                BaseFont.CreateFont(model.FontPath, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED) : BaseFont.CreateFont();

            await loadList;
            //вставляем заголовок
            iTextSharp.text.Paragraph paragraph;
            string title = "Отчет по тестам пользователя";
            if (list != null && list.Count > 0)
            {
                title += list[0].UserName;
            }
            var phraseTitle = new Phrase(title, // оптимизация
                    new iTextSharp.text.Font(baseFont, 16, iTextSharp.text.Font.BOLD));
                paragraph = new iTextSharp.text.Paragraph(phraseTitle)
                {
                    Alignment = Element.ALIGN_CENTER,
                    SpacingAfter = 12
                };
                doc.Add(paragraph);
            
            if (model.DateFrom.HasValue && model.DateTo.HasValue)
            {
                var phrasePeriod = new Phrase("c " + model.DateFrom.Value.ToShortDateString() +
                                        " по " + model.DateTo.Value.ToShortDateString(),
                                        new iTextSharp.text.Font(baseFont, 14, iTextSharp.text.Font.BOLD));
                paragraph = new iTextSharp.text.Paragraph(phrasePeriod)
                {
                    Alignment = Element.ALIGN_CENTER,
                    SpacingAfter = 12
                };
                doc.Add(paragraph);
            }
            PdfPTable table = new PdfPTable(5)
            {
                TotalWidth = 800F
            };
            PdfPCell cell = new PdfPCell();
            var fontForCellBold = new iTextSharp.text.Font(baseFont, 10, iTextSharp.text.Font.BOLD);
            table.AddCell(new PdfPCell(new Phrase("Шаблон", fontForCellBold))
            {
                HorizontalAlignment = Element.ALIGN_CENTER
            });
            table.AddCell(new PdfPCell(new Phrase("Дата", fontForCellBold))
            {
                HorizontalAlignment = Element.ALIGN_CENTER
            });
            table.AddCell(new PdfPCell(new Phrase("Всего вопросов", fontForCellBold))
            {
                HorizontalAlignment = Element.ALIGN_CENTER
            });
            table.AddCell(new PdfPCell(new Phrase("Правильных", fontForCellBold))
            {
                HorizontalAlignment = Element.ALIGN_CENTER
            });
            table.AddCell(new PdfPCell(new Phrase("Оценка", fontForCellBold))
            {
                HorizontalAlignment = Element.ALIGN_CENTER
            });
            var fontForCells = new iTextSharp.text.Font(baseFont, 10);

            if (list == null)
            {
                throw new Exception("Нет данных");
            }
            for (int i = 0; i < list.Count; i++)
            {
                cell = new PdfPCell(new Phrase(list[i].PatternName, fontForCells));
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase(list[i].DateCreate, fontForCells));
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase(list[i].Total.ToString(), fontForCells));
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase(list[i].Right.ToString(), fontForCells));
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase(list[i].Mark.ToString(), fontForCells));
                table.AddCell(cell);
            }
            cell = new PdfPCell(new Phrase("Итого:", fontForCellBold))
            {
                HorizontalAlignment = Element.ALIGN_RIGHT,
                Colspan = 3,
                Border = 0
            };
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase(list.Select(rec => rec.Mark).DefaultIfEmpty(0).Average().ToString(), fontForCellBold))
            {
                HorizontalAlignment = Element.ALIGN_RIGHT,
                Border = 0
            };
            table.AddCell(cell);

            doc.Add(table);

            doc.Close();

        }
    }
}
