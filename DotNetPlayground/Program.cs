using System.Globalization;

class Solution
{
    static void Main(string[] args)
    {
        // Data
        var lines = new List<Line>
        {
            new Line
            {
                Id = 1,
                AccountingTotal = (decimal?)10.2,
                Description = "Description 1",
                Price = string.Empty,
                Total = "10.00",
                LineId = "802"
            },
            new Line
            {
                Id = 2,
                AccountingTotal = (decimal?)10.2,
                Description = "Description 2",
                Price = "10.00",
                Total = "13.00",
                LineId = "802"
            },
            new Line
            {
                Id = 3,
                AccountingTotal = (decimal?)10.2,
                Description = "Description 3",
                Price = "11.00",
                Total = "10.00",
                LineId = "802"
            },
            new Line
            {
                Id = 4,
                AccountingTotal = (decimal?)10.2,
                Description = "Description 4",
                Price = "14.00",
                Total = "10.00",
                LineId = "802"
            }
        };

        try
        {
            var groupedLines = lines.GroupBy(line => line.LineId)
                .Select(linesGroup => linesGroup.Aggregate((firstLine, nextLine) =>
                {
                    // Sum up Price
                    if (double.TryParse(firstLine.Price, NumberStyles.Any, CultureInfo.InvariantCulture,
                            out var firstLinePrice)
                        && double.TryParse(nextLine.Price, NumberStyles.Any, CultureInfo.InvariantCulture,
                            out var nextLinePrice))
                    {
                        var test = firstLinePrice + nextLinePrice;
                        var test2 = (firstLinePrice + nextLinePrice).ToString("0.00", CultureInfo.InvariantCulture);
                        firstLine.Price = (firstLinePrice + nextLinePrice).ToString("0.00", CultureInfo.InvariantCulture);
                    }
                        
                       

                    // Sum up Total
                    if (double.TryParse(firstLine.Total, NumberStyles.Any, CultureInfo.InvariantCulture,
                            out var firstLineTotal)
                        && double.TryParse(nextLine.Total, NumberStyles.Any, CultureInfo.InvariantCulture,
                            out var nextLineTotal))
                        firstLine.Total = (firstLineTotal + nextLineTotal).ToString("0.00", CultureInfo.InvariantCulture);

                    // Sum up AccountingTotal
                    var firstLineAccountingTotal = firstLine.AccountingTotal ?? 0;
                    var nextLineAccountingTotal = nextLine.AccountingTotal ?? 0;
                    firstLine.AccountingTotal = firstLineAccountingTotal + nextLineAccountingTotal;
                
                    // Concat Description
                    firstLine.Description += $"|{nextLine.Description}";
                
                    // Concat Line Id
                    firstLine.MergedId += string.IsNullOrEmpty(firstLine.MergedId)
                        ? $"{firstLine.Id}|{nextLine.Id}"
                        : $"|{nextLine.Id}" ;

                    return firstLine;
                })).ToList();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }


    public class Line
    {
        public long Id { get; set; }
        public string MergedId { get; set; }
        public decimal? AccountingTotal { get; set; }
        public string Description { get; set; }
        public string Price { get; set; }
        public string Total { get; set; }
        public string LineId { get; set; }
    }
}